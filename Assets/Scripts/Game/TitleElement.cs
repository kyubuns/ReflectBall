using System;
using System.Collections.Generic;
using System.Linq;
using AnimeRx;
using GameSystem;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    [RequireComponent(typeof(CanvasGroup))]
    public class TitleElement : MonoBehaviour
    {
        public void Start()
        {
            var canvas = GetComponent<CanvasGroup>();
            var selectables = new List<Selectable>();

            Messenger.Broker.Receive<OnGameStart>()
                .Subscribe(_ =>
                {
                    selectables = GetComponentsInChildren<Selectable>().Where(x => x.interactable).ToList();
                    foreach (var selectable in selectables)
                    {
                        selectable.interactable = false;
                    }
                    Anime.Play(1.0f, 0.0f, Easing.OutCubic(TimeSpan.FromSeconds(0.3f)))
                        .Subscribe(x => { canvas.alpha = x; });
                })
                .AddTo(this);

            Messenger.Broker.Receive<OnGameFinish>()
                .Subscribe(_ =>
                {
                    Anime.Wait<float>(TimeSpan.FromSeconds(1.0f))
                        .Play(0.0f, 1.0f, Easing.InCubic(TimeSpan.FromSeconds(0.3f)))
                        .DoOnCompleted(() =>
                        {
                            foreach (var selectable in selectables)
                            {
                                selectable.interactable = true;
                            }
                        })
                        .Subscribe(x => { canvas.alpha = x; });
                })
                .AddTo(this);
        }
    }
}
