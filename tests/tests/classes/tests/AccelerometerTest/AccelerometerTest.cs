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
        CCLabel titleLabel;
        CCLabel subtitleLabel;

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
            titleLabel = new CCLabel(Title, "Arial", 40, CCLabelFormat.SpriteFont);
            titleLabel.AnchorPoint = new CCPoint(0.5f, 0.5f);
            titleLabel.VerticalAlignment = CCVerticalTextAlignment.Center;

            AddChild(titleLabel, 1);

            string subtitleStr = Subtitle;
            if (subtitleStr.Length > 0)
            {
                subtitleLabel = new CCLabel(subtitleStr, "arial", 20, CCLabelFormat.SpriteFont);
                subtitleLabel.AnchorPoint = new CCPoint(0.5f, 0.5f);
                subtitleLabel.VerticalAlignment = CCVerticalTextAlignment.Center;
                AddChild(subtitleLabel, 1);
            }

            ball = new CCSprite("Images/ball");
            AddChild(ball);
        }

        #endregion Constructors


        #region Setup content

        public override void OnEnter()
        {
            base.OnEnter(); CCSize windowSize = Layer.VisibleBoundsWorldspace.Size;

            titleLabel.Position = new CCPoint(windowSize.Width / 2, windowSize.Height - 80);

            if(subtitleLabel != null)
                subtitleLabel.Position = (new CCPoint(windowSize.Width / 2, windowSize.Height - 120));

            ball.Position = windowSize.Center;

#if !NETFX_CORE
            GameView.Accelerometer.Enabled = true;
#endif
            // Register Touch Event
            var accelListener = new CCEventListenerAccelerometer();

            accelListener.OnAccelerate = DidAccelerate;

            AddEventListener(accelListener); 
        }

        #endregion Setup content


        public void DidAccelerate(CCEventAccelerate accelEvent)
        {
            CCSize winSize = Layer.VisibleBoundsWorldspace.Size;

            /*FIXME: Testing on the Nexus S sometimes ball is NULL */
            if (ball == null)
            {
                return;
            }

            CCSize ballSize = ball.ContentSize;

            CCPoint ptNow = ball.PositionWorldspace;
            CCPoint ptTemp = Layer.WorldToScreenspace(ptNow);

            var orientation = Application.CurrentOrientation;

            if (accelEvent.Acceleration.X == 0.0f && accelEvent.Acceleration.Y == 0.0f)
                return;

            #if ANDROID || WINDOWS_PHONE8
            if (orientation == CCDisplayOrientation.LandscapeRight)
            {
                ptTemp.X -= (float) accelEvent.Acceleration.X * 9.81f;
                ptTemp.Y -= (float) accelEvent.Acceleration.Y * 9.81f;
            }
            else
            {
                ptTemp.X += (float) accelEvent.Acceleration.X * 9.81f;
                ptTemp.Y += (float) accelEvent.Acceleration.Y * 9.81f;
            }
            #else
            ptTemp.X += (float)accelEvent.Acceleration.X * 9.81f;
            ptTemp.Y += (float)accelEvent.Acceleration.Y * 9.81f;
            #endif

            CCPoint ptNext = Layer.ScreenToWorldspace(ptTemp);
            ptNext.X = MathHelper.Clamp(ptNext.X, (ballSize.Width / 2.0f), (winSize.Width - ballSize.Width / 2.0f));
            ptNext.Y = MathHelper.Clamp(ptNext.Y, (ballSize.Height / 2.0f), (winSize.Height - ballSize.Height / 2.0f));
            ball.Position = ball.WorldToParentspace(ptNext);
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
