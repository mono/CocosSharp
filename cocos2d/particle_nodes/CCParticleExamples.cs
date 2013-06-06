
namespace Cocos2D
{
    //
    // ParticleFire
    //
    public class CCParticleFire : CCParticleSystemQuad
    {
        public CCParticleFire () : base (250)
        { }

        public override bool InitWithTotalParticles(int numberOfParticles)
        {
            if (base.InitWithTotalParticles(numberOfParticles))
            {
                // duration
                m_fDuration = kCCParticleDurationInfinity;

                // Gravity Mode
                m_nEmitterMode = CCEmitterMode.Gravity;

                // Gravity Mode: gravity
                modeA.gravity = new CCPoint(0, 0);

                // Gravity Mode: radial acceleration
                modeA.radialAccel = 0;
                modeA.radialAccelVar = 0;

                // Gravity Mode: speed of particles
                modeA.speed = 60;
                modeA.speedVar = 20;

                // starting angle
                m_fAngle = 90;
                m_fAngleVar = 10;

                // emitter position
                CCSize winSize = CCDirector.SharedDirector.WinSize;
                SetPosition(winSize.Width / 2, 60);
                m_tPosVar = new CCPoint(40, 20);

                // life of particles
                m_fLife = 3;
                m_fLifeVar = 0.25f;


                // size, in pixels
                m_fStartSize = 54.0f;
                m_fStartSizeVar = 10.0f;
                m_fEndSize = kCCParticleStartSizeEqualToEndSize;

                // emits per frame
                m_fEmissionRate = m_uTotalParticles / m_fLife;

                // color of particles
                m_tStartColor.R = 0.76f;
                m_tStartColor.G = 0.25f;
                m_tStartColor.B = 0.12f;
                m_tStartColor.A = 1.0f;
                m_tStartColorVar.R = 0.0f;
                m_tStartColorVar.G = 0.0f;
                m_tStartColorVar.B = 0.0f;
                m_tStartColorVar.A = 0.0f;
                m_tEndColor.R = 0.0f;
                m_tEndColor.G = 0.0f;
                m_tEndColor.B = 0.0f;
                m_tEndColor.A = 1.0f;
                m_tEndColorVar.R = 0.0f;
                m_tEndColorVar.G = 0.0f;
                m_tEndColorVar.B = 0.0f;
                m_tEndColorVar.A = 0.0f;

                // additive
                BlendAdditive = true;
                return true;
            }
            return false;
        }
    }

    //
    // ParticleFireworks
    //
    public class CCParticleFireworks : CCParticleSystemQuad
    {
        public CCParticleFireworks () : base(1500)
        { }

        public override bool InitWithTotalParticles(int numberOfParticles)
        {
            if (base.InitWithTotalParticles(numberOfParticles))
            {
                // duration
                m_fDuration = kCCParticleDurationInfinity;

                // Gravity Mode
                m_nEmitterMode = CCEmitterMode.Gravity;

                // Gravity Mode: gravity
                modeA.gravity = new CCPoint(0, -90);

                // Gravity Mode:  radial
                modeA.radialAccel = 0;
                modeA.radialAccelVar = 0;

                //  Gravity Mode: speed of particles
                modeA.speed = 180;
                modeA.speedVar = 50;

                // emitter position
                CCSize winSize = CCDirector.SharedDirector.WinSize;
                Position = new CCPoint(winSize.Width / 2, winSize.Height / 2);

                // angle
                m_fAngle = 90;
                m_fAngleVar = 20;

                // life of particles
                m_fLife = 3.5f;
                m_fLifeVar = 1;

                // emits per frame
                m_fEmissionRate = m_uTotalParticles / m_fLife;

                // color of particles
                m_tStartColor.R = 0.5f;
                m_tStartColor.G = 0.5f;
                m_tStartColor.B = 0.5f;
                m_tStartColor.A = 1.0f;
                m_tStartColorVar.R = 0.5f;
                m_tStartColorVar.G = 0.5f;
                m_tStartColorVar.B = 0.5f;
                m_tStartColorVar.A = 0.1f;
                m_tEndColor.R = 0.1f;
                m_tEndColor.G = 0.1f;
                m_tEndColor.B = 0.1f;
                m_tEndColor.A = 0.2f;
                m_tEndColorVar.R = 0.1f;
                m_tEndColorVar.G = 0.1f;
                m_tEndColorVar.B = 0.1f;
                m_tEndColorVar.A = 0.2f;

                // size, in pixels
                m_fStartSize = 8.0f;
                m_fStartSizeVar = 2.0f;
                m_fEndSize = kCCParticleStartSizeEqualToEndSize;

                // additive
                BlendAdditive = false;
                return true;
            }
            return false;
        }
    }

