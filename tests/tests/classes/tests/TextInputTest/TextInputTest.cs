using System;
using CocosSharp;

namespace tests
{
    public class TextInputTest : TestNavigationLayer
    {
        KeyboardNotificationLayer notificationLayer;
        TextInputTestScene textinputTestScene = new TextInputTestScene();

        public override void RestartCallback(object sender)
        {
            CCScene s = new TextInputTestScene();
            s.AddChild(textinputTestScene.restartTextInputTest());
            Scene.Director.ReplaceScene(s);
        }

        public override void NextCallback(object sender)
        {
            CCScene s = new TextInputTestScene();
            s.AddChild(textinputTestScene.nextTextInputTest());
            Scene.Director.ReplaceScene(s);
        }

        public override void BackCallback(object sender)
        {
            CCScene s = new TextInputTestScene();
            s.AddChild(textinputTestScene.backTextInputTest());
            Scene.Director.ReplaceScene(s);
        }

        public void addKeyboardNotificationLayer(KeyboardNotificationLayer layer)
        {
            notificationLayer = layer;
            AddChild(layer);
        }

        public override string Title
        {
            get
            {
                return "text input test";
            }
        }

        public override string Subtitle
        {
            get
            {
                return base.Subtitle;
            }
        }

    }

    public class KeyboardNotificationLayer : CCNode
    {
        public KeyboardNotificationLayer()
        {
            // Register Touch Event
            var touchListener = new CCEventListenerTouchOneByOne();
            touchListener.IsSwallowTouches = true;

            touchListener.OnTouchBegan = onTouchBegan;
            touchListener.OnTouchEnded = onTouchEnded;

            AddEventListener(touchListener);
        }

        public virtual string subtitle()
        {
            throw new NotFiniteNumberException();
        }

        public virtual void onClickTrackNode(bool bClicked)
        {
            throw new NotFiniteNumberException();
        }

        bool onTouchBegan(CCTouch pTouch, CCEvent touchEvent)
        {
            m_beginPos = pTouch.LocationOnScreen;
            return true;
        }

        void onTouchEnded(CCTouch pTouch, CCEvent touchEvent)
        {
            if (trackNode == null)
            {
                return;
            }

            var endPos = pTouch.LocationOnScreen;

            if (trackNode.BoundingBox.ContainsPoint(m_beginPos) && trackNode.BoundingBox.ContainsPoint(endPos))
            {
                onClickTrackNode(true);
            }
            else
            {
                onClickTrackNode(false);
            }
        }

        protected CCTextField trackNode;
        protected CCPoint m_beginPos;
    }

    public class TextFieldDefaultTest : KeyboardNotificationLayer
    {
        public override void onClickTrackNode(bool bClicked)
        {
            if (bClicked && trackNode != null)
            {
                trackNode.Edit();
            }
            else
            {
                if (trackNode != null && trackNode != null)
                {
                    trackNode.EndEdit();
                }
            }
            
        }

        public override void OnEnter()
        {
            base.OnEnter();

            var s = VisibleBoundsWorldspace.Size;

            var textField = new CCTextField("<click here for input>", 
                TextInputTestScene.FONT_NAME, 
                TextInputTestScene.FONT_SIZE, 
                CCLabelFormat.SpriteFont);

            var imeImplementation = textField.TextFieldIMEImplementation;
            imeImplementation.KeyboardDidHide += ImeImplementation_KeyboardDidHide;
            imeImplementation.KeyboardDidShow += ImeImplementation_KeyboardDidShow;
            imeImplementation.KeyboardWillHide += ImeImplementation_KeyboardWillHide;
            imeImplementation.KeyboardWillShow += ImeImplementation_KeyboardWillShow;

            textField.Position = s.Center;

            textField.AutoEdit = true;

            AddChild(textField);

            trackNode = textField;
        }

        private void ImeImplementation_KeyboardWillShow(object sender, CCIMEKeyboardNotificationInfo e)
        {
            CCLog.Log("Keyboard will show");
        }

        private void ImeImplementation_KeyboardWillHide(object sender, CCIMEKeyboardNotificationInfo e)
        {
            CCLog.Log("Keyboard will Hide");
        }

        private void ImeImplementation_KeyboardDidShow(object sender, CCIMEKeyboardNotificationInfo e)
        {
            CCLog.Log("Keyboard did show");
        }

        private void ImeImplementation_KeyboardDidHide(object sender, CCIMEKeyboardNotificationInfo e)
        {
            CCLog.Log("Keyboard did hide");
        }

        public override void OnExit()
        {
            base.OnExit();

            // Remember to remove our event listeners.
            var imeImplementation = trackNode.TextFieldIMEImplementation;
            imeImplementation.KeyboardDidHide -= ImeImplementation_KeyboardDidHide;
            imeImplementation.KeyboardDidShow -= ImeImplementation_KeyboardDidShow;
            imeImplementation.KeyboardWillHide -= ImeImplementation_KeyboardWillHide;
            imeImplementation.KeyboardWillShow -= ImeImplementation_KeyboardWillShow;
        }


        public override string subtitle()
        {
            return "TextField with default behavior test";
        }
    }

    //////////////////////////////////////////////////////////////////////////
    // TextFieldActionTest
    //////////////////////////////////////////////////////////////////////////

