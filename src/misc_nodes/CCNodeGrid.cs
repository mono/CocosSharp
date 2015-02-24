using System;

namespace CocosSharp
{
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
        public new CCGridBase Grid 
        { 
            get { return grid; }
            set 
            {
                grid = value;
                if(value != null) 
                {
                    grid.Scene = Scene;
                    grid.Layer = Layer;
                }
            }
        }

        public override void Visit ()
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


            drawManager.PushMatrix();

            if (Grid != null && Grid.Active)
            {
                OnGridBeginDraw ();
            }

            Transform (drawManager);

            if (Target != null)
                Target.Visit ();

            int i = 0;

            if ((Children != null) && (Children.Count > 0))
            {
                SortAllChildren();

                CCNode[] elements = Children.Elements;
                int count = Children.Count;

                // draw children zOrder < 0
                for (; i < count; ++i)
                {
                    if (elements[i].LocalZOrder < 0)
                    {
                        // don't break loop on invisible children
                        if (elements[i].Visible)
                        {
                            elements[i].Visit();
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                // self draw
                Draw();
                // draw the children
                for (; i < count; ++i)
                {
                    // Draw the z >= 0 order children next.
                    if (elements[i].Visible)
                    {
                        elements[i].Visit();
                    }
                }
            }
            else
            {
                // self draw
                Draw();
            }

            if (Grid != null && Grid.Active)
            {
                OnGridEndDraw();
                //drawManager.SetIdentityMatrix();
                //Grid.Blit();
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

