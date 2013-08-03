using System.Collections.Generic;

namespace Cocos2D
{
    public class CCNodeLoaderLibrary 
    {
        private static CCNodeLoaderLibrary _instance;
        
        private Dictionary<string, CCNodeLoader> _nodeLoaders = new Dictionary<string, CCNodeLoader>();

        public CCNodeLoaderLibrary ()
        {
            RegisterDefaultCCNodeLoaders();
        }

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
            _nodeLoaders.Add(pClassName, pCCNodeLoader);
        }

        public void UnregisterCCNodeLoader(string pClassName)
        {
            _nodeLoaders.Remove(pClassName);
        }

        public CCNodeLoader GetCCNodeLoader(string pClassName)
        {
            return _nodeLoaders[pClassName];
        }

        public void Purge(bool pDelete)
        {
            _nodeLoaders.Clear();
        }

        public static CCNodeLoaderLibrary SharedInstance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new CCNodeLoaderLibrary();
                    _instance.RegisterDefaultCCNodeLoaders();
                }
                return _instance;
            }
        }

        public static void PurgeSharedCCNodeLoaderLibrary()
        {
            _instance = null;
        }
    }
}