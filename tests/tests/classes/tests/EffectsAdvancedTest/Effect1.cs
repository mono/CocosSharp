using CocosSharp;

namespace tests
{
    public class Effect1 : EffectAdvanceTextLayer
    {
        public override void OnEnter()
        {
            base.OnEnter();

            // To reuse a grid the grid size and the grid type must be the same.
            // in this case:
            //     Lens3D is Grid3D and it's size is (15,10)
            //     Waves3D is Grid3D and it's size is (15,10)

			var lens = new CCLens3D(0.0f, new CCGridSize(15, 10), CCVisibleRect.Center, 240);
			var waves = new CCWaves3D(10, new CCGridSize(15, 10), 18, 15);


			var reuse = new CCReuseGrid(1);
			var delay = new CCDelayTime (8);

			var orbit = new CCOrbitCamera(5, 1, 2, 0, 180, 0, -90);
			var orbit_back = orbit.Reverse();

			_bgNode.RepeatForever(orbit, orbit_back);
			_bgNode.RunActions(lens, delay, reuse, waves);
        }

        public override string title()
        {
            return "Lens + Waves3d and OrbitCamera";
        }
    }
}