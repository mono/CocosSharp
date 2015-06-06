using System;

namespace CocosSharp
{
    // CCNodeGrid allows the hosting of a target node that will have a CCGridAction effect applied to it.
    public class CCNodeGrid : CCNode
    {
        CCGridBase grid;
        CCCustomCommand renderGrid;
        CCCustomCommand renderBeginGrid;
        CCCustomCommand renderEndGrid;


        #region Properties

        public CCNode Target { get; set; }

        public CCGridBase Grid 
        { 
            get { return grid; }
            set 
            {
                if (grid != null)
                    grid.Dispose();
                
                grid = value;
                if(value != null) 
                {
                    grid.Scene = Scene;
                    grid.Layer = Layer;
                }
            }
        }

        #endregion Properties


        #region Constructors

        public CCNodeGrid () 
            : base ()
        {
            renderGrid = new CCCustomCommand(RenderGrid);
            renderBeginGrid = new CCCustomCommand(float.MinValue, OnGridBeginDraw);
            renderEndGrid = new CCCustomCommand(float.MaxValue, OnGridEndDraw);
        }

        public CCNodeGrid (CCSize contentSize) 
            : base (contentSize)
        {
        }

        #endregion Constructors


        public override void Visit (ref CCAffineTransform parentWorldTransform)
        {
            if (!Visible || Scene == null)
                return;

            var worldTransform = CCAffineTransform.Identity;
            var affineLocalTransform = AffineLocalTransform;
            CCAffineTransform.Concat(ref affineLocalTransform, ref parentWorldTransform, out worldTransform);


            if (Grid != null && Grid.Active)
            {
                renderGrid.GlobalDepth = worldTransform.Tz;
                Renderer.AddCommand(renderGrid);

                renderBeginGrid.WorldTransform = worldTransform;

                Renderer.PushGroup();
                Renderer.AddCommand(renderBeginGrid);
            }

            SortAllChildren();

            VisitRenderer(ref worldTransform);

            if(Children != null)
            {
                var elements = Children.Elements;
                for(int i = 0, N = Children.Count; i < N; ++i)
                {
                    var child = elements[i];
                    if (child.Visible)
                        child.Visit(ref worldTransform);
                }
            }

            if (Grid != null && Grid.Active)
            {
                renderEndGrid.GlobalDepth = worldTransform.Tz;
                Renderer.AddCommand(renderEndGrid);
                Renderer.PopGroup();
            }
        }

        void OnGridBeginDraw ()
        {
            if (Grid != null && Grid.Active)
                Grid.BeforeDraw();
        }

        void OnGridEndDraw ()
        {
            if (Grid != null && Grid.Active)
                Grid.AfterDraw(this);
        }

        void RenderGrid ()
        {
            if (Grid != null && Grid.Active)
                Grid.Blit();
        }
    }
}

