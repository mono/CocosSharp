namespace CocosDenshion {
    public partial class CCEffectPlayer : CocosDenshion.CCEffectPlayerCore, System.IDisposable {
        public CCEffectPlayer() { }
        public override bool Playing { get { return default(bool); } }
        public override float Volume { get { return default(float); } set { } }
        public override void Close() { }
        public void Dispose() { }
        protected virtual void Dispose(bool disposing) { }
        protected override void DisposeManagedResources() { }
        public override void Open(string fileName, int soundId) { }
        public override void Pause() { }
        public override void Play(bool loop=false) { }
        public override void Resume() { }
        public override void Rewind() { }
        public override void Stop() { }
    }
    public abstract partial class CCEffectPlayerCore {
        protected CCEffectPlayerCore() { }
        public abstract bool Playing { get; }
        public int SoundID { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } }
        public abstract float Volume { get; set; }
        public virtual void Close() { }
        protected abstract void DisposeManagedResources();
        public virtual void Open(string filename, int soundId) { }
        public abstract void Pause();
        public abstract void Play(bool loop=false);
        public abstract void Resume();
        public abstract void Rewind();
        public abstract void Stop();
    }
    public partial class CCMusicPlayer : CocosDenshion.CCMusicPlayerCore, System.IDisposable {
        public CCMusicPlayer() { }
        public override bool Playing { get { return default(bool); } }
        public override bool PlayingMySong { get { return default(bool); } }
        public override float Volume { get { return default(float); } set { } }
        public override void Close() { }
        public void Dispose() { }
        protected virtual void Dispose(bool disposing) { }
        protected override void DisposeManagedResources() { }
        ~CCMusicPlayer() { }
        public override void Open(string fileName, int soundId) { }
        public override void Pause() { }
        public override void Play(bool loop=false) { }
        public override void Resume() { }
        public override void Rewind() { }
        public override void Stop() { }
    }
    public abstract partial class CCMusicPlayerCore {
        protected CCMusicPlayerCore() { }
        public abstract bool Playing { get; }
        public abstract bool PlayingMySong { get; }
        public int SoundID { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } }
        public abstract float Volume { get; set; }
        public virtual void Close() { }
        protected abstract void DisposeManagedResources();
        public virtual void Open(string fileName, int soundId) { }
        public abstract void Pause();
        public abstract void Play(bool loop=false);
        public virtual void RestoreMediaState() { }
        public abstract void Resume();
        public abstract void Rewind();
        public virtual void SaveMediaState() { }
        public abstract void Stop();
    }
    public partial class CCSimpleAudioEngine {
        public CCSimpleAudioEngine() { }
        public bool BackgroundMusicPlaying { get { return default(bool); } }
        public float BackgroundMusicVolume { get { return default(float); } set { } }
        public float EffectsVolume { get { return default(float); } set { } }
        public static CocosDenshion.CCSimpleAudioEngine SharedEngine { get { return default(CocosDenshion.CCSimpleAudioEngine); } }
        public static System.Collections.Generic.Dictionary<System.Int32, CocosDenshion.CCEffectPlayer> SharedList { get { return default(System.Collections.Generic.Dictionary<System.Int32, CocosDenshion.CCEffectPlayer>); } }
        public void End() { }
        public static string FullPath(string path) { return default(string); }
        public void PauseBackgroundMusic() { }
        public void PauseEffect(int fxid) { }
        public void PlayBackgroundMusic(string filename, bool loop=false) { }
        public int PlayEffect(int fxid) { return default(int); }
        public int PlayEffect(int fxid, bool bLoop) { return default(int); }
        public int PlayEffect(string filename, bool loop=false) { return default(int); }
        public void PreloadBackgroundMusic(string filename) { }
        public void PreloadEffect(string filename) { }
        public void RestoreMediaState() { }
        public void ResumeBackgroundMusic() { }
        public void RewindBackgroundMusic() { }
        public void SaveMediaState() { }
        public void StopAllEffects() { }
        public void StopAllLoopingEffects() { }
        public void StopBackgroundMusic(bool releaseData=false) { }
        public void StopEffect(int soundId) { }
        public void UnloadEffect(string filename) { }
        public bool WillPlayBackgroundMusic() { return default(bool); }
    }
}
namespace CocosSharp {
    public partial class CCAccelAmplitude : CocosSharp.CCFiniteTimeAction {
        public CCAccelAmplitude(CocosSharp.CCAmplitudeAction pAction, float duration, float accelRate=1f) { }
        protected internal CocosSharp.CCAmplitudeAction OtherAction { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCAmplitudeAction); } }
        public float Rate { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } }
        public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public partial class CCAccelDeccelAmplitude : CocosSharp.CCAccelAmplitude {
        public CCAccelDeccelAmplitude(CocosSharp.CCAmplitudeAction pAction, float duration, float accDeccRate=1f) : base (default(CocosSharp.CCAmplitudeAction), default(float), default(float)) { }
        public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public partial class CCAcceleration {
        public double TimeStamp;
        public double X;
        public double Y;
        public double Z;
        public CCAcceleration() { }
    }
    public partial class CCAccelerometer {
        public CCAccelerometer(CocosSharp.CCWindow window) { }
        public bool Enabled { get { return default(bool); } set { } }
        public CocosSharp.CCWindow Window { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCWindow); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public void Update() { }
    }
    public abstract partial class CCAction {
        protected CCAction() { }
        public int Tag { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        protected internal virtual CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public partial class CCActionCamera : CocosSharp.CCFiniteTimeAction {
        protected CCActionCamera(float duration) { }
        public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public partial class CCActionEase : CocosSharp.CCFiniteTimeAction {
        protected CCActionEase() { }
        public CCActionEase(CocosSharp.CCFiniteTimeAction pAction) { }
        protected internal CocosSharp.CCFiniteTimeAction InnerAction { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCFiniteTimeAction); } }
        public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public partial class CCActionInstant : CocosSharp.CCFiniteTimeAction {
        protected CCActionInstant() { }
        public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public partial class CCActionInstantState : CocosSharp.CCFiniteTimeActionState {
        public CCActionInstantState(CocosSharp.CCActionInstant action, CocosSharp.CCNode target) : base (default(CocosSharp.CCFiniteTimeAction), default(CocosSharp.CCNode)) { }
        public override bool IsDone { get { return default(bool); } }
        protected internal override void Step(float dt) { }
        public override void Update(float time) { }
    }
    public partial class CCActionManager : CocosSharp.ICCUpdatable, System.IDisposable {
        public CCActionManager() { }
        public void Dispose() { }
        protected virtual void Dispose(bool disposing) { }
        ~CCActionManager() { }
        public CocosSharp.CCAction GetAction(int tag, CocosSharp.CCNode target) { return default(CocosSharp.CCAction); }
        public int NumberOfRunningActionsInTarget(CocosSharp.CCNode target) { return default(int); }
        public System.Collections.Generic.List<System.Object> PauseAllRunningActions() { return default(System.Collections.Generic.List<System.Object>); }
        public void PauseTarget(object target) { }
        public void RemoveAction(int tag, CocosSharp.CCNode target) { }
        public void RemoveAllActions() { }
        public void RemoveAllActionsFromTarget(CocosSharp.CCNode target) { }
        public void ResumeTarget(object target) { }
        public void ResumeTargets(System.Collections.Generic.List<System.Object> targetsToResume) { }
        public void Update(float dt) { }
    }
    public partial class CCActionState {
        public CCActionState(CocosSharp.CCAction action, CocosSharp.CCNode target) { }
        public CocosSharp.CCAction Action { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCAction); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]protected set { } }
        public virtual bool IsDone { get { return default(bool); } }
        protected CocosSharp.CCLayer Layer { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCLayer); } }
        public CocosSharp.CCNode OriginalTarget { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCNode); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]protected set { } }
        protected CocosSharp.CCScene Scene { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCScene); } }
        public CocosSharp.CCNode Target { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCNode); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]protected set { } }
        protected internal virtual void Step(float dt) { }
        protected internal virtual void Stop() { }
        public virtual void Update(float time) { }
    }
    public enum CCActionTag {
        Invalid = -1,
    }
    public partial class CCActionTween : CocosSharp.CCFiniteTimeAction {
        public CCActionTween(float aDuration, string key, float from, float to) { }
        public CCActionTween(float aDuration, string key, float from, float to, System.Action<System.Single, System.String> tweenAction) { }
        public float From { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } }
        public string Key { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(string); } }
        public float To { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } }
        public System.Action<System.Single, System.String> TweenAction { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Action<System.Single, System.String>); } }
        public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public partial struct CCAffineTransform {
        public float A;
        public float B;
        public float C;
        public float D;
        public static readonly CocosSharp.CCAffineTransform Identity;
        public float Tx;
        public float Ty;
        public CCAffineTransform(float a, float b, float c, float d, float tx, float ty) { throw new System.NotImplementedException(); }
        public float this[float scaleX, float scaleY, float angle] { set { } }
        public float Rotation { set { } }
        public float RotationX { get { return default(float); } }
        public float RotationY { get { return default(float); } }
        public float Scale { set { } }
        public float ScaleX { get { return default(float); } set { } }
        public float ScaleY { get { return default(float); } set { } }
        public void Concat(CocosSharp.CCAffineTransform m) { }
        public static CocosSharp.CCAffineTransform Concat(CocosSharp.CCAffineTransform t1, CocosSharp.CCAffineTransform t2) { return default(CocosSharp.CCAffineTransform); }
        public void Concat(ref CocosSharp.CCAffineTransform m) { }
        public static bool Equal(CocosSharp.CCAffineTransform t1, CocosSharp.CCAffineTransform t2) { return default(bool); }
        public bool Equals(ref CocosSharp.CCAffineTransform t) { return default(bool); }
        public override bool Equals(object obj) { return default(bool); }
        public override int GetHashCode() { return default(int); }
        public static CocosSharp.CCAffineTransform Invert(CocosSharp.CCAffineTransform t) { return default(CocosSharp.CCAffineTransform); }
        public void Lerp(CocosSharp.CCAffineTransform m1, CocosSharp.CCAffineTransform m2, float t) { }
        public static void LerpCopy(ref CocosSharp.CCAffineTransform m1, ref CocosSharp.CCAffineTransform m2, float t, out CocosSharp.CCAffineTransform res) { res = default(CocosSharp.CCAffineTransform); }
        public static CocosSharp.CCAffineTransform operator +(CocosSharp.CCAffineTransform affineTransform1, CocosSharp.CCAffineTransform affineTransform2) { return default(CocosSharp.CCAffineTransform); }
        public static CocosSharp.CCAffineTransform operator /(CocosSharp.CCAffineTransform affineTransform1, CocosSharp.CCAffineTransform affineTransform2) { return default(CocosSharp.CCAffineTransform); }
        public static CocosSharp.CCAffineTransform operator /(CocosSharp.CCAffineTransform affineTransform, float divider) { return default(CocosSharp.CCAffineTransform); }
        public static bool operator ==(CocosSharp.CCAffineTransform affineTransform1, CocosSharp.CCAffineTransform affineTransform2) { return default(bool); }
        public static bool operator !=(CocosSharp.CCAffineTransform affineTransform1, CocosSharp.CCAffineTransform affineTransform2) { return default(bool); }
        public static CocosSharp.CCAffineTransform operator *(CocosSharp.CCAffineTransform affinematrix1, CocosSharp.CCAffineTransform affinematrix2) { return default(CocosSharp.CCAffineTransform); }
        public static CocosSharp.CCAffineTransform operator -(CocosSharp.CCAffineTransform affineTransform1, CocosSharp.CCAffineTransform affineTransform2) { return default(CocosSharp.CCAffineTransform); }
        public static CocosSharp.CCAffineTransform operator -(CocosSharp.CCAffineTransform affineTransform1) { return default(CocosSharp.CCAffineTransform); }
        public static CocosSharp.CCAffineTransform Rotate(CocosSharp.CCAffineTransform t, float anAngle) { return default(CocosSharp.CCAffineTransform); }
        public static CocosSharp.CCAffineTransform ScaleCopy(CocosSharp.CCAffineTransform t, float sx, float sy) { return default(CocosSharp.CCAffineTransform); }
        public CocosSharp.CCPoint Transform(CocosSharp.CCPoint point) { return default(CocosSharp.CCPoint); }
        public static CocosSharp.CCPoint Transform(CocosSharp.CCPoint point, CocosSharp.CCAffineTransform t) { return default(CocosSharp.CCPoint); }
        public void Transform(ref CocosSharp.CCPoint point) { }
        public CocosSharp.CCRect Transform(CocosSharp.CCRect rect) { return default(CocosSharp.CCRect); }
        public static CocosSharp.CCRect Transform(CocosSharp.CCRect rect, CocosSharp.CCAffineTransform anAffineTransform) { return default(CocosSharp.CCRect); }
        public void Transform(ref CocosSharp.CCRect rect) { }
        public static CocosSharp.CCSize Transform(CocosSharp.CCSize size, CocosSharp.CCAffineTransform t) { return default(CocosSharp.CCSize); }
        public void Transform(ref CocosSharp.CCV3F_C4B_T2F quadPoint) { }
        public CocosSharp.CCV3F_C4B_T2F_Quad Transform(CocosSharp.CCV3F_C4B_T2F_Quad quad) { return default(CocosSharp.CCV3F_C4B_T2F_Quad); }
        public void Transform(ref int x, ref int y) { }
        public void Transform(float x, float y, out float xresult, out float yresult) { xresult = default(float); yresult = default(float); }
        public void Transform(ref float x, ref float y) { }
        public static CocosSharp.CCAffineTransform Translate(CocosSharp.CCAffineTransform t, float tx, float ty) { return default(CocosSharp.CCAffineTransform); }
    }
    public abstract partial class CCAmplitudeAction : CocosSharp.CCFiniteTimeAction {
        public CCAmplitudeAction(float duration, float amplitude=0f) { }
        public float Amplitude { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } }
    }
    public partial class CCAnimate : CocosSharp.CCFiniteTimeAction {
        public CCAnimate(CocosSharp.CCAnimation pAnimation) { }
        public CocosSharp.CCAnimation Animation { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCAnimation); } }
        public System.Collections.Generic.List<System.Single> SplitTimes { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Collections.Generic.List<System.Single>); } }
        public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public partial class CCAnimation {
        public CCAnimation() { }
        protected CCAnimation(CocosSharp.CCAnimation animation) { }
        public CCAnimation(CocosSharp.CCSpriteSheet cs, float delay) { }
        public CCAnimation(CocosSharp.CCSpriteSheet cs, System.String[] frames, float delay) { }
        public CCAnimation(System.Collections.Generic.List<CocosSharp.CCAnimationFrame> arrayOfAnimationFrameNames, float delayPerUnit, uint loops) { }
        public CCAnimation(System.Collections.Generic.List<CocosSharp.CCSpriteFrame> frames, float delay) { }
        public float DelayPerUnit { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public float Duration { get { return default(float); } }
        public System.Collections.Generic.List<CocosSharp.CCAnimationFrame> Frames { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Collections.Generic.List<CocosSharp.CCAnimationFrame>); } }
        public uint Loops { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(uint); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public bool RestoreOriginalFrame { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public float TotalDelayUnits { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } }
        public void AddSpriteFrame(CocosSharp.CCSprite sprite) { }
        public void AddSpriteFrame(CocosSharp.CCSpriteFrame frame) { }
        public void AddSpriteFrame(CocosSharp.CCTexture2D texture, CocosSharp.CCRect textureRect, System.Nullable<CocosSharp.CCSize> contentSize=null) { }
        public void AddSpriteFrame(string filename, System.Nullable<CocosSharp.CCSize> contentSize=null) { }
        public CocosSharp.CCAnimation Copy() { return default(CocosSharp.CCAnimation); }
    }
    public partial class CCAnimationCache {
        public CCAnimationCache() { }
        public CocosSharp.CCAnimation this[string index] { get { return default(CocosSharp.CCAnimation); } set { } }
        public static CocosSharp.CCAnimationCache SharedAnimationCache { get { return default(CocosSharp.CCAnimationCache); } }
        public void AddAnimation(CocosSharp.CCAnimation animation, string name) { }
        public void AddAnimations(string plistFilename) { }
        public static void PurgeSharedAnimationCached() { }
        public void RemoveAnimation(string animationName) { }
    }
    public partial class CCAnimationFrame {
        protected CCAnimationFrame(CocosSharp.CCAnimationFrame animFrame) { }
        public CCAnimationFrame(CocosSharp.CCSpriteFrame spriteFrame, float delayUnits, object userInfo) { }
        public float DelayUnits { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } }
        public CocosSharp.CCSpriteFrame SpriteFrame { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCSpriteFrame); } }
        public object UserInfo { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(object); } }
        public CocosSharp.CCAnimationFrame Copy() { return default(CocosSharp.CCAnimationFrame); }
    }
    public partial class CCApplication : Microsoft.Xna.Framework.DrawableGameComponent {
        public CCApplication(bool isFullScreen=true, System.Nullable<CocosSharp.CCSize> mainWindowSizeInPixels=null) : base (default(Microsoft.Xna.Framework.Game)) { }
        public CocosSharp.CCActionManager ActionManager { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCActionManager); } }
        public virtual double AnimationInterval { get { return default(double); } set { } }
        public CocosSharp.CCApplicationDelegate ApplicationDelegate { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCApplicationDelegate); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public string ContentRootDirectory { get { return default(string); } set { } }
        public System.Collections.Generic.List<System.String> ContentSearchPaths { get { return default(System.Collections.Generic.List<System.String>); } set { } }
        public System.Collections.Generic.List<System.String> ContentSearchResolutionOrder { get { return default(System.Collections.Generic.List<System.String>); } set { } }
        public CocosSharp.CCDisplayOrientation CurrentOrientation { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCDisplayOrientation); } }
        public bool HandleMediaStateAutomatically { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public CocosSharp.CCWindow MainWindow { get { return default(CocosSharp.CCWindow); } }
        public bool Paused { get { return default(bool); } }
        public bool PreferMultiSampling { get { return default(bool); } set { } }
        public CocosSharp.CCScheduler Scheduler { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCScheduler); } }
        public void ClearTouches() { }
        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime) { }
        public void ExitGame() { }
        public override void Initialize() { }
        public virtual bool InitInstance() { return default(bool); }
        protected override void LoadContent() { }
        public void PauseGame() { }
        public void PurgeAllCachedData() { }
        public void ResumeGame() { }
        public void StartGame() { }
        public void ToggleFullScreen() { }
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime) { }
    }
    public partial class CCApplicationDelegate {
        public CCApplicationDelegate() { }
        public virtual void ApplicationDidEnterBackground(CocosSharp.CCApplication application) { }
        public virtual void ApplicationDidFinishLaunching(CocosSharp.CCApplication application, CocosSharp.CCWindow mainWindow) { }
        public virtual void ApplicationWillEnterForeground(CocosSharp.CCApplication application) { }
    }
    public partial class CCAtlasNode : CocosSharp.CCNode, CocosSharp.ICCBlendable, CocosSharp.ICCTexture {
        public CCAtlasNode(CocosSharp.CCTexture2D texture, int tileWidth, int tileHeight, int itemsToRender) { }
        public CCAtlasNode(string tile, int tileWidth, int tileHeight, int itemsToRender) { }
        public CocosSharp.CCBlendFunc BlendFunc { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCBlendFunc); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public override CocosSharp.CCColor3B Color { get { return default(CocosSharp.CCColor3B); } set { } }
        protected CocosSharp.CCColor3B ColorUnmodified { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCColor3B); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
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
        protected override void Draw() { }
        public virtual void UpdateAtlasValues() { }
    }
    public partial class CCBezierBy : CocosSharp.CCFiniteTimeAction {
        public CCBezierBy(float t, CocosSharp.CCBezierConfig config) { }
        public CocosSharp.CCBezierConfig BezierConfig { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCBezierConfig); } }
        public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public partial struct CCBezierConfig {
        public CocosSharp.CCPoint ControlPoint1;
        public CocosSharp.CCPoint ControlPoint2;
        public CocosSharp.CCPoint EndPosition;
    }
    public partial class CCBezierTo : CocosSharp.CCBezierBy {
        public CCBezierTo(float t, CocosSharp.CCBezierConfig c) : base (default(float), default(CocosSharp.CCBezierConfig)) { }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Size=1)]
    public partial struct CCBlendFunc {
        public static readonly CocosSharp.CCBlendFunc Additive;
        public static readonly CocosSharp.CCBlendFunc AlphaBlend;
        public static readonly CocosSharp.CCBlendFunc NonPremultiplied;
        public static readonly CocosSharp.CCBlendFunc Opaque;
        public CCBlendFunc(int src, int dst) { throw new System.NotImplementedException(); }
        public int Destination { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public int Source { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public bool Equals(CocosSharp.CCBlendFunc other) { return default(bool); }
        public override bool Equals(object obj) { return default(bool); }
        public override int GetHashCode() { return default(int); }
        public static bool operator ==(CocosSharp.CCBlendFunc b1, CocosSharp.CCBlendFunc b2) { return default(bool); }
        public static bool operator !=(CocosSharp.CCBlendFunc b1, CocosSharp.CCBlendFunc b2) { return default(bool); }
    }
    public partial class CCBlink : CocosSharp.CCFiniteTimeAction {
        public CCBlink(float duration, uint uBlinks) { }
        public uint Times { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(uint); } }
        public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Size=1)]
    public partial struct CCBoundingBoxI {
        public static readonly CocosSharp.CCBoundingBoxI Null;
        public static readonly CocosSharp.CCBoundingBoxI Zero;
        public CCBoundingBoxI(int minX, int minY, int maxX, int maxY) { throw new System.NotImplementedException(); }
        public int MaxX { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public int MaxY { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public int MinX { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public int MinY { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public CocosSharp.CCSizeI Size { get { return default(CocosSharp.CCSizeI); } }
        public void ExpandToCircle(ref CocosSharp.CCPointI point, int radius) { }
        public void ExpandToCircle(int x, int y, int radius) { }
        public void ExpandToPoint(ref CocosSharp.CCPointI point) { }
        public void ExpandToPoint(int x, int y) { }
        public void ExpandToRect(ref CocosSharp.CCBoundingBoxI rect) { }
        public bool Intersects(ref CocosSharp.CCBoundingBoxI rect) { return default(bool); }
        public static implicit operator CocosSharp.CCRect (CocosSharp.CCBoundingBoxI box) { return default(CocosSharp.CCRect); }
        public void SetLerp(CocosSharp.CCBoundingBoxI a, CocosSharp.CCBoundingBoxI b, float ratio) { }
        public CocosSharp.CCBoundingBoxI Transform(CocosSharp.CCAffineTransform matrix) { return default(CocosSharp.CCBoundingBoxI); }
    }
    public enum CCBufferUsage {
        None = 0,
        WriteOnly = 1,
    }
    public partial class CCCallFunc : CocosSharp.CCActionInstant {
        public CCCallFunc() { }
        public CCCallFunc(System.Action selector) { }
        public System.Action CallFunction { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Action); } }
        public string ScriptFuncName { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(string); } }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public partial class CCCallFuncN : CocosSharp.CCCallFunc {
        public CCCallFuncN() { }
        public CCCallFuncN(System.Action<CocosSharp.CCNode> selector) { }
        public System.Action<CocosSharp.CCNode> CallFunctionN { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Action<CocosSharp.CCNode>); } }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public partial class CCCallFuncND : CocosSharp.CCCallFuncN {
        public CCCallFuncND(System.Action<CocosSharp.CCNode, System.Object> selector, object d) { }
        public System.Action<CocosSharp.CCNode, System.Object> CallFunctionND { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Action<CocosSharp.CCNode, System.Object>); } }
        public object Data { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(object); } }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public partial class CCCallFuncO : CocosSharp.CCCallFunc {
        public CCCallFuncO() { }
        public CCCallFuncO(System.Action<System.Object> selector, object pObject) { }
        public System.Action<System.Object> CallFunctionO { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Action<System.Object>); } }
        public object Object { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(object); } }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public partial class CCCamera {
        public CCCamera(CocosSharp.CCSize orthographicViewSizeWorldspaceIn) { }
        public CCCamera(CocosSharp.CCSize orthographicViewSizeWorldspaceIn, CocosSharp.CCPoint3 cameraCenterPositionWorldspaceIn, CocosSharp.CCPoint3 targetInWorldspaceIn) { }
        public CCCamera(float perspectiveFieldOfViewIn, float perspectiveAspectRatioIn, CocosSharp.CCPoint3 cameraCenterPositionWorldspaceIn, CocosSharp.CCPoint3 targetInWorldspaceIn) { }
        public CocosSharp.CCPoint3 CenterInWorldspace { get { return default(CocosSharp.CCPoint3); } set { } }
        public CocosSharp.CCPoint NearAndFarOrthographicZClipping { get { return default(CocosSharp.CCPoint); } set { } }
        public CocosSharp.CCPoint NearAndFarPerspectiveClipping { get { return default(CocosSharp.CCPoint); } set { } }
        public CocosSharp.CCSize OrthographicViewSizeWorldspace { get { return default(CocosSharp.CCSize); } set { } }
        public float PerspectiveAspectRatio { get { return default(float); } set { } }
        public float PerspectiveFieldOfView { get { return default(float); } set { } }
        public CocosSharp.CCCameraProjection Projection { get { return default(CocosSharp.CCCameraProjection); } set { } }
        public CocosSharp.CCPoint3 TargetInWorldspace { get { return default(CocosSharp.CCPoint3); } set { } }
        public CocosSharp.CCPoint3 UpDirection { get { return default(CocosSharp.CCPoint3); } set { } }
    }
    public enum CCCameraProjection {
        Default = 0,
        Projection2D = 0,
        Projection3D = 1,
    }
    public partial class CCCardinalSplineBy : CocosSharp.CCCardinalSplineTo {
        public CCCardinalSplineBy(float duration, System.Collections.Generic.List<CocosSharp.CCPoint> points, float tension) : base (default(float), default(System.Collections.Generic.List<CocosSharp.CCPoint>), default(float)) { }
        public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public partial class CCCardinalSplineTo : CocosSharp.CCFiniteTimeAction {
        public CCCardinalSplineTo(float duration, System.Collections.Generic.List<CocosSharp.CCPoint> points, float tension) { }
        public System.Collections.Generic.List<CocosSharp.CCPoint> Points { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Collections.Generic.List<CocosSharp.CCPoint>); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]protected set { } }
        public float Tension { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]protected set { } }
        public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public partial class CCCatmullRomBy : CocosSharp.CCCardinalSplineBy {
        public CCCatmullRomBy(float dt, System.Collections.Generic.List<CocosSharp.CCPoint> points) : base (default(float), default(System.Collections.Generic.List<CocosSharp.CCPoint>), default(float)) { }
    }
    public partial class CCCatmullRomTo : CocosSharp.CCCardinalSplineTo {
        public CCCatmullRomTo(float dt, System.Collections.Generic.List<CocosSharp.CCPoint> points) : base (default(float), default(System.Collections.Generic.List<CocosSharp.CCPoint>), default(float)) { }
    }
    public enum CCClipMode {
        Bounds = 1,
        BoundsWithRenderTarget = 2,
        None = 0,
    }
    public partial class CCClippingNode : CocosSharp.CCNode {
        public CCClippingNode() { }
        public CCClippingNode(CocosSharp.CCNode stencil) { }
        public float AlphaThreshold { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public bool Inverted { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public override CocosSharp.CCScene Scene { get { return default(CocosSharp.CCScene); } }
        public CocosSharp.CCNode Stencil { get { return default(CocosSharp.CCNode); } set { } }
        public override void OnEnter() { }
        public override void OnEnterTransitionDidFinish() { }
        public override void OnExit() { }
        public override void OnExitTransitionDidStart() { }
        public override void Visit() { }
    }
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Size=1)]
    public partial struct CCColor3B {
        public static readonly CocosSharp.CCColor3B Black;
        public static readonly CocosSharp.CCColor3B Blue;
        public static readonly CocosSharp.CCColor3B DarkGray;
        public static readonly CocosSharp.CCColor3B Gray;
        public static readonly CocosSharp.CCColor3B Green;
        public static readonly CocosSharp.CCColor3B Magenta;
        public static readonly CocosSharp.CCColor3B Orange;
        public static readonly CocosSharp.CCColor3B Red;
        public static readonly CocosSharp.CCColor3B White;
        public static readonly CocosSharp.CCColor3B Yellow;
        public CCColor3B(CocosSharp.CCColor4B color4B) { throw new System.NotImplementedException(); }
        public CCColor3B(byte red, byte green, byte blue) { throw new System.NotImplementedException(); }
        public byte B { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(byte); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public byte G { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(byte); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public byte R { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(byte); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public bool Equals(CocosSharp.CCColor3B other) { return default(bool); }
        public override bool Equals(object obj) { return default(bool); }
        public override int GetHashCode() { return default(int); }
        public static CocosSharp.CCColor3B operator /(CocosSharp.CCColor3B p1, float div) { return default(CocosSharp.CCColor3B); }
        public static bool operator ==(CocosSharp.CCColor3B p1, CocosSharp.CCColor3B p2) { return default(bool); }
        public static bool operator !=(CocosSharp.CCColor3B p1, CocosSharp.CCColor3B p2) { return default(bool); }
        public static CocosSharp.CCColor3B operator *(CocosSharp.CCColor3B p1, CocosSharp.CCColor3B p2) { return default(CocosSharp.CCColor3B); }
    }
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Size=1)]
    public partial struct CCColor4B {
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
        public CCColor4B(byte red, byte green, byte blue) { throw new System.NotImplementedException(); }
        public CCColor4B(byte red, byte green, byte blue, byte alpha) { throw new System.NotImplementedException(); }
        public CCColor4B(float red, float green, float blue, float alpha) { throw new System.NotImplementedException(); }
        public byte A { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(byte); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public byte B { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(byte); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public byte G { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(byte); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public byte R { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(byte); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public bool Equals(CocosSharp.CCColor4B other) { return default(bool); }
        public override bool Equals(object obj) { return default(bool); }
        public override int GetHashCode() { return default(int); }
        public static CocosSharp.CCColor4B Lerp(CocosSharp.CCColor4B value1, CocosSharp.CCColor4B value2, float amount) { return default(CocosSharp.CCColor4B); }
        public static CocosSharp.CCColor4B operator /(CocosSharp.CCColor4B p1, float div) { return default(CocosSharp.CCColor4B); }
        public static bool operator ==(CocosSharp.CCColor4B p1, CocosSharp.CCColor4B p2) { return default(bool); }
        public static implicit operator Microsoft.Xna.Framework.Color (CocosSharp.CCColor4B point) { return default(Microsoft.Xna.Framework.Color); }
        public static bool operator !=(CocosSharp.CCColor4B p1, CocosSharp.CCColor4B p2) { return default(bool); }
        public static CocosSharp.CCColor4B operator *(CocosSharp.CCColor4B p1, CocosSharp.CCColor4B p2) { return default(CocosSharp.CCColor4B); }
        public static CocosSharp.CCColor4B operator *(CocosSharp.CCColor4B p1, float scale) { return default(CocosSharp.CCColor4B); }
        public static CocosSharp.CCColor4B operator *(float scale, CocosSharp.CCColor4B p1) { return default(CocosSharp.CCColor4B); }
        public static CocosSharp.CCColor4B Parse(string s) { return default(CocosSharp.CCColor4B); }
        public override string ToString() { return default(string); }
    }
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Size=1)]
    public partial struct CCColor4F {
        public CCColor4F(CocosSharp.CCColor3B color3B) { throw new System.NotImplementedException(); }
        public CCColor4F(CocosSharp.CCColor4B color4B) { throw new System.NotImplementedException(); }
        public CCColor4F(float red, float green, float blue, float alpha) { throw new System.NotImplementedException(); }
        public float A { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public float B { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public float G { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public float R { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public bool Equals(CocosSharp.CCColor4F other) { return default(bool); }
        public override bool Equals(object obj) { return default(bool); }
        public override int GetHashCode() { return default(int); }
        public static bool operator ==(CocosSharp.CCColor4F a, CocosSharp.CCColor4F b) { return default(bool); }
        public static implicit operator CocosSharp.CCColor4B (CocosSharp.CCColor4F point) { return default(CocosSharp.CCColor4B); }
        public static implicit operator Microsoft.Xna.Framework.Color (CocosSharp.CCColor4F color4F) { return default(Microsoft.Xna.Framework.Color); }
        public static bool operator !=(CocosSharp.CCColor4F a, CocosSharp.CCColor4F b) { return default(bool); }
        public static CocosSharp.CCColor4F Parse(string s) { return default(CocosSharp.CCColor4F); }
        public override string ToString() { return default(string); }
    }
    public partial class CCConfiguration {
        internal CCConfiguration() { }
        public CocosSharp.CCGlesVersion GlesVersion { get { return default(CocosSharp.CCGlesVersion); } }
        protected bool Inited { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } }
        public bool IsSupportsBGRA8888 { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } }
        public bool IsSupportsDiscardFramebuffer { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } }
        public bool IsSupportsNPOT { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } }
        public bool IsSupportsPVRTC { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } }
        public int MaxModelviewStackDepth { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } }
        public int MaxTextureSize { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } }
        public uint OSVersion { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(uint); } }
        public static CocosSharp.CCConfiguration SharedConfiguration { get { return default(CocosSharp.CCConfiguration); } }
        public bool CheckForGLExtension(string searchName) { return default(bool); }
    }
    public partial class CCContentManager : Microsoft.Xna.Framework.Content.ContentManager {
        public static CocosSharp.CCContentManager SharedContentManager;
        public CCContentManager(System.IServiceProvider serviceProvider) : base (default(System.IServiceProvider)) { }
        public CCContentManager(System.IServiceProvider serviceProvider, string rootDirectory) : base (default(System.IServiceProvider)) { }
        public System.Collections.Generic.List<System.String> SearchPaths { get { return default(System.Collections.Generic.List<System.String>); } }
        public System.Collections.Generic.List<System.String> SearchResolutionsOrder { get { return default(System.Collections.Generic.List<System.String>); } }
        public System.IO.Stream GetAssetStream(string assetName) { return default(System.IO.Stream); }
        public override T Load<T>(string assetName) { return default(T); }
        public T Load<T>(string assetName, bool weakReference) { return default(T); }
        protected override void ReloadGraphicsAssets() { }
        public T TryLoad<T>(string assetName, bool weakReference=false) { return default(T); }
        public override void Unload() { }
    }
    public partial class CCDeccelAmplitude : CocosSharp.CCAccelAmplitude {
        public CCDeccelAmplitude(CocosSharp.CCAmplitudeAction pAction, float duration, float deccRate=1f) : base (default(CocosSharp.CCAmplitudeAction), default(float), default(float)) { }
        public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public partial class CCDelayTime : CocosSharp.CCFiniteTimeAction {
        public CCDelayTime(float d) { }
        public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public enum CCDepthFormat {
        Depth16 = 54,
        Depth24 = 51,
        Depth24Stencil8 = 48,
        None = -1,
    }
    public static partial class CCDevice {
        public static float DPI { get { return default(float); } }
    }
    public partial class CCDirector {
        public CCDirector() { }
        public bool CanPopScene { get { return default(bool); } }
        public bool IsSendCleanupToScene { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } }
        public CocosSharp.CCScene RunningScene { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCScene); } }
        public int SceneCount { get { return default(int); } }
        public bool DeserializeState() { return default(bool); }
        public void PopScene() { }
        public void PopScene(float t, CocosSharp.CCTransitionScene s) { }
        public void PopToRootScene() { }
        public void PopToSceneStackLevel(int level) { }
        public void PushScene(CocosSharp.CCScene pScene) { }
        public void ReplaceScene(CocosSharp.CCScene scene) { }
        public void ResetSceneStack() { }
        public void RunWithScene(CocosSharp.CCScene scene) { }
        public void SerializeState() { }
    }
    public partial class CCDisplayLinkDirector : CocosSharp.CCDirector {
        public CCDisplayLinkDirector() { }
    }
    [System.FlagsAttribute]
    public enum CCDisplayOrientation {
        Default = 0,
        LandscapeLeft = 1,
        LandscapeRight = 2,
        Portrait = 4,
        PortraitDown = 8,
        Unknown = 16,
    }
    public partial class CCDrawingPrimitives {
        public CCDrawingPrimitives() { }
        public static CocosSharp.CCColor4B DrawColor { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCColor4B); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public static float LineWidth { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public static float PointSize { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public static void Begin() { }
        public static void DrawArc(CocosSharp.CCRect rect, int startAngle, int sweepAngle) { }
        public static void DrawArc(CocosSharp.CCRect rect, int startAngle, int sweepAngle, CocosSharp.CCColor4B color) { }
        public static void DrawArc(int x, int y, int width, int height, int startAngle, int sweepAngle) { }
        public static void DrawArc(int x, int y, int width, int height, int startAngle, int sweepAngle, CocosSharp.CCColor4B color) { }
        public static void DrawCardinalSpline(System.Collections.Generic.List<CocosSharp.CCPoint> config, float tension, int segments) { }
        public static void DrawCatmullRom(System.Collections.Generic.List<CocosSharp.CCPoint> points, int segments) { }
        public static void DrawCircle(CocosSharp.CCPoint center, float radius, float angle, int segments, bool drawLineToCenter, CocosSharp.CCColor4B color, float scaleX=1f, float scaleY=1f) { }
        public static void DrawCircle(CocosSharp.CCPoint center, float radius, float angle, int segments, bool drawLineToCenter, float scaleX=1f, float scaleY=1f) { }
        public static void DrawCubicBezier(CocosSharp.CCPoint origin, CocosSharp.CCPoint control1, CocosSharp.CCPoint control2, CocosSharp.CCPoint destination, int segments) { }
        public static void DrawCubicBezier(CocosSharp.CCPoint origin, CocosSharp.CCPoint control1, CocosSharp.CCPoint control2, CocosSharp.CCPoint destination, int segments, CocosSharp.CCColor4B color) { }
        public static void DrawEllips(int x, int y, int width, int height) { }
        public static void DrawEllips(int x, int y, int width, int height, CocosSharp.CCColor4B color) { }
        public static void DrawEllipse(CocosSharp.CCRect rect) { }
        public static void DrawEllipse(CocosSharp.CCRect rect, CocosSharp.CCColor4B color) { }
        public static void DrawLine(CocosSharp.CCPoint origin, CocosSharp.CCPoint destination) { }
        public static void DrawLine(CocosSharp.CCPoint origin, CocosSharp.CCPoint destination, CocosSharp.CCColor4B color) { }
        public static void DrawPie(CocosSharp.CCRect rect, int startAngle, int sweepAngle) { }
        public static void DrawPie(CocosSharp.CCRect rect, int startAngle, int sweepAngle, CocosSharp.CCColor4B color) { }
        public static void DrawPie(int x, int y, int width, int height, int startAngle, int sweepAngle) { }
        public static void DrawPie(int x, int y, int width, int height, int startAngle, int sweepAngle, CocosSharp.CCColor4B color) { }
        public static void DrawPoint(CocosSharp.CCPoint point) { }
        public static void DrawPoint(CocosSharp.CCPoint point, float size) { }
        public static void DrawPoint(CocosSharp.CCPoint p, float size, CocosSharp.CCColor4B color) { }
        public static void DrawPoints(CocosSharp.CCPoint[] points) { }
        public static void DrawPoints(CocosSharp.CCPoint[] points, int numberOfPoints, float size, CocosSharp.CCColor4B color) { }
        public static void DrawPoints(CocosSharp.CCPoint[] points, float size) { }
        public static void DrawPoints(CocosSharp.CCPoint[] points, float size, CocosSharp.CCColor4B color) { }
        public static void DrawPoly(CocosSharp.CCPoint[] vertices, CocosSharp.CCColor4B color, bool closePolygon=false) { }
        public static void DrawPoly(CocosSharp.CCPoint[] vertices, bool closePolygon=false) { }
        public static void DrawPoly(CocosSharp.CCPoint[] vertices, int numOfVertices, bool closePolygon) { }
        public static void DrawPoly(CocosSharp.CCPoint[] vertices, int numOfVertices, bool closePolygon, CocosSharp.CCColor4B color) { }
        public static void DrawPoly(CocosSharp.CCPoint[] vertices, int numOfVertices, bool closePolygon, bool fill, CocosSharp.CCColor4B color) { }
        public static void DrawQuadBezier(CocosSharp.CCPoint origin, CocosSharp.CCPoint control, CocosSharp.CCPoint destination, int segments) { }
        public static void DrawQuadBezier(CocosSharp.CCPoint origin, CocosSharp.CCPoint control, CocosSharp.CCPoint destination, int segments, CocosSharp.CCColor4B color) { }
        public static void DrawRect(CocosSharp.CCRect rect, CocosSharp.CCColor4B color) { }
        public static void DrawSolidCircle(CocosSharp.CCPoint center, float radius, float angle, int segments, CocosSharp.CCColor4B color, float scaleX=1f, float scaleY=1f) { }
        public static void DrawSolidCircle(CocosSharp.CCPoint center, float radius, float angle, int segments, float scaleX=1f, float scaleY=1f) { }
        public static void DrawSolidPoly(CocosSharp.CCPoint[] vertices, CocosSharp.CCColor4B color) { }
        public static void DrawSolidPoly(CocosSharp.CCPoint[] vertices, int count, CocosSharp.CCColor4B color) { }
        public static void DrawSolidPoly(CocosSharp.CCPoint[] vertices, int count, CocosSharp.CCColor4B color, bool outline) { }
        public static void DrawSolidRect(CocosSharp.CCPoint origin, CocosSharp.CCPoint destination, CocosSharp.CCColor4B color) { }
        public static void End() { }
    }
    public partial class CCDrawNode : CocosSharp.CCNode {
        public CCDrawNode() { }
        public CocosSharp.CCBlendFunc BlendFunc { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCBlendFunc); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
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
        public CCEaseBackIn(CocosSharp.CCFiniteTimeAction pAction) { }
        public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public partial class CCEaseBackInOut : CocosSharp.CCActionEase {
        public CCEaseBackInOut(CocosSharp.CCFiniteTimeAction pAction) { }
        public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public partial class CCEaseBackOut : CocosSharp.CCActionEase {
        public CCEaseBackOut(CocosSharp.CCFiniteTimeAction pAction) { }
        public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public partial class CCEaseBounceIn : CocosSharp.CCActionEase {
        public CCEaseBounceIn(CocosSharp.CCFiniteTimeAction pAction) { }
        public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public partial class CCEaseBounceInOut : CocosSharp.CCActionEase {
        public CCEaseBounceInOut(CocosSharp.CCFiniteTimeAction pAction) { }
        public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public partial class CCEaseBounceOut : CocosSharp.CCActionEase {
        public CCEaseBounceOut(CocosSharp.CCFiniteTimeAction pAction) { }
        public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public partial class CCEaseCustom : CocosSharp.CCActionEase {
        public CCEaseCustom(CocosSharp.CCFiniteTimeAction pAction, System.Func<System.Single, System.Single> easeFunc) { }
        public System.Func<System.Single, System.Single> EaseFunc { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Func<System.Single, System.Single>); } }
        public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public partial class CCEaseElastic : CocosSharp.CCActionEase {
        public CCEaseElastic(CocosSharp.CCFiniteTimeAction pAction) { }
        public CCEaseElastic(CocosSharp.CCFiniteTimeAction pAction, float fPeriod) { }
        public float Period { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } }
        public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public partial class CCEaseElasticIn : CocosSharp.CCEaseElastic {
        public CCEaseElasticIn(CocosSharp.CCFiniteTimeAction pAction) : base (default(CocosSharp.CCFiniteTimeAction), default(float)) { }
        public CCEaseElasticIn(CocosSharp.CCFiniteTimeAction pAction, float fPeriod) : base (default(CocosSharp.CCFiniteTimeAction), default(float)) { }
        public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public partial class CCEaseElasticInOut : CocosSharp.CCEaseElastic {
        public CCEaseElasticInOut(CocosSharp.CCFiniteTimeAction pAction) : base (default(CocosSharp.CCFiniteTimeAction), default(float)) { }
        public CCEaseElasticInOut(CocosSharp.CCFiniteTimeAction pAction, float fPeriod) : base (default(CocosSharp.CCFiniteTimeAction), default(float)) { }
        public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public partial class CCEaseElasticOut : CocosSharp.CCEaseElastic {
        public CCEaseElasticOut(CocosSharp.CCFiniteTimeAction pAction) : base (default(CocosSharp.CCFiniteTimeAction), default(float)) { }
        public CCEaseElasticOut(CocosSharp.CCFiniteTimeAction pAction, float fPeriod) : base (default(CocosSharp.CCFiniteTimeAction), default(float)) { }
        public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public partial class CCEaseExponentialIn : CocosSharp.CCActionEase {
        public CCEaseExponentialIn(CocosSharp.CCFiniteTimeAction pAction) { }
        public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public partial class CCEaseExponentialInOut : CocosSharp.CCActionEase {
        public CCEaseExponentialInOut(CocosSharp.CCFiniteTimeAction pAction) { }
        public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public partial class CCEaseExponentialOut : CocosSharp.CCActionEase {
        public CCEaseExponentialOut(CocosSharp.CCFiniteTimeAction pAction) { }
        public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public partial class CCEaseIn : CocosSharp.CCEaseRateAction {
        public CCEaseIn(CocosSharp.CCFiniteTimeAction pAction, float fRate) : base (default(CocosSharp.CCFiniteTimeAction), default(float)) { }
        public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public partial class CCEaseInOut : CocosSharp.CCEaseRateAction {
        public CCEaseInOut(CocosSharp.CCFiniteTimeAction pAction, float fRate) : base (default(CocosSharp.CCFiniteTimeAction), default(float)) { }
        public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public partial class CCEaseOut : CocosSharp.CCEaseRateAction {
        public CCEaseOut(CocosSharp.CCFiniteTimeAction pAction, float fRate) : base (default(CocosSharp.CCFiniteTimeAction), default(float)) { }
        public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public partial class CCEaseRateAction : CocosSharp.CCActionEase {
        public CCEaseRateAction(CocosSharp.CCFiniteTimeAction pAction, float fRate) { }
        public float Rate { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } }
        public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public partial class CCEaseSineIn : CocosSharp.CCActionEase {
        public CCEaseSineIn(CocosSharp.CCFiniteTimeAction pAction) { }
        public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public partial class CCEaseSineInOut : CocosSharp.CCActionEase {
        public CCEaseSineInOut(CocosSharp.CCFiniteTimeAction pAction) { }
        public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public partial class CCEaseSineOut : CocosSharp.CCActionEase {
        public CCEaseSineOut(CocosSharp.CCFiniteTimeAction pAction) { }
        public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public enum CCEmitterMode {
        Gravity = 0,
        Radius = 1,
    }
    public partial class CCEvent {
        internal CCEvent() { }
        public static string EVENT_COME_TO_BACKGROUND;
        public static string EVENT_COME_TO_FOREGROUND;
        public CocosSharp.CCNode CurrentTarget { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCNode); } }
        public bool IsStopped { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public void StopPropogation() { }
    }
    public partial class CCEventAccelerate : CocosSharp.CCEvent {
        internal CCEventAccelerate() { }
        public CocosSharp.CCAcceleration Acceleration { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCAcceleration); } }
    }
    public enum CCEventCode {
        BEGAN = 0,
        CANCELLED = 3,
        ENDED = 2,
        MOVED = 1,
    }
    public partial class CCEventCustom : CocosSharp.CCEvent {
        public CCEventCustom(string eventName, object userData=null) { }
        public string EventName { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(string); } }
        public object UserData { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(object); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    }
    public partial class CCEventGamePad : CocosSharp.CCEvent {
        internal CCEventGamePad() { }
        public CocosSharp.CCGamePadEventType GamePadEventType { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCGamePadEventType); } }
    }
    public partial class CCEventGamePadButton : CocosSharp.CCEventGamePad {
        internal CCEventGamePadButton() { }
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
    }
    public partial class CCEventGamePadConnection : CocosSharp.CCEventGamePad {
        internal CCEventGamePadConnection() { }
        public bool IsConnected { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } }
        public CocosSharp.CCPlayerIndex Player { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCPlayerIndex); } }
    }
    public partial class CCEventGamePadDPad : CocosSharp.CCEventGamePad {
        internal CCEventGamePadDPad() { }
        public CocosSharp.CCGamePadButtonStatus Down { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCGamePadButtonStatus); } }
        public CocosSharp.CCGamePadButtonStatus Left { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCGamePadButtonStatus); } }
        public CocosSharp.CCPlayerIndex Player { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCPlayerIndex); } }
        public CocosSharp.CCGamePadButtonStatus Right { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCGamePadButtonStatus); } }
        public CocosSharp.CCGamePadButtonStatus Up { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCGamePadButtonStatus); } }
    }
    public partial class CCEventGamePadStick : CocosSharp.CCEventGamePad {
        internal CCEventGamePadStick() { }
        public CocosSharp.CCGameStickStatus Left { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCGameStickStatus); } }
        public CocosSharp.CCPlayerIndex Player { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCPlayerIndex); } }
        public CocosSharp.CCGameStickStatus Right { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCGameStickStatus); } }
    }
    public partial class CCEventGamePadTrigger : CocosSharp.CCEventGamePad {
        internal CCEventGamePadTrigger() { }
        public float Left { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } }
        public CocosSharp.CCPlayerIndex Player { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCPlayerIndex); } }
        public float Right { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } }
    }
    public partial class CCEventKeyboard : CocosSharp.CCEvent {
        internal CCEventKeyboard() { }
        public CocosSharp.CCKeyboardEventType KeyboardEventType { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCKeyboardEventType); } }
        public CocosSharp.CCKeyboardState KeyboardState { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCKeyboardState); } }
        public CocosSharp.CCKeys Keys { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCKeys); } }
    }
    public partial class CCEventListener : System.IDisposable {
        protected CCEventListener() { }
        protected CCEventListener(CocosSharp.CCEventListener eventListener) { }
        protected CCEventListener(CocosSharp.CCEventListenerType type, string listenerID) { }
        protected CCEventListener(CocosSharp.CCEventListenerType type, string listenerID, System.Action<CocosSharp.CCEvent> callback) { }
        public virtual bool IsAvailable { get { return default(bool); } }
        public virtual bool IsEnabled { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public virtual CocosSharp.CCEventListener Copy() { return default(CocosSharp.CCEventListener); }
        public void Dispose() { }
        protected virtual void Dispose(bool disposing) { }
        ~CCEventListener() { }
    }
    public partial class CCEventListenerAccelerometer : CocosSharp.CCEventListener {
        public static string LISTENER_ID;
        public CCEventListenerAccelerometer() { }
        public override bool IsAvailable { get { return default(bool); } }
        public System.Action<CocosSharp.CCEventAccelerate> OnAccelerate { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Action<CocosSharp.CCEventAccelerate>); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public override CocosSharp.CCEventListener Copy() { return default(CocosSharp.CCEventListener); }
    }
    public partial class CCEventListenerCustom : CocosSharp.CCEventListener {
        public CCEventListenerCustom(string eventName, System.Action<CocosSharp.CCEventCustom> callback) { }
        public override bool IsAvailable { get { return default(bool); } }
        public System.Action<CocosSharp.CCEventCustom> OnCustomEvent { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Action<CocosSharp.CCEventCustom>); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    }
    public partial class CCEventListenerGamePad : CocosSharp.CCEventListener {
        public static string LISTENER_ID;
        public CCEventListenerGamePad() { }
        public override bool IsAvailable { get { return default(bool); } }
        public System.Action<CocosSharp.CCEventGamePadButton> OnButtonStatus { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Action<CocosSharp.CCEventGamePadButton>); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public System.Action<CocosSharp.CCEventGamePadConnection> OnConnectionStatus { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Action<CocosSharp.CCEventGamePadConnection>); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public System.Action<CocosSharp.CCEventGamePadDPad> OnDPadStatus { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Action<CocosSharp.CCEventGamePadDPad>); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public System.Action<CocosSharp.CCEventGamePadStick> OnStickStatus { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Action<CocosSharp.CCEventGamePadStick>); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public System.Action<CocosSharp.CCEventGamePadTrigger> OnTriggerStatus { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Action<CocosSharp.CCEventGamePadTrigger>); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public override CocosSharp.CCEventListener Copy() { return default(CocosSharp.CCEventListener); }
    }
    public partial class CCEventListenerKeyboard : CocosSharp.CCEventListener {
        public static string LISTENER_ID;
        public CCEventListenerKeyboard() { }
        public override bool IsAvailable { get { return default(bool); } }
        public System.Action<CocosSharp.CCEventKeyboard> OnKeyPressed { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Action<CocosSharp.CCEventKeyboard>); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public System.Action<CocosSharp.CCEventKeyboard> OnKeyReleased { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Action<CocosSharp.CCEventKeyboard>); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public override CocosSharp.CCEventListener Copy() { return default(CocosSharp.CCEventListener); }
    }
    public partial class CCEventListenerMouse : CocosSharp.CCEventListener {
        public static string LISTENER_ID;
        public CCEventListenerMouse() { }
        public override bool IsAvailable { get { return default(bool); } }
        public System.Action<CocosSharp.CCEventMouse> OnMouseDown { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Action<CocosSharp.CCEventMouse>); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public System.Action<CocosSharp.CCEventMouse> OnMouseMove { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Action<CocosSharp.CCEventMouse>); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public System.Action<CocosSharp.CCEventMouse> OnMouseScroll { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Action<CocosSharp.CCEventMouse>); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public System.Action<CocosSharp.CCEventMouse> OnMouseUp { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Action<CocosSharp.CCEventMouse>); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public override CocosSharp.CCEventListener Copy() { return default(CocosSharp.CCEventListener); }
    }
    public partial class CCEventListenerTouchAllAtOnce : CocosSharp.CCEventListener {
        public static string LISTENER_ID;
        public CCEventListenerTouchAllAtOnce() { }
        public override bool IsAvailable { get { return default(bool); } }
        public System.Action<System.Collections.Generic.List<CocosSharp.CCTouch>, CocosSharp.CCEvent> OnTouchesBegan { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Action<System.Collections.Generic.List<CocosSharp.CCTouch>, CocosSharp.CCEvent>); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public System.Action<System.Collections.Generic.List<CocosSharp.CCTouch>, CocosSharp.CCEvent> OnTouchesCancelled { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Action<System.Collections.Generic.List<CocosSharp.CCTouch>, CocosSharp.CCEvent>); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public System.Action<System.Collections.Generic.List<CocosSharp.CCTouch>, CocosSharp.CCEvent> OnTouchesEnded { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Action<System.Collections.Generic.List<CocosSharp.CCTouch>, CocosSharp.CCEvent>); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public System.Action<System.Collections.Generic.List<CocosSharp.CCTouch>, CocosSharp.CCEvent> OnTouchesMoved { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Action<System.Collections.Generic.List<CocosSharp.CCTouch>, CocosSharp.CCEvent>); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public override CocosSharp.CCEventListener Copy() { return default(CocosSharp.CCEventListener); }
    }
    public partial class CCEventListenerTouchOneByOne : CocosSharp.CCEventListener {
        public static string LISTENER_ID;
        public CCEventListenerTouchOneByOne() { }
        public override bool IsAvailable { get { return default(bool); } }
        public bool IsSwallowTouches { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public System.Func<CocosSharp.CCTouch, CocosSharp.CCEvent, System.Boolean> OnTouchBegan { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Func<CocosSharp.CCTouch, CocosSharp.CCEvent, System.Boolean>); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public System.Action<CocosSharp.CCTouch, CocosSharp.CCEvent> OnTouchCancelled { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Action<CocosSharp.CCTouch, CocosSharp.CCEvent>); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public System.Action<CocosSharp.CCTouch, CocosSharp.CCEvent> OnTouchEnded { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Action<CocosSharp.CCTouch, CocosSharp.CCEvent>); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public System.Action<CocosSharp.CCTouch, CocosSharp.CCEvent> OnTouchMoved { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Action<CocosSharp.CCTouch, CocosSharp.CCEvent>); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public override CocosSharp.CCEventListener Copy() { return default(CocosSharp.CCEventListener); }
    }
    public enum CCEventListenerType {
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
        public float CursorX { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } }
        public float CursorY { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } }
        public CocosSharp.CCMouseButton MouseButton { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCMouseButton); } }
        public CocosSharp.CCMouseEventType MouseEventType { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCMouseEventType); } }
        public float ScrollX { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } }
        public float ScrollY { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } }
    }
    public partial class CCEventTouch : CocosSharp.CCEvent {
        internal CCEventTouch() { }
        public CocosSharp.CCEventCode EventCode { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCEventCode); } }
        public System.Collections.Generic.List<CocosSharp.CCTouch> Touches { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Collections.Generic.List<CocosSharp.CCTouch>); } }
    }
    public enum CCEventType {
        ACCELERATION = 2,
        CUSTOM = 5,
        GAMEPAD = 4,
        KEYBOARD = 1,
        MOUSE = 3,
        TOUCH = 0,
    }
    public partial class CCFadeIn : CocosSharp.CCFiniteTimeAction {
        public CCFadeIn(float d) { }
        public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public partial class CCFadeOut : CocosSharp.CCFiniteTimeAction {
        public CCFadeOut(float d) { }
        public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public partial class CCFadeOutBLTiles : CocosSharp.CCFadeOutTRTiles {
        public CCFadeOutBLTiles(float duration, CocosSharp.CCGridSize gridSize) : base (default(float)) { }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public partial class CCFadeOutDownTiles : CocosSharp.CCFadeOutUpTiles {
        public CCFadeOutDownTiles(float duration, CocosSharp.CCGridSize gridSize) : base (default(float), default(CocosSharp.CCGridSize)) { }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public partial class CCFadeOutTRTiles : CocosSharp.CCTiledGrid3DAction {
        public CCFadeOutTRTiles(float duration) : base (default(float)) { }
        public CCFadeOutTRTiles(float duration, CocosSharp.CCGridSize gridSize) : base (default(float)) { }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public partial class CCFadeOutUpTiles : CocosSharp.CCFadeOutTRTiles {
        public CCFadeOutUpTiles(float duration, CocosSharp.CCGridSize gridSize) : base (default(float)) { }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public partial class CCFadeTo : CocosSharp.CCFiniteTimeAction {
        public CCFadeTo(float duration, byte opacity) { }
        public byte ToOpacity { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(byte); } }
        public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public partial class CCFastRandom {
        public CCFastRandom() { }
        public CCFastRandom(int seed) { }
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
        public CCFileUtils() { }
        public static bool IsPopupNotify { get { return default(bool); } set { } }
        public static int CCLoadFileIntoMemory(string filename, out System.Char[] file) { file = default(System.Char[]); return default(int); }
        public static string CCRemoveHDSuffixFromFile(string path) { return default(string); }
        public static System.Collections.Generic.Dictionary<System.String, System.Object> DictionaryWithContentsOfFile(string filename) { return default(System.Collections.Generic.Dictionary<System.String, System.Object>); }
        public static string FullPathFromRelativeFile(string filename, string relativeFile) { return default(string); }
        public static string FullPathFromRelativePath(string relativePath) { return default(string); }
        public static System.Byte[] GetFileBytes(string filename) { return default(System.Byte[]); }
        public static string GetFileData(string filename) { return default(string); }
        public static System.Char[] GetFileDataFromZip(string zipFilePath, string filename, ulong pSize) { return default(System.Char[]); }
        public static System.IO.Stream GetFileStream(string fileName) { return default(System.IO.Stream); }
        public static string GetWriteablePath() { return default(string); }
        public static string RemoveExtension(string fileName) { return default(string); }
        public static void SetResource(string zipFilename) { }
        public static void SetResourcePath(string resourcePath) { }
    }
    public abstract partial class CCFiniteTimeAction : CocosSharp.CCAction {
        protected CCFiniteTimeAction() { }
        protected CCFiniteTimeAction(float duration) { }
        public virtual float Duration { get { return default(float); } set { } }
        public abstract CocosSharp.CCFiniteTimeAction Reverse();
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public partial class CCFiniteTimeActionState : CocosSharp.CCActionState {
        public CCFiniteTimeActionState(CocosSharp.CCFiniteTimeAction action, CocosSharp.CCNode target) : base (default(CocosSharp.CCAction), default(CocosSharp.CCNode)) { }
        public virtual float Duration { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public float Elapsed { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } }
        public override bool IsDone { get { return default(bool); } }
        protected internal override void Step(float dt) { }
    }
    public partial class CCFlipX : CocosSharp.CCActionInstant {
        public CCFlipX(bool x) { }
        public bool FlipX { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } }
        public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public partial class CCFlipX3D : CocosSharp.CCGrid3DAction {
        public CCFlipX3D(float duration) : base (default(float)) { }
        public CCFlipX3D(float duration, CocosSharp.CCGridSize gridSize) : base (default(float)) { }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public partial class CCFlipY : CocosSharp.CCActionInstant {
        public CCFlipY(bool y) { }
        public bool FlipY { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } }
        public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public partial class CCFlipY3D : CocosSharp.CCFlipX3D {
        public CCFlipY3D(float duration) : base (default(float), default(CocosSharp.CCGridSize)) { }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public delegate void CCFocusChangeDelegate(CocosSharp.ICCFocusable prev, CocosSharp.ICCFocusable current);
    public partial class CCFocusManager {
        internal CCFocusManager() { }
        public static float MenuScrollDelay;
        public bool Enabled { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public static CocosSharp.CCFocusManager Instance { get { return default(CocosSharp.CCFocusManager); } }
        public CocosSharp.ICCFocusable ItemWithFocus { get { return default(CocosSharp.ICCFocusable); } }
        public event CocosSharp.CCFocusChangeDelegate OnFocusChanged { add { } remove { } }
        public void Add(params CocosSharp.ICCFocusable[] focusItems) { }
        public void FocusNextItem() { }
        public void FocusPreviousItem() { }
        public void Remove(params CocosSharp.ICCFocusable[] focusItems) { }
    }
    public partial class CCFollow : CocosSharp.CCAction {
        public CCFollow(CocosSharp.CCNode followedNode, CocosSharp.CCRect rect) { }
        public bool BoundaryFullyCovered { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } }
        public bool BoundarySet { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } }
        protected internal CocosSharp.CCNode FollowedNode { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCNode); } }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public static partial class CCFPSImage {
        public static System.Byte[] PngData;
    }
    public partial class CCGame : Microsoft.Xna.Framework.Game {
        public CCGame() { }
        protected override void Update(Microsoft.Xna.Framework.GameTime gameTime) { }
    }
    public enum CCGamePadButtonStatus {
        NotApplicable = 3,
        Pressed = 0,
        Released = 1,
        Tapped = 2,
    }
    public enum CCGamePadEventType {
        GAMEPAD_BUTTON = 1,
        GAMEPAD_CONNECTION = 5,
        GAMEPAD_DPAD = 2,
        GAMEPAD_NONE = 0,
        GAMEPAD_STICK = 3,
        GAMEPAD_TRIGGER = 4,
    }
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public partial struct CCGameStickStatus {
        public CocosSharp.CCPoint Direction;
        public bool IsDown;
        public float Magnitude;
    }
    public partial class CCGameTime {
        public CCGameTime() { }
        public CCGameTime(System.TimeSpan totalGameTime, System.TimeSpan elapsedGameTime) { }
        public CCGameTime(System.TimeSpan totalRealTime, System.TimeSpan elapsedRealTime, bool isRunningSlowly) { }
        public System.TimeSpan ElapsedGameTime { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.TimeSpan); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public bool IsRunningSlowly { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public System.TimeSpan TotalGameTime { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.TimeSpan); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    }
    public enum CCGlesVersion {
        GLES_VER_1_0 = 1,
        GLES_VER_1_1 = 2,
        GLES_VER_2_0 = 3,
        GLES_VER_INVALID = 0,
    }
    public partial class CCGrabber {
        internal CCGrabber() { }
        public void AfterRender(CocosSharp.CCTexture2D texture) { }
        public void BeforeRender(CocosSharp.CCTexture2D texture) { }
        public void Grab(CocosSharp.CCTexture2D texture) { }
    }
    public partial class CCGraphicsResource : System.IDisposable {
        public CCGraphicsResource() { }
        public bool IsDisposed { get { return default(bool); } }
        public void Dispose() { }
        protected virtual void Dispose(bool disposing) { }
        ~CCGraphicsResource() { }
        public virtual void ReinitResource() { }
    }
    public partial class CCGrid3D : CocosSharp.CCGridBase {
        public CCGrid3D(CocosSharp.CCGridSize gridSize, CocosSharp.CCTexture2D texture, bool flipped=false) : base (default(CocosSharp.CCGridSize), default(CocosSharp.CCTexture2D), default(bool)) { }
        protected System.UInt16[] Indices { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.UInt16[]); } }
        public CocosSharp.CCVertex3F this[CocosSharp.CCGridSize pos] { get { return default(CocosSharp.CCVertex3F); } set { } }
        public CocosSharp.CCVertex3F this[int x, int y] { get { return default(CocosSharp.CCVertex3F); } set { } }
        protected CocosSharp.CCVertex3F[] OriginalVertices { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCVertex3F[]); } }
        public override void Blit() { }
        public override void CalculateVertexPoints() { }
        public CocosSharp.CCVertex3F OriginalVertex(CocosSharp.CCGridSize pos) { return default(CocosSharp.CCVertex3F); }
        public CocosSharp.CCVertex3F OriginalVertex(int x, int y) { return default(CocosSharp.CCVertex3F); }
        public override void Reuse() { }
    }
    public partial class CCGrid3DAction : CocosSharp.CCGridAction {
        protected CCGrid3DAction(float duration) : base (default(float)) { }
        protected CCGrid3DAction(float duration, CocosSharp.CCGridSize gridSize) : base (default(float)) { }
        protected CCGrid3DAction(float duration, CocosSharp.CCGridSize gridSize, float amplitude) : base (default(float)) { }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public partial class CCGridAction : CocosSharp.CCAmplitudeAction {
        public CCGridAction(float duration) : base (default(float), default(float)) { }
        public CCGridAction(float duration, CocosSharp.CCGridSize gridSize) : base (default(float), default(float)) { }
        protected CCGridAction(float duration, CocosSharp.CCGridSize gridSize, float amplitude) : base (default(float), default(float)) { }
        protected internal CocosSharp.CCGridSize GridSize { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCGridSize); } }
        public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public abstract partial class CCGridBase {
        protected CCGridBase(CocosSharp.CCGridSize gridSize, CocosSharp.CCTexture2D texture, bool flipped=false) { }
        public bool Active { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        protected CocosSharp.CCGrabber Grabber { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCGrabber); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public CocosSharp.CCGridSize GridSize { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCGridSize); } }
        public int ReuseGrid { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public CocosSharp.CCPoint Step { get { return default(CocosSharp.CCPoint); } }
        protected CocosSharp.CCTexture2D Texture { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCTexture2D); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public bool TextureFlipped { get { return default(bool); } set { } }
        public virtual void AfterDraw(CocosSharp.CCNode target) { }
        public virtual void BeforeDraw() { }
        public abstract void Blit();
        public abstract void CalculateVertexPoints();
        public ulong NextPOT(ulong x) { return default(ulong); }
        public abstract void Reuse();
    }
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public partial struct CCGridSize {
        public static readonly CocosSharp.CCGridSize One;
        public int X;
        public int Y;
        public static readonly CocosSharp.CCGridSize Zero;
        public CCGridSize(int x, int y) { throw new System.NotImplementedException(); }
    }
    public partial class CCHide : CocosSharp.CCActionInstant {
        public CCHide() { }
        public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public enum CCImageFormat {
        Gif = 4,
        Jpg = 0,
        Png = 1,
        RawData = 5,
        Tiff = 2,
        UnKnown = 6,
        Webp = 3,
    }
    public partial class CCIMEKeyboardNotificationInfo {
        public CocosSharp.CCRect Begin;
        public float Duration;
        public CocosSharp.CCRect End;
        public CCIMEKeyboardNotificationInfo() { }
    }
    public partial class CCJumpBy : CocosSharp.CCFiniteTimeAction {
        public CCJumpBy(float duration, CocosSharp.CCPoint position, float height, uint jumps) { }
        public float Height { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]protected set { } }
        public uint Jumps { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(uint); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]protected set { } }
        public CocosSharp.CCPoint Position { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCPoint); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]protected set { } }
        public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public partial class CCJumpTiles3D : CocosSharp.CCTiledGrid3DAction {
        public CCJumpTiles3D(float duration, CocosSharp.CCGridSize gridSize, int numberOfJumps=0, float amplitude=0f) : base (default(float)) { }
        protected internal int NumberOfJumps { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public partial class CCJumpTo : CocosSharp.CCJumpBy {
        public CCJumpTo(float duration, CocosSharp.CCPoint position, float height, uint jumps) : base (default(float), default(CocosSharp.CCPoint), default(float), default(uint)) { }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public enum CCKeyboardEventType {
        KEYBOARD_NONE = 0,
        KEYBOARD_PRESS = 1,
        KEYBOARD_RELEASE = 2,
        KEYBOARD_STATE = 3,
    }
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Size=1)]
    public partial struct CCKeyboardState {
        public CocosSharp.CCKeyState this[CocosSharp.CCKeys key] { get { return default(CocosSharp.CCKeyState); } }
        public override bool Equals(object obj) { return default(bool); }
        public override int GetHashCode() { return default(int); }
        public CocosSharp.CCKeys[] GetPressedKeys() { return default(CocosSharp.CCKeys[]); }
        public bool IsKeyDown(CocosSharp.CCKeys key) { return default(bool); }
        public bool IsKeyUp(CocosSharp.CCKeys key) { return default(bool); }
        public static bool operator ==(CocosSharp.CCKeyboardState a, CocosSharp.CCKeyboardState b) { return default(bool); }
        public static bool operator !=(CocosSharp.CCKeyboardState a, CocosSharp.CCKeyboardState b) { return default(bool); }
    }
    public enum CCKeys {
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
        Down = 1,
        Up = 0,
    }
    public partial class CCLabel : CocosSharp.CCLabelBMFont {
        public CCLabel() { }
        public CCLabel(string text, string fontName, float fontSize) { }
        public CCLabel(string text, string fontName, float fontSize, CocosSharp.CCSize dimensions) { }
        public CCLabel(string text, string fontName, float fontSize, CocosSharp.CCSize dimensions, CocosSharp.CCTextAlignment hAlignment) { }
        public CCLabel(string text, string fontName, float fontSize, CocosSharp.CCSize dimensions, CocosSharp.CCTextAlignment hAlignment, CocosSharp.CCVerticalTextAlignment vAlignment) { }
        public CCLabel(string text, string fontName, float fontSize, CocosSharp.CCTextAlignment hAlignment) { }
        public CCLabel(string text, string fontName, float fontSize, CocosSharp.CCTextAlignment hAlignment, CocosSharp.CCVerticalTextAlignment vAlignment) { }
        public string FontName { get { return default(string); } set { } }
        public float FontSize { get { return default(float); } set { } }
        public override string Text { get { return default(string); } set { } }
        protected override void AddedToNewScene() { }
        public static CocosSharp.CCLabel.ivec4 AllocateRegion(int width, int height) { return default(CocosSharp.CCLabel.ivec4); }
        protected override void Draw() { }
        public static void InitializeTTFAtlas(int width, int height) { }
        public static void SetRegionData(CocosSharp.CCLabel.ivec4 region, System.Int32[] data, int stride) { }
        [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public partial struct ivec4 {
            public int height;
            public int width;
            public int x;
            public int y;
        }
    }
    public partial class CCLabelAtlas : CocosSharp.CCAtlasNode, CocosSharp.ICCTextContainer {
        protected char m_cMapStartChar;
        protected string m_sString;
        public CCLabelAtlas(string label, CocosSharp.CCTexture2D texture, int itemWidth, int itemHeight, char startCharMap) : base (default(string), default(int), default(int), default(int)) { }
        public CCLabelAtlas(string label, string fntFile) : base (default(string), default(int), default(int), default(int)) { }
        public CCLabelAtlas(string label, string charMapFile, int itemWidth, int itemHeight, char startCharMap) : base (default(string), default(int), default(int), default(int)) { }
        public string Text { get { return default(string); } set { } }
        public override void UpdateAtlasValues() { }
    }
    public partial class CCLabelBMFont : CocosSharp.CCSpriteBatchNode, CocosSharp.ICCTextContainer {
        public const int AutomaticWidth = -1;
        protected string fntConfigFile;
        protected CocosSharp.CCTextAlignment horzAlignment;
        protected bool isColorModifiedByOpacity;
        protected CocosSharp.CCSize labelDimensions;
        protected string labelInitialText;
        protected string labelText;
        protected bool lineBreakWithoutSpaces;
        protected CocosSharp.CCSprite m_pReusedChar;
        protected CocosSharp.CCVerticalTextAlignment vertAlignment;
        public CCLabelBMFont() { }
        public CCLabelBMFont(string str, string fntFile) { }
        public CCLabelBMFont(string str, string fntFile, CocosSharp.CCSize dimensions, CocosSharp.CCTextAlignment hAlignment, CocosSharp.CCVerticalTextAlignment vAlignment, CocosSharp.CCPoint imageOffset, CocosSharp.CCTexture2D texture) { }
        public CCLabelBMFont(string str, string fntFile, float width) { }
        public CCLabelBMFont(string str, string fntFile, float width, CocosSharp.CCTextAlignment alignment) { }
        public CCLabelBMFont(string str, string fntFile, float width, CocosSharp.CCTextAlignment alignment, CocosSharp.CCPoint imageOffset) { }
        public CCLabelBMFont(string str, string fntFile, float width, CocosSharp.CCTextAlignment alignment, CocosSharp.CCPoint imageOffset, CocosSharp.CCTexture2D texture) { }
        public CCLabelBMFont(string str, string fntFile, float width, CocosSharp.CCTextAlignment hAlignment, CocosSharp.CCVerticalTextAlignment vAlignment, CocosSharp.CCPoint imageOffset, CocosSharp.CCTexture2D texture) { }
        public override CocosSharp.CCPoint AnchorPoint { get { return default(CocosSharp.CCPoint); } set { } }
        public CocosSharp.CCSize Dimensions { get { return default(CocosSharp.CCSize); } set { } }
        public string FntFile { get { return default(string); } set { } }
        public CocosSharp.CCTextAlignment HorizontalAlignment { get { return default(CocosSharp.CCTextAlignment); } set { } }
        protected CocosSharp.CCPoint ImageOffset { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCPoint); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public override bool IsColorModifiedByOpacity { get { return default(bool); } set { } }
        protected bool IsDirty { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public bool LineBreakWithoutSpace { get { return default(bool); } set { } }
        public override float Scale { set { } }
        public override float ScaleX { get { return default(float); } set { } }
        public override float ScaleY { get { return default(float); } set { } }
        public virtual string Text { get { return default(string); } set { } }
        public CocosSharp.CCVerticalTextAlignment VerticalAlignment { get { return default(CocosSharp.CCVerticalTextAlignment); } set { } }
        protected override void AddedToNewScene() { }
        public void CreateFontChars() { }
        protected override void Draw() { }
        protected void InitCCLabelBMFont(string theString, string fntFile, CocosSharp.CCSize dimensions, CocosSharp.CCTextAlignment hAlignment, CocosSharp.CCVerticalTextAlignment vAlignment, CocosSharp.CCPoint imageOffset, CocosSharp.CCTexture2D texture) { }
        public static void PurgeCachedData() { }
        public virtual void SetString(string newString, bool needUpdateLabel) { }
        public override void UpdateColor() { }
        protected void UpdateLabel() { }
        protected override void VisibleBoundsChanged() { }
    }
    public partial class CCLabelTtf : CocosSharp.CCSprite, CocosSharp.ICCTextContainer {
        protected string m_pString;
        public CCLabelTtf() { }
        public CCLabelTtf(string text, string fontName, float fontSize) { }
        public CCLabelTtf(string text, string fontName, float fontSize, CocosSharp.CCSize dimensions, CocosSharp.CCTextAlignment hAlignment) { }
        public CCLabelTtf(string text, string fontName, float fontSize, CocosSharp.CCSize dimensions, CocosSharp.CCTextAlignment hAlignment, CocosSharp.CCVerticalTextAlignment vAlignment) { }
        public CocosSharp.CCSize Dimensions { get { return default(CocosSharp.CCSize); } set { } }
        public string FontName { get { return default(string); } set { } }
        public float FontSize { get { return default(float); } set { } }
        public CocosSharp.CCTextAlignment HorizontalAlignment { get { return default(CocosSharp.CCTextAlignment); } set { } }
        public string Text { get { return default(string); } set { } }
        public CocosSharp.CCVerticalTextAlignment VerticalAlignment { get { return default(CocosSharp.CCVerticalTextAlignment); } set { } }
        public override string ToString() { return default(string); }
    }
    public partial class CCLayer : CocosSharp.CCNode {
        public CCLayer(CocosSharp.CCCamera camera) { }
        public CCLayer(CocosSharp.CCCamera camera, CocosSharp.CCClipMode clipMode) { }
        public CCLayer(CocosSharp.CCSize visibleBoundsDimensions, CocosSharp.CCClipMode clipMode=(CocosSharp.CCClipMode)(0)) { }
        public override CocosSharp.CCAffineTransform AffineLocalTransform { get { return default(CocosSharp.CCAffineTransform); } }
        public override CocosSharp.CCCamera Camera { get { return default(CocosSharp.CCCamera); } set { } }
        public CocosSharp.CCClipMode ChildClippingMode { get { return default(CocosSharp.CCClipMode); } set { } }
        public override CocosSharp.CCSize ContentSize { get { return default(CocosSharp.CCSize); } set { } }
        public override CocosSharp.CCLayer Layer { get { return default(CocosSharp.CCLayer); } }
        public CocosSharp.CCSize LayerSizeInPixels { get { return default(CocosSharp.CCSize); } }
        public CocosSharp.CCRect VisibleBoundsWorldspace { get { return default(CocosSharp.CCRect); } }
        public CocosSharp.CCPoint ScreenToWorldspace(CocosSharp.CCPoint point) { return default(CocosSharp.CCPoint); }
        public CocosSharp.CCSize ScreenToWorldspace(CocosSharp.CCSize size) { return default(CocosSharp.CCSize); }
        protected override void ViewportChanged() { }
        protected override void VisibleBoundsChanged() { }
        public override void Visit() { }
        public CocosSharp.CCPoint WorldToScreenspace(CocosSharp.CCPoint point) { return default(CocosSharp.CCPoint); }
        public CocosSharp.CCSize WorldToScreenspace(CocosSharp.CCSize size) { return default(CocosSharp.CCSize); }
    }
    public partial class CCLayerColor : CocosSharp.CCLayer, CocosSharp.ICCBlendable {
        public CCLayerColor(CocosSharp.CCCamera camera) : base (default(CocosSharp.CCSize), default(CocosSharp.CCClipMode)) { }
        public CCLayerColor(CocosSharp.CCCamera camera, CocosSharp.CCColor4B color) : base (default(CocosSharp.CCSize), default(CocosSharp.CCClipMode)) { }
        public CCLayerColor(CocosSharp.CCSize size) : base (default(CocosSharp.CCSize), default(CocosSharp.CCClipMode)) { }
        public CCLayerColor(CocosSharp.CCSize size, CocosSharp.CCColor4B color) : base (default(CocosSharp.CCSize), default(CocosSharp.CCClipMode)) { }
        public virtual CocosSharp.CCBlendFunc BlendFunc { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCBlendFunc); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public override CocosSharp.CCColor3B Color { get { return default(CocosSharp.CCColor3B); } set { } }
        public override CocosSharp.CCSize ContentSize { get { return default(CocosSharp.CCSize); } set { } }
        public override byte Opacity { get { return default(byte); } set { } }
        protected override void Draw() { }
        public override void UpdateColor() { }
        protected override void ViewportChanged() { }
        protected override void VisibleBoundsChanged() { }
    }
    public partial class CCLayerGradient : CocosSharp.CCLayerColor {
        public CCLayerGradient(CocosSharp.CCCamera camera) : base (default(CocosSharp.CCSize)) { }
        public CCLayerGradient(CocosSharp.CCCamera camera, CocosSharp.CCColor4B start, CocosSharp.CCColor4B end) : base (default(CocosSharp.CCSize)) { }
        public CCLayerGradient(CocosSharp.CCCamera camera, CocosSharp.CCColor4B start, CocosSharp.CCColor4B end, CocosSharp.CCPoint gradientDirection) : base (default(CocosSharp.CCSize)) { }
        public CCLayerGradient(CocosSharp.CCCamera camera, byte startOpacity, byte endOpacity) : base (default(CocosSharp.CCSize)) { }
        public CocosSharp.CCColor3B EndColor { get { return default(CocosSharp.CCColor3B); } set { } }
        public byte EndOpacity { get { return default(byte); } set { } }
        public bool IsCompressedInterpolation { get { return default(bool); } set { } }
        public CocosSharp.CCColor3B StartColor { get { return default(CocosSharp.CCColor3B); } set { } }
        public byte StartOpacity { get { return default(byte); } set { } }
        public CocosSharp.CCPoint Vector { get { return default(CocosSharp.CCPoint); } set { } }
        public override void UpdateColor() { }
    }
    public partial class CCLayerMultiplex : CocosSharp.CCLayer {
        public const int NoLayer = -1;
        public CCLayerMultiplex(CocosSharp.CCCamera camera) : base (default(CocosSharp.CCSize), default(CocosSharp.CCClipMode)) { }
        public CCLayerMultiplex(CocosSharp.CCCamera camera, CocosSharp.CCAction inAction, CocosSharp.CCAction outAction) : base (default(CocosSharp.CCSize), default(CocosSharp.CCClipMode)) { }
        public CCLayerMultiplex(CocosSharp.CCCamera camera, CocosSharp.CCAction inAction, CocosSharp.CCAction outAction, params CocosSharp.CCLayer[] layers) : base (default(CocosSharp.CCSize), default(CocosSharp.CCClipMode)) { }
        public CCLayerMultiplex(CocosSharp.CCCamera camera, params CocosSharp.CCLayer[] layers) : base (default(CocosSharp.CCSize), default(CocosSharp.CCClipMode)) { }
        public virtual CocosSharp.CCLayer ActiveLayer { get { return default(CocosSharp.CCLayer); } }
        protected int EnabledLayer { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public CocosSharp.CCAction InAction { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCAction); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        protected System.Collections.Generic.Dictionary<System.Int32, CocosSharp.CCLayer> Layers { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Collections.Generic.Dictionary<System.Int32, CocosSharp.CCLayer>); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public CocosSharp.CCAction OutAction { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCAction); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public override CocosSharp.CCScene Scene { get { return default(CocosSharp.CCScene); } }
        public bool ShowFirstLayerOnEnter { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public void AddLayer(CocosSharp.CCLayer layer) { }
        public override void OnEnter() { }
        public CocosSharp.CCLayer SwitchTo(int n) { return default(CocosSharp.CCLayer); }
        public CocosSharp.CCLayer SwitchToAndRemovePreviousLayer(int n) { return default(CocosSharp.CCLayer); }
        public CocosSharp.CCLayer SwitchToFirstLayer() { return default(CocosSharp.CCLayer); }
        public CocosSharp.CCLayer SwitchToNextLayer() { return default(CocosSharp.CCLayer); }
        public void SwitchToNone() { }
        public CocosSharp.CCLayer SwitchToPreviousLayer() { return default(CocosSharp.CCLayer); }
    }
    public partial class CCLens3D : CocosSharp.CCGrid3DAction {
        public CCLens3D(float duration, CocosSharp.CCGridSize gridSize) : base (default(float)) { }
        public CCLens3D(float duration, CocosSharp.CCGridSize gridSize, CocosSharp.CCPoint position, float radius) : base (default(float)) { }
        public bool Concave { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } }
        public float LensEffect { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } }
        public CocosSharp.CCPoint Position { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCPoint); } }
        public float Radius { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public partial class CCLiquid : CocosSharp.CCGrid3DAction {
        public CCLiquid(float duration, CocosSharp.CCGridSize gridSize, int waves=0, float amplitude=0f) : base (default(float)) { }
        public int Waves { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public partial class CCLog {
        public CCLog() { }
        public static void Log(string message) { }
        public static void Log(string format, params System.Object[] args) { }
    }
    public static partial class CCMacros {
        public static float CCDegreesToRadians(float angle) { return default(float); }
        public static float CCRadiansToDegrees(float angle) { return default(float); }
        public static float CCRandomBetween0And1() { return default(float); }
        public static float CCRandomBetweenNegative1And1() { return default(float); }
        public static void CCSwap<T>(ref T x, ref T y) { }
    }
    public partial class CCMaskedSprite : CocosSharp.CCSprite {
        public CCMaskedSprite(CocosSharp.CCTexture2D texture, System.Byte[] mask) { }
        public CCMaskedSprite(CocosSharp.CCTexture2D texture, System.Nullable<CocosSharp.CCRect> textureRect, System.Byte[] mask) { }
        public CCMaskedSprite(string fileName, System.Byte[] mask) { }
        public CCMaskedSprite(string fileName, System.Nullable<CocosSharp.CCRect> textureRect, System.Byte[] mask) { }
        public virtual System.Byte[] CollisionMask { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Byte[]); } }
        public virtual bool CollidesWith(CocosSharp.CCMaskedSprite target, out CocosSharp.CCPoint pt) { pt = default(CocosSharp.CCPoint); return default(bool); }
    }
    public static partial class CCMathHelper {
        public const float Pi = 3.14159274f;
        public const float TwoPi = 6.28318548f;
        public static int Clamp(int value, int min, int max) { return default(int); }
        public static float Clamp(float value, float min, float max) { return default(float); }
        public static float Cos(float radian) { return default(float); }
        public static int Lerp(int value1, int value2, float amount) { return default(int); }
        public static float Sin(float radian) { return default(float); }
        public static float ToDegrees(float radians) { return default(float); }
        public static float ToRadians(float degrees) { return default(float); }
    }
    public partial class CCMenu : CocosSharp.CCNode {
        public const int DefaultMenuHandlerPriority = -128;
        public const float DefaultPadding = 5f;
        public CCMenu(params CocosSharp.CCMenuItem[] items) { }
        public bool Enabled { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public CocosSharp.CCMenuItem FocusedItem { get { return default(CocosSharp.CCMenuItem); } }
        public override bool HasFocus { set { } }
        protected CocosSharp.CCMenuState MenuState { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCMenuState); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        protected CocosSharp.CCMenuItem SelectedMenuItem { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCMenuItem); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public void AddChild(CocosSharp.CCMenuItem menuItem) { }
        public void AddChild(CocosSharp.CCMenuItem menuItem, int zOrder, int tag=0) { }
        protected override void AddedToNewScene() { }
        public void AlignItemsHorizontally(float padding=5f) { }
        public void AlignItemsInColumns(params System.UInt32[] numOfItemsPerRow) { }
        public void AlignItemsInRows(params System.UInt32[] numOfItemsPerColumn) { }
        public void AlignItemsVertically(float padding=5f) { }
        protected virtual CocosSharp.CCMenuItem ItemForTouch(CocosSharp.CCTouch touch) { return default(CocosSharp.CCMenuItem); }
        public override void OnEnter() { }
        public override void OnExit() { }
        public void RemoveChild(CocosSharp.CCMenuItem menuItem, bool cleanup) { }
        protected override void VisibleBoundsChanged() { }
    }
    public partial class CCMenuItem : CocosSharp.CCNode {
        public CCMenuItem() { }
        public CCMenuItem(System.Action<System.Object> target) { }
        public virtual bool Enabled { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public virtual bool Selected { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public System.Action<System.Object> Target { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Action<System.Object>); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public virtual void Activate() { }
        public virtual void RegisterScriptHandler(string functionName) { }
    }
    public partial class CCMenuItemFont : CocosSharp.CCMenuItemLabelTTF {
        public CCMenuItemFont(string labelString, System.Action<System.Object> selector=null) : base (default(CocosSharp.CCLabelTtf), default(System.Action<System.Object>)) { }
        public static string FontName { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(string); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public static uint FontSize { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(uint); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    }
    public partial class CCMenuItemImage : CocosSharp.CCMenuItem {
        public CCMenuItemImage() { }
        public CCMenuItemImage(CocosSharp.CCSprite normalSprite, CocosSharp.CCSprite selectedSprite, CocosSharp.CCSprite disabledSprite, System.Action<System.Object> target=null) { }
        public CCMenuItemImage(CocosSharp.CCSprite normalSprite, CocosSharp.CCSprite selectedSprite, System.Action<System.Object> target=null) { }
        public CCMenuItemImage(CocosSharp.CCSprite normalSprite, System.Action<System.Object> selector=null) { }
        public CCMenuItemImage(CocosSharp.CCSpriteFrame normalSpFrame, CocosSharp.CCSpriteFrame selectedSpFrame, CocosSharp.CCSpriteFrame disabledSpFrame, System.Action<System.Object> target=null) { }
        public CCMenuItemImage(string normalSprite, string selectedSprite, System.Action<System.Object> target=null) { }
        public CCMenuItemImage(string normalSprite, string selectedSprite, string disabledSprite, System.Action<System.Object> target=null) { }
        public CocosSharp.CCSprite DisabledImage { get { return default(CocosSharp.CCSprite); } set { } }
        public CocosSharp.CCSpriteFrame DisabledImageSpriteFrame { set { } }
        public override bool Enabled { get { return default(bool); } set { } }
        public CocosSharp.CCSprite NormalImage { get { return default(CocosSharp.CCSprite); } set { } }
        public CocosSharp.CCSpriteFrame NormalImageSpriteFrame { set { } }
        public override bool Selected { set { } }
        public CocosSharp.CCSprite SelectedImage { get { return default(CocosSharp.CCSprite); } set { } }
        public CocosSharp.CCSpriteFrame SelectedImageSpriteFrame { set { } }
        public bool ZoomBehaviorOnTouch { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public override void Activate() { }
        protected override void UpdateTransform() { }
    }
    public partial class CCMenuItemLabel : CocosSharp.CCMenuItemLabelBase {
        public CCMenuItemLabel(CocosSharp.CCLabel label, System.Action<System.Object> target=null) : base (default(System.Action<System.Object>)) { }
        public override bool Enabled { get { return default(bool); } set { } }
        public CocosSharp.CCLabel Label { get { return default(CocosSharp.CCLabel); } set { } }
        public override CocosSharp.CCScene Scene { get { return default(CocosSharp.CCScene); } }
    }
    public partial class CCMenuItemLabelAtlas : CocosSharp.CCMenuItemLabelBase {
        public CCMenuItemLabelAtlas(CocosSharp.CCLabelAtlas labelAtlas, System.Action<System.Object> target=null) : base (default(System.Action<System.Object>)) { }
        public CCMenuItemLabelAtlas(System.Action<System.Object> target) : base (default(System.Action<System.Object>)) { }
        public CCMenuItemLabelAtlas(string value, string charMapFile, int itemWidth, int itemHeight, char startCharMap) : base (default(System.Action<System.Object>)) { }
        public CCMenuItemLabelAtlas(string value, string charMapFile, int itemWidth, int itemHeight, char startCharMap, CocosSharp.ICCUpdatable updatable, System.Action<System.Object> target) : base (default(System.Action<System.Object>)) { }
        public override bool Enabled { get { return default(bool); } set { } }
        public CocosSharp.CCLabelAtlas LabelAtlas { get { return default(CocosSharp.CCLabelAtlas); } set { } }
        public override CocosSharp.CCScene Scene { get { return default(CocosSharp.CCScene); } }
    }
    public abstract partial class CCMenuItemLabelBase : CocosSharp.CCMenuItem {
        protected CCMenuItemLabelBase(System.Action<System.Object> target=null) { }
        protected CocosSharp.CCColor3B ColorBackup { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCColor3B); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public CocosSharp.CCColor3B DisabledColor { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCColor3B); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public override bool Selected { set { } }
        public override void Activate() { }
        protected void LabelWillChange(CocosSharp.CCNode oldValue, CocosSharp.CCNode newValue) { }
    }
    public partial class CCMenuItemLabelBMFont : CocosSharp.CCMenuItemLabelBase {
        public CCMenuItemLabelBMFont(CocosSharp.CCLabelBMFont labelBMFont, System.Action<System.Object> target=null) : base (default(System.Action<System.Object>)) { }
        public override bool Enabled { get { return default(bool); } set { } }
        public CocosSharp.CCLabelBMFont LabelBMFont { get { return default(CocosSharp.CCLabelBMFont); } set { } }
        public override CocosSharp.CCScene Scene { get { return default(CocosSharp.CCScene); } }
    }
    public partial class CCMenuItemLabelTTF : CocosSharp.CCMenuItemLabelBase {
        public CCMenuItemLabelTTF(CocosSharp.CCLabelTtf labelTTF, System.Action<System.Object> target=null) : base (default(System.Action<System.Object>)) { }
        public CCMenuItemLabelTTF(System.Action<System.Object> target=null) : base (default(System.Action<System.Object>)) { }
        public override bool Enabled { get { return default(bool); } set { } }
        public CocosSharp.CCLabelTtf LabelTTF { get { return default(CocosSharp.CCLabelTtf); } set { } }
        public override CocosSharp.CCScene Scene { get { return default(CocosSharp.CCScene); } }
    }
    public partial class CCMenuItemToggle : CocosSharp.CCMenuItem {
        public CCMenuItemToggle(params CocosSharp.CCMenuItem[] items) { }
        public CCMenuItemToggle(System.Action<System.Object> target, params CocosSharp.CCMenuItem[] items) { }
        public override bool Enabled { get { return default(bool); } set { } }
        public override CocosSharp.CCScene Scene { get { return default(CocosSharp.CCScene); } }
        public override bool Selected { set { } }
        public int SelectedIndex { get { return default(int); } set { } }
        public CocosSharp.CCMenuItem SelectedItem { get { return default(CocosSharp.CCMenuItem); } }
        public override void Activate() { }
        public void AddToggleMenuItems(params CocosSharp.CCMenuItem[] items) { }
        public void RemoveToggleMenuItems(params CocosSharp.CCMenuItem[] items) { }
    }
    public enum CCMenuState {
        Focused = 2,
        TrackingTouch = 1,
        Waiting = 0,
    }
    public partial class CCMotionStreak : CocosSharp.CCNode, CocosSharp.ICCBlendable, CocosSharp.ICCTexture {
        public CCMotionStreak() { }
        public CCMotionStreak(float fade, float minSegIn, float strokeIn, CocosSharp.CCColor3B color, CocosSharp.CCTexture2D texture) { }
        public CCMotionStreak(float fade, float minSeg, float stroke, CocosSharp.CCColor3B color, string path) { }
        public CocosSharp.CCBlendFunc BlendFunc { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCBlendFunc); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public bool FastMode { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public override bool IsColorModifiedByOpacity { get { return default(bool); } set { } }
        public override byte Opacity { get { return default(byte); } set { } }
        public override CocosSharp.CCPoint Position { set { } }
        public bool StartingPositionInitialized { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } }
        public CocosSharp.CCTexture2D Texture { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCTexture2D); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        protected override void Draw() { }
        public void TintWithColor(CocosSharp.CCColor3B colors) { }
        public override void Update(float delta) { }
    }
    [System.FlagsAttribute]
    public enum CCMouseButton {
        ExtraButton1 = 8,
        ExtraButton2 = 22,
        LeftButton = 1,
        MiddleButton = 2,
        None = 0,
        RightButton = 4,
    }
    public enum CCMouseEventType {
        MOUSE_DOWN = 1,
        MOUSE_MOVE = 3,
        MOUSE_NONE = 0,
        MOUSE_SCROLL = 4,
        MOUSE_UP = 2,
    }
    public partial class CCMoveBy : CocosSharp.CCFiniteTimeAction {
        public CCMoveBy(float duration, CocosSharp.CCPoint position) { }
        public CocosSharp.CCPoint PositionDelta { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCPoint); } }
        public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public partial class CCMoveTo : CocosSharp.CCMoveBy {
        protected CocosSharp.CCPoint EndPosition;
        public CCMoveTo(float duration, CocosSharp.CCPoint position) : base (default(float), default(CocosSharp.CCPoint)) { }
        public CocosSharp.CCPoint PositionEnd { get { return default(CocosSharp.CCPoint); } }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public partial class CCNode : CocosSharp.ICCFocusable, CocosSharp.ICCUpdatable, System.Collections.Generic.IComparer<CocosSharp.CCNode>, System.IComparable<CocosSharp.CCNode> {
        public const int TagInvalid = -1;
        public CCNode() { }
        public CocosSharp.CCAffineTransform AdditionalTransform { get { return default(CocosSharp.CCAffineTransform); } set { } }
        public virtual CocosSharp.CCAffineTransform AffineLocalTransform { get { return default(CocosSharp.CCAffineTransform); } }
        public CocosSharp.CCAffineTransform AffineWorldTransform { get { return default(CocosSharp.CCAffineTransform); } }
        public virtual CocosSharp.CCPoint AnchorPoint { get { return default(CocosSharp.CCPoint); } set { } }
        public virtual CocosSharp.CCPoint AnchorPointInPoints { get { return default(CocosSharp.CCPoint); } }
        public virtual CocosSharp.CCApplication Application { get { return default(CocosSharp.CCApplication); } }
        public CocosSharp.CCRect BoundingBox { get { return default(CocosSharp.CCRect); } }
        public virtual CocosSharp.CCCamera Camera { get { return default(CocosSharp.CCCamera); } set { } }
        public virtual bool CanReceiveFocus { get { return default(bool); } }
        public CocosSharp.CCRawList<CocosSharp.CCNode> Children { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCRawList<CocosSharp.CCNode>); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]protected set { } }
        public int ChildrenCount { get { return default(int); } }
        public virtual CocosSharp.CCColor3B Color { get { return default(CocosSharp.CCColor3B); } set { } }
        public virtual CocosSharp.CCSize ContentSize { get { return default(CocosSharp.CCSize); } set { } }
        public virtual CocosSharp.CCDirector Director { get { return default(CocosSharp.CCDirector); } set { } }
        public CocosSharp.CCColor3B DisplayedColor { get { return default(CocosSharp.CCColor3B); } protected set { } }
        public byte DisplayedOpacity { get { return default(byte); } protected set { } }
        public float GlobalZOrder { get { return default(float); } set { } }
        public CocosSharp.CCGridBase Grid { get { return default(CocosSharp.CCGridBase); } set { } }
        public virtual bool HasFocus { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public virtual bool IgnoreAnchorPointForPosition { get { return default(bool); } set { } }
        public virtual bool IsColorCascaded { get { return default(bool); } set { } }
        public virtual bool IsColorModifiedByOpacity { get { return default(bool); } set { } }
        public virtual bool IsOpacityCascaded { get { return default(bool); } set { } }
        protected bool IsReorderChildDirty { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public bool IsRunning { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]protected set { } }
        public virtual bool IsSerializable { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]protected set { } }
        public CocosSharp.CCNode this[int tag] { get { return default(CocosSharp.CCNode); } }
        public virtual CocosSharp.CCLayer Layer { get { return default(CocosSharp.CCLayer); } }
        public int LocalZOrder { get { return default(int); } set { } }
        public string Name { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(string); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public int NumberOfRunningActions { get { return default(int); } }
        public virtual byte Opacity { get { return default(byte); } set { } }
        protected internal uint OrderOfArrival { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(uint); } }
        public CocosSharp.CCNode Parent { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCNode); } }
        public virtual CocosSharp.CCPoint Position { get { return default(CocosSharp.CCPoint); } set { } }
        public virtual CocosSharp.CCPoint PositionWorldspace { get { return default(CocosSharp.CCPoint); } }
        public float PositionX { get { return default(float); } set { } }
        public float PositionY { get { return default(float); } set { } }
        protected CocosSharp.CCColor3B RealColor { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCColor3B); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        protected byte RealOpacity { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(byte); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public virtual float Rotation { set { } }
        public virtual float RotationX { get { return default(float); } set { } }
        public virtual float RotationY { get { return default(float); } set { } }
        public virtual float Scale { set { } }
        public virtual CocosSharp.CCSize ScaledContentSize { get { return default(CocosSharp.CCSize); } }
        public virtual float ScaleX { get { return default(float); } set { } }
        public virtual float ScaleY { get { return default(float); } set { } }
        public virtual CocosSharp.CCScene Scene { get { return default(CocosSharp.CCScene); } }
        public virtual float SkewX { get { return default(float); } set { } }
        public virtual float SkewY { get { return default(float); } set { } }
        public int Tag { get { return default(int); } set { } }
        public CocosSharp.CCRect TransformedBoundingBox { get { return default(CocosSharp.CCRect); } }
        public CocosSharp.CCRect TransformedBoundingBoxWorldspace { get { return default(CocosSharp.CCRect); } }
        public object UserData { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(object); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public object UserObject { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(object); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public virtual float VertexZ { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public virtual CocosSharp.CCViewport Viewport { get { return default(CocosSharp.CCViewport); } set { } }
        public virtual bool Visible { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public virtual CocosSharp.CCWindow Window { get { return default(CocosSharp.CCWindow); } set { } }
        protected Microsoft.Xna.Framework.Matrix XnaWorldMatrix { get { return default(Microsoft.Xna.Framework.Matrix); } }
        public int ZOrder { get { return default(int); } set { } }
        public void AddAction(CocosSharp.CCAction action, bool paused=false) { }
        public void AddActions(bool paused, params CocosSharp.CCFiniteTimeAction[] actions) { }
        public void AddChild(CocosSharp.CCNode child) { }
        public void AddChild(CocosSharp.CCNode child, int zOrder) { }
        public virtual void AddChild(CocosSharp.CCNode child, int zOrder, int tag) { }
        public CocosSharp.CCEventListenerCustom AddCustomEventListener(string eventName, System.Action<CocosSharp.CCEventCustom> callback) { return default(CocosSharp.CCEventListenerCustom); }
        protected virtual void AddedToNewScene() { }
        public void AddEventListener(CocosSharp.CCEventListener listener, CocosSharp.CCNode node=null) { }
        public void AddEventListener(CocosSharp.CCEventListener listener, int fixedPriority) { }
        public virtual void Cleanup() { }
        public int CompareTo(CocosSharp.CCNode that) { return default(int); }
        public virtual void Deserialize(System.IO.Stream stream) { }
        protected internal void DisableCascadeColor() { }
        protected virtual void DisableCascadeOpacity() { }
        public void DispatchEvent(CocosSharp.CCEvent eventToDispatch) { }
        public void DispatchEvent(string customEvent, object userData=null) { }
        public void Dispose() { }
        protected virtual void Dispose(bool disposing) { }
        protected virtual void Draw() { }
        ~CCNode() { }
        public CocosSharp.CCAction GetAction(int tag) { return default(CocosSharp.CCAction); }
        public CocosSharp.CCActionState GetActionState(int tag) { return default(CocosSharp.CCActionState); }
        public CocosSharp.CCNode GetChildByTag(int tag) { return default(CocosSharp.CCNode); }
        public virtual void KeyBackClicked() { }
        public virtual void KeyMenuClicked() { }
        public virtual void OnEnter() { }
        public virtual void OnEnterTransitionDidFinish() { }
        public virtual void OnExit() { }
        public virtual void OnExitTransitionDidStart() { }
        public void Pause() { }
        public void PauseListeners(CocosSharp.CCNode target, bool recursive=false) { }
        public void PauseListeners(bool recursive=false) { }
        public virtual void RemoveAllChildren(bool cleanup=true) { }
        public virtual void RemoveAllChildrenByTag(int tag, bool cleanup=true) { }
        public void RemoveAllListeners() { }
        public virtual void RemoveChild(CocosSharp.CCNode child, bool cleanup=true) { }
        public void RemoveChildByTag(int tag, bool cleanup=true) { }
        public void RemoveEventListener(CocosSharp.CCEventListener listener) { }
        public void RemoveEventListeners(CocosSharp.CCEventListenerType listenerType) { }
        public void RemoveEventListeners(CocosSharp.CCNode target, bool recursive=false) { }
        public void RemoveEventListeners(bool recursive=false) { }
        public void RemoveFromParent(bool cleanup=true) { }
        public virtual void ReorderChild(CocosSharp.CCNode child, int zOrder) { }
        public CocosSharp.CCActionState Repeat(uint times, CocosSharp.CCFiniteTimeAction action) { return default(CocosSharp.CCActionState); }
        public CocosSharp.CCActionState Repeat(uint times, params CocosSharp.CCFiniteTimeAction[] actions) { return default(CocosSharp.CCActionState); }
        public CocosSharp.CCActionState RepeatForever(CocosSharp.CCFiniteTimeAction action) { return default(CocosSharp.CCActionState); }
        public CocosSharp.CCActionState RepeatForever(params CocosSharp.CCFiniteTimeAction[] actions) { return default(CocosSharp.CCActionState); }
        protected virtual void ResetCleanState() { }
        public void Resume() { }
        public void ResumeListeners(CocosSharp.CCNode target, bool recursive=false) { }
        public void ResumeListeners(bool recursive=false) { }
        public CocosSharp.CCActionState RunAction(CocosSharp.CCAction action) { return default(CocosSharp.CCActionState); }
        public CocosSharp.CCActionState RunActions(params CocosSharp.CCFiniteTimeAction[] actions) { return default(CocosSharp.CCActionState); }
        public void Schedule() { }
        public void Schedule(System.Action<System.Single> selector) { }
        public void Schedule(System.Action<System.Single> selector, float interval) { }
        public void Schedule(System.Action<System.Single> selector, float interval, uint repeat, float delay) { }
        public void Schedule(int priority) { }
        public void ScheduleOnce(System.Action<System.Single> selector, float delay) { }
        public virtual void Serialize(System.IO.Stream stream) { }
        public void SetListenerPriority(CocosSharp.CCEventListener listener, int fixedPriority) { }
        public virtual void SortAllChildren() { }
        public void StopAction(CocosSharp.CCActionState actionState) { }
        public void StopAction(int tag) { }
        public void StopAllActions() { }
        int System.Collections.Generic.IComparer<CocosSharp.CCNode>.Compare(CocosSharp.CCNode n1, CocosSharp.CCNode n2) { return default(int); }
        public void Transform() { }
        public void TransformAncestors() { }
        public void Unschedule() { }
        public void Unschedule(System.Action<System.Single> selector) { }
        public void UnscheduleAll() { }
        public virtual void Update(float dt) { }
        protected internal void UpdateCascadeColor() { }
        protected internal virtual void UpdateCascadeOpacity() { }
        public virtual void UpdateColor() { }
        public virtual void UpdateDisplayedColor(CocosSharp.CCColor3B parentColor) { }
        protected internal virtual void UpdateDisplayedOpacity(byte parentOpacity) { }
        protected virtual void UpdateTransform() { }
        protected virtual void ViewportChanged() { }
        protected virtual void VisibleBoundsChanged() { }
        public virtual void Visit() { }
        public CocosSharp.CCPoint WorldToParentspace(CocosSharp.CCPoint point) { return default(CocosSharp.CCPoint); }
    }
    public partial class CCOGLES {
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
        public CCOGLES() { }
    }
    public partial class CCOrbitCamera : CocosSharp.CCActionCamera {
        public CCOrbitCamera(float t, float radius, float deltaRadius, float angleZ, float deltaAngleZ, float angleX, float deltaAngleX) : base (default(float)) { }
        public float AngleX { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } }
        public float AngleZ { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } }
        public float DeltaAngleX { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } }
        public float DeltaAngleZ { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } }
        public float DeltaRadius { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } }
        public float Radius { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public partial class CCPageTurn3D : CocosSharp.CCGrid3DAction {
        public CCPageTurn3D(float duration, CocosSharp.CCGridSize gridSize) : base (default(float)) { }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public partial class CCParallaxNode : CocosSharp.CCNode {
        public CCParallaxNode() { }
        public virtual void AddChild(CocosSharp.CCNode child, int z, CocosSharp.CCPoint ratio, CocosSharp.CCPoint offset) { }
        public override void AddChild(CocosSharp.CCNode child, int zOrder, int tag) { }
        public override void RemoveAllChildren(bool cleanup) { }
        public override void RemoveChild(CocosSharp.CCNode child, bool cleanup) { }
        public override void Visit() { }
    }
    public partial class CCParallel : CocosSharp.CCFiniteTimeAction {
        public CCParallel(params CocosSharp.CCFiniteTimeAction[] actions) { }
        public CocosSharp.CCFiniteTimeAction[] Actions { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCFiniteTimeAction[]); } }
        public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public partial class CCParticleBatchNode : CocosSharp.CCNode, CocosSharp.ICCBlendable, CocosSharp.ICCTexture {
        public const int ParticleDefaultCapacity = 500;
        public CCParticleBatchNode(CocosSharp.CCTexture2D tex, int capacity=500) { }
        public CCParticleBatchNode(string imageFile, int capacity=500) { }
        public override CocosSharp.CCAffineTransform AffineLocalTransform { get { return default(CocosSharp.CCAffineTransform); } }
        public CocosSharp.CCBlendFunc BlendFunc { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCBlendFunc); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public override CocosSharp.CCSize ContentSize { get { return default(CocosSharp.CCSize); } set { } }
        public CocosSharp.CCTexture2D Texture { get { return default(CocosSharp.CCTexture2D); } set { } }
        public CocosSharp.CCTextureAtlas TextureAtlas { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCTextureAtlas); } }
        public override void AddChild(CocosSharp.CCNode child, int zOrder, int tag) { }
        public void DisableParticle(int particleIndex) { }
        protected override void Draw() { }
        public override void RemoveAllChildren(bool doCleanup) { }
        public override void RemoveChild(CocosSharp.CCNode child, bool cleanup) { }
        public void RemoveChildAtIndex(int index, bool doCleanup) { }
        public override void ReorderChild(CocosSharp.CCNode child, int zOrder) { }
        public override void Visit() { }
    }
    public partial class CCParticleExplosion : CocosSharp.CCParticleSystemQuad {
        public CCParticleExplosion(CocosSharp.CCPoint position) : base (default(int), default(CocosSharp.CCEmitterMode)) { }
    }
    public partial class CCParticleFire : CocosSharp.CCParticleSystemQuad {
        public CCParticleFire(CocosSharp.CCPoint position) : base (default(int), default(CocosSharp.CCEmitterMode)) { }
    }
    public partial class CCParticleFireworks : CocosSharp.CCParticleSystemQuad {
        public CCParticleFireworks(CocosSharp.CCPoint position) : base (default(int), default(CocosSharp.CCEmitterMode)) { }
    }
    public partial class CCParticleFlower : CocosSharp.CCParticleSystemQuad {
        public CCParticleFlower(CocosSharp.CCPoint position) : base (default(int), default(CocosSharp.CCEmitterMode)) { }
    }
    public partial class CCParticleGalaxy : CocosSharp.CCParticleSystemQuad {
        public CCParticleGalaxy(CocosSharp.CCPoint position) : base (default(int), default(CocosSharp.CCEmitterMode)) { }
    }
    public partial class CCParticleMeteor : CocosSharp.CCParticleSystemQuad {
        public CCParticleMeteor(CocosSharp.CCPoint position) : base (default(int), default(CocosSharp.CCEmitterMode)) { }
    }
    public partial class CCParticleRain : CocosSharp.CCParticleSystemQuad {
        public CCParticleRain(CocosSharp.CCPoint position) : base (default(int), default(CocosSharp.CCEmitterMode)) { }
    }
    public partial class CCParticleSmoke : CocosSharp.CCParticleSystemQuad {
        public CCParticleSmoke(CocosSharp.CCPoint position) : base (default(int), default(CocosSharp.CCEmitterMode)) { }
    }
    public partial class CCParticleSnow : CocosSharp.CCParticleSystemQuad {
        public CCParticleSnow(CocosSharp.CCPoint position) : base (default(int), default(CocosSharp.CCEmitterMode)) { }
    }
    public partial class CCParticleSpiral : CocosSharp.CCParticleSystemQuad {
        public CCParticleSpiral(CocosSharp.CCPoint position) : base (default(int), default(CocosSharp.CCEmitterMode)) { }
    }
    public partial class CCParticleSun : CocosSharp.CCParticleSystemQuad {
        public CCParticleSun(CocosSharp.CCPoint position) : base (default(int), default(CocosSharp.CCEmitterMode)) { }
        public CCParticleSun(CocosSharp.CCPoint position, int num) : base (default(int), default(CocosSharp.CCEmitterMode)) { }
    }
    public partial class CCParticleSystem : CocosSharp.CCNode, CocosSharp.ICCBlendable, CocosSharp.ICCTexture {
        public const int ParticleDurationInfinity = -1;
        public const int ParticleStartRadiusEqualToEndRadius = -1;
        public const int ParticleStartSizeEqualToEndSize = -1;
        public CCParticleSystem(CocosSharp.CCParticleSystemConfig particleConfig) { }
        protected CCParticleSystem(int numberOfParticles, CocosSharp.CCEmitterMode emitterMode=(CocosSharp.CCEmitterMode)(0)) { }
        public CCParticleSystem(string plistFile, string directoryName=null) { }
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
        public override CocosSharp.CCLayer Layer { get { return default(CocosSharp.CCLayer); } }
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
        public override CocosSharp.CCScene Scene { get { return default(CocosSharp.CCScene); } }
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
        public override void OnEnter() { }
        public override void OnExit() { }
        protected virtual void PostStep() { }
        public void ResetSystem() { }
        public void StopSystem() { }
        public override void Update(float dt) { }
        public virtual void UpdateQuads() { }
        public void UpdateWithNoTime() { }
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
        public CCParticleSystemCache(CocosSharp.CCApplication application) { }
        public CocosSharp.CCParticleSystemConfig this[string key] { get { return default(CocosSharp.CCParticleSystemConfig); } }
        public static CocosSharp.CCParticleSystemCache SharedParticleSystemCache { get { return default(CocosSharp.CCParticleSystemCache); } }
        public CocosSharp.CCParticleSystemConfig AddParticleSystem(string fileConfig, string directoryName=null) { return default(CocosSharp.CCParticleSystemConfig); }
        public void AddParticleSystemAsync(string fileConfig, System.Action<CocosSharp.CCParticleSystemConfig> action, string directoryName=null) { }
        public bool Contains(string assetFile) { return default(bool); }
        public void Dispose() { }
        protected virtual void Dispose(bool disposing) { }
        public void DumpCachedInfo() { }
        public CocosSharp.CCParticleSystemConfig ParticleSystemForKey(string key) { return default(CocosSharp.CCParticleSystemConfig); }
        public static void PurgeSharedParticleSystemCache() { }
        public void Remove(CocosSharp.CCParticleSystemConfig particleSystem) { }
        public void RemoveAll() { }
        public void RemoveForKey(string particleSystemKeyName) { }
        public void RemoveUnused() { }
        public void UnloadContent() { }
        public void Update(float dt) { }
    }
    public partial class CCParticleSystemConfig : System.IDisposable {
        public CCParticleSystemConfig() { }
        public CCParticleSystemConfig(string plistFile, string directoryName=null) { }
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
        public void Dispose() { }
        protected virtual void Dispose(bool disposing) { }
    }
    public partial class CCParticleSystemQuad : CocosSharp.CCParticleSystem {
        public CCParticleSystemQuad(CocosSharp.CCParticleSystemConfig config) : base (default(string), default(string)) { }
        public CCParticleSystemQuad(int numberOfParticles, CocosSharp.CCEmitterMode emitterMode=(CocosSharp.CCEmitterMode)(0)) : base (default(string), default(string)) { }
        public CCParticleSystemQuad(string plistFile, string directoryName=null) : base (default(string), default(string)) { }
        public override CocosSharp.CCParticleBatchNode BatchNode { set { } }
        public override CocosSharp.CCTexture2D Texture { set { } }
        public CocosSharp.CCRect TextureRect { set { } }
        public override int TotalParticles { set { } }
        public CocosSharp.CCParticleSystemQuad Clone() { return default(CocosSharp.CCParticleSystemQuad); }
        protected override void Draw() { }
        public override void UpdateQuads() { }
    }
    public enum CCParticleSystemType {
        Cocos2D = 1,
        Custom = 2,
        Internal = 0,
    }
    public partial class CCPlace : CocosSharp.CCActionInstant {
        public CCPlace(CocosSharp.CCPoint pos) { }
        public CCPlace(int posX, int posY) { }
        public CocosSharp.CCPoint Position { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCPoint); } }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public enum CCPlayerIndex {
        Four = 3,
        One = 0,
        Three = 2,
        Two = 1,
    }
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public partial struct CCPoint {
        public static readonly CocosSharp.CCPoint AnchorLowerLeft;
        public static readonly CocosSharp.CCPoint AnchorLowerRight;
        public static readonly CocosSharp.CCPoint AnchorMiddle;
        public static readonly CocosSharp.CCPoint AnchorMiddleBottom;
        public static readonly CocosSharp.CCPoint AnchorMiddleLeft;
        public static readonly CocosSharp.CCPoint AnchorMiddleRight;
        public static readonly CocosSharp.CCPoint AnchorMiddleTop;
        public static readonly CocosSharp.CCPoint AnchorUpperLeft;
        public static readonly CocosSharp.CCPoint AnchorUpperRight;
        public static readonly CocosSharp.CCPoint NegativeInfinity;
        public float X;
        public float Y;
        public static readonly CocosSharp.CCPoint Zero;
        public CCPoint(CocosSharp.CCPoint pt) { throw new System.NotImplementedException(); }
        public CCPoint(CocosSharp.CCVector2 v) { throw new System.NotImplementedException(); }
        public CCPoint(float x, float y) { throw new System.NotImplementedException(); }
        public float Angle { get { return default(float); } }
        public CocosSharp.CCPoint InvertX { get { return default(CocosSharp.CCPoint); } }
        public CocosSharp.CCPoint InvertY { get { return default(CocosSharp.CCPoint); } }
        public float Length { get { return default(float); } }
        public float LengthSquared { get { return default(float); } }
        public static float AngleSigned(CocosSharp.CCPoint a, CocosSharp.CCPoint b) { return default(float); }
        public static CocosSharp.CCPoint Clamp(CocosSharp.CCPoint p, CocosSharp.CCPoint from, CocosSharp.CCPoint to) { return default(CocosSharp.CCPoint); }
        public static CocosSharp.CCPoint Clamp(CocosSharp.CCPoint p, float len) { return default(CocosSharp.CCPoint); }
        public CocosSharp.CCPoint Clamp(float len) { return default(CocosSharp.CCPoint); }
        public static float Clamp(float value, float min_inclusive, float max_inclusive) { return default(float); }
        public static CocosSharp.CCPoint ComputationOperation(CocosSharp.CCPoint p, CocosSharp.CCPoint.ComputationOperationDelegate del) { return default(CocosSharp.CCPoint); }
        public static float CrossProduct(CocosSharp.CCPoint v1, CocosSharp.CCPoint v2) { return default(float); }
        public static float Distance(CocosSharp.CCPoint v1, CocosSharp.CCPoint v2) { return default(float); }
        public float DistanceSquared(ref CocosSharp.CCPoint v2) { return default(float); }
        public static float Dot(CocosSharp.CCPoint p1, CocosSharp.CCPoint p2) { return default(float); }
        public static bool Equal(ref CocosSharp.CCPoint point1, ref CocosSharp.CCPoint point2) { return default(bool); }
        public bool Equals(CocosSharp.CCPoint p) { return default(bool); }
        public override bool Equals(object obj) { return default(bool); }
        public static CocosSharp.CCPoint ForAngle(float a) { return default(CocosSharp.CCPoint); }
        public static bool FuzzyEqual(CocosSharp.CCPoint a, CocosSharp.CCPoint b, float variance) { return default(bool); }
        public override int GetHashCode() { return default(int); }
        public static CocosSharp.CCPoint IntersectPoint(CocosSharp.CCPoint A, CocosSharp.CCPoint B, CocosSharp.CCPoint C, CocosSharp.CCPoint D) { return default(CocosSharp.CCPoint); }
        public static bool IsNear(CocosSharp.CCPoint p1, CocosSharp.CCPoint p2, float dist) { return default(bool); }
        public bool IsNear(CocosSharp.CCPoint p2, float dist) { return default(bool); }
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
        public static CocosSharp.CCPoint PerpendicularCCW(CocosSharp.CCPoint p) { return default(CocosSharp.CCPoint); }
        public static CocosSharp.CCPoint PerpendicularCW(CocosSharp.CCPoint p) { return default(CocosSharp.CCPoint); }
        public static CocosSharp.CCPoint Project(CocosSharp.CCPoint v1, CocosSharp.CCPoint v2) { return default(CocosSharp.CCPoint); }
        public static CocosSharp.CCPoint Rotate(CocosSharp.CCPoint v1, CocosSharp.CCPoint v2) { return default(CocosSharp.CCPoint); }
        public static CocosSharp.CCPoint RotateByAngle(CocosSharp.CCPoint v, CocosSharp.CCPoint pivot, float angle) { return default(CocosSharp.CCPoint); }
        public static bool SegmentIntersect(CocosSharp.CCPoint A, CocosSharp.CCPoint B, CocosSharp.CCPoint C, CocosSharp.CCPoint D) { return default(bool); }
        public CocosSharp.CCPoint Sub(ref CocosSharp.CCPoint v2) { return default(CocosSharp.CCPoint); }
        public static float ToAngle(CocosSharp.CCPoint v) { return default(float); }
        public override string ToString() { return default(string); }
        public static CocosSharp.CCPoint Unrotate(CocosSharp.CCPoint v1, CocosSharp.CCPoint v2) { return default(CocosSharp.CCPoint); }
        public delegate float ComputationOperationDelegate(float a);
    }
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public partial struct CCPoint3 {
        public float X;
        public float Y;
        public float Z;
        public static readonly CocosSharp.CCPoint3 Zero;
        public CCPoint3(CocosSharp.CCPoint xy, float z) { throw new System.NotImplementedException(); }
        public CCPoint3(float x, float y, float z) { throw new System.NotImplementedException(); }
        public float Length { get { return default(float); } }
        public static bool operator ==(CocosSharp.CCPoint3 p1, CocosSharp.CCPoint3 p2) { return default(bool); }
        public static bool operator !=(CocosSharp.CCPoint3 p1, CocosSharp.CCPoint3 p2) { return default(bool); }
        public static CocosSharp.CCPoint3 operator -(CocosSharp.CCPoint3 p1, CocosSharp.CCPoint3 p2) { return default(CocosSharp.CCPoint3); }
    }
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public partial struct CCPointI {
        public int X;
        public int Y;
        public CCPointI(int x, int y) { throw new System.NotImplementedException(); }
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
        public CCPointObject(CocosSharp.CCPoint ratio, CocosSharp.CCPoint offset) { }
        public CocosSharp.CCNode Child { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCNode); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public CocosSharp.CCPoint Offset { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCPoint); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public CocosSharp.CCPoint Ratio { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCPoint); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    }
    public partial class CCPointSprite {
        public CCPointSprite() { }
        public CocosSharp.CCColor4B Color { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCColor4B); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public CocosSharp.CCVertex2F Position { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCVertex2F); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public float Size { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    }
    public enum CCPositionType {
        Free = 0,
        Grouped = 2,
        Relative = 1,
    }
    public partial class CCPrimitiveBatch : System.IDisposable {
        internal CCPrimitiveBatch() { }
        public void AddVertex(CocosSharp.CCVector2 vertex, CocosSharp.CCColor4B color, Microsoft.Xna.Framework.Graphics.PrimitiveType primitiveType) { }
        public void AddVertex(ref CocosSharp.CCVector2 vertex, CocosSharp.CCColor4B color, Microsoft.Xna.Framework.Graphics.PrimitiveType primitiveType) { }
        public void Begin() { }
        public void Dispose() { }
        protected virtual void Dispose(bool disposing) { }
        public void End() { }
        public bool IsReady() { return default(bool); }
        public void SetProjection(ref Microsoft.Xna.Framework.Matrix projection) { }
        public void UpdateMatrix() { }
    }
    public partial class CCProgressFromTo : CocosSharp.CCFiniteTimeAction {
        public CCProgressFromTo(float duration, float fromPercentage, float toPercentage) { }
        public float PercentFrom { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]protected set { } }
        public float PercentTo { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]protected set { } }
        public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public partial class CCProgressTimer : CocosSharp.CCNode {
        public CCProgressTimer(CocosSharp.CCSprite sp) { }
        public CCProgressTimer(string fileName) { }
        public CocosSharp.CCPoint BarChangeRate { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCPoint); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public override CocosSharp.CCColor3B Color { get { return default(CocosSharp.CCColor3B); } set { } }
        public override bool IsColorModifiedByOpacity { get { return default(bool); } set { } }
        public CocosSharp.CCPoint Midpoint { get { return default(CocosSharp.CCPoint); } set { } }
        public override byte Opacity { get { return default(byte); } set { } }
        public float Percentage { get { return default(float); } set { } }
        public bool ReverseDirection { get { return default(bool); } set { } }
        public CocosSharp.CCSprite Sprite { get { return default(CocosSharp.CCSprite); } set { } }
        public CocosSharp.CCProgressTimerType Type { get { return default(CocosSharp.CCProgressTimerType); } set { } }
        protected override void Draw() { }
        public override void UpdateColor() { }
    }
    public enum CCProgressTimerType {
        Bar = 1,
        Radial = 0,
    }
    public partial class CCProgressTo : CocosSharp.CCProgressFromTo {
        public CCProgressTo(float duration, float percentTo) : base (default(float), default(float), default(float)) { }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public partial struct CCQuad3 {
        public CocosSharp.CCVertex3F BottomLeft;
        public CocosSharp.CCVertex3F BottomRight;
        public CocosSharp.CCVertex3F TopLeft;
        public CocosSharp.CCVertex3F TopRight;
        public CCQuad3(System.Nullable<CocosSharp.CCVertex3F> tL=null, System.Nullable<CocosSharp.CCVertex3F> tR=null, System.Nullable<CocosSharp.CCVertex3F> bL=null, System.Nullable<CocosSharp.CCVertex3F> bR=null) { throw new System.NotImplementedException(); }
    }
    public partial class CCRandom {
        public CCRandom() { }
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
        public CCRawList(bool useArrayPool=false) { }
        public CCRawList(System.Collections.Generic.IList<T> elements, bool useArrayPool=false) { }
        public CCRawList(int initialCapacity, bool useArrayPool=false) { }
        public int Capacity { get { return default(int); } set { } }
        public int Count { get { return default(int); } set { } }
        public T[] Elements { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(T[]); } }
        public T this[int index] { get { return default(T); } set { } }
        bool System.Collections.Generic.ICollection<T>.IsReadOnly { get { return default(bool); } }
        public bool UseArrayPool { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
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
        public void FastRemove(int index) { }
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
        [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public partial struct Enumerator : System.Collections.Generic.IEnumerator<T>, System.Collections.IEnumerator, System.IDisposable {
            public Enumerator(CocosSharp.CCRawList<T> list) { throw new System.NotImplementedException(); }
            public T Current { get { return default(T); } }
            object System.Collections.IEnumerator.Current { get { return default(object); } }
            public void Dispose() { }
            public bool MoveNext() { return default(bool); }
            public void Reset() { }
        }
    }
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public partial struct CCRect {
        public CocosSharp.CCPoint Origin;
        public CocosSharp.CCSize Size;
        public static readonly CocosSharp.CCRect Zero;
        public CCRect(float x, float y, float width, float height) { throw new System.NotImplementedException(); }
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
        public CCRemoveSelf() { }
        public CCRemoveSelf(bool isNeedCleanUp) { }
        public bool IsNeedCleanUp { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } }
        public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public enum CCRenderTargetUsage {
        DiscardContents = 0,
        PlatformContents = 2,
        PreserveContents = 1,
    }
    public partial class CCRenderTexture : CocosSharp.CCNode {
        public CCRenderTexture() { }
        public CCRenderTexture(CocosSharp.CCSize contentSize, CocosSharp.CCSize textureSizeInPixels, CocosSharp.CCSurfaceFormat colorFormat=(CocosSharp.CCSurfaceFormat)(0), CocosSharp.CCDepthFormat depthFormat=(CocosSharp.CCDepthFormat)(-1), CocosSharp.CCRenderTargetUsage usage=(CocosSharp.CCRenderTargetUsage)(0)) { }
        protected CocosSharp.CCSurfaceFormat PixelFormat { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCSurfaceFormat); } }
        public CocosSharp.CCSprite Sprite { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCSprite); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public CocosSharp.CCTexture2D Texture { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCTexture2D); } }
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
    public partial class CCRepeat : CocosSharp.CCFiniteTimeAction {
        public CCRepeat(CocosSharp.CCFiniteTimeAction action, uint times) { }
        public bool ActionInstant { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } }
        public CocosSharp.CCFiniteTimeAction InnerAction { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCFiniteTimeAction); } }
        public uint Times { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(uint); } }
        public uint Total { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(uint); } }
        public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public partial class CCRepeatForever : CocosSharp.CCFiniteTimeAction {
        public CCRepeatForever(CocosSharp.CCFiniteTimeAction action) { }
        public CCRepeatForever(params CocosSharp.CCFiniteTimeAction[] actions) { }
        public CocosSharp.CCFiniteTimeAction InnerAction { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCFiniteTimeAction); } }
        public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public enum CCResolutionPolicy {
        Custom = 0,
        ExactFit = 1,
        FixedHeight = 4,
        FixedWidth = 5,
        NoBorder = 2,
        ShowAll = 3,
    }
    public abstract partial class CCReusedObject<T> where T : CocosSharp.CCReusedObject<T>, new() {
        protected CCReusedObject() { }
        public static T Create() { return default(T); }
        public void Free() { }
        protected abstract void PrepareForReuse();
    }
    public partial class CCReuseGrid : CocosSharp.CCActionInstant {
        public CCReuseGrid() { }
        public CCReuseGrid(int times) { }
        public int Times { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public partial class CCReverseTime : CocosSharp.CCFiniteTimeAction {
        public CCReverseTime(CocosSharp.CCFiniteTimeAction action) { }
        public CocosSharp.CCFiniteTimeAction Other { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCFiniteTimeAction); } }
        public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public partial class CCRipple3D : CocosSharp.CCGrid3DAction {
        public CCRipple3D(float duration, CocosSharp.CCGridSize gridSize) : base (default(float)) { }
        public CCRipple3D(float duration, CocosSharp.CCGridSize gridSize, CocosSharp.CCPoint position, float radius, int waves, float amplitude) : base (default(float)) { }
        public CocosSharp.CCPoint Position { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCPoint); } }
        public float Radius { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } }
        public int Waves { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public partial class CCRotateBy : CocosSharp.CCFiniteTimeAction {
        public CCRotateBy(float duration, float fDeltaAngle) { }
        public CCRotateBy(float duration, float fDeltaAngleX, float fDeltaAngleY) { }
        public float AngleX { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } }
        public float AngleY { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } }
        public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public partial class CCRotateTo : CocosSharp.CCFiniteTimeAction {
        public CCRotateTo(float duration, float fDeltaAngle) { }
        public CCRotateTo(float duration, float fDeltaAngleX, float fDeltaAngleY) { }
        public float DistanceAngleX { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } }
        public float DistanceAngleY { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } }
        public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public partial class CCScaleBy : CocosSharp.CCScaleTo {
        public CCScaleBy(float duration, float s) : base (default(float), default(float)) { }
        public CCScaleBy(float duration, float sx, float sy) : base (default(float), default(float)) { }
        public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public partial class CCScaleTo : CocosSharp.CCFiniteTimeAction {
        public CCScaleTo(float duration, float s) { }
        public CCScaleTo(float duration, float sx, float sy) { }
        public float EndScaleX { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } }
        public float EndScaleY { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } }
        public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public partial class CCScene : CocosSharp.CCNode {
        public CCScene(CocosSharp.CCScene scene) { }
        public CCScene(CocosSharp.CCWindow window) { }
        public CCScene(CocosSharp.CCWindow window, CocosSharp.CCDirector director) { }
        public CCScene(CocosSharp.CCWindow window, CocosSharp.CCViewport viewport, CocosSharp.CCDirector director) { }
        public override CocosSharp.CCCamera Camera { get { return default(CocosSharp.CCCamera); } set { } }
        public override CocosSharp.CCSize ContentSize { get { return default(CocosSharp.CCSize); } set { } }
        public override CocosSharp.CCDirector Director { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCDirector); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public virtual bool IsTransition { get { return default(bool); } }
        public CocosSharp.CCResolutionPolicy ResolutionPolicy { get { return default(CocosSharp.CCResolutionPolicy); } set { } }
        public override CocosSharp.CCScene Scene { get { return default(CocosSharp.CCScene); } }
        public override CocosSharp.CCViewport Viewport { get { return default(CocosSharp.CCViewport); } set { } }
        public CocosSharp.CCRect VisibleBoundsScreenspace { get { return default(CocosSharp.CCRect); } }
        public override CocosSharp.CCWindow Window { get { return default(CocosSharp.CCWindow); } set { } }
        protected override void Draw() { }
    }
    public static partial class CCSchedulePriority {
        public const uint RepeatForever = (uint)4294967294;
        public const int System = -2147483648;
        public const int User = -2147483647;
    }
    public partial class CCScheduler {
        internal CCScheduler() { }
        public bool IsActionManagerActive { get { return default(bool); } }
        public float TimeScale { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
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
        public CCScriptEngineManager() { }
        public CocosSharp.ICCScriptingEngine ScriptEngine { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.ICCScriptingEngine); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public static CocosSharp.CCScriptEngineManager SharedScriptEngineManager { get { return default(CocosSharp.CCScriptEngineManager); } }
        public void RemoveScriptEngine() { }
    }
    public partial class CCSequence : CocosSharp.CCFiniteTimeAction {
        public CCSequence(CocosSharp.CCFiniteTimeAction action1, CocosSharp.CCFiniteTimeAction action2) { }
        public CCSequence(params CocosSharp.CCFiniteTimeAction[] actions) { }
        public CocosSharp.CCFiniteTimeAction[] Actions { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCFiniteTimeAction[]); } }
        public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public static partial class CCSerialization {
    }
    public partial class CCShaky3D : CocosSharp.CCGrid3DAction {
        public CCShaky3D(float duration, CocosSharp.CCGridSize gridSize, int range=0, bool shakeZ=true) : base (default(float)) { }
        protected internal int Range { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } }
        protected internal bool Shake { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public partial class CCShakyTiles3D : CocosSharp.CCTiledGrid3DAction {
        public CCShakyTiles3D(float duration, CocosSharp.CCGridSize gridSize, int range=0, bool shakeZ=true) : base (default(float)) { }
        protected internal int Range { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } }
        protected internal bool ShakeZ { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public partial class CCShatteredTiles3D : CocosSharp.CCTiledGrid3DAction {
        public CCShatteredTiles3D(float duration, CocosSharp.CCGridSize gridSize, int range=0, bool shatterZ=true) : base (default(float)) { }
        protected internal int Range { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } }
        protected internal bool ShatterZ { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public partial class CCShow : CocosSharp.CCActionInstant {
        public CCShow() { }
        public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public partial class CCShuffleTiles : CocosSharp.CCTiledGrid3DAction {
        protected internal const int NoSeedSpecified = -1;
        public CCShuffleTiles(CocosSharp.CCGridSize gridSize, float duration, int seed=-1) : base (default(float)) { }
        protected internal int Seed { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public partial struct CCSize {
        public float Height;
        public float Width;
        public static readonly CocosSharp.CCSize Zero;
        public CCSize(float width, float height) { throw new System.NotImplementedException(); }
        public CocosSharp.CCPoint Center { get { return default(CocosSharp.CCPoint); } }
        public CocosSharp.CCSize Inverted { get { return default(CocosSharp.CCSize); } }
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
        public CCSizeI(int width, int height) { throw new System.NotImplementedException(); }
        public int Height { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public int Width { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public static implicit operator CocosSharp.CCSize (CocosSharp.CCSizeI p) { return default(CocosSharp.CCSize); }
    }
    public partial class CCSkewBy : CocosSharp.CCSkewTo {
        public CCSkewBy(float t, float deltaSkewXY) : base (default(float), default(float), default(float)) { }
        public CCSkewBy(float t, float deltaSkewX, float deltaSkewY) : base (default(float), default(float), default(float)) { }
        public float SkewByX { get { return default(float); } }
        public float SkewByY { get { return default(float); } }
        public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public partial class CCSkewTo : CocosSharp.CCFiniteTimeAction {
        protected float EndSkewX;
        protected float EndSkewY;
        protected float SkewX;
        protected float SkewY;
        public CCSkewTo(float t, float skewXY) { }
        public CCSkewTo(float t, float sx, float sy) { }
        public float SkewToX { get { return default(float); } }
        public float SkewToY { get { return default(float); } }
        public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public partial class CCSpawn : CocosSharp.CCFiniteTimeAction {
        protected CCSpawn(CocosSharp.CCFiniteTimeAction action1, CocosSharp.CCFiniteTimeAction action2) { }
        public CCSpawn(params CocosSharp.CCFiniteTimeAction[] actions) { }
        public CocosSharp.CCFiniteTimeAction ActionOne { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCFiniteTimeAction); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]protected set { } }
        public CocosSharp.CCFiniteTimeAction ActionTwo { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCFiniteTimeAction); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]protected set { } }
        public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public partial class CCSpeed : CocosSharp.CCAction {
        public CCSpeed(CocosSharp.CCFiniteTimeAction action, float speed) { }
        protected internal CocosSharp.CCFiniteTimeAction InnerAction { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCFiniteTimeAction); } }
        public float Speed { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } }
        public virtual CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public partial class CCSplitCols : CocosSharp.CCTiledGrid3DAction {
        public CCSplitCols(float duration, int nCols) : base (default(float)) { }
        protected internal int Columns { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public partial class CCSplitRows : CocosSharp.CCTiledGrid3DAction {
        public CCSplitRows(float duration, int nRows) : base (default(float)) { }
        protected internal int Rows { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public partial class CCSprite : CocosSharp.CCNode, CocosSharp.ICCBlendable, CocosSharp.ICCTexture {
        protected CocosSharp.CCV3F_C4B_T2F_Quad quad;
        public CCSprite() { }
        public CCSprite(CocosSharp.CCSize contentSize, CocosSharp.CCSpriteFrame spriteFrame) { }
        public CCSprite(CocosSharp.CCSpriteFrame spriteFrame) { }
        public CCSprite(CocosSharp.CCTexture2D texture=null, System.Nullable<CocosSharp.CCRect> texRectInPixels=null, bool rotated=false) { }
        public CCSprite(string fileName, System.Nullable<CocosSharp.CCRect> texRectInPixels=null) { }
        public int AtlasIndex { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } }
        public CocosSharp.CCSpriteBatchNode BatchNode { get { return default(CocosSharp.CCSpriteBatchNode); } set { } }
        public CocosSharp.CCBlendFunc BlendFunc { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCBlendFunc); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public override CocosSharp.CCColor3B Color { get { return default(CocosSharp.CCColor3B); } set { } }
        public override CocosSharp.CCSize ContentSize { get { return default(CocosSharp.CCSize); } set { } }
        public bool FlipX { get { return default(bool); } set { } }
        public bool FlipY { get { return default(bool); } set { } }
        public bool IsAntialiased { get { return default(bool); } set { } }
        public override bool IsColorModifiedByOpacity { get { return default(bool); } set { } }
        public bool IsTextureRectRotated { get { return default(bool); } set { } }
        public override byte Opacity { get { return default(byte); } set { } }
        public override CocosSharp.CCPoint Position { get { return default(CocosSharp.CCPoint); } set { } }
        protected internal CocosSharp.CCV3F_C4B_T2F_Quad Quad { get { return default(CocosSharp.CCV3F_C4B_T2F_Quad); } }
        public override float Rotation { set { } }
        public override float RotationX { get { return default(float); } set { } }
        public override float RotationY { get { return default(float); } set { } }
        public override float ScaleX { get { return default(float); } set { } }
        public override float ScaleY { get { return default(float); } set { } }
        public override float SkewX { get { return default(float); } set { } }
        public override float SkewY { get { return default(float); } set { } }
        public CocosSharp.CCSpriteFrame SpriteFrame { get { return default(CocosSharp.CCSpriteFrame); } set { } }
        public virtual CocosSharp.CCTexture2D Texture { get { return default(CocosSharp.CCTexture2D); } set { } }
        public CocosSharp.CCRect TextureRectInPixels { get { return default(CocosSharp.CCRect); } set { } }
        protected internal CocosSharp.CCV3F_C4B_T2F_Quad TransformedQuad { get { return default(CocosSharp.CCV3F_C4B_T2F_Quad); } }
        public override void AddChild(CocosSharp.CCNode child, int zOrder, int tag) { }
        public override void Deserialize(System.IO.Stream stream) { }
        protected override void Draw() { }
        public bool IsSpriteFrameDisplayed(CocosSharp.CCSpriteFrame frame) { return default(bool); }
        public override void RemoveAllChildren(bool cleanup) { }
        public override void RemoveChild(CocosSharp.CCNode child, bool cleanup) { }
        public override void ReorderChild(CocosSharp.CCNode child, int zOrder) { }
        public virtual void ScaleTo(CocosSharp.CCSize size) { }
        public override void Serialize(System.IO.Stream stream) { }
        public virtual void SetReorderChildDirtyRecursively() { }
        public void SetSpriteFrameWithAnimationName(string animationName, int frameIndex) { }
        public override void SortAllChildren() { }
        protected void UpdateBlendFunc() { }
        public override void UpdateColor() { }
    }
    public partial class CCSpriteBatchNode : CocosSharp.CCNode, CocosSharp.ICCBlendable, CocosSharp.ICCTexture {
        public CCSpriteBatchNode() { }
        public CCSpriteBatchNode(CocosSharp.CCTexture2D tex, int capacity=29) { }
        public CCSpriteBatchNode(string fileImage, int capacity=29) { }
        public override CocosSharp.CCAffineTransform AffineLocalTransform { get { return default(CocosSharp.CCAffineTransform); } }
        public CocosSharp.CCBlendFunc BlendFunc { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCBlendFunc); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public override CocosSharp.CCSize ContentSize { get { return default(CocosSharp.CCSize); } set { } }
        public CocosSharp.CCRawList<CocosSharp.CCSprite> Descendants { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCRawList<CocosSharp.CCSprite>); } }
        public bool IsAntialiased { get { return default(bool); } set { } }
        public virtual CocosSharp.CCTexture2D Texture { get { return default(CocosSharp.CCTexture2D); } set { } }
        public CocosSharp.CCTextureAtlas TextureAtlas { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCTextureAtlas); } }
        public override void AddChild(CocosSharp.CCNode child, int zOrder=0, int tag=-1) { }
        protected CocosSharp.CCSpriteBatchNode AddSpriteWithoutQuad(CocosSharp.CCSprite child, int z, int aTag) { return default(CocosSharp.CCSpriteBatchNode); }
        public void AppendChild(CocosSharp.CCSprite sprite) { }
        public int AtlasIndexForChild(CocosSharp.CCSprite pobSprite, int nZ) { return default(int); }
        protected override void Draw() { }
        public int HighestAtlasIndexInChild(CocosSharp.CCSprite pSprite) { return default(int); }
        public void IncreaseAtlasCapacity() { }
        protected void InitCCSpriteBatchNode(CocosSharp.CCTexture2D tex, int capacity=29) { }
        public void InsertChild(CocosSharp.CCSprite sprite, int uIndex) { }
        protected void InsertQuadFromSprite(CocosSharp.CCSprite sprite, int index) { }
        public int LowestAtlasIndexInChild(CocosSharp.CCSprite pSprite) { return default(int); }
        public int RebuildIndexInOrder(CocosSharp.CCSprite pobParent, int uIndex) { return default(int); }
        public override void RemoveAllChildren(bool cleanup) { }
        public override void RemoveChild(CocosSharp.CCNode child, bool cleanup) { }
        public void RemoveChildAtIndex(int index, bool doCleanup) { }
        public void RemoveSpriteFromAtlas(CocosSharp.CCSprite sprite) { }
        public void ReorderBatch(bool reorder) { }
        public override void ReorderChild(CocosSharp.CCNode child, int zOrder) { }
        public override void SortAllChildren() { }
        protected void UpdateQuadFromSprite(CocosSharp.CCSprite sprite, int index) { }
        public override void Visit() { }
    }
    public partial class CCSpriteFontCache {
        internal CCSpriteFontCache() { }
        public static string DefaultFont;
        public static string FontRoot;
        public static float FontScale { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public Microsoft.Xna.Framework.Graphics.SpriteFont this[string fontName] { get { return default(Microsoft.Xna.Framework.Graphics.SpriteFont); } }
        public static CocosSharp.CCSpriteFontCache SharedInstance { get { return default(CocosSharp.CCSpriteFontCache); } }
        public void Clear() { }
        public static void RegisterFont(string fontName, params System.Int32[] sizes) { }
    }
    public partial class CCSpriteFrame {
        public CCSpriteFrame() { }
        public CCSpriteFrame(CocosSharp.CCSize contentSize, CocosSharp.CCTexture2D texture, CocosSharp.CCRect textureRectInPxls, CocosSharp.CCSize originalSizeInPxls, bool rotated=false, System.Nullable<CocosSharp.CCPoint> offsetInPxls=null) { }
        public CCSpriteFrame(CocosSharp.CCSize contentSize, CocosSharp.CCTexture2D texture, CocosSharp.CCRect textureRectInPxls, bool rotated=false, System.Nullable<CocosSharp.CCPoint> offsetInPxls=null) { }
        protected CCSpriteFrame(CocosSharp.CCSpriteFrame spriteFrame) { }
        public CCSpriteFrame(CocosSharp.CCTexture2D texture, CocosSharp.CCRect rectInPxls) { }
        public CocosSharp.CCSize ContentSize { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCSize); } }
        public bool IsRotated { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public CocosSharp.CCPoint OffsetInPixels { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCPoint); } }
        public CocosSharp.CCSize OriginalSizeInPixels { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCSize); } }
        public CocosSharp.CCTexture2D Texture { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCTexture2D); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public string TextureFilename { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(string); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public CocosSharp.CCRect TextureRectInPixels { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCRect); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    }
    public partial class CCSpriteFrameCache {
        public CCSpriteFrameCache() { }
        public bool AllowFrameOverwrite { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public CocosSharp.CCSpriteFrame this[string name] { get { return default(CocosSharp.CCSpriteFrame); } }
        public static CocosSharp.CCSpriteFrameCache SharedSpriteFrameCache { get { return default(CocosSharp.CCSpriteFrameCache); } }
        public void AddSpriteFrame(CocosSharp.CCSpriteFrame frame, string frameName) { }
        public void AddSpriteFrames(System.IO.Stream plist, CocosSharp.CCTexture2D pobTexture) { }
        public void AddSpriteFrames(string plistFileName) { }
        public void AddSpriteFrames(string plistFileName, CocosSharp.CCTexture2D pobTexture) { }
        public void AddSpriteFrames(string plistFileName, string textureFileName) { }
        public static void PurgeSharedSpriteFrameCache() { }
        public void RemoveSpriteFrame(string frameName) { }
        public void RemoveSpriteFrames() { }
        public void RemoveSpriteFrames(CocosSharp.CCTexture2D texture) { }
        public void RemoveSpriteFrames(CocosSharp.PlistDictionary dictionary) { }
        public void RemoveSpriteFrames(string plistFileName) { }
        public void RemoveUnusedSpriteFrames() { }
    }
    public partial class CCSpriteSheet {
        public CCSpriteSheet(System.Collections.Generic.Dictionary<System.String, CocosSharp.CCSpriteFrame> frames) { }
        public CCSpriteSheet(System.IO.Stream stream, CocosSharp.CCTexture2D texture) { }
        public CCSpriteSheet(System.IO.Stream stream, string textureFileName) { }
        public CCSpriteSheet(string fileName) { }
        public CCSpriteSheet(string fileName, CocosSharp.CCTexture2D texture) { }
        public CCSpriteSheet(string fileName, string textureFileName) { }
        public System.Collections.Generic.List<CocosSharp.CCSpriteFrame> Frames { get { return default(System.Collections.Generic.List<CocosSharp.CCSpriteFrame>); } }
        public CocosSharp.CCSpriteFrame this[string name] { get { return default(CocosSharp.CCSpriteFrame); } }
    }
    public partial class CCSpriteSheetCache {
        public CCSpriteSheetCache() { }
        public static CocosSharp.CCSpriteSheetCache Instance { get { return default(CocosSharp.CCSpriteSheetCache); } }
        public CocosSharp.CCSpriteSheet this[string name] { get { return default(CocosSharp.CCSpriteSheet); } }
        public CocosSharp.CCSpriteSheet AddSpriteSheet(string fileName) { return default(CocosSharp.CCSpriteSheet); }
        public CocosSharp.CCSpriteSheet AddSpriteSheet(string fileName, CocosSharp.CCTexture2D texture) { return default(CocosSharp.CCSpriteSheet); }
        public CocosSharp.CCSpriteSheet AddSpriteSheet(string fileName, string textureFileName) { return default(CocosSharp.CCSpriteSheet); }
        public static void DestroyInstance() { }
        public void Remove(CocosSharp.CCSpriteSheet spriteSheet) { }
        public void Remove(string name) { }
        public void RemoveAll() { }
        public void RemoveUnused() { }
    }
    public partial class CCStats {
        public CCStats() { }
        public bool IsEnabled { get { return default(bool); } set { } }
        public bool IsInitialized { get { return default(bool); } }
        public void Draw(CocosSharp.CCWindow window) { }
        public void Initialize() { }
        public void UpdateEnd(float delta) { }
        public void UpdateStart() { }
    }
    public partial class CCStopGrid : CocosSharp.CCActionInstant {
        public CCStopGrid() { }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public enum CCSurfaceFormat {
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
    public partial class CCTargetedAction : CocosSharp.CCFiniteTimeAction {
        public CCTargetedAction(CocosSharp.CCNode target, CocosSharp.CCFiniteTimeAction pAction) { }
        public CocosSharp.CCNode ForcedTarget { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCNode); } }
        public CocosSharp.CCFiniteTimeAction TargetedAction { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCFiniteTimeAction); } }
        public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public static partial class CCTask {
        public static object RunAsync(CocosSharp.CCScheduler scheduler, System.Action action, System.Action<System.Object> taskCompleted=null) { return default(object); }
    }
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Size=1)]
    public partial struct CCTex2F {
        public CCTex2F(float u, float v) { throw new System.NotImplementedException(); }
        public float U { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public float V { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public override string ToString() { return default(string); }
    }
    public enum CCTextAlignment {
        Center = 1,
        Left = 0,
        Right = 2,
    }
    public partial class CCTextFieldTTF : CocosSharp.CCLabelTtf {
        public CCTextFieldTTF(string text, string fontName, float fontSize) { }
        public CCTextFieldTTF(string text, string fontName, float fontSize, CocosSharp.CCSize dimensions, CocosSharp.CCTextAlignment hAlignment) { }
        public CCTextFieldTTF(string text, string fontName, float fontSize, CocosSharp.CCSize dimensions, CocosSharp.CCTextAlignment hAlignment, CocosSharp.CCVerticalTextAlignment vAlignment) { }
        public bool AutoEdit { get { return default(bool); } set { } }
        public string EditDescription { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(string); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public string EditTitle { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(string); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public bool ReadOnly { get { return default(bool); } set { } }
        public event CocosSharp.CCTextFieldTTFDelegate BeginEditing { add { } remove { } }
        public event CocosSharp.CCTextFieldTTFDelegate EndEditing { add { } remove { } }
        protected virtual void DoBeginEditing(ref string newText, ref bool canceled) { }
        protected virtual void DoEndEditing(ref string newText, ref bool canceled) { }
        public void Edit() { }
        public void Edit(string title, string defaultText) { }
        public void EndEdit() { }
        public override void OnEnter() { }
        public override void OnExit() { }
        public bool TouchBegan(CocosSharp.CCTouch touch) { return default(bool); }
        public void TouchCancelled(CocosSharp.CCTouch pTouch) { }
        public void TouchEnded(CocosSharp.CCTouch touch) { }
        public void TouchMoved(CocosSharp.CCTouch touch) { }
    }
    public delegate void CCTextFieldTTFDelegate(object sender, ref string text, ref bool canceled);
    public partial class CCTexture2D : CocosSharp.CCGraphicsResource {
        public static CocosSharp.CCSurfaceFormat DefaultAlphaPixelFormat;
        public static bool DefaultIsAntialiased;
        public static bool OptimizeForPremultipliedAlpha;
        public CCTexture2D() { }
        public CCTexture2D(Microsoft.Xna.Framework.Graphics.Texture2D texture) { }
        public CCTexture2D(Microsoft.Xna.Framework.Graphics.Texture2D texture, CocosSharp.CCSurfaceFormat format, bool premultipliedAlpha=true, bool managed=false) { }
        public CCTexture2D(System.Byte[] data, CocosSharp.CCSurfaceFormat pixelFormat=(CocosSharp.CCSurfaceFormat)(0), bool mipMap=false) { }
        public CCTexture2D(int pixelsWide, int pixelsHigh, CocosSharp.CCSurfaceFormat pixelFormat=(CocosSharp.CCSurfaceFormat)(0), bool premultipliedAlpha=true, bool mipMap=false) { }
        public CCTexture2D(System.IO.Stream stream, CocosSharp.CCSurfaceFormat pixelFormat=(CocosSharp.CCSurfaceFormat)(0)) { }
        public CCTexture2D(string file) { }
        public CCTexture2D(string text, CocosSharp.CCSize dimensions, CocosSharp.CCTextAlignment hAlignment, CocosSharp.CCVerticalTextAlignment vAlignment, string fontName, float fontSize) { }
        public CCTexture2D(string text, string fontName, float fontSize) { }
        public uint BitsPerPixelForFormat { get { return default(uint); } }
        public CocosSharp.CCSize ContentSizeInPixels { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCSize); } }
        public bool HasPremultipliedAlpha { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } }
        public bool IsAntialiased { get { return default(bool); } set { } }
        public bool IsTextureDefined { get { return default(bool); } }
        public Microsoft.Xna.Framework.Graphics.Texture2D Name { get { return default(Microsoft.Xna.Framework.Graphics.Texture2D); } }
        public CocosSharp.CCSurfaceFormat PixelFormat { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCSurfaceFormat); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public int PixelsHigh { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } }
        public int PixelsWide { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } }
        public Microsoft.Xna.Framework.Graphics.SamplerState SamplerState { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Graphics.SamplerState); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public Microsoft.Xna.Framework.Graphics.Texture2D XNATexture { get { return default(Microsoft.Xna.Framework.Graphics.Texture2D); } }
        public static CocosSharp.CCImageFormat DetectImageFormat(System.IO.Stream stream) { return default(CocosSharp.CCImageFormat); }
        protected override void Dispose(bool disposing) { }
        public void GenerateMipmap() { }
        public override void ReinitResource() { }
        public void SaveAsJpeg(System.IO.Stream stream, int width, int height) { }
        public void SaveAsPng(System.IO.Stream stream, int width, int height) { }
        public override string ToString() { return default(string); }
    }
    public partial class CCTextureAtlas {
        public CCTextureAtlas(CocosSharp.CCTexture2D texture, int capacity) { }
        public CCTextureAtlas(string file, int capacity) { }
        public int Capacity { get { return default(int); } set { } }
        protected internal bool Dirty { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public bool IsAntialiased { get { return default(bool); } set { } }
        protected internal CocosSharp.CCRawList<CocosSharp.CCV3F_C4B_T2F_Quad> Quads { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCRawList<CocosSharp.CCV3F_C4B_T2F_Quad>); } }
        public CocosSharp.CCTexture2D Texture { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCTexture2D); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public int TotalQuads { get { return default(int); } }
        public void DrawNumberOfQuads(int n) { }
        public void DrawNumberOfQuads(int n, int start) { }
        public void DrawQuads() { }
        public void FillWithEmptyQuadsFromIndex(int index, int amount) { }
        public void IncreaseTotalQuadsWith(int amount) { }
        public void InsertQuad(ref CocosSharp.CCV3F_C4B_T2F_Quad quad, int index) { }
        public void InsertQuadFromIndex(int oldIndex, int newIndex) { }
        public void MoveQuadsFromIndex(int index, int newIndex) { }
        public void MoveQuadsFromIndex(int oldIndex, int amount, int newIndex) { }
        public void RemoveAllQuads() { }
        public void RemoveQuadAtIndex(int index) { }
        public void RemoveQuadsAtIndex(int index, int amount) { }
        public void ResizeCapacity(int newCapacity) { }
        public override string ToString() { return default(string); }
        public void UpdateQuad(ref CocosSharp.CCV3F_C4B_T2F_Quad quad, int index) { }
    }
    public partial class CCTextureCache : CocosSharp.ICCUpdatable, System.IDisposable {
        protected System.Collections.Generic.Dictionary<System.String, CocosSharp.CCTexture2D> textures;
        public CCTextureCache(CocosSharp.CCApplication application) { }
        public CocosSharp.CCTexture2D this[string key] { get { return default(CocosSharp.CCTexture2D); } }
        public static CocosSharp.CCTextureCache SharedTextureCache { get { return default(CocosSharp.CCTextureCache); } }
        public CocosSharp.CCTexture2D AddImage(System.Byte[] data, string assetName, CocosSharp.CCSurfaceFormat format) { return default(CocosSharp.CCTexture2D); }
        public CocosSharp.CCTexture2D AddImage(string fileimage) { return default(CocosSharp.CCTexture2D); }
        public void AddImageAsync(System.Byte[] data, string assetName, CocosSharp.CCSurfaceFormat format, System.Action<CocosSharp.CCTexture2D> action) { }
        public void AddImageAsync(string fileimage, System.Action<CocosSharp.CCTexture2D> action) { }
        public CocosSharp.CCTexture2D AddRawImage<T>(T[] data, int width, int height, string assetName, CocosSharp.CCSurfaceFormat format, bool premultiplied, bool mipMap=false) where T : struct { return default(CocosSharp.CCTexture2D); }
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
        public void UnloadContent() { }
        public void Update(float dt) { }
    }
    public partial class CCTile {
        public CocosSharp.CCGridSize Delta;
        public CocosSharp.CCPoint Position;
        public CocosSharp.CCPoint StartPosition;
        public CCTile() { }
    }
    public partial class CCTiledGrid3D : CocosSharp.CCGridBase {
        public CCTiledGrid3D(CocosSharp.CCGridSize gridSize, CocosSharp.CCTexture2D texture, bool flipped=false) : base (default(CocosSharp.CCGridSize), default(CocosSharp.CCTexture2D), default(bool)) { }
        protected System.Int16[] Indices { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Int16[]); } }
        public CocosSharp.CCQuad3 this[CocosSharp.CCGridSize pos] { get { return default(CocosSharp.CCQuad3); } set { } }
        public CocosSharp.CCQuad3 this[int x, int y] { get { return default(CocosSharp.CCQuad3); } set { } }
        protected CocosSharp.CCQuad3[] OriginalVertices { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCQuad3[]); } }
        public override void Blit() { }
        public override void CalculateVertexPoints() { }
        public CocosSharp.CCQuad3 OriginalTile(CocosSharp.CCGridSize pos) { return default(CocosSharp.CCQuad3); }
        public CocosSharp.CCQuad3 OriginalTile(int x, int y) { return default(CocosSharp.CCQuad3); }
        public override void Reuse() { }
    }
    public partial class CCTiledGrid3DAction : CocosSharp.CCGridAction {
        public CCTiledGrid3DAction(float duration) : base (default(float)) { }
        public CCTiledGrid3DAction(float duration, CocosSharp.CCGridSize gridSize) : base (default(float)) { }
        protected CCTiledGrid3DAction(float duration, CocosSharp.CCGridSize gridSize, float amplitude) : base (default(float)) { }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public partial class CCTileMapAtlas : CocosSharp.CCAtlasNode {
        public CCTileMapAtlas(string tile, string mapFile, int tileWidth, int tileHeight) : base (default(string), default(int), default(int), default(int)) { }
        protected int NumOfItemsToRender { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } }
        protected System.Collections.Generic.Dictionary<CocosSharp.CCGridSize, System.Int32> PositionToAtlasIndex { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Collections.Generic.Dictionary<CocosSharp.CCGridSize, System.Int32>); } }
        protected override void AddedToNewScene() { }
        public void ReleaseMap() { }
        public void SetTile(CocosSharp.CCColor4B tile, CocosSharp.CCGridSize position) { }
        public CocosSharp.CCColor4B TileAt(CocosSharp.CCGridSize position) { return default(CocosSharp.CCColor4B); }
        public override void UpdateAtlasValues() { }
    }
    public partial class CCTintBy : CocosSharp.CCFiniteTimeAction {
        public CCTintBy(float duration, short deltaRed, short deltaGreen, short deltaBlue) { }
        public short DeltaB { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(short); } }
        public short DeltaG { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(short); } }
        public short DeltaR { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(short); } }
        public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public partial class CCTintTo : CocosSharp.CCFiniteTimeAction {
        public CCTintTo(float duration, byte red, byte green, byte blue) { }
        public CocosSharp.CCColor3B ColorTo { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCColor3B); } }
        public override CocosSharp.CCFiniteTimeAction Reverse() { return default(CocosSharp.CCFiniteTimeAction); }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public partial class CCTMXLayer : CocosSharp.CCSpriteBatchNode {
        public CCTMXLayer(CocosSharp.CCTMXTilesetInfo tileSetInfo, CocosSharp.CCTMXLayerInfo layerInfo, CocosSharp.CCTMXMapInfo mapInfo) { }
        public override CocosSharp.CCLayer Layer { get { return default(CocosSharp.CCLayer); } }
        public string LayerName { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(string); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public CocosSharp.CCTMXOrientation LayerOrientation { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCTMXOrientation); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public CocosSharp.CCSize LayerSize { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCSize); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public CocosSharp.CCSize MapTileSize { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCSize); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public System.Collections.Generic.Dictionary<System.String, System.String> Properties { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Collections.Generic.Dictionary<System.String, System.String>); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public override CocosSharp.CCScene Scene { get { return default(CocosSharp.CCScene); } }
        public System.UInt32[] Tiles { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.UInt32[]); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public CocosSharp.CCTMXTilesetInfo TileSet { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCTMXTilesetInfo); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public override void AddChild(CocosSharp.CCNode child, int zOrder, int tag) { }
        protected override void Draw() { }
        public virtual CocosSharp.CCPoint PositionAt(CocosSharp.CCPoint tileCoord) { return default(CocosSharp.CCPoint); }
        public virtual CocosSharp.CCPoint PositionAt(float x, float y) { return default(CocosSharp.CCPoint); }
        public virtual string PropertyNamed(string propertyName) { return default(string); }
        public virtual void ReleaseMap() { }
        public override void RemoveChild(CocosSharp.CCNode node, bool cleanup) { }
        public virtual void RemoveTileAt(CocosSharp.CCPoint pos) { }
        public virtual void SetTileGID(uint gid, CocosSharp.CCPoint pos) { }
        public virtual void SetTileGID(uint gid, CocosSharp.CCPoint pos, uint flags) { }
        public virtual CocosSharp.CCSprite TileAt(CocosSharp.CCPoint pos) { return default(CocosSharp.CCSprite); }
        public virtual CocosSharp.CCSprite TileAt(float x, float y) { return default(CocosSharp.CCSprite); }
        public virtual uint TileGIDAt(CocosSharp.CCPoint pos) { return default(uint); }
        public virtual uint TileGIDAt(CocosSharp.CCPoint pos, out uint flags) { flags = default(uint); return default(uint); }
        public virtual uint TileGIDAt(float x, float y) { return default(uint); }
        public virtual uint TileGIDAt(float x, float y, out uint flags) { flags = default(uint); return default(uint); }
    }
    public enum CCTMXLayerAttrib {
        Base64 = 2,
        Gzip = 4,
        None = 1,
        Zlib = 8,
    }
    public partial class CCTMXLayerInfo {
        public CCTMXLayerInfo() { }
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
    }
    public partial class CCTMXMapInfo {
        public CCTMXMapInfo(System.IO.StreamReader stream) { }
        public CCTMXMapInfo(string tmxFile) { }
        protected int LayerAttribs { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } }
        public CocosSharp.CCSize MapSize { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCSize); } }
        public int Orientation { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } }
        public uint ParentGID { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(uint); } }
        public bool StoringCharacters { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } }
        public CocosSharp.CCSize TileSize { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCSize); } }
        public string TMXFileName { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(string); } }
        public void EndElement(object ctx, string elementName) { }
        public void StartElement(object ctx, string name, System.String[] atts) { }
        public void TextHandler(object ctx, System.Byte[] ch, int len) { }
    }
    public partial class CCTMXObjectGroup {
        public CCTMXObjectGroup() { }
        public string GroupName { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(string); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public System.Collections.Generic.List<System.Collections.Generic.Dictionary<System.String, System.String>> Objects { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Collections.Generic.List<System.Collections.Generic.Dictionary<System.String, System.String>>); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public CocosSharp.CCPoint PositionOffset { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCPoint); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public System.Collections.Generic.Dictionary<System.String, System.String> Properties { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Collections.Generic.Dictionary<System.String, System.String>); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public System.Collections.Generic.Dictionary<System.String, System.String> ObjectNamed(string objectName) { return default(System.Collections.Generic.Dictionary<System.String, System.String>); }
        public string PropertyNamed(string propertyName) { return default(string); }
    }
    public enum CCTMXOrientation {
        Hex = 1,
        Iso = 2,
        Ortho = 0,
    }
    public enum CCTMXProperty {
        Layer = 2,
        Map = 1,
        None = 0,
        Object = 4,
        ObjectGroup = 3,
        Tile = 5,
    }
    public partial class CCTMXTiledMap : CocosSharp.CCNode {
        public CCTMXTiledMap(CocosSharp.CCTMXMapInfo mapInfo) { }
        public CCTMXTiledMap(System.IO.StreamReader tmxFile) { }
        public CCTMXTiledMap(string tmxFile) { }
        public int MapOrientation { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } }
        public CocosSharp.CCSize MapSize { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCSize); } }
        public CocosSharp.CCSize TileSize { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCSize); } }
        public CocosSharp.CCTMXLayer LayerNamed(string layerName) { return default(CocosSharp.CCTMXLayer); }
        public CocosSharp.CCTMXObjectGroup ObjectGroupNamed(string groupName) { return default(CocosSharp.CCTMXObjectGroup); }
        public System.Collections.Generic.Dictionary<System.String, System.String> PropertiesForGID(uint GID) { return default(System.Collections.Generic.Dictionary<System.String, System.String>); }
        public string PropertyNamed(string propertyName) { return default(string); }
    }
    public partial class CCTMXTileFlags {
        public static uint FlippedAll;
        public static uint FlippedMask;
        public static uint Horizontal;
        public static uint TileDiagonal;
        public static uint Vertical;
        public CCTMXTileFlags() { }
    }
    public partial class CCTMXTilesetInfo {
        public CCTMXTilesetInfo() { }
        public uint FirstGid { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(uint); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public CocosSharp.CCSize ImageSize { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCSize); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public int Margin { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public string Name { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(string); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public string SourceImage { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(string); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public int Spacing { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public CocosSharp.CCSize TileSize { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCSize); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public CocosSharp.CCRect RectForGID(uint gid) { return default(CocosSharp.CCRect); }
    }
    public partial class CCToggleVisibility : CocosSharp.CCActionInstant {
        public CCToggleVisibility() { }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public partial class CCTouch {
        internal CCTouch() { }
        public CocosSharp.CCPoint Delta { get { return default(CocosSharp.CCPoint); } }
        public int Id { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } }
        public CocosSharp.CCPoint LocationOnScreen { get { return default(CocosSharp.CCPoint); } }
        public CocosSharp.CCPoint PreviousLocationOnScreen { get { return default(CocosSharp.CCPoint); } }
        public CocosSharp.CCPoint StartLocatiOnScreen { get { return default(CocosSharp.CCPoint); } }
    }
    public partial class CCTransitionCrossFade : CocosSharp.CCTransitionScene {
        public CCTransitionCrossFade(float t, CocosSharp.CCScene scene) : base (default(float), default(CocosSharp.CCScene)) { }
        protected override void Draw() { }
        public override void OnEnter() { }
        public override void OnExit() { }
    }
    public partial class CCTransitionFade : CocosSharp.CCTransitionScene {
        protected CocosSharp.CCColor4B Color;
        public CCTransitionFade(float t, CocosSharp.CCScene scene) : base (default(float), default(CocosSharp.CCScene)) { }
        public CCTransitionFade(float duration, CocosSharp.CCScene scene, CocosSharp.CCColor3B color) : base (default(float), default(CocosSharp.CCScene)) { }
        public override void OnEnter() { }
        public override void OnExit() { }
    }
    public partial class CCTransitionFadeBL : CocosSharp.CCTransitionFadeTR {
        public CCTransitionFadeBL(float t, CocosSharp.CCScene scene) : base (default(float), default(CocosSharp.CCScene)) { }
        public override CocosSharp.CCFiniteTimeAction CreateAction(CocosSharp.CCGridSize size) { return default(CocosSharp.CCFiniteTimeAction); }
    }
    public partial class CCTransitionFadeDown : CocosSharp.CCTransitionFadeTR {
        public CCTransitionFadeDown(float t, CocosSharp.CCScene scene) : base (default(float), default(CocosSharp.CCScene)) { }
        public override CocosSharp.CCFiniteTimeAction CreateAction(CocosSharp.CCGridSize size) { return default(CocosSharp.CCFiniteTimeAction); }
    }
    public partial class CCTransitionFadeTR : CocosSharp.CCTransitionScene, CocosSharp.ICCTransitionEaseScene {
        public CCTransitionFadeTR(float t, CocosSharp.CCScene scene) : base (default(float), default(CocosSharp.CCScene)) { }
        public virtual CocosSharp.CCFiniteTimeAction CreateAction(CocosSharp.CCGridSize size) { return default(CocosSharp.CCFiniteTimeAction); }
        public virtual CocosSharp.CCFiniteTimeAction EaseAction(CocosSharp.CCFiniteTimeAction action) { return default(CocosSharp.CCFiniteTimeAction); }
        public override void OnEnter() { }
        protected override void SceneOrder() { }
    }
    public partial class CCTransitionFadeUp : CocosSharp.CCTransitionFadeTR {
        public CCTransitionFadeUp(float t, CocosSharp.CCScene scene) : base (default(float), default(CocosSharp.CCScene)) { }
        public override CocosSharp.CCFiniteTimeAction CreateAction(CocosSharp.CCGridSize size) { return default(CocosSharp.CCFiniteTimeAction); }
    }
    public partial class CCTransitionFlipAngular : CocosSharp.CCTransitionSceneOriented {
        public CCTransitionFlipAngular(float t, CocosSharp.CCScene s, CocosSharp.CCTransitionOrientation o) : base (default(float), default(CocosSharp.CCScene), default(CocosSharp.CCTransitionOrientation)) { }
        public override void OnEnter() { }
    }
    public partial class CCTransitionFlipX : CocosSharp.CCTransitionSceneOriented {
        public CCTransitionFlipX(float t, CocosSharp.CCScene s, CocosSharp.CCTransitionOrientation o) : base (default(float), default(CocosSharp.CCScene), default(CocosSharp.CCTransitionOrientation)) { }
        public override void OnEnter() { }
    }
    public partial class CCTransitionFlipY : CocosSharp.CCTransitionSceneOriented {
        public CCTransitionFlipY(float t, CocosSharp.CCScene s, CocosSharp.CCTransitionOrientation o) : base (default(float), default(CocosSharp.CCScene), default(CocosSharp.CCTransitionOrientation)) { }
        public override void OnEnter() { }
    }
    public partial class CCTransitionJumpZoom : CocosSharp.CCTransitionScene {
        public CCTransitionJumpZoom(float t, CocosSharp.CCScene scene) : base (default(float), default(CocosSharp.CCScene)) { }
        public override void OnEnter() { }
    }
    public partial class CCTransitionMoveInB : CocosSharp.CCTransitionMoveInL {
        public CCTransitionMoveInB(float t, CocosSharp.CCScene scene) : base (default(float), default(CocosSharp.CCScene)) { }
        public override void InitScenes() { }
    }
    public partial class CCTransitionMoveInL : CocosSharp.CCTransitionScene, CocosSharp.ICCTransitionEaseScene {
        public CCTransitionMoveInL(float t, CocosSharp.CCScene scene) : base (default(float), default(CocosSharp.CCScene)) { }
        public virtual CocosSharp.CCFiniteTimeAction Action() { return default(CocosSharp.CCFiniteTimeAction); }
        public CocosSharp.CCFiniteTimeAction EaseAction(CocosSharp.CCFiniteTimeAction action) { return default(CocosSharp.CCFiniteTimeAction); }
        public virtual void InitScenes() { }
        public override void OnEnter() { }
    }
    public partial class CCTransitionMoveInR : CocosSharp.CCTransitionMoveInL {
        public CCTransitionMoveInR(float t, CocosSharp.CCScene scene) : base (default(float), default(CocosSharp.CCScene)) { }
        public override void InitScenes() { }
    }
    public partial class CCTransitionMoveInT : CocosSharp.CCTransitionMoveInL {
        public CCTransitionMoveInT(float t, CocosSharp.CCScene scene) : base (default(float), default(CocosSharp.CCScene)) { }
        public override void InitScenes() { }
    }
    public enum CCTransitionOrientation {
        DownOver = 1,
        LeftOver = 0,
        RightOver = 1,
        UpOver = 0,
    }
    public partial class CCTransitionPageTurn : CocosSharp.CCTransitionScene {
        protected bool Back;
        public CCTransitionPageTurn(float t, CocosSharp.CCScene scene, bool backwards) : base (default(float), default(CocosSharp.CCScene)) { }
        public CocosSharp.CCFiniteTimeAction ActionWithSize(CocosSharp.CCGridSize vector) { return default(CocosSharp.CCFiniteTimeAction); }
        public override void OnEnter() { }
        protected override void SceneOrder() { }
    }
    public abstract partial class CCTransitionProgress : CocosSharp.CCTransitionScene {
        protected float From;
        protected CocosSharp.CCScene SceneToBeModified;
        protected float To;
        public CCTransitionProgress(float t, CocosSharp.CCScene scene) : base (default(float), default(CocosSharp.CCScene)) { }
        public override void OnEnter() { }
        public override void OnExit() { }
        protected abstract CocosSharp.CCProgressTimer ProgressTimerNodeWithRenderTexture(CocosSharp.CCRenderTexture texture);
        protected override void SceneOrder() { }
        protected virtual void SetupTransition() { }
    }
    public partial class CCTransitionProgressHorizontal : CocosSharp.CCTransitionProgress {
        public CCTransitionProgressHorizontal(float t, CocosSharp.CCScene scene) : base (default(float), default(CocosSharp.CCScene)) { }
        protected override CocosSharp.CCProgressTimer ProgressTimerNodeWithRenderTexture(CocosSharp.CCRenderTexture texture) { return default(CocosSharp.CCProgressTimer); }
    }
    public partial class CCTransitionProgressInOut : CocosSharp.CCTransitionProgress {
        public CCTransitionProgressInOut(float t, CocosSharp.CCScene scene) : base (default(float), default(CocosSharp.CCScene)) { }
        protected override CocosSharp.CCProgressTimer ProgressTimerNodeWithRenderTexture(CocosSharp.CCRenderTexture texture) { return default(CocosSharp.CCProgressTimer); }
        protected override void SceneOrder() { }
        protected override void SetupTransition() { }
    }
    public partial class CCTransitionProgressOutIn : CocosSharp.CCTransitionProgress {
        public CCTransitionProgressOutIn(float t, CocosSharp.CCScene scene) : base (default(float), default(CocosSharp.CCScene)) { }
        protected override CocosSharp.CCProgressTimer ProgressTimerNodeWithRenderTexture(CocosSharp.CCRenderTexture texture) { return default(CocosSharp.CCProgressTimer); }
    }
    public partial class CCTransitionProgressRadialCCW : CocosSharp.CCTransitionProgress {
        public CCTransitionProgressRadialCCW(float t, CocosSharp.CCScene scene) : base (default(float), default(CocosSharp.CCScene)) { }
        protected override CocosSharp.CCProgressTimer ProgressTimerNodeWithRenderTexture(CocosSharp.CCRenderTexture texture) { return default(CocosSharp.CCProgressTimer); }
    }
    public partial class CCTransitionProgressRadialCW : CocosSharp.CCTransitionProgress {
        public CCTransitionProgressRadialCW(float t, CocosSharp.CCScene scene) : base (default(float), default(CocosSharp.CCScene)) { }
        protected override CocosSharp.CCProgressTimer ProgressTimerNodeWithRenderTexture(CocosSharp.CCRenderTexture texture) { return default(CocosSharp.CCProgressTimer); }
    }
    public partial class CCTransitionProgressVertical : CocosSharp.CCTransitionProgress {
        public CCTransitionProgressVertical(float t, CocosSharp.CCScene scene) : base (default(float), default(CocosSharp.CCScene)) { }
        protected override CocosSharp.CCProgressTimer ProgressTimerNodeWithRenderTexture(CocosSharp.CCRenderTexture texture) { return default(CocosSharp.CCProgressTimer); }
    }
    public partial class CCTransitionRotoZoom : CocosSharp.CCTransitionScene {
        public CCTransitionRotoZoom(float t, CocosSharp.CCScene scene) : base (default(float), default(CocosSharp.CCScene)) { }
        public override void OnEnter() { }
    }
    public partial class CCTransitionScene : CocosSharp.CCScene {
        public CCTransitionScene(float t, CocosSharp.CCScene scene) : base (default(CocosSharp.CCWindow), default(CocosSharp.CCViewport), default(CocosSharp.CCDirector)) { }
        protected float Duration { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        protected CocosSharp.CCScene InScene { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCScene); } }
        protected bool IsInSceneOnTop { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        protected bool IsSendCleanupToScene { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public override bool IsTransition { get { return default(bool); } }
        protected CocosSharp.CCScene OutScene { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCScene); } }
        public override CocosSharp.CCScene Scene { get { return default(CocosSharp.CCScene); } }
        protected override void AddedToNewScene() { }
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
        protected CocosSharp.CCTransitionOrientation Orientation;
        public CCTransitionSceneOriented(float t, CocosSharp.CCScene scene, CocosSharp.CCTransitionOrientation orientation) : base (default(float), default(CocosSharp.CCScene)) { }
    }
    public partial class CCTransitionShrinkGrow : CocosSharp.CCTransitionScene, CocosSharp.ICCTransitionEaseScene {
        public CCTransitionShrinkGrow(float t, CocosSharp.CCScene scene) : base (default(float), default(CocosSharp.CCScene)) { }
        public CocosSharp.CCFiniteTimeAction EaseAction(CocosSharp.CCFiniteTimeAction action) { return default(CocosSharp.CCFiniteTimeAction); }
        public override void OnEnter() { }
    }
    public partial class CCTransitionSlideInB : CocosSharp.CCTransitionSlideInL {
        public CCTransitionSlideInB(float t, CocosSharp.CCScene scene) : base (default(float), default(CocosSharp.CCScene)) { }
        public override CocosSharp.CCFiniteTimeAction Action() { return default(CocosSharp.CCFiniteTimeAction); }
        protected override void InitScenes() { }
        protected override void SceneOrder() { }
    }
    public partial class CCTransitionSlideInL : CocosSharp.CCTransitionScene, CocosSharp.ICCTransitionEaseScene {
        public CCTransitionSlideInL(float t, CocosSharp.CCScene scene) : base (default(float), default(CocosSharp.CCScene)) { }
        public virtual CocosSharp.CCFiniteTimeAction Action() { return default(CocosSharp.CCFiniteTimeAction); }
        public virtual CocosSharp.CCFiniteTimeAction EaseAction(CocosSharp.CCFiniteTimeAction action) { return default(CocosSharp.CCFiniteTimeAction); }
        protected virtual void InitScenes() { }
        public override void OnEnter() { }
        protected override void SceneOrder() { }
    }
    public partial class CCTransitionSlideInR : CocosSharp.CCTransitionSlideInL {
        public CCTransitionSlideInR(float t, CocosSharp.CCScene scene) : base (default(float), default(CocosSharp.CCScene)) { }
        public override CocosSharp.CCFiniteTimeAction Action() { return default(CocosSharp.CCFiniteTimeAction); }
        protected override void InitScenes() { }
        protected override void SceneOrder() { }
    }
    public partial class CCTransitionSlideInT : CocosSharp.CCTransitionSlideInL {
        public CCTransitionSlideInT(float t, CocosSharp.CCScene scene) : base (default(float), default(CocosSharp.CCScene)) { }
        public override CocosSharp.CCFiniteTimeAction Action() { return default(CocosSharp.CCFiniteTimeAction); }
        protected override void InitScenes() { }
        protected override void SceneOrder() { }
    }
    public partial class CCTransitionSplitCols : CocosSharp.CCTransitionScene, CocosSharp.ICCTransitionEaseScene {
        public CCTransitionSplitCols(float t, CocosSharp.CCScene scene) : base (default(float), default(CocosSharp.CCScene)) { }
        public virtual CocosSharp.CCFiniteTimeAction Action() { return default(CocosSharp.CCFiniteTimeAction); }
        public virtual CocosSharp.CCFiniteTimeAction EaseAction(CocosSharp.CCFiniteTimeAction action) { return default(CocosSharp.CCFiniteTimeAction); }
        public override void OnEnter() { }
    }
    public partial class CCTransitionSplitRows : CocosSharp.CCTransitionSplitCols {
        public CCTransitionSplitRows(float t, CocosSharp.CCScene scene) : base (default(float), default(CocosSharp.CCScene)) { }
        public override CocosSharp.CCFiniteTimeAction Action() { return default(CocosSharp.CCFiniteTimeAction); }
    }
    public partial class CCTransitionTurnOffTiles : CocosSharp.CCTransitionScene, CocosSharp.ICCTransitionEaseScene {
        public CCTransitionTurnOffTiles(float t, CocosSharp.CCScene scene) : base (default(float), default(CocosSharp.CCScene)) { }
        public virtual CocosSharp.CCFiniteTimeAction EaseAction(CocosSharp.CCFiniteTimeAction action) { return default(CocosSharp.CCFiniteTimeAction); }
        public override void OnEnter() { }
        protected override void SceneOrder() { }
    }
    public partial class CCTransitionZoomFlipAngular : CocosSharp.CCTransitionSceneOriented {
        public CCTransitionZoomFlipAngular(float t, CocosSharp.CCScene s, CocosSharp.CCTransitionOrientation o) : base (default(float), default(CocosSharp.CCScene), default(CocosSharp.CCTransitionOrientation)) { }
        public override void OnEnter() { }
    }
    public partial class CCTransitionZoomFlipX : CocosSharp.CCTransitionSceneOriented {
        public CCTransitionZoomFlipX(float t, CocosSharp.CCScene s, CocosSharp.CCTransitionOrientation o) : base (default(float), default(CocosSharp.CCScene), default(CocosSharp.CCTransitionOrientation)) { }
        public override void OnEnter() { }
    }
    public partial class CCTransitionZoomFlipY : CocosSharp.CCTransitionSceneOriented {
        public CCTransitionZoomFlipY(float t, CocosSharp.CCScene s, CocosSharp.CCTransitionOrientation o) : base (default(float), default(CocosSharp.CCScene), default(CocosSharp.CCTransitionOrientation)) { }
        public override void OnEnter() { }
    }
    public partial class CCTurnOffTiles : CocosSharp.CCShuffleTiles {
        public CCTurnOffTiles(float duration, CocosSharp.CCGridSize gridSize, int seed=-1) : base (default(CocosSharp.CCGridSize), default(float), default(int)) { }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public partial class CCTwirl : CocosSharp.CCGrid3DAction {
        public CCTwirl(float duration, CocosSharp.CCGridSize gridSize) : base (default(float)) { }
        public CCTwirl(float duration, CocosSharp.CCGridSize gridSize, CocosSharp.CCPoint position, int twirls=0, float amplitude=0f) : base (default(float)) { }
        public CocosSharp.CCPoint Position { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCPoint); } }
        public int Twirls { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public partial class CCUserDefault {
        internal CCUserDefault() { }
        public static CocosSharp.CCUserDefault SharedUserDefault { get { return default(CocosSharp.CCUserDefault); } }
        public void Flush() { }
        public bool GetBoolForKey(string key, bool defaultValue=false) { return default(bool); }
        public double GetDoubleForKey(string key, double defaultValue) { return default(double); }
        public float GetFloatForKey(string key, float defaultValue) { return default(float); }
        public int GetIntegerForKey(string key, int defaultValue=0) { return default(int); }
        public string GetStringForKey(string key, string defaultValue) { return default(string); }
        public void PurgeSharedUserDefault() { }
        public void SetBoolForKey(string key, bool value) { }
        public void SetDoubleForKey(string key, double value) { }
        public void SetFloatForKey(string key, float value) { }
        public void SetIntegerForKey(string key, int value) { }
        public void SetStringForKey(string key, string value) { }
    }
    public partial class CCV2F_C4B_T2F {
        public CocosSharp.CCColor4B Colors;
        public CocosSharp.CCTex2F TexCoords;
        public CocosSharp.CCVertex2F Vertices;
        public CCV2F_C4B_T2F() { }
    }
    public partial class CCV2F_C4B_T2F_Quad {
        public CocosSharp.CCV2F_C4B_T2F BottomLeft;
        public CocosSharp.CCV2F_C4B_T2F BottomRight;
        public CocosSharp.CCV2F_C4B_T2F TopLeft;
        public CocosSharp.CCV2F_C4B_T2F TopRight;
        public CCV2F_C4B_T2F_Quad() { }
    }
    public partial class CCV2F_C4F_T2F {
        public CocosSharp.CCColor4F Colors;
        public CocosSharp.CCTex2F TexCoords;
        public CocosSharp.CCVertex2F Vertices;
        public CCV2F_C4F_T2F() { }
    }
    public partial class CCV2F_C4F_T2F_Quad {
        public CocosSharp.CCV2F_C4F_T2F BottomLeft;
        public CocosSharp.CCV2F_C4F_T2F BottomRight;
        public CocosSharp.CCV2F_C4F_T2F TopLeft;
        public CocosSharp.CCV2F_C4F_T2F TopRight;
        public CCV2F_C4F_T2F_Quad() { }
    }
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public partial struct CCV3F_C4B_T2F : Microsoft.Xna.Framework.Graphics.IVertexType {
        public CocosSharp.CCColor4B Colors;
        public CocosSharp.CCTex2F TexCoords;
        public static readonly Microsoft.Xna.Framework.Graphics.VertexDeclaration VertexDeclaration;
        public CocosSharp.CCVertex3F Vertices;
        Microsoft.Xna.Framework.Graphics.VertexDeclaration Microsoft.Xna.Framework.Graphics.IVertexType.VertexDeclaration { get { return default(Microsoft.Xna.Framework.Graphics.VertexDeclaration); } }
    }
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public partial struct CCV3F_C4B_T2F_Quad : Microsoft.Xna.Framework.Graphics.IVertexType {
        public CocosSharp.CCV3F_C4B_T2F BottomLeft;
        public CocosSharp.CCV3F_C4B_T2F BottomRight;
        public CocosSharp.CCV3F_C4B_T2F TopLeft;
        public CocosSharp.CCV3F_C4B_T2F TopRight;
        public static readonly Microsoft.Xna.Framework.Graphics.VertexDeclaration VertexDeclaration;
        Microsoft.Xna.Framework.Graphics.VertexDeclaration Microsoft.Xna.Framework.Graphics.IVertexType.VertexDeclaration { get { return default(Microsoft.Xna.Framework.Graphics.VertexDeclaration); } }
    }
    [System.Runtime.Serialization.DataContractAttribute]
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public partial struct CCVector2 : System.IEquatable<CocosSharp.CCVector2> {
        [System.Runtime.Serialization.DataMemberAttribute]
        public float X;
        [System.Runtime.Serialization.DataMemberAttribute]
        public float Y;
        public CCVector2(float value) { throw new System.NotImplementedException(); }
        public CCVector2(float x, float y) { throw new System.NotImplementedException(); }
        public static CocosSharp.CCVector2 One { get { return default(CocosSharp.CCVector2); } }
        public static CocosSharp.CCVector2 UnitX { get { return default(CocosSharp.CCVector2); } }
        public static CocosSharp.CCVector2 UnitY { get { return default(CocosSharp.CCVector2); } }
        public static CocosSharp.CCVector2 Zero { get { return default(CocosSharp.CCVector2); } }
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
        public static readonly CocosSharp.CCVertex2F ZeroVector;
        public CCVertex2F(CocosSharp.CCVertex3F vertex3F) { throw new System.NotImplementedException(); }
        public CCVertex2F(float x, float y) { throw new System.NotImplementedException(); }
        public float X { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public float Y { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    }
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Size=1)]
    public partial struct CCVertex3F {
        public static readonly CocosSharp.CCVertex3F Zero;
        public CCVertex3F(float x, float y, float z) { throw new System.NotImplementedException(); }
        public float X { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public float Y { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public float Z { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public override string ToString() { return default(string); }
    }
    public enum CCVerticalTextAlignment {
        Bottom = 2,
        Center = 1,
        Top = 0,
    }
    public partial class CCViewport {
        public CCViewport(CocosSharp.CCRect exactFitLandscapeRatioIn, CocosSharp.CCRect exactFitPortraitRatioIn, CocosSharp.CCViewportResolutionPolicy resolutionPolicyIn=(CocosSharp.CCViewportResolutionPolicy)(0), CocosSharp.CCDisplayOrientation displayOrientationIn=(CocosSharp.CCDisplayOrientation)(1)) { }
        public CCViewport(CocosSharp.CCRect exactFitLandscapeRatioIn, CocosSharp.CCViewportResolutionPolicy resolutionPolicyIn=(CocosSharp.CCViewportResolutionPolicy)(0), CocosSharp.CCDisplayOrientation displayOrientationIn=(CocosSharp.CCDisplayOrientation)(1)) { }
        public CocosSharp.CCRect ExactFitLandscapeRatio { get { return default(CocosSharp.CCRect); } set { } }
        public CocosSharp.CCRect ExactFitPortraitRatio { get { return default(CocosSharp.CCRect); } set { } }
        public CocosSharp.CCViewportResolutionPolicy ResolutionPolicy { get { return default(CocosSharp.CCViewportResolutionPolicy); } set { } }
    }
    public enum CCViewportResolutionPolicy {
        AspectFill = 1,
        AspectFit = 2,
        ExactFit = 0,
    }
    public partial class CCWaves : CocosSharp.CCLiquid {
        public CCWaves(float duration, CocosSharp.CCGridSize gridSize, int waves=0, float amplitude=0f, bool horizontal=true, bool vertical=true) : base (default(float), default(CocosSharp.CCGridSize), default(int), default(float)) { }
        protected internal bool Horizontal { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } }
        protected internal bool Vertical { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public partial class CCWaves3D : CocosSharp.CCLiquid {
        public CCWaves3D(float duration, CocosSharp.CCGridSize gridSize, int waves=0, float amplitude=0f) : base (default(float), default(CocosSharp.CCGridSize), default(int), default(float)) { }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public partial class CCWavesTiles3D : CocosSharp.CCTiledGrid3DAction {
        public CCWavesTiles3D(float duration, CocosSharp.CCGridSize gridSize, int waves=0, float amplitude=0f) : base (default(float)) { }
        protected internal int Waves { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } }
        protected internal override CocosSharp.CCActionState StartAction(CocosSharp.CCNode target) { return default(CocosSharp.CCActionState); }
    }
    public partial class CCWindow {
        internal CCWindow() { }
        public static string EVENT_AFTER_DRAW;
        public static string EVENT_AFTER_UPDATE;
        public static string EVENT_AFTER_VISIT;
        public static string EVENT_PROJECTION_CHANGED;
        public CocosSharp.CCAccelerometer Accelerometer { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCAccelerometer); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public CocosSharp.CCDirector ActiveDirector { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCDirector); } }
        public bool AllowUserResizing { get { return default(bool); } set { } }
        public virtual double AnimationInterval { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(double); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public CocosSharp.CCApplication Application { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCApplication); } }
        public CocosSharp.CCDisplayOrientation CurrentDisplayOrientation { get { return default(CocosSharp.CCDisplayOrientation); } set { } }
        public bool DisplayStats { get { return default(bool); } set { } }
        public bool FullScreen { get { return default(bool); } set { } }
        public bool GamePadEnabled { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public bool IsUseAlphaBlending { set { } }
        public bool IsUseDepthTesting { get { return default(bool); } set { } }
        public CocosSharp.CCNode NotificationNode { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCNode); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public bool PreferMultiSampling { get { return default(bool); } set { } }
        protected CocosSharp.CCStats Stats { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.CCStats); } }
        public CocosSharp.CCDisplayOrientation SupportedDisplayOrientations { get { return default(CocosSharp.CCDisplayOrientation); } set { } }
        public CocosSharp.CCSize WindowSizeInPixels { get { return default(CocosSharp.CCSize); } }
        public void AddSceneDirector(CocosSharp.CCDirector sceneDirector) { }
        protected void Draw(CocosSharp.CCGameTime gameTime) { }
        public void EndAllSceneDirectors() { }
        public virtual void MainLoop(CocosSharp.CCGameTime gameTime) { }
        public void RemoveAllSceneDirectors() { }
        public void RemoveSceneDirector(CocosSharp.CCDirector sceneDirector) { }
        public void RunWithScene(CocosSharp.CCScene scene) { }
        public void SetActiveDirector(int index) { }
    }
    public partial interface ICCActionTweenDelegate {
        void UpdateTweenAction(float value, string key);
    }
    public partial interface ICCBlendable {
        CocosSharp.CCBlendFunc BlendFunc { get; set; }
    }
    public partial interface ICCFocusable {
        bool CanReceiveFocus { get; }
        bool HasFocus { get; set; }
    }
    public partial interface ICCIMEDelegate {
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
    public partial class ICCScriptingEngine {
        public ICCScriptingEngine() { }
        public virtual bool AddSearchPath(string path) { return default(bool); }
        public virtual bool ExecuteCallFunc(string funcName) { return default(bool); }
        public virtual bool ExecuteCallFunc0(string funcName, object pObject) { return default(bool); }
        public virtual bool ExecuteCallFuncN(string funcName, CocosSharp.CCNode node) { return default(bool); }
        public virtual bool ExecuteCallFuncNd(string funcName, CocosSharp.CCNode node, object pData) { return default(bool); }
        public virtual int ExecuteFuction(string funcName) { return default(int); }
        public virtual bool ExecuteSchedule(string funcName, float t) { return default(bool); }
        public virtual bool ExecuteScriptFile(string filename) { return default(bool); }
        public virtual bool ExecuteString(string codes) { return default(bool); }
        public virtual bool ExecuteTouchesEvent(string funcName, System.Collections.Generic.List<CocosSharp.CCTouch> touches) { return default(bool); }
        public virtual bool ExecuteTouchEvent(string funcName, CocosSharp.CCTouch touch) { return default(bool); }
    }
    public partial interface ICCTextContainer {
        string Text { get; set; }
    }
    public partial interface ICCTextFieldDelegate {
        bool onDraw(CocosSharp.CCTextFieldTTF sender);
        bool onTextFieldAttachWithIME(CocosSharp.CCTextFieldTTF sender);
        bool onTextFieldDeleteBackward(CocosSharp.CCTextFieldTTF sender, string delText, int nLen);
        bool onTextFieldDetachWithIME(CocosSharp.CCTextFieldTTF sender);
        bool onTextFieldInsertText(CocosSharp.CCTextFieldTTF sender, string text, int nLen);
    }
    public partial interface ICCTexture : CocosSharp.ICCBlendable {
        CocosSharp.CCTexture2D Texture { get; set; }
    }
    public partial interface ICCTransitionEaseScene {
        CocosSharp.CCFiniteTimeAction EaseAction(CocosSharp.CCFiniteTimeAction action);
    }
    public partial interface ICCUpdatable {
        void Update(float dt);
    }
    public static partial class OrientationExtension {
        public static bool IsPortrait(this CocosSharp.CCDisplayOrientation orientation) { return default(bool); }
    }
    public partial class PlistArray : CocosSharp.PlistObject<System.Collections.Generic.List<CocosSharp.PlistObjectBase>>, System.Collections.Generic.IEnumerable<CocosSharp.PlistObjectBase>, System.Collections.IEnumerable {
        public PlistArray() : base (default(System.Collections.Generic.List<CocosSharp.PlistObjectBase>)) { }
        public PlistArray(System.Collections.Generic.List<CocosSharp.PlistObjectBase> value) : base (default(System.Collections.Generic.List<CocosSharp.PlistObjectBase>)) { }
        public PlistArray(System.Collections.IEnumerable value) : base (default(System.Collections.Generic.List<CocosSharp.PlistObjectBase>)) { }
        public PlistArray(int capacity) : base (default(System.Collections.Generic.List<CocosSharp.PlistObjectBase>)) { }
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
        public PlistBoolean(bool value) : base (default(bool)) { }
        public override CocosSharp.PlistArray AsArray { get { return default(CocosSharp.PlistArray); } }
        public override System.Byte[] AsBinary { get { return default(System.Byte[]); } }
        public override bool AsBool { get { return default(bool); } }
        public override System.DateTime AsDate { get { return default(System.DateTime); } }
        public override CocosSharp.PlistDictionary AsDictionary { get { return default(CocosSharp.PlistDictionary); } }
        public override float AsFloat { get { return default(float); } }
        public override int AsInt { get { return default(int); } }
        public override string AsString { get { return default(string); } }
        public override void Write(System.Xml.XmlWriter writer) { }
    }
    public partial class PlistData : CocosSharp.PlistObject<System.Byte[]> {
        public PlistData(System.Byte[] data) : base (default(System.Byte[])) { }
        public PlistData(string value) : base (default(System.Byte[])) { }
        public override CocosSharp.PlistArray AsArray { get { return default(CocosSharp.PlistArray); } }
        public override System.Byte[] AsBinary { get { return default(System.Byte[]); } }
        public override bool AsBool { get { return default(bool); } }
        public override System.DateTime AsDate { get { return default(System.DateTime); } }
        public override CocosSharp.PlistDictionary AsDictionary { get { return default(CocosSharp.PlistDictionary); } }
        public override float AsFloat { get { return default(float); } }
        public override int AsInt { get { return default(int); } }
        public override string AsString { get { return default(string); } }
        public override void Write(System.Xml.XmlWriter writer) { }
    }
    public partial class PlistDate : CocosSharp.PlistObject<System.DateTime> {
        public PlistDate(System.DateTime value) : base (default(System.DateTime)) { }
        public override CocosSharp.PlistArray AsArray { get { return default(CocosSharp.PlistArray); } }
        public override System.Byte[] AsBinary { get { return default(System.Byte[]); } }
        public override bool AsBool { get { return default(bool); } }
        public override System.DateTime AsDate { get { return default(System.DateTime); } }
        public override CocosSharp.PlistDictionary AsDictionary { get { return default(CocosSharp.PlistDictionary); } }
        public override float AsFloat { get { return default(float); } }
        public override int AsInt { get { return default(int); } }
        public override string AsString { get { return default(string); } }
        public override void Write(System.Xml.XmlWriter writer) { }
    }
    public partial class PlistDictionary : CocosSharp.PlistObjectBase, System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<System.String, CocosSharp.PlistObjectBase>>, System.Collections.IEnumerable {
        public PlistDictionary() { }
        public PlistDictionary(bool keepOrder) { }
        public PlistDictionary(System.Collections.Generic.Dictionary<System.String, CocosSharp.PlistObjectBase> value) { }
        public PlistDictionary(System.Collections.Generic.Dictionary<System.String, CocosSharp.PlistObjectBase> value, bool keepOrder) { }
        public PlistDictionary(System.Collections.IDictionary value) { }
        public PlistDictionary(System.Collections.IDictionary value, bool keepOrder) { }
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
        public PlistDocument() { }
        public PlistDocument(CocosSharp.PlistObjectBase root) { }
        public PlistDocument(System.IO.Stream data) { }
        public PlistDocument(string data) { }
        public override CocosSharp.PlistArray AsArray { get { return default(CocosSharp.PlistArray); } }
        public override System.Byte[] AsBinary { get { return default(System.Byte[]); } }
        public override bool AsBool { get { return default(bool); } }
        public override System.DateTime AsDate { get { return default(System.DateTime); } }
        public override CocosSharp.PlistDictionary AsDictionary { get { return default(CocosSharp.PlistDictionary); } }
        public override float AsFloat { get { return default(float); } }
        public override int AsInt { get { return default(int); } }
        public override string AsString { get { return default(string); } }
        public CocosSharp.PlistObjectBase Root { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(CocosSharp.PlistObjectBase); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public void LoadFromXml(string data) { }
        public void LoadFromXml(System.Xml.XmlReader reader) { }
        public void LoadFromXmlFile(System.IO.Stream data) { }
        public void LoadFromXmlFile(string path) { }
        public override void Write(System.Xml.XmlWriter writer) { }
        public void WriteToFile(string filename) { }
    }
    public partial class PlistInteger : CocosSharp.PlistObject<System.Int32> {
        public PlistInteger(int value) : base (default(int)) { }
        public override CocosSharp.PlistArray AsArray { get { return default(CocosSharp.PlistArray); } }
        public override System.Byte[] AsBinary { get { return default(System.Byte[]); } }
        public override bool AsBool { get { return default(bool); } }
        public override System.DateTime AsDate { get { return default(System.DateTime); } }
        public override CocosSharp.PlistDictionary AsDictionary { get { return default(CocosSharp.PlistDictionary); } }
        public override float AsFloat { get { return default(float); } }
        public override int AsInt { get { return default(int); } }
        public override string AsString { get { return default(string); } }
        public override void Write(System.Xml.XmlWriter writer) { }
    }
    public partial class PlistNull : CocosSharp.PlistObject<System.Nullable<System.Int32>> {
        public PlistNull() : base (default(System.Nullable<System.Int32>)) { }
        public override CocosSharp.PlistArray AsArray { get { return default(CocosSharp.PlistArray); } }
        public override System.Byte[] AsBinary { get { return default(System.Byte[]); } }
        public override bool AsBool { get { return default(bool); } }
        public override System.DateTime AsDate { get { return default(System.DateTime); } }
        public override CocosSharp.PlistDictionary AsDictionary { get { return default(CocosSharp.PlistDictionary); } }
        public override float AsFloat { get { return default(float); } }
        public override int AsInt { get { return default(int); } }
        public override string AsString { get { return default(string); } }
        public override void Write(System.Xml.XmlWriter writer) { }
    }
    public abstract partial class PlistObject<T> : CocosSharp.PlistObjectBase {
        public PlistObject(T value) { }
        public virtual T Value { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(T); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    }
    public abstract partial class PlistObjectBase {
        protected PlistObjectBase() { }
        public abstract CocosSharp.PlistArray AsArray { get; }
        public abstract System.Byte[] AsBinary { get; }
        public abstract bool AsBool { get; }
        public abstract System.DateTime AsDate { get; }
        public abstract CocosSharp.PlistDictionary AsDictionary { get; }
        public abstract float AsFloat { get; }
        public abstract int AsInt { get; }
        public abstract string AsString { get; }
        protected static CocosSharp.PlistObjectBase ObjectToPlistObject(object value) { return default(CocosSharp.PlistObjectBase); }
        public static implicit operator CocosSharp.PlistObjectBase (bool value) { return default(CocosSharp.PlistObjectBase); }
        public static implicit operator CocosSharp.PlistObjectBase (int value) { return default(CocosSharp.PlistObjectBase); }
        public static implicit operator CocosSharp.PlistObjectBase (System.Object[] value) { return default(CocosSharp.PlistObjectBase); }
        public static implicit operator CocosSharp.PlistObjectBase (float value) { return default(CocosSharp.PlistObjectBase); }
        public static implicit operator CocosSharp.PlistObjectBase (string value) { return default(CocosSharp.PlistObjectBase); }
        public abstract void Write(System.Xml.XmlWriter writer);
    }
    public partial class PlistReal : CocosSharp.PlistObject<System.Single> {
        public PlistReal(float value) : base (default(float)) { }
        public override CocosSharp.PlistArray AsArray { get { return default(CocosSharp.PlistArray); } }
        public override System.Byte[] AsBinary { get { return default(System.Byte[]); } }
        public override bool AsBool { get { return default(bool); } }
        public override System.DateTime AsDate { get { return default(System.DateTime); } }
        public override CocosSharp.PlistDictionary AsDictionary { get { return default(CocosSharp.PlistDictionary); } }
        public override float AsFloat { get { return default(float); } }
        public override int AsInt { get { return default(int); } }
        public override string AsString { get { return default(string); } }
        public override void Write(System.Xml.XmlWriter writer) { }
    }
    public partial class PlistString : CocosSharp.PlistObject<System.String> {
        public PlistString(string value) : base (default(string)) { }
        public override CocosSharp.PlistArray AsArray { get { return default(CocosSharp.PlistArray); } }
        public override System.Byte[] AsBinary { get { return default(System.Byte[]); } }
        public override bool AsBool { get { return default(bool); } }
        public override System.DateTime AsDate { get { return default(System.DateTime); } }
        public override CocosSharp.PlistDictionary AsDictionary { get { return default(CocosSharp.PlistDictionary); } }
        public override float AsFloat { get { return default(float); } }
        public override int AsInt { get { return default(int); } }
        public override string AsString { get { return default(string); } }
        public override void Write(System.Xml.XmlWriter writer) { }
    }
}
