using System.Collections.Generic;
using System.Diagnostics;

namespace CocosSharp
{
    public class CCAnimate : CCActionInterval
    {
		public CCAnimation Animation { get; private set; }
		public List<float> SplitTimes { get; private set; }

        #region Constructors

        public CCAnimate(CCAnimation pAnimation) : this(pAnimation, pAnimation.Duration * pAnimation.Loops)
        {
        }

        private CCAnimate(CCAnimation pAnimation, float totalDuration) : base(totalDuration)
        {
            Debug.Assert(totalDuration == pAnimation.Duration * pAnimation.Loops);

            InitCCAnimate(pAnimation);
        }

        private void InitCCAnimate(CCAnimation pAnimation)
        {
            Debug.Assert(pAnimation != null);

            Animation = pAnimation;
			SplitTimes = new List<float>();

            SplitTimes.Capacity = pAnimation.Frames.Count;

            float singleDuration = Animation.Duration;
            float accumUnitsOfTime = 0;
            float newUnitOfTimeValue = singleDuration / Animation.TotalDelayUnits;

            var pFrames = Animation.Frames;

            //TODO: CCARRAY_VERIFY_TYPE(pFrames, CCAnimationFrame *);

            foreach (var pObj in pFrames)
            {
                var frame = (CCAnimationFrame) pObj;
                float value = (accumUnitsOfTime * newUnitOfTimeValue) / singleDuration;
                accumUnitsOfTime += frame.DelayUnits;
                SplitTimes.Add(value);
            }
        }

        #endregion Constructors

		protected internal override CCActionState StartAction (CCNode target)
		{
			return new CCAnimateState (this, target);

		}

		// Take me out later - See comments in CCAction
		public override bool HasState 
		{ 
			get { return true; }
		}

        public override CCFiniteTimeAction Reverse()
        {
            var pOldArray = Animation.Frames;
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

                    pNewArray.Add(pElement.DeepCopy());
                }
            }

            var newAnim = new CCAnimation(pNewArray, Animation.DelayPerUnit, Animation.Loops);
            newAnim.RestoreOriginalFrame = Animation.RestoreOriginalFrame;
            return new CCAnimate(newAnim);
        }
    }

	public class CCAnimateState : CCActionIntervalState
	{

		protected CCAnimation Animation { get; private set; }
		protected List<float> SplitTimes { get; private set; }
		protected int nextFrame;
		protected CCSpriteFrame originalFrame;
		private uint executedLoops;


		public CCAnimateState (CCAnimate action, CCNode target)
			: base(action, target)
		{ 
			Animation = action.Animation;
			SplitTimes = action.SplitTimes;

			var pSprite = (CCSprite) (target);

			originalFrame = null;

			if (Animation.RestoreOriginalFrame)
			{
				originalFrame = pSprite.DisplayFrame;
			}

			nextFrame = 0;
			executedLoops = 0;
		}

		public override void Stop()
		{
			if (Animation.RestoreOriginalFrame && Target != null)
			{
				((CCSprite) (Target)).DisplayFrame = originalFrame;
			}

			base.Stop();
		}

		public override void Update(float t)
		{
			// if t==1, ignore. Animation should finish with t==1
			if (t < 1.0f)
			{
				t *= Animation.Loops;

				// new loop?  If so, reset frame counter
				var loopNumber = (uint) t;
				if (loopNumber > executedLoops)
				{
					nextFrame = 0;
					executedLoops++;
				}

				// new t for animations
				t = t % 1.0f;
			}

			var frames = Animation.Frames;
			int numberOfFrames = frames.Count;

			for (int i = nextFrame; i < numberOfFrames; i++)
			{
				float splitTime = SplitTimes[i];

				if (splitTime <= t)
				{
					var frame = (CCAnimationFrame) frames[i];
					var frameToDisplay = frame.SpriteFrame;
					if (frameToDisplay != null)
					{
						((CCSprite) Target).DisplayFrame = frameToDisplay;
					}

					var dict = frame.UserInfo;
					if (dict != null)
					{
						//TODO: [[NSNotificationCenter defaultCenter] postNotificationName:CCAnimationFrameDisplayedNotification object:target_ userInfo:dict];
					}
					nextFrame = i + 1;
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