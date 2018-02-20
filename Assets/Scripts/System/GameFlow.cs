using UniRx;
using UnityEngine;

namespace GameSystem
{
    public class GameFlow : MonoBehaviour
    {
        private int score;
        private bool inTutorial;

        public void Start()
        {
            inTutorial = true;

            Messenger.Broker.Receive<OnGameStart>().Subscribe(_ =>
            {
                if (inTutorial) return;

                score = 0;
                Messenger.Broker.Publish(new OnUpdateScore{ Score = score });
            }).AddTo(this);

            Messenger.Broker.Receive<OnRefrectBall>().Subscribe(_ =>
            {
                if (inTutorial) return;
                score++;
                Messenger.Broker.Publish(new OnUpdateScore{ Score = score });
            }).AddTo(this);

            Messenger.Broker.Receive<OnDropBall>().Subscribe(_ =>
            {
                Messenger.Broker.Publish(new OnGameOver{ Score = score });
            }).AddTo(this);

            Messenger.Broker.Receive<FinishTutorial>().Subscribe(_ => inTutorial = false).AddTo(this);
        }
    }

    public class FinishTutorial
    {
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
