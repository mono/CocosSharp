using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cocos2d
{
    public class CCAcceleration
    {
        public double x;
        public double y;
        public double z;
        public double timestamp;
    }
    
    public interface ICCAccelerometerDelegate
    {
        void DidAccelerate(CCAcceleration pAccelerationValue);
    }
}
