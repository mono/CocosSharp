
namespace cocos2d
{
    public class CCTransitionJumpZoom : CCTransitionScene
    {
        public override void OnEnter()
        {
            base.OnEnter();
            CCSize s = CCDirector.SharedDirector.WinSize;

            m_pInScene.Scale = 0.5f;
            m_pInScene.Position = new CCPoint(s.width, 0);
            m_pInScene.AnchorPoint = new CCPoint(0.5f, 0.5f);
            m_pOutScene.AnchorPoint = new CCPoint(0.5f, 0.5f);

            CCActionInterval jump = CCJumpBy.Create(m_fDuration / 4, new CCPoint(-s.width, 0), s.width / 4, 2);
            CCActionInterval scaleIn = CCScaleTo.Create(m_fDuration / 4, 1.0f);
            CCActionInterval scaleOut = CCScaleTo.Create(m_fDuration / 4, 0.5f);

            CCSequence jumpZoomOut = (CCSequence.Create(scaleOut, jump));
            CCSequence jumpZoomIn = (CCSequence.Create((CCActionInterval) jump.Copy(), scaleIn));

            CCActionInterval delay = CCDelayTime.Create(m_fDuration / 2);

            m_pOutScene.RunAction(jumpZoomOut);
            m_pInScene.RunAction
                (
                    CCSequence.Create
                        (
                            delay,
                            jumpZoomIn,
                            CCCallFunc.Create(Finish)
                        )
                );
        }

        public new static CCTransitionJumpZoom Create(float t, CCScene scene)
        {
            var pScene = new CCTransitionJumpZoom();
            pScene.InitWithDuration(t, scene);
            return pScene;
        }
    }
}