using CocosSharp;

namespace tests
{
	public class FlipX3DDemo : CCSequence
    {
		public FlipX3DDemo (float t) 
			: base()
		{
			var flipX = new CCFlipX3D (t);
			var flipX_Reverse = flipX.Reverse ();
			var delay = new CCDelayTime (2);

			this.Actions = new CCFiniteTimeAction[] { flipX, delay, flipX_Reverse};
		}
    }
}