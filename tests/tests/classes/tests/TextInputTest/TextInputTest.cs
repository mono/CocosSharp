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
                return "CCTextField Text Input Test";
            }
        }

        public override string Subtitle
        {
            get
            {
                return (notificationLayer != null) ? notificationLayer.Subtitle : string.Empty;
            }
        }

    }

    public class KeyboardNotificationLayer : CCNode
    {

        CCTextField trackNode;
        protected CCPoint beginPosition;

        public KeyboardNotificationLayer()
        {
            // Register Touch Event
            var touchListener = new CCEventListenerTouchOneByOne();
            touchListener.IsSwallowTouches = true;

            touchListener.OnTouchBegan = OnTouchBegan;
            touchListener.OnTouchEnded = OnTouchEnded;

            AddEventListener(touchListener);
        }

        public virtual string Subtitle
        {
            get { return string.Empty; }
        }

        public virtual void OnClickTrackNode(bool bClicked)
        {
            throw new NotFiniteNumberException();
        }

        bool OnTouchBegan(CCTouch pTouch, CCEvent touchEvent)
        {
            beginPosition = pTouch.Location;
            return true;
        }

        void OnTouchEnded(CCTouch pTouch, CCEvent touchEvent)
        {
            if (trackNode == null)
            {
                return;
            }

            var endPos = pTouch.Location;

            if (trackNode.BoundingBox.ContainsPoint(beginPosition) && trackNode.BoundingBox.ContainsPoint(endPos))
            {
                OnClickTrackNode(true);
            }
            else
            {
                OnClickTrackNode(false);
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            if (trackNode != null)
            {
                trackNode.EndEdit();
                DetachListeners();
            }
        }

        protected CCTextField TrackNode 
        {
            get { return trackNode; }
            set 
            {
                if (value == null)
                {
                    if (trackNode != null)
                    {
                        DetachListeners();
                        trackNode = value;
                        return;
                    }
                }

                if (trackNode != value)
                {
                    DetachListeners();
                }

                trackNode = value;
                AttachListeners();
            }
        }

        void AttachListeners ()
        {
            // Remember to remove our event listeners.
            var imeImplementation = trackNode.TextFieldIMEImplementation;
            imeImplementation.KeyboardDidHide += OnKeyboardDidHide;
            imeImplementation.KeyboardDidShow += OnKeyboardDidShow;
            imeImplementation.KeyboardWillHide += OnKeyboardWillHide;
            imeImplementation.KeyboardWillShow += OnKeyboardWillShow;

        }

        void DetachListeners ()
        {
            if (TrackNode != null)
            {
                // Remember to remove our event listeners.
                var imeImplementation = TrackNode.TextFieldIMEImplementation;
                imeImplementation.KeyboardDidHide -= OnKeyboardDidHide;
                imeImplementation.KeyboardDidShow -= OnKeyboardDidShow;
                imeImplementation.KeyboardWillHide -= OnKeyboardWillHide;
                imeImplementation.KeyboardWillShow -= OnKeyboardWillShow;
            }
        }

        void OnKeyboardWillShow(object sender, CCIMEKeyboardNotificationInfo e)
        {
            CCLog.Log("Keyboard will show");
        }

        void OnKeyboardWillHide(object sender, CCIMEKeyboardNotificationInfo e)
        {
            CCLog.Log("Keyboard will Hide");
        }

        void OnKeyboardDidShow(object sender, CCIMEKeyboardNotificationInfo e)
        {
            CCLog.Log("Keyboard did show");
        }

        void OnKeyboardDidHide(object sender, CCIMEKeyboardNotificationInfo e)
        {
            CCLog.Log("Keyboard did hide");
        }

    }

    public class TextFieldDefaultTest : KeyboardNotificationLayer
    {
        CCMoveTo scrollUp;
        CCMoveTo scrollDown;

        public override void OnClickTrackNode(bool bClicked)
        {
            if (bClicked && TrackNode != null)
            {
                TrackNode.Edit();
            }
            else
            {
                if (TrackNode != null && TrackNode != null)
                {
                    TrackNode.EndEdit();
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

            textField.BeginEditing += OnBeginEditing;
            textField.EndEditing += OnEndEditing;
            textField.Position = s.Center;

            textField.AutoEdit = true;

            AddChild(textField);

            TrackNode = textField;

            scrollUp = new CCMoveTo(0.5f, VisibleBoundsWorldspace.Top() - new CCPoint(0, s.Height / 4));
            scrollDown = new CCMoveTo(0.5f, textField.Position);
        }

        private void OnEndEditing(object sender, ref string text, ref bool canceled)
        {
            ((CCNode)sender).RunAction(scrollDown);
        }

        private void OnBeginEditing(object sender, ref string text, ref bool canceled)
        {
            ((CCNode)sender).RunAction(scrollUp);
        }

        public override string Subtitle
        {
            get {
                return "TextField with default behavior test";
            }
        }
    }

    //////////////////////////////////////////////////////////////////////////
    // TextFieldActionTest
    //////////////////////////////////////////////////////////////////////////

    public class TextFieldActionTest : KeyboardNotificationLayer
    {
        CCTextField textField;
        static CCFiniteTimeAction textFieldAction = new CCSequence(
                                       new CCFadeOut(0.25f),
                                       new CCFadeIn(0.25f));

        CCActionState textFieldActionState;
        bool action;
        int charLimit;       // the textfield max char limit

        const int RANDOM_MAX = 32767;

        public void callbackRemoveNodeWhenDidAction(CCNode node)
        {
            this.RemoveChild(node, true);
        }

        public override string Subtitle
        {
            get {
                return "CCTextField with action and char limit test";
            }
        }

        public override void OnClickTrackNode(bool bClicked)
        {
            if (bClicked)
            {
                TrackNode.Edit();
            }
            else
            {
                TrackNode.EndEdit();
            }
        }

        // CCLayer
        public override void OnEnter()
        {
            base.OnEnter();

            charLimit = 12;

            action = false;

            textField = new CCTextField("<click here for input>",
                                              TextInputTestScene.FONT_NAME, 
                                              TextInputTestScene.FONT_SIZE,
                                              CCLabelFormat.SpriteFont);

            var imeImplementation = textField.TextFieldIMEImplementation;
            imeImplementation.KeyboardDidHide += OnKeyboardDidHide;
            imeImplementation.KeyboardDidShow += OnKeyboardDidShow;
            imeImplementation.InsertText += OnInsertText;
            imeImplementation.ReplaceText += OnReplaceText;
            imeImplementation.DeleteBackward += OnDeleteBackward;

            textField.Position = VisibleBoundsWorldspace.Center;
            textField.PositionY += VisibleBoundsWorldspace.Size.Height / 4;

            AddChild(textField);

            TrackNode = textField;
        }

        public override void OnExit()
        {
            base.OnExit();

            // Remember to remove our event listeners.
            var imeImplementation = TrackNode.TextFieldIMEImplementation;
            imeImplementation.KeyboardDidHide -= OnKeyboardDidHide;
            imeImplementation.KeyboardDidShow -= OnKeyboardDidShow;
            imeImplementation.InsertText -= OnInsertText;
            imeImplementation.ReplaceText -= OnReplaceText;
            imeImplementation.DeleteBackward -= OnDeleteBackward;

        }

        void OnDeleteBackward (object sender, CCIMEKeybardEventArgs e)
        {
            var focusedTextField = sender as CCTextField;

            if (focusedTextField == null || string.IsNullOrEmpty(focusedTextField.Text))
            {
                e.Cancel = true;
                return;
            }

            // Just cancel this if we would backspace over the PlaceHolderText as it would just be
            // replaced anyway and the Action below should not be executed.
            var delText = focusedTextField.Text;
            if (delText == focusedTextField.PlaceHolderText)
            {
                e.Cancel = true;
                return;
            }

            delText = delText.Substring(delText.Length - 1);

            // create a delete text sprite and do some action
            var label = new CCLabel(delText, TextInputTestScene.FONT_NAME, TextInputTestScene.FONT_SIZE + 3, CCLabelFormat.SpriteFont);
            this.AddChild(label);

            // move the sprite to fly out
            CCPoint beginPos = focusedTextField.Position;
            CCSize textfieldSize = focusedTextField.ContentSize;
            CCSize labelSize = label.ContentSize;
            beginPos.X += (textfieldSize.Width - labelSize.Width) / 2.0f;

            var nextRandom = (float)CCRandom.Next(RANDOM_MAX);

            CCSize winSize = Layer.VisibleBoundsWorldspace.Size;
            CCPoint endPos = new CCPoint(-winSize.Width / 4.0f, winSize.Height * (0.5f + nextRandom / (2.0f * RANDOM_MAX)));

            float duration = 1;
            float rotateDuration = 0.2f;
            int repeatTime = 5;
            label.Position = beginPos;

            var delAction = new CCSpawn(new CCMoveTo(duration, endPos),
                new CCRepeat(
                        new CCRotateBy(rotateDuration, (CCRandom.Next() % 2 > 0) ? 360 : -360),
                        (uint)repeatTime),
                        new CCFadeOut(duration)
                );

            label.RunActionsAsync(delAction, new CCRemoveSelf(true));

        }

        void OnInsertText (object sender, CCIMEKeybardEventArgs e)
        {
            var focusedTextField = sender as CCTextField;

            if (focusedTextField == null)
            {
                e.Cancel = true;
                return;
            }

            var text = e.Text;
            var currentText = focusedTextField.Text;

            // if insert enter, treat as default to detach with ime
            if ("\n" == text)
            {
                return;
            }

            // if the textfield's char count is more than charLimit, don't insert text anymore.
            if (focusedTextField.CharacterCount >= charLimit)
            {
                e.Cancel = true;
                return;
            }

            // create a insert text sprite and do some action
            var label = new CCLabel(text, TextInputTestScene.FONT_NAME, TextInputTestScene.FONT_SIZE, CCLabelFormat.SpriteFont);
            this.AddChild(label);
            var color = new CCColor3B { R = 226, G = 121, B = 7 };
            label.Color = color;

            var inputTextSize = focusedTextField.CharacterCount == 0 ? CCSize.Zero : focusedTextField.ContentSize;

            // move the sprite from top to position
            var endPos = focusedTextField.Position;
            endPos.Y -= inputTextSize.Height;

            if (currentText.Length > 0)
            {
                endPos.X += inputTextSize.Width / 2;
            }

            var beginPos = new CCPoint(endPos.X, VisibleBoundsWorldspace.Size.Height - inputTextSize.Height * 2);

            var duration = 0.5f;
            label.Position = beginPos;
            label.Scale = 8;

            CCAction seq = new CCSequence(
                new CCSpawn(
                    new CCMoveTo(duration, endPos),
                    new CCScaleTo(duration, 1),
                    new CCFadeOut(duration)),
                new CCRemoveSelf(true));
            label.RunAction(seq);
        }

        void OnReplaceText (object sender, CCIMEKeybardEventArgs e)
        {
            var focusedTextField = sender as CCTextField;

            if (e.Length > charLimit)
                e.Text = e.Text.Substring (0, charLimit - 1);
        }

        void OnKeyboardDidShow(object sender, CCIMEKeyboardNotificationInfo e)
        {
            if (!action)
            {
                textFieldActionState = textField.RepeatForever(textFieldAction);
                action = true;
            }
        }

        void OnKeyboardDidHide(object sender, CCIMEKeyboardNotificationInfo e)
        {
            if (action)
            {
                textField.StopAction(textFieldActionState);
                textField.Opacity = 255;
                action = false;
            }

        }

    }

    public class TextFieldUpperCaseTest : KeyboardNotificationLayer
    {
        public override void OnClickTrackNode(bool bClicked)
        {
            if (bClicked && TrackNode != null)
            {
                TrackNode.Edit();
            }
            else
            {
                if (TrackNode != null && TrackNode != null)
                {
                    TrackNode.EndEdit();
                }
            }

        }

        public override void OnEnter()
        {
            base.OnEnter();

            var s = VisibleBoundsWorldspace.Size;

            var textField = new CCTextField("<CLICK HERE FOR INPUT>", 
                TextInputTestScene.FONT_NAME, 
                TextInputTestScene.FONT_SIZE, 
                CCLabelFormat.SpriteFont);

            var imeImplementation = textField.TextFieldIMEImplementation;
            imeImplementation.InsertText += OnInsertText;
            imeImplementation.ReplaceText += OnReplaceText;


            textField.Position = s.Center;

            textField.AutoEdit = true;

            AddChild(textField);

            TrackNode = textField;
        }

        public override void OnExit()
        {
            base.OnExit();
            // Remember to remove our event listeners.
            var imeImplementation = TrackNode.TextFieldIMEImplementation;
            imeImplementation.InsertText -= OnInsertText;
            imeImplementation.ReplaceText -= OnReplaceText;

        }

        void OnInsertText (object sender, CCIMEKeybardEventArgs e)
        {
            var focusedTextField = sender as CCTextField;

            e.Text = e.Text.ToUpper();

        }

        void OnReplaceText (object sender, CCIMEKeybardEventArgs e)
        {
            var focusedTextField = sender as CCTextField;

            e.Text = e.Text.ToUpper();

        }


        public override string Subtitle
        {
            get {
                return "TextField Uppercase test";
            }
        }
    }

    public class TextFieldCustomIMETest : KeyboardNotificationLayer
    {
        public override void OnClickTrackNode(bool bClicked)
        {
            if (bClicked && TrackNode != null)
            {
                TrackNode.Edit();
            }
            else
            {
                if (TrackNode != null && TrackNode != null)
                {
                    TrackNode.EndEdit();
                }
            }

        }

        public override void OnEnter()
        {
            base.OnEnter();

            var s = VisibleBoundsWorldspace.Size;

            var textField = new CCTextField("<CLICK HERE FOR INPUT>", 
                TextInputTestScene.FONT_NAME, 
                TextInputTestScene.FONT_SIZE, 
                CCLabelFormat.SpriteFont);

            // Override the default implementation
            textField.TextFieldIMEImplementation = UppercaseIMEKeyboardImpl.SharedInstance;

            textField.Position = s.Center;

            textField.AutoEdit = true;

            AddChild(textField);

            TrackNode = textField;
        }

        public override string Subtitle
        {
            get {
                return "TextField Custom Uppercase IME implementation";
            }
        }
    }

}
