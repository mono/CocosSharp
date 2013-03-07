using System;
using Microsoft.Xna.Framework;

namespace cocos2d
{
    public class CCRipple3D : CCGrid3DAction
    {
        protected float m_fAmplitude;
        protected float m_fAmplitudeRate;
        protected float m_fRadius;
        protected int m_nWaves;
        protected CCPoint m_position;
        protected CCPoint m_positionInPixels;

        public CCPoint Position
        {
            get { return m_position; }
            set
            {
                m_position = value;
                m_positionInPixels.X = value.X * CCDirector.SharedDirector.ContentScaleFactor;
                m_positionInPixels.Y = value.Y * CCDirector.SharedDirector.ContentScaleFactor;
            }
        }

        public float Amplitude
        {
            get { return m_fAmplitude; }
            set { m_fAmplitude = value; }
        }

        public override float AmplitudeRate
        {
            set { m_fAmplitudeRate = value; }
            get { return m_fAmplitudeRate; }
        }

        public bool InitWithPosition(CCPoint pos, float r, int wav, float amp,
                                     CCGridSize gridSize, float duration)
        {
            if (base.InitWithSize(gridSize, duration))
            {
                m_positionInPixels = new CCPoint();

                Position = pos;
                m_fRadius = r;
                m_nWaves = wav;
                m_fAmplitude = amp;
                m_fAmplitudeRate = 1.0f;

                return true;
            }

            return false;
        }

        public override CCObject CopyWithZone(CCZone pZone)
        {
            CCRipple3D pCopy;
            if (pZone != null && pZone.m_pCopyObject != null)
            {
                //in case of being called at sub class
                pCopy = (CCRipple3D) (pZone.m_pCopyObject);
            }
            else
            {
                pCopy = new CCRipple3D();
                pZone = new CCZone(pCopy);
            }

            base.CopyWithZone(pZone);
            pCopy.InitWithPosition(m_position, m_fRadius, m_nWaves, m_fAmplitude, m_sGridSize, m_fDuration);

            return pCopy;
        }

        public override void Update(float time)
        {
            int i, j;

            CCGridSize gs;

            for (i = 0; i < (m_sGridSize.X + 1); ++i)
            {
                for (j = 0; j < (m_sGridSize.Y + 1); ++j)
                {
                    gs.X = i;
                    gs.Y = j;

                    CCVertex3F v = OriginalVertex(gs);

                    float x = m_positionInPixels.X - v.X;
                    float y = m_positionInPixels.Y - v.Y;

                    var r = (float) Math.Sqrt((x * x + y * y));

                    if (r < m_fRadius)
                    {
                        r = m_fRadius - r;
                        float r1 = r / m_fRadius;
                        float rate = r1 * r1;
                        v.Z += ((float) Math.Sin(time * MathHelper.Pi * m_nWaves * 2 + r * 0.1f) * m_fAmplitude * m_fAmplitudeRate * rate);
                    }

                    SetVertex(gs, ref v);
                }
            }
        }

        public static CCRipple3D Create(CCPoint pos, float r, int wav, float amp, CCGridSize gridSize, float duration)
        {
            var pAction = new CCRipple3D();
            pAction.InitWithPosition(pos, r, wav, amp, gridSize, duration);
            return pAction;
        }
    }
}