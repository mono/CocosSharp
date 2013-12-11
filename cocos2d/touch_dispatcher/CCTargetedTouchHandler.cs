
using System.Collections.Generic;

namespace CocosSharp
{
    public class CCTargetedTouchHandler : CCTouchHandler
    {
        protected bool m_bSwallowsTouches;
        protected List<CCTouch> m_pClaimedTouches;

        /// <summary>
        /// whether or not the touches are swallowed
        /// </summary>
        public bool IsSwallowsTouches
        {
            get { return m_bSwallowsTouches; }
            set { m_bSwallowsTouches = value; }
        }

        /// <summary>
        /// MutableSet that contains the claimed touches 
        /// </summary>
        public List<CCTouch> ClaimedTouches
        {
            get { return m_pClaimedTouches; }
        }

        /// <summary>
        ///  initializes a TargetedTouchHandler with a delegate, a priority and whether or not it swallows touches or not
        /// </summary>
        public bool InitWithDelegate(ICCTargetedTouchDelegate pDelegate, int nPriority, bool bSwallow)
        {
            if (base.InitWithDelegate(pDelegate, nPriority))
            {
                m_pClaimedTouches = new List<CCTouch>();
                m_bSwallowsTouches = bSwallow;

                return true;
            }

            return false;
        }

        /// <summary>
        /// allocates a TargetedTouchHandler with a delegate, a priority and whether or not it swallows touches or not 
        /// </summary>
        public static CCTargetedTouchHandler HandlerWithDelegate(ICCTargetedTouchDelegate pDelegate, int nPriority, bool bSwallow)
        {
            var pHandler = new CCTargetedTouchHandler();
            pHandler.InitWithDelegate(pDelegate, nPriority, bSwallow);
            return pHandler;
        }
    }
}