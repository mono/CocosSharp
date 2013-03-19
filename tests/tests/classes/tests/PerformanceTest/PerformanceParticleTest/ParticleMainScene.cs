using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;
using System.Diagnostics;

namespace tests
{
    public class ParticleMainScene : CCScene
    {
        public virtual void initWithSubTest(int asubtest, int particles)
        {
            //srandom(0);

            subtestNumber = asubtest;
            CCSize s = CCDirector.SharedDirector.WinSize;

            lastRenderedCount = 0;
            quantityParticles = particles;

            CCMenuItemFont.FontSize = 64;
            CCMenuItemFont decrease = CCMenuItemFont.Create(" - ", onDecrease);
            decrease.Color = new CCColor3B(0, 200, 20);
            CCMenuItemFont increase = CCMenuItemFont.Create(" + ", onIncrease);
            increase.Color = new CCColor3B(0, 200, 20);

            CCMenu menu = CCMenu.Create(decrease, increase);
            menu.AlignItemsHorizontally();
            menu.Position = new CCPoint(s.Width / 2, s.Height / 2 + 15);
            AddChild(menu, 1);

            CCLabelTTF infoLabel = CCLabelTTF.Create("0 nodes", "Marker Felt", 30);
            infoLabel.Color = new CCColor3B(0, 200, 20);
            infoLabel.Position = new CCPoint(s.Width / 2, s.Height - 90);
            AddChild(infoLabel, 1, PerformanceParticleTest.kTagInfoLayer);

            // particles on stage
            CCLabelAtlas labelAtlas = CCLabelAtlas.Create("0000", "Images/fps_Images", 16, 24, '.');
            AddChild(labelAtlas, 0, PerformanceParticleTest.kTagLabelAtlas);
            labelAtlas.Position = new CCPoint(s.Width - 66, 50);

            // Next Prev Test
            ParticleMenuLayer pMenu = new ParticleMenuLayer(true, PerformanceParticleTest.TEST_COUNT, PerformanceParticleTest.s_nParCurIdx);
            AddChild(pMenu, 1, PerformanceParticleTest.kTagMenuLayer);

            // Sub Tests
            CCMenuItemFont.FontSize = 38;
            CCMenu pSubMenu = CCMenu.Create(null);
            for (int i = 1; i <= 6; ++i)
            {
                //char str[10] = {0};
                string str;
                //sprintf(str, "%d ", i);
                str = string.Format("{0:G}", i);
                CCMenuItemFont itemFont = CCMenuItemFont.Create(str, testNCallback);
                itemFont.Tag = i;
                pSubMenu.AddChild(itemFont, 10);

                if (i <= 3)
                {
                    itemFont.Color = new CCColor3B(200, 20, 20);
                }
                else
                {
                    itemFont.Color = new CCColor3B(0, 200, 20);
                }
            }
            pSubMenu.AlignItemsHorizontally();
            pSubMenu.Position = new CCPoint(s.Width / 2, 80);
            AddChild(pSubMenu, 2);

            CCLabelTTF label = CCLabelTTF.Create(title(), "arial", 38);
            AddChild(label, 1);
            label.Position = new CCPoint(s.Width / 2, s.Height - 32);
            label.Color = new CCColor3B(255, 255, 40);

            updateQuantityLabel();
            createParticleSystem();

            Schedule(step);
        }

        public virtual string title()
        {
            return "No title";
        }

        public void step(float dt)
        {
            CCLabelAtlas atlas = (CCLabelAtlas)GetChildByTag(PerformanceParticleTest.kTagLabelAtlas);
            CCParticleSystem emitter = (CCParticleSystem)GetChildByTag(PerformanceParticleTest.kTagParticleSystem);

            var str = string.Format("{0:0000}", emitter.ParticleCount);
            atlas.Label = (str);
        }

