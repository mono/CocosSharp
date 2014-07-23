using CocosSharp;
using System;

namespace tests
{
    public class Effect1 : EffectAdvanceTextLayer
    {
        public override void OnEnter()
        {
            base.OnEnter();

            CCRect visibleBounds = VisibleBoundsWorldspace;

            // To reuse a grid the grid size and the grid type must be the same.
            // in this case:
            //     Lens3D is Grid3D and it's size is (15,10)
            //     Waves3D is Grid3D and it's size is (15,10)


            CCCamera contentCamera = contentLayer.Camera;

            contentCamera.Projection = CCCameraProjection.Projection3D;
            contentCamera.PerspectiveAspectRatio = 1.0f;

            CCPoint3 cameraCenter = contentCamera.CenterInWorldspace;
            CCPoint3 cameraTarget = contentCamera.TargetInWorldspace;

            float targeCenterLength = (cameraTarget - cameraCenter).Length;


            contentCamera.NearAndFarPerspectiveClipping = new CCPoint (0.1f, 100.0f);

            contentCamera.PerspectiveFieldOfView = (float)Math.Atan(visibleBounds.Size.Height / (2.0f * targeCenterLength));

            var lens = new CCLens3D(0.0f, new CCGridSize(30, 20), bgNode.ContentSize.Center, 120);
			var waves = new CCWaves3D(10, new CCGridSize(15, 10), 18, 15);


			var reuse = new CCReuseGrid(1);
			var delay = new CCDelayTime (8);

            bgNode.AnchorPoint = CCPoint.AnchorMiddle;
            var orbit = new CCOrbitCamera(5, 1, 2, 0, 180, 0, -90);
			var orbit_back = orbit.Reverse();

			bgNode.RepeatForever(orbit, orbit_back);
			bgNode.RunActions(lens, delay, reuse, waves);
        }

        public override void OnExit()
        {
            contentLayer.Camera.Projection = CCCameraProjection.Projection2D;

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