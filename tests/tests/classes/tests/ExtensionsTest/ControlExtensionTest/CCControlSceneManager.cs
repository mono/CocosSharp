using System;
using CocosSharp;

namespace tests.Extensions
{
	public class CCControlSceneManager 
	{
        public const int kCCControlTestMax = 9;
        
        public const int kCCControlSliderTest = 0;
		//public const int kCCControlColourPickerTest = 2;
		public const int kCCControlSwitchTest = 1;
		public const int kCCControlButtonTest_HelloVariableSize = 2;
		public const int kCCControlButtonTest_Event = 3;
		public const int kCCControlButtonTest_Styling = 4;
        public const int kCCControlButtonTest_Inset = 5;
	    public const int kCCControlButtonTest_Potentiometer = 6;
        public const int kCCControlButtonTest_ColourPicker = 7;
        public const int kCCControlButtonTest_Stepper = 8;


		public CCControlSceneManager()
		{
			m_nCurrentControlSceneId = kCCControlSliderTest;
		}


		private static string[] s_testArray = {
		    "ControlButtonTest_Slider",
		    "ControlButtonTest_Switch",
		    "ControlButtonTest_HelloVariableSize",
		    "ControlButtonTest_Event",
		    "ControlButtonTest_Styling",
            "ControlButtonTest_Inset",
            "ControlButtonTest_Potentiometer",
            "ControlButtonTest_ColourPicker",
            "ControlButtonTest_Stepper"
		};

		private static CCControlSceneManager sharedInstance = null;


		/** Returns the singleton of the control scene manager. */
		public static CCControlSceneManager sharedControlSceneManager()
		{
			if (sharedInstance == null)
			{
				sharedInstance = new CCControlSceneManager();
			}
			return sharedInstance;
		}



		/** Returns the next control scene. */
		public CCScene nextControlScene()
		{
			m_nCurrentControlSceneId = (m_nCurrentControlSceneId + 1) % kCCControlTestMax;

			return currentControlScene();
		}

		/** Returns the previous control scene. */
		public CCScene previousControlScene()
		{
			m_nCurrentControlSceneId = m_nCurrentControlSceneId - 1;
			if (m_nCurrentControlSceneId < 0)
			{
				m_nCurrentControlSceneId = kCCControlTestMax - 1;
			}

			return currentControlScene();
		}



		/** Returns the current control scene. */
		public CCScene currentControlScene()
		{
			switch (m_nCurrentControlSceneId)
			{
				case kCCControlSliderTest:
					return CCControlSliderTest.sceneWithTitle(s_testArray[m_nCurrentControlSceneId]);
			//case kCCControlColourPickerTest:
			//    return CCControlColourPickerTest.sceneWithTitle(s_testArray[m_nCurrentControlSceneId]);
				case kCCControlSwitchTest:
					return CCControlSwitchTest.sceneWithTitle(s_testArray[m_nCurrentControlSceneId]);
				case kCCControlButtonTest_HelloVariableSize:
					return CCControlButtonTest_HelloVariableSize.sceneWithTitle(s_testArray[m_nCurrentControlSceneId]);
				case kCCControlButtonTest_Event:
					return CCControlButtonTest_Event.sceneWithTitle(s_testArray[m_nCurrentControlSceneId]);
				case kCCControlButtonTest_Styling:
					return CCControlButtonTest_Styling.sceneWithTitle(s_testArray[m_nCurrentControlSceneId]);
                case kCCControlButtonTest_Inset:
                    return CCControlButtonTest_Inset.sceneWithTitle(s_testArray[m_nCurrentControlSceneId]);
                case kCCControlButtonTest_Potentiometer:
                    return CCControlPotentiometerTest.sceneWithTitle(s_testArray[m_nCurrentControlSceneId]);
                case kCCControlButtonTest_ColourPicker:
                    return CCControlColourPickerTest.sceneWithTitle(s_testArray[m_nCurrentControlSceneId]);
                case kCCControlButtonTest_Stepper:
                    return CCControlStepperTest.sceneWithTitle(s_testArray[m_nCurrentControlSceneId]);
            }
			return null;
		}

		/** Control scene id. */
		protected int m_nCurrentControlSceneId;
		public virtual int getCurrentControlSceneId() { return m_nCurrentControlSceneId; }
		public virtual void setCurrentControlSceneId(int var){ m_nCurrentControlSceneId = var; }

	}
}
