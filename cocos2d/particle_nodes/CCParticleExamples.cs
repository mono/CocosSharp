
using System.IO;
using Microsoft.Xna.Framework.Graphics;

namespace CocosSharp
{

    internal static class CCParticleExample
    {
        static byte[] firePngData =
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

        static CCTexture2D defaultTexture;


        public static CCTexture2D DefaultTexture
        {
            get
            {
                if (defaultTexture == null)
                {
					defaultTexture = CCTextureCache.SharedTextureCache.AddImage(firePngData, "__firePngData", CCSurfaceFormat.Color);
                }

                return defaultTexture;
            }
        }
    }

    //
    // ParticleFire
    //
    public class CCParticleFire : CCParticleSystemQuad
    {

		private static CCParticleSystemConfig config;

        public CCParticleFire() : base(250)
        {
            CCSize winSize = CCDirector.SharedDirector.WinSize;

			if (config == null) 
			{
				config = new CCParticleSystemConfig ();
				config.ParticleSystemType = CCParticleSystemType.Internal;

				config.Duration = ParticleDurationInfinity;
				config.Life = 3;
				config.LifeVar = 0.25f;
				config.Position = new CCPoint(winSize.Width / 2, 60);
				config.PositionVar = new CCPoint(40, 20);
				config.Angle = 90;
				config.AngleVar = 10;
				config.StartSize = 54.0f;
				config.StartSizeVar = 10.0f;
				config.EndSize = ParticleStartSizeEqualToEndSize;

				config.EmitterMode = CCEmitterMode.Gravity;

				CCColor4F cstartColor = new CCColor4F();
				cstartColor.R = 0.76f;
				cstartColor.G = 0.25f;
				cstartColor.B = 0.12f;
				cstartColor.A = 1.0f;
				config.StartColor = cstartColor;

				CCColor4F cstartColorVar = new CCColor4F();
				cstartColorVar.R = 0.0f;
				cstartColorVar.G = 0.0f;
				cstartColorVar.B = 0.0f;
				cstartColorVar.A = 0.0f;
				config.StartColorVar = cstartColorVar;

				CCColor4F cendColor = new CCColor4F();
				cendColor.R = 0.0f;
				cendColor.G = 0.0f;
				cendColor.B = 0.0f;
				cendColor.A = 1.0f;
				config.EndColor = cendColor;

				CCColor4F cendColorVar = new CCColor4F();
				cendColorVar.R = 0.0f;
				cendColorVar.G = 0.0f;
				cendColorVar.B = 0.0f;
				cendColorVar.A = 0.0f;
				config.EndColorVar = cendColorVar;

				config.Gravity = new CCPoint(0, 0);
				config.GravityRadialAccel = 0;
				config.GravityRadialAccelVar = 0;
				config.GravitySpeed = 60;
				config.GravitySpeedVar = 20;
				config.EmitterMode = CCEmitterMode.Gravity;

				config.Texture = CCParticleExample.DefaultTexture;

			}


			Duration = config.Duration;
			Life = config.Life;
			LifeVar = config.LifeVar;
			Position = config.Position;
			PositionVar = config.PositionVar;
			Angle = config.Angle;
			AngleVar = config.AngleVar;
			StartSize = config.StartSize;
			StartSizeVar = config.StartSizeVar;
			EndSize = config.EndSize;

			EmitterMode = config.EmitterMode;
            EmissionRate = TotalParticles / Life;

            CCColor4F startColor = new CCColor4F();
			startColor.R = config.StartColor.R;
			startColor.G = config.StartColor.G;
			startColor.B = config.StartColor.B;
			startColor.A = config.StartColor.A;
            StartColor = startColor;

            CCColor4F startColorVar = new CCColor4F();
			startColorVar.R = config.StartColorVar.R;
			startColorVar.G = config.StartColorVar.G;
			startColorVar.B = config.StartColorVar.B;
			startColorVar.A = config.StartColorVar.A;
            StartColorVar = startColorVar;

            CCColor4F endColor = new CCColor4F();
			endColor.R = config.EndColor.R;
			endColor.G = config.EndColor.G;
			endColor.B = config.EndColor.B;
			endColor.A = config.EndColor.A;
            EndColor = endColor;

            CCColor4F endColorVar = new CCColor4F();
			endColorVar.R = config.EndColorVar.R;
			endColorVar.G = config.EndColorVar.G;
			endColorVar.B = config.EndColorVar.B;
			endColorVar.A = config.EndColorVar.A;
            EndColorVar = endColorVar;

            GravityMoveMode gravityMode = new GravityMoveMode();
			gravityMode.Gravity = new CCPoint(config.Gravity);
			gravityMode.RadialAccel = config.GravityRadialAccel;
			gravityMode.RadialAccelVar = config.GravityRadialAccelVar;
			gravityMode.Speed = config.GravitySpeed;
			gravityMode.SpeedVar = config.GravitySpeedVar;
            GravityMode = gravityMode;

			BlendAdditive = true;

			Texture = config.Texture;
        }
    }

