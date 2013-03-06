using System;

namespace cocos2d
{
    public class CCTwirl : CCGrid3DAction
    {
        protected float m_fAmplitude;
        protected float m_fAmplitudeRate;
        protected int m_nTwirls;
        protected CCPoint m_position;
        protected CCPoint m_positionInPixels;

        public CCPoint Position
        {
            get { return m_position; }
            set
            {
                m_position = value;
                m_positionInPixels.x = value.x * CCDirector.SharedDirector.ContentScaleFactor;
                m_positionInPixels.y = value.y * CCDirector.SharedDirector.ContentScaleFactor;
            }
        }

        public float Amplitude
        {
            get { return m_fAmplitude; }
            set { m_fAmplitude = value; }
        }

        public override float AmplitudeRate
        {
            get { return m_fAmplitudeRate; }
            set { m_fAmplitudeRate = value; }
        }

        public bool InitWithPosition(CCPoint pos, int t, float amp, ccGridSize gridSize,
                                     float duration)
        {
            if (base.InitWithSize(gridSize, duration))
            {
                m_positionInPixels = new CCPoint();
                Position = pos;
                m_nTwirls = t;
                m_fAmplitude = amp;
                m_fAmplitudeRate = 1.0f;

                return true;
            }

            return false;
        }

        public override CCObject CopyWithZone(CCZone pZone)
        {
            CCTwirl pCopy;
            if (pZone != null && pZone.m_pCopyObject != null)
            {
                //in case of being called at sub class
                pCopy = (CCTwirl) (pZone.m_pCopyObject);
            }
            else
            {
                pCopy = new CCTwirl();
                pZone = new CCZone(pCopy);
            }

            base.CopyWithZone(pZone);

            pCopy.InitWithPosition(m_position, m_nTwirls, m_fAmplitude, m_sGridSize, m_fDuration);
            return pCopy;
        }

        public override void Update(float time)
        {
            int i, j;
            CCPoint c = m_positionInPixels;

            for (i = 0; i < (m_sGridSize.x + 1); ++i)
            {
                for (j = 0; j < (m_sGridSize.y + 1); ++j)
                {
                    CCVertex3F v = OriginalVertex(new ccGridSize(i, j));

                    var avg = new CCPoint(i - (m_sGridSize.x / 2.0f), j - (m_sGridSize.y / 2.0f));
                    var r = (float) Math.Sqrt((avg.x * avg.x + avg.y * avg.y));

                    float amp = 0.1f * m_fAmplitude * m_fAmplitudeRate;
                    float a = r * (float) Math.Cos((float) Math.PI / 2.0f + time * (float) Math.PI * m_nTwirls * 2) * amp;

                    float dx = (float) Math.Sin(a) * (v.Y - c.y) + (float) Math.Cos(a) * (v.X - c.x);
                    float dy = (float) Math.Cos(a) * (v.Y - c.y) - (float) Math.Sin(a) * (v.X - c.x);

                    v.X = c.x + dx;
                    v.Y = c.y + dy;

                    SetVertex(new ccGridSize(i, j), ref v);
                }
            }
        }

        public static CCTwirl Create(CCPoint pos, int t, float amp, ccGridSize gridSize, float duration)
        {
            var pAction = new CCTwirl();
            pAction.InitWithPosition(pos, t, amp, gridSize, duration);
            return pAction;
        }
    }
}