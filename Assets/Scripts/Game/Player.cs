using System;
using AnimeRx;
using GameSystem;
using UniRx;
using UnityEngine;

namespace Game
{
    public class Player : MonoBehaviour
    {
        public void OnHitBall(Ball ball)
        {
            Messenger.Broker.Publish(new OnRefrectBall());

            ball.IsAlive = false;

            Anime.PlayRelative(ball.transform.position, new Vector3(0f, 15f, 0f), Easing.OutExpo(TimeSpan.FromSeconds(1.5f)))
                .SubscribeToPosition(ball.gameObject)
                .AddTo(ball.gameObject);

            Anime.PlayRelative(ball.transform.localScale, new Vector3(-0.2f, 1f, 0f), Easing.OutCubic(TimeSpan.FromSeconds(0.5f)))
                .SubscribeToLocalScale(ball.gameObject)
                .AddTo(ball.gameObject);
        }
    }
}
