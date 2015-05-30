using System;

namespace CocosSharp
{
    // CCNodeGrid allows the hosting of a target node that will have a CCGridAction effect applied to it.
    public class CCNodeGrid : CCNode
    {
        CCGridBase grid;


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

        public CCNodeGrid () : base ()
        {
        }

        public CCNodeGrid (CCSize contentSize) : base (contentSize)
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
                CCCustomCommand command = new CCCustomCommand(float.MinValue, worldTransform, OnGridBeginDraw);
                Renderer.PushGroup();
                Renderer.AddCommand(command);
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
                CCCustomCommand command = new CCCustomCommand(float.MaxValue, worldTransform, OnGridEndDraw);
                Renderer.AddCommand(command);
                Renderer.PopGroup();
            }
        }

        protected void OnGridBeginDraw ()
        {
            if (Grid != null && Grid.Active)
            {
                Grid.BeforeDraw();
            }
        }

        protected void OnGridEndDraw ()
        {
            if (Grid != null && Grid.Active)
            {
                Grid.AfterDraw(this);
                DrawManager.SetIdentityMatrix();
                Grid.Blit();
            }

        }
    }
}

