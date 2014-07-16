using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using CocosSharp;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#if OPENGL
#if MACOS
using MonoMac.OpenGL;
#elif WINDOWS || LINUX
using OpenTK.Graphics.OpenGL;
#elif GLES
using OpenTK.Graphics.ES20;
#endif
#endif

namespace tests
{
    public enum enumTag
    {
        kTagLabel = 1,
        kTagSprite1 = 2,
        kTagSprite2 = 3,
    };

    public class TextureTestScene : TestScene
    {

		private static int TEST_CASE_COUNT = 7;

        private static int sceneIdx = -1;

        public static CCLayer createTextureTest(int index)
        {
            CCLayer pLayer = null;

            switch (index)
            {
                case 0:
                    pLayer = new TextureCache1();
                    break;
                case 1:
                    pLayer = new TextureSizeTest();
                    break;
                case 2:
                    pLayer = new TextureGLRepeat();
                    break;
                case 3:
                    pLayer = new TextureGLClamp();
                    break;
                case 4:
                    pLayer = new TextureGLMirror();
                    break;
                case 5:
                    pLayer = new TextureAsync();
                    break;
				case 6:
					pLayer = new TextureAlias(); 
					break;
                    //case 1:
                    //    pLayer = new TextureMipMap(); break;
                    //case 2:
                    //    pLayer = new TexturePVRMipMap(); break;
                    //case 3:
                    //    pLayer = new TexturePVRMipMap2(); break;
                    //case 4:
                    //    pLayer = new TexturePVRNonSquare(); break;
                    //case 5:
                    //    pLayer = new TexturePVRNPOT4444(); break;
                    //case 6:
                    //    pLayer = new TexturePVRNPOT8888(); break;
                    //case 7:
                    //    pLayer = new TexturePVR2BPP(); break;
                    //case 8:
                    //    pLayer = new TexturePVRRaw(); break;
                    //case 9:
                    //    pLayer = new TexturePVR(); break;
                    //case 10:
                    //    pLayer = new TexturePVR4BPP(); break;
                    //case 11:
                    //    pLayer = new TexturePVRRGBA8888(); break;
                    //case 12:
                    //    pLayer = new TexturePVRBGRA8888(); break;
                    //case 13:
                    //    pLayer = new TexturePVRRGBA4444(); break;
                    //case 14:
                    //    pLayer = new TexturePVRRGBA4444GZ(); break;
                    //case 15:
                    //    pLayer = new TexturePVRRGBA4444CCZ(); break;
                    //case 16:
                    //    pLayer = new TexturePVRRGBA5551(); break;
                    //case 17:
                    //    pLayer = new TexturePVRRGB565(); break;
                    //case 18:
                    //    pLayer = new TexturePVRA8(); break;
                    //case 19:
                    //    pLayer = new TexturePVRI8(); break;
                    //case 20:
                    //    pLayer = new TexturePVRAI88(); break;
                    //case 21:
                    //    pLayer = new TexturePVRBadEncoding(); break;
                    //case 22:
                    //    pLayer = new TexturePNG(); break;
                    //case 23:
                    //    pLayer = new TextureJPEG(); break;
                    //case 24:
                    //    pLayer = new TexturePixelFormat(); break;
                    //case 25:
                    //    pLayer = new TextureBlend(); break;
                    //case 26:
                    //    pLayer = new TextureGlClamp(); break;
                    //case 27:
                    //    pLayer = new TextureGlRepeat(); break;
                    //case 28:
                    //    pLayer = new TextureSizeTest(); break;
                    //case 29:
                    //    pLayer = new TextureCache1(); break;
                default:
                    break;
            }

            return pLayer;
        }

        protected override void NextTestCase()
        {
            nextTextureTest();
        }

        protected override void PreviousTestCase()
        {
            backTextureTest();
        }

        protected override void RestTestCase()
        {
            restartTextureTest();
        }

        public static CCLayer nextTextureTest()
        {
            sceneIdx++;
            sceneIdx = sceneIdx % TEST_CASE_COUNT;

            return createTextureTest(sceneIdx);
        }

        public static CCLayer backTextureTest()
        {
            sceneIdx--;
            if (sceneIdx < 0)
                sceneIdx = TEST_CASE_COUNT - 1;

            return createTextureTest(sceneIdx);
        }

        public static CCLayer restartTextureTest()
        {
            return createTextureTest(sceneIdx);
        }

        public override void runThisTest()
        {
            CCLayer pLayer = nextTextureTest();
            AddChild(pLayer);
            Scene.Director.ReplaceScene(this);
        }

    }


    ////------------------------------------------------------------------
    ////
    //// TextureDemo
    ////
    ////------------------------------------------------------------------
    public class TextureDemo : CCLayer
    {
        public override void OnEnter()
        {
            base.OnEnter();

            CCTextureCache.SharedTextureCache.DumpCachedTextureInfo();
            CCSize s = Layer.VisibleBoundsWorldspace.Size;
            CCLabelTtf label = new CCLabelTtf(title(), "arial", 26);
            AddChild(label, 1, (int) (enumTag.kTagLabel));
            label.Position = new CCPoint(s.Width / 2, s.Height - 50);

            string strSubtitle = subtitle();
            if (strSubtitle.Length > 0)
            {
                CCLabelTtf l = new CCLabelTtf(strSubtitle, "arial", 16);
                AddChild(l, 1);
                l.Position = new CCPoint(s.Width / 2, s.Height - 80);
            }

            CCMenuItemImage item1 = new CCMenuItemImage(TestResource.s_pPathB1, TestResource.s_pPathB2, (backCallback));
            CCMenuItemImage item2 = new CCMenuItemImage(TestResource.s_pPathR1, TestResource.s_pPathR2,
                (restartCallback));
            CCMenuItemImage item3 = new CCMenuItemImage(TestResource.s_pPathF1, TestResource.s_pPathF2, (nextCallback));

            CCMenu menu = new CCMenu(item1, item2, item3);
            menu.Position = new CCPoint(0, 0);
            item1.Position = new CCPoint(s.Width / 2 - 100, 30);
            item2.Position = new CCPoint(s.Width / 2, 30);
            item3.Position = new CCPoint(s.Width / 2 + 100, 30);
            AddChild(menu, 1);
            CCTextureCache.SharedTextureCache.DumpCachedTextureInfo();
        }

        ~TextureDemo()
        {
            CCTextureCache.SharedTextureCache.RemoveUnusedTextures();

            CCTextureCache.SharedTextureCache.DumpCachedTextureInfo();
        }

        public void restartCallback(object pSender)
        {
            CCScene s = new TextureTestScene();
            s.AddChild(TextureTestScene.restartTextureTest());
            Scene.Director.ReplaceScene(s);
        }

