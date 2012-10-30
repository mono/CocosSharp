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

namespace cocos2d
{
    public class CCTransitionFade : CCTransitionScene
    {
        private const int kSceneFade = 2147483647;
        protected ccColor4B m_tColor;

        /// <summary>
        /// creates the transition with a duration and with an RGB color
        /// Example: FadeTransition::create(2, scene, ccc3(255,0,0); // red color
        /// </summary>
        public static CCTransitionFade Create(float duration, CCScene scene, ccColor3B color)
        {
            var pTransition = new CCTransitionFade();
            pTransition.InitWithDuration(duration, scene, color);
            return pTransition;
        }

        /// <summary>
        /// initializes the transition with a duration and with an RGB color 
        /// </summary>
        public virtual bool InitWithDuration(float duration, CCScene scene, ccColor3B color)
        {
            if (base.InitWithDuration(duration, scene))
            {
                m_tColor = new ccColor4B {r = color.r, g = color.g, b = color.b, a = 0};
            }
            return true;
        }

        public new static CCTransitionScene Create(float t, CCScene scene)
        {
            return Create(t, scene, new ccColor3B());
        }

        public override bool InitWithDuration(float t, CCScene scene)
        {
            InitWithDuration(t, scene, new ccColor3B(Color.Black));
            return true;
        }

        public override void OnEnter()
        {
            base.OnEnter();

            CCLayerColor l = CCLayerColor.Create(m_tColor);
            m_pInScene.Visible = false;

            AddChild(l, 2, kSceneFade);
            CCNode f = GetChildByTag(kSceneFade);

            var a = (CCActionInterval) CCSequence.Create
                                           (
                                               CCFadeIn.Create(m_fDuration / 2),
                                               CCCallFunc.Create((HideOutShowIn)),
                                               CCFadeOut.Create(m_fDuration / 2),
                                               CCCallFunc.Create((Finish))
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