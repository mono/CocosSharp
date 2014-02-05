using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class Effect5 : EffectAdvanceTextLayer
    {
        public override void OnEnter()
        {
            base.OnEnter();

            //CCDirector::sharedDirector()->setProjection(CCDirectorProjection2D);

			var effect = new CCLiquid (2, new CCGridSize(32, 24), 1, 20);

			var bg = _bgNode;
			bg.RunActions(effect, new CCDelayTime (2), new CCStopGrid());
        }

        public override void OnExit()
        {
            base.OnExit();

            CCDirector.SharedDirector.Projection = CCDirectorProjection.Projection3D;
        }

        public override string title()
        {
			return "Test Stop-Copy-Restart";
        }
    }
}