        public void nextCallback(object pSender)
        {
            CCScene s = new TextureTestScene();
            s.AddChild(TextureTestScene.nextTextureTest());
            Scene.Director.ReplaceScene(s);
        }

        public void backCallback(object pSender)
        {
            CCScene s = new TextureTestScene();
            s.AddChild(TextureTestScene.backTextureTest());
            Scene.Director.ReplaceScene(s);
        }

        public virtual string title()
        {
            return "No title";
        }

        public virtual string subtitle()
        {
            return "";
        }

    }


    //------------------------------------------------------------------
    //
    // TextureSizeTest
    //
    //------------------------------------------------------------------
    public class TextureSizeTest : TextureDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();

            CCSize size = Layer.VisibleBoundsWorldspace.Size;

            CCLog.Log("Loading 512x512 image...");
            CCSprite sprite1 = new CCSprite("Images/texture512x512");
            if (sprite1 != null)
            {
                CCLog.Log("OK\n");
                sprite1.Position = new CCPoint(size.Width - 50, size.Height - 50);
                this.AddChild(sprite1);
            }
            else
                CCLog.Log("Error\n");

            CCLog.Log("Loading 1024x1024 image...");
            CCSprite sprite2 = new CCSprite("Images/texture1024x1024");
            if (sprite2 != null)
            {
                CCLog.Log("OK\n");
                this.AddChild(sprite2);
            }
            else
                CCLog.Log("Error\n");
            CCLog.Log("Loading 2048x2048 image...");
            CCSprite sprite3 = new CCSprite("Images/texture2048x2048");
            if (sprite3 != null)
            {
                CCLog.Log("OK\n");
                this.AddChild(sprite3);
            }
            else
                CCLog.Log("Error\n");
            // 	@todo
            // This won't work in XNA4 - max is 2048 x 2048.
            // 	CCLog("Loading 4096x4096 image...");
            // 	sprite = CCSprite::create("Images/texture4096x4096.png");
            // 	if( sprite )
            // 		CCLog("OK\n");
            // 	else
            // 		CCLog("Error\n");
        }

        public override string title()
        {
            return "Different Texture Sizes";
        }

        public override string subtitle()
        {
            return "512x512, 1024x1024. See the console.";
        }
    }

    ////------------------------------------------------------------------
    ////
    //// TexturePNG
    ////
    ////------------------------------------------------------------------
    //void TexturePNG::onEnter()
    //{
    //    TextureDemo::onEnter();	

    //    CCSize s = CCDirector::sharedDirector()->getWinSize();

    //    CCSprite *img = CCSprite::create("Images/test_image.png");
    //    img->setPosition(ccp( s.width/2.0f, s.height/2.0f));
    //    addChild(img);
    //    CCTextureCache::sharedTextureCache()->dumpCachedTextureInfo();
    //}

    //std::string TexturePNG::title()
    //{
    //    return "PNG Test";
    //}

    ////------------------------------------------------------------------
    ////
    //// TextureJPEG
    ////
    ////------------------------------------------------------------------
    //void TextureJPEG::onEnter()
    //{
    //    TextureDemo::onEnter();
    //    CCSize s = CCDirector::sharedDirector()->getWinSize();

    //    CCSprite *img = CCSprite::create("Images/test_image.jpeg");
    //    img->setPosition(ccp( s.width/2.0f, s.height/2.0f));
    //    addChild(img);
    //    CCTextureCache::sharedTextureCache()->dumpCachedTextureInfo();
    //}

    //std::string TextureJPEG::title()
    //{
    //    return "JPEG Test";
    //}

    ////------------------------------------------------------------------
    ////
    //// TextureMipMap
    ////
    ////------------------------------------------------------------------
    //void TextureMipMap::onEnter()
    //{
    //    TextureDemo::onEnter();
    //    CCSize s = CCDirector::sharedDirector()->getWinSize();

    //    CCTexture2D *texture0 = CCTextureCache::sharedTextureCache()->addImage("Images/grossini_dance_atlas.png");
    //    texture0->generateMipmap();
    //    ccTexParams texParams = { GL_LINEAR_MIPMAP_LINEAR, GL_LINEAR, GL_CLAMP_TO_EDGE, GL_CLAMP_TO_EDGE };	
    //    texture0->setTexParameters(&texParams);

    //    CCTexture2D *texture1 = CCTextureCache::sharedTextureCache()->addImage("Images/grossini_dance_atlas_nomipmap.png");

    //    CCSprite *img0 = CCSprite::spriteWithTexture(texture0);
    //    img0->setTextureRectInPixels(CCRectMake(85, 121, 85, 121));
    //    img0->setPosition(ccp( s.width/3.0f, s.height/2.0f));
    //    addChild(img0);

    //    CCSprite *img1 = CCSprite::spriteWithTexture(texture1);
    //    img1->setTextureRectInPixels(CCRectMake(85, 121, 85, 121));
    //    img1->setPosition(ccp( 2*s.width/3.0f, s.height/2.0f));
    //    addChild(img1);


    //    CCEaseOut* scale1 = CCEaseOut::actionWithAction(CCScaleBy::actionWithDuration(4, 0.01f), 3);
    //    CCActionInterval* sc_back = scale1->reverse();

    //    CCEaseOut* scale2 = (CCEaseOut*) (scale1->copy());
    //    scale2->autorelease();
    //    CCActionInterval* sc_back2 = scale2->reverse();

    //    img0->runAction(CCRepeatForever::actionWithAction((CCActionInterval*)(CCSequence::actions(scale1, sc_back, NULL))));
    //    img1->runAction(CCRepeatForever::actionWithAction((CCActionInterval*)(CCSequence::actions(scale2, sc_back2, NULL))));
    //    CCTextureCache::sharedTextureCache()->dumpCachedTextureInfo();
    //}

    //std::string TextureMipMap::title()
    //{
    //    return "Texture Mipmap";
    //}

    //std::string TextureMipMap::subtitle()
    //{
    //    return "Left image uses mipmap. Right image doesn't";
    //}

    ////------------------------------------------------------------------
    ////
    //// TexturePVRMipMap
    //// To generate PVR images read this article:
    //// http://developer.apple.com/iphone/library/qa/qa2008/qa1611.html
    ////
    ////------------------------------------------------------------------
    //void TexturePVRMipMap::onEnter()
    //{
    //    TextureDemo::onEnter();
    //    CCSize s = CCDirector::sharedDirector()->getWinSize();

    //    CCSprite *imgMipMap = CCSprite::create("Images/logo-mipmap.pvr");
    //    if( imgMipMap )
    //    {
    //        imgMipMap->setPosition(ccp( s.width/2.0f-100, s.height/2.0f));
    //        addChild(imgMipMap);

    //        // support mipmap filtering
    //        ccTexParams texParams = { GL_LINEAR_MIPMAP_LINEAR, GL_LINEAR, GL_CLAMP_TO_EDGE, GL_CLAMP_TO_EDGE };	
    //        imgMipMap->getTexture()->setTexParameters(&texParams);
    //    }

    //    CCSprite *img = CCSprite::create("Images/logo-nomipmap.pvr");
    //    if( img )
    //    {
    //        img->setPosition(ccp( s.width/2.0f+100, s.height/2.0f));
    //        addChild(img);

    //        CCEaseOut* scale1 = CCEaseOut::actionWithAction(CCScaleBy::actionWithDuration(4, 0.01f), 3);
    //        CCActionInterval* sc_back = scale1->reverse();

    //        CCEaseOut* scale2 = (CCEaseOut*) (scale1->copy());
    //        scale2->autorelease();
    //        CCActionInterval* sc_back2 = scale2->reverse();

    //        imgMipMap->runAction(CCRepeatForever::actionWithAction((CCActionInterval*)(CCSequence::actions(scale1, sc_back, NULL))));
    //        img->runAction(CCRepeatForever::actionWithAction((CCActionInterval*)(CCSequence::actions(scale2, sc_back2, NULL))));
    //    }
    //    CCTextureCache::sharedTextureCache()->dumpCachedTextureInfo();
    //}

    //std::string TexturePVRMipMap::title()
    //{
    //    return "PVRTC MipMap Test";
    //}
    //std::string TexturePVRMipMap::subtitle()
    //{
    //    return "Left image uses mipmap. Right image doesn't";
    //}

    ////------------------------------------------------------------------
    ////
    //// TexturePVRMipMap2
    ////
    ////------------------------------------------------------------------
    //void TexturePVRMipMap2::onEnter()
    //{
    //    TextureDemo::onEnter();
    //    CCSize s = CCDirector::sharedDirector()->getWinSize();

    //    CCSprite *imgMipMap = CCSprite::create("Images/test_image_rgba4444_mipmap.pvr");
    //    imgMipMap->setPosition(ccp( s.width/2.0f-100, s.height/2.0f));
    //    addChild(imgMipMap);

    //    // support mipmap filtering
    //    ccTexParams texParams = { GL_LINEAR_MIPMAP_LINEAR, GL_LINEAR, GL_CLAMP_TO_EDGE, GL_CLAMP_TO_EDGE };	
    //    imgMipMap->getTexture()->setTexParameters(&texParams);

    //    CCSprite *img = CCSprite::create("Images/test_image.png");
    //    img->setPosition(ccp( s.width/2.0f+100, s.height/2.0f));
    //    addChild(img);

    //    CCEaseOut* scale1 = CCEaseOut::actionWithAction(CCScaleBy::actionWithDuration(4, 0.01f), 3);
    //    CCActionInterval* sc_back = scale1->reverse();

    //    CCEaseOut* scale2 = (CCEaseOut*) (scale1->copy());
    //    scale2->autorelease();
    //    CCActionInterval* sc_back2 = scale2->reverse();

    //    imgMipMap->runAction(CCRepeatForever::actionWithAction((CCActionInterval*)(CCSequence::actions(scale1, sc_back, NULL))));
    //    img->runAction(CCRepeatForever::actionWithAction((CCActionInterval*)(CCSequence::actions(scale2, sc_back2, NULL))));
    //    CCTextureCache::sharedTextureCache()->dumpCachedTextureInfo();
    //}

    //std::string TexturePVRMipMap2::title()
    //{
    //    return "PVR MipMap Test #2";
    //}

    //std::string TexturePVRMipMap2::subtitle()
    //{
    //    return "Left image uses mipmap. Right image doesn't";
    //}

    ////------------------------------------------------------------------
    ////
    //// TexturePVR2BPP
    //// Image generated using PVRTexTool:
    //// http://www.imgtec.com/powervr/insider/powervr-pvrtextool.asp
    ////
    ////------------------------------------------------------------------
    //void TexturePVR2BPP::onEnter()
    //{
    //    TextureDemo::onEnter();
    //    CCSize s = CCDirector::sharedDirector()->getWinSize();

    //    CCSprite *img = CCSprite::create("Images/test_image_pvrtc2bpp.pvr");

    //    if( img )
    //    {
    //        img->setPosition(ccp( s.width/2.0f, s.height/2.0f));
    //        addChild(img);
    //    }
    //    CCTextureCache::sharedTextureCache()->dumpCachedTextureInfo();
    //}

    //std::string TexturePVR2BPP::title()
    //{
    //    return "PVR TC 2bpp Test";
    //}

    ////------------------------------------------------------------------
    ////
    //// TexturePVRRaw
    //// To generate PVR images read this article:
    //// http://developer.apple.com/iphone/library/qa/qa2008/qa1611.html
    ////
    ////------------------------------------------------------------------
    //void TexturePVRRaw::onEnter()
    //{
    //    TextureDemo::onEnter();
    //#ifdef CC_SUPPORT_PVRTC
    //    CCSize s = CCDirector::sharedDirector()->getWinSize();

    //    CCTexture2D *tex = CCTextureCache::sharedTextureCache()->addPVRTCImage("Images/test_image.pvrraw", 4, true, 128);
    //    CCSprite *img = CCSprite::spriteWithTexture(tex);
    //    img->setPosition(ccp( s.width/2.0f, s.height/2.0f));
    //    addChild(img);
    //#else
    //    CCLog("Not support PVRTC!");
    //#endif

    //    CCTextureCache::sharedTextureCache()->dumpCachedTextureInfo();
    //}

    //std::string TexturePVRRaw::title()
    //{
    //    return "PVR TC 4bpp Test #1 (Raw)";
    //}

    ////------------------------------------------------------------------
    ////
    //// TexturePVR
    //// To generate PVR images read this article:
    //// http://developer.apple.com/iphone/library/qa/qa2008/qa1611.html
    ////
    ////------------------------------------------------------------------
    //void TexturePVR::onEnter()
    //{
    //    TextureDemo::onEnter();
    //    CCSize s = CCDirector::sharedDirector()->getWinSize();

    //    CCSprite *img = CCSprite::create("Images/test_image.pvr");

    //    if( img )
    //    {
    //        img->setPosition(ccp( s.width/2.0f, s.height/2.0f));
    //        addChild(img);
    //    }
    //    else
    //    {
    //        CCLog("This test is not supported.");
    //    }
    //    CCTextureCache::sharedTextureCache()->dumpCachedTextureInfo();

    //}

    //std::string TexturePVR::title()
    //{
    //    return "PVR TC 4bpp Test #2";
    //}

    ////------------------------------------------------------------------
    ////
    //// TexturePVR4BPP
    //// Image generated using PVRTexTool:
    //// http://www.imgtec.com/powervr/insider/powervr-pvrtextool.asp
    ////
    ////------------------------------------------------------------------
    //void TexturePVR4BPP::onEnter()
    //{
    //    TextureDemo::onEnter();
    //    CCSize s = CCDirector::sharedDirector()->getWinSize();

    //    CCSprite *img = CCSprite::create("Images/test_image_pvrtc4bpp.pvr");

    //    if( img )
    //    {
    //        img->setPosition(ccp( s.width/2.0f, s.height/2.0f));
    //        addChild(img);
    //    }
    //    else
    //    {
    //        CCLog("This test is not supported in cocos2d-mac");
    //    }
    //    CCTextureCache::sharedTextureCache()->dumpCachedTextureInfo();
    //}

    //std::string TexturePVR4BPP::title()
    //{
    //    return "PVR TC 4bpp Test #3";
    //}

    ////------------------------------------------------------------------
    ////
    //// TexturePVRRGBA8888
    //// Image generated using PVRTexTool:
    //// http://www.imgtec.com/powervr/insider/powervr-pvrtextool.asp
    ////
    ////------------------------------------------------------------------
    //void TexturePVRRGBA8888::onEnter()
    //{
    //    TextureDemo::onEnter();
    //    CCSize s = CCDirector::sharedDirector()->getWinSize();

    //    CCSprite *img = CCSprite::create("Images/test_image_rgba8888.pvr");
    //    img->setPosition(ccp( s.width/2.0f, s.height/2.0f));
    //    addChild(img);
    //    CCTextureCache::sharedTextureCache()->dumpCachedTextureInfo();
    //}

    //std::string TexturePVRRGBA8888::title()
    //{
    //    return "PVR + RGBA  8888 Test";
    //}

    ////------------------------------------------------------------------
    ////
    //// TexturePVRBGRA8888
    //// Image generated using PVRTexTool:
    //// http://www.imgtec.com/powervr/insider/powervr-pvrtextool.asp
    ////
    ////------------------------------------------------------------------
    //void TexturePVRBGRA8888::onEnter()
    //{
    //    TextureDemo::onEnter();
    //    CCSize s = CCDirector::sharedDirector()->getWinSize();

    //    CCSprite *img = CCSprite::create("Images/test_image_bgra8888.pvr");
    //    if( img )
    //    {
    //        img->setPosition(ccp( s.width/2.0f, s.height/2.0f));
    //        addChild(img);
    //    }
    //    else
    //    {
    //        CCLog("BGRA8888 images are not supported");
    //    }
    //    CCTextureCache::sharedTextureCache()->dumpCachedTextureInfo();
    //}

    //std::string TexturePVRBGRA8888::title()
    //{
    //    return "PVR + BGRA 8888 Test";
    //}

    ////------------------------------------------------------------------
    ////
    //// TexturePVRRGBA5551
    //// Image generated using PVRTexTool:
    //// http://www.imgtec.com/powervr/insider/powervr-pvrtextool.asp
    ////
    ////------------------------------------------------------------------
    //void TexturePVRRGBA5551::onEnter()
    //{
    //    TextureDemo::onEnter();
    //    CCSize s = CCDirector::sharedDirector()->getWinSize();

    //    CCSprite *img = CCSprite::create("Images/test_image_rgba5551.pvr");
    //    img->setPosition(ccp( s.width/2.0f, s.height/2.0f));
    //    addChild(img);
    //    CCTextureCache::sharedTextureCache()->dumpCachedTextureInfo();
    //}

    //std::string TexturePVRRGBA5551::title()
    //{
    //    return "PVR + RGBA 5551 Test";
    //}

    ////------------------------------------------------------------------
    ////
    //// TexturePVRRGBA4444
    //// Image generated using PVRTexTool:
    //// http://www.imgtec.com/powervr/insider/powervr-pvrtextool.asp
    ////
    ////------------------------------------------------------------------
    //void TexturePVRRGBA4444::onEnter()
    //{
    //    TextureDemo::onEnter();
    //    CCSize s = CCDirector::sharedDirector()->getWinSize();

    //    CCSprite *img = CCSprite::create("Images/test_image_rgba4444.pvr");
    //    img->setPosition(ccp( s.width/2.0f, s.height/2.0f));
    //    addChild(img);
    //    CCTextureCache::sharedTextureCache()->dumpCachedTextureInfo();
    //}

    //std::string TexturePVRRGBA4444::title()
    //{
    //    return "PVR + RGBA 4444 Test";
    //}

    ////------------------------------------------------------------------
    ////
    //// TexturePVRRGBA4444GZ
    //// Image generated using PVRTexTool:
    //// http://www.imgtec.com/powervr/insider/powervr-pvrtextool.asp
    ////
    ////------------------------------------------------------------------
    //void TexturePVRRGBA4444GZ::onEnter()
    //{
    //    TextureDemo::onEnter();
    //    CCSize s = CCDirector::sharedDirector()->getWinSize();

    //#if (CC_TARGET_PLATFORM == CC_PLATFORM_ANDROID)
    //    // android can not pack .gz file into apk file
    //    CCSprite *img = CCSprite::create("Images/test_image_rgba4444.pvr");
    //#else
    //    CCSprite *img = CCSprite::create("Images/test_image_rgba4444.pvr.gz");
    //#endif
    //    img->setPosition(ccp( s.width/2.0f, s.height/2.0f));
    //    addChild(img);
    //    CCTextureCache::sharedTextureCache()->dumpCachedTextureInfo();
    //}

    //std::string TexturePVRRGBA4444GZ::title()
    //{
    //    return "PVR + RGBA 4444 + GZ Test";
    //}

    //std::string TexturePVRRGBA4444GZ::subtitle()
    //{
    //    return "This is a gzip PVR image";
    //}

    ////------------------------------------------------------------------
    ////
    //// TexturePVRRGBA4444CCZ
    //// Image generated using PVRTexTool:
    //// http://www.imgtec.com/powervr/insider/powervr-pvrtextool.asp
    ////
    ////------------------------------------------------------------------
    //void TexturePVRRGBA4444CCZ::onEnter()
    //{
    //    TextureDemo::onEnter();
    //    CCSize s = CCDirector::sharedDirector()->getWinSize();

    //    CCSprite *img = CCSprite::create("Images/test_image_rgba4444.pvr.ccz");
    //    img->setPosition(ccp( s.width/2.0f, s.height/2.0f));
    //    addChild(img);	
    //    CCTextureCache::sharedTextureCache()->dumpCachedTextureInfo();
    //}

    //std::string TexturePVRRGBA4444CCZ::title()
    //{
    //    return "PVR + RGBA 4444 + CCZ Test";
    //}

    //std::string TexturePVRRGBA4444CCZ::subtitle()
    //{
    //    return "This is a ccz PVR image";
    //}

    ////------------------------------------------------------------------
    ////
    //// TexturePVRRGB565
    //// Image generated using PVRTexTool:
    //// http://www.imgtec.com/powervr/insider/powervr-pvrtextool.asp
    ////
    ////------------------------------------------------------------------
    //void TexturePVRRGB565::onEnter()
    //{
    //    TextureDemo::onEnter();
    //    CCSize s = CCDirector::sharedDirector()->getWinSize();

    //    CCSprite *img = CCSprite::create("Images/test_image_rgb565.pvr");
    //    img->setPosition(ccp( s.width/2.0f, s.height/2.0f));
    //    addChild(img);
    //    CCTextureCache::sharedTextureCache()->dumpCachedTextureInfo();
    //}

    //std::string TexturePVRRGB565::title()
    //{
    //    return "PVR + RGB 565 Test";
    //}

    ////------------------------------------------------------------------
    ////
    //// TexturePVRA8
    //// Image generated using PVRTexTool:
    //// http://www.imgtec.com/powervr/insider/powervr-pvrtextool.asp
    ////
    ////------------------------------------------------------------------
    //void TexturePVRA8::onEnter()
    //{
    //    TextureDemo::onEnter();
    //    CCSize s = CCDirector::sharedDirector()->getWinSize();

    //    CCSprite *img = CCSprite::create("Images/test_image_a8.pvr");
    //    img->setPosition(ccp( s.width/2.0f, s.height/2.0f));
    //    addChild(img);
    //    CCTextureCache::sharedTextureCache()->dumpCachedTextureInfo();

    //}

    //std::string TexturePVRA8::title()
    //{
    //    return "PVR + A8 Test";
    //}

    ////------------------------------------------------------------------
    ////
    //// TexturePVRI8
    //// Image generated using PVRTexTool:
    //// http://www.imgtec.com/powervr/insider/powervr-pvrtextool.asp
    ////
    ////------------------------------------------------------------------
    //void TexturePVRI8::onEnter()
    //{
    //    TextureDemo::onEnter();
    //    CCSize s = CCDirector::sharedDirector()->getWinSize();

    //    CCSprite *img = CCSprite::create("Images/test_image_i8.pvr");
    //    img->setPosition(ccp( s.width/2.0f, s.height/2.0f));
    //    addChild(img);
    //    CCTextureCache::sharedTextureCache()->dumpCachedTextureInfo();
    //}

    //std::string TexturePVRI8::title()
    //{
    //    return "PVR + I8 Test";
    //}

    ////------------------------------------------------------------------
    ////
    //// TexturePVRAI88
    //// Image generated using PVRTexTool:
    //// http://www.imgtec.com/powervr/insider/powervr-pvrtextool.asp
    ////
    ////------------------------------------------------------------------
    //void TexturePVRAI88::onEnter()
    //{
    //    TextureDemo::onEnter();
    //    CCSize s = CCDirector::sharedDirector()->getWinSize();

    //    CCSprite *img = CCSprite::create("Images/test_image_ai88.pvr");
    //    img->setPosition(ccp( s.width/2.0f, s.height/2.0f));
    //    addChild(img);
    //    CCTextureCache::sharedTextureCache()->dumpCachedTextureInfo();
    //}

    //std::string TexturePVRAI88::title()
    //{
    //    return "PVR + AI88 Test";
    //}

    ////------------------------------------------------------------------
    ////
    //// TexturePVRBadEncoding
    //// Image generated using PVRTexTool:
    //// http://www.imgtec.com/powervr/insider/powervr-pvrtextool.asp
    ////
    ////------------------------------------------------------------------
    //void TexturePVRBadEncoding::onEnter()
    //{
    //    TextureDemo::onEnter();
    //    CCSize s = CCDirector::sharedDirector()->getWinSize();

    //    CCSprite *img = CCSprite::create("Images/test_image-bad_encoding.pvr");
    //    if( img )
    //    {
    //        img->setPosition(ccp( s.width/2.0f, s.height/2.0f));
    //        addChild(img);
    //    }
    //}

    //std::string TexturePVRBadEncoding::title()
    //{
    //    return "PVR Unsupported encoding";
    //}

    //std::string TexturePVRBadEncoding::subtitle()
    //{
    //    return "You should not see any image";
    //}

    ////------------------------------------------------------------------
    ////
    //// TexturePVRNonSquare
    ////
    ////------------------------------------------------------------------
    //void TexturePVRNonSquare::onEnter()
    //{
    //    TextureDemo::onEnter();
    //    CCSize s = CCDirector::sharedDirector()->getWinSize();

    //    CCSprite *img = CCSprite::create("Images/grossini_128x256_mipmap.pvr");
    //    img->setPosition(ccp( s.width/2.0f, s.height/2.0f));
    //    addChild(img);
    //    CCTextureCache::sharedTextureCache()->dumpCachedTextureInfo();
    //}

    //std::string TexturePVRNonSquare::title()
    //{
    //    return "PVR + Non square texture";
    //}

    //std::string TexturePVRNonSquare::subtitle()
    //{
    //    return "Loading a 128x256 texture";
    //}

    ////------------------------------------------------------------------
    ////
    //// TexturePVRNPOT4444
    ////
    ////------------------------------------------------------------------
    //void TexturePVRNPOT4444::onEnter()
    //{
    //    TextureDemo::onEnter();
    //    CCSize s = CCDirector::sharedDirector()->getWinSize();

    //    CCSprite *img = CCSprite::create("Images/grossini_pvr_rgba4444.pvr");
    //    if ( img )
    //    {
    //        img->setPosition(ccp( s.width/2.0f, s.height/2.0f));
    //        addChild(img);
    //    }
    //    CCTextureCache::sharedTextureCache()->dumpCachedTextureInfo();
    //}

    //std::string TexturePVRNPOT4444::title()
    //{
    //    return "PVR RGBA4 + NPOT texture";
    //}

    //std::string TexturePVRNPOT4444::subtitle()
    //{
    //    return "Loading a 81x121 RGBA4444 texture.";
    //}

    ////------------------------------------------------------------------
    ////
    //// TexturePVRNPOT8888
    ////
    ////------------------------------------------------------------------
    //void TexturePVRNPOT8888::onEnter()
    //{
    //    TextureDemo::onEnter();
    //    CCSize s = CCDirector::sharedDirector()->getWinSize();

    //    CCSprite *img = CCSprite::create("Images/grossini_pvr_rgba8888.pvr");
    //    if( img )
    //    {
    //        img->setPosition(ccp( s.width/2.0f, s.height/2.0f));
    //        addChild(img);
    //    }
    //    CCTextureCache::sharedTextureCache()->dumpCachedTextureInfo();
    //}

    //std::string TexturePVRNPOT8888::title()
    //{
    //    return "PVR RGBA8 + NPOT texture";
    //}

    //std::string TexturePVRNPOT8888::subtitle()
    //{
    //    return "Loading a 81x121 RGBA8888 texture.";
    //}

    //------------------------------------------------------------------
    //
    // TextureAlias
    //
    //------------------------------------------------------------------
    public class TextureAlias : TextureDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();
            CCSize s = Layer.VisibleBoundsWorldspace.Size;

            //
            // Sprite 1: GL_LINEAR
            //
            // Default filter is GL_LINEAR

            CCSprite sprite = new CCSprite("Images/grossinis_sister1");
            sprite.Position = new CCPoint(s.Width / 3.0f, s.Height / 2.0f);
            AddChild(sprite);

            // this is the default filterting
            sprite.IsAntialiased = true;

            //
            // Sprite 1: GL_NEAREST
            //	

            CCSprite sprite2 = new CCSprite("Images/grossinis_sister2");
            sprite2.Position = new CCPoint(2 * s.Width / 3.0f, s.Height / 2.0f);
            AddChild(sprite2);

            // Use Nearest in this one
            sprite2.IsAntialiased = false;

            // scale them to show
			var sc = new CCScaleBy(3, 8.0f);
			var sc_back = sc.Reverse();
            CCRepeatForever scaleforever = new CCRepeatForever(sc, sc_back);

            sprite2.RunAction(scaleforever);
			sprite.RunAction(scaleforever);
            CCTextureCache.SharedTextureCache.DumpCachedTextureInfo();
        }

        public override string title()
        {
            return "AntiAlias / Alias textures";
        }

        public override string subtitle()
        {
            return "Left image is antialiased. Right image is aliases";
        }
    }

    ////------------------------------------------------------------------
    ////
    //// TexturePixelFormat
    ////
    ////------------------------------------------------------------------
    //void TexturePixelFormat::onEnter()
    //{
    //    //
    //    // This example displays 1 png images 4 times.
    //    // Each time the image is generated using:
    //    // 1- 32-bit RGBA8
    //    // 2- 16-bit RGBA4
    //    // 3- 16-bit RGB5A1
    //    // 4- 16-bit RGB565
    //    TextureDemo::onEnter();

    //    CCLabelTTF *label = (CCLabelTTF*) getChildByTag(kTagLabel);
    //    label->setColor(ccc3(16,16,255));

    //    CCSize s = CCDirector::sharedDirector()->getWinSize();

    //    CCLayerColor *background = CCLayerColor::layerWithColorWidthHeight(ccc4(128,128,128,255), s.width, s.height);
    //    addChild(background, -1);

    //    // RGBA 8888 image (32-bit)
    //    CCTexture2D::setDefaultAlphaPixelFormat(kCCTexture2DPixelFormat_RGBA8888);
    //    CCSprite *sprite1 = CCSprite::create("Images/test-rgba1.png");
    //    sprite1->setPosition(ccp(1*s.width/6, s.height/2+32));
    //    addChild(sprite1, 0);

    //    // remove texture from texture manager	
    //    CCTextureCache::sharedTextureCache()->removeTexture(sprite1->getTexture());

    //    // RGBA 4444 image (16-bit)
    //    CCTexture2D::setDefaultAlphaPixelFormat(kCCTexture2DPixelFormat_RGBA4444);
    //    CCSprite *sprite2 = CCSprite::create("Images/test-rgba1.png");
    //    sprite2->setPosition(ccp(2*s.width/6, s.height/2-32));
    //    addChild(sprite2, 0);

    //    // remove texture from texture manager	
    //    CCTextureCache::sharedTextureCache()->removeTexture(sprite2->getTexture());

    //    // RGB5A1 image (16-bit)
    //    CCTexture2D::setDefaultAlphaPixelFormat(kCCTexture2DPixelFormat_RGB5A1);
    //    CCSprite *sprite3 = CCSprite::create("Images/test-rgba1.png");
    //    sprite3->setPosition(ccp(3*s.width/6, s.height/2+32));
    //    addChild(sprite3, 0);

    //    // remove texture from texture manager	
    //    CCTextureCache::sharedTextureCache()->removeTexture(sprite3->getTexture());

    //    // RGB565 image (16-bit)
    //    CCTexture2D::setDefaultAlphaPixelFormat(kCCTexture2DPixelFormat_RGB565);
    //    CCSprite *sprite4 = CCSprite::create("Images/test-rgba1.png");
    //    sprite4->setPosition(ccp(4*s.width/6, s.height/2-32));
    //    addChild(sprite4, 0);

    //    // remove texture from texture manager	
    //    CCTextureCache::sharedTextureCache()->removeTexture(sprite4->getTexture());

    //    // A8 image (8-bit)
    //    CCTexture2D::setDefaultAlphaPixelFormat(kCCTexture2DPixelFormat_A8);
    //    CCSprite *sprite5 = CCSprite::create("Images/test-rgba1.png");
    //    sprite5->setPosition(ccp(5*s.width/6, s.height/2+32));
    //    addChild(sprite5, 0);

    //    // remove texture from texture manager	
    //    CCTextureCache::sharedTextureCache()->removeTexture(sprite5->getTexture());

    //    CCFadeOut* fadeout = CCFadeOut::actionWithDuration(2);
    //    CCFadeIn*  fadein  = CCFadeIn::actionWithDuration(2);
    //    CCFiniteTimeAction* seq = CCSequence::actions(CCDelayTime::actionWithDuration(2), fadeout, fadein, NULL);
    //    CCRepeatForever* seq_4ever = CCRepeatForever::actionWithAction((CCActionInterval*) seq);
    //    CCRepeatForever* seq_4ever2 = (CCRepeatForever*) (seq_4ever->copy()); seq_4ever2->autorelease();
    //    CCRepeatForever* seq_4ever3 = (CCRepeatForever*) (seq_4ever->copy()); seq_4ever3->autorelease();
    //    CCRepeatForever* seq_4ever4 = (CCRepeatForever*) (seq_4ever->copy()); seq_4ever4->autorelease();
    //    CCRepeatForever* seq_4ever5 = (CCRepeatForever*) (seq_4ever->copy()); seq_4ever5->autorelease();

    //    sprite1->runAction(seq_4ever);
    //    sprite2->runAction(seq_4ever2);
    //    sprite3->runAction(seq_4ever3);
    //    sprite4->runAction(seq_4ever4);
    //    sprite5->runAction(seq_4ever5);

    //    // restore default
    //    CCTexture2D::setDefaultAlphaPixelFormat(kCCTexture2DPixelFormat_Default);
    //    CCTextureCache::sharedTextureCache()->dumpCachedTextureInfo();
    //}

    //std::string TexturePixelFormat::title()
    //{
    //    return "Texture Pixel Formats";
    //}

    //std::string TexturePixelFormat::subtitle()
    //{
    //    return "Textures: RGBA8888, RGBA4444, RGB5A1, RGB565, A8";
    //}

    ////------------------------------------------------------------------
    ////
    //// TextureBlend
    ////
    ////------------------------------------------------------------------
    //void TextureBlend::onEnter()
    //{
    //    TextureDemo::onEnter();

    //    for( int i=0;i < 15;i++ )
    //    {
    //        // BOTTOM sprites have alpha pre-multiplied
    //        // they use by default GL_ONE, GL_ONE_MINUS_SRC_ALPHA
    //        CCSprite *cloud = CCSprite::create("Images/test_blend.png");
    //        addChild(cloud, i+1, 100+i);
    //        cloud->setPosition(ccp(50+25*i, 80));
    //        ccBlendFunc blendFunc1 = { GL_ONE, GL_ONE_MINUS_SRC_ALPHA };
    //        cloud->setBlendFunc(blendFunc1);

    //        // CENTER sprites have also alpha pre-multiplied
    //        // they use by default GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA
    //        cloud = CCSprite::create("Images/test_blend.png");
    //        addChild(cloud, i+1, 200+i);
    //        cloud->setPosition(ccp(50+25*i, 160));
    //        ccBlendFunc blendFunc2 = { GL_ONE_MINUS_DST_COLOR, GL_ZERO };
    //        cloud->setBlendFunc(blendFunc2);

    //        // UPPER sprites are using custom blending function
    //        // You can set any blend function to your sprites
    //        cloud = CCSprite::create("Images/test_blend.png");
    //        addChild(cloud, i+1, 200+i);
    //        cloud->setPosition(ccp(50+25*i, 320-80));
    //        ccBlendFunc blendFunc3 = { GL_SRC_ALPHA, GL_ONE };
    //        cloud->setBlendFunc(blendFunc3);  // additive blending
    //    }
    //}

    //std::string TextureBlend::title()
    //{
    //    return "Texture Blending";
    //}

    //std::string TextureBlend::subtitle()
    //{
    //    return "Testing 3 different blending modes";
    //}

    ////------------------------------------------------------------------
    ////
    //// TextureGlClamp
    ////
    ////------------------------------------------------------------------
    //void TextureGlClamp::onEnter()
    //{
    //    TextureDemo::onEnter();

    //    CCSize size = CCDirector::sharedDirector()->getWinSize();

    //    // The .png image MUST be power of 2 in order to create a continue effect.
    //    // eg: 32x64, 512x128, 256x1024, 64x64, etc..
    //    CCSprite *sprite = CCSprite::create("Images/pattern1.png", CCRectMake(0,0,512,256));
    //    addChild(sprite, -1, kTagSprite1);
    //    sprite->setPosition(ccp(size.width/2,size.height/2));
    //    ccTexParams params = {GL_LINEAR,GL_LINEAR,GL_CLAMP_TO_EDGE, GL_CLAMP_TO_EDGE};
    //    sprite->getTexture()->setTexParameters(&params);

    //    CCRotateBy* rotate = CCRotateBy::actionWithDuration(4, 360);
    //    sprite->runAction(rotate);
    //    CCScaleBy* scale = CCScaleBy::actionWithDuration(2, 0.04f);
    //    CCScaleBy* scaleBack = (CCScaleBy*) (scale->reverse());
    //    CCFiniteTimeAction* seq = CCSequence::actions(scale, scaleBack, NULL);
    //    sprite->runAction(seq);
    //}

    //std::string TextureGlClamp::title()
    //{
    //    return "Texture GL_CLAMP";
    //}

    //TextureGlClamp::~TextureGlClamp()
    //{
    //    CCTextureCache::sharedTextureCache()->removeUnusedTextures();
    //}
    //------------------------------------------------------------------
    //
    // TextureGLClamp
    //
    //------------------------------------------------------------------
    public class TextureGLClamp : TextureDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();
            CCSize size = Layer.VisibleBoundsWorldspace.Size;

            // The .png image MUST be power of 2 in order to create a continue effect.
            // eg: 32x64, 512x128, 256x1024, 64x64, etc..
            var sprite = new CCSprite("Images/pattern1.png", new CCRect(0, 0, 512, 256));
            AddChild(sprite, -1, (int) enumTag.kTagSprite1);
            sprite.Position = new CCPoint(size.Width / 2, size.Height / 2);

            // Cocos2D-XNA no longer uses TexParameters.  Please use the XNA SamplerState
