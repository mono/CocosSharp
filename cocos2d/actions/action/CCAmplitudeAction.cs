using System;

namespace CocosSharp
{
    public abstract class CCAmplitudeAction : CCActionInterval
    {
        public float Amplitude { get; private set; }


        #region Constructors

        public CCAmplitudeAction(float duration, float amplitude = 0) : base(duration)
        {
            Amplitude = amplitude;
        }

        #endregion Constructors
    }


    #region Action state

    public abstract class CCAmplitudeActionState : CCActionIntervalState
    {
        protected float Amplitude { get; private set; }
        protected internal float AmplitudeRate { get; set; }

        public CCAmplitudeActionState(CCAmplitudeAction action, CCNode target) : base(action, target)
        {
            Amplitude = action.Amplitude;
            AmplitudeRate = 1.0f;
        }
    }

    #endregion Action state
}

