using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Box2D.Collision;

namespace Box2D.Dynamics
{
    public interface Ib2RayCastCallback
    {
        float RayCastCallback(b2RayCastInput input, int proxyId);
    }
}
