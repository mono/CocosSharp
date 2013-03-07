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
using System.Diagnostics;

namespace tests
{
    public enum PaddleState
    {
        kPaddleStateGrabbed,
        kPaddleStateUngrabbed
    }

    public class Paddle : CCSprite, ICCTargetedTouchDelegate
    {
        PaddleState m_state;
        public CCRect rect()
        {
            CCSize s = Texture.ContentSize;
            return new CCRect(-s.Width / 2, -s.Height / 2, s.Width, s.Height);
        }

        public bool initWithTexture(CCTexture2D aTexture)
        {
            if (base.InitWithTexture(aTexture))
            {
                m_state = PaddleState.kPaddleStateUngrabbed;
            }
            return true;
        }

        public override void OnEnter()
        {
            CCDirector.SharedDirector.TouchDispatcher.AddTargetedDelegate(this, 0, true);
            base.OnEnter();
        }

        public override void OnExit()
        {
            CCDirector.SharedDirector.TouchDispatcher.RemoveDelegate(this);
            base.OnExit();
        }

        public bool containsTouchLocation(CCTouch touch)
        {
            return CCRect.CCRectContainsPoint(rect(), ConvertTouchToNodeSpaceAr(touch));
        }

        public virtual bool TouchBegan(CCTouch touch, CCEvent eventer)
        {
            if (m_state != PaddleState.kPaddleStateUngrabbed) return false;
            if (!containsTouchLocation(touch)) return false;

            m_state = PaddleState.kPaddleStateGrabbed;
            return true;
        }

        public virtual void TouchMoved(CCTouch touch, CCEvent eventer)
        {
            // If it weren't for the TouchDispatcher, you would need to keep a reference
            // to the touch from touchBegan and check that the current touch is the same
            // as that one.
            // Actually, it would be even more complicated since in the Cocos dispatcher
            // you get CCSets instead of 1 UITouch, so you'd need to loop through the set
            // in each touchXXX method.

            Debug.Assert(m_state == PaddleState.kPaddleStateGrabbed, "Paddle - Unexpected state!");

            CCPoint touchPoint = touch.LocationInView;
            touchPoint = CCDirector.SharedDirector.ConvertToGl(touchPoint);

            base.Position = new CCPoint(touchPoint.X, base.Position.Y);
        }

        public virtual void TouchEnded(CCTouch touch, CCEvent eventer)
        {
            Debug.Assert(m_state == PaddleState.kPaddleStateGrabbed, "Paddle - Unexpected state!");
            m_state = PaddleState.kPaddleStateUngrabbed;
        }

        public static Paddle paddleWithTexture(CCTexture2D aTexture)
        {
            Paddle pPaddle = new Paddle();
            pPaddle.initWithTexture(aTexture);
            //pPaddle->autorelease();

            return pPaddle;
        }

        public void TouchCancelled(CCTouch pTouch, CCEvent pEvent)
        {
            throw new NotImplementedException();
        }
    }
}