    public class TextFieldActionTest : KeyboardNotificationLayer
    {
        CCTextField m_pTextField;
        CCAction m_pTextFieldAction;
        bool m_bAction;
        int m_nCharLimit;       // the textfield max char limit

        public void callbackRemoveNodeWhenDidAction(CCNode node)
        {
            this.RemoveChild(node, true);
        }

        // KeyboardNotificationLayer
        public override string subtitle()
        {
            return "CCTextFieldTTF with action and char limit test";
        }

        public override void onClickTrackNode(bool bClicked)
        {
            if (bClicked)
            {
                trackNode.Edit();
            }
            else
            {
                trackNode.EndEdit();
            }
        }

        // CCLayer
        public override void OnEnter()
        {
            base.OnEnter();

            m_nCharLimit = 12;

            m_pTextFieldAction = new CCRepeatForever(
                (CCFiniteTimeAction)new CCSequence(
                                       new CCFadeOut(0.25f),
                                       new CCFadeIn(0.25f)));
            m_bAction = false;

            // add CCTextFieldTTF
            CCSize s = Layer.VisibleBoundsWorldspace.Size;

            m_pTextField = new CCTextField("<click here for input>",
                                              TextInputTestScene.FONT_NAME, 
                                              TextInputTestScene.FONT_SIZE,
                                              CCLabelFormat.SpriteFont);
            AddChild(m_pTextField);

            trackNode = m_pTextField;
        }

        // CCTextFieldDelegate
        public virtual bool onTextFieldAttachWithIME(CCTextField pSender)
        {
            //            if (m_bAction != null)
            //            {
            //                m_pTextField.RunAction(m_pTextFieldAction);
            //                m_bAction = true;
            //            }
            return false;
        }

        public virtual bool onTextFieldDetachWithIME(CCTextField pSender)
        {
            //            if (m_bAction != null)
            //            {
            //                m_pTextField.StopAction(m_pTextFieldAction);
            //                m_pTextField.Opacity = 255;
            //                m_bAction = false;
            //            }
            return false;
        }

        public virtual bool onTextFieldInsertText(CCTextField pSender, string text, int nLen)
        {
            // if insert enter, treat as default to detach with ime
            if ("\n" == text)
            {
                return false;
            }

            // if the textfield's char count more than m_nCharLimit, doesn't insert text anymore.
            if (pSender.Text.Length >= m_nCharLimit)
            {
                return true;
            }

            // create a insert text sprite and do some action
            var label = new CCLabel(text, TextInputTestScene.FONT_NAME, TextInputTestScene.FONT_SIZE, CCLabelFormat.SpriteFont);
            this.AddChild(label);
            CCColor3B color = new CCColor3B { R = 226, G = 121, B = 7 };
            label.Color = color;

            // move the sprite from top to position
            CCPoint endPos = pSender.Position;
            if (pSender.Text.Length > 0)
            {
                endPos.X += pSender.ContentSize.Width / 2;
            }
            CCSize inputTextSize = label.ContentSize;
            CCPoint beginPos = new CCPoint(endPos.X, Layer.VisibleBoundsWorldspace.Size.Height - inputTextSize.Height * 2);

            float duration = 0.5f;
            label.Position = beginPos;
            label.Scale = 8;

            CCAction seq = new CCSequence(
                new CCSpawn(
                    new CCMoveTo(duration, endPos),
                    new CCScaleTo(duration, 1),
                    new CCFadeOut(duration)),
                new CCCallFuncN(callbackRemoveNodeWhenDidAction));
            label.RunAction(seq);
            return false;
        }

        public virtual bool onTextFieldDeleteBackward(CCTextFieldTTF pSender, string delText, int nLen)
        {
            // create a delete text sprite and do some action
            var label = new CCLabel(delText, TextInputTestScene.FONT_NAME, TextInputTestScene.FONT_SIZE, CCLabelFormat.SpriteFont);
            this.AddChild(label);

            // move the sprite to fly out
            CCPoint beginPos = pSender.Position;
            CCSize textfieldSize = pSender.ContentSize;
            CCSize labelSize = label.ContentSize;
            beginPos.X += (textfieldSize.Width - labelSize.Width) / 2.0f;

            int RAND_MAX = 32767;
            CCRandom rand = new CCRandom();

            CCSize winSize = Layer.VisibleBoundsWorldspace.Size;
            CCPoint endPos = new CCPoint(-winSize.Width / 4.0f, winSize.Height * (0.5f + (float)CCRandom.Next() / (2.0f * RAND_MAX)));
            float duration = 1;
            float rotateDuration = 0.2f;
            int repeatTime = 5;
            label.Position = beginPos;

            CCAction seq = new CCSequence(
                new CCSpawn(
                    new CCMoveTo(duration, endPos),
                    new CCRepeat(
                        new CCRotateBy(rotateDuration, (CCRandom.Next() % 2 > 0) ? 360 : -360),
                        (uint)repeatTime),
                    new CCFadeOut(duration)),
                new CCCallFuncN(callbackRemoveNodeWhenDidAction));
            label.RunAction(seq);
            return false;
        }

        public virtual bool onDraw(CCTextField pSender)
        {
            return false;
        }
    }
}
