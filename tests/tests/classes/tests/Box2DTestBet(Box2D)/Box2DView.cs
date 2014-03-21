using System.Collections.Generic;
using Box2D.Common;
using CocosSharp;

namespace Box2D.TestBed
{
    public class Box2DView : CCLayer
    {
        private TestEntry m_entry;
        private Test m_test;
        private int m_entryID;

        private Settings settings = new Settings();
		CCEventListenerTouchOneByOne touchListener;

        public bool initWithEntryID(int entryId)
        {
			// Register Touch Event
			touchListener = new CCEventListenerTouchOneByOne();
			touchListener.IsSwallowTouches = true;

			touchListener.OnTouchBegan = onTouchBegan;
			touchListener.OnTouchMoved = onTouchMoved;
			touchListener.OnTouchEnded = onTouchEnded;

			EventDispatcher.AddEventListener(touchListener, -10);

			var keyboardListener = new CCEventListenerKeyboard ();
			keyboardListener.OnKeyPressed = onKeyPressed;
			keyboardListener.OnKeyReleased = onKeyReleased;

			EventDispatcher.AddEventListener (keyboardListener, this);

            Schedule ();

            m_entry = TestEntries.TestList[entryId];
            m_test = m_entry.CreateFcn();

            return true;
        }

        public string title()
        {
            return m_entry.Name;
        }

        public override void Update(float dt)
        {
            base.Update(dt);
        }

        protected override void Draw()
        {
            m_test.Step(settings);

            base.Draw();

            m_test.InternalDraw(settings);
        }

		public override void OnExit ()
		{
			if (touchListener != null)
				EventDispatcher.RemoveEventListener (touchListener);
			base.OnExit ();
		}

		bool onTouchBegan(CCTouch touch, CCEvent touchEvent)
        {
            CCPoint touchLocation = touch.Location;

            CCPoint nodePosition = ConvertToNodeSpace(touchLocation);
            //    NSLog(@"pos: %f,%f -> %f,%f", touchLocation.x, touchLocation.y, nodePosition.x, nodePosition.y);

			return m_test.MouseDown(new b2Vec2(nodePosition.X, nodePosition.Y));

        }

		void onTouchMoved(CCTouch touch, CCEvent touchEvent)
        {
            CCPoint touchLocation = touch.Location;
            CCPoint nodePosition = ConvertToNodeSpace(touchLocation);

            m_test.MouseMove(new b2Vec2(nodePosition.X, nodePosition.Y));
        }

		void onTouchEnded(CCTouch touch, CCEvent touchEvent)
        {
            CCPoint touchLocation = touch.Location;
            CCPoint nodePosition = ConvertToNodeSpace(touchLocation);

            m_test.MouseUp(new b2Vec2(nodePosition.X, nodePosition.Y));
        }

        //virtual void accelerometer(UIAccelerometer* accelerometer, CCAcceleration* acceleration);

		void onKeyPressed(CCEventKeyboard keyEvent)
        {
			var result = Convert( keyEvent );
            if (result.Length > 0)
            {
                m_test.Keyboard(result[0]);
            }
        }

		void onKeyReleased(CCEventKeyboard keyEvent)
        {
			var result = Convert( keyEvent );
            if (result.Length > 0)
            {
                m_test.KeyboardUp(result[0]);
            }
        }

		//        private KeyboardState _keyboardState;

//        public override void KeyboardCurrentState(KeyboardState currentState)
//        {
//            _keyboardState = currentState;
//        }

		public string Convert(CCEventKeyboard keyEvent)
        {
            string output = "";

			var state = new List<CCKeys>(keyEvent.KeyboardState.GetPressedKeys());

			bool usesShift = (state.Contains(CCKeys.LeftShift) || state.Contains(CCKeys.RightShift));

			foreach (CCKeys key in state)
            {
				if (key >= CCKeys.A && key <= CCKeys.Z)
                    output += key.ToString();
				else if (key >= CCKeys.NumPad0 && key <= CCKeys.NumPad9)
                    output += ((int) (key - CCKeys.NumPad0)).ToString();
				else if (key >= CCKeys.D0 && key <= CCKeys.D9)
                {
					string num = ((int) (key - CCKeys.D0)).ToString();

                    #region special num chars

                    if (usesShift)
                    {
                        switch (num)
                        {
                            case "1":
                            {
                                num = "!";
                            }
                                break;
                            case "2":
                            {
                                num = "@";
                            }
                                break;
                            case "3":
                            {
                                num = "#";
                            }
                                break;
                            case "4":
                            {
                                num = "$";
                            }
                                break;
                            case "5":
                            {
                                num = "%";
                            }
                                break;
                            case "6":
                            {
                                num = "^";
                            }
                                break;
                            case "7":
                            {
                                num = "&";
                            }
                                break;
                            case "8":
                            {
                                num = "*";
                            }
                                break;
                            case "9":
                            {
                                num = "(";
                            }
                                break;
                            case "0":
                            {
                                num = ")";
                            }
                                break;
                            default:
                                //wtf?
                                break;
                        }
                    }

                    #endregion

                    output += num;
                }
                else if (key == CCKeys.OemComma)
                    output += ",";
                else if (key == CCKeys.OemPeriod)
                    output += ".";
                else if (key == CCKeys.OemTilde)
                    output += "'";
                else if (key == CCKeys.Space)
                    output += " ";
                else if (key == CCKeys.OemMinus)
                    output += "-";
                else if (key == CCKeys.OemPlus)
                    output += "+";
                else if (key == CCKeys.OemQuestion && usesShift)
                    output += "?";
                else if (key == CCKeys.Back) //backspace
                    output += "\b";

                if (!usesShift) //shouldn't need to upper because it's automagically in upper case
                    output = output.ToLower();
            }
            return output;
        }

        public static Box2DView viewWithEntryID(int entryId)
        {
            var pView = new Box2DView();
            pView.initWithEntryID(entryId);
            return pView;
        }
    }
}
