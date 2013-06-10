using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cocos2D;

namespace tests
{
    public class Effect3 : EffectAdvanceTextLayer
    {
        public override void OnEnter()
        {
            base.OnEnter();

            CCNode bg = GetChildByTag(EffectAdvanceScene.kTagBackground);
            CCNode target1 = bg.GetChildByTag(EffectAdvanceScene.kTagSprite1);
            CCNode target2 = bg.GetChildByTag(EffectAdvanceScene.kTagSprite2);

            CCActionInterval waves = new CCWaves(5, new CCGridSize(15, 10), 5, 20, true, false);
            CCActionInterval shaky = new CCShaky3D(5, new CCGridSize(15, 10), 4, false);

            target1.RunAction(new CCRepeatForever (waves));
            target2.RunAction(new CCRepeatForever (shaky));

            // moving background. Testing issue #244
            CCActionInterval move = new CCMoveBy (3, new CCPoint(200, 0));
            bg.RunAction(new CCRepeatForever (new CCSequence(move, move.Reverse())));
        }

        public override string title()
        {
            return "Effects on 2 sprites";
        }
    }
}
