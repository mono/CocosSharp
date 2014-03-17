using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.TestBed.Framework;
using FarseerPhysics.TestBed.Tests;
using Microsoft.Xna.Framework;
using CocosSharp;

namespace tests.classes.tests.Box2DTestBet
{
    public class Box2DView : CCLayer
    {
        private TestEntry m_entry;
        private Test m_test;
        private int m_entryID;

        private GameSettings settings = new GameSettings();

        public bool initWithEntryID(int entryId)
        {

			//TouchEnabled = true;

            Schedule(tick);

            m_entry = TestEntries.TestList[entryId];
            m_test = m_entry.CreateFcn();
            m_test.Initialize();

			// Register Touch Event
			var touchListener = new CCEventListenerTouchOneByOne();
			touchListener.IsSwallowTouches = true;

			touchListener.OnTouchBegan = onTouchBegan;
			touchListener.OnTouchMoved = onTouchMoved;
			touchListener.OnTouchEnded = onTouchEnded;

			EventDispatcher.AddEventListener(touchListener, this);


            return true;
        }

        public string title()
        {
            return m_entry.Name;
        }

        public void tick(float dt)
        {
            m_test.TextLine = 30;
            m_test.Update(settings, CCApplication.SharedApplication.GameTime);
        }

        protected override void Draw()
        {
            base.Draw();

            //CCDrawManager.PushMatrix();

            m_test.DebugView.RenderDebugData();

            //CCDrawManager.PopMatrix();
        }

		bool onTouchBegan(CCTouch touch, CCEvent touchEvent)
        {
            CCPoint touchLocation = touch.Location;

            CCPoint nodePosition = ConvertToNodeSpace(touchLocation);
            //    NSLog(@"pos: %f,%f -> %f,%f", touchLocation.x, touchLocation.y, nodePosition.x, nodePosition.y);

            m_test.MouseDown(new Vector2(nodePosition.X, nodePosition.Y));

            return true;
        }

		void onTouchMoved(CCTouch touch, CCEvent touchEvent)
        {
            CCPoint touchLocation = touch.Location;
            CCPoint nodePosition = ConvertToNodeSpace(touchLocation);

            m_test.MouseMove(new Vector2(nodePosition.X, nodePosition.Y));
        }

		void onTouchEnded(CCTouch touch, CCEvent touchEvent)
        {
            CCPoint touchLocation = touch.Location;
            CCPoint nodePosition = ConvertToNodeSpace(touchLocation);

            m_test.MouseUp();
        }

        //virtual void accelerometer(UIAccelerometer* accelerometer, CCAcceleration* acceleration);

        public static Box2DView viewWithEntryID(int entryId)
        {
            var pView = new Box2DView();
            pView.initWithEntryID(entryId);
            return pView;
        }
    }
}
