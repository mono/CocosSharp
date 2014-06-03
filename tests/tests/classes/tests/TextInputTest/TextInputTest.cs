using System;

namespace CocosSharp
{
    public class TextInputTest : CCLayer
    {
        KeyboardNotificationLayer m_pNotificationLayer;
        TextInputTestScene textinputTestScene = new TextInputTestScene();

        public void restartCallback(object pSender)
        {
            CCScene s = new TextInputTestScene();
            s.AddChild(textinputTestScene.restartTextInputTest());
            CCDirector.SharedDirector.ReplaceScene(s);
        }

        public void nextCallback(object pSender)
        {
            CCScene s = new TextInputTestScene();
            s.AddChild(textinputTestScene.nextTextInputTest());
            CCDirector.SharedDirector.ReplaceScene(s);
        }

        public void backCallback(object pSender)
        {
            CCScene s = new TextInputTestScene();
            s.AddChild(textinputTestScene.backTextInputTest());
            CCDirector.SharedDirector.ReplaceScene(s);
        }

        public virtual string title()
        {
            return "text input test";
        }

        public void addKeyboardNotificationLayer(KeyboardNotificationLayer pLayer)
        {
            m_pNotificationLayer = pLayer;
            AddChild(pLayer);
        }

        public override void OnEnter()
        {
            base.OnEnter();

            CCSize s = CCDirector.SharedDirector.WinSize;

            CCLabelTtf label = new CCLabelTtf(title(), "arial", 24);
            AddChild(label);
            label.Position = new CCPoint(s.Width / 2, s.Height - 50);

            string subTitle = m_pNotificationLayer.subtitle();
            if (subTitle != null)
            {
                CCLabelTtf l = new CCLabelTtf(subTitle, subtitle(), 16);
                AddChild(l, 1);
                l.Position = new CCPoint(s.Width / 2, s.Height - 80);
            }

            CCMenuItemImage item1 = new CCMenuItemImage("Images/b1.png", "Images/b2.png", backCallback);
            CCMenuItemImage item2 = new CCMenuItemImage("Images/r1.png", "Images/r2.png", restartCallback);
            CCMenuItemImage item3 = new CCMenuItemImage("Images/f1.png", "Images/f2.png", nextCallback);

            CCMenu menu = new CCMenu(item1, item2, item3);
            menu.Position = new CCPoint(0, 0);
            item1.Position = new CCPoint(s.Width / 2 - 100, 30);
            item2.Position = new CCPoint(s.Width / 2, 30);
            item3.Position = new CCPoint(s.Width / 2 + 100, 30);

            AddChild(menu, 1);
        }

        public virtual string subtitle()
        {
            return String.Empty;
        }
    }

