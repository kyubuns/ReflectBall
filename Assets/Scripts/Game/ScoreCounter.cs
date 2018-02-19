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

        public void Start()
        {
            text = GetComponent<Text>();
            text.text = "Tutorial";

            Messenger.Broker.Receive<OnUpdateScore>().Subscribe(x => UpdateScoreText(x.Score)).AddTo(this);
            Messenger.Broker.Receive<FinishTutorial>().Subscribe(_ =>
            {
                inTutorial = false;
                UpdateScoreText(0);
            }).AddTo(this);
        }

        private void UpdateScoreText(int score)
        {
            if (inTutorial) return;
            text.text = $"Score: {score}";
        }
    }
}
