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
    public class Ball : CCSprite
    {
        public Ball(CCTexture2D tex): base(tex)
        {
        }

        public float radius()
        {
			return Texture.ContentSizeInPixels.Width / 2;
        }

        //BOOL initWithTexture(CCTexture2D* aTexture);
        //virtual void setTexture(CCTexture2D* newTexture);
        public void move(float delta)
        {
            this.Position = Position + (m_velocity * delta);

            if (Position.X > CCVisibleRect.Right.X - radius())
            {
				Position = new CCPoint(CCVisibleRect.Right.X - radius(), Position.Y);
                m_velocity.X *= -1;
            }
			else if (Position.X < CCVisibleRect.Left.X + radius())
            {
				Position = new CCPoint(CCVisibleRect.Left.X + radius(), Position.Y);
                m_velocity.X *= -1;
            }
        }

        public void collideWithPaddle(Paddle paddle)
        {
            CCRect paddleRect = paddle.rect();
            paddleRect.Origin.X += paddle.Position.X;
            paddleRect.Origin.Y += paddle.Position.Y;

            float lowY = paddleRect.MinY;
            float midY = paddleRect.MidY;
            float highY = paddleRect.MaxY;

            float leftX = paddleRect.MinX;
            float rightX = paddleRect.MaxX;

            if (Position.X > leftX && Position.X < rightX)
            {

                bool hit = false;
                float angleOffset = 0.0f;

                if (Position.Y > midY && Position.Y <= highY + radius())
                {
                    Position = new CCPoint(Position.X, highY + radius());
                    hit = true;
                    angleOffset = (float)Math.PI / 2;
                }
                else if (Position.Y < midY && Position.Y >= lowY - radius())
                {
                    Position = new CCPoint(Position.X, lowY - radius());
                    hit = true;
                    angleOffset = -(float)Math.PI / 2;
                }

                if (hit)
                {
					float hitAngle = (paddle.Position - Position).Angle + angleOffset;
                    float scalarVelocity = m_velocity.Length * 1.05f;
                    float velocityAngle = -m_velocity.Angle + 0.5f * hitAngle;

					m_velocity = CCPoint.ForAngle(velocityAngle) * scalarVelocity;
                }
            }
        }

        public static Ball ballWithTexture(CCTexture2D aTexture)
        {
            Ball pBall = new Ball(aTexture);
            //pBall->autorelease();

            return pBall;
        }

        CCPoint m_velocity;
        public CCPoint Velocity
        {
            get { return m_velocity; }
            set { m_velocity = value; }
        }
    }
}
