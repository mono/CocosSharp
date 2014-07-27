/****************************************************************************
Copyright (c) 2010-2012 cocos2d-x.org
Copyright (c) 2008-2010 Ricardo Quesada
Copyright (c) 2011 Zynga Inc.

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

namespace CocosSharp
{
    public class CCTransitionShrinkGrow : CCTransitionScene, ICCTransitionEaseScene
    {
        public CCTransitionShrinkGrow (float t, CCScene scene) : base (t, scene)
        { }

        #region ICCTransitionEaseScene Members

        public CCFiniteTimeAction EaseAction(CCFiniteTimeAction action)
        {
            return new CCEaseOut(action, 2.0f);
        }

        #endregion

        public override void OnEnter()
        {
            base.OnEnter();

            InScene.Scale = 0.001f;
            OutScene.Scale = (1.0f);

            InScene.AnchorPoint = new CCPoint(2 / 3.0f, 0.5f);
            OutScene.AnchorPoint = new CCPoint(1 / 3.0f, 0.5f);

            CCFiniteTimeAction scaleOut = new CCScaleTo(Duration, 0.01f);
            CCFiniteTimeAction scaleIn = new CCScaleTo(Duration, 1.0f);

            InScene.RunAction(EaseAction(scaleIn));
            OutScene.RunAction
                (
                    new CCSequence
                        (
                            EaseAction(scaleOut),
                            new CCCallFunc((Finish))
                        )
                );
        }

    }
}