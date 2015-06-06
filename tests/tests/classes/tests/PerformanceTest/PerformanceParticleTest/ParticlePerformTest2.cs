using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class ParticlePerformTest2 : ParticleMainScene
    {

        public override string title()
        {
            //char str[20] = {0};
            string str;
            //sprintf(str, "B (%d) size=8", subtestNumber);
            str = string.Format("B {0:D} size=8", subtestNumber);
            string strRet = str;
            return strRet;
        }

        public override void doTest()
        {
            CCSize s = Layer.VisibleBoundsWorldspace.Size;
            var particleSystem = (CCParticleSystemQuad)GetChildByTag(PerformanceParticleTest.kTagParticleSystem);

            // duration
            particleSystem.Duration = -1;

            // gravity
            particleSystem.Gravity = (new CCPoint(0, -90));

            // angle
            particleSystem.Angle = 90;
            particleSystem.AngleVar = 0;

            // radial
            particleSystem.RadialAccel = (0);
            particleSystem.RadialAccelVar = (0);

            // speed of particles
            particleSystem.Speed = (180);
            particleSystem.SpeedVar = (50);

            // emitter position
            particleSystem.Position = new CCPoint(s.Width / 2, 100);
            particleSystem.PositionVar = new CCPoint(s.Width / 2, 0);

            // life of particles
            particleSystem.Life = 2.0f;
            particleSystem.LifeVar = 1;

            // emits per frame
            particleSystem.EmissionRate = particleSystem.TotalParticles / particleSystem.Life;

            // color of particles
            CCColor4F startColor = new CCColor4F { R = 0.5f, G = 0.5f, B = 0.5f, A = 1.0f };
            particleSystem.StartColor = startColor;

            CCColor4F startColorVar = new CCColor4F { R = 0.5f, G = 0.5f, B = 0.5f, A = 1.0f };
            particleSystem.StartColor = startColorVar;

            CCColor4F endColor = new CCColor4F { R = 0.1f, G = 0.1f, B = 0.1f, A = 0.2f };
            particleSystem.EndColor = endColor;

            CCColor4F endColorVar = new CCColor4F { R = 0.1f, G = 0.1f, B = 0.1f, A = 0.2f };
            particleSystem.EndColorVar = endColorVar;

            // size, in pixels
            particleSystem.EndSize = 8.0f;
            particleSystem.StartSize = 8.0f;
            particleSystem.EndSizeVar = 0;
            particleSystem.StartSizeVar = 0;

            // additive
            particleSystem.BlendAdditive = false;
        }
    }
}
