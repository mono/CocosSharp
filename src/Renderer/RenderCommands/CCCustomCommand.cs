using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace CocosSharp
{
    [DebuggerDisplay("{DebugDisplayString,nq}")]
    public class CCCustomCommand : CCRenderCommand
    {
        public Action Action { get; internal set; }


        #region Constructors

        public CCCustomCommand(float globalZOrder, CCAffineTransform worldTransform, Action action) 
            : base(globalZOrder, worldTransform)
        {
            Action = action;
        }

        public CCCustomCommand(float globalZOrder, Action action)
            : this(globalZOrder, CCAffineTransform.Identity, action)
        {
        }

        public CCCustomCommand(Action action)
            : this(0.0f, CCAffineTransform.Identity, action)
        {
        }

        protected CCCustomCommand(CCCustomCommand copy)
            : base(copy)
        {
            Action = copy.Action; 
        }

        public override CCRenderCommand Copy()
        {
            return new CCCustomCommand(this);
        }

        #endregion Constructors


        internal override void RequestRenderCommand(CCRenderer renderer)
        {
            if(Action != null)
                renderer.ProcessCustomRenderCommand(this);
        }

        internal void RenderCustomCommand(CCDrawManager drawManager)
        {
            bool originalDepthTestState = drawManager.DepthTest;
            drawManager.DepthTest = UsingDepthTest;

            drawManager.PushMatrix();
            drawManager.SetIdentityMatrix();

            if (WorldTransform != CCAffineTransform.Identity)
            {
                var worldTrans = WorldTransform.XnaMatrix;
                drawManager.MultMatrix(ref worldTrans);
            }
            Action();

            drawManager.PopMatrix();

            drawManager.DepthTest = originalDepthTestState;
        }

        internal new string DebugDisplayString
        {
            get
            {
                return ToString();
            }
        }

        public override string ToString()
        {
            return string.Concat("[CCCustomCommand: Group ", Group.ToString(), " Depth ", GlobalDepth.ToString(),"]");
        }
    }
}

