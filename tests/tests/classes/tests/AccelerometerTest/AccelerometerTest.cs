using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using cocos2d;

namespace tests
{
    public class AccelerometerTest : CCLayer
    {

        protected CCSprite m_pBall;
        protected double m_fLastTime;

        public override void DidAccelerate(CCAcceleration pAccelerationValue)
        {
            CCDirector pDir = CCDirector.SharedDirector;
            CCSize winSize = pDir.WinSize;

            /*FIXME: Testing on the Nexus S sometimes m_pBall is NULL */
            if (m_pBall == null)
            {
                return;
            }

            CCSize ballSize = m_pBall.ContentSize;

            CCPoint ptNow = m_pBall.Position;
            CCPoint ptTemp = pDir.ConvertToUi(ptNow);

            ptTemp.X += (float) pAccelerationValue.X * 9.81f;
            ptTemp.Y -= (float) pAccelerationValue.Y * 9.81f;

            CCPoint ptNext = pDir.ConvertToGl(ptTemp);
            ptNext.X = MathHelper.Clamp(ptNext.X, (ballSize.Width / 2.0f), (winSize.Width - ballSize.Width / 2.0f));
            ptNext.Y = MathHelper.Clamp(ptNext.Y, (ballSize.Height / 2.0f), (winSize.Height - ballSize.Height / 2.0f));
            m_pBall.Position = ptNext;
        }

        public virtual string title()
        {
            return "AccelerometerTest";
        }

        public override void OnEnter()
        {
            base.OnEnter();

            AccelerometerEnabled = true;

            CCSize s = CCDirector.SharedDirector.WinSize;

            CCLabelTTF label = CCLabelTTF.Create(title(), "Arial", 32);
            AddChild(label, 1);
            label.Position = new CCPoint(s.Width / 2, s.Height - 50);

            m_pBall = new CCSprite("Images/ball");
            m_pBall.Position = new CCPoint(s.Width / 2, s.Height / 2);
            AddChild(m_pBall);
        }
    }

    public class AccelerometerTestScene : TestScene
    {
        protected override void NextTestCase()
        {
        }
        protected override void PreviousTestCase()
        {
        }
        protected override void RestTestCase()
        {
        }
        public override void runThisTest()
        {
            CCLayer pLayer = new AccelerometerTest();
            AddChild(pLayer);

            CCDirector.SharedDirector.ReplaceScene(this);
        }
    }
}
