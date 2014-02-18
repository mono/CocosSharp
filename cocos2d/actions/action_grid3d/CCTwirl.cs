using System;

namespace CocosSharp
{
    public class CCTwirl : CCGrid3DAction
    {
        CCPoint position;

        public float Amplitude { get; private set; }
        public override float AmplitudeRate { get; protected set; }
        public int Twirls { get; private set; }
        public CCPoint PositionInPixels { get; private set; }
        public CCPoint Position
        {
            get { return position; }
            set
            {
                position = value;
                PositionInPixels = value * CCDirector.SharedDirector.ContentScaleFactor;
            }
        }


        #region Constructors

		public CCTwirl(float duration, CCGridSize gridSize)
			: this(duration, gridSize, CCPoint.Zero)
		{ }

		public CCTwirl(float duration, CCGridSize gridSize, CCPoint position, int twirls= 0, float amplitude = 0) : base(duration, gridSize)
        {
            Position = position;
            Twirls = twirls;
            Amplitude = amplitude;
            AmplitudeRate = 1.0f;
        }

        #endregion Constructors


        protected internal override CCActionState StartAction(CCNode target)
        {
            return new CCTwirlState(this, target);
        }
    }


    #region Action state

    public class CCTwirlState : CCGrid3DActionState
    {
        protected float CachedAmplitude { get; private set; }
        protected int CachedTwirls { get; private set; }
        protected CCPoint CachedPositionInPixels { get; private set; }

        public CCTwirlState(CCTwirl action, CCNode target) : base(action, target)
        {
            CachedAmplitude = action.Amplitude;
            CachedTwirls = action.Twirls;
            CachedPositionInPixels = action.PositionInPixels;
        }

        public override void Update(float time)
        {
            int i, j;
            CCPoint avg = CCPoint.Zero;
            CCPoint c = CachedPositionInPixels;
            int twirls = CachedTwirls;

            for (i = 0; i < (CachedGridSize.X + 1); ++i)
            {
                for (j = 0; j < (CachedGridSize.Y + 1); ++j)
                {
                    CCVertex3F v = OriginalVertex(i,j);

                    avg.X = i - (CachedGridSize.X / 2.0f);
                    avg.Y = j - (CachedGridSize.Y / 2.0f);

                    var r = (float) Math.Sqrt((avg.X * avg.X + avg.Y * avg.Y));

                    float amp = 0.1f * CachedAmplitude * StateAmplitudeRate;
                    float a = r * (float) Math.Cos((float) Math.PI / 2.0f + time * (float) Math.PI * twirls * 2) * amp;

                    float dx = (float) Math.Sin(a) * (v.Y - c.Y) + (float) Math.Cos(a) * (v.X - c.X);
                    float dy = (float) Math.Cos(a) * (v.Y - c.Y) - (float) Math.Sin(a) * (v.X - c.X);

                    v.X = c.X + dx;
                    v.Y = c.Y + dy;

                    SetVertex(i,j, ref v);
                }
            }
        }

    }

    #endregion Action state
}