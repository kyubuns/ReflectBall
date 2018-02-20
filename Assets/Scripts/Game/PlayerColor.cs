using System;
using AnimeRx;
using GameSystem;
using UniRx;
using UnityEngine;

namespace Game
{
    public class PlayerColor : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer top;
        [SerializeField] private SpriteRenderer bottom;

        public void Start()
        {
            var c = Colors.Top;
            c.a = 0.0f;
            top.color = c;
            bottom.color = Colors.Bottom;

            Messenger.Broker.Receive<OnShowTopBar>()
                .Select(x => Anime.Play(c.a, 1.0f, Easing.InOutCubic(TimeSpan.FromSeconds(0.8f))))
                .Switch()
                .Subscribe(x =>
                {
                    c.a = x;
                    top.color = c;
                })
                .AddTo(this);
        }
    }

    public class OnShowTopBar
    {
    }
}
