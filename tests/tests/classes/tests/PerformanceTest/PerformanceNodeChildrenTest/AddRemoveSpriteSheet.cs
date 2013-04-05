using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;
using Random = cocos2d.Random;

namespace tests
{
    public class AddRemoveSpriteSheet : NodeChildrenMainScene
    {
        public override void updateQuantityOfNodes()
        {
            CCSize s = CCDirector.SharedDirector.WinSize;

            // increase nodes
            if (currentQuantityOfNodes < quantityOfNodes)
            {
                for (int i = 0; i < (quantityOfNodes - currentQuantityOfNodes); i++)
                {
                    CCSprite sprite = new CCSprite(batchNode.Texture, new CCRect(0, 0, 32, 32));
                    batchNode.AddChild(sprite);
                    sprite.Position = new CCPoint(Random.Next() * s.Width, Random.Next() * s.Height);
                    sprite.Visible = false;
                }
            }
            // decrease nodes
            else if (currentQuantityOfNodes > quantityOfNodes)
            {
                for (int i = 0; i < (currentQuantityOfNodes - quantityOfNodes); i++)
                {
                    int index = currentQuantityOfNodes - i - 1;
                    batchNode.RemoveChildAtIndex(index, true);
                }
            }

            currentQuantityOfNodes = quantityOfNodes;
        }

        public override void initWithQuantityOfNodes(int nNodes)
        {
            batchNode = CCSpriteBatchNode.Create("Images/spritesheet1");
            AddChild(batchNode);

            base.initWithQuantityOfNodes(nNodes);

            ScheduleUpdate();
        }

        public override void Update(float dt)
        {
            throw new NotFiniteNumberException();
        }

        public virtual string profilerName()
        {
            return "none";
        }

        protected CCSpriteBatchNode batchNode;
        //#if CC_ENABLE_PROFILERS
        //    CCProfilingTimer* _profilingTimer;
        //#endif
    }
}
