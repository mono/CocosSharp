using System;
using Cocos2D;

namespace tests.classes
{
    public class BaseTest : CCLayer
    {
        public override void OnEnter()
        {
            base.OnEnter();

            // add title and subtitle
            var label = new CCLabelTTF(Title(), "Arial", 32);
            AddChild(label, 9999);
            label.Position = new CCPoint(CCVisibleRect.Center.X, CCVisibleRect.Top.Y - 30);

            string strSubtitle = Subtitle();
            if (!string.IsNullOrEmpty(strSubtitle))
            {
                var l = new CCLabelTTF(strSubtitle, "Thonburi", 16);
                AddChild(l, 9999);
                l.Position = new CCPoint(CCVisibleRect.Center.X, CCVisibleRect.Top.Y - 60);
            }

            // add menu
            // CC_CALLBACK_1 == std::bind( function_ptr, instance, std::placeholders::_1, ...)
            var item1 = new CCMenuItemImage(TestResource.s_pPathB1, TestResource.s_pPathB2,
                                            BackCallback);
            var item2 = new CCMenuItemImage(TestResource.s_pPathR1, TestResource.s_pPathR2,
                                            RestartCallback);
            var item3 = new CCMenuItemImage(TestResource.s_pPathF1, TestResource.s_pPathF2,
                                            NextCallback);

            var menu = new CCMenu(item1, item2, item3);

            menu.Position = CCPoint.Zero;
            item1.Position = new CCPoint(CCVisibleRect.Center.X - item2.ContentSize.Width * 2,
                                         CCVisibleRect.Bottom.Y + item2.ContentSize.Height / 2);
            item2.Position = new CCPoint(CCVisibleRect.Center.X,
                                         CCVisibleRect.Bottom.Y + item2.ContentSize.Height / 2);
            item3.Position = new CCPoint(CCVisibleRect.Center.X + item2.ContentSize.Width * 2,
                                         CCVisibleRect.Bottom.Y + item2.ContentSize.Height / 2);

            AddChild(menu, 9999);
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public virtual string Title()
        {
            return "";
        }

        public virtual string Subtitle()
        {
            return "";
        }

        public virtual void RestartCallback(Object pSender)
        {
            CCLog.Log("override restart!");
        }

        public virtual void NextCallback(Object pSender)
        {
            CCLog.Log("override next!");
        }

        public virtual void BackCallback(Object pSender)
        {
            CCLog.Log("override back!");
        }
    }
}