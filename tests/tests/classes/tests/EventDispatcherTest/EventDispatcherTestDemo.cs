using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
namespace tests
{


	public enum EventDispatchTests
	{
		TOUCHABLE_SPRITE_TEST = 0,
		EVENT_MOUSE,
		TEST_LABEL_KEYBOARD,
		TEST_ACCELEROMETER,
		TEST_CASE_COUNT
	};

	// the class inherit from TestScene
	// every Scene each test used must inherit from TestScene,
	// make sure the test have the menu item for back to main menu
	public class EventDispatcherTestScene : TestScene
	{
		public static int sceneIndex = -1;

		public static CCLayer CreateLayer(int index)
		{
			CCLayer testLayer = null;

			switch (index)
			{
			case (int) EventDispatchTests.TOUCHABLE_SPRITE_TEST:
				testLayer = new TouchableSpriteTest();
				break;
			case (int) EventDispatchTests.EVENT_MOUSE:
				testLayer = new MouseEventTest();
				break;
			case (int) EventDispatchTests.TEST_LABEL_KEYBOARD:
				testLayer = new LabelKeyboardEventTest();
				break;
			case (int) EventDispatchTests.TEST_ACCELEROMETER:
				testLayer = new SpriteAccelerationEventTest();
				break;
			default:
				break;
			}

			return testLayer;
		}

		protected override void NextTestCase()
		{
			NextAction();
		}
		protected override void PreviousTestCase()
		{
			BackAction();
		}
		protected override void RestTestCase()
		{
			RestartAction();
		}

		public static CCLayer NextAction()
		{
			++sceneIndex;
			sceneIndex = sceneIndex % (int)EventDispatchTests.TEST_CASE_COUNT;

			var testLayer = CreateLayer(sceneIndex);

			return testLayer;
		}

		public static CCLayer BackAction()
		{
			--sceneIndex;
			if (sceneIndex < 0)
				sceneIndex += (int)EventDispatchTests.TEST_CASE_COUNT;

			var testLayer = CreateLayer(sceneIndex);

			return testLayer;
		}

		public static CCLayer RestartAction()
		{
			var testLayer = CreateLayer(sceneIndex);

			return testLayer;
		}


		public override void runThisTest()
		{
			sceneIndex = -1;
			AddChild(NextAction());

			CCDirector.SharedDirector.ReplaceScene(this);
		}
	}


	public class EventDispatcherTest : CCLayer
	{
		public override void OnEnter ()
		{
			base.OnEnter ();

			var label = new CCLabelTtf(title(), "arial", 32);
			AddChild(label, TestScene.TITLE_LEVEL);
			label.Position = new CCPoint(CCVisibleRect.Center.X, CCVisibleRect.Top.Y - 30);

			var strSubtitle = subtitle();
			if (!string.IsNullOrEmpty(strSubtitle))
			{
				var l = new CCLabelTtf(strSubtitle, "Thonburi", 16);
				AddChild(l, TestScene.TITLE_LEVEL);
				l.Position = new CCPoint(CCVisibleRect.Center.X, CCVisibleRect.Top.Y - 60);
			}

			var item1 = new CCMenuItemImage(TestResource.s_pPathB1, TestResource.s_pPathB2, BackCallback);
			var item2 = new CCMenuItemImage(TestResource.s_pPathR1, TestResource.s_pPathR2, RestartCallback);
			var item3 = new CCMenuItemImage(TestResource.s_pPathF1, TestResource.s_pPathF2, NextCallback);

			var menu = new CCMenu(item1, item2, item3);

			menu.Position = CCPoint.Zero;
			item1.Position = new CCPoint (CCVisibleRect.Center.X - item2.ContentSize.Width * 2, CCVisibleRect.Bottom.Y + item2.ContentSize.Height / 2);
			item2.Position = new CCPoint (CCVisibleRect.Center.X, CCVisibleRect.Bottom.Y + item2.ContentSize.Height / 2);
			item3.Position = new CCPoint (CCVisibleRect.Center.X + item2.ContentSize.Width * 2, CCVisibleRect.Bottom.Y + item2.ContentSize.Height / 2);

			AddChild (menu, TestScene.MENU_LEVEL);
		}


