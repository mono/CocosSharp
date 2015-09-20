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

namespace CocosSharp
{
    public class CCTransitionSplitCols : CCTransitionScene
    {
        protected virtual CCFiniteTimeAction Action
        {
            get { return new CCSplitCols(Duration / 2.0f, 3); }
        }

        protected override CCFiniteTimeAction OutSceneAction
        {
            get
            {
                return new CCSequence(
                    Action,
                    new CCCallFunc((HideOutShowIn)),
                    new CCDelayTime(Duration / 2.0f)
                );
            }
        }

        protected override CCFiniteTimeAction InSceneAction
        {
            get
            {
                return new CCSequence(
                    new CCDelayTime(Duration / 2.0f),
                    Action.Reverse()
                );
            }
        }

        #region Constructors

        public CCTransitionSplitCols(float t, CCScene scene) : base(t, scene)
        {
        }

        #endregion Constructors

        public virtual CCFiniteTimeAction EaseAction(CCFiniteTimeAction action)
        {
            return new CCEaseInOut(action, 3.0f);
        }


        protected override void InitialiseScenes()
        {
            base.InitialiseScenes();

            InSceneNodeContainer.Visible = false;
        }
    }
}