    //
    // ParticleFireworks
    //
    public class CCParticleFireworks : CCParticleSystemQuad
    {
		private static CCParticleSystemConfig config;

        public CCParticleFireworks() : base(1500)
        {
            CCSize winSize = CCDirector.SharedDirector.WinSize;

            Duration = ParticleDurationInfinity;
            Life = 3.5f;
            LifeVar = 1;
            Position = new CCPoint (winSize.Width / 2, winSize.Height / 2);
            Angle = 90;
            AngleVar = 20;
            StartSize = 8.0f;
            StartSizeVar = 2.0f;
            EndSize = ParticleStartSizeEqualToEndSize;

            EmissionRate = TotalParticles / Life;
            EmitterMode = CCEmitterMode.Gravity;

            StartColor = new CCColor4F (0.5f, 0.5f, 0.5f, 1.0f);

            CCColor4F startColorVar = new CCColor4F();
            startColorVar.R = 0.5f;
            startColorVar.G = 0.5f;
            startColorVar.B = 0.5f;
            startColorVar.A = 0.1f;
            StartColorVar = startColorVar;

            CCColor4F endColor = new CCColor4F();
            endColor.R = 0.1f;
            endColor.G = 0.1f;
            endColor.B = 0.1f;
            endColor.A = 0.2f;
            EndColor = endColor;

            CCColor4F endColorVar = new CCColor4F();
            endColorVar.R = 0.1f;
            endColorVar.G = 0.1f;
            endColorVar.B = 0.1f;
            endColorVar.A = 0.2f;
            EndColorVar = endColorVar;

            GravityMoveMode gravityMode = new GravityMoveMode();
            gravityMode.Gravity = new CCPoint (0, -90);
            gravityMode.RadialAccel = 0;
            gravityMode.RadialAccelVar = 0;
            gravityMode.Speed = 180;
            gravityMode.SpeedVar = 50;
            GravityMode = gravityMode;

            BlendAdditive = false;

            Texture = CCParticleExample.DefaultTexture;
        }
    }

    //
    // ParticleSun
    //
    public class CCParticleSun : CCParticleSystemQuad
    {
        public CCParticleSun() : this(350)
        { 
        }
        
        public CCParticleSun (int num) : base(num)
        {
            CCSize winSize = CCDirector.SharedDirector.WinSize;

            Duration = ParticleDurationInfinity;
            Life = 1;
            LifeVar = 0.5f;
            Position = new CCPoint(winSize.Width / 2, winSize.Height / 2);
            PositionVar = CCPoint.Zero;
            Angle = 90;
            AngleVar = 360;
            StartSize = 30.0f;
            StartSizeVar = 10.0f;
            EndSize = ParticleStartSizeEqualToEndSize;

            EmissionRate = TotalParticles / Life;
            EmitterMode = CCEmitterMode.Gravity;

            StartColor = new CCColor4F(0.76f, 0.25f, 0.12f, 1.0f);
            StartColorVar = new CCColor4F();
            EndColor = new CCColor4F(0.0f, 0.0f, 0.0f, 1.0f);
            EndColorVar = new CCColor4F();

            GravityMoveMode gravityMode = new GravityMoveMode();
            gravityMode.Gravity = new CCPoint(0, 0);
            gravityMode.RadialAccel = 0;
            gravityMode.RadialAccelVar = 0;
            gravityMode.Speed = 20;
            gravityMode.SpeedVar = 5;
            GravityMode = gravityMode;

            BlendAdditive = true;

            Texture = CCParticleExample.DefaultTexture;
        }
    }