		public void RestartCallback(object pSender)
		{
			CCScene s = new EventDispatcherTestScene();
			s.AddChild(EventDispatcherTestScene.RestartAction ());

			CCDirector.SharedDirector.ReplaceScene(s);
		}

		public void NextCallback(object pSender)
		{

			CCScene s = new EventDispatcherTestScene();
			s.AddChild(EventDispatcherTestScene.NextAction ());

			CCDirector.SharedDirector.ReplaceScene(s);
		}

		public void BackCallback(object pSender)
		{

			CCScene s = new EventDispatcherTestScene();
			s.AddChild(EventDispatcherTestScene.BackAction ());

			CCDirector.SharedDirector.ReplaceScene(s);
		}

		public virtual string title()
		{
			return "No title";
		}

		public virtual string subtitle()
		{
			return "";
		}
	}

	public class MouseEventTest : EventDispatcherTest
	{

		CCLabelTtf mousePosition;
		CCLabelTtf mouseButtonDown;
		CCLabelTtf mouseButtonUp;
		CCLabelTtf scrollWheel;

		public override void OnEnter ()
		{
			base.OnEnter ();

			int line = (int)(CCVisibleRect.VisibleRect.Size.Height / 2);

			mousePosition = new CCLabelTtf ("Mouse Position: ", "arial", 20);
			mousePosition.Position = new CCPoint (30, line + 60);
			mousePosition.AnchorPoint = CCPoint.AnchorMiddleLeft;
			AddChild (mousePosition);

			mouseButtonDown = new CCLabelTtf ("Mouse Button Down: ", "arial", 20);
			mouseButtonDown.Position = new CCPoint (30, line + 20);
			mouseButtonDown.AnchorPoint = CCPoint.AnchorMiddleLeft;
			AddChild (mouseButtonDown);

			mouseButtonUp = new CCLabelTtf ("Mouse Button Up: ", "arial", 20);
			mouseButtonUp.Position = new CCPoint (30, line - 20);
			mouseButtonUp.AnchorPoint = CCPoint.AnchorMiddleLeft;
			AddChild (mouseButtonUp);

			scrollWheel = new CCLabelTtf ("Scroll Wheel Delta: ", "arial", 20);
			scrollWheel.Position = new CCPoint (30, line - 60);
			scrollWheel.AnchorPoint = CCPoint.AnchorMiddleLeft;
			AddChild (scrollWheel);


			var mouseListener = new CCEventListenerMouse();
			mouseListener.OnMouseScroll = OnMouseScroll;
			mouseListener.OnMouseDown = OnMouseDown;
			mouseListener.OnMouseUp = OnMouseUp;
			mouseListener.OnMouseMove = OnMouseMove;
			EventDispatcher.AddEventListener (mouseListener, this);
		}


		void OnMouseUp(CCEventMouse mouseEvent)
		{
			mouseButtonUp.Text = "Mouse Button Up: " + mouseEvent.MouseButton;
		}
		void OnMouseDown(CCEventMouse mouseEvent)
		{
			mouseButtonDown.Text = "Mouse Button Down: " + mouseEvent.MouseButton;
		}
		void OnMouseScroll(CCEventMouse mouseEvent)
		{
			scrollWheel.Text = "Scroll Wheel Delta: X: " + mouseEvent.ScrollX + " Y: " + mouseEvent.ScrollY;
		}
		void OnMouseMove(CCEventMouse mouseEvent)
		{
			mousePosition.Text = "Mouse Position: X: " + mouseEvent.CursorX + " Y: " + mouseEvent.CursorY;
		}

		public override string title()
		{
			return "Testing Mouse EventsDispatcher";
		}

