using System.Collections.Generic;
using System.Diagnostics;

namespace cocos2d
{
    public delegate void CCBAnimationManagerDelegate(string name);

    public class CCBAnimationManager : CCObject
    {
        private readonly Dictionary<CCNode, Dictionary<string, object>> mBaseValues = new Dictionary<CCNode, Dictionary<string, object>>();

        private readonly Dictionary<CCNode, Dictionary<int, Dictionary<string, CCBSequenceProperty>>> mNodeSequences =
            new Dictionary<CCNode, Dictionary<int, Dictionary<string, CCBSequenceProperty>>>();

        private readonly List<CCBSequence> mSequences = new List<CCBSequence>();

        private int mAutoPlaySequenceId;
        private CCBAnimationManagerDelegate mDelegate;
        private CCSize mRootContainerSize;
        private CCNode mRootNode;
        private CCBSequence mRunningSequence;

        public CCBAnimationManager()
        {
            Init();
        }

        public virtual bool Init()
        {
            return true;
        }

        public List<CCBSequence> Sequences
        {
            get { return mSequences; }
        }

        public int AutoPlaySequenceId
        {
            set { mAutoPlaySequenceId = value; }
            get { return mAutoPlaySequenceId; }
        }

        public CCNode RootNode
        {
            get { return mRootNode; }
            set { mRootNode = value; }
        }

        public CCSize RootContainerSize
        {
            get { return mRootContainerSize; }
            set { mRootContainerSize = value; }
        }

        public CCBAnimationManagerDelegate Delegate
        {
            get { return mDelegate; }
            set { mDelegate = value; }
        }

        public string RunningSequenceName
        {
            get { return mRunningSequence.Name; }
        }

        public CCSize GetContainerSize(CCNode node)
        {
            if (node != null)
            {
                return node.ContentSize;
            }
            else
            {
                return mRootContainerSize;
            }
        }

        public void AddNode(CCNode node, Dictionary<int, Dictionary<string, CCBSequenceProperty>> pSeq)
        {
            mNodeSequences.Add(node, pSeq);
        }

        public void SetBaseValue(object pValue, CCNode node, string pPropName)
        {
            Dictionary<string, object> props;
            if (!mBaseValues.TryGetValue(node, out props))
            {
                props = new Dictionary<string, object>();
                mBaseValues.Add(node, props);
            }

            props[pPropName] = pValue;
        }

        public void RunAnimations(string pName, float fTweenDuration)
        {
            int seqId = GetSequenceId(pName);
            RunAnimations(seqId, fTweenDuration);
        }

        public void RunAnimations(string pName)
        {
            RunAnimations(pName, 0);
        }

        public void RunAnimations(int nSeqId, float fTweenDuration)
        {
            Debug.Assert(nSeqId != -1, "Sequence id couldn't be found");

            mRootNode.StopAllActions();

            foreach (var pElement in mNodeSequences)
            {
                CCNode node = pElement.Key;
                node.StopAllActions();

                // Refer to CCBReader::readKeyframe() for the real type of value
                Dictionary<int, Dictionary<string, CCBSequenceProperty>> seqs = pElement.Value;

                var seqNodePropNames = new List<string>();

                Dictionary<string, CCBSequenceProperty> seqNodeProps;
                if (seqs.TryGetValue(nSeqId, out seqNodeProps))
                {
                    // Reset nodes that have sequence node properties, and run actions on them
                    foreach (var pElement1 in seqNodeProps)
                    {
                        string propName = pElement1.Key;
                        CCBSequenceProperty seqProp = pElement1.Value;
                        seqNodePropNames.Add(propName);

                        SetFirstFrame(node, seqProp, fTweenDuration);
                        RunAction(node, seqProp, fTweenDuration);
                    }
                }

                // Reset the nodes that may have been changed by other timelines
                Dictionary<string, object> nodeBaseValues;
                if (mBaseValues.TryGetValue(node, out nodeBaseValues))
                {
                    foreach (var pElement2 in nodeBaseValues)
                    {
                        if (!seqNodePropNames.Contains(pElement2.Key))
                        {
                            object value = pElement2.Value;

                            if (value != null)
                            {
                                SetAnimatedProperty(pElement2.Key, node, value, fTweenDuration);
                            }
                        }
                    }
                }
            }

            // Make callback at end of sequence
            CCBSequence seq = GetSequence(nSeqId);
            CCAction completeAction = CCSequence.ActionOneTwo(
                CCDelayTime.Create(seq.Duration + fTweenDuration),
                CCCallFunc.Create(SequenceCompleted)
                );

            mRootNode.RunAction(completeAction);

            // Set the running scene
            mRunningSequence = GetSequence(nSeqId);
        }

