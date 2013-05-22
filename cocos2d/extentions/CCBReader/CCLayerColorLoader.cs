namespace Cocos2D
{
    internal class CCLayerColorLoader : CCLayerLoader
    {
        private const string PROPERTY_COLOR = "color";
        private const string PROPERTY_OPACITY = "opacity";
        private const string PROPERTY_BLENDFUNC = "blendFunc";

        public override CCNode CreateCCNode()
        {
            return new CCLayerColor();
        }

        protected override void OnHandlePropTypeColor3(CCNode node, CCNode parent, string propertyName, CCColor3B color, CCBReader reader)
        {
            if (propertyName == PROPERTY_COLOR)
            {
                ((CCLayerColor) node).Color = color;
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
                ((CCLayerColor) node).Opacity = pByte;
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
                ((CCLayerColor) node).BlendFunc = blendFunc;
            }
            else
            {
                base.OnHandlePropTypeBlendFunc(node, parent, propertyName, blendFunc, reader);
            }
        }
    }
}