namespace Cocos2D
{
    internal class CCLabelBMFontLoader : CCNodeLoader
    {
        private const string PROPERTY_COLOR = "color";
        private const string PROPERTY_OPACITY = "opacity";
        private const string PROPERTY_BLENDFUNC = "blendFunc";
        private const string PROPERTY_FNTFILE = "fntFile";
        private const string PROPERTY_STRING = "string";

        public override CCNode CreateCCNode()
        {
            return new CCLabelBMFont();
        }

        protected override void OnHandlePropTypeColor3(CCNode node, CCNode parent, string propertyName, CCColor3B color, CCBReader reader)
        {
            if (propertyName == PROPERTY_COLOR)
            {
                ((CCLabelBMFont) node).Color = color;
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
                ((CCLabelBMFont) node).Opacity = pByte;
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
                ((CCLabelBMFont) node).BlendFunc = blendFunc;
            }
            else
            {
                base.OnHandlePropTypeBlendFunc(node, parent, propertyName, blendFunc, reader);
            }
        }

        protected override void OnHandlePropTypeFntFile(CCNode node, CCNode parent, string propertyName, string pFntFile, CCBReader reader)
        {
            if (propertyName == PROPERTY_FNTFILE)
            {
                ((CCLabelBMFont) node).FntFile = pFntFile;
            }
            else
            {
                base.OnHandlePropTypeFntFile(node, parent, propertyName, pFntFile, reader);
            }
        }

        protected override void OnHandlePropTypeText(CCNode node, CCNode parent, string propertyName, string pText, CCBReader reader)
        {
            if (propertyName == PROPERTY_STRING)
            {
                ((CCLabelBMFont) node).Text = (pText);
            }
            else
            {
                base.OnHandlePropTypeText(node, parent, propertyName, pText, reader);
            }
        }
    }
}