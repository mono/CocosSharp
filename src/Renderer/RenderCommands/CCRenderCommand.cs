using System;
using System.Diagnostics;

namespace CocosSharp
{
    [DebuggerDisplay("{DebugDisplayString,nq}")]
    public abstract class CCRenderCommand : IComparable<CCRenderCommand>
    {
        long renderFlags;


        #region Properties

        protected bool RenderFlagsDirty { get; set; }

        protected internal long RenderFlags 
        { 
            get 
            { 
                if(RenderFlagsDirty)
                {
                    GenerateFlags(ref renderFlags);
                    RenderFlagsDirty = false;
                }

                return renderFlags;
            }
        }

        internal bool UsingDepthTest { get; set; }
        internal uint ArrivalIndex { get; set; }
        internal byte ViewportGroup { get; set; }
        internal byte LayerGroup { get; set; }
        internal byte Group { get; set; }
        internal float GlobalDepth { get; set; }
        internal CCAffineTransform WorldTransform { get; set; }

        internal string DebugDisplayString
        {
            get { return ToString(); }
        }

        #endregion Properties


        #region Constructors

        public CCRenderCommand(float gobalDepth, CCAffineTransform worldTransform)
        {
            GlobalDepth = gobalDepth;
            WorldTransform = worldTransform;

            GenerateFlags(ref renderFlags);
        }

        public CCRenderCommand()
            : this(0, CCAffineTransform.Identity)
        {
        }

        protected CCRenderCommand(CCRenderCommand copy)
            : this(copy.GlobalDepth, copy.WorldTransform)
        {
            renderFlags = copy.renderFlags;
            RenderFlagsDirty = copy.RenderFlagsDirty;

            // All other ivars should be updated dy dynamically depending on where it was added
            // to the render queue
        }


        protected virtual void GenerateFlags(ref long renderFlags)
        {
            renderFlags = 0;
        }

        public abstract CCRenderCommand Copy();

        #endregion Constructors


        public override string ToString()
        {
            return String.Format("[CCRenderCommand: LayerGroup {0} Group {1} GlobalDepth {2} Flags {3}]", 
                LayerGroup, Group, GlobalDepth, RenderFlags);
        }

        // The issue with using a single render id to order commands is the handling of
        // depth which is a floating point number that can't simply be cast as an int value
        // due to the potential of overflow/rounding errors
        // So instead we first perform the layer group, group, depth and arrival index comparisons individually
        // and then package any remaining traits for comparison (e.g. Material) into the RenderFlags
        public int CompareTo(CCRenderCommand otherCommand)
        {
            // Sort groups in descending order
            // This is because the typical use case is to group nodes into a render texture
            // Then the render texture's corresponding sprite is then rendered to the scene
            // So the render texture needs to be filled beforehand or else we will have a corrupted sprite
            int compare = Group.CompareTo(otherCommand.Group) * -1;

            if(compare == 0)
            {
                // Only sort by depth if depth testing is used by both commands
                // Main issue is that oftentimes a pure 2d game will typically have all nodes with vertex z = 0
                // Normally, sorting by depth wouldn't be a problem - depths are equal so compare using other traits below
                // However, if a 3d effect (e.g. orbit camera) is used then vertex z of nodes will be altered
                // Here, floating-point errors plus the inprecision of the depth-buffer the further a node is from a camera
                // means that two nodes originally with vertex z = 0 may not have the exact same vertex z post-effect
                compare = UsingDepthTest && otherCommand.UsingDepthTest ? GlobalDepth.CompareTo(otherCommand.GlobalDepth) : 0; 

                // If all traits are equal, then use the arrival index to differentiate
                // This is necessary because we rely on quick sort to sort our command queue which is
                // an unstable sorting algorithm
                // i.e. Does not guarantee to presever order between two equal elements
                if(compare == 0)
                {
                    compare = ArrivalIndex.CompareTo(otherCommand.ArrivalIndex);

                    if(compare == 0)
                    {
                        compare = RenderFlags.CompareTo(otherCommand.RenderFlags);
                    }
                }
            }

            return compare;
        }

        internal abstract void RequestRenderCommand(CCRenderer renderer);
    }
}

