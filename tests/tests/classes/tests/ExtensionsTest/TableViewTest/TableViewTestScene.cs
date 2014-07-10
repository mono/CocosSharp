using System;
using CocosSharp;

namespace tests.Extensions
{
    public class TableViewTestLayer : CCLayer, ICCTableViewDataSource, ICCTableViewDelegate
    {

        public TableViewTestLayer()
        {

            InitTableViewTestLayer();
        }

        public static void runTableViewTest()
        {
            var pScene = new CCScene(AppDelegate.SharedWindow, AppDelegate.SharedCamera, AppDelegate.SharedViewport, AppDelegate.SharedDirector);
            var pLayer = new TableViewTestLayer();
            pScene.AddChild(pLayer);
            AppDelegate.SharedDirector.ReplaceScene(pScene);
        }

        private bool InitTableViewTestLayer()
        {

            var winSize = Scene.VisibleBoundsWorldspace.Size;

            var tableView = new CCTableView(this, new CCSize(250, 60));
            tableView.Direction = CCScrollViewDirection.Horizontal;
            tableView.Position = new CCPoint(20, winSize.Height / 2 - 30);
            tableView.Delegate = this;
            this.AddChild(tableView);
            tableView.ReloadData();

            tableView = new CCTableView(this, new CCSize(60, 280));
            tableView.Direction = CCScrollViewDirection.Vertical;
            tableView.Position = new CCPoint(winSize.Width - 150, winSize.Height / 2 - 120);
            tableView.Delegate = this;
            tableView.VerticalFillOrder = CCTableViewVerticalFillOrder.FillTopDown;
            this.AddChild(tableView);
            tableView.ReloadData();

            // Back Menu
            var itemBack = new CCMenuItemFont("Back", toExtensionsMainLayer);
            itemBack.Position = new CCPoint(winSize.Width - 50, 25);
            var menuBack = new CCMenu(itemBack);
            menuBack.Position = CCPoint.Zero;
            AddChild(menuBack);

            return true;
        }

        public void toExtensionsMainLayer(object sender)
        {
            var pScene = new ExtensionsTestScene();
            pScene.runThisTest();
        }

        //CREATE_FUNC(TableViewTestLayer);

        public virtual void ScrollViewDidScroll(CCScrollView view)
        {
        }

        public virtual void ScrollViewDidZoom(CCScrollView view)
        {
        }

        public virtual void TableCellTouched(CCTableView table, CCTableViewCell cell)
        {
            CCLog.Log("cell touched at index: {0}", cell.Index);
        }

        public void TableCellHighlight(CCTableView table, CCTableViewCell cell)
        {
        }

        public void TableCellUnhighlight(CCTableView table, CCTableViewCell cell)
        {
        }

        public void TableCellWillRecycle(CCTableView table, CCTableViewCell cell)
        {
        }

        public CCSize TableCellSizeForIndex(CCTableView table, int idx)
        {
            if (idx == 2)
            {
                return new CCSize(100, 100);
            }
            return new CCSize(60, 60);
        }

        public virtual CCTableViewCell TableCellAtIndex(CCTableView table, int idx)
        {
            string str = idx.ToString();
            var cell = table.DequeueCell();

            if (cell == null)
            {
                cell = new CustomTableViewCell();
                var sprite = new CCSprite("Images/Icon");
                sprite.AnchorPoint = CCPoint.Zero;
                sprite.Position = new CCPoint(0, 0);
                cell.AddChild(sprite);

                var label = new CCLabelTtf(str, "Helvetica", 20.0f);
                label.Position = CCPoint.Zero;
                label.AnchorPoint = CCPoint.Zero;
                label.Tag = 123;
                cell.AddChild(label);
            }
            else
            {
                var label = (CCLabelTtf) cell.GetChildByTag(123);
                label.Text = (str);
            }


            return cell;
        }

        public virtual int NumberOfCellsInTableView(CCTableView table)
        {
            return 20;
        }
    }
}