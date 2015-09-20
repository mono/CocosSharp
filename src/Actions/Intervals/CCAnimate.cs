using System.Collections.Generic;
using System.Diagnostics;

namespace CocosSharp
{
    public class CCAnimate : CCFiniteTimeAction
    {
        public CCAnimation Animation { get; private set; }

        public List<float> SplitTimes { get; private set; }

        #region Constructors

        public CCAnimate (CCAnimation animation) : this (animation, animation.Duration * animation.Loops)
        {
        }

        private CCAnimate (CCAnimation animation, float totalDuration) : base (totalDuration)
        {
            Debug.Assert (animation != null);
            Debug.Assert (totalDuration == animation.Duration * animation.Loops);

            Animation = animation;
            SplitTimes = new List<float> ();

            SplitTimes.Capacity = animation.Frames.Count;

            float singleDuration = Animation.Duration;
            float accumUnitsOfTime = 0;
            float newUnitOfTimeValue = singleDuration / Animation.TotalDelayUnits;

            var pFrames = Animation.Frames;

            //TODO: CCARRAY_VERIFY_TYPE(pFrames, CCAnimationFrame *);

            foreach (var pObj in pFrames)
            {
                var frame = (CCAnimationFrame)pObj;
                float value = (accumUnitsOfTime * newUnitOfTimeValue) / singleDuration;
                accumUnitsOfTime += frame.DelayUnits;
                SplitTimes.Add (value);
            }
        }

        #endregion Constructors


        protected internal override CCActionState StartAction(CCNode target)
        {
            return new CCAnimateState (this, target);

        }

        public override CCFiniteTimeAction Reverse ()
        {
            var oldArray = Animation.Frames;
            var newArray = new List<CCAnimationFrame> (oldArray.Count);

            //TODO: CCARRAY_VERIFY_TYPE(pOldArray, CCAnimationFrame*);

            if (oldArray.Count > 0)
            {
                for (int i = oldArray.Count - 1; i >= 0; i--)
                {
                    var pElement = (CCAnimationFrame)oldArray [i];
                    if (pElement == null)
                    {
                        break;
                    }

                    newArray.Add (pElement.Copy ());
                }
            }

            var newAnim = new CCAnimation (newArray, Animation.DelayPerUnit, Animation.Loops);
            newAnim.RestoreOriginalFrame = Animation.RestoreOriginalFrame;
            return new CCAnimate (newAnim);
        }
    }

    public class CCAnimateState : CCFiniteTimeActionState
    {

        protected CCAnimation Animation { get; private set; }

        protected List<float> SplitTimes { get; private set; }

        protected int NextFrame;
        protected CCSpriteFrame OriginalFrame;
        private uint ExecutedLoops;


        public CCAnimateState (CCAnimate action, CCNode target)
            : base (action, target)
        { 
            Animation = action.Animation;
            SplitTimes = action.SplitTimes;

            var sprite = (CCSprite)(target);

            OriginalFrame = null;

            if (Animation.RestoreOriginalFrame)
            {
                OriginalFrame = sprite.SpriteFrame;
            }

            NextFrame = 0;
            ExecutedLoops = 0;
        }

        protected internal override void Stop()
        {
            if (Animation.RestoreOriginalFrame && Target != null)
            {
                ((CCSprite)(Target)).SpriteFrame = OriginalFrame;
            }

            base.Stop();
        }

        public override void Update (float time)
        {
            // if t==1, ignore. Animation should finish with t==1
            if (time < 1.0f)
            {
                time *= Animation.Loops;

                // new loop?  If so, reset frame counter
                var loopNumber = (uint)time;
                if (loopNumber > ExecutedLoops)
                {
                    NextFrame = 0;
                    ExecutedLoops++;
                }

                // new t for animations
                time = time % 1.0f;
            }

            var frames = Animation.Frames;
            int numberOfFrames = frames.Count;

            for (int i = NextFrame; i < numberOfFrames; i++)
            {
                float splitTime = SplitTimes [i];

                if (splitTime <= time)
                {
                    var frame = (CCAnimationFrame)frames [i];
                    var frameToDisplay = frame.SpriteFrame;
                    if (frameToDisplay != null)
                    {
                        ((CCSprite)Target).SpriteFrame = frameToDisplay;
                    }

                    var dict = frame.UserInfo;
                    if (dict != null)
                    {
                        //TODO: [[NSNotificationCenter defaultCenter] postNotificationName:CCAnimationFrameDisplayedNotification object:target_ userInfo:dict];
                    }
                    NextFrame = i + 1;
                }
                // Issue 1438. Could be more than one frame per tick, due to low frame rate or frame delta < 1/FPS
                else
                {
                    break;
                }
            }
        }

    }
}