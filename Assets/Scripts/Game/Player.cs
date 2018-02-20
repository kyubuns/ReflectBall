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
        private readonly TimeSpan anime1 = TimeSpan.FromSeconds(0.05f);

        public void Start()
        {
            subject.Select(_ => Anime
                    .Play(new Vector3(0.8f, 0.6f, 1f), new Vector3(0.8f, 0.6f, 1f), Easing.Linear(anime1))
                    .Play(new Vector3(1.5f, 1.5f, 1f), new Vector3(1f, 1f, 1f), Easing.OutElastic(TimeSpan.FromSeconds(0.5f))))
                .Switch()
                .SubscribeToLocalScale(sprite.transform)
                .AddTo(this);
        }

        public void OnHitBall(Ball ball)
        {
            Messenger.Broker.Publish(new OnRefrectBall());

            ball.IsAlive = false;
            subject.OnNext(Unit.Default);

            var spriteRenderer = ball.GetComponent<SpriteRenderer>();
            spriteRenderer.color = spriteRenderer.color / 1.5f;
            spriteRenderer.sortingLayerID = SortingLayer.NameToID("ReflectedBall");

            var s = ball.transform.localScale;
            s.y /= 2f;
            ball.transform.localScale = s;

            Observable.Timer(anime1)
                .TakeUntilDestroy(ball.gameObject)
                .Subscribe(_ =>
                {
                    Anime.PlayRelative(ball.transform.position, new Vector3(0f, 30f, 0f), Easing.OutExpo(TimeSpan.FromSeconds(3f)))
                        .TakeUntilDestroy(ball.gameObject)
                        .DoOnCompleted(() => Destroy(ball.gameObject))
                        .SubscribeToPosition(ball.transform);

                    Anime.PlayRelative(ball.transform.localScale, new Vector3(-0.3f, 10f, 0f), Easing.OutExpo(TimeSpan.FromSeconds(0.5f)))
                        .TakeUntilDestroy(ball.gameObject)
                        .SubscribeToLocalScale(ball.transform);
                }).AddTo(ball);
        }
    }
}
