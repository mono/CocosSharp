using System;

namespace CocosSharp
{
	internal static class CCEaseMath
    {
		internal static float BackIn(float time)
        {
            const float overshoot = 1.70158f;
            
            return time * time * ((overshoot + 1) * time - overshoot);
        }

		internal static float BackOut(float time)
        {
            const float overshoot = 1.70158f;

            time = time - 1;
            return time * time * ((overshoot + 1) * time + overshoot) + 1;
        }

		internal static float BackInOut(float time)
        {
            const float overshoot = 1.70158f * 1.525f;

            time = time * 2;
            if (time < 1)
            {
                return (time * time * ((overshoot + 1) * time - overshoot)) / 2;
            }
            else
            {
                time = time - 2;
                return (time * time * ((overshoot + 1) * time + overshoot)) / 2 + 1;
            }
        }

		internal static float BounceOut(float time)
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

		internal static float BounceIn(float time)
        {
            return 1f - BounceOut(1f - time);
        }

		internal static float BounceInOut(float time)
        {
            if (time < 0.5f)
            {
                time = time * 2;
                return (1 - BounceOut(1 - time)) * 0.5f;
            }
            return BounceOut(time * 2 - 1) * 0.5f + 0.5f;
        }

		internal static float SineOut(float time)
        {
			return (float) Math.Sin(time * CCMathHelper.TwoPi);
        }

		internal static float SineIn(float time)
        {
			return -1f * (float)Math.Cos(time * CCMathHelper.TwoPi) + 1f;
        }

		internal static float SineInOut(float time)
        {
            return -0.5f * ((float)Math.Cos((float)Math.PI * time) - 1f);
        }

		internal static float ExponentialOut(float time)
        {
            return time == 1f ? 1f : (-(float)Math.Pow(2f, -10f * time / 1f) + 1f);
        }

		internal static float ExponentialIn(float time)
        {
            return time == 0f ? 0f : (float)Math.Pow(2f, 10f * (time / 1f - 1f)) - 1f * 0.001f;
        }

		internal static float ExponentialInOut(float time)
        {
            time /= 0.5f;
            if (time < 1)
            {
                return 0.5f * (float)Math.Pow(2f, 10f * (time - 1f));
            }
            else
            {
                return 0.5f * (-(float)Math.Pow(2f, -10f * (time - 1f)) + 2f);
            }
        }

		internal static float ElasticIn(float time, float period)
        {
            if (time == 0 || time == 1)
            {
                return time;
            }
            else
            {
                float s = period / 4;
                time = time - 1;
				return -(float)(Math.Pow(2, 10 * time) * Math.Sin((time - s) * CCMathHelper.Pi * 2.0f / period));
            }
        }

		internal static float ElasticOut(float time, float period)
        {
            if (time == 0 || time == 1)
            {
                return time;
            }
            else
            {
                float s = period / 4;
				return (float)(Math.Pow(2, -10 * time) * Math.Sin((time - s) * CCMathHelper.Pi * 2f / period) + 1);
            }
        }

		internal static float ElasticInOut(float time, float period)
        {
            if (time == 0 || time == 1)
            {
                return time;
            }
            else
            {
                time = time * 2;
                if (period == 0)
                {
                    period = 0.3f * 1.5f;
                }

                float s = period / 4;

                time = time - 1;
                if (time < 0)
                {
					return (float)(-0.5f * Math.Pow(2, 10 * time) * Math.Sin((time - s) * CCMathHelper.TwoPi / period));
                }
                else
                {
					return (float)(Math.Pow(2, -10 * time) * Math.Sin((time - s) * CCMathHelper.TwoPi / period) * 0.5f + 1);
                }
            }
        }

    }
}
