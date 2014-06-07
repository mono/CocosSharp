using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;
using System.IO;

namespace tests
{
    public class SpriteMaskTest : SpriteTestDemo
    {
        private CCMaskedSprite grossini, ball;
        private CCSprite hit;

        public SpriteMaskTest()
        {
            grossini = new CCMaskedSprite("Images/grossini", GetCollisionMask("grossini"));
            ball = new CCMaskedSprite("Images/ball-hd", GetCollisionMask("ball-hd"));
            hit = new CCSprite("Images/Icon");

            CCSize s = CCApplication.SharedApplication.MainWindowDirector.WindowSizeInPoints;
            grossini.Position = new CCPoint(s.Width / 3f, s.Height / 2f);
            ball.Position = new CCPoint(s.Width * 2f / 3f, s.Height / 2f);
            AddChild(grossini, 1);
            AddChild(ball, 1);
            AddChild(hit, 5);
            hit.Visible = false;
            grossini.RunAction(new CCRepeatForever(new CCParallel(
                new CCRotateBy(9f, 360f), 
                new CCSequence(new CCMoveBy(3f, new CCPoint(-100f, 0)), new CCMoveBy(3f, new CCPoint(500f, 0f)), new CCMoveBy(3f, new CCPoint(-400f, 0)))
                )));
            ball.RunAction(new CCRepeatForever(new CCSequence(new CCMoveBy(1.5f, new CCPoint(-400f, 0)), new CCMoveBy(2f, new CCPoint(600f, 0f)), new CCMoveBy(1f, new CCPoint(-200f, 0)))));
            Schedule(new Action<float>(UpdateTest), .25f);
        }

        private void UpdateTest(float dt)
        {
            CCPoint where;
            if (grossini.CollidesWith(ball, out where))
            {
                hit.Position = where;
                hit.Visible = true;
            }
            else
            {
                hit.Visible = false;
            }
        }

        public override string title()
        {
            return "Sprite Collision Test";
        }

        #region Collision Mask Support Code

        private Stream GetEmbeddedResource(string name)
        {
#if !WINRT && !NETFX_CORE
            System.Reflection.Assembly assem = System.Reflection.Assembly.GetExecutingAssembly();
            Stream stream = assem.GetManifestResourceStream(name);
            if (stream == null)
            {
                stream = assem.GetManifestResourceStream("tests." + name);
            }
            return (stream);
#else
            return null;
#endif
        }

        private byte[] ReadCollisionMask(Stream stream)
        {
            MemoryStream ms = new MemoryStream();
            using (stream)
            {
                StreamReader sr = new StreamReader(stream);
                while (true)
                {
                    string s = sr.ReadLine();
                    if (s == null)
                    {
                        break;
                    }
                    if (s.Length == 0 || s[0] == '#')
                    {
                        continue;
                    }
                    // This is hokey and lame, but let's get this working!
                    for (int i = 0; i < s.Length; i++)
                    {
                        if (s[i] == '1')
                        {
                            ms.WriteByte(1);
                        }
                        else
                        {
                            ms.WriteByte(0);
                        }
                    }
                }
            }
            return (ms.ToArray());
        }

        private byte[] GetCollisionMask(string refName)
        {
            byte[] mask = null;
            try
            {
                Stream stream = GetEmbeddedResource(refName + ".mask");
                if (stream != null)
                {
                    mask = ReadCollisionMask(stream);
                }
                else
                {
                    CCLog.Log("Failed to load the mask for " + refName);
                }
            }
            catch (Exception)
            {
                CCLog.Log("Failed to load the mask for " + refName);
            }
            return (mask);
        }

        #endregion

    }

    public class Sprite1 : SpriteTestDemo
    {
        public Sprite1()
        {
			// Register Touch Event
			var touchListener = new CCEventListenerTouchAllAtOnce();
			touchListener.OnTouchesEnded = onTouchesEnded;

			AddEventListener(touchListener);

            CCSize s = CCApplication.SharedApplication.MainWindowDirector.WindowSizeInPoints;
            addNewSpriteWithCoords(new CCPoint(s.Width / 2, s.Height / 2));
        }

        public void addNewSpriteWithCoords(CCPoint p)
        {
            int idx = (int)(CCMacros.CCRandomBetween0And1() * 1400.0f / 100.0f);
            int x = (idx % 5) * 85;
            int y = (idx / 5) * 121;

            CCSprite sprite = new CCSprite("Images/grossini_dance_atlas", new CCRect(x, y, 85, 121));
            AddChild(sprite);

            sprite.Position = p;

            CCActionInterval action;
            float random = CCMacros.CCRandomBetween0And1();

            if (random < 0.20)
                action = new CCScaleBy(3, 2);
            else if (random < 0.40)
                action = new CCRotateBy (3, 360);
            else if (random < 0.60)
                action = new CCBlink (1, 3);
            else if (random < 0.8)
                action = new CCTintBy (2, 0, -255, -255);
            else
                action = new CCFadeOut  (2);
            object obj = action.Reverse();
            CCActionInterval action_back = (CCActionInterval)action.Reverse();
            CCActionInterval seq = (CCActionInterval)(new CCSequence(action, action_back));

            sprite.RunAction(new CCRepeatForever (seq));
        }

        public override string title()
        {
            return "Sprite (tap screen)";
        }


		void onTouchesEnded(List<CCTouch> touches, CCEvent touchEvent)
        {
            foreach (CCTouch touch in touches)
            {
                var location = touch.Location;
                addNewSpriteWithCoords(location);
            }
        }
    }
}
