using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;
using System.Diagnostics;
using Random = CocosSharp.CCRandom;

namespace tests
{
    public class Bug422Layer : BugsTestBaseLayer
    {

        public Bug422Layer()
        {
            reset();
        }

        public void reset()
        {

            int localtag = 0;
            localtag++;

            // TO TRIGGER THE BUG:
            // remove the itself from parent from an action
            // The menu will be removed, but the instance will be alive
            // and then a new node will be allocated occupying the memory.
            // => CRASH BOOM BANG
            CCNode node = GetChildByTag(localtag - 1);
            CCLog.Log("Menu: %p", node);
            RemoveChild(node, false);
            //	[self removeChildByTag:localtag-1 cleanup:NO];

            CCMenuItem item1 = new CCMenuItemFont("One", menuCallback);
            CCLog.Log("MenuItemFont: %p", item1);
            CCMenuItem item2 = new CCMenuItemFont("Two", menuCallback);
            CCMenu menu = new CCMenu(item1, item2);
            menu.AlignItemsVertically();

            float x = CCRandom.Next() * 50;
            float y = CCRandom.Next() * 50;
            menu.Position = menu.Position + new CCPoint(x, y);
            AddChild(menu, 0, localtag);

            //[self check:self];
        }

        public void check(CCNode t)
        {
            //List<CCNode> array = t.children;
            //object pChild = null;
            //foreach (var array in pChild)
            //{
            //     //CC_BREAK_IF(! pChild);
            //    CCNode node = (CCNode)pChild;
            //    //CCLog.Log("%p, rc: %d", node, node.retainCount());
            //    check(node);
            //}

            //CCArray *array = t->getChildren();
            //object* pChild = NULL;
            //CCARRAY_FOREACH(array, pChild)
            //{
            //    CC_BREAK_IF(! pChild);
            //    CCNode* node = (CCNode*) pChild;
            //    CCLog("%p, rc: %d", node, node->retainCount());
            //    check(node);
            //}
            throw new NotFiniteNumberException();
            
        }

        public void menuCallback(object sender)
        {
            reset();
        }
    }
}
