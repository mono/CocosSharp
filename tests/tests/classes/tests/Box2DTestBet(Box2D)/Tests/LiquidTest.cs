using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Box2D.Collision.Shapes;
using Box2D.Common;
using Box2D.Dynamics;

namespace Box2D.TestBed.Tests
{
    internal class LiquidTest : Test
    {
        // IF YOU CHANGE THIS change the corresponding value in World
        public static int LIQUID_INT = 1234598372;

        private b2Body bod;

        private bool firstTime = true;

        private const int nParticles = 1000;

        private float totalMass = 10.0f;
        private float boxWidth = 2.0f;
        private float boxHeight = 20.0f;

        private float fluidMinX = -11.0f;
        private float fluidMaxX = 5.0f;
        private float fluidMinY = -10.0f;
        private float fluidMaxY = 10.0f;

        private b2Body[] liquid;

        private float rad = 0.6f;
        private float visc = 0.004f; //0.005f;

        private List<int>[,] hash;
        private int hashWidth, hashHeight;


        public LiquidTest()
        {
            hash = new List<int>[40, 40];
            for (int i = 0; i < 40; ++i)
            {
                for (int j = 0; j < 40; ++j)
                {
                    hash[i, j] = new List<int>();
                }
            }
            hashWidth = 40;
            hashHeight = 40;

            //if (firstTime)
            //{
            //    setCamera(new Vec2(0, 2), 35f);
            //    firstTime = false;
            //}

            //m_getWorld().setGravity(new Vec2(0.0f,0.0f));

            b2Body ground = null;
            {
                b2BodyDef bd = new b2BodyDef();
                bd.position.Set(0.0f, 0.0f);
                ground = m_world.CreateBody(bd);
                b2PolygonShape shape = new b2PolygonShape();
                shape.SetAsBox(5.0f, 0.5f);
                ground.CreateFixture(shape, 0);

                shape.SetAsBox(1.0f, 0.2f, new b2Vec2(0.0f, 4.0f), -0.2f);
                ground.CreateFixture(shape, 0);
                shape.SetAsBox(1.5f, 0.2f, new b2Vec2(-1.2f, 5.2f), -1.5f);
                ground.CreateFixture(shape, 0);
                shape.SetAsBox(0.5f, 50.0f, new b2Vec2(5.0f, 0.0f), 0.0f);
                ground.CreateFixture(shape, 0);

                shape.SetAsBox(0.5f, 3.0f, new b2Vec2(-8.0f, 0.0f), 0.0f);
                ground.CreateFixture(shape, 0);

                shape.SetAsBox(2.0f, 0.1f, new b2Vec2(-6.0f, -2.8f), 0.1f);
                ground.CreateFixture(shape, 0);

                b2CircleShape cd = new b2CircleShape();
                cd.Radius = 0.5f;
                cd.Position.Set(-0.5f, -4.0f);
                ground.CreateFixture(cd, 0);

            }

            liquid = new b2Body[nParticles];
            float massPerParticle = totalMass / nParticles;
            //		PointDef pd = new PointDef();
            //		pd.mass = massPerParticle;
            //		pd.restitution = 0.0f;
            //		pd.filter.groupIndex = -10;
            b2CircleShape pd = new b2CircleShape();
            b2FixtureDef fd = new b2FixtureDef();
            fd.shape = pd;
            fd.density = 1f;
            fd.filter.groupIndex = -10;
            pd.Radius = .05f;
            fd.restitution = 0.4f;
            fd.friction = 0.0f;
            float cx = 0.0f;
            float cy = 25.0f;
            for (int i = 0; i < nParticles; ++i)
            {
                b2BodyDef bd = new b2BodyDef();
                bd.position = new b2Vec2(Rand.RandomFloat(cx - boxWidth * .5f, cx + boxWidth * .5f),
                    Rand.RandomFloat(cy - boxHeight * .5f, cy + boxHeight * .5f));
                bd.fixedRotation = true;
                bd.type = b2BodyType.b2_dynamicBody;
                b2Body b = m_world.CreateBody(bd);

                b.CreateFixture(fd).UserData = LIQUID_INT;

                b2MassData md = new b2MassData();
                md.mass = massPerParticle;
                md.I = 1.0f;
                b.SetMassData(md);
                b.SetSleepingAllowed(false);
                liquid[i] = b;
            }

            b2PolygonShape polyDef = new b2PolygonShape();
            polyDef.SetAsBox(Rand.RandomFloat(0.3f, 0.7f), Rand.RandomFloat(0.3f, 0.7f));
            b2BodyDef bodyDef = new b2BodyDef();
            bodyDef.position = new b2Vec2(0.0f, 25.0f);
            bodyDef.type = b2BodyType.b2_dynamicBody;
            bod = m_world.CreateBody(bodyDef);
            bod.CreateFixture(polyDef, 1f);
        }

