using System;
using System.Collections.Generic;
using CocosSharp;

namespace ${SolutionName}
{
    public class GameLayer : CCLayerGradient
    {
        CCParticleSystem galaxySystem;

        CCSprite monkeySprite1;
        CCSprite monkeySprite2;

        CCRepeatForever repeatedAction;
        CCSpawn dreamAction;

        int redColorIncrement = 10;

        public GameLayer()
            : base(CCColor4B.Blue, CCColor4B.AliceBlue)
        {
            // Set the layer gradient direction
            this.Vector = new CCPoint(0.5f, 0.5f);

            // Create and add sprites
            monkeySprite1 = new CCSprite("monkey");
            AddChild(monkeySprite1, 1);

            monkeySprite2 = new CCSprite("monkey");
            AddChild(monkeySprite2, 1);

            // Define actions
            var moveUp = new CCMoveBy(1.0f, new CCPoint(0.0f, 50.0f));
            var moveDown = moveUp.Reverse();

            // A CCSequence action runs the list of actions in ... sequence!
            CCSequence moveSeq = new CCSequence(new CCEaseBackInOut(moveUp), new CCEaseBackInOut(moveDown));

            repeatedAction = new CCRepeatForever(moveSeq);

            // A CCSpawn action runs the list of actions concurrently
            dreamAction = new CCSpawn(new CCFadeIn(5.0f), new CCWaves(5.0f, new CCGridSize(10, 20), 4, 4));

            // Schedule for method to be called every 0.1s
            Schedule(UpdateLayerGradient, 0.1f);
        }

        protected override void AddedToScene()
        {
            base.AddedToScene();

            CCRect visibleBounds = VisibleBoundsWorldspace;
            CCPoint centerBounds = visibleBounds.Center;
            CCPoint quarterWidthDelta = new CCPoint(visibleBounds.Size.Width / 4.0f, 0.0f);

            // Layout the positioning of sprites based on visibleBounds
            monkeySprite1.AnchorPoint = CCPoint.AnchorMiddle;
            monkeySprite1.Position = centerBounds + quarterWidthDelta;

            monkeySprite2.AnchorPoint = CCPoint.AnchorMiddle;
            monkeySprite2.Position = centerBounds - quarterWidthDelta;

            // Run actions on sprite
            // Note: we can reuse the same action definition on multiple sprites!
            monkeySprite1.RunAction(new CCSequence(dreamAction, repeatedAction));
            monkeySprite2.RunAction(new CCSequence(dreamAction, new CCDelayTime(0.5f), repeatedAction));

            // Create preloaded galaxy particle system  
            galaxySystem = new CCParticleGalaxy(centerBounds);

            // Customise default behaviour of predefined particle system
            galaxySystem.EmissionRate = 20.0f;
            galaxySystem.EndSize = 3.0f;
            galaxySystem.EndRadius = visibleBounds.Size.Width;
            galaxySystem.Life = 10.0f;

            AddChild(galaxySystem, 0);

            // Register to touch event
            var touchListener = new CCEventListenerTouchAllAtOnce();
            touchListener.OnTouchesEnded = OnTouchesEnded;
            AddEventListener (touchListener, this);
        }

        protected void UpdateLayerGradient(float dt)
        {
            CCColor3B startColor = this.StartColor;

            int newRedColor = startColor.R + redColorIncrement;

            if (newRedColor <= byte.MinValue)
            {
                newRedColor = 0;
                redColorIncrement *= -1;
            }
            else if (newRedColor >= byte.MaxValue)
            {
                newRedColor = byte.MaxValue;
                redColorIncrement *= -1;
            }

            startColor.R = (byte)(newRedColor);

            StartColor = startColor;
        }

        void OnTouchesEnded(List<CCTouch> touches, CCEvent touchEvent)
        {
            if (touches.Count > 0)
            {
                CCTouch touch = touches[0];
                CCPoint touchLocation = touch.Location;

                // Move particle system to touch location
                galaxySystem.StopAllActions();
                galaxySystem.RunAction(new CCMoveTo(3.0f, touchLocation));
            }
        }
    }
}