        public void createParticleSystem()
        {
            CCParticleSystem particleSystem = null;

            /*
            * Tests:
            * 1: Point Particle System using 32-bit textures (PNG)
            * 2: Point Particle System using 16-bit textures (PNG)
            * 3: Point Particle System using 8-bit textures (PNG)
            * 4: Point Particle System using 4-bit textures (PVRTC)

            * 5: Quad Particle System using 32-bit textures (PNG)
            * 6: Quad Particle System using 16-bit textures (PNG)
            * 7: Quad Particle System using 8-bit textures (PNG)
            * 8: Quad Particle System using 4-bit textures (PVRTC)
            */
            RemoveChildByTag(PerformanceParticleTest.kTagParticleSystem, true);

            // remove the "fire.png" from the TextureCache cache. 
            CCTexture2D texture = CCTextureCache.SharedTextureCache.AddImage("Images/fire");
            CCTextureCache.SharedTextureCache.RemoveTexture(texture);

            particleSystem = new CCParticleSystemQuad();

            switch (subtestNumber)
            {
                case 1:
                    CCTexture2D.DefaultAlphaPixelFormat = CCTexture2DPixelFormat.kCCTexture2DPixelFormat_RGBA8888;
                    particleSystem.InitWithTotalParticles(quantityParticles);
                    particleSystem.Texture = CCTextureCache.SharedTextureCache.AddImage("Images/fire");
                    break;
                case 2:
                    CCTexture2D.DefaultAlphaPixelFormat = CCTexture2DPixelFormat.kCCTexture2DPixelFormat_RGBA4444;
                    particleSystem.InitWithTotalParticles(quantityParticles);
                    particleSystem.Texture = CCTextureCache.SharedTextureCache.AddImage("Images/fire");
                    break;
                case 3:
                    CCTexture2D.DefaultAlphaPixelFormat = CCTexture2DPixelFormat.kCCTexture2DPixelFormat_A8;
                    particleSystem.InitWithTotalParticles(quantityParticles);
                    particleSystem.Texture = CCTextureCache.SharedTextureCache.AddImage("Images/fire");
                    break;
                //     case 4:
                //         particleSystem->initWithTotalParticles(quantityParticles);
                //         ////---- particleSystem.texture = [[CCTextureCache sharedTextureCache] addImage:@"fire.pvr"];
                //         particleSystem->setTexture(CCTextureCache::sharedTextureCache()->addImage("Images/fire.png"));
                //         break;
                case 4:
                    CCTexture2D.DefaultAlphaPixelFormat = CCTexture2DPixelFormat.kCCTexture2DPixelFormat_RGBA8888;
                    particleSystem.InitWithTotalParticles(quantityParticles);
                    particleSystem.Texture = CCTextureCache.SharedTextureCache.AddImage("Images/fire");
                    break;
                case 5:
                    CCTexture2D.DefaultAlphaPixelFormat = CCTexture2DPixelFormat.kCCTexture2DPixelFormat_RGBA4444;
                    particleSystem.InitWithTotalParticles(quantityParticles);
                    particleSystem.Texture = CCTextureCache.SharedTextureCache.AddImage("Images/fire");
                    break;
                case 6:
                    CCTexture2D.DefaultAlphaPixelFormat = CCTexture2DPixelFormat.kCCTexture2DPixelFormat_A8;
                    particleSystem.InitWithTotalParticles(quantityParticles);
                    particleSystem.Texture = CCTextureCache.SharedTextureCache.AddImage("Images/fire");
                    break;
                //     case 8:
                //         particleSystem->initWithTotalParticles(quantityParticles);
                //         ////---- particleSystem.texture = [[CCTextureCache sharedTextureCache] addImage:@"fire.pvr"];
                //         particleSystem->setTexture(CCTextureCache::sharedTextureCache()->addImage("Images/fire.png"));
                //         break;
                default:
                    particleSystem = null;
                    CCLog.Log("Shall not happen!");
                    break;
            }
            AddChild(particleSystem, 0, PerformanceParticleTest.kTagParticleSystem);

            doTest();

            // restore the default pixel format
            CCTexture2D.DefaultAlphaPixelFormat = CCTexture2DPixelFormat.kCCTexture2DPixelFormat_RGBA8888;
        }

        public void onDecrease(CCObject pSender)
        {
            quantityParticles -= PerformanceParticleTest.kNodesIncrease;
            if (quantityParticles < 0)
                quantityParticles = 0;

            updateQuantityLabel();
            createParticleSystem();
        }

        public void onIncrease(CCObject pSender)
        {
            quantityParticles += PerformanceParticleTest.kNodesIncrease;
            if (quantityParticles > PerformanceParticleTest.kMaxParticles)
                quantityParticles = PerformanceParticleTest.kMaxParticles;

            updateQuantityLabel();
            createParticleSystem();
        }

        public void testNCallback(CCObject pSender)
        {
            subtestNumber = ((CCNode)pSender).Tag;

            ParticleMenuLayer pMenu = (ParticleMenuLayer)GetChildByTag(PerformanceParticleTest.kTagMenuLayer);
            pMenu.restartCallback(pSender);
        }

        public void updateQuantityLabel()
        {
            if (quantityParticles != lastRenderedCount)
            {
                CCLabelTTF infoLabel = (CCLabelTTF)GetChildByTag(PerformanceParticleTest.kTagInfoLayer);
                string str = string.Format("{0} particles", quantityParticles);
                infoLabel.Label = (str);

                lastRenderedCount = quantityParticles;
            }
        }

        public int getSubTestNum()
        { 
            return subtestNumber;
        }

        public int getParticlesNum()
        { 
            return quantityParticles;
        }

        public virtual void doTest()
        {
            throw new NotFiniteNumberException();
        }


        protected int lastRenderedCount;
        protected int quantityParticles;
        protected int subtestNumber;
    }
}
