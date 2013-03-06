using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;
using System.Diagnostics;

namespace tests
{
    public class QuestionContainerSprite : CCSprite
    {
        public override bool Init()
        {
            if (base.Init())
            {
                //Add label
                CCLabelTTF label = CCLabelTTF.Create("Answer 1", "arial", 12);
                label.Tag = 100;

                //Add the background
                CCSize size = CCDirector.SharedDirector.WinSize;
                CCSprite corner = CCSprite.Create("Images/bugs/corner");

                int width = (int)(size.Width * 0.9f - (corner.ContentSize.Width * 2));
                int height = (int)(size.Height * 0.15f - (corner.ContentSize.Height * 2));
                //CCLayerColor layer = CCLayerColor.layerWithColorWidthHeight(new ccColor4B(r = 255, g = 255, b = 255, a = 255 * 0.75), width, height);
                //layer.position = new CCPoint(-width / 2, -height / 2);

                //First button is blue,
                //Second is red
                //Used for testing - change later
                int a = 0;

                if (a == 0)
                    label.Color = new CCColor3B(0, 0, 255); //ccBLUE
                else
                {
                    CCLog.Log("Color changed");
                    label.Color = new CCColor3B(255, 0, 0);
                }
                a++;
                //addChild(layer);

                corner.Position = new CCPoint(-(width / 2 + corner.ContentSize.Width / 2), -(height / 2 + corner.ContentSize.Height / 2));
                AddChild(corner);

                CCSprite corner2 = CCSprite.Create("Images/bugs/corner");
                corner2.Position = new CCPoint(-corner.Position.x, corner.Position.y);
                corner2.FlipX = true;
                AddChild(corner2);

                CCSprite corner3 = CCSprite.Create("Images/bugs/corner");
                corner3.Position = new CCPoint(corner.Position.x, -corner.Position.y);
                corner3.FlipY = true;
                AddChild(corner3);

                CCSprite corner4 = CCSprite.Create("Images/bugs/corner");
                corner4.Position = new CCPoint(corner2.Position.x, -corner2.Position.y);
                corner4.FlipX = true;
                corner4.FlipY = true;
                AddChild(corner4);

                CCSprite edge = CCSprite.Create("Images/bugs/edge");
                edge.ScaleX = width;
                edge.Position = new CCPoint(corner.Position.x + (corner.ContentSize.Width / 2) + (width / 2), corner.Position.y);
                AddChild(edge);

                CCSprite edge2 = CCSprite.Create("Images/bugs/edge");
                edge2.ScaleX = width;
                edge2.Position = new CCPoint(corner.Position.x + (corner.ContentSize.Width / 2) + (width / 2), -corner.Position.y);
                edge2.FlipX = true;
                AddChild(edge2);

                CCSprite edge3 = CCSprite.Create("Images/bugs/edge");
                edge3.Rotation = 90;
                edge3.ScaleX = height;
                edge3.Position = new CCPoint(corner.Position.x, corner.Position.y + (corner.ContentSize.Height / 2) + (height / 2));
                AddChild(edge3);

                CCSprite edge4 = CCSprite.Create("Images/bugs/edge");
                edge4.Rotation = 270;
                edge4.ScaleX = height;
                edge4.Position = new CCPoint(-corner.Position.x, corner.Position.y + (corner.ContentSize.Height / 2) + (height / 2));
                AddChild(edge4);

                AddChild(label);
                return true;
            }

            return false;
        }
    }
}
