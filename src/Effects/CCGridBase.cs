using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace CocosSharp
{
    /// <summary>
    /// Base class for other
    /// </summary>
    public abstract class CCGridBase 
    {
        bool textureFlipped;
        CCPoint step;
        CCScene scene;

        #region Properties

        public int ReuseGrid { get; set; }                                  // number of times that the grid will be reused 
        public bool Active { get; set; }
        public CCGridSize GridSize { get; private set; }
        protected CCRenderTexture RenderTexture { get; private set; }

        internal CCLayer Layer { get; set; }

        internal CCScene Scene 
        { 
            get { return scene; }
            set 
            {
                if(scene != value) 
                {
                    scene = value;

                    if (scene != null && RenderTexture != null) 
                        CalculateVertexPoints();
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

        #endregion Properties


        #region Constructors

        protected CCGridBase(CCGridSize gridSize, CCRenderTexture renderTexture, bool flipped=false)
        {
            GridSize = gridSize;
            RenderTexture = renderTexture;
            textureFlipped = flipped;
            CCSize texSize = renderTexture.Texture.ContentSizeInPixels;
            Step = new CCPoint ((float)Math.Ceiling(texSize.Width / GridSize.X), (float)Math.Ceiling(texSize.Height / GridSize.Y));
        }

        #endregion Constructors

        #region Cleaning up

        ~CCGridBase ()
        {
            this.Dispose(false);
        }

        public void Dispose()
        {
            this.Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if(disposing) 
            {
                if(RenderTexture != null)
                    RenderTexture.Dispose();
            }
        }

        #endregion


        public abstract void Reuse();
        public abstract void CalculateVertexPoints();

        public virtual void Blit()
        {
            if(RenderTexture != null) 
            {
                CCDrawManager drawManager = Scene.GameView.DrawManager;
                drawManager.BindTexture(RenderTexture.Texture);
            }
        }

        public virtual void BeforeDraw()
        {
            if(RenderTexture != null)
                RenderTexture.BeginWithClear(CCColor4B.Transparent);
        }

        public virtual void AfterDraw(CCNode target)
        {
            if(RenderTexture != null)
                RenderTexture.End();
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