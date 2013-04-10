using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;
using System.Diagnostics;

namespace tests
{
    public class Bug624Layer : BugsTestBaseLayer
    {
        public override bool Init()
        {
            if (base.Init())
            {
                CCSize size = CCDirector.SharedDirector.WinSize;
                CCLabelTTF label = new CCLabelTTF("Layer1", "Marker Felt", 36);

                label.Position = new CCPoint(size.Width / 2, size.Height / 2);
                AddChild(label);
                AccelerometerEnabled = true;
                Schedule(switchLayer, 5.0f);

                return true;
            }

            return false;
        }

        public void switchLayer(float dt)
        {
            //unschedule(Bug624Layer.switchLayer);

            CCScene scene = new CCScene();
            scene.AddChild(Bug624Layer2.Create(), 0);
            CCDirector.SharedDirector.ReplaceScene(new CCTransitionFade(2.0f, scene, new CCColor3B { R = 255, G = 255, B = 255 }));
        }

        public virtual void didAccelerate(CCAcceleration pAccelerationValue)
        {
            CCLog.Log("Layer1 accel");
        }

        //LAYER_NODE_FUNC(Bug624Layer);
    }

    public class Bug624Layer2 : BugsTestBaseLayer
    {
        public override bool Init()
        {
            if (base.Init())
            {
                CCSize size = CCDirector.SharedDirector.WinSize;
                CCLabelTTF label = new CCLabelTTF("Layer2", "Marker Felt", 36);

                label.Position = new CCPoint(size.Width / 2, size.Height / 2);
                AddChild(label);
                AccelerometerEnabled = true;
                Schedule(switchLayer, 5.0f);

                return true;
            }

            return false;
        }

        public void switchLayer(float dt)
        {
            //unschedule(schedule_selector(Bug624Layer::switchLayer));

            CCScene scene = new CCScene();
            scene.AddChild(Bug624Layer.Create(), 0);
            CCDirector.SharedDirector.ReplaceScene(new CCTransitionFade(2.0f, scene, new CCColor3B { R = 255, G = 0, B = 0 }));
        }

        public virtual void didAccelerate(CCAcceleration pAccelerationValue)
        {
            CCLog.Log("Layer2 accel");
        }

        public new static Bug624Layer2 Create()
        {
            var ret = new Bug624Layer2();
            ret.Init();
            return ret;
        }

        //LAYER_NODE_FUNC(Bug624Layer2);
    }
}
