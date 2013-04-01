using System;
using cocos2d;
using Random = cocos2d.Random;

namespace tests
{
    public class ParticleTestScene : TestScene
    {
        #region eIDClick enum

        public enum eIDClick
        {
            IDC_NEXT = 100,
            IDC_BACK,
            IDC_RESTART,
            IDC_TOGGLE
        };

        #endregion

        public static int kTagLabelAtlas = 1;

        public static int sceneIdx = -1;

        public static int MAX_LAYER = 43;

        public static CCLayer createParticleLayer(int nIndex)
        {
            switch (nIndex)
            {
                case 0:
                    return new ParticleReorder();
                case 1:
                    return new ParticleBatchHybrid();
                case 2:
                    return new ParticleBatchMultipleEmitters();
                case 3:
                    return new DemoFlower();
                case 4:
                    return new DemoGalaxy();
                case 5:
                    return new DemoFirework();
                case 6:
                    return new DemoSpiral();
                case 7:
                    return new DemoSun();
                case 8:
                    return new DemoMeteor();
                case 9:
                    return new DemoFire();
                case 10:
                    return new DemoSmoke();
                case 11:
                    return new DemoExplosion();
                case 12:
                    return new DemoSnow();
                case 13:
                    return new DemoRain();
                case 14:
                    return new DemoBigFlower();
                case 15:
                    return new DemoRotFlower();
                case 16:
                    return new DemoModernArt();
                case 17:
                    return new DemoRing();
                case 18:
                    return new ParallaxParticle();
                case 19:
                    return new DemoParticleFromFile("BoilingFoam");
                case 20:
                    return new DemoParticleFromFile("BurstPipe");
                case 21:
                    return new DemoParticleFromFile("Comet");
                case 22:
                    return new DemoParticleFromFile("debian");
                case 23:
                    return new DemoParticleFromFile("ExplodingRing");
                case 24:
                    return new DemoParticleFromFile("LavaFlow");
                case 25:
                    return new DemoParticleFromFile("SpinningPeas");
                case 26:
                    return new DemoParticleFromFile("SpookyPeas");
                case 27:
                    return new DemoParticleFromFile("Upsidedown");
                case 28:
                    return new DemoParticleFromFile("Flower");
                case 29:
                    return new DemoParticleFromFile("Spiral");
                case 30:
                    return new DemoParticleFromFile("Galaxy");
                case 31:
                    return new DemoParticleFromFile("Phoenix");
                case 32:
                    return new RadiusMode1();
                case 33:
                    return new RadiusMode2();
                case 34:
                    return new Issue704();
                case 35:
                    return new Issue870();
                case 36:
                    return new Issue1201();
                    // v1.1 tests
                case 37:
                    return new MultipleParticleSystems();
                case 38:
                    return new MultipleParticleSystemsBatched();
                case 39:
                    return new AddAndDeleteParticleSystems();
                case 40:
                    return new ReorderParticleSystems();
                case 41:
                    return new PremultipliedAlphaTest();
                case 42:
                    return new PremultipliedAlphaTest2();
            }

            return null;
        }

        protected override void NextTestCase()
        {
            nextParticleAction();
        }
        protected override void PreviousTestCase()
        {
            backParticleAction();
        }
        protected override void RestTestCase()
        {
            restartParticleAction();
        }

        public static CCLayer nextParticleAction()
        {
            sceneIdx++;
            sceneIdx = sceneIdx % MAX_LAYER;

            CCLayer pLayer = createParticleLayer(sceneIdx);

            return pLayer;
        }

        public static CCLayer backParticleAction()
        {
            sceneIdx--;
            int total = MAX_LAYER;
            if (sceneIdx < 0)
                sceneIdx += total;

            CCLayer pLayer = createParticleLayer(sceneIdx);

            return pLayer;
        }

        public static CCLayer restartParticleAction()
        {
            CCLayer pLayer = createParticleLayer(sceneIdx);

            return pLayer;
        }


        public override void runThisTest()
        {
            AddChild(nextParticleAction());

            CCDirector.SharedDirector.ReplaceScene(this);
        }
    };

    public class ParticleDemo : CCLayerColor
    {
        public CCSprite m_background;
        public CCParticleSystem m_emitter;

        public ParticleDemo()
        {
            InitWithColor(CCTypes.CreateColor(127, 127, 127, 255));

            m_emitter = null;

            TouchEnabled = true;

            CCSize s = CCDirector.SharedDirector.WinSize;
            CCLabelTTF label = CCLabelTTF.Create(title(), "arial", 28);
            AddChild(label, 100, 1000);
            label.Position = new CCPoint(s.Width / 2, s.Height - 50);

            CCLabelTTF tapScreen = CCLabelTTF.Create(subtitle(), "arial", 20);
            tapScreen.Position = new CCPoint(s.Width / 2, s.Height - 80);
            AddChild(tapScreen, 100);

            CCMenuItemImage item1 = CCMenuItemImage.Create(TestResource.s_pPathB1, TestResource.s_pPathB2, backCallback);
            CCMenuItemImage item2 = CCMenuItemImage.Create(TestResource.s_pPathR1, TestResource.s_pPathR2, restartCallback);
            CCMenuItemImage item3 = CCMenuItemImage.Create(TestResource.s_pPathF1, TestResource.s_pPathF2, nextCallback);

            CCMenuItemToggle item4 = CCMenuItemToggle.Create(toggleCallback,
                                                                     CCMenuItemFont.Create("Free Movement"),
                                                                     CCMenuItemFont.Create("Relative Movement"),
                                                                     CCMenuItemFont.Create("Grouped Movement"));

            CCMenu menu = CCMenu.Create(item1, item2, item3, item4);

            menu.Position = new CCPoint(0, 0);
            item1.Position = new CCPoint(s.Width / 2 - 100, 30);
            item2.Position = new CCPoint(s.Width / 2, 30);
            item3.Position = new CCPoint(s.Width / 2 + 100, 30);
            item4.Position = new CCPoint(0, 100);
            item4.AnchorPoint = new CCPoint(0, 0);

            AddChild(menu, 100);

            CCLabelAtlas labelAtlas;
            try
            {
                labelAtlas = CCLabelAtlas.Create("0000", "Images/fps_Images", 16, 24, '.');
            }
            catch (Exception)
            {
                labelAtlas = CCLabelAtlas.Create("0000", "Images/fps_Images", 16, 24, '.');
            }
            AddChild(labelAtlas, 100, ParticleTestScene.kTagLabelAtlas);
            labelAtlas.Position = new CCPoint(s.Width - 66, 50);

            // moving background
            m_background = CCSprite.Create(TestResource.s_back3);
            AddChild(m_background, 5);
            m_background.Position = new CCPoint(s.Width / 2, s.Height - 180);

            CCActionInterval move = new CCMoveBy (4, new CCPoint(300, 0));
            CCFiniteTimeAction move_back = move.Reverse();
            CCFiniteTimeAction seq = CCSequence.Create(move, move_back);
            m_background.RunAction(CCRepeatForever.Create((CCActionInterval) seq));

            Schedule(step);
        }

