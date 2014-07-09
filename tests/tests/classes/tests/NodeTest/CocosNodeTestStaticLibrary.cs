
using CocosSharp;

namespace tests
{
    public static class CocosNodeTestStaticLibrary
    {
        public static int kTagSprite1 = 1;
        public static int kTagSprite2 = 2;
        public static int kTagSprite3 = 3;


		public static CCRotateBy nodeRotate = new CCRotateBy (5, 360);
		public static CCMoveBy nodeMove = new CCMoveBy (3, new CCPoint(200, 0));
		public static CCOrbitCamera nodeOrbit  = new CCOrbitCamera(10, 0, 1, 0, 360, 0, 90);
    }
}