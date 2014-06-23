using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;
using Random = CocosSharp.CCRandom;

namespace tests
{
    public class SpriteBatchNodeNewTexture : SpriteTestDemo
    {
        CCTexture2D texture1;
        CCTexture2D texture2;


        #region Properties

        public override string Title
        {
            get { return "SpriteBatchNode new texture (tap)"; }
        }

        #endregion Properties


        #region Constructors

        public SpriteBatchNodeNewTexture()
        {
            CCSpriteBatchNode batch = new CCSpriteBatchNode("Images/grossini_dance_atlas", 50);
            AddChild(batch, 0, (int)kTags.kTagSpriteBatchNode);

            texture1 = batch.Texture;
            texture2 = CCApplication.SharedApplication.TextureCache.AddImage("Images/grossini_dance_atlas-mono");
        }

        #endregion Constructors


        #region Setup content

        protected override void RunningOnNewWindow(CCSize windowSize)
        {
            base.RunningOnNewWindow(windowSize);

            // Register Touch Event
            var touchListener = new CCEventListenerTouchAllAtOnce();
            touchListener.OnTouchesEnded = OnTouchesEnded;

            EventDispatcher.AddEventListener(touchListener, this);

            for (int i = 0; i < 30; i++)
                AddNewSprite();
        }

        #endregion Setup content


        void AddNewSprite()
        {
            CCSize s = Director.WindowSizeInPoints;

            CCPoint p = new CCPoint((float)(CCRandom.NextDouble() * s.Width), (float)(CCRandom.NextDouble() * s.Height));

            CCSpriteBatchNode batch = (CCSpriteBatchNode)GetChildByTag((int)kTags.kTagSpriteBatchNode);

            int idx = (int)(CCRandom.NextDouble() * 1400 / 100);
            int x = (idx % 5) * 85;
            int y = (idx / 5) * 121;


            CCSprite sprite = new CCSprite(batch.Texture, new CCRect(x, y, 85, 121));
            batch.AddChild(sprite);

            sprite.Position = (new CCPoint(p.X, p.Y));

            CCActionInterval action;
            float random = (float)CCRandom.NextDouble();

            if (random < 0.20)
                action = new CCScaleBy(3, 2);
            else if (random < 0.40)
                action = new CCRotateBy (3, 360);
            else if (random < 0.60)
                action = new CCBlink (1, 3);
            else if (random < 0.8)
                action = new CCTintBy (2, 0, -255, -255);
            else
                action = new CCFadeOut  (2);
            CCActionInterval action_back = (CCActionInterval)action.Reverse();
            CCActionInterval seq = (CCActionInterval)(new CCSequence(action, action_back));

            sprite.RunAction(new CCRepeatForever (seq));
        }

        void OnTouchesEnded(List<CCTouch> touches, CCEvent touchEvent)
        {
            CCSpriteBatchNode batch = (CCSpriteBatchNode)GetChildByTag((int)kTags. kTagSpriteBatchNode);

            if (batch.Texture == texture1)
                batch.Texture = texture2;
            else
                batch.Texture = texture1;   

        }
    }
}
