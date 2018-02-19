using System;
using Game.System;
using UnityEngine;

namespace Game
{
    public class Ball : MonoBehaviour
    {
        public Vector2 Velocity;
        public float Radius;
        public BallType Type;
        public Color Top;
        public Color Bottom;

        public enum BallType
        {
            Top,
            Bottom,
        }

        private LayerMask hitLayerMask;

        public void Start()
        {
            var spriteRenderer = GetComponent<SpriteRenderer>();

            switch (Type)
            {
                case BallType.Top:
                    spriteRenderer.color = Top;
                    hitLayerMask = LayerMask.GetMask("Stage", "TopBar");
                    break;

                case BallType.Bottom:
                    spriteRenderer.color = Bottom;
                    hitLayerMask = LayerMask.GetMask("Stage", "BottomBar");
                    break;

                default:
                    throw new ArgumentOutOfRangeException($"Unknown Type {Type}");
            }
        }

        public void Update()
        {
            var vec = Velocity * Time.deltaTime;
            var current = new Vector2(transform.position.x, transform.position.y);
            var next = new Vector2(transform.position.x + vec.x, transform.position.y + vec.y);
            var distance = Vector2.Distance(current, next);
            var direction = next - current;

            var hit = Physics2D.CircleCast(current, Radius, direction, distance, hitLayerMask);
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
            return current + direction.normalized * (hit.distance - 0.01f);
        }

        private void HitDead()
        {
            Messenger.Broker.Publish(new OnGameOver());
        }

        private void HitPlayer()
        {
            Destroy(gameObject);
        }
    }
}
