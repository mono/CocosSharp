using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna;
using Microsoft.Xna.Framework.Input;
using System.Reflection;

namespace CocosSharp
{
    public class CCAcceleration
    {
        public double X;
        public double Y;
        public double Z;
        public double TimeStamp;
    }

    public class CCAccelerometer
    {
        #if !WINDOWS && !OUYA && !NETFX_CORE && !MACOS && !WINDOWSGL
        // the accelerometer sensor on the device
        static Microsoft.Devices.Sensors.Accelerometer accelerometer = null;
        #endif

        const float TG3_GRAVITY_EARTH = 9.80665f;
        readonly CCAcceleration accelerationValue = new CCAcceleration();


        CCEventAccelerate accelerateEvent = new CCEventAccelerate();
        bool enabled = false;


        #region Properties

        public CCWindow Window { get; set; }

        bool Active { get; set; }
        bool Emulating { get; set; }

        public bool Enabled 
        {
            get { return enabled; }

            set 
            {
                enabled = value;
                if (enabled && !Active)
                {
                    #if !WINDOWS && !OUYA && !NETFX_CORE && !MACOS && !WINDOWSGL
                    try
                    {
                        if(Microsoft.Devices.Sensors.Accelerometer.IsSupported)
                        {
                            accelerometer.CurrentValueChanged += accelerometer_CurrentValueChanged;
                            accelerometer.Start();
                            Active = true;
                        }
                        else
                        {
                            Active = false;
                        }
                    }
                    catch (Microsoft.Devices.Sensors.AccelerometerFailedException)
                    {
                        Active = false;
                    }
                    #endif

                    if(!Active)
                    {
                        Active = true;
                        Emulating = true;
                    }
                    else
                    {
                        Emulating = false;
                    }
                }
                else
                {
                    if (Active && !Emulating)
                    {
                        #if !WINDOWS && !OUYA &&!NETFX_CORE && !MACOS && !WINDOWSGL
                        if (accelerometer != null)
                        {
                            accelerometer.CurrentValueChanged -= accelerometer_CurrentValueChanged;
                            accelerometer.Stop();
                        }
                        #endif
                    }

                    ResetAccelerometer();

                    Active = false;
                    Emulating = false;
                }
            }
        }

        #endregion Properties


        #region Constructors

        public CCAccelerometer(CCWindow window)
        {
            Window = window;

            #if !WINDOWS && !OUYA && !NETFX_CORE && !MACOS && !WINDOWSGL
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

        #endregion Constructors


        void ResetAccelerometer()
        {
            accelerationValue.X = 0;
            accelerationValue.Y = 0;
            accelerationValue.Z = 0;
        }

        #if !WINDOWS && !OUYA && !NETFX_CORE && !MACOS && !WINDOWSGL
        void accelerometer_CurrentValueChanged(object sender, Microsoft.Devices.Sensors.SensorReadingEventArgs<Microsoft.Devices.Sensors.AccelerometerReading> e)
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

            accelerationValue.TimeStamp = e.SensorReading.Timestamp.Ticks;
        }

        void UpdateAccelerationValue(string acceleration)
        {
            string[] temp = acceleration.Substring(1, acceleration.Length - 2).Split(':');

            // The format of the string is {X: 0000 Y: 0000 Z: 0000}
            // Here we need to parse differently so that we can get a constant value back
            //  Cocos2D-XNA mapps the Sensor reading of the X value to be our Y value
            //  and the Y value to our X value.  Also the values need to be negated so that 
            //  it maps correctly.
            
            #if ANDROID 
            accelerationValue.X = -float.Parse(temp[1].Substring(0, temp[1].Length - 1));
            accelerationValue.Y = -float.Parse(temp[2].Substring(0, temp[2].Length - 1));
            accelerationValue.Z = float.Parse(temp[3]);

            #elif WINDOWS_PHONE8
            accelerationValue.Y = float.Parse(temp[1].Substring(0, temp[1].Length - 1));
            accelerationValue.X = -float.Parse(temp[2].Substring(0, temp[2].Length - 1));
            accelerationValue.Z = float.Parse(temp[3]);
            
            #else
            accelerationValue.Y = -float.Parse(temp[1].Substring(0, temp[1].Length - 1));
            accelerationValue.X = -float.Parse(temp[2].Substring(0, temp[2].Length - 1));
            accelerationValue.Z = float.Parse(temp[3]);
            #endif
        }

        static Vector3 ParseVector3(string acceleration)
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
            var dispatcher = Window.EventDispatcher;
            if (dispatcher.IsEventListenersFor(CCEventListenerAccelerometer.LISTENER_ID))
            {
                if (Emulating)
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

                    accelerationValue.X = stateValue.X;
                    accelerationValue.Y = stateValue.Y;
                    accelerationValue.Z = stateValue.Z;
                    accelerationValue.TimeStamp = DateTime.Now.Ticks;
                }

                accelerateEvent.Acceleration = accelerationValue;
                dispatcher.DispatchEvent (accelerateEvent);
            }
        }
    }
}
