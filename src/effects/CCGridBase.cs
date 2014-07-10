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

        CCScene scene;


        #region Properties

        public int ReuseGrid { get; set; }                                  // number of times that the grid will be reused 
        public bool Active { get; set; }
        public CCGridSize GridSize { get; private set; }
        public CCPoint Step { get; private set; }                           // pixels between the grids 

        protected CCGrabber Grabber { get; set; }
        protected CCTexture2D Texture { get; set; }

        internal CCScene Scene 
        { 
            get { return scene; }
            set 
            {
                if(scene != value) 
                {
                    scene = value;

                    if (scene != null && Texture != null) 
                    {
                        CCSize texSize = Texture.ContentSizeInPixels;
                        Step = new CCPoint(texSize.Width / GridSize.X, texSize.Height / GridSize.Y);

                        Grabber = new CCGrabber(scene.Window.DrawManager);
                        if (Grabber != null && Texture != null)
                        {
                            Grabber.Grab(Texture);
                        }

                        CalculateVertexPoints();
                    }
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

        }

        #endregion Constructors


        public abstract void Blit();
        public abstract void Reuse();
        public abstract void CalculateVertexPoints();

        public virtual void BeforeDraw()
        {
            Grabber.BeforeRender(Texture);
        }

        public virtual void AfterDraw(CCNode target)
        {
            Grabber.AfterRender(Texture);

            Scene.Window.DrawManager.BindTexture(Texture);
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
    }
}