    //
    // ParticleGalaxy
    //
    public class CCParticleGalaxy : CCParticleSystemQuad
    {
        public CCParticleGalaxy() : base(200)
        {
            CCSize winSize = CCDirector.SharedDirector.WinSize;

            Duration = ParticleDurationInfinity;
            Life = 4;
            LifeVar = 1;
            Position = new CCPoint(winSize.Width / 2, winSize.Height / 2);
            PositionVar = CCPoint.Zero;
            Angle = 90;
            AngleVar = 360;
            StartSize = 37.0f;
            StartSizeVar = 10.0f;
            EndSize = ParticleStartSizeEqualToEndSize;

            EmitterMode = CCEmitterMode.Gravity;
            EmissionRate = TotalParticles / Life;

            StartColor = new CCColor4F(0.12f, 0.25f, 0.76f, 1.0f);
            StartColorVar = new CCColor4F();
            EndColor = new CCColor4F(0.0f, 0.0f, 0.0f, 1.0f);
            EndColorVar = new CCColor4F();

            GravityMoveMode gravityMode = new GravityMoveMode();
            gravityMode.Gravity = new CCPoint(0, 0);
            gravityMode.RadialAccel = -80;
            gravityMode.RadialAccelVar = 0;
            gravityMode.Speed = 60;
            gravityMode.SpeedVar = 10;
            gravityMode.TangentialAccel = 80;
            gravityMode.TangentialAccelVar = 0;
            GravityMode = gravityMode;

            // additive
            BlendAdditive = true;

            Texture = CCParticleExample.DefaultTexture;
        }
    }

    public class CCParticleFlower : CCParticleSystemQuad
    {
        public CCParticleFlower() : base(250)
        {
            CCSize winSize = CCDirector.SharedDirector.WinSize;

            Duration = ParticleDurationInfinity;
            Life = 4;
            LifeVar = 1;
            Position = new CCPoint(winSize.Width / 2, winSize.Height / 2);
            PositionVar = CCPoint.Zero;
            Angle = 90;
            AngleVar = 360;
            StartSize = 30.0f;
            StartSizeVar = 10.0f;
            EndSize = ParticleStartSizeEqualToEndSize;

            EmissionRate = TotalParticles / Life;
            EmitterMode = CCEmitterMode.Gravity;

            StartColor = new CCColor4F(0.5f, 0.5f, 0.5f, 1.0f);
            StartColorVar = new CCColor4F(0.5f, 0.5f, 0.5f, 0.5f);
            EndColor = new CCColor4F(0.0f, 0.0f, 0.0f, 1.0f);
            EndColorVar = new CCColor4F();

            GravityMoveMode gravityMode = new GravityMoveMode();
            gravityMode.Gravity = new CCPoint(0, 0);
            gravityMode.RadialAccel = -60;
            gravityMode.RadialAccelVar = 0;
            gravityMode.Speed = 80;
            gravityMode.SpeedVar = 10;
            gravityMode.TangentialAccel = 15;
            gravityMode.TangentialAccelVar = 0;
            GravityMode = gravityMode;

            BlendAdditive = true;

            Texture = CCParticleExample.DefaultTexture;
        }
    }

    public class CCParticleMeteor : CCParticleSystemQuad
    {
        public CCParticleMeteor() : base(150)
        {
            CCSize winSize = CCDirector.SharedDirector.WinSize;

            Duration = ParticleDurationInfinity;
            Life = 2;
            LifeVar = 1;
            Position = new CCPoint(winSize.Width / 2, winSize.Height / 2);
            PositionVar = CCPoint.Zero;
            Angle = 90;
            AngleVar = 360;
            StartSize = 60.0f;
            StartSizeVar = 10.0f;
            EndSize = ParticleStartSizeEqualToEndSize;

            EmissionRate = TotalParticles / Life;
            EmitterMode = CCEmitterMode.Gravity;

            StartColor = new CCColor4F(0.2f, 0.4f, 0.7f, 1.0f);
            StartColorVar = new CCColor4F(0.0f, 0.0f, 0.2f, 0.1f);
            EndColor = new CCColor4F(0.0f, 0.0f, 0.0f, 1.0f);
            EndColorVar = new CCColor4F();

            GravityMoveMode gravityMode = new GravityMoveMode();
            gravityMode.Gravity = new CCPoint(-200, 200);
            gravityMode.RadialAccel = 0;
            gravityMode.RadialAccelVar = 0;
            gravityMode.Speed = 15;
            gravityMode.SpeedVar = 5;
            gravityMode.TangentialAccel = 0;
            gravityMode.TangentialAccelVar = 0;
            GravityMode = gravityMode;

            BlendAdditive = true;

            Texture = CCParticleExample.DefaultTexture;
        }
    }

