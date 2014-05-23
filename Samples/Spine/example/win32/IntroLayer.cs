using System;
using CocosSharp;
using Microsoft.Xna.Framework;
using CocosSharp.Spine;
using Spine;

namespace spine_cocossharp
{
    class IntroLayer : CCNode
    {

        CCSkeletonAnimation skeletonNode;
 
        public IntroLayer()
        {

            CCSize windowSize = CCDirector.SharedDirector.WinSize;

            String name = @"goblins-ffd";
            //String name = @"goblins";
            skeletonNode = new CCSkeletonAnimation(name + ".json", name + ".atlas", 0.5f);
            skeletonNode.PremultipliedAlpha = true;

            skeletonNode.SetSkin("goblin");

            var wt = skeletonNode.NodeToWorldTransform;
            skeletonNode.SetSlotsToSetupPose();
            skeletonNode.UpdateWorldTransform();

            skeletonNode.AddAnimation(0, "walk", true, 4);
            skeletonNode.SetAnimation(0, "walk", true);

            skeletonNode.Start += Start;
            skeletonNode.End += End;
            skeletonNode.Complete += Complete;
            skeletonNode.Event += Event;

            //skeletonNode.RepeatForever(new CCFadeOut(1), new CCFadeIn(1));

            skeletonNode.RepeatForever(
                new CCMoveTo(5, new CCPoint(windowSize.Width, 10)), new CCMoveTo(5, new CCPoint(10, 10))
                );


            skeletonNode.Position = new CCPoint(windowSize.Center.X, skeletonNode.ContentSize.Height / 2);
            AddChild(skeletonNode);

            var listener = new CCEventListenerTouchOneByOne();
            listener.OnTouchBegan = (touch, touchEvent) =>
                {
                    if (!skeletonNode.DebugBones)
                        skeletonNode.DebugBones = true;
                    else if (skeletonNode.TimeScale == 1)
                        skeletonNode.TimeScale = 0.3f;
                    else if (skeletonNode.Skeleton.Skin.Name == "goblin")
                        skeletonNode.SetSkin("goblingirl");
                    return true;
                };
            EventDispatcher.AddEventListener(listener, this);
	

        }

        public void Start(AnimationState state, int trackIndex)
        {
            CCLog.Log(trackIndex + " " + state.GetCurrent(trackIndex) + ": start");
        }

        public void End(AnimationState state, int trackIndex)
        {
            CCLog.Log(trackIndex + " " + state.GetCurrent(trackIndex) + ": end");
        }

        public void Complete(AnimationState state, int trackIndex, int loopCount)
        {
            CCLog.Log(trackIndex + " " + state.GetCurrent(trackIndex) + ": complete " + loopCount);
        }

        public void Event(AnimationState state, int trackIndex, Event e)
        {
            CCLog.Log(trackIndex + " " + state.GetCurrent(trackIndex) + ": event " + e);
        }

        public static CCScene Scene
        {
            get
            {
                // 'scene' is an autorelease object.
                var scene = new CCScene();

                // 'layer' is an autorelease object.
                var layer = new IntroLayer();

                // add layer as a child to scene
                scene.AddChild(layer);

                // return the scene
                return scene;

            }

        }

    }
}

