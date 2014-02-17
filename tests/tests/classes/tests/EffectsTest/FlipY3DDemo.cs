using CocosSharp;

namespace tests
{
	public class FlipY3DDemo
    {
		public static CCActionInterval ActionWithDuration(float t)
        {
            var flipY = new CCFlipY3D (t);
            var flipY_Reverse = flipY.Reverse();
            var delay = new CCDelayTime (2);

			return new CCSequence(flipY, delay, flipY_Reverse);
        }

    }
}