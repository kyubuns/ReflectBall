using UnityEngine;

namespace Game
{
    public class PlayerMover : MonoBehaviour
    {
        private Vector2 originalPosition;

        public void Awake()
        {
            originalPosition = transform.position;
        }

        public void Update()
        {
            var worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPosition.y = originalPosition.y;
            worldPosition.z = 0.0f;
            transform.position = worldPosition;
        }
    }
}
