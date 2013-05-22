namespace Cocos2D
{
    internal class CCControlButtonLoader : CCControlLoader
    {
        private const string PROPERTY_ZOOMONTOUCHDOWN = "zoomOnTouchDown";
        private const string PROPERTY_TITLE_NORMAL = "title|1";
        private const string PROPERTY_TITLE_HIGHLIGHTED = "title|2";
        private const string PROPERTY_TITLE_DISABLED = "title|3";
        private const string PROPERTY_TITLECOLOR_NORMAL = "titleColor|1";
        private const string PROPERTY_TITLECOLOR_HIGHLIGHTED = "titleColor|2";
        private const string PROPERTY_TITLECOLOR_DISABLED = "titleColor|3";
        private const string PROPERTY_TITLETTF_NORMAL = "titleTTF|1";
        private const string PROPERTY_TITLETTF_HIGHLIGHTED = "titleTTF|2";
        private const string PROPERTY_TITLETTF_DISABLED = "titleTTF|3";
        private const string PROPERTY_TITLETTFSIZE_NORMAL = "titleTTFSize|1";
        private const string PROPERTY_TITLETTFSIZE_HIGHLIGHTED = "titleTTFSize|2";
        private const string PROPERTY_TITLETTFSIZE_DISABLED = "titleTTFSize|3";
        private const string PROPERTY_LABELANCHORPOINT = "labelAnchorPoint";
        // TODO Should be="preferredSize". This is a typo in cocos2d-iphone, cocos2d-x and CocosBuilder!
        private const string PROPERTY_PREFEREDSIZE = "preferedSize";
        private const string PROPERTY_BACKGROUNDSPRITEFRAME_NORMAL = "backgroundSpriteFrame|1";
        private const string PROPERTY_BACKGROUNDSPRITEFRAME_HIGHLIGHTED = "backgroundSpriteFrame|2";
        private const string PROPERTY_BACKGROUNDSPRITEFRAME_DISABLED = "backgroundSpriteFrame|3";

        public override CCNode CreateCCNode()
        {
            return new CCControlButton();
        }

        protected override void OnHandlePropTypeCheck(CCNode node, CCNode parent, string propertyName, bool pCheck, CCBReader reader)
        {
            if (propertyName == PROPERTY_ZOOMONTOUCHDOWN)
            {
                ((CCControlButton) node).ZoomOnTouchDown = pCheck;
            }
            else
            {
                base.OnHandlePropTypeCheck(node, parent, propertyName, pCheck, reader);
            }
        }

        protected override void OnHandlePropTypeString(CCNode node, CCNode parent, string propertyName, string pString, CCBReader reader)
        {
            if (propertyName == PROPERTY_TITLE_NORMAL)
            {
                ((CCControlButton) node).SetTitleForState(pString, CCControlState.Normal);
            }
            else if (propertyName == PROPERTY_TITLE_HIGHLIGHTED)
            {
                ((CCControlButton) node).SetTitleForState(pString, CCControlState.Highlighted);
            }
            else if (propertyName == PROPERTY_TITLE_DISABLED)
            {
                ((CCControlButton) node).SetTitleForState(pString, CCControlState.Disabled);
            }
            else
            {
                base.OnHandlePropTypeString(node, parent, propertyName, pString, reader);
            }
        }

        protected override void OnHandlePropTypeFontTTF(CCNode node, CCNode parent, string propertyName, string fontTTF, CCBReader reader)
        {
            if (propertyName == PROPERTY_TITLETTF_NORMAL)
            {
                ((CCControlButton) node).SetTitleTtfForState(fontTTF, CCControlState.Normal);
            }
            else if (propertyName == PROPERTY_TITLETTF_HIGHLIGHTED)
            {
                ((CCControlButton) node).SetTitleTtfForState(fontTTF, CCControlState.Highlighted);
            }
            else if (propertyName == PROPERTY_TITLETTF_DISABLED)
            {
                ((CCControlButton) node).SetTitleTtfForState(fontTTF, CCControlState.Disabled);
            }
            else
            {
                base.OnHandlePropTypeFontTTF(node, parent, propertyName, fontTTF, reader);
            }
        }

        protected override void OnHandlePropTypeFloatScale(CCNode node, CCNode parent, string propertyName, float floatScale, CCBReader reader)
        {
            if (propertyName == PROPERTY_TITLETTFSIZE_NORMAL)
            {
                ((CCControlButton) node).SetTitleTtfSizeForState(floatScale, CCControlState.Normal);
            }
            else if (propertyName == PROPERTY_TITLETTFSIZE_HIGHLIGHTED)
            {
                ((CCControlButton) node).SetTitleTtfSizeForState(floatScale, CCControlState.Highlighted);
            }
            else if (propertyName == PROPERTY_TITLETTFSIZE_DISABLED)
            {
                ((CCControlButton) node).SetTitleTtfSizeForState(floatScale, CCControlState.Disabled);
            }
            else
            {
                base.OnHandlePropTypeFloatScale(node, parent, propertyName, floatScale, reader);
            }
        }

        protected override void OnHandlePropTypePoint(CCNode node, CCNode parent, string propertyName, CCPoint point, CCBReader reader)
        {
            if (propertyName == PROPERTY_LABELANCHORPOINT)
            {
                ((CCControlButton) node).LabelAnchorPoint = point;
            }
            else
            {
                base.OnHandlePropTypePoint(node, parent, propertyName, point, reader);
            }
        }

        protected override void OnHandlePropTypeSize(CCNode node, CCNode parent, string propertyName, CCSize pSize, CCBReader reader)
        {
            if (propertyName == PROPERTY_PREFEREDSIZE)
            {
                ((CCControlButton) node).PreferredSize = pSize;
            }
            else
            {
                base.OnHandlePropTypeSize(node, parent, propertyName, pSize, reader);
            }
        }

        protected override void OnHandlePropTypeSpriteFrame(CCNode node, CCNode parent, string propertyName, CCSpriteFrame spriteFrame,
                                                            CCBReader reader)
        {
            if (propertyName == PROPERTY_BACKGROUNDSPRITEFRAME_NORMAL)
            {
                if (spriteFrame != null)
                {
                    ((CCControlButton) node).SetBackgroundSpriteFrameForState(spriteFrame, CCControlState.Normal);
                }
            }
            else if (propertyName == PROPERTY_BACKGROUNDSPRITEFRAME_HIGHLIGHTED)
            {
                if (spriteFrame != null)
                {
                    ((CCControlButton) node).SetBackgroundSpriteFrameForState(spriteFrame, CCControlState.Highlighted);
                }
            }
            else if (propertyName == PROPERTY_BACKGROUNDSPRITEFRAME_DISABLED)
            {
                if (spriteFrame != null)
                {
                    ((CCControlButton) node).SetBackgroundSpriteFrameForState(spriteFrame, CCControlState.Disabled);
                }
            }
            else
            {
                base.OnHandlePropTypeSpriteFrame(node, parent, propertyName, spriteFrame, reader);
            }
        }

        protected override void OnHandlePropTypeColor3(CCNode node, CCNode parent, string propertyName, CCColor3B color, CCBReader reader)
        {
            if (propertyName == PROPERTY_TITLECOLOR_NORMAL)
            {
                ((CCControlButton) node).SetTitleColorForState(color, CCControlState.Normal);
            }
            else if (propertyName == PROPERTY_TITLECOLOR_HIGHLIGHTED)
            {
                ((CCControlButton) node).SetTitleColorForState(color, CCControlState.Highlighted);
            }
            else if (propertyName == PROPERTY_TITLECOLOR_DISABLED)
            {
                ((CCControlButton) node).SetTitleColorForState(color, CCControlState.Disabled);
            }
            else
            {
                base.OnHandlePropTypeColor3(node, parent, propertyName, color, reader);
            }
        }
    }
}