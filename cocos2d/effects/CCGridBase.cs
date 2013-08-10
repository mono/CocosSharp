using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cocos2D
{
    /// <summary>
    /// Base class for other
    /// </summary>
    public abstract class CCGridBase 
    {
        protected bool m_bActive;
        protected bool m_bIsTextureFlipped;
        protected CCDirectorProjection m_directorProjection;

        protected int m_nReuseGrid;
        protected CCPoint m_obStep;
        protected CCGrabber m_pGrabber;
        protected CCTexture2D m_pTexture;

        protected CCGridSize m_sGridSize;

        /// <summary>
        ///  wheter or not the grid is active
        /// </summary>
        public bool Active
        {
            get { return m_bActive; }
            set
            {
                m_bActive = value;
                if (!m_bActive)
                {
                    CCDirector director = CCDirector.SharedDirector;
                    director.Projection = director.Projection;
                }
            }
        }

        /// <summary>
        /// number of times that the grid will be reused 
        /// </summary>
        public int ReuseGrid
        {
            get { return m_nReuseGrid; }
            set { m_nReuseGrid = value; }
        }

        /// <summary>
        /// size of the grid 
        /// </summary>
        public CCGridSize GridSize
        {
            get { return m_sGridSize; }
            set { m_sGridSize = value; }
        }

        /// <summary>
        /// pixels between the grids 
        /// </summary>
        public CCPoint Step
        {
            get { return m_obStep; }
            set { m_obStep = value; }
        }

        /// <summary>
        /// is texture flipped 
        /// </summary>
        public bool TextureFlipped
        {
            get { return m_bIsTextureFlipped; }
            set
            {
                if (m_bIsTextureFlipped != value)
                {
                    m_bIsTextureFlipped = value;
                    CalculateVertexPoints();
                }
            }
        }

        protected virtual bool InitWithSize(CCGridSize gridSize, CCTexture2D pTexture, bool bFlipped)
        {
            bool bRet = true;

            m_bActive = false;
            m_nReuseGrid = 0;
            m_sGridSize = gridSize;

            m_pTexture = pTexture;

            m_bIsTextureFlipped = bFlipped;

            CCSize texSize = m_pTexture.ContentSize;
            m_obStep.X = texSize.Width / m_sGridSize.X;
            m_obStep.Y = texSize.Height / m_sGridSize.Y;

            m_pGrabber = new CCGrabber();
            if (m_pGrabber != null)
            {
                m_pGrabber.Grab(m_pTexture);
            }
            else
            {
                bRet = false;
            }

            //m_pShaderProgram = CCShaderCache::sharedShaderCache()->programForKey(kCCShader_PositionTexture);
            CalculateVertexPoints();

            return bRet;
        }

        protected virtual bool InitWithSize(CCGridSize gridSize)
        {
            return InitWithSize(gridSize, CCDirector.SharedDirector.WinSizeInPixels);
        }

        protected virtual bool InitWithSize(CCGridSize gridSize, CCSize size)
        {
            var pTexture = new CCTexture2D();
            // we only use rgba8888
            pTexture.Init((int)size.Width, (int)size.Height, SurfaceFormat.Color, true);

            InitWithSize(gridSize, pTexture, false);

            return true;
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

        public virtual void BeforeDraw()
        {
            m_directorProjection = CCDirector.SharedDirector.Projection;

            m_pGrabber.BeforeRender(m_pTexture);

            Set2DProjection();
        }

        public virtual void AfterDraw(CCNode target)
        {
            m_pGrabber.AfterRender(m_pTexture);

            CCDirector.SharedDirector.Projection = m_directorProjection;

            if (target.Camera.Dirty)
            {
                CCPoint offset = target.AnchorPointInPoints;

                CCDrawManager.Translate(offset.X, offset.Y, 0);
                target.Camera.Locate();
                CCDrawManager.Translate(-offset.X, -offset.Y, 0);
            }

            CCDrawManager.BindTexture(m_pTexture);

            //Blit();

            // restore projection for default FBO .fixed bug #543 #544
            //TODO:         CCDirector::sharedDirector()->setProjection(CCDirector::sharedDirector()->getProjection());
            //TODO:         CCDirector::sharedDirector()->applyOrientation();
        }

        public abstract void Blit();

        public abstract void Reuse();

        public abstract void CalculateVertexPoints();

        public void Set2DProjection()
        {
            CCSize size = m_pTexture.ContentSizeInPixels;

            CCDrawManager.SetViewPort(0, 0, (int)size.Width, (int)size.Height);

            CCDrawManager.ViewMatrix = Matrix.Identity;
            CCDrawManager.ProjectionMatrix = Matrix.Identity;
            
            Matrix projection = Matrix.CreateOrthographicOffCenter(0, size.Width, 0, size.Height, -1024.0f, 1024.0f);
            Matrix halfPixelOffset = Matrix.CreateTranslation(-0.5f, -0.5f, 0);
            CCDrawManager.WorldMatrix = (halfPixelOffset * projection);
        }
    }
}