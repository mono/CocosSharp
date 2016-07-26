using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using CocosSharp;
using Microsoft.Xna.Framework;

namespace tests
{

	// the class inherit from TestScene
	// every Scene each test used must inherit from TestScene,
	// make sure the test have the menu item for back to main menu
	public class EventDispatcherTestScene : TestScene
	{
		public static int sceneIndex = -1;
        public static int MaxTests;

        public EventDispatcherTestScene()
        {
            MaxTests = eventDispatcherTestFunctions.Count;
        }

        static List<Func<CCLayer>> eventDispatcherTestFunctions = new List<Func<CCLayer>> ()
            {
                                () => new MouseEventTest(),

                () => new TouchableSpriteTest(),
                () => new TouchableSpriteTest2(),
                () => new FixedPriorityTest(),
                () => new MouseEventTest(),
                () => new LabelKeyboardEventTest(),
                () => new SpriteAccelerationEventTest(),
                () => new CustomEventTest(),
                () => new RemoveAndRetainNodeTest(),
                () => new RemoveListenerAfterAddingTest(),
                () => new DirectorTest(),
                () => new GlobalZTouchTest(),
                () => new PauseResumeTest(),
                () => new RemoveAllTest(),
                () => new SmoothFollowTest(),
                () => new StopPropagationTest(),
            };

		public static CCLayer CreateLayer(int index)
		{
            return eventDispatcherTestFunctions[index]();
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
			sceneIndex = sceneIndex % MaxTests;

			var testLayer = CreateLayer(sceneIndex);

			return testLayer;
		}

