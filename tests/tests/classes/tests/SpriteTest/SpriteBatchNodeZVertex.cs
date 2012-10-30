using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class SpriteBatchNodeZVertex : SpriteTestDemo
    {
        int m_dir;
        float m_time;
        public override void OnEnter()
        {
            base.OnEnter();

            // TIP: don't forget to enable Alpha test
            //glEnable(GL_ALPHA_TEST);
            //glAlphaFunc(GL_GREATER, 0.0f);

            CCDirector.SharedDirector.Projection = ccDirectorProjection.kCCDirectorProjection3D;
        }

        public override void OnExit()
        {
            //glDisable(GL_ALPHA_TEST);
            CCDirector.SharedDirector.Projection = (ccDirectorProjection.kCCDirectorProjection2D);
            base.OnExit();
        }

        public SpriteBatchNodeZVertex()
        {
            //
            // This test tests z-order
            // If you are going to use it is better to use a 3D projection
            //
            // WARNING:
            // The developer is resposible for ordering it's sprites according to it's Z if the sprite has
            // transparent parts.
            //


            CCSize s = CCDirector.SharedDirector.WinSize;
            float step = s.width / 12;

            // small capacity. Testing resizing.
            // Don't use capacity=1 in your real game. It is expensive to resize the capacity
            CCSpriteBatchNode batch = CCSpriteBatchNode.Create("Images/grossini_dance_atlas", 1);
            // camera uses the center of the image as the pivoting point
            batch.ContentSize = new CCSize(s.width, s.height);
            batch.AnchorPoint = (new CCPoint(0.5f, 0.5f));
            batch.Position = (new CCPoint(s.width / 2, s.height / 2));


            AddChild(batch, 0, (int)kTags.kTagSpriteBatchNode);

            for (int i = 0; i < 5; i++)
            {
                CCSprite sprite = CCSprite.Create(batch.Texture, new CCRect(85 * 0, 121 * 1, 85, 121));
                sprite.Position = (new CCPoint((i + 1) * step, s.height / 2));
                sprite.VertexZ = (10 + i * 40);
                batch.AddChild(sprite, 0);

            }

            for (int i = 5; i < 11; i++)
            {
                CCSprite sprite = CCSprite.Create(batch.Texture, new CCRect(85 * 1, 121 * 0, 85, 121));
                sprite.Position = (new CCPoint((i + 1) * step, s.height / 2));
                sprite.VertexZ = 10 + (10 - i) * 40;
                batch.AddChild(sprite, 0);
            }

            batch.RunAction(CCOrbitCamera.Create(10, 1, 0, 0, 360, 0, 0));
        }

        public override string title()
        {
            return "SpriteBatchNode: openGL Z vertex";
        }
    }
}
