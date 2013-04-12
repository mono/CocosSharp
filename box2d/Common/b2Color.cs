using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Box2D.Common
{
    public struct b2Color
    {

        /*public b2Color() { _r = 0f; _g = 0f; _b = 0f; }*/
        public b2Color(float xr, float xg, float xb) { r = xr; g = xg; b = xb; }
        public void Set(float ri, float gi, float bi) { r = ri; g = gi; b = bi; }
        public float r, g, b;
    }
}
