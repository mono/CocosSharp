using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Box2D.Common;
using Box2D.Collision;

namespace Box2D.Dynamics
{
    public class b2WorldRayCastWrapper : Ib2RayCastCallback
    {
        public float RayCastCallback(b2RayCastInput input, int proxyId)
        {
            b2FixtureProxy proxy = (b2FixtureProxy)broadPhase.GetUserData(proxyId);
            b2Fixture fixture = proxy.fixture;
            int index = proxy.childIndex;
            b2RayCastOutput output;
            bool hit = fixture.RayCast(out output, input, index);

            if (hit)
            {
                float fraction = output.fraction;
                b2Vec2 point = (1.0f - fraction) * input.p1 + fraction * input.p2;
                return callback.ReportFixture(fixture, point, output.normal, fraction);
            }

            return input.maxFraction;
        }

        private b2BroadPhase broadPhase;
        public b2BroadPhase BroadPhase
        {
            get { return (broadPhase); }
            set { broadPhase = value; }
        }
             
        private b2RayCastCallback callback;
        public b2RayCastCallback Callback
        {
            get { return (callback); }
            set { callback = value; }
        }
    }
}
