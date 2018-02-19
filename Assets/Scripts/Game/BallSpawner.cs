using System.Collections;
using GameSystem;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

// ReSharper disable IteratorNeverReturns

namespace Game
{
    public class BallSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject ballPrefab;
        [SerializeField] private Transform barTop;
        [SerializeField] private Transform barBottom;

        private float rangeMin;
        private float rangeMax;
        private const float Sec = 6.0f;

        public void Start()
        {
            var layerMask = LayerMask.GetMask("Stage");
            var size = ballPrefab.GetComponent<Ball>().Radius;
            var leftHit = Physics2D.Raycast(transform.position, Vector2.left, float.PositiveInfinity, layerMask);
            var rightHit = Physics2D.Raycast(transform.position, Vector2.right, float.PositiveInfinity, layerMask);
            rangeMin = transform.position.x - leftHit.distance + size;
            rangeMax = transform.position.x + rightHit.distance - size;

            Messenger.Broker.Receive<OnGameStart>().Subscribe(_ => StartCoroutine(Spawner())).AddTo(this);
            Messenger.Broker.Receive<OnGameFinish>().Subscribe(_ => StopAllCoroutines()).AddTo(this);
        }

        private IEnumerator Spawner()
        {
            while (true)
            {
                yield return new WaitForSeconds(1.0f);

                var targetPositionX = 0.0f;

                for (var i = 0; i < 5; ++i)
                {
                    var targetType = new[] {Ball.BallType.Bottom, Ball.BallType.Top}[Random.Range(0, 2)];
                    var targetPositionY = targetType == Ball.BallType.Top ? barTop.position.y : barBottom.position.y;

                    var ballObject = Instantiate(ballPrefab, transform, true);

                    var ball = ballObject.GetComponent<Ball>();
                    ball.Type = targetType;
                    ball.Velocity = new Vector2(Random.Range(-6f, 6f), Random.Range(-2f, -6f));

                    var startY = targetPositionY - ball.Velocity.y * Sec;

                    var startX = targetPositionX - ball.Velocity.x * Sec;
                    var d = rangeMax - rangeMin;
                    while (startX < rangeMin || rangeMax < startX)
                    {
                        startX += startX > 0f ? -d : d;
                    }

                    ballObject.transform.position = new Vector3(startX, startY, transform.position.z);
                }
            }
        }
    }
}
