using System;
using System.Diagnostics;

namespace CocosSharp
{
    // Extra action for making a CCSequence or CCSpawn when only adding one action to it.
    internal class CCExtraAction : CCFiniteTimeAction
    {
        public override CCFiniteTimeAction Reverse ()
        {
            return new CCExtraAction ();
        }

        internal override CCActionState StartAction(CCNode target)
        {
            return new CCExtraActionState (this, target);

        }

        #region Action State

        public class CCExtraActionState : CCFiniteTimeActionState
        {

            public CCExtraActionState (CCExtraAction action, CCNode target)
                : base (action, target)
            {
            }

            protected internal override void Step (float dt)
            {
            }

            public override void Update (float time)
            {
            }
        }

        #endregion Action State
    }
}