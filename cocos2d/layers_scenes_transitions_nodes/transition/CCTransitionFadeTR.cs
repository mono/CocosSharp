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

namespace Cocos2D
{
    /// <summary>
    /// @brief CCTransitionFadeTR:
    /// Fade the tiles of the outgoing scene from the left-bottom corner the to top-right corner.
    /// </summary>
    public class CCTransitionFadeTR : CCTransitionScene, ICCTransitionEaseScene
    {
        #region ICCTransitionEaseScene Members

        public virtual CCFiniteTimeAction EaseAction(CCActionInterval action)
        {
            return action;
        }

        #endregion

        public virtual CCActionInterval CreateAction(CCGridSize size)
        {
            return new CCFadeOutTRTiles(m_fDuration, size);
        }

        public override void OnEnter()
        {
            base.OnEnter();

            CCSize s = CCDirector.SharedDirector.WinSize;
            float aspect = s.Width / s.Height;
            var x = (int) (12 * aspect);
            int y = 12;

            CCActionInterval action = CreateAction(new CCGridSize(x, y));

            m_pOutScene.RunAction
                (
                    new CCSequence
                        (
                            EaseAction(action),
                            new CCCallFunc(Finish),
                            new CCStopGrid()
                        )
                );
        }

        public CCTransitionFadeTR() { }
        public CCTransitionFadeTR(float t, CCScene scene) : base(t, scene)
        {
            InitWithDuration(t, scene);
        }

        protected override void SceneOrder()
        {
            m_bIsInSceneOnTop = false;
        }
    }
}