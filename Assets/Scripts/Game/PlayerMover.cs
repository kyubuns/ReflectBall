using UnityEngine;

namespace Game
{
    public class PlayerMover : MonoBehaviour
    {
        private Vector2 originalPosition;
        private float rangeMin;
        private float rangeMax;

        public void Start()
        {
            originalPosition = transform.position;
            var size = GetComponentInChildren<Collider2D>().bounds.size;
            var leftHit = Physics2D.Raycast(originalPosition, Vector2.left, float.PositiveInfinity, LayerMask.GetMask("Stage"));
            var rightHit = Physics2D.Raycast(originalPosition, Vector2.right, float.PositiveInfinity, LayerMask.GetMask("Stage"));
            rangeMin = originalPosition.x - leftHit.distance + size.x / 2.0f;
            rangeMax = originalPosition.x + rightHit.distance - size.x / 2.0f;
        }

        public void Update()
        {
            var worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPosition.x = Mathf.Clamp(worldPosition.x, rangeMin, rangeMax);
            worldPosition.y = originalPosition.y;
            worldPosition.z = 0.0f;
            transform.position = worldPosition;
        }
    }
}
