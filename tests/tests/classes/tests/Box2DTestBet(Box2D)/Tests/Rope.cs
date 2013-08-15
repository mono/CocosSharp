using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Box2D.Common;
using Box2D.Rope;

namespace Box2D.TestBed.Tests
{
    public class Rope : Test
    {
        public Rope()
        {
            const int N = 40;
            b2Vec2[] vertices = new b2Vec2[N];
            float[] masses = new float[N];

            for (int i = 0; i < N; ++i)
            {
                vertices[i].Set(0.0f, 20.0f - 0.25f * i);
                masses[i] = 1.0f;
            }
            masses[0] = 0.0f;
            masses[1] = 0.0f;

            b2RopeDef def = new b2RopeDef();
            def.vertices = vertices;
            def.count = N;
            def.gravity.Set(0.0f, -10.0f);
            def.masses = masses;
            def.damping = 0.1f;
            def.k2 = 1.0f;
            def.k3 = 0.5f;

            m_rope.Initialize(def);

            m_angle = 0.0f;
            m_rope.SetAngle(m_angle);
        }

        public override void Keyboard(char key)
        {
            switch (key)
            {
                case 'q':
                    m_angle = Math.Max(-b2Settings.b2_pi, m_angle - 0.05f * b2Settings.b2_pi);
                    m_rope.SetAngle(m_angle);
                    break;

                case 'e':
                    m_angle = Math.Min(b2Settings.b2_pi, m_angle + 0.05f * b2Settings.b2_pi);
                    m_rope.SetAngle(m_angle);
                    break;
            }
        }

        protected override void Draw(Settings settings)
        {
            base.Draw(settings);

            m_rope.Draw(m_debugDraw);

            m_debugDraw.DrawString(5, m_textLine, "Press (q,e) to adjust target angle");
            m_textLine += 15;
            m_debugDraw.DrawString(5, m_textLine, "Target angle = {0} degrees", m_angle * 180.0f / b2Settings.b2_pi);
            m_textLine += 15;
        }

        public override void Step(Settings settings)
        {
            float dt = settings.hz > 0.0f ? 1.0f / settings.hz : 0.0f;

            if (settings.pause && settings.singleStep)
            {
                dt = 0.0f;
            }

            m_rope.Step(dt, 1);

            base.Step(settings);
        }

        public b2Rope m_rope = new b2Rope();
        public float m_angle;
    }
}
