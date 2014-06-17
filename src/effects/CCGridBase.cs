using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CocosSharp
{
    /// <summary>
    /// Base class for other
    /// </summary>
    public abstract class CCGridBase 
    {
        bool active;
        bool textureFlipped;

        CCDirector director;


        #region Properties

        public int ReuseGrid { get; set; }                                  // number of times that the grid will be reused 
        public CCGridSize GridSize { get; private set; }
        public CCPoint Step { get; private set; }                           // pixels between the grids 

        protected CCDirectorProjection DirectorProjection { get; set; }
        protected CCGrabber Grabber { get; set; }
        protected CCTexture2D Texture { get; set; }

        internal CCDirector Director 
        { 
            get { return director; }
            set 
            {
                director = value;

                if(director != null && Texture != null) 
                {
                    CCSize texSize = Texture.ContentSize(director.ContentScaleFactor);
                    Step = new CCPoint(texSize.Width / GridSize.X, texSize.Height / GridSize.Y);

                    CalculateVertexPoints();
                }
            }
        }


        public bool Active
        {
            get { return active; }
            set
            {
                active = value;
                if (!active && Director != null)
                {
                    Director.Projection = Director.Projection;
                }
            }
        }

        public bool TextureFlipped
        {
            get { return textureFlipped; }
            set
            {
                if (textureFlipped != value)
                {
                    textureFlipped = value;
                    CalculateVertexPoints();
                }
            }
        }

        #endregion Properties


        #region Constructors

        protected CCGridBase(CCGridSize gridSize, CCTexture2D texture, bool flipped=false)
        {
            GridSize = gridSize;
            Texture = texture;
            textureFlipped = flipped;

            Grabber = new CCGrabber();
            if (Grabber != null)
            {
                Grabber.Grab(Texture);
            }
        }

        protected CCGridBase(CCGridSize gridSize, CCSize size) 
            : this(gridSize, new CCTexture2D((int)size.Width, (int)size.Height, CCSurfaceFormat.Color, true, false))
        {
        }

        #endregion Constructors


        public abstract void Blit();
        public abstract void Reuse();
        public abstract void CalculateVertexPoints();

        public virtual void BeforeDraw()
        {
            DirectorProjection = Director.Projection;

            Grabber.BeforeRender(Texture);

            Set2DProjection();
        }

        public virtual void AfterDraw(CCNode target)
        {
            Grabber.AfterRender(Texture);

            Director.Projection = DirectorProjection;

            if (target.Camera.IsDirty)
            {
                CCPoint offset = target.AnchorPointInPoints;

                CCDrawManager.Translate(offset.X, offset.Y, 0);
                target.Camera.Locate();
                CCDrawManager.Translate(-offset.X, -offset.Y, 0);
            }

            CCDrawManager.BindTexture(Texture);

            //Blit();
        }

        public ulong NextPOT(ulong x)
        {
            x = x - 1;
            x = x | (x >> 1);
            x = x | (x >> 2);
            x = x | (x >> 4);
            x = x | (x >> 8);
            x = x | (x >> 16);
            return x + 1;
        }

        public void Set2DProjection()
        {
            CCSize size = Texture.ContentSizeInPixels;

            CCDrawManager.SetViewPort(0, 0, (int)size.Width, (int)size.Height);

            CCDrawManager.ViewMatrix = Matrix.Identity;
            CCDrawManager.ProjectionMatrix = Matrix.Identity;

            Matrix projection = Matrix.CreateOrthographicOffCenter(0, size.Width, 0, size.Height, -1024.0f, 1024.0f);
            Matrix halfPixelOffset = Matrix.CreateTranslation(-0.5f, -0.5f, 0);
            CCDrawManager.WorldMatrix = (halfPixelOffset * projection);
        }
    }
}