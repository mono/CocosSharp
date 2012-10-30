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
using cocos2d;

namespace tests
{
    public class Ball : CCSprite
    {
        public float radius()
        {
            return Texture.ContentSize.width / 2;
        }

        //BOOL initWithTexture(CCTexture2D* aTexture);
        //virtual void setTexture(CCTexture2D* newTexture);
        public void move(float delta)
        {
            this.Position = 
            CCPointExtension.ccpAdd(Position, CCPointExtension.ccpMult(m_velocity, delta));
            if (Position.x > 320 - radius())
            {
                Position = new CCPoint(320 - radius(), Position.y);
                m_velocity.x *= -1;
            }
            else if (Position.x < radius())
            {
                Position = new CCPoint(radius(), Position.y);
                m_velocity.x *= -1;
            }
        }

        public void collideWithPaddle(Paddle paddle)
        {
            CCRect paddleRect = paddle.rect();
            paddleRect.origin.x += paddle.Position.x;
            paddleRect.origin.y += paddle.Position.y;

            float lowY = CCRect.CCRectGetMinY(paddleRect);
            float midY = CCRect.CCRectGetMidY(paddleRect);
            float highY = CCRect.CCRectGetMaxY(paddleRect);

            float leftX = CCRect.CCRectGetMinX(paddleRect);
            float rightX = CCRect.CCRectGetMaxX(paddleRect);

            if (Position.x > leftX && Position.x < rightX)
            {

                bool hit = false;
                float angleOffset = 0.0f;

                if (Position.y > midY && Position.y <= highY + radius())
                {
                    Position = new CCPoint(Position.x, highY + radius());
                    hit = true;
                    angleOffset = (float)Math.PI / 2;
                }
                else if (Position.y < midY && Position.y >= lowY - radius())
                {
                    Position = new CCPoint(Position.x, lowY - radius());
                    hit = true;
                    angleOffset = -(float)Math.PI / 2;
                }

                if (hit)
                {
                    float hitAngle = (float)Math.Atan2(new CCPoint(paddle.Position.x - Position.x, paddle.Position.y - Position.y).y, new CCPoint(paddle.Position.x - Position.x, paddle.Position.y - Position.y).x) + angleOffset;

                    float scalarVelocity = (float)Math.Sqrt((double)(m_velocity.x * m_velocity.x + m_velocity.y * m_velocity.y)) * 1.05f;
                    float velocityAngle = -(float)Math.Atan2(m_velocity.y, m_velocity.x) + 0.5f * hitAngle;

                    m_velocity = new CCPoint(new CCPoint((float)Math.Cos(velocityAngle), (float)Math.Sin(velocityAngle)).x * scalarVelocity, new CCPoint((float)Math.Cos(velocityAngle), (float)Math.Sin(velocityAngle)).y * scalarVelocity);
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