        public static float Map(float val, float fromMin, float fromMax, float toMin, float toMax)
        {
            float mult = (val - fromMin) / (fromMax - fromMin);
            return toMin + mult * (toMax - toMin);
        }

        private int hashX(float x)
        {
            return (int)Map(x, fluidMinX, fluidMaxX, 0, hashWidth - .001f);
        }

        private int hashY(float y)
        {
            return (int)Map(y, fluidMinY, fluidMaxY, 0, hashHeight - .001f);
        }

        private void hashLocations()
        {
            for (int a = 0; a < hashWidth; a++)
            {
                for (int b = 0; b < hashHeight; b++)
                {
                    hash[a, b].Clear();
                }
            }

            for (int a = 0; a < liquid.Length; a++)
            {
                int hcell = hashX(liquid[a].Sweep.c.x);
                int vcell = hashY(liquid[a].Sweep.c.y);
                if (hcell > -1 && hcell < hashWidth && vcell > -1 && vcell < hashHeight)
                    hash[hcell, vcell].Add(a);
            }
        }

        float[] _xchange = new float[nParticles];
        float[] _ychange = new float[nParticles];
        float[] _xs = new float[nParticles];
        float[] _ys = new float[nParticles];
        float[] _vxs = new float[nParticles];
        float[] _vys = new float[nParticles];
        float[] _vlen = new float[64];

        private void applyLiquidConstraint(float deltaT)
        {
            /*
		 * Unfortunately, this simulation method is not actually scale
		 * invariant, and it breaks down for rad < ~3 or so.  So we need
		 * to scale everything to an ideal rad and then scale it back after.
		 */
            const float idealRad = 50.0f;
            float multiplier = idealRad / rad;

            float[] xchange = _xchange;
            float[] ychange = _ychange;

            Array.Clear(xchange, 0, xchange.Length);
            Array.Clear(ychange, 0, xchange.Length);

            float[] xs = _xs;
            float[] ys = _ys;
            float[] vxs = _vxs;
            float[] vys = _vys;

            for (int i = 0; i < liquid.Length; ++i)
            {
                xs[i] = multiplier * liquid[i].Sweep.c.x;
                ys[i] = multiplier * liquid[i].Sweep.c.y;
                vxs[i] = multiplier * liquid[i].LinearVelocity.x;
                vys[i] = multiplier * liquid[i].LinearVelocity.y;
            }

            List<int> neighbors = new List<int>();

            for (int i = 0; i < liquid.Length; i++)
            {
                // Populate the neighbor list from the 9 proximate cells
                neighbors.Clear();
                int hcell = hashX(liquid[i].Sweep.c.x);
                int vcell = hashY(liquid[i].Sweep.c.y);
                for (int nx = -1; nx < 2; nx++)
                {
                    for (int ny = -1; ny < 2; ny++)
                    {
                        int xc = hcell + nx;
                        int yc = vcell + ny;
                        if (xc > -1 && xc < hashWidth && yc > -1 && yc < hashHeight && hash[xc, yc].Count > 0)
                        {
                            for (int a = 0; a < hash[xc, yc].Count; a++)
                            {
                                var ne = hash[xc, yc][a];
                                if (ne != i) neighbors.Add(ne);
                            }
                        }
                    }
                }

                // Particle pressure calculated by particle proximity
                // Pressures = 0 iff all particles within range are idealRad distance away
                if (_vlen == null || _vlen.Length < neighbors.Count)
                {
                    var len = _vlen.Length;
                    while (len < neighbors.Count) len *= 2;
                    _vlen = new float[len];
                }
                float[] vlen = _vlen;
                float p = 0.0f;
                float pnear = 0.0f;
                for (int a = 0; a < neighbors.Count; a++)
                {
                    var n = neighbors[a];
                    int j = n;
                    float vx = xs[j] - xs[i]; //liquid[j].m_sweep.c.x - liquid[i].m_sweep.c.x;
                    float vy = ys[j] - ys[i]; //liquid[j].m_sweep.c.y - liquid[i].m_sweep.c.y;

                    //early exit check
                    if (vx > -idealRad && vx < idealRad && vy > -idealRad && vy < idealRad)
                    {
                        float vlensqr = (vx * vx + vy * vy);
                        //within idealRad check
                        if (vlensqr < idealRad * idealRad)
                        {
                            vlen[a] = (float) Math.Sqrt(vlensqr);
                            if (vlen[a] < float.Epsilon) vlen[a] = idealRad - .01f;
                            float oneminusq = 1.0f - (vlen[a] / idealRad);
                            p = (p + oneminusq * oneminusq);
                            pnear = (pnear + oneminusq * oneminusq * oneminusq);
                        }
                        else
                        {
                            vlen[a] = float.MaxValue;
                        }
                    }
                }

                // Now actually apply the forces
                //System.out.println(p);
                float pressure = (p - 5F) / 2.0F; //normal pressure term
                float presnear = pnear / 2.0F; //near particles term
                float changex = 0.0F;
                float changey = 0.0F;
                for (int a = 0; a < neighbors.Count; a++)
                {
                    var n = neighbors[a];
                    int j = n;
                    float vx = xs[j] - xs[i]; //liquid[j].m_sweep.c.x - liquid[i].m_sweep.c.x;
                    float vy = ys[j] - ys[i]; //liquid[j].m_sweep.c.y - liquid[i].m_sweep.c.y;
                    if (vx > -idealRad && vx < idealRad && vy > -idealRad && vy < idealRad)
                    {
                        if (vlen[a] < idealRad)
                        {
                            float q = vlen[a] / idealRad;
                            float oneminusq = 1.0f - q;
                            float factor = oneminusq * (pressure + presnear * oneminusq) / (2.0F * vlen[a]);
                            float dx = vx * factor;
                            float dy = vy * factor;
                            float relvx = vxs[j] - vxs[i];
                            float relvy = vys[j] - vys[i];
                            factor = visc * oneminusq * deltaT;
                            dx -= relvx * factor;
                            dy -= relvy * factor;
                            //liquid[j].m_xf.position.x += dx;//*deltaT*deltaT;
                            //liquid[j].m_xf.position.y += dy;//*deltaT*deltaT;
                            //liquid[j].m_linearVelocity.x += dx;///deltaT;//deltaT;
                            //liquid[j].m_linearVelocity.y += dy;///deltaT;//deltaT;
                            xchange[j] += dx;
                            ychange[j] += dy;
                            changex -= dx;
                            changey -= dy;
                        }
                    }
                }
                //liquid[i].m_xf.position.x += changex;//*deltaT*deltaT;
                //liquid[i].m_xf.position.y += changey;//*deltaT*deltaT;
                //liquid[i].m_linearVelocity.x += changex;///deltaT;//deltaT;
                //liquid[i].m_linearVelocity.y += changey;///deltaT;//deltaT;
                xchange[i] += changex;
                ychange[i] += changey;
            }
            //multiplier *= deltaT;
            for (int i = 0; i < liquid.Length; ++i)
            {
                liquid[i].Transform.p.x += xchange[i] / multiplier;
                liquid[i].Transform.p.y += ychange[i] / multiplier;
                liquid[i].m_linearVelocity.x += xchange[i] / (multiplier * deltaT);
                liquid[i].m_linearVelocity.y += ychange[i] / (multiplier * deltaT);
            }

        }