//            sprite.Texture.TexParameters = new CCTexParams() {  MagFilter = (uint)All.Linear,
//                MinFilter = (uint)All.Linear,
//                WrapS = (uint)All.ClampToEdge,
//                WrapT = (uint)All.ClampToEdge
//            };
            sprite.Texture.SamplerState = SamplerState.LinearClamp;

            var rotate = new CCRotateBy(4, 360);
            sprite.RunAction(rotate);
            var scale = new CCScaleBy(2, 0.04f);
            var scaleBack = (CCScaleBy) scale.Reverse();
            var seq = new CCSequence(scale, scaleBack);
            sprite.RunAction(seq);

        }

        public override string title()
        {
            return "Texture GL_CLAMP";
        }

//        public override string subtitle()
//        {
//            return "Texture is repeated within the area.";
//        }
    }

    //------------------------------------------------------------------
    //
    // TextureGLMirror
    //
    //------------------------------------------------------------------
    public class TextureGLMirror : TextureDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();
            CCSize size = Layer.VisibleBoundsWorldspace.Size;

            // The .png image MUST be power of 2 in order to create a continue effect.
            // eg: 32x64, 512x128, 256x1024, 64x64, etc..
            var sprite = new CCSprite("Images/pattern1.png", new CCRect(0, 0, 512, 256));
            AddChild(sprite, -1, (int) enumTag.kTagSprite1);
            sprite.Position = new CCPoint(size.Width / 2, size.Height / 2);

            // Cocos2D-XNA no longer uses TexParameters.  Please use the XNA SamplerState
