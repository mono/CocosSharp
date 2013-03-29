using System.Collections.Generic;

namespace cocos2d
{
    public class CCNodeLoaderLibrary 
    {
        private static CCNodeLoaderLibrary sSharedCCNodeLoaderLibrary;
        
        private Dictionary<string, CCNodeLoader> mCCNodeLoaders = new Dictionary<string, CCNodeLoader>();

        public void RegisterDefaultCCNodeLoaders()
        {
            RegisterCCNodeLoader("CCNode", new CCNodeLoader());
            RegisterCCNodeLoader("CCLayer", new CCLayerLoader());
            RegisterCCNodeLoader("CCLayerColor", new CCLayerColorLoader());
            RegisterCCNodeLoader("CCLayerGradient", new CCLayerGradientLoader());
            RegisterCCNodeLoader("CCSprite", new CCSpriteLoader());
            RegisterCCNodeLoader("CCLabelBMFont", new CCLabelBMFontLoader());
            RegisterCCNodeLoader("CCLabelTTF", new CCLabelTTFLoader());
            RegisterCCNodeLoader("CCScale9Sprite", new CCScale9SpriteLoader());
            RegisterCCNodeLoader("CCScrollView", new CCScrollViewLoader());
            RegisterCCNodeLoader("CCBFile", new CCBFileLoader());
            RegisterCCNodeLoader("CCMenu", new CCMenuLoader());
            RegisterCCNodeLoader("CCMenuItemImage", new CCMenuItemImageLoader());
            RegisterCCNodeLoader("CCControlButton", new CCControlButtonLoader());
            RegisterCCNodeLoader("CCParticleSystemQuad", new CCParticleSystemQuadLoader());
        }

        public void RegisterCCNodeLoader(string pClassName, CCNodeLoader pCCNodeLoader)
        {
            mCCNodeLoaders.Add(pClassName, pCCNodeLoader);
        }

        public void UnregisterCCNodeLoader(string pClassName)
        {
            mCCNodeLoaders.Remove(pClassName);
        }

        public CCNodeLoader GetCCNodeLoader(string pClassName)
        {
            return mCCNodeLoaders[pClassName];
        }

        public void Purge(bool pDelete)
        {
            mCCNodeLoaders.Clear();
        }

        public static CCNodeLoaderLibrary SharedCCNodeLoaderLibrary
        {
            get
            {
                if (sSharedCCNodeLoaderLibrary == null)
                {
                    sSharedCCNodeLoaderLibrary = new CCNodeLoaderLibrary();
                    sSharedCCNodeLoaderLibrary.RegisterDefaultCCNodeLoaders();
                }
                return sSharedCCNodeLoaderLibrary;
            }
        }

        public static void PurgeSharedCCNodeLoaderLibrary()
        {
            sSharedCCNodeLoaderLibrary = null;
        }

        public static CCNodeLoaderLibrary NewDefaultCCNodeLoaderLibrary()
        {
            var ccNodeLoaderLibrary = new CCNodeLoaderLibrary();
            ccNodeLoaderLibrary.RegisterDefaultCCNodeLoaders();
            return ccNodeLoaderLibrary;
        }
    }
}