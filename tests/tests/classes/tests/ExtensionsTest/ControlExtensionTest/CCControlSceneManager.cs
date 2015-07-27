using System;
using System.Collections.Generic;
using System.Linq;
using CocosSharp;

namespace tests.Extensions
{
	public class CCControlSceneManager 
	{
        public int CCControlTestMax = 0;
        /** Control scene id. */
        protected int CurrentControlSceneId { get; set; }

        public CCControlSceneManager()
		{
            CCControlTestMax = testCases.Count;
			CurrentControlSceneId = 0;
		}

        public static Dictionary<string, Func<string, CCScene>> testCases = new Dictionary<string, Func<string, CCScene>>
        {
                {"ControlButtonTest_Slider", (title) => CCControlSliderTest.SceneWithTitle(title)},
                {"ControlButtonTest_Switch", (title) => CCControlSwitchTest.SceneWithTitle(title)},
                {"ControlButtonTest_HelloVariableSize", (title) => CCControlButtonTest_HelloVariableSize.SceneWithTitle(title)},
                {"ControlButtonTest_Event", (title) => CCControlButtonTest_Event.SceneWithTitle(title)},
                {"ControlButtonTest_Styling", (title) => CCControlButtonTest_Styling.SceneWithTitle(title)},
                {"ControlButtonTest_Inset", (title) => CCControlButtonTest_Inset.SceneWithTitle(title)},
                {"ControlButtonTest_Potentiometer", (title) => CCControlPotentiometerTest.SceneWithTitle(title)},
                {"ControlButtonTest_ColourPicker", (title) => CCControlColourPickerTest.SceneWithTitle(title)},
                {"ControlButtonTest_Stepper", (title) => CCControlStepperTest.SceneWithTitle(title)},

        };

		private static CCControlSceneManager sharedInstance = null;


		/** Returns the singleton of the control scene manager. */
		public static CCControlSceneManager SharedControlSceneManager
		{
            get
            {
                if (sharedInstance == null)
                {
                    sharedInstance = new CCControlSceneManager();
                }
                return sharedInstance;
            }
		}



		/** Returns the next control scene. */
		public CCScene NextControlScene
		{
            get
            {
                CurrentControlSceneId = (CurrentControlSceneId + 1) % CCControlTestMax;

                return CurrentControlScene;
            }
		}

		/** Returns the previous control scene. */
		public CCScene PreviousControlScene
		{
            get
            {
                CurrentControlSceneId = CurrentControlSceneId - 1;
                if (CurrentControlSceneId < 0)
                {
                    CurrentControlSceneId = CCControlTestMax - 1;
                }

                return CurrentControlScene;
            }
		}



		/** Returns the current control scene. */
		public CCScene CurrentControlScene
		{
            get
            {
                var title = testCases.Keys.ElementAt(CurrentControlSceneId);
                return testCases[title](title);
            }
		}

	}
}
