using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class PerformanceTextureTest
    {
        public static int TEST_COUNT = 1;
        public static int s_nTexCurCase = 0;

        //        public float calculateDeltaTime( struct timeval *lastUpdate )
        //{
        //    struct timeval now;

        //    gettimeofday( &now, NULL);

        //    float dt = (now.tv_sec - lastUpdate->tv_sec) + (now.tv_usec - lastUpdate->tv_usec) / 1000000.0f;

        //    return dt;
        //}

        public static void runTextureTest()
        {
            s_nTexCurCase = 0;
            CCScene pScene = TextureTest.scene();
            CCDirector.SharedDirector.ReplaceScene(pScene);
        }
    }
}
