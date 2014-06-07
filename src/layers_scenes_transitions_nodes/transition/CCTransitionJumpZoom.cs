
namespace CocosSharp
{
    public class CCTransitionJumpZoom : CCTransitionScene
    {
        public CCTransitionJumpZoom (float t, CCScene scene) : base (t, scene)
        { }

        public override void OnEnter()
        {
            base.OnEnter();
            CCSize s = Director.WindowSizeInPoints;

            InScene.Scale = 0.5f;
            InScene.Position = new CCPoint(s.Width, 0);
            InScene.AnchorPoint = new CCPoint(0.5f, 0.5f);
            OutScene.AnchorPoint = new CCPoint(0.5f, 0.5f);

            CCActionInterval jump = new CCJumpBy (Duration / 4, new CCPoint(-s.Width, 0), s.Width / 4, 2);
            CCActionInterval scaleIn = new CCScaleTo(Duration / 4, 1.0f);
            CCActionInterval scaleOut = new CCScaleTo(Duration / 4, 0.5f);

            CCSequence jumpZoomOut = (new CCSequence(scaleOut, jump));
            CCSequence jumpZoomIn = (new CCSequence(jump, scaleIn));

            CCActionInterval delay = new CCDelayTime (Duration / 2);

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