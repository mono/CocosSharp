using System.Collections.Generic;
using System.Diagnostics;

namespace Cocos2D
{
    public class CCAnimate : CCActionInterval
    {
        protected CCAnimation m_pAnimation;
        protected List<float> m_pSplitTimes = new List<float>();
        protected int m_nNextFrame;
        protected CCSpriteFrame m_pOrigFrame;
        private uint m_uExecutedLoops;

        public CCAnimate(CCAnimation pAnimation)
        {
            InitWithAnimation(pAnimation);
        }

        protected CCAnimate(CCAnimate animate) : base(animate)
        {
            InitWithAnimation((CCAnimation) animate.m_pAnimation.Copy());
        }

        protected bool InitWithAnimation(CCAnimation pAnimation)
        {
            Debug.Assert(pAnimation != null);

            float singleDuration = pAnimation.Duration;

            if (base.InitWithDuration(singleDuration * pAnimation.Loops))
            {
                m_nNextFrame = 0;
                m_pAnimation = pAnimation;
                m_pOrigFrame = null;
                m_uExecutedLoops = 0;

                m_pSplitTimes.Capacity = pAnimation.Frames.Count;

                float accumUnitsOfTime = 0;
                float newUnitOfTimeValue = singleDuration / pAnimation.TotalDelayUnits;

                var pFrames = pAnimation.Frames;

                //TODO: CCARRAY_VERIFY_TYPE(pFrames, CCAnimationFrame *);

                foreach (var pObj in pFrames)
                {
                    var frame = (CCAnimationFrame) pObj;
                    float value = (accumUnitsOfTime * newUnitOfTimeValue) / singleDuration;
                    accumUnitsOfTime += frame.DelayUnits;
                    m_pSplitTimes.Add(value);
                }
                return true;
            }
            return false;
        }

        public override object Copy(ICCCopyable pZone)
        {
            if (pZone != null)
            {
                //in case of being called at sub class
                var pCopy = (CCAnimate) (pZone);
                base.Copy(pZone);

                pCopy.InitWithAnimation((CCAnimation) m_pAnimation.Copy());

                return pCopy;
            }
            return new CCAnimate(this);
        }

        protected internal override void StartWithTarget(CCNode target)
        {
            base.StartWithTarget(target);
            var pSprite = (CCSprite) (target);

            m_pOrigFrame = null;

            if (m_pAnimation.RestoreOriginalFrame)
            {
                m_pOrigFrame = pSprite.DisplayFrame;
            }

            m_nNextFrame = 0;
            m_uExecutedLoops = 0;
        }

        public override void Stop()
        {
            if (m_pAnimation.RestoreOriginalFrame && m_pTarget != null)
            {
                ((CCSprite) (m_pTarget)).DisplayFrame = m_pOrigFrame;
            }

            base.Stop();
        }

        public override void Update(float t)
        {
            // if t==1, ignore. Animation should finish with t==1
            if (t < 1.0f)
            {
                t *= m_pAnimation.Loops;

                // new loop?  If so, reset frame counter
                var loopNumber = (uint) t;
                if (loopNumber > m_uExecutedLoops)
                {
                    m_nNextFrame = 0;
                    m_uExecutedLoops++;
                }

                // new t for animations
                t = t % 1.0f;
            }

            var frames = m_pAnimation.Frames;
            int numberOfFrames = frames.Count;

            for (int i = m_nNextFrame; i < numberOfFrames; i++)
            {
                float splitTime = m_pSplitTimes[i];

                if (splitTime <= t)
                {
                    var frame = (CCAnimationFrame) frames[i];
                    var frameToDisplay = frame.SpriteFrame;
                    if (frameToDisplay != null)
                    {
                        ((CCSprite) m_pTarget).DisplayFrame = frameToDisplay;
                    }

                    var dict = frame.UserInfo;
                    if (dict != null)
                    {
                        //TODO: [[NSNotificationCenter defaultCenter] postNotificationName:CCAnimationFrameDisplayedNotification object:target_ userInfo:dict];
                    }
                    m_nNextFrame = i + 1;
                }
                    // Issue 1438. Could be more than one frame per tick, due to low frame rate or frame delta < 1/FPS
                else
                {
                    break;
                }
            }
        }

        public override CCFiniteTimeAction Reverse()
        {
            var pOldArray = m_pAnimation.Frames;
            var pNewArray = new List<CCAnimationFrame>(pOldArray.Count);

            //TODO: CCARRAY_VERIFY_TYPE(pOldArray, CCAnimationFrame*);

            if (pOldArray.Count > 0)
            {
                for (int i = pOldArray.Count - 1; i >= 0; i--)
                {
                    var pElement = (CCAnimationFrame) pOldArray[i];
                    if (pElement == null)
                    {
                        break;
                    }

                    pNewArray.Add(pElement.Copy() as CCAnimationFrame);
                }
            }

            var newAnim = new CCAnimation(pNewArray, m_pAnimation.DelayPerUnit, m_pAnimation.Loops);
            newAnim.RestoreOriginalFrame = m_pAnimation.RestoreOriginalFrame;
            return new CCAnimate(newAnim);
        }
    }
}