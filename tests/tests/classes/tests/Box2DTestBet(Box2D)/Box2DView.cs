using System.Collections.Generic;
using Box2D.Common;
using Cocos2D;
using Microsoft.Xna.Framework.Input;

namespace Box2D.TestBed
{
    public class Box2DView : CCLayer
    {
        private TestEntry m_entry;
        private Test m_test;
        private int m_entryID;

        private Settings settings = new Settings();

        public bool initWithEntryID(int entryId)
        {
            TouchEnabled = true;
            KeyboardEnabled = true;

            ScheduleUpdate();

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
            m_test.Step(settings);
        }

        public override void Draw()
        {
            base.Draw();

            m_test.InternalDraw(settings);
        }

        public override void RegisterWithTouchDispatcher()
        {
            CCDirector pDirector = CCDirector.SharedDirector;
            pDirector.TouchDispatcher.AddTargetedDelegate(this, -10, true);
        }

        public override bool TouchBegan(CCTouch touch)
        {
            CCPoint touchLocation = touch.Location;

            CCPoint nodePosition = ConvertToNodeSpace(touchLocation);
            //    NSLog(@"pos: %f,%f -> %f,%f", touchLocation.x, touchLocation.y, nodePosition.x, nodePosition.y);

            m_test.MouseDown(new b2Vec2(nodePosition.X, nodePosition.Y));

            return true;
        }

        public override void TouchMoved(CCTouch touch)
        {
            CCPoint touchLocation = touch.Location;
            CCPoint nodePosition = ConvertToNodeSpace(touchLocation);

            m_test.MouseMove(new b2Vec2(nodePosition.X, nodePosition.Y));
        }

        public override void TouchEnded(CCTouch touch)
        {
            CCPoint touchLocation = touch.Location;
            CCPoint nodePosition = ConvertToNodeSpace(touchLocation);

            m_test.MouseUp(new b2Vec2(nodePosition.X, nodePosition.Y));
        }

        //virtual void accelerometer(UIAccelerometer* accelerometer, CCAcceleration* acceleration);

        public override void KeyPressed(Keys key)
        {
            var result = Convert(new Keys[] {key});
            if (result.Length > 0)
            {
                m_test.Keyboard(result[0]);
            }
        }

        public override void KeyReleased(Keys key)
        {
            var result = Convert(new Keys[] { key });
            if (result.Length > 0)
            {
                m_test.KeyboardUp(result[0]);
            }
        }

        private KeyboardState _keyboardState;

        public override void KeyboardCurrentState(KeyboardState currentState)
        {
            _keyboardState = currentState;
        }

        public string Convert(Keys[] keys)
        {
            string output = "";

            var state = new List<Keys>(_keyboardState.GetPressedKeys());

            bool usesShift = (state.Contains(Keys.LeftShift) || state.Contains(Keys.RightShift));

            foreach (Keys key in keys)
            {
                if (key >= Keys.A && key <= Keys.Z)
                    output += key.ToString();
                else if (key >= Keys.NumPad0 && key <= Keys.NumPad9)
                    output += ((int) (key - Keys.NumPad0)).ToString();
                else if (key >= Keys.D0 && key <= Keys.D9)
                {
                    string num = ((int) (key - Keys.D0)).ToString();

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
                else if (key == Keys.OemComma)
                    output += ",";
                else if (key == Keys.OemPeriod)
                    output += ".";
                else if (key == Keys.OemTilde)
                    output += "'";
                else if (key == Keys.Space)
                    output += " ";
                else if (key == Keys.OemMinus)
                    output += "-";
                else if (key == Keys.OemPlus)
                    output += "+";
                else if (key == Keys.OemQuestion && usesShift)
                    output += "?";
                else if (key == Keys.Back) //backspace
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
