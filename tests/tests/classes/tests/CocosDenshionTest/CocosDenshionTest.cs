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
        static readonly string EFFECT_FILE = "Sounds/effect1";
        static readonly string MUSIC_FILE = "Sounds/background";
        const int LINE_SPACE = 40;

        CCMenu testMenu;
        List<CCMenuItem> testMenuItems;
        CCPoint beginPos;
        int soundId;


        #region Constructors

        public CocosDenshionTest()
        {
            testMenu = null;
            beginPos = new CCPoint(0,0);
            soundId = 0;

            string[] testItems = {
                "Play background music",
                "Stop background music",
                "Pause background music",
                "Resume background music",
                "Rewind background music",
                "Is background music playing",
                "Play effect",
                "Play effect repeatly",
                "Stop effect",
                "Unload effect",
                "Add background music volume",
                "Sub background music volume",
                "Add effects volume",
                "Sub effects volume"
            };

            testMenu = new CCMenu(null);
            testMenuItems = new List<CCMenuItem>();

            for(int i=0; i < testItems.Count(); ++i)
            {
                CCLabelTtf label = new CCLabelTtf(testItems[i], "arial", 24);
                CCMenuItemLabelTTF menuItem = new CCMenuItemLabelTTF(label, MenuCallback);
                testMenu.AddChild(menuItem, i + 10000);
                testMenuItems.Add(menuItem);
            }

            AddChild(testMenu);

            // preload background music and effect
            CCSimpleAudioEngine.SharedEngine.PreloadBackgroundMusic(CCFileUtils.FullPathFromRelativePath(MUSIC_FILE));
            CCSimpleAudioEngine.SharedEngine.PreloadEffect(CCFileUtils.FullPathFromRelativePath(EFFECT_FILE));

            // set default volume
            CCSimpleAudioEngine.SharedEngine.EffectsVolume = 0.5f;
            CCSimpleAudioEngine.SharedEngine.BackgroundMusicVolume = 0.5f;

            Camera = AppDelegate.SharedCamera;
        }

        #endregion Constructors


        #region Setup content

        public override void OnEnter()
        {
base.OnEnter(); CCSize windowSize = Layer.VisibleBoundsWorldspace.Size;

            // Layout content
            int testCount = testMenuItems.Count();

            testMenu.ContentSize = new CCSize(windowSize.Width, (testCount + 1) * LINE_SPACE);
            testMenu.Position = new CCPoint(0,0);

            for(int i=0; i < testCount; ++i)
            {
                testMenuItems[i].Position 
                    = new CCPoint( windowSize.Width / 2, (windowSize.Height - (i + 1) * LINE_SPACE));
            }

            // Register touch events
            var touchListener = new CCEventListenerTouchAllAtOnce();

            touchListener.OnTouchesBegan = OnTouchesBegan;
            touchListener.OnTouchesMoved = OnTouchesMoved;

            AddEventListener(touchListener);
        }

        #endregion Setup content


        public override void OnExit()
        {
            base.OnExit();

            CCSimpleAudioEngine.SharedEngine.End();
        }

        public void MenuCallback(object sender)
        {
            // get the userdata, it's the index of the menu item clicked
            CCMenuItem menuItem = (CCMenuItem)(sender);
            int nIdx = menuItem.ZOrder - 10000;

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
                    if(CCSimpleAudioEngine.SharedEngine.BackgroundMusicPlaying)
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
                    soundId = CCSimpleAudioEngine.SharedEngine.PlayEffect(CCFileUtils.FullPathFromRelativePath(EFFECT_FILE));
                    break;
                // play effect
                case 7:
                    soundId = CCSimpleAudioEngine.SharedEngine.PlayEffect(CCFileUtils.FullPathFromRelativePath(EFFECT_FILE), true);
                    break;
                // stop effect
                case 8:
                    CCSimpleAudioEngine.SharedEngine.StopEffect(soundId);
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

        #region Event handling

        void OnTouchesBegan(List<CCTouch> touches, CCEvent touchEvent)
        {
            CCTouch touch = touches.FirstOrDefault();

            var beginPos = touch.LocationOnScreen;
        }

        void OnTouchesMoved(List<CCTouch> touches, CCEvent touchEvent)
        {
            CCTouch touch = touches.FirstOrDefault();

            CCPoint touchLocation = Layer.ScreenToWorldspace(touch.LocationOnScreen);   
            float nMoveY = touchLocation.Y - beginPos.Y;

            CCPoint curPos  = testMenu.Position;
            CCPoint nextPos = new CCPoint(curPos.X, curPos.Y + nMoveY);
            CCSize winSize = Layer.VisibleBoundsWorldspace.Size;

            if (nextPos.Y < 0.0f)
            {
                testMenu.Position = new CCPoint(0,0);
                return;
            }

            int testCount = testMenuItems.Count();

            if (nextPos.Y > ((testCount + 1)* LINE_SPACE - winSize.Height))
            {
                testMenu.Position = new CCPoint(0, ((testCount + 1)* LINE_SPACE - winSize.Height));
                return;
            }

            testMenu.Position = nextPos;
            beginPos = touchLocation;
        }

        #endregion Event handling
    }


    public class CocosDenshionTestScene : TestScene
    {
        public override void runThisTest()
        {
            CCLayer layer = new CocosDenshionTest();
            AddChild(layer);

            Director.ReplaceScene(this);
        }
    }
}