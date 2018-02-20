using UniRx;
using UnityEngine;

namespace GameSystem
{
    public class GameFlow : MonoBehaviour
    {
        private int score;

        public void Start()
        {
            Messenger.Broker.Receive<OnGameStart>().Subscribe(_ =>
            {
                score = 0;
                Messenger.Broker.Publish(new OnUpdateScore{ Score = score });
            }).AddTo(this);

            Messenger.Broker.Receive<OnRefrectBall>().Subscribe(_ =>
            {
                score++;
                Messenger.Broker.Publish(new OnUpdateScore{ Score = score });
            }).AddTo(this);

            Messenger.Broker.Receive<OnDropBall>().Subscribe(_ =>
            {
                Messenger.Broker.Publish(new OnGameOver{ Score = score });
            }).AddTo(this);
        }
    }

    public class OnDropBall
    {
    }

    public class OnUpdateScore
    {
        public int Score { get; set; }
    }

    public class OnRefrectBall
    {
    }

    public class OnGameOver
    {
        public int Score { get; set; }
    }
}
