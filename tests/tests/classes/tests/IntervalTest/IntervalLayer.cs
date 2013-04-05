using cocos2d;

namespace tests
{
    public class IntervalLayer : CCLayer
    {
        protected CCLabelBMFont m_label0;
        protected CCLabelBMFont m_label1;
        protected CCLabelBMFont m_label2;
        protected CCLabelBMFont m_label3;
        protected CCLabelBMFont m_label4;

        private float m_time0, m_time1, m_time2, m_time3, m_time4;
        
        private const string s_pPathGrossini = "Images/grossini";

        public IntervalLayer()
        {
            m_time0 = m_time1 = m_time2 = m_time3 = m_time4 = 0.0f;

            CCSize s = CCDirector.SharedDirector.WinSize;

            // sun
            CCParticleSystem sun = CCParticleSun.Create();
            sun.Texture = CCTextureCache.SharedTextureCache.AddImage("Images/fire");
            sun.Position = (new CCPoint(s.Width - 32, s.Height - 32));

            sun.TotalParticles = 130;
            sun.Life = (0.6f);
            AddChild(sun);

            // timers
            m_label0 = CCLabelBMFont.Create("0", "fonts/bitmapFontTest4.fnt");
            m_label1 = CCLabelBMFont.Create("0", "fonts/bitmapFontTest4.fnt");
            m_label2 = CCLabelBMFont.Create("0", "fonts/bitmapFontTest4.fnt");
            m_label3 = CCLabelBMFont.Create("0", "fonts/bitmapFontTest4.fnt");
            m_label4 = CCLabelBMFont.Create("0", "fonts/bitmapFontTest4.fnt");

            ScheduleUpdate();

            Schedule(step1);
            Schedule(step2, 0);
            Schedule(step3, 1.0f);
            Schedule(step4, 2.0f);

            m_label0.Position = new CCPoint(s.Width * 1 / 6, s.Height / 2);
            m_label1.Position = new CCPoint(s.Width * 2 / 6, s.Height / 2);
            m_label2.Position = new CCPoint(s.Width * 3 / 6, s.Height / 2);
            m_label3.Position = new CCPoint(s.Width * 4 / 6, s.Height / 2);
            m_label4.Position = new CCPoint(s.Width * 5 / 6, s.Height / 2);

            AddChild(m_label0);
            AddChild(m_label1);
            AddChild(m_label2);
            AddChild(m_label3);
            AddChild(m_label4);

            // Sprite
            CCSprite sprite = new CCSprite(s_pPathGrossini);
            sprite.Position = new CCPoint(40, 50);

            CCJumpBy jump = new CCJumpBy (3, new CCPoint(s.Width - 80, 0), 50, 4);

            AddChild(sprite);
            sprite.RunAction(new CCRepeatForever (
                (CCActionInterval) (CCSequence.FromActions(jump, jump.Reverse())))
                );

            // pause button
            CCMenuItem item1 = CCMenuItemFont.Create("Pause", onPause);
            CCMenu menu = CCMenu.Create(item1);
            menu.Position = new CCPoint(s.Width / 2, s.Height - 50);

            AddChild(menu);
        }

        public void onPause(object pSender)
        {
            if (CCDirector.SharedDirector.IsPaused)
                CCDirector.SharedDirector.Resume();
            else
                CCDirector.SharedDirector.Pause();
        }

        public void step1(float dt)
        {
            m_time1 += dt;

            string str = string.Format("{0,3:f1}", m_time1);
            m_label1.Label = (str);
        }

        public void step2(float dt)
        {
            m_time2 += dt;

            string str = string.Format("{0,3:f1}", m_time2);
            m_label2.Label = (str);
        }

        public void step3(float dt)
        {
            m_time3 += dt;
            string str = string.Format("{0,3:f1}", m_time3);
            m_label3.Label = (str);
        }

        public void step4(float dt)
        {
            m_time4 += dt;
            string str = string.Format("{0,3:f1}", m_time4);
            m_label4.Label = (str);
        }

        public override void Update(float dt)
        {
            m_time0 += dt;

            string str = string.Format("{0,3:f1}", m_time0);
            m_label0.Label = (str);
        }
    }
}