using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using CocosDenshion;

namespace CocosSharp
{
	public delegate void CCBAnimationManagerDelegate(string name);

	public class CCBAnimationManager
	{
		private Dictionary<CCNode, Dictionary<string, object>> _baseValues = new Dictionary<CCNode, Dictionary<string, object>>();

		private readonly Dictionary<CCNode, Dictionary<int, Dictionary<string, CCBSequenceProperty>>> _nodeSequences =
			new Dictionary<CCNode, Dictionary<int, Dictionary<string, CCBSequenceProperty>>>();

		private List<CCBSequence> _sequences = new List<CCBSequence>();

		private int _autoPlaySequenceId;
		private CCBAnimationManagerDelegate _delegate;
		private CCSize _rootContainerSize;
		private CCNode _rootNode;
		private CCBSequence _runningSequence;

		private readonly List<string> _documentOutletNames = new List<string>();
		private readonly List<CCNode> _documentOutletNodes = new List<CCNode>();
		private readonly List<string> _documentCallbackNames = new List<string>();
		private readonly List<CCNode> _documentCallbackNodes = new List<CCNode>();
		private readonly List<string> _keyframeCallbacks = new List<string>();
		private readonly Dictionary<string, CCAction> _keyframeCallFuncs = new Dictionary<string, CCAction>();
    
		private string _documentControllerName;
		private string _lastCompletedSequenceName;
    
		private Action _animationCompleteCallbackFunc;

		public bool JSControlled { get; set; }

		public Object Owner { get; set; }

		public CCBAnimationManager()
		{
		}

		public List<CCBSequence> Sequences
		{
			get { return _sequences; }
			set { _sequences = value; }
		}

		public int AutoPlaySequenceId
		{
			set { _autoPlaySequenceId = value; }
			get { return _autoPlaySequenceId; }
		}

		public CCNode RootNode
		{
			get { return _rootNode; }
			set { _rootNode = value; }
		}

		public void AddDocumentCallbackNode(CCNode node)
		{
			_documentCallbackNodes.Add(node);
		}

		public void AddDocumentCallbackName(string name)
		{
			_documentCallbackNames.Add(name);
		}

		public void AddDocumentOutletNode(CCNode node)
		{
			_documentOutletNodes.Add(node);
		}

		public void AddDocumentOutletName(string name)
		{
			_documentOutletNames.Add(name);
		}

		public string DocumentControllerName
		{
			set { _documentControllerName = value; }
			get { return _documentControllerName; }
		}


		public List<string> GetDocumentCallbackNames()
		{
			return _documentCallbackNames;
		}

		public List<CCNode> GetDocumentCallbackNodes()
		{
			return _documentCallbackNodes;
		}

		public List<string> GetDocumentOutletNames()
		{
			return _documentOutletNames;
		}

		public List<CCNode> GetDocumentOutletNodes()
		{
			return _documentOutletNodes;
		}

		public string GetLastCompletedSequenceName()
		{
			return _lastCompletedSequenceName;
		}

		public List<string> GetKeyframeCallbacks()
		{
			return _keyframeCallbacks;
		}

		public CCSize RootContainerSize
		{
			get { return _rootContainerSize; }
			set { _rootContainerSize = value; }
		}

		public CCBAnimationManagerDelegate Delegate
		{
			get { return _delegate; }
			set { _delegate = value; }
		}

		public string RunningSequenceName
		{
			get
			{
				if (_runningSequence != null)
				{
					return _runningSequence.Name;
				}
				return null;
			}
		}

		public CCSize GetContainerSize(CCNode node)
		{
			if (node != null)
			{
				return node.ContentSize;
			}
			else
			{
				return _rootContainerSize;
			}
		}

		public void AddNode(CCNode node, Dictionary<int, Dictionary<string, CCBSequenceProperty>> pSeq)
		{
			_nodeSequences.Add(node, pSeq);
		}

		public void SetBaseValue(object pValue, CCNode node, string pPropName)
		{
			Dictionary<string, object> props;
			if (!_baseValues.TryGetValue(node, out props))
			{
				props = new Dictionary<string, object>();
				_baseValues.Add(node, props);
			}

			props[pPropName] = pValue;
		}

		private object GetBaseValue(CCNode node, string pPropName)
		{
			return _baseValues[node][pPropName];
		}

		private int GetSequenceId(string pSequenceName)
		{
			for (int i = 0; i < _sequences.Count; i++)
			{
				if (_sequences[i].Name == pSequenceName)
				{
					return _sequences[i].SequenceId;
				}
			}
			return -1;
		}

