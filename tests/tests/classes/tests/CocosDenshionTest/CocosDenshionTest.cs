using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using CocosSharp;
using CocosDenshion;
using System.Diagnostics;

namespace tests 
{
    public class CocosDenshionTest : CCLayer
    {
        string EFFECT_FILE = "Sounds/effect1";
        string MUSIC_FILE = "Sounds/background";
        int LINE_SPACE = 40;

        CCMenu m_pItmeMenu;
	    CCPoint m_tBeginPos;
	    int m_nTestCount;
	    int m_nSoundId;

        public CocosDenshionTest()
        {
            m_pItmeMenu = null;
            m_tBeginPos = new CCPoint(0,0);
            m_nSoundId = 0;

	        string[] testItems = {
		        "play background music",
		        "stop background music",
		        "pause background music",
		        "resume background music",
		        "rewind background music",
		        "is background music playing",
		        "play effect",
                "play effect repeatly",
		        "stop effect",
		        "unload effect",
		        "add background music volume",
		        "sub background music volume",
		        "add effects volume",
		        "sub effects volume"
	        };

	        // add menu items for tests
	        m_pItmeMenu = new CCMenu(null);
	        CCSize s = CCDirector.SharedDirector.WinSize;
	        m_nTestCount = testItems.Count<string>();

	        for (int i = 0; i < m_nTestCount; ++i)
	        {
                CCLabelTtf label = new CCLabelTtf(testItems[i], "arial", 24);
                CCMenuItemLabelTTF pMenuItem = new CCMenuItemLabelTTF(label, menuCallback);
		
		        m_pItmeMenu.AddChild(pMenuItem, i + 10000);
		        pMenuItem.Position = new CCPoint( s.Width / 2, (s.Height - (i + 1) * LINE_SPACE) );
	        }

	        m_pItmeMenu.ContentSize = new CCSize(s.Width, (m_nTestCount + 1) * LINE_SPACE);
	        m_pItmeMenu.Position = new CCPoint(0,0);
	        AddChild(m_pItmeMenu);

			// Register Touch Event
			var touchListener = new CCEventListenerTouchAllAtOnce();

			touchListener.OnTouchesBegan = onTouchesBegan;
			touchListener.OnTouchesMoved = onTouchesMoved;

			AddEventListener(touchListener);   

	        // preload background music and effect
	        CCSimpleAudioEngine.SharedEngine.PreloadBackgroundMusic(CCFileUtils.FullPathFromRelativePath(MUSIC_FILE));
	        CCSimpleAudioEngine.SharedEngine.PreloadEffect(CCFileUtils.FullPathFromRelativePath(EFFECT_FILE));
    
            // set default volume
            CCSimpleAudioEngine.SharedEngine.EffectsVolume = 0.5f;
            CCSimpleAudioEngine.SharedEngine.BackgroundMusicVolume = 0.5f;
        }

        ~CocosDenshionTest()
        {
        }

        public override void OnExit()
        {
	        base.OnExit();

	        CCSimpleAudioEngine.SharedEngine.End();
        }

