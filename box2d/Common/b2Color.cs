using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Box2D.Common
{
    public struct b2Color
    {
        public float r { get { return (_r); } set { _r = value; } }
        public float g { get { return (_g); } set { _g = value; } }
        public float b { get { return (_b); } set { _b = value; } }

        /*public b2Color() { _r = 0f; _g = 0f; _b = 0f; }*/
        public b2Color(float r, float g, float b) { _r = r; _g = g; _b = b; }
        public void Set(float ri, float gi, float bi) { _r = ri; _g = gi; _b = bi; }
        private float _r, _g, _b;
    }
}
