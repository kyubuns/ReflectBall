﻿using System.Collections;
using Game;
using UniRx;
using UnityEngine;
// ReSharper disable IteratorNeverReturns

namespace GameSystem
{
    public class Director : MonoBehaviour
    {
        private bool tutorialFinished;

        public void Start()
        {
            Messenger.Broker.Receive<OnGameStart>().Subscribe(_ => StartCoroutine(Flow())).AddTo(this);
            Messenger.Broker.Receive<OnGameFinish>().Subscribe(_ => StopAllCoroutines()).AddTo(this);
        }

        private IEnumerator Flow()
        {
            // Tutorial
            if (!tutorialFinished)
            {
                Messenger.Broker.Publish(new RequestBall {Types = new[] {Ball.BallType.Bottom}, Num = 1});
                yield return new WaitForSeconds(3.0f);

                Messenger.Broker.Publish(new RequestBall {Types = new[] {Ball.BallType.Bottom}, Num = 1});
                yield return new WaitForSeconds(2.0f);

                Messenger.Broker.Publish(new RequestBall {Types = new[] {Ball.BallType.Bottom}, Num = 1});
                yield return new WaitForSeconds(2.0f);

                Messenger.Broker.Publish(new RequestBall {Types = new[] {Ball.BallType.Bottom}, Num = 2});
                yield return new WaitForSeconds(2.0f);

                Messenger.Broker.Publish(new RequestBall {Types = new[] {Ball.BallType.Bottom}, Num = 3});
                yield return new WaitForSeconds(5.0f);

                Messenger.Broker.Publish(new OnShowTopBar());
                yield return new WaitForSeconds(1.0f);

                Messenger.Broker.Publish(new RequestBall {Types = new[] {Ball.BallType.Top}, Num = 1});
                yield return new WaitForSeconds(3.0f);

                Messenger.Broker.Publish(new RequestBall {Types = new[] {Ball.BallType.Top}, Num = 3});
                Messenger.Broker.Publish(new FinishTutorial());
                tutorialFinished = true;
                yield return new WaitForSeconds(3.0f);
            }

            var loop = 0;
            while (true)
            {
                var num = Mathf.RoundToInt(Random.Range(1, 2 + Mathf.Sqrt(loop)));
                var wait = Random.Range(0.2f, Mathf.Max(3.0f - loop * 0.05f, 0.7f));
                Debug.Log($"{loop}: {num}, {wait}");
                Messenger.Broker.Publish(new RequestBall {Types = new[] {Ball.BallType.Top, Ball.BallType.Bottom}, Num = num});

                yield return new WaitForSeconds(wait);
                loop++;
            }
        }
    }
}
