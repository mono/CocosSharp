using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cocos2D;

namespace tests
{
    public class SpriteZVertex : SpriteTestDemo
    {
        int m_dir;
        float m_time;
        public override void OnEnter()
        {
            base.OnEnter();

            // TIP: don't forget to enable Alpha test
            //glEnable(GL_ALPHA_TEST);
            //glAlphaFunc(GL_GREATER, 0.0f);
            CCDirector.SharedDirector.Projection = (CCDirectorProjection.Projection3D);
        }
        public override void OnExit()
        {
            //glDisable(GL_ALPHA_TEST);
            CCDirector.SharedDirector.Projection = (CCDirectorProjection.Projection2D);
            base.OnExit();
        }
        public SpriteZVertex()
        {
            //
            // This test tests z-order
            // If you are going to use it is better to use a 3D projection
            //
            // WARNING:
            // The developer is resposible for ordering it's sprites according to it's Z if the sprite has
            // transparent parts.
            //

            m_dir = 1;
            m_time = 0;

            CCSize s = CCDirector.SharedDirector.WinSize;
            float step = s.Width / 12;

            CCNode node = new CCNode ();
            // camera uses the center of the image as the pivoting point
            node.ContentSize = (new CCSize(s.Width, s.Height));
            node.AnchorPoint = (new CCPoint(0.5f, 0.5f));
            node.Position = (new CCPoint(s.Width / 2, s.Height / 2));

            AddChild(node, 0);

            for (int i = 0; i < 5; i++)
            {
                CCSprite sprite = new CCSprite("Images/grossini_dance_atlas", new CCRect(85 * 0, 121 * 1, 85, 121));
                sprite.Position = (new CCPoint((i + 1) * step, s.Height / 2));
                sprite.VertexZ = (10 + i * 40);
                node.AddChild(sprite, 0);
            }

            for (int i = 5; i < 11; i++)
            {
                CCSprite sprite = new CCSprite("Images/grossini_dance_atlas", new CCRect(85 * 1, 121 * 0, 85, 121));
                sprite.Position = (new CCPoint((i + 1) * step, s.Height / 2));
                sprite.VertexZ = 10 + (10 - i) * 40;
                node.AddChild(sprite, 0);
            }

            node.RunAction(new CCOrbitCamera(10, 1, 0, 0, 360, 0, 0));
        }

        public override string title()
        {
            return "Sprite: openGL Z vertex";
        }
    }
}
