using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna;
using Microsoft.Xna.Framework.Input;
using System.Reflection;

namespace CocosSharp
{
    public class CCAccelerometer
    {
#if !WINDOWS && !PSM && !XBOX && !OUYA && !XBOX360 &&!NETFX_CORE && !MACOS && !WINDOWSGL
        // the accelerometer sensor on the device
        private static Microsoft.Devices.Sensors.Accelerometer accelerometer = null;
#endif

        private const float TG3_GRAVITY_EARTH = 9.80665f;
        private ICCAccelerometerDelegate m_pAccelDelegate;
        private readonly CCAcceleration m_obAccelerationValue = new CCAcceleration();

        private bool m_bActive;
        private bool m_bEmulation;

        static CCAccelerometer()
        {
#if !WINDOWS && !PSM && !XBOX && !OUYA && !XBOX360 &&!NETFX_CORE && !MACOS && !WINDOWSGL
            try
            {
                accelerometer = new Microsoft.Devices.Sensors.Accelerometer();
            }
            catch (Exception ex)
            {
                CCLog.Log(ex.ToString());
                CCLog.Log("No accelerometer on platform. CCAccelerometer will default to emulation code.");
            }
#endif
        }

        private void ResetAccelerometer()
        {
            m_obAccelerationValue.X = 0;
            m_obAccelerationValue.Y = 0;
            m_obAccelerationValue.Z = 0;
        }

        public void SetDelegate(ICCAccelerometerDelegate pDelegate)
        {
            m_pAccelDelegate = pDelegate;

            if (pDelegate != null && !m_bActive)
            {
#if !WINDOWS && !PSM && !OUYA && !XBOX360 &&!NETFX_CORE && !MACOS && !WINDOWSGL
                    try
                {
                    if (Microsoft.Devices.Sensors.Accelerometer.IsSupported)
                    {
                        accelerometer.CurrentValueChanged += accelerometer_CurrentValueChanged;
                        accelerometer.Start();
                        m_bActive = true;
                    }
                    else
                    {
                        m_bActive = false;
                    }
                }
                catch (Microsoft.Devices.Sensors.AccelerometerFailedException)
                {
                    m_bActive = false;
                }
#endif
                if (!m_bActive)
                {
                    m_bActive = true;
                    m_bEmulation = true;
                }
                else
                {
                    m_bEmulation = false;
                }
            }
            else
            {
                if (m_bActive && !m_bEmulation)
                {
#if !WINDOWS && !PSM && !OUYA && !XBOX360 &&!NETFX_CORE && !MACOS && !WINDOWSGL
                    //if (accelerometer != null)
                    //{
                    //    accelerometer.CurrentValueChanged -= accelerometer_CurrentValueChanged;
                    //    accelerometer.Stop();
                    //}
#endif
                }
                
                ResetAccelerometer();

                m_bActive = false;
                m_bEmulation = false;
            }
        }


#if !WINDOWS && !PSM && !OUYA && !XBOX360 &&!NETFX_CORE && !MACOS && !WINDOWSGL
        private void accelerometer_CurrentValueChanged(object sender, Microsoft.Devices.Sensors.SensorReadingEventArgs<Microsoft.Devices.Sensors.AccelerometerReading> e)
        {

            // We have to use reflection to get the Vector3 value out of Acceleration
            // What happens is that the Sensor used XNA Vector3 and what we have done is replaced
            // the XNA with MonoGame which our Sensor does not compile against.
            //
            // Result is this ugly hack.
            object val = e.SensorReading.GetType()
             .GetProperty("Acceleration",
                          BindingFlags.FlattenHierarchy |
                          BindingFlags.Instance |
                          BindingFlags.Public)
             .GetValue(e.SensorReading, null);

            if (val == null)
                return;

            // store the accelerometer value in our acceleration object to be updated.
            UpdateAccelerationValue(val.ToString());
  
            m_obAccelerationValue.TimeStamp = e.SensorReading.Timestamp.Ticks;
        }

        private void UpdateAccelerationValue(string acceleration)
        {
            string[] temp = acceleration.Substring(1, acceleration.Length - 2).Split(':');

            // The format of the string is {X: 0000 Y: 0000 Z: 0000}
            // Here we need to parse differently so that we can get a constant value back
            //  Cocos2D-XNA mapps the Sensor reading of the X value to be our Y value
            //  and the Y value to our X value.  Also the values need to be negated so that 
            //  it maps correctly.
			#if ANDROID
			m_obAccelerationValue.X = -float.Parse(temp[1].Substring(0, temp[1].Length - 1));
			m_obAccelerationValue.Y = float.Parse(temp[2].Substring(0, temp[2].Length - 1));
			m_obAccelerationValue.Z = float.Parse(temp[3]);
			#else
            m_obAccelerationValue.Y = -float.Parse(temp[1].Substring(0, temp[1].Length - 1));
            m_obAccelerationValue.X = -float.Parse(temp[2].Substring(0, temp[2].Length - 1));
            m_obAccelerationValue.Z = float.Parse(temp[3]);
			#endif

        }

        private static Vector3 ParseVector3(string acceleration)
        {

            string[] temp = acceleration.Substring(1, acceleration.Length - 2).Split(':');
            float x = float.Parse(temp[1].Substring(0, temp[1].Length - 1));
            float y = float.Parse(temp[2].Substring(0, temp[2].Length - 1));
            float z = float.Parse(temp[3]);
            Vector3 rValue = new Vector3(x, y, z);
            return rValue;
        }

#endif

		public void Update()
        {
            if (m_pAccelDelegate != null)
            {
                if (m_bEmulation)
                {
                    // if we're in the emulator, we'll generate a fake acceleration value using the arrow keys
                    // press the pause/break key to toggle keyboard input for the emulator
                    KeyboardState keyboardState = Keyboard.GetState();

                    var stateValue = new Vector3();

                    stateValue.Z = -1;

                    if (keyboardState.IsKeyDown(Keys.Left))
                        stateValue.X = -.1f;
                    if (keyboardState.IsKeyDown(Keys.Right))
                        stateValue.X = .1f;
                    if (keyboardState.IsKeyDown(Keys.Up))
                        stateValue.Y = -.1f;
                    if (keyboardState.IsKeyDown(Keys.Down))
                        stateValue.Y = .1f;

                    stateValue.Normalize();

                    m_obAccelerationValue.X = stateValue.X;
                    m_obAccelerationValue.Y = stateValue.Y;
                    m_obAccelerationValue.Z = stateValue.Z;
                    m_obAccelerationValue.TimeStamp = DateTime.Now.Ticks;
                }

                m_pAccelDelegate.DidAccelerate(m_obAccelerationValue);
            }
        }
    }
}
