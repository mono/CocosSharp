using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cocos2D;

namespace tests
{
    public class SpriteBatchNodeReorderIssue766 : SpriteTestDemo
    {
        public SpriteBatchNodeReorderIssue766()
        {
            batchNode = new CCSpriteBatchNode("Images/piece", 15);
            AddChild(batchNode, 1, 0);

            sprite1 = makeSpriteZ(2);
            sprite1.Position = (new CCPoint(200, 160));

            sprite2 = makeSpriteZ(3);
            sprite2.Position = (new CCPoint(264, 160));

            sprite3 = makeSpriteZ(4);
            sprite3.Position = (new CCPoint(328, 160));

            Schedule(reorderSprite, 2);
        }

        public override string title()
        {
            return "SpriteBatchNode: reorder issue #766";
        }

        public override string subtitle()
        {
            return "In 2 seconds 1 sprite will be reordered";
        }
        public void reorderSprite(float dt)
        {
            Unschedule(reorderSprite);
            batchNode.ReorderChild(sprite1, 4);
        }

        public CCSprite makeSpriteZ(int aZ)
        {
            CCSprite sprite = new CCSprite(batchNode.Texture, new CCRect(128, 0, 64, 64));
            batchNode.AddChild(sprite, aZ + 1, 0);

            //children
            CCSprite spriteShadow = new CCSprite(batchNode.Texture, new CCRect(0, 0, 64, 64));
            spriteShadow.Opacity = 128;
            sprite.AddChild(spriteShadow, aZ, 3);

            CCSprite spriteTop = new CCSprite(batchNode.Texture, new CCRect(64, 0, 64, 64));
            sprite.AddChild(spriteTop, aZ + 2, 3);

            return sprite;
        }

        private CCSpriteBatchNode batchNode;
        private CCSprite sprite1;
        private CCSprite sprite2;
        private CCSprite sprite3;
    }
}
