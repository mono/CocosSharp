using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cocos2D
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
                    ((CCMenuItemImage) node).SetNormalSpriteFrame(spriteFrame);
                }
            }
            else if (propertyName == PROPERTY_SELECTEDDISPLAYFRAME)
            {
                if (spriteFrame != null)
                {
                    ((CCMenuItemImage) node).SetSelectedSpriteFrame(spriteFrame);
                }
            }
            else if (propertyName == PROPERTY_DISABLEDDISPLAYFRAME)
            {
                if (spriteFrame != null)
                {
                    ((CCMenuItemImage) node).SetDisabledSpriteFrame(spriteFrame);
                }
            }
            else
            {
                base.OnHandlePropTypeSpriteFrame(node, parent, propertyName, spriteFrame, reader);
            }
        }
    }
}
    

