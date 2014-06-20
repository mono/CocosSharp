using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using CocosSharp;
using Microsoft.Xna.Framework;

namespace tests
{


	public enum EventDispatchTests
	{
		TOUCHABLE_SPRITE_TEST = 0,
		FIXED_PRIORITY_TEST,
		EVENT_MOUSE,
		TEST_LABEL_KEYBOARD,
		TEST_ACCELEROMETER,
		TEST_CUSTOM_EVENT,
		TEST_REMOVE_RETAIN_NODE,
		TEST_REMOVE_AFTER_ADDING,
		TEST_DIRECTOR,
		TEST_GLOBAL_Z_TOUCH,
		TEST_PAUSE_RESUME,
		TEST_SMOOTH_FOLLOW,
		TEST_STOP_PROPAGATION,
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
			case (int) EventDispatchTests.FIXED_PRIORITY_TEST:
				testLayer = new FixedPriorityTest();
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
			case (int) EventDispatchTests.TEST_CUSTOM_EVENT:
				testLayer = new CustomEventTest();
				break;
			case (int) EventDispatchTests.TEST_REMOVE_RETAIN_NODE:
				testLayer = new RemoveAndRetainNodeTest();
				break;
			case (int) EventDispatchTests.TEST_REMOVE_AFTER_ADDING:
				testLayer = new RemoveListenerAfterAddingTest();
				break;
			case (int) EventDispatchTests.TEST_DIRECTOR:
				testLayer = new DirectorTest();
				break;
			case (int) EventDispatchTests.TEST_GLOBAL_Z_TOUCH:
				testLayer = new GlobalZTouchTest();
				break;
			case (int) EventDispatchTests.TEST_PAUSE_RESUME:
				testLayer = new PauseResumeTest();
				break;
			case (int) EventDispatchTests.TEST_SMOOTH_FOLLOW:
				testLayer = new SmoothFollowTest();
				break;
			case (int) EventDispatchTests.TEST_STOP_PROPAGATION:
				testLayer = new StopPropagationTest();
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

