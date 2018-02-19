using Game.System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class GameStartButton : MonoBehaviour
    {
        public void Start()
        {
            GetComponent<Button>().OnClickAsObservable().Subscribe(_ =>
            {
                Messenger.Broker.Publish(new InputGameStart());
            }).AddTo(this);
        }
    }
}
