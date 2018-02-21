using System;
using System.Collections;
using Game;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

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
                // 真っ直ぐ下に1つ落とす
                Messenger.Broker.Publish(new RequestBall
                {
                    Types = new[] {Ball.BallType.Bottom},
                    Num = 1,
                    VelocityX = 0f,
                    VelocityY = Tuple.Create(3f, 3f)
                });
                yield return new WaitForSeconds(3.0f);

                // ちょっと角度をつけて1つ
                Messenger.Broker.Publish(new RequestBall
                {
                    Types = new[] {Ball.BallType.Bottom},
                    Num = 1,
                    VelocityX = 2f,
                    VelocityY = Tuple.Create(3f, 3f)
                });
                yield return new WaitForSeconds(2.0f);

                // もう一回
                Messenger.Broker.Publish(new RequestBall
                {
                    Types = new[] {Ball.BallType.Bottom},
                    Num = 1,
                    VelocityX = 3f,
                    VelocityY = Tuple.Create(3f, 3f)
                });
                yield return new WaitForSeconds(2.0f);

                // 数を増やす
                Messenger.Broker.Publish(new RequestBall
                {
                    Types = new[] {Ball.BallType.Bottom},
                    Num = 2,
                    VelocityX = 3f,
                    VelocityY = Tuple.Create(3f, 3f)
                });
                yield return new WaitForSeconds(2.0f);

                // 3つ
                Messenger.Broker.Publish(new RequestBall
                {
                    Types = new[] {Ball.BallType.Bottom},
                    Num = 3,
                    VelocityX = 3f,
                    VelocityY = Tuple.Create(3f, 3f)
                });
                yield return new WaitForSeconds(5.0f);

                // 上のバーも出して
                Messenger.Broker.Publish(new OnShowTopBar());
                yield return new WaitForSeconds(1.0f);

                // 同じことを繰り返し
                Messenger.Broker.Publish(new RequestBall
                {
                    Types = new[] {Ball.BallType.Top},
                    Num = 1,
                    VelocityX = 3f,
                    VelocityY = Tuple.Create(3f, 3f)
                });
                yield return new WaitForSeconds(3.0f);

                Messenger.Broker.Publish(new RequestBall
                {
                    Types = new[] {Ball.BallType.Top},
                    Num = 2,
                    VelocityX = 3f,
                    VelocityY = Tuple.Create(3f, 3f)
                });
                yield return new WaitForSeconds(3.0f);

                // 混合タイプ
                Messenger.Broker.Publish(new RequestBall
                {
                    Types = new[] {Ball.BallType.Top, Ball.BallType.Bottom},
                    Num = 3,
                    VelocityX = 3f,
                    VelocityY = Tuple.Create(3f, 3f)
                });
                yield return new WaitForSeconds(3.0f);

                Messenger.Broker.Publish(new RequestBall
                {
                    Types = new[] {Ball.BallType.Top, Ball.BallType.Bottom},
                    Num = 3,
                    VelocityX = 3f,
                    VelocityY = Tuple.Create(3f, 3f)
                });
                yield return new WaitForSeconds(3.0f);

                // 速度もばらつかせる
                Messenger.Broker.Publish(new RequestBall
                {
                    Types = new[] {Ball.BallType.Top, Ball.BallType.Bottom},
                    Num = 3,
                    VelocityX = 3f,
                    VelocityY = Tuple.Create(3f, 5f)
                });
                yield return new WaitForSeconds(3.0f);

                // 速度もばらつかせる
                Messenger.Broker.Publish(new RequestBall
                {
                    Types = new[] {Ball.BallType.Top, Ball.BallType.Bottom},
                    Num = 3,
                    VelocityX = 3f,
                    VelocityY = Tuple.Create(3f, 5f)
                });
                yield return new WaitForSeconds(3.0f);

                yield return new WaitForSeconds(2.0f);
                Messenger.Broker.Publish(new FinishTutorial());
                tutorialFinished = true;
            }

            var loop = 1;
            while (true)
            {
                var numMax = Mathf.Clamp((loop / 3) + 1, 1, 5);
                var num = Random.Range(1, numMax + 1);
                var velocityX = Mathf.Clamp((loop / 6f) + 2f, 2f, 10f);
                var velocityY = Mathf.Clamp((loop / 8f) + 3f, 3f, 6f);
                var wait = Mathf.Clamp(1.5f - loop / 50.0f, 0.35f, 1.0f) + Random.Range(-0.1f, 0.1f);
                var types =
                    Random.Range(0, 5) != 0 ? new[] {Ball.BallType.Top, Ball.BallType.Bottom} :
                    Random.Range(0, 2) == 0 ? new[] {Ball.BallType.Top} : new[] {Ball.BallType.Bottom};

                Messenger.Broker.Publish(new RequestBall
                {
                    Types = types,
                    Num = num,
                    VelocityX = velocityX,
                    VelocityY = Tuple.Create(3f, velocityY)
                });
                Debug.Log($"{loop}: {num}({numMax}) x:{velocityX:0.00} y:{velocityY:0.00} wait:{wait:0.00}");

                yield return new WaitForSeconds(wait);
                loop++;
            }
        }
    }
}
