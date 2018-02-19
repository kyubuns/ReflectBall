using GameSystem;
using UnityEngine;

namespace Game
{
    public class Player : MonoBehaviour
    {
        public void OnHitBall(Ball ball)
        {
            Messenger.Broker.Publish(new OnRefrectBall());
            Destroy(ball.gameObject);
        }
    }
}
