using System;

namespace CocosSharp
{
    internal abstract class CCRenderCommand
    {
        bool idDirty;
        long renderId;

        byte group;


        #region Properties

        protected internal long RenderId 
        { 
            get 
            { 
                if(idDirty)
                {
                    GenerateId(ref renderId);
                    idDirty = false;
                }

                return renderId;
            }
        }

        internal byte Group 
        { 
            get { return group; }
            set { group = value; idDirty = true; }
        }

        internal float GlobalDepth { get; private set; }
        internal CCAffineTransform WorldTransform { get; private set; }

        #endregion Properties


        #region Constructors

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

        public override string ToString()
        {
            return string.Format("[CCRenderCommand: Command Depth {0}]", GlobalDepth);
        }

    }
}

