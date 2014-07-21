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

        CCPoint step;
        CCSize contentSize;

        CCScene scene;


        #region Properties

        public int ReuseGrid { get; set; }                                  // number of times that the grid will be reused 
        public bool Active { get; set; }
        public CCGridSize GridSize { get; private set; }
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

        // pixels between the grids 
        public CCPoint Step 
        { 
            get { return step; }
            private set 
            {
                if (step != value) 
                {
                    step = value;
                    CalculateVertexPoints();
                }
            }
        }

        public CCSize ContentSize
        {
            get { return contentSize; }
            set 
            {
                if (contentSize != value) 
                {
                    contentSize = value;
                    Step = new CCPoint (contentSize.Width / GridSize.X, contentSize.Height / GridSize.Y);
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