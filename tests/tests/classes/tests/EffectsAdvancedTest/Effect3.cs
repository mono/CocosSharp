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

			var waves = new CCWaves(5, new CCGridSize(15, 10), 5, 20, true, false);
			var shaky = new CCShaky3D(5, new CCGridSize(15, 10), 4, false);

			Target1.RepeatForever(waves);
			Target2.RepeatForever(shaky);

            // moving background. Testing issue #244
			var move = new CCMoveBy (3, new CCPoint(200, 0));

            bgNode.RepeatForever(move, move.Reverse());
        }

		public override string Title
		{
			get
			{
				return "Effects on 2 sprites #2";
			}
		}
    }

    public class Effect6 : EffectAdvanceTextLayer
    {
        public override void OnEnter()
        {
            base.OnEnter();

            Color = CCColor3B.Blue;
            Opacity = 255;

            // Make sure we set the Opacity to cascade or fadein action will not work correctly.
            Target1.IsOpacityCascaded = true;
            Target2.IsOpacityCascaded = true;

            // Define actions
            var moveUp = new CCMoveBy(1.0f, new CCPoint(0.0f, 50.0f));
            var moveDown = moveUp.Reverse();

            // A CCSequence action runs the list of actions in ... sequence!
            var moveSeq = new CCSequence(new CCEaseBackInOut(moveUp), new CCEaseBackInOut(moveDown));

            var repeatedAction = new CCRepeatForever(moveSeq);

            // A CCSpawn action runs the list of actions concurrently
            var dreamAction = new CCSpawn(new CCFadeIn(5.0f), new CCWaves(5.0f, new CCGridSize(10, 20), 5, 20, true, false));


            Target1.RunActions(dreamAction, repeatedAction);

            Target2.RunActions(dreamAction, new CCDelayTime(0.5f), repeatedAction);

            // moving background. Testing issue #244
            var move = new CCMoveBy (3, new CCPoint(200, 0));

            bgNode.RepeatForever(move, move.Reverse());
        }

        public override string Title
        {
            get
            {
                return "Effects on 2 sprites";
            }
        }

        public override string Subtitle {
            get {
                return "Testing CCNodeGrid Opacity Cascading";
            }
        }
    }
}
