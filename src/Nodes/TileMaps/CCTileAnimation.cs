using System;
using System.Collections.Generic;

namespace CocosSharp
{
    internal class CCTileAnimation : CCFiniteTimeAction
    {
        // Tiled expresses key frame duration in milliseconds, but we're after seconds
        public const float KeyFrameDurationFactor = 1000.0f;

        public short OriginalGid { get; private set; }
        public List<CCTileAnimationKeyFrame> TileKeyFrames { get; private set; }


        #region Constructors

        static float FindTileAnimationDuration(List<CCTileAnimationKeyFrame> keyFrames)
        {
            float duration = 0.0f;

            foreach(CCTileAnimationKeyFrame frame in keyFrames)
                duration += frame.Duration / KeyFrameDurationFactor;

            return duration;
        }

        public CCTileAnimation(short gid, List<CCTileAnimationKeyFrame> keyFrames) 
            : base(FindTileAnimationDuration(keyFrames))
        {
            OriginalGid = gid;
            TileKeyFrames = keyFrames;
        }

        public override CCFiniteTimeAction Reverse()
        {
            var reverseList = new List<CCTileAnimationKeyFrame>(TileKeyFrames);
            reverseList.Reverse();
            return new CCTileAnimation(OriginalGid, reverseList);
        }

        #endregion Constructors


        protected internal override CCActionState StartAction(CCNode target)
        {
            return new CCTileAnimationState(this, target);
        }
    }

    internal class CCTileAnimationState : CCFiniteTimeActionState
    {
        const int InvalidKeyFrameIndex = -1;

        #region Internal structs 

        struct CCTileAnimationKeyFrameInfo
        {
            public short Gid { get; private set; }
            public float StartingTime { get; private set; }
            public float Duration { get; private set; }
            public float EndTime { get { return StartingTime + Duration;} }

            public CCTileAnimationKeyFrameInfo(short gid, float startingTime, float duration) 
                : this()
            {
                Gid = gid;
                StartingTime = startingTime;
                Duration = duration;
            }
        }

        #endregion Internal structs

        #region Properties

        short OriginalGid { get; set; }
        int CurrentKeyFrame { get; set; }
        CCTileAnimationKeyFrameInfo[] TileKeyFramesInfo { get; set; }
        CCTileMapLayer TileMapLayer { get { return Target as CCTileMapLayer; } }

        #endregion Properties


        public CCTileAnimationState(CCTileAnimation action, CCNode target) : base (action, target)
        {
            OriginalGid = action.OriginalGid;
            CurrentKeyFrame = InvalidKeyFrameIndex;

            var keyFrames = action.TileKeyFrames;
            TileKeyFramesInfo = new CCTileAnimationKeyFrameInfo[keyFrames.Count];


            float totalTime = 0.0f;
            int index = 0;
            foreach (CCTileAnimationKeyFrame keyFrame in keyFrames)
            {
                float duration = keyFrame.Duration / CCTileAnimation.KeyFrameDurationFactor;

                TileKeyFramesInfo[index] 
                    = new CCTileAnimationKeyFrameInfo(keyFrame.Gid, totalTime, duration);

                totalTime += duration;
                index += 1;
            }
        }

        public override void Update(float time)
        {
            short newGid = 0;
            int newFrameIdx = CurrentKeyFrame;

            for(int frameIdx = 0; frameIdx < TileKeyFramesInfo.Length; frameIdx++)
            {
                var frameInfo = TileKeyFramesInfo[frameIdx];

                // Time is in the sweet spot
                if(frameInfo.StartingTime <= Elapsed && Elapsed < frameInfo.EndTime)
                {
                    newGid = frameInfo.Gid;
                    newFrameIdx = frameIdx;
                    break;
                }
            }

            // Only ask tile map layer to update if necessary 
            if(newGid != 0 && CurrentKeyFrame != newFrameIdx)
            {
                CurrentKeyFrame = newFrameIdx;
                TileMapLayer.ReplaceTileGIDQuad(OriginalGid, newGid);
            }
        }
    }
}

