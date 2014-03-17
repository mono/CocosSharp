using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using CocosSharp;

namespace tests
{
    public class AccelerometerTest : CCLayer
    {

        protected CCSprite m_pBall;
        protected double m_fLastTime;

		public void DidAccelerate(CCAcceleration accelerationValue)
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

			var orientation = CCApplication.SharedApplication.CurrentOrientation;

			//CCLog.Log("Accelerate : X: {0} Y: {1} Z: {2} orientation: {3}", accelerationValue.X, accelerationValue.Y, accelerationValue.Z, orientation );
#if ANDROID || WINDOWS_PHONE8
            if (orientation == DisplayOrientation.LandscapeRight)
			{
				ptTemp.X -= (float) accelerationValue.X * 9.81f;
				ptTemp.Y -= (float) accelerationValue.Y * 9.81f;
			}
			else
			{
				ptTemp.X += (float) accelerationValue.X * 9.81f;
				ptTemp.Y += (float) accelerationValue.Y * 9.81f;
			}
#else
            //ptTemp.X -= (float) pAccelerationValue.Y * 9.81f;
            //ptTemp.Y -= (float) pAccelerationValue.X * 9.81f;
            ptTemp.X += (float)accelerationValue.X * 9.81f;
            ptTemp.Y += (float)accelerationValue.Y * 9.81f;
#endif

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

            CCLabelTtf label = new CCLabelTtf(title(), "Arial", 32);
            AddChild(label, 1);
            label.Position = new CCPoint(s.Width / 2, s.Height - 50);

            m_pBall = new CCSprite("Images/ball");
            m_pBall.Position = s.Center;
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
