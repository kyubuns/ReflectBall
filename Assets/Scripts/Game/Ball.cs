using System;
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

        private bool inGame;
        private LayerMask hitLayerMask;

        public void Start()
        {
            var spriteRenderer = GetComponent<SpriteRenderer>();
            inGame = true;

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
                inGame = false;
                Destroy(gameObject);
            }).AddTo(this);
        }

        public void Update()
        {
            if (!inGame) return;

            var vec = Velocity * Time.deltaTime;
            var current = new Vector2(transform.position.x, transform.position.y);
            var next = new Vector2(transform.position.x + vec.x, transform.position.y + vec.y);
            var distance = Vector2.Distance(current, next) + Radius;
            var direction = (next - current).normalized;

            var hit = Physics2D.Raycast(current, direction, distance, hitLayerMask);
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.CompareTag("Player"))
                {
                    HitPlayer();
                    return;
                }

                if (hit.collider.gameObject.CompareTag("Dead"))
                {
                    HitDead();
                    return;
                }

                if (hit.collider.gameObject.CompareTag("Eraser"))
                {
                    HitEraser();
                    return;
                }

                if (hit.collider.gameObject.CompareTag("Stage"))
                {
                    next = HitStage(hit, current, direction);
                }
                else
                {
                    throw new ArgumentOutOfRangeException($"Unknown gameobject {hit.collider.gameObject}");
                }
            }

            transform.position = new Vector3(next.x, next.y, transform.position.z);
        }

        private Vector2 HitStage(RaycastHit2D hit, Vector2 current, Vector2 direction)
        {
            Velocity.x *= -1f;
            return current + direction * Mathf.Max(hit.distance - Radius, 0.0f);
        }

        private void HitDead()
        {
            Messenger.Broker.Publish(new OnDropBall());
            Destroy(gameObject);
        }

        private void HitPlayer()
        {
            Messenger.Broker.Publish(new OnRefrectBall());
            Destroy(gameObject);
        }

        private void HitEraser()
        {
            Destroy(gameObject);
        }
    }
}
