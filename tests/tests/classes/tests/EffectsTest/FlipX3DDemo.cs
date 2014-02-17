using CocosSharp;

namespace tests
{
	public class FlipX3DDemo
	{
		public static CCActionInterval ActionWithDuration(float t)
		{
			CCFlipX3D flipx = new CCFlipX3D(t);
			CCFiniteTimeAction flipx_back = flipx.Reverse();
			CCDelayTime delay = new CCDelayTime (2);

			return new CCSequence(flipx, delay, flipx_back);
		}
	}
}