		public override string subtitle()
		{
			return "Mouse Move, Buttons and Scroll";
		}

	}

	public class LabelKeyboardEventTest : EventDispatcherTest
	{

		public override void OnEnter ()
		{
			base.OnEnter ();

			var origin = CCDirector.SharedDirector.VisibleOrigin;
			var size = CCDirector.SharedDirector.VisibleSize;

			var statusLabel = new CCLabelTtf("No keyboard event received!", "arial", 20);
			statusLabel.Position = origin + size.Center;
			AddChild(statusLabel);

			// Create our Keyboard Listener
			var listener = new CCEventListenerKeyboard();

			// We will use Lambda expressions to attach the event process
			listener.OnKeyPressed = (keyboardEvent) => {
				var labelText = string.Format("Key {0} was pressed.", keyboardEvent.Keys);
				statusLabel.Text = labelText;
			};
			listener.OnKeyReleased = (keyboardEvent) => {
				var labelText = string.Format("Key {0} was released.", keyboardEvent.Keys);
				statusLabel.Text = labelText;
			};

			// Now we tell the event dispatcher that the status label is interested in keyboard events
			CCDirector.SharedDirector.EventDispatcher.AddEventListener(listener, statusLabel);		
		}

		public override string title()
		{
			return "Label Receives Keyboard Event";;
		}

		public override string subtitle()
		{
			return "Please click keyboard\n(Only available on Desktop and Android)";
		}

	}

	public class SpriteAccelerationEventTest : EventDispatcherTest
	{

		public override void OnEnter ()
		{
			base.OnEnter ();

			AccelerometerEnabled = true;

			var origin = CCDirector.SharedDirector.VisibleOrigin;
			var size = CCDirector.SharedDirector.VisibleSize;

			var sprite = new CCSprite(TestResource.s_Ball);
			sprite.Position = origin + size.Center;
			AddChild(sprite);

			// Create our Accelerometer Listener
			var listener = new CCEventListenerAccelerometer();

			// We will use Lambda expressions to attach the event process
			listener.OnAccelerate = (acceleration) => {
				var ballSize  = sprite.ContentSize;
				var acc = acceleration.Acceleration;
				var ptNow  = sprite.Position;

				var orientation = CCApplication.SharedApplication.CurrentOrientation;

				//CCLog.Log("Accelerate : X: {0} Y: {1} Z: {2} orientation: {3}", accelerationValue.X, accelerationValue.Y, accelerationValue.Z, orientation );
				#if ANDROID || WINDOWS_PHONE8
				if (orientation == DisplayOrientation.LandscapeRight)
				{
					ptNow.X -= (float) acc.X * 9.81f;
					ptNow.Y -= (float) acc.Y * 9.81f;
				}
				else
				{
					ptNow.X += (float)acc.X * 9.81f;
					ptNow.Y += (float)acc.Y * 9.81f;
				}
				#else
				ptNow.X += (float)acc.X * 9.81f;
				ptNow.Y += (float)acc.Y * 9.81f;
				#endif
				ptNow.X = MathHelper.Clamp(ptNow.X, (float)(CCVisibleRect.Left.X+ballSize.Width / 2.0), (float)(CCVisibleRect.Right.X - ballSize.Width / 2.0));
				ptNow.Y = MathHelper.Clamp(ptNow.Y, (float)(CCVisibleRect.Bottom.Y+ballSize.Height / 2.0), (float)(CCVisibleRect.Top.Y - ballSize.Height / 2.0));
				sprite.Position = ptNow;
			};

			// Now we tell the event dispatcher that the sprite is interested in Accelerometer events
			CCDirector.SharedDirector.EventDispatcher.AddEventListener(listener, sprite);		
		}

		public override void OnExit ()
		{
			base.OnExit ();
			AccelerometerEnabled = false;

		}
		public override string title()
		{
			return "Sprite Receives Acceleration Event";
		}