    //
    // ParticleSun
    //
    public class CCParticleSun : CCParticleSystemQuad
    {
        public CCParticleSun (int num) : base(num)
        { }

        public CCParticleSun () : base (350)
        { }

        public override bool InitWithTotalParticles(int numberOfParticles)
        {
            if (base.InitWithTotalParticles(numberOfParticles))
            {
                // additive
                BlendAdditive = true;

                // duration
                m_fDuration = kCCParticleDurationInfinity;

                // Gravity Mode
                m_nEmitterMode = CCEmitterMode.Gravity;

                // Gravity Mode: gravity
                modeA.gravity = new CCPoint(0, 0);

                // Gravity mode: radial acceleration
                modeA.radialAccel = 0;
                modeA.radialAccelVar = 0;

                // Gravity mode: speed of particles
                modeA.speed = 20;
                modeA.speedVar = 5;


                // angle
                m_fAngle = 90;
                m_fAngleVar = 360;

                // emitter position
                CCSize winSize = CCDirector.SharedDirector.WinSize;
                Position = new CCPoint(winSize.Width / 2, winSize.Height / 2);
                m_tPosVar = CCPoint.Zero;

                // life of particles
                m_fLife = 1;
                m_fLifeVar = 0.5f;

                // size, in pixels
                m_fStartSize = 30.0f;
                m_fStartSizeVar = 10.0f;
                m_fEndSize = kCCParticleStartSizeEqualToEndSize;

                // emits per seconds
                m_fEmissionRate = m_uTotalParticles / m_fLife;

                // color of particles
                m_tStartColor.R = 0.76f;
                m_tStartColor.G = 0.25f;
                m_tStartColor.B = 0.12f;
                m_tStartColor.A = 1.0f;
                m_tStartColorVar.R = 0.0f;
                m_tStartColorVar.G = 0.0f;
                m_tStartColorVar.B = 0.0f;
                m_tStartColorVar.A = 0.0f;
                m_tEndColor.R = 0.0f;
                m_tEndColor.G = 0.0f;
                m_tEndColor.B = 0.0f;
                m_tEndColor.A = 1.0f;
                m_tEndColorVar.R = 0.0f;
                m_tEndColorVar.G = 0.0f;
                m_tEndColorVar.B = 0.0f;
                m_tEndColorVar.A = 0.0f;

                return true;
            }
            return false;
        }
    }

    //
    // ParticleGalaxy
    //
    public class CCParticleGalaxy : CCParticleSystemQuad
    {
        public CCParticleGalaxy ()  :base (200)
        {}

        public override bool InitWithTotalParticles(int numberOfParticles)
        {
            if (base.InitWithTotalParticles(numberOfParticles))
            {
                // duration
                m_fDuration = kCCParticleDurationInfinity;

                // Gravity Mode
                m_nEmitterMode = CCEmitterMode.Gravity;

                // Gravity Mode: gravity
                modeA.gravity = new CCPoint(0, 0);

                // Gravity Mode: speed of particles
                modeA.speed = 60;
                modeA.speedVar = 10;

                // Gravity Mode: radial
                modeA.radialAccel = -80;
                modeA.radialAccelVar = 0;

                // Gravity Mode: tagential
                modeA.tangentialAccel = 80;
                modeA.tangentialAccelVar = 0;

                // angle
                m_fAngle = 90;
                m_fAngleVar = 360;

                // emitter position
                CCSize winSize = CCDirector.SharedDirector.WinSize;
                Position = new CCPoint(winSize.Width / 2, winSize.Height / 2);
                m_tPosVar = CCPoint.Zero;

                // life of particles
                m_fLife = 4;
                m_fLifeVar = 1;

                // size, in pixels
                m_fStartSize = 37.0f;
                m_fStartSizeVar = 10.0f;
                m_fEndSize = kCCParticleStartSizeEqualToEndSize;

                // emits per second
                m_fEmissionRate = m_uTotalParticles / m_fLife;

                // color of particles
                m_tStartColor.R = 0.12f;
                m_tStartColor.G = 0.25f;
                m_tStartColor.B = 0.76f;
                m_tStartColor.A = 1.0f;
                m_tStartColorVar.R = 0.0f;
                m_tStartColorVar.G = 0.0f;
                m_tStartColorVar.B = 0.0f;
                m_tStartColorVar.A = 0.0f;
                m_tEndColor.R = 0.0f;
                m_tEndColor.G = 0.0f;
                m_tEndColor.B = 0.0f;
                m_tEndColor.A = 1.0f;
                m_tEndColorVar.R = 0.0f;
                m_tEndColorVar.G = 0.0f;
                m_tEndColorVar.B = 0.0f;
                m_tEndColorVar.A = 0.0f;

                // additive
                BlendAdditive = true;
                return true;
            }
            return false;
        }
    }