			Director.ReplaceScene(this);
		}
	}


	public class EventDispatcherTest : TestNavigationLayer
	{

		public override void RestartCallback(object sender)
		{
			CCScene s = new EventDispatcherTestScene();
			s.AddChild(EventDispatcherTestScene.RestartAction ());

			Director.ReplaceScene(s);
		}


		public override void NextCallback(object sender)
		{

			CCScene s = new EventDispatcherTestScene();
			s.AddChild(EventDispatcherTestScene.NextAction ());

			Director.ReplaceScene(s);
		}

		public override void BackCallback(object sender)
		{

			CCScene s = new EventDispatcherTestScene();
			s.AddChild(EventDispatcherTestScene.BackAction ());

			Director.ReplaceScene(s);
		}

		public override string Title
		{
			get
			{
				return "No title";
			}
		}

		public override string Subtitle
		{
			get
			{
				return string.Empty;
			}
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
            EventDispatcher.AddEventListener(mouseListener, this);
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
		public override string Title
		{
			get
			{
				return "Testing Mouse EventsDispatcher";
			}
		}

		public override string Subtitle
		{
			get
			{
				return "Mouse Move, Buttons and Scroll";
			}
		}

	}

	public class LabelKeyboardEventTest : EventDispatcherTest
	{

		public override void OnEnter ()
		{
			base.OnEnter ();

			var origin = Director.VisibleOrigin;
			var size = Director.VisibleSize;

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
			EventDispatcher.AddEventListener(listener, statusLabel);		
		}

		public override string Title
		{
			get
			{
				return "Label Receives Keyboard Event";
			}
		}
		public override string Subtitle
		{
			get
			{
				return "Please click keyboard\n(Only available on Desktop and Android)";
			}
		}

	}

	public class SpriteAccelerationEventTest : EventDispatcherTest
	{

		public override void OnEnter ()
		{
			base.OnEnter ();

			var origin = Director.VisibleOrigin;
			var size = Director.VisibleSize;

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
				if (orientation == CCDisplayOrientation.LandscapeRight)
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
			EventDispatcher.AddEventListener(listener, sprite);		
		}

		public override void OnExit ()
		{
			base.OnExit ();

		}

		public override string Title
		{
			get
			{
				return "Sprite Receives Acceleration Event";
			}
		}
		public override string Subtitle
		{
			get
			{
				return "Please move your device\n(Only available on mobile and emulated on Desktop)";
			}
		}

	}

	public class TouchableSpriteTest : EventDispatcherTest
	{

		public override void OnEnter ()
		{
			base.OnEnter ();

			var origin = Director.VisibleOrigin;
			var size = Director.VisibleSize;

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
            EventDispatcher.AddEventListener(listener1.Copy(), sprite2);
            EventDispatcher.AddEventListener(listener1.Copy(), sprite3);


			var removeAllTouchItem = new CCMenuItemFont("Remove All Touch Listeners", (sender) => {
				var senderItem = (CCMenuItemFont)sender;
				senderItem.LabelTTF.Text = "Only Next item could be clicked";

                Director.EventDispatcher.RemoveEventListeners(CCEventListenerType.TOUCH_ONE_BY_ONE);

				var nextItem = new CCMenuItemFont("Next", (senderNext) => NextCallback(senderNext));
			

				CCMenuItemFont.FontSize = 16;
				nextItem.Position = CCVisibleRect.Right + new CCPoint(-100, -30);

				var menu2 = new CCMenu(nextItem);
				menu2.Position = CCPoint.Zero;
				menu2.AnchorPoint = CCPoint.AnchorLowerLeft;
				this.AddChild(menu2);
			});

			CCMenuItemFont.FontSize = 16;
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

		public override string Title
		{
			get
			{
				return "Touchable Sprite Test";
			}
		}

		public override string Subtitle
		{
			get
			{
				return "Please drag the blocks";
			}
		}

	}

	public class FixedPriorityTest : EventDispatcherTest
	{

		public override void OnEnter()
		{
			base.OnEnter ();

			var origin = Director.VisibleOrigin;
			var size = Director.VisibleSize;

			var sprite1 = new TouchableSprite (30);
			var texture = CCApplication.SharedApplication.TextureCache.AddImage("Images/CyanSquare.png");
			sprite1.Texture = texture;
			sprite1.Position = origin + new CCPoint (size.Width / 2, size.Height / 2) + new CCPoint (-80, 80);
			AddChild(sprite1, 10);

			var sprite2 = new TouchableSprite (20);
			texture = CCApplication.SharedApplication.TextureCache.AddImage("Images/MagentaSquare.png");
			sprite2.Texture = texture;
			sprite2.Position = origin + new CCPoint (size.Width / 2, size.Height / 2);
			AddChild(sprite2, 20);

			var sprite3 = new TouchableSprite (10);
			texture = CCApplication.SharedApplication.TextureCache.AddImage("Images/YellowSquare.png");
			sprite3.Texture = texture;
			sprite3.Position = CCPoint.Zero;
			sprite2.AddChild(sprite3, 1);

		}

		public override string Title
		{
			get
			{
				return "Fixed priority test";
			}
		}

		public override string Subtitle
		{
			get
			{
				return "Fixed Priority, Blue: 30, Red: 20, Yellow: 10\n The lower value the higher priority will be.";
			}
		}

	}

	class TouchableSprite : CCSprite
	{

		CCEventListenerTouchOneByOne Listener { get; set; }
		int FixedPriority { get; set; }
		bool IsRemoveListenerOnTouchEnded { get; set; }

		public TouchableSprite(int priority = 0)
		{
			FixedPriority = priority;
			IsRemoveListenerOnTouchEnded = false;
		}

		public override void OnEnter ()
		{
			base.OnEnter ();
			var listener = new CCEventListenerTouchOneByOne();
			listener.IsSwallowTouches = true;

			listener.OnTouchBegan = (CCTouch touch, CCEvent rouchEvent) => 
			{

				var locationInNode = ConvertToNodeSpace(touch.Location);
				var s = ContentSize;
				var rect = new CCRect(0, 0, s.Width, s.Height);

				if (rect.ContainsPoint(locationInNode))
				{
					Color = CCColor3B.Red;
					return true;
				}
				return false;
			};

			listener.OnTouchEnded = (CCTouch touch, CCEvent rouchEvent) =>
			{
				Color = CCColor3B.White;

				if (IsRemoveListenerOnTouchEnded)
				{
                    this.EventDispatcher.RemoveEventListener(Listener);
				}
			};

            if (FixedPriority != 0)
            {
                EventDispatcher.AddEventListener(listener, FixedPriority);
            }
            else
            {
                EventDispatcher.AddEventListener(listener, this);
            }

			Listener = listener;
		}

		public override void OnExit ()
		{
            this.EventDispatcher.RemoveEventListener(Listener);
			base.OnExit();
		}
	}

	public class CustomEventTest : EventDispatcherTest
	{

		CCEventListenerCustom listener;
		CCEventListenerCustom listener2;

		public override void OnEnter ()
		{
			base.OnEnter ();

			var origin = Director.VisibleOrigin;
			var size = Director.VisibleSize;

			//MenuItemFont::setFontSize(20);

			var statusLabel = new CCLabelTtf("No custom event 1 received!", "", 20);
			statusLabel.Position = origin + new CCPoint(size.Width/2, size.Height-90);
			AddChild(statusLabel);

			listener = new CCEventListenerCustom("game_custom_event1", (customEvent) =>
				{
					var str = "Custom event 1 received, ";
					var buf = customEvent.UserData;
					str += buf;
					str += " times";
					statusLabel.Text = str;
			});

            EventDispatcher.AddEventListener(listener, 1);
			var count = 0;
			var sendItem = new CCMenuItemFont("Send Custom Event 1", (sender) =>
				{
					++count;
					var userData = string.Format("{0}", count);
					var customEvent = new CCEventCustom("game_custom_event1");
					customEvent.UserData = userData;
                    EventDispatcher.DispatchEvent(customEvent);
			});

			sendItem.Position = origin + size.Center;

			var statusLabel2 = new CCLabelTtf("No custom event 2 received!", "", 20);
			statusLabel2.Position = origin + new CCPoint(size.Width/2, size.Height-120);
			AddChild(statusLabel2);

			listener2 = new CCEventListenerCustom("game_custom_event2", (customEvent) =>
				{
					statusLabel2.Text = string.Format("Custom event 2 received, {0} times", customEvent.UserData);
				});

            this.EventDispatcher.AddEventListener(listener2, 1);

			var count2 = 0;
			var sendItem2 = new CCMenuItemFont("Send Custom Event 2", (sender) =>
				{
					var customEvent = new CCEventCustom("game_custom_event2");
					customEvent.UserData = ++count2;
                    EventDispatcher.DispatchEvent(customEvent);
				});

			sendItem2.Position = origin + new CCPoint(size.Width / 2, size.Height / 2 - 40);

			var menu = new CCMenu(sendItem, sendItem2);
			menu.Position = CCPoint.Zero;
			menu.AnchorPoint = CCPoint.AnchorUpperLeft;
			AddChild(menu, -1);
		}

		public override void OnExit ()
		{
			// Don't forget to remove the fixed priority Event listeners yourself.
            this.EventDispatcher.RemoveEventListener(listener);
            this.EventDispatcher.RemoveEventListener(listener2);
			base.OnExit ();
		}
		public override string Title
		{
			get
			{
				return "Send Custom Event";
			}
		}

	}

	public class RemoveAndRetainNodeTest : EventDispatcherTest
	{

		bool spriteSaved = false;

		public override void OnEnter ()
		{

			spriteSaved = false;

			base.OnEnter ();

			var origin = Director.VisibleOrigin;
			var size = Director.VisibleSize;

			//MenuItemFont::setFontSize(20);

			var sprite = new CCSprite("Images/CyanSquare.png");
			sprite.Position = origin + size.Center;
			AddChild(sprite, 10);

			// Make sprite1 touchable
			var listener1 = new CCEventListenerTouchOneByOne ();
			listener1.IsSwallowTouches = true;

			listener1.OnTouchBegan = (touch, touchEvent) => 
			{
				var target = (CCSprite) touchEvent.CurrentTarget;

				var locationInNode = target.ConvertToNodeSpace(touch.Location);
				var s = target.ContentSize;
				var rect = new CCRect(0, 0, s.Width, s.Height);

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
				var target = (CCSprite) touchEvent.CurrentTarget;
				target.Position += touch.Delta;
			};

			listener1.OnTouchEnded = (touch, touchEvent) =>
			{
				var target = (CCSprite) touchEvent.CurrentTarget;
				CCLog.Log("sprite onTouchesEnded.. ");
				target.Opacity = 255;
			};

            EventDispatcher.AddEventListener(listener1, sprite);

			RunActions(new CCDelayTime(5.0f),
				new CCCallFunc(() => 
				{
					spriteSaved = true;
					sprite.RemoveFromParent();
				}),
				new CCDelayTime(5.0f),
				new CCCallFunc(() =>
				{
					spriteSaved = false;
					AddChild(sprite);
				})
			);


		}

		public override void OnExit ()
		{
			// release it not needed.
			//if (spriteSaved)
				// do release
			base.OnExit ();
		}

		public override string Title
		{
			get
			{
				return "RemoveAndRetainNodeTest";
			}
		}

		public override string Subtitle
		{
			get
			{
				return "Sprite should be removed after 5s, add to scene again after 5s";
			}
		}

	}

	public class RemoveListenerAfterAddingTest : EventDispatcherTest
	{

		public override void OnEnter ()
		{

			base.OnEnter ();

			var item1 = new  CCMenuItemFont("Click Me 1", (sender) =>
				{
					var listener = new CCEventListenerTouchOneByOne();
					listener.OnTouchBegan = (touch, touchEvent) => 
					{
						Debug.Assert(false, "Should not come here!");
						return true;
					};

                    this.EventDispatcher.AddEventListener(listener, -1);
                    this.EventDispatcher.RemoveEventListeners(CCEventListenerType.TOUCH_ONE_BY_ONE);
			});

			item1.Position = CCVisibleRect.Center + new CCPoint(0, 80);

			var addNextButton = new Action( () =>
			{
				var next = new CCMenuItemFont("Please Click Me To Reset!", (sender) => 
				{
						RestartCallback(null);
				});
				next.Position = CCVisibleRect.Center + new CCPoint(0, -40);

				var menuNext = new CCMenu(next);
				menuNext.Position = CCVisibleRect.LeftBottom;
				menuNext.AnchorPoint = CCPoint.Zero;
				AddChild(menuNext);
				});

			var item2 = new CCMenuItemFont ("Click Me 2", (sender) => 
			{
				var listener = new CCEventListenerTouchOneByOne ();
				listener.OnTouchBegan = (touch, touchEvent) => {
					Debug.Assert (false, "Should not come here!");
					return true;
				};

                    this.EventDispatcher.AddEventListener(listener, -1);
                    this.EventDispatcher.RemoveEventListeners(CCEventListenerType.TOUCH_ONE_BY_ONE);

					addNextButton ();
			});

			item2.Position = CCVisibleRect.Center + new CCPoint(0, 40);

			var item3 = new CCMenuItemFont("Click Me 3", (sender) => 
				{
					var listener = new CCEventListenerTouchOneByOne ();
					listener.OnTouchBegan = (touch, touchEvent) => {
						Debug.Assert (false, "Should not come here!");
						return true;
					};

                    EventDispatcher.AddEventListener(listener, -1);
                    EventDispatcher.RemoveEventListeners(CCEventListenerType.TOUCH_ONE_BY_ONE);

					addNextButton();
			});

			item3.Position = CCVisibleRect.Center;

			var menu = new CCMenu(item1, item2, item3);
			menu.Position = CCVisibleRect.LeftBottom;
			menu.AnchorPoint = CCPoint.Zero;

			AddChild(menu);

		}

		public override string Title
		{
			get
			{
				return "RemoveListenerAfterAddingTest";
			}
		}

		public override string Subtitle
		{
			get
			{
				return "Should not crash!";
			}
		}
	}

	public class DirectorTest : EventDispatcherTest
	{

		int count1, count2, count3, count4;

		CCLabelTtf label1, label2, label3, label4;
		CCEventListenerCustom event1, event2, event3, event4;

		public override void OnEnter ()
		{

			base.OnEnter ();

			var origin = Director.VisibleOrigin;
			var s = Director.VisibleSize;

			label1 = new CCLabelTtf("Update: 0", "arial", 20);
			label1.AnchorPoint = CCPoint.AnchorUpperLeft;
			label1.Position = new CCPoint(30,s.Height/2 + 60);
			AddChild(label1);

			label2 = new CCLabelTtf("Visit: 0", "arial", 20);
			label2.AnchorPoint = CCPoint.AnchorUpperLeft;
			label2.Position = new CCPoint(30,s.Height/2 + 20);
			AddChild(label2);

			label3 = new CCLabelTtf("Draw: 0", "arial", 20);
			label3.AnchorPoint = CCPoint.AnchorUpperLeft;
			label3.Position = new CCPoint(30,s.Height/2 - 20);
			AddChild(label3);

			label4 = new CCLabelTtf("Projection: 0", "arial", 20);
			label4.AnchorPoint = CCPoint.AnchorUpperLeft;
			label4.Position = new CCPoint(30,s.Height/2 - 60);
			AddChild(label4);

			var dispatcher = Director.EventDispatcher;

			event1 = dispatcher.AddCustomEventListener(CCDirector.EVENT_AFTER_UPDATE, OnEvent1);
			event2 = dispatcher.AddCustomEventListener(CCDirector.EVENT_AFTER_VISIT, OnEvent2);
			event3 = dispatcher.AddCustomEventListener(CCDirector.EVENT_AFTER_DRAW, (customEvent) =>
				{
					label3.Text = string.Format("Draw: {0}", count3++);
				});
			event4 = dispatcher.AddCustomEventListener(CCDirector.EVENT_PROJECTION_CHANGED, (customEvent) =>
				{
					label4.Text = string.Format("Projection: {0}", count4++);
				});

			Schedule();

		}

		float time = 0;

		public override void Update (float dt)
		{
			base.Update (dt);

			time += dt;
			if(time > 0.5) {
				Director.Projection = CCDirectorProjection.Projection2D;
				time = 0;
			}

		}

		void OnEvent1(CCEventCustom customEvent)
		{
			label1.Text = string.Format("Update: {0}", count1++);
		}

		void OnEvent2(CCEventCustom customEvent)
		{
			label2.Text = string.Format("Visit: {0}", count2++);
		}

		public override void OnExit ()
		{
            this.EventDispatcher.RemoveEventListener (event1);
            this.EventDispatcher.RemoveEventListener (event2);
            this.EventDispatcher.RemoveEventListener (event3);
            this.EventDispatcher.RemoveEventListener (event4);
			base.OnExit ();
		}

		public override string Title
		{
			get
			{
				return "Testing Director Events";
			}
		}

		public override string Subtitle
		{
			get
			{
				return "after visit, after draw, after update, projection changed";
			}
		}

	}

	public class GlobalZTouchTest : EventDispatcherTest
	{

		const int TAG_SPRITE = 100;
		const int SPRITE_COUNT = 8;
		const int TAG_SPRITE_END = TAG_SPRITE + SPRITE_COUNT;

		float updateAccumulator = 0;
		CCSprite blueSprite;

		public GlobalZTouchTest() : base()
		{

			for (int i = 0; i < SPRITE_COUNT; i++)
			{
				CCSprite sprite;

				if(i==4)
				{
					sprite = new CCSprite("Images/CyanSquare.png") { Tag = TAG_SPRITE + i};
					blueSprite = sprite;
					blueSprite.GlobalZOrder = -1;

				}
				else
				{
					sprite = new CCSprite("Images/YellowSquare.png") { Tag = TAG_SPRITE + i};
				}

				AddChild(sprite);

			}

		}

		protected override void RunningOnNewWindow (CCSize windowSize)
		{
			base.RunningOnNewWindow(windowSize);

			var listener = new CCEventListenerTouchOneByOne();
			listener.IsSwallowTouches = true;

			listener.OnTouchBegan = (touch, touchEvent) =>
				{
					var target = (CCSprite)touchEvent.CurrentTarget;

					var locationInNode = target.ConvertToNodeSpace(touch.Location);
					var s = target.ContentSize;
					var rect = new CCRect(0, 0, s.Width, s.Height);
		
					if (rect.ContainsPoint(locationInNode))
					{
						CCLog.Log("sprite began... x = {0}, y = {1}", locationInNode.X, locationInNode.Y);
						target.Opacity = 180;
						return true;
					}
					return false;

				};

			listener.OnTouchMoved = (touch, touchEvent) =>
			{
				var target = (CCSprite)touchEvent.CurrentTarget;
				target.Position = target.Position + touch.Delta;
			};

			listener.OnTouchEnded = (touch, touchEvent) => 
				{
					var target = (CCSprite)touchEvent.CurrentTarget;
					CCLog.Log ("sprite OnTouchEnded...");
					target.Opacity = 255;
				};

			var visibleSize = Director.VisibleSize;
			var i = 0;
			foreach (var child in Children)
			{
				if (child.Tag >= TAG_SPRITE && child.Tag <= TAG_SPRITE_END)
				{
					child.Position = new CCPoint(CCVisibleRect.Left.X + visibleSize.Width / (SPRITE_COUNT - 1) * i, CCVisibleRect.Center.Y);
					i++;
					EventDispatcher.AddEventListener(listener.Copy(), child);
				}
			}

			Schedule();

		}

		public override void Update(float dt)
		{
			base.Update(dt);
			updateAccumulator += dt;
			if ( updateAccumulator > 2.0f)
			{
				var z = blueSprite.GlobalZOrder;
				CCLog.Log("GlobalZOrder {0} - New GlobalZOrder {1}.", z, -z);
				blueSprite.GlobalZOrder = -z;
				updateAccumulator = 0;
			}

		}

		public override string Title
		{
			get
			{
				return "Global Z Value, Try touch blue sprite";
			}
		}

		public override string Subtitle
		{
			get
			{
				return "Blue Sprite should change go from foreground to background";
			}
		}

	}

	public class PauseResumeTest : EventDispatcherTest
	{

		public override void OnEnter ()
		{
			base.OnEnter ();

			var origin = Director.VisibleOrigin;
			var size = Director.VisibleSize;

			var sprite1 = new TouchableSprite ();
			var texture = CCApplication.SharedApplication.TextureCache.AddImage("Images/CyanSquare.png");
			sprite1.Texture = texture;
			sprite1.Position = origin + new CCPoint (size.Width / 2, size.Height / 2) + new CCPoint (-80, 80);
			AddChild(sprite1, -10);

			var sprite2 = new TouchableSprite ();
			texture = CCApplication.SharedApplication.TextureCache.AddImage("Images/MagentaSquare.png");
			sprite2.Texture = texture;
			sprite2.Position = origin + new CCPoint (size.Width / 2, size.Height / 2);
			AddChild(sprite2, -20);

			var sprite3 = new TouchableSprite ();
			texture = CCApplication.SharedApplication.TextureCache.AddImage("Images/YellowSquare.png");
			sprite3.Texture = texture;
			sprite3.Position = CCPoint.Zero;
			sprite2.AddChild(sprite3, -1);

			CCMenuItemFont.FontSize = 20;
			CCMenuItemFont.FontName = "arial";

			var popup = new CCMenuItemFont("Popup", (sender) =>
				{

                    EventDispatcher.Pause(this,true);

					var colorLayer = new CCLayerColor(new CCColor4B(0, 0, 255, 100));
					AddChild(colorLayer, 99999);

					var closeItem = new CCMenuItemFont("close", (closeSender) =>
						{
							colorLayer.RemoveFromParent();
                            EventDispatcher.Resume(this, true);
				});

					closeItem.Position = CCVisibleRect.Center;

					var closeMenu = new CCMenu(closeItem);
					closeMenu.AnchorPoint = CCPoint.AnchorLowerLeft;
					closeMenu.Position = CCPoint.Zero;

					colorLayer.AddChild(closeMenu);
			});

			popup.AnchorPoint = CCPoint.AnchorMiddleRight;
			popup.Position = CCVisibleRect.Right;

			var menu = new CCMenu(popup);
			menu.AnchorPoint = CCPoint.AnchorLowerLeft;
			menu.Position = CCPoint.Zero;

			AddChild(menu);

		}

		public override string Title
		{
			get
			{
				return  "PauseResumeTargetTest";
			}
		}

	}

	public class SmoothFollowTest : EventDispatcherTest
	{

		CCTouch cyanTouch;
		const int TAG_CYAN_SPRITE = 101;

		public override void OnEnter ()
		{
			base.OnEnter ();

			var origin = Director.VisibleOrigin;
			var size = Director.VisibleSize;

			var sprite1 = new CCSprite("Images/CyanSquare.png");
			sprite1.Position = origin + size.Center;
			sprite1.Scale = 0.5f;
			sprite1.Name = "cyan";
			sprite1.Tag = TAG_CYAN_SPRITE;
			AddChild(sprite1, 10);

			// Make sprite1 touchable
			var listener1 = new CCEventListenerTouchAllAtOnce ();

			listener1.OnTouchesBegan = (touches, touchEvent) => 
			{
				cyanTouch = touches[0];
			};

            sprite1.EventDispatcher.AddEventListener(listener1, this);

			Schedule ();
		}

		public override void Update (float dt)
		{
			base.Update (dt);

			if (cyanTouch != null) 
			{
				MoveSpriteTowardPoint (cyanTouch.Location, dt);
			}

		}

		void MoveSpriteTowardPoint (CCPoint point, float dt)
		{
			var spriteSpeed = 130;
			var cyan = GetChildByTag (TAG_CYAN_SPRITE);
			var distanceLeft = Math.Sqrt(Math.Pow(cyan.Position.X - point.X, 2) +
				Math.Pow(cyan.Position.Y - point.Y, 2));

			if (distanceLeft > 4) {
				var distanceToTravel = dt * spriteSpeed;
				var angle = Math.Atan2 (point.Y - cyan.Position.Y,
					            point.X - cyan.Position.X);
				var yOffset = (float)(distanceToTravel * Math.Sin (angle));
				var xOffset = (float)(distanceToTravel * Math.Cos (angle));
				cyan.Position = new CCPoint (cyan.Position.X + xOffset,
					cyan.Position.Y + yOffset);
			} 
			else 
			{
				cyanTouch = null;
			}
		}

		public override void OnExit ()
		{
			base.OnExit ();

		}

		public override string Title
		{
			get
			{
				return "Smooth Follow Test";
			}
		}

		public override string Subtitle
		{
			get
			{
				return "Please touch and drag on the screen";
			}
		}

	}

	public class StopPropagationTest : EventDispatcherTest
	{

		const int TAG_BLUE_SPRITE = 101;
		const int TAG_BLUE_SPRITE2 = 102;

		public StopPropagationTest() : base()
		{
			var touchOneByOneListener = new CCEventListenerTouchOneByOne();
			touchOneByOneListener.IsSwallowTouches = true;

			touchOneByOneListener.OnTouchBegan = (touch, touchEvent) =>
			{
				// Skip if don't touch top half screen.
				if (!IsPointInTopHalfAreaOfScreen(touch.Location))
					return false;

				var target = (CCSprite)touchEvent.CurrentTarget;
				Debug.Assert (target.Tag == TAG_BLUE_SPRITE, "Yellow blocks shouldn't response event.");

				if (IsPointInNode(touch.Location, target))
				{
					target.Opacity = 180;
					return true;
				}

				// Stop propagation, so yellow blocks will not be able to receive event.
				touchEvent.StopPropogation();
				return false;
			};

			touchOneByOneListener.OnTouchEnded = (touch, touchEvent) => 
			{
				var target = (CCSprite)touchEvent.CurrentTarget;
				target.Opacity = 255;
			};

			var touchAllAtOnceListener = new CCEventListenerTouchAllAtOnce();
			touchAllAtOnceListener.OnTouchesBegan = (touches, touchEvent) => 
			{
				// Skip if don't touch top half screen.
				if (IsPointInTopHalfAreaOfScreen(touches[0].Location))
					return;

				var target = (CCSprite)touchEvent.CurrentTarget;
				Debug.Assert(target.Tag == TAG_BLUE_SPRITE2, "Yellow blocks shouldn't response event.");

				if (IsPointInNode(touches[0].Location, target))
				{
					target.Opacity = 180;
				}
				// Stop propagation, so yellow blocks will not be able to receive event.
				touchEvent.StopPropogation();
			};

			touchAllAtOnceListener.OnTouchesEnded = (touches, touchEvent) => 
			{
				// Skip if don't touch top half screen.
				if (IsPointInTopHalfAreaOfScreen(touches[0].Location))
					return;

				var target = (CCSprite)touchEvent.CurrentTarget;
				Debug.Assert(target.Tag == TAG_BLUE_SPRITE2, "Yellow blocks shouldn't response event.");

				if (IsPointInNode(touches[0].Location, target))
				{
					target.Opacity = 255;;
				}
				// Stop propagation, so yellow blocks will not be able to receive event.
				touchEvent.StopPropogation();
			};

			var keyboardEventListener = new CCEventListenerKeyboard();

			keyboardEventListener.OnKeyPressed = (keyboardEvent) => 
			{
				var target = (CCSprite)keyboardEvent.CurrentTarget;
				Debug.Assert(target.Tag == TAG_BLUE_SPRITE || target.Tag == TAG_BLUE_SPRITE2, "Yellow blocks shouldn't response event.");
				// Stop propagation, so yellow blocks will not be able to receive event.
				keyboardEvent.StopPropogation();
			};


			const int SPRITE_COUNT = 8;

			for (int i = 0; i < SPRITE_COUNT; i++)
			{
				CCSprite sprite;
				CCSprite sprite2;

				if(i==4)
				{
					sprite = new CCSprite("Images/CyanSquare.png");
					sprite.Tag = TAG_BLUE_SPRITE;
					AddChild(sprite, 100);

					sprite2 = new CCSprite("Images/CyanSquare.png");
					sprite2.Tag = TAG_BLUE_SPRITE2;
					AddChild(sprite2, 100);
				}
				else
				{
					sprite = new CCSprite("Images/YellowSquare.png");
					AddChild(sprite, 0);
					sprite2 = new CCSprite("Images/YellowSquare.png");
					AddChild(sprite2, 0);
				}

				EventDispatcher.AddEventListener(touchOneByOneListener.Copy(), sprite);
				EventDispatcher.AddEventListener(keyboardEventListener.Copy(), sprite);

				EventDispatcher.AddEventListener(touchAllAtOnceListener.Copy(), sprite2);
				EventDispatcher.AddEventListener(keyboardEventListener.Copy(), sprite2);


				var visibleSize = Director.VisibleSize;
				sprite.Position = new CCPoint( CCVisibleRect.Left.X + visibleSize.Width / (SPRITE_COUNT - 1) * i, CCVisibleRect.Center.Y + sprite2.ContentSize.Height/2 +10);
				sprite2.Position = new CCPoint( CCVisibleRect.Left.X + visibleSize.Width / (SPRITE_COUNT - 1) * i, CCVisibleRect.Center.Y - sprite2.ContentSize.Height/2-10);
			}

		}

		bool IsPointInNode(CCPoint pt, CCNode node)
		{
			var locationInNode = node.ConvertToNodeSpace(pt);
			var s = node.ContentSize;
			var rect = new CCRect(0, 0, s.Width, s.Height);

			if (rect.ContainsPoint(locationInNode))
			{
				return true;
			}
			return false;
		}

		bool IsPointInTopHalfAreaOfScreen(CCPoint pt)
		{
			var winSize = Director.WindowSizeInPoints;

			if (pt.Y >= winSize.Height/2) {
				return true;
			}

			return false;
		}

		public override string Title
		{
			get
			{
				return "Stop Propagation Test";
			}
		}

		public override string Subtitle
		{
			get
			{
				return "Shouldn't crash and only blue block could be clicked";
			}
		}

	}

}
