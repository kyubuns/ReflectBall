using GameSystem;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    [RequireComponent(typeof(Text))]
    public class HighScoreText : MonoBehaviour
    {
        private Text text;
        private int highScore;

        public void Start()
        {
            text = GetComponent<Text>();
            text.text = "";

            Messenger.Broker.Receive<OnGameFinish>().Subscribe(x =>
            {
                UpdateScoreText(x.Score);
            }).AddTo(this);

            Messenger.Broker.Receive<FinishTutorial>().Subscribe(_ =>
            {
                UpdateScoreText(0);
            }).AddTo(this);
        }

        private void UpdateScoreText(int score)
        {
            highScore = Mathf.Max(score, highScore);
            text.text = $"HighScore: {highScore}";
        }
    }
}
