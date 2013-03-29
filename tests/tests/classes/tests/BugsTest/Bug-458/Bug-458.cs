using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;
using System.Diagnostics;

namespace tests
{
    public class Bug458Layer : BugsTestBaseLayer
    {
        public override bool Init()
        {
            if (base.Init())
            {
                // ask director the the window size
                CCSize size = CCDirector.SharedDirector.WinSize;

                QuestionContainerSprite question = new QuestionContainerSprite();
                QuestionContainerSprite question2 = new QuestionContainerSprite();
                question.Init();
                question2.Init();

                //		[question setContentSize:CGSizeMake(50,50)];
                //		[question2 setContentSize:CGSizeMake(50,50)];

                CCMenuItemSprite sprite = CCMenuItemSprite.Create(question2, question, this, selectAnswer);
                CCLayerColor layer = CCLayerColor.Create(new CCColor4B(0, 0, 255, 255), 100, 100);


                CCLayerColor layer2 = CCLayerColor.Create(new CCColor4B(255, 0, 0, 255), 100, 100);
                CCMenuItemSprite sprite2 = CCMenuItemSprite.Create(layer, layer2, this, selectAnswer);
                CCMenu menu = CCMenu.Create(sprite, sprite2, null);
                menu.AlignItemsVerticallyWithPadding(100);
                menu.Position = new CCPoint(size.Width / 2, size.Height / 2);

                // add the label as a child to this Layer
                AddChild(menu);

                return true;
            }
            return false;
        }

        public void selectAnswer(object sender)
        {
            CCLog.Log("Selected");
        }
    }
}
