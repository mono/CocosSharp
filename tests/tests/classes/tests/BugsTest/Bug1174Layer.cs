using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;
using System.Diagnostics;
using Random = cocos2d.Random;

namespace tests
{
    public class Bug1174Layer : BugsTestBaseLayer
    {
        public override bool Init()
        {
            if (base.Init())
            {
                // 		// seed
                // 		srand(0);

                CCPoint A, B, C, D, p1, p2, p3, p4;
                float s, t;

                int err = 0;
                int ok = 0;

                //
                // Test 1.
                //
                CCLog.Log("Test1 - Start");
                for (int i = 0; i < 10000; i++)
                {
                    // A | b
                    // -----
                    // c | d
                    float ax = Random.Next() * -5000;
                    float ay = Random.Next() * 5000;

                    // a | b
                    // -----
                    // c | D
                    float dx = Random.Next() * 5000;
                    float dy = Random.Next() * -5000;

                    // a | B
                    // -----
                    // c | d
                    float bx = Random.Next() * 5000;
                    float by = Random.Next() * 5000;

                    // a | b
                    // -----
                    // C | d
                    float cx = Random.Next() * -5000;
                    float cy = Random.Next() * -5000;

                    A = new CCPoint(ax, ay);
                    B = new CCPoint(bx, by);
                    C = new CCPoint(cx, cy);
                    D = new CCPoint(dx, dy);
                    //if (CCPointExtension.ccpLineIntersect(A, D, B, C, ref s, ref t))
                    //{
                    //    if (check_for_error(A, D, B, C, s, t) != 0)
                    //        err++;
                    //    else
                    //        ok++;
                    //}
                }
                CCLog.Log("Test1 - End. OK=%i, Err=%i", ok, err);

                //
                // Test 2.
                //
                CCLog.Log("Test2 - Start");

                p1 = new CCPoint(220, 480);
                p2 = new CCPoint(304, 325);
                p3 = new CCPoint(264, 416);
                p4 = new CCPoint(186, 416);
                s = 0.0f;
                t = 0.0f;
                if (CCPointExtension.LineIntersect(p1, p2, p3, p4, ref s, ref t))
                    check_for_error(p1, p2, p3, p4, s, t);

                CCLog.Log("Test2 - End");

                
                //
                // Test 3
                //
                CCLog.Log("Test3 - Start");

                ok = 0;
                err = 0;
                for (int i = 0; i < 10000; i++)
                {
                    // A | b
                    // -----
                    // c | d
                    float ax = Random.Next() * -500;
                    float ay = Random.Next() * 500;
                    p1 = new CCPoint(ax, ay);

                    // a | b
                    // -----
                    // c | D
                    float dx = Random.Next() * 500;
                    float dy = Random.Next() * -500;
                    p2 = new CCPoint(dx, dy);


                    //////

                    float y = ay - ((ay - dy) / 2.0f);

                    // a | b
                    // -----
                    // C | d
                    float cx = Random.Next() * -500;
                    p3 = new CCPoint(cx, y);

                    // a | B
                    // -----
                    // c | d
                    float bx = Random.Next() * 500;
                    p4 = new CCPoint(bx, y);

                    s = 0.0f;
                    t = 0.0f;
                    if (CCPointExtension.LineIntersect(p1, p2, p3, p4, ref s, ref t))
                    {
                        if (check_for_error(p1, p2, p3, p4, s, t) != 0)
                            err++;
                        else
                            ok++;
                    }
                }

                CCLog.Log("Test3 - End. OK=%i, err=%i", ok, err);
                return true;
            }

            return false;
        }

        public int check_for_error(CCPoint p1, CCPoint p2, CCPoint p3, CCPoint p4, float s, float t)
        {
            //	the hit point is		p3 + t * (p4 - p3);
            //	the hit point also is	p1 + s * (p2 - p1);

            CCPoint p4_p3 = p4 - p3;
            CCPoint p4_p3_t = p4_p3 * t;
            CCPoint hitPoint1 = p3 + p4_p3_t;

            CCPoint p2_p1 = p2 - p1;
            CCPoint p2_p1_s = p2_p1 * s;
            CCPoint hitPoint2 = p1 + p2_p1_s;

            // Since float has rounding errors, only check if diff is < 0.05
            if ((Math.Abs(hitPoint1.X - hitPoint2.X) > 0.1f) || (Math.Abs(hitPoint1.Y - hitPoint2.Y) > 0.1f))
            {
                CCLog.Log("ERROR: (%f,%f) != (%f,%f)", hitPoint1.X, hitPoint1.Y, hitPoint2.X, hitPoint2.Y);
                return 1;
            }

            return 0;
        }
    }
}