		private CCBSequence GetSequence(int nSequenceId)
		{
			for (int i = 0; i < _sequences.Count; i++)
			{
				if (_sequences[i].SequenceId == nSequenceId)
				{
					return _sequences[i];
				}
			}
			return null;
		}

		public void MoveAnimationsFromNode(CCNode fromNode, CCNode toNode)
		{
			// Move base values

			Dictionary<string, object> baseValue;
			if (_baseValues.TryGetValue(fromNode, out baseValue))
			{
				_baseValues[toNode] = baseValue;
				_baseValues.Remove(fromNode);
			}

			// Move seqs
			Dictionary<int, Dictionary<string, CCBSequenceProperty>> seqs;
			if (_nodeSequences.TryGetValue(fromNode, out seqs))
			{
				_nodeSequences[toNode] = seqs;
				_nodeSequences.Remove(fromNode);
			}
		}

		private CCActionInterval GetAction(CCBKeyframe pKeyframe0, CCBKeyframe pKeyframe1, string pPropName, CCNode node)
		{
			float duration = pKeyframe1.Time - (pKeyframe0 != null ? pKeyframe0.Time : 0);

			switch (pPropName)
			{
				case "rotationX":
					{
						CCBValue value = (CCBValue)pKeyframe1.Value;
						return new CCBRotateXTo(duration, value.GetFloatValue());
					}
				case "rotationY":
					{
						CCBValue value = (CCBValue)pKeyframe1.Value;
						return new CCBRotateYTo(duration, value.GetFloatValue());
					}
				case "rotation":
					{
						var value = (CCBValue)pKeyframe1.Value;
						return new CCBRotateTo(duration, value.GetFloatValue());
					}
				case "opacity":
					{
						var value = (CCBValue)pKeyframe1.Value;
						return new CCFadeTo(duration, value.GetByteValue());
					}
				case "color":
					{
						var color = (CCColor3BWapper)pKeyframe1.Value;
						CCColor3B c = color.Color;

						return new CCTintTo(duration, c.R, c.G, c.B);
					}
				case "visible":
					{
						var value = (CCBValue)pKeyframe1.Value;
						if (value.GetBoolValue())
						{
							return new CCSequence(new CCDelayTime(duration), new CCShow());
						}
						return new CCSequence(new CCDelayTime(duration), new CCHide());
					}
				case "displayFrame":
					return new CCSequence(new CCDelayTime(duration), new CCBSetSpriteFrame((CCSpriteFrame)pKeyframe1.Value));
				case "position":
					{
						// Get position type
						var array = (List<CCBValue>)GetBaseValue(node, pPropName);
						var type = (CCBPositionType)array[2].GetIntValue();

						// Get relative position
						var value = (List<CCBValue>)pKeyframe1.Value;
						float x = value[0].GetFloatValue();
						float y = value[1].GetFloatValue();

						CCSize containerSize = GetContainerSize(node.Parent);

						CCPoint absPos = CCBHelper.GetAbsolutePosition(new CCPoint(x, y), type, containerSize, pPropName);

						return new CCMoveTo(duration, absPos);
					}
				case "scale":
					{
						// Get position type
						var array = (List<CCBValue>)GetBaseValue(node, pPropName);
						var type = (CCBScaleType)array[2].GetIntValue();

						// Get relative scale
						var value = (List<CCBValue>)pKeyframe1.Value;
						float x = value[0].GetFloatValue();
						float y = value[1].GetFloatValue();

						if (type == CCBScaleType.MultiplyResolution)
						{
							float resolutionScale = CCBReader.ResolutionScale;
							x *= resolutionScale;
							y *= resolutionScale;
						}

						return new CCScaleTo(duration, x, y);
					}
				case "skew":
					{
						// Get relative skew
						var value = (List<CCBValue>)pKeyframe1.Value;
						float x = value[0].GetFloatValue();
						float y = value[1].GetFloatValue();
        
						return new CCSkewTo(duration, x, y);
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
				kf1.EasingType = CCBEasingType.Linear;

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
					var array = (List<CCBValue>)GetBaseValue(node, pPropName);
					var type = (CCBPositionType)array[2].GetIntValue();

					// Get relative position
					var value = (List<CCBValue>)pValue;
					float x = value[0].GetFloatValue();
					float y = value[1].GetFloatValue();

					node.Position = CCBHelper.GetAbsolutePosition(new CCPoint(x, y), type, GetContainerSize(node.Parent), pPropName);
				}
				else if (pPropName == "scale")
				{
					// Get scale type
					var array = (List<CCBValue>)GetBaseValue(node, pPropName);
					var type = (CCBScaleType)array[2].GetIntValue();

					// Get relative scale
					var value = (List<CCBValue>)pValue;
					float x = value[0].GetFloatValue();
					float y = value[1].GetFloatValue();

					CCBHelper.SetRelativeScale(node, x, y, type, pPropName);
				}
				else if (pPropName == "skew")
				{
					// Get relative scale
					var value = (List<CCBValue>)pValue;
					float x = value[0].GetFloatValue();
					float y = value[1].GetFloatValue();

					node.SkewX = x;
					node.SkewY = y;
				}
				else
				{
					// [node setValue:value forKey:name];

					// TODO only handle rotation, opacity, displayFrame, color
					if (pPropName == "rotation")
					{
						float rotate = ((CCBValue)pValue).GetFloatValue();
						node.Rotation = rotate;
					}
					else if (pPropName == "rotationX")
					{
						float rotate = ((CCBValue)pValue).GetFloatValue();
						node.RotationX = rotate;
					}
					else if (pPropName == "rotationY")
					{
						float rotate = ((CCBValue)pValue).GetFloatValue();
						node.RotationY = rotate;
					}
					else if (pPropName == "opacity")
					{
						byte opacity = ((CCBValue)pValue).GetByteValue();
						((ICCColor)node).Opacity = opacity;
					}
					else if (pPropName == "displayFrame")
					{
						((CCSprite)node).DisplayFrame = (CCSpriteFrame)pValue;
					}
					else if (pPropName == "color")
					{
						var color = (CCColor3BWapper)pValue;
						((ICCColor)node).Color = color.Color;
					}
					else if (pPropName == "visible")
					{
						bool visible = ((CCBValue)pValue).GetBoolValue();
						node.Visible = visible;
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

		private CCActionInterval GetEaseAction(CCActionInterval pAction, CCBEasingType nEasingTypeType, float fEasingOpt)
		{
			if (pAction is CCSequence)
			{
				return pAction;
			}

			switch (nEasingTypeType)
			{
				case CCBEasingType.Linear:
					return pAction;
				case CCBEasingType.Instant:
					return new CCBEaseInstant(pAction);
				case CCBEasingType.CubicIn:
					return new CCEaseIn(pAction, fEasingOpt);
				case CCBEasingType.CubicOut:
					return new CCEaseOut(pAction, fEasingOpt);
				case CCBEasingType.CubicInOut:
					return new CCEaseInOut(pAction, fEasingOpt);
				case CCBEasingType.BackIn:
					return new CCEaseBackIn(pAction);
				case CCBEasingType.BackOut:
					return new CCEaseBackOut(pAction);
				case CCBEasingType.BackInOut:
					return new CCEaseBackInOut(pAction);
				case CCBEasingType.BounceIn:
					return new CCEaseBounceIn(pAction);
				case CCBEasingType.BounceOut:
					return new CCEaseBounceOut(pAction);
				case CCBEasingType.BounceInOut:
					return new CCEaseBounceInOut(pAction);
				case CCBEasingType.ElasticIn:
					return new CCEaseElasticIn(pAction, fEasingOpt);
				case CCBEasingType.ElasticOut:
					return new CCEaseElasticOut(pAction, fEasingOpt);
				case CCBEasingType.ElasticInOut:
					return new CCEaseElasticInOut(pAction, fEasingOpt);
				default:
					CCLog.Log("CCBReader: Unkown easing type {0}", nEasingTypeType);
					return pAction;
			}
		}

		public Object ActionForCallbackChannel(CCBSequenceProperty channel)
		{
			float lastKeyframeTime = 0;

			var actions = new List<CCFiniteTimeAction>();
			var keyframes = channel.Keyframes;
			int numKeyframes = keyframes.Count;

			for (int i = 0; i < numKeyframes; ++i)
			{

				CCBKeyframe keyframe = keyframes[i];
				float timeSinceLastKeyframe = keyframe.Time - lastKeyframeTime;
				lastKeyframeTime = keyframe.Time;
				if (timeSinceLastKeyframe > 0)
				{
					actions.Add(new CCDelayTime(timeSinceLastKeyframe));
				}

				var keyVal = (List<CCBValue>)keyframe.Value;
				string selectorName = keyVal[0].GetStringValue();
				CCBTargetType selectorTarget =
					(CCBTargetType)int.Parse(keyVal[1].GetStringValue());

				if (JSControlled)
				{
					string callbackName = string.Format("{0}:{1}", selectorTarget, selectorName);
					CCCallFunc callback = (CCCallFunc)_keyframeCallFuncs[callbackName];

					if (callback != null)
					{
						actions.Add(callback);
					}
				}
				else
				{
					Object target = null;

					if (selectorTarget == CCBTargetType.DocumentRoot)
						target = _rootNode;
					else if (selectorTarget == CCBTargetType.Owner)
						target = Owner;

					if (target != null)
					{
						if (selectorName.Length > 0)
						{
							Action<CCNode> selCallFunc = null;

							ICCBSelectorResolver targetAsCCBSelectorResolver = target as ICCBSelectorResolver;

							if (targetAsCCBSelectorResolver != null)
							{
								selCallFunc = targetAsCCBSelectorResolver.OnResolveCCBCCCallFuncSelector(target,
									selectorName);
							}

							if (selCallFunc == null)
							{
								CCLog.Log("Skipping selector {0} since no CCBSelectorResolver is present.",
									selectorName);
							}
							else
							{
								CCCallFuncN callback = new CCCallFuncN(selCallFunc);
								actions.Add(callback);
							}
						}
						else
						{
							CCLog.Log("Unexpected empty selector.");
						}
					}
				}
			}
			if (actions.Capacity < 1)
				return null;

			return new CCSequence(actions.ToArray());
		}

		public Object ActionForSoundChannel(CCBSequenceProperty channel)
		{
			float lastKeyframeTime = 0;

			var actions = new List<CCFiniteTimeAction>();
			var keyframes = channel.Keyframes;
			int numKeyframes = keyframes.Count;

			for (int i = 0; i < numKeyframes; ++i)
			{

				CCBKeyframe keyframe = keyframes[i];
				float timeSinceLastKeyframe = keyframe.Time - lastKeyframeTime;
				lastKeyframeTime = keyframe.Time;
				if (timeSinceLastKeyframe > 0)
				{
					actions.Add(new CCDelayTime(timeSinceLastKeyframe));
				}

				var keyVal = (List<CCBValue>)keyframe.Value;
				string soundFile = keyVal[0].GetStringValue();

				float pitch, pan, gain;

				pitch = float.Parse(keyVal[1].GetStringValue());

				pan = float.Parse(keyVal[2].GetStringValue());

				gain = float.Parse(keyVal[3].GetStringValue());

				actions.Add(new CCBSoundEffect(soundFile, pitch, pan, gain));
			}

			if (actions.Count < 1)
				return null;

			return new CCSequence(actions.ToArray());
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
					actions.Add(new CCDelayTime(timeFirst));
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
					CCFiniteTimeAction seq = new CCSequence(actions.ToArray());
					node.RunAction(seq);
				}
				else
				{
					node.RunAction(actions[0]);
				}
			}
		}

		public void RunAnimations(string pName, float fTweenDuration)
		{
			RunAnimationsForSequenceNamedTweenDuration(pName, fTweenDuration);
		}

		public void RunAnimations(string pName)
		{
			RunAnimationsForSequenceNamed(pName);
		}

		public void RunAnimations(int nSeqId, float fTweenDuraiton)
		{
			RunAnimationsForSequenceIdTweenDuration(nSeqId, fTweenDuraiton);
		}

		public void RunAnimationsForSequenceIdTweenDuration(int nSeqId, float fTweenDuration)
		{
			Debug.Assert(nSeqId != -1, "Sequence id couldn't be found");

			_rootNode.StopAllActions();

			foreach (var pElement in _nodeSequences)
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
				if (_baseValues.TryGetValue(node, out nodeBaseValues))
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
			CCAction completeAction = new CCSequence(
				                                   new CCDelayTime(seq.Duration + fTweenDuration),
				                                   new CCCallFunc(SequenceCompleted)
			                                   );

			_rootNode.RunAction(completeAction);

			// Set the running scene

			if (seq.CallBackChannel != null)
			{
				CCAction action = (CCAction)ActionForCallbackChannel(seq.CallBackChannel);
				if (action != null)
				{
					_rootNode.RunAction(action);
				}
			}

			if (seq.SoundChannel != null)
			{
				CCAction action = (CCAction)ActionForSoundChannel(seq.SoundChannel);
				if (action != null)
				{
					_rootNode.RunAction(action);
				}
			}

			_runningSequence = GetSequence(nSeqId);
		}

		public void RunAnimationsForSequenceNamedTweenDuration(string pName, float fTweenDuration)
		{
			int seqId = GetSequenceId(pName);
			RunAnimationsForSequenceIdTweenDuration(seqId, fTweenDuration);
		}

		public void RunAnimationsForSequenceNamed(string pName)
		{
			RunAnimationsForSequenceNamedTweenDuration(pName, 0);
		}

		public void SetAnimationCompletedCallback(Action callbackFunc)
		{
			_animationCompleteCallbackFunc = callbackFunc;
		}

		// Commented out for now as it does not seem to be used
		//public void Debug()
		//{
		//}

		public void SetCallFunc(CCAction callFunc, string callbackNamed)
		{
			_keyframeCallFuncs.Add(callbackNamed, callFunc);
		}

		private void SequenceCompleted()
		{

			string runningSequenceName = _runningSequence.Name;
			int nextSeqId = _runningSequence.ChainedSequenceId;
			_runningSequence = null;

			if (_lastCompletedSequenceName != runningSequenceName)
			{
				_lastCompletedSequenceName = runningSequenceName;
			}

			if (_delegate != null)
			{
				_delegate(_runningSequence.Name);
			}

			if (_animationCompleteCallbackFunc != null)
			{
				_animationCompleteCallbackFunc();
			}


			if (nextSeqId != -1)
			{
				RunAnimations(nextSeqId, 0);
			}
		}
	}

	public class CCBSetSpriteFrame : CCActionInstant
	{
		internal CCSpriteFrame SpriteFrame { get; set; }

		/** creates a Place action with a position */

		//        public CCBSetSpriteFrame()
		//        {
		//        }

		public CCBSetSpriteFrame(CCSpriteFrame pSpriteFrame)
		{

			SpriteFrame = pSpriteFrame;
		}

		/// <summary>
		/// Start the show operation on the given target.
		/// </summary>
		/// <param name="target"></param>
		protected override CCActionState StartAction(CCNode target)
		{
			return new CCBSetSpriteFrameState(this, target);

		}
		//        public override void Update(float time)
		//        {
		//			((CCSprite) m_pTarget).DisplayFrame = SpriteFrame;
		//        }
		//

		public override CCFiniteTimeAction Reverse()
		{
			return this;
		}

	}

	internal class CCBSetSpriteFrameState : CCActionInstantState
	{
		private CCSpriteFrame SpriteFrame { get; set; }

		public CCBSetSpriteFrameState(CCBSetSpriteFrame action, CCNode target)
			: base(action, target)
		{	
			SpriteFrame = action.SpriteFrame;
		}

		protected override void Update(float time)
		{
			((CCSprite)Target).DisplayFrame = SpriteFrame;
		}
	}

	public class CCBSoundEffect : CCActionInstant
	{

		public string SoundFile { get; private set; }

		public float Pitch { get; private set; }

		public float Pan { get; private set; }

		public float Gain { get; private set; }

		public CCBSoundEffect(string file, float pitch, float pan, float gain)
		{
			SoundFile = file;
			Pitch = pitch;
			Pan = pan;
			Gain = gain;
		}

		/// <summary>
		/// Start the CCBRotateTo operation on the given target.
		/// </summary>
		/// <param name="target"></param>
		protected override CCActionState StartAction(CCNode target)
		{
			return new CCBSoundEffectState(this, target);

		}

		public override CCFiniteTimeAction Reverse()
		{
			return this;
		}

		private class CCBSoundEffectState : CCActionInstantState
		{
			protected string SoundFile { get; set; }

			protected float Pitch { get; set; }

			protected float Pan { get; set; }

			protected float Gain { get; set; }

			public CCBSoundEffectState(CCBSoundEffect action, CCNode target)
				: base(action, target)
			{	
				SoundFile = action.SoundFile;
				Pitch = action.Pitch;
				Pan = action.Pan;
				Gain = action.Gain;
			}

			protected override void Update(float time)
			{
				CCSimpleAudioEngine.SharedEngine.PlayEffect(SoundFile);
			}

		}


	}

	public class CCBRotateTo : CCActionInterval
	{
		internal float DstAngle { get; private set; }

		#region Constructors

		public CCBRotateTo(float duration, float angle)
			: base(duration)
		{
			DstAngle = angle;
		}

		#endregion Constructors

		/// <summary>
		/// Start the CCBRotateTo operation on the given target.
		/// </summary>
		/// <param name="target"></param>
		protected override CCActionState StartAction(CCNode target)
		{
			return new CCBRotateToState(this, target);

		}

		public override CCFiniteTimeAction Reverse()
		{
			Debug.Assert(false, "reverse() is not supported in CCBRotateTo");
			return null;
		}

		private class CCBRotateToState : CCActionIntervalState
		{
			float DstAngle { get; set; }

			CCPoint diffAngle;
			CCPoint startAngle;

			public CCBRotateToState(CCBRotateTo action, CCNode target)
				: base(action, target)
			{	
				DstAngle = action.DstAngle;
				startAngle = new CCPoint(Target.RotationX, Target.RotationY);
				diffAngle = new CCPoint(DstAngle - startAngle.X, DstAngle - startAngle.Y);
			}

			protected override void Update(float time)
			{
				Target.RotationX = startAngle.X + (diffAngle.X * time);
				Target.RotationY = startAngle.Y + (diffAngle.Y * time);
			}

		}

	}


	public class CCBRotateXTo : CCActionInterval
	{

		internal float DstAngle { get; set; }

		#region Constructors


		public CCBRotateXTo(float duration, float angle)
			: base(duration)
		{
			DstAngle = angle;
		}

		#endregion Constructors

		/// <summary>
		/// Start the CCBRotateXTo operation on the given target.
		/// </summary>
		/// <param name="target"></param>
		protected override CCActionState StartAction(CCNode target)
		{
			return new CCBRotateXToState(this, target);

		}

		public override CCFiniteTimeAction Reverse()
		{
			Debug.Assert(false, "reverse() is not supported in CCBRotateTo");
			return null;
		}

		private class CCBRotateXToState : CCActionIntervalState
		{
			private float DstAngle { get; set; }

			private float diffAngle;
			private float startAngle;

			public CCBRotateXToState(CCBRotateXTo action, CCNode target)
				: base(action, target)
			{	
				DstAngle = action.DstAngle;
				startAngle = Target.RotationX;
				diffAngle = DstAngle - startAngle;
			}

			protected override void Update(float time)
			{
				Target.RotationX = startAngle + (diffAngle * time);
			}
		}

	}

	public class CCBRotateYTo : CCActionInterval
	{

		internal float DstAngle { get; set; }

		#region Constructors

		public CCBRotateYTo(float duration, float angle)
			: base(duration)
		{
			DstAngle = angle;
		}

		#endregion Constructors

		/// <summary>
		/// Start the CCBRotateYTo operation on the given target.
		/// </summary>
		/// <param name="target"></param>
		protected override CCActionState StartAction(CCNode target)
		{
			return new CCBRotateYToState(this, target);

		}

		public override CCFiniteTimeAction Reverse()
		{
			Debug.Assert(false, "reverse() is not supported in CCBRotateYTo");
			return null;
		}


		private class CCBRotateYToState : CCActionIntervalState
		{
			private float DstAngle { get; set; }

			private float diffAngle;
			private float startAngle;

			public CCBRotateYToState(CCBRotateYTo action, CCNode target)
				: base(action, target)
			{	
				DstAngle = action.DstAngle;
				startAngle = Target.RotationY;
				diffAngle = DstAngle - startAngle;
			}

			protected override void Update(float time)
			{
				Target.RotationY = startAngle + (diffAngle * time);
			}
		}

	}

	class CCBEaseInstant : CCActionEase
	{
		#region Constructors

		public CCBEaseInstant(CCActionInterval action)
			: base(action)
		{
		}

		#endregion Constructors

		/// <summary>
		/// Start the EaseInstant operation on the given target.
		/// </summary>
		/// <param name="target"></param>
		protected override CCActionState StartAction(CCNode target)
		{
			return new CCBEaseInstantState(this, target);

		}

		public override CCFiniteTimeAction Reverse()
		{
			return new CCBEaseInstant((CCActionInterval)InnerAction.Reverse());
		}

		private class CCBEaseInstantState : CCActionEaseState
		{

			public CCBEaseInstantState(CCBEaseInstant action, CCNode target)
				: base(action, target)
			{
			}

			protected override void Update(float time)
			{
				if (time < 0)
				{
					((CCBEaseInstantState)InnerActionState).Update(0);
				}
				else
				{
					((CCBEaseInstantState)InnerActionState).Update(1);
				}
			}
		}
	}

}