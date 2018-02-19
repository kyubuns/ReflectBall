using UnityEngine;

namespace Game
{
    public class Eraser : MonoBehaviour
    {
        public void OnHitBall(Ball ball)
        {
            Destroy(ball.gameObject);
        }
    }
}
