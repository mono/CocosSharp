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

namespace CocosSharp
{
    public class CCTransitionZoomFlipY : CCTransitionSceneOriented
    {
		#region Constructors

        public CCTransitionZoomFlipY (float t, CCScene s, CCTransitionOrientation o) : base (t, s, o)
        { 
		}

		#endregion Constructors


        public override void OnEnter()
        {
            base.OnEnter();

            CCActionInterval inA, outA;
            InScene.Visible = false;

            float inDeltaZ, inAngleZ;
            float outDeltaZ, outAngleZ;

            if (Orientation == CCTransitionOrientation.UpOver)
            {
                inDeltaZ = 90;
                inAngleZ = 270;
                outDeltaZ = 90;
                outAngleZ = 0;
            }
            else
            {
                inDeltaZ = -90;
                inAngleZ = 90;
                outDeltaZ = -90;
                outAngleZ = 0;
            }

            inA = new CCSequence
                (
                    new CCDelayTime (Duration / 2),
                    new CCSpawn
                        (
                            new CCOrbitCamera(Duration / 2, 1, 0, inAngleZ, inDeltaZ, 90, 0),
                            new CCScaleTo(Duration / 2, 1),
                            new CCShow()
                        ),
                    new CCCallFunc(Finish)
                );

            outA = new CCSequence
                (
                    new CCSpawn
                        (
                            new CCOrbitCamera(Duration / 2, 1, 0, outAngleZ, outDeltaZ, 90, 0),
                            new CCScaleTo(Duration / 2, 0.5f)
                        ),
                    new CCHide(),
                    new CCDelayTime (Duration / 2)
                );

            InScene.Scale = 0.5f;
            InScene.RunAction(inA);
            OutScene.RunAction(outA);
        }
    }
}