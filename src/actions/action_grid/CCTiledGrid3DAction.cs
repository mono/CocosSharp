using System.Diagnostics;

namespace CocosSharp
{
    public class CCTiledGrid3DAction : CCGridAction
    {
        #region Constructors

        public CCTiledGrid3DAction (float duration) : base (duration)
        {
        }

        public CCTiledGrid3DAction (float duration, CCGridSize gridSize) : this (duration, gridSize, 0)
        {
        }

        protected CCTiledGrid3DAction (float duration, CCGridSize gridSize, float amplitude)
            : base (duration, gridSize, amplitude)
        {
        }

        #endregion Constructors


        protected internal override CCActionState StartAction(CCNode target)
        {
            return new CCTiledGrid3DActionState (this, target);
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
                CCSize gridTextureSizeInPixels = Target.Scene.VisibleBoundsScreenspace.Size;
                CCTexture2D gridTexture = new CCTexture2D( 
                    (int)gridTextureSizeInPixels.Width, (int)gridTextureSizeInPixels.Height, CCSurfaceFormat.Color, true, false);
                tiledGrid3D = new CCTiledGrid3D (GridSize, gridTexture);
                tiledGrid3D.Scene = Target.Scene;
                return tiledGrid3D;
            }
            protected set 
            {
                Debug.Assert (value is CCTiledGrid3D);
                tiledGrid3D = (CCTiledGrid3D)value;
            }
        }

        public CCTiledGrid3DActionState (CCTiledGrid3DAction action, CCNode target) : base (action, target)
        {
        }


        #region Grid Vertex manipulation

        /// <summary>
        /// returns the tile quad at a given position
        /// </summary>
        public CCQuad3 Tile (CCGridSize pos)
        {
            return tiledGrid3D [pos];
        }

        /// <summary>
        /// returns the tile quad at a given position
        /// </summary>
        public CCQuad3 Tile (int x, int y)
        {
            return tiledGrid3D [x, y];
        }

        /// <summary>
        /// returns the original (non-transformed) tile quad at a given position
        /// </summary>
        public CCQuad3 OriginalTile (CCGridSize pos)
        {
            return tiledGrid3D.OriginalTile (pos);
        }

        /// <summary>
        /// returns the original (non-transformed) tile quad at a given position
        /// </summary>
        public CCQuad3 OriginalTile (int x, int y)
        {
            return tiledGrid3D.OriginalTile (x, y);
        }

        /// <summary>
        /// sets a new tile quad at a given position
        /// </summary>
        public void SetTile (CCGridSize pos, ref CCQuad3 coords)
        {
            tiledGrid3D [pos] = coords;
        }

        /// <summary>
        /// sets a new tile quad at a given position
        /// </summary>
        public void SetTile (int x, int y, ref CCQuad3 coords)
        {
            tiledGrid3D [x, y] = coords;
        }

        #endregion Grid Vertex manipulation
    }

    #endregion Action state
}