using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class ActionManagerTest : TestNavigationLayer
    {

        private CCDirector director;

        protected CCTextureAtlas m_atlas;

        protected string m_strTitle;

        public ActionManagerTest() { }

        public override string Title
        {
            get
            {
                return base.Title;
            }
        }

        protected override void AddedToScene()
        {
            base.AddedToScene();

            // We no longer have a static director so when we remove
            // this in the Crash test 1 we do not have the director anymore
            // we will save this off so that we can issue commands to the
            // directory.
            director = Director;
        }

        public override void RestartCallback(object sender)
        {
            base.RestartCallback(sender);

            CCScene s = new ActionManagerTestScene();
            s.AddChild(restartActionManagerAction());

            director.ReplaceScene(s);

        }

        public override void NextCallback(object sender)
        {
            base.NextCallback(sender);

            CCScene s = new ActionManagerTestScene();

            s.AddChild(nextActionManagerAction());

            director.ReplaceScene(s);
        }

        public override void BackCallback(object sender)
        {
            base.BackCallback(sender);

            CCScene s = new ActionManagerTestScene();
            s.AddChild(backActionManagerAction());
            director.ReplaceScene(s);
        }

        public static int sceneIdx = -1;
        public static int MAX_LAYER = 5;

        public static CCLayer backActionManagerAction()
        {
            sceneIdx--;
            int total = MAX_LAYER;
            if (sceneIdx < 0)
                sceneIdx += total;

            CCLayer pLayer = createActionManagerLayer(sceneIdx);

            return pLayer;
        }

        public static CCLayer createActionManagerLayer(int nIndex)
        {
            switch (nIndex)
            {
                case 0: return new CrashTest();
                case 1: return new LogicTest();
                case 2: return new PauseTest();
                case 3: return new RemoveTest();
                case 4: return new ResumeTest();
            }

            return null;
        }

        public static CCLayer nextActionManagerAction()
        {
            sceneIdx++;
            sceneIdx = sceneIdx % MAX_LAYER;

            CCLayer pLayer = createActionManagerLayer(sceneIdx);

            return pLayer;
        }

        public static CCLayer restartActionManagerAction()
        {
            CCLayer pLayer = createActionManagerLayer(sceneIdx);

            return pLayer;
        }
    }
}
