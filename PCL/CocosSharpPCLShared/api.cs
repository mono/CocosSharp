using System;
using System.ComponentModel;

namespace CocosSharp {
  public static partial class ArrayPool<T> {
    // Methods
    public static T[] Create(int length, bool pow=true) { return default(T[]); }
    public static void Free(T[] array) { }
    public static void Resize(ref T[] array, int length, bool pow=true) { }
  }

  public partial class BlockCCControlData {
    // Fields
    public CocosSharp.CCControlEvent mControlEvents;
    public System.Action<System.Object, CocosSharp.CCControlEvent> mSELCCControlHandler;
    public object mTarget;
     
    // Constructors
    public BlockCCControlData() { }
  }
  public partial class BlockData {
    // Fields
    public System.Action<System.Object> mSELMenuHandler;
    public object mTarget;
     
    // Constructors
    public BlockData() { }
  }
  public partial class CCAccelAmplitude : CocosSharp.CCActionInterval {
    // Constructors
    public CCAccelAmplitude(CocosSharp.CCAmplitudeAction pAction, float duration, float accelRate=1f) { }
     
    // Properties
    protected internal CocosSharp.CCAmplitudeAction OtherAction { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCAmplitudeAction); } }
    public float Rate { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } }
     
    // Methods
    public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCAccelAmplitudeState : CocosSharp.CCActionIntervalState {
    // Constructors
    public CCAccelAmplitudeState(CocosSharp.CCAccelAmplitude action, CocosSharp.CCNode target) : base (default(CocosSharp.CCActionInterval), default(CocosSharp.CCNode)) { }
     
    // Properties
    protected CocosSharp.CCAmplitudeActionState OtherActionState { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCAmplitudeActionState); } }
    public float Rate { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
     
    // Methods
    public override void Update(float time) { }
  }
  public partial class CCAccelDeccelAmplitude : CocosSharp.CCAccelAmplitude {
    // Constructors
    public CCAccelDeccelAmplitude(CocosSharp.CCAmplitudeAction pAction, float duration, float accDeccRate=1f) : base (default(CocosSharp.CCAmplitudeAction), default(float), default(float)) { }
     
    // Methods
    public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCAccelDeccelAmplitudeState : CocosSharp.CCAccelAmplitudeState {
    // Constructors
    public CCAccelDeccelAmplitudeState(CocosSharp.CCAccelDeccelAmplitude action, CocosSharp.CCNode target) : base (default(CocosSharp.CCAccelAmplitude), default(CocosSharp.CCNode)) { }
     
    // Methods
    public override void Update(float time) { }
  }
  public partial class CCAcceleration {
    // Fields
    public double TimeStamp;
    public double X;
    public double Y;
    public double Z;
     
    // Constructors
    public CCAcceleration() { }
  }
  public partial class CCAccelerometer {
    // Constructors
    public CCAccelerometer() { }
     
    // Methods
    public void Update() { }
  }
  public partial class CCAction {
    // Constructors
    public CCAction() { }
     
    // Properties
    public CocosSharp.CCNode OriginalTarget { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCNode); } }
    public int Tag { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public CocosSharp.CCNode Target { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCNode); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]protected set { } }
     
    // Methods
    protected internal virtual CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCActionCamera : CocosSharp.CCActionInterval {
    // Constructors
    protected CCActionCamera(float duration) { }
     
    // Methods
    public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCActionCameraState : CocosSharp.CCActionIntervalState {
    // Fields
    protected float CenterXOrig;
    protected float CenterYOrig;
    protected float CenterZOrig;
    protected float EyeXOrig;
    protected float EyeYOrig;
    protected float EyeZOrig;
    protected float UpXOrig;
    protected float UpYOrig;
    protected float UpZOrig;
     
    // Constructors
    public CCActionCameraState(CocosSharp.CCActionCamera action, CocosSharp.CCNode target) : base (default(CocosSharp.CCActionInterval), default(CocosSharp.CCNode)) { }
  }
  public partial class CCActionEase : CocosSharp.CCActionInterval {
    // Constructors
    protected CCActionEase() { }
    public CCActionEase(CocosSharp.CCActionInterval pAction) { }
     
    // Properties
    protected internal CocosSharp.CCActionInterval InnerAction { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCActionInterval); } }
     
    // Methods
    public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCActionEaseState : CocosSharp.CCActionIntervalState {
    // Constructors
    public CCActionEaseState(CocosSharp.CCActionEase action, CocosSharp.CCNode target) : base (default(CocosSharp.CCActionInterval), default(CocosSharp.CCNode)) { }
     
    // Properties
    protected CocosSharp.CCActionIntervalState InnerActionState { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCActionIntervalState); } }
     
    // Methods
    public override void Stop() { }
    public override void Update(float time) { }
  }
  public partial class CCActionInstant : CocosSharp.CCFiniteTimeAction {
    // Constructors
    protected CCActionInstant() { }
    protected CCActionInstant(float d) { }
     
    // Methods
    public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCActionInstantState : CocosSharp.CCFiniteTimeActionState {
    // Constructors
    public CCActionInstantState(CocosSharp.CCActionInstant action, CocosSharp.CCNode target) : base (default(CocosSharp.CCFiniteTimeAction), default(CocosSharp.CCNode)) { }
     
    // Properties
    public override bool IsDone { get { return default(bool); } }
     
    // Methods
    public override void Step(float dt) { }
    public override void Update(float time) { }
  }
  public partial class CCActionInterval : CocosSharp.CCFiniteTimeAction {
    // Fields
    protected bool FirstTick;
     
    // Constructors
    protected CCActionInterval() { }
    public CCActionInterval(float d) { }
     
    // Properties
    public override float Duration { get { return default(float); } set { } }
    public float Elapsed { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]protected set { } }
     
    // Methods
    public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCActionIntervalState : CocosSharp.CCFiniteTimeActionState {
    // Constructors
    public CCActionIntervalState(CocosSharp.CCActionInterval action, CocosSharp.CCNode target) : base (default(CocosSharp.CCFiniteTimeAction), default(CocosSharp.CCNode)) { }
     
    // Properties
    public float Elapsed { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } }
    protected bool FirstTick { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } }
    public override bool IsDone { get { return default(bool); } }
     
    // Methods
    public override void Step(float dt) { }
  }
  public partial class CCActionManager : CocosSharp.ICCUpdatable, System.IDisposable {
    // Constructors
    public CCActionManager() { }
     
    // Methods
    protected void ActionAllocWithHashElement(CocosSharp.CCActionManager.HashElement element) { }
    public CocosSharp.CCActionState AddAction(CocosSharp.CCAction action, CocosSharp.CCNode target, bool paused=false) { return default(CocosSharp.CCActionState); }
    protected void DeleteHashElement(CocosSharp.CCActionManager.HashElement element) { }
    public void Dispose() { }
    protected virtual void Dispose(bool disposing) { }
    ~CCActionManager() { }
    public CocosSharp.CCAction GetActionByTag(int tag, CocosSharp.CCNode target) { return default(CocosSharp.CCAction); }
    public int NumberOfRunningActionsInTarget(CocosSharp.CCNode target) { return default(int); }
    public System.Collections.Generic.List<System.Object> PauseAllRunningActions() { return default(System.Collections.Generic.List<System.Object>); }
    public void PauseTarget(object target) { }
    public void RemoveAction(CocosSharp.CCAction action) { }
    public void RemoveAction(CocosSharp.CCActionState actionState) { }
    protected void RemoveActionAtIndex(int index, CocosSharp.CCActionManager.HashElement element) { }
    public void RemoveActionByTag(int tag, CocosSharp.CCNode target) { }
    public void RemoveAllActions() { }
    public void RemoveAllActionsFromTarget(CocosSharp.CCNode target) { }
    public void ResumeTarget(object target) { }
    public void ResumeTargets(System.Collections.Generic.List<System.Object> targetsToResume) { }
    public void Update(float dt) { }
     
    // Nested Types
    protected partial class HashElement {
      // Fields
      public int ActionIndex;
      public System.Collections.Generic.List<CocosSharp.CCAction> Actions;
      public System.Collections.Generic.List<CocosSharp.CCActionState> ActionStates;
      public CocosSharp.CCAction CurrentAction;
      public bool CurrentActionSalvaged;
      public CocosSharp.CCActionState CurrentActionState;
      public bool Paused;
      public object Target;
       
      // Constructors
      public HashElement() { }
    }
  }
  public partial class CCActionState {
    // Constructors
    public CCActionState(CocosSharp.CCAction action, CocosSharp.CCNode target) { }
     
    // Properties
    public CocosSharp.CCAction Action { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCAction); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]protected set { } }
    public virtual bool IsDone { get { return default(bool); } }
    public CocosSharp.CCNode OriginalTarget { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCNode); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]protected set { } }
    public CocosSharp.CCNode Target { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCNode); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]protected set { } }
     
    // Methods
    public virtual void Step(float dt) { }
    public virtual void Stop() { }
    public virtual void Update(float time) { }
  }
  public enum CCActionTag {
    // Fields
    Invalid = -1,
  }
  public partial class CCActionTween : CocosSharp.CCActionInterval {
    // Constructors
    public CCActionTween(float aDuration, string key, float from, float to) { }
    public CCActionTween(float aDuration, string key, float from, float to, System.Action<System.Single, System.String> tweenAction) { }
     
    // Properties
    public float From { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } }
    public string Key { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(string); } }
    public float To { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } }
    public System.Action<System.Single, System.String> TweenAction { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Action<System.Single, System.String>); } }
     
    // Methods
    public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCActionTweenState : CocosSharp.CCActionIntervalState {
    // Fields
    protected float Delta;
     
    // Constructors
    public CCActionTweenState(CocosSharp.CCActionTween action, CocosSharp.CCNode target) : base (default(CocosSharp.CCActionInterval), default(CocosSharp.CCNode)) { }
     
    // Properties
    protected float From { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } }
    protected string Key { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(string); } }
    protected float To { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } }
    protected System.Action<System.Single, System.String> TweenAction { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Action<System.Single, System.String>); } }
     
    // Methods
    public override void Update(float dt) { }
  }
  [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
  public partial struct CCAffineTransform {
    // Fields
    public float a;
    public float b;
    public float c;
    public float d;
    public static readonly CocosSharp.CCAffineTransform Identity;
    public float tx;
    public float ty;
     
    // Constructors
    public CCAffineTransform(float a, float b, float c, float d, float tx, float ty) { throw new System.NotImplementedException(); }
     
    // Properties
    public float TranslationX { get { return default(float); } set { } }
    public float TranslationY { get { return default(float); } set { } }
     
    // Methods
    public void Concat(CocosSharp.CCAffineTransform m) { }
    public static CocosSharp.CCAffineTransform Concat(CocosSharp.CCAffineTransform t1, CocosSharp.CCAffineTransform t2) { return default(CocosSharp.CCAffineTransform); }
    public void Concat(ref CocosSharp.CCAffineTransform m) { }
    public void ConcatScale(float xscale, float yscale) { }
    public void ConcatTranslation(float x, float y) { }
    public static bool Equal(CocosSharp.CCAffineTransform t1, CocosSharp.CCAffineTransform t2) { return default(bool); }
    public bool Equals(ref CocosSharp.CCAffineTransform t) { return default(bool); }
    public override bool Equals(object obj) { return default(bool); }
    public override int GetHashCode() { return default(int); }
    public float GetRotation() { return default(float); }
    public float GetRotationX() { return default(float); }
    public float GetRotationY() { return default(float); }
    public float GetScaleX() { return default(float); }
    public float GetScaleY() { return default(float); }
    public static CocosSharp.CCAffineTransform Invert(CocosSharp.CCAffineTransform t) { return default(CocosSharp.CCAffineTransform); }
    public static void Lerp(ref CocosSharp.CCAffineTransform m1, ref CocosSharp.CCAffineTransform m2, float t, out CocosSharp.CCAffineTransform res) { res = default(CocosSharp.CCAffineTransform); }
    public static CocosSharp.CCAffineTransform operator +(CocosSharp.CCAffineTransform affineTransform1, CocosSharp.CCAffineTransform affineTransform2) { return default(CocosSharp.CCAffineTransform); }
    public static CocosSharp.CCAffineTransform operator /(CocosSharp.CCAffineTransform affineTransform1, CocosSharp.CCAffineTransform affineTransform2) { return default(CocosSharp.CCAffineTransform); }
    public static CocosSharp.CCAffineTransform operator /(CocosSharp.CCAffineTransform affineTransform, float divider) { return default(CocosSharp.CCAffineTransform); }
    public static bool operator ==(CocosSharp.CCAffineTransform affineTransform1, CocosSharp.CCAffineTransform affineTransform2) { return default(bool); }
    public static bool operator !=(CocosSharp.CCAffineTransform affineTransform1, CocosSharp.CCAffineTransform affineTransform2) { return default(bool); }
    public static CocosSharp.CCAffineTransform operator *(CocosSharp.CCAffineTransform affinematrix1, CocosSharp.CCAffineTransform affinematrix2) { return default(CocosSharp.CCAffineTransform); }
    public static CocosSharp.CCAffineTransform operator -(CocosSharp.CCAffineTransform affineTransform1, CocosSharp.CCAffineTransform affineTransform2) { return default(CocosSharp.CCAffineTransform); }
    public static CocosSharp.CCAffineTransform operator -(CocosSharp.CCAffineTransform affineTransform1) { return default(CocosSharp.CCAffineTransform); }
    public static CocosSharp.CCAffineTransform Rotate(CocosSharp.CCAffineTransform t, float anAngle) { return default(CocosSharp.CCAffineTransform); }
    public static CocosSharp.CCAffineTransform Scale(CocosSharp.CCAffineTransform t, float sx, float sy) { return default(CocosSharp.CCAffineTransform); }
    public void SetLerp(CocosSharp.CCAffineTransform m1, CocosSharp.CCAffineTransform m2, float t) { }
    public void SetRotation(float rotation) { }
    public void SetScale(float scaleX, float scaleY) { }
    public void SetScaleRotation(float scaleX, float scaleY, float angle) { }
    public void SetScaleX(float scaleX) { }
    public void SetScaleY(float scaleY) { }
    public void SetTranslation(float x, float y) { }
    public CocosSharp.CCPoint Transform(CocosSharp.CCPoint point) { return default(CocosSharp.CCPoint); }
    public static CocosSharp.CCPoint Transform(CocosSharp.CCPoint point, CocosSharp.CCAffineTransform t) { return default(CocosSharp.CCPoint); }
    public void Transform(ref CocosSharp.CCPoint point) { }
    public CocosSharp.CCRect Transform(CocosSharp.CCRect rect) { return default(CocosSharp.CCRect); }
    public static CocosSharp.CCRect Transform(CocosSharp.CCRect rect, CocosSharp.CCAffineTransform anAffineTransform) { return default(CocosSharp.CCRect); }
    public void Transform(ref CocosSharp.CCRect rect) { }
    public static CocosSharp.CCSize Transform(CocosSharp.CCSize size, CocosSharp.CCAffineTransform t) { return default(CocosSharp.CCSize); }
    public void Transform(ref int x, ref int y) { }
    public void Transform(float x, float y, out float xresult, out float yresult) { xresult = default(float); yresult = default(float); }
    public void Transform(ref float x, ref float y) { }
    public static CocosSharp.CCAffineTransform Translate(CocosSharp.CCAffineTransform t, float tx, float ty) { return default(CocosSharp.CCAffineTransform); }
  }
  public abstract partial class CCAmplitudeAction : CocosSharp.CCActionInterval {
    // Constructors
    public CCAmplitudeAction(float duration, float amplitude=0f) { }
     
    // Properties
    public float Amplitude { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } }
     
    // Methods
  }
  public abstract partial class CCAmplitudeActionState : CocosSharp.CCActionIntervalState {
    // Constructors
    public CCAmplitudeActionState(CocosSharp.CCAmplitudeAction action, CocosSharp.CCNode target) : base (default(CocosSharp.CCActionInterval), default(CocosSharp.CCNode)) { }
     
    // Properties
    protected float Amplitude { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } }
    protected internal float AmplitudeRate { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
     
    // Methods
  }
  public partial class CCAnimate : CocosSharp.CCActionInterval {
    // Constructors
    public CCAnimate(CocosSharp.CCAnimation pAnimation) { }
     
    // Properties
    public CocosSharp.CCAnimation Animation { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCAnimation); } }
    public System.Collections.Generic.List<System.Single> SplitTimes { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Collections.Generic.List<System.Single>); } }
     
    // Methods
    public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCAnimateState : CocosSharp.CCActionIntervalState {
    // Fields
    protected int NextFrame;
    protected CocosSharp.CCSpriteFrame OriginalFrame;
     
    // Constructors
    public CCAnimateState(CocosSharp.CCAnimate action, CocosSharp.CCNode target) : base (default(CocosSharp.CCActionInterval), default(CocosSharp.CCNode)) { }
     
    // Properties
    protected CocosSharp.CCAnimation Animation { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCAnimation); } }
    protected System.Collections.Generic.List<System.Single> SplitTimes { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Collections.Generic.List<System.Single>); } }
     
    // Methods
    public override void Stop() { }
    public override void Update(float t) { }
  }
  public partial class CCAnimation {
    // Fields
    protected bool m_bRestoreOriginalFrame;
    protected float m_fDelayPerUnit;
    protected float m_fTotalDelayUnits;
    protected System.Collections.Generic.List<CocosSharp.CCAnimationFrame> m_pFrames;
    protected uint m_uLoops;
     
    // Constructors
    public CCAnimation() { }
    protected CCAnimation(CocosSharp.CCAnimation animation) { }
    public CCAnimation(CocosSharp.CCSpriteSheet cs, float delay) { }
    public CCAnimation(CocosSharp.CCSpriteSheet cs, System.String[] frames, float delay) { }
    public CCAnimation(System.Collections.Generic.List<CocosSharp.CCAnimationFrame> arrayOfAnimationFrameNames, float delayPerUnit, uint loops) { }
    public CCAnimation(System.Collections.Generic.List<CocosSharp.CCSpriteFrame> frames, float delay) { }
     
    // Properties
    public float DelayPerUnit { get { return default(float); } set { } }
    public float Duration { get { return default(float); } }
    public System.Collections.Generic.List<CocosSharp.CCAnimationFrame> Frames { get { return default(System.Collections.Generic.List<CocosSharp.CCAnimationFrame>); } }
    public uint Loops { get { return default(uint); } set { } }
    public bool RestoreOriginalFrame { get { return default(bool); } set { } }
    public float TotalDelayUnits { get { return default(float); } }
     
    // Methods
    public void AddSprite(CocosSharp.CCSprite sprite) { }
    public void AddSpriteFrame(CocosSharp.CCSpriteFrame pFrame) { }
    public void AddSpriteFrameWithFileName(string pszFileName) { }
    public void AddSpriteFrameWithTexture(CocosSharp.CCTexture2D pobTexture, CocosSharp.CCRect rect) { }
    public CocosSharp.CCAnimation Copy() { return default(CocosSharp.CCAnimation); }
  }
  public partial class CCAnimationCache {
    // Constructors
    protected CCAnimationCache() { }
     
    // Properties
    public CocosSharp.CCAnimation this[string index] { get { return default(CocosSharp.CCAnimation); } set { } }
    public static CocosSharp.CCAnimationCache SharedAnimationCache { get { return default(CocosSharp.CCAnimationCache); } }
     
    // Methods
    public void AddAnimation(CocosSharp.CCAnimation animation, string name) { }
    public void AddAnimationsWithDictionary(CocosSharp.PlistDictionary dictionary) { }
    public void AddAnimationsWithFile(string plist) { }
    public CocosSharp.CCAnimation AnimationByName(string name) { return default(CocosSharp.CCAnimation); }
    public static void PurgeSharedAnimationCache() { }
    public void RemoveAnimationByName(string name) { }
  }
  public partial class CCAnimationFrame {
    // Constructors
    protected CCAnimationFrame(CocosSharp.CCAnimationFrame animFrame) { }
    public CCAnimationFrame(CocosSharp.CCSpriteFrame spriteFrame, float delayUnits, CocosSharp.PlistDictionary userInfo) { }
     
    // Properties
    public float DelayUnits { get { return default(float); } }
    public CocosSharp.CCSpriteFrame SpriteFrame { get { return default(CocosSharp.CCSpriteFrame); } }
    public CocosSharp.PlistDictionary UserInfo { get { return default(CocosSharp.PlistDictionary); } }
     
    // Methods
    public CocosSharp.CCAnimationFrame Copy() { return default(CocosSharp.CCAnimationFrame); }
  }
 
  public partial class CCArray {
    // Fields
    public System.Int32[] arr;
    public int max;
    public int num;
     
    // Constructors
    public CCArray(int capacity) { }
     
    // Methods
    public static void DoubleCapacity(CocosSharp.CCArray arr) { }
    public static void InsertValueAtIndex(CocosSharp.CCArray arr, int value, int index) { }
    public static void RemoveValueAtIndex(CocosSharp.CCArray arr, int index) { }
  }
  public partial class CCArrayForObjectSorting : System.Collections.Generic.List<System.Object> {
    // Fields
    public const int CC_INVALID_INDEX = -1;
     
    // Constructors
    public CCArrayForObjectSorting() { }
     
    // Methods
    public int IndexOfSortedObject(CocosSharp.ICCSortableObject obj) { return default(int); }
    public void InsertSortedObject(CocosSharp.ICCSortableObject obj) { }
    public CocosSharp.ICCSortableObject ObjectWithObjectID(int tag) { return default(CocosSharp.ICCSortableObject); }
    public void RemoveSortedObject(CocosSharp.ICCSortableObject obj) { }
    public void SetObjectID_ofSortedObject(int tag, CocosSharp.ICCSortableObject obj) { }
  }
  public partial class CCAtlasNode : CocosSharp.CCNodeRGBA, CocosSharp.ICCBlendable, CocosSharp.ICCTexture {
    // Constructors
    public CCAtlasNode(CocosSharp.CCTexture2D texture, int tileWidth, int tileHeight, int itemsToRender) { }
    public CCAtlasNode(string tile, int tileWidth, int tileHeight, int itemsToRender) { }
     
    // Properties
    public CocosSharp.CCBlendFunc BlendFunc { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCBlendFunc); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public override CocosSharp.CCColor3B Color { get { return default(CocosSharp.CCColor3B); } set { } }
    protected CocosSharp.CCColor3B ColorUnmodified { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCColor3B); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    protected internal bool IgnoreContentScaleFactor { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public bool IsAntialiased { get { return default(bool); } set { } }
    public override bool IsColorModifiedByOpacity { get { return default(bool); } set { } }
    protected bool IsOpacityModifyRGB { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    protected int ItemHeight { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    protected int ItemsPerColumn { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    protected int ItemsPerRow { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    protected int ItemWidth { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public override byte Opacity { get { return default(byte); } set { } }
    protected int QuadsToDraw { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public virtual CocosSharp.CCTexture2D Texture { get { return default(CocosSharp.CCTexture2D); } set { } }
    public CocosSharp.CCTextureAtlas TextureAtlas { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCTextureAtlas); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    protected int UniformColor { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
     
    // Methods
    protected override void Draw() { }
    public virtual void UpdateAtlasValues() { }
  }
  public partial class CCBAnimationManager {
    // Fields
    public bool _jsControlled;
    public object _owner;
     
    // Constructors
    public CCBAnimationManager() { }
     
    // Properties
    public int AutoPlaySequenceId { get { return default(int); } set { } }
    public CocosSharp.CCBAnimationManagerDelegate Delegate { get { return default(CocosSharp.CCBAnimationManagerDelegate); } set { } }
    public string DocumentControllerName { get { return default(string); } set { } }
    public CocosSharp.CCSize RootContainerSize { get { return default(CocosSharp.CCSize); } set { } }
    public CocosSharp.CCNode RootNode { get { return default(CocosSharp.CCNode); } set { } }
    public string RunningSequenceName { get { return default(string); } }
    public System.Collections.Generic.List<CocosSharp.CCBSequence> Sequences { get { return default(System.Collections.Generic.List<CocosSharp.CCBSequence>); } set { } }
     
    // Methods
    public object ActionForCallbackChannel(CocosSharp.CCBSequenceProperty channel) { return default(object); }
    public object ActionForSoundChannel(CocosSharp.CCBSequenceProperty channel) { return default(object); }
    public void AddDocumentCallbackName(string name) { }
    public void AddDocumentCallbackNode(CocosSharp.CCNode node) { }
    public void AddDocumentOutletName(string name) { }
    public void AddDocumentOutletNode(CocosSharp.CCNode node) { }
    public void AddNode(CocosSharp.CCNode node, System.Collections.Generic.Dictionary<System.Int32, System.Collections.Generic.Dictionary<System.String, CocosSharp.CCBSequenceProperty>> pSeq) { }
    public CocosSharp.CCSize GetContainerSize(CocosSharp.CCNode node) { return default(CocosSharp.CCSize); }
    public System.Collections.Generic.List<System.String> GetDocumentCallbackNames() { return default(System.Collections.Generic.List<System.String>); }
    public System.Collections.Generic.List<CocosSharp.CCNode> GetDocumentCallbackNodes() { return default(System.Collections.Generic.List<CocosSharp.CCNode>); }
    public System.Collections.Generic.List<System.String> GetDocumentOutletNames() { return default(System.Collections.Generic.List<System.String>); }
    public System.Collections.Generic.List<CocosSharp.CCNode> GetDocumentOutletNodes() { return default(System.Collections.Generic.List<CocosSharp.CCNode>); }
    public System.Collections.Generic.List<System.String> GetKeyframeCallbacks() { return default(System.Collections.Generic.List<System.String>); }
    public string GetLastCompletedSequenceName() { return default(string); }
    public virtual bool Init() { return default(bool); }
    public void MoveAnimationsFromNode(CocosSharp.CCNode fromNode, CocosSharp.CCNode toNode) { }
    public void RunAnimations(int nSeqId, float fTweenDuraiton) { }
    public void RunAnimations(string pName) { }
    public void RunAnimations(string pName, float fTweenDuration) { }
    public void RunAnimationsForSequenceIdTweenDuration(int nSeqId, float fTweenDuration) { }
    public void RunAnimationsForSequenceNamed(string pName) { }
    public void RunAnimationsForSequenceNamedTweenDuration(string pName, float fTweenDuration) { }
    public void SetAnimationCompletedCallback(System.Action callbackFunc) { }
    public void SetBaseValue(object pValue, CocosSharp.CCNode node, string pPropName) { }
    public void SetCallFunc(CocosSharp.CCAction callFunc, string callbackNamed) { }
  }
  public delegate void CCBAnimationManagerDelegate(string name);
  public enum CCBEasingType {
    // Fields
    BackIn = 11,
    BackInOut = 13,
    BackOut = 12,
    BounceIn = 8,
    BounceInOut = 10,
    BounceOut = 9,
    CubicIn = 2,
    CubicInOut = 4,
    CubicOut = 3,
    ElasticIn = 5,
    ElasticInOut = 7,
    ElasticOut = 6,
    Instant = 0,
    Linear = 1,
  }
  public partial class CCBezierBy : CocosSharp.CCActionInterval {
    // Constructors
    public CCBezierBy(float t, CocosSharp.CCBezierConfig config) { }
     
    // Properties
    public CocosSharp.CCBezierConfig BezierConfig { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCBezierConfig); } }
     
    // Methods
    public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCBezierByState : CocosSharp.CCActionIntervalState {
    // Constructors
    public CCBezierByState(CocosSharp.CCBezierBy action, CocosSharp.CCNode target) : base (default(CocosSharp.CCActionInterval), default(CocosSharp.CCNode)) { }
     
    // Properties
    protected CocosSharp.CCBezierConfig BezierConfig { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCBezierConfig); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    protected CocosSharp.CCPoint PreviousPosition { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCPoint); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    protected CocosSharp.CCPoint StartPosition { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCPoint); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
     
    // Methods
    public override void Update(float time) { }
  }
  [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
  public partial struct CCBezierConfig {
    // Fields
    public CocosSharp.CCPoint ControlPoint1;
    public CocosSharp.CCPoint ControlPoint2;
    public CocosSharp.CCPoint EndPosition;
  }
  public partial class CCBezierTo : CocosSharp.CCBezierBy {
    // Constructors
    public CCBezierTo(float t, CocosSharp.CCBezierConfig c) : base (default(float), default(CocosSharp.CCBezierConfig)) { }
     
    // Methods
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCBezierToState : CocosSharp.CCBezierByState {
    // Constructors
    public CCBezierToState(CocosSharp.CCBezierBy action, CocosSharp.CCNode target) : base (default(CocosSharp.CCBezierBy), default(CocosSharp.CCNode)) { }
  }
  public partial class CCBFile : CocosSharp.CCNode {
    // Constructors
    public CCBFile() { }
     
    // Properties
    public CocosSharp.CCNode FileNode { get { return default(CocosSharp.CCNode); } set { } }
     
    // Methods
  }
  public static partial class CCBHelper {
    // Methods
    public static CocosSharp.CCPoint GetAbsolutePosition(CocosSharp.CCPoint pt, CocosSharp.CCBPositionType nType, CocosSharp.CCSize containerSize, string pPropName) { return default(CocosSharp.CCPoint); }
    public static void SetRelativeScale(CocosSharp.CCNode node, float fScaleX, float fScaleY, CocosSharp.CCBScaleType nType, string pPropName) { }
  }
  public partial class CCBKeyframe {
    // Fields
    public float EasingOpt;
    public CocosSharp.CCBEasingType EasingType;
    public float Time;
    public object Value;
     
    // Constructors
    public CCBKeyframe() { }
  }
  [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Size=1)]
  public partial struct CCBlendFunc {
    // Fields
    public static readonly CocosSharp.CCBlendFunc Additive;
    public static readonly CocosSharp.CCBlendFunc AlphaBlend;
    public static readonly CocosSharp.CCBlendFunc NonPremultiplied;
    public static readonly CocosSharp.CCBlendFunc Opaque;
     
    // Constructors
    public CCBlendFunc(int src, int dst) { throw new System.NotImplementedException(); }
     
    // Properties
    public int Destination { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public int Source { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
     
    // Methods
    public bool Equals(CocosSharp.CCBlendFunc other) { return default(bool); }
    public override bool Equals(object obj) { return default(bool); }
    public override int GetHashCode() { return default(int); }
    public static bool operator ==(CocosSharp.CCBlendFunc b1, CocosSharp.CCBlendFunc b2) { return default(bool); }
    public static bool operator !=(CocosSharp.CCBlendFunc b1, CocosSharp.CCBlendFunc b2) { return default(bool); }
  }
  public partial class CCBlink : CocosSharp.CCActionInterval {
    // Constructors
    public CCBlink(float duration, uint uBlinks) { }
     
    // Properties
    public uint Times { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(uint); } }
     
    // Methods
    public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCBlinkState : CocosSharp.CCActionIntervalState {
    // Constructors
    public CCBlinkState(CocosSharp.CCBlink action, CocosSharp.CCNode target) : base (default(CocosSharp.CCActionInterval), default(CocosSharp.CCNode)) { }
     
    // Properties
    protected bool OriginalState { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    protected uint Times { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(uint); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
     
    // Methods
    public override void Stop() { }
    public override void Update(float time) { }
  }
  [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Size=1)]
  public partial struct CCBoundingBoxI {
    // Fields
    public static readonly CocosSharp.CCBoundingBoxI Null;
    public static readonly CocosSharp.CCBoundingBoxI Zero;
     
    // Constructors
    public CCBoundingBoxI(int minx, int miny, int maxx, int maxy) { throw new System.NotImplementedException(); }
     
    // Properties
    public int MaxX { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public int MaxY { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public int MinX { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public int MinY { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public CocosSharp.CCSizeI Size { get { return default(CocosSharp.CCSizeI); } }
     
    // Methods
    public void ExpandToCircle(ref CocosSharp.CCPointI point, int radius) { }
    public void ExpandToCircle(int x, int y, int radius) { }
    public void ExpandToPoint(ref CocosSharp.CCPointI point) { }
    public void ExpandToPoint(int x, int y) { }
    public void ExpandToRect(ref CocosSharp.CCBoundingBoxI r) { }
    public bool Intersects(ref CocosSharp.CCBoundingBoxI rect) { return default(bool); }
    public static implicit operator CocosSharp.CCRect (CocosSharp.CCBoundingBoxI box) { return default(CocosSharp.CCRect); }
    public void SetLerp(CocosSharp.CCBoundingBoxI a, CocosSharp.CCBoundingBoxI b, float ratio) { }
    public CocosSharp.CCBoundingBoxI Transform(CocosSharp.CCAffineTransform matrix) { return default(CocosSharp.CCBoundingBoxI); }
  }

  public enum CCBPositionType {
    // Fields
    MultiplyResolution = 5,
    Percent = 4,
    RelativeBottomLeft = 0,
    RelativeBottomRight = 3,
    RelativeTopLeft = 1,
    RelativeTopRight = 2,
  }
  public enum CCBPropertyType {
    // Fields
    Animation = 22,
    Blendmode = 16,
    Block = 21,
    BlockCCControl = 25,
    Byte = 12,
    CCBFile = 23,
    Check = 9,
    Color3 = 13,
    Color4FVar = 14,
    Degrees = 5,
    Flip = 15,
    Float = 7,
    FloatScale = 26,
    FloatVar = 8,
    FloatXY = 27,
    FntFile = 17,
    FontTTF = 19,
    Integer = 6,
    IntegerLabeled = 20,
    Point = 2,
    PointLock = 3,
    Position = 0,
    ScaleLock = 4,
    Size = 1,
    SpriteFrame = 10,
    String = 24,
    Text = 18,
    Texture = 11,
  }
  public partial class CCBReader {
    // Fields
    public bool _hasScriptingOwner;
     
    // Constructors
    public CCBReader() { }
    public CCBReader(CocosSharp.CCBReader reader) { }
    public CCBReader(CocosSharp.CCNodeLoaderLibrary nodeLoaderLibrary) { }
    public CCBReader(CocosSharp.CCNodeLoaderLibrary nodeLoaderLibrary, CocosSharp.ICCBMemberVariableAssigner memberVariableAssigner) { }
    public CCBReader(CocosSharp.CCNodeLoaderLibrary nodeLoaderLibrary, CocosSharp.ICCBMemberVariableAssigner memberVariableAssigner, CocosSharp.ICCBSelectorResolver selectorResolver) { }
    public CCBReader(CocosSharp.CCNodeLoaderLibrary nodeLoaderLibrary, CocosSharp.ICCBMemberVariableAssigner memberVariableAssigner, CocosSharp.ICCBSelectorResolver selectorResolver, CocosSharp.ICCNodeLoaderListener nodeLoaderListener) { }
     
    // Properties
    public System.Collections.Generic.List<System.String> AnimatedProperties { get { return default(System.Collections.Generic.List<System.String>); } }
    public CocosSharp.CCBAnimationManager AnimationManager { get { return default(CocosSharp.CCBAnimationManager); } set { } }
    public System.Collections.Generic.Dictionary<CocosSharp.CCNode, CocosSharp.CCBAnimationManager> AnimationManagers { get { return default(System.Collections.Generic.Dictionary<CocosSharp.CCNode, CocosSharp.CCBAnimationManager>); } set { } }
    public System.Collections.Generic.List<CocosSharp.CCBAnimationManager> AnimationManagersForNodes { get { return default(System.Collections.Generic.List<CocosSharp.CCBAnimationManager>); } }
    public string CCBRootPath { get { return default(string); } set { } }
    public System.Collections.Generic.List<System.String> LoadedSpriteSheet { get { return default(System.Collections.Generic.List<System.String>); } }
    public CocosSharp.ICCBMemberVariableAssigner MemberVariableAssigner { get { return default(CocosSharp.ICCBMemberVariableAssigner); } }
    public System.Collections.Generic.List<CocosSharp.CCNode> NodesWithAnimationManagers { get { return default(System.Collections.Generic.List<CocosSharp.CCNode>); } }
    public object Owner { get { return default(object); } }
    public System.Collections.Generic.List<System.String> OwnerCallbackNames { get { return default(System.Collections.Generic.List<System.String>); } }
    public System.Collections.Generic.List<CocosSharp.CCNode> OwnerCallbackNodes { get { return default(System.Collections.Generic.List<CocosSharp.CCNode>); } }
    public System.Collections.Generic.List<System.String> OwnerOutletNames { get { return default(System.Collections.Generic.List<System.String>); } }
    public System.Collections.Generic.List<CocosSharp.CCNode> OwnerOutletNodes { get { return default(System.Collections.Generic.List<CocosSharp.CCNode>); } }
    public static float ResolutionScale { get { return default(float); } set { } }
    public CocosSharp.ICCBSelectorResolver SelectorResolver { get { return default(CocosSharp.ICCBSelectorResolver); } }
     
    // Methods
    public void AddDocumentCallbackName(string name) { }
    public void AddDocumentCallbackNode(CocosSharp.CCNode node) { }
    public void AddOwnerCallbackName(string name) { }
    public void AddOwnerCallbackNode(CocosSharp.CCNode node) { }
    public void AddOwnerOutletName(string name) { }
    public void AddOwnerOutletNode(CocosSharp.CCNode node) { }
    public CocosSharp.CCScene CreateSceneWithNodeGraphFromFile(string fileName) { return default(CocosSharp.CCScene); }
    public CocosSharp.CCScene CreateSceneWithNodeGraphFromFile(string fileName, object owner) { return default(CocosSharp.CCScene); }
    public CocosSharp.CCScene CreateSceneWithNodeGraphFromFile(string fileName, object owner, CocosSharp.CCSize parentSize) { return default(CocosSharp.CCScene); }
    public static string DeletePathExtension(string pPath) { return default(string); }
    public static bool EndsWith(string pString, string pEnding) { return default(bool); }
    public bool IsJSControlled() { return default(bool); }
    public static string LastPathComponent(string pPath) { return default(string); }
    public bool ReadBool() { return default(bool); }
    public byte ReadByte() { return default(byte); }
    public string ReadCachedString() { return default(string); }
    public bool ReadCallbackKeyframesForSeq(CocosSharp.CCBSequence seq) { return default(bool); }
    public CocosSharp.CCNode ReadFileWithCleanUp(bool bCleanUp, System.Collections.Generic.Dictionary<CocosSharp.CCNode, CocosSharp.CCBAnimationManager> am) { return default(CocosSharp.CCNode); }
    public float ReadFloat() { return default(float); }
    public int ReadInt(bool pSigned) { return default(int); }
    public CocosSharp.CCNode ReadNodeGraphFromData(System.Byte[] bytes, object owner, CocosSharp.CCSize parentSize) { return default(CocosSharp.CCNode); }
    public CocosSharp.CCNode ReadNodeGraphFromFile(string fileName) { return default(CocosSharp.CCNode); }
    public CocosSharp.CCNode ReadNodeGraphFromFile(string fileName, object owner) { return default(CocosSharp.CCNode); }
    public CocosSharp.CCNode ReadNodeGraphFromFile(string fileName, object owner, CocosSharp.CCSize parentSize) { return default(CocosSharp.CCNode); }
    public bool ReadSoundKeyframesForSeq(CocosSharp.CCBSequence seq) { return default(bool); }
    public string ReadUTF8() { return default(string); }
    public static string ToLowerCase(string pString) { return default(string); }
  }
  public partial class CCBRotateTo : CocosSharp.CCActionInterval {
    // Constructors
    public CCBRotateTo(float fDuration, float fAngle) { }
     
    // Methods
    public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCBRotateXTo : CocosSharp.CCActionInterval {
    // Constructors
    public CCBRotateXTo(float fDuration, float fAngle) { }
     
    // Methods
    public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCBRotateYTo : CocosSharp.CCActionInterval {
    // Constructors
    public CCBRotateYTo(float fDuration, float fAngle) { }
     
    // Methods
    public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public enum CCBScaleType {
    // Fields
    Absolute = 0,
    MultiplyResolution = 1,
  }
  public partial class CCBSequence {
    // Constructors
    public CCBSequence() { }
     
    // Properties
    public CocosSharp.CCBSequenceProperty CallBackChannel { get { return default(CocosSharp.CCBSequenceProperty); } set { } }
    public int ChainedSequenceId { get { return default(int); } set { } }
    public float Duration { get { return default(float); } set { } }
    public string Name { get { return default(string); } set { } }
    public int SequenceId { get { return default(int); } set { } }
    public CocosSharp.CCBSequenceProperty SoundChannel { get { return default(CocosSharp.CCBSequenceProperty); } set { } }
     
    // Methods
  }
  public partial class CCBSequenceProperty {
    // Constructors
    public CCBSequenceProperty() { }
     
    // Properties
    public System.Collections.Generic.List<CocosSharp.CCBKeyframe> Keyframes { get { return default(System.Collections.Generic.List<CocosSharp.CCBKeyframe>); } }
    public string Name { get { return default(string); } set { } }
    public CocosSharp.CCBPropertyType Type { get { return default(CocosSharp.CCBPropertyType); } set { } }
     
    // Methods
  }
  public partial class CCBSetSpriteFrame : CocosSharp.CCActionInstant {
    // Constructors
    public CCBSetSpriteFrame(CocosSharp.CCSpriteFrame pSpriteFrame) { }
     
    // Methods
    public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCBSoundEffect : CocosSharp.CCActionInstant {
    // Constructors
    public CCBSoundEffect(string file, float pitch, float pan, float gain) { }
     
    // Properties
    public float Gain { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } }
    public float Pan { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } }
    public float Pitch { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } }
    public string SoundFile { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(string); } }
     
    // Methods
    public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public enum CCBTargetType {
    // Fields
    DocumentRoot = 1,
    None = 0,
    Owner = 2,
  }
  public enum CCBufferUsage {
    // Fields
    None = 0,
    WriteOnly = 1,
  }
  public delegate void CCButtonTapDelegate(object sender);
  public partial class CCBValue {
    // Constructors
    public CCBValue(bool bValue) { }
    public CCBValue(byte bValue) { }
    public CCBValue(int nValue) { }
    public CCBValue(object pArr) { }
    public CCBValue(float fValue) { }
    public CCBValue(string pStr) { }
     
    // Properties
    public CocosSharp.CCBValueType Type { get { return default(CocosSharp.CCBValueType); } }
     
    // Methods
    public object GetArrayValue() { return default(object); }
    public bool GetBoolValue() { return default(bool); }
    public byte GetByteValue() { return default(byte); }
    public float GetFloatValue() { return default(float); }
    public int GetIntValue() { return default(int); }
    public string GetStringValue() { return default(string); }
  }
  public enum CCBValueType {
    // Fields
    Array = 5,
    Bool = 2,
    Float = 1,
    Int = 0,
    String = 4,
    UnsignedChar = 3,
  }
  public partial class CCCallFunc : CocosSharp.CCActionInstant {
    // Constructors
    public CCCallFunc() { }
    public CCCallFunc(System.Action selector) { }
     
    // Properties
    public System.Action CallFunction { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Action); } }
    public string ScriptFuncName { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(string); } }
     
    // Methods
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCCallFuncN : CocosSharp.CCCallFunc {
    // Constructors
    public CCCallFuncN() { }
    public CCCallFuncN(System.Action<CocosSharp.CCNode> selector) { }
     
    // Properties
    public System.Action<CocosSharp.CCNode> CallFunctionN { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Action<CocosSharp.CCNode>); } }
     
    // Methods
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCCallFuncND : CocosSharp.CCCallFuncN {
    // Constructors
    public CCCallFuncND(System.Action<CocosSharp.CCNode, System.Object> selector, object d) { }
     
    // Properties
    public System.Action<CocosSharp.CCNode, System.Object> CallFunctionND { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Action<CocosSharp.CCNode, System.Object>); } }
    public object Data { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(object); } }
     
    // Methods
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCCallFuncNDState : CocosSharp.CCCallFuncState {
    // Constructors
    public CCCallFuncNDState(CocosSharp.CCCallFuncND action, CocosSharp.CCNode target) : base (default(CocosSharp.CCCallFunc), default(CocosSharp.CCNode)) { }
     
    // Properties
    protected System.Action<CocosSharp.CCNode, System.Object> CallFunctionND { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Action<CocosSharp.CCNode, System.Object>); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    protected object Data { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(object); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
     
    // Methods
    public override void Execute() { }
  }
  public partial class CCCallFuncNState : CocosSharp.CCCallFuncState {
    // Constructors
    public CCCallFuncNState(CocosSharp.CCCallFuncN action, CocosSharp.CCNode target) : base (default(CocosSharp.CCCallFunc), default(CocosSharp.CCNode)) { }
     
    // Properties
    protected System.Action<CocosSharp.CCNode> CallFunctionN { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Action<CocosSharp.CCNode>); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
     
    // Methods
    public override void Execute() { }
  }
  public partial class CCCallFuncO : CocosSharp.CCCallFunc {
    // Constructors
    public CCCallFuncO() { }
    public CCCallFuncO(System.Action<System.Object> selector, object pObject) { }
     
    // Properties
    public System.Action<System.Object> CallFunctionO { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Action<System.Object>); } }
    public object Object { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(object); } }
     
    // Methods
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCCallFuncOState : CocosSharp.CCCallFuncState {
    // Constructors
    public CCCallFuncOState(CocosSharp.CCCallFuncO action, CocosSharp.CCNode target) : base (default(CocosSharp.CCCallFunc), default(CocosSharp.CCNode)) { }
     
    // Properties
    protected System.Action<System.Object> CallFunctionO { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Action<System.Object>); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    protected object Object { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(object); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
     
    // Methods
    public override void Execute() { }
  }
  public partial class CCCallFuncState : CocosSharp.CCActionInstantState {
    // Constructors
    public CCCallFuncState(CocosSharp.CCCallFunc action, CocosSharp.CCNode target) : base (default(CocosSharp.CCActionInstant), default(CocosSharp.CCNode)) { }
     
    // Properties
    protected System.Action CallFunction { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Action); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    protected string ScriptFuncName { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(string); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
     
    // Methods
    public virtual void Execute() { }
    public override void Update(float time) { }
  }
  public partial class CCCamera {
    // Constructors
    public CCCamera() { }
     
    // Properties
    protected float CenterX { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    protected float CenterY { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    protected float CenterZ { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    protected float EyeX { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    protected float EyeY { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    protected float EyeZ { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public bool IsDirty { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]protected set { } }
    protected float UpX { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    protected float UpY { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    protected float UpZ { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
     
    // Methods
    public void GetCenterXyz(out float pCenterX, out float pCenterY, out float pCenterZ) { pCenterX = default(float); pCenterY = default(float); pCenterZ = default(float); }
    public void GetEyeXyz(out float pEyeX, out float pEyeY, out float pEyeZ) { pEyeX = default(float); pEyeY = default(float); pEyeZ = default(float); }
    public void GetUpXyz(out float pUpX, out float pUpY, out float pUpZ) { pUpX = default(float); pUpY = default(float); pUpZ = default(float); }
    public static float GetZEye() { return default(float); }
    public void Locate() { }
    public void Restore() { }
    public void SetCenterXyz(float fCenterX, float fCenterY, float fCenterZ) { }
    public void SetEyeXyz(float fEyeX, float fEyeY, float fEyeZ) { }
    public void SetUpXyz(float fUpX, float fUpY, float fUpZ) { }
    public override string ToString() { return default(string); }
  }
  public partial class CCCardinalSplineBy : CocosSharp.CCCardinalSplineTo {
    // Constructors
    public CCCardinalSplineBy(float duration, System.Collections.Generic.List<CocosSharp.CCPoint> points, float tension) : base (default(float), default(System.Collections.Generic.List<CocosSharp.CCPoint>), default(float)) { }
     
    // Methods
    public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCCardinalSplineByState : CocosSharp.CCCardinalSplineToState {
    // Constructors
    public CCCardinalSplineByState(CocosSharp.CCCardinalSplineTo action, CocosSharp.CCNode target) : base (default(CocosSharp.CCCardinalSplineTo), default(CocosSharp.CCNode)) { }
     
    // Properties
    protected CocosSharp.CCPoint StartPosition { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCPoint); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
     
    // Methods
    public override void UpdatePosition(CocosSharp.CCPoint newPos) { }
  }
  public partial class CCCardinalSplineTo : CocosSharp.CCActionInterval {
    // Constructors
    public CCCardinalSplineTo(float duration, System.Collections.Generic.List<CocosSharp.CCPoint> points, float tension) { }
     
    // Properties
    public System.Collections.Generic.List<CocosSharp.CCPoint> Points { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Collections.Generic.List<CocosSharp.CCPoint>); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]protected set { } }
    public float Tension { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]protected set { } }
     
    // Methods
    public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCCardinalSplineToState : CocosSharp.CCActionIntervalState {
    // Constructors
    public CCCardinalSplineToState(CocosSharp.CCCardinalSplineTo action, CocosSharp.CCNode target) : base (default(CocosSharp.CCActionInterval), default(CocosSharp.CCNode)) { }
     
    // Properties
    protected CocosSharp.CCPoint AccumulatedDiff { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCPoint); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    protected float DeltaT { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    protected System.Collections.Generic.List<CocosSharp.CCPoint> Points { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Collections.Generic.List<CocosSharp.CCPoint>); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    protected CocosSharp.CCPoint PreviousPosition { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCPoint); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    protected float Tension { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
     
    // Methods
    public override void Update(float time) { }
    public virtual void UpdatePosition(CocosSharp.CCPoint newPos) { }
  }
  public partial class CCCatmullRomBy : CocosSharp.CCCardinalSplineBy {
    // Constructors
    public CCCatmullRomBy(float dt, System.Collections.Generic.List<CocosSharp.CCPoint> points) : base (default(float), default(System.Collections.Generic.List<CocosSharp.CCPoint>), default(float)) { }
  }
  public partial class CCCatmullRomTo : CocosSharp.CCCardinalSplineTo {
    // Constructors
    public CCCatmullRomTo(float dt, System.Collections.Generic.List<CocosSharp.CCPoint> points) : base (default(float), default(System.Collections.Generic.List<CocosSharp.CCPoint>), default(float)) { }
  }
  public enum CCClipMode {
    // Fields
    Bounds = 1,
    BoundsWithRenderTarget = 2,
    None = 0,
  }
  public partial class CCClippingNode : CocosSharp.CCNode {
    // Constructors
    public CCClippingNode() { }
    public CCClippingNode(CocosSharp.CCNode stencil) { }
     
    // Properties
    public float AlphaThreshold { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public bool Inverted { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public CocosSharp.CCNode Stencil { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCNode); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
     
    // Methods
    public override void OnEnter() { }
    public override void OnEnterTransitionDidFinish() { }
    public override void OnExit() { }
    public override void OnExitTransitionDidStart() { }
    public override void Visit() { }
  }
  [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Size=1)]
  public partial struct CCColor3B {
    // Fields
    public static readonly CocosSharp.CCColor3B Black;
    public static readonly CocosSharp.CCColor3B Blue;
    public static readonly CocosSharp.CCColor3B Gray;
    public static readonly CocosSharp.CCColor3B Green;
    public static readonly CocosSharp.CCColor3B Magenta;
    public static readonly CocosSharp.CCColor3B Orange;
    public static readonly CocosSharp.CCColor3B Red;
    public static readonly CocosSharp.CCColor3B White;
    public static readonly CocosSharp.CCColor3B Yellow;
     
    // Constructors
    public CCColor3B(CocosSharp.CCColor4B color4B) { throw new System.NotImplementedException(); }
    public CCColor3B(byte inr, byte ing, byte inb) { throw new System.NotImplementedException(); }
     
    // Properties
    public byte B { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(byte); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public byte G { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(byte); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public byte R { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(byte); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
     
    // Methods
    public bool Equals(CocosSharp.CCColor3B other) { return default(bool); }
    public override bool Equals(object obj) { return default(bool); }
    public override int GetHashCode() { return default(int); }
    public static CocosSharp.CCColor3B operator /(CocosSharp.CCColor3B p1, float div) { return default(CocosSharp.CCColor3B); }
    public static bool operator ==(CocosSharp.CCColor3B p1, CocosSharp.CCColor3B p2) { return default(bool); }
    public static bool operator !=(CocosSharp.CCColor3B p1, CocosSharp.CCColor3B p2) { return default(bool); }
    public static CocosSharp.CCColor3B operator *(CocosSharp.CCColor3B p1, CocosSharp.CCColor3B p2) { return default(CocosSharp.CCColor3B); }
  }
  public partial class CCColor3BWapper {
    // Constructors
    public CCColor3BWapper(CocosSharp.CCColor3B xcolor) { }
     
    // Properties
    public CocosSharp.CCColor3B Color { get { return default(CocosSharp.CCColor3B); } }
     
    // Methods
  }
  [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Size=1)]
  public partial struct CCColor4B {
    // Fields
    public static readonly CocosSharp.CCColor4B AliceBlue;
    public static readonly CocosSharp.CCColor4B Aquamarine;
    public static readonly CocosSharp.CCColor4B Black;
    public static readonly CocosSharp.CCColor4B Blue;
    public static readonly CocosSharp.CCColor4B Gray;
    public static readonly CocosSharp.CCColor4B Green;
    public static readonly CocosSharp.CCColor4B Magenta;
    public static readonly CocosSharp.CCColor4B Orange;
    public static readonly CocosSharp.CCColor4B Red;
    public static readonly CocosSharp.CCColor4B Transparent;
    public static readonly CocosSharp.CCColor4B White;
    public static readonly CocosSharp.CCColor4B Yellow;
     
    // Constructors
    public CCColor4B(byte inr, byte ing, byte inb) { throw new System.NotImplementedException(); }
    public CCColor4B(byte inr, byte ing, byte inb, byte ina) { throw new System.NotImplementedException(); }
    public CCColor4B(float fR, float fG, float fB, float fA) { throw new System.NotImplementedException(); }
     
    // Properties
    public byte A { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(byte); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public byte B { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(byte); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public byte G { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(byte); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public byte R { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(byte); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
     
    // Methods
    public bool Equals(CocosSharp.CCColor4B other) { return default(bool); }
    public override bool Equals(object obj) { return default(bool); }
    public override int GetHashCode() { return default(int); }
    public static CocosSharp.CCColor4B Lerp(CocosSharp.CCColor4B value1, CocosSharp.CCColor4B value2, float amount) { return default(CocosSharp.CCColor4B); }
    public static CocosSharp.CCColor4B operator /(CocosSharp.CCColor4B p1, float div) { return default(CocosSharp.CCColor4B); }
    public static bool operator ==(CocosSharp.CCColor4B p1, CocosSharp.CCColor4B p2) { return default(bool); }
    public static bool operator !=(CocosSharp.CCColor4B p1, CocosSharp.CCColor4B p2) { return default(bool); }
    public static CocosSharp.CCColor4B operator *(CocosSharp.CCColor4B p1, CocosSharp.CCColor4B p2) { return default(CocosSharp.CCColor4B); }
    public static CocosSharp.CCColor4B operator *(CocosSharp.CCColor4B p1, float scale) { return default(CocosSharp.CCColor4B); }
    public static CocosSharp.CCColor4B operator *(float scale, CocosSharp.CCColor4B p1) { return default(CocosSharp.CCColor4B); }
    public static CocosSharp.CCColor4B Parse(string s) { return default(CocosSharp.CCColor4B); }
    public override string ToString() { return default(string); }
  }
  [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Size=1)]
  public partial struct CCColor4F {
    // Constructors
    public CCColor4F(CocosSharp.CCColor3B c) { throw new System.NotImplementedException(); }
    public CCColor4F(CocosSharp.CCColor4B c) { throw new System.NotImplementedException(); }
    public CCColor4F(float inr, float ing, float inb, float ina) { throw new System.NotImplementedException(); }
     
    // Properties
    public float A { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public float B { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public float G { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public float R { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
     
    // Methods
    public bool Equals(CocosSharp.CCColor4F other) { return default(bool); }
    public override bool Equals(object obj) { return default(bool); }
    public override int GetHashCode() { return default(int); }
    public static bool operator ==(CocosSharp.CCColor4F a, CocosSharp.CCColor4F b) { return default(bool); }
    public static bool operator !=(CocosSharp.CCColor4F a, CocosSharp.CCColor4F b) { return default(bool); }
    public static CocosSharp.CCColor4F Parse(string s) { return default(CocosSharp.CCColor4F); }
    public override string ToString() { return default(string); }
  }
  public partial class CCConfiguration {
    internal CCConfiguration() { }
    // Fields
    protected bool m_bInited;
    protected bool m_bSupportsBGRA8888;
    protected bool m_bSupportsDiscardFramebuffer;
    protected bool m_bSupportsNPOT;
    protected bool m_bSupportsPVRTC;
    protected int m_nMaxModelviewStackDepth;
    protected int m_nMaxSamplesAllowed;
    protected int m_nMaxTextureSize;
    protected string m_pGlExtensions;
    protected uint m_uOSVersion;
     
    // Properties
    public bool IsSupportsBGRA8888 { get { return default(bool); } }
    public bool IsSupportsDiscardFramebuffer { get { return default(bool); } }
    public bool IsSupportsNPOT { get { return default(bool); } }
    public bool IsSupportsPVRTC { get { return default(bool); } }
    public int MaxModelviewStackDepth { get { return default(int); } }
    public int MaxTextureSize { get { return default(int); } }
    public uint OSVersion { get { return default(uint); } }
    public static CocosSharp.CCConfiguration SharedConfiguration { get { return default(CocosSharp.CCConfiguration); } }
     
    // Methods
    public bool CheckForGLExtension(string searchName) { return default(bool); }
    public CocosSharp.CCGlesVersion getGlesVersion() { return default(CocosSharp.CCGlesVersion); }
  }
  public partial class CCControl : CocosSharp.CCLayerRGBA {
    // Fields
    protected System.Collections.Generic.Dictionary<CocosSharp.CCControlEvent, CocosSharp.CCRawList<CocosSharp.CCInvocation>> _dispatchTable;
    protected bool _enabled;
    protected bool _hasVisibleParents;
    protected bool _highlighted;
    protected bool _selected;
    protected CocosSharp.CCControlState _state;
     
    // Constructors
    public CCControl() { }
     
    // Properties
    public int DefaultTouchPriority { get { return default(int); } set { } }
    public virtual bool Enabled { get { return default(bool); } set { } }
    public virtual bool Highlighted { get { return default(bool); } set { } }
    public override bool IsColorModifiedByOpacity { get { return default(bool); } set { } }
    public virtual bool Selected { get { return default(bool); } set { } }
    public CocosSharp.CCControlState State { get { return default(CocosSharp.CCControlState); } set { } }
     
    // Methods
    public void AddTargetWithActionForControlEvent(object target, System.Action<System.Object, CocosSharp.CCControlEvent> action, CocosSharp.CCControlEvent controlEvent) { }
    public virtual void AddTargetWithActionForControlEvents(object target, System.Action<System.Object, CocosSharp.CCControlEvent> action, CocosSharp.CCControlEvent controlEvents) { }
    protected CocosSharp.CCRawList<CocosSharp.CCInvocation> DispatchListforControlEvent(CocosSharp.CCControlEvent controlEvent) { return default(CocosSharp.CCRawList<CocosSharp.CCInvocation>); }
    public virtual CocosSharp.CCPoint GetTouchLocation(CocosSharp.CCTouch touch) { return default(CocosSharp.CCPoint); }
    public bool HasVisibleParents() { return default(bool); }
    public virtual bool IsTouchInside(CocosSharp.CCTouch touch) { return default(bool); }
    public virtual void NeedsLayout() { }
    public void RemoveTargetWithActionForControlEvent(object target, System.Action<System.Object, CocosSharp.CCControlEvent> action, CocosSharp.CCControlEvent controlEvent) { }
    public virtual void RemoveTargetWithActionForControlEvents(object target, System.Action<System.Object, CocosSharp.CCControlEvent> action, CocosSharp.CCControlEvent controlEvents) { }
    public virtual void SendActionsForControlEvents(CocosSharp.CCControlEvent controlEvents) { }
  }
  public partial class CCControlButton : CocosSharp.CCControl {
    // Fields
    protected CocosSharp.CCNode _backgroundSprite;
    protected System.Collections.Generic.Dictionary<CocosSharp.CCControlState, CocosSharp.CCNode> _backgroundSpriteDispatchTable;
    protected string _currentTitle;
    protected CocosSharp.CCColor3B _currentTitleColor;
    protected bool _doesAdjustBackgroundImage;
    protected bool _isPushed;
    protected CocosSharp.CCPoint _labelAnchorPoint;
    protected int _marginH;
    protected int _marginV;
    protected bool _parentInited;
    protected CocosSharp.CCSize _preferredSize;
    protected System.Collections.Generic.Dictionary<CocosSharp.CCControlState, CocosSharp.CCColor3B> _titleColorDispatchTable;
    protected System.Collections.Generic.Dictionary<CocosSharp.CCControlState, System.String> _titleDispatchTable;
    protected CocosSharp.CCNode _titleLabel;
    protected System.Collections.Generic.Dictionary<CocosSharp.CCControlState, CocosSharp.CCNode> _titleLabelDispatchTable;
    protected bool _zoomOnTouchDown;
    public const int CCControlButtonMarginLR = 8;
    public const int CCControlButtonMarginTB = 2;
    public const int kZoomActionTag = 2093678593;
     
    // Constructors
    public CCControlButton() { }
    public CCControlButton(CocosSharp.CCNode sprite) { }
    public CCControlButton(CocosSharp.CCNode label, CocosSharp.CCNode backgroundSprite) { }
    public CCControlButton(string title, string fontName, float fontSize) { }
     
    // Properties
    public CocosSharp.CCNode BackgroundSprite { get { return default(CocosSharp.CCNode); } set { } }
    public override CocosSharp.CCColor3B Color { get { return default(CocosSharp.CCColor3B); } set { } }
    public override bool Enabled { get { return default(bool); } set { } }
    public override bool Highlighted { set { } }
    public bool IsPushed { get { return default(bool); } }
    public CocosSharp.CCPoint LabelAnchorPoint { get { return default(CocosSharp.CCPoint); } set { } }
    public override byte Opacity { get { return default(byte); } set { } }
    public CocosSharp.CCSize PreferredSize { get { return default(CocosSharp.CCSize); } set { } }
    public override bool Selected { set { } }
    public CocosSharp.CCNode TitleLabel { get { return default(CocosSharp.CCNode); } set { } }
    public bool ZoomOnTouchDown { get { return default(bool); } set { } }
     
    // Events
    public event CocosSharp.CCButtonTapDelegate OnButtonTap { add { } remove { } }
     
    // Methods
    public bool DoesAdjustBackgroundImage() { return default(bool); }
    public virtual CocosSharp.CCNode GetBackgroundSpriteForState(CocosSharp.CCControlState state) { return default(CocosSharp.CCNode); }
    public virtual string GetTitleBmFontForState(CocosSharp.CCControlState state) { return default(string); }
    public virtual CocosSharp.CCColor3B GetTitleColorForState(CocosSharp.CCControlState state) { return default(CocosSharp.CCColor3B); }
    public virtual string GetTitleForState(CocosSharp.CCControlState state) { return default(string); }
    public virtual CocosSharp.CCNode GetTitleLabelForState(CocosSharp.CCControlState state) { return default(CocosSharp.CCNode); }
    public virtual string GetTitleTtfForState(CocosSharp.CCControlState state) { return default(string); }
    public virtual float GetTitleTtfSizeForState(CocosSharp.CCControlState state) { return default(float); }
    public override void NeedsLayout() { }
    public void SetAdjustBackgroundImage(bool adjustBackgroundImage) { }
    public virtual void SetBackgroundSpriteForState(CocosSharp.CCNode sprite, CocosSharp.CCControlState state) { }
    public virtual void SetBackgroundSpriteFrameForState(CocosSharp.CCSpriteFrame spriteFrame, CocosSharp.CCControlState state) { }
    protected virtual void SetMargins(int marginH, int marginV) { }
    public virtual void SetTitleBmFontForState(string fntFile, CocosSharp.CCControlState state) { }
    public virtual void SetTitleColorForState(CocosSharp.CCColor3B color, CocosSharp.CCControlState state) { }
    public virtual void SetTitleForState(string title, CocosSharp.CCControlState state) { }
    public virtual void SetTitleLabelForState(CocosSharp.CCNode titleLabel, CocosSharp.CCControlState state) { }
    public virtual void SetTitleTtfForState(string fntFile, CocosSharp.CCControlState state) { }
    public virtual void SetTitleTtfSizeForState(float size, CocosSharp.CCControlState state) { }
  }
  public partial class CCControlColourPicker : CocosSharp.CCControl {
    // Fields
    protected CocosSharp.HSV _hsv;
     
    // Constructors
    public CCControlColourPicker() { }
     
    // Properties
    public CocosSharp.CCSprite Background { get { return default(CocosSharp.CCSprite); } set { } }
    public CocosSharp.CCControlSaturationBrightnessPicker ColourPicker { get { return default(CocosSharp.CCControlSaturationBrightnessPicker); } set { } }
    public CocosSharp.CCControlHuePicker HuePicker { get { return default(CocosSharp.CCControlHuePicker); } set { } }
     
    // Methods
    public void ColourSliderValueChanged(object sender, CocosSharp.CCControlEvent controlEvent) { }
    public void HueSliderValueChanged(object sender, CocosSharp.CCControlEvent controlEvent) { }
    public void SetColor(CocosSharp.CCColor3B colorValue) { }
    public void SetEnabled(bool bEnabled) { }
    protected void UpdateControlPicker() { }
    protected void UpdateHueAndControlPicker() { }
  }
  [System.FlagsAttribute]
  public enum CCControlEvent {
    // Fields
    TouchCancel = 128,
    TouchDown = 1,
    TouchDragEnter = 8,
    TouchDragExit = 16,
    TouchDragInside = 2,
    TouchDragOutside = 4,
    TouchUpInside = 32,
    TouchUpOutside = 64,
    ValueChanged = 256,
  }
  public partial class CCControlHuePicker : CocosSharp.CCControl {
    // Constructors
    public CCControlHuePicker() { }
    public CCControlHuePicker(CocosSharp.CCNode target, CocosSharp.CCPoint pos) { }
     
    // Properties
    public CocosSharp.CCSprite Background { get { return default(CocosSharp.CCSprite); } set { } }
    public override bool Enabled { get { return default(bool); } set { } }
    public float Hue { get { return default(float); } set { } }
    public float HuePercentage { get { return default(float); } set { } }
    public CocosSharp.CCSprite Slider { get { return default(CocosSharp.CCSprite); } set { } }
    public CocosSharp.CCPoint StartPos { get { return default(CocosSharp.CCPoint); } set { } }
     
    // Methods
    protected bool CheckSliderPosition(CocosSharp.CCPoint location) { return default(bool); }
    protected void UpdateSliderPosition(CocosSharp.CCPoint location) { }
  }
  public partial class CCControlPotentiometer : CocosSharp.CCControl {
    // Fields
    protected float _maximumValue;
    protected float _minimumValue;
    protected float _value;
     
    // Constructors
    public CCControlPotentiometer() { }
    public CCControlPotentiometer(string backgroundFile, string progressFile, string thumbFile) { }
     
    // Properties
    public override bool Enabled { get { return default(bool); } set { } }
    public float MaximumValue { get { return default(float); } set { } }
    public float MinimumValue { get { return default(float); } set { } }
    protected CocosSharp.CCPoint PreviousLocation { get { return default(CocosSharp.CCPoint); } set { } }
    protected CocosSharp.CCProgressTimer ProgressTimer { get { return default(CocosSharp.CCProgressTimer); } set { } }
    protected CocosSharp.CCSprite ThumbSprite { get { return default(CocosSharp.CCSprite); } set { } }
    public float Value { get { return default(float); } set { } }
     
    // Methods
    protected float AngleInDegreesBetweenLineFromPoint_toPoint_toLineFromPoint_toPoint(CocosSharp.CCPoint beginLineA, CocosSharp.CCPoint endLineA, CocosSharp.CCPoint beginLineB, CocosSharp.CCPoint endLineB) { return default(float); }
    protected float DistanceBetweenPointAndPoint(CocosSharp.CCPoint point1, CocosSharp.CCPoint point2) { return default(float); }
    public override bool IsTouchInside(CocosSharp.CCTouch touch) { return default(bool); }
    protected void PotentiometerBegan(CocosSharp.CCPoint location) { }
    protected void PotentiometerEnded(CocosSharp.CCPoint location) { }
    protected void PotentiometerMoved(CocosSharp.CCPoint location) { }
  }
  public partial class CCControlSaturationBrightnessPicker : CocosSharp.CCControl {
    // Fields
    protected int boxPos;
    protected int boxSize;
     
    // Constructors
    public CCControlSaturationBrightnessPicker() { }
    public CCControlSaturationBrightnessPicker(CocosSharp.CCNode target, CocosSharp.CCPoint pos) { }
     
    // Properties
    public CocosSharp.CCSprite Background { get { return default(CocosSharp.CCSprite); } set { } }
    public float Brightness { get { return default(float); } set { } }
    public override bool Enabled { get { return default(bool); } set { } }
    public CocosSharp.CCSprite Overlay { get { return default(CocosSharp.CCSprite); } set { } }
    public float Saturation { get { return default(float); } set { } }
    public CocosSharp.CCSprite Shadow { get { return default(CocosSharp.CCSprite); } set { } }
    public CocosSharp.CCSprite Slider { get { return default(CocosSharp.CCSprite); } set { } }
    public CocosSharp.CCPoint StartPos { get { return default(CocosSharp.CCPoint); } set { } }
     
    // Methods
    protected bool CheckSliderPosition(CocosSharp.CCPoint location) { return default(bool); }
    public virtual void UpdateDraggerWithHSV(CocosSharp.HSV hsv) { }
    protected void UpdateSliderPosition(CocosSharp.CCPoint sliderPosition) { }
    public virtual void UpdateWithHSV(CocosSharp.HSV hsv) { }
  }
  public partial class CCControlSlider : CocosSharp.CCControl {
    // Constructors
    public CCControlSlider(CocosSharp.CCSprite backgroundSprite, CocosSharp.CCSprite progressSprite, CocosSharp.CCSprite thumbSprite) { }
    public CCControlSlider(string bgFile, string progressFile, string thumbFile) { }
     
    // Properties
    public CocosSharp.CCSprite BackgroundSprite { get { return default(CocosSharp.CCSprite); } set { } }
    public override bool Enabled { get { return default(bool); } set { } }
    public float MaximumAllowedValue { get { return default(float); } set { } }
    public float MaximumValue { get { return default(float); } set { } }
    public float MinimumAllowedValue { get { return default(float); } set { } }
    public float MinimumValue { get { return default(float); } set { } }
    public CocosSharp.CCSprite ProgressSprite { get { return default(CocosSharp.CCSprite); } set { } }
    public float SnappingInterval { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public CocosSharp.CCSprite ThumbSprite { get { return default(CocosSharp.CCSprite); } set { } }
    public float Value { get { return default(float); } set { } }
     
    // Methods
    public override bool IsTouchInside(CocosSharp.CCTouch touch) { return default(bool); }
    protected virtual CocosSharp.CCPoint LocationFromTouch(CocosSharp.CCTouch touch) { return default(CocosSharp.CCPoint); }
    public override void NeedsLayout() { }
    protected void SliderBegan(CocosSharp.CCPoint location) { }
    protected void SliderEnded(CocosSharp.CCPoint location) { }
    protected void SliderMoved(CocosSharp.CCPoint location) { }
    protected float ValueForLocation(CocosSharp.CCPoint location) { return default(float); }
  }
  [System.FlagsAttribute]
  public enum CCControlState {
    // Fields
    Disabled = 4,
    Highlighted = 2,
    Normal = 1,
    Selected = 8,
  }
  public partial class CCControlStepper : CocosSharp.CCControl {
    // Fields
    protected bool _autorepeat;
    protected int _autorepeatCount;
    protected bool _continuous;
    protected float _maximumValue;
    protected float _minimumValue;
    protected CocosSharp.CCLabelTtf _minusLabel;
    protected CocosSharp.CCSprite _minusSprite;
    protected CocosSharp.CCLabelTtf _plusLabel;
    protected CocosSharp.CCSprite _plusSprite;
    protected float _stepValue;
    protected CocosSharp.CCControlStepperPart _touchedPart;
    protected bool _touchInsideFlag;
    protected float _value;
    protected bool _wraps;
     
    // Constructors
    public CCControlStepper() { }
    public CCControlStepper(CocosSharp.CCSprite minusSprite, CocosSharp.CCSprite plusSprite) { }
     
    // Properties
    public virtual bool IsContinuous { get { return default(bool); } }
    public virtual float MaximumValue { get { return default(float); } set { } }
    public virtual float MinimumValue { get { return default(float); } set { } }
    public CocosSharp.CCLabelTtf MinusLabel { get { return default(CocosSharp.CCLabelTtf); } set { } }
    public CocosSharp.CCSprite MinusSprite { get { return default(CocosSharp.CCSprite); } set { } }
    public CocosSharp.CCLabelTtf PlusLabel { get { return default(CocosSharp.CCLabelTtf); } set { } }
    public CocosSharp.CCSprite PlusSprite { get { return default(CocosSharp.CCSprite); } set { } }
    public virtual float StepValue { get { return default(float); } set { } }
    public virtual float Value { get { return default(float); } set { } }
    public virtual bool Wraps { get { return default(bool); } set { } }
     
    // Methods
    public virtual void SetValueWithSendingEvent(float value, bool send) { }
    protected void StartAutorepeat() { }
    protected void StopAutorepeat() { }
    public override void Update(float dt) { }
    protected void UpdateLayoutUsingTouchLocation(CocosSharp.CCPoint location) { }
  }
  public enum CCControlStepperPart {
    // Fields
    PartMinus = 0,
    PartNone = 2,
    PartPlus = 1,
  }
  public partial class CCControlSwitch : CocosSharp.CCControl {
    // Fields
    protected float _initialTouchXPosition;
    protected bool _moved;
    protected bool _on;
    protected CocosSharp.CCControlSwitchSprite _switchSprite;
     
    // Constructors
    public CCControlSwitch(CocosSharp.CCSprite maskSprite, CocosSharp.CCSprite onSprite, CocosSharp.CCSprite offSprite, CocosSharp.CCSprite thumbSprite) { }
    public CCControlSwitch(CocosSharp.CCSprite maskSprite, CocosSharp.CCSprite onSprite, CocosSharp.CCSprite offSprite, CocosSharp.CCSprite thumbSprite, CocosSharp.CCLabelTtf onLabel, CocosSharp.CCLabelTtf offLabel) { }
     
    // Properties
    public override bool Enabled { get { return default(bool); } set { } }
     
    // Events
    public event CocosSharp.CCSwitchValueChangedDelegate OnValueChanged { add { } remove { } }
     
    // Methods
    public bool HasMoved() { return default(bool); }
    public bool IsOn() { return default(bool); }
    public CocosSharp.CCPoint LocationFromTouch(CocosSharp.CCTouch touch) { return default(CocosSharp.CCPoint); }
    public void SetOn(bool isOn) { }
    public void SetOn(bool isOn, bool animated) { }
  }
  public partial class CCControlSwitchSprite : CocosSharp.CCSprite, CocosSharp.ICCActionTweenDelegate {
    // Constructors
    public CCControlSwitchSprite() : base (default(CocosSharp.CCTexture2D), default(CocosSharp.CCRect), default(bool)) { }
    public CCControlSwitchSprite(CocosSharp.CCSprite maskSprite, CocosSharp.CCSprite onSprite, CocosSharp.CCSprite offSprite, CocosSharp.CCSprite thumbSprite, CocosSharp.CCLabelTtf onLabel, CocosSharp.CCLabelTtf offLabel) : base (default(CocosSharp.CCTexture2D), default(CocosSharp.CCRect), default(bool)) { }
     
    // Properties
    public CocosSharp.CCSprite MaskSprite { get { return default(CocosSharp.CCSprite); } set { } }
    public CocosSharp.CCLabelTtf OffLabel { get { return default(CocosSharp.CCLabelTtf); } set { } }
    public float OffPosition { get { return default(float); } set { } }
    public float OffSideWidth { get { return default(float); } }
    public CocosSharp.CCSprite OffSprite { get { return default(CocosSharp.CCSprite); } set { } }
    public CocosSharp.CCLabelTtf OnLabel { get { return default(CocosSharp.CCLabelTtf); } set { } }
    public float OnPosition { get { return default(float); } set { } }
    public float OnSideWidth { get { return default(float); } }
    public CocosSharp.CCSprite OnSprite { get { return default(CocosSharp.CCSprite); } set { } }
    public float SliderXPosition { get { return default(float); } set { } }
    public CocosSharp.CCSprite ThumbSprite { get { return default(CocosSharp.CCSprite); } set { } }
     
    // Methods
    protected override void Draw() { }
    public void NeedsLayout() { }
    public virtual void UpdateTweenAction(float value, string key) { }
  }
  public partial class CCDeccelAmplitude : CocosSharp.CCAccelAmplitude {
    // Constructors
    public CCDeccelAmplitude(CocosSharp.CCAmplitudeAction pAction, float duration, float deccRate=1f) : base (default(CocosSharp.CCAmplitudeAction), default(float), default(float)) { }
     
    // Methods
    public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCDeccelAmplitudeState : CocosSharp.CCAccelAmplitudeState {
    // Constructors
    public CCDeccelAmplitudeState(CocosSharp.CCDeccelAmplitude action, CocosSharp.CCNode target) : base (default(CocosSharp.CCAccelAmplitude), default(CocosSharp.CCNode)) { }
     
    // Methods
    public override void Update(float time) { }
  }
  public partial class CCDelayTime : CocosSharp.CCActionInterval {
    // Constructors
    public CCDelayTime(float d) { }
     
    // Methods
    public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCDelayTimeState : CocosSharp.CCActionIntervalState {
    // Constructors
    public CCDelayTimeState(CocosSharp.CCDelayTime action, CocosSharp.CCNode target) : base (default(CocosSharp.CCActionInterval), default(CocosSharp.CCNode)) { }
     
    // Methods
    public override void Update(float time) { }
  }
  public enum CCDepthFormat {
    // Fields
    Depth16 = 54,
    Depth24 = 51,
    Depth24Stencil8 = 48,
    None = -1,
  }
  public static partial class CCDevice {
    // Methods
    public static float GetDPI() { return default(float); }
  }
  public abstract partial class CCDirector {
    // Fields
    public static string EVENT_AFTER_DRAW;
    public static string EVENT_AFTER_UPDATE;
    public static string EVENT_AFTER_VISIT;
    public static string EVENT_PROJECTION_CHANGED;
    protected CocosSharp.CCStats Stats;
     
    // Constructors
    protected CCDirector() { }
     
    // Properties
    public CocosSharp.CCAccelerometer Accelerometer { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCAccelerometer); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public CocosSharp.CCActionManager ActionManager { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCActionManager); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public virtual double AnimationInterval { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(double); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public float ContentScaleFactor { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public bool DisplayStats { get { return default(bool); } set { } }
    public CocosSharp.CCEventDispatcher EventDispatcher { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCEventDispatcher); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public bool GamePadEnabled { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public bool IsCanPopScene { get { return default(bool); } }
    public bool IsNextDeltaTimeZero { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public bool IsPaused { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } }
    protected bool IsPurgeDirectorInNextLoop { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public bool IsSendCleanupToScene { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } }
    public bool IsUseAlphaBlending { set { } }
    public bool IsUseDepthTesting { get { return default(bool); } set { } }
    public CocosSharp.CCKeypadDispatcher KeypadDispatcher { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCKeypadDispatcher); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public CocosSharp.CCNode NotificationNode { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCNode); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    protected double OldAnimationInterval { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(double); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public CocosSharp.CCDirectorProjection Projection { get { return default(CocosSharp.CCDirectorProjection); } set { } }
    public CocosSharp.ICCDirectorDelegate ProjectionDelegate { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.ICCDirectorDelegate); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public CocosSharp.CCScene RunningScene { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCScene); } }
    public int SceneCount { get { return default(int); } }
    public CocosSharp.CCScheduler Scheduler { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCScheduler); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public static CocosSharp.CCDirector SharedDirector { get { return default(CocosSharp.CCDirector); } }
    public CocosSharp.CCPoint VisibleOrigin { get { return default(CocosSharp.CCPoint); } }
    public CocosSharp.CCSize VisibleSize { get { return default(CocosSharp.CCSize); } }
    public CocosSharp.CCSize WinSize { get { return default(CocosSharp.CCSize); } }
    public CocosSharp.CCSize WinSizeInPixels { get { return default(CocosSharp.CCSize); } }
    public float ZEye { get { return default(float); } }
     
    // Methods
    public CocosSharp.CCPoint ConvertToGl(CocosSharp.CCPoint uiPoint) { return default(CocosSharp.CCPoint); }
    public CocosSharp.CCPoint ConvertToUi(CocosSharp.CCPoint glPoint) { return default(CocosSharp.CCPoint); }
    public bool DeserializeState() { return default(bool); }
    protected void DrawScene(CocosSharp.CCGameTime gameTime) { }
    public void End() { }
    public abstract void MainLoop(CocosSharp.CCGameTime gameTime);
    public void Pause() { }
    public void PopScene() { }
    public void PopScene(float t, CocosSharp.CCTransitionScene s) { }
    public void PopToRootScene() { }
    public void PopToSceneStackLevel(int level) { }
    public void PurgeCachedData() { }
    protected void PurgeDirector() { }
    public void PushScene(CocosSharp.CCScene pScene) { }
    public void ReplaceScene(CocosSharp.CCScene pScene) { }
    public void ResetSceneStack() { }
    public void Resume() { }
    public void ResumeFromBackground() { }
    public void RunWithScene(CocosSharp.CCScene pScene) { }
    public void SerializeState() { }
    protected void SetDefaultValues() { }
    protected void SetNextScene() { }
    public void SetOpenGlView() { }
    public void SetViewport() { }
    public abstract void StartAnimation();
    public abstract void StopAnimation();
    public void Update(CocosSharp.CCGameTime gameTime) { }
  }
  public enum CCDirectorProjection {
    // Fields
    Custom = 2,
    Default = 1,
    Projection2D = 0,
    Projection3D = 1,
  }
  public partial class CCDisplayLinkDirector : CocosSharp.CCDirector {
    // Constructors
    public CCDisplayLinkDirector() { }
     
    // Properties
    public override double AnimationInterval { get { return default(double); } set { } }
     
    // Methods
    public override void MainLoop(CocosSharp.CCGameTime gameTime) { }
    public override void StartAnimation() { }
    public override void StopAnimation() { }
  }
  [System.FlagsAttribute]
  public enum CCDisplayOrientation {
    // Fields
    Default = 0,
    LandscapeLeft = 1,
    LandscapeRight = 2,
    Portrait = 4,
    PortraitDown = 8,
    Unknown = 16,
  }
  public partial class CCDrawingPrimitives {
    // Constructors
    public CCDrawingPrimitives() { }
     
    // Properties
    public CocosSharp.CCColor4B DefaultColor { get { return default(CocosSharp.CCColor4B); } set { } }
     
    // Methods
    public static void Begin() { }
    public static void DrawArc(CocosSharp.CCRect rect, int startAngle, int sweepAngle, CocosSharp.CCColor4B color) { }
    public static void DrawArc(int x, int y, int width, int height, int startAngle, int sweepAngle, CocosSharp.CCColor4B color) { }
    public static void DrawCardinalSpline(System.Collections.Generic.List<CocosSharp.CCPoint> config, float tension, int segments) { }
    public static void DrawCatmullRom(System.Collections.Generic.List<CocosSharp.CCPoint> points, int segments) { }
    public static void DrawCircle(CocosSharp.CCPoint center, float radius, float angle, int segments, bool drawLineToCenter, CocosSharp.CCColor4B color) { }
    public static void DrawCubicBezier(CocosSharp.CCPoint origin, CocosSharp.CCPoint control1, CocosSharp.CCPoint control2, CocosSharp.CCPoint destination, int segments, CocosSharp.CCColor4B color) { }
    public static void DrawEllips(int x, int y, int width, int height, CocosSharp.CCColor4B color) { }
    public static void DrawEllipse(CocosSharp.CCRect rect, CocosSharp.CCColor4B color) { }
    public static void DrawLine(CocosSharp.CCPoint origin, CocosSharp.CCPoint destination, CocosSharp.CCColor4B color) { }
    public static void DrawPie(CocosSharp.CCRect rect, int startAngle, int sweepAngle, CocosSharp.CCColor4B color) { }
    public static void DrawPie(int x, int y, int width, int height, int startAngle, int sweepAngle, CocosSharp.CCColor4B color) { }
    public static void DrawPoint(CocosSharp.CCPoint point) { }
    public static void DrawPoint(CocosSharp.CCPoint point, float size) { }
    public static void DrawPoint(CocosSharp.CCPoint p, float size, CocosSharp.CCColor4B color) { }
    public static void DrawPoints(CocosSharp.CCPoint[] points, int numberOfPoints, float size, CocosSharp.CCColor4B color) { }
    public static void DrawPoints(CocosSharp.CCPoint[] points, float size, CocosSharp.CCColor4B color) { }
    public static void DrawPoly(CocosSharp.CCPoint[] vertices, int numOfVertices, bool closePolygon, CocosSharp.CCColor4B color) { }
    public static void DrawPoly(CocosSharp.CCPoint[] vertices, int numOfVertices, bool closePolygon, bool fill, CocosSharp.CCColor4B color) { }
    public static void DrawQuadBezier(CocosSharp.CCPoint origin, CocosSharp.CCPoint control, CocosSharp.CCPoint destination, int segments, CocosSharp.CCColor4B color) { }
    public static void DrawRect(CocosSharp.CCRect rect, CocosSharp.CCColor4B color) { }
    public static void DrawSolidPoly(CocosSharp.CCPoint[] vertices, int count, CocosSharp.CCColor4B color) { }
    public static void DrawSolidPoly(CocosSharp.CCPoint[] vertices, int count, CocosSharp.CCColor4B color, bool outline) { }
    public static void DrawSolidRect(CocosSharp.CCPoint origin, CocosSharp.CCPoint destination, CocosSharp.CCColor4B color) { }
    public static void End() { }
  }
  public static partial class CCDrawManager {
    // Fields
    public static string DefaultFont;
    public static int DrawCount;
     
    // Properties
    public static bool DepthTest { get { return default(bool); } set { } }
    public static CocosSharp.CCSize DesignResolutionSize { get { return default(CocosSharp.CCSize); } }
    public static CocosSharp.CCSize FrameSize { get { return default(CocosSharp.CCSize); } set { } }
    public static CocosSharp.CCResolutionPolicy ResolutionPolicy { get { return default(CocosSharp.CCResolutionPolicy); } }
    public static float ScaleX { get { return default(float); } }
    public static float ScaleY { get { return default(float); } }
    public static CocosSharp.CCRect ScissorRect { get { return default(CocosSharp.CCRect); } }
    public static bool ScissorRectEnabled { get { return default(bool); } set { } }
    public static CocosSharp.CCDisplayOrientation SupportedOrientations { get { return default(CocosSharp.CCDisplayOrientation); } set { } }
    public static bool TextureEnabled { get { return default(bool); } set { } }
    public static bool VertexColorEnabled { get { return default(bool); } set { } }
    public static CocosSharp.CCRect ViewPortRect { get { return default(CocosSharp.CCRect); } }
    public static CocosSharp.CCPoint VisibleOrigin { get { return default(CocosSharp.CCPoint); } }
    public static CocosSharp.CCSize VisibleSize { get { return default(CocosSharp.CCSize); } }
     
    // Methods
    public static void BeginDraw() { }
    public static bool BeginDrawMask() { return default(bool); }
    public static bool BeginDrawMask(bool inverted) { return default(bool); }
    public static bool BeginDrawMask(bool inverted, float alphaTreshold) { return default(bool); }
    public static bool BeginDrawMask(float alphaTreshold) { return default(bool); }
    public static void BindTexture(CocosSharp.CCTexture2D texture) { }
    public static void BlendFunc(CocosSharp.CCBlendFunc blendFunc) { }
    public static void Clear(CocosSharp.CCColor4B color) { }
    public static void Clear(CocosSharp.CCColor4B color, float depth) { }
    public static void Clear(CocosSharp.CCColor4B color, float depth, int stencil) { }
    public static void EndDraw() { }
    public static void EndDrawMask() { }
    public static void EndMask() { }
    public static void MultMatrix(CocosSharp.CCAffineTransform transform, float z) { }
    public static void MultMatrix(ref CocosSharp.CCAffineTransform transform, float z) { }
    public static void PopMatrix() { }
    public static void PurgeDrawManager() { }
    public static void PushMatrix() { }
    public static CocosSharp.CCPoint ScreenToWorld(float x, float y) { return default(CocosSharp.CCPoint); }
    public static void SetClearMaskState(int layer, bool inverted) { }
    public static void SetDesignResolutionSize(float width, float height, CocosSharp.CCResolutionPolicy resolutionPolicy) { }
    public static void SetDrawMaskedState(int layer, bool depth) { }
    public static void SetDrawMaskState(int layer, bool inverted) { }
    public static void SetFrameZoom(float zoomFactor) { }
    public static void SetIdentityMatrix() { }
    public static void SetOrientation(CocosSharp.CCDisplayOrientation supportedOrientations) { }
    public static void SetRenderTarget(CocosSharp.CCTexture2D pTexture) { }
    public static void SetScissorInPoints(float x, float y, float w, float h) { }
    public static void SetViewPort(int x, int y, int width, int height) { }
    public static void SetViewPortInPoints(int x, int y, int width, int height) { }
    public static void Translate(float x, float y, int z) { }
  }
  public partial class CCDrawNode : CocosSharp.CCNode {
    // Constructors
    public CCDrawNode() { }
     
    // Properties
    public CocosSharp.CCBlendFunc BlendFunc { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCBlendFunc); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
     
    // Methods
    public void Clear() { }
    protected override void Draw() { }
    public void DrawCircle(CocosSharp.CCPoint center, float radius, CocosSharp.CCColor4B color) { }
    public void DrawCircle(CocosSharp.CCPoint center, float radius, float angle, int segments, CocosSharp.CCColor4B color) { }
    public void DrawDot(CocosSharp.CCPoint pos, float radius, CocosSharp.CCColor4F color) { }
    public void DrawPolygon(CocosSharp.CCPoint[] verts, int count, CocosSharp.CCColor4F fillColor, float borderWidth, CocosSharp.CCColor4F borderColor) { }
    public void DrawRect(CocosSharp.CCRect rect, CocosSharp.CCColor4B color) { }
    public void DrawSegment(CocosSharp.CCPoint from, CocosSharp.CCPoint to, float radius, CocosSharp.CCColor4F color) { }
  }
  public partial class CCEaseBackIn : CocosSharp.CCActionEase {
    // Constructors
    public CCEaseBackIn(CocosSharp.CCActionInterval pAction) { }
     
    // Methods
    public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCEaseBackInOut : CocosSharp.CCActionEase {
    // Constructors
    public CCEaseBackInOut(CocosSharp.CCActionInterval pAction) { }
     
    // Methods
    public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCEaseBackInOutState : CocosSharp.CCActionEaseState {
    // Constructors
    public CCEaseBackInOutState(CocosSharp.CCEaseBackInOut action, CocosSharp.CCNode target) : base (default(CocosSharp.CCActionEase), default(CocosSharp.CCNode)) { }
     
    // Methods
    public override void Update(float time) { }
  }
  public partial class CCEaseBackInState : CocosSharp.CCActionEaseState {
    // Constructors
    public CCEaseBackInState(CocosSharp.CCEaseBackIn action, CocosSharp.CCNode target) : base (default(CocosSharp.CCActionEase), default(CocosSharp.CCNode)) { }
     
    // Methods
    public override void Update(float time) { }
  }
  public partial class CCEaseBackOut : CocosSharp.CCActionEase {
    // Constructors
    public CCEaseBackOut(CocosSharp.CCActionInterval pAction) { }
     
    // Methods
    public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCEaseBackOutState : CocosSharp.CCActionEaseState {
    // Constructors
    public CCEaseBackOutState(CocosSharp.CCEaseBackOut action, CocosSharp.CCNode target) : base (default(CocosSharp.CCActionEase), default(CocosSharp.CCNode)) { }
     
    // Methods
    public override void Update(float time) { }
  }
  public partial class CCEaseBounceIn : CocosSharp.CCActionEase {
    // Constructors
    public CCEaseBounceIn(CocosSharp.CCActionInterval pAction) { }
     
    // Methods
    public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCEaseBounceInOut : CocosSharp.CCActionEase {
    // Constructors
    public CCEaseBounceInOut(CocosSharp.CCActionInterval pAction) { }
     
    // Methods
    public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCEaseBounceInOutState : CocosSharp.CCActionEaseState {
    // Constructors
    public CCEaseBounceInOutState(CocosSharp.CCEaseBounceInOut action, CocosSharp.CCNode target) : base (default(CocosSharp.CCActionEase), default(CocosSharp.CCNode)) { }
     
    // Methods
    public override void Update(float time) { }
  }
  public partial class CCEaseBounceInState : CocosSharp.CCActionEaseState {
    // Constructors
    public CCEaseBounceInState(CocosSharp.CCEaseBounceIn action, CocosSharp.CCNode target) : base (default(CocosSharp.CCActionEase), default(CocosSharp.CCNode)) { }
     
    // Methods
    public override void Update(float time) { }
  }
  public partial class CCEaseBounceOut : CocosSharp.CCActionEase {
    // Constructors
    public CCEaseBounceOut(CocosSharp.CCActionInterval pAction) { }
     
    // Methods
    public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCEaseBounceOutState : CocosSharp.CCActionEaseState {
    // Constructors
    public CCEaseBounceOutState(CocosSharp.CCEaseBounceOut action, CocosSharp.CCNode target) : base (default(CocosSharp.CCActionEase), default(CocosSharp.CCNode)) { }
     
    // Methods
    public override void Update(float time) { }
  }
  public partial class CCEaseCustom : CocosSharp.CCActionEase {
    // Constructors
    public CCEaseCustom(CocosSharp.CCActionInterval pAction, System.Func<System.Single, System.Single> easeFunc) { }
     
    // Properties
    public System.Func<System.Single, System.Single> EaseFunc { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Func<System.Single, System.Single>); } }
     
    // Methods
    public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCEaseCustomState : CocosSharp.CCActionEaseState {
    // Constructors
    public CCEaseCustomState(CocosSharp.CCEaseCustom action, CocosSharp.CCNode target) : base (default(CocosSharp.CCActionEase), default(CocosSharp.CCNode)) { }
     
    // Properties
    protected System.Func<System.Single, System.Single> EaseFunc { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Func<System.Single, System.Single>); } }
     
    // Methods
    public override void Update(float time) { }
  }
  public partial class CCEaseElastic : CocosSharp.CCActionEase {
    // Constructors
    public CCEaseElastic(CocosSharp.CCActionInterval pAction) { }
    public CCEaseElastic(CocosSharp.CCActionInterval pAction, float fPeriod) { }
     
    // Properties
    public float Period { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } }
     
    // Methods
    public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCEaseElasticIn : CocosSharp.CCEaseElastic {
    // Constructors
    public CCEaseElasticIn(CocosSharp.CCActionInterval pAction) : base (default(CocosSharp.CCActionInterval), default(float)) { }
    public CCEaseElasticIn(CocosSharp.CCActionInterval pAction, float fPeriod) : base (default(CocosSharp.CCActionInterval), default(float)) { }
     
    // Methods
    public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCEaseElasticInOut : CocosSharp.CCEaseElastic {
    // Constructors
    public CCEaseElasticInOut(CocosSharp.CCActionInterval pAction) : base (default(CocosSharp.CCActionInterval), default(float)) { }
    public CCEaseElasticInOut(CocosSharp.CCActionInterval pAction, float fPeriod) : base (default(CocosSharp.CCActionInterval), default(float)) { }
     
    // Methods
    public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCEaseElasticInOutState : CocosSharp.CCEaseElasticState {
    // Constructors
    public CCEaseElasticInOutState(CocosSharp.CCEaseElasticInOut action, CocosSharp.CCNode target) : base (default(CocosSharp.CCEaseElastic), default(CocosSharp.CCNode)) { }
     
    // Methods
    public override void Update(float time) { }
  }
  public partial class CCEaseElasticInState : CocosSharp.CCEaseElasticState {
    // Constructors
    public CCEaseElasticInState(CocosSharp.CCEaseElasticIn action, CocosSharp.CCNode target) : base (default(CocosSharp.CCEaseElastic), default(CocosSharp.CCNode)) { }
     
    // Methods
    public override void Update(float time) { }
  }
  public partial class CCEaseElasticOut : CocosSharp.CCEaseElastic {
    // Constructors
    public CCEaseElasticOut(CocosSharp.CCActionInterval pAction) : base (default(CocosSharp.CCActionInterval), default(float)) { }
    public CCEaseElasticOut(CocosSharp.CCActionInterval pAction, float fPeriod) : base (default(CocosSharp.CCActionInterval), default(float)) { }
     
    // Methods
    public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCEaseElasticOutState : CocosSharp.CCEaseElasticState {
    // Constructors
    public CCEaseElasticOutState(CocosSharp.CCEaseElasticOut action, CocosSharp.CCNode target) : base (default(CocosSharp.CCEaseElastic), default(CocosSharp.CCNode)) { }
     
    // Methods
    public override void Update(float time) { }
  }
  public partial class CCEaseElasticState : CocosSharp.CCActionEaseState {
    // Constructors
    public CCEaseElasticState(CocosSharp.CCEaseElastic action, CocosSharp.CCNode target) : base (default(CocosSharp.CCActionEase), default(CocosSharp.CCNode)) { }
     
    // Properties
    protected float Period { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } }
     
    // Methods
  }
  public partial class CCEaseExponentialIn : CocosSharp.CCActionEase {
    // Constructors
    public CCEaseExponentialIn(CocosSharp.CCActionInterval pAction) { }
     
    // Methods
    public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCEaseExponentialInOut : CocosSharp.CCActionEase {
    // Constructors
    public CCEaseExponentialInOut(CocosSharp.CCActionInterval pAction) { }
     
    // Methods
    public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCEaseExponentialInOutState : CocosSharp.CCActionEaseState {
    // Constructors
    public CCEaseExponentialInOutState(CocosSharp.CCEaseExponentialInOut action, CocosSharp.CCNode target) : base (default(CocosSharp.CCActionEase), default(CocosSharp.CCNode)) { }
     
    // Methods
    public override void Update(float time) { }
  }
  public partial class CCEaseExponentialInState : CocosSharp.CCActionEaseState {
    // Constructors
    public CCEaseExponentialInState(CocosSharp.CCEaseExponentialIn action, CocosSharp.CCNode target) : base (default(CocosSharp.CCActionEase), default(CocosSharp.CCNode)) { }
     
    // Methods
    public override void Update(float time) { }
  }
  public partial class CCEaseExponentialOut : CocosSharp.CCActionEase {
    // Constructors
    public CCEaseExponentialOut(CocosSharp.CCActionInterval pAction) { }
     
    // Methods
    public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCEaseExponentialOutState : CocosSharp.CCActionEaseState {
    // Constructors
    public CCEaseExponentialOutState(CocosSharp.CCEaseExponentialOut action, CocosSharp.CCNode target) : base (default(CocosSharp.CCActionEase), default(CocosSharp.CCNode)) { }
     
    // Methods
    public override void Update(float time) { }
  }
  public partial class CCEaseIn : CocosSharp.CCEaseRateAction {
    // Constructors
    public CCEaseIn(CocosSharp.CCActionInterval pAction, float fRate) : base (default(CocosSharp.CCActionInterval), default(float)) { }
     
    // Methods
    public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCEaseInOut : CocosSharp.CCEaseRateAction {
    // Constructors
    public CCEaseInOut(CocosSharp.CCActionInterval pAction, float fRate) : base (default(CocosSharp.CCActionInterval), default(float)) { }
     
    // Methods
    public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCEaseInOutState : CocosSharp.CCEaseRateActionState {
    // Constructors
    public CCEaseInOutState(CocosSharp.CCEaseInOut action, CocosSharp.CCNode target) : base (default(CocosSharp.CCEaseRateAction), default(CocosSharp.CCNode)) { }
     
    // Methods
    public override void Update(float time) { }
  }
  public partial class CCEaseInState : CocosSharp.CCEaseRateActionState {
    // Constructors
    public CCEaseInState(CocosSharp.CCEaseIn action, CocosSharp.CCNode target) : base (default(CocosSharp.CCEaseRateAction), default(CocosSharp.CCNode)) { }
     
    // Methods
    public override void Update(float time) { }
  }
  public partial class CCEaseOut : CocosSharp.CCEaseRateAction {
    // Constructors
    public CCEaseOut(CocosSharp.CCActionInterval pAction, float fRate) : base (default(CocosSharp.CCActionInterval), default(float)) { }
     
    // Methods
    public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCEaseOutState : CocosSharp.CCEaseRateActionState {
    // Constructors
    public CCEaseOutState(CocosSharp.CCEaseOut action, CocosSharp.CCNode target) : base (default(CocosSharp.CCEaseRateAction), default(CocosSharp.CCNode)) { }
     
    // Methods
    public override void Update(float time) { }
  }
  public partial class CCEaseRateAction : CocosSharp.CCActionEase {
    // Constructors
    public CCEaseRateAction(CocosSharp.CCActionInterval pAction, float fRate) { }
     
    // Properties
    public float Rate { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } }
     
    // Methods
    public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCEaseRateActionState : CocosSharp.CCActionEaseState {
    // Constructors
    public CCEaseRateActionState(CocosSharp.CCEaseRateAction action, CocosSharp.CCNode target) : base (default(CocosSharp.CCActionEase), default(CocosSharp.CCNode)) { }
     
    // Properties
    protected float Rate { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } }
     
    // Methods
    public override void Update(float time) { }
  }
  public partial class CCEaseSineIn : CocosSharp.CCActionEase {
    // Constructors
    public CCEaseSineIn(CocosSharp.CCActionInterval pAction) { }
     
    // Methods
    public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCEaseSineInOut : CocosSharp.CCActionEase {
    // Constructors
    public CCEaseSineInOut(CocosSharp.CCActionInterval pAction) { }
     
    // Methods
    public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCEaseSineInOutState : CocosSharp.CCActionEaseState {
    // Constructors
    public CCEaseSineInOutState(CocosSharp.CCEaseSineInOut action, CocosSharp.CCNode target) : base (default(CocosSharp.CCActionEase), default(CocosSharp.CCNode)) { }
     
    // Methods
    public override void Update(float time) { }
  }
  public partial class CCEaseSineInState : CocosSharp.CCActionEaseState {
    // Constructors
    public CCEaseSineInState(CocosSharp.CCEaseSineIn action, CocosSharp.CCNode target) : base (default(CocosSharp.CCActionEase), default(CocosSharp.CCNode)) { }
     
    // Methods
    public override void Update(float time) { }
  }
  public partial class CCEaseSineOut : CocosSharp.CCActionEase {
    // Constructors
    public CCEaseSineOut(CocosSharp.CCActionInterval pAction) { }
     
    // Methods
    public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCEaseSineOutState : CocosSharp.CCActionEaseState {
    // Constructors
    public CCEaseSineOutState(CocosSharp.CCEaseSineOut action, CocosSharp.CCNode target) : base (default(CocosSharp.CCActionEase), default(CocosSharp.CCNode)) { }
     
    // Methods
    public override void Update(float time) { }
  }
  public enum CCEmitterMode {
    // Fields
    Gravity = 0,
    Radius = 1,
  }
  public partial class CCEvent {
    internal CCEvent() { }
    // Properties
    public CocosSharp.CCNode CurrentTarget { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCNode); } }
    public bool IsStopped { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
     
    // Methods
    public void StopPropogation() { }
  }
  public partial class CCEventAccelerate : CocosSharp.CCEvent {
    internal CCEventAccelerate() { }
    // Properties
    public CocosSharp.CCAcceleration Acceleration { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCAcceleration); } }
     
    // Methods
  }
  public enum CCEventCode {
    // Fields
    BEGAN = 0,
    CANCELLED = 3,
    ENDED = 2,
    MOVED = 1,
  }
  public partial class CCEventCustom : CocosSharp.CCEvent {
    // Constructors
    public CCEventCustom(string eventName) { }
    public CCEventCustom(string eventName, object userData) { }
     
    // Properties
    public string EventName { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(string); } }
    public object UserData { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(object); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
     
    // Methods
  }
  public partial class CCEventDispatcher {
    // Constructors
    public CCEventDispatcher() { }
     
    // Properties
    public bool IsEnabled { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    protected CocosSharp.CCNode MarkDirty { set { } }
     
    // Methods
    public CocosSharp.CCEventListenerCustom AddCustomEventListener(string eventName, System.Action<CocosSharp.CCEventCustom> callback) { return default(CocosSharp.CCEventListenerCustom); }
    public void AddEventListener(CocosSharp.CCEventListener listener, CocosSharp.CCNode node) { }
    public void AddEventListener(CocosSharp.CCEventListener listener, int fixedPriority) { }
    public void DispatchEvent(CocosSharp.CCEvent eventToDispatch) { }
    public void Pause(CocosSharp.CCNode target, bool recursive=false) { }
    public void RemoveAll() { }
    public void RemoveEventListener(CocosSharp.CCEventListener listener) { }
    public void RemoveEventListeners(CocosSharp.CCEventListenerType listenerType) { }
    public void RemoveEventListeners(CocosSharp.CCNode target, bool recursive=false) { }
    public void Resume(CocosSharp.CCNode target, bool recursive=false) { }
    public void SetPriority(CocosSharp.CCEventListener listener, int fixedPriority) { }
  }
  public partial class CCEventGamePad : CocosSharp.CCEvent {
    internal CCEventGamePad() { }
    // Properties
    public CocosSharp.CCGamePadEventType GamePadEventType { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCGamePadEventType); } }
     
    // Methods
  }
  public partial class CCEventGamePadButton : CocosSharp.CCEventGamePad {
    internal CCEventGamePadButton() { }
    // Properties
    public CocosSharp.CCGamePadButtonStatus A { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCGamePadButtonStatus); } }
    public CocosSharp.CCGamePadButtonStatus B { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCGamePadButtonStatus); } }
    public CocosSharp.CCGamePadButtonStatus Back { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCGamePadButtonStatus); } }
    public CocosSharp.CCGamePadButtonStatus LeftShoulder { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCGamePadButtonStatus); } }
    public CocosSharp.CCPlayerIndex Player { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCPlayerIndex); } }
    public CocosSharp.CCGamePadButtonStatus RightShoulder { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCGamePadButtonStatus); } }
    public CocosSharp.CCGamePadButtonStatus Start { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCGamePadButtonStatus); } }
    public CocosSharp.CCGamePadButtonStatus System { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCGamePadButtonStatus); } }
    public CocosSharp.CCGamePadButtonStatus X { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCGamePadButtonStatus); } }
    public CocosSharp.CCGamePadButtonStatus Y { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCGamePadButtonStatus); } }
     
    // Methods
  }
  public partial class CCEventGamePadConnection : CocosSharp.CCEventGamePad {
    internal CCEventGamePadConnection() { }
    // Properties
    public bool IsConnected { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } }
    public CocosSharp.CCPlayerIndex Player { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCPlayerIndex); } }
     
    // Methods
  }
  public partial class CCEventGamePadDPad : CocosSharp.CCEventGamePad {
    internal CCEventGamePadDPad() { }
    // Properties
    public CocosSharp.CCGamePadButtonStatus Down { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCGamePadButtonStatus); } }
    public CocosSharp.CCGamePadButtonStatus Left { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCGamePadButtonStatus); } }
    public CocosSharp.CCPlayerIndex Player { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCPlayerIndex); } }
    public CocosSharp.CCGamePadButtonStatus Right { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCGamePadButtonStatus); } }
    public CocosSharp.CCGamePadButtonStatus Up { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCGamePadButtonStatus); } }
     
    // Methods
  }
  public partial class CCEventGamePadStick : CocosSharp.CCEventGamePad {
    internal CCEventGamePadStick() { }
    // Properties
    public CocosSharp.CCGameStickStatus Left { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCGameStickStatus); } }
    public CocosSharp.CCPlayerIndex Player { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCPlayerIndex); } }
    public CocosSharp.CCGameStickStatus Right { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCGameStickStatus); } }
     
    // Methods
  }
  public partial class CCEventGamePadTrigger : CocosSharp.CCEventGamePad {
    internal CCEventGamePadTrigger() { }
    // Properties
    public float Left { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } }
    public CocosSharp.CCPlayerIndex Player { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCPlayerIndex); } }
    public float Right { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } }
     
    // Methods
  }
  public partial class CCEventKeyboard : CocosSharp.CCEvent {
    internal CCEventKeyboard() { }
    // Properties
    public CocosSharp.CCKeyboardEventType KeyboardEventType { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCKeyboardEventType); } }
    public CocosSharp.CCKeyboardState KeyboardState { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCKeyboardState); } }
    public CocosSharp.CCKeys Keys { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCKeys); } }
     
    // Methods
  }
  public partial class CCEventListener : System.IDisposable {
    // Constructors
    protected CCEventListener() { }
    protected CCEventListener(CocosSharp.CCEventListener eventListener) { }
    protected CCEventListener(CocosSharp.CCEventListenerType type, string listenerID) { }
    protected CCEventListener(CocosSharp.CCEventListenerType type, string listenerID, System.Action<CocosSharp.CCEvent> callback) { }
     
    // Properties
    public virtual bool IsAvailable { get { return default(bool); } }
    public virtual bool IsEnabled { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
     
    // Methods
    public virtual CocosSharp.CCEventListener Copy() { return default(CocosSharp.CCEventListener); }
    public void Dispose() { }
    protected virtual void Dispose(bool disposing) { }
    ~CCEventListener() { }
  }
  public partial class CCEventListenerAccelerometer : CocosSharp.CCEventListener {
    // Fields
    public static string LISTENER_ID;
     
    // Constructors
    public CCEventListenerAccelerometer() { }
     
    // Properties
    public override bool IsAvailable { get { return default(bool); } }
    public System.Action<CocosSharp.CCEventAccelerate> OnAccelerate { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Action<CocosSharp.CCEventAccelerate>); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
     
    // Methods
    public override CocosSharp.CCEventListener Copy() { return default(CocosSharp.CCEventListener); }
  }
  public partial class CCEventListenerCustom : CocosSharp.CCEventListener {
    // Constructors
    public CCEventListenerCustom(string eventName, System.Action<CocosSharp.CCEventCustom> callback) { }
     
    // Properties
    public override bool IsAvailable { get { return default(bool); } }
    public System.Action<CocosSharp.CCEventCustom> OnCustomEvent { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Action<CocosSharp.CCEventCustom>); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
     
    // Methods
  }
  public partial class CCEventListenerGamePad : CocosSharp.CCEventListener {
    // Fields
    public static string LISTENER_ID;
     
    // Constructors
    public CCEventListenerGamePad() { }
     
    // Properties
    public override bool IsAvailable { get { return default(bool); } }
    public System.Action<CocosSharp.CCEventGamePadButton> OnButtonStatus { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Action<CocosSharp.CCEventGamePadButton>); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public System.Action<CocosSharp.CCEventGamePadConnection> OnConnectionStatus { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Action<CocosSharp.CCEventGamePadConnection>); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public System.Action<CocosSharp.CCEventGamePadDPad> OnDPadStatus { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Action<CocosSharp.CCEventGamePadDPad>); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public System.Action<CocosSharp.CCEventGamePadStick> OnStickStatus { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Action<CocosSharp.CCEventGamePadStick>); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public System.Action<CocosSharp.CCEventGamePadTrigger> OnTriggerStatus { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Action<CocosSharp.CCEventGamePadTrigger>); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
     
    // Methods
    public override CocosSharp.CCEventListener Copy() { return default(CocosSharp.CCEventListener); }
  }
  public partial class CCEventListenerKeyboard : CocosSharp.CCEventListener {
    // Fields
    public static string LISTENER_ID;
     
    // Constructors
    public CCEventListenerKeyboard() { }
     
    // Properties
    public override bool IsAvailable { get { return default(bool); } }
    public System.Action<CocosSharp.CCEventKeyboard> OnKeyPressed { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Action<CocosSharp.CCEventKeyboard>); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public System.Action<CocosSharp.CCEventKeyboard> OnKeyReleased { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Action<CocosSharp.CCEventKeyboard>); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
     
    // Methods
    public override CocosSharp.CCEventListener Copy() { return default(CocosSharp.CCEventListener); }
  }
  public partial class CCEventListenerMouse : CocosSharp.CCEventListener {
    // Fields
    public static string LISTENER_ID;
     
    // Constructors
    public CCEventListenerMouse() { }
     
    // Properties
    public override bool IsAvailable { get { return default(bool); } }
    public System.Action<CocosSharp.CCEventMouse> OnMouseDown { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Action<CocosSharp.CCEventMouse>); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public System.Action<CocosSharp.CCEventMouse> OnMouseMove { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Action<CocosSharp.CCEventMouse>); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public System.Action<CocosSharp.CCEventMouse> OnMouseScroll { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Action<CocosSharp.CCEventMouse>); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public System.Action<CocosSharp.CCEventMouse> OnMouseUp { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Action<CocosSharp.CCEventMouse>); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
     
    // Methods
    public override CocosSharp.CCEventListener Copy() { return default(CocosSharp.CCEventListener); }
  }
  public partial class CCEventListenerTouchAllAtOnce : CocosSharp.CCEventListener {
    // Fields
    public static string LISTENER_ID;
     
    // Constructors
    public CCEventListenerTouchAllAtOnce() { }
     
    // Properties
    public override bool IsAvailable { get { return default(bool); } }
    public System.Action<System.Collections.Generic.List<CocosSharp.CCTouch>, CocosSharp.CCEvent> OnTouchesBegan { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Action<System.Collections.Generic.List<CocosSharp.CCTouch>, CocosSharp.CCEvent>); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public System.Action<System.Collections.Generic.List<CocosSharp.CCTouch>, CocosSharp.CCEvent> OnTouchesCancelled { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Action<System.Collections.Generic.List<CocosSharp.CCTouch>, CocosSharp.CCEvent>); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public System.Action<System.Collections.Generic.List<CocosSharp.CCTouch>, CocosSharp.CCEvent> OnTouchesEnded { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Action<System.Collections.Generic.List<CocosSharp.CCTouch>, CocosSharp.CCEvent>); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public System.Action<System.Collections.Generic.List<CocosSharp.CCTouch>, CocosSharp.CCEvent> OnTouchesMoved { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Action<System.Collections.Generic.List<CocosSharp.CCTouch>, CocosSharp.CCEvent>); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
     
    // Methods
    public override CocosSharp.CCEventListener Copy() { return default(CocosSharp.CCEventListener); }
  }
  public partial class CCEventListenerTouchOneByOne : CocosSharp.CCEventListener {
    // Fields
    public static string LISTENER_ID;
     
    // Constructors
    public CCEventListenerTouchOneByOne() { }
     
    // Properties
    public override bool IsAvailable { get { return default(bool); } }
    public bool IsSwallowTouches { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public System.Func<CocosSharp.CCTouch, CocosSharp.CCEvent, System.Boolean> OnTouchBegan { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Func<CocosSharp.CCTouch, CocosSharp.CCEvent, System.Boolean>); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public System.Action<CocosSharp.CCTouch, CocosSharp.CCEvent> OnTouchCancelled { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Action<CocosSharp.CCTouch, CocosSharp.CCEvent>); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public System.Action<CocosSharp.CCTouch, CocosSharp.CCEvent> OnTouchEnded { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Action<CocosSharp.CCTouch, CocosSharp.CCEvent>); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public System.Action<CocosSharp.CCTouch, CocosSharp.CCEvent> OnTouchMoved { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Action<CocosSharp.CCTouch, CocosSharp.CCEvent>); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
     
    // Methods
    public override CocosSharp.CCEventListener Copy() { return default(CocosSharp.CCEventListener); }
  }
  public enum CCEventListenerType {
    // Fields
    ACCELEROMETER = 5,
    CUSTOM = 7,
    GAMEPAD = 6,
    KEYBOARD = 3,
    MOUSE = 4,
    TOUCH_ALL_AT_ONCE = 2,
    TOUCH_ONE_BY_ONE = 1,
    UNKNOWN = 0,
  }
  public partial class CCEventMouse : CocosSharp.CCEvent {
    internal CCEventMouse() { }
    // Properties
    public float CursorX { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } }
    public float CursorY { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } }
    public CocosSharp.CCMouseButton MouseButton { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCMouseButton); } }
    public CocosSharp.CCMouseEventType MouseEventType { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCMouseEventType); } }
    public float ScrollX { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } }
    public float ScrollY { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } }
     
    // Methods
  }
  public partial class CCEventTouch : CocosSharp.CCEvent {
    internal CCEventTouch() { }
    // Properties
    public CocosSharp.CCEventCode EventCode { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCEventCode); } }
    public System.Collections.Generic.List<CocosSharp.CCTouch> Touches { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Collections.Generic.List<CocosSharp.CCTouch>); } }
     
    // Methods
  }
  public enum CCEventType {
    // Fields
    ACCELERATION = 2,
    CUSTOM = 5,
    GAMEPAD = 4,
    KEYBOARD = 1,
    MOUSE = 3,
    TOUCH = 0,
  }
  public partial class CCFadeIn : CocosSharp.CCActionInterval {
    // Constructors
    public CCFadeIn(float d) { }
     
    // Methods
    public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCFadeInState : CocosSharp.CCActionIntervalState {
    // Constructors
    public CCFadeInState(CocosSharp.CCFadeIn action, CocosSharp.CCNode target) : base (default(CocosSharp.CCActionInterval), default(CocosSharp.CCNode)) { }
     
    // Properties
    protected bool OriginalState { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    protected uint Times { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(uint); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
     
    // Methods
    public override void Update(float time) { }
  }
  public partial class CCFadeOut : CocosSharp.CCActionInterval {
    // Constructors
    public CCFadeOut(float d) { }
     
    // Methods
    public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCFadeOutBLTiles : CocosSharp.CCFadeOutTRTiles {
    // Constructors
    public CCFadeOutBLTiles(float duration, CocosSharp.CCGridSize gridSize) : base (default(float)) { }
     
    // Methods
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCFadeOutBLTilesState : CocosSharp.CCFadeOutTRTilesState {
    // Constructors
    public CCFadeOutBLTilesState(CocosSharp.CCFadeOutBLTiles action, CocosSharp.CCNode target) : base (default(CocosSharp.CCFadeOutTRTiles), default(CocosSharp.CCNode)) { }
     
    // Methods
    public override float TestFunc(CocosSharp.CCGridSize pos, float time) { return default(float); }
  }
  public partial class CCFadeOutDownTiles : CocosSharp.CCFadeOutUpTiles {
    // Constructors
    public CCFadeOutDownTiles(float duration, CocosSharp.CCGridSize gridSize) : base (default(float), default(CocosSharp.CCGridSize)) { }
     
    // Methods
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCFadeOutDownTilesState : CocosSharp.CCFadeOutUpTilesState {
    // Constructors
    public CCFadeOutDownTilesState(CocosSharp.CCFadeOutDownTiles action, CocosSharp.CCNode target) : base (default(CocosSharp.CCFadeOutUpTiles), default(CocosSharp.CCNode)) { }
     
    // Methods
    public override float TestFunc(CocosSharp.CCGridSize pos, float time) { return default(float); }
  }
  public partial class CCFadeOutState : CocosSharp.CCActionIntervalState {
    // Constructors
    public CCFadeOutState(CocosSharp.CCFadeOut action, CocosSharp.CCNode target) : base (default(CocosSharp.CCActionInterval), default(CocosSharp.CCNode)) { }
     
    // Methods
    public override void Update(float time) { }
  }
  public partial class CCFadeOutTRTiles : CocosSharp.CCTiledGrid3DAction {
    // Constructors
    public CCFadeOutTRTiles(float duration) : base (default(float)) { }
    public CCFadeOutTRTiles(float duration, CocosSharp.CCGridSize gridSize) : base (default(float)) { }
     
    // Methods
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCFadeOutTRTilesState : CocosSharp.CCTiledGrid3DActionState {
    // Constructors
    public CCFadeOutTRTilesState(CocosSharp.CCFadeOutTRTiles action, CocosSharp.CCNode target) : base (default(CocosSharp.CCTiledGrid3DAction), default(CocosSharp.CCNode)) { }
     
    // Methods
    public virtual float TestFunc(CocosSharp.CCGridSize pos, float time) { return default(float); }
    public virtual void TransformTile(CocosSharp.CCGridSize pos, float distance) { }
    public void TurnOffTile(CocosSharp.CCGridSize pos) { }
    public void TurnOnTile(CocosSharp.CCGridSize pos) { }
    public override void Update(float time) { }
  }
  public partial class CCFadeOutUpTiles : CocosSharp.CCFadeOutTRTiles {
    // Constructors
    public CCFadeOutUpTiles(float duration, CocosSharp.CCGridSize gridSize) : base (default(float)) { }
     
    // Methods
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCFadeOutUpTilesState : CocosSharp.CCFadeOutTRTilesState {
    // Constructors
    public CCFadeOutUpTilesState(CocosSharp.CCFadeOutUpTiles action, CocosSharp.CCNode target) : base (default(CocosSharp.CCFadeOutTRTiles), default(CocosSharp.CCNode)) { }
     
    // Methods
    public override float TestFunc(CocosSharp.CCGridSize pos, float time) { return default(float); }
    public override void TransformTile(CocosSharp.CCGridSize pos, float distance) { }
  }
  public partial class CCFadeTo : CocosSharp.CCActionInterval {
    // Constructors
    public CCFadeTo(float duration, byte opacity) { }
     
    // Properties
    public byte ToOpacity { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(byte); } }
     
    // Methods
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCFadeToState : CocosSharp.CCActionIntervalState {
    // Constructors
    public CCFadeToState(CocosSharp.CCFadeTo action, CocosSharp.CCNode target) : base (default(CocosSharp.CCActionInterval), default(CocosSharp.CCNode)) { }
     
    // Properties
    protected byte FromOpacity { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(byte); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    protected byte ToOpacity { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(byte); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
     
    // Methods
    public override void Update(float time) { }
  }
  public partial class CCFastRandom {
    // Constructors
    public CCFastRandom() { }
    public CCFastRandom(int seed) { }
     
    // Methods
    public int Next() { return default(int); }
    public int Next(int upperBound) { return default(int); }
    public int Next(int lowerBound, int upperBound) { return default(int); }
    public bool NextBool() { return default(bool); }
    public void NextBytes(System.Byte[] buffer) { }
    public double NextDouble() { return default(double); }
    public int NextInt() { return default(int); }
    public uint NextUInt() { return default(uint); }
    public void Reinitialise(int seed) { }
  }
  public partial class CCFileUtils {
    // Constructors
    public CCFileUtils() { }
     
    // Properties
    public static bool IsPopupNotify { get { return default(bool); } set { } }
     
    // Methods
    public static int CCLoadFileIntoMemory(string filename, out System.Char[] file) { file = default(System.Char[]); return default(int); }
    public static string CCRemoveHDSuffixFromFile(string path) { return default(string); }
    public static System.Collections.Generic.Dictionary<System.String, System.Object> DictionaryWithContentsOfFile(string pFileName) { return default(System.Collections.Generic.Dictionary<System.String, System.Object>); }
    public static string FullPathFromRelativeFile(string pszFilename, string pszRelativeFile) { return default(string); }
    public static string FullPathFromRelativePath(string pszRelativePath) { return default(string); }
    public static System.Byte[] GetFileBytes(string pszFileName) { return default(System.Byte[]); }
    public static string GetFileData(string pszFileName) { return default(string); }
    public static System.Char[] GetFileDataFromZip(string pszZipFilePath, string pszFileName, ulong pSize) { return default(System.Char[]); }
    public static System.IO.Stream GetFileStream(string fileName) { return default(System.IO.Stream); }
    public static string GetWriteablePath() { return default(string); }
    public static string RemoveExtension(string fileName) { return default(string); }
    public static void SetResource(string pszZipFileName) { }
    public static void SetResourcePath(string pszResourcePath) { }
  }
  public partial class CCFiniteTimeAction : CocosSharp.CCAction {
    // Constructors
    protected CCFiniteTimeAction() { }
    protected CCFiniteTimeAction(float d) { }
     
    // Properties
    public virtual float Duration { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
     
    // Methods
    public virtual CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCFiniteTimeActionState : CocosSharp.CCActionState {
    // Constructors
    public CCFiniteTimeActionState(CocosSharp.CCFiniteTimeAction action, CocosSharp.CCNode target) : base (default(CocosSharp.CCAction), default(CocosSharp.CCNode)) { }
     
    // Properties
    public virtual float Duration { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
     
    // Methods
  }
  public partial class CCFlipX : CocosSharp.CCActionInstant {
    // Constructors
    public CCFlipX(bool x) { }
     
    // Properties
    public bool FlipX { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } }
     
    // Methods
    public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCFlipX3D : CocosSharp.CCGrid3DAction {
    // Constructors
    public CCFlipX3D(float duration) : base (default(float)) { }
    public CCFlipX3D(float duration, CocosSharp.CCGridSize gridSize) : base (default(float)) { }
     
    // Methods
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCFlipX3DState : CocosSharp.CCGrid3DActionState {
    // Constructors
    public CCFlipX3DState(CocosSharp.CCFlipX3D action, CocosSharp.CCNode target) : base (default(CocosSharp.CCGrid3DAction), default(CocosSharp.CCNode)) { }
     
    // Methods
    public override void Update(float time) { }
  }
  public partial class CCFlipXState : CocosSharp.CCActionInstantState {
    // Constructors
    public CCFlipXState(CocosSharp.CCFlipX action, CocosSharp.CCNode target) : base (default(CocosSharp.CCActionInstant), default(CocosSharp.CCNode)) { }
  }
  public partial class CCFlipY : CocosSharp.CCActionInstant {
    // Constructors
    public CCFlipY(bool y) { }
     
    // Properties
    public bool FlipY { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } }
     
    // Methods
    public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCFlipY3D : CocosSharp.CCFlipX3D {
    // Constructors
    public CCFlipY3D(float duration) : base (default(float), default(CocosSharp.CCGridSize)) { }
     
    // Methods
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCFlipY3DState : CocosSharp.CCFlipX3DState {
    // Constructors
    public CCFlipY3DState(CocosSharp.CCFlipY3D action, CocosSharp.CCNode target) : base (default(CocosSharp.CCFlipX3D), default(CocosSharp.CCNode)) { }
     
    // Methods
    public override void Update(float time) { }
  }
  public partial class CCFlipYState : CocosSharp.CCActionInstantState {
    // Constructors
    public CCFlipYState(CocosSharp.CCFlipY action, CocosSharp.CCNode target) : base (default(CocosSharp.CCActionInstant), default(CocosSharp.CCNode)) { }
  }
  public delegate void CCFocusChangeDelegate(CocosSharp.ICCFocusable prev, CocosSharp.ICCFocusable current);
  public partial class CCFocusManager {
    internal CCFocusManager() { }
    // Fields
    public static float MenuScrollDelay;
     
    // Properties
    public bool Enabled { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public static CocosSharp.CCFocusManager Instance { get { return default(CocosSharp.CCFocusManager); } }
    public CocosSharp.ICCFocusable ItemWithFocus { get { return default(CocosSharp.ICCFocusable); } }
     
    // Events
    public event CocosSharp.CCFocusChangeDelegate OnFocusChanged { add { } remove { } }
     
    // Methods
    public void Add(params CocosSharp.ICCFocusable[] focusItems) { }
    public void FocusNextItem() { }
    public void FocusPreviousItem() { }
    public void Remove(params CocosSharp.ICCFocusable[] focusItems) { }
  }
  public partial class CCFollow : CocosSharp.CCAction {
    // Constructors
    public CCFollow(CocosSharp.CCNode followedNode, CocosSharp.CCRect rect) { }
     
    // Properties
    public bool BoundaryFullyCovered { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } }
    public bool BoundarySet { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } }
    protected internal CocosSharp.CCNode FollowedNode { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCNode); } }
     
    // Methods
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCFollowState : CocosSharp.CCActionState {
    // Constructors
    public CCFollowState(CocosSharp.CCFollow action, CocosSharp.CCNode target) : base (default(CocosSharp.CCAction), default(CocosSharp.CCNode)) { }
     
    // Properties
    protected CocosSharp.CCFollow FollowAction { get { return default(CocosSharp.CCFollow); } }
    public override bool IsDone { get { return default(bool); } }
     
    // Methods
    public override void Step(float dt) { }
    public override void Stop() { }
  }
  public static partial class CCFPSImage {
    // Fields
    public static System.Byte[] PngData;
  }
  public enum CCGamePadButtonStatus {
    // Fields
    NotApplicable = 3,
    Pressed = 0,
    Released = 1,
    Tapped = 2,
  }
  public enum CCGamePadEventType {
    // Fields
    GAMEPAD_BUTTON = 1,
    GAMEPAD_CONNECTION = 5,
    GAMEPAD_DPAD = 2,
    GAMEPAD_NONE = 0,
    GAMEPAD_STICK = 3,
    GAMEPAD_TRIGGER = 4,
  }
  [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
  public partial struct CCGameStickStatus {
    // Fields
    public CocosSharp.CCPoint Direction;
    public bool IsDown;
    public float Magnitude;
  }
  public partial class CCGameTime {
    // Constructors
    public CCGameTime() { }
    public CCGameTime(System.TimeSpan totalGameTime, System.TimeSpan elapsedGameTime) { }
    public CCGameTime(System.TimeSpan totalRealTime, System.TimeSpan elapsedRealTime, bool isRunningSlowly) { }
     
    // Properties
    public System.TimeSpan ElapsedGameTime { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.TimeSpan); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public bool IsRunningSlowly { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public System.TimeSpan TotalGameTime { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.TimeSpan); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
     
    // Methods
  }
  public enum CCGlesVersion {
    // Fields
    GLES_VER_1_0 = 1,
    GLES_VER_1_1 = 2,
    GLES_VER_2_0 = 3,
    GLES_VER_INVALID = 0,
  }
  public partial class CCGrabber {
    // Constructors
    public CCGrabber() { }
     
    // Methods
    public void AfterRender(CocosSharp.CCTexture2D pTexture) { }
    public void BeforeRender(CocosSharp.CCTexture2D pTexture) { }
    public void Grab(CocosSharp.CCTexture2D pTexture) { }
  }
  public partial class CCGraphicsResource : System.IDisposable {
    // Constructors
    public CCGraphicsResource() { }
     
    // Properties
    public bool IsDisposed { get { return default(bool); } }
     
    // Methods
    public void Dispose() { }
    protected virtual void Dispose(bool disposing) { }
    ~CCGraphicsResource() { }
    public virtual void Reinit() { }
  }
  public partial class CCGrid3D : CocosSharp.CCGridBase {
    // Fields
    protected System.UInt16[] m_pIndices;
    protected CocosSharp.CCVertex3F[] m_pOriginalVertices;
     
    // Constructors
    public CCGrid3D(CocosSharp.CCGridSize gridSize) { }
    public CCGrid3D(CocosSharp.CCGridSize gridSize, CocosSharp.CCSize size) { }
    public CCGrid3D(CocosSharp.CCGridSize gridSize, CocosSharp.CCTexture2D pTexture, bool bFlipped) { }
     
    // Methods
    public override void Blit() { }
    public override void CalculateVertexPoints() { }
    public CocosSharp.CCVertex3F OriginalVertex(CocosSharp.CCGridSize pos) { return default(CocosSharp.CCVertex3F); }
    public CocosSharp.CCVertex3F OriginalVertex(int x, int y) { return default(CocosSharp.CCVertex3F); }
    public override void Reuse() { }
    public void SetVertex(CocosSharp.CCGridSize pos, ref CocosSharp.CCVertex3F vertex) { }
    public void SetVertex(int x, int y, ref CocosSharp.CCVertex3F vertex) { }
    public CocosSharp.CCVertex3F Vertex(CocosSharp.CCGridSize pos) { return default(CocosSharp.CCVertex3F); }
    public CocosSharp.CCVertex3F Vertex(int x, int y) { return default(CocosSharp.CCVertex3F); }
  }
  public partial class CCGrid3DAction : CocosSharp.CCGridAction {
    // Constructors
    protected CCGrid3DAction(float duration) : base (default(float)) { }
    protected CCGrid3DAction(float duration, CocosSharp.CCGridSize gridSize) : base (default(float)) { }
    protected CCGrid3DAction(float duration, CocosSharp.CCGridSize gridSize, float amplitude) : base (default(float)) { }
     
    // Methods
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCGrid3DActionState : CocosSharp.CCGridActionState {
    // Constructors
    public CCGrid3DActionState(CocosSharp.CCGrid3DAction action, CocosSharp.CCNode target) : base (default(CocosSharp.CCGridAction), default(CocosSharp.CCNode)) { }
     
    // Properties
    public override CocosSharp.CCGridBase Grid { get { return default(CocosSharp.CCGridBase); } protected set { } }
     
    // Methods
    public CocosSharp.CCVertex3F OriginalVertex(CocosSharp.CCGridSize pos) { return default(CocosSharp.CCVertex3F); }
    public CocosSharp.CCVertex3F OriginalVertex(int x, int y) { return default(CocosSharp.CCVertex3F); }
    public void SetVertex(CocosSharp.CCGridSize pos, ref CocosSharp.CCVertex3F vertex) { }
    public void SetVertex(int x, int y, ref CocosSharp.CCVertex3F vertex) { }
    public CocosSharp.CCVertex3F Vertex(CocosSharp.CCGridSize pos) { return default(CocosSharp.CCVertex3F); }
    public CocosSharp.CCVertex3F Vertex(int x, int y) { return default(CocosSharp.CCVertex3F); }
  }
  public partial class CCGridAction : CocosSharp.CCAmplitudeAction {
    // Constructors
    public CCGridAction(float duration) : base (default(float), default(float)) { }
    public CCGridAction(float duration, CocosSharp.CCGridSize gridSize) : base (default(float), default(float)) { }
    protected CCGridAction(float duration, CocosSharp.CCGridSize gridSize, float amplitude) : base (default(float), default(float)) { }
     
    // Properties
    protected internal CocosSharp.CCGridSize GridSize { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCGridSize); } }
     
    // Methods
    public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCGridActionState : CocosSharp.CCAmplitudeActionState {
    // Constructors
    public CCGridActionState(CocosSharp.CCGridAction action, CocosSharp.CCNode target) : base (default(CocosSharp.CCAmplitudeAction), default(CocosSharp.CCNode)) { }
     
    // Properties
    public virtual CocosSharp.CCGridBase Grid { get { return default(CocosSharp.CCGridBase); } protected set { } }
    protected CocosSharp.CCGridSize GridSize { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCGridSize); } }
     
    // Methods
  }
  public abstract partial class CCGridBase {
    // Fields
    protected bool m_bActive;
    protected bool m_bIsTextureFlipped;
    protected CocosSharp.CCDirectorProjection m_directorProjection;
    protected int m_nReuseGrid;
    protected CocosSharp.CCPoint m_obStep;
    protected CocosSharp.CCGrabber m_pGrabber;
    protected CocosSharp.CCTexture2D m_pTexture;
    protected CocosSharp.CCGridSize m_sGridSize;
     
    // Constructors
    protected CCGridBase() { }
     
    // Properties
    public bool Active { get { return default(bool); } set { } }
    public CocosSharp.CCGridSize GridSize { get { return default(CocosSharp.CCGridSize); } set { } }
    public int ReuseGrid { get { return default(int); } set { } }
    public CocosSharp.CCPoint Step { get { return default(CocosSharp.CCPoint); } set { } }
    public bool TextureFlipped { get { return default(bool); } set { } }
     
    // Methods
    public virtual void AfterDraw(CocosSharp.CCNode target) { }
    public virtual void BeforeDraw() { }
    public abstract void Blit();
    public abstract void CalculateVertexPoints();
    protected void InitWithSize(CocosSharp.CCGridSize gridSize) { }
    protected void InitWithSize(CocosSharp.CCGridSize gridSize, CocosSharp.CCSize size) { }
    protected virtual bool InitWithSize(CocosSharp.CCGridSize gridSize, CocosSharp.CCTexture2D pTexture, bool bFlipped) { return default(bool); }
    public ulong NextPOT(ulong x) { return default(ulong); }
    public abstract void Reuse();
    public void Set2DProjection() { }
  }
  [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
  public partial struct CCGridSize {
    // Fields
    public static readonly CocosSharp.CCGridSize One;
    public int X;
    public int Y;
    public static readonly CocosSharp.CCGridSize Zero;
     
    // Constructors
    public CCGridSize(int inx, int iny) { throw new System.NotImplementedException(); }
  }
  public partial class CCHide : CocosSharp.CCActionInstant {
    // Constructors
    public CCHide() { }
     
    // Methods
    public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCHideState : CocosSharp.CCActionInstantState {
    // Constructors
    public CCHideState(CocosSharp.CCHide action, CocosSharp.CCNode target) : base (default(CocosSharp.CCActionInstant), default(CocosSharp.CCNode)) { }
  }
  public enum CCImageFormat {
    // Fields
    Gif = 4,
    Jpg = 0,
    Png = 1,
    RawData = 5,
    Tiff = 2,
    UnKnown = 6,
    Webp = 3,
  }
  public partial class CCIMEKeyboardNotificationInfo {
    // Fields
    public CocosSharp.CCRect begin;
    public float duration;
    public CocosSharp.CCRect end;
     
    // Constructors
    public CCIMEKeyboardNotificationInfo() { }
  }
  public partial class CCInvocation {
    // Constructors
    public CCInvocation(object target, System.Action<System.Object, CocosSharp.CCControlEvent> action, CocosSharp.CCControlEvent controlEvent) { }
     
    // Properties
    public System.Action<System.Object, CocosSharp.CCControlEvent> Action { get { return default(System.Action<System.Object, CocosSharp.CCControlEvent>); } }
    public CocosSharp.CCControlEvent ControlEvent { get { return default(CocosSharp.CCControlEvent); } }
    public object Target { get { return default(object); } }
     
    // Methods
    public void Invoke(object sender) { }
  }
  public partial class CCJumpBy : CocosSharp.CCActionInterval {
    // Constructors
    public CCJumpBy(float duration, CocosSharp.CCPoint position, float height, uint jumps) { }
     
    // Properties
    public float Height { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]protected set { } }
    public uint Jumps { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(uint); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]protected set { } }
    public CocosSharp.CCPoint Position { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCPoint); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]protected set { } }
     
    // Methods
    public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCJumpByState : CocosSharp.CCActionIntervalState {
    // Fields
    protected CocosSharp.CCPoint Delta;
    protected float Height;
    protected uint Jumps;
    protected CocosSharp.CCPoint P;
    protected CocosSharp.CCPoint StartPosition;
     
    // Constructors
    public CCJumpByState(CocosSharp.CCJumpBy action, CocosSharp.CCNode target) : base (default(CocosSharp.CCActionInterval), default(CocosSharp.CCNode)) { }
     
    // Methods
    public override void Update(float time) { }
  }
  public partial class CCJumpTiles3D : CocosSharp.CCTiledGrid3DAction {
    // Constructors
    public CCJumpTiles3D(float duration, CocosSharp.CCGridSize gridSize, int numberOfJumps=0, float amplitude=0f) : base (default(float)) { }
     
    // Properties
    protected internal int NumberOfJumps { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } }
     
    // Methods
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCJumpTiles3DState : CocosSharp.CCTiledGrid3DActionState {
    // Constructors
    public CCJumpTiles3DState(CocosSharp.CCJumpTiles3D action, CocosSharp.CCNode target) : base (default(CocosSharp.CCTiledGrid3DAction), default(CocosSharp.CCNode)) { }
     
    // Properties
    protected int NumberOfJumps { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
     
    // Methods
    public override void Update(float time) { }
  }
  public partial class CCJumpTo : CocosSharp.CCJumpBy {
    // Constructors
    public CCJumpTo(float duration, CocosSharp.CCPoint position, float height, uint jumps) : base (default(float), default(CocosSharp.CCPoint), default(float), default(uint)) { }
     
    // Methods
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCJumpToState : CocosSharp.CCJumpByState {
    // Constructors
    public CCJumpToState(CocosSharp.CCJumpBy action, CocosSharp.CCNode target) : base (default(CocosSharp.CCJumpBy), default(CocosSharp.CCNode)) { }
  }
  public enum CCKeyboardEventType {
    // Fields
    KEYBOARD_NONE = 0,
    KEYBOARD_PRESS = 1,
    KEYBOARD_RELEASE = 2,
    KEYBOARD_STATE = 3,
  }
  [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Size=1)]
  public partial struct CCKeyboardState {
    // Properties
    public CocosSharp.CCKeyState this[CocosSharp.CCKeys key] { get { return default(CocosSharp.CCKeyState); } }
     
    // Methods
    public override bool Equals(object obj) { return default(bool); }
    public override int GetHashCode() { return default(int); }
    public CocosSharp.CCKeys[] GetPressedKeys() { return default(CocosSharp.CCKeys[]); }
    public bool IsKeyDown(CocosSharp.CCKeys key) { return default(bool); }
    public bool IsKeyUp(CocosSharp.CCKeys key) { return default(bool); }
    public static bool operator ==(CocosSharp.CCKeyboardState a, CocosSharp.CCKeyboardState b) { return default(bool); }
    public static bool operator !=(CocosSharp.CCKeyboardState a, CocosSharp.CCKeyboardState b) { return default(bool); }
  }
  public partial class CCKeypadDispatcher {
    // Fields
    protected System.Collections.Generic.List<CocosSharp.CCKeypadHandler> delegates;
    protected System.Collections.Generic.List<CocosSharp.ICCKeypadDelegate> handlersToAdd;
    protected System.Collections.Generic.List<CocosSharp.ICCKeypadDelegate> handlersToRemove;
    protected bool isLocked;
    protected bool isToAdd;
    protected bool isToRemove;
     
    // Constructors
    public CCKeypadDispatcher() { }
     
    // Methods
    public void AddDelegate(CocosSharp.ICCKeypadDelegate keyPadDelegate) { }
    public bool DispatchKeypadMsg(CocosSharp.CCKeypadMSGType keypadMsgType) { return default(bool); }
    public void ForceAddDelegate(CocosSharp.ICCKeypadDelegate keypadDelegate) { }
    public void ForceRemoveDelegate(CocosSharp.ICCKeypadDelegate pDelegate) { }
    public void RemoveDelegate(CocosSharp.ICCKeypadDelegate keypadDelegate) { }
  }
  public partial class CCKeypadHandler {
    // Fields
    protected CocosSharp.ICCKeypadDelegate m_pDelegate;
     
    // Constructors
    public CCKeypadHandler() { }
     
    // Properties
    public CocosSharp.ICCKeypadDelegate Delegate { get { return default(CocosSharp.ICCKeypadDelegate); } set { } }
     
    // Methods
    public static CocosSharp.CCKeypadHandler HandlerWithDelegate(CocosSharp.ICCKeypadDelegate pDelegate) { return default(CocosSharp.CCKeypadHandler); }
    public virtual bool InitWithDelegate(CocosSharp.ICCKeypadDelegate pDelegate) { return default(bool); }
  }
  public enum CCKeypadMSGType {
    // Fields
    BackClicked = 1,
    MenuClicked = 2,
  }
  public enum CCKeys {
    // Fields
    A = 65,
    Add = 107,
    Apps = 93,
    Attn = 246,
    B = 66,
    Back = 8,
    BrowserBack = 166,
    BrowserFavorites = 171,
    BrowserForward = 167,
    BrowserHome = 172,
    BrowserRefresh = 168,
    BrowserSearch = 170,
    BrowserStop = 169,
    C = 67,
    CapsLock = 20,
    ChatPadGreen = 202,
    ChatPadOrange = 203,
    Crsel = 247,
    D = 68,
    D0 = 48,
    D1 = 49,
    D2 = 50,
    D3 = 51,
    D4 = 52,
    D5 = 53,
    D6 = 54,
    D7 = 55,
    D8 = 56,
    D9 = 57,
    Decimal = 110,
    Delete = 46,
    Divide = 111,
    Down = 40,
    E = 69,
    End = 35,
    Enter = 13,
    EraseEof = 249,
    Escape = 27,
    Execute = 43,
    Exsel = 248,
    F = 70,
    F1 = 112,
    F10 = 121,
    F11 = 122,
    F12 = 123,
    F13 = 124,
    F14 = 125,
    F15 = 126,
    F16 = 127,
    F17 = 128,
    F18 = 129,
    F19 = 130,
    F2 = 113,
    F20 = 131,
    F21 = 132,
    F22 = 133,
    F23 = 134,
    F24 = 135,
    F3 = 114,
    F4 = 115,
    F5 = 116,
    F6 = 117,
    F7 = 118,
    F8 = 119,
    F9 = 120,
    G = 71,
    H = 72,
    Help = 47,
    Home = 36,
    I = 73,
    ImeConvert = 28,
    ImeNoConvert = 29,
    Insert = 45,
    J = 74,
    K = 75,
    Kana = 21,
    Kanji = 25,
    L = 76,
    LaunchApplication1 = 182,
    LaunchApplication2 = 183,
    LaunchMail = 180,
    Left = 37,
    LeftAlt = 164,
    LeftControl = 162,
    LeftShift = 160,
    LeftWindows = 91,
    M = 77,
    MediaNextTrack = 176,
    MediaPlayPause = 179,
    MediaPreviousTrack = 177,
    MediaStop = 178,
    Multiply = 106,
    N = 78,
    None = 0,
    NumLock = 144,
    NumPad0 = 96,
    NumPad1 = 97,
    NumPad2 = 98,
    NumPad3 = 99,
    NumPad4 = 100,
    NumPad5 = 101,
    NumPad6 = 102,
    NumPad7 = 103,
    NumPad8 = 104,
    NumPad9 = 105,
    O = 79,
    Oem8 = 223,
    OemAuto = 243,
    OemBackslash = 226,
    OemClear = 254,
    OemCloseBrackets = 221,
    OemComma = 188,
    OemCopy = 242,
    OemEnlW = 244,
    OemMinus = 189,
    OemOpenBrackets = 219,
    OemPeriod = 190,
    OemPipe = 220,
    OemPlus = 187,
    OemQuestion = 191,
    OemQuotes = 222,
    OemSemicolon = 186,
    OemTilde = 192,
    P = 80,
    Pa1 = 253,
    PageDown = 34,
    PageUp = 33,
    Pause = 19,
    Play = 250,
    Print = 42,
    PrintScreen = 44,
    ProcessKey = 229,
    Q = 81,
    R = 82,
    Right = 39,
    RightAlt = 165,
    RightControl = 163,
    RightShift = 161,
    RightWindows = 92,
    S = 83,
    Scroll = 145,
    Select = 41,
    SelectMedia = 181,
    Separator = 108,
    Sleep = 95,
    Space = 32,
    Subtract = 109,
    T = 84,
    Tab = 9,
    U = 85,
    Up = 38,
    V = 86,
    VolumeDown = 174,
    VolumeMute = 173,
    VolumeUp = 175,
    W = 87,
    X = 88,
    Y = 89,
    Z = 90,
    Zoom = 251,
  }
  public enum CCKeyState {
    // Fields
    Down = 1,
    Up = 0,
  }
  public partial class CCLabel : CocosSharp.CCLabelBMFont {
    // Fields
    protected bool m_bFontDirty;
    protected static bool m_bTextureDirty;
    protected string m_FontName;
    protected float m_FontSize;
    public static CocosSharp.CCTexture2D m_pTexture;
     
    // Constructors
    public CCLabel() { }
    public CCLabel(string text, string fontName, float fontSize) { }
    public CCLabel(string text, string fontName, float fontSize, CocosSharp.CCSize dimensions) { }
    public CCLabel(string text, string fontName, float fontSize, CocosSharp.CCSize dimensions, CocosSharp.CCTextAlignment hAlignment) { }
    public CCLabel(string text, string fontName, float fontSize, CocosSharp.CCSize dimensions, CocosSharp.CCTextAlignment hAlignment, CocosSharp.CCVerticalTextAlignment vAlignment) { }
    public CCLabel(string text, string fontName, float fontSize, CocosSharp.CCTextAlignment hAlignment) { }
    public CCLabel(string text, string fontName, float fontSize, CocosSharp.CCTextAlignment hAlignment, CocosSharp.CCVerticalTextAlignment vAlignment) { }
     
    // Properties
    public string FontName { get { return default(string); } set { } }
    public float FontSize { get { return default(float); } set { } }
    public override string Text { get { return default(string); } set { } }
     
    // Methods
    public static CocosSharp.CCLabel.ivec4 AllocateRegion(int width, int height) { return default(CocosSharp.CCLabel.ivec4); }
    protected override void Draw() { }
    public static void InitializeTTFAtlas(int width, int height) { }
    public static void SetRegionData(CocosSharp.CCLabel.ivec4 region, System.Int32[] data, int stride) { }
     
    // Nested Types
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public partial struct ivec4 {
      // Fields
      public int height;
      public int width;
      public int x;
      public int y;
    }
  }
  public partial class CCLabelAtlas : CocosSharp.CCAtlasNode, CocosSharp.ICCTextContainer {
    // Fields
    protected char m_cMapStartChar;
    protected string m_sString;
     
    // Constructors
    public CCLabelAtlas(string label, CocosSharp.CCTexture2D texture, int itemWidth, int itemHeight, char startCharMap) : base (default(string), default(int), default(int), default(int)) { }
    public CCLabelAtlas(string label, string fntFile) : base (default(string), default(int), default(int), default(int)) { }
    public CCLabelAtlas(string label, string charMapFile, int itemWidth, int itemHeight, char startCharMap) : base (default(string), default(int), default(int), default(int)) { }
     
    // Properties
    public string Text { get { return default(string); } set { } }
     
    // Methods
    public override void UpdateAtlasValues() { }
  }
  public partial class CCLabelBMFont : CocosSharp.CCSpriteBatchNode, CocosSharp.ICCColor, CocosSharp.ICCTextContainer {
    // Fields
    public const int AutomaticWidth = -1;
    protected CocosSharp.CCColor3B displayedColor;
    protected byte displayedOpacity;
    protected string fntConfigFile;
    protected CocosSharp.CCTextAlignment horzAlignment;
    protected bool isColorCascaded;
    protected bool isColorModifiedByOpacity;
    protected bool isOpacityCascaded;
    protected CocosSharp.CCSize labelDimensions;
    protected string labelInitialText;
    protected string labelText;
    protected bool lineBreakWithoutSpaces;
    protected CocosSharp.CCSprite m_pReusedChar;
    protected CocosSharp.CCColor3B realColor;
    protected byte realOpacity;
    protected CocosSharp.CCVerticalTextAlignment vertAlignment;
     
    // Constructors
    public CCLabelBMFont() { }
    public CCLabelBMFont(string str, string fntFile) { }
    public CCLabelBMFont(string str, string fntFile, CocosSharp.CCSize dimensions, CocosSharp.CCTextAlignment hAlignment, CocosSharp.CCVerticalTextAlignment vAlignment, CocosSharp.CCPoint imageOffset, CocosSharp.CCTexture2D texture) { }
    public CCLabelBMFont(string str, string fntFile, float width) { }
    public CCLabelBMFont(string str, string fntFile, float width, CocosSharp.CCTextAlignment alignment) { }
    public CCLabelBMFont(string str, string fntFile, float width, CocosSharp.CCTextAlignment alignment, CocosSharp.CCPoint imageOffset) { }
    public CCLabelBMFont(string str, string fntFile, float width, CocosSharp.CCTextAlignment alignment, CocosSharp.CCPoint imageOffset, CocosSharp.CCTexture2D texture) { }
    public CCLabelBMFont(string str, string fntFile, float width, CocosSharp.CCTextAlignment hAlignment, CocosSharp.CCVerticalTextAlignment vAlignment, CocosSharp.CCPoint imageOffset, CocosSharp.CCTexture2D texture) { }
     
    // Properties
    public override CocosSharp.CCPoint AnchorPoint { get { return default(CocosSharp.CCPoint); } set { } }
    public virtual CocosSharp.CCColor3B Color { get { return default(CocosSharp.CCColor3B); } set { } }
    public CocosSharp.CCSize Dimensions { get { return default(CocosSharp.CCSize); } set { } }
    public virtual CocosSharp.CCColor3B DisplayedColor { get { return default(CocosSharp.CCColor3B); } }
    public virtual byte DisplayedOpacity { get { return default(byte); } }
    public string FntFile { get { return default(string); } set { } }
    public CocosSharp.CCTextAlignment HorizontalAlignment { get { return default(CocosSharp.CCTextAlignment); } set { } }
    protected CocosSharp.CCPoint ImageOffset { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCPoint); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public virtual bool IsColorCascaded { get { return default(bool); } set { } }
    public virtual bool IsColorModifiedByOpacity { get { return default(bool); } set { } }
    protected bool IsDirty { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public virtual bool IsOpacityCascaded { get { return default(bool); } set { } }
    public bool LineBreakWithoutSpace { get { return default(bool); } set { } }
    public virtual byte Opacity { get { return default(byte); } set { } }
    public override float Scale { set { } }
    public override float ScaleX { get { return default(float); } set { } }
    public override float ScaleY { get { return default(float); } set { } }
    public virtual string Text { get { return default(string); } set { } }
    public CocosSharp.CCVerticalTextAlignment VerticalAlignment { get { return default(CocosSharp.CCVerticalTextAlignment); } set { } }
     
    // Methods
    public void CreateFontChars() { }
    protected override void Draw() { }
    public static void FNTConfigRemoveCache() { }
    protected void InitCCLabelBMFont(string theString, string fntFile, CocosSharp.CCSize dimensions, CocosSharp.CCTextAlignment hAlignment, CocosSharp.CCVerticalTextAlignment vAlignment, CocosSharp.CCPoint imageOffset, CocosSharp.CCTexture2D texture) { }
    public static void PurgeCachedData() { }
    public virtual void SetString(string newString, bool needUpdateLabel) { }
    public virtual void UpdateDisplayedColor(CocosSharp.CCColor3B parentColor) { }
    public virtual void UpdateDisplayedOpacity(byte parentOpacity) { }
    protected void UpdateLabel() { }
  }
  public partial class CCLabelTtf : CocosSharp.CCSprite, CocosSharp.ICCTextContainer {
    // Fields
    protected string m_pString;
     
    // Constructors
    public CCLabelTtf() : base (default(CocosSharp.CCTexture2D), default(CocosSharp.CCRect), default(bool)) { }
    public CCLabelTtf(string text, string fontName, float fontSize) : base (default(CocosSharp.CCTexture2D), default(CocosSharp.CCRect), default(bool)) { }
    public CCLabelTtf(string text, string fontName, float fontSize, CocosSharp.CCSize dimensions, CocosSharp.CCTextAlignment hAlignment) : base (default(CocosSharp.CCTexture2D), default(CocosSharp.CCRect), default(bool)) { }
    public CCLabelTtf(string text, string fontName, float fontSize, CocosSharp.CCSize dimensions, CocosSharp.CCTextAlignment hAlignment, CocosSharp.CCVerticalTextAlignment vAlignment) : base (default(CocosSharp.CCTexture2D), default(CocosSharp.CCRect), default(bool)) { }
     
    // Properties
    public CocosSharp.CCSize Dimensions { get { return default(CocosSharp.CCSize); } set { } }
    public string FontName { get { return default(string); } set { } }
    public float FontSize { get { return default(float); } set { } }
    public CocosSharp.CCTextAlignment HorizontalAlignment { get { return default(CocosSharp.CCTextAlignment); } set { } }
    public string Text { get { return default(string); } set { } }
    public CocosSharp.CCVerticalTextAlignment VerticalAlignment { get { return default(CocosSharp.CCVerticalTextAlignment); } set { } }
     
    // Methods
    public override string ToString() { return default(string); }
  }
  public partial class CCLayer : CocosSharp.CCNode {
    // Constructors
    public CCLayer() { }
    public CCLayer(CocosSharp.CCClipMode clipMode) { }
     
    // Properties
    public bool AccelerometerEnabled { get { return default(bool); } set { } }
    public CocosSharp.CCClipMode ChildClippingMode { get { return default(CocosSharp.CCClipMode); } set { } }
    public override CocosSharp.CCSize ContentSize { get { return default(CocosSharp.CCSize); } set { } }
     
    // Methods
    public override void OnEnter() { }
    public override void OnExit() { }
    protected virtual bool TouchBegan(CocosSharp.CCTouch touch, CocosSharp.CCEvent touchEvent) { return default(bool); }
    protected virtual void TouchCancelled(CocosSharp.CCTouch touch, CocosSharp.CCEvent touchEvent) { }
    protected virtual void TouchEnded(CocosSharp.CCTouch touch, CocosSharp.CCEvent touchEvent) { }
    protected virtual void TouchesBegan(System.Collections.Generic.List<CocosSharp.CCTouch> touches, CocosSharp.CCEvent touchEvent) { }
    protected virtual void TouchesCancelled(System.Collections.Generic.List<CocosSharp.CCTouch> touches, CocosSharp.CCEvent touchEvent) { }
    protected virtual void TouchesEnded(System.Collections.Generic.List<CocosSharp.CCTouch> touches, CocosSharp.CCEvent touchEvent) { }
    protected virtual void TouchesMoved(System.Collections.Generic.List<CocosSharp.CCTouch> touches, CocosSharp.CCEvent touchEvent) { }
    protected virtual void TouchMoved(CocosSharp.CCTouch touch, CocosSharp.CCEvent touchEvent) { }
    public override void Visit() { }
  }
  public partial class CCLayerColor : CocosSharp.CCLayerRGBA, CocosSharp.ICCBlendable {
    // Constructors
    public CCLayerColor() { }
    public CCLayerColor(CocosSharp.CCColor4B color) { }
    public CCLayerColor(CocosSharp.CCColor4B color, float width, float height) { }
     
    // Properties
    public virtual CocosSharp.CCBlendFunc BlendFunc { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCBlendFunc); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public override CocosSharp.CCColor3B Color { get { return default(CocosSharp.CCColor3B); } set { } }
    public override CocosSharp.CCSize ContentSize { get { return default(CocosSharp.CCSize); } set { } }
    public override byte Opacity { get { return default(byte); } set { } }
     
    // Methods
    public void ChangeHeight(float h) { }
    public void ChangeWidth(float w) { }
    public void ChangeWidthAndHeight(float w, float h) { }
    protected override void Draw() { }
    protected virtual void UpdateColor() { }
  }
  public partial class CCLayerGradient : CocosSharp.CCLayerColor {
    // Constructors
    public CCLayerGradient() { }
    public CCLayerGradient(CocosSharp.CCColor4B start, CocosSharp.CCColor4B end) { }
    public CCLayerGradient(CocosSharp.CCColor4B start, CocosSharp.CCColor4B end, CocosSharp.CCPoint gradientDirection) { }
    public CCLayerGradient(byte startOpacity, byte endOpacity) { }
     
    // Properties
    public CocosSharp.CCColor3B EndColor { get { return default(CocosSharp.CCColor3B); } set { } }
    public byte EndOpacity { get { return default(byte); } set { } }
    public bool IsCompressedInterpolation { get { return default(bool); } set { } }
    public CocosSharp.CCColor3B StartColor { get { return default(CocosSharp.CCColor3B); } set { } }
    public byte StartOpacity { get { return default(byte); } set { } }
    public CocosSharp.CCPoint Vector { get { return default(CocosSharp.CCPoint); } set { } }
     
    // Methods
    protected override void UpdateColor() { }
  }
  public partial class CCLayerLoader : CocosSharp.CCNodeLoader {
    // Constructors
    public CCLayerLoader() { }
     
    // Methods
    public override CocosSharp.CCNode CreateCCNode() { return default(CocosSharp.CCNode); }
    protected override void OnHandlePropTypeCheck(CocosSharp.CCNode node, CocosSharp.CCNode parent, string propertyName, bool pCheck, CocosSharp.CCBReader reader) { }
  }
  public partial class CCLayerMultiplex : CocosSharp.CCLayerRGBA {
    // Fields
    public const int NoLayer = -1;
     
    // Constructors
    public CCLayerMultiplex() { }
    public CCLayerMultiplex(CocosSharp.CCAction inAction, CocosSharp.CCAction outAction) { }
    public CCLayerMultiplex(CocosSharp.CCAction inAction, CocosSharp.CCAction outAction, params CocosSharp.CCLayer[] layers) { }
    public CCLayerMultiplex(params CocosSharp.CCLayer[] layers) { }
     
    // Properties
    public virtual CocosSharp.CCLayer ActiveLayer { get { return default(CocosSharp.CCLayer); } }
    protected int EnabledLayer { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public CocosSharp.CCAction InAction { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCAction); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    protected System.Collections.Generic.Dictionary<System.Int32, CocosSharp.CCLayer> Layers { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Collections.Generic.Dictionary<System.Int32, CocosSharp.CCLayer>); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public CocosSharp.CCAction OutAction { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCAction); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public bool ShowFirstLayerOnEnter { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
     
    // Methods
    public void AddLayer(CocosSharp.CCLayer layer) { }
    public override void OnEnter() { }
    public CocosSharp.CCLayer SwitchTo(int n) { return default(CocosSharp.CCLayer); }
    public CocosSharp.CCLayer SwitchToAndRemovePreviousLayer(int n) { return default(CocosSharp.CCLayer); }
    public CocosSharp.CCLayer SwitchToFirstLayer() { return default(CocosSharp.CCLayer); }
    public CocosSharp.CCLayer SwitchToNextLayer() { return default(CocosSharp.CCLayer); }
    public void SwitchToNone() { }
    public CocosSharp.CCLayer SwitchToPreviousLayer() { return default(CocosSharp.CCLayer); }
  }
  public partial class CCLayerRGBA : CocosSharp.CCLayer, CocosSharp.ICCColor {
    // Constructors
    public CCLayerRGBA() { }
     
    // Properties
    public virtual CocosSharp.CCColor3B Color { get { return default(CocosSharp.CCColor3B); } set { } }
    public virtual CocosSharp.CCColor3B DisplayedColor { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCColor3B); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]protected set { } }
    public virtual byte DisplayedOpacity { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(byte); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]protected set { } }
    public virtual bool IsColorCascaded { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public virtual bool IsColorModifiedByOpacity { get { return default(bool); } set { } }
    public virtual bool IsOpacityCascaded { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public virtual byte Opacity { get { return default(byte); } set { } }
    protected CocosSharp.CCColor3B RealColor { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCColor3B); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    protected byte RealOpacity { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(byte); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
     
    // Methods
    public virtual void UpdateDisplayedColor(CocosSharp.CCColor3B parentColor) { }
    public virtual void UpdateDisplayedOpacity(byte parentOpacity) { }
  }
  public partial class CCLens3D : CocosSharp.CCGrid3DAction {
    // Constructors
    public CCLens3D(float duration, CocosSharp.CCGridSize gridSize) : base (default(float)) { }
    public CCLens3D(float duration, CocosSharp.CCGridSize gridSize, CocosSharp.CCPoint position, float radius) : base (default(float)) { }
     
    // Properties
    public bool Concave { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } }
    public float LensEffect { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } }
    public CocosSharp.CCPoint Position { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCPoint); } }
    public float Radius { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } }
     
    // Methods
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCLens3DState : CocosSharp.CCGrid3DActionState {
    // Constructors
    public CCLens3DState(CocosSharp.CCLens3D action, CocosSharp.CCNode target) : base (default(CocosSharp.CCGrid3DAction), default(CocosSharp.CCNode)) { }
     
    // Properties
    public bool Concave { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public float LensEffect { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public CocosSharp.CCPoint Position { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCPoint); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public float Radius { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
     
    // Methods
    public override void Update(float time) { }
  }
  public partial class CCLiquid : CocosSharp.CCGrid3DAction {
    // Constructors
    public CCLiquid(float duration, CocosSharp.CCGridSize gridSize, int waves=0, float amplitude=0f) : base (default(float)) { }
     
    // Properties
    public int Waves { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } }
     
    // Methods
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCLiquidState : CocosSharp.CCGrid3DActionState {
    // Constructors
    public CCLiquidState(CocosSharp.CCLiquid action, CocosSharp.CCNode target) : base (default(CocosSharp.CCGrid3DAction), default(CocosSharp.CCNode)) { }
     
    // Properties
    public int Waves { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
     
    // Methods
    public override void Update(float time) { }
  }
  public partial class CCLog {
    // Constructors
    public CCLog() { }
     
    // Methods
    public static void Log(string message) { }
    public static void Log(string format, params System.Object[] args) { }
  }
  public static partial class CCMacros {
    // Fields
    public static readonly float CCDirectorStatsUpdateIntervalInSeconds;
    public static readonly string CCHiResDisplayFilenameSuffix;
    public static readonly int CCSpriteIndexNotInitialized;
     
    // Methods
    public static float CCContentScaleFactor() { return default(float); }
    public static float CCDegreesToRadians(float angle) { return default(float); }
    public static float CCRadiansToDegrees(float angle) { return default(float); }
    public static float CCRandomBetween0And1() { return default(float); }
    public static float CCRandomBetweenNegative1And1() { return default(float); }
    public static void CCSwap<T>(ref T x, ref T y) { }
    public static CocosSharp.CCPoint PixelsToPoints(this CocosSharp.CCPoint p) { return default(CocosSharp.CCPoint); }
    public static CocosSharp.CCRect PixelsToPoints(this CocosSharp.CCRect r) { return default(CocosSharp.CCRect); }
    public static CocosSharp.CCSize PixelsToPoints(this CocosSharp.CCSize s) { return default(CocosSharp.CCSize); }
    public static CocosSharp.CCPoint PointsToPixels(this CocosSharp.CCPoint p) { return default(CocosSharp.CCPoint); }
    public static CocosSharp.CCRect PointsToPixels(this CocosSharp.CCRect r) { return default(CocosSharp.CCRect); }
    public static CocosSharp.CCSize PointsToPixels(this CocosSharp.CCSize s) { return default(CocosSharp.CCSize); }
  }
  public partial class CCMaskedSprite : CocosSharp.CCSprite {
    // Constructors
    public CCMaskedSprite() : base (default(CocosSharp.CCTexture2D), default(CocosSharp.CCRect), default(bool)) { }
    public CCMaskedSprite(CocosSharp.CCSpriteFrame pSpriteFrame, System.Byte[] mask) : base (default(CocosSharp.CCTexture2D), default(CocosSharp.CCRect), default(bool)) { }
    public CCMaskedSprite(CocosSharp.CCTexture2D texture, CocosSharp.CCRect rect, System.Byte[] mask) : base (default(CocosSharp.CCTexture2D), default(CocosSharp.CCRect), default(bool)) { }
    public CCMaskedSprite(CocosSharp.CCTexture2D texture, System.Byte[] mask) : base (default(CocosSharp.CCTexture2D), default(CocosSharp.CCRect), default(bool)) { }
    public CCMaskedSprite(string fileName, CocosSharp.CCRect rect, System.Byte[] mask) : base (default(CocosSharp.CCTexture2D), default(CocosSharp.CCRect), default(bool)) { }
    public CCMaskedSprite(string fileName, System.Byte[] mask) : base (default(CocosSharp.CCTexture2D), default(CocosSharp.CCRect), default(bool)) { }
     
    // Properties
    public virtual System.Byte[] CollisionMask { get { return default(System.Byte[]); } set { } }
     
    // Methods
    public virtual bool CollidesWith(CocosSharp.CCMaskedSprite target, out CocosSharp.CCPoint pt) { pt = default(CocosSharp.CCPoint); return default(bool); }
  }
  public static partial class CCMathHelper {
    // Methods
    public static int Clamp(int value, int min, int max) { return default(int); }
    public static float Clamp(float value, float min, float max) { return default(float); }
    public static float Cos(float radian) { return default(float); }
    public static int Lerp(int value1, int value2, float amount) { return default(int); }
    public static float Sin(float radian) { return default(float); }
    public static float ToDegrees(float radians) { return default(float); }
    public static float ToRadians(float degrees) { return default(float); }
  }
  public partial class CCMenu : CocosSharp.CCLayerRGBA {
    // Fields
    public const int DefaultMenuHandlerPriority = -128;
    public const float DefaultPadding = 5f;
     
    // Constructors
    public CCMenu(params CocosSharp.CCMenuItem[] items) { }
     
    // Properties
    public bool Enabled { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public CocosSharp.CCMenuItem FocusedItem { get { return default(CocosSharp.CCMenuItem); } }
    public override bool HasFocus { set { } }
    protected CocosSharp.CCMenuState MenuState { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCMenuState); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    protected CocosSharp.CCMenuItem SelectedMenuItem { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCMenuItem); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
     
    // Methods
    public void AddChild(CocosSharp.CCMenuItem menuItem) { }
    public void AddChild(CocosSharp.CCMenuItem menuItem, int zOrder, int tag=0) { }
    public void AlignItemsHorizontally(float padding=5f) { }
    public void AlignItemsInColumns(params System.UInt32[] numOfItemsPerRow) { }
    public void AlignItemsInRows(params System.UInt32[] numOfItemsPerColumn) { }
    public void AlignItemsVertically(float padding=5f) { }
    protected virtual CocosSharp.CCMenuItem ItemForTouch(CocosSharp.CCTouch touch) { return default(CocosSharp.CCMenuItem); }
    public override void OnEnter() { }
    public override void OnExit() { }
    public void RemoveChild(CocosSharp.CCMenuItem menuItem, bool cleanup) { }
  }
  public partial class CCMenuItem : CocosSharp.CCNodeRGBA {
    // Constructors
    public CCMenuItem() { }
    public CCMenuItem(System.Action<System.Object> target) { }
     
    // Properties
    public virtual bool Enabled { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public CocosSharp.CCRect Rectangle { get { return default(CocosSharp.CCRect); } }
    public virtual bool Selected { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public System.Action<System.Object> Target { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Action<System.Object>); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    protected CocosSharp.CCActionState ZoomActionState { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCActionState); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
     
    // Methods
    public virtual void Activate() { }
    public virtual void RegisterScriptHandler(string pszFunctionName) { }
  }
  public partial class CCMenuItemFont : CocosSharp.CCMenuItemLabelTTF {
    // Constructors
    public CCMenuItemFont(string labelString) : base (default(CocosSharp.CCLabelTtf), default(System.Action<System.Object>)) { }
    public CCMenuItemFont(string labelString, System.Action<System.Object> selector=null) : base (default(CocosSharp.CCLabelTtf), default(System.Action<System.Object>)) { }
    public CCMenuItemFont(string labelString, string fontNameIn="arial", uint fontSizeIn=(uint)32, System.Action<System.Object> selector=null) : base (default(CocosSharp.CCLabelTtf), default(System.Action<System.Object>)) { }
    public CCMenuItemFont(string labelString, uint fontSizeIn=(uint)32, System.Action<System.Object> selector=null) : base (default(CocosSharp.CCLabelTtf), default(System.Action<System.Object>)) { }
     
    // Properties
    public string FontName { get { return default(string); } set { } }
    public uint FontSize { get { return default(uint); } set { } }
     
    // Methods
    protected void RecreateLabel() { }
  }
  public partial class CCMenuItemImage : CocosSharp.CCMenuItem {
    // Constructors
    public CCMenuItemImage() { }
    public CCMenuItemImage(CocosSharp.CCSprite normalSprite, CocosSharp.CCSprite selectedSprite, CocosSharp.CCSprite disabledSprite, System.Action<System.Object> target=null) { }
    public CCMenuItemImage(CocosSharp.CCSprite normalSprite, CocosSharp.CCSprite selectedSprite, System.Action<System.Object> target=null) { }
    public CCMenuItemImage(CocosSharp.CCSprite normalSprite, System.Action<System.Object> selector=null) { }
    public CCMenuItemImage(CocosSharp.CCSpriteFrame normalSpFrame, CocosSharp.CCSpriteFrame selectedSpFrame, CocosSharp.CCSpriteFrame disabledSpFrame, System.Action<System.Object> target=null) { }
    public CCMenuItemImage(string normalSprite, string selectedSprite, System.Action<System.Object> target=null) { }
    public CCMenuItemImage(string normalSprite, string selectedSprite, string disabledSprite, System.Action<System.Object> target=null) { }
     
    // Properties
    public CocosSharp.CCSprite DisabledImage { get { return default(CocosSharp.CCSprite); } set { } }
    public CocosSharp.CCSpriteFrame DisabledImageSpriteFrame { set { } }
    public override bool Enabled { get { return default(bool); } set { } }
    public CocosSharp.CCSprite NormalImage { get { return default(CocosSharp.CCSprite); } set { } }
    public CocosSharp.CCSpriteFrame NormalImageSpriteFrame { set { } }
    public override bool Selected { set { } }
    public CocosSharp.CCSprite SelectedImage { get { return default(CocosSharp.CCSprite); } set { } }
    public CocosSharp.CCSpriteFrame SelectedImageSpriteFrame { set { } }
    public bool ZoomBehaviorOnTouch { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
     
    // Methods
    public override void Activate() { }
  }
  public partial class CCMenuItemImageLoader : CocosSharp.CCMenuItemLoader {
    // Constructors
    public CCMenuItemImageLoader() { }
     
    // Methods
    public override CocosSharp.CCNode CreateCCNode() { return default(CocosSharp.CCNode); }
    protected override void OnHandlePropTypeSpriteFrame(CocosSharp.CCNode node, CocosSharp.CCNode parent, string propertyName, CocosSharp.CCSpriteFrame spriteFrame, CocosSharp.CCBReader reader) { }
  }
  public partial class CCMenuItemLabel : CocosSharp.CCMenuItemLabelBase {
    // Constructors
    public CCMenuItemLabel(CocosSharp.CCLabel label, System.Action<System.Object> target=null) : base (default(System.Action<System.Object>)) { }
     
    // Properties
    public override bool Enabled { get { return default(bool); } set { } }
    public CocosSharp.CCLabel Label { get { return default(CocosSharp.CCLabel); } set { } }
     
    // Methods
  }
  public partial class CCMenuItemLabelAtlas : CocosSharp.CCMenuItemLabelBase {
    // Constructors
    public CCMenuItemLabelAtlas(CocosSharp.CCLabelAtlas labelAtlas, System.Action<System.Object> target=null) : base (default(System.Action<System.Object>)) { }
    public CCMenuItemLabelAtlas(System.Action<System.Object> target) : base (default(System.Action<System.Object>)) { }
    public CCMenuItemLabelAtlas(string value, string charMapFile, int itemWidth, int itemHeight, char startCharMap) : base (default(System.Action<System.Object>)) { }
    public CCMenuItemLabelAtlas(string value, string charMapFile, int itemWidth, int itemHeight, char startCharMap, CocosSharp.ICCUpdatable updatable, System.Action<System.Object> target) : base (default(System.Action<System.Object>)) { }
     
    // Properties
    public override bool Enabled { get { return default(bool); } set { } }
    public CocosSharp.CCLabelAtlas LabelAtlas { get { return default(CocosSharp.CCLabelAtlas); } set { } }
     
    // Methods
  }
  public abstract partial class CCMenuItemLabelBase : CocosSharp.CCMenuItem {
    // Constructors
    protected CCMenuItemLabelBase(System.Action<System.Object> target=null) { }
     
    // Properties
    protected CocosSharp.CCColor3B ColorBackup { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCColor3B); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public CocosSharp.CCColor3B DisabledColor { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCColor3B); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public override bool Selected { set { } }
     
    // Methods
    public override void Activate() { }
    protected void LabelWillChange(CocosSharp.CCNode oldValue, CocosSharp.CCNode newValue) { }
  }
  public partial class CCMenuItemLabelBMFont : CocosSharp.CCMenuItemLabelBase {
    // Constructors
    public CCMenuItemLabelBMFont(CocosSharp.CCLabelBMFont labelBMFont, System.Action<System.Object> target=null) : base (default(System.Action<System.Object>)) { }
     
    // Properties
    public override bool Enabled { get { return default(bool); } set { } }
    public CocosSharp.CCLabelBMFont LabelBMFont { get { return default(CocosSharp.CCLabelBMFont); } set { } }
     
    // Methods
  }
  public partial class CCMenuItemLabelTTF : CocosSharp.CCMenuItemLabelBase {
    // Constructors
    public CCMenuItemLabelTTF(CocosSharp.CCLabelTtf labelTTF, System.Action<System.Object> target=null) : base (default(System.Action<System.Object>)) { }
     
    // Properties
    public override bool Enabled { get { return default(bool); } set { } }
    public CocosSharp.CCLabelTtf LabelTTF { get { return default(CocosSharp.CCLabelTtf); } set { } }
     
    // Methods
  }
  public partial class CCMenuItemLoader : CocosSharp.CCNodeLoader {
    // Constructors
    public CCMenuItemLoader() { }
     
    // Methods
    public override CocosSharp.CCNode CreateCCNode() { return default(CocosSharp.CCNode); }
    protected override void OnHandlePropTypeBlock(CocosSharp.CCNode node, CocosSharp.CCNode parent, string propertyName, CocosSharp.BlockData pBlockData, CocosSharp.CCBReader reader) { }
    protected override void OnHandlePropTypeCheck(CocosSharp.CCNode node, CocosSharp.CCNode parent, string propertyName, bool pCheck, CocosSharp.CCBReader reader) { }
  }
  public partial class CCMenuItemToggle : CocosSharp.CCMenuItem {
    // Constructors
    public CCMenuItemToggle(params CocosSharp.CCMenuItem[] items) { }
    public CCMenuItemToggle(System.Action<System.Object> target, params CocosSharp.CCMenuItem[] items) { }
     
    // Properties
    public override bool Enabled { get { return default(bool); } set { } }
    public override bool Selected { set { } }
    public int SelectedIndex { get { return default(int); } set { } }
    public CocosSharp.CCMenuItem SelectedItem { get { return default(CocosSharp.CCMenuItem); } }
    public System.Collections.Generic.List<CocosSharp.CCMenuItem> SubItems { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Collections.Generic.List<CocosSharp.CCMenuItem>); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
     
    // Methods
    public override void Activate() { }
  }
  public partial class CCMenuLoader : CocosSharp.CCLayerLoader {
    // Constructors
    public CCMenuLoader() { }
     
    // Methods
    public override CocosSharp.CCNode CreateCCNode() { return default(CocosSharp.CCNode); }
  }
  public enum CCMenuState {
    // Fields
    Focused = 2,
    TrackingTouch = 1,
    Waiting = 0,
  }
  public partial class CCMotionStreak : CocosSharp.CCNodeRGBA, CocosSharp.ICCBlendable, CocosSharp.ICCTexture {
    // Constructors
    public CCMotionStreak() { }
    public CCMotionStreak(float fade, float minSegIn, float strokeIn, CocosSharp.CCColor3B color, CocosSharp.CCTexture2D texture) { }
    public CCMotionStreak(float fade, float minSeg, float stroke, CocosSharp.CCColor3B color, string path) { }
     
    // Properties
    public CocosSharp.CCBlendFunc BlendFunc { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCBlendFunc); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public bool FastMode { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public override bool IsColorModifiedByOpacity { get { return default(bool); } set { } }
    public override byte Opacity { get { return default(byte); } set { } }
    public override CocosSharp.CCPoint Position { set { } }
    public bool StartingPositionInitialized { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } }
    public CocosSharp.CCTexture2D Texture { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCTexture2D); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
     
    // Methods
    protected override void Draw() { }
    public void TintWithColor(CocosSharp.CCColor3B colors) { }
    public override void Update(float delta) { }
  }
  [System.FlagsAttribute]
  public enum CCMouseButton {
    // Fields
    ExtraButton1 = 8,
    ExtraButton2 = 22,
    LeftButton = 1,
    MiddleButton = 2,
    None = 0,
    RightButton = 4,
  }
  public enum CCMouseEventType {
    // Fields
    MOUSE_DOWN = 1,
    MOUSE_MOVE = 3,
    MOUSE_NONE = 0,
    MOUSE_SCROLL = 4,
    MOUSE_UP = 2,
  }
  public partial class CCMoveBy : CocosSharp.CCActionInterval {
    // Constructors
    public CCMoveBy(float duration, CocosSharp.CCPoint position) { }
     
    // Properties
    public CocosSharp.CCPoint PositionDelta { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCPoint); } }
     
    // Methods
    public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCMoveByState : CocosSharp.CCActionIntervalState {
    // Fields
    protected CocosSharp.CCPoint EndPosition;
    protected CocosSharp.CCPoint PositionDelta;
    protected CocosSharp.CCPoint PreviousPosition;
    protected CocosSharp.CCPoint StartPosition;
     
    // Constructors
    public CCMoveByState(CocosSharp.CCMoveBy action, CocosSharp.CCNode target) : base (default(CocosSharp.CCActionInterval), default(CocosSharp.CCNode)) { }
     
    // Methods
    public override void Update(float time) { }
  }
  public partial class CCMoveTo : CocosSharp.CCMoveBy {
    // Fields
    protected CocosSharp.CCPoint EndPosition;
     
    // Constructors
    public CCMoveTo(float duration, CocosSharp.CCPoint position) : base (default(float), default(CocosSharp.CCPoint)) { }
     
    // Properties
    public CocosSharp.CCPoint PositionEnd { get { return default(CocosSharp.CCPoint); } }
     
    // Methods
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCMoveToState : CocosSharp.CCMoveByState {
    // Constructors
    public CCMoveToState(CocosSharp.CCMoveTo action, CocosSharp.CCNode target) : base (default(CocosSharp.CCMoveBy), default(CocosSharp.CCNode)) { }
     
    // Methods
    public override void Update(float time) { }
  }
  public partial class CCNode : CocosSharp.ICCFocusable, CocosSharp.ICCKeypadDelegate, CocosSharp.ICCUpdatable, System.Collections.Generic.IComparer<CocosSharp.CCNode>, System.IComparable<CocosSharp.CCNode> {
    // Fields
    protected CocosSharp.CCActionManager actionManager;
    public CocosSharp.CCAffineTransform AffineTransform;
    protected CocosSharp.CCCamera camera;
    protected System.Collections.Generic.Dictionary<System.Int32, System.Collections.Generic.List<CocosSharp.CCNode>> childrenByTag;
    protected bool InverseDirty;
    protected CocosSharp.CCAffineTransform nodeToWorldTransform;
    protected float rotationX;
    protected float rotationY;
    protected bool Running;
    protected float scaleX;
    protected float scaleY;
    protected CocosSharp.CCScheduler scheduler;
    public const int TagInvalid = -1;
    protected bool visible;
     
    // Constructors
    public CCNode() { }
     
    // Properties
    public CocosSharp.CCActionManager ActionManager { get { return default(CocosSharp.CCActionManager); } set { } }
    public CocosSharp.CCAffineTransform AdditionalTransform { get { return default(CocosSharp.CCAffineTransform); } set { } }
    public virtual CocosSharp.CCPoint AnchorPoint { get { return default(CocosSharp.CCPoint); } set { } }
    public virtual CocosSharp.CCPoint AnchorPointInPoints { get { return default(CocosSharp.CCPoint); } }
    public CocosSharp.CCRect BoundingBox { get { return default(CocosSharp.CCRect); } }
    public CocosSharp.CCRect BoundingBoxInPixels { get { return default(CocosSharp.CCRect); } }
    public CocosSharp.CCCamera Camera { get { return default(CocosSharp.CCCamera); } }
    public virtual bool CanReceiveFocus { get { return default(bool); } }
    public CocosSharp.CCRawList<CocosSharp.CCNode> Children { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCRawList<CocosSharp.CCNode>); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]protected set { } }
    public int ChildrenCount { get { return default(int); } }
    public virtual CocosSharp.CCSize ContentSize { get { return default(CocosSharp.CCSize); } set { } }
    public virtual CocosSharp.CCSize ContentSizeInPixels { get { return default(CocosSharp.CCSize); } }
    public CocosSharp.CCEventDispatcher EventDispatcher { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCEventDispatcher); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public float GlobalZOrder { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public CocosSharp.CCGridBase Grid { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCGridBase); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public virtual bool HasFocus { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public virtual bool IgnoreAnchorPointForPosition { get { return default(bool); } set { } }
    protected bool IsReorderChildDirty { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public bool IsRunning { get { return default(bool); } }
    public virtual bool IsSerializable { get { return default(bool); } protected set { } }
    protected bool IsTransformDirty { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public CocosSharp.CCNode this[int tag] { get { return default(CocosSharp.CCNode); } }
    public virtual bool KeypadEnabled { get { return default(bool); } set { } }
    public int LocalZOrder { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public string Name { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(string); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public CocosSharp.CCAffineTransform NodeToWorldTransform { get { return default(CocosSharp.CCAffineTransform); } }
    protected internal uint OrderOfArrival { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(uint); } }
    public CocosSharp.CCNode Parent { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCNode); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public CocosSharp.CCAffineTransform ParentToNodeTransform { get { return default(CocosSharp.CCAffineTransform); } }
    public virtual CocosSharp.CCPoint Position { get { return default(CocosSharp.CCPoint); } set { } }
    public float PositionX { get { return default(float); } set { } }
    public float PositionY { get { return default(float); } set { } }
    public virtual float Rotation { set { } }
    public virtual float RotationX { get { return default(float); } set { } }
    public virtual float RotationY { get { return default(float); } set { } }
    public virtual float Scale { set { } }
    public virtual float ScaleX { get { return default(float); } set { } }
    public virtual float ScaleY { get { return default(float); } set { } }
    public CocosSharp.CCScheduler Scheduler { get { return default(CocosSharp.CCScheduler); } set { } }
    public virtual float SkewX { get { return default(float); } set { } }
    public virtual float SkewY { get { return default(float); } set { } }
    public int Tag { get { return default(int); } set { } }
    public object UserData { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(object); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public object UserObject { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(object); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public virtual float VertexZ { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public virtual bool Visible { get { return default(bool); } set { } }
    public CocosSharp.CCRect WorldBoundingBox { get { return default(CocosSharp.CCRect); } }
    public CocosSharp.CCAffineTransform WorldToNodeTransform { get { return default(CocosSharp.CCAffineTransform); } }
    public int ZOrder { get { return default(int); } set { } }
     
    // Methods
    public void AddAction(CocosSharp.CCAction action, bool paused=false) { }
    public void AddActions(bool paused, params CocosSharp.CCFiniteTimeAction[] actions) { }
    public void AddChild(CocosSharp.CCNode child) { }
    public void AddChild(CocosSharp.CCNode child, int zOrder) { }
    public virtual void AddChild(CocosSharp.CCNode child, int zOrder, int tag) { }
    public virtual void Cleanup() { }
    public int CompareTo(CocosSharp.CCNode that) { return default(int); }
    public CocosSharp.CCPoint ConvertPointTo(ref CocosSharp.CCPoint ptInNode, CocosSharp.CCNode target) { return default(CocosSharp.CCPoint); }
    public CocosSharp.CCPoint ConvertToNodeSpace(CocosSharp.CCPoint worldPoint) { return default(CocosSharp.CCPoint); }
    public CocosSharp.CCPoint ConvertToNodeSpaceAr(CocosSharp.CCPoint worldPoint) { return default(CocosSharp.CCPoint); }
    public CocosSharp.CCPoint ConvertTouchToNodeSpace(CocosSharp.CCTouch touch) { return default(CocosSharp.CCPoint); }
    public CocosSharp.CCPoint ConvertTouchToNodeSpaceAr(CocosSharp.CCTouch touch) { return default(CocosSharp.CCPoint); }
    public CocosSharp.CCPoint ConvertToWindowSpace(CocosSharp.CCPoint nodePoint) { return default(CocosSharp.CCPoint); }
    public CocosSharp.CCPoint ConvertToWorldSpace(CocosSharp.CCPoint nodePoint) { return default(CocosSharp.CCPoint); }
    public CocosSharp.CCPoint ConvertToWorldSpaceAr(CocosSharp.CCPoint nodePoint) { return default(CocosSharp.CCPoint); }
    public virtual void Deserialize(System.IO.Stream stream) { }
    public void Dispose() { }
    protected virtual void Dispose(bool disposing) { }
    protected virtual void Draw() { }
    ~CCNode() { }
    public virtual void ForceTransformRefresh() { }
    public CocosSharp.CCAction GetActionByTag(int tag) { return default(CocosSharp.CCAction); }
    public CocosSharp.CCRect GetBoundingBox(CocosSharp.CCNode target) { return default(CocosSharp.CCRect); }
    public CocosSharp.CCNode GetChildByTag(int tag) { return default(CocosSharp.CCNode); }
    public void GetPosition(out float x, out float y) { x = default(float); y = default(float); }
    public virtual void KeyBackClicked() { }
    public virtual void KeyMenuClicked() { }
    public virtual CocosSharp.CCAffineTransform NodeToParentTransform() { return default(CocosSharp.CCAffineTransform); }
    public int NumberOfRunningActions() { return default(int); }
    public virtual void OnEnter() { }
    public virtual void OnEnterTransitionDidFinish() { }
    public virtual void OnExit() { }
    public virtual void OnExitTransitionDidStart() { }
    public void Pause() { }
    public virtual void RemoveAllChildren() { }
    public virtual void RemoveAllChildrenByTag(int tag) { }
    public virtual void RemoveAllChildrenByTag(int tag, bool cleanup) { }
    public virtual void RemoveAllChildrenWithCleanup(bool cleanup) { }
    public void RemoveChild(CocosSharp.CCNode child) { }
    public virtual void RemoveChild(CocosSharp.CCNode child, bool cleanup) { }
    public void RemoveChildByTag(int tag) { }
    public void RemoveChildByTag(int tag, bool cleanup) { }
    public void RemoveFromParent() { }
    public void RemoveFromParentAndCleanup(bool cleanup) { }
    public virtual void ReorderChild(CocosSharp.CCNode child, int zOrder) { }
    public CocosSharp.CCActionState Repeat(uint times, CocosSharp.CCActionInterval action) { return default(CocosSharp.CCActionState); }
    public CocosSharp.CCActionState Repeat(uint times, params CocosSharp.CCFiniteTimeAction[] actions) { return default(CocosSharp.CCActionState); }
    public CocosSharp.CCActionState RepeatForever(CocosSharp.CCActionInterval action) { return default(CocosSharp.CCActionState); }
    public CocosSharp.CCActionState RepeatForever(params CocosSharp.CCFiniteTimeAction[] actions) { return default(CocosSharp.CCActionState); }
    protected virtual void ResetCleanState() { }
    public void Resume() { }
    public CocosSharp.CCActionState RunAction(CocosSharp.CCAction action) { return default(CocosSharp.CCActionState); }
    public CocosSharp.CCActionState RunActions(params CocosSharp.CCFiniteTimeAction[] actions) { return default(CocosSharp.CCActionState); }
    public void Schedule() { }
    public void Schedule(System.Action<System.Single> selector) { }
    public void Schedule(System.Action<System.Single> selector, float interval) { }
    public void Schedule(System.Action<System.Single> selector, float interval, uint repeat, float delay) { }
    public void Schedule(int priority) { }
    public void ScheduleOnce(System.Action<System.Single> selector, float delay) { }
    public virtual void Serialize(System.IO.Stream stream) { }
    public void SetPosition(float x, float y) { }
    protected virtual void SetTransformIsDirty() { }
    protected virtual void SetWorldTransformIsDirty() { }
    public virtual void SortAllChildren() { }
    public void StopAction(CocosSharp.CCAction action) { }
    public void StopActionByTag(int tag) { }
    public void StopAllActions() { }
    int System.Collections.Generic.IComparer<CocosSharp.CCNode>.Compare(CocosSharp.CCNode n1, CocosSharp.CCNode n2) { return default(int); }
    public void Transform() { }
    public void TransformAncestors() { }
    public void Unschedule() { }
    public void Unschedule(System.Action<System.Single> selector) { }
    public void UnscheduleAll() { }
    public virtual void Update(float dt) { }
    public virtual void UpdateTransform() { }
    public virtual void Visit() { }
  }
  public partial class CCNodeLoader {
    // Fields
    protected System.Collections.Generic.Dictionary<System.String, CocosSharp.CCBValue> _customProperties;
    protected const string PROPERTY_ANCHORPOINT = "anchorPoint";
    protected const string PROPERTY_CONTENTSIZE = "contentSize";
    protected const string PROPERTY_IGNOREANCHORPOINTFORPOSITION = "ignoreAnchorPointForPosition";
    protected const string PROPERTY_POSITION = "position";
    protected const string PROPERTY_ROTATION = "rotation";
    protected const string PROPERTY_ROTATIONX = "rotationX";
    protected const string PROPERTY_ROTATIONY = "rotationY";
    protected const string PROPERTY_SCALE = "scale";
    protected const string PROPERTY_SKEW = "skew";
    protected const string PROPERTY_TAG = "tag";
    protected const string PROPERTY_VISIBLE = "visible";
     
    // Constructors
    public CCNodeLoader() { }
     
    // Properties
    public virtual System.Collections.Generic.Dictionary<System.String, CocosSharp.CCBValue> CustomProperties { get { return default(System.Collections.Generic.Dictionary<System.String, CocosSharp.CCBValue>); } }
     
    // Methods
    public virtual CocosSharp.CCNode CreateCCNode() { return default(CocosSharp.CCNode); }
    public virtual CocosSharp.CCNode LoadCCNode(CocosSharp.CCNode parent, CocosSharp.CCBReader reader) { return default(CocosSharp.CCNode); }
    protected virtual void OnHandlePropTypeAnimation(CocosSharp.CCNode node, CocosSharp.CCNode parent, string propertyName, CocosSharp.CCAnimation animation, CocosSharp.CCBReader reader) { }
    protected virtual void OnHandlePropTypeBlendFunc(CocosSharp.CCNode node, CocosSharp.CCNode parent, string propertyName, CocosSharp.CCBlendFunc blendFunc, CocosSharp.CCBReader reader) { }
    protected virtual void OnHandlePropTypeBlock(CocosSharp.CCNode node, CocosSharp.CCNode parent, string propertyName, CocosSharp.BlockData pBlockData, CocosSharp.CCBReader reader) { }
    protected virtual void OnHandlePropTypeBlockCcControl(CocosSharp.CCNode node, CocosSharp.CCNode parent, string propertyName, CocosSharp.BlockCCControlData blockControlData, CocosSharp.CCBReader reader) { }
    protected virtual void OnHandlePropTypeByte(CocosSharp.CCNode node, CocosSharp.CCNode parent, string propertyName, byte pByte, CocosSharp.CCBReader reader) { }
    protected virtual void OnHandlePropTypeCCBFile(CocosSharp.CCNode node, CocosSharp.CCNode parent, string propertyName, CocosSharp.CCNode fileNode, CocosSharp.CCBReader reader) { }
    protected virtual void OnHandlePropTypeCheck(CocosSharp.CCNode node, CocosSharp.CCNode parent, string propertyName, bool pCheck, CocosSharp.CCBReader reader) { }
    protected virtual void OnHandlePropTypeColor3(CocosSharp.CCNode node, CocosSharp.CCNode parent, string propertyName, CocosSharp.CCColor3B color, CocosSharp.CCBReader reader) { }
    protected virtual void OnHandlePropTypeColor4FVar(CocosSharp.CCNode node, CocosSharp.CCNode parent, string propertyName, CocosSharp.CCColor4F[] colorVar, CocosSharp.CCBReader reader) { }
    protected virtual void OnHandlePropTypeDegrees(CocosSharp.CCNode node, CocosSharp.CCNode parent, string propertyName, float pDegrees, CocosSharp.CCBReader reader) { }
    protected virtual void OnHandlePropTypeFlip(CocosSharp.CCNode node, CocosSharp.CCNode parent, string propertyName, System.Boolean[] pFlip, CocosSharp.CCBReader reader) { }
    protected virtual void OnHandlePropTypeFloat(CocosSharp.CCNode node, CocosSharp.CCNode parent, string propertyName, float pFloat, CocosSharp.CCBReader reader) { }
    protected virtual void OnHandlePropTypeFloatScale(CocosSharp.CCNode node, CocosSharp.CCNode parent, string propertyName, float floatScale, CocosSharp.CCBReader reader) { }
    protected virtual void OnHandlePropTypeFloatVar(CocosSharp.CCNode node, CocosSharp.CCNode parent, string propertyName, System.Single[] pFoatVar, CocosSharp.CCBReader reader) { }
    protected virtual void OnHandlePropTypeFloatXY(CocosSharp.CCNode pNode, CocosSharp.CCNode pParent, string pPropertyName, System.Single[] pFoatVar, CocosSharp.CCBReader ccbReader) { }
    protected virtual void OnHandlePropTypeFntFile(CocosSharp.CCNode node, CocosSharp.CCNode parent, string propertyName, string pFntFile, CocosSharp.CCBReader reader) { }
    protected virtual void OnHandlePropTypeFontTTF(CocosSharp.CCNode node, CocosSharp.CCNode parent, string propertyName, string fontTTF, CocosSharp.CCBReader reader) { }
    protected virtual void OnHandlePropTypeInteger(CocosSharp.CCNode node, CocosSharp.CCNode parent, string propertyName, int pInteger, CocosSharp.CCBReader reader) { }
    protected virtual void OnHandlePropTypeIntegerLabeled(CocosSharp.CCNode node, CocosSharp.CCNode parent, string propertyName, int pIntegerLabeled, CocosSharp.CCBReader reader) { }
    protected virtual void OnHandlePropTypePoint(CocosSharp.CCNode node, CocosSharp.CCNode parent, string propertyName, CocosSharp.CCPoint point, CocosSharp.CCBReader reader) { }
    protected virtual void OnHandlePropTypePointLock(CocosSharp.CCNode node, CocosSharp.CCNode parent, string propertyName, CocosSharp.CCPoint pPointLock, CocosSharp.CCBReader reader) { }
    protected virtual void OnHandlePropTypePosition(CocosSharp.CCNode node, CocosSharp.CCNode parent, string propertyName, CocosSharp.CCPoint pPosition, CocosSharp.CCBReader reader) { }
    protected virtual void OnHandlePropTypeScaleLock(CocosSharp.CCNode node, CocosSharp.CCNode parent, string propertyName, System.Single[] pScaleLock, CocosSharp.CCBReader reader) { }
    protected virtual void OnHandlePropTypeSize(CocosSharp.CCNode node, CocosSharp.CCNode parent, string propertyName, CocosSharp.CCSize pSize, CocosSharp.CCBReader reader) { }
    protected virtual void OnHandlePropTypeSpriteFrame(CocosSharp.CCNode node, CocosSharp.CCNode parent, string propertyName, CocosSharp.CCSpriteFrame spriteFrame, CocosSharp.CCBReader reader) { }
    protected virtual void OnHandlePropTypeString(CocosSharp.CCNode node, CocosSharp.CCNode parent, string propertyName, string pString, CocosSharp.CCBReader reader) { }
    protected virtual void OnHandlePropTypeText(CocosSharp.CCNode node, CocosSharp.CCNode parent, string propertyName, string pText, CocosSharp.CCBReader reader) { }
    protected virtual void OnHandlePropTypeTexture(CocosSharp.CCNode node, CocosSharp.CCNode parent, string propertyName, CocosSharp.CCTexture2D texture, CocosSharp.CCBReader reader) { }
    public virtual void ParseProperties(CocosSharp.CCNode node, CocosSharp.CCNode parent, CocosSharp.CCBReader reader) { }
    protected virtual CocosSharp.CCAnimation ParsePropTypeAnimation(CocosSharp.CCNode node, CocosSharp.CCNode parent, CocosSharp.CCBReader reader) { return default(CocosSharp.CCAnimation); }
    protected virtual CocosSharp.CCBlendFunc ParsePropTypeBlendFunc(CocosSharp.CCNode node, CocosSharp.CCNode parent, CocosSharp.CCBReader reader) { return default(CocosSharp.CCBlendFunc); }
    protected virtual CocosSharp.BlockData ParsePropTypeBlock(CocosSharp.CCNode node, CocosSharp.CCNode parent, CocosSharp.CCBReader reader) { return default(CocosSharp.BlockData); }
    protected virtual CocosSharp.BlockCCControlData ParsePropTypeBlockCcControl(CocosSharp.CCNode node, CocosSharp.CCNode parent, CocosSharp.CCBReader reader) { return default(CocosSharp.BlockCCControlData); }
    protected virtual byte ParsePropTypeByte(CocosSharp.CCNode node, CocosSharp.CCNode parent, CocosSharp.CCBReader reader, string propertyName) { return default(byte); }
    protected virtual CocosSharp.CCNode ParsePropTypeCcbFile(CocosSharp.CCNode node, CocosSharp.CCNode parent, CocosSharp.CCBReader pCCBReader) { return default(CocosSharp.CCNode); }
    protected virtual bool ParsePropTypeCheck(CocosSharp.CCNode node, CocosSharp.CCNode parent, CocosSharp.CCBReader reader, string propertyName) { return default(bool); }
    protected virtual CocosSharp.CCColor3B ParsePropTypeColor3(CocosSharp.CCNode node, CocosSharp.CCNode parent, CocosSharp.CCBReader reader, string propertyName) { return default(CocosSharp.CCColor3B); }
    protected virtual CocosSharp.CCColor4F[] ParsePropTypeColor4FVar(CocosSharp.CCNode node, CocosSharp.CCNode parent, CocosSharp.CCBReader reader) { return default(CocosSharp.CCColor4F[]); }
    protected virtual float ParsePropTypeDegrees(CocosSharp.CCNode node, CocosSharp.CCNode parent, CocosSharp.CCBReader reader, string propertyName) { return default(float); }
    protected virtual System.Boolean[] ParsePropTypeFlip(CocosSharp.CCNode node, CocosSharp.CCNode parent, CocosSharp.CCBReader reader) { return default(System.Boolean[]); }
    protected virtual float ParsePropTypeFloat(CocosSharp.CCNode node, CocosSharp.CCNode parent, CocosSharp.CCBReader reader) { return default(float); }
    protected virtual float ParsePropTypeFloatScale(CocosSharp.CCNode node, CocosSharp.CCNode parent, CocosSharp.CCBReader reader) { return default(float); }
    protected virtual System.Single[] ParsePropTypeFloatVar(CocosSharp.CCNode node, CocosSharp.CCNode parent, CocosSharp.CCBReader reader) { return default(System.Single[]); }
    protected virtual System.Single[] ParsePropTypeFloatXY(CocosSharp.CCNode pNode, CocosSharp.CCNode pParent, CocosSharp.CCBReader ccbReader) { return default(System.Single[]); }
    protected virtual string ParsePropTypeFntFile(CocosSharp.CCNode node, CocosSharp.CCNode parent, CocosSharp.CCBReader reader) { return default(string); }
    protected virtual string ParsePropTypeFontTTF(CocosSharp.CCNode node, CocosSharp.CCNode parent, CocosSharp.CCBReader reader) { return default(string); }
    protected virtual int ParsePropTypeInteger(CocosSharp.CCNode node, CocosSharp.CCNode parent, CocosSharp.CCBReader reader) { return default(int); }
    protected virtual int ParsePropTypeIntegerLabeled(CocosSharp.CCNode node, CocosSharp.CCNode parent, CocosSharp.CCBReader reader) { return default(int); }
    protected virtual CocosSharp.CCPoint ParsePropTypePoint(CocosSharp.CCNode node, CocosSharp.CCNode parent, CocosSharp.CCBReader reader) { return default(CocosSharp.CCPoint); }
    protected virtual CocosSharp.CCPoint ParsePropTypePointLock(CocosSharp.CCNode node, CocosSharp.CCNode parent, CocosSharp.CCBReader reader) { return default(CocosSharp.CCPoint); }
    protected virtual CocosSharp.CCPoint ParsePropTypePosition(CocosSharp.CCNode node, CocosSharp.CCNode parent, CocosSharp.CCBReader reader, string propertyName) { return default(CocosSharp.CCPoint); }
    protected virtual System.Single[] ParsePropTypeScaleLock(CocosSharp.CCNode node, CocosSharp.CCNode parent, CocosSharp.CCBReader reader, string propertyName) { return default(System.Single[]); }
    protected virtual CocosSharp.CCSize ParsePropTypeSize(CocosSharp.CCNode node, CocosSharp.CCNode parent, CocosSharp.CCBReader reader) { return default(CocosSharp.CCSize); }
    protected virtual CocosSharp.CCSpriteFrame ParsePropTypeSpriteFrame(CocosSharp.CCNode node, CocosSharp.CCNode parent, CocosSharp.CCBReader reader, string propertyName) { return default(CocosSharp.CCSpriteFrame); }
    protected virtual string ParsePropTypeString(CocosSharp.CCNode node, CocosSharp.CCNode parent, CocosSharp.CCBReader reader) { return default(string); }
    protected virtual string ParsePropTypeText(CocosSharp.CCNode node, CocosSharp.CCNode parent, CocosSharp.CCBReader reader) { return default(string); }
    protected virtual CocosSharp.CCTexture2D ParsePropTypeTexture(CocosSharp.CCNode node, CocosSharp.CCNode parent, CocosSharp.CCBReader reader) { return default(CocosSharp.CCTexture2D); }
  }
  public partial class CCNodeLoaderLibrary {
    // Constructors
    public CCNodeLoaderLibrary() { }
     
    // Properties
    public static CocosSharp.CCNodeLoaderLibrary SharedInstance { get { return default(CocosSharp.CCNodeLoaderLibrary); } }
     
    // Methods
    public CocosSharp.CCNodeLoader GetCCNodeLoader(string pClassName) { return default(CocosSharp.CCNodeLoader); }
    public void Purge(bool pDelete) { }
    public static void PurgeSharedCCNodeLoaderLibrary() { }
    public void RegisterCCNodeLoader(string pClassName, CocosSharp.CCNodeLoader pCCNodeLoader) { }
    public void RegisterDefaultCCNodeLoaders() { }
    public void UnregisterCCNodeLoader(string pClassName) { }
  }
  public partial class CCNodeRGBA : CocosSharp.CCNode, CocosSharp.ICCColor {
    // Constructors
    public CCNodeRGBA() { }
     
    // Properties
    public virtual CocosSharp.CCColor3B Color { get { return default(CocosSharp.CCColor3B); } set { } }
    public CocosSharp.CCColor3B DisplayedColor { get { return default(CocosSharp.CCColor3B); } }
    public byte DisplayedOpacity { get { return default(byte); } }
    public virtual bool IsColorCascaded { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public virtual bool IsColorModifiedByOpacity { get { return default(bool); } set { } }
    public virtual bool IsOpacityCascaded { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public virtual byte Opacity { get { return default(byte); } set { } }
    protected CocosSharp.CCColor3B RealColor { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCColor3B); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    protected byte RealOpacity { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(byte); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
     
    // Methods
    public virtual void UpdateDisplayedColor(CocosSharp.CCColor3B parentColor) { }
    public virtual void UpdateDisplayedOpacity(byte parentOpacity) { }
  }
  public partial class CCOGLES {
    // Fields
    public static readonly int GL_ALWAYS;
    public const int GL_DST_ALPHA = 772;
    public const int GL_DST_COLOR = 774;
    public static readonly int GL_EQUAL;
    public static readonly int GL_GEQUAL;
    public static readonly int GL_GREATER;
    public static readonly int GL_LEQUAL;
    public static readonly int GL_LESS;
    public const int GL_NEVER = 512;
    public static readonly int GL_NOTEQUAL;
    public const int GL_ONE = 1;
    public const int GL_ONE_MINUS_DST_ALPHA = 773;
    public const int GL_ONE_MINUS_DST_COLOR = 775;
    public const int GL_ONE_MINUS_SRC_ALPHA = 771;
    public const int GL_ONE_MINUS_SRC_COLOR = 769;
    public const int GL_SRC_ALPHA = 770;
    public const int GL_SRC_ALPHA_SATURATE = 776;
    public const int GL_SRC_COLOR = 768;
    public const int GL_ZERO = 0;
     
    // Constructors
    public CCOGLES() { }
  }
  public partial class CCOrbitCamera : CocosSharp.CCActionCamera {
    // Constructors
    public CCOrbitCamera(float t, float radius, float deltaRadius, float angleZ, float deltaAngleZ, float angleX, float deltaAngleX) : base (default(float)) { }
     
    // Properties
    public float AngleX { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } }
    public float AngleZ { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } }
    public float DeltaAngleX { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } }
    public float DeltaAngleZ { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } }
    public float DeltaRadius { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } }
    public float Radius { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } }
     
    // Methods
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCOrbitCameraState : CocosSharp.CCActionCameraState {
    // Constructors
    public CCOrbitCameraState(CocosSharp.CCOrbitCamera action, CocosSharp.CCNode target) : base (default(CocosSharp.CCActionCamera), default(CocosSharp.CCNode)) { }
     
    // Properties
    protected float AngleX { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    protected float AngleZ { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    protected float DeltaAngleX { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    protected float DeltaAngleZ { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    protected float DeltaRadius { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    protected float RadDeltaX { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    protected float RadDeltaZ { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    protected float Radius { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    protected float RadX { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    protected float RadZ { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
     
    // Methods
    public override void Update(float time) { }
  }
  public partial class CCPageTurn3D : CocosSharp.CCGrid3DAction {
    // Constructors
    public CCPageTurn3D(float duration, CocosSharp.CCGridSize gridSize) : base (default(float)) { }
     
    // Methods
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCPageTurn3DState : CocosSharp.CCGrid3DActionState {
    // Constructors
    public CCPageTurn3DState(CocosSharp.CCPageTurn3D action, CocosSharp.CCNode target) : base (default(CocosSharp.CCGrid3DAction), default(CocosSharp.CCNode)) { }
     
    // Methods
    public override void Update(float time) { }
  }
  public partial class CCParallaxNode : CocosSharp.CCNode {
    // Constructors
    public CCParallaxNode() { }
     
    // Methods
    public virtual void AddChild(CocosSharp.CCNode child, int z, CocosSharp.CCPoint ratio, CocosSharp.CCPoint offset) { }
    public override void AddChild(CocosSharp.CCNode child, int zOrder, int tag) { }
    public override void RemoveAllChildrenWithCleanup(bool cleanup) { }
    public override void RemoveChild(CocosSharp.CCNode child, bool cleanup) { }
    public override void Visit() { }
  }
  public partial class CCParallel : CocosSharp.CCActionInterval {
    // Constructors
    public CCParallel(params CocosSharp.CCFiniteTimeAction[] actions) { }
     
    // Properties
    public CocosSharp.CCFiniteTimeAction[] Actions { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCFiniteTimeAction[]); } }
     
    // Methods
    public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCParallelState : CocosSharp.CCActionIntervalState {
    // Constructors
    public CCParallelState(CocosSharp.CCParallel action, CocosSharp.CCNode target) : base (default(CocosSharp.CCActionInterval), default(CocosSharp.CCNode)) { }
     
    // Properties
    protected CocosSharp.CCFiniteTimeAction[] Actions { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCFiniteTimeAction[]); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    protected CocosSharp.CCFiniteTimeActionState[] ActionStates { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCFiniteTimeActionState[]); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
     
    // Methods
    public override void Stop() { }
    public override void Update(float time) { }
  }
  public partial class CCParticleBatchNode : CocosSharp.CCNode, CocosSharp.ICCBlendable, CocosSharp.ICCTexture {
    // Fields
    public const int ParticleDefaultCapacity = 500;
     
    // Constructors
    public CCParticleBatchNode(CocosSharp.CCTexture2D tex, int capacity=500) { }
    public CCParticleBatchNode(string imageFile, int capacity=500) { }
     
    // Properties
    public CocosSharp.CCBlendFunc BlendFunc { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCBlendFunc); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public CocosSharp.CCTexture2D Texture { get { return default(CocosSharp.CCTexture2D); } set { } }
    public CocosSharp.CCTextureAtlas TextureAtlas { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCTextureAtlas); } }
     
    // Methods
    public override void AddChild(CocosSharp.CCNode child, int zOrder, int tag) { }
    public void DisableParticle(int particleIndex) { }
    protected override void Draw() { }
    public override void RemoveAllChildrenWithCleanup(bool doCleanup) { }
    public override void RemoveChild(CocosSharp.CCNode child, bool cleanup) { }
    public void RemoveChildAtIndex(int index, bool doCleanup) { }
    public override void ReorderChild(CocosSharp.CCNode child, int zOrder) { }
    public override void Visit() { }
  }
  public partial class CCParticleExplosion : CocosSharp.CCParticleSystemQuad {
    // Constructors
    public CCParticleExplosion() : base (default(int), default(CocosSharp.CCEmitterMode)) { }
  }
  public partial class CCParticleFire : CocosSharp.CCParticleSystemQuad {
    // Constructors
    public CCParticleFire() : base (default(int), default(CocosSharp.CCEmitterMode)) { }
  }
  public partial class CCParticleFireworks : CocosSharp.CCParticleSystemQuad {
    // Constructors
    public CCParticleFireworks() : base (default(int), default(CocosSharp.CCEmitterMode)) { }
  }
  public partial class CCParticleFlower : CocosSharp.CCParticleSystemQuad {
    // Constructors
    public CCParticleFlower() : base (default(int), default(CocosSharp.CCEmitterMode)) { }
  }
  public partial class CCParticleGalaxy : CocosSharp.CCParticleSystemQuad {
    // Constructors
    public CCParticleGalaxy() : base (default(int), default(CocosSharp.CCEmitterMode)) { }
  }
  public partial class CCParticleMeteor : CocosSharp.CCParticleSystemQuad {
    // Constructors
    public CCParticleMeteor() : base (default(int), default(CocosSharp.CCEmitterMode)) { }
  }
  public partial class CCParticleRain : CocosSharp.CCParticleSystemQuad {
    // Constructors
    public CCParticleRain() : base (default(int), default(CocosSharp.CCEmitterMode)) { }
  }
  public partial class CCParticleSmoke : CocosSharp.CCParticleSystemQuad {
    // Constructors
    public CCParticleSmoke() : base (default(int), default(CocosSharp.CCEmitterMode)) { }
  }
  public partial class CCParticleSnow : CocosSharp.CCParticleSystemQuad {
    // Constructors
    public CCParticleSnow() : base (default(int), default(CocosSharp.CCEmitterMode)) { }
  }
  public partial class CCParticleSpiral : CocosSharp.CCParticleSystemQuad {
    // Constructors
    public CCParticleSpiral() : base (default(int), default(CocosSharp.CCEmitterMode)) { }
  }
  public partial class CCParticleSun : CocosSharp.CCParticleSystemQuad {
    // Constructors
    public CCParticleSun() : base (default(int), default(CocosSharp.CCEmitterMode)) { }
    public CCParticleSun(int num) : base (default(int), default(CocosSharp.CCEmitterMode)) { }
  }
  public partial class CCParticleSystem : CocosSharp.CCNode, CocosSharp.ICCBlendable, CocosSharp.ICCTexture {
    // Fields
    public const int ParticleDurationInfinity = -1;
    public const int ParticleStartRadiusEqualToEndRadius = -1;
    public const int ParticleStartSizeEqualToEndSize = -1;
     
    // Constructors
    public CCParticleSystem(CocosSharp.CCParticleSystemConfig particleConfig) { }
    protected CCParticleSystem(int numberOfParticles, CocosSharp.CCEmitterMode emitterMode=(CocosSharp.CCEmitterMode)(0)) { }
    public CCParticleSystem(string plistFile, string directoryName=null) { }
     
    // Properties
    protected int AllocatedParticles { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public float Angle { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public float AngleVar { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public int AtlasIndex { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public bool AutoRemoveOnFinish { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public virtual CocosSharp.CCParticleBatchNode BatchNode { get { return default(CocosSharp.CCParticleBatchNode); } set { } }
    public bool BlendAdditive { get { return default(bool); } set { } }
    public CocosSharp.CCBlendFunc BlendFunc { get { return default(CocosSharp.CCBlendFunc); } set { } }
    public float Duration { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    protected float Elapsed { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } }
    public float EmissionRate { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    protected float EmitCounter { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public CocosSharp.CCEmitterMode EmitterMode { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCEmitterMode); } }
    public CocosSharp.CCColor4F EndColor { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCColor4F); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public CocosSharp.CCColor4F EndColorVar { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCColor4F); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public float EndRadius { get { return default(float); } set { } }
    public float EndRadiusVar { get { return default(float); } set { } }
    public float EndSize { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public float EndSizeVar { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public float EndSpin { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public float EndSpinVar { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public CocosSharp.CCPoint Gravity { get { return default(CocosSharp.CCPoint); } set { } }
    protected CocosSharp.CCParticleSystem.GravityMoveMode GravityMode { get { return default(CocosSharp.CCParticleSystem.GravityMoveMode); } set { } }
    protected CocosSharp.CCParticleSystem.CCParticleGravity[] GravityParticles { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCParticleSystem.CCParticleGravity[]); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public bool IsActive { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } }
    public bool IsFull { get { return default(bool); } }
    public float Life { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public float LifeVar { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public bool OpacityModifyRGB { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public int ParticleCount { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } }
    public CocosSharp.CCPositionType PositionType { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCPositionType); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public CocosSharp.CCPoint PositionVar { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCPoint); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public float RadialAccel { get { return default(float); } set { } }
    public float RadialAccelVar { get { return default(float); } set { } }
    protected CocosSharp.CCParticleSystem.RadialMoveMode RadialMode { get { return default(CocosSharp.CCParticleSystem.RadialMoveMode); } set { } }
    protected CocosSharp.CCParticleSystem.CCParticleRadial[] RadialParticles { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCParticleSystem.CCParticleRadial[]); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public float RotatePerSecond { get { return default(float); } set { } }
    public float RotatePerSecondVar { get { return default(float); } set { } }
    public bool RotationIsDir { get { return default(bool); } set { } }
    public CocosSharp.CCPoint SourcePosition { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCPoint); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public float Speed { get { return default(float); } set { } }
    public float SpeedVar { get { return default(float); } set { } }
    public CocosSharp.CCColor4F StartColor { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCColor4F); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public CocosSharp.CCColor4F StartColorVar { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCColor4F); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public float StartRadius { get { return default(float); } set { } }
    public float StartRadiusVar { get { return default(float); } set { } }
    public float StartSize { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public float StartSizeVar { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public float StartSpin { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public float StartSpinVar { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public float TangentialAccel { get { return default(float); } set { } }
    public float TangentialAccelVar { get { return default(float); } set { } }
    public virtual CocosSharp.CCTexture2D Texture { get { return default(CocosSharp.CCTexture2D); } set { } }
    public virtual int TotalParticles { get { return default(int); } set { } }
     
    // Methods
    public override void OnEnter() { }
    public override void OnExit() { }
    protected virtual void PostStep() { }
    public void ResetSystem() { }
    public void StopSystem() { }
    public override void Update(float dt) { }
    public virtual void UpdateQuads() { }
    public void UpdateWithNoTime() { }
     
    // Nested Types
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Size=1)]
    protected partial struct CCParticleBase {
    }
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    protected partial struct CCParticleGravity {
    }
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    protected partial struct CCParticleRadial {
    }
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Size=1)]
    protected partial struct GravityMoveMode {
    }
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Size=1)]
    protected partial struct RadialMoveMode {
    }
  }
  public partial class CCParticleSystemCache : CocosSharp.ICCUpdatable, System.IDisposable {
    internal CCParticleSystemCache() { }
    // Fields
    protected System.Collections.Generic.Dictionary<System.String, CocosSharp.CCParticleSystemConfig> psConfigs;
     
    // Properties
    public CocosSharp.CCParticleSystemConfig this[string key] { get { return default(CocosSharp.CCParticleSystemConfig); } }
    public static CocosSharp.CCParticleSystemCache SharedParticleSystemCache { get { return default(CocosSharp.CCParticleSystemCache); } }
     
    // Methods
    public CocosSharp.CCParticleSystemConfig AddParticleSystem(string fileConfig, string directoryName=null) { return default(CocosSharp.CCParticleSystemConfig); }
    public void AddParticleSystemAsync(string fileConfig, System.Action<CocosSharp.CCParticleSystemConfig> action, string directoryName=null) { }
    public bool Contains(string assetFile) { return default(bool); }
    public void Dispose() { }
    protected virtual void Dispose(bool disposing) { }
    public void DumpCachedInfo() { }
    public CocosSharp.CCParticleSystemConfig ParticleSystemForKey(string key) { return default(CocosSharp.CCParticleSystemConfig); }
    public static void PurgeSharedConfigCache() { }
    public void Remove(CocosSharp.CCParticleSystemConfig particleSystem) { }
    public void RemoveAll() { }
    public void RemoveForKey(string particleSystemKeyName) { }
    public void RemoveUnused() { }
    public void UnloadContent() { }
    public void Update(float dt) { }
  }
  public partial class CCParticleSystemConfig : System.IDisposable {
    // Constructors
    public CCParticleSystemConfig() { }
    public CCParticleSystemConfig(string plistFile, string directoryName=null) { }
     
    // Properties
    public float Angle { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public float AngleVar { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public CocosSharp.CCBlendFunc BlendFunc { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCBlendFunc); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public string DirectoryName { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(string); } }
    public float Duration { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public CocosSharp.CCEmitterMode EmitterMode { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCEmitterMode); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public CocosSharp.CCColor4F EndColor { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCColor4F); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public CocosSharp.CCColor4F EndColorVar { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCColor4F); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public float EndSize { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public float EndSizeVar { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public float EndSpin { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public float EndSpinVar { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public CocosSharp.CCPoint Gravity { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCPoint); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public float GravityRadialAccel { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public float GravityRadialAccelVar { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public bool GravityRotationIsDir { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public float GravitySpeed { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public float GravitySpeedVar { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public float GravityTangentialAccel { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public float GravityTangentialAccelVar { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public float Life { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public float LifeVar { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public int MaxParticles { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public string Name { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(string); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public CocosSharp.CCParticleSystemType ParticleSystemType { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCParticleSystemType); } }
    public CocosSharp.CCPoint Position { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCPoint); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public CocosSharp.CCPositionType PositionType { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCPositionType); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public CocosSharp.CCPoint PositionVar { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCPoint); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public float RadialEndRadius { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public float RadialEndRadiusVar { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public float RadialRotatePerSecond { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public float RadialRotatePerSecondVar { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public float RadialStartRadius { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public float RadialStartRadiusVar { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public CocosSharp.CCColor4F StartColor { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCColor4F); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public CocosSharp.CCColor4F StartColorVar { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCColor4F); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public float StartSize { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public float StartSizeVar { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public float StartSpin { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public float StartSpinVar { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public CocosSharp.CCTexture2D Texture { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCTexture2D); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public string TextureData { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(string); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public string TextureName { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(string); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
     
    // Methods
    public void Dispose() { }
    protected virtual void Dispose(bool disposing) { }
  }
  public partial class CCParticleSystemQuad : CocosSharp.CCParticleSystem {
    // Constructors
    public CCParticleSystemQuad(CocosSharp.CCParticleSystemConfig config) : base (default(string), default(string)) { }
    public CCParticleSystemQuad(int numberOfParticles, CocosSharp.CCEmitterMode emitterMode=(CocosSharp.CCEmitterMode)(0)) : base (default(string), default(string)) { }
    public CCParticleSystemQuad(string plistFile, string directoryName=null) : base (default(string), default(string)) { }
     
    // Properties
    public override CocosSharp.CCParticleBatchNode BatchNode { set { } }
    public override CocosSharp.CCTexture2D Texture { set { } }
    public CocosSharp.CCRect TextureRect { set { } }
    public override int TotalParticles { set { } }
     
    // Methods
    public CocosSharp.CCParticleSystemQuad Clone() { return default(CocosSharp.CCParticleSystemQuad); }
    protected override void Draw() { }
    public override void UpdateQuads() { }
  }
  public enum CCParticleSystemType {
    // Fields
    Cocos2D = 1,
    Custom = 2,
    Internal = 0,
  }
  public partial class CCPlace : CocosSharp.CCActionInstant {
    // Constructors
    public CCPlace(CocosSharp.CCPoint pos) { }
    public CCPlace(int posX, int posY) { }
     
    // Properties
    public CocosSharp.CCPoint Position { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCPoint); } }
     
    // Methods
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCPlaceState : CocosSharp.CCActionInstantState {
    // Constructors
    public CCPlaceState(CocosSharp.CCPlace action, CocosSharp.CCNode target) : base (default(CocosSharp.CCActionInstant), default(CocosSharp.CCNode)) { }
  }
  public enum CCPlayerIndex {
    // Fields
    Four = 3,
    One = 0,
    Three = 2,
    Two = 1,
  }
  [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
  public partial struct CCPoint {
    // Fields
    public static readonly CocosSharp.CCPoint AnchorLowerLeft;
    public static readonly CocosSharp.CCPoint AnchorLowerRight;
    public static readonly CocosSharp.CCPoint AnchorMiddle;
    public static readonly CocosSharp.CCPoint AnchorMiddleBottom;
    public static readonly CocosSharp.CCPoint AnchorMiddleLeft;
    public static readonly CocosSharp.CCPoint AnchorMiddleRight;
    public static readonly CocosSharp.CCPoint AnchorMiddleTop;
    public static readonly CocosSharp.CCPoint AnchorUpperLeft;
    public static readonly CocosSharp.CCPoint AnchorUpperRight;
    public float X;
    public float Y;
    public static readonly CocosSharp.CCPoint Zero;
     
    // Constructors
    public CCPoint(CocosSharp.CCPoint pt) { throw new System.NotImplementedException(); }
    public CCPoint(CocosSharp.CCVector2 v) { throw new System.NotImplementedException(); }
    public CCPoint(float x, float y) { throw new System.NotImplementedException(); }
     
    // Properties
    public CocosSharp.CCPoint InvertY { get { return default(CocosSharp.CCPoint); } }
    public float Length { get { return default(float); } }
    public float LengthSQ { get { return default(float); } }
    public float LengthSquare { get { return default(float); } }
    public CocosSharp.CCPoint Reverse { get { return default(CocosSharp.CCPoint); } }
     
    // Methods
    public static float AngleSigned(CocosSharp.CCPoint a, CocosSharp.CCPoint b) { return default(float); }
    public static CocosSharp.CCPoint Clamp(CocosSharp.CCPoint p, CocosSharp.CCPoint from, CocosSharp.CCPoint to) { return default(CocosSharp.CCPoint); }
    public static float Clamp(float value, float min_inclusive, float max_inclusive) { return default(float); }
    public static CocosSharp.CCPoint ComputationOperation(CocosSharp.CCPoint p, CocosSharp.CCPoint.ComputationOperationDelegate del) { return default(CocosSharp.CCPoint); }
    public static float CrossProduct(CocosSharp.CCPoint v1, CocosSharp.CCPoint v2) { return default(float); }
    public static float Distance(CocosSharp.CCPoint v1, CocosSharp.CCPoint v2) { return default(float); }
    public float DistanceSQ(ref CocosSharp.CCPoint v2) { return default(float); }
    public static float Dot(CocosSharp.CCPoint p1, CocosSharp.CCPoint p2) { return default(float); }
    public static float DotProduct(CocosSharp.CCPoint v1, CocosSharp.CCPoint v2) { return default(float); }
    public static bool Equal(ref CocosSharp.CCPoint point1, ref CocosSharp.CCPoint point2) { return default(bool); }
    public bool Equals(CocosSharp.CCPoint p) { return default(bool); }
    public override bool Equals(object obj) { return default(bool); }
    public static CocosSharp.CCPoint ForAngle(float a) { return default(CocosSharp.CCPoint); }
    [System.ObsoleteAttribute("Use explicit cast (CCPoint)size.")]
    public static CocosSharp.CCPoint FromSize(CocosSharp.CCSize s) { return default(CocosSharp.CCPoint); }
    public static bool FuzzyEqual(CocosSharp.CCPoint a, CocosSharp.CCPoint b, float variance) { return default(bool); }
    public override int GetHashCode() { return default(int); }
    public static CocosSharp.CCPoint IntersectPoint(CocosSharp.CCPoint A, CocosSharp.CCPoint B, CocosSharp.CCPoint C, CocosSharp.CCPoint D) { return default(CocosSharp.CCPoint); }
    public static CocosSharp.CCPoint Lerp(CocosSharp.CCPoint a, CocosSharp.CCPoint b, float alpha) { return default(CocosSharp.CCPoint); }
    public static bool LineIntersect(CocosSharp.CCPoint A, CocosSharp.CCPoint B, CocosSharp.CCPoint C, CocosSharp.CCPoint D, ref float S, ref float T) { return default(bool); }
    public static CocosSharp.CCPoint Midpoint(CocosSharp.CCPoint p1, CocosSharp.CCPoint p2) { return default(CocosSharp.CCPoint); }
    public static CocosSharp.CCPoint MultiplyComponents(CocosSharp.CCPoint a, CocosSharp.CCPoint b) { return default(CocosSharp.CCPoint); }
    public float Normalize() { return default(float); }
    public static CocosSharp.CCPoint Normalize(CocosSharp.CCPoint p) { return default(CocosSharp.CCPoint); }
    public CocosSharp.CCPoint Offset(float dx, float dy) { return default(CocosSharp.CCPoint); }
    public static CocosSharp.CCPoint operator +(CocosSharp.CCPoint p1, CocosSharp.CCPoint p2) { return default(CocosSharp.CCPoint); }
    public static CocosSharp.CCPoint operator /(CocosSharp.CCPoint p, float value) { return default(CocosSharp.CCPoint); }
    public static bool operator ==(CocosSharp.CCPoint p1, CocosSharp.CCPoint p2) { return default(bool); }
    public static explicit operator CocosSharp.CCPoint (CocosSharp.CCSize size) { return default(CocosSharp.CCPoint); }
    public static implicit operator CocosSharp.CCVector2 (CocosSharp.CCPoint point) { return default(CocosSharp.CCVector2); }
    public static implicit operator CocosSharp.CCPoint (CocosSharp.CCVector2 point) { return default(CocosSharp.CCPoint); }
    public static bool operator !=(CocosSharp.CCPoint p1, CocosSharp.CCPoint p2) { return default(bool); }
    public static CocosSharp.CCPoint operator *(CocosSharp.CCPoint p1, CocosSharp.CCPoint p2) { return default(CocosSharp.CCPoint); }
    public static CocosSharp.CCPoint operator *(CocosSharp.CCPoint p, float value) { return default(CocosSharp.CCPoint); }
    public static CocosSharp.CCPoint operator -(CocosSharp.CCPoint p1, CocosSharp.CCPoint p2) { return default(CocosSharp.CCPoint); }
    public static CocosSharp.CCPoint operator -(CocosSharp.CCPoint p1) { return default(CocosSharp.CCPoint); }
    public static CocosSharp.CCPoint operator +(CocosSharp.CCPoint p1) { return default(CocosSharp.CCPoint); }
    public static CocosSharp.CCPoint Parse(string s) { return default(CocosSharp.CCPoint); }
    public static CocosSharp.CCPoint Perp(CocosSharp.CCPoint p) { return default(CocosSharp.CCPoint); }
    public static CocosSharp.CCPoint PerpendicularClockwise(CocosSharp.CCPoint v) { return default(CocosSharp.CCPoint); }
    public static CocosSharp.CCPoint PerpendicularCounterClockwise(CocosSharp.CCPoint v) { return default(CocosSharp.CCPoint); }
    public static CocosSharp.CCPoint Project(CocosSharp.CCPoint v1, CocosSharp.CCPoint v2) { return default(CocosSharp.CCPoint); }
    public static CocosSharp.CCPoint Rotate(CocosSharp.CCPoint v1, CocosSharp.CCPoint v2) { return default(CocosSharp.CCPoint); }
    public static CocosSharp.CCPoint RotateByAngle(CocosSharp.CCPoint v, CocosSharp.CCPoint pivot, float angle) { return default(CocosSharp.CCPoint); }
    public static bool SegmentIntersect(CocosSharp.CCPoint A, CocosSharp.CCPoint B, CocosSharp.CCPoint C, CocosSharp.CCPoint D) { return default(bool); }
    public CocosSharp.CCPoint Sub(ref CocosSharp.CCPoint v2) { return default(CocosSharp.CCPoint); }
    public static float ToAngle(CocosSharp.CCPoint v) { return default(float); }
    public override string ToString() { return default(string); }
    public static CocosSharp.CCPoint Unrotate(CocosSharp.CCPoint v1, CocosSharp.CCPoint v2) { return default(CocosSharp.CCPoint); }
     
    // Nested Types
    public delegate float ComputationOperationDelegate(float a);
  }

  [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
  public partial struct CCPointI {
    // Fields
    public int X;
    public int Y;
     
    // Constructors
    public CCPointI(int x, int y) { throw new System.NotImplementedException(); }
     
    // Methods
    public int Distance(ref CocosSharp.CCPointI p) { return default(int); }
    public bool Equals(CocosSharp.CCPointI other) { return default(bool); }
    public bool Equals(ref CocosSharp.CCPointI p) { return default(bool); }
    public override bool Equals(object obj) { return default(bool); }
    public override int GetHashCode() { return default(int); }
    public static CocosSharp.CCPointI operator +(CocosSharp.CCPointI p1, CocosSharp.CCPointI p2) { return default(CocosSharp.CCPointI); }
    public static bool operator ==(CocosSharp.CCPointI p1, CocosSharp.CCPointI p2) { return default(bool); }
    public static implicit operator CocosSharp.CCPoint (CocosSharp.CCPointI p) { return default(CocosSharp.CCPoint); }
    public static bool operator !=(CocosSharp.CCPointI p1, CocosSharp.CCPointI p2) { return default(bool); }
    public static CocosSharp.CCPointI operator -(CocosSharp.CCPointI p1, CocosSharp.CCPointI p2) { return default(CocosSharp.CCPointI); }
    public static CocosSharp.CCPointI operator -(CocosSharp.CCPointI p1) { return default(CocosSharp.CCPointI); }
    public static CocosSharp.CCPointI operator +(CocosSharp.CCPointI p1) { return default(CocosSharp.CCPointI); }
  }
  public partial class CCPointObject {
    // Constructors
    public CCPointObject(CocosSharp.CCPoint ratio, CocosSharp.CCPoint offset) { }
     
    // Properties
    public CocosSharp.CCNode Child { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCNode); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public CocosSharp.CCPoint Offset { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCPoint); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public CocosSharp.CCPoint Ratio { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCPoint); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
     
    // Methods
  }
  public partial class CCPointSprite {
    // Constructors
    public CCPointSprite() { }
     
    // Properties
    public CocosSharp.CCColor4B Color { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCColor4B); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public CocosSharp.CCVertex2F Position { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCVertex2F); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public float Size { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
     
    // Methods
  }
  public enum CCPositionType {
    // Fields
    Free = 0,
    Grouped = 2,
    Relative = 1,
  }
  public partial class CCProgressFromTo : CocosSharp.CCActionInterval {
    // Constructors
    public CCProgressFromTo(float duration, float fromPercentage, float toPercentage) { }
     
    // Properties
    public float PercentFrom { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]protected set { } }
    public float PercentTo { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]protected set { } }
     
    // Methods
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCProgressFromToState : CocosSharp.CCActionIntervalState {
    // Constructors
    public CCProgressFromToState(CocosSharp.CCProgressFromTo action, CocosSharp.CCNode target) : base (default(CocosSharp.CCActionInterval), default(CocosSharp.CCNode)) { }
     
    // Properties
    protected float PercentFrom { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    protected float PercentTo { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
     
    // Methods
    public override void Update(float time) { }
  }
  public partial class CCProgressTimer : CocosSharp.CCNodeRGBA {
    // Constructors
    public CCProgressTimer(CocosSharp.CCSprite sp) { }
    public CCProgressTimer(string fileName) { }
     
    // Properties
    public CocosSharp.CCPoint BarChangeRate { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCPoint); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public override CocosSharp.CCColor3B Color { get { return default(CocosSharp.CCColor3B); } set { } }
    public override bool IsColorModifiedByOpacity { get { return default(bool); } set { } }
    public CocosSharp.CCPoint Midpoint { get { return default(CocosSharp.CCPoint); } set { } }
    public override byte Opacity { get { return default(byte); } set { } }
    public float Percentage { get { return default(float); } set { } }
    public bool ReverseDirection { get { return default(bool); } set { } }
    public CocosSharp.CCSprite Sprite { get { return default(CocosSharp.CCSprite); } set { } }
    public CocosSharp.CCProgressTimerType Type { get { return default(CocosSharp.CCProgressTimerType); } set { } }
     
    // Methods
    protected override void Draw() { }
  }
  public enum CCProgressTimerType {
    // Fields
    Bar = 1,
    Radial = 0,
  }
  public partial class CCProgressTo : CocosSharp.CCProgressFromTo {
    // Constructors
    public CCProgressTo(float duration, float percentTo) : base (default(float), default(float), default(float)) { }
     
    // Methods
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCProgressToState : CocosSharp.CCProgressFromToState {
    // Constructors
    public CCProgressToState(CocosSharp.CCProgressTo action, CocosSharp.CCNode target) : base (default(CocosSharp.CCProgressFromTo), default(CocosSharp.CCNode)) { }
  }
  [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
  public partial struct CCQuad3 {
    // Fields
    public CocosSharp.CCVertex3F BottomLeft;
    public CocosSharp.CCVertex3F BottomRight;
    public CocosSharp.CCVertex3F TopLeft;
    public CocosSharp.CCVertex3F TopRight;
     
    // Constructors
  }
  public partial class CCRandom {
    // Constructors
    public CCRandom() { }
     
    // Methods
    public static float Float_0_1() { return default(float); }
    public static float Float_Minus1_1() { return default(float); }
    public static float GetRandomFloat(float min, float max) { return default(float); }
    public static int GetRandomInt(int min, int max) { return default(int); }
    public static int Next() { return default(int); }
    public static int Next(int max) { return default(int); }
    public static int Next(int min, int max) { return default(int); }
    public static double NextDouble() { return default(double); }
    public static void Randomize(int seed) { }
  }
  public partial class CCRawList<T> : System.Collections.Generic.ICollection<T>, System.Collections.Generic.IEnumerable<T>, System.Collections.Generic.IList<T>, System.Collections.IEnumerable {
    // Fields
    public int count;
    public T[] Elements;
    public bool UseArrayPool;
     
    // Constructors
    public CCRawList(bool useArrayPool=false) { }
    public CCRawList(System.Collections.Generic.IList<T> elements, bool useArrayPool=false) { }
    public CCRawList(int initialCapacity, bool useArrayPool=false) { }
     
    // Properties
    public int Capacity { get { return default(int); } set { } }
    public int Count { get { return default(int); } set { } }
    public T this[int index] { get { return default(T); } set { } }
    bool System.Collections.Generic.ICollection<T>.IsReadOnly { get { return default(bool); } }
     
    // Methods
    public void Add(T item) { }
    public void Add(ref T item) { }
    public void AddRange(CocosSharp.CCRawList<T> items) { }
    public void AddRange(CocosSharp.CCRawList<T> items, int offset, int c) { }
    public void AddRange(System.Collections.Generic.IList<T> items) { }
    public void AddRange(System.Collections.Generic.List<T> items) { }
    public void Clear() { }
    public bool Contains(T item) { return default(bool); }
    public void CopyTo(T[] array, int arrayIndex) { }
    public void FastInsert(int index, T item) { }
    public bool FastRemove(T item) { return default(bool); }
    public void FastRemoveAt(int index) { }
    public T First() { return default(T); }
    public void Free() { }
    public CocosSharp.CCRawList<T>.Enumerator GetEnumerator() { return default(CocosSharp.CCRawList<T>.Enumerator); }
    public void IncreaseCount(int size) { }
    public int IndexOf(T item) { return default(int); }
    public void Insert(int index, T item) { }
    public void InsertRange(int index, CocosSharp.CCRawList<T> c) { }
    public T Last() { return default(T); }
    public void PackToCount() { }
    public T Peek() { return default(T); }
    public T Pop() { return default(T); }
    public void Push(T item) { }
    public bool Remove(T item) { return default(bool); }
    public void RemoveAt(int index) { }
    public void RemoveAt(int index, int amount) { }
    public virtual void RemoveRange(int index, int count) { }
    public void Reverse() { }
    public void Sort(System.Collections.Generic.IComparer<T> comparer) { }
    System.Collections.Generic.IEnumerator<T> System.Collections.Generic.IEnumerable<T>.GetEnumerator() { return default(System.Collections.Generic.IEnumerator<T>); }
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { return default(System.Collections.IEnumerator); }
    public T[] ToArray() { return default(T[]); }
     
    // Nested Types
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public partial struct Enumerator : System.Collections.Generic.IEnumerator<T>, System.Collections.IEnumerator, System.IDisposable {
      // Constructors
      public Enumerator(CocosSharp.CCRawList<T> list) { throw new System.NotImplementedException(); }
       
      // Properties
      public T Current { get { return default(T); } }
      object System.Collections.IEnumerator.Current { get { return default(object); } }
       
      // Methods
      public void Dispose() { }
      public bool MoveNext() { return default(bool); }
      public void Reset() { }
    }
  }
  [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
  public partial struct CCRect {
    // Fields
    public CocosSharp.CCPoint Origin;
    public CocosSharp.CCSize Size;
    public static readonly CocosSharp.CCRect Zero;
     
    // Constructors
    public CCRect(float x, float y, float width, float height) { throw new System.NotImplementedException(); }
     
    // Properties
    public CocosSharp.CCPoint Center { get { return default(CocosSharp.CCPoint); } }
    public CocosSharp.CCRect InvertedSize { get { return default(CocosSharp.CCRect); } }
    public CocosSharp.CCPoint LowerLeft { get { return default(CocosSharp.CCPoint); } }
    public float MaxX { get { return default(float); } }
    public float MaxY { get { return default(float); } }
    public float MidX { get { return default(float); } }
    public float MidY { get { return default(float); } }
    public float MinX { get { return default(float); } }
    public float MinY { get { return default(float); } }
    public CocosSharp.CCPoint UpperRight { get { return default(CocosSharp.CCPoint); } }
     
    // Methods
    public bool ContainsPoint(CocosSharp.CCPoint point) { return default(bool); }
    public static bool ContainsPoint(ref CocosSharp.CCRect rect, ref CocosSharp.CCPoint point) { return default(bool); }
    public bool ContainsPoint(float x, float y) { return default(bool); }
    public static bool Equal(ref CocosSharp.CCRect rect1, ref CocosSharp.CCRect rect2) { return default(bool); }
    public bool Equals(CocosSharp.CCRect rect) { return default(bool); }
    public override bool Equals(object obj) { return default(bool); }
    public override int GetHashCode() { return default(int); }
    public CocosSharp.CCRect Intersection(CocosSharp.CCRect rect) { return default(CocosSharp.CCRect); }
    public bool IntersectsRect(CocosSharp.CCRect rect) { return default(bool); }
    public bool IntersectsRect(ref CocosSharp.CCRect rect) { return default(bool); }
    public static bool IntersetsRect(ref CocosSharp.CCRect rectA, ref CocosSharp.CCRect rectB) { return default(bool); }
    public static bool operator ==(CocosSharp.CCRect p1, CocosSharp.CCRect p2) { return default(bool); }
    public static bool operator !=(CocosSharp.CCRect p1, CocosSharp.CCRect p2) { return default(bool); }
    public static CocosSharp.CCRect Parse(string s) { return default(CocosSharp.CCRect); }
    public override string ToString() { return default(string); }
  }

  public partial class CCRemoveSelf : CocosSharp.CCActionInstant {
    // Constructors
    public CCRemoveSelf() { }
    public CCRemoveSelf(bool isNeedCleanUp) { }
     
    // Properties
    public bool IsNeedCleanUp { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } }
     
    // Methods
    public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCRemoveSelfState : CocosSharp.CCActionInstantState {
    // Constructors
    public CCRemoveSelfState(CocosSharp.CCRemoveSelf action, CocosSharp.CCNode target) : base (default(CocosSharp.CCActionInstant), default(CocosSharp.CCNode)) { }
     
    // Properties
    protected bool IsNeedCleanUp { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
     
    // Methods
    public override void Update(float time) { }
  }
  public enum CCRenderTargetUsage {
    // Fields
    DiscardContents = 0,
    PlatformContents = 2,
    PreserveContents = 1,
  }
  public partial class CCRenderTexture : CocosSharp.CCNode {
    // Constructors
    public CCRenderTexture() { }
    public CCRenderTexture(int w, int h) { }
    public CCRenderTexture(int w, int h, CocosSharp.CCSurfaceFormat format) { }
    public CCRenderTexture(int w, int h, CocosSharp.CCSurfaceFormat colorFormat, CocosSharp.CCDepthFormat depthFormat, CocosSharp.CCRenderTargetUsage usage) { }
     
    // Properties
    protected CocosSharp.CCSurfaceFormat PixelFormat { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCSurfaceFormat); } }
    public CocosSharp.CCSprite Sprite { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCSprite); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    protected CocosSharp.CCTexture2D Texture { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCTexture2D); } }
     
    // Methods
    public virtual void Begin() { }
    public void BeginWithClear(float r, float g, float b, float a) { }
    public void BeginWithClear(float r, float g, float b, float a, float depthValue) { }
    public void BeginWithClear(float r, float g, float b, float a, float depthValue, int stencilValue) { }
    public void Clear(float r, float g, float b, float a) { }
    public void ClearDepth(float depthValue) { }
    public void ClearStencil(int stencilValue) { }
    public virtual void End() { }
    public bool SaveToStream(System.IO.Stream stream, CocosSharp.CCImageFormat format) { return default(bool); }
  }
  public partial class CCRepeat : CocosSharp.CCActionInterval {
    // Constructors
    public CCRepeat(CocosSharp.CCFiniteTimeAction action, uint times) { }
     
    // Properties
    public bool ActionInstant { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } }
    public CocosSharp.CCFiniteTimeAction InnerAction { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCFiniteTimeAction); } }
    public uint Times { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(uint); } }
    public uint Total { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(uint); } }
     
    // Methods
    public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCRepeatForever : CocosSharp.CCActionInterval {
    // Constructors
    public CCRepeatForever(CocosSharp.CCActionInterval action) { }
    public CCRepeatForever(params CocosSharp.CCFiniteTimeAction[] actions) { }
     
    // Properties
    public CocosSharp.CCActionInterval InnerAction { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCActionInterval); } }
     
    // Methods
    public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCRepeatForeverState : CocosSharp.CCActionIntervalState {
    // Constructors
    public CCRepeatForeverState(CocosSharp.CCRepeatForever action, CocosSharp.CCNode target) : base (default(CocosSharp.CCActionInterval), default(CocosSharp.CCNode)) { }
     
    // Properties
    public override bool IsDone { get { return default(bool); } }
     
    // Methods
    public override void Step(float dt) { }
  }
  public partial class CCRepeatState : CocosSharp.CCActionIntervalState {
    // Constructors
    public CCRepeatState(CocosSharp.CCRepeat action, CocosSharp.CCNode target) : base (default(CocosSharp.CCActionInterval), default(CocosSharp.CCNode)) { }
     
    // Properties
    protected bool ActionInstant { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    protected CocosSharp.CCFiniteTimeAction InnerAction { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCFiniteTimeAction); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    protected CocosSharp.CCFiniteTimeActionState InnerActionState { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCFiniteTimeActionState); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public override bool IsDone { get { return default(bool); } }
    protected float NextDt { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    protected uint Times { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(uint); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    protected uint Total { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(uint); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
     
    // Methods
    public override void Stop() { }
    public override void Update(float dt) { }
  }
  public enum CCResolutionPolicy {
    // Fields
    ExactFit = 1,
    FixedHeight = 4,
    FixedWidth = 5,
    NoBorder = 2,
    ShowAll = 3,
    UnKnown = 0,
  }
  public abstract partial class CCReusedObject<T> where T : CocosSharp.CCReusedObject<T>, new() {
    // Constructors
    protected CCReusedObject() { }
     
    // Methods
    public static T Create() { return default(T); }
    public void Free() { }
    protected abstract void PrepareForReuse();
  }
  public partial class CCReuseGrid : CocosSharp.CCActionInstant {
    // Constructors
    public CCReuseGrid() { }
    public CCReuseGrid(int times) { }
     
    // Properties
    public int Times { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } }
     
    // Methods
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCReuseGridState : CocosSharp.CCActionInstantState {
    // Constructors
    public CCReuseGridState(CocosSharp.CCReuseGrid action, CocosSharp.CCNode target) : base (default(CocosSharp.CCActionInstant), default(CocosSharp.CCNode)) { }
  }
  public partial class CCReverseTime : CocosSharp.CCActionInterval {
    // Constructors
    public CCReverseTime(CocosSharp.CCFiniteTimeAction action) { }
     
    // Properties
    public CocosSharp.CCFiniteTimeAction Other { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCFiniteTimeAction); } }
     
    // Methods
    public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCReverseTimeState : CocosSharp.CCActionIntervalState {
    // Constructors
    public CCReverseTimeState(CocosSharp.CCReverseTime action, CocosSharp.CCNode target) : base (default(CocosSharp.CCActionInterval), default(CocosSharp.CCNode)) { }
     
    // Properties
    protected CocosSharp.CCFiniteTimeAction Other { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCFiniteTimeAction); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    protected CocosSharp.CCFiniteTimeActionState OtherState { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCFiniteTimeActionState); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
     
    // Methods
    public override void Stop() { }
    public override void Update(float time) { }
  }
  public partial class CCRipple3D : CocosSharp.CCGrid3DAction {
    // Constructors
    public CCRipple3D(float duration, CocosSharp.CCGridSize gridSize) : base (default(float)) { }
    public CCRipple3D(float duration, CocosSharp.CCGridSize gridSize, CocosSharp.CCPoint position, float radius, int waves, float amplitude) : base (default(float)) { }
     
    // Properties
    public CocosSharp.CCPoint Position { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCPoint); } }
    public float Radius { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } }
    public int Waves { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } }
     
    // Methods
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCRipple3DState : CocosSharp.CCGrid3DActionState {
    // Constructors
    public CCRipple3DState(CocosSharp.CCRipple3D action, CocosSharp.CCNode target) : base (default(CocosSharp.CCGrid3DAction), default(CocosSharp.CCNode)) { }
     
    // Properties
    public CocosSharp.CCPoint Position { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCPoint); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public float Radius { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public int Waves { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
     
    // Methods
    public override void Update(float time) { }
  }
  public partial class CCRotateBy : CocosSharp.CCActionInterval {
    // Constructors
    public CCRotateBy(float duration, float fDeltaAngle) { }
    public CCRotateBy(float duration, float fDeltaAngleX, float fDeltaAngleY) { }
     
    // Properties
    public float AngleX { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } }
    public float AngleY { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } }
     
    // Methods
    public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCRotateByState : CocosSharp.CCActionIntervalState {
    // Constructors
    public CCRotateByState(CocosSharp.CCRotateBy action, CocosSharp.CCNode target) : base (default(CocosSharp.CCActionInterval), default(CocosSharp.CCNode)) { }
     
    // Properties
    protected float AngleX { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    protected float AngleY { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    protected float StartAngleX { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    protected float StartAngleY { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
     
    // Methods
    public override void Update(float time) { }
  }
  public partial class CCRotateTo : CocosSharp.CCActionInterval {
    // Constructors
    public CCRotateTo(float duration, float fDeltaAngle) { }
    public CCRotateTo(float duration, float fDeltaAngleX, float fDeltaAngleY) { }
     
    // Properties
    public float DistanceAngleX { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } }
    public float DistanceAngleY { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } }
     
    // Methods
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCRotateToState : CocosSharp.CCActionIntervalState {
    // Fields
    protected float DiffAngleX;
    protected float DiffAngleY;
    protected float StartAngleX;
    protected float StartAngleY;
     
    // Constructors
    public CCRotateToState(CocosSharp.CCRotateTo action, CocosSharp.CCNode target) : base (default(CocosSharp.CCActionInterval), default(CocosSharp.CCNode)) { }
     
    // Properties
    protected float DistanceAngleX { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    protected float DistanceAngleY { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
     
    // Methods
    public override void Update(float time) { }
  }
  public partial class CCSAXParser {
    // Constructors
    public CCSAXParser() { }
     
    // Methods
    public static void EndElement(object ctx, string name) { }
    public bool Init(string pszEncoding) { return default(bool); }
    public bool ParseContent(System.IO.TextReader sr) { return default(bool); }
    public bool ParseContent(string str) { return default(bool); }
    public bool ParseContentFile(string pszFile) { return default(bool); }
    public void SetDelegator(CocosSharp.ICCSAXDelegator pDelegator) { }
    public static void StartElement(object ctx, string name, System.String[] atts) { }
    public static void TextHandler(object ctx, System.Byte[] ch, int len) { }
  }
  public partial class CCScale9Sprite : CocosSharp.CCNodeRGBA {
    // Fields
    protected CocosSharp.CCSprite _bottom;
    protected CocosSharp.CCSprite _bottomLeft;
    protected CocosSharp.CCSprite _bottomRight;
    protected CocosSharp.CCRect _capInsetsInternal;
    protected CocosSharp.CCSprite _centre;
    protected CocosSharp.CCSprite _left;
    protected byte _opacity;
    protected bool _opacityModifyRGB;
    protected bool _positionsAreDirty;
    protected CocosSharp.CCSprite _right;
    protected CocosSharp.CCSpriteBatchNode _scale9Image;
    protected bool _spriteFrameRotated;
    protected CocosSharp.CCRect _spriteRect;
    protected bool _spritesGenerated;
    protected CocosSharp.CCSprite _top;
    protected CocosSharp.CCSprite _topLeft;
    protected CocosSharp.CCSprite _topRight;
     
    // Constructors
    public CCScale9Sprite() { }
    public CCScale9Sprite(CocosSharp.CCRect capInsets) { }
    public CCScale9Sprite(CocosSharp.CCSpriteBatchNode batchnode, CocosSharp.CCRect rect, CocosSharp.CCRect capInsets) { }
    public CCScale9Sprite(CocosSharp.CCSpriteBatchNode batchnode, CocosSharp.CCRect rect, bool rotated, CocosSharp.CCRect capInsets) { }
    public CCScale9Sprite(CocosSharp.CCSpriteFrame spriteFrame) { }
    public CCScale9Sprite(CocosSharp.CCSpriteFrame spriteFrame, CocosSharp.CCRect capInsets) { }
    public CCScale9Sprite(string file) { }
    public CCScale9Sprite(string file, CocosSharp.CCRect rect) { }
    public CCScale9Sprite(string file, CocosSharp.CCRect rect, CocosSharp.CCRect capInsets) { }
     
    // Properties
    public CocosSharp.CCRect CapInsets { get { return default(CocosSharp.CCRect); } set { } }
    public override CocosSharp.CCColor3B Color { get { return default(CocosSharp.CCColor3B); } set { } }
    public override CocosSharp.CCSize ContentSize { get { return default(CocosSharp.CCSize); } set { } }
    public float InsetBottom { get { return default(float); } set { } }
    public float InsetLeft { get { return default(float); } set { } }
    public float InsetRight { get { return default(float); } set { } }
    public float InsetTop { get { return default(float); } set { } }
    public override bool IsColorModifiedByOpacity { get { return default(bool); } set { } }
    public override byte Opacity { get { return default(byte); } set { } }
    public CocosSharp.CCSize PreferredSize { get { return default(CocosSharp.CCSize); } set { } }
     
    // Methods
    public void SetSpriteFrame(CocosSharp.CCSpriteFrame spriteFrame) { }
    public static CocosSharp.CCScale9Sprite SpriteWithFrameName(string spriteFrameName) { return default(CocosSharp.CCScale9Sprite); }
    public static CocosSharp.CCScale9Sprite SpriteWithFrameName(string spriteFrameName, CocosSharp.CCRect capInsets) { return default(CocosSharp.CCScale9Sprite); }
    protected void UpdateCapInset() { }
    public override void UpdateDisplayedColor(CocosSharp.CCColor3B parentColor) { }
    public override void UpdateDisplayedOpacity(byte parentOpacity) { }
    protected void UpdatePositions() { }
    public bool UpdateWithBatchNode(CocosSharp.CCSpriteBatchNode batchnode, CocosSharp.CCRect rect, bool rotated, CocosSharp.CCRect capInsets) { return default(bool); }
    public override void Visit() { }
  }
  public partial class CCScale9SpriteFile : CocosSharp.CCScale9Sprite {
    // Constructors
    public CCScale9SpriteFile(CocosSharp.CCRect capInsets, string file) { }
    public CCScale9SpriteFile(string file) { }
    public CCScale9SpriteFile(string file, CocosSharp.CCRect rect) { }
    public CCScale9SpriteFile(string file, CocosSharp.CCRect rect, CocosSharp.CCRect capInsets) { }
  }
  public partial class CCScale9SpriteFrame : CocosSharp.CCScale9Sprite {
    // Constructors
    public CCScale9SpriteFrame(CocosSharp.CCSpriteFrame spriteFrame) { }
    public CCScale9SpriteFrame(CocosSharp.CCSpriteFrame spriteFrame, CocosSharp.CCRect capInsets) { }
    public CCScale9SpriteFrame(string alias) { }
    public CCScale9SpriteFrame(string spriteFrameName, CocosSharp.CCRect capInsets) { }
  }
  public partial class CCScaleBy : CocosSharp.CCScaleTo {
    // Constructors
    public CCScaleBy(float duration, float s) : base (default(float), default(float)) { }
    public CCScaleBy(float duration, float sx, float sy) : base (default(float), default(float)) { }
     
    // Methods
    public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCScaleByState : CocosSharp.CCScaleToState {
    // Constructors
    public CCScaleByState(CocosSharp.CCScaleTo action, CocosSharp.CCNode target) : base (default(CocosSharp.CCScaleTo), default(CocosSharp.CCNode)) { }
  }
  public partial class CCScaleTo : CocosSharp.CCActionInterval {
    // Constructors
    public CCScaleTo(float duration, float s) { }
    public CCScaleTo(float duration, float sx, float sy) { }
     
    // Properties
    public float EndScaleX { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } }
    public float EndScaleY { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } }
     
    // Methods
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCScaleToState : CocosSharp.CCActionIntervalState {
    // Fields
    protected float DeltaX;
    protected float DeltaY;
    protected float EndScaleX;
    protected float EndScaleY;
    protected float StartScaleX;
    protected float StartScaleY;
     
    // Constructors
    public CCScaleToState(CocosSharp.CCScaleTo action, CocosSharp.CCNode target) : base (default(CocosSharp.CCActionInterval), default(CocosSharp.CCNode)) { }
     
    // Methods
    public override void Update(float time) { }
  }
  public partial class CCScene : CocosSharp.CCNode {
    // Constructors
    public CCScene() { }
     
    // Properties
    public virtual bool IsTransition { get { return default(bool); } }
     
    // Methods
  }
  public static partial class CCSchedulePriority {
    // Fields
    public const uint RepeatForever = (uint)4294967294;
    public const int System = -2147483648;
    public const int User = -2147483647;
  }
  public partial class CCScheduler {
    internal CCScheduler() { }
    // Properties
    public bool IsActionManagerActive { get { return default(bool); } }
    public float TimeScale { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
     
    // Methods
    public bool IsTargetPaused(CocosSharp.ICCUpdatable target) { return default(bool); }
    public System.Collections.Generic.List<CocosSharp.ICCUpdatable> PauseAllTargets() { return default(System.Collections.Generic.List<CocosSharp.ICCUpdatable>); }
    public System.Collections.Generic.List<CocosSharp.ICCUpdatable> PauseAllTargets(int minPriority) { return default(System.Collections.Generic.List<CocosSharp.ICCUpdatable>); }
    public void PauseTarget(CocosSharp.ICCUpdatable target) { }
    public void Resume(CocosSharp.ICCUpdatable target) { }
    public void Resume(System.Collections.Generic.List<CocosSharp.ICCUpdatable> targetsToResume) { }
    public void Schedule(CocosSharp.ICCUpdatable targt, int priority, bool paused) { }
    public void Schedule(System.Action<System.Single> selector, CocosSharp.ICCUpdatable target, float interval, uint repeat, float delay, bool paused) { }
    public void StartActionManager() { }
    public void Unschedule(CocosSharp.ICCUpdatable target) { }
    public void Unschedule(System.Action<System.Single> selector, CocosSharp.ICCUpdatable target) { }
    public void UnscheduleAll() { }
    public void UnscheduleAll(CocosSharp.ICCUpdatable target) { }
    public void UnscheduleAll(int minPriority) { }
  }
  public partial class CCScriptEngineManager {
    // Constructors
    public CCScriptEngineManager() { }
     
    // Properties
    public CocosSharp.ICCScriptingEngine ScriptEngine { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.ICCScriptingEngine); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public static CocosSharp.CCScriptEngineManager SharedScriptEngineManager { get { return default(CocosSharp.CCScriptEngineManager); } }
     
    // Methods
    public void RemoveScriptEngine() { }
  }
  public partial class CCScrollView : CocosSharp.CCLayer {
    // Fields
    protected bool _bounceable;
    protected bool _clippingToBounds;
    protected CocosSharp.CCNode _container;
    protected CocosSharp.CCPoint _contentOffset;
    protected CocosSharp.ICCScrollViewDelegate _delegate;
    protected CocosSharp.CCScrollViewDirection _direction;
    protected bool _dragging;
    protected CocosSharp.CCPoint _maxInset;
    protected float _maxScale;
    protected CocosSharp.CCPoint _minInset;
    protected float _minScale;
    protected CocosSharp.CCPoint _scrollDistance;
    protected System.Collections.Generic.List<CocosSharp.CCTouch> _touches;
    protected float _touchLength;
    protected bool _touchMoved;
    protected CocosSharp.CCPoint _touchPoint;
    protected CocosSharp.CCSize _viewSize;
     
    // Constructors
    public CCScrollView() { }
    public CCScrollView(CocosSharp.CCSize size) { }
    public CCScrollView(CocosSharp.CCSize size, CocosSharp.CCNode container) { }
     
    // Properties
    public bool Bounceable { get { return default(bool); } set { } }
    public bool ClippingToBounds { get { return default(bool); } set { } }
    public CocosSharp.CCNode Container { get { return default(CocosSharp.CCNode); } set { } }
    public override CocosSharp.CCSize ContentSize { get { return default(CocosSharp.CCSize); } set { } }
    public CocosSharp.ICCScrollViewDelegate Delegate { get { return default(CocosSharp.ICCScrollViewDelegate); } set { } }
    public CocosSharp.CCScrollViewDirection Direction { get { return default(CocosSharp.CCScrollViewDirection); } set { } }
    public bool IsDragging { get { return default(bool); } }
    public bool IsTouchMoved { get { return default(bool); } }
    public CocosSharp.CCPoint MaxContainerOffset { get { return default(CocosSharp.CCPoint); } }
    public float MaxScale { get { return default(float); } set { } }
    public CocosSharp.CCPoint MinContainerOffset { get { return default(CocosSharp.CCPoint); } }
    public float MinScale { get { return default(float); } set { } }
    public bool TouchEnabled { get { return default(bool); } set { } }
    public CocosSharp.CCSize ViewSize { get { return default(CocosSharp.CCSize); } set { } }
    public float ZoomScale { get { return default(float); } set { } }
     
    // Methods
    public override void AddChild(CocosSharp.CCNode child, int zOrder, int tag) { }
    public CocosSharp.CCPoint GetContentOffset() { return default(CocosSharp.CCPoint); }
    public bool IsNodeVisible(CocosSharp.CCNode node) { return default(bool); }
    public void Pause(object sender) { }
    public void Resume(object sender) { }
    public void SetContentOffset(CocosSharp.CCPoint offset) { }
    public void SetContentOffset(CocosSharp.CCPoint offset, bool animated) { }
    public void SetContentOffsetInDuration(CocosSharp.CCPoint offset, float dt) { }
    public void SetZoomScale(float value, bool animated) { }
    public void SetZoomScaleInDuration(float s, float dt) { }
    public virtual new bool TouchBegan(CocosSharp.CCTouch pTouch, CocosSharp.CCEvent touchEvent) { return default(bool); }
    public virtual new void TouchCancelled(CocosSharp.CCTouch touch, CocosSharp.CCEvent touchEvent) { }
    public virtual new void TouchEnded(CocosSharp.CCTouch touch, CocosSharp.CCEvent touchEvent) { }
    public virtual new void TouchMoved(CocosSharp.CCTouch touch, CocosSharp.CCEvent touchEvent) { }
    public void UpdateInset() { }
  }
  public enum CCScrollViewDirection {
    // Fields
    Both = 2,
    Horizontal = 0,
    None = -1,
    Vertical = 1,
  }
  public partial class CCScrollViewLoader : CocosSharp.CCNodeLoader {
    // Fields
    protected const string PROPERTY_BOUNCES = "bounces";
    protected const string PROPERTY_CLIPSTOBOUNDS = "clipsToBounds";
    protected const string PROPERTY_CONTAINER = "container";
    protected const string PROPERTY_DIRECTION = "direction";
     
    // Constructors
    public CCScrollViewLoader() { }
     
    // Methods
    public override CocosSharp.CCNode CreateCCNode() { return default(CocosSharp.CCNode); }
    protected override void OnHandlePropTypeCCBFile(CocosSharp.CCNode node, CocosSharp.CCNode parent, string propertyName, CocosSharp.CCNode fileNode, CocosSharp.CCBReader reader) { }
    protected override void OnHandlePropTypeCheck(CocosSharp.CCNode node, CocosSharp.CCNode parent, string propertyName, bool pCheck, CocosSharp.CCBReader reader) { }
    protected override void OnHandlePropTypeFloat(CocosSharp.CCNode node, CocosSharp.CCNode parent, string propertyName, float pFloat, CocosSharp.CCBReader reader) { }
    protected override void OnHandlePropTypeIntegerLabeled(CocosSharp.CCNode node, CocosSharp.CCNode parent, string propertyName, int pIntegerLabeled, CocosSharp.CCBReader reader) { }
    protected override void OnHandlePropTypeSize(CocosSharp.CCNode node, CocosSharp.CCNode parent, string propertyName, CocosSharp.CCSize pSize, CocosSharp.CCBReader reader) { }
  }
  public partial class CCSequence : CocosSharp.CCActionInterval {
    // Constructors
    public CCSequence(CocosSharp.CCFiniteTimeAction action1, CocosSharp.CCFiniteTimeAction action2) { }
    public CCSequence(params CocosSharp.CCFiniteTimeAction[] actions) { }
     
    // Properties
    public CocosSharp.CCFiniteTimeAction[] Actions { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCFiniteTimeAction[]); } }
     
    // Methods
    public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCSequenceState : CocosSharp.CCActionIntervalState {
    // Fields
    protected CocosSharp.CCFiniteTimeAction[] actionSequences;
    protected CocosSharp.CCFiniteTimeActionState[] actionStates;
    protected int last;
    protected float split;
     
    // Constructors
    public CCSequenceState(CocosSharp.CCSequence action, CocosSharp.CCNode target) : base (default(CocosSharp.CCActionInterval), default(CocosSharp.CCNode)) { }
     
    // Properties
    public override bool IsDone { get { return default(bool); } }
     
    // Methods
    public override void Step(float dt) { }
    public override void Stop() { }
    public override void Update(float t) { }
  }
  public static partial class CCSerialization {
  }
  public partial class CCShaky3D : CocosSharp.CCGrid3DAction {
    // Constructors
    public CCShaky3D(float duration, CocosSharp.CCGridSize gridSize, int range=0, bool shakeZ=true) : base (default(float)) { }
     
    // Properties
    protected internal int Range { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } }
    protected internal bool Shake { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } }
     
    // Methods
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCShaky3DState : CocosSharp.CCGrid3DActionState {
    // Constructors
    public CCShaky3DState(CocosSharp.CCShaky3D action, CocosSharp.CCNode target) : base (default(CocosSharp.CCGrid3DAction), default(CocosSharp.CCNode)) { }
     
    // Properties
    public int Range { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public bool Shake { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
     
    // Methods
    public override void Update(float time) { }
  }
  public partial class CCShakyTiles3D : CocosSharp.CCTiledGrid3DAction {
    // Constructors
    public CCShakyTiles3D(float duration, CocosSharp.CCGridSize gridSize, int nRange=0, bool bShakeZ=true) : base (default(float)) { }
     
    // Properties
    protected internal int Range { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } }
    protected internal bool ShakeZ { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } }
     
    // Methods
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCShakyTiles3DState : CocosSharp.CCTiledGrid3DActionState {
    // Constructors
    public CCShakyTiles3DState(CocosSharp.CCShakyTiles3D action, CocosSharp.CCNode target) : base (default(CocosSharp.CCTiledGrid3DAction), default(CocosSharp.CCNode)) { }
     
    // Properties
    public int Range { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public bool ShakeZ { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
     
    // Methods
    public override void Update(float time) { }
  }
  public partial class CCShatteredTiles3D : CocosSharp.CCTiledGrid3DAction {
    // Constructors
    public CCShatteredTiles3D(float duration, CocosSharp.CCGridSize gridSize, int nRange=0, bool bShatterZ=true) : base (default(float)) { }
     
    // Properties
    protected internal int Range { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } }
    protected internal bool ShatterZ { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } }
     
    // Methods
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCShatteredTiles3DState : CocosSharp.CCTiledGrid3DActionState {
    // Constructors
    public CCShatteredTiles3DState(CocosSharp.CCShatteredTiles3D action, CocosSharp.CCNode target) : base (default(CocosSharp.CCTiledGrid3DAction), default(CocosSharp.CCNode)) { }
     
    // Properties
    public int Range { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } }
    protected bool ShatterOnce { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } }
    public bool ShatterZ { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
     
    // Methods
    public override void Update(float time) { }
  }
  public partial class CCShow : CocosSharp.CCActionInstant {
    // Constructors
    public CCShow() { }
     
    // Methods
    public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCShowState : CocosSharp.CCActionInstantState {
    // Constructors
    public CCShowState(CocosSharp.CCShow action, CocosSharp.CCNode target) : base (default(CocosSharp.CCActionInstant), default(CocosSharp.CCNode)) { }
  }
  public partial class CCShuffleTiles : CocosSharp.CCTiledGrid3DAction {
    // Fields
    protected internal const int NoSeedSpecified = -1;
     
    // Constructors
    public CCShuffleTiles(CocosSharp.CCGridSize gridSize, float duration, int seed=-1) : base (default(float)) { }
     
    // Properties
    protected internal int Seed { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } }
     
    // Methods
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCShuffleTilesState : CocosSharp.CCTiledGrid3DActionState {
    // Constructors
    public CCShuffleTilesState(CocosSharp.CCShuffleTiles action, CocosSharp.CCNode target) : base (default(CocosSharp.CCTiledGrid3DAction), default(CocosSharp.CCNode)) { }
     
    // Properties
    protected CocosSharp.CCTile[] Tiles { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCTile[]); } }
    protected int TilesCount { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } }
    protected System.Int32[] TilesOrder { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Int32[]); } }
     
    // Methods
    protected CocosSharp.CCGridSize GetDelta(CocosSharp.CCGridSize pos) { return default(CocosSharp.CCGridSize); }
    protected CocosSharp.CCGridSize GetDelta(int x, int y) { return default(CocosSharp.CCGridSize); }
    protected void PlaceTile(CocosSharp.CCGridSize pos, CocosSharp.CCTile tile) { }
    protected void PlaceTile(int x, int y, CocosSharp.CCTile tile) { }
    public void Shuffle(ref System.Int32[] pArray, int nLen) { }
    public override void Update(float time) { }
  }
  [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
  public partial struct CCSize {
    // Fields
    public float Height;
    public float Width;
    public static readonly CocosSharp.CCSize Zero;
     
    // Constructors
    public CCSize(float width, float height) { throw new System.NotImplementedException(); }
     
    // Properties
    public CocosSharp.CCPoint Center { get { return default(CocosSharp.CCPoint); } }
    public CocosSharp.CCSize Inverted { get { return default(CocosSharp.CCSize); } }
     
    // Methods
    public static bool Equal(ref CocosSharp.CCSize size1, ref CocosSharp.CCSize size2) { return default(bool); }
    public bool Equals(CocosSharp.CCSize s) { return default(bool); }
    public override bool Equals(object obj) { return default(bool); }
    public override int GetHashCode() { return default(int); }
    public static CocosSharp.CCSize operator +(CocosSharp.CCSize p, float f) { return default(CocosSharp.CCSize); }
    public static CocosSharp.CCSize operator /(CocosSharp.CCSize p, float f) { return default(CocosSharp.CCSize); }
    public static bool operator ==(CocosSharp.CCSize p1, CocosSharp.CCSize p2) { return default(bool); }
    public static explicit operator CocosSharp.CCSize (CocosSharp.CCPoint point) { return default(CocosSharp.CCSize); }
    public static bool operator !=(CocosSharp.CCSize p1, CocosSharp.CCSize p2) { return default(bool); }
    public static CocosSharp.CCSize operator *(CocosSharp.CCSize p, float f) { return default(CocosSharp.CCSize); }
    public static CocosSharp.CCSize operator -(CocosSharp.CCSize p, float f) { return default(CocosSharp.CCSize); }
    public static CocosSharp.CCSize Parse(string s) { return default(CocosSharp.CCSize); }
    public override string ToString() { return default(string); }
  }

  [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Size=1)]
  public partial struct CCSizeI {
    // Constructors
    public CCSizeI(int width, int height) { throw new System.NotImplementedException(); }
     
    // Properties
    public int Height { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public int Width { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
     
    // Methods
    public static implicit operator CocosSharp.CCSize (CocosSharp.CCSizeI p) { return default(CocosSharp.CCSize); }
  }
  public partial class CCSkewBy : CocosSharp.CCSkewTo {
    // Constructors
    public CCSkewBy(float t, float deltaSkewXY) : base (default(float), default(float), default(float)) { }
    public CCSkewBy(float t, float deltaSkewX, float deltaSkewY) : base (default(float), default(float), default(float)) { }
     
    // Properties
    public float SkewByX { get { return default(float); } }
    public float SkewByY { get { return default(float); } }
     
    // Methods
    public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCSkewByState : CocosSharp.CCSkewToState {
    // Constructors
    public CCSkewByState(CocosSharp.CCSkewBy action, CocosSharp.CCNode target) : base (default(CocosSharp.CCSkewTo), default(CocosSharp.CCNode)) { }
  }
  public partial class CCSkewTo : CocosSharp.CCActionInterval {
    // Fields
    protected float EndSkewX;
    protected float EndSkewY;
    protected float SkewX;
    protected float SkewY;
     
    // Constructors
    public CCSkewTo(float t, float skewXY) { }
    public CCSkewTo(float t, float sx, float sy) { }
     
    // Properties
    public float SkewToX { get { return default(float); } }
    public float SkewToY { get { return default(float); } }
     
    // Methods
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCSkewToState : CocosSharp.CCActionIntervalState {
    // Fields
    protected float DeltaX;
    protected float DeltaY;
    protected float EndSkewX;
    protected float EndSkewY;
    protected float SkewX;
    protected float SkewY;
    protected float StartSkewX;
    protected float StartSkewY;
     
    // Constructors
    public CCSkewToState(CocosSharp.CCSkewTo action, CocosSharp.CCNode target) : base (default(CocosSharp.CCActionInterval), default(CocosSharp.CCNode)) { }
     
    // Methods
    public override void Update(float time) { }
  }
  public partial class CCSpawn : CocosSharp.CCActionInterval {
    // Constructors
    protected CCSpawn(CocosSharp.CCFiniteTimeAction action1, CocosSharp.CCFiniteTimeAction action2) { }
    public CCSpawn(params CocosSharp.CCFiniteTimeAction[] actions) { }
     
    // Properties
    public CocosSharp.CCFiniteTimeAction ActionOne { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCFiniteTimeAction); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]protected set { } }
    public CocosSharp.CCFiniteTimeAction ActionTwo { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCFiniteTimeAction); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]protected set { } }
     
    // Methods
    public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCSpawnState : CocosSharp.CCActionIntervalState {
    // Constructors
    public CCSpawnState(CocosSharp.CCSpawn action, CocosSharp.CCNode target) : base (default(CocosSharp.CCActionInterval), default(CocosSharp.CCNode)) { }
     
    // Properties
    protected CocosSharp.CCFiniteTimeAction ActionOne { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCFiniteTimeAction); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    protected CocosSharp.CCFiniteTimeAction ActionTwo { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCFiniteTimeAction); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
     
    // Methods
    public override void Stop() { }
    public override void Update(float time) { }
  }
  public partial class CCSpeed : CocosSharp.CCAction {
    // Constructors
    public CCSpeed(CocosSharp.CCActionInterval action, float fRate) { }
     
    // Properties
    protected internal CocosSharp.CCActionInterval InnerAction { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCActionInterval); } }
    public float Speed { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } }
     
    // Methods
    public virtual CocosSharp.CCActionInterval Reverse() { return default(CocosSharp.CCActionInterval); }
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCSpeedState : CocosSharp.CCActionState {
    // Constructors
    public CCSpeedState(CocosSharp.CCSpeed action, CocosSharp.CCNode target) : base (default(CocosSharp.CCAction), default(CocosSharp.CCNode)) { }
     
    // Properties
    protected CocosSharp.CCActionIntervalState InnerActionState { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCActionIntervalState); } }
    public override bool IsDone { get { return default(bool); } }
    public float Speed { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } }
     
    // Methods
    public override void Step(float dt) { }
    public override void Stop() { }
  }
  public partial class CCSplitCols : CocosSharp.CCTiledGrid3DAction {
    // Constructors
    public CCSplitCols(float duration, int nCols) : base (default(float)) { }
     
    // Properties
    protected internal int Columns { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } }
     
    // Methods
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCSplitColsState : CocosSharp.CCTiledGrid3DActionState {
    // Constructors
    public CCSplitColsState(CocosSharp.CCSplitCols action, CocosSharp.CCNode target) : base (default(CocosSharp.CCTiledGrid3DAction), default(CocosSharp.CCNode)) { }
     
    // Properties
    protected CocosSharp.CCSize WinSize { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCSize); } }
     
    // Methods
    public override void Update(float time) { }
  }
  public partial class CCSplitRows : CocosSharp.CCTiledGrid3DAction {
    // Constructors
    public CCSplitRows(float duration, int nRows) : base (default(float)) { }
     
    // Properties
    protected internal int Rows { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } }
     
    // Methods
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCSplitRowsState : CocosSharp.CCTiledGrid3DActionState {
    // Constructors
    public CCSplitRowsState(CocosSharp.CCSplitRows action, CocosSharp.CCNode target) : base (default(CocosSharp.CCTiledGrid3DAction), default(CocosSharp.CCNode)) { }
     
    // Properties
    protected CocosSharp.CCSize WinSize { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCSize); } }
     
    // Methods
    public override void Update(float time) { }
  }
  public partial class CCSprite : CocosSharp.CCNodeRGBA, CocosSharp.ICCBlendable, CocosSharp.ICCTexture {
    // Fields
    protected bool m_bDirty;
    protected bool m_bFlipX;
    protected bool m_bFlipY;
    protected bool m_bHasChildren;
    protected bool m_bOpacityModifyRGB;
    protected bool m_bRectRotated;
    protected bool m_bRecursiveDirty;
    protected bool m_bShouldBeHidden;
    protected CocosSharp.CCPoint m_obOffsetPosition;
    protected CocosSharp.CCRect m_obRect;
    protected CocosSharp.CCPoint m_obUnflippedOffsetPositionFromCenter;
    protected CocosSharp.CCSpriteBatchNode m_pobBatchNode;
    protected CocosSharp.CCTexture2D m_pobTexture;
    protected CocosSharp.CCTextureAtlas m_pobTextureAtlas;
    protected CocosSharp.CCBlendFunc m_sBlendFunc;
    protected CocosSharp.CCAffineTransform m_transformToBatch;
    protected int m_uAtlasIndex;
     
    // Constructors
    public CCSprite(CocosSharp.CCSize size) { }
    public CCSprite(CocosSharp.CCSpriteFrame spriteFrame) { }
    public CCSprite(CocosSharp.CCTexture2D texture) { }
		public CCSprite(CocosSharp.CCTexture2D texture=null, CocosSharp.CCRect rect=default(CCRect), bool rotated=false) { }
		public CCSprite(string fileName, CocosSharp.CCRect rect=default(CCRect)) { }
     
    // Properties
    public override CocosSharp.CCPoint AnchorPoint { get { return default(CocosSharp.CCPoint); } set { } }
    public int AtlasIndex { get { return default(int); } set { } }
    public CocosSharp.CCSpriteBatchNode BatchNode { get { return default(CocosSharp.CCSpriteBatchNode); } set { } }
    public CocosSharp.CCBlendFunc BlendFunc { get { return default(CocosSharp.CCBlendFunc); } set { } }
    public override CocosSharp.CCColor3B Color { get { return default(CocosSharp.CCColor3B); } set { } }
    public virtual bool Dirty { get { return default(bool); } set { } }
    public CocosSharp.CCSpriteFrame DisplayFrame { get { return default(CocosSharp.CCSpriteFrame); } set { } }
    public bool FlipX { get { return default(bool); } set { } }
    public bool FlipY { get { return default(bool); } set { } }
    public override bool IgnoreAnchorPointForPosition { get { return default(bool); } set { } }
    public bool IsAntialiased { get { return default(bool); } set { } }
    public override bool IsColorModifiedByOpacity { get { return default(bool); } set { } }
    public bool IsTextureRectRotated { get { return default(bool); } }
    public CocosSharp.CCPoint OffsetPosition { get { return default(CocosSharp.CCPoint); } }
    public override byte Opacity { get { return default(byte); } set { } }
    public override CocosSharp.CCPoint Position { get { return default(CocosSharp.CCPoint); } set { } }
    public override float Rotation { set { } }
    public override float RotationX { get { return default(float); } set { } }
    public override float RotationY { get { return default(float); } set { } }
    public override float Scale { set { } }
    public override float ScaleX { get { return default(float); } set { } }
    public override float ScaleY { get { return default(float); } set { } }
    public override float SkewX { get { return default(float); } set { } }
    public override float SkewY { get { return default(float); } set { } }
    public virtual CocosSharp.CCTexture2D Texture { get { return default(CocosSharp.CCTexture2D); } set { } }
    public CocosSharp.CCRect TextureRect { get { return default(CocosSharp.CCRect); } set { } }
    public override float VertexZ { get { return default(float); } set { } }
    public override bool Visible { get { return default(bool); } set { } }
     
    // Methods
    public override void AddChild(CocosSharp.CCNode child, int zOrder, int tag) { }
    public override void Deserialize(System.IO.Stream stream) { }
    protected override void Draw() { }
    public bool IsFrameDisplayed(CocosSharp.CCSpriteFrame pFrame) { return default(bool); }
    public override void RemoveAllChildrenWithCleanup(bool cleanup) { }
    public override void RemoveChild(CocosSharp.CCNode child, bool cleanup) { }
    public override void ReorderChild(CocosSharp.CCNode child, int zOrder) { }
    public virtual void ScaleTo(CocosSharp.CCSize size) { }
    public override void Serialize(System.IO.Stream stream) { }
    public virtual void SetDirtyRecursively(bool bValue) { }
    public void SetDisplayFrameWithAnimationName(string animationName, int frameIndex) { }
    public virtual void SetReorderChildDirtyRecursively() { }
    public void SetTextureRect(CocosSharp.CCRect rect) { }
    public void SetTextureRect(CocosSharp.CCRect value, bool rotated, CocosSharp.CCSize untrimmedSize) { }
    protected virtual void SetVertexRect(CocosSharp.CCRect rect) { }
    public override void SortAllChildren() { }
    protected void UpdateBlendFunc() { }
    public override void UpdateDisplayedColor(CocosSharp.CCColor3B parentColor) { }
    public override void UpdateDisplayedOpacity(byte parentOpacity) { }
    public override void UpdateTransform() { }
  }
  public partial class CCSpriteBatchNode : CocosSharp.CCNode, CocosSharp.ICCBlendable, CocosSharp.ICCTexture {
    // Fields
    protected CocosSharp.CCBlendFunc m_blendFunc;
    protected CocosSharp.CCRawList<CocosSharp.CCSprite> m_pobDescendants;
    protected CocosSharp.CCTextureAtlas m_pobTextureAtlas;
     
    // Constructors
    public CCSpriteBatchNode() { }
    public CCSpriteBatchNode(CocosSharp.CCTexture2D tex, int capacity=29) { }
    public CCSpriteBatchNode(string fileImage, int capacity=29) { }
     
    // Properties
    public CocosSharp.CCBlendFunc BlendFunc { get { return default(CocosSharp.CCBlendFunc); } set { } }
    public CocosSharp.CCRawList<CocosSharp.CCSprite> Descendants { get { return default(CocosSharp.CCRawList<CocosSharp.CCSprite>); } }
    public bool IsAntialiased { get { return default(bool); } set { } }
    public virtual CocosSharp.CCTexture2D Texture { get { return default(CocosSharp.CCTexture2D); } set { } }
    public CocosSharp.CCTextureAtlas TextureAtlas { get { return default(CocosSharp.CCTextureAtlas); } set { } }
     
    // Methods
    public override void AddChild(CocosSharp.CCNode child, int zOrder, int tag) { }
    protected CocosSharp.CCSpriteBatchNode AddSpriteWithoutQuad(CocosSharp.CCSprite child, int z, int aTag) { return default(CocosSharp.CCSpriteBatchNode); }
    public void AppendChild(CocosSharp.CCSprite sprite) { }
    public int AtlasIndexForChild(CocosSharp.CCSprite pobSprite, int nZ) { return default(int); }
    protected override void Draw() { }
    public int HighestAtlasIndexInChild(CocosSharp.CCSprite pSprite) { return default(int); }
    public void IncreaseAtlasCapacity() { }
    protected void InitCCSpriteBatchNode(CocosSharp.CCTexture2D tex, int capacity=29) { }
    public void InsertChild(CocosSharp.CCSprite pobSprite, int uIndex) { }
    protected void InsertQuadFromSprite(CocosSharp.CCSprite sprite, int index) { }
    public int LowestAtlasIndexInChild(CocosSharp.CCSprite pSprite) { return default(int); }
    public int RebuildIndexInOrder(CocosSharp.CCSprite pobParent, int uIndex) { return default(int); }
    public override void RemoveAllChildrenWithCleanup(bool cleanup) { }
    public override void RemoveChild(CocosSharp.CCNode child, bool cleanup) { }
    public void RemoveChildAtIndex(int index, bool doCleanup) { }
    public void RemoveSpriteFromAtlas(CocosSharp.CCSprite pobSprite) { }
    public void ReorderBatch(bool reorder) { }
    public override void ReorderChild(CocosSharp.CCNode child, int zOrder) { }
    public override void SortAllChildren() { }
    protected void UpdateQuadFromSprite(CocosSharp.CCSprite sprite, int index) { }
    public override void Visit() { }
  }
  public partial class CCSpriteFontCache {
    internal CCSpriteFontCache() { }
    // Fields
    public static string FontRoot;
     
    // Properties
    public static float FontScale { get { return default(float); } set { } }
    public static CocosSharp.CCSpriteFontCache SharedInstance { get { return default(CocosSharp.CCSpriteFontCache); } }
     
    // Methods
    public void Clear() { }
    public static void RegisterFont(string fontName, params System.Int32[] sizes) { }
  }
  public partial class CCSpriteFrame {
    // Fields
    protected bool m_bRotated;
    protected CocosSharp.CCPoint m_obOffset;
    protected CocosSharp.CCPoint m_obOffsetInPixels;
    protected CocosSharp.CCSize m_obOriginalSize;
    protected CocosSharp.CCSize m_obOriginalSizeInPixels;
    protected CocosSharp.CCRect m_obRect;
    protected CocosSharp.CCRect m_obRectInPixels;
    protected CocosSharp.CCTexture2D m_pobTexture;
    protected string m_strTextureFilename;
     
    // Constructors
    public CCSpriteFrame() { }
    protected CCSpriteFrame(CocosSharp.CCSpriteFrame spriteFrame) { }
    public CCSpriteFrame(CocosSharp.CCTexture2D pobTexture, CocosSharp.CCRect rect) { }
    public CCSpriteFrame(CocosSharp.CCTexture2D pobTexture, CocosSharp.CCRect rect, CocosSharp.CCSize originalSize) { }
    public CCSpriteFrame(CocosSharp.CCTexture2D pobTexture, CocosSharp.CCRect rect, bool rotated, CocosSharp.CCPoint offset, CocosSharp.CCSize originalSize) { }
     
    // Properties
    public bool IsRotated { get { return default(bool); } set { } }
    public CocosSharp.CCPoint Offset { get { return default(CocosSharp.CCPoint); } set { } }
    public CocosSharp.CCPoint OffsetInPixels { get { return default(CocosSharp.CCPoint); } set { } }
    public CocosSharp.CCSize OriginalSize { get { return default(CocosSharp.CCSize); } set { } }
    public CocosSharp.CCSize OriginalSizeInPixels { get { return default(CocosSharp.CCSize); } set { } }
    public CocosSharp.CCRect Rect { get { return default(CocosSharp.CCRect); } set { } }
    public CocosSharp.CCRect RectInPixels { get { return default(CocosSharp.CCRect); } set { } }
    public CocosSharp.CCTexture2D Texture { get { return default(CocosSharp.CCTexture2D); } set { } }
    public string TextureFilename { get { return default(string); } set { } }
     
    // Methods
    public CocosSharp.CCSpriteFrame Copy() { return default(CocosSharp.CCSpriteFrame); }
  }
  public partial class CCSpriteFrameCache {
    // Fields
    protected System.Collections.Generic.Dictionary<System.String, CocosSharp.CCSpriteFrame> m_pSpriteFrames;
    protected System.Collections.Generic.Dictionary<System.String, System.String> m_pSpriteFramesAliases;
    public static CocosSharp.CCSpriteFrameCache pSharedSpriteFrameCache;
     
    // Constructors
    protected CCSpriteFrameCache() { }
     
    // Properties
    public bool AllowFrameOverrite { get { return default(bool); } set { } }
    public static CocosSharp.CCSpriteFrameCache SharedSpriteFrameCache { get { return default(CocosSharp.CCSpriteFrameCache); } }
     
    // Methods
    public void AddSpriteFrame(CocosSharp.CCSpriteFrame pobFrame, string pszFrameName) { }
    public void AddSpriteFramesWithDictionary(CocosSharp.PlistDictionary pobDictionary, CocosSharp.CCTexture2D pobTexture) { }
    public void AddSpriteFramesWithFile(System.IO.Stream plist, CocosSharp.CCTexture2D pobTexture) { }
    public void AddSpriteFramesWithFile(string pszPlist) { }
    public void AddSpriteFramesWithFile(string pszPlist, CocosSharp.CCTexture2D pobTexture) { }
    public void AddSpriteFramesWithFile(string plist, string textureFileName) { }
    public static void PurgeSharedSpriteFrameCache() { }
    public void RemoveSpriteFrameByName(string pszName) { }
    public void RemoveSpriteFrames() { }
    public void RemoveSpriteFramesFromDictionary(CocosSharp.PlistDictionary dictionary) { }
    public void RemoveSpriteFramesFromFile(string plist) { }
    public void RemoveSpriteFramesFromTexture(CocosSharp.CCTexture2D texture) { }
    public void RemoveUnusedSpriteFrames() { }
    public CocosSharp.CCSpriteFrame SpriteFrameByName(string pszName) { return default(CocosSharp.CCSpriteFrame); }
  }
  public partial class CCSpriteSheet {
    // Constructors
    public CCSpriteSheet(CocosSharp.PlistDictionary dictionary, CocosSharp.CCTexture2D texture) { }
    public CCSpriteSheet(System.Collections.Generic.Dictionary<System.String, CocosSharp.CCSpriteFrame> frames) { }
    public CCSpriteSheet(System.IO.Stream stream, CocosSharp.CCTexture2D texture) { }
    public CCSpriteSheet(System.IO.Stream stream, string textureFileName) { }
    public CCSpriteSheet(string fileName) { }
    public CCSpriteSheet(string fileName, CocosSharp.CCTexture2D texture) { }
    public CCSpriteSheet(string fileName, string textureFileName) { }
     
    // Properties
    public System.Collections.Generic.List<CocosSharp.CCSpriteFrame> Frames { get { return default(System.Collections.Generic.List<CocosSharp.CCSpriteFrame>); } }
    public CocosSharp.CCSpriteFrame this[string name] { get { return default(CocosSharp.CCSpriteFrame); } }
     
    // Methods
    public CocosSharp.CCSpriteFrame SpriteFrameByName(string name) { return default(CocosSharp.CCSpriteFrame); }
  }
  public partial class CCSpriteSheetCache {
    // Constructors
    public CCSpriteSheetCache() { }
     
    // Properties
    public static CocosSharp.CCSpriteSheetCache Instance { get { return default(CocosSharp.CCSpriteSheetCache); } }
     
    // Methods
    public CocosSharp.CCSpriteSheet AddSpriteSheet(CocosSharp.PlistDictionary dictionary, CocosSharp.CCTexture2D texture, string name) { return default(CocosSharp.CCSpriteSheet); }
    public CocosSharp.CCSpriteSheet AddSpriteSheet(System.IO.Stream stream, CocosSharp.CCTexture2D texture, string name) { return default(CocosSharp.CCSpriteSheet); }
    public CocosSharp.CCSpriteSheet AddSpriteSheet(string fileName) { return default(CocosSharp.CCSpriteSheet); }
    public CocosSharp.CCSpriteSheet AddSpriteSheet(string fileName, CocosSharp.CCTexture2D texture) { return default(CocosSharp.CCSpriteSheet); }
    public CocosSharp.CCSpriteSheet AddSpriteSheet(string fileName, string textureFileName) { return default(CocosSharp.CCSpriteSheet); }
    public static void DestroyInstance() { }
    public void Remove(CocosSharp.CCSpriteSheet spriteSheet) { }
    public void Remove(string name) { }
    public void RemoveAll() { }
    public void RemoveUnused() { }
    public CocosSharp.CCSpriteSheet SpriteSheetForKey(string name) { return default(CocosSharp.CCSpriteSheet); }
  }
  public partial class CCStats {
    // Constructors
    public CCStats() { }
     
    // Properties
    public bool IsEnabled { get { return default(bool); } set { } }
    public bool IsInitialized { get { return default(bool); } }
     
    // Methods
    public void Draw() { }
    public void Initialize() { }
    public void UpdateEnd(float delta) { }
    public void UpdateStart() { }
  }
  public partial class CCStopGrid : CocosSharp.CCActionInstant {
    // Constructors
    public CCStopGrid() { }
     
    // Methods
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCStopGridState : CocosSharp.CCActionInstantState {
    // Constructors
    public CCStopGridState(CocosSharp.CCStopGrid action, CocosSharp.CCNode target) : base (default(CocosSharp.CCActionInstant), default(CocosSharp.CCNode)) { }
  }
  public enum CCSurfaceFormat {
    // Fields
    Alpha8 = 12,
    Bgr32 = 20,
    Bgr565 = 1,
    Bgra32 = 21,
    Bgra4444 = 3,
    Bgra5551 = 2,
    CCVector2 = 14,
    Color = 0,
    Dxt1 = 4,
    Dxt1a = 70,
    Dxt3 = 5,
    Dxt5 = 6,
    HalfCCVector2 = 17,
    HalfSingle = 16,
    HalfVector4 = 18,
    HdrBlendable = 19,
    NormalizedByte2 = 7,
    NormalizedByte4 = 8,
    Rg32 = 10,
    Rgba1010102 = 9,
    Rgba64 = 11,
    RgbaPvrtc2Bpp = 52,
    RgbaPvrtc4Bpp = 53,
    RgbEtc1 = 60,
    RgbPvrtc2Bpp = 50,
    RgbPvrtc4Bpp = 51,
    Single = 13,
    Vector4 = 15,
  }
  public delegate void CCSwitchValueChangedDelegate(object sender, bool bState);
  public partial class CCTableView : CocosSharp.CCScrollView, CocosSharp.ICCScrollViewDelegate {
    // Fields
    protected CocosSharp.CCArrayForObjectSorting _cellsFreed;
    protected System.Collections.Generic.List<System.Single> _cellsPositions;
    protected CocosSharp.CCArrayForObjectSorting _cellsUsed;
    protected CocosSharp.ICCTableViewDataSource _dataSource;
    protected System.Collections.Generic.List<System.Int32> _indices;
    protected CocosSharp.CCScrollViewDirection _oldDirection;
    protected CocosSharp.ICCTableViewDelegate _tableViewDelegate;
    protected CocosSharp.CCTableViewCell _touchedCell;
    protected CocosSharp.CCTableViewVerticalFillOrder _vordering;
     
    // Constructors
    public CCTableView() { }
    public CCTableView(CocosSharp.ICCTableViewDataSource dataSource, CocosSharp.CCSize size) { }
    public CCTableView(CocosSharp.ICCTableViewDataSource dataSource, CocosSharp.CCSize size, CocosSharp.CCNode container) { }
     
    // Properties
    public CocosSharp.ICCTableViewDataSource DataSource { get { return default(CocosSharp.ICCTableViewDataSource); } set { } }
    public new CocosSharp.ICCTableViewDelegate Delegate { get { return default(CocosSharp.ICCTableViewDelegate); } set { } }
    public CocosSharp.CCTableViewVerticalFillOrder VerticalFillOrder { get { return default(CocosSharp.CCTableViewVerticalFillOrder); } set { } }
     
    // Methods
    protected int __indexFromOffset(CocosSharp.CCPoint offset) { return default(int); }
    protected CocosSharp.CCPoint __offsetFromIndex(int index) { return default(CocosSharp.CCPoint); }
    protected void _addCellIfNecessary(CocosSharp.CCTableViewCell cell) { }
    protected int _indexFromOffset(CocosSharp.CCPoint offset) { return default(int); }
    protected void _moveCellOutOfSight(CocosSharp.CCTableViewCell cell) { }
    protected CocosSharp.CCPoint _offsetFromIndex(int index) { return default(CocosSharp.CCPoint); }
    protected void _setIndexForCell(int index, CocosSharp.CCTableViewCell cell) { }
    protected void _updateCellPositions() { }
    protected void _updateContentSize() { }
    public CocosSharp.CCTableViewCell CellAtIndex(int idx) { return default(CocosSharp.CCTableViewCell); }
    public CocosSharp.CCTableViewCell DequeueCell() { return default(CocosSharp.CCTableViewCell); }
    public void ReloadData() { }
    public void RemoveCellAtIndex(int idx) { }
    public virtual void ScrollViewDidScroll(CocosSharp.CCScrollView view) { }
    public virtual void ScrollViewDidZoom(CocosSharp.CCScrollView view) { }
    public void TouchEnded(CocosSharp.CCTouch pTouch) { }
    public void UpdateCellAtIndex(int idx) { }
  }
  public partial class CCTableViewCell : CocosSharp.CCNode, CocosSharp.ICCSortableObject {
    // Constructors
    public CCTableViewCell() { }
     
    // Properties
    public int Index { get { return default(int); } set { } }
    public int ObjectID { get { return default(int); } set { } }
     
    // Methods
    public void Reset() { }
  }
  public enum CCTableViewVerticalFillOrder {
    // Fields
    FillBottomUp = 1,
    FillTopDown = 0,
  }
  public partial class CCTargetedAction : CocosSharp.CCActionInterval {
    // Constructors
    public CCTargetedAction(CocosSharp.CCNode target, CocosSharp.CCFiniteTimeAction pAction) { }
     
    // Properties
    public CocosSharp.CCNode ForcedTarget { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCNode); } }
    public CocosSharp.CCFiniteTimeAction TargetedAction { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCFiniteTimeAction); } }
     
    // Methods
    public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCTargetedActionState : CocosSharp.CCActionIntervalState {
    // Constructors
    public CCTargetedActionState(CocosSharp.CCTargetedAction action, CocosSharp.CCNode target) : base (default(CocosSharp.CCActionInterval), default(CocosSharp.CCNode)) { }
     
    // Properties
    protected CocosSharp.CCFiniteTimeActionState ActionState { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCFiniteTimeActionState); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    protected CocosSharp.CCNode ForcedTarget { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCNode); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    protected CocosSharp.CCFiniteTimeAction TargetedAction { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCFiniteTimeAction); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
     
    // Methods
    public override void Stop() { }
    public override void Update(float time) { }
  }
  public static partial class CCTask {
    // Methods
    public static object RunAsync(System.Action action) { return default(object); }
    public static object RunAsync(System.Action action, System.Action<System.Object> taskCompleted) { return default(object); }
  }
  [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Size=1)]
  public partial struct CCTex2F {
    // Constructors
    public CCTex2F(float inu, float inv) { throw new System.NotImplementedException(); }
     
    // Properties
    public float U { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public float V { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
     
    // Methods
    public override string ToString() { return default(string); }
  }
  public enum CCTextAlignment {
    // Fields
    Center = 1,
    Left = 0,
    Right = 2,
  }
  public partial class CCTextFieldTTF : CocosSharp.CCLabelTtf {
    // Constructors
    public CCTextFieldTTF(string text, string fontName, float fontSize) { }
    public CCTextFieldTTF(string text, string fontName, float fontSize, CocosSharp.CCSize dimensions, CocosSharp.CCTextAlignment hAlignment) { }
    public CCTextFieldTTF(string text, string fontName, float fontSize, CocosSharp.CCSize dimensions, CocosSharp.CCTextAlignment hAlignment, CocosSharp.CCVerticalTextAlignment vAlignment) { }
     
    // Properties
    public bool AutoEdit { get { return default(bool); } set { } }
    public string EditDescription { get { return default(string); } set { } }
    public string EditTitle { get { return default(string); } set { } }
    public bool ReadOnly { get { return default(bool); } set { } }
     
    // Events
    public event CocosSharp.CCTextFieldTTFDelegate BeginEditing { add { } remove { } }
    public event CocosSharp.CCTextFieldTTFDelegate EndEditing { add { } remove { } }
     
    // Methods
    protected virtual void DoBeginEditing(ref string newText, ref bool canceled) { }
    protected virtual void DoEndEditing(ref string newText, ref bool canceled) { }
    public void Edit() { }
    public void Edit(string title, string defaultText) { }
    public void EndEdit() { }
    public override void OnEnter() { }
    public override void OnExit() { }
    public bool TouchBegan(CocosSharp.CCTouch pTouch) { return default(bool); }
    public void TouchCancelled(CocosSharp.CCTouch pTouch) { }
    public void TouchEnded(CocosSharp.CCTouch pTouch) { }
    public void TouchMoved(CocosSharp.CCTouch pTouch) { }
  }
  public delegate void CCTextFieldTTFDelegate(object sender, ref string text, ref bool canceled);
  public partial class CCTexture2D : CocosSharp.CCGraphicsResource {
    // Fields
    public static CocosSharp.CCSurfaceFormat DefaultAlphaPixelFormat;
    public static bool DefaultIsAntialiased;
    public static bool OptimizeForPremultipliedAlpha;
     
    // Constructors
    public CCTexture2D() { }
    public CCTexture2D(System.Byte[] data) { }
    public CCTexture2D(System.Byte[] data, CocosSharp.CCSurfaceFormat pixelFormat) { }
    public CCTexture2D(System.Byte[] data, CocosSharp.CCSurfaceFormat pixelFormat, bool mipMap) { }
    public CCTexture2D(System.Byte[] data, bool mipMap) { }
    public CCTexture2D(int pixelsWide, int pixelsHigh) { }
    public CCTexture2D(int pixelsWide, int pixelsHigh, CocosSharp.CCSurfaceFormat pixelFormat) { }
    public CCTexture2D(int pixelsWide, int pixelsHigh, CocosSharp.CCSurfaceFormat pixelFormat, bool premultipliedAlpha, bool mipMap) { }
    public CCTexture2D(System.IO.Stream stream) { }
    public CCTexture2D(System.IO.Stream stream, CocosSharp.CCSurfaceFormat pixelFormat) { }
    public CCTexture2D(string file) { }
    public CCTexture2D(string text, CocosSharp.CCSize dimensions, CocosSharp.CCTextAlignment hAlignment, CocosSharp.CCVerticalTextAlignment vAlignment, string fontName, float fontSize) { }
    public CCTexture2D(string text, string fontName, float fontSize) { }
     
    // Properties
    public uint BitsPerPixelForFormat { get { return default(uint); } }
    public CocosSharp.CCSize ContentSize { get { return default(CocosSharp.CCSize); } }
    public CocosSharp.CCSize ContentSizeInPixels { get { return default(CocosSharp.CCSize); } set { } }
    public bool HasPremultipliedAlpha { get { return default(bool); } set { } }
    public bool IsAntialiased { get { return default(bool); } set { } }
    public bool IsTextureDefined { get { return default(bool); } }
    public CocosSharp.CCSurfaceFormat PixelFormat { get { return default(CocosSharp.CCSurfaceFormat); } set { } }
    public int PixelsHigh { get { return default(int); } set { } }
    public int PixelsWide { get { return default(int); } set { } }
     
    // Methods
    public static CocosSharp.CCImageFormat DetectImageFormat(System.IO.Stream stream) { return default(CocosSharp.CCImageFormat); }
    protected override void Dispose(bool disposing) { }
    public void GenerateMipmap() { }
    public override void Reinit() { }
    public void SaveAsJpeg(System.IO.Stream stream, int width, int height) { }
    public void SaveAsPng(System.IO.Stream stream, int width, int height) { }
    [System.ObsoleteAttribute("Use IsAntialiased property.")]
    public void SetAliasTexParameters() { }
    [System.ObsoleteAttribute("Use IsAntialiased property.")]
    public void SetAntiAliasTexParameters() { }
    public override string ToString() { return default(string); }
  }
  public partial class CCTextureAtlas {
    // Fields
    protected CocosSharp.CCTexture2D texture;
     
    // Constructors
    public CCTextureAtlas() { }
    public CCTextureAtlas(CocosSharp.CCTexture2D texture, int capacity) { }
    public CCTextureAtlas(string file, int capacity) { }
     
    // Properties
    public int Capacity { get { return default(int); } set { } }
    public bool IsAntialiased { get { return default(bool); } set { } }
    public CocosSharp.CCTexture2D Texture { get { return default(CocosSharp.CCTexture2D); } set { } }
    public int TotalQuads { get { return default(int); } }
     
    // Methods
    public void DrawNumberOfQuads(int n) { }
    public void DrawNumberOfQuads(int n, int start) { }
    public void DrawQuads() { }
    public void FillWithEmptyQuadsFromIndex(int index, int amount) { }
    public void IncreaseTotalQuadsWith(int amount) { }
    public void InsertQuadFromIndex(int oldIndex, int newIndex) { }
    public void MoveQuadsFromIndex(int index, int newIndex) { }
    public void MoveQuadsFromIndex(int oldIndex, int amount, int newIndex) { }
    public void RemoveAllQuads() { }
    public void RemoveQuadAtIndex(int index) { }
    public void RemoveQuadsAtIndex(int index, int amount) { }
    public bool ResizeCapacity(int newCapacity) { return default(bool); }
    public override string ToString() { return default(string); }
  }
  public partial class CCTextureCache : CocosSharp.ICCUpdatable, System.IDisposable {
    internal CCTextureCache() { }
    // Fields
    protected System.Collections.Generic.Dictionary<System.String, CocosSharp.CCTexture2D> m_pTextures;
     
    // Properties
    public CocosSharp.CCTexture2D this[string key] { get { return default(CocosSharp.CCTexture2D); } }
    public static CocosSharp.CCTextureCache SharedTextureCache { get { return default(CocosSharp.CCTextureCache); } }
     
    // Methods
    public CocosSharp.CCTexture2D AddImage(System.Byte[] data, string assetName, CocosSharp.CCSurfaceFormat format) { return default(CocosSharp.CCTexture2D); }
    public CocosSharp.CCTexture2D AddImage(string fileimage) { return default(CocosSharp.CCTexture2D); }
    public void AddImageAsync(System.Byte[] data, string assetName, CocosSharp.CCSurfaceFormat format, System.Action<CocosSharp.CCTexture2D> action) { }
    public void AddImageAsync(string fileimage, System.Action<CocosSharp.CCTexture2D> action) { }
    public CocosSharp.CCTexture2D AddRawImage<T>(T[] data, int width, int height, string assetName, CocosSharp.CCSurfaceFormat format, bool premultiplied) where T : struct { return default(CocosSharp.CCTexture2D); }
    public CocosSharp.CCTexture2D AddRawImage<T>(T[] data, int width, int height, string assetName, CocosSharp.CCSurfaceFormat format, bool premultiplied, bool mipMap) where T : struct { return default(CocosSharp.CCTexture2D); }
    public CocosSharp.CCTexture2D AddRawImage<T>(T[] data, int width, int height, string assetName, CocosSharp.CCSurfaceFormat format, bool premultiplied, bool mipMap, CocosSharp.CCSize contentSize) where T : struct { return default(CocosSharp.CCTexture2D); }
    public bool Contains(string assetFile) { return default(bool); }
    public void Dispose() { }
    protected virtual void Dispose(bool disposing) { }
    public void DumpCachedTextureInfo() { }
    public static void PurgeSharedTextureCache() { }
    public void RemoveAllTextures() { }
    public void RemoveTexture(CocosSharp.CCTexture2D texture) { }
    public void RemoveTextureForKey(string textureKeyName) { }
    public void RemoveUnusedTextures() { }
    public CocosSharp.CCTexture2D TextureForKey(string key) { return default(CocosSharp.CCTexture2D); }
    public void UnloadContent() { }
    public void Update(float dt) { }
  }
  public partial class CCTile {
    // Fields
    public CocosSharp.CCGridSize Delta;
    public CocosSharp.CCPoint Position;
    public CocosSharp.CCPoint StartPosition;
     
    // Constructors
    public CCTile() { }
  }
  public partial class CCTiledGrid3D : CocosSharp.CCGridBase {
    // Fields
    protected System.Int16[] m_pIndices;
    protected CocosSharp.CCQuad3[] m_pOriginalVertices;
     
    // Constructors
    public CCTiledGrid3D(CocosSharp.CCGridSize gridSize) { }
    public CCTiledGrid3D(CocosSharp.CCGridSize gridSize, CocosSharp.CCTexture2D pTexture, bool bFlipped) { }
     
    // Methods
    public override void Blit() { }
    public override void CalculateVertexPoints() { }
    public CocosSharp.CCQuad3 OriginalTile(CocosSharp.CCGridSize pos) { return default(CocosSharp.CCQuad3); }
    public CocosSharp.CCQuad3 OriginalTile(int x, int y) { return default(CocosSharp.CCQuad3); }
    public override void Reuse() { }
    public void SetTile(CocosSharp.CCGridSize pos, ref CocosSharp.CCQuad3 coords) { }
    public void SetTile(int x, int y, ref CocosSharp.CCQuad3 coords) { }
    public CocosSharp.CCQuad3 Tile(CocosSharp.CCGridSize pos) { return default(CocosSharp.CCQuad3); }
    public CocosSharp.CCQuad3 Tile(int x, int y) { return default(CocosSharp.CCQuad3); }
  }
  public partial class CCTiledGrid3DAction : CocosSharp.CCGridAction {
    // Constructors
    public CCTiledGrid3DAction(float duration) : base (default(float)) { }
    public CCTiledGrid3DAction(float duration, CocosSharp.CCGridSize gridSize) : base (default(float)) { }
    protected CCTiledGrid3DAction(float duration, CocosSharp.CCGridSize gridSize, float amplitude) : base (default(float)) { }
     
    // Methods
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCTiledGrid3DActionState : CocosSharp.CCGridActionState {
    // Constructors
    public CCTiledGrid3DActionState(CocosSharp.CCTiledGrid3DAction action, CocosSharp.CCNode target) : base (default(CocosSharp.CCGridAction), default(CocosSharp.CCNode)) { }
     
    // Properties
    public override CocosSharp.CCGridBase Grid { get { return default(CocosSharp.CCGridBase); } protected set { } }
     
    // Methods
    public CocosSharp.CCQuad3 OriginalTile(CocosSharp.CCGridSize pos) { return default(CocosSharp.CCQuad3); }
    public CocosSharp.CCQuad3 OriginalTile(int x, int y) { return default(CocosSharp.CCQuad3); }
    public void SetTile(CocosSharp.CCGridSize pos, ref CocosSharp.CCQuad3 coords) { }
    public void SetTile(int x, int y, ref CocosSharp.CCQuad3 coords) { }
    public CocosSharp.CCQuad3 Tile(CocosSharp.CCGridSize pos) { return default(CocosSharp.CCQuad3); }
    public CocosSharp.CCQuad3 Tile(int x, int y) { return default(CocosSharp.CCQuad3); }
  }
  public partial class CCTileMapAtlas : CocosSharp.CCAtlasNode {
    // Constructors
    public CCTileMapAtlas(string tile, string mapFile, int tileWidth, int tileHeight) : base (default(string), default(int), default(int), default(int)) { }
     
    // Properties
    protected int NumOfItemsToRender { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } }
    protected System.Collections.Generic.Dictionary<CocosSharp.CCGridSize, System.Int32> PositionToAtlasIndex { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Collections.Generic.Dictionary<CocosSharp.CCGridSize, System.Int32>); } }
     
    // Methods
    public void ReleaseMap() { }
    public void SetTile(CocosSharp.CCColor4B tile, CocosSharp.CCGridSize position) { }
    public CocosSharp.CCColor4B TileAt(CocosSharp.CCGridSize position) { return default(CocosSharp.CCColor4B); }
    public override void UpdateAtlasValues() { }
  }
  public partial class CCTintBy : CocosSharp.CCActionInterval {
    // Constructors
    public CCTintBy(float duration, short deltaRed, short deltaGreen, short deltaBlue) { }
     
    // Properties
    public short DeltaB { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(short); } }
    public short DeltaG { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(short); } }
    public short DeltaR { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(short); } }
     
    // Methods
    public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCTintByState : CocosSharp.CCActionIntervalState {
    // Constructors
    public CCTintByState(CocosSharp.CCTintBy action, CocosSharp.CCNode target) : base (default(CocosSharp.CCActionInterval), default(CocosSharp.CCNode)) { }
     
    // Properties
    protected short DeltaB { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(short); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    protected short DeltaG { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(short); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    protected short DeltaR { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(short); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    protected short FromB { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(short); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    protected short FromG { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(short); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    protected short FromR { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(short); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
     
    // Methods
    public override void Update(float time) { }
  }
  public partial class CCTintTo : CocosSharp.CCActionInterval {
    // Constructors
    public CCTintTo(float duration, byte red, byte green, byte blue) { }
     
    // Properties
    public CocosSharp.CCColor3B ColorTo { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCColor3B); } }
     
    // Methods
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCTintToState : CocosSharp.CCActionIntervalState {
    // Constructors
    public CCTintToState(CocosSharp.CCTintTo action, CocosSharp.CCNode target) : base (default(CocosSharp.CCActionInterval), default(CocosSharp.CCNode)) { }
     
    // Properties
    protected CocosSharp.CCColor3B ColorFrom { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCColor3B); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    protected CocosSharp.CCColor3B ColorTo { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCColor3B); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
     
    // Methods
    public override void Update(float time) { }
  }
  public partial class CCTMXLayer : CocosSharp.CCSpriteBatchNode {
    // Constructors
    public CCTMXLayer(CocosSharp.CCTMXTilesetInfo tileSetInfo, CocosSharp.CCTMXLayerInfo layerInfo, CocosSharp.CCTMXMapInfo mapInfo) { }
     
    // Properties
    public string LayerName { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(string); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public CocosSharp.CCTMXOrientation LayerOrientation { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCTMXOrientation); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public CocosSharp.CCSize LayerSize { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCSize); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public CocosSharp.CCSize MapTileSize { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCSize); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public System.Collections.Generic.Dictionary<System.String, System.String> Properties { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Collections.Generic.Dictionary<System.String, System.String>); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public System.UInt32[] Tiles { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.UInt32[]); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public CocosSharp.CCTMXTilesetInfo TileSet { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCTMXTilesetInfo); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
     
    // Methods
    public override void AddChild(CocosSharp.CCNode child, int zOrder, int tag) { }
    protected override void Draw() { }
    public virtual CocosSharp.CCPoint PositionAt(CocosSharp.CCPoint tileCoord) { return default(CocosSharp.CCPoint); }
    public virtual string PropertyNamed(string propertyName) { return default(string); }
    public virtual void ReleaseMap() { }
    public override void RemoveChild(CocosSharp.CCNode node, bool cleanup) { }
    public virtual void RemoveTileAt(CocosSharp.CCPoint pos) { }
    public virtual void SetTileGID(uint gid, CocosSharp.CCPoint pos) { }
    public virtual void SetTileGID(uint gid, CocosSharp.CCPoint pos, uint flags) { }
    public virtual CocosSharp.CCSprite TileAt(CocosSharp.CCPoint pos) { return default(CocosSharp.CCSprite); }
    public virtual uint TileGIDAt(CocosSharp.CCPoint pos) { return default(uint); }
    public virtual uint TileGIDAt(CocosSharp.CCPoint pos, out uint flags) { flags = default(uint); return default(uint); }
  }
  public enum CCTMXLayerAttrib {
    // Fields
    Base64 = 2,
    Gzip = 4,
    None = 1,
    Zlib = 8,
  }
  public partial class CCTMXLayerInfo {
    // Constructors
    public CCTMXLayerInfo() { }
     
    // Properties
    public CocosSharp.CCSize LayerSize { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCSize); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public uint MaxGID { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(uint); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public uint MinGID { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(uint); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public string Name { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(string); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public CocosSharp.CCPoint Offset { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCPoint); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public byte Opacity { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(byte); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public bool OwnTiles { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public System.Collections.Generic.Dictionary<System.String, System.String> Properties { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Collections.Generic.Dictionary<System.String, System.String>); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public System.UInt32[] Tiles { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.UInt32[]); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public bool Visible { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
     
    // Methods
  }
  public partial class CCTMXMapInfo : CocosSharp.ICCSAXDelegator {
    // Constructors
    public CCTMXMapInfo(System.IO.StreamReader stream) { }
    public CCTMXMapInfo(string tmxFile) { }
     
    // Properties
    protected int LayerAttribs { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } }
    public CocosSharp.CCSize MapSize { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCSize); } }
    public int Orientation { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } }
    public uint ParentGID { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(uint); } }
    public bool StoringCharacters { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } }
    public CocosSharp.CCSize TileSize { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCSize); } }
    public string TMXFileName { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(string); } }
     
    // Methods
    public void EndElement(object ctx, string elementName) { }
    public void StartElement(object ctx, string name, System.String[] atts) { }
    public void TextHandler(object ctx, System.Byte[] ch, int len) { }
  }
  public partial class CCTMXObjectGroup {
    // Constructors
    public CCTMXObjectGroup() { }
     
    // Properties
    public string GroupName { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(string); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public System.Collections.Generic.List<System.Collections.Generic.Dictionary<System.String, System.String>> Objects { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Collections.Generic.List<System.Collections.Generic.Dictionary<System.String, System.String>>); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public CocosSharp.CCPoint PositionOffset { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCPoint); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public System.Collections.Generic.Dictionary<System.String, System.String> Properties { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Collections.Generic.Dictionary<System.String, System.String>); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
     
    // Methods
    public System.Collections.Generic.Dictionary<System.String, System.String> ObjectNamed(string objectName) { return default(System.Collections.Generic.Dictionary<System.String, System.String>); }
    public string PropertyNamed(string propertyName) { return default(string); }
  }
  public enum CCTMXOrientation {
    // Fields
    Hex = 1,
    Iso = 2,
    Ortho = 0,
  }
  public enum CCTMXProperty {
    // Fields
    Layer = 2,
    Map = 1,
    None = 0,
    Object = 4,
    ObjectGroup = 3,
    Tile = 5,
  }
  public partial class CCTMXTiledMap : CocosSharp.CCNode {
    // Constructors
    public CCTMXTiledMap(CocosSharp.CCTMXMapInfo mapInfo) { }
    public CCTMXTiledMap(System.IO.StreamReader tmxFile) { }
    public CCTMXTiledMap(string tmxFile) { }
     
    // Properties
    public int MapOrientation { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } }
    public CocosSharp.CCSize MapSize { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCSize); } }
    public CocosSharp.CCSize TileSize { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCSize); } }
     
    // Methods
    public CocosSharp.CCTMXLayer LayerNamed(string layerName) { return default(CocosSharp.CCTMXLayer); }
    public CocosSharp.CCTMXObjectGroup ObjectGroupNamed(string groupName) { return default(CocosSharp.CCTMXObjectGroup); }
    public System.Collections.Generic.Dictionary<System.String, System.String> PropertiesForGID(uint GID) { return default(System.Collections.Generic.Dictionary<System.String, System.String>); }
    public string PropertyNamed(string propertyName) { return default(string); }
  }
  public partial class CCTMXTileFlags {
    // Fields
    public static uint FlippedAll;
    public static uint FlippedMask;
    public static uint Horizontal;
    public static uint TileDiagonal;
    public static uint Vertical;
     
    // Constructors
    public CCTMXTileFlags() { }
  }
  public partial class CCTMXTilesetInfo {
    // Constructors
    public CCTMXTilesetInfo() { }
     
    // Properties
    public uint FirstGid { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(uint); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public CocosSharp.CCSize ImageSize { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCSize); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public int Margin { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public string Name { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(string); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public string SourceImage { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(string); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public int Spacing { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public CocosSharp.CCSize TileSize { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCSize); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
     
    // Methods
    public CocosSharp.CCRect RectForGID(uint gid) { return default(CocosSharp.CCRect); }
  }
  public partial class CCToggleVisibility : CocosSharp.CCActionInstant {
    // Constructors
    public CCToggleVisibility() { }
     
    // Methods
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCToggleVisibilityState : CocosSharp.CCActionInstantState {
    // Constructors
    public CCToggleVisibilityState(CocosSharp.CCToggleVisibility action, CocosSharp.CCNode target) : base (default(CocosSharp.CCActionInstant), default(CocosSharp.CCNode)) { }
  }
  public partial class CCTouch {
    // Constructors
    public CCTouch() { }
    public CCTouch(int id, float x, float y) { }
     
    // Properties
    public CocosSharp.CCPoint Delta { get { return default(CocosSharp.CCPoint); } }
    public int Id { get { return default(int); } }
    public CocosSharp.CCPoint Location { get { return default(CocosSharp.CCPoint); } }
    public CocosSharp.CCPoint LocationInView { get { return default(CocosSharp.CCPoint); } }
    public CocosSharp.CCPoint PreviousLocation { get { return default(CocosSharp.CCPoint); } }
    public CocosSharp.CCPoint PreviousLocationInView { get { return default(CocosSharp.CCPoint); } }
    public CocosSharp.CCPoint StartLocation { get { return default(CocosSharp.CCPoint); } }
    public CocosSharp.CCPoint StartLocationInView { get { return default(CocosSharp.CCPoint); } }
     
    // Methods
    public void SetTouchInfo(int id, float x, float y) { }
  }
  public partial class CCTransitionCrossFade : CocosSharp.CCTransitionScene {
    // Constructors
    public CCTransitionCrossFade(float t, CocosSharp.CCScene scene) { }
     
    // Methods
    protected override void Draw() { }
    public override void OnEnter() { }
    public override void OnExit() { }
  }
  public partial class CCTransitionFade : CocosSharp.CCTransitionScene {
    // Fields
    protected CocosSharp.CCColor4B m_tColor;
     
    // Constructors
    public CCTransitionFade(float t, CocosSharp.CCScene scene) { }
    public CCTransitionFade(float duration, CocosSharp.CCScene scene, CocosSharp.CCColor3B color) { }
     
    // Methods
    public override void OnEnter() { }
    public override void OnExit() { }
  }
  public partial class CCTransitionFadeBL : CocosSharp.CCTransitionFadeTR {
    // Constructors
    public CCTransitionFadeBL() { }
    public CCTransitionFadeBL(float t, CocosSharp.CCScene scene) { }
     
    // Methods
    public override CocosSharp.CCActionInterval CreateAction(CocosSharp.CCGridSize size) { return default(CocosSharp.CCActionInterval); }
  }
  public partial class CCTransitionFadeDown : CocosSharp.CCTransitionFadeTR {
    // Constructors
    public CCTransitionFadeDown() { }
    public CCTransitionFadeDown(float t, CocosSharp.CCScene scene) { }
     
    // Methods
    public override CocosSharp.CCActionInterval CreateAction(CocosSharp.CCGridSize size) { return default(CocosSharp.CCActionInterval); }
  }
  public partial class CCTransitionFadeTR : CocosSharp.CCTransitionScene, CocosSharp.ICCTransitionEaseScene {
    // Constructors
    public CCTransitionFadeTR() { }
    public CCTransitionFadeTR(float t, CocosSharp.CCScene scene) { }
     
    // Methods
    public virtual CocosSharp.CCActionInterval CreateAction(CocosSharp.CCGridSize size) { return default(CocosSharp.CCActionInterval); }
    public virtual CocosSharp.CCFiniteTimeAction EaseAction(CocosSharp.CCActionInterval action) { return default(CocosSharp.CCFiniteTimeAction); }
    public override void OnEnter() { }
    protected override void SceneOrder() { }
  }
  public partial class CCTransitionFadeUp : CocosSharp.CCTransitionFadeTR {
    // Constructors
    public CCTransitionFadeUp() { }
    public CCTransitionFadeUp(float t, CocosSharp.CCScene scene) { }
     
    // Methods
    public override CocosSharp.CCActionInterval CreateAction(CocosSharp.CCGridSize size) { return default(CocosSharp.CCActionInterval); }
  }
  public partial class CCTransitionFlipAngular : CocosSharp.CCTransitionSceneOriented {
    // Constructors
    public CCTransitionFlipAngular() { }
    public CCTransitionFlipAngular(float t, CocosSharp.CCScene s, CocosSharp.CCTransitionOrientation o) { }
     
    // Methods
    public override void OnEnter() { }
  }
  public partial class CCTransitionFlipX : CocosSharp.CCTransitionSceneOriented {
    // Constructors
    public CCTransitionFlipX() { }
    public CCTransitionFlipX(float t, CocosSharp.CCScene s, CocosSharp.CCTransitionOrientation o) { }
     
    // Methods
    public override void OnEnter() { }
  }
  public partial class CCTransitionFlipY : CocosSharp.CCTransitionSceneOriented {
    // Constructors
    public CCTransitionFlipY() { }
    public CCTransitionFlipY(float t, CocosSharp.CCScene s, CocosSharp.CCTransitionOrientation o) { }
     
    // Methods
    public override void OnEnter() { }
  }
  public partial class CCTransitionJumpZoom : CocosSharp.CCTransitionScene {
    // Constructors
    public CCTransitionJumpZoom() { }
    public CCTransitionJumpZoom(float t, CocosSharp.CCScene scene) { }
     
    // Methods
    public override void OnEnter() { }
  }
  public partial class CCTransitionMoveInB : CocosSharp.CCTransitionMoveInL {
    // Constructors
    public CCTransitionMoveInB() { }
    public CCTransitionMoveInB(float t, CocosSharp.CCScene scene) { }
     
    // Methods
    public override void InitScenes() { }
  }
  public partial class CCTransitionMoveInL : CocosSharp.CCTransitionScene, CocosSharp.ICCTransitionEaseScene {
    // Constructors
    public CCTransitionMoveInL() { }
    public CCTransitionMoveInL(float t, CocosSharp.CCScene scene) { }
     
    // Methods
    public virtual CocosSharp.CCActionInterval Action() { return default(CocosSharp.CCActionInterval); }
    public CocosSharp.CCFiniteTimeAction EaseAction(CocosSharp.CCActionInterval action) { return default(CocosSharp.CCFiniteTimeAction); }
    public virtual void InitScenes() { }
    public override void OnEnter() { }
  }
  public partial class CCTransitionMoveInR : CocosSharp.CCTransitionMoveInL {
    // Constructors
    public CCTransitionMoveInR() { }
    public CCTransitionMoveInR(float t, CocosSharp.CCScene scene) { }
     
    // Methods
    public override void InitScenes() { }
  }
  public partial class CCTransitionMoveInT : CocosSharp.CCTransitionMoveInL {
    // Constructors
    public CCTransitionMoveInT() { }
    public CCTransitionMoveInT(float t, CocosSharp.CCScene scene) { }
     
    // Methods
    public override void InitScenes() { }
  }
  public enum CCTransitionOrientation {
    // Fields
    DownOver = 1,
    LeftOver = 0,
    RightOver = 1,
    UpOver = 0,
  }
  public partial class CCTransitionPageTurn : CocosSharp.CCTransitionScene {
    // Fields
    protected bool m_bBack;
     
    // Constructors
    public CCTransitionPageTurn() { }
    public CCTransitionPageTurn(float t, CocosSharp.CCScene scene, bool backwards) { }
     
    // Methods
    public CocosSharp.CCActionInterval ActionWithSize(CocosSharp.CCGridSize vector) { return default(CocosSharp.CCActionInterval); }
    public override void OnEnter() { }
    protected override void SceneOrder() { }
  }
  public abstract partial class CCTransitionProgress : CocosSharp.CCTransitionScene {
    // Fields
    protected float m_fFrom;
    protected float m_fTo;
    protected CocosSharp.CCScene m_pSceneToBeModified;
     
    // Constructors
    public CCTransitionProgress() { }
    public CCTransitionProgress(float t, CocosSharp.CCScene scene) { }
     
    // Methods
    public override void OnEnter() { }
    public override void OnExit() { }
    protected abstract CocosSharp.CCProgressTimer ProgressTimerNodeWithRenderTexture(CocosSharp.CCRenderTexture texture);
    protected override void SceneOrder() { }
    protected virtual void SetupTransition() { }
  }
  public partial class CCTransitionProgressHorizontal : CocosSharp.CCTransitionProgress {
    // Constructors
    public CCTransitionProgressHorizontal(float t, CocosSharp.CCScene scene) { }
     
    // Methods
    protected override CocosSharp.CCProgressTimer ProgressTimerNodeWithRenderTexture(CocosSharp.CCRenderTexture texture) { return default(CocosSharp.CCProgressTimer); }
  }
  public partial class CCTransitionProgressInOut : CocosSharp.CCTransitionProgress {
    // Constructors
    public CCTransitionProgressInOut(float t, CocosSharp.CCScene scene) { }
     
    // Methods
    protected override CocosSharp.CCProgressTimer ProgressTimerNodeWithRenderTexture(CocosSharp.CCRenderTexture texture) { return default(CocosSharp.CCProgressTimer); }
    protected override void SceneOrder() { }
    protected override void SetupTransition() { }
  }
  public partial class CCTransitionProgressOutIn : CocosSharp.CCTransitionProgress {
    // Constructors
    public CCTransitionProgressOutIn(float t, CocosSharp.CCScene scene) { }
     
    // Methods
    protected override CocosSharp.CCProgressTimer ProgressTimerNodeWithRenderTexture(CocosSharp.CCRenderTexture texture) { return default(CocosSharp.CCProgressTimer); }
  }
  public partial class CCTransitionProgressRadialCCW : CocosSharp.CCTransitionProgress {
    // Constructors
    public CCTransitionProgressRadialCCW(float t, CocosSharp.CCScene scene) { }
     
    // Methods
    protected override CocosSharp.CCProgressTimer ProgressTimerNodeWithRenderTexture(CocosSharp.CCRenderTexture texture) { return default(CocosSharp.CCProgressTimer); }
  }
  public partial class CCTransitionProgressRadialCW : CocosSharp.CCTransitionProgress {
    // Constructors
    public CCTransitionProgressRadialCW(float t, CocosSharp.CCScene scene) { }
     
    // Methods
    protected override CocosSharp.CCProgressTimer ProgressTimerNodeWithRenderTexture(CocosSharp.CCRenderTexture texture) { return default(CocosSharp.CCProgressTimer); }
  }
  public partial class CCTransitionProgressVertical : CocosSharp.CCTransitionProgress {
    // Constructors
    public CCTransitionProgressVertical(float t, CocosSharp.CCScene scene) { }
     
    // Methods
    protected override CocosSharp.CCProgressTimer ProgressTimerNodeWithRenderTexture(CocosSharp.CCRenderTexture texture) { return default(CocosSharp.CCProgressTimer); }
  }
  public partial class CCTransitionRotoZoom : CocosSharp.CCTransitionScene {
    // Constructors
    public CCTransitionRotoZoom() { }
    public CCTransitionRotoZoom(float t, CocosSharp.CCScene scene) { }
     
    // Methods
    public override void OnEnter() { }
  }
  public partial class CCTransitionScene : CocosSharp.CCScene {
    // Constructors
    protected CCTransitionScene() { }
    public CCTransitionScene(float t, CocosSharp.CCScene scene) { }
     
    // Properties
    protected float Duration { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    protected CocosSharp.CCScene InScene { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCScene); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    protected bool IsInSceneOnTop { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    protected bool IsSendCleanupToScene { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public override bool IsTransition { get { return default(bool); } }
    protected CocosSharp.CCScene OutScene { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCScene); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
     
    // Methods
    public override void Cleanup() { }
    protected override void Draw() { }
    public void Finish() { }
    public void HideOutShowIn() { }
    public override void OnEnter() { }
    public override void OnExit() { }
    public virtual void Reset(float t, CocosSharp.CCScene scene) { }
    protected virtual void SceneOrder() { }
  }
  public partial class CCTransitionSceneOriented : CocosSharp.CCTransitionScene {
    // Fields
    protected CocosSharp.CCTransitionOrientation m_eOrientation;
     
    // Constructors
    public CCTransitionSceneOriented() { }
    public CCTransitionSceneOriented(float t, CocosSharp.CCScene scene, CocosSharp.CCTransitionOrientation orientation) { }
  }
  public partial class CCTransitionShrinkGrow : CocosSharp.CCTransitionScene, CocosSharp.ICCTransitionEaseScene {
    // Constructors
    public CCTransitionShrinkGrow() { }
    public CCTransitionShrinkGrow(float t, CocosSharp.CCScene scene) { }
     
    // Methods
    public CocosSharp.CCFiniteTimeAction EaseAction(CocosSharp.CCActionInterval action) { return default(CocosSharp.CCFiniteTimeAction); }
    public override void OnEnter() { }
  }
  public partial class CCTransitionSlideInB : CocosSharp.CCTransitionSlideInL {
    // Constructors
    public CCTransitionSlideInB() { }
    public CCTransitionSlideInB(float t, CocosSharp.CCScene scene) { }
     
    // Methods
    public override CocosSharp.CCActionInterval Action() { return default(CocosSharp.CCActionInterval); }
    protected override void InitScenes() { }
    protected override void SceneOrder() { }
  }
  public partial class CCTransitionSlideInL : CocosSharp.CCTransitionScene, CocosSharp.ICCTransitionEaseScene {
    // Constructors
    public CCTransitionSlideInL() { }
    public CCTransitionSlideInL(float t, CocosSharp.CCScene scene) { }
     
    // Methods
    public virtual CocosSharp.CCActionInterval Action() { return default(CocosSharp.CCActionInterval); }
    public virtual CocosSharp.CCFiniteTimeAction EaseAction(CocosSharp.CCActionInterval action) { return default(CocosSharp.CCFiniteTimeAction); }
    protected virtual void InitScenes() { }
    public override void OnEnter() { }
    protected override void SceneOrder() { }
  }
  public partial class CCTransitionSlideInR : CocosSharp.CCTransitionSlideInL {
    // Constructors
    public CCTransitionSlideInR() { }
    public CCTransitionSlideInR(float t, CocosSharp.CCScene scene) { }
     
    // Methods
    public override CocosSharp.CCActionInterval Action() { return default(CocosSharp.CCActionInterval); }
    protected override void InitScenes() { }
    protected override void SceneOrder() { }
  }
  public partial class CCTransitionSlideInT : CocosSharp.CCTransitionSlideInL {
    // Constructors
    public CCTransitionSlideInT() { }
    public CCTransitionSlideInT(float t, CocosSharp.CCScene scene) { }
     
    // Methods
    public override CocosSharp.CCActionInterval Action() { return default(CocosSharp.CCActionInterval); }
    protected override void InitScenes() { }
    protected override void SceneOrder() { }
  }
  public partial class CCTransitionSplitCols : CocosSharp.CCTransitionScene, CocosSharp.ICCTransitionEaseScene {
    // Constructors
    public CCTransitionSplitCols() { }
    public CCTransitionSplitCols(float t, CocosSharp.CCScene scene) { }
     
    // Methods
    public virtual CocosSharp.CCActionInterval Action() { return default(CocosSharp.CCActionInterval); }
    public virtual CocosSharp.CCFiniteTimeAction EaseAction(CocosSharp.CCActionInterval action) { return default(CocosSharp.CCFiniteTimeAction); }
    public override void OnEnter() { }
  }
  public partial class CCTransitionSplitRows : CocosSharp.CCTransitionSplitCols {
    // Constructors
    public CCTransitionSplitRows() { }
    public CCTransitionSplitRows(float t, CocosSharp.CCScene scene) { }
     
    // Methods
    public override CocosSharp.CCActionInterval Action() { return default(CocosSharp.CCActionInterval); }
  }
  public partial class CCTransitionTurnOffTiles : CocosSharp.CCTransitionScene, CocosSharp.ICCTransitionEaseScene {
    // Constructors
    public CCTransitionTurnOffTiles() { }
    public CCTransitionTurnOffTiles(float t, CocosSharp.CCScene scene) { }
     
    // Methods
    public virtual CocosSharp.CCFiniteTimeAction EaseAction(CocosSharp.CCActionInterval action) { return default(CocosSharp.CCFiniteTimeAction); }
    public override void OnEnter() { }
    protected override void SceneOrder() { }
  }
  public partial class CCTransitionZoomFlipAngular : CocosSharp.CCTransitionSceneOriented {
    // Constructors
    public CCTransitionZoomFlipAngular() { }
    public CCTransitionZoomFlipAngular(float t, CocosSharp.CCScene s, CocosSharp.CCTransitionOrientation o) { }
     
    // Methods
    public override void OnEnter() { }
  }
  public partial class CCTransitionZoomFlipX : CocosSharp.CCTransitionSceneOriented {
    // Constructors
    public CCTransitionZoomFlipX() { }
    public CCTransitionZoomFlipX(float t, CocosSharp.CCScene s, CocosSharp.CCTransitionOrientation o) { }
     
    // Methods
    public override void OnEnter() { }
  }
  public partial class CCTransitionZoomFlipY : CocosSharp.CCTransitionSceneOriented {
    // Constructors
    public CCTransitionZoomFlipY() { }
    public CCTransitionZoomFlipY(float t, CocosSharp.CCScene s, CocosSharp.CCTransitionOrientation o) { }
     
    // Methods
    public override void OnEnter() { }
  }
  public partial class CCTurnOffTiles : CocosSharp.CCShuffleTiles {
    // Constructors
    public CCTurnOffTiles(float duration, CocosSharp.CCGridSize gridSize, int seed=-1) : base (default(CocosSharp.CCGridSize), default(float), default(int)) { }
     
    // Methods
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCTurnOffTilesState : CocosSharp.CCTiledGrid3DActionState {
    // Constructors
    public CCTurnOffTilesState(CocosSharp.CCTurnOffTiles action, CocosSharp.CCNode target) : base (default(CocosSharp.CCTiledGrid3DAction), default(CocosSharp.CCNode)) { }
     
    // Properties
    protected int TilesCount { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } }
    protected System.Int32[] TilesOrder { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Int32[]); } }
     
    // Methods
    public void Shuffle(System.Int32[] pArray, int nLen) { }
    public void TurnOffTile(CocosSharp.CCGridSize pos) { }
    public void TurnOnTile(CocosSharp.CCGridSize pos) { }
    public override void Update(float time) { }
  }
  public partial class CCTwirl : CocosSharp.CCGrid3DAction {
    // Constructors
    public CCTwirl(float duration, CocosSharp.CCGridSize gridSize) : base (default(float)) { }
    public CCTwirl(float duration, CocosSharp.CCGridSize gridSize, CocosSharp.CCPoint position, int twirls=0, float amplitude=0f) : base (default(float)) { }
     
    // Properties
    public CocosSharp.CCPoint Position { get { return default(CocosSharp.CCPoint); } }
    public CocosSharp.CCPoint PositionInPixels { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCPoint); } }
    public int Twirls { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } }
     
    // Methods
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCTwirlState : CocosSharp.CCGrid3DActionState {
    // Constructors
    public CCTwirlState(CocosSharp.CCTwirl action, CocosSharp.CCNode target) : base (default(CocosSharp.CCGrid3DAction), default(CocosSharp.CCNode)) { }
     
    // Properties
    public CocosSharp.CCPoint PositionInPixels { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCPoint); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public int Twirls { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
     
    // Methods
    public override void Update(float time) { }
  }
  public partial class CCUserDefault {
    internal CCUserDefault() { }
    // Properties
    public static CocosSharp.CCUserDefault SharedUserDefault { get { return default(CocosSharp.CCUserDefault); } }
     
    // Methods
    public void Flush() { }
    public bool GetBoolForKey(string pKey) { return default(bool); }
    public bool GetBoolForKey(string pKey, bool defaultValue) { return default(bool); }
    public double GetDoubleForKey(string pKey, double defaultValue) { return default(double); }
    public float GetFloatForKey(string pKey, float defaultValue) { return default(float); }
    public int GetIntegerForKey(string pKey) { return default(int); }
    public int GetIntegerForKey(string pKey, int defaultValue) { return default(int); }
    public string GetStringForKey(string pKey, string defaultValue) { return default(string); }
    public void PurgeSharedUserDefault() { }
    public void SetBoolForKey(string pKey, bool value) { }
    public void SetDoubleForKey(string pKey, double value) { }
    public void SetFloatForKey(string pKey, float value) { }
    public void SetIntegerForKey(string pKey, int value) { }
    public void SetStringForKey(string pKey, string value) { }
  }
  public partial class CCUtils {
    // Constructors
    public CCUtils() { }
     
    // Methods
    public static CocosSharp.CCPoint CCCardinalSplineAt(CocosSharp.CCPoint p0, CocosSharp.CCPoint p1, CocosSharp.CCPoint p2, CocosSharp.CCPoint p3, float tension, float t) { return default(CocosSharp.CCPoint); }
    public static int CCNextPOT(int x) { return default(int); }
    public static long CCNextPOT(long x) { return default(long); }
    public static float CCParseFloat(string toParse) { return default(float); }
    public static float CCParseFloat(string toParse, System.Globalization.NumberStyles ns) { return default(float); }
    public static int CCParseInt(string toParse) { return default(int); }
    public static int CCParseInt(string toParse, System.Globalization.NumberStyles ns) { return default(int); }
    public static System.Collections.Generic.List<System.String> GetGLExtensions() { return default(System.Collections.Generic.List<System.String>); }
    public static void Split(string src, string token, System.Collections.Generic.List<System.String> vect) { }
    public static bool SplitWithForm(string pStr, System.Collections.Generic.List<System.String> strs) { return default(bool); }
  }
  public partial class CCV2F_C4B_T2F {
    // Fields
    public CocosSharp.CCColor4B Colors;
    public CocosSharp.CCTex2F TexCoords;
    public CocosSharp.CCVertex2F Vertices;
     
    // Constructors
    public CCV2F_C4B_T2F() { }
  }
  public partial class CCV2F_C4B_T2F_Quad {
    // Fields
    public CocosSharp.CCV2F_C4B_T2F BottomLeft;
    public CocosSharp.CCV2F_C4B_T2F BottomRight;
    public CocosSharp.CCV2F_C4B_T2F TopLeft;
    public CocosSharp.CCV2F_C4B_T2F TopRight;
     
    // Constructors
    public CCV2F_C4B_T2F_Quad() { }
  }
  public partial class CCV2F_C4F_T2F {
    // Fields
    public CocosSharp.CCColor4F Colors;
    public CocosSharp.CCTex2F TexCoords;
    public CocosSharp.CCVertex2F Vertices;
     
    // Constructors
    public CCV2F_C4F_T2F() { }
  }
  public partial class CCV2F_C4F_T2F_Quad {
    // Fields
    public CocosSharp.CCV2F_C4F_T2F BottomLeft;
    public CocosSharp.CCV2F_C4F_T2F BottomRight;
    public CocosSharp.CCV2F_C4F_T2F TopLeft;
    public CocosSharp.CCV2F_C4F_T2F TopRight;
     
    // Constructors
    public CCV2F_C4F_T2F_Quad() { }
  }
		
  [System.Runtime.Serialization.DataContractAttribute]
  [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
  public partial struct CCVector2 : System.IEquatable<CocosSharp.CCVector2> {
    // Fields
    [System.Runtime.Serialization.DataMemberAttribute]
    public float X;
    [System.Runtime.Serialization.DataMemberAttribute]
    public float Y;
     
    // Constructors
    public CCVector2(float value) { throw new System.NotImplementedException(); }
    public CCVector2(float x, float y) { throw new System.NotImplementedException(); }
     
    // Properties
    public static CocosSharp.CCVector2 One { get { return default(CocosSharp.CCVector2); } }
    public static CocosSharp.CCVector2 UnitX { get { return default(CocosSharp.CCVector2); } }
    public static CocosSharp.CCVector2 UnitY { get { return default(CocosSharp.CCVector2); } }
    public static CocosSharp.CCVector2 Zero { get { return default(CocosSharp.CCVector2); } }
     
    // Methods
    public static CocosSharp.CCVector2 Add(CocosSharp.CCVector2 value1, CocosSharp.CCVector2 value2) { return default(CocosSharp.CCVector2); }
    public static void Add(ref CocosSharp.CCVector2 value1, ref CocosSharp.CCVector2 value2, out CocosSharp.CCVector2 result) { result = default(CocosSharp.CCVector2); }
    public static CocosSharp.CCVector2 Barycentric(CocosSharp.CCVector2 value1, CocosSharp.CCVector2 value2, CocosSharp.CCVector2 value3, float amount1, float amount2) { return default(CocosSharp.CCVector2); }
    public static void Barycentric(ref CocosSharp.CCVector2 value1, ref CocosSharp.CCVector2 value2, ref CocosSharp.CCVector2 value3, float amount1, float amount2, out CocosSharp.CCVector2 result) { result = default(CocosSharp.CCVector2); }
    public static CocosSharp.CCVector2 CatmullRom(CocosSharp.CCVector2 value1, CocosSharp.CCVector2 value2, CocosSharp.CCVector2 value3, CocosSharp.CCVector2 value4, float amount) { return default(CocosSharp.CCVector2); }
    public static void CatmullRom(ref CocosSharp.CCVector2 value1, ref CocosSharp.CCVector2 value2, ref CocosSharp.CCVector2 value3, ref CocosSharp.CCVector2 value4, float amount, out CocosSharp.CCVector2 result) { result = default(CocosSharp.CCVector2); }
    public static CocosSharp.CCVector2 Clamp(CocosSharp.CCVector2 value1, CocosSharp.CCVector2 min, CocosSharp.CCVector2 max) { return default(CocosSharp.CCVector2); }
    public static void Clamp(ref CocosSharp.CCVector2 value1, ref CocosSharp.CCVector2 min, ref CocosSharp.CCVector2 max, out CocosSharp.CCVector2 result) { result = default(CocosSharp.CCVector2); }
    public static float Distance(CocosSharp.CCVector2 value1, CocosSharp.CCVector2 value2) { return default(float); }
    public static void Distance(ref CocosSharp.CCVector2 value1, ref CocosSharp.CCVector2 value2, out float result) { result = default(float); }
    public static float DistanceSquared(CocosSharp.CCVector2 value1, CocosSharp.CCVector2 value2) { return default(float); }
    public static void DistanceSquared(ref CocosSharp.CCVector2 value1, ref CocosSharp.CCVector2 value2, out float result) { result = default(float); }
    public static CocosSharp.CCVector2 Divide(CocosSharp.CCVector2 value1, CocosSharp.CCVector2 value2) { return default(CocosSharp.CCVector2); }
    public static CocosSharp.CCVector2 Divide(CocosSharp.CCVector2 value1, float divider) { return default(CocosSharp.CCVector2); }
    public static void Divide(ref CocosSharp.CCVector2 value1, ref CocosSharp.CCVector2 value2, out CocosSharp.CCVector2 result) { result = default(CocosSharp.CCVector2); }
    public static void Divide(ref CocosSharp.CCVector2 value1, float divider, out CocosSharp.CCVector2 result) { result = default(CocosSharp.CCVector2); }
    public static float Dot(CocosSharp.CCVector2 value1, CocosSharp.CCVector2 value2) { return default(float); }
    public static void Dot(ref CocosSharp.CCVector2 value1, ref CocosSharp.CCVector2 value2, out float result) { result = default(float); }
    public bool Equals(CocosSharp.CCVector2 other) { return default(bool); }
    public override bool Equals(object obj) { return default(bool); }
    public override int GetHashCode() { return default(int); }
    public static CocosSharp.CCVector2 Hermite(CocosSharp.CCVector2 value1, CocosSharp.CCVector2 tangent1, CocosSharp.CCVector2 value2, CocosSharp.CCVector2 tangent2, float amount) { return default(CocosSharp.CCVector2); }
    public static void Hermite(ref CocosSharp.CCVector2 value1, ref CocosSharp.CCVector2 tangent1, ref CocosSharp.CCVector2 value2, ref CocosSharp.CCVector2 tangent2, float amount, out CocosSharp.CCVector2 result) { result = default(CocosSharp.CCVector2); }
    public float Length() { return default(float); }
    public float LengthSquared() { return default(float); }
    public static CocosSharp.CCVector2 Lerp(CocosSharp.CCVector2 value1, CocosSharp.CCVector2 value2, float amount) { return default(CocosSharp.CCVector2); }
    public static void Lerp(ref CocosSharp.CCVector2 value1, ref CocosSharp.CCVector2 value2, float amount, out CocosSharp.CCVector2 result) { result = default(CocosSharp.CCVector2); }
    public static CocosSharp.CCVector2 Max(CocosSharp.CCVector2 value1, CocosSharp.CCVector2 value2) { return default(CocosSharp.CCVector2); }
    public static void Max(ref CocosSharp.CCVector2 value1, ref CocosSharp.CCVector2 value2, out CocosSharp.CCVector2 result) { result = default(CocosSharp.CCVector2); }
    public static CocosSharp.CCVector2 Min(CocosSharp.CCVector2 value1, CocosSharp.CCVector2 value2) { return default(CocosSharp.CCVector2); }
    public static void Min(ref CocosSharp.CCVector2 value1, ref CocosSharp.CCVector2 value2, out CocosSharp.CCVector2 result) { result = default(CocosSharp.CCVector2); }
    public static CocosSharp.CCVector2 Multiply(CocosSharp.CCVector2 value1, CocosSharp.CCVector2 value2) { return default(CocosSharp.CCVector2); }
    public static CocosSharp.CCVector2 Multiply(CocosSharp.CCVector2 value1, float scaleFactor) { return default(CocosSharp.CCVector2); }
    public static void Multiply(ref CocosSharp.CCVector2 value1, ref CocosSharp.CCVector2 value2, out CocosSharp.CCVector2 result) { result = default(CocosSharp.CCVector2); }
    public static void Multiply(ref CocosSharp.CCVector2 value1, float scaleFactor, out CocosSharp.CCVector2 result) { result = default(CocosSharp.CCVector2); }
    public static CocosSharp.CCVector2 Negate(CocosSharp.CCVector2 value) { return default(CocosSharp.CCVector2); }
    public static void Negate(ref CocosSharp.CCVector2 value, out CocosSharp.CCVector2 result) { result = default(CocosSharp.CCVector2); }
    public void Normalize() { }
    public static CocosSharp.CCVector2 Normalize(CocosSharp.CCVector2 value) { return default(CocosSharp.CCVector2); }
    public static void Normalize(ref CocosSharp.CCVector2 value, out CocosSharp.CCVector2 result) { result = default(CocosSharp.CCVector2); }
    public static CocosSharp.CCVector2 operator +(CocosSharp.CCVector2 value1, CocosSharp.CCVector2 value2) { return default(CocosSharp.CCVector2); }
    public static CocosSharp.CCVector2 operator /(CocosSharp.CCVector2 value1, CocosSharp.CCVector2 value2) { return default(CocosSharp.CCVector2); }
    public static CocosSharp.CCVector2 operator /(CocosSharp.CCVector2 value1, float divider) { return default(CocosSharp.CCVector2); }
    public static bool operator ==(CocosSharp.CCVector2 value1, CocosSharp.CCVector2 value2) { return default(bool); }
    public static bool operator !=(CocosSharp.CCVector2 value1, CocosSharp.CCVector2 value2) { return default(bool); }
    public static CocosSharp.CCVector2 operator *(CocosSharp.CCVector2 value1, CocosSharp.CCVector2 value2) { return default(CocosSharp.CCVector2); }
    public static CocosSharp.CCVector2 operator *(CocosSharp.CCVector2 value, float scaleFactor) { return default(CocosSharp.CCVector2); }
    public static CocosSharp.CCVector2 operator *(float scaleFactor, CocosSharp.CCVector2 value) { return default(CocosSharp.CCVector2); }
    public static CocosSharp.CCVector2 operator -(CocosSharp.CCVector2 value1, CocosSharp.CCVector2 value2) { return default(CocosSharp.CCVector2); }
    public static CocosSharp.CCVector2 operator -(CocosSharp.CCVector2 value) { return default(CocosSharp.CCVector2); }
    public static CocosSharp.CCVector2 Reflect(CocosSharp.CCVector2 vector, CocosSharp.CCVector2 normal) { return default(CocosSharp.CCVector2); }
    public static void Reflect(ref CocosSharp.CCVector2 vector, ref CocosSharp.CCVector2 normal, out CocosSharp.CCVector2 result) { result = default(CocosSharp.CCVector2); }
    public static CocosSharp.CCVector2 SmoothStep(CocosSharp.CCVector2 value1, CocosSharp.CCVector2 value2, float amount) { return default(CocosSharp.CCVector2); }
    public static void SmoothStep(ref CocosSharp.CCVector2 value1, ref CocosSharp.CCVector2 value2, float amount, out CocosSharp.CCVector2 result) { result = default(CocosSharp.CCVector2); }
    public static CocosSharp.CCVector2 Subtract(CocosSharp.CCVector2 value1, CocosSharp.CCVector2 value2) { return default(CocosSharp.CCVector2); }
    public static void Subtract(ref CocosSharp.CCVector2 value1, ref CocosSharp.CCVector2 value2, out CocosSharp.CCVector2 result) { result = default(CocosSharp.CCVector2); }
    public override string ToString() { return default(string); }
    public static CocosSharp.CCVector2 Transform(CocosSharp.CCVector2 position, CocosSharp.CCAffineTransform matrix) { return default(CocosSharp.CCVector2); }
    public static void Transform(ref CocosSharp.CCVector2 position, ref CocosSharp.CCAffineTransform affineTransform, out CocosSharp.CCVector2 result) { result = default(CocosSharp.CCVector2); }
    public static void Transform(CocosSharp.CCVector2[] sourceArray, ref CocosSharp.CCAffineTransform affineTransform, CocosSharp.CCVector2[] destinationArray) { }
    public static void Transform(CocosSharp.CCVector2[] sourceArray, int sourceIndex, ref CocosSharp.CCAffineTransform matrix, CocosSharp.CCVector2[] destinationArray, int destinationIndex, int length) { }
    public static void TransformNormal(ref CocosSharp.CCVector2 normal, ref CocosSharp.CCAffineTransform affineTransform, out CocosSharp.CCVector2 result) { result = default(CocosSharp.CCVector2); }
  }
  [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Size=1)]
  public partial struct CCVertex2F {
    // Fields
    public static readonly CocosSharp.CCVertex2F ZeroVector;
     
    // Constructors
    public CCVertex2F(CocosSharp.CCVertex3F ver3) { throw new System.NotImplementedException(); }
    public CCVertex2F(float inx, float iny) { throw new System.NotImplementedException(); }
     
    // Properties
    public float X { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public float Y { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
     
    // Methods
  }
  [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Size=1)]
  public partial struct CCVertex3F {
    // Fields
    public static readonly CocosSharp.CCVertex3F Zero;
     
    // Constructors
    public CCVertex3F(float inx, float iny, float inz) { throw new System.NotImplementedException(); }
     
    // Properties
    public float X { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public float Y { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public float Z { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
     
    // Methods
    public override string ToString() { return default(string); }
  }
  public enum CCVerticalTextAlignment {
    // Fields
    Bottom = 2,
    Center = 1,
    Top = 0,
  }
  public partial class CCWaves : CocosSharp.CCLiquid {
    // Constructors
    public CCWaves(float duration, CocosSharp.CCGridSize gridSize, int waves=0, float amplitude=0f, bool horizontal=true, bool vertical=true) : base (default(float), default(CocosSharp.CCGridSize), default(int), default(float)) { }
     
    // Properties
    protected internal bool Horizontal { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } }
    protected internal bool Vertical { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } }
     
    // Methods
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCWaves3D : CocosSharp.CCLiquid {
    // Constructors
    public CCWaves3D(float duration, CocosSharp.CCGridSize gridSize, int waves=0, float amplitude=0f) : base (default(float), default(CocosSharp.CCGridSize), default(int), default(float)) { }
     
    // Methods
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCWaves3DState : CocosSharp.CCLiquidState {
    // Constructors
    public CCWaves3DState(CocosSharp.CCWaves3D action, CocosSharp.CCNode target) : base (default(CocosSharp.CCLiquid), default(CocosSharp.CCNode)) { }
     
    // Methods
    public override void Update(float time) { }
  }
  public partial class CCWavesState : CocosSharp.CCLiquidState {
    // Constructors
    public CCWavesState(CocosSharp.CCWaves action, CocosSharp.CCNode target) : base (default(CocosSharp.CCLiquid), default(CocosSharp.CCNode)) { }
     
    // Properties
    public bool Horizontal { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    public bool Vertical { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
     
    // Methods
    public override void Update(float time) { }
  }
  public partial class CCWavesTiles3D : CocosSharp.CCTiledGrid3DAction {
    // Constructors
    public CCWavesTiles3D(float duration, CocosSharp.CCGridSize gridSize, int waves=0, float amplitude=0f) : base (default(float)) { }
     
    // Properties
    protected internal int Waves { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } }
     
    // Methods
    protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
  }
  public partial class CCWavesTiles3DState : CocosSharp.CCTiledGrid3DActionState {
    // Constructors
    public CCWavesTiles3DState(CocosSharp.CCWavesTiles3D action, CocosSharp.CCNode target) : base (default(CocosSharp.CCTiledGrid3DAction), default(CocosSharp.CCNode)) { }
     
    // Properties
    public int Waves { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
     
    // Methods
    public override void Update(float time) { }
  }
  public enum CCZ_COMPRESSION {
    // Fields
    Bzip2 = 1,
    Gzip = 2,
    None = 3,
    Zlib = 0,
  }
  public partial class CCZHeader {
    // Fields
    public ushort compression_type;
    public uint len;
    public uint reserved;
    public System.Byte[] sig;
    public ushort version;
     
    // Constructors
    public CCZHeader() { }
  }
  [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
  public partial struct HSV {
    // Fields
    public float h;
    public float s;
    public float v;
  }
  public partial interface ICCActionTweenDelegate {
    // Methods
    void UpdateTweenAction(float value, string key);
  }
  public partial interface ICCBlendable {
    // Properties
    CocosSharp.CCBlendFunc BlendFunc { get; set; }
     
    // Methods
  }
  public partial interface ICCBMemberVariableAssigner {
    // Methods
    bool OnAssignCCBCustomProperty(object pTarget, string pMemberVariableName, CocosSharp.CCBValue pCCBValue);
    bool OnAssignCCBMemberVariable(object target, string memberVariableName, CocosSharp.CCNode node);
  }
  public partial interface ICCBScriptOwnerProtocol {
    // Methods
    CocosSharp.ICCBSelectorResolver CreateNew();
  }
  public partial interface ICCBSelectorResolver {
    // Methods
    System.Action<CocosSharp.CCNode> OnResolveCCBCCCallFuncSelector(object pTarget, string pSelectorName);
    System.Action<System.Object, CocosSharp.CCControlEvent> OnResolveCCBCCControlSelector(object target, string pSelectorName);
    System.Action<System.Object> OnResolveCCBCCMenuItemSelector(object target, string pSelectorName);
  }
  public partial interface ICCColor {
    // Properties
    CocosSharp.CCColor3B Color { get; set; }
    CocosSharp.CCColor3B DisplayedColor { get; }
    byte DisplayedOpacity { get; }
    bool IsColorCascaded { get; set; }
    bool IsColorModifiedByOpacity { get; set; }
    bool IsOpacityCascaded { get; set; }
    byte Opacity { get; set; }
     
    // Methods
    void UpdateDisplayedColor(CocosSharp.CCColor3B color);
    void UpdateDisplayedOpacity(byte opacity);
  }
  public partial interface ICCDirectorDelegate {
    // Methods
    void UpdateProjection();
  }
  public partial interface ICCFocusable {
    // Properties
    bool CanReceiveFocus { get; }
    bool HasFocus { get; set; }
     
    // Methods
  }
  public partial interface ICCIMEDelegate {
    // Methods
    bool AttachWithIME();
    bool CanAttachWithIME();
    bool CanDetachWithIME();
    void DeleteBackward();
    bool DetachWithIME();
    bool DidAttachWithIME();
    bool DidDetachWithIME();
    string GetContentText();
    void InsertText(string text, int len);
    void KeyboardDidHide(CocosSharp.CCIMEKeyboardNotificationInfo info);
    void KeyboardDidShow(CocosSharp.CCIMEKeyboardNotificationInfo info);
    void KeyboardWillHide(CocosSharp.CCIMEKeyboardNotificationInfo info);
    void KeyboardWillShow(CocosSharp.CCIMEKeyboardNotificationInfo info);
  }
  public partial interface ICCKeypadDelegate {
    // Methods
    void KeyBackClicked();
    void KeyMenuClicked();
  }
  public partial interface ICCNodeLoaderListener {
    // Methods
    void OnNodeLoaded(CocosSharp.CCNode node, CocosSharp.CCNodeLoader nodeLoader);
  }
  public partial interface ICCProjection {
    // Methods
    void UpdateProjection();
  }
  public partial interface ICCSAXDelegator {
    // Methods
    void EndElement(object ctx, string name);
    void StartElement(object ctx, string name, System.String[] atts);
    void TextHandler(object ctx, System.Byte[] ch, int len);
  }
  public partial class ICCScriptingEngine {
    // Constructors
    public ICCScriptingEngine() { }
     
    // Methods
    public virtual bool AddSearchPath(string pszPath) { return default(bool); }
    public virtual bool ExecuteCallFunc(string pszFuncName) { return default(bool); }
    public virtual bool ExecuteCallFunc0(string pszFuncName, object pObject) { return default(bool); }
    public virtual bool ExecuteCallFuncN(string pszFuncName, CocosSharp.CCNode node) { return default(bool); }
    public virtual bool ExecuteCallFuncNd(string pszFuncName, CocosSharp.CCNode node, object pData) { return default(bool); }
    public virtual int ExecuteFuction(string pszFuncName) { return default(int); }
    public virtual bool ExecuteSchedule(string pszFuncName, float t) { return default(bool); }
    public virtual bool ExecuteScriptFile(string pszFileName) { return default(bool); }
    public virtual bool ExecuteString(string pszCodes) { return default(bool); }
    public virtual bool ExecuteTouchesEvent(string pszFuncName, System.Collections.Generic.List<CocosSharp.CCTouch> pTouches) { return default(bool); }
    public virtual bool ExecuteTouchEvent(string pszFuncName, CocosSharp.CCTouch pTouch) { return default(bool); }
  }
  public partial interface ICCScrollViewDelegate {
    // Methods
    void ScrollViewDidScroll(CocosSharp.CCScrollView view);
    void ScrollViewDidZoom(CocosSharp.CCScrollView view);
  }
  public partial interface ICCSortableObject {
    // Properties
    int ObjectID { get; set; }
     
    // Methods
  }
  public partial interface ICCTableViewDataSource {
    // Methods
    int NumberOfCellsInTableView(CocosSharp.CCTableView table);
    CocosSharp.CCTableViewCell TableCellAtIndex(CocosSharp.CCTableView table, int idx);
    CocosSharp.CCSize TableCellSizeForIndex(CocosSharp.CCTableView table, int idx);
  }
  public partial interface ICCTableViewDelegate : CocosSharp.ICCScrollViewDelegate {
    // Methods
    void TableCellHighlight(CocosSharp.CCTableView table, CocosSharp.CCTableViewCell cell);
    void TableCellTouched(CocosSharp.CCTableView table, CocosSharp.CCTableViewCell cell);
    void TableCellUnhighlight(CocosSharp.CCTableView table, CocosSharp.CCTableViewCell cell);
    void TableCellWillRecycle(CocosSharp.CCTableView table, CocosSharp.CCTableViewCell cell);
  }
  public partial interface ICCTextContainer {
    // Properties
    string Text { get; set; }
     
    // Methods
  }
  public partial interface ICCTextFieldDelegate {
    // Methods
    bool onDraw(CocosSharp.CCTextFieldTTF sender);
    bool onTextFieldAttachWithIME(CocosSharp.CCTextFieldTTF sender);
    bool onTextFieldDeleteBackward(CocosSharp.CCTextFieldTTF sender, string delText, int nLen);
    bool onTextFieldDetachWithIME(CocosSharp.CCTextFieldTTF sender);
    bool onTextFieldInsertText(CocosSharp.CCTextFieldTTF sender, string text, int nLen);
  }
  public partial interface ICCTexture : CocosSharp.ICCBlendable {
    // Properties
    CocosSharp.CCTexture2D Texture { get; set; }
     
    // Methods
  }
  public partial interface ICCTransitionEaseScene {
    // Methods
    CocosSharp.CCFiniteTimeAction EaseAction(CocosSharp.CCActionInterval action);
  }
  public partial interface ICCUpdatable {
    // Methods
    void Update(float dt);
  }
  public partial class PlistArray : CocosSharp.PlistObject<System.Collections.Generic.List<CocosSharp.PlistObjectBase>>, System.Collections.Generic.IEnumerable<CocosSharp.PlistObjectBase>, System.Collections.IEnumerable {
    // Constructors
    public PlistArray() : base (default(System.Collections.Generic.List<CocosSharp.PlistObjectBase>)) { }
    public PlistArray(System.Collections.Generic.List<CocosSharp.PlistObjectBase> value) : base (default(System.Collections.Generic.List<CocosSharp.PlistObjectBase>)) { }
    public PlistArray(System.Collections.IEnumerable value) : base (default(System.Collections.Generic.List<CocosSharp.PlistObjectBase>)) { }
    public PlistArray(int capacity) : base (default(System.Collections.Generic.List<CocosSharp.PlistObjectBase>)) { }
     
    // Properties
    public override CocosSharp.PlistArray AsArray { get { return default(CocosSharp.PlistArray); } }
    public override System.Byte[] AsBinary { get { return default(System.Byte[]); } }
    public override bool AsBool { get { return default(bool); } }
    public override System.DateTime AsDate { get { return default(System.DateTime); } }
    public override CocosSharp.PlistDictionary AsDictionary { get { return default(CocosSharp.PlistDictionary); } }
    public override float AsFloat { get { return default(float); } }
    public override int AsInt { get { return default(int); } }
    public override string AsString { get { return default(string); } }
    public int Count { get { return default(int); } }
    public CocosSharp.PlistObjectBase this[int index] { get { return default(CocosSharp.PlistObjectBase); } set { } }
     
    // Methods
    public void Add(CocosSharp.PlistObjectBase value) { }
    public void Add(System.Collections.IDictionary value) { }
    public void Clear() { }
    public bool Contains(CocosSharp.PlistObjectBase value) { return default(bool); }
    public System.Collections.Generic.IEnumerator<CocosSharp.PlistObjectBase> GetEnumerator() { return default(System.Collections.Generic.IEnumerator<CocosSharp.PlistObjectBase>); }
    public bool Remove(CocosSharp.PlistObjectBase value) { return default(bool); }
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { return default(System.Collections.IEnumerator); }
    public override void Write(System.Xml.XmlWriter writer) { }
  }
  public partial class PlistBoolean : CocosSharp.PlistObject<System.Boolean> {
    // Constructors
    public PlistBoolean(bool value) : base (default(bool)) { }
     
    // Properties
    public override CocosSharp.PlistArray AsArray { get { return default(CocosSharp.PlistArray); } }
    public override System.Byte[] AsBinary { get { return default(System.Byte[]); } }
    public override bool AsBool { get { return default(bool); } }
    public override System.DateTime AsDate { get { return default(System.DateTime); } }
    public override CocosSharp.PlistDictionary AsDictionary { get { return default(CocosSharp.PlistDictionary); } }
    public override float AsFloat { get { return default(float); } }
    public override int AsInt { get { return default(int); } }
    public override string AsString { get { return default(string); } }
     
    // Methods
    public override void Write(System.Xml.XmlWriter writer) { }
  }
  public partial class PlistData : CocosSharp.PlistObject<System.Byte[]> {
    // Constructors
    public PlistData(System.Byte[] data) : base (default(System.Byte[])) { }
    public PlistData(string value) : base (default(System.Byte[])) { }
     
    // Properties
    public override CocosSharp.PlistArray AsArray { get { return default(CocosSharp.PlistArray); } }
    public override System.Byte[] AsBinary { get { return default(System.Byte[]); } }
    public override bool AsBool { get { return default(bool); } }
    public override System.DateTime AsDate { get { return default(System.DateTime); } }
    public override CocosSharp.PlistDictionary AsDictionary { get { return default(CocosSharp.PlistDictionary); } }
    public override float AsFloat { get { return default(float); } }
    public override int AsInt { get { return default(int); } }
    public override string AsString { get { return default(string); } }
     
    // Methods
    public override void Write(System.Xml.XmlWriter writer) { }
  }
  public partial class PlistDate : CocosSharp.PlistObject<System.DateTime> {
    // Constructors
    public PlistDate(System.DateTime value) : base (default(System.DateTime)) { }
     
    // Properties
    public override CocosSharp.PlistArray AsArray { get { return default(CocosSharp.PlistArray); } }
    public override System.Byte[] AsBinary { get { return default(System.Byte[]); } }
    public override bool AsBool { get { return default(bool); } }
    public override System.DateTime AsDate { get { return default(System.DateTime); } }
    public override CocosSharp.PlistDictionary AsDictionary { get { return default(CocosSharp.PlistDictionary); } }
    public override float AsFloat { get { return default(float); } }
    public override int AsInt { get { return default(int); } }
    public override string AsString { get { return default(string); } }
     
    // Methods
    public override void Write(System.Xml.XmlWriter writer) { }
  }
  public partial class PlistDictionary : CocosSharp.PlistObjectBase, System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<System.String, CocosSharp.PlistObjectBase>>, System.Collections.IEnumerable {
    // Constructors
    public PlistDictionary() { }
    public PlistDictionary(bool keepOrder) { }
    public PlistDictionary(System.Collections.Generic.Dictionary<System.String, CocosSharp.PlistObjectBase> value) { }
    public PlistDictionary(System.Collections.Generic.Dictionary<System.String, CocosSharp.PlistObjectBase> value, bool keepOrder) { }
    public PlistDictionary(System.Collections.IDictionary value) { }
    public PlistDictionary(System.Collections.IDictionary value, bool keepOrder) { }
     
    // Properties
    public override CocosSharp.PlistArray AsArray { get { return default(CocosSharp.PlistArray); } }
    public override System.Byte[] AsBinary { get { return default(System.Byte[]); } }
    public override bool AsBool { get { return default(bool); } }
    public override System.DateTime AsDate { get { return default(System.DateTime); } }
    public override CocosSharp.PlistDictionary AsDictionary { get { return default(CocosSharp.PlistDictionary); } }
    public override float AsFloat { get { return default(float); } }
    public override int AsInt { get { return default(int); } }
    public override string AsString { get { return default(string); } }
    public int Count { get { return default(int); } }
    public CocosSharp.PlistObjectBase this[string key] { get { return default(CocosSharp.PlistObjectBase); } set { } }
     
    // Methods
    public void Add(string key, CocosSharp.PlistObjectBase value) { }
    public void Clear() { }
    public bool ContainsKey(string key) { return default(bool); }
    public System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<System.String, CocosSharp.PlistObjectBase>> GetEnumerator() { return default(System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<System.String, CocosSharp.PlistObjectBase>>); }
    public bool Remove(string key) { return default(bool); }
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { return default(System.Collections.IEnumerator); }
    public CocosSharp.PlistObjectBase TryGetValue(string key) { return default(CocosSharp.PlistObjectBase); }
    public override void Write(System.Xml.XmlWriter writer) { }
  }
  public partial class PlistDocument : CocosSharp.PlistObjectBase {
    // Constructors
    public PlistDocument() { }
    public PlistDocument(CocosSharp.PlistObjectBase root) { }
    public PlistDocument(System.IO.Stream data) { }
    public PlistDocument(string data) { }
     
    // Properties
    public override CocosSharp.PlistArray AsArray { get { return default(CocosSharp.PlistArray); } }
    public override System.Byte[] AsBinary { get { return default(System.Byte[]); } }
    public override bool AsBool { get { return default(bool); } }
    public override System.DateTime AsDate { get { return default(System.DateTime); } }
    public override CocosSharp.PlistDictionary AsDictionary { get { return default(CocosSharp.PlistDictionary); } }
    public override float AsFloat { get { return default(float); } }
    public override int AsInt { get { return default(int); } }
    public override string AsString { get { return default(string); } }
    public CocosSharp.PlistObjectBase Root { get { return default(CocosSharp.PlistObjectBase); } set { } }
     
    // Methods
    public void LoadFromXml(string data) { }
    public void LoadFromXml(System.Xml.XmlReader reader) { }
    public void LoadFromXmlFile(System.IO.Stream data) { }
    public void LoadFromXmlFile(string path) { }
    public override void Write(System.Xml.XmlWriter writer) { }
    public void WriteToFile(string filename) { }
  }
  public partial class PlistInteger : CocosSharp.PlistObject<System.Int32> {
    // Constructors
    public PlistInteger(int value) : base (default(int)) { }
     
    // Properties
    public override CocosSharp.PlistArray AsArray { get { return default(CocosSharp.PlistArray); } }
    public override System.Byte[] AsBinary { get { return default(System.Byte[]); } }
    public override bool AsBool { get { return default(bool); } }
    public override System.DateTime AsDate { get { return default(System.DateTime); } }
    public override CocosSharp.PlistDictionary AsDictionary { get { return default(CocosSharp.PlistDictionary); } }
    public override float AsFloat { get { return default(float); } }
    public override int AsInt { get { return default(int); } }
    public override string AsString { get { return default(string); } }
     
    // Methods
    public override void Write(System.Xml.XmlWriter writer) { }
  }
  public partial class PlistNull : CocosSharp.PlistObject<System.Nullable<System.Int32>> {
    // Constructors
    public PlistNull() : base (default(System.Nullable<System.Int32>)) { }
     
    // Properties
    public override CocosSharp.PlistArray AsArray { get { return default(CocosSharp.PlistArray); } }
    public override System.Byte[] AsBinary { get { return default(System.Byte[]); } }
    public override bool AsBool { get { return default(bool); } }
    public override System.DateTime AsDate { get { return default(System.DateTime); } }
    public override CocosSharp.PlistDictionary AsDictionary { get { return default(CocosSharp.PlistDictionary); } }
    public override float AsFloat { get { return default(float); } }
    public override int AsInt { get { return default(int); } }
    public override string AsString { get { return default(string); } }
     
    // Methods
    public override void Write(System.Xml.XmlWriter writer) { }
  }
  public abstract partial class PlistObject<T> : CocosSharp.PlistObjectBase {
    // Constructors
    public PlistObject(T value) { }
     
    // Properties
    public virtual T Value { get { return default(T); } set { } }
     
    // Methods
  }
  public abstract partial class PlistObjectBase {
    // Constructors
    protected PlistObjectBase() { }
     
    // Properties
    public abstract CocosSharp.PlistArray AsArray { get; }
    public abstract System.Byte[] AsBinary { get; }
    public abstract bool AsBool { get; }
    public abstract System.DateTime AsDate { get; }
    public abstract CocosSharp.PlistDictionary AsDictionary { get; }
    public abstract float AsFloat { get; }
    public abstract int AsInt { get; }
    public abstract string AsString { get; }
     
    // Methods
    protected static CocosSharp.PlistObjectBase ObjectToPlistObject(object value) { return default(CocosSharp.PlistObjectBase); }
    public static implicit operator CocosSharp.PlistObjectBase (bool value) { return default(CocosSharp.PlistObjectBase); }
    public static implicit operator CocosSharp.PlistObjectBase (int value) { return default(CocosSharp.PlistObjectBase); }
    public static implicit operator CocosSharp.PlistObjectBase (System.Object[] value) { return default(CocosSharp.PlistObjectBase); }
    public static implicit operator CocosSharp.PlistObjectBase (float value) { return default(CocosSharp.PlistObjectBase); }
    public static implicit operator CocosSharp.PlistObjectBase (string value) { return default(CocosSharp.PlistObjectBase); }
    public abstract void Write(System.Xml.XmlWriter writer);
  }
  public partial class PlistReal : CocosSharp.PlistObject<System.Single> {
    // Constructors
    public PlistReal(float value) : base (default(float)) { }
     
    // Properties
    public override CocosSharp.PlistArray AsArray { get { return default(CocosSharp.PlistArray); } }
    public override System.Byte[] AsBinary { get { return default(System.Byte[]); } }
    public override bool AsBool { get { return default(bool); } }
    public override System.DateTime AsDate { get { return default(System.DateTime); } }
    public override CocosSharp.PlistDictionary AsDictionary { get { return default(CocosSharp.PlistDictionary); } }
    public override float AsFloat { get { return default(float); } }
    public override int AsInt { get { return default(int); } }
    public override string AsString { get { return default(string); } }
     
    // Methods
    public override void Write(System.Xml.XmlWriter writer) { }
  }
  public partial class PlistString : CocosSharp.PlistObject<System.String> {
    // Constructors
    public PlistString(string value) : base (default(string)) { }
     
    // Properties
    public override CocosSharp.PlistArray AsArray { get { return default(CocosSharp.PlistArray); } }
    public override System.Byte[] AsBinary { get { return default(System.Byte[]); } }
    public override bool AsBool { get { return default(bool); } }
    public override System.DateTime AsDate { get { return default(System.DateTime); } }
    public override CocosSharp.PlistDictionary AsDictionary { get { return default(CocosSharp.PlistDictionary); } }
    public override float AsFloat { get { return default(float); } }
    public override int AsInt { get { return default(int); } }
    public override string AsString { get { return default(string); } }
     
    // Methods
    public override void Write(System.Xml.XmlWriter writer) { }
  }
  public partial class ZipUtils {
    // Constructors
    public ZipUtils() { }
     
    // Methods
    public static int InflateCCZFile(string filename, System.Byte[] parameterout) { return default(int); }
    public static int InflateGZipFile(char filename, System.Byte[] parameterout) { return default(int); }
    public static int InflateMemory(System.Byte[] parameterin, uint inLength, System.Byte[] parameterout) { return default(int); }
    public static int InflateMemoryWithHint(System.Byte[] parameterin, uint inLength, System.Byte[] parameterout, int outLenghtHint) { return default(int); }
  }
}