
using System.IO;
using Microsoft.Xna.Framework.Graphics;

namespace Cocos2D
{

    internal static class CCParticleExample
    {
        private static byte[] _firePngData =
            {
                0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A, 0x00, 0x00, 0x00, 0x0D, 0x49, 0x48, 0x44, 0x52,
                0x00, 0x00, 0x00, 0x20, 0x00, 0x00, 0x00, 0x20, 0x08, 0x06, 0x00, 0x00, 0x00, 0x73, 0x7A, 0x7A,
                0xF4, 0x00, 0x00, 0x00, 0x04, 0x67, 0x41, 0x4D, 0x41, 0x00, 0x00, 0xAF, 0xC8, 0x37, 0x05, 0x8A,
                0xE9, 0x00, 0x00, 0x00, 0x19, 0x74, 0x45, 0x58, 0x74, 0x53, 0x6F, 0x66, 0x74, 0x77, 0x61, 0x72,
                0x65, 0x00, 0x41, 0x64, 0x6F, 0x62, 0x65, 0x20, 0x49, 0x6D, 0x61, 0x67, 0x65, 0x52, 0x65, 0x61,
                0x64, 0x79, 0x71, 0xC9, 0x65, 0x3C, 0x00, 0x00, 0x02, 0x64, 0x49, 0x44, 0x41, 0x54, 0x78, 0xDA,
                0xC4, 0x97, 0x89, 0x6E, 0xEB, 0x20, 0x10, 0x45, 0xBD, 0xE1, 0x2D, 0x4B, 0xFF, 0xFF, 0x37, 0x5F,
                0x5F, 0x0C, 0xD8, 0xC4, 0xAE, 0x2D, 0xDD, 0xA9, 0x6E, 0xA7, 0x38, 0xC1, 0x91, 0xAA, 0x44, 0xBA,
                0xCA, 0x06, 0xCC, 0x99, 0x85, 0x01, 0xE7, 0xCB, 0xB2, 0x64, 0xEF, 0x7C, 0x55, 0x2F, 0xCC, 0x69,
                0x56, 0x15, 0xAB, 0x72, 0x68, 0x81, 0xE6, 0x55, 0xFE, 0xE8, 0x62, 0x79, 0x62, 0x04, 0x36, 0xA3,
                0x06, 0xC0, 0x9B, 0xCA, 0x08, 0xC0, 0x7D, 0x55, 0x80, 0xA6, 0x54, 0x98, 0x67, 0x11, 0xA8, 0xA1,
                0x86, 0x3E, 0x0B, 0x44, 0x41, 0x00, 0x33, 0x19, 0x1F, 0x21, 0x43, 0x9F, 0x5F, 0x02, 0x68, 0x49,
                0x1D, 0x20, 0x1A, 0x82, 0x28, 0x09, 0xE0, 0x4E, 0xC6, 0x3D, 0x64, 0x57, 0x39, 0x80, 0xBA, 0xA3,
                0x00, 0x1D, 0xD4, 0x93, 0x3A, 0xC0, 0x34, 0x0F, 0x00, 0x3C, 0x8C, 0x59, 0x4A, 0x99, 0x44, 0xCA,
                0xA6, 0x02, 0x88, 0xC7, 0xA7, 0x55, 0x67, 0xE8, 0x44, 0x10, 0x12, 0x05, 0x0D, 0x30, 0x92, 0xE7,
                0x52, 0x33, 0x32, 0x26, 0xC3, 0x38, 0xF7, 0x0C, 0xA0, 0x06, 0x40, 0x0F, 0xC3, 0xD7, 0x55, 0x17,
                0x05, 0xD1, 0x92, 0x77, 0x02, 0x20, 0x85, 0xB7, 0x19, 0x18, 0x28, 0x4D, 0x05, 0x19, 0x9F, 0xA1,
                0xF1, 0x08, 0xC0, 0x05, 0x10, 0x57, 0x7C, 0x4F, 0x01, 0x10, 0xEF, 0xC5, 0xF8, 0xAC, 0x76, 0xC8,
                0x2E, 0x80, 0x14, 0x99, 0xE4, 0xFE, 0x44, 0x51, 0xB8, 0x52, 0x14, 0x3A, 0x32, 0x22, 0x00, 0x13,
                0x85, 0xBF, 0x52, 0xC6, 0x05, 0x8E, 0xE5, 0x63, 0x00, 0x86, 0xB6, 0x9C, 0x86, 0x38, 0xAB, 0x54,
                0x74, 0x18, 0x5B, 0x50, 0x58, 0x6D, 0xC4, 0xF3, 0x89, 0x6A, 0xC3, 0x61, 0x8E, 0xD9, 0x03, 0xA8,
                0x08, 0xA0, 0x55, 0xBB, 0x40, 0x40, 0x3E, 0x00, 0xD2, 0x53, 0x47, 0x94, 0x0E, 0x38, 0xD0, 0x7A,
                0x73, 0x64, 0x57, 0xF0, 0x16, 0xFE, 0x95, 0x82, 0x86, 0x1A, 0x4C, 0x4D, 0xE9, 0x68, 0xD5, 0xAE,
                0xB8, 0x00, 0xE2, 0x8C, 0xDF, 0x4B, 0xE4, 0xD7, 0xC1, 0xB3, 0x4C, 0x75, 0xC2, 0x36, 0xD2, 0x3F,
                0x2A, 0x7C, 0xF7, 0x0C, 0x50, 0x60, 0xB1, 0x4A, 0x81, 0x18, 0x88, 0xD3, 0x22, 0x75, 0xD1, 0x63,
                0x5C, 0x80, 0xF7, 0x19, 0x15, 0xA2, 0xA5, 0xB9, 0xB5, 0x5A, 0xB7, 0xA4, 0x34, 0x7D, 0x03, 0x48,
                0x5F, 0x17, 0x90, 0x52, 0x01, 0x19, 0x95, 0x9E, 0x1E, 0xD1, 0x30, 0x30, 0x9A, 0x21, 0xD7, 0x0D,
                0x81, 0xB3, 0xC1, 0x92, 0x0C, 0xE7, 0xD4, 0x1B, 0xBE, 0x49, 0xF2, 0x04, 0x15, 0x2A, 0x52, 0x06,
                0x69, 0x31, 0xCA, 0xB3, 0x22, 0x71, 0xBD, 0x1F, 0x00, 0x4B, 0x82, 0x66, 0xB5, 0xA7, 0x37, 0xCF,
                0x6F, 0x78, 0x0F, 0xF8, 0x5D, 0xC6, 0xA4, 0xAC, 0xF7, 0x23, 0x05, 0x6C, 0xE4, 0x4E, 0xE2, 0xE3,
                0x95, 0xB7, 0xD3, 0x40, 0xF3, 0xA5, 0x06, 0x1C, 0xFE, 0x1F, 0x09, 0x2A, 0xA8, 0xF5, 0xE6, 0x3D,
                0x00, 0xDD, 0xAD, 0x02, 0x2D, 0xC4, 0x4D, 0x66, 0xA0, 0x6A, 0x1F, 0xD5, 0x2E, 0xF8, 0x8F, 0xFF,
                0x2D, 0xC6, 0x4F, 0x04, 0x1E, 0x14, 0xD0, 0xAC, 0x01, 0x3C, 0xAA, 0x5C, 0x1F, 0xA9, 0x2E, 0x72,
                0xBA, 0x49, 0xB5, 0xC7, 0xFA, 0xC0, 0x27, 0xD2, 0x62, 0x69, 0xAE, 0xA7, 0xC8, 0x04, 0xEA, 0x0F,
                0xBF, 0x1A, 0x51, 0x50, 0x61, 0x16, 0x8F, 0x1B, 0xD5, 0x5E, 0x03, 0x75, 0x35, 0xDD, 0x09, 0x6F,
                0x88, 0xC4, 0x0D, 0x73, 0x07, 0x82, 0x61, 0x88, 0xE8, 0x59, 0x30, 0x45, 0x8E, 0xD4, 0x7A, 0xA7,
                0xBD, 0xDA, 0x07, 0x67, 0x81, 0x40, 0x30, 0x88, 0x55, 0xF5, 0x11, 0x05, 0xF0, 0x58, 0x94, 0x9B,
                0x48, 0xEC, 0x60, 0xF1, 0x09, 0xC7, 0xF1, 0x66, 0xFC, 0xDF, 0x0E, 0x84, 0x7F, 0x74, 0x1C, 0x8F,
                0x58, 0x44, 0x77, 0xAC, 0x59, 0xB5, 0xD7, 0x67, 0x00, 0x12, 0x85, 0x4F, 0x2A, 0x4E, 0x17, 0xBB,
                0x1F, 0xC6, 0x00, 0xB8, 0x99, 0xB0, 0xE7, 0x23, 0x9D, 0xF7, 0xCF, 0x6E, 0x44, 0x83, 0x4A, 0x45,
                0x32, 0x40, 0x86, 0x81, 0x7C, 0x8D, 0xBA, 0xAB, 0x1C, 0xA7, 0xDE, 0x09, 0x87, 0x48, 0x21, 0x26,
                0x5F, 0x4A, 0xAD, 0xBA, 0x6E, 0x4F, 0xCA, 0xFB, 0x23, 0xB7, 0x62, 0xF7, 0xCA, 0xAD, 0x58, 0x22,
                0xC1, 0x00, 0x47, 0x9F, 0x0B, 0x7C, 0xCA, 0x73, 0xC1, 0xDB, 0x9F, 0x8C, 0xF2, 0x17, 0x1E, 0x4E,
                0xDF, 0xF2, 0x6C, 0xF8, 0x67, 0xAF, 0x22, 0x7B, 0xF3, 0xEB, 0x4B, 0x80, 0x01, 0x00, 0xB8, 0x21,
                0x72, 0x89, 0x08, 0x10, 0x07, 0x7D, 0x00, 0x00, 0x00, 0x00, 0x49, 0x45, 0x4E, 0x44, 0xAE, 0x42,
                0x60, 0x82
            };

        private static CCTexture2D _defaultTexture;

        public static CCTexture2D DefaultTexture
        {
            get
            {
                if (_defaultTexture == null)
                {
                    _defaultTexture = CCTextureCache.SharedTextureCache.AddImage(_firePngData, "__firePngData", SurfaceFormat.Color);
                }

                return _defaultTexture;
            }
        }
    }

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

                Texture = CCParticleExample.DefaultTexture;

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

                Texture = CCParticleExample.DefaultTexture;

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

                Texture = CCParticleExample.DefaultTexture;

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

                Texture = CCParticleExample.DefaultTexture;

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

                Texture = CCParticleExample.DefaultTexture;

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

                Texture = CCParticleExample.DefaultTexture;

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

                Texture = CCParticleExample.DefaultTexture;

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

                Texture = CCParticleExample.DefaultTexture;

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

                Texture = CCParticleExample.DefaultTexture;

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

                Texture = CCParticleExample.DefaultTexture;

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

                Texture = CCParticleExample.DefaultTexture;

                return true;
            }
            return false;
        }
    }
}