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

        public void Start()
        {
            text = GetComponent<Text>();

            UpdateScoreText(0);
            Messenger.Broker.Receive<OnUpdateScore>().Subscribe(x => UpdateScoreText(x.Score)).AddTo(this);
        }

        private void UpdateScoreText(int score)
        {
            text.text = $"Score: {score}";
        }
    }
}
