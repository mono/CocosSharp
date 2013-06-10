using System;

namespace Cocos2D
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
            get { return m_fAmplitudeRate; }
            set { m_fAmplitudeRate = value; }
        }

        protected virtual bool initWithDuration(float duration, CCGridSize gridSize, CCPoint position, int twirls,
                                                float amplitude)
        {
            if (base.InitWithDuration(duration, gridSize))
            {
                m_positionInPixels = new CCPoint();
                Position = position;
                m_nTwirls = twirls;
                m_fAmplitude = amplitude;
                m_fAmplitudeRate = 1.0f;

                return true;
            }

            return false;
        }

        public override object Copy(ICCCopyable pZone)
        {
            CCTwirl pCopy;
            if (pZone != null)
            {
                //in case of being called at sub class
                pCopy = (CCTwirl) (pZone);
            }
            else
            {
                pCopy = new CCTwirl();
                pZone = (pCopy);
            }

            base.Copy(pZone);

            pCopy.initWithDuration(m_fDuration, m_sGridSize, m_position, m_nTwirls, m_fAmplitude);
            return pCopy;
        }

        public override void Update(float time)
        {
            int i, j;
            CCPoint c = m_positionInPixels;

            for (i = 0; i < (m_sGridSize.X + 1); ++i)
            {
                for (j = 0; j < (m_sGridSize.Y + 1); ++j)
                {
                    CCVertex3F v = OriginalVertex(new CCGridSize(i, j));

                    var avg = new CCPoint(i - (m_sGridSize.X / 2.0f), j - (m_sGridSize.Y / 2.0f));
                    var r = (float) Math.Sqrt((avg.X * avg.X + avg.Y * avg.Y));

                    float amp = 0.1f * m_fAmplitude * m_fAmplitudeRate;
                    float a = r * (float) Math.Cos((float) Math.PI / 2.0f + time * (float) Math.PI * m_nTwirls * 2) *
                              amp;

                    float dx = (float) Math.Sin(a) * (v.Y - c.Y) + (float) Math.Cos(a) * (v.X - c.X);
                    float dy = (float) Math.Cos(a) * (v.Y - c.Y) - (float) Math.Sin(a) * (v.X - c.X);

                    v.X = c.X + dx;
                    v.Y = c.Y + dy;

                    SetVertex(new CCGridSize(i, j), ref v);
                }
            }
        }

        protected CCTwirl()
        {
        }

        public CCTwirl(float duration, CCGridSize gridSize, CCPoint position, int twirls, float amplitude)
            : base(duration)
        {
            initWithDuration(duration, gridSize, position, twirls, amplitude);
        }
    }
}