using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    /// <summary>
    /// This test exercised code form user Arkiliknam, (c) by "Arkiliknam" 2013. Please see:
    /// https://github.com/mono/CocosSharp/discussions/438585
    /// </summary>
    public class RenderTextureCompositeTest : RenderTextureTestDemo
    {
        private CCAnimate _swingAnimate;
        private CCAnimate _thrustAnimate;
        private CCAnimate _dodgeAnimate;
        private CCAnimate _collapseAnimate;

        private CCAnimate _swingAnimate2;
        private CCAnimate _thrustAnimate2;
        private CCAnimate _dodgeAnimate2;
        private CCAnimate _collapseAnimate2;

        private CCSprite _testSprite;
        private CCSprite _testSprite2;

        public RenderTextureCompositeTest()
        {
            var winSize = CCDirector.SharedDirector.WinSize;
            var characterSpriteFactory = new CharacterSpriteFactory();

            _testSprite = new CCSprite(@"Images\grossini_dance_01");
            _testSprite2 = new CCSprite(@"Images\grossini_dance_02");

            _swingAnimate = characterSpriteFactory.CreateAnimateAction();
            _thrustAnimate = characterSpriteFactory.CreateAnimateAction();
            _dodgeAnimate = characterSpriteFactory.CreateAnimateAction();
            _collapseAnimate = characterSpriteFactory.CreateAnimateAction();

            _swingAnimate2 = characterSpriteFactory.CreateAnimateAction();
            _thrustAnimate2 = characterSpriteFactory.CreateAnimateAction();
            _dodgeAnimate2 = characterSpriteFactory.CreateAnimateAction();
            _collapseAnimate2 = characterSpriteFactory.CreateAnimateAction();

            _testSprite.Position = new CCPoint(winSize.Width / 2 -200, winSize.Height / 2 + 100);
            AddChild(_testSprite);

            _testSprite2.Position = new CCPoint(winSize.Width / 2 + 200, winSize.Height / 2 + 100);
            _testSprite2.FlipX = true;
            AddChild(_testSprite2);

            AnimationLoop();
            AnimationLoop2();
        }

        private void AnimationLoop()
        {
            var seq = new CCSequence(_swingAnimate, _thrustAnimate, _dodgeAnimate, _collapseAnimate, new CCCallFunc(AnimationLoop));
            _testSprite.RunAction(seq);
        }

        private void AnimationLoop2()
        {
            var seq = new CCSequence(_swingAnimate2, _thrustAnimate2, _dodgeAnimate2, _collapseAnimate2, new CCCallFunc(AnimationLoop2));
            _testSprite2.RunAction(seq);
        }

        public override string title()
        {
            return "Compositing Test From CodePlex";
        }

        public override string subtitle()
        {
            return "Should Not Crash";
        }
    }

    public class CharacterSpriteFactory
    {
        private CCSprite CreateCustomisationSprite()
        {
            var sprite = new CCSprite(@"Images\grossini_dance_01");
            sprite.Color = new CCColor3B
            {
                B = (byte)CocosSharp.CCRandom.Next(0, 255),
                G = (byte)CocosSharp.CCRandom.Next(0, 255),
                R = (byte)CocosSharp.CCRandom.Next(0, 255)
            };
            return sprite;
        }

        public CCTexture2D CreateCharacterTexture()
        {
            const int width = 490;
            const int height = 278;

            var centerPoint = new CCPoint(width / 2, height / 2);
            var characterTexture = new CCRenderTexture(width, height);

            characterTexture.BeginWithClear(100, 0, 0, 0);

            var bodySprite = CreateCustomisationSprite();
            bodySprite.Position = centerPoint;
            bodySprite.Visit();

            var armorSprite = CreateCustomisationSprite();
            armorSprite.Position = centerPoint;
            armorSprite.Visit();

            var eyesSprite = CreateCustomisationSprite();
            eyesSprite.Position = centerPoint;
            eyesSprite.Visit();

            var noseSprite = CreateCustomisationSprite();
            noseSprite.Position = centerPoint;
            noseSprite.Visit();

            var hairSprite = CreateCustomisationSprite();
            hairSprite.Position = centerPoint;
            hairSprite.Visit();

            var moustacheSprite = CreateCustomisationSprite();
            moustacheSprite.Position = centerPoint;
            moustacheSprite.Visit();

            var beardSprite = CreateCustomisationSprite();
            beardSprite.Position = centerPoint;
            beardSprite.Visit();

            var helmutSprite = CreateCustomisationSprite();
            helmutSprite.Position = centerPoint;
            helmutSprite.Visit();

            var weaponSprite = CreateCustomisationSprite();
            weaponSprite.Position = centerPoint;
            weaponSprite.Visit();

            characterTexture.End();

            return characterTexture.Sprite.Texture;
        }


        public CCAnimate CreateAnimateAction()
        {
            var frameList = new List<CCSpriteFrame>();

            for (var i = 0; i < 7; i++)
            {
                var texture = CreateCharacterTexture();

                var sprite = new CCSpriteFrame(texture, new CCRect(0, 0, texture.ContentSize.Width, texture.ContentSize.Height));
                frameList.Add(sprite);
            }
            var animation = new CCAnimation(frameList, 0.1f);
            var animate = new CCAnimate (animation);

            return animate;
        }
    }
}
