using System.Collections;
using Game;
using UniRx;
using UnityEngine;
// ReSharper disable IteratorNeverReturns

namespace GameSystem
{
    public class Director : MonoBehaviour
    {
        private bool inGame;
        private bool tutorialFinished;

        public void Start()
        {
            Messenger.Broker.Receive<OnGameStart>().Subscribe(_ =>
            {
                inGame = true;
                StartCoroutine(Flow());
            }).AddTo(this);

            Messenger.Broker.Receive<OnGameFinish>().Subscribe(_ =>
            {
                inGame = false;
                StopAllCoroutines();
            }).AddTo(this);

            Messenger.Broker.Receive<InputRequestTutorialSkip>().Subscribe(_ =>
            {
                if (inGame) return;
                Messenger.Broker.Publish(new OnShowTopBar());
                Messenger.Broker.Publish(new FinishTutorial());
            }).AddTo(this);

            Messenger.Broker.Receive<FinishTutorial>().Subscribe(_ => tutorialFinished = true).AddTo(this);
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
                yield return new WaitForSeconds(3.0f);

                Messenger.Broker.Publish(new RequestBall {Types = new[] {Ball.BallType.Top, Ball.BallType.Bottom}, Num = 3});
                yield return new WaitForSeconds(3.0f);

                Messenger.Broker.Publish(new RequestBall {Types = new[] {Ball.BallType.Top, Ball.BallType.Bottom}, Num = 3});
                yield return new WaitForSeconds(3.0f);

                Messenger.Broker.Publish(new RequestBall {Types = new[] {Ball.BallType.Top, Ball.BallType.Bottom}, Num = 3});
                yield return new WaitForSeconds(5.0f);

                Messenger.Broker.Publish(new FinishTutorial());
                tutorialFinished = true;
            }

            var loop = 1;
            while (true)
            {
                var numMax = Mathf.Clamp((loop / 3) + 1, 1, 5);
                var num = Random.Range(1, numMax + 1);
                if (Random.Range(0, 3) % 3 == 0) num = 1;

                var wait = Mathf.Clamp(3.0f - loop / 10.0f, 0.3f, 3.0f);
                Messenger.Broker.Publish(new RequestBall {Types = new[] {Ball.BallType.Top, Ball.BallType.Bottom}, Num = num});
                Debug.Log($"{loop}: {num}({numMax}) {wait}");

                yield return new WaitForSeconds(wait);
                loop++;
            }
        }
    }
}