//			sprite.Texture.TexParameters = new CCTexParams() {  MagFilter = (uint)All.Linear,
//				MinFilter = (uint)All.Linear,
//				WrapS = (uint)All.MirroredRepeat,
//				WrapT = (uint)All.Repeat
//			};

            var state = new SamplerState();
            state.AddressU = TextureAddressMode.Mirror;
            state.AddressV = TextureAddressMode.Wrap;
            sprite.Texture.SamplerState = state;

            var rotate = new CCRotateBy(4, 360);
            sprite.RunAction(rotate);
            var scale = new CCScaleBy(2, 0.04f);
            var scaleBack = (CCScaleBy) scale.Reverse();
            var seq = new CCSequence(scale, scaleBack);
            sprite.RunAction(seq);

        }

        public override string title()
        {
            return "Texture GL_MIRROR";
        }

        public override string subtitle()
        {
            return "Texture is repeated within the area and Mirrored.";
        }
    }

    //------------------------------------------------------------------
    //
    // TextureGLRepeat
    //
    //------------------------------------------------------------------
    public class TextureGLRepeat : TextureDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();
            CCSize size = Layer.VisibleBoundsWorldspace.Size;

            // The .png image MUST be power of 2 in order to create a continue effect.
            // eg: 32x64, 512x128, 256x1024, 64x64, etc..
            var sprite = new CCSprite("Images/pattern1.png", new CCRect(0, 0, 4096, 4096));
            AddChild(sprite, -1, (int) enumTag.kTagSprite1);
            sprite.Position = new CCPoint(size.Width / 2, size.Height / 2);

            // Cocos2D-XNA no longer uses TexParameters.  Please use the XNA SamplerState
