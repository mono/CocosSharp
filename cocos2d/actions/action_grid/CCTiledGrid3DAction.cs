using System.Diagnostics;

namespace CocosSharp
{
    public class CCTiledGrid3DAction : CCGridAction
    {
        #region Constructors

        public CCTiledGrid3DAction()
        {
        }

        public CCTiledGrid3DAction(float duration) : base(duration)
        {
        }

        public CCTiledGrid3DAction(float duration, CCGridSize gridSize) : base(duration, gridSize)
        {
        }

        #endregion Constructors


        protected internal override CCActionState StartAction(CCNode target)
        {
            return new CCTiledGrid3DActionState(this, target);
        }
    }


    #region Action state

    public class CCTiledGrid3DActionState : CCGridActionState
    {
        private CCTiledGrid3D tiledGrid3D;

        public override CCGridBase Grid
        {
            get
            {
                CCGridSize gridSize = GridAction.GridSize;
                tiledGrid3D = new CCTiledGrid3D(gridSize);
                return tiledGrid3D;
            }
            protected set
            {
                Debug.Assert(value is CCTiledGrid3D);
                tiledGrid3D = (CCTiledGrid3D)value;
            }
        }

        public CCTiledGrid3DActionState(CCTiledGrid3DAction action, CCNode target) : base(action, target)
        {
        }


        #region Grid Vertex manipulation

        /// <summary>
        /// returns the tile quad at a given position
        /// </summary>
        public CCQuad3 Tile(CCGridSize pos)
        {
            return tiledGrid3D.Tile(pos);
        }

        /// <summary>
        /// returns the tile quad at a given position
        /// </summary>
        public CCQuad3 Tile(int x, int y)
        {
            return tiledGrid3D.Tile(x, y);
        }

        /// <summary>
        /// returns the original (non-transformed) tile quad at a given position
        /// </summary>
        public CCQuad3 OriginalTile(CCGridSize pos)
        {
            return tiledGrid3D.OriginalTile(pos);
        }

        /// <summary>
        /// returns the original (non-transformed) tile quad at a given position
        /// </summary>
        public CCQuad3 OriginalTile(int x, int y)
        {
            return tiledGrid3D.OriginalTile(x, y);
        }

        /// <summary>
        /// sets a new tile quad at a given position
        /// </summary>
        public void SetTile(CCGridSize pos, ref CCQuad3 coords)
        {
            tiledGrid3D.SetTile(pos, ref coords);
        }

        /// <summary>
        /// sets a new tile quad at a given position
        /// </summary>
        public void SetTile(int x, int y, ref CCQuad3 coords)
        {
            tiledGrid3D.SetTile(x, y, ref coords);
        }

        #endregion Grid Vertex manipulation
    }

    #endregion Action state
}