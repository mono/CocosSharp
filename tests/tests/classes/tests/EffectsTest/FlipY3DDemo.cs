using CocosSharp;

namespace tests
{
	public class FlipY3DDemo : CCSequence
    {
		public FlipY3DDemo (float t) 
		{
			var flipY = new CCFlipY3D (t);
			var flipY_Reverse = flipY.Reverse ();
			var delay = new CCDelayTime (2);

			this.Actions = new CCFiniteTimeAction[] {flipY, delay, flipY_Reverse};
		}

    }
}