namespace Cocos2D
{
    internal class CCControlLoader : CCNodeLoader
    {
        private const string PROPERTY_ENABLED = "enabled";
        private const string PROPERTY_SELECTED = "selected";
        private const string PROPERTY_CCCONTROL = "ccControl";

        protected override void OnHandlePropTypeCheck(CCNode node, CCNode parent, string propertyName, bool pCheck, CCBReader reader)
        {
            if (propertyName == PROPERTY_ENABLED)
            {
                ((CCControl) node).Enabled = pCheck;
            }
            else if (propertyName == PROPERTY_SELECTED)
            {
                ((CCControl) node).Selected = pCheck;
            }
            else
            {
                base.OnHandlePropTypeCheck(node, parent, propertyName, pCheck, reader);
            }
        }

        protected override void OnHandlePropTypeBlockCcControl(CCNode node, CCNode parent, string propertyName,
                                                               BlockCCControlData blockControlData, CCBReader reader)
        {
            if (propertyName == PROPERTY_CCCONTROL)
            {
                ((CCControl) node).AddTargetWithActionForControlEvents(blockControlData.mTarget, blockControlData.mSELCCControlHandler,
                                                                       blockControlData.mControlEvents);
            }
            else
            {
                base.OnHandlePropTypeBlockCcControl(node, parent, propertyName, blockControlData, reader);
            }
        }
    }
}