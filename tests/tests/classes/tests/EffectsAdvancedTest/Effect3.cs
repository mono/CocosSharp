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
				return "Effects on 2 sprites";
			}
		}
    }
}