        public override void Step(Settings settings)
        {
            base.Step(settings);
            float hz = settings.hz;
            float dt = 1.0f / hz;

            int n = 1;
            for (int i = 0; i < n; ++i)
            {
                hashLocations();
                applyLiquidConstraint(dt * n);
            }
            dampenLiquid();

            checkBounds();
        }

        private void checkBounds()
        {
            for (int i = 0; i < liquid.Length; ++i)
            {
                if (liquid[i].WorldCenter.y < -10.0f)
                {
                    m_world.DestroyBody(liquid[i]);
                    float massPerParticle = totalMass / nParticles;

                    var pd = new b2CircleShape();
                    var fd = new b2FixtureDef();
                    fd.shape = pd;
                    fd.density = 1.0f;
                    fd.filter.groupIndex = -10;
                    pd.Radius = .05f;
                    fd.restitution = 0.4f;
                    fd.friction = 0.0f;
                    float cx = 0.0f + Rand.RandomFloat(-0.6f, 0.6f);
                    float cy = 15.0f + Rand.RandomFloat(-2.3f, 2.0f);
                    var bd = new b2BodyDef();
                    bd.position = new b2Vec2(cx, cy);
                    bd.fixedRotation = true;
                    bd.type = b2BodyType.b2_dynamicBody;
                    var b = m_world.CreateBody(bd);
                    b.CreateFixture(fd).UserData = LIQUID_INT;
                    var md = new b2MassData();
                    md.mass = massPerParticle;
                    md.I = 1.0f;
                    b.SetMassData(md);
                    b.SetSleepingAllowed(false);
                    liquid[i] = b;
                }
            }

            if (bod.WorldCenter.y < -15.0f)
            {
                m_world.DestroyBody(bod);
                var polyDef = new b2PolygonShape();
                polyDef.SetAsBox(Rand.RandomFloat(0.3f, 0.7f), Rand.RandomFloat(0.3f, 0.7f));
                var bodyDef = new b2BodyDef();
                bodyDef.position = new b2Vec2(0.0f, 25.0f);
                bodyDef.type = b2BodyType.b2_dynamicBody;
                bod = m_world.CreateBody(bodyDef);
                bod.CreateFixture(polyDef, 1f);
            }
        }

        private void dampenLiquid()
        {
            for (int i = 0; i < liquid.Length; ++i)
            {
                var b = liquid[i];
                b.LinearVelocity = b.LinearVelocity * 0.995f;
            }
        }
    }
}

