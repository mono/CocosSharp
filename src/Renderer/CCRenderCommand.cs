using System;

namespace CocosSharp
{

    internal enum CommandType
    {
        UNKNOWN_COMMAND,
        QUAD_COMMAND,
        CUSTOM_COMMAND,
        BATCH_COMMAND,
        GROUP_COMMAND,
        MESH_COMMAND,
        PRIMITIVE_COMMAND,
        TRIANGLES_COMMAND
    }

    public abstract class CCRenderCommand
    {

        internal CommandType CommandType { get; set; }
        internal float GlobalOrder { get; set; }
        internal bool IsTransparent { get; set; }
        internal bool IsSkipBatching { get; set; }
        internal bool Is3D { get; set; }
        internal float Depth { get; set; }
        internal CCAffineTransform ModelViewTransform;
        internal int MaterialId { get; set; }

        public CCRenderCommand(float globalZOrder, CCAffineTransform modelViewTransform, 
            int flags = 0, int materialId = CCRenderer.MATERIAL_ID_DO_NOT_BATCH)
        {

            if (modelViewTransform == null)
                modelViewTransform = CCAffineTransform.Identity;

            CommandType = CommandType.UNKNOWN_COMMAND;
            GlobalOrder = globalZOrder;
            IsTransparent = true;
            IsSkipBatching = false;
            Is3D = false;
            Depth = 0;

            ModelViewTransform = modelViewTransform;
            MaterialId = materialId;
        }

        internal CCRenderCommand(float globalZOrder)
            : this (globalZOrder, CCAffineTransform.Identity)
        {

        }

        internal abstract void Execute(CCDrawManager drawManager);

        public override string ToString()
        {
            return string.Format("[CCRenderCommand: Command Depth {0}]", GlobalOrder);
        }

    }
}

