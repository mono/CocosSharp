using System;
using CocosSharp;
using Microsoft.Xna.Framework;
using CocosSharp.Spine;
using Spine;

namespace spine_cocossharp
{
    class SpineBoyLayer : CCNode
    {

        CCSkeletonAnimation skeletonNode;

        public SpineBoyLayer()
        {

            CCSize windowSize = CCDirector.SharedDirector.WinSize;

            var labelBones = new CCLabelTtf("B = Toggle Debug Bones", "arial", 12);
            labelBones.Position = new CCPoint(15, windowSize.Height - 10);
            labelBones.AnchorPoint = CCPoint.AnchorMiddleLeft;
            AddChild(labelBones);

            var labelSlots = new CCLabelTtf("M = Toggle Debug Slots", "arial", 12);
            labelSlots.Position = new CCPoint(15, windowSize.Height - 25);
            labelSlots.AnchorPoint = CCPoint.AnchorMiddleLeft;
            AddChild(labelSlots);

            var labelTimeScale = new CCLabelTtf("Up/Down = TimeScale +/-", "arial", 12);
            labelTimeScale.Position = new CCPoint(15, windowSize.Height - 40);
            labelTimeScale.AnchorPoint = CCPoint.AnchorMiddleLeft;
            AddChild(labelTimeScale);

            var labelScene = new CCLabelTtf("G = Goblins", "arial", 12);
            labelScene.Position = new CCPoint(15, windowSize.Height - 55);
            labelScene.AnchorPoint = CCPoint.AnchorMiddleLeft;
            AddChild(labelScene);

            String name = @"spineboy";
            skeletonNode = new CCSkeletonAnimation(name + ".json", name + ".atlas", 0.25f);

            skeletonNode.SetMix("walk", "jump", 0.2f);
            skeletonNode.SetMix("jump", "run", 0.2f);
            skeletonNode.SetAnimation(0, "walk", true);
            TrackEntry jumpEntry = skeletonNode.AddAnimation(0, "jump", false, 3);
            skeletonNode.AddAnimation(0, "run", true);

            skeletonNode.Start += Start;
            skeletonNode.End += End;
            skeletonNode.Complete += Complete;
            skeletonNode.Event += Event;

            skeletonNode.Position = new CCPoint(windowSize.Center.X, 10);
            AddChild(skeletonNode);

            var listener = new CCEventListenerTouchOneByOne();
            listener.OnTouchBegan = (touch, touchEvent) =>
                {
                    if (!skeletonNode.DebugBones)
                    {
                        skeletonNode.DebugBones = true; 
                    }
                    else if (skeletonNode.TimeScale == 1)
                        skeletonNode.TimeScale = 0.3f;
                    else if (skeletonNode.Skeleton.Skin.Name == "goblin")
                        skeletonNode.SetSkin("goblingirl");
                    return true;
                };
            EventDispatcher.AddEventListener(listener, this);

            var keyListener = new CCEventListenerKeyboard();
            keyListener.OnKeyPressed = (keyEvent) =>
                {
                    switch (keyEvent.Keys)
                    {
                        case CCKeys.B:
                            skeletonNode.DebugBones = !skeletonNode.DebugBones;
                            break;
                        case CCKeys.M:
                            skeletonNode.DebugSlots = !skeletonNode.DebugSlots;
                            break;
                        case CCKeys.Up:
                            skeletonNode.TimeScale += 0.1f;
                            break;
                        case CCKeys.Down:
                            skeletonNode.TimeScale -= 0.1f;
                            break;
                        case CCKeys.G:
                            CCDirector.SharedDirector.ReplaceScene(GoblinLayer.Scene);
                            break;

                    }

                };
            EventDispatcher.AddEventListener(keyListener, this);
        }

        public void Start(AnimationState state, int trackIndex)
        {
            var entry = state.GetCurrent(trackIndex);
            var animationName = (entry != null && entry.Animation != null) ? entry.Animation.Name : string.Empty;

            CCLog.Log(trackIndex + ":start " + animationName);
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
                var layer = new SpineBoyLayer();

                // add layer as a child to scene
                scene.AddChild(layer);

                // return the scene
                return scene;

            }

        }

    }
}

