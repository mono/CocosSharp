using System;
using System.IO;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;

namespace CocosSharp
{
    public class CCScene : CCNode
    {
        CCGameView gameView;


        #region Properties

        public virtual bool IsTransition
        {
            get { return false; }
        }

        public CCRect VisibleBoundsScreenspace
        {
            get { return new CCRect(Viewport.Bounds); }
        }

        public override CCScene Scene
        {
            get { return this; }
        }

        public override CCGameView GameView
        {
            get { return gameView; }
            protected set 
            { 
                gameView = value;

                if(gameView != null)
                    InitializeLazySceneGraph(Children);
            }
        }

        public override CCDirector Director { get { return GameView.Director; } }

        public override CCLayer Layer
        {
            get
            {
                return null;
            }

            internal set
            {
            }
        }

        public override CCCamera Camera 
        { 
            get { return null; }
            set 
            {
            }
        }

        internal override CCEventDispatcher EventDispatcher 
        { 
            get { return GameView != null ? GameView.EventDispatcher : null; }
        }

        public override CCAffineTransform AffineLocalTransform
        {
            get
            {
                return CCAffineTransform.Identity;
            }
        }

        #endregion Properties


        #region Constructors


        public CCScene(CCGameView gameView)
        {
            GameView = gameView;
        }

        public CCScene(CCScene scene)
            : this(scene.GameView)
        {
        }

        #endregion Constructors


        void InitializeLazySceneGraph(CCRawList<CCNode> children)
        {
            if (children == null)
                return;

            foreach (var child in children)
            {
                if (child != null)
                {
                    child.AttachEvents();
                    child.AttachActions();
                    child.AttachSchedules ();
                    InitializeLazySceneGraph(child.Children);
                }
            }
        }

        public void AddLayer(CCLayer layer)
        {
            gameView.ViewportChanged += layer.OnViewportChanged; 
            AddChild(layer);
        }
    }
}