        // Commented out for now as it does not seem to be used
//        public void Debug()
//        {
//        }

        private object GetBaseValue(CCNode node, string pPropName)
        {
            return mBaseValues[node][pPropName];
        }

        private int GetSequenceId(string pSequenceName)
        {
            for (int i = 0; i < mSequences.Count; i++)
            {
                if (mSequences[i].Name == pSequenceName)
                {
                    return mSequences[i].SequenceId;
                }
            }
            return -1;
        }

        private CCBSequence GetSequence(int nSequenceId)
        {
            for (int i = 0; i < mSequences.Count; i++)
            {
                if (mSequences[i].SequenceId == nSequenceId)
                {
                    return mSequences[i];
                }
            }
            return null;
        }

        private CCActionInterval GetAction(CCBKeyframe pKeyframe0, CCBKeyframe pKeyframe1, string pPropName, CCNode node)
        {
            float duration = pKeyframe1.Time - (pKeyframe0 != null ? pKeyframe0.Time : 0);

            switch (pPropName)
            {
                case "rotation":
                    {
                        var value = (CCBValue) pKeyframe1.Value;
                        return CCBRotateTo.Create(duration, value.GetFloatValue());
                    }
                case "opacity":
                    {
                        var value = (CCBValue) pKeyframe1.Value;
                        return CCFadeTo.Create(duration, value.GetByteValue());
                    }
                case "color":
                    {
                        var color = (ccColor3BWapper) pKeyframe1.Value;
                        CCColor3B c = color.getColor();

                        return CCTintTo.Create(duration, c.r, c.g, c.b);
                    }
                case "visible":
                    {
                        var value = (CCBValue) pKeyframe1.Value;
                        if (value.GetBoolValue())
                        {
                            return CCSequence.ActionOneTwo(CCDelayTime.Create(duration), CCShow.Create());
                        }
                        return CCSequence.ActionOneTwo(CCDelayTime.Create(duration), CCHide.Create());
                    }
                case "displayFrame":
                    return CCSequence.ActionOneTwo(CCDelayTime.Create(duration), CCBSetSpriteFrame.Create((CCSpriteFrame) pKeyframe1.Value));
                case "position":
                    {
                        // Get position type
                        var array = (List<CCBValue>) GetBaseValue(node, pPropName);
                        var type = (kCCBPositionType) array[2].GetIntValue();

                        // Get relative position
                        var value = (List<CCBValue>) pKeyframe1.Value;
                        float x = value[0].GetFloatValue();
                        float y = value[1].GetFloatValue();

                        CCSize containerSize = GetContainerSize(node.Parent);

                        CCPoint absPos = CCBHelper.GetAbsolutePosition(new CCPoint(x, y), type, containerSize, pPropName);

                        return CCMoveTo.Create(duration, absPos);
                    }
                case "scale":
                    {
                        // Get position type
                        var array = (List<CCBValue>) GetBaseValue(node, pPropName);
                        var type = (kCCBScaleType) array[2].GetIntValue();

                        // Get relative scale
                        var value = (List<CCBValue>) pKeyframe1.Value;
                        float x = value[0].GetFloatValue();
                        float y = value[1].GetFloatValue();

                        if (type == kCCBScaleType.kCCBScaleTypeMultiplyResolution)
                        {
                            float resolutionScale = CCBReader.ResolutionScale;
                            x *= resolutionScale;
                            y *= resolutionScale;
                        }

                        return CCScaleTo.Create(duration, x, y);
                    }
                default:
                    CCLog.Log("CCBReader: Failed to create animation for property: {0}", pPropName);
                    break;
            }

            return null;
        }

