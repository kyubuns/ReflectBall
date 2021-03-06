﻿using System.Collections;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
// ReSharper disable IteratorNeverReturns

namespace GameSystem
{
    public class SceneFlow : MonoBehaviour
    {
        public IEnumerator Start()
        {
            Messenger.Broker.Receive<InputReloadScene>().Subscribe(_ =>
            {
                Debug.Log("Reload Scene");
                SceneManager.LoadScene("Main");
            }).AddTo(this);

            while (true)
            {
                yield return Messenger.Broker.Receive<InputGameStart>().First().ToYieldInstruction();

                Debug.Log("Game Start");
                Messenger.Broker.Publish(new OnGameStart());

                var message = Messenger.Broker.Receive<OnGameOver>().First().ToYieldInstruction();
                yield return message;

                Debug.Log("Game Finish");
                Messenger.Broker.Publish(new OnGameFinish{ Score = message.Result.Score });
            }
        }
    }

    public class InputReloadScene
    {
    }

    public class InputGameStart
    {
    }

    public class OnGameStart
    {
    }

    public class OnGameFinish
    {
        public int Score { get; set; }
    }
}