    public class CCParticleFlower : CCParticleSystemQuad
    {
        public CCParticleFlower () : base (250)
        { }

        public override bool InitWithTotalParticles(int numberOfParticles)
        {
            if (base.InitWithTotalParticles(numberOfParticles))
            {
                // duration
                m_fDuration = kCCParticleDurationInfinity;

                // Gravity Mode
                m_nEmitterMode = CCEmitterMode.Gravity;

                // Gravity Mode: gravity
                modeA.gravity = new CCPoint(0, 0);

                // Gravity Mode: speed of particles
                modeA.speed = 80;
                modeA.speedVar = 10;

                // Gravity Mode: radial
                modeA.radialAccel = -60;
                modeA.radialAccelVar = 0;

                // Gravity Mode: tagential
                modeA.tangentialAccel = 15;
                modeA.tangentialAccelVar = 0;

                // angle
                m_fAngle = 90;
                m_fAngleVar = 360;

                // emitter position
                CCSize winSize = CCDirector.SharedDirector.WinSize;
                Position = new CCPoint(winSize.Width / 2, winSize.Height / 2);
                m_tPosVar = CCPoint.Zero;

                // life of particles
                m_fLife = 4;
                m_fLifeVar = 1;

                // size, in pixels
                m_fStartSize = 30.0f;
                m_fStartSizeVar = 10.0f;
                m_fEndSize = kCCParticleStartSizeEqualToEndSize;

                // emits per second
                m_fEmissionRate = m_uTotalParticles / m_fLife;

                // color of particles
                m_tStartColor.R = 0.50f;
                m_tStartColor.G = 0.50f;
                m_tStartColor.B = 0.50f;
                m_tStartColor.A = 1.0f;
                m_tStartColorVar.R = 0.5f;
                m_tStartColorVar.G = 0.5f;
                m_tStartColorVar.B = 0.5f;
                m_tStartColorVar.A = 0.5f;
                m_tEndColor.R = 0.0f;
                m_tEndColor.G = 0.0f;
                m_tEndColor.B = 0.0f;
                m_tEndColor.A = 1.0f;
                m_tEndColorVar.R = 0.0f;
                m_tEndColorVar.G = 0.0f;
                m_tEndColorVar.B = 0.0f;
                m_tEndColorVar.A = 0.0f;

                // additive
                BlendAdditive = true;
                return true;
            }
            return false;
        }
    }

    public class CCParticleMeteor : CCParticleSystemQuad
    {
        public CCParticleMeteor () : base(150)
        { }

