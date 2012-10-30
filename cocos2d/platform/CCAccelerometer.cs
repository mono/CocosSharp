using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace cocos2d
{
    public class CCAccelerometer
    {
#if !WINDOWS
        // the accelerometer sensor on the device
        private static Microsoft.Devices.Sensors.Accelerometer accelerometer = 
            new Microsoft.Devices.Sensors.Accelerometer();
#endif

        private const float TG3_GRAVITY_EARTH = 9.80665f;
        private ICCAccelerometerDelegate m_pAccelDelegate;
        private readonly CCAcceleration m_obAccelerationValue = new CCAcceleration();

        private bool m_bActive;
        private bool m_bEmulation;

        private void ResetAccelerometer()
        {
            m_obAccelerationValue.x = 0;
            m_obAccelerationValue.y = 0;
            m_obAccelerationValue.z = 0;
        }

        public void SetDelegate(ICCAccelerometerDelegate pDelegate)
        {
            m_pAccelDelegate = pDelegate;

            if (pDelegate != null && !m_bActive)
            {
#if !WINDOWS
                if (Microsoft.Devices.Sensors.Accelerometer.IsSupported)            
                {
                    try
                    {
                        accelerometer.CurrentValueChanged += accelerometer_CurrentValueChanged;
                        accelerometer.Start();
                        m_bActive = true;
                    }
                    catch (Microsoft.Devices.Sensors.AccelerometerFailedException)
                    {
                        m_bActive = false;
                    }
                }
                else
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
#if !WINDOWS
                    accelerometer.CurrentValueChanged -= accelerometer_CurrentValueChanged;
                    accelerometer.Stop();
#endif
                }
                
                ResetAccelerometer();

                m_bActive = false;
                m_bEmulation = false;
            }
        }


#if !WINDOWS
        private void accelerometer_CurrentValueChanged(object sender, Microsoft.Devices.Sensors.SensorReadingEventArgs<Microsoft.Devices.Sensors.AccelerometerReading> e)
        {
            // store the accelerometer value in our variable to be used on the next Update
            m_obAccelerationValue.x = e.SensorReading.Acceleration.Y;
            m_obAccelerationValue.y = -e.SensorReading.Acceleration.X;
            m_obAccelerationValue.z = e.SensorReading.Acceleration.Z;
            m_obAccelerationValue.timestamp = e.SensorReading.Timestamp.Ticks;
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
                        stateValue.Y = .1f;
                    if (keyboardState.IsKeyDown(Keys.Down))
                        stateValue.Y = -.1f;

                    stateValue.Normalize();

                    m_obAccelerationValue.x = stateValue.X;
                    m_obAccelerationValue.y = stateValue.Y;
                    m_obAccelerationValue.z = stateValue.Z;
                    m_obAccelerationValue.timestamp = DateTime.Now.Ticks;
                }

                m_pAccelDelegate.DidAccelerate(m_obAccelerationValue);
            }
        }
    }
}
