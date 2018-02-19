using System.Collections;
using Game;
using UniRx;
using UnityEngine;
// ReSharper disable IteratorNeverReturns

namespace GameSystem
{
    public class Director : MonoBehaviour
    {
        public void Start()
        {
            Messenger.Broker.Receive<OnGameStart>().Subscribe(_ => StartCoroutine(Flow())).AddTo(this);
            Messenger.Broker.Receive<OnGameFinish>().Subscribe(_ => StopAllCoroutines()).AddTo(this);
        }

        private IEnumerator Flow()
        {
            // Tutorial
            Messenger.Broker.Publish(new RequestBall {Types = new[] {Ball.BallType.Bottom}, Num = 1});
            yield return new WaitForSeconds(2.0f);

            Messenger.Broker.Publish(new RequestBall {Types = new[] {Ball.BallType.Bottom}, Num = 2});
            yield return new WaitForSeconds(2.0f);

            Messenger.Broker.Publish(new RequestBall {Types = new[] {Ball.BallType.Bottom}, Num = 3});
            yield return new WaitForSeconds(2.0f);

            Messenger.Broker.Publish(new RequestBall {Types = new[] {Ball.BallType.Top}, Num = 1});
            yield return new WaitForSeconds(2.0f);

            Messenger.Broker.Publish(new RequestBall {Types = new[] {Ball.BallType.Top, Ball.BallType.Bottom}, Num = 3});
            yield return new WaitForSeconds(2.0f);

            Messenger.Broker.Publish(new RequestBall {Types = new[] {Ball.BallType.Top, Ball.BallType.Bottom}, Num = 5});
            yield return new WaitForSeconds(2.0f);

            while (true)
            {
                Messenger.Broker.Publish(new RequestBall {Types = new[] {Ball.BallType.Top, Ball.BallType.Bottom}, Num = 3});
                yield return new WaitForSeconds(1.0f);
            }
        }
    }
}
