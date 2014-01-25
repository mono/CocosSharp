using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;
using System.Diagnostics;

namespace tests
{
    public class Bug458Layer : BugsTestBaseLayer
    {
        public Bug458Layer()
        {
            InitBug458Layer();
        }

        private void InitBug458Layer()
        {
            // ask director the the window size
            CCSize size = CCDirector.SharedDirector.WinSize;

            QuestionContainerSprite question = new QuestionContainerSprite();
            QuestionContainerSprite question2 = new QuestionContainerSprite();

            //		[question setContentSize:CGSizeMake(50,50)];
            //		[question2 setContentSize:CGSizeMake(50,50)];

            CCMenuItemSprite sprite = new CCMenuItemSprite(question2, question, this, selectAnswer);
            CCLayerColor layer = new CCLayerColor(new CCColor4B(0, 0, 255, 255), 100, 100);


            CCLayerColor layer2 = new CCLayerColor(new CCColor4B(255, 0, 0, 255), 100, 100);
            CCMenuItemSprite sprite2 = new CCMenuItemSprite(layer, layer2, this, selectAnswer);
            CCMenu menu = new CCMenu(sprite, sprite2, null);
            menu.AlignItemsVerticallyWithPadding(100);
            menu.Position = new CCPoint(size.Width / 2, size.Height / 2);

            // add the label as a child to this Layer
            AddChild(menu);

        }

        public void selectAnswer(object sender)
        {
            CCLog.Log("Selected");
        }
    }
}
