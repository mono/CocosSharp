using Spine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CocosSharp.Spine
{
    public class CCSkeletonAnimation : CCSkeleton
    {

        public float TimeScale = 1;

        AnimationState State { get; set; }
        public bool OwnsAnimationStateData { get; private set; }

        public delegate void StartEndDelegate(AnimationState state, int trackIndex);
        public event StartEndDelegate Start;
        public event StartEndDelegate End;

        public delegate void EventDelegate(AnimationState state, int trackIndex, Event e);
        public event EventDelegate Event;

        public delegate void CompleteDelegate(AnimationState state, int trackIndex, int loopCount);
        public event CompleteDelegate Complete;

        public CCSkeletonAnimation()
        {
            this.Initializer();

        }

        public CCSkeletonAnimation(SkeletonData skeletonData) : 
            base(skeletonData,false)
        {
            this.Initializer();
        }

        public CCSkeletonAnimation(string skeletonDataFile, Atlas atlas, float scale)
            :base(skeletonDataFile,atlas,scale)
        {
            this.Initializer();
        }

        public CCSkeletonAnimation(string skeletonDataFile, string atlasFile, float scale) 
            : base(skeletonDataFile,atlasFile,scale)
        {
           
           Initializer();
        }

        
        public void SetAnimationStateData (AnimationStateData stateData ) {
	        
	        if (stateData!=null)

	        OwnsAnimationStateData = false;
	        State = new AnimationState(stateData);
            State.Event += OnEvent;
            State.Start += OnStart;
            State.Complete += OnComplete;
            State.End += OnEnd;

        }

        public override void Update(float dt)
        {
            base.Update(dt);

            dt *= TimeScale;
            State.Update(dt);
            State.Apply(Skeleton);
            UpdateWorldTransform();
        }

        public void SetMix(string fromAnimation, string toAnimation, float duration)
        {
            State.Data.SetMix(fromAnimation, toAnimation, duration);
        }

        ~CCSkeletonAnimation()
        {
        }

        void Initializer()
        {
            OwnsAnimationStateData = true;

            State = new AnimationState(new AnimationStateData(Skeleton.Data));
            State.Event += OnEvent;
            State.Start += OnStart;
            State.Complete += OnComplete;
            State.End += OnEnd;
        }

        void OnEnd(AnimationState state, int trackIndex)
        {
            if (End != null)
                End(state, trackIndex);

        }

        private void OnComplete(AnimationState state, int trackIndex, int loopCount)
        {
            // spAnimationState_clearTrack
            if (Complete != null)
                Complete(state, trackIndex, loopCount);
        }

        void OnStart(AnimationState state, int trackIndex)
        {
            if (Start != null)
                Start(state, trackIndex);
        }

        void OnEvent(AnimationState state, int trackIndex, Event e)
        {
            if (Event != null)
                Event(state, trackIndex, e);

        }

       public TrackEntry SetAnimation(int trackIndex, string name, bool loop)
        {

            Animation animation = Skeleton.Data.FindAnimation(name);
            if (animation == null)
            {
                CCLog.Log(string.Format("Spine: Animation not found: %s", name)); //CCLog("Spine: Animation not found: %s", name);
                return null;
            }
            return State.SetAnimation(trackIndex, animation, loop);
        }

       public TrackEntry AddAnimation(int trackIndex, string name, bool loop, float delay = 0)
        {
            Animation animation = Skeleton.Data.FindAnimation(name);
            if (animation == null)
            {
                CCLog.Log("Spine: Animation not found: %s", name);
                return null;
            }
            return State.AddAnimation(trackIndex, animation, loop, delay);
        }


    }
}
