
namespace CocosSharp
{
    public class CCTransitionJumpZoom : CCTransitionScene
    {
        CCSequence jumpZoomOut;
        CCSequence jumpZoomIn;
        CCFiniteTimeAction delay;

        #region Properties

        protected override CCFiniteTimeAction OutSceneAction
        {
            get
            {
                return new CCSequence(jumpZoomOut, delay);
            }
        }

        protected override CCFiniteTimeAction InSceneAction
        {
            get { return new CCSequence(delay, jumpZoomIn); }
        }

        #endregion Properties


        #region Constructors

        public CCTransitionJumpZoom (float t, CCScene scene) : base (t, scene)
        { 
        }

        #endregion Constructors


        protected override void InitialiseScenes()
        {
            base.InitialiseScenes();
            var bounds = Layer.VisibleBoundsWorldspace;

            InSceneNodeContainer.Scale = 0.5f;
            InSceneNodeContainer.Position = new CCPoint(bounds.Origin.X + bounds.Size.Width, bounds.Origin.Y);
            InSceneNodeContainer.AnchorPoint = new CCPoint(0.5f, 0.5f);
            OutSceneNodeContainer.AnchorPoint = new CCPoint(0.5f, 0.5f);

            InSceneNodeContainer.IgnoreAnchorPointForPosition = true;
            OutSceneNodeContainer.IgnoreAnchorPointForPosition = true;

            CCJumpBy jump = new CCJumpBy (Duration / 4, new CCPoint(-bounds.Size.Width, 0), bounds.Size.Width / 4, 2);
            CCFiniteTimeAction scaleIn = new CCScaleTo(Duration / 4, 1.0f);
            CCFiniteTimeAction scaleOut = new CCScaleTo(Duration / 4, 0.5f);

            jumpZoomOut = (new CCSequence(scaleOut, jump));
            jumpZoomIn = (new CCSequence(jump, scaleIn));

            delay = new CCDelayTime (Duration / 2);
        }

    }
}