﻿using System;
using AnimeRx;
using GameSystem;
using UniRx;
using UnityEngine;

namespace Game
{
    public class Ball : MonoBehaviour
    {
        public Vector2 Velocity;
        public float Radius;
        public BallType Type;

        public enum BallType
        {
            Top,
            Bottom,
        }

        public bool IsAlive { get; set; }
        private LayerMask hitLayerMask;

        public void Start()
        {
            var spriteRenderer = GetComponent<SpriteRenderer>();
            IsAlive = true;

            switch (Type)
            {
                case BallType.Top:
                    spriteRenderer.color = Colors.Top;
                    hitLayerMask = LayerMask.GetMask("Stage", "TopBar", "Dead", "Eraser");
                    break;

                case BallType.Bottom:
                    spriteRenderer.color = Colors.Bottom;
                    hitLayerMask = LayerMask.GetMask("Stage", "BottomBar", "Dead", "Eraser");
                    break;

                default:
                    throw new ArgumentOutOfRangeException($"Unknown Type {Type}");
            }

            Messenger.Broker.Receive<OnGameFinish>().Subscribe(_ =>
            {
                // バーには当たらないように進める
                hitLayerMask = LayerMask.GetMask("Stage", "Dead", "Eraser");

                var c = spriteRenderer.color;
                Anime.Play(1f, 0f, Easing.InOutCubic(TimeSpan.FromSeconds(1f)))
                    .TakeUntilDestroy(gameObject)
                    .DoOnCompleted(() => Destroy(gameObject))
                    .Subscribe(x =>
                    {
                        c.a = x;
                        spriteRenderer.color = c;
                    });
            }).AddTo(this);
        }

        public void Update()
        {
            if (!IsAlive) return;

            var vec = Velocity * Time.deltaTime;
            var current = new Vector2(transform.position.x, transform.position.y);
            var next = new Vector2(transform.position.x + vec.x, transform.position.y + vec.y);
            var distance = Vector2.Distance(current, next) + Radius;
            var direction = (next - current).normalized;

            var hit = Physics2D.Raycast(current, direction, distance, hitLayerMask);
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.CompareTag("Stage"))
                {
                    next = HitStage(hit, current, direction);
                }
                else
                {
                    hit.collider.gameObject.SendMessage("OnHitBall", this);
                }
            }

            transform.position = new Vector3(next.x, next.y, transform.position.z);
        }

        private Vector2 HitStage(RaycastHit2D hit, Vector2 current, Vector2 direction)
        {
            Velocity.x *= -1f;
            return current + direction * Mathf.Max(hit.distance - Radius, 0.0f);
        }
    }
}
