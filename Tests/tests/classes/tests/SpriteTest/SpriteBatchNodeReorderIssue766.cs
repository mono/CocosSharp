using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class SpriteBatchNodeReorderIssue766 : SpriteTestDemo
    {
        CCSpriteBatchNode batchNode;
        CCSprite sprite1;
        CCSprite sprite2;
        CCSprite sprite3;


        #region Properties

        public override string Title
        {
            get { return "Testing SpriteBatchNode"; }
        }

        public override string Subtitle
        {
            get { return "reorder issue #766. In 2 seconds 1 sprite will be reordered"; }
        }

        #endregion Properties


        #region Constructors

        public SpriteBatchNodeReorderIssue766()
        {
            batchNode = new CCSpriteBatchNode("Images/piece", 15);
            AddChild(batchNode, 1, 0);

            sprite1 = MakeSpriteZ(2);
            sprite2 = MakeSpriteZ(3);
            sprite3 = MakeSpriteZ(4);
        }

        #endregion Constructors


        #region Setup content

        public override void OnEnter()
        {
            base.OnEnter(); 
            CCSize windowSize = VisibleBoundsWorldspace.Size;

            sprite1.Position = (new CCPoint(200, 160));
            sprite2.Position = (new CCPoint(264, 160));
            sprite3.Position = (new CCPoint(328, 160));

            Schedule(ReorderSprite, 2);
        }

        #endregion Setup content


        void ReorderSprite(float dt)
        {
            Unschedule(ReorderSprite);
            batchNode.ReorderChild(sprite1, 4);
        }

        CCSprite MakeSpriteZ(int aZ)
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
    }
}
