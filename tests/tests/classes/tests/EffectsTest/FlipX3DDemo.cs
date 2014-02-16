using CocosSharp;

namespace tests
{
	public class FlipX3DDemo : CCSequence
    {
        public static FlipX3DDemo CreateWithDuration(float t)
        {
            var flipX = new CCFlipX3D(t);
            var flipX_Reverse = flipX.Reverse();
            var delay = new CCDelayTime (2);
            var actions = new CCFiniteTimeAction[] { flipX, delay, flipX_Reverse};

            return new FlipX3DDemo(actions);
        }

        public FlipX3DDemo (params CCFiniteTimeAction[] actions) : base(actions) 
		{
		}
    }
}