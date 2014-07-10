using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class SchedulerTestScene : TestScene
    {
        public override void runThisTest()
        {
            CCLayer pLayer = nextSchedulerTest();
            AddChild(pLayer);

            Scene.Director.ReplaceScene(this);
        }

		static int MAX_TESTS = 13;
        static int sceneIdx = -1;


        public static CCLayer createSchedulerTest(int nIndex)
        {
            CCLayer pLayer = null;

			switch (nIndex) {
			case 0:
				pLayer = new SchedulerAutoremove ();
				break;
			case 1:
				pLayer = new SchedulerPauseResume ();
				break;
			case 2:
				pLayer = new SchedulerPauseResumeAll ();
				break;
			case 3:
				pLayer = new SchedulerPauseResumeUser ();
				break;
			case 4:
				pLayer = new SchedulerUnscheduleAll ();
				break;
			case 5:
				pLayer = new SchedulerUnscheduleAllHard ();
				break;
			case 6:
				pLayer = new SchedulerUnscheduleAllUserLevel ();
				break;
			case 7:
				pLayer = new SchedulerSchedulesAndRemove ();
				break;
			case 8:
				pLayer = new SchedulerUpdate ();
				break;
			case 9:
				pLayer = new SchedulerUpdateAndCustom ();
				break;
			case 10:
				pLayer = new SchedulerUpdateFromCustom ();
				break;
			case 11:
				pLayer = new RescheduleSelector ();
				break;
			case 12:
				pLayer = new SchedulerDelayAndRepeat ();
				break;
			default:
				break;
			}

            return pLayer;
        }
        protected override void NextTestCase()
        {
            nextSchedulerTest();
        }
        protected override void PreviousTestCase()
        {
            backSchedulerTest();
        }
        protected override void RestTestCase()
        {
            restartSchedulerTest();
        }

        public static CCLayer nextSchedulerTest()
        {

            sceneIdx++;
            sceneIdx = sceneIdx % MAX_TESTS;

            return createSchedulerTest(sceneIdx);
        }

        public static CCLayer backSchedulerTest()
        {
            sceneIdx--;
            if (sceneIdx < 0)
                sceneIdx += MAX_TESTS;

            return createSchedulerTest(sceneIdx);
        }

        public static CCLayer restartSchedulerTest()
        {
            return createSchedulerTest(sceneIdx);
        }

    }
}