        public override bool InitWithTotalParticles(int numberOfParticles)
        {
            if (base.InitWithTotalParticles(numberOfParticles))
            {
                // duration
                m_fDuration = kCCParticleDurationInfinity;

                // Gravity Mode
                m_nEmitterMode = CCEmitterMode.Gravity;

                // Gravity Mode: gravity
                modeA.gravity = new CCPoint(-200, 200);

                // Gravity Mode: speed of particles
                modeA.speed = 15;
                modeA.speedVar = 5;

                // Gravity Mode: radial
                modeA.radialAccel = 0;
                modeA.radialAccelVar = 0;

                // Gravity Mode: tagential
                modeA.tangentialAccel = 0;
                modeA.tangentialAccelVar = 0;

                // angle
                m_fAngle = 90;
                m_fAngleVar = 360;

                // emitter position
                CCSize winSize = CCDirector.SharedDirector.WinSize;
                Position = new CCPoint(winSize.Width / 2, winSize.Height / 2);
                m_tPosVar = CCPoint.Zero;

                // life of particles
                m_fLife = 2;
                m_fLifeVar = 1;

                // size, in pixels
                m_fStartSize = 60.0f;
                m_fStartSizeVar = 10.0f;
                m_fEndSize = kCCParticleStartSizeEqualToEndSize;

                // emits per second
                m_fEmissionRate = m_uTotalParticles / m_fLife;

                // color of particles
                m_tStartColor.R = 0.2f;
                m_tStartColor.G = 0.4f;
                m_tStartColor.B = 0.7f;
                m_tStartColor.A = 1.0f;
                m_tStartColorVar.R = 0.0f;
                m_tStartColorVar.G = 0.0f;
                m_tStartColorVar.B = 0.2f;
                m_tStartColorVar.A = 0.1f;
                m_tEndColor.R = 0.0f;
                m_tEndColor.G = 0.0f;
                m_tEndColor.B = 0.0f;
                m_tEndColor.A = 1.0f;
                m_tEndColorVar.R = 0.0f;
                m_tEndColorVar.G = 0.0f;
                m_tEndColorVar.B = 0.0f;
                m_tEndColorVar.A = 0.0f;

                // additive
                BlendAdditive = true;
                return true;
            }
            return false;
        }
    }

    public class CCParticleSpiral : CCParticleSystemQuad
    {
        public CCParticleSpiral () : base(500)
        { }

        public override bool InitWithTotalParticles(int numberOfParticles)
        {
            if (base.InitWithTotalParticles(numberOfParticles))
            {
                // duration
                m_fDuration = kCCParticleDurationInfinity;

                // Gravity Mode
                m_nEmitterMode = CCEmitterMode.Gravity;

                // Gravity Mode: gravity
                modeA.gravity = new CCPoint(0, 0);

                // Gravity Mode: speed of particles
                modeA.speed = 150;
                modeA.speedVar = 0;

                // Gravity Mode: radial
                modeA.radialAccel = -380;
                modeA.radialAccelVar = 0;

                // Gravity Mode: tagential
                modeA.tangentialAccel = 45;
                modeA.tangentialAccelVar = 0;

                // angle
                m_fAngle = 90;
                m_fAngleVar = 0;

                // emitter position
                CCSize winSize = CCDirector.SharedDirector.WinSize;
                Position = new CCPoint(winSize.Width / 2, winSize.Height / 2);
                m_tPosVar = CCPoint.Zero;

                // life of particles
                m_fLife = 12;
                m_fLifeVar = 0;

                // size, in pixels
                m_fStartSize = 20.0f;
                m_fStartSizeVar = 0.0f;
                m_fEndSize = kCCParticleStartSizeEqualToEndSize;

                // emits per second
                m_fEmissionRate = m_uTotalParticles / m_fLife;

                // color of particles
                m_tStartColor.R = 0.5f;
                m_tStartColor.G = 0.5f;
                m_tStartColor.B = 0.5f;
                m_tStartColor.A = 1.0f;
                m_tStartColorVar.R = 0.5f;
                m_tStartColorVar.G = 0.5f;
                m_tStartColorVar.B = 0.5f;
                m_tStartColorVar.A = 0.0f;
                m_tEndColor.R = 0.5f;
                m_tEndColor.G = 0.5f;
                m_tEndColor.B = 0.5f;
                m_tEndColor.A = 1.0f;
                m_tEndColorVar.R = 0.5f;
                m_tEndColorVar.G = 0.5f;
                m_tEndColorVar.B = 0.5f;
                m_tEndColorVar.A = 0.0f;

                // additive
                BlendAdditive = false;
                return true;
            }
            return false;
        }
    }

    public class CCParticleExplosion : CCParticleSystemQuad
    {
        public CCParticleExplosion () : base(700)
        {   }

