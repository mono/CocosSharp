using System;
using CocosSharp;
using Microsoft.Xna.Framework;

namespace tests
{
    public class ParallaxDemo : CCLayer
    {
        protected CCTextureAtlas m_atlas;
        public const int kTagTileMap = 1;

        private const string s_pPathB1 = "Images/b1";
        private const string s_pPathB2 = "Images/b2";
        private const string s_pPathF1 = "Images/f1";
        private const string s_pPathF2 = "Images/f2";
        private const string s_pPathR1 = "Images/r1";
        private const string s_pPathR2 = "Images/r2";

        private CCGamePadButtonDelegate _GamePadButtonDelegate;
        private CCGamePadDPadDelegate _GamePadDPadDelegate;
        private CCGamePadStickUpdateDelegate _GamePadStickDelegate;
        private CCGamePadTriggerDelegate _GamePadTriggerDelegate;

        public ParallaxDemo()
        {
            _GamePadDPadDelegate = new CCGamePadDPadDelegate(MyOnGamePadDPadUpdate);
            _GamePadButtonDelegate = new CCGamePadButtonDelegate(MyOnGamePadButtonUpdate);
            _GamePadStickDelegate = new CCGamePadStickUpdateDelegate(MyOnGameStickUpdate);
            _GamePadTriggerDelegate = new CCGamePadTriggerDelegate(MyGamePadTriggerUpdate);
        }

        #region GamePad Support
        private bool _aButtonWasPressed = false;
        private bool _yButtonWasPressed = false;
        private bool _xButtonWasPressed = false;

        private void MyOnGamePadButtonUpdate(CCGamePadButtonStatus backButton, CCGamePadButtonStatus startButton, CCGamePadButtonStatus systemButton, CCGamePadButtonStatus aButton, CCGamePadButtonStatus bButton, CCGamePadButtonStatus xButton, CCGamePadButtonStatus yButton, CCGamePadButtonStatus leftShoulder, CCGamePadButtonStatus rightShoulder, Microsoft.Xna.Framework.PlayerIndex player)
        {
            if (aButton == CCGamePadButtonStatus.Pressed)
            {
                _aButtonWasPressed = true;
            }
            else if (aButton == CCGamePadButtonStatus.Released && _aButtonWasPressed)
            {
                // Select the menu
                restartCallback(null);
            }

            if (yButton == CCGamePadButtonStatus.Pressed)
            {
                _yButtonWasPressed = true;
            }
            else if (yButton == CCGamePadButtonStatus.Released && _yButtonWasPressed)
            {
                CCNode node = GetChildByTag(kTagTileMap);
                node.RunAction(new CCRotateBy (1f, 15f));
            }

            if (xButton == CCGamePadButtonStatus.Pressed)
            {
                _xButtonWasPressed = true;
            }
            else if (xButton == CCGamePadButtonStatus.Released && _xButtonWasPressed)
            {
                CCNode node = GetChildByTag(kTagTileMap);
                if (node != null)
                {
                    node.RunAction(new CCRotateBy (1f, -15f));
                }
            }
        }

        private long _FirstTicks;
        private bool _bDownPress = false;
        private bool _bUpPress = false;

        private void MyOnGamePadDPadUpdate(CCGamePadButtonStatus leftButton, CCGamePadButtonStatus upButton, CCGamePadButtonStatus rightButton, CCGamePadButtonStatus downButton, Microsoft.Xna.Framework.PlayerIndex player)
        {
            // Down and Up only
            if (rightButton == CCGamePadButtonStatus.Pressed)
            {
                if (_FirstTicks == 0L)
                {
                    _FirstTicks = DateTime.Now.Ticks;
                    _bDownPress = true;
                }
            }
            else if (rightButton == CCGamePadButtonStatus.Released && _FirstTicks > 0L && _bDownPress)
            {
                _FirstTicks = 0L;
                nextCallback(null);
                _bDownPress = false;
            }
            if (leftButton == CCGamePadButtonStatus.Pressed)
            {
                if (_FirstTicks == 0L)
                {
                    _FirstTicks = DateTime.Now.Ticks;
                    _bUpPress = true;
                }
            }
            else if (leftButton == CCGamePadButtonStatus.Released && _FirstTicks > 0L && _bUpPress)
            {
                _FirstTicks = 0L;
                backCallback(null);
                _bUpPress = false;
            }
        }

        private void MyGamePadTriggerUpdate(float leftTriggerStrength, float rightTriggerStrength, PlayerIndex player)
        {
            CCNode node = GetChildByTag(kTagTileMap);
            if (node != null)
            {
                node.Rotation += rightTriggerStrength * CCMacros.CCDegreesToRadians(15f) - leftTriggerStrength * CCMacros.CCDegreesToRadians(15f);
            }
        }
        private void MyOnGameStickUpdate(CCGameStickStatus left, CCGameStickStatus right, PlayerIndex player)
        {
            CCNode node = GetChildByTag(kTagTileMap);
            if (node != null)
            {
                CCParallaxNode map = (CCParallaxNode)node;
                if (left.Magnitude > 0f)
                {
                    // use the left stick to move the map
                    CCPoint diff = left.Direction.InvertY * left.Magnitude * 10f;
                    CCPoint currentPos = node.Position;
                    node.Position = currentPos + diff;
                }
                if (right.Magnitude > 0f)
                {
                    float scale = (1f - right.Direction.Y * right.Magnitude);
                    node.Scale += scale;
                    if (node.Scale < 1f)
                    {
                        node.Scale = 1f;
                    }
                }
            }
        }
        #endregion

        public virtual string title()
        {
            return "No title";
        }

        public override void OnEnter()
        {
            base.OnEnter();

            CCSize s = CCDirector.SharedDirector.WinSize;

            CCLabelTtf label = new CCLabelTtf(title(), "arial", 28);
            AddChild(label, 1);
            label.Position = new CCPoint(s.Width / 2, s.Height - 50);

            CCMenuItemImage item1 = new CCMenuItemImage(s_pPathB1, s_pPathB2, backCallback);
            CCMenuItemImage item2 = new CCMenuItemImage(s_pPathR1, s_pPathR2, restartCallback);
            CCMenuItemImage item3 = new CCMenuItemImage(s_pPathF1, s_pPathF2, nextCallback);

            CCMenu menu = new CCMenu(item1, item2, item3);

            menu.Position = new CCPoint(0, 0);
            item1.Position = new CCPoint(s.Width / 2 - 100, 30);
            item2.Position = new CCPoint(s.Width / 2, 30);
            item3.Position = new CCPoint(s.Width / 2 + 100, 30);

            AddChild(menu, 1);
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public void restartCallback(object pSender)
        {
            CCScene s = new ParallaxTestScene();
            s.AddChild(ParallaxTestScene.restartParallaxAction());

            CCDirector.SharedDirector.ReplaceScene(s);
        }

        public void nextCallback(object pSender)
        {
            CCScene s = new ParallaxTestScene();
            s.AddChild(ParallaxTestScene.nextParallaxAction());
            CCDirector.SharedDirector.ReplaceScene(s);
        }

        public void backCallback(object pSender)
        {
            CCScene s = new ParallaxTestScene();
            s.AddChild(ParallaxTestScene.backParallaxAction());
            CCDirector.SharedDirector.ReplaceScene(s);
        }
    }
}