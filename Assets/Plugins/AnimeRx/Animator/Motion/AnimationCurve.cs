﻿using System;
using UnityEngine;

namespace AnimeRx
{
    public static partial class Motion
    {
        public static IAnimator From(AnimationCurve curve, float velocity = 1.0f)
        {
            return new AnimationCurveVelocityAnimator(curve, velocity);
        }

        public static IAnimator From(AnimationCurve curve, TimeSpan duration)
        {
            return new AnimationCurveTimeAnimator(curve, duration);
        }

        private class AnimationCurveVelocityAnimator : IAnimator
        {
            private readonly AnimationCurve curve;
            private readonly float velocity;

            public AnimationCurveVelocityAnimator(AnimationCurve curve, float velocity)
            {
                this.curve = curve;
                this.velocity = velocity;
            }

            public float CalcFinishTime(float distance)
            {
                return curve.keys[curve.keys.Length - 1].time / velocity;
            }

            public float CalcPosition(float time, float distance)
            {
                return curve.Evaluate(time / CalcFinishTime(distance));
            }
        }

        private class AnimationCurveTimeAnimator : IAnimator
        {
            private readonly AnimationCurve curve;
            private readonly float duration;

            public AnimationCurveTimeAnimator(AnimationCurve curve, TimeSpan duration)
            {
                this.curve = curve;
                this.duration = (float) duration.TotalSeconds;
            }

            public float CalcFinishTime(float distance)
            {
                return duration;
            }

            public float CalcPosition(float time, float distance)
            {
                return curve.Evaluate(time / duration);
            }
        }
    }
}