    public class CCParticleSpiral : CCParticleSystemQuad
    {
        public CCParticleSpiral() : base(500)
        {
            CCSize winSize = CCDirector.SharedDirector.WinSize;

            Duration = ParticleDurationInfinity;
            Position = new CCPoint(winSize.Width / 2, winSize.Height / 2);
            PositionVar = CCPoint.Zero;
            Life = 12;
            LifeVar = 0;
            Angle = 90;
            AngleVar = 0;
            StartSize = 20.0f;
            StartSizeVar = 0.0f;
            EndSize = ParticleStartSizeEqualToEndSize;

            EmissionRate = TotalParticles / Life;
            EmitterMode = CCEmitterMode.Gravity;

            StartColor = new CCColor4F(0.5f, 0.5f, 0.5f, 1.0f);
            StartColorVar = new CCColor4F(0.5f, 0.5f, 0.5f, 0.0f);
            EndColor = new CCColor4F(0.5f, 0.5f, 0.5f, 1.0f);
            EndColorVar = new CCColor4F(0.5f, 0.5f, 0.5f, 0.0f);

            GravityMoveMode gravityMode = new GravityMoveMode();
            gravityMode.Gravity = new CCPoint(0, 0);
            gravityMode.RadialAccel = -380;
            gravityMode.RadialAccelVar = 0;
            gravityMode.Speed = 150;
            gravityMode.SpeedVar = 0;
            gravityMode.TangentialAccel = 45;
            gravityMode.TangentialAccelVar = 0;
            GravityMode = gravityMode;

            BlendAdditive = false;

            Texture = CCParticleExample.DefaultTexture;
        }
    }

    public class CCParticleExplosion : CCParticleSystemQuad
    {
        public CCParticleExplosion() : base(700)
        {
            CCSize winSize = CCDirector.SharedDirector.WinSize;

            Duration = 0.1f;
            Life = 5.0f;
            LifeVar = 2;
            Position = new CCPoint(winSize.Width / 2, winSize.Height / 2);
            PositionVar = CCPoint.Zero;
            Angle = 90;
            AngleVar = 360;
            StartSize = 15.0f;
            StartSizeVar = 10.0f;
            EndSize = ParticleStartSizeEqualToEndSize;

            EmissionRate = TotalParticles / Duration;
            EmitterMode = CCEmitterMode.Gravity;

            StartColor = new CCColor4F(0.7f, 0.1f, 0.2f, 1.0f);
            StartColorVar = new CCColor4F(0.5f, 0.5f, 0.5f, 0.0f);
            EndColor = new CCColor4F(0.5f, 0.5f, 0.5f, 0.0f);
            EndColorVar = new CCColor4F(0.5f, 0.5f, 0.5f, 0.0f);

            GravityMoveMode gravityMode = new GravityMoveMode();
            gravityMode.Gravity = new CCPoint(0, 0);
            gravityMode.RadialAccel = 0;
            gravityMode.RadialAccelVar = 0;
            gravityMode.Speed = 70;
            gravityMode.SpeedVar = 40;
            gravityMode.TangentialAccel = 0;
            gravityMode.TangentialAccelVar = 0;
            GravityMode = gravityMode;

            BlendAdditive = false;

            Texture = CCParticleExample.DefaultTexture;
        }
    }

