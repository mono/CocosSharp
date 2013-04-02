using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Box2D.Dynamics
{
    public interface Ib2QueryCallback
    {
        bool QueryCallback(int proxyId);
    }
}
