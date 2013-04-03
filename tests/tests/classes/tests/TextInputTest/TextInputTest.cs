using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;
using System.Diagnostics;

namespace cocos2d
{
    //@brief	TextInputTest for retain prev, reset, next, main menu buttons.
    public class TextInputTest : CCLayer
    {
        KeyboardNotificationLayer m_pNotificationLayer;
        TextInputTestScene textinputTestScene = new TextInputTestScene();

        public void restartCallback(object pSender)
        {
            CCScene s = new TextInputTestScene();
            s.AddChild(textinputTestScene.restartTextInputTest());
            CCDirector.SharedDirector.ReplaceScene(s);
            //s->release();
        }

        public void nextCallback(object pSender)
        {
            CCScene s = new TextInputTestScene();
            s.AddChild(textinputTestScene.nextTextInputTest());
            CCDirector.SharedDirector.ReplaceScene(s);
            //s->release();
        }

        public void backCallback(object pSender)
        {
            CCScene s = new TextInputTestScene();
            s.AddChild(textinputTestScene.backTextInputTest());
            CCDirector.SharedDirector.ReplaceScene(s);
            //s->release();
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

            CCLabelTTF label = CCLabelTTF.Create(title(), "arial", 24);
            AddChild(label);
            label.Position = new CCPoint(s.Width / 2, s.Height - 50);

            string subTitle = m_pNotificationLayer.subtitle();
            if (subTitle != null)
            {
                CCLabelTTF l = CCLabelTTF.Create(subTitle, subtitle(), 16);
                AddChild(l, 1);
                l.Position = new CCPoint(s.Width / 2, s.Height - 80);
            }

            CCMenuItemImage item1 = CCMenuItemImage.Create("Images/b1.png", "Images/b2.png", backCallback);
            CCMenuItemImage item2 = CCMenuItemImage.Create("Images/r1.png", "Images/r2.png", restartCallback);
            CCMenuItemImage item3 = CCMenuItemImage.Create("Images/f1.png", "Images/f2.png", nextCallback);

            CCMenu menu = CCMenu.Create(item1, item2, item3);
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

    //////////////////////////////////////////////////////////////////////////
    // KeyboardNotificationLayer for test IME keyboard notification.
    //////////////////////////////////////////////////////////////////////////

    public class KeyboardNotificationLayer : CCLayer
    {
        public KeyboardNotificationLayer()
        {
            base.TouchEnabled = true;
        }

        public virtual string subtitle()
        {
            throw new NotFiniteNumberException();
        }

        public void onClickTrackNode(bool bClicked)
        {
            throw new NotFiniteNumberException();
        }

        public override void RegisterWithTouchDispatcher()
        {
            CCDirector.SharedDirector.TouchDispatcher.AddTargetedDelegate(this, 0, false);
        }

        public virtual void keyboardWillShow(CCIMEKeyboardNotificationInfo info)
        {
            CCLog.Log("TextInputTest:keyboardWillShowAt(origin:%f,%f, size:%f,%f)",
        info.end.Origin.X, info.end.Origin.Y, info.end.Size.Width, info.end.Size.Height);

            if (m_pTrackNode != null)
            {
                return;
            }

            CCRect rectTracked = TextInputTestScene.getRect(m_pTrackNode);
            CCLog.Log("TextInputTest:trackingNodeAt(origin:%f,%f, size:%f,%f)",
                rectTracked.Origin.X, rectTracked.Origin.Y, rectTracked.Size.Width, rectTracked.Size.Height);

            // if the keyboard area doesn't intersect with the tracking node area, nothing need to do.
            if (!CCRect.CCRectIntersetsRect(rectTracked, info.end))
            {
                return;
            }

            // assume keyboard at the bottom of screen, calculate the vertical adjustment.
            float adjustVert = CCRect.CCRectGetMaxY(info.end) - CCRect.CCRectGetMinY(rectTracked);
            CCLog.Log("TextInputTest:needAdjustVerticalPosition(%f)", adjustVert);

            // move all the children node of KeyboardNotificationLayer
            CCNode ccnoed = new CCNode();

            var children = ccnoed.Children;
            CCNode node;
            int count = children.Count;
            CCPoint pos;
            for (int i = 0; i < count; ++i)
            {
                node = (CCNode)children[i];
                pos = node.Position;
                pos.Y += adjustVert;
                node.Position = pos;
            }
        }

        // CCLayer
        public override bool TouchBegan(CCTouch pTouch, CCEvent pEvent)
        {
            CCLog.Log("++++++++++++++++++++++++++++++++++++++++++++");
            m_beginPos = pTouch.LocationInView;
            m_beginPos = CCDirector.SharedDirector.ConvertToGl(m_beginPos);
            return true;
        }

        public override void TouchEnded(CCTouch pTouch, CCEvent pEvent)
        {
            if (m_pTrackNode != null)
            {
                return;
            }

            CCPoint endPos = pTouch.LocationInView;
            endPos = CCDirector.SharedDirector.ConvertToGl(endPos);

            float delta = 5.0f;
            if (Math.Abs(endPos.X - m_beginPos.X) > delta
                || Math.Abs(endPos.Y - m_beginPos.Y) > delta)
            {
                // not click
                m_beginPos.X = m_beginPos.Y = -1;
                return;
            }

            // decide the trackNode is clicked.
            CCRect rect;
            CCPoint point = ConvertTouchToNodeSpaceAr(pTouch);
            CCLog.Log("KeyboardNotificationLayer:clickedAt(%f,%f)", point.X, point.Y);

            rect = TextInputTestScene.getRect(m_pTrackNode);
            CCLog.Log("KeyboardNotificationLayer:TrackNode at(origin:%f,%f, size:%f,%f)",
                rect.Origin.X, rect.Origin.Y, rect.Size.Width, rect.Size.Height);

            this.onClickTrackNode(CCRect.CCRectContainsPoint(rect, point));
            CCLog.Log("----------------------------------");
        }

        protected CCNode m_pTrackNode;
        protected CCPoint m_beginPos;
    }

    //////////////////////////////////////////////////////////////////////////
    // TextFieldTTFDefaultTest for test TextFieldTTF default behavior.
    //////////////////////////////////////////////////////////////////////////

    public class TextFieldTTFDefaultTest : KeyboardNotificationLayer
    {
        // KeyboardNotificationLayer
        public virtual string subtitle()
        {
            return "TextFieldTTF with default behavior test";
        }

        public virtual void onClickTrackNode(bool bClicked)
        {
            CCTextFieldTTF pTextField = (CCTextFieldTTF)m_pTrackNode;
            if (bClicked)
            {
                // TextFieldTTFTest be clicked
                CCLog.Log("TextFieldTTFDefaultTest:CCTextFieldTTF attachWithIME");
                pTextField.AttachWithIME();
            }
            else
            {
                // TextFieldTTFTest not be clicked
                CCLog.Log("TextFieldTTFDefaultTest:CCTextFieldTTF detachWithIME");
                pTextField.DetachWithIME();
            }
        }

        // CCLayer
        public override void OnEnter()
        {
            base.OnEnter();

            // add CCTextFieldTTF
            CCSize s = CCDirector.SharedDirector.WinSize;

            CCTextFieldTTF pTextField = CCTextFieldTTF.TextFieldWithPlaceHolder("<click here for input>",
                TextInputTestScene.FONT_NAME, TextInputTestScene.FONT_SIZE);
            AddChild(pTextField);

            //#if (CC_TARGET_PLATFORM == CC_PLATFORM_ANDROID)	
            //    // on android, CCTextFieldTTF cannot auto adjust its position when soft-keyboard pop up
            //    // so we had to set a higher position to make it visable
            //    pTextField->setPosition(ccp(s.width / 2, s.height/2 + 50));
            //#else
            //    pTextField->setPosition(ccp(s.width / 2, s.height / 2));
            //#endif

            m_pTrackNode = pTextField;
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

        public virtual void onClickTrackNode(bool bClicked)
        {
            CCTextFieldTTF pTextField = (CCTextFieldTTF)m_pTrackNode;
            if (bClicked)
            {
                // TextFieldTTFTest be clicked
                CCLog.Log("TextFieldTTFActionTest:CCTextFieldTTF attachWithIME");
                pTextField.AttachWithIME();
            }
            else
            {
                // TextFieldTTFTest not be clicked
                CCLog.Log("TextFieldTTFActionTest:CCTextFieldTTF detachWithIME");
                pTextField.DetachWithIME();
            }
        }

        // CCLayer
        public override void OnEnter()
        {
            base.OnEnter();

            m_nCharLimit = 12;

            m_pTextFieldAction = new CCRepeatForever (
                (CCActionInterval)CCSequence.FromActions(
                    new CCFadeOut  (0.25f),
                    new CCFadeIn  (0.25f)));
            //m_pTextFieldAction->retain();
            m_bAction = false;

            // add CCTextFieldTTF
            CCSize s = CCDirector.SharedDirector.WinSize;

            m_pTextField = CCTextFieldTTF.TextFieldWithPlaceHolder("<click here for input>",
            TextInputTestScene.FONT_NAME, TextInputTestScene.FONT_SIZE);
            AddChild(m_pTextField);

            //m_pTextField.setDelegate(this);


            //#if (CC_TARGET_PLATFORM == CC_PLATFORM_ANDROID)	
            //    // on android, CCTextFieldTTF cannot auto adjust its position when soft-keyboard pop up
            //    // so we had to set a higher position
            //    m_pTextField->setPosition(new CCPoint(s.width / 2, s.height/2 + 50));
            //#else
            //    m_pTextField->setPosition(ccp(s.width / 2, s.height / 2));
            //#endif

            m_pTrackNode = m_pTextField;
        }

        public override void OnExit()
        {
            base.OnExit();
            //m_pTextFieldAction->release();
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
            if (pSender.CharCount >= m_nCharLimit)
            {
                return true;
            }

            // create a insert text sprite and do some action
            CCLabelTTF label = CCLabelTTF.Create(text, TextInputTestScene.FONT_NAME, TextInputTestScene.FONT_SIZE);
            this.AddChild(label);
            CCColor3B color = new CCColor3B { R = 226, G = 121, B = 7 };
            label.Color = color;

            // move the sprite from top to position
            CCPoint endPos = pSender.Position;
            if (pSender.CharCount > 0)
            {
                endPos.X += pSender.ContentSize.Width / 2;
            }
            CCSize inputTextSize = label.ContentSize;
            CCPoint beginPos = new CCPoint(endPos.X, CCDirector.SharedDirector.WinSize.Height - inputTextSize.Height * 2);

            float duration = 0.5f;
            label.Position = beginPos;
            label.Scale = 8;

            CCAction seq = CCSequence.FromActions(
                CCSpawn.FromActions(
                    new CCMoveTo (duration, endPos),
                    CCScaleTo.Create(duration, 1),
                    new CCFadeOut  (duration)),
                CCCallFuncN.Create(callbackRemoveNodeWhenDidAction));
            label.RunAction(seq);
            return false;
        }

        public virtual bool onTextFieldDeleteBackward(CCTextFieldTTF pSender, string delText, int nLen)
        {
            // create a delete text sprite and do some action
            CCLabelTTF label = CCLabelTTF.Create(delText, TextInputTestScene.FONT_NAME, TextInputTestScene.FONT_SIZE);
            this.AddChild(label);

            // move the sprite to fly out
            CCPoint beginPos = pSender.Position;
            CCSize textfieldSize = pSender.ContentSize;
            CCSize labelSize = label.ContentSize;
            beginPos.X += (textfieldSize.Width - labelSize.Width) / 2.0f;

            int RAND_MAX = 32767;
            Random rand = new Random();

            CCSize winSize = CCDirector.SharedDirector.WinSize;
            CCPoint endPos = new CCPoint(-winSize.Width / 4.0f, winSize.Height * (0.5f + (float)Random.Next() / (2.0f * RAND_MAX)));
            float duration = 1;
            float rotateDuration = 0.2f;
            int repeatTime = 5;
            label.Position = beginPos;

            CCAction seq = CCSequence.FromActions(
                CCSpawn.FromActions(
                    new CCMoveTo (duration, endPos),
                    new CCRepeat (
                        new CCRotateBy (rotateDuration, (Random.Next() % 2 > 0) ? 360 : -360),
                        (uint)repeatTime),
                    new CCFadeOut  (duration)),
                CCCallFuncN.Create(callbackRemoveNodeWhenDidAction));
            label.RunAction(seq);
            return false;
        }

        public virtual bool onDraw(CCTextFieldTTF pSender)
        {
            return false;
        }
    }
}
