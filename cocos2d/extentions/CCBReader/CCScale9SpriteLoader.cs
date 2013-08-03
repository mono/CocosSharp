namespace Cocos2D
{
    internal class CCScale9SpriteLoader : CCNodeLoader
    {
        protected const string PROPERTY_SPRITEFRAME = "spriteFrame";
        protected const string PROPERTY_COLOR = "color";
        protected const string PROPERTY_OPACITY = "opacity";
        protected const string PROPERTY_BLENDFUNC = "blendFunc";
        // TODO Should be "preferredSize". This is a typo in cocos2d-iphone, cocos2d-x and CocosBuilder!
        protected const string PROPERTY_PREFEREDSIZE = "preferedSize";
        protected const string PROPERTY_INSETLEFT = "insetLeft";
        protected const string PROPERTY_INSETTOP = "insetTop";
        protected const string PROPERTY_INSETRIGHT = "insetRight";
        protected const string PROPERTY_INSETBOTTOM = "insetBottom";

        public override CCNode CreateCCNode()
        {
            return new CCScale9Sprite();
        }

        protected override void OnHandlePropTypeSpriteFrame(CCNode node, CCNode parent, string propertyName, CCSpriteFrame spriteFrame,
                                                            CCBReader reader)
        {
            if (propertyName == PROPERTY_SPRITEFRAME)
            {
                ((CCScale9Sprite) node).InitWithSpriteFrame(spriteFrame);
            }
            else
            {
                base.OnHandlePropTypeSpriteFrame(node, parent, propertyName, spriteFrame, reader);
            }
        }

        protected override void OnHandlePropTypeColor3(CCNode node, CCNode parent, string propertyName, CCColor3B color, CCBReader reader)
        {
            if (propertyName == PROPERTY_COLOR)
            {
                ((CCScale9Sprite) node).Color = color;
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
                ((CCScale9Sprite) node).Opacity = pByte;
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
                // TODO Not exported by CocosBuilder yet!
                // ((CCScale9Sprite )node).setBlendFunc(blendFunc);
            }
            else
            {
                base.OnHandlePropTypeBlendFunc(node, parent, propertyName, blendFunc, reader);
            }
        }

        protected override void OnHandlePropTypeSize(CCNode node, CCNode parent, string propertyName, CCSize pSize, CCBReader reader)
        {
            if (propertyName == PROPERTY_CONTENTSIZE)
            {
                //((CCScale9Sprite )node).setContentSize(pSize);
            }
            else if (propertyName == PROPERTY_PREFEREDSIZE)
            {
                ((CCScale9Sprite) node).PreferredSize = pSize;
            }
            else
            {
                base.OnHandlePropTypeSize(node, parent, propertyName, pSize, reader);
            }
        }

        protected override void OnHandlePropTypeFloat(CCNode node, CCNode parent, string propertyName, float pFloat, CCBReader reader)
        {
            if (propertyName == PROPERTY_INSETLEFT)
            {
                ((CCScale9Sprite) node).InsetLeft = pFloat;
            }
            else if (propertyName == PROPERTY_INSETTOP)
            {
                ((CCScale9Sprite) node).InsetTop = pFloat;
            }
            else if (propertyName == PROPERTY_INSETRIGHT)
            {
                ((CCScale9Sprite) node).InsetRight = pFloat;
            }
            else if (propertyName == PROPERTY_INSETBOTTOM)
            {
                ((CCScale9Sprite) node).InsetBottom = pFloat;
            }
            else
            {
                base.OnHandlePropTypeFloat(node, parent, propertyName, pFloat, reader);
            }
        }
    }
}