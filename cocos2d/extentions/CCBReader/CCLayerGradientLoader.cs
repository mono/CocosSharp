namespace Cocos2D
{
    internal class CCLayerGradientLoader : CCLayerLoader
    {
        private const string PROPERTY_STARTCOLOR = "startColor";
        private const string PROPERTY_ENDCOLOR = "endColor";
        private const string PROPERTY_STARTOPACITY = "startOpacity";
        private const string PROPERTY_ENDOPACITY = "endOpacity";
        private const string PROPERTY_VECTOR = "vector";
        private const string PROPERTY_BLENDFUNC = "blendFunc";

        public override CCNode CreateCCNode()
        {
            return new CCLayerGradient((byte)255,(byte)255);
        }

        protected override void OnHandlePropTypeColor3(CCNode node, CCNode parent, string propertyName, CCColor3B color, CCBReader reader)
        {
            if (propertyName == PROPERTY_STARTCOLOR)
            {
                ((CCLayerGradient) node).StartColor = color;
            }
            else if (propertyName == PROPERTY_ENDCOLOR)
            {
                ((CCLayerGradient) node).EndColor = color;
            }
            else
            {
                base.OnHandlePropTypeColor3(node, parent, propertyName, color, reader);
            }
        }

        protected override void OnHandlePropTypeByte(CCNode node, CCNode parent, string propertyName, byte pByte, CCBReader reader)
        {
            if (propertyName == PROPERTY_STARTOPACITY)
            {
                ((CCLayerGradient) node).StartOpacity = pByte;
            }
            else if (propertyName == PROPERTY_ENDOPACITY)
            {
                ((CCLayerGradient) node).EndOpacity = pByte;
            }
            else
            {
                base.OnHandlePropTypeByte(node, parent, propertyName, pByte, reader);
            }
        }

        protected override void OnHandlePropTypePoint(CCNode node, CCNode parent, string propertyName, CCPoint point, CCBReader reader)
        {
            if (propertyName == PROPERTY_VECTOR)
            {
                ((CCLayerGradient) node).Vector = point;

                // TODO Not passed along the ccbi file.
                // ((CCLayerGradient *)node).setCompressedInterpolation(true);
            }
            else
            {
                base.OnHandlePropTypePoint(node, parent, propertyName, point, reader);
            }
        }

        protected override void OnHandlePropTypeBlendFunc(CCNode node, CCNode parent, string propertyName, CCBlendFunc blendFunc,
                                                          CCBReader reader)
        {
            if (propertyName == PROPERTY_BLENDFUNC)
            {
                ((CCLayerGradient) node).BlendFunc = blendFunc;
            }
            else
            {
                base.OnHandlePropTypeBlendFunc(node, parent, propertyName, blendFunc, reader);
            }
        }
    }
}