        public virtual string subtitle()
        {
            return String.Empty;
        }

        ~ParticleDemo()
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();

            var pLabel = (CCLabelTTF) (GetChildByTag(1000));
            pLabel.Label = (title());
        }

        public virtual string title()
        {
            return "No title";
        }

        public void restartCallback(object pSender)
        {
            if (m_emitter != null)
            {
                m_emitter.ResetSystem();
            }
        }

        public void nextCallback(object pSender)
        {
            var s = new ParticleTestScene();
            s.AddChild(ParticleTestScene.nextParticleAction());
            CCDirector.SharedDirector.ReplaceScene(s);
        }

        public void backCallback(object pSender)
        {
            var s = new ParticleTestScene();
            s.AddChild(ParticleTestScene.backParticleAction());
            CCDirector.SharedDirector.ReplaceScene(s);
        }

        public void toggleCallback(object pSender)
        {
            if (m_emitter != null)
            {
                if (m_emitter.PositionType == CCPositionType.kCCPositionTypeGrouped)
                    m_emitter.PositionType = CCPositionType.kCCPositionTypeFree;
                else if (m_emitter.PositionType == CCPositionType.kCCPositionTypeFree)
                    m_emitter.PositionType = CCPositionType.kCCPositionTypeRelative;
                else if (m_emitter.PositionType == CCPositionType.kCCPositionTypeRelative)
                    m_emitter.PositionType = CCPositionType.kCCPositionTypeGrouped;
            }
        }

        public override void RegisterWithTouchDispatcher()
        {
            CCDirector.SharedDirector.TouchDispatcher.AddTargetedDelegate(this, 0, false);
        }

        public override bool TouchBegan(CCTouch touch, CCEvent eve)
        {
            return true;
        }

        public override void TouchMoved(CCTouch touch, CCEvent eve)
        {
            TouchEnded(touch, eve);
        }

        public override void TouchEnded(CCTouch touch, CCEvent eve)
        {
            CCPoint location = touch.LocationInView;
            CCPoint convertedLocation = CCDirector.SharedDirector.ConvertToGl(location);

            var pos = new CCPoint(0, 0);
            if (m_background != null)
            {
                pos = m_background.ConvertToWorldSpace(new CCPoint(0, 0));
            }

            if (m_emitter != null)
            {
                m_emitter.Position = CCPointExtension.Subtract(convertedLocation, pos);
            }
        }

        public void step(float dt)
        {
            var atlas = (CCLabelAtlas) GetChildByTag(ParticleTestScene.kTagLabelAtlas);

            if (m_emitter != null)
            {
                string str = string.Format("{0:0000}", m_emitter.ParticleCount);
                atlas.Label = (str);
            }
            else
            {
                int count = 0;
                for (int i = 0; i < m_pChildren.Count; i++)
                {
                    if (m_pChildren[i] is CCParticleSystem)
                    {
                        count += ((CCParticleSystem) m_pChildren[i]).ParticleCount;
                    }
                    else if (m_pChildren[i] is CCParticleBatchNode)
                    {
                        var bn = (CCParticleBatchNode) m_pChildren[i];
                        for (int j = 0; j < bn.ChildrenCount; j++)
                        {
                            if (bn.Children[j] is CCParticleSystem)
                            {
                                count += ((CCParticleSystem) bn.Children[j]).ParticleCount;
                            }
                        }
                    }
                }
                string str = string.Format("{0:0000}", count);
                atlas.Label = (str);
            }
        }

