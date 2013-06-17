using System;
using System.Diagnostics;

namespace Cocos2D
{
    public static class CCMathHelper
    {
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
    }
}
