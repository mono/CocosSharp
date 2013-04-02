using cocos2d;

namespace tests
{
    public class FlipY3DDemo : CCFlipY3D
    {
        public static CCActionInterval actionWithDuration(float t)
        {
            CCFlipX3D flipx = Create(t);
            CCFiniteTimeAction flipx_back = flipx.Reverse();
            CCDelayTime delay = new CCDelayTime (2);

            return (CCSequence.FromActions(flipx, delay, flipx_back));
        }
    }
}