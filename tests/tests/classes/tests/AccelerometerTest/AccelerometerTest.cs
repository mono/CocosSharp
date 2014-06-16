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
        CCLabelTtf titleLabel;
        CCLabelTtf subtitleLabel;

        CCSprite ball;
        double lastTime;


        #region Properties

        public virtual string Title
        {
            get { return "AccelerometerTest"; }
        }

        public virtual string Subtitle
        {
            get { return "Use arrow keys to simulate"; }
        }

        #endregion Properties


        #region Constructors

        public AccelerometerTest()
        {
            titleLabel = new CCLabelTtf(Title, "Arial", 32);
            AddChild(titleLabel, 1);

            string subtitleStr = Subtitle;
            if (subtitleStr.Length > 0)
            {
                subtitleLabel = new CCLabelTtf(subtitleStr, "arial", 16);
                AddChild(subtitleLabel, 1);
            }

            ball = new CCSprite("Images/ball");
            AddChild(ball);
        }

        #endregion Constructors


        #region Setup content

        protected override void RunningOnNewWindow(CCSize windowSize)
        {
            base.RunningOnNewWindow(windowSize);

            titleLabel.Position = new CCPoint(windowSize.Width / 2, windowSize.Height - 50);

            if(subtitleLabel != null)
                subtitleLabel.Position = (new CCPoint(windowSize.Width / 2, windowSize.Height - 80));

            ball.Position = windowSize.Center;


            Director.Accelerometer.Enabled = true;

            // Register Touch Event
            var accelListener = new CCEventListenerAccelerometer();

            accelListener.OnAccelerate = DidAccelerate;

            EventDispatcher.AddEventListener(accelListener, this); 
        }

        #endregion Setup content


        public void DidAccelerate(CCEventAccelerate accelEvent)
        {
            CCSize winSize = Director.WindowSizeInPoints;

            /*FIXME: Testing on the Nexus S sometimes ball is NULL */
            if (ball == null)
            {
                return;
            }

            CCSize ballSize = ball.ContentSize;

            CCPoint ptNow = ball.Position;
            CCPoint ptTemp = Director.ConvertToUi(ptNow);

            var orientation = CCApplication.SharedApplication.CurrentOrientation;

            #if ANDROID || WINDOWS_PHONE8
            if (orientation == CCDisplayOrientation.LandscapeRight)
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
            ptTemp.X += (float)accelEvent.Acceleration.X * 9.81f;
            ptTemp.Y += (float)accelEvent.Acceleration.Y * 9.81f;
            #endif

            CCPoint ptNext = Director.ConvertToGl(ptTemp);
            ptNext.X = MathHelper.Clamp(ptNext.X, (ballSize.Width / 2.0f), (winSize.Width - ballSize.Width / 2.0f));
            ptNext.Y = MathHelper.Clamp(ptNext.Y, (ballSize.Height / 2.0f), (winSize.Height - ballSize.Height / 2.0f));
            ball.Position = ptNext;
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
            CCLayer layer = new AccelerometerTest();
            AddChild(layer);

            Director.ReplaceScene(this);
        }
    }
}
