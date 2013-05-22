using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cocos2D;

namespace tests
{
    public class LayerTestBlend : LayerTest
    {
        int kTagLayer = 1;
        string s_pPathSister1 = "Images/grossinis_sister1";
        string s_pPathSister2 = "Images/grossinis_sister2";
        public LayerTestBlend()
        {
            CCSize s = CCDirector.SharedDirector.WinSize;
            CCLayerColor layer1 = new CCLayerColor(new CCColor4B(255, 255, 255, 80));

            CCSprite sister1 = new CCSprite(s_pPathSister1);
            CCSprite sister2 = new CCSprite(s_pPathSister2);

            AddChild(sister1);
            AddChild(sister2);
            AddChild(layer1, 100, kTagLayer);

            sister1.Position = new CCPoint(160, s.Height / 2);
            sister2.Position = new CCPoint(320, s.Height / 2);

            Schedule(newBlend, 1.0f);
        }

        public void newBlend(float dt)
        {
            CCLayerColor layer = (CCLayerColor)GetChildByTag(kTagLayer);

            int src;
            int dst;

            if (layer.BlendFunc.Destination == OGLES.GL_ZERO)
            {
                src = OGLES.GL_SRC_ALPHA;
                dst = OGLES.GL_ONE_MINUS_SRC_ALPHA;
            }
            else
            {
                src = OGLES.GL_ONE_MINUS_DST_COLOR;
                dst = OGLES.GL_ZERO;
            }

            layer.BlendFunc = new CCBlendFunc(src, dst);
        }

        public override string title()
        {
            return "ColorLayer: blend";
        }
    }
}
