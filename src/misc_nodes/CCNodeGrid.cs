using System;

namespace CocosSharp
{
    /// <summary>
    /// CCNodeGrid allows the hosting of a target node that will have a CCGridAction effect applied to it.
    /// </summary>
    public class CCNodeGrid : CCNode
    {

        private CCGridBase grid;

        /// <summary>
        /// Gets or sets the target that the effect is applied to.
        /// </summary>
        /// <value>The target.</value>
        public CCNode Target { get; set; }

        public CCNodeGrid () : base ()
        {
        }

        /// <summary>
        /// Gets or sets the grid object that is used when applying effects.
        /// </summary>
        /// <value>The grid.</value>
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

        public override void Visit (ref CCAffineTransform parentWorldTransform)
        {
            bool renderTarget = CCDrawManager.SharedDrawManager.CurrentRenderTarget != null;

            if ((!Visible || Scene == null) && !renderTarget)
            {
                return;
            }

            var drawManager = DrawManager;

            // Set camera view/proj matrix even if ChildClippingMode is None
            if(Camera != null && !renderTarget)
            {
                drawManager.ViewMatrix = Camera.ViewMatrix;
                drawManager.ProjectionMatrix = Camera.ProjectionMatrix;
            }

            var worldTransform = CCAffineTransform.Identity;
            CCAffineTransform.Concat(ref parentWorldTransform, ref affineLocalTransform, out worldTransform);

            drawManager.PushMatrix();

            if (Grid != null && Grid.Active)
            {
                CCCustomCommand command = new CCCustomCommand(float.MinValue, worldTransform, OnGridBeginDraw);
                Renderer.AddCommand(command);
            }

            if (Target != null)
                Target.Visit (ref worldTransform);

            int i = 0;

            if ((Children != null) && (Children.Count > 0))
            {
                SortAllChildren();

                CCNode[] elements = Children.Elements;
                int count = Children.Count;

                // draw children zOrder < 0
                for (; i < count; ++i)
                {
                    if (elements[i].ZOrder < 0)
                    {
                        // don't break loop on invisible children
                        if (elements[i].Visible)
                        {
                            elements[i].Visit(ref worldTransform);
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                // self draw
                VisitRenderer(ref worldTransform);
                // draw the children
                for (; i < count; ++i)
                {
                    // Draw the z >= 0 order children next.
                    if (elements[i].Visible)
                    {
                        elements[i].Visit(ref worldTransform);
                    }
                }
            }
            else
            {
                // self draw
                VisitRenderer(ref worldTransform);
            }

            if (Grid != null && Grid.Active)
            {
                CCCustomCommand command = new CCCustomCommand(float.MaxValue, worldTransform, OnGridEndDraw);
                Renderer.AddCommand(command);
            }

            drawManager.PopMatrix();

        }

        protected virtual void OnGridBeginDraw ()
        {
            if (Grid != null && Grid.Active)
            {
                Grid.BeforeDraw();
            }
        }

        protected virtual void OnGridEndDraw ()
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