        private void SetAnimatedProperty(string pPropName, CCNode node, object pValue, float fTweenDuraion)
        {
            if (fTweenDuraion > 0)
            {
                // Create a fake keyframe to generate the action from
                var kf1 = new CCBKeyframe();
                kf1.Value = pValue;
                kf1.Time = fTweenDuraion;
                kf1.EasingType = kCCBKeyframeEasing.kCCBKeyframeEasingLinear;

                // Animate
                CCActionInterval tweenAction = GetAction(null, kf1, pPropName, node);
                node.RunAction(tweenAction);
            }
            else
            {
                // Just set the value

                if (pPropName == "position")
                {
                    // Get position type
                    var array = (List<CCBValue>) GetBaseValue(node, pPropName);
                    var type = (kCCBPositionType) array[2].GetIntValue();

                    // Get relative position
                    var value = (List<CCBValue>) pValue;
                    float x = value[0].GetFloatValue();
                    float y = value[1].GetFloatValue();

                    node.Position = CCBHelper.GetAbsolutePosition(new CCPoint(x, y), type, GetContainerSize(node.Parent), pPropName);
                }
                else if (pPropName == "scale")
                {
                    // Get scale type
                    var array = (List<CCBValue>) GetBaseValue(node, pPropName);
                    var type = (kCCBScaleType) array[2].GetIntValue();

                    // Get relative scale
                    var value = (List<CCBValue>) pValue;
                    float x = value[0].GetFloatValue();
                    float y = value[1].GetFloatValue();

                    CCBHelper.SetRelativeScale(node, x, y, type, pPropName);
                }
                else
                {
                    // [node setValue:value forKey:name];

                    // TODO only handle rotation, opacity, displayFrame, color
                    if (pPropName == "rotation")
                    {
                        float rotate = ((CCBValue) pValue).GetFloatValue();
                        node.Rotation = rotate;
                    }
                    else if (pPropName == "opacity")
                    {
                        byte opacity = ((CCBValue) pValue).GetByteValue();
                        ((ICCRGBAProtocol) node).Opacity = opacity;
                    }
                    else if (pPropName == "displayFrame")
                    {
                        ((CCSprite) node).DisplayFrame = (CCSpriteFrame) pValue;
                    }
                    else if (pPropName == "color")
                    {
                        var color = (ccColor3BWapper) pValue;
                        ((CCSprite) node).Color = color.getColor();
                    }
                    else
                    {
                        CCLog.Log("unsupported property name is {0}", pPropName);
                        Debug.Assert(false, "unsupported property now");
                    }
                }
            }
        }

        private void SetFirstFrame(CCNode node, CCBSequenceProperty pSeqProp, float fTweenDuration)
        {
            List<CCBKeyframe> keyframes = pSeqProp.Keyframes;

            if (keyframes.Count == 0)
            {
                // Use base value (no animation)
                object baseValue = GetBaseValue(node, pSeqProp.Name);
                Debug.Assert(baseValue != null, "No baseValue found for property");
                SetAnimatedProperty(pSeqProp.Name, node, baseValue, fTweenDuration);
            }
            else
            {
                // Use first keyframe
                CCBKeyframe keyframe = keyframes[0];
                SetAnimatedProperty(pSeqProp.Name, node, keyframe.Value, fTweenDuration);
            }
        }

        private CCActionInterval GetEaseAction(CCActionInterval pAction, kCCBKeyframeEasing nEasingType, float fEasingOpt)
        {
            switch (nEasingType)
            {
                case kCCBKeyframeEasing.kCCBKeyframeEasingInstant:
                case kCCBKeyframeEasing.kCCBKeyframeEasingLinear:
                    return pAction;
                case kCCBKeyframeEasing.kCCBKeyframeEasingCubicIn:
                    return CCEaseIn.Create(pAction, fEasingOpt);
                case kCCBKeyframeEasing.kCCBKeyframeEasingCubicOut:
                    return CCEaseOut.Create(pAction, fEasingOpt);
                case kCCBKeyframeEasing.kCCBKeyframeEasingCubicInOut:
                    return CCEaseInOut.Create(pAction, fEasingOpt);
                case kCCBKeyframeEasing.kCCBKeyframeEasingBackIn:
                    return CCEaseBackIn.Create(pAction);
                case kCCBKeyframeEasing.kCCBKeyframeEasingBackOut:
                    return CCEaseBackOut.Create(pAction);
                case kCCBKeyframeEasing.kCCBKeyframeEasingBackInOut:
                    return CCEaseBackInOut.Create(pAction);
                case kCCBKeyframeEasing.kCCBKeyframeEasingBounceIn:
                    return CCEaseBounceIn.Create(pAction);
                case kCCBKeyframeEasing.kCCBKeyframeEasingBounceOut:
                    return CCEaseBounceOut.Create(pAction);
                case kCCBKeyframeEasing.kCCBKeyframeEasingBounceInOut:
                    return CCEaseBounceInOut.Create(pAction);
                case kCCBKeyframeEasing.kCCBKeyframeEasingElasticIn:
                    return CCEaseElasticIn.Create(pAction, fEasingOpt);
                case kCCBKeyframeEasing.kCCBKeyframeEasingElasticOut:
                    return CCEaseElasticOut.Create(pAction, fEasingOpt);
                case kCCBKeyframeEasing.kCCBKeyframeEasingElasticInOut:
                    return CCEaseElasticInOut.Create(pAction, fEasingOpt);
                default:
                    CCLog.Log("CCBReader: Unkown easing type {0}", nEasingType);
                    return pAction;
            }
        }

