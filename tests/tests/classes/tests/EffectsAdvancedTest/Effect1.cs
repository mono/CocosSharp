using CocosSharp;
using System;

namespace tests
{
    public class Effect1 : EffectAdvanceTextLayer
    {
        public override void OnEnter()
        {
            base.OnEnter();

            GameView.DepthTesting = true;

            CCRect visibleBounds = VisibleBoundsWorldspace;

            // To reuse a grid the grid size and the grid type must be the same.
            // in this case:
            //     Lens3D is Grid3D and it's size is (15,10)
            //     Waves3D is Grid3D and it's size is (15,10)

            var lens = new CCLens3D(0.0f, new CCGridSize(15, 10), visibleBounds.Center, 200);
			var waves = new CCWaves3D(10, new CCGridSize(15, 10), 18, 30);


			var reuse = new CCReuseGrid(1);
			var delay = new CCDelayTime (8);

            //bgNode.AnchorPoint = CCPoint.AnchorMiddle;
            var orbit = new CCOrbitCamera(5, 1.0f, 2, 0, 180, 0, -90);
			var orbit_back = orbit.Reverse();

			//bgNode.RepeatForever(orbit, orbit_back);
			bgNode.RunActions(lens, delay, reuse, waves);
        }

        public override void OnExit()
        {
            base.OnExit();
        }

		public override string Title
		{
			get
			{
				return "Lens + Waves3d and OrbitCamera";
			}
		}
    }
}