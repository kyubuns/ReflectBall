using GameSystem;
using UnityEngine;

namespace Game
{
    public class Dead : MonoBehaviour
    {
        public void OnHitBall(Ball ball)
        {
            Messenger.Broker.Publish(new OnDropBall());
            Destroy(ball.gameObject);
        }
    }
}
