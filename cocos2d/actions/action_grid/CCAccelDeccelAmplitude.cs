using System;

namespace CocosSharp
{
    public class CCAccelDeccelAmplitude : CCAccelAmplitude
    {
        #region Constructors

        public CCAccelDeccelAmplitude(CCAction pAction, float duration) : base(pAction, duration)
        {
        }

        #endregion Constructors


        protected internal override CCActionState StartAction(CCNode target)
        {
            return new CCAccelDeccelAmplitudeState(this, target);
        }

        public override CCFiniteTimeAction Reverse()
        {
            return new CCAccelDeccelAmplitude(OtherAction.Reverse(), Duration);
        }
    }


    #region Action state

    public class CCAccelDeccelAmplitudeState : CCAccelAmplitudeState
    {
        public CCAccelDeccelAmplitudeState(CCAccelDeccelAmplitude action, CCNode target) : base(action, target)
        {
        }

        public override void Update(float time)
        {
            float f = time * 2;

            if (f > 1)
            {
                f -= 1;
                f = 1 - f;
            }

            OtherActionState.StateAmplitudeRate = (float)Math.Pow(f, CachedRate);
        }
    }

    #endregion Action state
}