		public static CCLayer BackAction()
		{
			--sceneIndex;
			if (sceneIndex < 0)
				sceneIndex += MaxTests;

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

		CCLabel mousePosition;
		CCLabel mouseButtonDown;
		CCLabel mouseButtonUp;
		CCLabel scrollWheel;

        public MouseEventTest()
        {
            Color = CCColor3B.Blue;
            Opacity = 255;
        }

		public override void OnEnter ()
		{
            CCRect visibleBounds = VisibleBoundsWorldspace;

            base.OnEnter ();

            int line = (int)(visibleBounds.Size.Height / 2);

            mousePosition = new CCLabel ("Mouse Position: ", "arial", 20, CCLabelFormat.SpriteFont);
            mousePosition.Position = new CCPoint (130, line + 60);
			mousePosition.AnchorPoint = CCPoint.AnchorMiddleLeft;
			AddChild (mousePosition);

            mouseButtonDown = new CCLabel ("Mouse Button Down: ", "arial", 20, CCLabelFormat.SpriteFont);
            mouseButtonDown.Position = new CCPoint (130, line + 20);
			mouseButtonDown.AnchorPoint = CCPoint.AnchorMiddleLeft;
			AddChild (mouseButtonDown);

            mouseButtonUp = new CCLabel ("Mouse Button Up: ", "arial", 20, CCLabelFormat.SpriteFont);
            mouseButtonUp.Position = new CCPoint (130, line - 20);
			mouseButtonUp.AnchorPoint = CCPoint.AnchorMiddleLeft;
			AddChild (mouseButtonUp);

            scrollWheel = new CCLabel ("Scroll Wheel Delta: ", "arial", 20, CCLabelFormat.SpriteFont);
            scrollWheel.Position = new CCPoint (130, line - 60);
			scrollWheel.AnchorPoint = CCPoint.AnchorMiddleLeft;
			AddChild (scrollWheel);


			var mouseListener = new CCEventListenerMouse();
			mouseListener.OnMouseScroll = OnMouseScroll;
			mouseListener.OnMouseDown = OnMouseDown;
			mouseListener.OnMouseUp = OnMouseUp;
			mouseListener.OnMouseMove = OnMouseMove;
            AddEventListener(mouseListener);
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
            var cursor = mouseEvent.Cursor;
			mousePosition.Text = "Mouse Position: X: " + cursor.X + " Y: " + cursor.Y;
            mouseButtonDown.Text = "Mouse Button Down: " + mouseEvent.MouseButton;
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

			var origin = Layer.VisibleBoundsWorldspace.Origin;
			var size = Layer.VisibleBoundsWorldspace.Size;

            var statusLabel = new CCLabel("No keyboard event received!", "arial", 20, CCLabelFormat.SpriteFont);
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
			statusLabel.AddEventListener(listener);		
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

            CCRect visibleBounds = VisibleBoundsWorldspace;

			var origin = Layer.VisibleBoundsWorldspace.Origin;
			var size = Layer.VisibleBoundsWorldspace.Size;

			var sprite = new CCSprite(TestResource.s_Ball);
			sprite.Position = origin + size.Center;
			AddChild(sprite);

			// Create our Accelerometer Listener
			var listener = new CCEventListenerAccelerometer();

            #if !MACOS
            if (!GameView.Accelerometer.Enabled)
                GameView.Accelerometer.Enabled = true;
            #endif

			// We will use Lambda expressions to attach the event process
			listener.OnAccelerate = (acceleration) => {
				var ballSize  = sprite.ContentSize;
				var acc = acceleration.Acceleration;
				var ptNow  = sprite.Position;

                //var orientation = Application.CurrentOrientation;

				//CCLog.Log("Accelerate : X: {0} Y: {1} Z: {2} orientation: {3}", accelerationValue.X, accelerationValue.Y, accelerationValue.Z, orientation );
                //if (orientation == CCDisplayOrientation.LandscapeRight || orientation == CCDisplayOrientation.LandscapeLeft)
                //{

#if ANDROID
            
                ptNow.X += (float) acc.Y * 9.81f;
                ptNow.Y -= (float) acc.X * 9.81f;

#elif NETFX_CORE || WINDOWS_PHONE8
                    ptNow.X -= (float)acc.Y * 9.81f;
                    ptNow.Y += (float)acc.X * 9.81f;
#elif IOS
                    ptNow.X += (float)acc.Y * 9.81f;
                    ptNow.Y -= (float)acc.X * 9.81f;
#endif
                //}
                ptNow.X = MathHelper.Clamp(ptNow.X, (float)(visibleBounds.Origin.X+ballSize.Width / 2.0), (float)(visibleBounds.Origin.X + visibleBounds.Size.Width - ballSize.Width / 2.0));
                ptNow.Y = MathHelper.Clamp(ptNow.Y, (float)(visibleBounds.Origin.Y+ballSize.Height / 2.0), (float)(visibleBounds.Origin.Y + visibleBounds.Size.Height - ballSize.Height / 2.0));
				sprite.Position = ptNow;
			};

			// Now we tell the event dispatcher that the sprite is interested in Accelerometer events
			sprite.AddEventListener(listener);		
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

            var visibleBounds = VisibleBoundsWorldspace;
			var origin = VisibleBoundsWorldspace.Origin;
			var size = VisibleBoundsWorldspace.Size;

			var sprite1 = new CCSprite("Images/CyanSquare.png");
            sprite1.Position = origin + size.Center + new CCPoint (-80, 80);
			AddChild(sprite1, 10);

			var sprite2 = new CCSprite("Images/MagentaSquare.png");
            sprite2.Position = origin + size.Center;
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

				var locationInNode = touch.Location;
				var s = target.ContentSize;
                CCRect rect = target.BoundingBoxTransformedToWorld;

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
                CCPoint pt = touch.PreviousLocation + touch.Delta;
                target.Position = target.WorldToParentspace(pt);
			};

			listener1.OnTouchEnded = (touch, touchEvent) => 
			{
				var target = (CCSprite)touchEvent.CurrentTarget;
				CCLog.Log("sprite onTouchesEnded..");
				target.Opacity = 255;
				if (target == sprite2)
				{
                    sprite1.ZOrder = 100;
				}
				else if(target == sprite1)
				{
                    sprite1.ZOrder = 0;
				}
			};


            sprite1.AddEventListener(listener1);
            sprite2.AddEventListener(listener1.Copy());
            sprite3.AddEventListener(listener1.Copy());


			var removeAllTouchItem = new CCMenuItemFont("Remove All Touch Listeners", (sender) => {
				var senderItem = (CCMenuItemFont)sender;
				senderItem.Label.Text = "Only Next item could be clicked";

                RemoveEventListeners(CCEventListenerType.TOUCH_ONE_BY_ONE);

				var nextItem = new CCMenuItemFont("Next", (senderNext) => NextCallback(senderNext));
			

				CCMenuItemFont.FontSize = 16;
                nextItem.Position =  new CCPoint(visibleBounds.Origin.X + visibleBounds.Size.Width -100, 100);

				var menu2 = new CCMenu(nextItem);
				menu2.Position = CCPoint.Zero;
				menu2.AnchorPoint = CCPoint.AnchorLowerLeft;
				this.AddChild(menu2);
			});

			CCMenuItemFont.FontSize = 16;
            removeAllTouchItem.Position = new CCPoint(visibleBounds.Origin.X + visibleBounds.Size.Width -100, 80);

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

    public class TouchableSpriteTest2 : EventDispatcherTest
    {

        public override void OnEnter()
        {
            base.OnEnter();

            var visibleBounds = VisibleBoundsWorldspace;
            var origin = VisibleBoundsWorldspace.Origin;
            var size = VisibleBoundsWorldspace.Size;

            var sprite1 = new CCSprite("Images/CyanSquare.png");
            sprite1.Position = origin + size.Center + new CCPoint(-80, 80);
            AddChild(sprite1, 10);

            var follow = new CCFollow(sprite1, new CCRect(0, 0, 3000, 600));
            RunAction(follow);

            sprite1.PositionX = size.Width + 100;
            sprite1.PositionY = size.Height / 2;

            // Make sprite1 touchable
            var listener1 = new CCEventListenerTouchOneByOne();
            listener1.IsSwallowTouches = true;

            listener1.OnTouchBegan = (touch, touchEvent) =>
            {
                var layerOffset = Layer.PositionWorldspace;
                var target = (CCSprite)touchEvent.CurrentTarget;
                var locationInNode = touch.Location;

                CCRect rect = target.BoundingBoxTransformedToWorld;
                rect.Origin += layerOffset;

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
                CCPoint pt = touch.PreviousLocation + touch.Delta;
                target.Position = target.WorldToParentspace(pt);
            };

            listener1.OnTouchEnded = (touch, touchEvent) =>
            {
                var target = (CCSprite)touchEvent.CurrentTarget;
                CCLog.Log("sprite onTouchesEnded..");
                target.Opacity = 255;
            };


            sprite1.AddEventListener(listener1);

        }

        public override void OnExit()
        {
            base.OnExit();

        }

        public override string Title
        {
            get
            {
                return "Touchable Sprite Test 2";
            }
        }

        public override string Subtitle
        {
            get
            {
                return "Touch Event when using CCFollow";
            }
        }

    }

	public class FixedPriorityTest : EventDispatcherTest
	{

		public override void OnEnter()
		{
			base.OnEnter ();

			var origin = VisibleBoundsWorldspace.Origin;
			var size = VisibleBoundsWorldspace.Size;

			var sprite1 = new TouchableSprite (30);
			var texture = CCTextureCache.SharedTextureCache.AddImage("Images/CyanSquare.png");
			sprite1.Texture = texture;
            sprite1.Position = origin + size.Center + new CCPoint (-80, 80);
			AddChild(sprite1, 10);

			var sprite2 = new TouchableSprite (20);
			texture = CCTextureCache.SharedTextureCache.AddImage("Images/MagentaSquare.png");
			sprite2.Texture = texture;
            sprite2.Position = origin + size.Center;
			AddChild(sprite2, 20);

			var sprite3 = new TouchableSprite (10);
			texture = CCTextureCache.SharedTextureCache.AddImage("Images/YellowSquare.png");
			sprite3.Texture = texture;
            sprite3.Position = CCPoint.Zero; //new CCPoint (-120, 120) ;
            sprite3.AnchorPoint = CCPoint.AnchorMiddle;
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

            listener.OnTouchBegan = (CCTouch touch, CCEvent touchEvent) => 
			{

                var locationInNode = touch.Location;//     Layer.ScreenToWorldspace(touch.LocationOnScreen);
                var s = ContentSize;
                CCRect rect = BoundingBoxTransformedToWorld;

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
                    RemoveEventListener(Listener);
				}
			};

            if (FixedPriority != 0)
            {
                AddEventListener(listener, FixedPriority);
            }
            else
            {
                AddEventListener(listener);
            }

			Listener = listener;
		}

		public override void OnExit ()
		{
            RemoveEventListener(Listener);
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

			var origin = Layer.VisibleBoundsWorldspace.Origin;
			var size = Layer.VisibleBoundsWorldspace.Size;

			//MenuItemFont::setFontSize(20);

            var statusLabel = new CCLabel("No custom event 1 received!", "arial", 20, CCLabelFormat.SpriteFont);
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

            AddEventListener(listener, 1);
			var count = 0;
			var sendItem = new CCMenuItemFont("Send Custom Event 1", (sender) =>
				{
					++count;
					var userData = string.Format("{0}", count);
					DispatchEvent("game_custom_event1",userData);
			});

			sendItem.Position = origin + size.Center;

            var statusLabel2 = new CCLabel("No custom event 2 received!", "arial", 20, CCLabelFormat.SpriteFont);
			statusLabel2.Position = origin + new CCPoint(size.Width/2, size.Height-120);
			AddChild(statusLabel2);

			listener2 = new CCEventListenerCustom("game_custom_event2", (customEvent) =>
				{
					statusLabel2.Text = string.Format("Custom event 2 received, {0} times", customEvent.UserData);
				});

            AddEventListener(listener2, 1);

			var count2 = 0;
			var sendItem2 = new CCMenuItemFont("Send Custom Event 2", (sender) =>
				{
					var customEvent = new CCEventCustom("game_custom_event2");
					customEvent.UserData = ++count2;
                    DispatchEvent(customEvent);
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
            RemoveEventListener(listener);
            RemoveEventListener(listener2);
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

			var origin = Layer.VisibleBoundsWorldspace.Origin;
			var size = Layer.VisibleBoundsWorldspace.Size;

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

				var locationInNode = target.Layer.ScreenToWorldspace(touch.LocationOnScreen);
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

            sprite.AddEventListener(listener1);

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
            CCRect visibleBounds = Layer.VisibleBoundsWorldspace;

			base.OnEnter ();

			var item1 = new  CCMenuItemFont("Click Me 1", (sender) =>
				{
					var listener = new CCEventListenerTouchOneByOne();
					listener.OnTouchBegan = (touch, touchEvent) => 
					{
						Debug.Assert(false, "Should not come here!");
						return true;
					};

                    AddEventListener(listener, -1);
                    RemoveEventListener(listener);
			});

            item1.Position = visibleBounds.Center + new CCPoint(0, 80);

			var addNextButton = new Action( () =>
			{
				var next = new CCMenuItemFont("Please Click Me To Reset!", (sender) => 
				{
						RestartCallback(null);
				});
                next.Position = visibleBounds.Center + new CCPoint(0, -40);

				var menuNext = new CCMenu(next);
                menuNext.Position = visibleBounds.Origin;
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

                    AddEventListener(listener, -1);
                    RemoveEventListeners(CCEventListenerType.TOUCH_ONE_BY_ONE);

					addNextButton ();
			});

            item2.Position = visibleBounds.Center + new CCPoint(0, 40);

			var item3 = new CCMenuItemFont("Click Me 3", (sender) => 
				{
					var listener = new CCEventListenerTouchOneByOne ();
					listener.OnTouchBegan = (touch, touchEvent) => {
						Debug.Assert (false, "Should not come here!");
						return true;
					};

                    AddEventListener(listener, -1);
                    RemoveEventListeners(CCEventListenerType.TOUCH_ONE_BY_ONE);

					addNextButton();
			});

            item3.Position = visibleBounds.Center;

			var menu = new CCMenu(item1, item2, item3);
            menu.Position = visibleBounds.Origin;
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

		CCLabel label1, label2, label3, label4;
		CCEventListenerCustom event1, event2, event3, event4;

		public override void OnEnter ()
		{

			base.OnEnter ();

			var origin = Layer.VisibleBoundsWorldspace.Origin;
			var s = Layer.VisibleBoundsWorldspace.Size;

            label1 = new CCLabel("Update: 0", "arial", 20, CCLabelFormat.SpriteFont);
			label1.AnchorPoint = CCPoint.AnchorUpperLeft;
			label1.Position = new CCPoint(30,s.Height/2 + 60);
			AddChild(label1);

            label2 = new CCLabel("Visit: 0", "arial", 20, CCLabelFormat.SpriteFont);
			label2.AnchorPoint = CCPoint.AnchorUpperLeft;
			label2.Position = new CCPoint(30,s.Height/2 + 20);
			AddChild(label2);

            label3 = new CCLabel("Draw: 0", "arial", 20, CCLabelFormat.SpriteFont);
			label3.AnchorPoint = CCPoint.AnchorUpperLeft;
			label3.Position = new CCPoint(30,s.Height/2 - 20);
			AddChild(label3);

            label4 = new CCLabel("Projection: 0", "arial", 20, CCLabelFormat.SpriteFont);
			label4.AnchorPoint = CCPoint.AnchorUpperLeft;
			label4.Position = new CCPoint(30,s.Height/2 - 60);
			AddChild(label4);

//            event1 = AddCustomEventListener(CCGameView.EVENT_AFTER_UPDATE, OnEvent1);
//            event2 = AddCustomEventListener(CCWindow.EVENT_AFTER_VISIT, OnEvent2);
//            event3 = AddCustomEventListener(CCWindow.EVENT_AFTER_DRAW, (customEvent) =>
//				{
//					label3.Text = string.Format("Draw: {0}", count3++);
//				});
//            event4 = AddCustomEventListener(CCWindow.EVENT_PROJECTION_CHANGED, (customEvent) =>
//				{
//					label4.Text = string.Format("Projection: {0}", count4++);
//				});

			Schedule();

		}

		float time = 0;

		public override void Update (float dt)
		{
			base.Update (dt);

			time += dt;
			if(time > 0.5) {
				//Director.Projection = CCDirectorProjection.Projection2D;
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
//            RemoveEventListener (event1);
//            RemoveEventListener (event2);
//            RemoveEventListener (event3);
//            RemoveEventListener (event4);
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


		}

		public override void OnEnter()
		{
            base.OnEnter(); 

            int i;

            for (i = 0; i < SPRITE_COUNT; i++)
            {
                CCSprite sprite;

                if(i==4)
                {
                    sprite = new CCSprite("Images/CyanSquare.png") { Tag = TAG_SPRITE + i};
                    blueSprite = sprite;
                    //blueSprite.Scene = Scene;
                    blueSprite.ZOrder = -1;

                }
                else
                {
                    sprite = new CCSprite("Images/YellowSquare.png") { Tag = TAG_SPRITE + i};
                }

                // For right now since we do not draw by GlobalZOrder we will move a value to the
                // Local z-order so we can fake this.  It is a Hack for now for the foreground
                // to background movement.
                AddChild(sprite, 2);

            }

            CCRect visibleRect = Layer.VisibleBoundsWorldspace;

			var listener = new CCEventListenerTouchOneByOne();
			listener.IsSwallowTouches = true;

			listener.OnTouchBegan = (touch, touchEvent) =>
				{
					var target = (CCSprite)touchEvent.CurrentTarget;

					var locationInNode = target.Layer.ScreenToWorldspace(touch.LocationOnScreen);
					var s = target.ContentSize;
                    var rect = target.BoundingBoxTransformedToWorld;
		
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

			i = 0;
			foreach (var child in Children)
			{
				if (child.Tag >= TAG_SPRITE && child.Tag <= TAG_SPRITE_END)
				{
                    child.Position = new CCPoint(visibleRect.Origin.X + visibleRect.Size.Width / (SPRITE_COUNT - 1) * i, visibleRect.Center.Y);
					i++;
					child.AddEventListener(listener.Copy());
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
                var z = blueSprite.ZOrder;
				CCLog.Log("ZOrder {0} - New ZOrder {1}.  ", z, -z);
                blueSprite.ZOrder = -z;
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

            CCRect visibleBounds = VisibleBoundsWorldspace;
			var origin = Layer.VisibleBoundsWorldspace.Origin;
			var size = Layer.VisibleBoundsWorldspace.Size;

			var sprite1 = new TouchableSprite ();
			var texture = CCTextureCache.SharedTextureCache.AddImage("Images/CyanSquare.png");
			sprite1.Texture = texture;
			sprite1.Position = origin + new CCPoint (size.Width / 2, size.Height / 2) + new CCPoint (-80, 80);
			AddChild(sprite1, -10);

			var sprite2 = new TouchableSprite ();
			texture = CCTextureCache.SharedTextureCache.AddImage("Images/MagentaSquare.png");
			sprite2.Texture = texture;
			sprite2.Position = origin + new CCPoint (size.Width / 2, size.Height / 2);
			AddChild(sprite2, -20);

			var sprite3 = new TouchableSprite ();
			texture = CCTextureCache.SharedTextureCache.AddImage("Images/YellowSquare.png");
			sprite3.Texture = texture;
			sprite3.Position = CCPoint.Zero;
			sprite2.AddChild(sprite3, -1);

			CCMenuItemFont.FontSize = 20;
			CCMenuItemFont.FontName = "arial";

			var popup = new CCMenuItemFont("Popup", (sender) =>
				{

                    this.PauseListeners(true);

					var colorLayer = new CCLayerColor(new CCColor4B(0, 0, 255, 100));
					AddChild(colorLayer, 99999);

					var closeItem = new CCMenuItemFont("close", (closeSender) =>
						{
							colorLayer.RemoveFromParent();
                            this.ResumeListeners(true);
				});

                    closeItem.Position = visibleBounds.Center;

					var closeMenu = new CCMenu(closeItem);
					closeMenu.AnchorPoint = CCPoint.AnchorLowerLeft;
					closeMenu.Position = CCPoint.Zero;

					colorLayer.AddChild(closeMenu);
			});

			popup.AnchorPoint = CCPoint.AnchorMiddleRight;
            popup.Position = new CCPoint(visibleBounds.Origin.X + visibleBounds.Size.Width,
                visibleBounds.Origin.Y + visibleBounds.Size.Height / 2);

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

	public class RemoveAllTest : EventDispatcherTest
	{

		bool bugFixed;

		CCEventListenerCustom customlistener;

		CCMenuItemFont removeAllTouchItem;
		CCMenu menu;

		public RemoveAllTest ()
		{
            CCRect visibleBounds = Layer.VisibleBoundsWorldspace;

			CCMenuItemFont.FontSize = 16;

			removeAllTouchItem = new CCMenuItemFont("Remove all listeners", (sender) =>
				{
					var senderItem = (CCMenuItemFont) sender;

					senderItem.Label.Text = "Only 'Reset' item could be clicked";

					RemoveAllListeners();

					var nextItem = new CCMenuItemFont("Reset", (resetSender) => 
						{
							Debug.Assert(bugFixed, "This issue was not fixed");
							RestartCallback(null);
						}
					);

					CCMenuItemFont.FontSize = 16;
                    nextItem.Position = new CCPoint(visibleBounds.Origin.X + visibleBounds.Size.Width,
                        visibleBounds.Origin.Y + visibleBounds.Size.Height / 2) + new CCPoint(-100, -30);
					var menu2 = new CCMenu(nextItem);
					menu2.AnchorPoint = CCPoint.AnchorLowerLeft;
                    menu2.Position = visibleBounds.Center;
					AddChild(menu2);


					DispatchEvent(CCEvent.EVENT_COME_TO_BACKGROUND);
				});

			menu = new CCMenu(removeAllTouchItem);
			menu.AnchorPoint = CCPoint.AnchorLowerLeft;
			AddChild(menu);

		}

		public override void OnEnter()
		{
			base.OnEnter(); 
            CCRect visibleBounds = Layer.VisibleBoundsWorldspace;

			customlistener = AddCustomEventListener(CCEvent.EVENT_COME_TO_BACKGROUND, (customEvent) => 
				{

                    var label = new CCLabel("Yeah, this issue was fixed.", "", 20, CCLabelFormat.SpriteFont);
					label.AnchorPoint = CCPoint.AnchorMiddleLeft;
                    label.Position = new CCPoint(visibleBounds.Origin.X, visibleBounds.Origin.Y + visibleBounds.Size.Height / 2);

					AddChild(label);

					// After test, remove it.
					RemoveEventListener(customlistener);
					customlistener = null;

					bugFixed = true;
				});

            removeAllTouchItem.Position = new CCPoint (visibleBounds.Origin.X + visibleBounds.Size.Width - 100,
                visibleBounds.Origin.Y + visibleBounds.Size.Height / 2);
			menu.Position = CCPoint.Zero;

		}

		public override void OnExit()
		{
			if (customlistener != null)
				RemoveEventListener(customlistener);

			base.OnExit();
		}
		public override string Title
		{
			get
			{
				return  "Remove All Listeners";
			}
		}

		public override string Subtitle
		{
			get
			{
				return "Should see 'Yeah, this issue was fixed.'";
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

			var origin = Layer.VisibleBoundsWorldspace.Origin;
			var size = Layer.VisibleBoundsWorldspace.Size;

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

            AddEventListener(listener1, this);

			Schedule ();
		}

		public override void Update (float dt)
		{
			base.Update (dt);

			if (cyanTouch != null) 
			{
                MoveSpriteTowardPoint (Layer.ScreenToWorldspace(cyanTouch.LocationOnScreen), dt);
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
				if (!IsPointInTopHalfAreaOfScreen(touch.LocationOnScreen))
					return false;

				var target = (CCSprite)touchEvent.CurrentTarget;
				Debug.Assert (target.Tag == TAG_BLUE_SPRITE, "Yellow blocks shouldn't response event.");

				if (IsPointInNode(touch.LocationOnScreen, target))
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
				if (IsPointInTopHalfAreaOfScreen(touches[0].LocationOnScreen))
					return;

				var target = (CCSprite)touchEvent.CurrentTarget;
				Debug.Assert(target.Tag == TAG_BLUE_SPRITE2, "Yellow blocks shouldn't response event.");

				if (IsPointInNode(touches[0].LocationOnScreen, target))
				{
					target.Opacity = 180;
				}
				// Stop propagation, so yellow blocks will not be able to receive event.
				touchEvent.StopPropogation();
			};

			touchAllAtOnceListener.OnTouchesEnded = (touches, touchEvent) => 
			{
				// Skip if don't touch top half screen.
				if (IsPointInTopHalfAreaOfScreen(touches[0].LocationOnScreen))
					return;

				var target = (CCSprite)touchEvent.CurrentTarget;
				Debug.Assert(target.Tag == TAG_BLUE_SPRITE2, "Yellow blocks shouldn't response event.");

				if (IsPointInNode(touches[0].LocationOnScreen, target))
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

				sprite.AddEventListener(touchOneByOneListener.Copy());
				sprite.AddEventListener(keyboardEventListener.Copy());

				sprite2.AddEventListener(touchAllAtOnceListener.Copy());
				sprite2.AddEventListener(keyboardEventListener.Copy());


				var visibleBounds = Layer.VisibleBoundsWorldspace;
                sprite.Position = new CCPoint( visibleBounds.Origin.X + visibleBounds.Size.Width / (SPRITE_COUNT - 1) * i, visibleBounds.Center.Y + sprite2.ContentSize.Height/2 +10);
                sprite2.Position = new CCPoint( visibleBounds.Origin.X + visibleBounds.Size.Width / (SPRITE_COUNT - 1) * i, visibleBounds.Center.Y - sprite2.ContentSize.Height/2-10);
			}

		}

		bool IsPointInNode(CCPoint pt, CCNode node)
		{
			var locationInNode = node.Layer.ScreenToWorldspace(pt);
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
			var winSize = Layer.VisibleBoundsWorldspace.Size;

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
