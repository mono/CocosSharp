
namespace CocosSharp
{
    public class CCTransitionJumpZoom : CCTransitionScene
    {
        public CCTransitionJumpZoom (float t, CCScene scene) : base (t, scene)
        { }

        public override void OnEnter()
        {
            base.OnEnter();
            var bounds = Layer.VisibleBoundsWorldspace;

            InScene.Scale = 0.5f;
            InScene.Position = new CCPoint(bounds.Origin.X + bounds.Size.Width, bounds.Origin.Y);
            InScene.AnchorPoint = new CCPoint(0.5f, 0.5f);
            OutScene.AnchorPoint = new CCPoint(0.5f, 0.5f);

            CCFiniteTimeAction jump = new CCJumpBy (Duration / 4, new CCPoint(-bounds.Size.Width, 0), bounds.Size.Width / 4, 2);
            CCFiniteTimeAction scaleIn = new CCScaleTo(Duration / 4, 1.0f);
            CCFiniteTimeAction scaleOut = new CCScaleTo(Duration / 4, 0.5f);

            CCSequence jumpZoomOut = (new CCSequence(scaleOut, jump));
            CCSequence jumpZoomIn = (new CCSequence(jump, scaleIn));

            CCFiniteTimeAction delay = new CCDelayTime (Duration / 2);

            OutScene.RunAction(jumpZoomOut);
            InScene.RunAction
                (
                        new CCSequence
                        (
                            delay,
                            jumpZoomIn,
                            new CCCallFunc(Finish)
                        )
                );
        }

    }
}