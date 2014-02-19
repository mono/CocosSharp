using CocosSharp;

namespace tests
{
    public class Effect4 : EffectAdvanceTextLayer
    {
        public override void OnEnter()
        {
            base.OnEnter();

			var lens = new CCLens3D(10, new CCGridSize(32, 24), new CCPoint(100, 180), 150);
			var move = new CCJumpBy (5, new CCPoint(380, 0), 100, 4);
            var move_back = move.Reverse();

            CCLens3DState lensState = _bgNode.RunAction(lens) as CCLens3DState;

            var target = new Lens3DTarget(lensState);

            // Please make sure the target has been added to its parent.
            AddChild(target);

            target.AddActions(false, move, move_back);
        }

        public override string title()
        {
            return "Jumpy Lens3D";
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