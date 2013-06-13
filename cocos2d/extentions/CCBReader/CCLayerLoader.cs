namespace Cocos2D
{
    public class CCLayerLoader : CCNodeLoader
    {
        private const string PROPERTY_TOUCH_ENABLED = "isTouchEnabled";
        private const string PROPERTY_ACCELEROMETER_ENABLED = "isAccelerometerEnabled";
        private const string PROPERTY_MOUSE_ENABLED = "isMouseEnabled";
        private const string PROPERTY_KEYBOARD_ENABLED = "isKeyboardEnabled";

        public override CCNode CreateCCNode()
        {
            return new CCLayer();
        }

        protected override void OnHandlePropTypeCheck(CCNode node, CCNode parent, string propertyName, bool pCheck, CCBReader reader)
        {
            if (propertyName == PROPERTY_TOUCH_ENABLED)
            {
                ((CCLayer) node).TouchEnabled = pCheck;
            }
            else if (propertyName == PROPERTY_ACCELEROMETER_ENABLED)
            {
                ((CCLayer) node).AccelerometerEnabled = pCheck;
            }
            else if (propertyName == PROPERTY_MOUSE_ENABLED)
            {
                // TODO XXX
                CCLog.Log("The property '{0}' is not supported!", PROPERTY_MOUSE_ENABLED);
            }
            else if (propertyName == PROPERTY_KEYBOARD_ENABLED)
            {
                // TODO XXX
                CCLog.Log("The property '{0}' is not supported!", PROPERTY_KEYBOARD_ENABLED);
                // This comes closest: ((CCLayer *)node).setKeypadEnabled(pCheck);
            }
            else
            {
                base.OnHandlePropTypeCheck(node, parent, propertyName, pCheck, reader);
            }
        }
    }
}