using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Box2D.Collision;
using Box2D.Dynamics;
using Box2D.Common;
using Box2D.Collision.Shapes;
using Box2D.Dynamics.Contacts;
using Box2D.Dynamics.Joints;

namespace Box2D
{
#if XBOX || WINDOWS_PHONE || PCL
    public static class b2FlagExtensions
    {
            public static bool HasFlag(this b2BodyFlags flag, b2BodyFlags testFlag)
            {
                return ((flag & testFlag) == testFlag);
            }

            public static bool HasFlag(this b2WorldFlags flag, b2WorldFlags testFlag)
            {
                return ((flag & testFlag) == testFlag);
            }

            public static bool HasFlag(this b2ContactFlags flag, b2ContactFlags testFlag)
            {
                return ((flag & testFlag) == testFlag);
            }

            public static bool HasFlag(this b2DrawFlags flag, b2DrawFlags testFlag)
            {
                return ((flag & testFlag) == testFlag);
            }
    }
#endif
}
