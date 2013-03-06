using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class Effect5 : EffectAdvanceTextLayer
    {
        public override void OnEnter()
        {
            base.OnEnter();

            //CCDirector::sharedDirector()->setProjection(CCDirectorProjection2D);

            CCActionInterval effect = CCLiquid.Create(1, 20, new CCGridSize(32, 24), 2);

            CCActionInterval stopEffect = (CCActionInterval)(CCSequence.Create(
                                                 effect,
                                                 CCDelayTime.Create(2),
                                                 CCStopGrid.Create()
                //					 [DelayTime::actionWithDuration:2],
                //					 [[effect copy] autorelease],
                                                 ));

            CCNode bg = GetChildByTag(EffectAdvanceScene.kTagBackground);
            bg.RunAction(stopEffect);
        }

        public override void OnExit()
        {
            base.OnExit();

            CCDirector.SharedDirector.Projection = ccDirectorProjection.kCCDirectorProjection3D;
        }

        public override string title()
        {
            return "Test Stop-Copy-Restar";
        }
    }
}
