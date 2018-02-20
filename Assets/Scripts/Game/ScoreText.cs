using GameSystem;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    [RequireComponent(typeof(Text))]
    public class ScoreText : MonoBehaviour
    {
        private Text text;
        private bool inTutorial = true;

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
            text.text = $"Score: {score}";
        }
    }
}
