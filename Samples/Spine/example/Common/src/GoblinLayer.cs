using System;
using CocosSharp;
using Microsoft.Xna.Framework;
using CocosSharp.Spine;
using Spine;

namespace spine_cocossharp
{
    class GoblinLayer : CCNode
    {

        CCSkeletonAnimation skeletonNode;
        CCActionState skeletonActionState;
        CCSequence skeletonMoveAction;
        bool isMoving;

        public GoblinLayer()
        {

			CCSize windowSize = Director.WindowSizeInPoints;

            var labelBones = new CCLabelTtf("B = Toggle Debug Bones", "arial", 12);
            labelBones.Position = new CCPoint(15, windowSize.Height - 10);
            labelBones.AnchorPoint = CCPoint.AnchorMiddleLeft;
            AddChild(labelBones);

            var labelSlots = new CCLabelTtf("M = Toggle Debug Slots", "arial", 12);
            labelSlots.Position = new CCPoint(15, windowSize.Height - 25);
            labelSlots.AnchorPoint = CCPoint.AnchorMiddleLeft;
            AddChild(labelSlots);

            var labelSkin = new CCLabelTtf("S = Toggle Skin", "arial", 12);
            labelSkin.Position = new CCPoint(15, windowSize.Height - 40);
            labelSkin.AnchorPoint = CCPoint.AnchorMiddleLeft;
            AddChild(labelSkin);

            var labelTimeScale = new CCLabelTtf("Up/Down = TimeScale +/-", "arial", 12);
            labelTimeScale.Position = new CCPoint(15, windowSize.Height - 70);
            labelTimeScale.AnchorPoint = CCPoint.AnchorMiddleLeft;
            AddChild(labelTimeScale);

            var labelAction = new CCLabelTtf("A = Toggle Move Action", "arial", 12);
            labelAction.Position = new CCPoint(15, windowSize.Height - 55);
            labelAction.AnchorPoint = CCPoint.AnchorMiddleLeft;
            AddChild(labelAction);

            var labelScene = new CCLabelTtf("P = SpineBoy", "arial", 12);
            labelScene.Position = new CCPoint(15, windowSize.Height - 85);
            labelScene.AnchorPoint = CCPoint.AnchorMiddleLeft;
            AddChild(labelScene);

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

            skeletonMoveAction = new CCSequence(new CCMoveTo(5, new CCPoint(windowSize.Width, 10)), new CCMoveTo(5, new CCPoint(10, 10)));

            skeletonActionState = skeletonNode.RepeatForever(skeletonMoveAction);
            isMoving = true;

            skeletonNode.Position = new CCPoint(windowSize.Center.X, skeletonNode.ContentSize.Height / 2);
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

			Director.EventDispatcher.AddEventListener(listener, this);

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
                        case CCKeys.S:
                            if (skeletonNode.Skeleton.Skin.Name == "goblin")
                                skeletonNode.SetSkin("goblingirl");
                            else
                                skeletonNode.SetSkin("goblin");
                            break;
                        case CCKeys.Up:
                            skeletonNode.TimeScale += 0.1f;
                            break;
                        case CCKeys.Down:
                            skeletonNode.TimeScale -= 0.1f;
                            break;
                        case CCKeys.A:
                            if (isMoving)
                            {
								StopAction(skeletonActionState);
                                isMoving = false;
                            }
                            else
                            {
                                skeletonActionState = skeletonNode.RepeatForever(skeletonMoveAction);
                                isMoving = true;
                            }
                            break;
                        case CCKeys.P:
							Director.ReplaceScene(SpineBoyLayer.Scene);
                            break;
                    }

                };
			Director.EventDispatcher.AddEventListener(keyListener, this);
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
                var layer = new GoblinLayer();

                // add layer as a child to scene
                scene.AddChild(layer);

                // return the scene
                return scene;

            }

        }

    }
}

