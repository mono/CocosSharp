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
				pLayer = (CCLayer)new MouseEventTest();
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


//	public class EventDispatcherTest : EventDispatcherTestScene
//	{
//		//static int sceneIdx = -1;
//
//		public EventDispatcherTest()
//		{
//			CCSize s = CCDirector.SharedDirector.WinSize;
//
//			CCLabelTtf label = new CCLabelTtf(title(), "Arial", 26);
//			AddChild(label, TestScene.TITLE_LEVEL);
//			label.Position = new CCPoint(s.Width / 2, s.Height - 50);
//
//			var sub = subtitle();
//			if (!string.IsNullOrEmpty(sub))
//			{
//				CCLabelTtf l = new CCLabelTtf(sub, "Arial", 16);
//				AddChild(l, TestScene.TITLE_LEVEL);
//				l.Position = new CCPoint(s.Width / 2, s.Height - 80);
//			}
//
//			CCMenuItemImage item1 = new CCMenuItemImage(TestResource.s_pPathB1, TestResource.s_pPathB2,  BackAction);
//			CCMenuItemImage item2 = new CCMenuItemImage(TestResource.s_pPathR1, TestResource.s_pPathR2,  RestartAction);
//			CCMenuItemImage item3 = new CCMenuItemImage(TestResource.s_pPathF1, TestResource.s_pPathF2,  NextAction);
//
//			CCMenu menu = new CCMenu(item1, item2, item3);
//			menu.Position = new CCPoint();
//			item1.Position = new CCPoint(s.Width / 2 - 100, 30);
//			item2.Position = new CCPoint(s.Width / 2, 30);
//			item3.Position = new CCPoint(s.Width / 2 + 100, 30);
//
//			AddChild (menu, TestScene.MENU_LEVEL);
//		}
//
//
//		public static CCLayer CreateTestCaseLayer(int index)
//		{
//			switch (index)
//			{
//			case 0:
//				MouseTest pRet = new MouseTest();
//				return pRet;
//			default:
//				return null;
//			}
//		}
//
////		public static CCLayer NextEventDispatcherTestCase()
////		{
////			sceneIdx++;
////			sceneIdx = sceneIdx % MAX_LAYER;
////
////			return CreateTestCaseLayer(sceneIdx);
////		}
////
////		public static CCLayer BackEventDispatcherTestCase()
////		{
////			sceneIdx--;
////			if (sceneIdx < 0)
////				sceneIdx += MAX_LAYER;
////
////			return CreateTestCaseLayer(sceneIdx);
////		}
////
////		public static CCLayer RestartEventDispatcherTestCase()
////		{
////			return CreateTestCaseLayer(sceneIdx);
////		}
////
////		public void RestartCallback(object pSender)
////		{
////			CCScene s = new EventDispatcherTestScene();
////			s.AddChild(RestartEventDispatcherTestCase());
////			CCDirector.SharedDirector.ReplaceScene(s);
////		}
////
////		public void NextCallback(object pSender)
////		{
////			CCScene s = new EventDispatcherTestScene();
////			s.AddChild(NextEventDispatcherTestCase());
////			CCDirector.SharedDirector.ReplaceScene(s);
////		}
////
////		public void BackCallback(object pSender)
////		{
////			CCScene s = new EventDispatcherTestScene();
////			s.AddChild(BackEventDispatcherTestCase());
////			CCDirector.SharedDirector.ReplaceScene(s);
////		}
//
//		public virtual string title()
//		{
//			return "No title";
//		}
//
//		public virtual string subtitle()
//		{
//			return "";
//		}
//	}

	public class MouseEventTest : CCLayer
	{

		public MouseEventTest() : base()
		{

			//				TouchEnabled = true;
			CCSize s = CCDirector.SharedDirector.WinSize;


			//CCMenuItem item = new CCMenuItemFont("Rotate Device", RotateDevice);
			//CCMenu menu = new CCMenu(item);
			//menu.Position = new CCPoint(s.Width / 2, s.Height / 2);
			//AddChild(menu);

		}

		public override string title()
		{
			return "Testing Mouse Events";
		}

		public override string subtitle()
		{
			return "Mouse Move, Buttons and Scroll";
		}

	}

}