        public void menuCallback(object pSender)
        {
	        // get the userdata, it's the index of the menu item clicked
	        CCMenuItem pMenuItem = (CCMenuItem)(pSender);
	        int nIdx = pMenuItem.ZOrder - 10000;

	        switch(nIdx)
	        {
	        // play background music
	        case 0:

		        CCSimpleAudioEngine.SharedEngine.PlayBackgroundMusic(CCFileUtils.FullPathFromRelativePath(MUSIC_FILE), true);
		        break;
	        // stop background music
	        case 1:
		        CCSimpleAudioEngine.SharedEngine.StopBackgroundMusic();
		        break;
	        // pause background music
	        case 2:
		        CCSimpleAudioEngine.SharedEngine.PauseBackgroundMusic();
		        break;
	        // resume background music
	        case 3:
		        CCSimpleAudioEngine.SharedEngine.ResumeBackgroundMusic();
		        break;
	        // rewind background music
	        case 4:
		        CCSimpleAudioEngine.SharedEngine.RewindBackgroundMusic();
		        break;
	        // is background music playing
	        case 5:
		        if (CCSimpleAudioEngine.SharedEngine.BackgroundMusicPlaying)
		        {
			        CCLog.Log("background music is playing");
		        }
		        else
		        {
                    CCLog.Log("background music is not playing");
		        }
		        break;
	        // play effect
	        case 6:
		        m_nSoundId = CCSimpleAudioEngine.SharedEngine.PlayEffect(CCFileUtils.FullPathFromRelativePath(EFFECT_FILE));
		        break;
            // play effect
            case 7:
                m_nSoundId = CCSimpleAudioEngine.SharedEngine.PlayEffect(CCFileUtils.FullPathFromRelativePath(EFFECT_FILE), true);
                break;
            // stop effect
	        case 8:
		        CCSimpleAudioEngine.SharedEngine.StopEffect(m_nSoundId);
		        break;
	        // unload effect
	        case 9:
		        CCSimpleAudioEngine.SharedEngine.UnloadEffect(CCFileUtils.FullPathFromRelativePath(EFFECT_FILE));
		        break;
		        // add bakcground music volume
	        case 10:
		        CCSimpleAudioEngine.SharedEngine.BackgroundMusicVolume = CCSimpleAudioEngine.SharedEngine.BackgroundMusicVolume + 0.1f;
		        break;
		        // sub backgroud music volume
	        case 11:
		        CCSimpleAudioEngine.SharedEngine.BackgroundMusicVolume = CCSimpleAudioEngine.SharedEngine.BackgroundMusicVolume - 0.1f;
		        break;
		        // add effects volume
	        case 12:
		        CCSimpleAudioEngine.SharedEngine.EffectsVolume = CCSimpleAudioEngine.SharedEngine.EffectsVolume + 0.1f;
		        break;
		        // sub effects volume
	        case 13:
		        CCSimpleAudioEngine.SharedEngine.EffectsVolume = CCSimpleAudioEngine.SharedEngine.EffectsVolume - 0.1f;
		        break;
	        }
	
        }

		void onTouchesBegan(List<CCTouch> touches, CCEvent touchEvent)
        {
			CCTouch touch = touches.FirstOrDefault();

            var m_tBeginPos = touch.Location;
        }

		void onTouchesMoved(List<CCTouch> touches, CCEvent touchEvent)
        {
			CCTouch touch = touches.FirstOrDefault();

	        CCPoint touchLocation = touch.LocationInView;	
	        touchLocation = CCDirector.SharedDirector.ConvertToGl( touchLocation );
	        float nMoveY = touchLocation.Y - m_tBeginPos.Y;

	        CCPoint curPos  = m_pItmeMenu.Position;
	        CCPoint nextPos = new CCPoint(curPos.X, curPos.Y + nMoveY);
	        CCSize winSize = CCDirector.SharedDirector.WinSize;
	        if (nextPos.Y < 0.0f)
	        {
		        m_pItmeMenu.Position = new CCPoint(0,0);
		        return;
	        }

	        if (nextPos.Y > ((m_nTestCount + 1)* LINE_SPACE - winSize.Height))
	        {
		        m_pItmeMenu.Position = new CCPoint(0, ((m_nTestCount + 1)* LINE_SPACE - winSize.Height));
		        return;
	        }

	        m_pItmeMenu.Position = nextPos;
	        m_tBeginPos = touchLocation;
        }


    }


    public class CocosDenshionTestScene : TestScene
    {
        protected override void NextTestCase()
        {
        }
        protected override void PreviousTestCase()
        {
        }
        protected override void RestTestCase()
        {
        }
        public override void runThisTest()
        {
	        CCLayer pLayer = new CocosDenshionTest();
	        AddChild(pLayer);

	        CCDirector.SharedDirector.ReplaceScene(this);
        }
    }
}