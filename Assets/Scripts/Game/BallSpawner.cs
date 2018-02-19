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
            var d = rangeMax - rangeMin;

            while (true)
            {
                var targetPositionX = Random.Range(rangeMin, rangeMax);

                for (var i = 0; i < 3; ++i)
                {
                    var targetType = new[] {Ball.BallType.Bottom, Ball.BallType.Top}[Random.Range(0, 2)];
                    var targetPositionY = targetType == Ball.BallType.Top ? barTop.position.y : barBottom.position.y;

                    var ballObject = Instantiate(ballPrefab, transform, true);

                    var ball = ballObject.GetComponent<Ball>();
                    ball.Type = targetType;
                    ball.Velocity = new Vector2(Random.Range(-6f, 6f), Random.Range(-2f, -6f));

                    var startY = targetPositionY - ball.Velocity.y * Sec;
                    var calcX = targetPositionX - ball.Velocity.x * Sec;

                    var startX = (calcX - rangeMin) % d + rangeMin;
                    var b = calcX > rangeMin ? Mathf.FloorToInt((calcX - rangeMin) / d) : Mathf.CeilToInt((calcX - rangeMin) / d);
                    if (startX < rangeMin)
                    {
                        startX += d;
                        b++;
                    }
                    startX *= b % 2 == 0 ? 1 : -1;
                    ball.Velocity.x = ball.Velocity.x * Mathf.Pow(-1.0f, b);
                    Debug.Log($"Spawn ({startX}, {startY}) {ball.Velocity}");
                    ballObject.transform.position = new Vector3(startX, startY, transform.position.z);
                }

                yield return new WaitForSeconds(5.0f);
            }
        }
    }
}