        public override bool InitWithTotalParticles(int numberOfParticles)
        {
            if (base.InitWithTotalParticles(numberOfParticles))
            {
                // duration
                m_fDuration = 0.1f;

                m_nEmitterMode = CCEmitterMode.Gravity;

                // Gravity Mode: gravity
                modeA.gravity = new CCPoint(0, 0);

                // Gravity Mode: speed of particles
                modeA.speed = 70;
                modeA.speedVar = 40;

                // Gravity Mode: radial
                modeA.radialAccel = 0;
                modeA.radialAccelVar = 0;

                // Gravity Mode: tagential
                modeA.tangentialAccel = 0;
                modeA.tangentialAccelVar = 0;

                // angle
                m_fAngle = 90;
                m_fAngleVar = 360;

                // emitter position
                CCSize winSize = CCDirector.SharedDirector.WinSize;
                Position = new CCPoint(winSize.Width / 2, winSize.Height / 2);
                m_tPosVar = CCPoint.Zero;

                // life of particles
                m_fLife = 5.0f;
                m_fLifeVar = 2;

                // size, in pixels
                m_fStartSize = 15.0f;
                m_fStartSizeVar = 10.0f;
                m_fEndSize = kCCParticleStartSizeEqualToEndSize;

                // emits per second
                m_fEmissionRate = m_uTotalParticles / m_fDuration;

                // color of particles
                m_tStartColor.R = 0.7f;
                m_tStartColor.G = 0.1f;
                m_tStartColor.B = 0.2f;
                m_tStartColor.A = 1.0f;
                m_tStartColorVar.R = 0.5f;
                m_tStartColorVar.G = 0.5f;
                m_tStartColorVar.B = 0.5f;
                m_tStartColorVar.A = 0.0f;
                m_tEndColor.R = 0.5f;
                m_tEndColor.G = 0.5f;
                m_tEndColor.B = 0.5f;
                m_tEndColor.A = 0.0f;
                m_tEndColorVar.R = 0.5f;
                m_tEndColorVar.G = 0.5f;
                m_tEndColorVar.B = 0.5f;
                m_tEndColorVar.A = 0.0f;

                // additive
                BlendAdditive = false;
                return true;
            }
            return false;
        }
    }

    public class CCParticleSmoke : CCParticleSystemQuad
    {
        public CCParticleSmoke () : base(200)
        { }

        public override bool InitWithTotalParticles(int numberOfParticles)
        {
            if (base.InitWithTotalParticles(numberOfParticles))
            {
                // duration
                m_fDuration = kCCParticleDurationInfinity;

                // Emitter mode: Gravity Mode
                m_nEmitterMode = CCEmitterMode.Gravity;

                // Gravity Mode: gravity
                modeA.gravity = new CCPoint(0, 0);

                // Gravity Mode: radial acceleration
                modeA.radialAccel = 0;
                modeA.radialAccelVar = 0;

                // Gravity Mode: speed of particles
                modeA.speed = 25;
                modeA.speedVar = 10;

                // angle
                m_fAngle = 90;
                m_fAngleVar = 5;

                // emitter position
                CCSize winSize = CCDirector.SharedDirector.WinSize;
                Position = new CCPoint(winSize.Width / 2, 0);
                m_tPosVar = new CCPoint(20, 0);

                // life of particles
                m_fLife = 4;
                m_fLifeVar = 1;

                // size, in pixels
                m_fStartSize = 60.0f;
                m_fStartSizeVar = 10.0f;
                m_fEndSize = kCCParticleStartSizeEqualToEndSize;

                // emits per frame
                m_fEmissionRate = m_uTotalParticles / m_fLife;

                // color of particles
                m_tStartColor.R = 0.8f;
                m_tStartColor.G = 0.8f;
                m_tStartColor.B = 0.8f;
                m_tStartColor.A = 1.0f;
                m_tStartColorVar.R = 0.02f;
                m_tStartColorVar.G = 0.02f;
                m_tStartColorVar.B = 0.02f;
                m_tStartColorVar.A = 0.0f;
                m_tEndColor.R = 0.0f;
                m_tEndColor.G = 0.0f;
                m_tEndColor.B = 0.0f;
                m_tEndColor.A = 1.0f;
                m_tEndColorVar.R = 0.0f;
                m_tEndColorVar.G = 0.0f;
                m_tEndColorVar.B = 0.0f;
                m_tEndColorVar.A = 0.0f;

                // additive
                BlendAdditive = false;
                return true;
            }
            return false;
        }
    }

    public class CCParticleSnow : CCParticleSystemQuad
    {
        public CCParticleSnow () : base(700)
        { }