		public override string subtitle()
		{
			return "Please move your device\n(Only available on mobile and emulated on Desktop)";
		}

	}

	public class TouchableSpriteTest : EventDispatcherTest
	{

		public override void OnEnter ()
		{
			base.OnEnter ();

			var origin = CCDirector.SharedDirector.VisibleOrigin;
			var size = CCDirector.SharedDirector.VisibleSize;

			var sprite1 = new CCSprite("Images/CyanSquare.png");
			sprite1.Position = origin + new CCPoint (size.Width / 2, size.Height / 2) + new CCPoint (-80, 80);
			AddChild(sprite1, 10);

			var sprite2 = new CCSprite("Images/MagentaSquare.png");
			sprite2.Position = origin + new CCPoint (size.Width / 2, size.Height / 2);
			AddChild(sprite2, 20);

			var sprite3 = new CCSprite("Images/YellowSquare.png");
			sprite3.Position = CCPoint.Zero;
			sprite2.AddChild(sprite3, 1);

			// Make sprite1 touchable
			var listener1 = new CCEventListenerTouchOneByOne ();
			listener1.IsSwallowTouches = true;

			listener1.OnTouchBegan = (touch, touchEvent) => 
			{
				var target = (CCSprite)touchEvent.CurrentTarget;

				var locationInNode = target.ConvertToNodeSpace(touch.Location);
				var s = target.ContentSize;
				CCRect rect = new CCRect(0, 0, s.Width, s.Height);

				if (rect.ContainsPoint(locationInNode))
				{
					CCLog.Log("sprite began... x = {0}, y = {1}", locationInNode.X, locationInNode.Y);
					target.Opacity = 180;
					return true;
				}
				return false;
			};

			listener1.OnTouchMoved = (touch, touchEvent) => 
			{
				var target = (CCSprite)touchEvent.CurrentTarget;
				target.Position += touch.Delta;
			};

			listener1.OnTouchEnded = (touch, touchEvent) => 
			{
				var target = (CCSprite)touchEvent.CurrentTarget;
				CCLog.Log("sprite onTouchesEnded..");
				target.Opacity = 255;
				if (target == sprite2)
				{
					sprite1.LocalZOrder = 100;
				}
				else if(target == sprite1)
				{
					sprite1.LocalZOrder = 0;
				}
			};



			EventDispatcher.AddEventListener(listener1, sprite1);
			EventDispatcher.AddEventListener (listener1.Copy(), sprite2);
			EventDispatcher.AddEventListener (listener1.Copy(), sprite3);

			var removeAllTouchItem = new CCMenuItemFont("Remove All Touch Listeners", (sender) => {
				var senderItem = (CCMenuItemFont)sender;
				senderItem.LabelTTF.Text = "Only Next item could be clicked";

				EventDispatcher.RemoveEventListeners(CCEventListenerType.TOUCH_ONE_BY_ONE);

				var nextItem = new CCMenuItemFont("Next", (senderNext) => NextCallback(senderNext));
			

				nextItem.FontSize = 16;
				nextItem.Position = CCVisibleRect.Right + new CCPoint(-100, -30);

				var menu2 = new CCMenu(nextItem);
				menu2.Position = CCPoint.Zero;
				menu2.AnchorPoint = CCPoint.AnchorLowerLeft;
				this.AddChild(menu2);
			});

			removeAllTouchItem.FontSize = 16;
			removeAllTouchItem.Position = CCVisibleRect.Right + new CCPoint(-100, 0);

			var menu = new CCMenu(removeAllTouchItem);
			menu.Position = CCPoint.Zero;
			menu.AnchorPoint = CCPoint.AnchorLowerLeft;
			AddChild(menu);


		}

		public override void OnExit ()
		{
			base.OnExit ();

		}
		public override string title()
		{
			return "Touchable Sprite Test";
		}

		public override string subtitle()
		{
			return "Please drag the blocks";
		}

	}


}