        public void setEmitterPosition()
        {
            if (m_emitter != null)
            {
                CCSize s = CCDirector.SharedDirector.WinSize;

                m_emitter.Position = new CCPoint(s.Width / 2, s.Height / 2 - 30);
            }
        }
    };

    //------------------------------------------------------------------
    //
    // DemoFirework
    //
    //------------------------------------------------------------------
    public class DemoFirework : ParticleDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();

            m_emitter = CCParticleFireworks.Create();
            m_background.AddChild(m_emitter, 10);

            m_emitter.Texture = CCTextureCache.SharedTextureCache.AddImage(TestResource.s_stars1);

            setEmitterPosition();
        }

        public override string title()
        {
            return "ParticleFireworks";
        }
    };

    //------------------------------------------------------------------
    //
    // DemoFire
    //
    //------------------------------------------------------------------
    public class DemoFire : ParticleDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();

            m_emitter = CCParticleFire.Create();
            m_background.AddChild(m_emitter, 10);

            m_emitter.Texture = CCTextureCache.SharedTextureCache.AddImage(TestResource.s_fire); //.pvr"];
            CCPoint p = m_emitter.Position;
            m_emitter.Position = new CCPoint(p.X, 100);

            setEmitterPosition();
        }

        public override string title()
        {
            return "ParticleFire";
        }
    };

    //------------------------------------------------------------------
    //
    // DemoSun
    //
    //------------------------------------------------------------------
    public class DemoSun : ParticleDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();

            m_emitter = CCParticleSun.Create();
            m_background.AddChild(m_emitter, 10);

            m_emitter.Texture = CCTextureCache.SharedTextureCache.AddImage(TestResource.s_fire);

            setEmitterPosition();
        }

        public override string title()
        {
            return "ParticleSun";
        }
    };

    //------------------------------------------------------------------
    //
    // DemoGalaxy
    //
    //------------------------------------------------------------------
    public class DemoGalaxy : ParticleDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();

            m_emitter = CCParticleGalaxy.Create();
            m_background.AddChild(m_emitter, 10);

            m_emitter.Texture = CCTextureCache.SharedTextureCache.AddImage(TestResource.s_fire);

            setEmitterPosition();
        }

        public override string title()
        {
            return "ParticleGalaxy";
        }
    };

    //------------------------------------------------------------------
    //
    // DemoFlower
    //
    //------------------------------------------------------------------
    public class DemoFlower : ParticleDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();

            m_emitter = CCParticleFlower.Create();
            m_background.AddChild(m_emitter, 10);
            m_emitter.Texture = CCTextureCache.SharedTextureCache.AddImage(TestResource.s_stars1);

            setEmitterPosition();
        }

        public override string title()
        {
            return "ParticleFlower";
        }
    };

    //------------------------------------------------------------------
    //
    // DemoBigFlower
    //
    //------------------------------------------------------------------
    public class DemoBigFlower : ParticleDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();

            m_emitter = new CCParticleSystemQuad();
            m_emitter.InitWithTotalParticles(50);
            //m_emitter.autorelease();

            m_background.AddChild(m_emitter, 10);
            ////m_emitter.release();	// win32 :  use this line or remove this line and use autorelease()
            m_emitter.Texture = CCTextureCache.SharedTextureCache.AddImage(TestResource.s_stars1);

            m_emitter.Duration = -1;

            // gravity
            m_emitter.Gravity = (new CCPoint(0, 0));

            // angle
            m_emitter.Angle = 90;
            m_emitter.AngleVar = 360;

            // speed of particles
            m_emitter.Speed = (160);
            m_emitter.SpeedVar = (20);

            // radial
            m_emitter.RadialAccel = (-120);
            m_emitter.RadialAccelVar = (0);

            // tagential
            m_emitter.TangentialAccel = (30);
            m_emitter.TangentialAccelVar = (0);

            // emitter position
            m_emitter.Position = new CCPoint(160, 240);
            m_emitter.PosVar = new CCPoint(0, 0);

            // life of particles
            m_emitter.Life = 4;
            m_emitter.LifeVar = 1;

            // spin of particles
            m_emitter.StartSpin = 0;
            m_emitter.StartSizeVar = 0;
            m_emitter.EndSpin = 0;
            m_emitter.EndSpinVar = 0;

            // color of particles
            var startColor = new CCColor4F(0.5f, 0.5f, 0.5f, 1.0f);
            m_emitter.StartColor = startColor;

            var startColorVar = new CCColor4F(0.5f, 0.5f, 0.5f, 1.0f);
            m_emitter.StartColorVar = startColorVar;

            var endColor = new CCColor4F(0.1f, 0.1f, 0.1f, 0.2f);
            m_emitter.EndColor = endColor;

            var endColorVar = new CCColor4F(0.1f, 0.1f, 0.1f, 0.2f);
            m_emitter.EndColorVar = endColorVar;

            // size, in pixels
            m_emitter.StartSize = 80.0f;
            m_emitter.StartSizeVar = 40.0f;
            m_emitter.EndSize = CCParticleSystem.kParticleStartSizeEqualToEndSize;

            // emits per second
            m_emitter.EmissionRate = m_emitter.TotalParticles / m_emitter.Life;

            // additive
            m_emitter.BlendAdditive = true;

            setEmitterPosition();
        }

        public override string title()
        {
            return "ParticleBigFlower";
        }
    };

    //------------------------------------------------------------------
    //
    // DemoRotFlower
    //
    //------------------------------------------------------------------
    public class DemoRotFlower : ParticleDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();

            m_emitter = new CCParticleSystemQuad();
            m_emitter.InitWithTotalParticles(300);
            //m_emitter.autorelease();

            m_background.AddChild(m_emitter, 10);
            ////m_emitter.release();	// win32 : Remove this line
            m_emitter.Texture = CCTextureCache.SharedTextureCache.AddImage(TestResource.s_stars2);

            // duration
            m_emitter.Duration = -1;

            // gravity
            m_emitter.Gravity = (new CCPoint(0, 0));

            // angle
            m_emitter.Angle = 90;
            m_emitter.AngleVar = 360;

            // speed of particles
            m_emitter.Speed = (160);
            m_emitter.SpeedVar = (20);

            // radial
            m_emitter.RadialAccel = (-120);
            m_emitter.RadialAccelVar = (0);

            // tagential
            m_emitter.TangentialAccel = (30);
            m_emitter.TangentialAccelVar = (0);

            // emitter position
            m_emitter.Position = new CCPoint(160, 240);
            m_emitter.PosVar = new CCPoint(0, 0);

            // life of particles
            m_emitter.Life = 3;
            m_emitter.LifeVar = 1;

            // spin of particles
            m_emitter.StartSpin = 0;
            m_emitter.StartSpinVar = 0;
            m_emitter.EndSpin = 0;
            m_emitter.EndSpinVar = 2000;

            // color of particles
            var startColor = new CCColor4F(0.5f, 0.5f, 0.5f, 1.0f);
            m_emitter.StartColor = startColor;

            var startColorVar = new CCColor4F(0.5f, 0.5f, 0.5f, 1.0f);
            m_emitter.StartColorVar = startColorVar;

            var endColor = new CCColor4F(0.1f, 0.1f, 0.1f, 0.2f);
            m_emitter.EndColor = endColor;

            var endColorVar = new CCColor4F(0.1f, 0.1f, 0.1f, 0.2f);
            m_emitter.EndColorVar = endColorVar;

            // size, in pixels
            m_emitter.StartSize = 30.0f;
            m_emitter.StartSizeVar = 00.0f;
            m_emitter.EndSize = CCParticleSystem.kParticleStartSizeEqualToEndSize;

            // emits per second
            m_emitter.EmissionRate = m_emitter.TotalParticles / m_emitter.Life;

            // additive
            m_emitter.BlendAdditive = false;

            setEmitterPosition();
        }

        public override string title()
        {
            return "ParticleRotFlower";
        }
    };

    public class DemoMeteor : ParticleDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();

            m_emitter = CCParticleMeteor.Create();

            m_background.AddChild(m_emitter, 10);

            m_emitter.Texture = CCTextureCache.SharedTextureCache.AddImage(TestResource.s_fire);

            setEmitterPosition();
        }

        public override string title()
        {
            return "ParticleMeteor";
        }
    };

    public class DemoSpiral : ParticleDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();

            m_emitter = CCParticleSpiral.Create();

            m_background.AddChild(m_emitter, 10);

            m_emitter.Texture = CCTextureCache.SharedTextureCache.AddImage(TestResource.s_fire);

            setEmitterPosition();
        }

        public override string title()
        {
            return "ParticleSpiral";
        }
    };

    public class DemoExplosion : ParticleDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();

            m_emitter = CCParticleExplosion.Create();

            m_background.AddChild(m_emitter, 10);

            m_emitter.Texture = CCTextureCache.SharedTextureCache.AddImage(TestResource.s_stars1);

            m_emitter.AutoRemoveOnFinish = true;

            setEmitterPosition();
        }

        public override string title()
        {
            return "ParticleExplosion";
        }
    };

    public class DemoSmoke : ParticleDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();

            m_emitter = CCParticleSmoke.Create();

            m_background.AddChild(m_emitter, 10);
            m_emitter.Texture = CCTextureCache.SharedTextureCache.AddImage(TestResource.s_fire);

            CCPoint p = m_emitter.Position;
            m_emitter.Position = new CCPoint(p.X, 100);

            setEmitterPosition();
        }

        public override string title()
        {
            return "ParticleSmoke";
        }
    };

    public class DemoSnow : ParticleDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();

            m_emitter = CCParticleSnow.Create();

            m_background.AddChild(m_emitter, 10);

            CCPoint p = m_emitter.Position;
            m_emitter.Position = new CCPoint(p.X, p.Y - 110);
            m_emitter.Life = 3;
            m_emitter.LifeVar = 1;

            // gravity
            m_emitter.Gravity = (new CCPoint(0, -10));

            // speed of particles
            m_emitter.Speed = (130);
            m_emitter.SpeedVar = (30);

            var startColor = m_emitter.StartColor;
            startColor.R = 0.9f;
            startColor.G = 0.9f;
            startColor.B = 0.9f;
            m_emitter.StartColor = startColor;

            var startColorVar = m_emitter.StartColorVar;
            startColorVar.B = 0.1f;
            m_emitter.StartColorVar = startColorVar;

            m_emitter.EmissionRate = m_emitter.TotalParticles / m_emitter.Life;

            m_emitter.Texture = CCTextureCache.SharedTextureCache.AddImage(TestResource.s_snow);

            setEmitterPosition();
        }

        public override string title()
        {
            return "ParticleSnow";
        }
    };

    public class DemoRain : ParticleDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();

            m_emitter = CCParticleRain.Create();

            m_background.AddChild(m_emitter, 10);

            CCPoint p = m_emitter.Position;
            m_emitter.Position = new CCPoint(p.X, p.Y - 100);
            m_emitter.Life = 4;

            m_emitter.Texture = CCTextureCache.SharedTextureCache.AddImage(TestResource.s_fire);

            setEmitterPosition();
        }

        public override string title()
        {
            return "ParticleRain";
        }
    };

    // todo: CCParticleSystemPoint::draw() hasn't been implemented.
    public class DemoModernArt : ParticleDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();

            m_emitter = new CCParticleSystemQuad();
            m_emitter.InitWithTotalParticles(1000);
            //m_emitter.autorelease();

            m_background.AddChild(m_emitter, 10);
            ////m_emitter.release();

            CCSize s = CCDirector.SharedDirector.WinSize;

            // duration
            m_emitter.Duration = -1;

            // gravity
            m_emitter.Gravity = (new CCPoint(0, 0));

            // angle
            m_emitter.Angle = 0;
            m_emitter.AngleVar = 360;

            // radial
            m_emitter.RadialAccel = (70);
            m_emitter.RadialAccelVar = (10);

            // tagential
            m_emitter.TangentialAccel = (80);
            m_emitter.TangentialAccelVar = (0);

            // speed of particles
            m_emitter.Speed = (50);
            m_emitter.SpeedVar = (10);

            // emitter position
            m_emitter.Position = new CCPoint(s.Width / 2, s.Height / 2);
            m_emitter.PosVar = new CCPoint(0, 0);

            // life of particles
            m_emitter.Life = 2.0f;
            m_emitter.LifeVar = 0.3f;

            // emits per frame
            m_emitter.EmissionRate = m_emitter.TotalParticles / m_emitter.Life;

            // color of particles
            var startColor = new CCColor4F(0.5f, 0.5f, 0.5f, 1.0f);
            m_emitter.StartColor = startColor;

            var startColorVar = new CCColor4F(0.5f, 0.5f, 0.5f, 1.0f);
            m_emitter.StartColorVar = startColorVar;

            var endColor = new CCColor4F(0.1f, 0.1f, 0.1f, 0.2f);
            m_emitter.EndColor = endColor;

            var endColorVar = new CCColor4F(0.1f, 0.1f, 0.1f, 0.2f);
            m_emitter.EndColorVar = endColorVar;

            // size, in pixels
            m_emitter.StartSize = 1.0f;
            m_emitter.StartSizeVar = 1.0f;
            m_emitter.EndSize = 32.0f;
            m_emitter.EndSizeVar = 8.0f;

            // texture
            m_emitter.Texture = CCTextureCache.SharedTextureCache.AddImage(TestResource.s_fire);

            // additive
            m_emitter.BlendAdditive = false;

            setEmitterPosition();
        }

        public override string title()
        {
            return "Varying size";
        }
    };

    public class DemoRing : ParticleDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();

            m_emitter = CCParticleFlower.Create();


            m_background.AddChild(m_emitter, 10);

            m_emitter.Texture = CCTextureCache.SharedTextureCache.AddImage(TestResource.s_stars1);
            m_emitter.LifeVar = 0;
            m_emitter.Life = 10;
            m_emitter.Speed = (100);
            m_emitter.SpeedVar = (0);
            m_emitter.EmissionRate = 10000;

            setEmitterPosition();
        }

        public override string title()
        {
            return "Ring Demo";
        }
    };

    public class ParallaxParticle : ParticleDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();

            m_background.Parent.RemoveChild(m_background, true);
            m_background = null;

            CCParallaxNode p = CCParallaxNode.Create();
            AddChild(p, 5);

            CCSprite p1 = CCSprite.Create(TestResource.s_back3);
            CCSprite p2 = CCSprite.Create(TestResource.s_back3);

            p.AddChild(p1, 1, new CCPoint(0.5f, 1), new CCPoint(0, 250));
            p.AddChild(p2, 2, new CCPoint(1.5f, 1), new CCPoint(0, 50));

            m_emitter = CCParticleFlower.Create();

            m_emitter.Texture = CCTextureCache.SharedTextureCache.AddImage(TestResource.s_fire);

            p1.AddChild(m_emitter, 10);
            m_emitter.Position = new CCPoint(250, 200);

            CCParticleSun par = CCParticleSun.Create();
            p2.AddChild(par, 10);
            par.Texture = CCTextureCache.SharedTextureCache.AddImage(TestResource.s_fire);

            CCActionInterval move = new CCMoveBy (4, new CCPoint(300, 0));
            CCFiniteTimeAction move_back = move.Reverse();
            CCFiniteTimeAction seq = CCSequence.Create(move, move_back);
            p.RunAction(CCRepeatForever.Create((CCActionInterval) seq));
        }

        public override string title()
        {
            return "Parallax + Particles";
        }
    };

    public class DemoParticleFromFile : ParticleDemo
    {
        private readonly string m_title;

        public DemoParticleFromFile()
        {
        }

        public DemoParticleFromFile(string file)
        {
            m_title = file;
        }

        public override void OnEnter()
        {
            base.OnEnter();

            Color = new CCColor3B(0, 0, 0);
            RemoveChild(m_background, true);
            m_background = null;

            m_emitter = new CCParticleSystemQuad();

            string filename = "Particles/" + m_title;
            m_emitter.InitWithFile(filename);
            AddChild(m_emitter, 10);

            m_emitter.BlendAdditive = true;

            setEmitterPosition();
        }

        public override string title()
        {
            if (null != m_title)
            {
                return m_title;
            }
            else
            {
                return "ParticleFromFile";
            }
        }
    };

    public class RadiusMode1 : ParticleDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();

            Color = new CCColor3B(0, 0, 0);
            RemoveChild(m_background, true);
            m_background = null;

            m_emitter = new CCParticleSystemQuad();
            m_emitter.InitWithTotalParticles(200);
            AddChild(m_emitter, 10);
            m_emitter.Texture = CCTextureCache.SharedTextureCache.AddImage("Images/stars-grayscale");

            // duration
            m_emitter.Duration = CCParticleSystem.kCCParticleDurationInfinity;

            // radius mode
            m_emitter.EmitterMode = CCEmitterMode.kCCParticleModeRadius;

            // radius mode: start and end radius in pixels
            m_emitter.StartRadius = (0);
            m_emitter.StartRadiusVar = (0);
            m_emitter.EndRadius = (160);
            m_emitter.EndRadiusVar = (0);

            // radius mode: degrees per second
            m_emitter.RotatePerSecond = (180);
            m_emitter.RotatePerSecondVar = (0);


            // angle
            m_emitter.Angle = 90;
            m_emitter.AngleVar = 0;

            // emitter position
            CCSize size = CCDirector.SharedDirector.WinSize;
            m_emitter.Position = new CCPoint(size.Width / 2, size.Height / 2);
            m_emitter.PosVar = new CCPoint(0, 0);

            // life of particles
            m_emitter.Life = 5;
            m_emitter.LifeVar = 0;

            // spin of particles
            m_emitter.StartSpin = 0;
            m_emitter.StartSpinVar = 0;
            m_emitter.EndSpin = 0;
            m_emitter.EndSpinVar = 0;

            // color of particles
            var startColor = new CCColor4F(0.5f, 0.5f, 0.5f, 1.0f);
            m_emitter.StartColor = startColor;

            var startColorVar = new CCColor4F(0.5f, 0.5f, 0.5f, 1.0f);
            m_emitter.StartColorVar = startColorVar;

            var endColor = new CCColor4F(0.1f, 0.1f, 0.1f, 0.2f);
            m_emitter.EndColor = endColor;

            var endColorVar = new CCColor4F(0.1f, 0.1f, 0.1f, 0.2f);
            m_emitter.EndColorVar = endColorVar;

            // size, in pixels
            m_emitter.StartSize = 32;
            m_emitter.StartSizeVar = 0;
            m_emitter.EndSize = CCParticleSystem.kCCParticleStartSizeEqualToEndSize;

            // emits per second
            m_emitter.EmissionRate = m_emitter.TotalParticles / m_emitter.Life;

            // additive
            m_emitter.BlendAdditive = false;
        }

        public override string title()
        {
            return "Radius Mode: Spiral";
        }
    };

    public class RadiusMode2 : ParticleDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();

            Color = new CCColor3B(0, 0, 0);
            RemoveChild(m_background, true);
            m_background = null;

            m_emitter = new CCParticleSystemQuad();
            m_emitter.InitWithTotalParticles(200);
            AddChild(m_emitter, 10);
            m_emitter.Texture = CCTextureCache.SharedTextureCache.AddImage("Images/stars-grayscale");

            // duration
            m_emitter.Duration = CCParticleSystem.kCCParticleDurationInfinity;

            // radius mode
            m_emitter.EmitterMode = CCEmitterMode.kCCParticleModeRadius;

            // radius mode: start and end radius in pixels
            m_emitter.StartRadius = (100);
            m_emitter.StartRadiusVar = (0);
            m_emitter.EndRadius = (CCParticleSystem.kCCParticleStartRadiusEqualToEndRadius);
            m_emitter.EndRadiusVar = (0);

            // radius mode: degrees per second
            m_emitter.RotatePerSecond = (45);
            m_emitter.RotatePerSecondVar = (0);


            // angle
            m_emitter.Angle = 90;
            m_emitter.AngleVar = 0;

            // emitter position
            CCSize size = CCDirector.SharedDirector.WinSize;
            m_emitter.Position = new CCPoint(size.Width / 2, size.Height / 2);
            m_emitter.PosVar = new CCPoint(0, 0);

            // life of particles
            m_emitter.Life = 4;
            m_emitter.LifeVar = 0;

            // spin of particles
            m_emitter.StartSpin = 0;
            m_emitter.StartSpinVar = 0;
            m_emitter.EndSpin = 0;
            m_emitter.EndSpinVar = 0;

            // color of particles
            var startColor = new CCColor4F(0.5f, 0.5f, 0.5f, 1.0f);
            m_emitter.StartColor = startColor;

            var startColorVar = new CCColor4F(0.5f, 0.5f, 0.5f, 1.0f);
            m_emitter.StartColorVar = startColorVar;

            var endColor = new CCColor4F(0.1f, 0.1f, 0.1f, 0.2f);
            m_emitter.EndColor = endColor;

            var endColorVar = new CCColor4F(0.1f, 0.1f, 0.1f, 0.2f);
            m_emitter.EndColorVar = endColorVar;

            // size, in pixels
            m_emitter.StartSize = 32;
            m_emitter.StartSizeVar = 0;
            m_emitter.EndSize = CCParticleSystem.kCCParticleStartSizeEqualToEndSize;

            // emits per second
            m_emitter.EmissionRate = m_emitter.TotalParticles / m_emitter.Life;

            // additive
            m_emitter.BlendAdditive = false;
        }

        public override string title()
        {
            return "Radius Mode: Semi Circle";
        }
    };

    public class Issue704 : ParticleDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();

            Color = new CCColor3B(0, 0, 0);
            RemoveChild(m_background, true);
            m_background = null;

            m_emitter = new CCParticleSystemQuad();
            m_emitter.InitWithTotalParticles(100);
            AddChild(m_emitter, 10);
            m_emitter.Texture = CCTextureCache.SharedTextureCache.AddImage("Images/fire");

            // duration
            m_emitter.Duration = CCParticleSystem.kCCParticleDurationInfinity;

            // radius mode
            m_emitter.EmitterMode = CCEmitterMode.kCCParticleModeRadius;

            // radius mode: start and end radius in pixels
            m_emitter.StartRadius = (50);
            m_emitter.StartRadiusVar = (0);
            m_emitter.EndRadius = (CCParticleSystem.kCCParticleStartRadiusEqualToEndRadius);
            m_emitter.EndRadiusVar = (0);

            // radius mode: degrees per second
            m_emitter.RotatePerSecond = (0);
            m_emitter.RotatePerSecondVar = (0);


            // angle
            m_emitter.Angle = 90;
            m_emitter.AngleVar = 0;

            // emitter position
            CCSize size = CCDirector.SharedDirector.WinSize;
            m_emitter.Position = new CCPoint(size.Width / 2, size.Height / 2);
            m_emitter.PosVar = new CCPoint(0, 0);

            // life of particles
            m_emitter.Life = 5;
            m_emitter.LifeVar = 0;

            // spin of particles
            m_emitter.StartSpin = 0;
            m_emitter.StartSpinVar = 0;
            m_emitter.EndSpin = 0;
            m_emitter.EndSpinVar = 0;

            // color of particles
            var startColor = new CCColor4F(0.5f, 0.5f, 0.5f, 1.0f);
            m_emitter.StartColor = startColor;

            var startColorVar = new CCColor4F(0.5f, 0.5f, 0.5f, 1.0f);
            m_emitter.StartColorVar = startColorVar;

            var endColor = new CCColor4F(0.1f, 0.1f, 0.1f, 0.2f);
            m_emitter.EndColor = endColor;

            var endColorVar = new CCColor4F(0.1f, 0.1f, 0.1f, 0.2f);
            m_emitter.EndColorVar = endColorVar;

            // size, in pixels
            m_emitter.StartSize = 16;
            m_emitter.StartSizeVar = 0;
            m_emitter.EndSize = CCParticleSystem.kCCParticleStartSizeEqualToEndSize;

            // emits per second
            m_emitter.EmissionRate = m_emitter.TotalParticles / m_emitter.Life;

            // additive
            m_emitter.BlendAdditive = false;

            CCRotateBy rot = new CCRotateBy (16, 360);
            m_emitter.RunAction(CCRepeatForever.Create(rot));
        }

        public override string title()
        {
            return "Issue 704. Free + Rot";
        }

        public override string subtitle()
        {
            return "Emitted particles should not rotate";
        }
    };

    public class Issue870 : ParticleDemo
    {
        private int m_nIndex;

        public override void OnEnter()
        {
            base.OnEnter();

            Color = new CCColor3B(0, 0, 0);
            RemoveChild(m_background, true);
            m_background = null;

            var system = new CCParticleSystemQuad();
            system.InitWithFile("Particles/SpinningPeas");
            system.SetTextureWithRect(CCTextureCache.SharedTextureCache.AddImage("Images/particles"),
                                      new CCRect(0, 0, 32, 32));
            AddChild(system, 10);
            m_emitter = system;

            m_nIndex = 0;
            Schedule(updateQuads, 2.0f);
        }

        public void updateQuads(float dt)
        {
            m_nIndex = (m_nIndex + 1) % 4;
            var rect = new CCRect(m_nIndex * 32, 0, 32, 32);
            var system = (CCParticleSystemQuad) m_emitter;
            system.SetTextureWithRect(m_emitter.Texture, rect);
        }

        public override string title()
        {
            return "Issue 870. SubRect";
        }

        public override string subtitle()
        {
            return "Every 2 seconds the particle should change";
        }
    }

    public class ParticleReorder : ParticleDemo
    {
        private int m_nOrder;

        public override void OnEnter()
        {
            base.OnEnter();

            m_nOrder = 0;
            Color = CCTypes.CCBlack;
            RemoveChild(m_background, true);
            m_background = null;

            CCParticleSystemQuad ignore = CCParticleSystemQuad.Create("Particles/SmallSun");
            //ignore.TotalParticles = 200;
            CCNode parent1 = CCNode.Create();
            CCParticleBatchNode parent2 = CCParticleBatchNode.Create(ignore.Texture);
            ignore.UnscheduleUpdate();

            for (int i = 0; i < 2; i++)
            {
                CCNode parent = (i == 0 ? parent1 : parent2);

                CCParticleSystemQuad emitter1 = CCParticleSystemQuad.Create("Particles/SmallSun");
                //emitter1.TotalParticles = 200;
                emitter1.StartColor = (new CCColor4F(1, 0, 0, 1));
                emitter1.BlendAdditive = (false);
                CCParticleSystemQuad emitter2 = CCParticleSystemQuad.Create("Particles/SmallSun");
                //emitter2.TotalParticles = 200;
                emitter2.StartColor = (new CCColor4F(0, 1, 0, 1));
                emitter2.BlendAdditive = (false);
                CCParticleSystemQuad emitter3 = CCParticleSystemQuad.Create("Particles/SmallSun");
                //emitter3.TotalParticles = 200;
                emitter3.StartColor = (new CCColor4F(0, 0, 1, 1));
                emitter3.BlendAdditive = (false);

                CCSize s = CCDirector.SharedDirector.WinSize;

                int neg = (i == 0 ? 1 : -1);

                emitter1.Position = (new CCPoint(s.Width / 2 - 30, s.Height / 2 + 60 * neg));
                emitter2.Position = (new CCPoint(s.Width / 2, s.Height / 2 + 60 * neg));
                emitter3.Position = (new CCPoint(s.Width / 2 + 30, s.Height / 2 + 60 * neg));

                parent.AddChild(emitter1, 0, 1);
                parent.AddChild(emitter2, 0, 2);
                parent.AddChild(emitter3, 0, 3);

                AddChild(parent, 10, 1000 + i);
            }

            Schedule(reorderParticles, 1.0f);
        }

        public override string title()
        {
            return "Reordering particles";
        }

        public override string subtitle()
        {
            return "Reordering particles with and without batches batches";
        }

        private void reorderParticles(float dt)
        {
            for (int i = 0; i < 2; i++)
            {
                CCNode parent = GetChildByTag(1000 + i);

                CCNode child1 = parent.GetChildByTag(1);
                CCNode child2 = parent.GetChildByTag(2);
                CCNode child3 = parent.GetChildByTag(3);

                if (m_nOrder % 3 == 0)
                {
                    parent.ReorderChild(child1, 1);
                    parent.ReorderChild(child2, 2);
                    parent.ReorderChild(child3, 3);
                }
                else if (m_nOrder % 3 == 1)
                {
                    parent.ReorderChild(child1, 3);
                    parent.ReorderChild(child2, 1);
                    parent.ReorderChild(child3, 2);
                }
                else if (m_nOrder % 3 == 2)
                {
                    parent.ReorderChild(child1, 2);
                    parent.ReorderChild(child2, 3);
                    parent.ReorderChild(child3, 1);
                }
            }

            m_nOrder++;
        }
    }


    public class ParticleBatchHybrid : ParticleDemo
    {
        private CCNode m_pParent1;
        private CCNode m_pParent2;

        public override void OnEnter()
        {
            base.OnEnter();

            Color = CCTypes.CCBlack;
            RemoveChild(m_background, true);
            m_background = null;

            m_emitter = CCParticleSystemQuad.Create("Particles/LavaFlow");
            m_emitter.Texture = CCTextureCache.SharedTextureCache.AddImage("Images/fire");
            CCParticleBatchNode batch = CCParticleBatchNode.Create(m_emitter.Texture);

            batch.AddChild(m_emitter);

            AddChild(batch, 10);

            Schedule(switchRender, 2.0f);

            CCLayer node = Create();
            AddChild(node);

            m_pParent1 = batch;
            m_pParent2 = node;
        }

        private void switchRender(float dt)
        {
            bool usingBatch = (m_emitter.BatchNode != null);
            m_emitter.RemoveFromParentAndCleanup(false);

            CCNode newParent = (usingBatch ? m_pParent2 : m_pParent1);
            newParent.AddChild(m_emitter);

            CCLog.Log("Particle: Using new parent: {0}", usingBatch ? "CCNode" : "CCParticleBatchNode");
        }

        public override string title()
        {
            return "Paticle Batch";
        }

        public override string subtitle()
        {
            return "Hybrid: batched and non batched every 2 seconds";
        }
    }

    public class ParticleBatchMultipleEmitters : ParticleDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();

            Color = CCTypes.CCBlack;
            RemoveChild(m_background, true);
            m_background = null;

            CCParticleSystemQuad emitter1 = CCParticleSystemQuad.Create("Particles/LavaFlow");
            emitter1.StartColor = (new CCColor4F(1, 0, 0, 1));
            CCParticleSystemQuad emitter2 = CCParticleSystemQuad.Create("Particles/LavaFlow");
            emitter2.StartColor = (new CCColor4F(0, 1, 0, 1));
            CCParticleSystemQuad emitter3 = CCParticleSystemQuad.Create("Particles/LavaFlow");
            emitter3.StartColor = (new CCColor4F(0, 0, 1, 1));

            CCSize s = CCDirector.SharedDirector.WinSize;

            emitter1.Position = (new CCPoint(s.Width / 1.25f, s.Height / 1.25f));
            emitter2.Position = (new CCPoint(s.Width / 2, s.Height / 2));
            emitter3.Position = (new CCPoint(s.Width / 4, s.Height / 4));

            emitter1.Texture = CCTextureCache.SharedTextureCache.AddImage("Images/fire");
            emitter2.Texture = emitter1.Texture;
            emitter3.Texture = emitter1.Texture;

            CCParticleBatchNode batch = CCParticleBatchNode.Create(emitter1.Texture);

            batch.AddChild(emitter1, 0);
            batch.AddChild(emitter2, 0);
            batch.AddChild(emitter3, 0);

            AddChild(batch, 10);
        }

        public override string title()
        {
            return "Paticle Batch";
        }

        public override string subtitle()
        {
            return "Multiple emitters. One Batch";
        }
    }

    public class RainbowEffect : CCParticleSystemQuad
    {
        public bool init()
        {
            return InitWithTotalParticles(150);
        }

        public override bool InitWithTotalParticles(int numberOfParticles)
        {
            if (base.InitWithTotalParticles(numberOfParticles))
            {
                // additive
                BlendAdditive = (false);

                // duration
                Duration = (kCCParticleDurationInfinity);

                // Gravity Mode
                EmitterMode = CCEmitterMode.kCCParticleModeGravity;

                // Gravity Mode: gravity
                Gravity = (new CCPoint(0, 0));

                // Gravity mode: radial acceleration
                RadialAccel = (0);
                RadialAccelVar = (0);

                // Gravity mode: speed of particles
                Speed = (120);
                SpeedVar = (0);


                // angle
                Angle = (180);
                AngleVar = (0);

                // emitter position
                CCSize winSize = CCDirector.SharedDirector.WinSize;
                Position = (new CCPoint(winSize.Width / 2, winSize.Height / 2));
                PosVar = (CCPoint.Zero);

                // life of particles
                Life = (0.5f);
                LifeVar = (0);

                // size, in pixels
                StartSize = (25.0f);
                StartSizeVar = (0);
                EndSize = (kCCParticleStartSizeEqualToEndSize);

                // emits per seconds
                EmissionRate = (TotalParticles / Life);

                // color of particles
                StartColor = (new CCColor4F(50, 50, 50, 50));
                EndColor = (new CCColor4F(0, 0, 0, 0));

                m_tStartColorVar.R = 0.0f;
                m_tStartColorVar.G = 0.0f;
                m_tStartColorVar.B = 0.0f;
                m_tStartColorVar.A = 0.0f;
                m_tEndColorVar.R = 0.0f;
                m_tEndColorVar.G = 0.0f;
                m_tEndColorVar.B = 0.0f;
                m_tEndColorVar.A = 0.0f;

                Texture = (CCTextureCache.SharedTextureCache.AddImage("Images/particles"));
                return true;
            }
            return false;
        }

        public override void Update(float dt)
        {
            m_fEmitCounter = 0;
            base.Update(dt);
        }
    }

    public class Issue1201 : ParticleDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();

            Color = CCTypes.CCBlack;
            RemoveChild(m_background, true);
            m_background = null;

            var particle = new RainbowEffect();
            particle.InitWithTotalParticles(50);

            AddChild(particle);

            CCSize s = CCDirector.SharedDirector.WinSize;

            particle.Position = (new CCPoint(s.Width / 2, s.Height / 2));

            m_emitter = particle;
        }

        public override string title()
        {
            return "Issue 1201. Unfinished";
        }

        public override string subtitle()
        {
            return "Unfinished test. Ignore it";
        }
    }

    public class MultipleParticleSystems : ParticleDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();

            Color = CCTypes.CCBlack;
            RemoveChild(m_background, true);
            m_background = null;

            CCTextureCache.SharedTextureCache.AddImage("Images/particles");

            for (int i = 0; i < 5; i++)
            {
                CCParticleSystemQuad particleSystem = CCParticleSystemQuad.Create("Particles/SpinningPeas");

                particleSystem.Position = (new CCPoint(i * 50, i * 50));

                particleSystem.PositionType = CCPositionType.kCCPositionTypeGrouped;
                AddChild(particleSystem);
            }

            m_emitter = null;
        }

        public override string title()
        {
            return "Multiple particle systems";
        }

        public override string subtitle()
        {
            return "v1.1 test: FPS should be lower than next test";
        }
    }

    public class MultipleParticleSystemsBatched : ParticleDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();

            Color = CCTypes.CCBlack;
            RemoveChild(m_background, true);
            m_background = null;

            CCParticleBatchNode batchNode = CCParticleBatchNode.Create("Images/fire", 3000);

            AddChild(batchNode, 1, 2);

            for (int i = 0; i < 5; i++)
            {
                CCParticleSystemQuad particleSystem = CCParticleSystemQuad.Create("Particles/SpinningPeas");

                particleSystem.PositionType = CCPositionType.kCCPositionTypeGrouped;
                particleSystem.Position = (new CCPoint(i * 50, i * 50));

                particleSystem.Texture = batchNode.Texture;
                batchNode.AddChild(particleSystem);
            }


            m_emitter = null;
        }

        public override string title()
        {
            return "Multiple particle systems";
        }

        public override string subtitle()
        {
            return "v1.1 test: FPS should be lower than next test";
        }
    }


    public class AddAndDeleteParticleSystems : ParticleDemo
    {
        private CCParticleBatchNode m_pBatchNode;

        public override void OnEnter()
        {
            base.OnEnter();

            Color = CCTypes.CCBlack;
            RemoveChild(m_background, true);
            m_background = null;

            //adds the texture inside the plist to the texture cache
            m_pBatchNode = CCParticleBatchNode.Create("Images/fire", 16000);

            AddChild(m_pBatchNode, 1, 2);

            for (int i = 0; i < 6; i++)
            {
                CCParticleSystemQuad particleSystem = CCParticleSystemQuad.Create("Particles/Spiral");
                particleSystem.Texture = m_pBatchNode.Texture;

                particleSystem.PositionType = CCPositionType.kCCPositionTypeGrouped;
                particleSystem.TotalParticles = (200);

                particleSystem.Position = (new CCPoint(i * 15 + 100, i * 15 + 100));

                int randZ = Random.Next(100);
                m_pBatchNode.AddChild(particleSystem, randZ, -1);
            }

            Schedule(removeSystem, 0.5f);
            m_emitter = null;
        }

        private void removeSystem(float dt)
        {
            int nChildrenCount = m_pBatchNode.ChildrenCount;
            if (nChildrenCount > 0)
            {
                CCLog.Log("remove random system");
                int uRand = Random.Next(nChildrenCount - 1);
                m_pBatchNode.RemoveChild(m_pBatchNode.Children[uRand], true);

                CCParticleSystemQuad particleSystem = CCParticleSystemQuad.Create("Particles/Spiral");
                //add new

                particleSystem.PositionType = CCPositionType.kCCPositionTypeGrouped;
                particleSystem.TotalParticles = (200);

                particleSystem.Position = (new CCPoint(Random.Next(300), Random.Next(400)));

                CCLog.Log("add a new system");
                int randZ = Random.Next(100);
                particleSystem.Texture = m_pBatchNode.Texture;
                m_pBatchNode.AddChild(particleSystem, randZ, -1);
            }
        }

        public override string title()
        {
            return "Add and remove Particle System";
        }

        public override string subtitle()
        {
            return "v1.1 test: every 2 sec 1 system disappear, 1 appears";
        }
    }

    public class ReorderParticleSystems : ParticleDemo
    {
        private CCParticleBatchNode m_pBatchNode;

        public override void OnEnter()
        {
            base.OnEnter();

            Color = CCTypes.CCBlack;
            RemoveChild(m_background, true);
            m_background = null;

            m_pBatchNode = CCParticleBatchNode.Create("Images/stars-grayscale", 3000);

            AddChild(m_pBatchNode, 1, 2);


            for (int i = 0; i < 3; i++)
            {
                var particleSystem = new CCParticleSystemQuad();
                particleSystem.InitWithTotalParticles(200);
                particleSystem.Texture = (m_pBatchNode.Texture);

                // duration
                particleSystem.Duration = CCParticleSystem.kCCParticleDurationInfinity;

                // radius mode
                particleSystem.EmitterMode = CCEmitterMode.kCCParticleModeRadius;

                // radius mode: 100 pixels from center
                particleSystem.StartRadius = (100);
                particleSystem.StartRadiusVar = (0);
                particleSystem.EndRadius = (CCParticleSystem.kCCParticleStartRadiusEqualToEndRadius);
                particleSystem.EndRadiusVar = (0); // not used when start == end

                // radius mode: degrees per second
                // 45 * 4 seconds of life = 180 degrees
                particleSystem.RotatePerSecond = (45);
                particleSystem.RotatePerSecondVar = (0);


                // angle
                particleSystem.Angle = (90);
                particleSystem.AngleVar = (0);

                // emitter position
                particleSystem.PosVar = (CCPoint.Zero);

                // life of particles
                particleSystem.Life = (4);
                particleSystem.LifeVar = (0);

                // spin of particles
                particleSystem.StartSpin = (0);
                particleSystem.StartSpinVar = (0);
                particleSystem.EndSpin = (0);
                particleSystem.EndSpinVar = (0);

                // color of particles
                var color = new float[3] {0, 0, 0};
                color[i] = 1;
                var startColor = new CCColor4F(color[0], color[1], color[2], 1.0f);
                particleSystem.StartColor = (startColor);

                var startColorVar = new CCColor4F(0, 0, 0, 0);
                particleSystem.StartColorVar = (startColorVar);

                CCColor4F endColor = startColor;
                particleSystem.EndColor = (endColor);

                CCColor4F endColorVar = startColorVar;
                particleSystem.EndColorVar = (endColorVar);

                // size, in pixels
                particleSystem.StartSize = (32);
                particleSystem.StartSizeVar = (0);
                particleSystem.EndSize = CCParticleSystem.kCCParticleStartSizeEqualToEndSize;

                // emits per second
                particleSystem.EmissionRate = (particleSystem.TotalParticles / particleSystem.Life);

                // additive

                particleSystem.Position = (new CCPoint(i * 10 + 120, 200));


                m_pBatchNode.AddChild(particleSystem);
                particleSystem.PositionType = CCPositionType.kCCPositionTypeFree;

                //[pBNode addChild:particleSystem z:10 tag:0);
            }

            Schedule(reorderSystem, 2.0f);
            m_emitter = null;
        }

        private void reorderSystem(float dt)
        {
            var system = (CCParticleSystem) m_pBatchNode.Children[1];
            m_pBatchNode.ReorderChild(system, system.ZOrder - 1);
        }

        public override string title()
        {
            return "reorder systems";
        }

        public override string subtitle()
        {
            return "changes every 2 seconds";
        }
    }

    public class PremultipliedAlphaTest : ParticleDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();

            Color = CCTypes.CCBlack;
            RemoveChild(m_background, true);
            m_background = null;

            m_emitter = CCParticleSystemQuad.Create("Particles/BoilingFoam");

            // Particle Designer "normal" blend func causes black halo on premul textures (ignores multiplication)
            //this->emitter.blendFunc = (ccBlendFunc){ GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA };

            // Cocos2d "normal" blend func for premul causes alpha to be ignored (oversaturates colors)
            var tBlendFunc = new CCBlendFunc(OGLES.GL_ONE, OGLES.GL_ONE_MINUS_SRC_ALPHA);
            m_emitter.BlendFunc = tBlendFunc;

            //Debug.Assert(m_emitter.OpacityModifyRGB, "Particle texture does not have premultiplied alpha, test is useless");

            // Toggle next line to see old behavior
            //	this->emitter.opacityModifyRGB = NO;

            m_emitter.StartColor = new CCColor4F(1, 1, 1, 1);
            m_emitter.EndColor = new CCColor4F(1, 1, 1, 0);
            m_emitter.StartColorVar = new CCColor4F(0, 0, 0, 0);
            m_emitter.EndColorVar = new CCColor4F(0, 0, 0, 0);

            AddChild(m_emitter, 10);
        }


        public override string title()
        {
            return "premultiplied alpha";
        }

        public override string subtitle()
        {
            return "no black halo, particles should fade out";
        }
    }

    public class PremultipliedAlphaTest2 : ParticleDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();

            Color = CCTypes.CCBlack;
            RemoveChild(m_background, true);
            m_background = null;

            m_emitter = CCParticleSystemQuad.Create("Particles/TestPremultipliedAlpha");
            AddChild(m_emitter, 10);
        }


        public override string title()
        {
            return "premultiplied alpha 2";
        }

        public override string subtitle()
        {
            return "Arrows should be faded";
        }
    }
}