        public override bool InitWithTotalParticles(int numberOfParticles)
        {
            if (base.InitWithTotalParticles(numberOfParticles))
            {
                // duration
                m_fDuration = kCCParticleDurationInfinity;

                // set gravity mode.
                m_nEmitterMode = CCEmitterMode.Gravity;

                // Gravity Mode: gravity
                modeA.gravity = new CCPoint(0, -1);

                // Gravity Mode: speed of particles
                modeA.speed = 5;
                modeA.speedVar = 1;

                // Gravity Mode: radial
                modeA.radialAccel = 0;
                modeA.radialAccelVar = 1;

                // Gravity mode: tagential
                modeA.tangentialAccel = 0;
                modeA.tangentialAccelVar = 1;

                // emitter position
                CCSize winSize = CCDirector.SharedDirector.WinSize;
                Position = new CCPoint(winSize.Width / 2, winSize.Height + 10);
                m_tPosVar = new CCPoint(winSize.Width / 2, 0);

                // angle
                m_fAngle = -90;
                m_fAngleVar = 5;

                // life of particles
                m_fLife = 45;
                m_fLifeVar = 15;

                // size, in pixels
                m_fStartSize = 10.0f;
                m_fStartSizeVar = 5.0f;
                m_fEndSize = kCCParticleStartSizeEqualToEndSize;

                // emits per second
                m_fEmissionRate = 10;

                // color of particles
                m_tStartColor.R = 1.0f;
                m_tStartColor.G = 1.0f;
                m_tStartColor.B = 1.0f;
                m_tStartColor.A = 1.0f;
                m_tStartColorVar.R = 0.0f;
                m_tStartColorVar.G = 0.0f;
                m_tStartColorVar.B = 0.0f;
                m_tStartColorVar.A = 0.0f;
                m_tEndColor.R = 1.0f;
                m_tEndColor.G = 1.0f;
                m_tEndColor.B = 1.0f;
                m_tEndColor.A = 0.0f;
                m_tEndColorVar.R = 0.0f;
                m_tEndColorVar.G = 0.0f;
                m_tEndColorVar.B = 0.0f;
                m_tEndColorVar.A = 0.0f;

                // additive
                BlendAdditive = false;
                return true;
            }
            return false;
        }
    }

    public class CCParticleRain : CCParticleSystemQuad
    {
        public CCParticleRain () : base(1000)
        { }

        public override bool InitWithTotalParticles(int numberOfParticles)
        {
            if (base.InitWithTotalParticles(numberOfParticles))
            {
                // duration
                m_fDuration = kCCParticleDurationInfinity;

                m_nEmitterMode = CCEmitterMode.Gravity;

                // Gravity Mode: gravity
                modeA.gravity = new CCPoint(10, -10);

                // Gravity Mode: radial
                modeA.radialAccel = 0;
                modeA.radialAccelVar = 1;

                // Gravity Mode: tagential
                modeA.tangentialAccel = 0;
                modeA.tangentialAccelVar = 1;

                // Gravity Mode: speed of particles
                modeA.speed = 130;
                modeA.speedVar = 30;

                // angle
                m_fAngle = -90;
                m_fAngleVar = 5;


                // emitter position
                CCSize winSize = CCDirector.SharedDirector.WinSize;
                Position = new CCPoint(winSize.Width / 2, winSize.Height);
                m_tPosVar = new CCPoint(winSize.Width / 2, 0);

                // life of particles
                m_fLife = 4.5f;
                m_fLifeVar = 0;

                // size, in pixels
                m_fStartSize = 4.0f;
                m_fStartSizeVar = 2.0f;
                m_fEndSize = kCCParticleStartSizeEqualToEndSize;

                // emits per second
                m_fEmissionRate = 20;

                // color of particles
                m_tStartColor.R = 0.7f;
                m_tStartColor.G = 0.8f;
                m_tStartColor.B = 1.0f;
                m_tStartColor.A = 1.0f;
                m_tStartColorVar.R = 0.0f;
                m_tStartColorVar.G = 0.0f;
                m_tStartColorVar.B = 0.0f;
                m_tStartColorVar.A = 0.0f;
                m_tEndColor.R = 0.7f;
                m_tEndColor.G = 0.8f;
                m_tEndColor.B = 1.0f;
                m_tEndColor.A = 0.5f;
                m_tEndColorVar.R = 0.0f;
                m_tEndColorVar.G = 0.0f;
                m_tEndColorVar.B = 0.0f;
                m_tEndColorVar.A = 0.0f;

                // additive
                BlendAdditive = false;
                return true;
            }
            return false;
        }
    }
}