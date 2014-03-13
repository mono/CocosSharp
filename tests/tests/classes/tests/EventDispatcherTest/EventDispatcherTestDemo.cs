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
		EVENT_MOUSE = 0,
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
			CCLayer pLayer = null;

			switch (index)
			{
			case (int) EventDispatchTests.EVENT_MOUSE:
				pLayer = new MouseEventTest();
				break;
			default:
				break;
			}

			return pLayer;
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

			var pLayer = CreateLayer(sceneIndex);

			return pLayer;
		}

		public static CCLayer BackAction()
		{
			--sceneIndex;
			if (sceneIndex < 0)
				sceneIndex += (int)EventDispatchTests.TEST_CASE_COUNT;

			var pLayer = CreateLayer(sceneIndex);

			return pLayer;
		}

		public static CCLayer RestartAction()
		{
			var pLayer = CreateLayer(sceneIndex);

			return pLayer;
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
		//static int sceneIdx = -1;

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
			EventDispatcherTestScene.RestartAction ();
		}

		public void NextCallback(object pSender)
		{
			EventDispatcherTestScene.NextAction ();
		}

		public void BackCallback(object pSender)
		{
			EventDispatcherTestScene.BackAction ();
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

}