    public class KeyboardNotificationLayer : CCLayer
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
            m_beginPos = pTouch.Location;
            return true;
        }

		void onTouchEnded(CCTouch pTouch, CCEvent touchEvent)
        {
            if (m_pTrackNode == null)
            {
                return;
            }

            var endPos = pTouch.Location;

            if (m_pTrackNode.BoundingBox.ContainsPoint(m_beginPos) && m_pTrackNode.BoundingBox.ContainsPoint(endPos))
            {
                onClickTrackNode(true);
            }
            else
            {
                onClickTrackNode(false);
            }
        }

        protected CCTextFieldTTF m_pTrackNode;
        protected CCPoint m_beginPos;
    }

    public class TextFieldTTFDefaultTest : KeyboardNotificationLayer
    {
        public override void onClickTrackNode(bool bClicked)
        {
            if (bClicked && m_pTrackNode != null)
            {
                m_pTrackNode.Edit("CCTextFieldTTF test", "Enter text");
            }
        }

        public override void OnEnter()
        {
            base.OnEnter();

            var s = CCDirector.SharedDirector.WinSize;

            var pTextField = new CCTextFieldTTF(
                "<click here for input>", TextInputTestScene.FONT_NAME, TextInputTestScene.FONT_SIZE
                );

            pTextField.Position = CCDirector.SharedDirector.WinSize.Center;

            pTextField.AutoEdit = true;

            AddChild(pTextField);

            //m_pTrackNode = pTextField;
        }

        public override string subtitle()
        {
            return "TextFieldTTF with default behavior test";
        }
    }

    //////////////////////////////////////////////////////////////////////////
    // TextFieldTTFActionTest
    //////////////////////////////////////////////////////////////////////////

    public class TextFieldTTFActionTest : KeyboardNotificationLayer
    {
        CCTextFieldTTF m_pTextField;
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
                m_pTrackNode.Edit();
            }
            else
            {
                m_pTrackNode.EndEdit();
            }
        }

        // CCLayer
        public override void OnEnter()
        {
            base.OnEnter();

            m_nCharLimit = 12;

            m_pTextFieldAction = new CCRepeatForever(
                (CCActionInterval) new CCSequence(
                                       new CCFadeOut(0.25f),
                                       new CCFadeIn(0.25f)));
            //m_pTextFieldAction->retain();
            m_bAction = false;

            // add CCTextFieldTTF
            CCSize s = CCDirector.SharedDirector.WinSize;

            m_pTextField = new CCTextFieldTTF("<click here for input>",
                                              TextInputTestScene.FONT_NAME, TextInputTestScene.FONT_SIZE);
            AddChild(m_pTextField);

            m_pTrackNode = m_pTextField;
        }

        // CCTextFieldDelegate
        public virtual bool onTextFieldAttachWithIME(CCTextFieldTTF pSender)
        {
            if (m_bAction != null)
            {
                m_pTextField.RunAction(m_pTextFieldAction);
                m_bAction = true;
            }
            return false;
        }

        public virtual bool onTextFieldDetachWithIME(CCTextFieldTTF pSender)
        {
            if (m_bAction != null)
            {
                m_pTextField.StopAction(m_pTextFieldAction);
                m_pTextField.Opacity = 255;
                m_bAction = false;
            }
            return false;
        }

        public virtual bool onTextFieldInsertText(CCTextFieldTTF pSender, string text, int nLen)
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
            CCLabelTtf label = new CCLabelTtf(text, TextInputTestScene.FONT_NAME, TextInputTestScene.FONT_SIZE);
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
            CCPoint beginPos = new CCPoint(endPos.X, CCDirector.SharedDirector.WinSize.Height - inputTextSize.Height * 2);

            float duration = 0.5f;
            label.Position = beginPos;
            label.Scale = 8;

            CCAction seq = new CCSequence(
                new CCSpawn(
                    new CCMoveTo (duration, endPos),
                    new CCScaleTo(duration, 1),
                    new CCFadeOut  (duration)),
                new CCCallFuncN(callbackRemoveNodeWhenDidAction));
            label.RunAction(seq);
            return false;
        }

        public virtual bool onTextFieldDeleteBackward(CCTextFieldTTF pSender, string delText, int nLen)
        {
            // create a delete text sprite and do some action
            CCLabelTtf label = new CCLabelTtf(delText, TextInputTestScene.FONT_NAME, TextInputTestScene.FONT_SIZE);
            this.AddChild(label);

            // move the sprite to fly out
            CCPoint beginPos = pSender.Position;
            CCSize textfieldSize = pSender.ContentSize;
            CCSize labelSize = label.ContentSize;
            beginPos.X += (textfieldSize.Width - labelSize.Width) / 2.0f;

            int RAND_MAX = 32767;
            CCRandom rand = new CCRandom();

            CCSize winSize = CCDirector.SharedDirector.WinSize;
            CCPoint endPos = new CCPoint(-winSize.Width / 4.0f, winSize.Height * (0.5f + (float)CCRandom.Next() / (2.0f * RAND_MAX)));
            float duration = 1;
            float rotateDuration = 0.2f;
            int repeatTime = 5;
            label.Position = beginPos;

            CCAction seq = new CCSequence(
                new CCSpawn(
                    new CCMoveTo (duration, endPos),
                    new CCRepeat (
                        new CCRotateBy (rotateDuration, (CCRandom.Next() % 2 > 0) ? 360 : -360),
                        (uint)repeatTime),
                    new CCFadeOut  (duration)),
                new CCCallFuncN(callbackRemoveNodeWhenDidAction));
            label.RunAction(seq);
            return false;
        }

        public virtual bool onDraw(CCTextFieldTTF pSender)
        {
            return false;
        }
    }
}
