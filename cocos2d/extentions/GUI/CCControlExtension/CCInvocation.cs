/*
 *
 * Helper class to store targets and selectors (and eventually, params?) in the same CCMutableArray. Basically a very crude form of a NSInvocation
 */

namespace cocos2d
{
    public delegate void SEL_CCControlHandler(CCObject obj, CCControlEvent ce);

    //#define cccontrol_selector(_SELECTOR) (SEL_CCControlHandler)(&_SELECTOR)

    public class CCInvocation : CCObject
    {
        private readonly SEL_CCControlHandler m_pAction;
        private readonly CCControlEvent m_pControlEvent;

        private readonly CCObject m_target;

        public CCInvocation(CCObject target, SEL_CCControlHandler action, CCControlEvent controlEvent)
        {
            m_target = target;
            m_pAction = action;
            m_pControlEvent = controlEvent;
        }

        public SEL_CCControlHandler Action
        {
            get { return m_pAction; }
        }

        public CCObject Target
        {
            get { return m_target; }
        }

        public CCControlEvent ControlEvent
        {
            get { return m_pControlEvent; }
        }

        public void Invoke(CCObject sender)
        {
            if (m_target != null && m_pAction != null)
            {
                Action(sender, m_pControlEvent);
            }
        }
    }
}
