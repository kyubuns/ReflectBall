using System.Collections;
using Game.System;
using UniRx;
using UnityEngine;

namespace Game
{
    public class BallSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject ballPrefab;

        private float rangeMin;
        private float rangeMax;

        public void Start()
        {
            ballPrefab.SetActive(false);

            var layerMask = LayerMask.GetMask("Stage");
            var size = ballPrefab.GetComponent<Ball>().Radius;
            var leftHit = Physics2D.Raycast(transform.position, Vector2.left, float.PositiveInfinity, layerMask);
            var rightHit = Physics2D.Raycast(transform.position, Vector2.right, float.PositiveInfinity, layerMask);
            rangeMin = transform.position.x - leftHit.distance + size / 2.0f;
            rangeMax = transform.position.x + rightHit.distance - size / 2.0f;

            Messenger.Broker.Receive<OnGameStart>().Subscribe(_ => StartCoroutine(Spawner())).AddTo(this);
            Messenger.Broker.Receive<OnGameFinish>().Subscribe(_ => StopAllCoroutines()).AddTo(this);
        }

        private IEnumerator Spawner()
        {
            while (true)
            {
                yield return new WaitForSeconds(1.0f);

                var ballObject = Instantiate(ballPrefab, transform, true);
                ballObject.transform.position = new Vector3(Random.Range(rangeMin, rangeMax), transform.position.y, transform.position.z);

                var ball = ballObject.GetComponent<Ball>();
                ball.Type = new []{ Ball.BallType.Bottom, Ball.BallType.Top }[Random.Range(0, 2)];
                ball.Velocity = new Vector2(Random.Range(-6f, 6f), Random.Range(-2f, -6f));

                ballObject.SetActive(true);
            }
        }
    }
}
