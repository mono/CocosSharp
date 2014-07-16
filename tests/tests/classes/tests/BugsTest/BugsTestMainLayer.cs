using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class BugsTestMainLayer : CCLayer
    {
        public override void OnEnter()
        {
            base.OnEnter();

            CCSize s = Layer.VisibleBoundsWorldspace.Size;
            m_pItmeMenu = new CCMenu(null);

            for (int i = 0; i < BugsTestScene.MAX_COUNT; ++i)
            {
                CCMenuItemFont pItem = new CCMenuItemFont(BugsTestScene.testsName[i], menuCallback);
                pItem.Position = new CCPoint(s.Width / 2, s.Height - (i + 1) * BugsTestScene.LINE_SPACE);
                m_pItmeMenu.AddChild(pItem, BugsTestScene.kItemTagBasic + i);
            }

            m_pItmeMenu.Position = BugsTestScene.s_tCurPos;
            AddChild(m_pItmeMenu);
			// Register Touch Event
			var touchListener = new CCEventListenerTouchAllAtOnce();

			touchListener.OnTouchesBegan = onTouchesBegan;
			touchListener.OnTouchesMoved = onTouchesMoved;

            AddEventListener(touchListener);
        }

        public void menuCallback(object pSender)
        {
            CCMenuItemFont pItem = (CCMenuItemFont)pSender;
            int nIndex = pItem.ZOrder - BugsTestScene.kItemTagBasic;

            CCScene pScene = new CCScene(Scene);
            CCLayer pLayer = null;

            switch (nIndex)
            {
                case 0:
                    pLayer = new Bug350Layer();
                    break;
                case 1:
                    pLayer = new Bug422Layer();
                    break;
                case 2:
                    pLayer = new Bug458Layer();
                    break;
                case 3:
                    pLayer = new Bug624Layer();
                    break;
                case 4:
                    pLayer = new Bug886Layer();
                    break;
                case 5:
                    pLayer = new Bug899Layer();
                    break;
                case 6:
                    pLayer = new Bug914Layer();
                    break;
                case 7:
                    pLayer = new Bug1159Layer();
                    break;
                case 8:
                    pLayer = new Bug1174Layer();
                    break;
                default:
                    break;
            }
            pScene.AddChild(pLayer);
            Scene.Director.ReplaceScene(pScene);
        }

		void onTouchesBegan(List<CCTouch> touches, CCEvent touchEvent)
        {
			foreach (var it in touches)
            {
                CCTouch touch = it;
                var m_tBeginPos = touch.LocationOnScreen;
            }

        }

		void onTouchesMoved(List<CCTouch> touches, CCEvent touchEvent)
        {
			foreach (var it in touches)
            {
                CCTouch touch = it;

                var touchLocation = touch.LocationOnScreen;
                float nMoveY = touchLocation.Y - m_tBeginPos.Y;

                CCPoint curPos = m_pItmeMenu.Position;
                CCPoint nextPos = new CCPoint(curPos.X, curPos.Y + nMoveY);
                CCSize winSize = Layer.VisibleBoundsWorldspace.Size;
                if (nextPos.Y < 0.0f)
                {
                    m_pItmeMenu.Position = new CCPoint(0, 0);
                    return;
                }

                if (nextPos.Y > ((BugsTestScene.MAX_COUNT + 1) * BugsTestScene.LINE_SPACE - winSize.Height))
                {
                    m_pItmeMenu.Position = new CCPoint(0, ((BugsTestScene.MAX_COUNT + 1) * BugsTestScene.LINE_SPACE - winSize.Height));
                    return;
                }

                m_pItmeMenu.Position = nextPos;
                m_tBeginPos = touchLocation;
                BugsTestScene.s_tCurPos = nextPos;
            }

        }

        protected CCPoint m_tBeginPos;
        protected CCMenu m_pItmeMenu;
    }
}
