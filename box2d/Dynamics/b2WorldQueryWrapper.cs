using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Box2D.Common;
using Box2D.Collision;

namespace Box2D.Dynamics
{
    public class b2WorldQueryWrapper : Ib2QueryCallback
    {
        public bool QueryCallback(int proxyId)
        {
            b2FixtureProxy proxy = (b2FixtureProxy)broadPhase.GetUserData(proxyId);
            return callback.ReportFixture(proxy.fixture);
        }

        private b2BroadPhase broadPhase;
        public b2BroadPhase BroadPhase
        {
            get { return (broadPhase); }
            set { broadPhase = value; }
        }

        private b2QueryCallback callback;
        public b2QueryCallback Callback
        {
            get { return (callback); }
            set { callback = value; }
        }
    }
}
