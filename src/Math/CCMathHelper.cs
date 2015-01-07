using System;
using System.Diagnostics;

namespace CocosSharp
{
    public static class CCMathHelper
    {
		public const float Pi = (float)Math.PI;
		public const float TwoPi = (float)(Math.PI * 2.0);

		private static float _lastSinAngle = 0f;
        private static float _lastSinValue = (float) Math.Sin(0);
        private static float _lastCosAngle = 0f;
        private static float _lastCosValue = (float) Math.Cos(0);

        public static float Sin(float radian)
        {
            if (radian != _lastSinAngle)
            {
                _lastSinAngle = radian;
                _lastSinValue = (float) Math.Sin(radian);
            }
            return _lastSinValue;
        }

        public static float Cos(float radian)
        {
            if (radian != _lastCosAngle)
            {
                _lastCosAngle = radian;
                _lastCosValue = (float)Math.Cos(radian);
            }
            return _lastCosValue;
        }

        public static float ToDegrees(float radians)
        {
            // This method uses double precission internally,
            // though it returns single float
            // Factor = 180 / pi
            return (float)(radians * 57.295779513082320876798154814105);
        }

        public static float ToRadians(float degrees)
        {
            // This method uses double precission internally,
            // though it returns single float
            // Factor = pi / 180
            return (float)(degrees * 0.017453292519943295769236907684886);
        }

        public static int Lerp(int value1, int value2, float amount)
        {
            return (int) (value1 + ((value2 - value1) * amount));
        }

        /// <summary>
        /// Linearly interpolates between two values.
        /// </summary>
        /// <param name="value1">Source value.</param>
        /// <param name="value2">Source value.</param>
        /// <param name="amount">Value between 0 and 1 indicating the weight of value2.</param>
        /// <returns>Interpolated value.</returns> 
        /// <remarks>This method performs the linear interpolation based on the following formula.
        /// <c>value1 + (value2 - value1) * amount</c>
        /// Passing amount a value of 0 will cause value1 to be returned, a value of 1 will cause value2 to be returned.
        /// </remarks>
        public static float Lerp(float value1, float value2, float amount)
        {
            return value1 + (value2 - value1) * amount;
        }

		public static float Clamp(float value, float min, float max)
		{
			value = (value > max) ? max : value;
			value = (value < min) ? min : value;

			return value;
		}

		public static int Clamp(int value, int min, int max)
		{
			value = (value > max) ? max : value;
			value = (value < min) ? min : value;

			return value;
		}
    }
}
