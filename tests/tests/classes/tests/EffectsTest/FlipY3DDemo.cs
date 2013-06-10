using Cocos2D;

namespace tests
{
    public class FlipY3DDemo : CCFlipY3D
    {
        public static CCActionInterval actionWithDuration(float t)
        {
            CCFlipX3D flipx = new CCFlipY3D(t);
            CCFiniteTimeAction flipx_back = flipx.Reverse();
            CCDelayTime delay = new CCDelayTime (2);

            return (new CCSequence(flipx, delay, flipx_back));
        }
    }
}