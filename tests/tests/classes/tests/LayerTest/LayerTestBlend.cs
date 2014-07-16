using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class LayerTestBlend : LayerTest
    {
        int kTagLayer = 1;
        string s_pPathSister1 = "Images/grossinis_sister1";
        string s_pPathSister2 = "Images/grossinis_sister2";

		CCSprite sister1;
		CCSprite sister2;

        public LayerTestBlend()
        {
            CCLayerColor layer1 = new CCLayerColor(new CCColor4B(255, 255, 255, 80));

            sister1 = new CCSprite(s_pPathSister1);
            sister2 = new CCSprite(s_pPathSister2);

            AddChild(sister1);
            AddChild(sister2);
            AddChild(layer1, 100, kTagLayer);

            Schedule(newBlend, 1.0f);
        }

        protected override void AddedToNewScene()
        {
            base.AddedToNewScene();

            CCSize s = Layer.VisibleBoundsWorldspace.Size;
			sister1.Position = new CCPoint(160, s.Height / 2);
			sister2.Position = new CCPoint(320, s.Height / 2);
		}

        public void newBlend(float dt)
        {
            CCLayerColor layer = (CCLayerColor)GetChildByTag(kTagLayer);

            int src;
            int dst;

            if (layer.BlendFunc.Destination == CCOGLES.GL_ZERO)
            {
                src = CCOGLES.GL_SRC_ALPHA;
                dst = CCOGLES.GL_ONE_MINUS_SRC_ALPHA;
            }
            else
            {
                src = CCOGLES.GL_ONE_MINUS_DST_COLOR;
                dst = CCOGLES.GL_ZERO;
            }

            layer.BlendFunc = new CCBlendFunc(src, dst);
        }

		public override string Title
		{
			get
			{
				return "ColorLayer: blend";
			}
		}
    }
}
