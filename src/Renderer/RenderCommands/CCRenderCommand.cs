using System;

namespace CocosSharp
{
    internal abstract class CCRenderCommand
    {
        #region Properties

        protected internal long RenderId { get; protected set; }
        internal float GlobalDepth { get; private set; }
        internal CCAffineTransform WorldTransform { get; private set; }
        internal CCRenderer.CCCommandType CommandType { get; set; }

        #endregion Properties


        #region Constructors

        public CCRenderCommand(float gobalDepth, CCAffineTransform worldTransform)
        {
            GlobalDepth = gobalDepth;
            WorldTransform = worldTransform;

            GenerateId();
        }

        internal CCRenderCommand(float globalZOrder)
            : this(globalZOrder, CCAffineTransform.Identity)
        {
        }

        void GenerateId()
        {
            // 64 - 57 : Group id (byte)
            // 56 - 25 : Global depth (float)
            // 24 - 1 : Material id (24 bit)

            RenderId = (long)0x0 << 56
                | (long)GlobalDepth << 24
                | (long)0x0;
        }

        #endregion Constructors


        internal abstract void RequestRenderCommand(CCRenderer renderer);

        public override string ToString()
        {
            return string.Format("[CCRenderCommand: Command Depth {0}]", GlobalDepth);
        }

    }
}

