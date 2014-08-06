using System.Diagnostics;
using System;

namespace CocosSharp
{
    public interface ICCActionTweenDelegate
    {
        void UpdateTweenAction (float value, string key);
    }

    public class CCActionTween : CCFiniteTimeAction
    {
        #region Properties

        public float From { get; private set; }
        public float To { get; private set; }
        public string Key { get; private set; }
        public Action<float, string> TweenAction { get; private set; }

        #endregion Properties


        #region Constructors

        public CCActionTween (float duration, string key, float from, float to)
            : base (duration)
        {
            Key = key;
            To = to;
            From = from;
        }

        public CCActionTween (float duration, string key, float from, float to, Action<float,string> tweenAction) : this (duration, key, from, to)
        {
            TweenAction = tweenAction;
        }

        #endregion Constructors

        protected internal override CCActionState StartAction(CCNode target)
        {
            return new CCActionTweenState (this, target);

        }

        public override CCFiniteTimeAction Reverse ()
        {
            return new CCActionTween (Duration, Key, To, From, TweenAction);
        }
    }

    public class CCActionTweenState : CCFiniteTimeActionState
    {
        protected float Delta;

        protected float From { get; private set; }

        protected float To { get; private set; }

        protected string Key { get; private set; }

        protected Action<float, string> TweenAction { get; private set; }

        public CCActionTweenState (CCActionTween action, CCNode target)
            : base (action, target)
        { 
            Debug.Assert (Target is ICCActionTweenDelegate, "target must implement CCActionTweenDelegate");
            TweenAction = action.TweenAction;
            From = action.From;
            To = action.To;
            Key = action.Key;
            Delta = To - From;
        }

        public override void Update (float time)
        {
            float amt = To - Delta * (1 - time);
            if (TweenAction != null)
            {
                TweenAction (amt, Key);
            }
            else if (Target is ICCActionTweenDelegate)
            {
                ((ICCActionTweenDelegate)Target).UpdateTweenAction (amt, Key);
            }
        }

    }
}