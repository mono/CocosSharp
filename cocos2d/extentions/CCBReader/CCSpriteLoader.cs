namespace Cocos2D
{
    internal class CCSpriteLoader : CCNodeLoader
    {
        private const string PROPERTY_FLIP = "flip";
        private const string PROPERTY_DISPLAYFRAME = "displayFrame";
        private const string PROPERTY_COLOR = "color";
        private const string PROPERTY_OPACITY = "opacity";
        private const string PROPERTY_BLENDFUNC = "blendFunc";

        public override CCNode CreateCCNode()
        {
            return new CCSprite();
        }

        protected override void OnHandlePropTypeSpriteFrame(CCNode node, CCNode parent, string propertyName, CCSpriteFrame spriteFrame,
                                                            CCBReader reader)
        {
            if (propertyName == PROPERTY_DISPLAYFRAME)
            {
                if (spriteFrame != null)
                {
                    ((CCSprite) node).DisplayFrame = spriteFrame;
                }
                else
                {
                    CCLog.Log("ERROR: SpriteFrame NULL");
                }
            }
            else
            {
                base.OnHandlePropTypeSpriteFrame(node, parent, propertyName, spriteFrame, reader);
            }
        }

        protected override void OnHandlePropTypeFlip(CCNode node, CCNode parent, string propertyName, bool[] pFlip, CCBReader reader)
        {
            if (propertyName == PROPERTY_FLIP)
            {
                ((CCSprite) node).FlipX = pFlip[0];
                ((CCSprite) node).FlipY = pFlip[1];
            }
            else
            {
                base.OnHandlePropTypeFlip(node, parent, propertyName, pFlip, reader);
            }
        }

        protected override void OnHandlePropTypeColor3(CCNode node, CCNode parent, string propertyName, CCColor3B color, CCBReader reader)
        {
            if (propertyName == PROPERTY_COLOR)
            {
                ((CCSprite) node).Color = color;
            }
            else
            {
                base.OnHandlePropTypeColor3(node, parent, propertyName, color, reader);
            }
        }

        protected override void OnHandlePropTypeByte(CCNode node, CCNode parent, string propertyName, byte pByte, CCBReader reader)
        {
            if (propertyName == PROPERTY_OPACITY)
            {
                ((CCSprite) node).Opacity = pByte;
            }
            else
            {
                base.OnHandlePropTypeByte(node, parent, propertyName, pByte, reader);
            }
        }

        protected override void OnHandlePropTypeBlendFunc(CCNode node, CCNode parent, string propertyName, CCBlendFunc blendFunc,
                                                          CCBReader reader)
        {
            if (propertyName == PROPERTY_BLENDFUNC)
            {
                ((CCSprite) node).BlendFunc = blendFunc;
            }
            else
            {
                base.OnHandlePropTypeBlendFunc(node, parent, propertyName, blendFunc, reader);
            }
        }
    }
}