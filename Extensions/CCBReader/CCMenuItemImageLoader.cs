using System;
using System.Collections.Generic;

namespace CocosSharp
{
    public class CCMenuItemImageLoader : CCMenuItemLoader
    {
        private const string PROPERTY_NORMALDISPLAYFRAME = "normalSpriteFrame";
        private const string PROPERTY_SELECTEDDISPLAYFRAME = "selectedSpriteFrame";
        private const string PROPERTY_DISABLEDDISPLAYFRAME = "disabledSpriteFrame";

        public override CCNode CreateCCNode()
        {
            return new CCMenuItemImage();
        }

        protected override void OnHandlePropTypeSpriteFrame(CCNode node, CCNode parent, string propertyName, CCSpriteFrame spriteFrame,
                                                            CCBReader reader)
        {
            if (propertyName == PROPERTY_NORMALDISPLAYFRAME)
            {
                if (spriteFrame != null)
                {
                    ((CCMenuItemImage)node).NormalImageSpriteFrame = spriteFrame;
                }
            }
            else if (propertyName == PROPERTY_SELECTEDDISPLAYFRAME)
            {
                if (spriteFrame != null)
                {
                    ((CCMenuItemImage)node).SelectedImageSpriteFrame = spriteFrame;
                }
            }
            else if (propertyName == PROPERTY_DISABLEDDISPLAYFRAME)
            {
                if (spriteFrame != null)
                {
                    ((CCMenuItemImage) node).DisabledImageSpriteFrame = spriteFrame;
                }
            }
            else
            {
                base.OnHandlePropTypeSpriteFrame(node, parent, propertyName, spriteFrame, reader);
            }
        }
    }
}
    