    public class CCParticleSmoke : CCParticleSystemQuad
    {
        public CCParticleSmoke() : base(200)
        {
            CCSize winSize = CCDirector.SharedDirector.WinSize;

            Duration = ParticleDurationInfinity;
            Life = 4;
            LifeVar = 1;
            Position = new CCPoint(winSize.Width / 2, 0);
            PositionVar = new CCPoint(20, 0);
            Angle = 90;
            AngleVar = 5;
            StartSize = 60.0f;
            StartSizeVar = 10.0f;
            EndSize = ParticleStartSizeEqualToEndSize;

            EmissionRate = TotalParticles / Life;
            EmitterMode = CCEmitterMode.Gravity;

            StartColor = new CCColor4F(0.8f, 0.8f, 0.8f, 1.0f);
            StartColorVar = new CCColor4F(0.02f, 0.02f, 0.02f, 0.0f);
            EndColor = new CCColor4F(0.0f, 0.0f, 0.0f, 1.0f);
            EndColorVar = new CCColor4F();

            GravityMoveMode gravityMode = new GravityMoveMode();
            gravityMode.Gravity = new CCPoint(0, 0);
            gravityMode.RadialAccel = 0;
            gravityMode.RadialAccelVar = 0;
            gravityMode.Speed = 25;
            gravityMode.SpeedVar = 10;
            GravityMode = gravityMode;

            BlendAdditive = false;

            Texture = CCParticleExample.DefaultTexture;
        }
    }

    public class CCParticleSnow : CCParticleSystemQuad
    {
        public CCParticleSnow() : base(700)
        {
            CCSize winSize = CCDirector.SharedDirector.WinSize;

            Duration = ParticleDurationInfinity;
            Life = 45;
            LifeVar = 15;
            Position = new CCPoint(winSize.Width / 2, winSize.Height + 10);
            PositionVar = new CCPoint(winSize.Width / 2, 0);
            Angle = -90;
            AngleVar = 5;
            StartSize = 10.0f;
            StartSizeVar = 5.0f;
            EndSize = ParticleStartSizeEqualToEndSize;

            EmissionRate = 10;
            EmitterMode = CCEmitterMode.Gravity;

            StartColor = new CCColor4F(1.0f, 1.0f, 1.0f, 1.0f);
            StartColorVar = new CCColor4F();
            EndColor = new CCColor4F(1.0f, 1.0f, 1.0f, 0.0f);
            EndColorVar = new CCColor4F();

            GravityMoveMode gravityMode = new GravityMoveMode();
            gravityMode.Gravity = new CCPoint(0, -1);
            gravityMode.RadialAccel = 0;
            gravityMode.RadialAccelVar = 1;
            gravityMode.TangentialAccel = 0;
            gravityMode.TangentialAccelVar = 1;
            gravityMode.Speed = 5;
            gravityMode.SpeedVar = 1;
            GravityMode = gravityMode;

            BlendAdditive = false;

            Texture = CCParticleExample.DefaultTexture;
        }
    }

    public class CCParticleRain : CCParticleSystemQuad
    {
        public CCParticleRain() : base(1000)
        {
            CCSize winSize = CCDirector.SharedDirector.WinSize;

            Duration = ParticleDurationInfinity;
            Position = new CCPoint(winSize.Width / 2, winSize.Height);
            PositionVar = new CCPoint(winSize.Width / 2, 0);
            Life = 4.5f;
            LifeVar = 0;
            Angle = -90;
            AngleVar = 5;
            StartSize = 4.0f;
            StartSizeVar = 2.0f;
            EndSize = ParticleStartSizeEqualToEndSize;

            EmitterMode = CCEmitterMode.Gravity;
            EmissionRate = 20;

            StartColor = new CCColor4F(0.7f, 0.8f, 1.0f, 1.0f);
            StartColorVar = new CCColor4F();
            EndColor = new CCColor4F(0.7f, 0.8f, 1.0f, 0.5f);
            EndColorVar = new CCColor4F();

            GravityMoveMode gravityMode = new GravityMoveMode();
            gravityMode.Gravity = new CCPoint(10, -10);
            gravityMode.RadialAccel = 0;
            gravityMode.RadialAccelVar = 1;
            gravityMode.TangentialAccel = 0;
            gravityMode.TangentialAccelVar = 1;
            gravityMode.Speed = 130;
            gravityMode.SpeedVar = 30;
            GravityMode = gravityMode;

            BlendAdditive = false;

            Texture = CCParticleExample.DefaultTexture;
        }
    }
}