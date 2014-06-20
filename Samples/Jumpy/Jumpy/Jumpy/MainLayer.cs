// MainLayer.cs
//
// Author(s)
//	Stephane Delcroix <stephane@delcroix.org>
//
// Copyright (C) 2012 s. Delcroix
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software 
// and associated documentation files (the "Software"), to deal in the Software without restriction, 
// including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, 
// subject to the following conditions:
//		
// 		The above copyright notice and this permission notice shall be included in all copies or 
//		substantial portions of the Software.
//		
//		THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING 
//		BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND 
//		NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
//		DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
//		OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using Microsoft.Xna.Framework;
using CocosSharp;

namespace Jumpy
{
    public class MainLayer : CCLayer
    {
        protected int currentCloudTag;
        protected int numClouds = 12;

        protected enum Tags : int
        {
            SpriteManager = 0,
            Bird,
            ScoreLabel,
            Particles,
            CloudsStart = 100,
            PlatformsStart = 200,
            BomusStart = 300,
        }

        public MainLayer()
        {
            var batchnode = new CCSpriteBatchNode("Images/sprites", 10);
            AddChild(batchnode, -1, (int)Tags.SpriteManager);

            var background = new CCSprite(batchnode.Texture, new CCRect(0, 0, 320, 480));
            background.Position = new CCPoint(160, 240);
            batchnode.AddChild(background);
        }

        public override void OnEnter()
        {
            base.OnEnter();

            InitClouds();


        }

        void InitClouds()
        {

            currentCloudTag = (int)Tags.CloudsStart;
            while (currentCloudTag < (int)Tags.CloudsStart + numClouds)
            {
                InitCloud();
                currentCloudTag++;
            }

            ResetClouds();
        }

        private Random ran = new Random();

        void InitCloud()
        {

            CCRect rect;
            switch (ran.Next() % 3)
            {
                case 0:
                    rect = new CCRect(336, 16, 256, 108);
                    break;
                case 1:
                    rect = new CCRect(336, 128, 257, 110);
                    break;
                default:
                    rect = new CCRect(336, 240, 252, 119);
                    break;
            }

            var batchNode = GetChildByTag((int)Tags.SpriteManager) as CCSpriteBatchNode;
            var cloud = new CCSprite(batchNode.Texture, rect);
            batchNode.AddChild(cloud, 3, currentCloudTag);

            cloud.Opacity = 128;
        }

        protected void ResetClouds()
        {
            //	NSLog(@"resetClouds");

            currentCloudTag = (int)Tags.CloudsStart;

            while (currentCloudTag < (int)Tags.CloudsStart + numClouds)
            {
                ResetCloud();

                var batchNode = GetChildByTag((int)Tags.SpriteManager) as CCSpriteBatchNode;
                var cloud = batchNode.GetChildByTag(currentCloudTag) as CCSprite;
                cloud.Position = new CCPoint(cloud.Position.X, cloud.Position.Y - 480);

                currentCloudTag++;
            }
        }

        protected void ResetCloud()
        {

            var batchNode = GetChildByTag((int)Tags.SpriteManager) as CCSpriteBatchNode;
            var cloud = batchNode.GetChildByTag(currentCloudTag) as CCSprite;

            float distance = ran.Next() % 20 + 5;

            float scale = 5.0f / distance;
            cloud.Scale = scale;
            if (ran.Next() % 2 == 1)
                cloud.ScaleX = -cloud.ScaleX;

            var size = cloud.ContentSize;
            float scaled_width = size.Width * scale;
            float x = ran.Next() % (320 + (int)scaled_width) - scaled_width / 2;
            float y = ran.Next() % (480 - (int)scaled_width) + scaled_width / 2 + 480;

            cloud.Position = new CCPoint(x, y);
        }

        protected virtual void Step(float dt)
        {
            //	NSLog(@"Main::step");

            for (var t = (int)Tags.CloudsStart; t < (int)Tags.CloudsStart + numClouds; t++)
            {
                var batchNode = GetChildByTag((int)Tags.SpriteManager) as CCSpriteBatchNode;
                var cloud = batchNode.GetChildByTag(t) as CCSprite;

                var pos = cloud.Position;
                var size = cloud.ContentSize;
                pos.X += 0.1f * cloud.ScaleY;
                if (pos.X > 320 + size.Width / 2)
                {
                    pos.X = -size.Width / 2;
                }
                cloud.Position = pos;
            }
        }
    }
}

