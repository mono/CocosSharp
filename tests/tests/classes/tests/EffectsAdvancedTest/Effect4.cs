using CocosSharp;
using System;

namespace tests
{
    public class Effect4 : EffectAdvanceTextLayer
    {
        public override void OnEnter()
        {
            base.OnEnter();

            CCRect visibleBounds = VisibleBoundsWorldspace;

			var lens = new CCLens3D(10, new CCGridSize(64, 48), new CCPoint(100, 180), 80);
			var move = new CCJumpBy (5, new CCPoint(600, 0), 100, 5);
            var move_back = move.Reverse();

            CCLens3DState lensState = bgNode.RunAction(lens) as CCLens3DState;

            var target = new Lens3DTarget(lensState);

            // Please make sure the target has been added to its parent.
            AddChild(target);

            target.AddActions(false, move, move_back);
        }

        public override void OnExit()
        {

            base.OnExit();
        }

		public override string Title
		{
			get
			{
				return "Jumpy Lens3D";
			}
		}

        #region Nested type: Lens3DTarget

        private class Lens3DTarget : CCNode
        {
            CCLens3DState lensState;

            public override CCPoint Position
            {
                get { return lensState.Position; }
                set { lensState.Position = value; }
            }

            public Lens3DTarget (CCLens3DState state)
            {
                lensState = state;
            }
        }

        #endregion
    }
}