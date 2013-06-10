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
    /// @brief A transition which peels back the bottom right hand corner of a scene
    /// to transition to the scene beneath it simulating a page turn.
    ///    This uses a 3DAction so it's strongly recommended that depth buffering
    /// is turned on in CCDirector using:
    /// CCDirector::sharedDirector()->setDepthBufferFormat(kDepthBuffer16);
    /// @since v0.8.2
    /// </summary>
    public class CCTransitionPageTurn : CCTransitionScene
    {
        protected bool m_bBack;

        public CCTransitionPageTurn() { }
        
        /// <summary>
        /// Creates a base transition with duration and incoming scene.
        /// If back is true then the effect is reversed to appear as if the incoming 
        /// scene is being turned from left over the outgoing scene.
        /// </summary>
        public CCTransitionPageTurn (float t, CCScene scene, bool backwards)
        {
            // We can not call base here because m_bBack needs to be set first
            InitWithDuration(t, scene, backwards);
        }

        /// <summary>
        /// Creates a base transition with duration and incoming scene.
        /// If back is true then the effect is reversed to appear as if the incoming 
        /// scene is being turned from left over the outgoing scene.
        /// </summary>
        public virtual bool InitWithDuration(float t, CCScene scene, bool backwards)
        {
            // XXX: needed before [super init]
            m_bBack = backwards;

            if (base.InitWithDuration(t, scene))
            {
                // do something
            }

            return true;
        }

        public CCActionInterval ActionWithSize(CCGridSize vector)
        {
            if (m_bBack)
            {
                // Get hold of the PageTurn3DAction
                return new CCReverseTime
                    (
                        new CCPageTurn3D (m_fDuration, vector)
                    );
            }
            else
            {
                // Get hold of the PageTurn3DAction
                return new CCPageTurn3D (m_fDuration, vector);
            }
        }

        public override void OnEnter()
        {
            base.OnEnter();

            CCSize s = CCDirector.SharedDirector.WinSize;
            int x, y;
            if (s.Width > s.Height)
            {
                x = 16;
                y = 12;
            }
            else
            {
                x = 12;
                y = 16;
            }

            CCActionInterval action = ActionWithSize(CCTypes.GridSize(x, y));

            if (!m_bBack)
            {
                m_pOutScene.RunAction(new CCSequence
                                          (
                                              action,
                                              new CCCallFunc(Finish),
                                              new CCStopGrid()));
            }
            else
            {
                // to prevent initial flicker
                m_pInScene.Visible = false;
                m_pInScene.RunAction(new CCSequence
                                         (
                                             new CCShow(),
                                             action,
                                             new CCCallFunc(Finish),
                                             new CCStopGrid()));
            }
        }

        protected override void SceneOrder()
        {
            m_bIsInSceneOnTop = m_bBack;
        }
    }
}
