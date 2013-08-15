using Box2D.Common;
using Cocos2D;

namespace Box2D.TestBed
{
    public class Box2DView : CCLayer
    {
        private TestEntry m_entry;
        private Test m_test;
        private int m_entryID;

        private Settings settings = new Settings();

        public bool initWithEntryID(int entryId)
        {

            TouchEnabled = true;

            Schedule(tick);

            m_entry = TestEntries.TestList[entryId];
            m_test = m_entry.CreateFcn();

            return true;
        }

        public string title()
        {
            return m_entry.Name;
        }

        public void tick(float dt)
        {
            m_test.Step(settings);
        }

        public override void Draw()
        {
            base.Draw();

            m_test.Draw(settings);

            //CCDrawManager.PushMatrix();

            //m_test.m_debugDraw.RenderDebugData();

            //CCDrawManager.PopMatrix();
        }

        public override void RegisterWithTouchDispatcher()
        {
            CCDirector pDirector = CCDirector.SharedDirector;
            pDirector.TouchDispatcher.AddTargetedDelegate(this, -10, true);
        }

        public override bool TouchBegan(CCTouch touch)
        {
            CCPoint touchLocation = touch.Location;

            CCPoint nodePosition = ConvertToNodeSpace(touchLocation);
            //    NSLog(@"pos: %f,%f -> %f,%f", touchLocation.x, touchLocation.y, nodePosition.x, nodePosition.y);

            m_test.MouseDown(new b2Vec2(nodePosition.X, nodePosition.Y));

            return true;
        }

        public override void TouchMoved(CCTouch touch)
        {
            CCPoint touchLocation = touch.Location;
            CCPoint nodePosition = ConvertToNodeSpace(touchLocation);

            m_test.MouseMove(new b2Vec2(nodePosition.X, nodePosition.Y));
        }

        public override void TouchEnded(CCTouch touch)
        {
            CCPoint touchLocation = touch.Location;
            CCPoint nodePosition = ConvertToNodeSpace(touchLocation);

            m_test.MouseUp(new b2Vec2(nodePosition.X, nodePosition.Y));
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
