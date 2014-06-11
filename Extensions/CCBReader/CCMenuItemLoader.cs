using System;
using System.Collections.Generic;

namespace CocosSharp
{
    public class CCMenuItemLoader : CCNodeLoader
    {
        private const string PROPERTY_BLOCK = "block";
        private const string PROPERTY_ISENABLED = "isEnabled";

        public override CCNode CreateCCNode()
        {
            return new CCMenuItem();
        }

        protected override void OnHandlePropTypeBlock(CCNode node, CCNode parent, string propertyName, BlockData pBlockData, CCBReader reader)
        {
            if (propertyName == PROPERTY_BLOCK)
            {
                if (null != pBlockData) // Add this condition to allow CCMenuItemImage without target/selector predefined 
                {
					((CCMenuItem) node).Target = pBlockData.mSELMenuHandler;
                }
            }
            else
            {
                base.OnHandlePropTypeBlock(node, parent, propertyName, pBlockData, reader);
            }
        }

        protected override void OnHandlePropTypeCheck(CCNode node, CCNode parent, string propertyName, bool pCheck, CCBReader reader)
        {
            if (propertyName == PROPERTY_ISENABLED)
            {
                ((CCMenuItem) node).Enabled = pCheck;
            }
            else
            {
                base.OnHandlePropTypeCheck(node, parent, propertyName, pCheck, reader);
            }
        }
    }
}