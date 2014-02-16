using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace CocosSharp
{
    public class CCGrid3DAction : CCGridAction
    {

        #region Constructors

        protected CCGrid3DAction()
        {
        }

        protected CCGrid3DAction(float duration) : base(duration)
        {
        }

        protected CCGrid3DAction(float duration, CCGridSize gridSize) : base(duration, gridSize)
        {
        }

        #endregion Constructors


        protected internal override CCActionState StartAction(CCNode target)
        {
            return new CCGrid3DActionState(this, target);
        }
    }


    #region Action state

    public class CCGrid3DActionState : CCGridActionState
    {
        private CCGrid3D grid3D;

        public override CCGridBase Grid
        {
            get
            {
                CCGridSize gridSize = GridAction.GridSize;

                if (Target != null && !Target.ContentSize.Equals(CCSize.Zero))
                {
                    grid3D = new CCGrid3D(gridSize, Target.ContentSize.PointsToPixels());
                }
                else
                {
                    grid3D = new CCGrid3D(gridSize);
                }

                return grid3D;
            }
            protected set
            {
                Debug.Assert(value is CCGrid3D);
                grid3D = (CCGrid3D)value;
            }
        }

        public CCGrid3DActionState(CCGrid3DAction action, CCNode target) : base(action, target)
        {
        }


        #region Grid Vertex manipulation

        /// <summary>
        /// returns the vertex at a given position
        /// </summary>
        public CCVertex3F Vertex(CCGridSize pos)
        {
            return grid3D.Vertex(pos);
        }

        /// <summary>
        /// returns the vertex at a given position
        /// </summary>
        public CCVertex3F Vertex(int x, int y)
        {
            return grid3D.Vertex(x,y);
        }

        /// <summary>
        /// returns the original (non-transformed) vertex at a given position
        /// </summary>
        public CCVertex3F OriginalVertex(CCGridSize pos)
        {
            return grid3D.OriginalVertex(pos);
        }

        /// <summary>
        /// returns the original (non-transformed) vertex at a given position
        /// </summary>
        public CCVertex3F OriginalVertex(int x, int y)
        {
            return grid3D.OriginalVertex(x,y);
        }

        /// <summary>
        /// sets a new vertex at a given position
        /// </summary>
        public void SetVertex(CCGridSize pos, ref CCVertex3F vertex)
        {
            grid3D.SetVertex(pos, ref vertex);
        }

        /// <summary>
        /// sets a new vertex at a given position
        /// </summary>
        public void SetVertex(int x, int y, ref CCVertex3F vertex)
        {
            grid3D.SetVertex(x,y, ref vertex);
        }

        #endregion Grid Vertex manipulation
    }

    #endregion Action state
}