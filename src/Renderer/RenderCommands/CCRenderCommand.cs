using System;
using System.Diagnostics;

namespace CocosSharp
{
    [DebuggerDisplay("{DebugDisplayString,nq}")]
    internal abstract class CCRenderCommand
    {
        long renderId;
        byte group;
        float globalDepth;
        CCAffineTransform worldTransform;


        #region Properties

        protected bool RenderIdDirty { get; set; }

        protected internal long RenderId 
        { 
            get 
            { 
                if(RenderIdDirty)
                {
                    GenerateId(ref renderId);
                    RenderIdDirty = false;
                }

                return renderId;
            }
        }

        internal byte Group 
        { 
            get { return group; }
            set { group = value; RenderIdDirty = true; }
        }

        internal float GlobalDepth
        {
            get { return globalDepth; }
            set { globalDepth = value; RenderIdDirty = true; }
        }

        internal CCAffineTransform WorldTransform 
        { 
            get { return worldTransform; }
            set { worldTransform = value; RenderIdDirty = true; }
        }

        #endregion Properties


        #region Constructors

        public CCRenderCommand() {}

        public CCRenderCommand(float gobalDepth, CCAffineTransform worldTransform)
        {
            GlobalDepth = gobalDepth;
            WorldTransform = worldTransform;

            GenerateId(ref renderId);
        }

        internal CCRenderCommand(float globalZOrder)
            : this(globalZOrder, CCAffineTransform.Identity)
        {
        }

        protected virtual void GenerateId(ref long renderId)
        {
            // 64 - 57 : Group id (byte)
            // 56 - 25 : Global depth (float)
            // 24 - 1 : Material id (24 bit)

            renderId = (long)Group << 56
                | (long)GlobalDepth << 24
                | (long)0x0;
        }

        #endregion Constructors


        internal abstract void RequestRenderCommand(CCRenderer renderer);

        internal string DebugDisplayString
        {
            get
            {
                return ToString();
            }
        }

        public override string ToString()
        {
            return string.Concat("[CCRenderCommand: Group ", Group.ToString(), " Depth ", GlobalDepth.ToString(),"]");
        }

    }
}

