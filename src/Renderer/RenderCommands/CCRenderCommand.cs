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

        internal uint ArrivalIndex { get; set; }
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

        protected virtual void GenerateFlags(ref long renderFlags)
        {
            renderFlags = 0;
        }

        #endregion Constructors

        public override string ToString()
        {
            return String.Format("[CCRenderCommand: LayerGroup {0} Group {1} GlobalDepth {2} Flags {3}]", 
                LayerGroup, Group, GlobalDepth, RenderFlags);
        }

        // The issue with using a single render id to order commands is the handling of
        // depth which is a floating point number that can't simply be cast as an int value
        // due to the potential of overflow/rounding errors
        // So instead we first perform the layer group, group and depth comparisons individually
        // and then package any remaining traits for comparison (e.g. Material) into the RenderFlags
        public int CompareTo(CCRenderCommand otherCommand)
        {
            int compare = Group.CompareTo(otherCommand.Group);

            if(compare == 0)
            {
                compare = GlobalDepth.CompareTo(otherCommand.GlobalDepth); 

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

