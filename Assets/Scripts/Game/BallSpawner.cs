using System;
using GameSystem;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game
{
    public class BallSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject ballPrefab;
        [SerializeField] private Transform barTop;
        [SerializeField] private Transform barBottom;

        private float rangeMin;
        private float rangeMax;
        private const float Sec = 4.0f;

        public void Start()
        {
            var layerMask = LayerMask.GetMask("Stage");
            var size = ballPrefab.GetComponent<Ball>().Radius;
            var leftHit = Physics2D.Raycast(transform.position, Vector2.left, float.PositiveInfinity, layerMask);
            var rightHit = Physics2D.Raycast(transform.position, Vector2.right, float.PositiveInfinity, layerMask);
            rangeMin = transform.position.x - leftHit.distance + size;
            rangeMax = transform.position.x + rightHit.distance - size;

            Messenger.Broker.Receive<RequestBall>().Subscribe(Spawn).AddTo(this);
        }

        private void Spawn(RequestBall args)
        {
            var d = rangeMax - rangeMin;
            var targetPositionX = Random.Range(rangeMin, rangeMax);

            for (var i = 0; i < args.Num; ++i)
            {
                var targetType = args.Types[Random.Range(0, args.Types.Length)];
                var targetPositionY = targetType == Ball.BallType.Top ? barTop.position.y : barBottom.position.y;

                var ballObject = Instantiate(ballPrefab, transform, true);

                var ball = ballObject.GetComponent<Ball>();
                ball.Type = targetType;
                ball.Velocity = new Vector2(Random.Range(-args.VelocityX, args.VelocityX), -Random.Range(args.VelocityY.Item1, args.VelocityY.Item2));

                var startY = targetPositionY - ball.Velocity.y * Sec;
                var calcX = targetPositionX - ball.Velocity.x * Sec;

                var startX = (calcX - rangeMin) % d + rangeMin;
                var b = calcX > rangeMin
                    ? Mathf.FloorToInt((calcX - rangeMin) / d)
                    : Mathf.CeilToInt((calcX - rangeMin) / d);

                if (startX < rangeMin)
                {
                    startX += d;
                    b++;
                }

                startX *= b % 2 == 0 ? 1 : -1;
                ball.Velocity.x = ball.Velocity.x * Mathf.Pow(-1.0f, b);
                ballObject.transform.position = new Vector3(startX, startY, transform.position.z);
            }
        }
    }

    public class RequestBall
    {
        public Ball.BallType[] Types { get; set; }
        public int Num { get; set; }
        public float VelocityX { get; set; }
        public Tuple<float, float> VelocityY { get; set; }
    }
}
