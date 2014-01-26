using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class Effect3 : EffectAdvanceTextLayer
    {
        public override void OnEnter()
        {
			base.OnEnter();

			var bg = this[EffectAdvanceScene.kTagBackground];
			var target1 = bg[EffectAdvanceScene.kTagSprite1];
			var target2 = bg[EffectAdvanceScene.kTagSprite2];

			var waves = new CCWaves(5, new CCGridSize(15, 10), 5, 20, true, false);
			var shaky = new CCShaky3D(5, new CCGridSize(15, 10), 4, false);

			target1.RunAction(new CCRepeatForever (waves));
			target2.RunAction(new CCRepeatForever (shaky));

            // moving background. Testing issue #244
			var move = new CCMoveBy (3, new CCPoint(200, 0));

			bg.RunAction(new CCRepeatForever (move, move.Reverse()));
        }

        public override string title()
        {
            return "Effects on 2 sprites";
        }
    }
}
