using System.Collections;
using GameSystem;
using UniRx;
using UnityEngine;
// ReSharper disable IteratorNeverReturns

namespace Game
{
    public class BallSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject ballPrefab;
        [SerializeField] private GameObject barTop;
        [SerializeField] private GameObject barBottom;

        private float rangeMin;
        private float rangeMax;
        private const float Sec = 3.0f;

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
                yield return new WaitForSeconds(Random.Range(0.3f, 1.0f));

                var ballObject = Instantiate(ballPrefab, transform, true);

                var ball = ballObject.GetComponent<Ball>();
                ball.Type = new[] {Ball.BallType.Bottom, Ball.BallType.Top}[Random.Range(0, 2)];
                ball.Velocity = new Vector2(Random.Range(-6f, 6f), Random.Range(-2f, -6f));

                var barPosition = ball.Type == Ball.BallType.Top
                    ? barTop.transform.position.y
                    : barBottom.transform.position.y;

                ballObject.transform.position = new Vector3(
                    Random.Range(rangeMin, rangeMax),
                    barPosition - ball.Velocity.y * Sec,
                    transform.position.z);

                yield break;
            }
        }
    }
}
