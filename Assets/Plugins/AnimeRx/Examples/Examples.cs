﻿using System;
using UniRx;
using UnityEngine;

namespace AnimeRx.Development
{
    public class Examples : MonoBehaviour
    {
        private CompositeDisposable animatingDisposable = new CompositeDisposable();

        public static readonly string[] Samples = {
            "1: Linear",
            "2: Easing",
            "3: Wait",
            "4: Combine",
            "5: Path",
            "6: Easing to Linear",
        };

        public void Initialize()
        {
            animatingDisposable.Dispose();
            animatingDisposable = new CompositeDisposable();
        }

        public void Sample1()
        {
            var cube1 = CreatePrimitiveCube();

            Anime.Play(new Vector3(-5f, 0f, 0f), new Vector3(5f, 0f, 0f), Motion.Uniform(5f))
                .SubscribeToPosition(cube1)
                .AddTo(cube1);
        }

        public void Sample2()
        {
            var cube1 = CreatePrimitiveCube();

            Anime.Play(new Vector3(-5f, 0f, 0f), new Vector3(5f, 0f, 0f), Easing.InOutQuad(TimeSpan.FromSeconds(3f)))
                .SubscribeToPosition(cube1)
                .AddTo(cube1);
        }

        public void Sample3()
        {
            var cube1 = CreatePrimitiveCube();

            Anime.Play(new Vector3(-5f, 0f, 0f), new Vector3(0f, 0f, 0f), Easing.OutExpo(TimeSpan.FromSeconds(2f)))
                .Wait(TimeSpan.FromSeconds(0.5f))
                .Play(new Vector3(5f, 0f, 0f), Easing.OutExpo(TimeSpan.FromSeconds(2f)))
                .SubscribeToPosition(cube1)
                .AddTo(cube1);
        }

        public void Sample4()
        {
            var cube1 = CreatePrimitiveCube();

            var x = Anime.Play(-5f, 5f, Easing.InOutSine(TimeSpan.FromSeconds(3f)));

            var y = Anime.Play(0f, -3f, Easing.InOutSine(TimeSpan.FromSeconds(1.5f)))
                .Play(0f, Easing.InOutSine(TimeSpan.FromSeconds(1.5f)));

            var z = Anime.Stay(0f);

            Observable.CombineLatest(x, y, z)
                .SubscribeToPosition(cube1)
                .AddTo(cube1);
        }

        public void Sample5()
        {
            var cube1 = CreatePrimitiveCube();

            var positions = new[]
            {
                new Vector3(-5f, 0f, 0f),
                new Vector3(0f, 3f, 0f),
                new Vector3(5f, 0f, 0f),
                new Vector3(0f, -3f, 0f),
                new Vector3(-5f, 0f, 0f),
            };

            Anime.Play(positions, Easing.InOutSine(TimeSpan.FromSeconds(6f)))
                .SubscribeToPosition(cube1)
                .AddTo(cube1);
        }

        public void Sample6()
        {
            var cube1 = CreatePrimitiveCube();

            Anime.PlayIn(-5f, 0f, 5f, Easing.InCubic(TimeSpan.FromSeconds(1.0)))
                .SubscribeToPositionX(cube1)
                .AddTo(cube1);
        }

        private GameObject CreatePrimitiveCube()
        {
            var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.localScale = Vector3.one;
            animatingDisposable.Add(Disposable.Create(() => { Destroy(cube); }));
            return cube;
        }
    }
}
