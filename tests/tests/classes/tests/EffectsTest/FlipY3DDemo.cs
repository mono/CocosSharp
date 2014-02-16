using CocosSharp;

namespace tests
{
	public class FlipY3DDemo : CCSequence
    {
        public static FlipY3DDemo CreateWithDuration(float t)
        {
            var flipY = new CCFlipY3D (t);
            var flipY_Reverse = flipY.Reverse();
            var delay = new CCDelayTime (2);
            var actions = new CCFiniteTimeAction[] {flipY, delay, flipY_Reverse};

            return new FlipY3DDemo(actions);
        }

        public FlipY3DDemo(params CCFiniteTimeAction[] actions) : base(actions) 
        {
        }

    }
}