using System;
using AnimeRx;
using GameSystem;
using UniRx;
using UnityEngine;

namespace Game
{
    public class Dead : MonoBehaviour
    {
        [SerializeField] private GameObject prefab;

        public void OnHitBall(Ball ball)
        {
            Messenger.Broker.Publish(new OnDropBall());

            var position = ball.transform.position;
            Destroy(ball.gameObject);

            var effect = Instantiate(prefab, transform);
            effect.transform.position = position;

            var c = ball.GetComponent<SpriteRenderer>().color;
            Anime.Play(1.0f, 0.0f, Easing.InOutSine(TimeSpan.FromSeconds(1.5f)))
                .TakeUntilDestroy(effect)
                .Subscribe(x =>
                {
                    c.a = x;
                    effect.GetComponent<SpriteRenderer>().color = c;
                }).AddTo(effect);

            var s = effect.transform.localScale;
            Observable.CombineLatest(
                Anime.Play(0.1f, 20.0f, Easing.InCirc(TimeSpan.FromSeconds(2f))),
                Anime.Play(0.1f, 25.0f, Easing.InCirc(TimeSpan.FromSeconds(0.5f)))
            )
            .TakeUntilDestroy(effect)
            .Subscribe(x =>
            {
                s.x = x[0];
                s.y = x[1];
                effect.transform.localScale = s;
            }).AddTo(effect);
        }
    }
}
