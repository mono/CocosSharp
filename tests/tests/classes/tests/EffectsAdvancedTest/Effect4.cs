using CocosSharp;

namespace tests
{
    public class Effect4 : EffectAdvanceTextLayer
    {
        public override void OnEnter()
        {
            base.OnEnter();

			var size = CCDirector.SharedDirector.WinSize;  
			var lens = new CCLens3D(10, new CCGridSize(32, 24), new CCPoint(100, 180), 150);
			var move = new CCJumpBy (5, new CCPoint(380, 0), 100, 4);
            var move_back = move.Reverse();
			var seq = new CCSequence(move, move_back);

            /* In cocos2d-iphone, the type of action's target is 'id', so it supports using the instance of 'CCLens3D' as its target.
                While in cocos2d-x, the target of action only supports CCNode or its subclass,
                so we make an encapsulation for CCLens3D to achieve that.
            */
			var target = new Lens3DTarget(lens);
            // Please make sure the target been added to its parent.
			//            AddChild(target);

            ActionManager.AddAction(seq, target, false);
			RunAction(lens);
        }

        public override string title()
        {
            return "Jumpy Lens3D";
        }

        #region Nested type: Lens3DTarget

        private class Lens3DTarget : CCNode
        {
            private CCLens3D m_pLens3D;

            public override CCPoint Position
            {
                get { return m_pLens3D.Position; }
                set { m_pLens3D.Position = value; }
            }

            public Lens3DTarget (CCLens3D pAction)
            {
                m_pLens3D = pAction;
            }
        }

        #endregion
    }
}