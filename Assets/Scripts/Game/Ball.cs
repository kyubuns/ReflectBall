using UnityEngine;

namespace Game
{
    public class Ball : MonoBehaviour
    {
        public Vector2 Velocity;
        public float Radius;

        private LayerMask hitLayerMask;

        public void Awake()
        {
            hitLayerMask = LayerMask.GetMask("Stage");
        }

        public void Update()
        {
            var current = new Vector2(transform.position.x, transform.position.y);
            var vec = Velocity * Time.deltaTime;
            var next = new Vector2(transform.position.x + vec.x, transform.position.y + vec.y);
            var distance = Vector2.Distance(current, next);
            var direction = next - current;

            var hit = Physics2D.CircleCast(current, Radius, direction, distance, hitLayerMask);
            if (hit.collider != null)
            {
                next = current + direction.normalized * (hit.distance - 0.01f);
                Velocity.x *= -1f;
            }

            transform.position = new Vector3(next.x, next.y, transform.position.z);
        }
    }
}
