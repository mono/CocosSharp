using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cocos2D
{
    public class CCAcceleration
    {
        public double X;
        public double Y;
        public double Z;
        public double TimeStamp;
    }
    
    public interface ICCAccelerometerDelegate
    {
        void DidAccelerate(CCAcceleration pAccelerationValue);
    }
}
