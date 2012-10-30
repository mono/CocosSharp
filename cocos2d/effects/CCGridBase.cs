using Microsoft.Xna.Framework;

namespace cocos2d
{
    /// <summary>
    /// Base class for other
    /// </summary>
    public abstract class CCGridBase : CCObject
    {
        protected bool m_bActive;
        protected bool m_bIsTextureFlipped;
        protected ccDirectorProjection m_directorProjection;

        protected int m_nReuseGrid;
        protected CCPoint m_obStep;
        protected CCGrabber m_pGrabber;
        protected CCTexture2D m_pTexture;

        protected ccGridSize m_sGridSize;

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
        public ccGridSize GridSize
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

        public bool InitWithSize(ccGridSize gridSize, CCTexture2D pTexture, bool bFlipped)
        {
            bool bRet = true;

            m_bActive = false;
            m_nReuseGrid = 0;
            m_sGridSize = gridSize;

            m_pTexture = pTexture;

            m_bIsTextureFlipped = bFlipped;

            CCSize texSize = m_pTexture.ContentSize;
            m_obStep.x = texSize.width / m_sGridSize.x;
            m_obStep.y = texSize.height / m_sGridSize.y;

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

        public bool InitWithSize(ccGridSize gridSize)
        {
            return InitWithSize(gridSize, CCDirector.SharedDirector.WinSizeInPixels);
        }

        public bool InitWithSize(ccGridSize gridSize, CCSize size)
        {
            //ulong POTWide = ccNextPOT((uint) size.width);
            //ulong POTHigh = ccNextPOT((uint) size.width);
            ulong potWide = (uint) size.width;
            ulong potHigh = (uint) size.height;

            // we only use rgba8888
            var format = CCTexture2DPixelFormat.kCCTexture2DPixelFormat_RGBA8888;

            var pTexture = new CCTexture2D();
            pTexture.InitWithData(null, format, (uint) potWide, (uint) potHigh, size);

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

        public void BeforeDraw()
        {
            m_directorProjection = CCDirector.SharedDirector.Projection;

            m_pGrabber.BeforeRender(m_pTexture);

            Set2DProjection();
        }

        public void AfterDraw(CCNode target)
        {
            m_pGrabber.AfterRender(m_pTexture);

            CCDirector.SharedDirector.Projection = m_directorProjection;

            if (target.Camera.Dirty)
            {
                CCPoint offset = target.AnchorPointInPoints;

                DrawManager.Translate(offset.x, offset.y, 0);
                target.Camera.Locate();
                DrawManager.Translate(-offset.x, -offset.y, 0);
            }

            DrawManager.BindTexture(m_pTexture);

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

            DrawManager.SetViewPort(0, 0,
                                    (int) (size.width * ccMacros.CC_CONTENT_SCALE_FACTOR()),
                                    (int) (size.height * ccMacros.CC_CONTENT_SCALE_FACTOR())
                );

            /*
            DrawManager.ProjectionMatrix = Matrix.Identity;
            
            Matrix orthoMatrix = Matrix.CreateOrthographicOffCenter(
                0, size.width * ccMacros.CC_CONTENT_SCALE_FACTOR(),
                0, size.height * ccMacros.CC_CONTENT_SCALE_FACTOR(),
                -1, 1
                );

            DrawManager.MultMatrix(ref orthoMatrix);
            */
            
            DrawManager.ViewMatrix = Matrix.Identity;
            DrawManager.ProjectionMatrix = Matrix.Identity;
            
            Matrix projection = Matrix.CreateOrthographicOffCenter(0, size.width, 0, size.height, -1024.0f, 1024.0f);
            Matrix halfPixelOffset = Matrix.CreateTranslation(-0.5f, -0.5f, 0);
            DrawManager.WorldMatrix = (halfPixelOffset * projection);
            

            /*
            CCDirector *director = CCDirector::sharedDirector();

            CCSize    size = director->getWinSizeInPixels();

            glViewport(0, 0, (GLsizei)(size.width * CC_CONTENT_SCALE_FACTOR()), (GLsizei)(size.height * CC_CONTENT_SCALE_FACTOR()) );
            kmGLMatrixMode(KM_GL_PROJECTION);
            kmGLLoadIdentity();

            kmMat4 orthoMatrix;
            kmMat4OrthographicProjection(&orthoMatrix, 0, size.width * CC_CONTENT_SCALE_FACTOR(), 0, size.height * CC_CONTENT_SCALE_FACTOR(), -1, 1);
            kmGLMultMatrix( &orthoMatrix );

            kmGLMatrixMode(KM_GL_MODELVIEW);
            kmGLLoadIdentity();


            ccSetProjectionMatrixDirty();
            */
        }
    }
}