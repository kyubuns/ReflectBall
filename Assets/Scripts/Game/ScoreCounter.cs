using GameSystem;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    [RequireComponent(typeof(Text))]
    public class ScoreCounter : MonoBehaviour
    {
        private Text text;
        private bool inTutorial = true;
        private int highScore;

        public void Start()
        {
            text = GetComponent<Text>();
            text.text = "";

            Messenger.Broker.Receive<OnGameStart>().Subscribe(x =>
            {
                if (inTutorial) text.text = "Tutorial";
                else UpdateScoreText(0);
            }).AddTo(this);

            Messenger.Broker.Receive<OnUpdateScore>().Subscribe(x =>
            {
                UpdateScoreText(x.Score);
            }).AddTo(this);

            Messenger.Broker.Receive<FinishTutorial>().Subscribe(_ =>
            {
                inTutorial = false;
                UpdateScoreText(0);
            }).AddTo(this);
        }

        private void UpdateScoreText(int score)
        {
            if (inTutorial) return;
            highScore = Mathf.Max(score, highScore);
            text.text = $"Score: {score}\n(High Score: {highScore})";
        }
    }
}
