using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CocosSharp
{
    internal enum CCNodeTag
    {
        Invalid = -1,
    };

    /** @brief CCNode is the main element. Anything thats gets drawn or contains things that get drawn is a CCNode.
        The most popular CCNodes are: CCScene, CCLayer, CCSprite, CCMenu.

        The main features of a CCNode are:
        - They can contain other CCNode nodes (addChild, getChildByTag, removeChild, etc)
        - They can schedule periodic callback (schedule, unschedule, etc)
        - They can execute actions (runAction, stopAction, etc)

        Some CCNode nodes provide extra functionality for them or their children.

        Subclassing a CCNode usually means (one/all) of:
        - overriding init to initialize resources and schedule callbacks
        - create callbacks to handle the advancement of time
        - overriding draw to render the node

        Features of CCNode:
        - position
        - scale (x, y)
        - rotation (in degrees, clockwise)
        - CCCamera (an interface to gluLookAt )
        - CCGridBase (to do mesh transformations)
        - anchor point
        - size
        - visible
        - z-order
        - openGL z position

        Default values:
        - rotation: 0
        - position: (x=0,y=0)
        - scale: (x=1,y=1)
        - contentSize: (x=0,y=0)
        - anchorPoint: (x=0,y=0)

        Limitations:
        - A CCNode is a "void" object. It doesn't have a texture

        Order in transformations with grid disabled
        -# The node will be translated (position)
        -# The node will be rotated (rotation)
        -# The node will be scaled (scale)
        -# The node will be moved according to the camera values (camera)

        Order in transformations with grid enabled
        -# The node will be translated (position)
        -# The node will be rotated (rotation)
        -# The node will be scaled (scale)
        -# The grid will capture the screen
        -# The node will be moved according to the camera values (camera)
        -# The grid will render the captured screen

        Camera:
        - Each node has a camera. By default it points to the center of the CCNode.
        */

    public class CCNode : ICCUpdatable, ICCFocusable, IComparer<CCNode>, IComparable<CCNode>
    {
        public const int TagInvalid = -1;                               // Use this to determine if a tag has been set on the node.
        static uint globalOrderOfArrival = 1;

        bool ignoreAnchorPointForPosition;
        bool isCleaned = false;
        bool isOpacityCascaded;
        bool isColorCascaded;

        int tag;
        int zOrder;
        int localZOrder;
        float globalZOrder;

        float rotationX;
        float rotationY;
        float scaleX;
        float scaleY;
        float skewX;
        float skewY;

        // opacity controls
        byte displayedOpacity;
        CCColor3B displayedColor;

        CCPoint anchorPoint;
        CCPoint anchorPointInPoints;
        CCPoint position;

        CCSize contentSize;

        CCPoint3 fauxLocalCameraCenter;
        CCPoint3 fauxLocalCameraTarget;
        CCPoint3 fauxLocalCameraUpDirection;

        Dictionary<int, List<CCNode>> childrenByTag;

        CCGridBase grid;

        CCScene scene;
        CCLayer layer;

        Matrix xnaWorldMatrix;
        CCAffineTransform affineLocalTransform;
        CCAffineTransform additionalTransform;

        List<CCEventListener> toBeAddedListeners;                       // The listeners to be added lazily when an EventDispatcher is not yet available

		struct lazyAction
		{
			public CCAction Action;
			public CCNode Target;
			public bool Paused;

			public lazyAction(CCAction action, CCNode target, bool paused = false)
			{
				Action = action;
				Target = target;
				Paused = paused;
			}
		}

		List<lazyAction> toBeAddedActions;                       // The Actions to be added lazily when an ActionManager is not yet available

        #region Properties

        // Auto-implemented properties

        public bool IsRunning { get; protected set; }
        public virtual bool HasFocus { get; set; }
        public virtual bool Visible { get; set; }
        public virtual bool IsSerializable { get; protected set; }      // If this is true, the screen will be recorded into the director's state

        public virtual float VertexZ { get; set; }
        public object UserData { get; set; }
        public object UserObject { get; set; }
        public string Name { get; set; }
        public CCRawList<CCNode> Children { get; protected set; }
        public CCNode Parent { get; internal set; }

        internal protected uint OrderOfArrival { get; internal set; }

        protected bool IsReorderChildDirty { get; set; }
        protected byte RealOpacity { get; set; }
        protected CCColor3B RealColor { get; set; }

        // Manually implemented properties

        public virtual bool CanReceiveFocus
        {
            get { return Visible; }
        }

        public virtual bool IgnoreAnchorPointForPosition
        {
            get { return ignoreAnchorPointForPosition; }
            set
            {
                if (value != ignoreAnchorPointForPosition)
                {
                    ignoreAnchorPointForPosition = value;
                    UpdateTransform();
                }
            }
        }

        public virtual bool IsOpacityCascaded 
        { 
            get { return isOpacityCascaded; }
            set 
            {
                if (isOpacityCascaded == value)
                    return;

                isOpacityCascaded = value;

                if (isOpacityCascaded)
                {
                    UpdateCascadeOpacity();
                }
                else
                {
                    DisableCascadeOpacity();
                }

            }
        }

        public virtual bool IsColorCascaded 
        { 
            get { return isColorCascaded; }
            set 
            {
                if (isColorCascaded == value)
                    return;

                isColorCascaded = value;

                if (isColorCascaded)
                {
                    UpdateCascadeColor();
                }
                else
                {
                    DisableCascadeColor();
                }


            }
        }

        public virtual bool IsColorModifiedByOpacity
        {
            get { return false; }
            set { }
        }

        public virtual byte Opacity
        {
            get { return RealOpacity; }
            set
            {
                displayedOpacity = RealOpacity = value;

                UpdateCascadeOpacity();
            }
        }

        public byte DisplayedOpacity 
        { 
            get { return displayedOpacity; }
            protected set 
            {
                displayedOpacity = value;
            }
        }

        public int Tag
        {
            get { return tag; }
            set
            {
                if (tag != value)
                {
                    if (Parent != null)
                    {
                        Parent.ChangedChildTag(this, tag, value);
                    }
                    tag = value;
                }
            }
        }

        public int ChildrenCount
        {
            get { return Children == null ? 0 : Children.Count; }
        }

        public int ZOrder
        {
            get { return zOrder; }
            set
            {
                zOrder = value;
                if (Parent != null)
                {
                    Parent.ReorderChild(this, value);
                }
            }
        }

        public int LocalZOrder 
        { 
            get { return localZOrder; }

            set 
            {
                if (localZOrder != value)
                {
                    localZOrder = value;
                    if (Parent != null)
                    {
                        Parent.ReorderChild(this, localZOrder);
                    }

                    if(EventDispatcher != null)
                        EventDispatcher.MarkDirty = this;
                }
            }
        }

        public int NumberOfRunningActions
        {
            get { return ActionManager.NumberOfRunningActionsInTarget (this); }
        }

        public float GlobalZOrder 
        { 
            get { return globalZOrder; }
            set
            {
                if (globalZOrder != value)
                {
                    globalZOrder = value;
                    EventDispatcher.MarkDirty = this;
                }

            }
        }

        public virtual float SkewX
        {
            get { return skewX; }
            set
            {
                skewX = value;
                UpdateTransform();
            }
        }

        public virtual float SkewY
        {
            get { return skewY; }
            set
            {
                skewY = value;
                UpdateTransform();
            }
        }

        // 2D rotation of the node relative to the 0,1 vector in a clock-wise orientation.
        public virtual float Rotation
        {
            set
            {
                rotationX = rotationY = value;
                UpdateTransform();
            }
        }

        public virtual float RotationX
        {
            get { return rotationX; }
            set
            {
                rotationX = value;
                UpdateTransform();
            }
        }

        public virtual float RotationY
        {
            get { return rotationY; }
            set
            {
                rotationY = value;
                UpdateTransform();
            }
        }

        // The general scale that applies to both X and Y directions.
        public virtual float Scale
        {
            set
            {
                scaleX = scaleY = value;
                UpdateTransform();
            }
        }

        public virtual float ScaleX
        {
            get { return scaleX; }
            set
            {
                scaleX = value;
                UpdateTransform();
            }
        }

        public virtual float ScaleY
        {
            get { return scaleY; }
            set
            {
                scaleY = value;
                UpdateTransform();
            }
        }

        public float PositionX
        {
            get { return position.X; }
            set { Position = new CCPoint(value, position.Y); }
        }

        public float PositionY
        {
            get { return position.Y; }
            set { Position = new CCPoint(position.X, value); }
        }

        public CCColor3B DisplayedColor 
        { 
            get { return displayedColor; } 
            protected set 
            { 
                displayedColor = value;
            }
        }

        public virtual CCColor3B Color
        {
            get { return RealColor; }
            set
            {
                displayedColor = RealColor = value;

                UpdateCascadeColor();
            }
        }

        public virtual CCPoint Position
        {
            get { return position; }
            set
            {
                if (position != value) 
                {
                    position = value;
                    UpdateTransform();
                }
            }
        }

        public virtual CCPoint PositionWorldspace
        {
            get 
            {
                CCAffineTransform parentWorldTransform 
                = Parent != null ? Parent.AffineWorldTransform : CCAffineTransform.Identity;

                return parentWorldTransform.Transform(Position);
            }
        }

        // Returns the anchor point in pixels, AnchorPoint * ContentSize. This does not use
        // the scale factor of the node.
        public virtual CCPoint AnchorPointInPoints
        {
            get { return anchorPointInPoints; }
        }

        // Returns the Anchor Point of the node as a value [0,1], where 1 is 100% of the dimension and 0 is 0%.
        public virtual CCPoint AnchorPoint
        {
            get { return anchorPoint; }
            set
            {
                if (!value.Equals(anchorPoint))
                {
                    anchorPoint = value;
                    anchorPointInPoints = new CCPoint(contentSize.Width * anchorPoint.X, contentSize.Height * anchorPoint.Y);
                    UpdateTransform();
                }
            }
        }

        public virtual CCSize ScaledContentSize
        {
            get { return new CCSize(ContentSize.Width * ScaleX, ContentSize.Height * ScaleY); }
        }

        public virtual CCSize ContentSize
        {
            get { return contentSize; }
            set
            {
                if (!CCSize.Equal(ref value, ref contentSize))
                {
                    contentSize = value;
                    anchorPointInPoints = new CCPoint(contentSize.Width * anchorPoint.X, contentSize.Height * anchorPoint.Y);

                    UpdateTransform();
                }
            }
        }

        // Returns the bounding box of this node in parent space
        public CCRect BoundingBox
        {
            get
            {
                CCPoint boundingBoxOrigin = Position;

                if(IgnoreAnchorPointForPosition == false) 
                {
                    boundingBoxOrigin -= AnchorPointInPoints;
                }

                return new CCRect(boundingBoxOrigin.X, boundingBoxOrigin.Y, contentSize.Width, contentSize.Height);
            }
        }

        // Bounding box after scale/rotation/skew in parent space
        public CCRect TransformedBoundingBox
        {
            get 
            { 
                CCAffineTransform localTransform = AffineLocalTransform;
                CCRect transformedBounds = localTransform.Transform(new CCRect(0.0f, 0.0f, contentSize.Width, contentSize.Height));
                return transformedBounds; 
            }
        }

        // Bounding box after scale/rotation/skew in world space
        public CCRect TransformedBoundingBoxWorldspace
        {
            get 
            { 
                CCAffineTransform localTransform = AffineWorldTransform;
                CCRect worldtransformedBounds = localTransform.Transform(new CCRect(0.0f, 0.0f, contentSize.Width, contentSize.Height));
                return worldtransformedBounds; 
            }
        }

        public virtual CCAffineTransform AffineLocalTransform
        {
            get { return affineLocalTransform; }
        }

        public CCAffineTransform AffineWorldTransform
        {
            get 
            {
                CCAffineTransform worldTransform = CCAffineTransform.Identity;
                CCNode parent = this.Parent;
                if (parent != null) 
                {
                    worldTransform = CCAffineTransform.Concat(AffineLocalTransform, parent.AffineWorldTransform);
                }

                return worldTransform;
            }
        }

        public CCAffineTransform AdditionalTransform
        {
            get { return additionalTransform; }
            set 
            {
                if (value != additionalTransform) 
                {
                    additionalTransform = value;
                    UpdateTransform();
                }
            }
        }

        public CCNode this[int tag]
        {
            get { return GetChildByTag(tag); }
        }

        public CCGridBase Grid 
        { 
            get { return grid; }
            set 
            {
                grid = value;
                if(value != null && Scene != null) 
                {
                    grid.Scene = Scene;
                }
            }
        }

        public virtual CCScene Scene
        {
            get { return scene; }
            internal set 
            {
                if(scene != value) 
                {
                    if(scene != null) 
                    {
                        scene.SceneViewportChanged -= OnSceneViewportChanged;
                    }

                    scene = value;

                    // All the children should belong to same scene
                    if (Children != null) 
                    {
                        foreach (CCNode child in Children) 
                        {
                            child.Scene = scene;
                        }
                    }

                    if (grid != null)
                        grid.Scene = scene;

                    if (scene != null) 
                    {
                        scene.SceneViewportChanged += OnSceneViewportChanged;

                        OnSceneViewportChanged(this, null);

                        AddedToScene();

                        AttachActions();
                    }

                    AttachEvents();
                }
            }
        }

        public virtual CCLayer Layer 
        {
            get { return layer; }
            internal set 
            {
                if (layer != value) 
                {
                    if (layer != null) 
                    {
                        layer.LayerVisibleBoundsChanged -= OnLayerVisibleBoundsChanged;
                    }

                    layer = value;

                    // All the children should belong to same layer
                    if (Children != null) 
                    {
                        foreach (CCNode child in Children) 
                        {
                            child.Layer = layer;
                        }
                    }

                    if (layer != null) 
                    {
                        layer.LayerVisibleBoundsChanged += OnLayerVisibleBoundsChanged;

                        OnLayerVisibleBoundsChanged(this, null);
                    }

                    if (layer.Scene != null)
                        this.Scene = layer.Scene;
                }
            }
        }


        public virtual CCApplication Application
        {
            get { return Window != null ? Window.Application : null; }
        }

        public virtual CCDirector Director
        { 
            get { return Scene.Director; }
            set { Scene.Director = value; }
        }

        public virtual CCCamera Camera
        {
            get { return Layer.Camera; }
            set { Layer.Camera = value; }
        }

        public virtual CCWindow Window 
        { 
            get { return Scene != null ? Scene.Window : null; }
            set { Scene.Window = value; }
        }

        public virtual CCViewport Viewport
        {
            get { return Scene != null ? Scene.Viewport : null; }
            set { Scene.Viewport = value; }
        }

        protected Matrix XnaWorldMatrix 
        { 
            get { return xnaWorldMatrix; }
            private set 
            {
                xnaWorldMatrix = value;
            }
        }

        internal virtual CCEventDispatcher EventDispatcher 
        { 
            get { return Scene != null ? Scene.EventDispatcher : null; }
        }

        internal bool EventDispatcherIsEnabled
        {
            get { return EventDispatcher != null ? EventDispatcher.IsEnabled : false; }
            set
            {
                if (EventDispatcher != null)
                    EventDispatcher.IsEnabled = value;
            }
        }

        internal CCPoint3 FauxLocalCameraCenter 
        { 
            get { return fauxLocalCameraCenter; }
            set 
            {
                if (fauxLocalCameraCenter != value) 
                {
                    fauxLocalCameraCenter = value;
                    UpdateTransform();
                }
            }
        }

        internal CCPoint3 FauxLocalCameraTarget
        { 
            get { return fauxLocalCameraTarget; }
            set 
            {
                if (fauxLocalCameraTarget != value) 
                {
                    fauxLocalCameraTarget = value;
                    UpdateTransform();
                }
            }
        }

        internal CCPoint3 FauxLocalCameraUpDirection
        { 
            get { return fauxLocalCameraUpDirection; }
            set 
            {
                if (fauxLocalCameraUpDirection != value) 
                {
                    fauxLocalCameraUpDirection = value;
                    UpdateTransform();
                }
            }
        }

        CCScheduler Scheduler
        {
            get { return Application != null ? Application.Scheduler : null; }
        }

        CCActionManager ActionManager
        {
            get { return Application != null ? Application.ActionManager : null; }
        }

        #endregion Properties


        #region Constructors

        public CCNode()
        {
            additionalTransform = CCAffineTransform.Identity;
            xnaWorldMatrix = Matrix.Identity;
            scaleX = 1.0f;
            scaleY = 1.0f;
            Visible = true;
            tag = TagInvalid;

            HasFocus = false;
            IsSerializable = true;
            IsColorCascaded = false;
            IsOpacityCascaded = false;

            displayedOpacity = 255;
            RealOpacity = 255;
            displayedColor = CCColor3B.White;
            RealColor = CCColor3B.White;

            FauxLocalCameraUpDirection = new CCPoint3(0.0f, 1.0f, 0.0f);
        }

        #endregion Constructors


        #region Event dispatcher handling

        internal void AttachEvents()
        {
			if (EventDispatcher == null)
				return;

            if (toBeAddedListeners != null && toBeAddedListeners.Count > 0) 
            {
                var eventDispatcher = EventDispatcher;
                foreach (var listener in toBeAddedListeners) 
                {
                    if (listener.SceneGraphPriority != null)
                        eventDispatcher.AddEventListener (listener, listener.SceneGraphPriority);
                    else
                        eventDispatcher.AddEventListener (listener, listener.FixedPriority);
                }

                toBeAddedListeners.Clear ();
                toBeAddedListeners = null;
            }
        }

        #endregion Event dispatcher handling


        #region Scene callbacks

        // Hidden event handlers

        void OnSceneViewportChanged (object sender, EventArgs e)
        {
            if (Scene != null && Window != null && Viewport != null && Camera != null) 
            {
                ViewportChanged ();
                VisibleBoundsChanged ();
            }
        }

        void OnLayerVisibleBoundsChanged (object sender, EventArgs e)
        {
            if (Scene != null && Viewport != null && Camera != null)
                VisibleBoundsChanged();
        }

        // Users override methods below to listen to changes to scene

        protected virtual void VisibleBoundsChanged()
        {
        }

        protected virtual void ViewportChanged()
        {
        }

        protected virtual void AddedToScene()
        {
        }

        #endregion Scene callbacks


        #region Cleaning up

        ~CCNode()
        {
            this.Dispose(false);
        }

        public void Dispose()
        {
            this.Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if(disposing) 
            {
                // Dispose of managed resources
            }

            // Want to stop all actions and timers regardless of whether or not this object was explicitly disposed
            this.Cleanup();

            if (EventDispatcher != null)
                EventDispatcher.RemoveEventListeners (this);

            // Clean up the UserData and UserObject as these may hold references to other CCNodes.
            UserData = null;
            UserObject = null;

            if (Children != null && Children.Count > 0)
            {
                CCNode[] elements = Children.Elements;
                foreach(CCNode child in Children.Elements)
                {
                    if (child != null) 
                    {
                        if (!child.isCleaned) {
                            child.OnExit ();
                        }
                        child.Parent = null;
                    }
                }
            }

        }

        protected virtual void ResetCleanState()
        {
            isCleaned = false;
            if (Children != null && Children.Count > 0)
            {
                CCNode[] elements = Children.Elements;
                for (int i = 0, count = Children.Count; i < count; i++)
                {
                    elements[i].ResetCleanState();
                }
            }
        }

        public virtual void Cleanup()
        {
            if (isCleaned == true)
            {
                return;
            }

            // actions
            StopAllActions();

            // timers
            UnscheduleAll();

            if (Children != null && Children.Count > 0)
            {
                CCNode[] elements = Children.Elements;
                for (int i = 0, count = Children.Count; i < count; i++)
                {
                    elements[i].Cleanup();
                }
            }

            isCleaned = true;
        }

        #endregion Cleaning up


        #region Unit conversion

        public CCPoint WorldToParentspace(CCPoint point)
        {
            CCAffineTransform parentWorldTransform 
            = Parent != null ? Parent.AffineWorldTransform : CCAffineTransform.Identity;

            return parentWorldTransform.Transform(point);
        }

        #endregion Unit conversion


        #region Serialization

        public virtual void Serialize(Stream stream) 
        {
            StreamWriter sw = new StreamWriter(stream);
            CCSerialization.SerializeData(Visible, sw);
            CCSerialization.SerializeData(rotationX, sw);
            CCSerialization.SerializeData(rotationY, sw);
            CCSerialization.SerializeData(scaleX, sw);
            CCSerialization.SerializeData(scaleY, sw);
            CCSerialization.SerializeData(skewX, sw);
            CCSerialization.SerializeData(skewY, sw);
            CCSerialization.SerializeData(VertexZ, sw);
            CCSerialization.SerializeData(ignoreAnchorPointForPosition, sw);
            CCSerialization.SerializeData(IsRunning, sw);
            CCSerialization.SerializeData(IsReorderChildDirty, sw);
            CCSerialization.SerializeData(OrderOfArrival, sw);
            CCSerialization.SerializeData(tag, sw);
            CCSerialization.SerializeData(zOrder, sw);
            CCSerialization.SerializeData(anchorPoint, sw);
            CCSerialization.SerializeData(contentSize, sw);
            CCSerialization.SerializeData(Position, sw);
            if (Children != null)
            {
                CCSerialization.SerializeData(Children.Count, sw);
                foreach (CCNode child in Children)
                {
                    sw.WriteLine(child.GetType().AssemblyQualifiedName);
                }
                foreach (CCNode child in Children)
                {
                    child.Serialize(stream);
                }
            }
            else
            {
                CCSerialization.SerializeData(0, sw); // No children
            }
        }

        public virtual void Deserialize(Stream stream) 
        {
            StreamReader sr = new StreamReader(stream);
            Visible = CCSerialization.DeSerializeBool(sr); 
            rotationX = CCSerialization.DeSerializeFloat(sr);
            rotationY = CCSerialization.DeSerializeFloat(sr);
            scaleX = CCSerialization.DeSerializeFloat(sr);
            scaleY = CCSerialization.DeSerializeFloat(sr);
            skewX = CCSerialization.DeSerializeFloat(sr);
            skewY = CCSerialization.DeSerializeFloat(sr);
            VertexZ = CCSerialization.DeSerializeFloat(sr);
            ignoreAnchorPointForPosition = CCSerialization.DeSerializeBool(sr);
            IsRunning = CCSerialization.DeSerializeBool(sr);
            IsReorderChildDirty = CCSerialization.DeSerializeBool(sr);
            OrderOfArrival = (uint)CCSerialization.DeSerializeInt(sr);
            tag = CCSerialization.DeSerializeInt(sr);
            zOrder = CCSerialization.DeSerializeInt(sr);
            AnchorPoint = CCSerialization.DeSerializePoint(sr);
            ContentSize = CCSerialization.DeSerializeSize(sr);
            Position = CCSerialization.DeSerializePoint(sr);
            // m_UserData is handled by the specialized class.
            // TODO: Serializze the action manager
            // TODO :Serialize the grid
            // TODO: Serialize the camera
            string s;
            int count = CCSerialization.DeSerializeInt(sr);
            for (int i = 0; i < count; i++)
            {
                s = sr.ReadLine();
                Type screenType = Type.GetType(s);
                CCNode scene = Activator.CreateInstance(screenType) as CCNode;
                AddChild(scene);
                scene.Deserialize(stream);
            }
        }

        #endregion Serialization

        public CCNode GetChildByTag(int tag)
        {
            Debug.Assert(tag != (int) CCNodeTag.Invalid, "Invalid tag");

            if (childrenByTag != null && childrenByTag.Count > 0)
            {
                Debug.Assert(Children != null && Children.Count > 0);

                List<CCNode> list;
                if (childrenByTag.TryGetValue(tag, out list))
                {
                    if (list.Count > 0)
                    {
                        return list[0];
                    }
                }
            }
            return null;
        }

        #region AddChild

        public void AddChild(CCNode child)
        {
            Debug.Assert(child != null, "Argument must be no-null");
            AddChild(child, child.LocalZOrder, child.Tag);
        }

        public void AddChild(CCNode child, int zOrder)
        {
            Debug.Assert(child != null, "Argument must be no-null");
            AddChild(child, zOrder, child.Tag);
        }

        public virtual void AddChild(CCNode child, int zOrder, int tag)
        {
            Debug.Assert(child != null, "Argument must be non-null");
            Debug.Assert(child.Parent == null, "child already added. It can't be added again");
            Debug.Assert(child != this, "Can not add myself to myself.");

            if (Children == null)
            {
                Children = new CCRawList<CCNode>();
            }

            InsertChild(child, zOrder, tag);

            child.Parent = this;
            child.tag = tag;
            child.OrderOfArrival = globalOrderOfArrival++;
            if (child.isCleaned)
            {
                child.ResetCleanState();
            }

            // Try to setup defaults if need be
            if (child is CCLayer && child.Camera == null && this is CCScene) 
                child.Camera = new CCCamera (this.Window.WindowSizeInPixels);

            // We want all our children to have the same layer as us
            // Set this before we call child.OnEnter
            child.Layer = this.Layer;
            child.Scene = this.Scene;

            if (IsRunning)
            {
                child.OnEnter();
                child.OnEnterTransitionDidFinish();
            }
        }

        void InsertChild(CCNode child, int z, int tag)
        {
            IsReorderChildDirty = true;
            Children.Add(child);

            ChangedChildTag(child, TagInvalid, tag);

            child.zOrder = z;
            child.LocalZOrder = z;
        }

        #endregion AddChild


        #region RemoveChild

        public void RemoveFromParent(bool cleanup=true)
        {
            if (Parent != null)
            {
                Parent.RemoveChild(this, cleanup);
            }
        }

        public virtual void RemoveChild(CCNode child, bool cleanup=true)
        {
            // explicit nil handling
            if (Children == null || child == null)
            {
                return;
            }

            ChangedChildTag(child, child.Tag, TagInvalid);

            if (Children.Contains(child))
            {
                DetachChild(child, cleanup);
            }
        }

        public void RemoveChildByTag(int tag, bool cleanup=true)
        {
            Debug.Assert(tag != (int) CCNodeTag.Invalid, "Invalid tag");

            CCNode child = this[tag];

            if (child == null)
            {
                CCLog.Log("CocosSharp: removeChildByTag: child not found!");
            }
            else
            {
                RemoveChild(child, cleanup);
            }
        }

        public virtual void RemoveAllChildrenByTag(int tag, bool cleanup=true)
        {
            Debug.Assert(tag != (int)CCNodeTag.Invalid, "Invalid tag");
            while (true)
            {
                CCNode child = this[tag];
                if (child == null)
                {
                    break;
                }
                RemoveChild(child, cleanup);
            }
        }

        public virtual void RemoveAllChildren(bool cleanup=true)
        {
            // not using detachChild improves speed here
            if (Children != null && Children.Count > 0)
            {
                if (childrenByTag != null)
                {
                    childrenByTag.Clear();
                }

                CCNode[] elements = Children.Elements;
                for (int i = 0, count = Children.Count; i < count; i++)
                {
                    CCNode node = elements[i];

                    // IMPORTANT:
                    //  -1st do onExit
                    //  -2nd cleanup
                    if (IsRunning)
                    {
                        node.OnExitTransitionDidStart();
                        node.OnExit();
                    }

                    if (cleanup)
                    {
                        node.Cleanup();
                    }

                    // set parent nil at the end
                    node.Parent = null;
                }

                Children.Clear();
            }
        }

        void DetachChild(CCNode child, bool doCleanup)
        {
            // IMPORTANT:
            //  -1st do onExit
            //  -2nd cleanup
            if (IsRunning)
            {
                child.OnExitTransitionDidStart();
                child.OnExit();
            }

            // If you don't do cleanup, the child's actions will not get removed and the
            // its scheduledSelectors_ dict will not get released!
            if (doCleanup)
            {
                child.Cleanup();
            }

            // set parent nil at the end
            child.Parent = null;

            Children.Remove(child);
        }

        #endregion RemoveChild


        #region Child Sorting

        int IComparer<CCNode>.Compare(CCNode n1, CCNode n2)
        {
            if (n1.LocalZOrder < n2.LocalZOrder || (n1.LocalZOrder == n2.LocalZOrder && n1.OrderOfArrival < n2.OrderOfArrival))
            {
                return -1;
            }

            if (n1 == n2)
            {
                return 0;
            }

            return 1;
        }

        public int CompareTo(CCNode that)
        {
            if (this.LocalZOrder < that.LocalZOrder || (this.LocalZOrder == that.LocalZOrder && this.OrderOfArrival < that.OrderOfArrival))
            {
                return -1;
            }

            if (this == that)
            {
                return 0;
            }

            return 1;
        }

        public virtual void SortAllChildren()
        {
            if (IsReorderChildDirty)
            {
                Array.Sort(Children.Elements, 0, Children.Count, this);
                IsReorderChildDirty = false;
            }
        }

        void ChangedChildTag(CCNode child, int oldTag, int newTag)
        {
            List<CCNode> list;

            if (childrenByTag != null && oldTag != TagInvalid)
            {
                if (childrenByTag.TryGetValue(oldTag, out list))
                {
                    list.Remove(child);
                }
            }

            if (newTag != TagInvalid)
            {
                if (childrenByTag == null)
                {
                    childrenByTag = new Dictionary<int, List<CCNode>>();
                }

                if (!childrenByTag.TryGetValue(newTag, out list))
                {
                    list = new List<CCNode>();
                    childrenByTag.Add(newTag, list);
                }

                list.Add(child);
            }
        }

        public virtual void ReorderChild(CCNode child, int zOrder)
        {
            Debug.Assert(child != null, "Child must be non-null");

            IsReorderChildDirty = true;
            child.OrderOfArrival = globalOrderOfArrival++;
            child.zOrder = zOrder;
            child.LocalZOrder = zOrder;
        }

        #endregion Child Sorting


        #region Events and Listeners

        /// <summary>
        /// Adds a event listener for a specified event with the priority of scene graph.
        /// The priority of scene graph will be fixed value 0. So the order of listener item
        /// in the vector will be ' <0, scene graph (0 priority), >0'.
        /// </summary>
        /// <param name="listener">The listener of a specified event.</param>
        /// <param name="node">The priority of the listener is based on the draw order of this node.</param>
        public void AddEventListener(CCEventListener listener, CCNode node = null)
        {

            if (node == null)
                node = this;

            if (EventDispatcherIsEnabled)
            {
                EventDispatcher.AddEventListener(listener, node);
            }
            else
            {
                if (toBeAddedListeners == null)
                    toBeAddedListeners = new List<CCEventListener>();

                listener.SceneGraphPriority = node;
                toBeAddedListeners.Add(listener);
            }
        }

        /// <summary>
        /// Adds a event listener for a specified event with the fixed priority.
        /// A lower priority will be called before the ones that have a higher value.
        /// 0 priority is not allowed for fixed priority since it's used for scene graph based priority.
        /// </summary>
        /// <param name="listener">The listener of a specified event.</param>
        /// <param name="fixedPriority">The fixed priority of the listener.</param>
        public void AddEventListener(CCEventListener listener, int fixedPriority)
        {
            if (EventDispatcherIsEnabled)
            {
                EventDispatcher.AddEventListener(listener, fixedPriority);
            }
            else
            {
                if (toBeAddedListeners == null)
                    toBeAddedListeners = new List<CCEventListener>();

                listener.FixedPriority = fixedPriority;
                toBeAddedListeners.Add(listener);
            }
        }

        /// <summary>
        /// Adds a Custom event listener.
        /// It will use a fixed priority of 1.
        /// </summary>
        /// <returns>The generated event. Needed in order to remove the event from the dispather.</returns>
        /// <param name="eventName">Event name.</param>
        /// <param name="callback">Callback.</param>
        public CCEventListenerCustom AddCustomEventListener(string eventName, Action<CCEventCustom> callback)
        {
            var listener = new CCEventListenerCustom(eventName, callback);
            AddEventListener(listener, 1);
            return listener;
        }

        /// <summary>
        /// Remove a listener
        /// </summary>
        /// <param name="listener">The specified event listener which needs to be removed.</param>
        public void RemoveEventListener(CCEventListener listener)
        {
            if (EventDispatcherIsEnabled)
                EventDispatcher.RemoveEventListener(listener);

            if (toBeAddedListeners != null && toBeAddedListeners.Contains(listener))
                toBeAddedListeners.Remove(listener);
        }

        /// <summary>
        /// Removes all listeners with the same event listener type
        /// </summary>
        /// <param name="listenerType"></param>
        public void RemoveEventListeners(CCEventListenerType listenerType)
        {
            if (EventDispatcher != null)
                EventDispatcher.RemoveEventListeners(listenerType);

            if (toBeAddedListeners != null)
            {

                var listenerID = string.Empty;
                switch (listenerType) 
                {
                case CCEventListenerType.TOUCH_ONE_BY_ONE:
                    listenerID = CCEventListenerTouchOneByOne.LISTENER_ID;
                    break;
                case CCEventListenerType.TOUCH_ALL_AT_ONCE:
                    listenerID = CCEventListenerTouchAllAtOnce.LISTENER_ID;
                    break;
                case CCEventListenerType.MOUSE:
                    listenerID = CCEventListenerMouse.LISTENER_ID;
                    break;
                case CCEventListenerType.ACCELEROMETER:
                    listenerID = CCEventListenerAccelerometer.LISTENER_ID;
                    break;
                case CCEventListenerType.KEYBOARD:
                    listenerID = CCEventListenerKeyboard.LISTENER_ID;
                    break;
                case CCEventListenerType.GAMEPAD:
                    listenerID = CCEventListenerGamePad.LISTENER_ID;
                    break;

                default:
                    Debug.Assert (false, "Invalid listener type!");
                    break;
                }

                for (int i = 0; i < toBeAddedListeners.Count; i++)
                {
                    if (toBeAddedListeners[i].ListenerID == listenerID)
                    {
                        toBeAddedListeners.RemoveAt(i);
                    }
                }

                if (toBeAddedListeners.Count == 0)
                    toBeAddedListeners = null;

            }
        }

        /// <summary>
        /// Removes all listeners which are associated with the specified target.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="recursive"></param>
        public void RemoveEventListeners(CCNode target, bool recursive = false)
        {
            if (EventDispatcher != null)
                EventDispatcher.RemoveEventListeners(target, recursive);

            if (toBeAddedListeners != null)
            {
                for (int i = 0; i < toBeAddedListeners.Count; i++)
                {
                    if (toBeAddedListeners[i].SceneGraphPriority == target)
                    {
                        toBeAddedListeners.RemoveAt(i);
                    }
                }

                if (toBeAddedListeners.Count == 0)
                    toBeAddedListeners = null;

            }
        }

        /// <summary>
        /// Removes all listeners which are associated with this node.
        /// </summary>
        /// <param name="recursive"></param>
        public void RemoveEventListeners(bool recursive = false)
        {
            RemoveEventListeners(this, recursive);
        }

        /// <summary>
        /// Removes all listeners
        /// </summary>
        public void RemoveAllListeners()
        {
            if (EventDispatcher != null)
                EventDispatcher.RemoveAll();

            if (toBeAddedListeners != null)
            {
                toBeAddedListeners.Clear();
                toBeAddedListeners = null;
            }
        }

        /// <summary>
        /// Pauses all listeners which are associated the specified target.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="recursive"></param>
        public void PauseListeners(CCNode target, bool recursive = false)
        {
            if (EventDispatcher != null)
                EventDispatcher.Pause(target, recursive);
        }

        /// <summary>
        /// Pauses all listeners which are associated the specified this node.
        /// </summary>
        /// <param name="recursive"></param>
        public void PauseListeners(bool recursive = false)
        {
            PauseListeners(this, recursive);
        }

        /// <summary>
        /// Resumes all listeners which are associated the specified target.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="recursive"></param>
        public void ResumeListeners(CCNode target, bool recursive = false)
        {
            if (EventDispatcher != null)
                EventDispatcher.Resume(target, recursive);

        }

        /// <summary>
        /// Resumes all listeners which are associated the this node.
        /// </summary>
        /// <param name="recursive"></param>
        public void ResumeListeners(bool recursive = false)
        {
            ResumeListeners(this, recursive);
        }

        /// <summary>
        /// Sets listener's priority with fixed value.
        /// </summary>
        /// <param name="listener"></param>
        /// <param name="fixedPriority"></param>
        public void SetListenerPriority(CCEventListener listener, int fixedPriority)
        {
            if (EventDispatcherIsEnabled)
                EventDispatcher.SetPriority(listener, fixedPriority);

            if (toBeAddedListeners != null && toBeAddedListeners.Contains(listener))
            {
                var found = toBeAddedListeners.IndexOf(listener);
                toBeAddedListeners[found].FixedPriority = fixedPriority;
            }
        }

        /// <summary>
        /// Convenience method to dispatch a custom event
        /// </summary>
        /// <param name="eventToDispatch"></param>
        public void DispatchEvent(string customEvent, object userData = null)
        {
            if (EventDispatcherIsEnabled)
                EventDispatcher.DispatchEvent(customEvent, userData);

        }

        /// <summary>
        /// Dispatches the event
        /// Also removes all EventListeners marked for deletion from the event dispatcher list.
        /// </summary>
        /// <param name="eventToDispatch"></param>
        public void DispatchEvent(CCEvent eventToDispatch)
        {
            if (EventDispatcherIsEnabled)
                EventDispatcher.DispatchEvent(eventToDispatch);
        }


        #endregion Events and Listeners

        public virtual void Update(float dt)
        {
        }

        protected virtual void Draw()
        {
        }

        // This is called with every call to the MainLoop on the CCDirector class. In XNA, this is the same as the Draw() call.
        public virtual void Visit()
        {
            if (!Visible || Scene == null)
            {
                return;
            }

            Window.DrawManager.PushMatrix();

            if (Grid != null && Grid.Active)
            {
                Window.DrawManager.SetIdentityMatrix();
                Grid.BeforeDraw();
            }
            else
            {
                Transform();
            }

            int i = 0;

            if ((Children != null) && (Children.Count > 0))
            {
                SortAllChildren();

                CCNode[] elements = Children.Elements;
                int count = Children.Count;

                // draw children zOrder < 0
                for (; i < count; ++i)
                {
                    if (elements[i].Visible && elements[i].zOrder < 0)
                    {
                        elements[i].Visit();
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
                    if (elements[i].Visible/* && elements[i].m_nZOrder >= 0*/)
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
                Grid.AfterDraw(this);
                Window.DrawManager.MultMatrix(AffineWorldTransform, VertexZ);
                Grid.Blit();
            }

            Window.DrawManager.PopMatrix();
        }

        public void Transform()
        {
            Window.DrawManager.MultMatrix(ref xnaWorldMatrix);
        }

        public void TransformAncestors()
        {
            if (Parent != null)
            {
                Parent.TransformAncestors();
                Parent.Transform();
            }
        }


        #region Color and Opacity

        protected internal virtual void UpdateDisplayedOpacity(byte parentOpacity)
        {
            displayedOpacity = (byte) (RealOpacity * parentOpacity / 255.0f);

            UpdateColor();

            if (IsOpacityCascaded && Children != null)
            {
                foreach(CCNode node in Children)
                {
                    node.UpdateDisplayedOpacity(DisplayedOpacity);
                }
            }
        }

        protected internal virtual void UpdateCascadeOpacity ()
        {
            byte parentOpacity = 255;
            var pParent = Parent;
            if (pParent != null && pParent.IsOpacityCascaded)
            {
                parentOpacity = pParent.DisplayedOpacity;
            }
            UpdateDisplayedOpacity(parentOpacity);

        }

        protected virtual void DisableCascadeOpacity()
        {
            DisplayedOpacity = RealOpacity;

            foreach(CCNode node in Children.Elements)
            {
                node.UpdateDisplayedOpacity(255);
            }
        }

        public virtual void UpdateColor()
        {
            // Override the opdate of color here
        }

        public virtual void UpdateDisplayedColor(CCColor3B parentColor)
        {
            displayedColor.R = (byte)(RealColor.R * parentColor.R / 255.0f);
            displayedColor.G = (byte)(RealColor.G * parentColor.G / 255.0f);
            displayedColor.B = (byte)(RealColor.B * parentColor.B / 255.0f);

            UpdateColor();

            if (IsColorCascaded)
            {
                if (IsOpacityCascaded && Children != null)
                {
                    foreach(CCNode node in Children)
                    {
                        if (node != null)
                        {
                            node.UpdateDisplayedColor(DisplayedColor);
                        }
                    }
                }
            }
        }

        protected internal void UpdateCascadeColor()
        {
            var parentColor = CCColor3B.White;
            if (Parent != null && Parent.IsColorCascaded)
            {
                parentColor = Parent.DisplayedColor;
            }

            UpdateDisplayedColor(parentColor);
        }

        protected internal void DisableCascadeColor()
        {
            if (Children == null)
                return;

            foreach (var child in Children)
            {
                child.UpdateDisplayedColor(CCColor3B.White);
            }
        }

        #endregion Color and Opacity


        #region Entering and exiting

        public virtual void OnEnter()
        {

            if (Children != null && Children.Count > 0)
            {
                CCNode[] elements = Children.Elements;
                for (int i = 0, count = Children.Count; i < count; i++)
                {
                    elements[i].OnEnter();
                }
            }

            Resume();

            IsRunning = true;

        }

        public virtual void OnEnterTransitionDidFinish()
        {
            if (Children != null && Children.Count > 0)
            {
                CCNode[] elements = Children.Elements;
                for (int i = 0, count = Children.Count; i < count; i++)
                {
                    elements[i].OnEnterTransitionDidFinish();
                }
            }
        }

        public virtual void OnExitTransitionDidStart()
        {
            if (Children != null && Children.Count > 0)
            {
                CCNode[] elements = Children.Elements;
                for (int i = 0, count = Children.Count; i < count; i++)
                {
                    elements[i].OnExitTransitionDidStart();
                }
            }
        }

        public virtual void OnExit()
        {
            Pause();

            IsRunning = false;

            if (Children != null && Children.Count > 0)
            {
                CCNode[] elements = Children.Elements;
                for (int i = 0, count = Children.Count; i < count; i++)
                {
                    elements[i].OnExit();
                }
            }
        }

        #endregion Entering and exiting


        #region Actions

		internal void AttachActions()
		{
			if (toBeAddedActions != null && toBeAddedActions.Count > 0) 
			{
				var actionManger = ActionManager;
				foreach (var action in toBeAddedActions) 
				{
					ActionManager.AddAction(action.Action, action.Target, action.Paused);
				}

				toBeAddedActions.Clear ();
				toBeAddedActions = null;
			}
		}

		CCActionState AddLazyAction (CCAction action, CCNode target, bool paused = false)
		{
			if (toBeAddedActions == null)
				toBeAddedActions = new List<lazyAction>();

			toBeAddedActions.Add(new lazyAction(action, target, paused));
			return null;
		}

        public void AddAction(CCAction action, bool paused = false)
        {
			if (ActionManager != null)
				ActionManager.AddAction(action, this, paused);
			else
				AddLazyAction(action, this, paused);
        }

        public void AddActions(bool paused, params CCFiniteTimeAction[] actions)
        {
			if (ActionManager != null)
				ActionManager.AddAction(new CCSequence(actions), this, paused);
			else
				AddLazyAction(new CCSequence(actions), this, paused);
        }

        public CCActionState Repeat(uint times, params CCFiniteTimeAction[] actions)
        {
            return RunAction (new CCRepeat (new CCSequence(actions), times));
        }

        public CCActionState Repeat (uint times, CCFiniteTimeAction action)
        {
            return RunAction (new CCRepeat (action, times));
        }

        public CCActionState RepeatForever(params CCFiniteTimeAction[] actions)
        {
            return RunAction(new CCRepeatForever (actions));
        }

        public CCActionState RepeatForever(CCFiniteTimeAction action)
        {
            return RunAction(new CCRepeatForever (action) { Tag = action.Tag });
        }

        public CCActionState RunAction(CCAction action)
        {
            Debug.Assert(action != null, "Argument must be non-nil");

            return ActionManager != null ? ActionManager.AddAction(action, this, !IsRunning) : AddLazyAction(action, this, !IsRunning);
        }

        public CCActionState RunActions(params CCFiniteTimeAction[] actions)
        {
            Debug.Assert(actions != null, "Argument must be non-nil");
			Debug.Assert(actions.Length > 0, "Paremeter: actions has length of zero. At least one action must be set to run.");
			var action = actions.Length > 1 ? new CCSequence(actions) : actions[0];

            return ActionManager != null ? ActionManager.AddAction (action, this, !IsRunning) : AddLazyAction(action, this, !IsRunning);
        }

        public void StopAllActions()
        {
            if(ActionManager != null)
                ActionManager.RemoveAllActionsFromTarget(this);
        }

        public void StopAction(CCActionState actionState)
        {
            if(ActionManager != null)
                ActionManager.RemoveAction(actionState);
        }

        public void StopAction(int tag)
        {
            Debug.Assert(tag != (int) CCNodeTag.Invalid, "Invalid tag");
            ActionManager.RemoveAction(tag, this);
        }

        public CCAction GetAction(int tag)
        {
            Debug.Assert(tag != (int) CCNodeTag.Invalid, "Invalid tag");
            return ActionManager.GetAction(tag, this);
        }

        public CCActionState GetActionState(int tag)
        {
            Debug.Assert(tag != (int) CCNodeTag.Invalid, "Invalid tag");
            return ActionManager.GetActionState(tag, this);
        }

        #endregion Actions


        #region Scheduling

        public void Schedule()
        {
            Schedule(0);
        }

        public void Schedule(int priority)
        {
            Scheduler.Schedule(this, priority, !IsRunning);
        }

        public void Unschedule ()
        {
            Scheduler.Unschedule (this);
        }

        public void Schedule (Action<float> selector)
        {
            Schedule (selector, 0.0f, CCSchedulePriority.RepeatForever, 0.0f);
        }

        public void Schedule (Action<float> selector, float interval)
        {
            Schedule (selector, interval, CCSchedulePriority.RepeatForever, 0.0f);
        }

        public void Schedule (Action<float> selector, float interval, uint repeat, float delay)
        {
            Debug.Assert (selector != null, "Argument must be non-nil");
            Debug.Assert (interval >= 0, "Argument must be positive");

            Scheduler.Schedule (selector, this, interval, repeat, delay, !IsRunning);
        }

        public void ScheduleOnce (Action<float> selector, float delay)
        {
            Schedule (selector, 0.0f, 0, delay);
        }

        public void Unschedule (Action<float> selector)
        {
            // explicit nil handling
            if (selector == null)
                return;

            Scheduler.Unschedule (selector, this);
        }

        public void UnscheduleAll ()
        {
            if(Scheduler != null)
                Scheduler.UnscheduleAll(this);
        }

        public void Resume()
        {
            Scheduler.Resume(this);
            ActionManager.ResumeTarget(this);
            EventDispatcher.Resume(this);
        }

        public void Pause()
        {
            Scheduler.PauseTarget(this);
            ActionManager.PauseTarget(this);
            if (EventDispatcher != null)
                EventDispatcher.Pause (this);
        }

        #endregion Scheduling


        #region Transformations

        protected virtual void UpdateTransform()
        {
            // Translate values
            float x = position.X;
            float y = position.Y;

            affineLocalTransform = CCAffineTransform.Identity;

            if (ignoreAnchorPointForPosition)
            {
                x += anchorPointInPoints.X;
                y += anchorPointInPoints.Y;
            }

            // Rotation values
            // Change rotation code to handle X and Y
            // If we skew with the exact same value for both x and y then we're simply just rotating
            float cx = 1, sx = 0, cy = 1, sy = 0;
            if (rotationX != 0 || rotationY != 0)
            {
                float radiansX = -CCMacros.CCDegreesToRadians(rotationX);
                float radiansY = -CCMacros.CCDegreesToRadians(rotationY);
                cx = (float)Math.Cos(radiansX);
                sx = (float)Math.Sin(radiansX);
                cy = (float)Math.Cos(radiansY);
                sy = (float)Math.Sin(radiansY);
            }

            bool needsSkewMatrix = (skewX != 0f || skewY != 0f);

            // optimization:
            // inline anchor point calculation if skew is not needed
            if (!needsSkewMatrix && !anchorPointInPoints.Equals(CCPoint.Zero))
            {
                x += cy * -anchorPointInPoints.X * scaleX + -sx * -anchorPointInPoints.Y * scaleY;
                y += sy * -anchorPointInPoints.X * scaleX + cx * -anchorPointInPoints.Y * scaleY;
            }

            // Build Transform Matrix
            // Adjusted transform calculation for rotational skew
            affineLocalTransform.A = cy * scaleX;
            affineLocalTransform.B = sy * scaleX;
            affineLocalTransform.C = -sx * scaleY;
            affineLocalTransform.D = cx * scaleY;
            affineLocalTransform.Tx = x;
            affineLocalTransform.Ty = y;

            // XXX: Try to inline skew
            // If skew is needed, apply skew and then anchor point
            if (needsSkewMatrix)
            {
                var skewMatrix = new CCAffineTransform(
                    1.0f, (float) Math.Tan(CCMacros.CCDegreesToRadians(skewY)),
                    (float) Math.Tan(CCMacros.CCDegreesToRadians(skewX)), 1.0f,
                    0.0f, 0.0f);

                affineLocalTransform = CCAffineTransform.Concat(skewMatrix, affineLocalTransform);

                // adjust anchor point
                if (!anchorPointInPoints.Equals(CCPoint.Zero))
                {
                    affineLocalTransform = CCAffineTransform.Translate(affineLocalTransform,
                        -anchorPointInPoints.X,
                        -anchorPointInPoints.Y);
                }
            }

            affineLocalTransform = CCAffineTransform.Concat(additionalTransform, affineLocalTransform);


            Matrix fauxLocalCameraTransform = Matrix.Identity;

            if(FauxLocalCameraCenter != FauxLocalCameraTarget)
            {
                fauxLocalCameraTransform =  Matrix.CreateLookAt(
                    new Vector3(FauxLocalCameraCenter.X, FauxLocalCameraCenter.Y, FauxLocalCameraCenter.Z),
                    new Vector3(FauxLocalCameraTarget.X, FauxLocalCameraTarget.Y, FauxLocalCameraTarget.Z),
                    new Vector3(FauxLocalCameraUpDirection.X, FauxLocalCameraUpDirection.Y, FauxLocalCameraUpDirection.Z));
                fauxLocalCameraTransform *= Matrix.CreateTranslation(new Vector3 (AnchorPointInPoints.X, AnchorPointInPoints.Y, VertexZ));
            }

            affineLocalTransform = CCAffineTransform.Concat(new CCAffineTransform(fauxLocalCameraTransform), affineLocalTransform);


            XnaWorldMatrix = affineLocalTransform.XnaMatrix;
        }

        #endregion Transformations


        public virtual void KeyBackClicked()
        {
        }

        public virtual void KeyMenuClicked()
        {
        }
    }
}