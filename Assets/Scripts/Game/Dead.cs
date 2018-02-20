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
            Anime.Play(1.0f, 0.0f, Easing.InOutCubic(TimeSpan.FromSeconds(2f)))
                .Subscribe(x =>
                {
                    c.a = x;
                    effect.GetComponent<SpriteRenderer>().color = c;
                }).AddTo(this);

            var s = effect.transform.localScale;
            Observable.CombineLatest(
                Anime.Play(0.3f, 0.0f, Easing.OutExpo(TimeSpan.FromSeconds(2f))),
                Anime.Play(1.0f, 25.0f, Easing.OutExpo(TimeSpan.FromSeconds(1f)))
            ).Subscribe(x =>
            {
                s.x = x[0];
                s.y = x[1];
                effect.transform.localScale = s;
            }).AddTo(this);
        }
    }
}
