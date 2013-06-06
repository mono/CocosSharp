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
using Cocos2D;

namespace tests
{
    public class Ball : CCSprite
    {
        public float radius()
        {
            return Texture.ContentSize.Width / 2;
        }

        //BOOL initWithTexture(CCTexture2D* aTexture);
        //virtual void setTexture(CCTexture2D* newTexture);
        public void move(float delta)
        {
            this.Position = Position + (m_velocity * delta);

            if (Position.X > 320 - radius())
            {
                Position = new CCPoint(320 - radius(), Position.Y);
                m_velocity.X *= -1;
            }
            else if (Position.X < radius())
            {
                Position = new CCPoint(radius(), Position.Y);
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
                    float hitAngle = (float)Math.Atan2(new CCPoint(paddle.Position.X - Position.X, paddle.Position.Y - Position.Y).Y, new CCPoint(paddle.Position.X - Position.X, paddle.Position.Y - Position.Y).X) + angleOffset;

                    float scalarVelocity = (float)Math.Sqrt((double)(m_velocity.X * m_velocity.X + m_velocity.Y * m_velocity.Y)) * 1.05f;
                    float velocityAngle = -(float)Math.Atan2(m_velocity.Y, m_velocity.X) + 0.5f * hitAngle;

                    m_velocity = new CCPoint(new CCPoint((float)Math.Cos(velocityAngle), (float)Math.Sin(velocityAngle)).X * scalarVelocity, new CCPoint((float)Math.Cos(velocityAngle), (float)Math.Sin(velocityAngle)).Y * scalarVelocity);
                }
            }
        }

        public static Ball ballWithTexture(CCTexture2D aTexture)
        {
            Ball pBall = new Ball();
            pBall.InitWithTexture(aTexture);
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
