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

            var spriteRenderer = ball.GetComponent<SpriteRenderer>();
            spriteRenderer.color = spriteRenderer.color / 2;
            spriteRenderer.sortingLayerID = SortingLayer.NameToID("ReflectedBall");

            var moveVector2 = ball.Velocity.normalized;
            moveVector2.y *= -1f;
            var moveVector3 = new Vector3(moveVector2.x, moveVector2.y, ball.transform.position.z);

            Anime.PlayRelative(ball.transform.position, moveVector3 * 15f, Easing.OutExpo(TimeSpan.FromSeconds(2.0f)))
                .TakeUntilDestroy(ball.gameObject)
                .DoOnCompleted(() => Destroy(ball.gameObject))
                .SubscribeToPosition(ball.gameObject);

            ball.transform.localRotation = Quaternion.FromToRotation(Vector3.up,  moveVector2);
            Anime.PlayRelative(ball.transform.localScale, new Vector3(-0.2f, 1f, 0f), Easing.OutCubic(TimeSpan.FromSeconds(0.75f)))
                .TakeUntilDestroy(ball.gameObject)
                .SubscribeToLocalScale(ball.gameObject);
        }
    }
}
