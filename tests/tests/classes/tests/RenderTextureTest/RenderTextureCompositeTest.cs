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
        CCAnimate swingAnimate;
        CCAnimate thrustAnimate;
        CCAnimate dodgeAnimate;
        CCAnimate collapseAnimate;

        CCAnimate swingAnimate2;
        CCAnimate thrustAnimate2;
        CCAnimate dodgeAnimate2;
        CCAnimate collapseAnimate2;

        CCSprite testSprite;
        CCSprite testSprite2;


        #region Properties

        public override string Title
        {
            get { return "Compositing Test From CodePlex"; }
        }

        public override string Subtitle
        {
            get { return "Should Not Crash"; }
        }

        #endregion Properties


        #region Constructors

        public RenderTextureCompositeTest()
        {
            var characterSpriteFactory = new CharacterSpriteFactory();

            testSprite = new CCSprite(@"Images\grossini_dance_01");
            testSprite2 = new CCSprite(@"Images\grossini_dance_02");

            swingAnimate = characterSpriteFactory.CreateAnimateAction();
            thrustAnimate = characterSpriteFactory.CreateAnimateAction();
            dodgeAnimate = characterSpriteFactory.CreateAnimateAction();
            collapseAnimate = characterSpriteFactory.CreateAnimateAction();

            swingAnimate2 = characterSpriteFactory.CreateAnimateAction();
            thrustAnimate2 = characterSpriteFactory.CreateAnimateAction();
            dodgeAnimate2 = characterSpriteFactory.CreateAnimateAction();
            collapseAnimate2 = characterSpriteFactory.CreateAnimateAction();

            AddChild(testSprite);
            AddChild(testSprite2);
        }

        #endregion Constructors


        #region Setup content

        protected override void RunningOnNewWindow(CCSize windowSize)
        {
            base.RunningOnNewWindow(windowSize);

            testSprite.Position = new CCPoint(windowSize.Width / 2 -200, windowSize.Height / 2 + 100);
            testSprite2.Position = new CCPoint(windowSize.Width / 2 + 200, windowSize.Height / 2 + 100);
            testSprite2.FlipX = true;

            AnimationLoop();
            AnimationLoop2();
        }

        #endregion Setup content


        void AnimationLoop()
        {
            var seq = new CCSequence(swingAnimate, thrustAnimate, dodgeAnimate, collapseAnimate, new CCCallFunc(AnimationLoop));
            testSprite.RunAction(seq);
        }

        void AnimationLoop2()
        {
            var seq = new CCSequence(swingAnimate2, thrustAnimate2, dodgeAnimate2, collapseAnimate2, new CCCallFunc(AnimationLoop2));
            testSprite2.RunAction(seq);
        }
    }


    public class CharacterSpriteFactory
    {
        CCSprite CreateCustomisationSprite()
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
