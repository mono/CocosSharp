


using System;
using Microsoft.Xna.Framework;

namespace Cocos2D
{
    public static class CCEase
    {
        public static float BounceOut(float time)
        {
            if (time < 1 / 2.75)
            {
                return 7.5625f * time * time;
            }
            else if (time < 2 / 2.75)
            {
                time -= 1.5f / 2.75f;
                return 7.5625f * time * time + 0.75f;
            }
            else if (time < 2.5 / 2.75)
            {
                time -= 2.25f / 2.75f;
                return 7.5625f * time * time + 0.9375f;
            }

            time -= 2.625f / 2.75f;
            return 7.5625f * time * time + 0.984375f;
        }

        public static float BounceIn(float time)
        {
            return 1f - BounceOut(1f - time);
        }

        public static float BounceInOut(float time)
        {
            if (time < 0.5f)
            {
                time = time * 2;
                return (1 - BounceOut(1 - time)) * 0.5f;
            }
            return BounceOut(time * 2 - 1) * 0.5f + 0.5f;
        }

        public static float SineOut(float time)
        {
            return (float) Math.Sin(time * MathHelper.TwoPi);
        }

        public static float SineIn(float time)
        {
            return -1f * (float)Math.Cos(time * MathHelper.TwoPi) + 1f;
        }

        public static float SineInOut(float time)
        {
            return -0.5f * ((float)Math.Cos((float)Math.PI * time) - 1f);
        }

        public static float ExponentialOut(float time)
        {
            return time == 1f ? 1f : (-(float) Math.Pow(2f, -10f * time / 1f) + 1f);
        }

        public static float ExponentialIn(float time)
        {
            return time == 0f ? 0f : (float) Math.Pow(2f, 10f * (time / 1f - 1f)) - 1f * 0.001f;
        }

        public static float ExponentialInOut(float time)
        {
            time /= 0.5f;
            if (time < 1)
            {
                return 0.5f * (float) Math.Pow(2, 10 * (time - 1));
            }
            return 0.5f * (-(float) Math.Pow(2, 10 * (time - 1)) + 2);
        }
    }
}
