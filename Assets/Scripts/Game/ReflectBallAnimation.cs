using System;
using AnimeRx;
using GameSystem;
using UniRx;
using UnityEngine;

namespace Game
{
    public class ReflectBallAnimation : MonoBehaviour
    {
        [SerializeField] private Vector3 big;

        public void Start()
        {
            Messenger.Broker.Receive<OnRefrectBall>().Select(_ => Anime.Play(big, Vector3.one, Easing.OutElastic(TimeSpan.FromSeconds(0.5f))))
                .TakeUntilDestroy(gameObject)
                .Switch()
                .SubscribeToLocalScale(transform);
        }
    }
}