        private void RunAction(CCNode node, CCBSequenceProperty pSeqProp, float fTweenDuration)
        {
            List<CCBKeyframe> keyframes = pSeqProp.Keyframes;
            int numKeyframes = keyframes.Count;

            if (numKeyframes > 1)
            {
                // Make an animation!
                var actions = new List<CCFiniteTimeAction>();

                CCBKeyframe keyframeFirst = keyframes[0];
                float timeFirst = keyframeFirst.Time + fTweenDuration;

                if (timeFirst > 0)
                {
                    actions.Add(CCDelayTime.Create(timeFirst));
                }

                for (int i = 0; i < numKeyframes - 1; ++i)
                {
                    CCBKeyframe kf0 = keyframes[i];
                    CCBKeyframe kf1 = keyframes[i + 1];

                    CCActionInterval action = GetAction(kf0, kf1, pSeqProp.Name, node);
                    if (action != null)
                    {
                        // Apply easing
                        action = GetEaseAction(action, kf0.EasingType, kf0.EasingOpt);

                        actions.Add(action);
                    }
                }

                if (actions.Count > 1)
                {
                    CCFiniteTimeAction seq = CCSequence.Create(actions.ToArray());
                    node.RunAction(seq);
                }
                else
                {
                    node.RunAction(actions[0]);
                }
            }
        }

        private void SequenceCompleted()
        {
            if (mDelegate != null)
            {
                mDelegate(mRunningSequence.Name);
            }

            int nextSeqId = mRunningSequence.ChainedSequenceId;
            mRunningSequence = null;

            if (nextSeqId != -1)
            {
                RunAnimations(nextSeqId, 0);
            }
        }
    }

    internal class CCBSetSpriteFrame : CCActionInstant
    {
        private CCSpriteFrame mSpriteFrame;

        /** creates a Place action with a position */

        public static CCBSetSpriteFrame Create(CCSpriteFrame pSpriteFrame)
        {
            var ret = new CCBSetSpriteFrame();
            ret.InitWithSpriteFrame(pSpriteFrame);
            return ret;
        }

        public bool InitWithSpriteFrame(CCSpriteFrame pSpriteFrame)
        {
            mSpriteFrame = pSpriteFrame;
            return true;
        }

        public override void Update(float time)
        {
            ((CCSprite) m_pTarget).DisplayFrame = mSpriteFrame;
        }

        public override CCObject CopyWithZone(CCZone pZone)
        {
            CCBSetSpriteFrame pRet;

            if (pZone != null && pZone.m_pCopyObject != null)
            {
                pRet = (CCBSetSpriteFrame) (pZone.m_pCopyObject);
            }
            else
            {
                pRet = new CCBSetSpriteFrame();
                pZone = new CCZone(pRet);
            }

            pRet.InitWithSpriteFrame(mSpriteFrame);
            base.CopyWithZone(pZone);
            return pRet;
        }
    }

    internal class CCBRotateTo : CCActionInterval
    {
        private float mDiffAngle;
        private float mDstAngle;
        private float mStartAngle;

        public static CCBRotateTo Create(float fDuration, float fAngle)
        {
            var ret = new CCBRotateTo();
            ret.InitWithDuration(fDuration, fAngle);
            return ret;
        }

        public bool InitWithDuration(float fDuration, float fAngle)
        {
            if (base.InitWithDuration(fDuration))
            {
                mDstAngle = fAngle;

                return true;
            }
            return false;
        }

        public override void Update(float time)
        {
            m_pTarget.Rotation = mStartAngle + (mDiffAngle * time);
        }

        public override CCObject CopyWithZone(CCZone pZone)
        {
            CCBRotateTo pRet;

            if (pZone != null && pZone.m_pCopyObject != null)
            {
                pRet = (CCBRotateTo) (pZone.m_pCopyObject);
            }
            else
            {
                pRet = new CCBRotateTo();
                pZone = new CCZone(pRet);
            }

            pRet.InitWithDuration(m_fDuration, mDstAngle);
            base.CopyWithZone(pZone);
            return pRet;
        }

        public override void StartWithTarget(CCNode node)
        {
            base.StartWithTarget(node);
            mStartAngle = m_pTarget.Rotation;
            mDiffAngle = mDstAngle - mStartAngle;
        }
    }
}