//            sprite.Texture.TexParameters = new CCTexParams() {  MagFilter = (uint)All.Linear,
//                                                                MinFilter = (uint)All.Linear,
//                                                                WrapS = (uint)All.Repeat,
//                                                                WrapT = (uint)All.Repeat
//                                                              };

            sprite.Texture.SamplerState = SamplerState.LinearWrap;

            var rotate = new CCRotateBy(4, 360);
            sprite.RunAction(rotate);
            var scale = new CCScaleBy(2, 0.04f);
            var scaleBack = (CCScaleBy) scale.Reverse();
            var seq = new CCSequence(scale, scaleBack);
            sprite.RunAction(seq);

        }

        public override string title()
        {
            return "Texture GL_REPEAT";
        }

        public override string subtitle()
        {
            return "Texture is repeated within the area.";
        }
    }

    //------------------------------------------------------------------
    //
    // TextureCache1
    //
    //------------------------------------------------------------------
    public class TextureCache1 : TextureDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();

            CCSize s = Layer.VisibleBoundsWorldspace.Size;

            CCSprite sprite;

            sprite = new CCSprite("Images/grossinis_sister1");
            sprite.Position = new CCPoint(s.Width / 5 * 1, s.Height / 2);
            sprite.IsAntialiased = false;
            sprite.Scale = 2;
            AddChild(sprite);

            CCTextureCache.SharedTextureCache.RemoveTexture(sprite.Texture);

            sprite = new CCSprite("Images/grossinis_sister1");
            sprite.Position = new CCPoint(s.Width / 5 * 2, s.Height / 2);
            sprite.IsAntialiased = true;
            sprite.Scale = 2;
            AddChild(sprite);

            // 2nd set of sprites

            sprite = new CCSprite("Images/grossinis_sister2");
            sprite.Position = new CCPoint(s.Width / 5 * 3, s.Height / 2);
            sprite.IsAntialiased = false;
            sprite.Scale = 2;
            AddChild(sprite);

            CCTextureCache.SharedTextureCache.RemoveTextureForKey("Images/grossinis_sister2");

            sprite = new CCSprite("Images/grossinis_sister2");
            sprite.Position = new CCPoint(s.Width / 5 * 4, s.Height / 2);
            sprite.IsAntialiased = true;
            sprite.Scale = 2;
            AddChild(sprite);
        }

        public override string title()
        {
            return "CCTextureCache: remove";
        }

        public override string subtitle()
        {
            // temp to remove: for prealpha version
            return "4 images should appear";

            // return "4 images should appear: alias, antialias, alias, antilias";
        }
    }

    internal class TextureAsync : TextureDemo
    {
        private int m_nImageOffset;

        public override void OnEnter()
        {
            base.OnEnter();

            m_nImageOffset = 0;

            CCSize size = Layer.VisibleBoundsWorldspace.Size;

            CCLabelTtf label = new CCLabelTtf("Loading...", "Marker Felt", 32);
            label.Position = size.Center;
            AddChild(label, 10);

			var scale = new CCScaleBy(0.3f, 2);
			label.RepeatForever(scale, scale.Reverse());

            ScheduleOnce(LoadImages, 1.0f);
        }

        public override void OnExit()
        {
            base.OnExit();
            CCTextureCache.SharedTextureCache.RemoveAllTextures();
        }

        private void LoadImages(float dt)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    var szSpriteName = String.Format("Images/sprites_test/sprite-{0}-{1}.png", i, j);
                    CCTextureCache.SharedTextureCache.AddImageAsync(szSpriteName, ImageLoaded);
                }
            }

            CCTextureCache.SharedTextureCache.AddImageAsync("Images/background1.jpg", ImageLoaded);
            CCTextureCache.SharedTextureCache.AddImageAsync("Images/background2.jpg", ImageLoaded);
            CCTextureCache.SharedTextureCache.AddImageAsync("Images/background.png", ImageLoaded);
            CCTextureCache.SharedTextureCache.AddImageAsync("Images/atlastest.png", ImageLoaded);
            CCTextureCache.SharedTextureCache.AddImageAsync("Images/grossini_dance_atlas.png", ImageLoaded);
        }

        private void ImageLoaded(CCTexture2D tex)
        {
            CCDirector director = Scene.Director;

            //CCAssert( [NSThread currentThread] == [director runningThread], @"FAIL. Callback should be on cocos2d thread");

            // IMPORTANT: The order on the callback is not guaranteed. Don't depend on the callback

            // This test just creates a sprite based on the Texture

            CCSprite sprite = new CCSprite(tex);
            sprite.AnchorPoint = CCPoint.Zero;
            AddChild(sprite, -1);

            CCSize size = Layer.VisibleBoundsWorldspace.Size;
            int i = m_nImageOffset * 32;
            sprite.Position = new CCPoint(i % (int) size.Width, (i / (int) size.Width) * 32);

            m_nImageOffset++;
        }

        public override string title()
        {
            return "Texture Async Load";
        }

        public override string subtitle()
        {
            return "Textures should load while an animation is being run";
        }
    }
}
