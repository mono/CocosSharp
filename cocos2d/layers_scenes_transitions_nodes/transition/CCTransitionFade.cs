/****************************************************************************
Copyright (c) 2010-2012 cocos2d-x.org
Copyright (c) 2008-2010 Ricardo Quesada
Copyright (c) 2011 Zynga Inc.
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

using Microsoft.Xna.Framework;

namespace Cocos2D
{
    public class CCTransitionFade : CCTransitionScene
    {
        private const int kSceneFade = 2147483647;
        protected CCColor4B m_tColor;

        /// <summary>
        /// creates the transition with a duration and with an RGB color
        /// Example: FadeTransition::create(2, scene, ccc3(255,0,0); // red color
        /// </summary>
        public CCTransitionFade (float duration, CCScene scene, CCColor3B color) : base (duration, scene)
        {
            InitWithDuration(duration, scene, color);
        }

        public CCTransitionFade (float t, CCScene scene) : this (t, scene, new CCColor3B())
        {
            //return Create(t, scene, new CCColor3B());
        }

        /// <summary>
        /// initializes the transition with a duration and with an RGB color 
        /// </summary>
        protected virtual bool InitWithDuration(float duration, CCScene scene, CCColor3B color)
        {
            if (base.InitWithDuration(duration, scene))
            {
                m_tColor = new CCColor4B {R = color.R, G = color.G, B = color.B, A = 0};
            }
            return true;
        }

        protected override bool InitWithDuration(float t, CCScene scene)
        {
            InitWithDuration(t, scene, new CCColor3B(Color.Black));
            return true;
        }

        public override void OnEnter()
        {
            base.OnEnter();

            CCLayerColor l = new CCLayerColor(m_tColor);
            m_pInScene.Visible = false;

            AddChild(l, 2, kSceneFade);
            CCNode f = GetChildByTag(kSceneFade);

            var a = (CCActionInterval) new CCSequence
                                           (
                                               new CCFadeIn (m_fDuration / 2),
                                               new CCCallFunc((HideOutShowIn)),
                                               new CCFadeOut  (m_fDuration / 2),
                                               new CCCallFunc((Finish))
                                           );

            f.RunAction(a);
        }

        public override void OnExit()
        {
            base.OnExit();
            RemoveChildByTag(kSceneFade, false);
        }
    }
}