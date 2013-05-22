using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cocos2D
{
    public class CCScrollViewLoader : CCNodeLoader
    {
        protected const string PROPERTY_CONTAINER = "container";
        protected const string PROPERTY_DIRECTION = "direction";
        protected const string PROPERTY_CLIPSTOBOUNDS = "clipsToBounds";
        protected const string PROPERTY_BOUNCES = "bounces";

        public override CCNode CreateCCNode()
        {
            return new CCScrollView();
        }

        protected override void OnHandlePropTypeSize(CCNode node, CCNode parent, string propertyName, CCSize pSize, CCBReader reader)
        {
            if (propertyName == PROPERTY_CONTENTSIZE)
            {
                ((CCScrollView) node).ViewSize = pSize;
            }
            else
            {
                base.OnHandlePropTypeSize(node, parent, propertyName, pSize, reader);
            }
        }

        protected override void OnHandlePropTypeCheck(CCNode node, CCNode parent, string propertyName, bool pCheck, CCBReader reader)
        {
            if (propertyName == PROPERTY_CLIPSTOBOUNDS)
            {
                ((CCScrollView) node).ClippingToBounds = pCheck;
            }
            else if (propertyName == PROPERTY_BOUNCES)
            {
                ((CCScrollView) node).Bounceable = pCheck;
            }
            else
            {
                base.OnHandlePropTypeCheck(node, parent, propertyName, pCheck, reader);
            }
        }

        protected override void OnHandlePropTypeCCBFile(CCNode node, CCNode parent, string propertyName, CCNode fileNode, CCBReader reader)
        {
            if (propertyName == PROPERTY_CONTAINER)
            {
                ((CCScrollView) node).Container = fileNode;
                ((CCScrollView) node).UpdateInset();
            }
            else
            {
                base.OnHandlePropTypeCCBFile(node, parent, propertyName, fileNode, reader);
            }
        }

        protected override void OnHandlePropTypeFloat(CCNode node, CCNode parent, string propertyName, float pFloat, CCBReader reader)
        {
            if (propertyName == PROPERTY_SCALE)
            {
                node.Scale = pFloat;
            }
            else
            {
                base.OnHandlePropTypeFloat(node, parent, propertyName, pFloat, reader);
            }
        }

        protected override void OnHandlePropTypeIntegerLabeled(CCNode node, CCNode parent, string propertyName, int pIntegerLabeled,
                                                               CCBReader reader)
        {
            if (propertyName == PROPERTY_DIRECTION)
            {
                ((CCScrollView) node).Direction = (CCScrollViewDirection) pIntegerLabeled;
            }
            else
            {
                base.OnHandlePropTypeFloatScale(node, parent, propertyName, pIntegerLabeled, reader);
            }
        }

    }
}