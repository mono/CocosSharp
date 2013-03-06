using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

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

            CCActionInterval waves = CCWaves.Create(5, 20, true, false, new CCGridSize(15, 10), 5);
            CCActionInterval shaky = CCShaky3D.Create(4, false, new CCGridSize(15, 10), 5);

            target1.RunAction(CCRepeatForever.Create(waves));
            target2.RunAction(CCRepeatForever.Create(shaky));

            // moving background. Testing issue #244
            CCActionInterval move = CCMoveBy.Create(3, new CCPoint(200, 0));
            bg.RunAction(CCRepeatForever.Create(CCSequence.Create(move, move.Reverse())));
        }

        public override string title()
        {
            return "Effects on 2 sprites";
        }
    }
}
