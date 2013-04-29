
namespace cocos2d
{
    public class CCTransitionJumpZoom : CCTransitionScene
    {
        public CCTransitionJumpZoom (float t, CCScene scene) : base (t, scene)
        { }

        public override void OnEnter()
        {
            base.OnEnter();
            CCSize s = CCDirector.SharedDirector.WinSize;

            m_pInScene.Scale = 0.5f;
            m_pInScene.Position = new CCPoint(s.Width, 0);
            m_pInScene.AnchorPoint = new CCPoint(0.5f, 0.5f);
            m_pOutScene.AnchorPoint = new CCPoint(0.5f, 0.5f);

            CCActionInterval jump = new CCJumpBy (m_fDuration / 4, new CCPoint(-s.Width, 0), s.Width / 4, 2);
            CCActionInterval scaleIn = new CCScaleTo(m_fDuration / 4, 1.0f);
            CCActionInterval scaleOut = new CCScaleTo(m_fDuration / 4, 0.5f);

            CCSequence jumpZoomOut = (CCSequence.FromActions(scaleOut, jump));
            CCSequence jumpZoomIn = (CCSequence.FromActions((CCActionInterval) jump.Copy(), scaleIn));

            CCActionInterval delay = new CCDelayTime (m_fDuration / 2);

            m_pOutScene.RunAction(jumpZoomOut);
            m_pInScene.RunAction
                (
                    CCSequence.FromActions
                        (
                            delay,
                            jumpZoomIn,
                            new CCCallFunc(Finish)
                        )
                );
        }

    }
}