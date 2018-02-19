using System;
using AnimeRx;
using GameSystem;
using UniRx;
using UnityEngine;

namespace Game
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer sprite;
        private readonly Subject<Unit> subject = new Subject<Unit>();

        public void Start()
        {
            subject.Select(_ => Anime.Play(new Vector3(1.5f, 1.5f, 1.5f), new Vector3(1f, 1f, 1f), Easing.OutElastic(TimeSpan.FromSeconds(0.5f))))
                .Switch()
                .SubscribeToLocalScale(sprite.transform)
                .AddTo(this);
        }

        public void OnHitBall(Ball ball)
        {
            Messenger.Broker.Publish(new OnRefrectBall());

            ball.IsAlive = false;
            subject.OnNext(Unit.Default);

            Anime.PlayRelative(ball.transform.position, new Vector3(0f, 15f, 0f), Easing.OutExpo(TimeSpan.FromSeconds(1.5f)))
                .DoOnCompleted(() => Destroy(ball.gameObject))
                .SubscribeToPosition(ball.gameObject)
                .AddTo(ball.gameObject);

            Anime.PlayRelative(ball.transform.localScale, new Vector3(-0.2f, 1f, 0f), Easing.OutCubic(TimeSpan.FromSeconds(0.5f)))
                .SubscribeToLocalScale(ball.gameObject)
                .AddTo(ball.gameObject);
        }
    }
}
