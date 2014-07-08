/****************************************************************************
Copyright (c) 2010-2012 cocos2d-x.org
Copyright (c) 2008-2009 Jason Booth
Copyright (c) 2011-2012 openxlive.com

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
****************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class PongLayer : CCLayer
    {

		float kStatusBarHeight = 0.0f;
        string s_Ball = "Images/ball";
        string s_Paddle = "Images/paddle";
        Ball m_ball;
        List<Paddle> paddles;
        CCPoint m_ballStartingVelocity;

        public PongLayer()
        {
            m_ballStartingVelocity = new CCPoint(20.0f, -100.0f);

            m_ball = Ball.ballWithTexture(CCApplication.SharedApplication.TextureCache.AddImage(s_Ball));
            m_ball.Position = new CCPoint(160.0f, 240.0f);
            m_ball.Velocity = m_ballStartingVelocity;
            AddChild(m_ball);

            CCTexture2D paddleTexture = CCApplication.SharedApplication.TextureCache.AddImage(s_Paddle);

            var paddlesM = new List<Paddle>(4);

            Paddle paddle = new Paddle(paddleTexture);
            paddlesM.Add(paddle);

            paddle = new Paddle(paddleTexture);
            paddle.Position = new CCPoint(160, 480 - 20f - 15);
            paddlesM.Add(paddle);

            paddle = new Paddle(paddleTexture);
            paddle.Position = new CCPoint(160, 100);
            paddlesM.Add(paddle);

            paddle = new Paddle(paddleTexture);
            paddle.Position = new CCPoint(160, 480 - 20.0f - 100);
            paddlesM.Add(paddle);

			paddles = paddlesM;

            for (int i = 0; i < paddles.Count; i++)
            {
                paddle = paddles[i];

                if (paddle == null) break;

                AddChild(paddle);
            }
            Schedule(this.doStep);
        }

		protected override void RunningOnNewWindow(CCSize windowSize)
		{
			base.RunningOnNewWindow(windowSize);

			paddles[0].Position = new CCPoint(CCVisibleRect.Center.X, CCVisibleRect.Bottom.Y + 15);
			paddles[1].Position = new CCPoint(CCVisibleRect.Center.X, CCVisibleRect.Top.Y - kStatusBarHeight - 15);
			paddles[2].Position = new CCPoint(CCVisibleRect.Center.X, CCVisibleRect.Bottom.Y + 100);
			paddles[3].Position = new CCPoint(CCVisibleRect.Center.X, CCVisibleRect.Top.Y - kStatusBarHeight - 100);

		}


        public void resetAndScoreBallForPlayer(int player)
        {
            m_ballStartingVelocity = new CCPoint(m_ballStartingVelocity.X * -1.1f, m_ballStartingVelocity.Y * -1.1f);
            m_ball.Velocity = m_ballStartingVelocity;
            m_ball.Position = new CCPoint(160.0f, 240.0f);

            // TODO -- scoring
        }
        public void doStep(float delta)
        {
            m_ball.move(delta);

            Paddle paddle;
            for (int i = 0; i < paddles.Count; i++)
            {
                paddle = (Paddle)paddles[i];
                if (paddle == null) break;
                m_ball.collideWithPaddle(paddle);
            }

            if (m_ball.Position.Y > 480 - 20.0f + m_ball.radius())
                resetAndScoreBallForPlayer((int)PlayerTouches.kLowPlayer);
            else if (m_ball.Position.Y < -m_ball.radius())
                resetAndScoreBallForPlayer((int)PlayerTouches.kHighPlayer);

            // this code exists in c++ version,
            // but it seems unnecessary here, and it causes a redundant ball bug
            // m_ball.draw();
        }
    }
    public enum PlayerTouches
    {
        kHighPlayer,
        kLowPlayer
    }
}
