using System;
using System.Diagnostics;
using System.IO;

namespace Cocos2D
{
    internal enum NodeTag
    {
        kCCNodeTagInvalid = -1,
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

    public class CCNode : SelectorProtocol, CCIFocusable
    {
        private static int kCCNodeTagInvalid = -1;
        private static int s_globalOrderOfArrival = 1;
        protected bool m_bIgnoreAnchorPointForPosition;

        // transform
        public CCAffineTransform m_tTransform;
        protected bool m_bIsInverseDirty;
        protected bool m_bIsRunning;
        public bool m_bIsTransformDirty;
        protected bool m_bIsVisible;
        protected bool m_bReorderChildDirty;
        protected float m_fRotation;
        protected float m_fScaleX;
        protected float m_fScaleY;

        //protected int m_nScriptHandler;

        protected float m_fSkewX;
        protected float m_fSkewY;
        protected float m_fVertexZ;
        internal protected int m_nOrderOfArrival;
        protected int m_nTag;
        internal int m_nZOrder;
        protected CCActionManager m_pActionManager;
        protected CCCamera m_pCamera;
        protected RawList<CCNode> m_pChildren;
        protected CCGridBase m_pGrid;
        protected CCNode m_pParent;
        protected CCScheduler m_pScheduler;

        protected object m_pUserData;
        protected CCPoint m_tAnchorPoint;
        protected CCPoint m_tAnchorPointInPoints;
        protected CCSize m_tContentSize;
        private CCAffineTransform m_tInverse;
        protected CCPoint m_tPosition;
        private bool m_bHasFocus = false;

        public CCNode()
        {
            m_fScaleX = 1.0f;
            m_fScaleY = 1.0f;
            m_bIsVisible = true;
            m_nTag = kCCNodeTagInvalid;

            m_tTransform = CCAffineTransform.Identity;
            m_bIsInverseDirty = true;

            // set default scheduler and actionManager
            CCDirector director = CCDirector.SharedDirector;
            m_pActionManager = director.ActionManager;
            m_pScheduler = director.Scheduler;
        }

#if ANDROID
        /// <summary>
        /// Sets all of the sprite font labels as dirty so they redraw. This is necessary
        /// in Android when the application resumes from the background.
        /// </summary>
        public virtual void DirtyLabels()
        {
            if (Children == null || Children.Count == 0)
            {
                return;
            }
            foreach (CCNode node in Children)
            {
                if (node == null)
                {
                    continue;
                }
                if (node is CCLabelTTF)
                {
                    ((CCLabelTTF)node).Dirty = true;
                    ((CCLabelTTF)node).Refresh();
                }
                node.DirtyLabels();
            }
        }
#endif
        #region Game State Management
        /// <summary>
        /// Gets whether or not this scene is serializable. If this is true,
        /// the screen will be recorded into the director's state and
        /// its Serialize and Deserialize methods will be called as appropriate.
        /// If this is false, the screen will be ignored during serialization.
        /// By default, all screens are assumed to be serializable.
        /// </summary>
        public virtual bool IsSerializable
        {
            get { return m_isSerializable; }
            protected set { m_isSerializable = value; }
        }

        private bool m_isSerializable = true;

        /// <summary>
        /// Tells the screen to serialize its state into the given stream.
        /// </summary>
        public virtual void Serialize(Stream stream) 
        {
            StreamWriter sw = new StreamWriter(stream);
            CCSerialization.SerializeData(m_bIsVisible, sw);
            CCSerialization.SerializeData(m_fRotation, sw);
            CCSerialization.SerializeData(m_fScaleX, sw);
            CCSerialization.SerializeData(m_fScaleY, sw);
            CCSerialization.SerializeData(m_fSkewX, sw);
            CCSerialization.SerializeData(m_fSkewY, sw);
            CCSerialization.SerializeData(m_fVertexZ, sw);
            CCSerialization.SerializeData(m_bIgnoreAnchorPointForPosition, sw);
            CCSerialization.SerializeData(m_bIsInverseDirty, sw);
            CCSerialization.SerializeData(m_bIsRunning, sw);
            CCSerialization.SerializeData(m_bIsTransformDirty, sw);
            CCSerialization.SerializeData(m_bReorderChildDirty, sw);
            CCSerialization.SerializeData(m_nOrderOfArrival, sw);
            CCSerialization.SerializeData(m_nTag, sw);
            CCSerialization.SerializeData(m_nZOrder, sw);
            CCSerialization.SerializeData(m_tAnchorPoint, sw);
            CCSerialization.SerializeData(m_tContentSize, sw);
            CCSerialization.SerializeData(Position, sw);
            if (m_pChildren != null)
            {
                CCSerialization.SerializeData(m_pChildren.Count, sw);
                foreach (CCNode child in m_pChildren)
                {
                    sw.WriteLine(child.GetType().AssemblyQualifiedName);
                }
                foreach (CCNode child in m_pChildren)
                {
                    child.Serialize(stream);
                }
            }
            else
            {
                CCSerialization.SerializeData(0, sw); // No children
            }
        }

        /// <summary>
        /// Tells the screen to deserialize its state from the given stream.
        /// </summary>
        public virtual void Deserialize(Stream stream) 
        {
            StreamReader sr = new StreamReader(stream);
            m_bIsVisible = CCSerialization.DeSerializeBool(sr); 
            m_fRotation = CCSerialization.DeSerializeFloat(sr);
            m_fScaleX = CCSerialization.DeSerializeFloat(sr);
            m_fScaleY = CCSerialization.DeSerializeFloat(sr);
            m_fSkewX = CCSerialization.DeSerializeFloat(sr);
            m_fSkewY = CCSerialization.DeSerializeFloat(sr);
            m_fVertexZ = CCSerialization.DeSerializeFloat(sr);
            m_bIgnoreAnchorPointForPosition = CCSerialization.DeSerializeBool(sr);
            m_bIsInverseDirty = CCSerialization.DeSerializeBool(sr);
            m_bIsRunning = CCSerialization.DeSerializeBool(sr);
            m_bIsTransformDirty = CCSerialization.DeSerializeBool(sr);
            m_bReorderChildDirty = CCSerialization.DeSerializeBool(sr);
            m_nOrderOfArrival = CCSerialization.DeSerializeInt(sr);
            m_nTag = CCSerialization.DeSerializeInt(sr);
            m_nZOrder = CCSerialization.DeSerializeInt(sr);
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


        #endregion

        #region CCIFocusable
        public virtual bool HasFocus
        {
            get { return (m_bHasFocus); }
            set { m_bHasFocus = value; }
        }
        public virtual bool CanReceiveFocus
        {
            get
            {
                return (Visible);
            }
        }
        #endregion

        public int Tag
        {
            get { return m_nTag; }
            set { m_nTag = value; }
        }

        public object UserData
        {
            get { return m_pUserData; }
            set { m_pUserData = value; }
        }

        public object UserObject { get; set; }

        public virtual float SkewX
        {
            get { return m_fSkewX; }
            set
            {
                m_fSkewX = value;
                m_bIsTransformDirty = m_bIsInverseDirty = true;
            }
        }

        public virtual float SkewY
        {
            get { return m_fSkewY; }
            set
            {
                m_fSkewY = value;
                m_bIsTransformDirty = m_bIsInverseDirty = true;
            }
        }

        public int ZOrder
        {
            get { return m_nZOrder; }
            set
            {
                m_nZOrder = value;
                if (m_pParent != null)
                {
                    m_pParent.ReorderChild(this, value);
                }
            }
        }

        public virtual float VertexZ
        {
            get { return m_fVertexZ; }
            set { m_fVertexZ = value; }
        }

        /// <summary>
        /// 2D rotation of the node relative to the 0,1 vector in a clock-wise orientation.
        /// </summary>
        public virtual float Rotation
        {
            get { return m_fRotation; }
            set
            {
                m_fRotation = value;
                m_bIsTransformDirty = m_bIsInverseDirty = true;
            }
        }

        /// <summary>
        /// The general scale that applies to both X and Y directions.
        /// </summary>
        public virtual float Scale
        {
            get
            {
                Debug.Assert(m_fScaleX == m_fScaleY, "CCNode#scale. ScaleX != ScaleY. Don't know which one to return");
                return m_fScaleX;
            }
            set
            {
                m_fScaleX = m_fScaleY = value;
                m_bIsTransformDirty = m_bIsInverseDirty = true;
            }
        }

        /// <summary>
        /// Scale of the node in the X direction (left to right)
        /// </summary>
        public virtual float ScaleX
        {
            get { return m_fScaleX; }
            set
            {
                m_fScaleX = value;
                m_bIsTransformDirty = m_bIsInverseDirty = true;
            }
        }

        /// <summary>
        /// Scale of the node in the Y direction (top to bottom)
        /// </summary>
        public virtual float ScaleY
        {
            get { return m_fScaleY; }
            set
            {
                m_fScaleY = value;
                m_bIsTransformDirty = m_bIsInverseDirty = true;
            }
        }

        /// <summary>
        /// Sets and gets the position of the node. For Menus, this is the center of the menu. For layers,
        /// this is the lower left corner of the layer.
        /// </summary>
        public virtual CCPoint Position
        {
            get { return m_tPosition; }
            set
            {
                m_tPosition = value;
                m_bIsTransformDirty = m_bIsInverseDirty = true;
            }
        }

        public float PositionX
        {
            get { return m_tPosition.X; }
            set { SetPosition(value, m_tPosition.Y); }
        }

        public float PositionY
        {
            get { return m_tPosition.Y; }
            set { SetPosition(m_tPosition.X, value); }
        }

        public RawList<CCNode> Children
        {
            get { return m_pChildren; }
        }

        public int ChildrenCount
        {
            get { return m_pChildren == null ? 0 : m_pChildren.count; }
        }

        public CCCamera Camera
        {
            get { return m_pCamera ?? (m_pCamera = new CCCamera()); }
        }

        public CCGridBase Grid
        {
            get { return m_pGrid; }
            set { m_pGrid = value; }
        }

        public virtual bool Visible
        {
            get { return m_bIsVisible; }
            set { m_bIsVisible = value; }
        }

        /// <summary>
        /// Returns the anchor point in pixels, AnchorPoint * ContentSize. This does not use
        /// the scale factor of the node.
        /// </summary>
        public virtual CCPoint AnchorPointInPoints
        {
            get { return m_tAnchorPointInPoints; }
        }

        /// <summary>
        /// returns the Anchor Point of the node as a value [0,1], where 1 is 100% of the dimension and 0 is 0%.
        /// </summary>
        public virtual CCPoint AnchorPoint
        {
            get { return m_tAnchorPoint; }
            set
            {
                if (!value.Equals(m_tAnchorPoint))
                {
                    m_tAnchorPoint = value;
                    m_tAnchorPointInPoints = new CCPoint(m_tContentSize.Width * m_tAnchorPoint.X,
                                                                  m_tContentSize.Height * m_tAnchorPoint.Y);
                    m_bIsTransformDirty = m_bIsInverseDirty = true;
                }
            }
        }

        /// <summary>
        /// Returns the content size with the scale applied.
        /// </summary>
        public virtual CCSize ContentSizeInPixels
        {
            get { 
                CCSize size = new CCSize(ContentSize.Width * ScaleX, ContentSize.Height * ScaleY);
                return (size);
            }
        }

        public virtual CCSize ContentSize
        {
            get { return m_tContentSize; }
            set
            {
                if (!CCSize.CCSizeEqualToSize(value, m_tContentSize))
                {
                    m_tContentSize = value;
                    m_tAnchorPointInPoints = new CCPoint(m_tContentSize.Width * m_tAnchorPoint.X,
                                                                  m_tContentSize.Height * m_tAnchorPoint.Y);
                    m_bIsTransformDirty = m_bIsInverseDirty = true;
                }
            }
        }

        public bool IsRunning
        {
            // read only
            get { return m_bIsRunning; }
        }

        public CCNode Parent
        {
            get { return m_pParent; }
            set { m_pParent = value; }
        }

        public virtual bool IgnoreAnchorPointForPosition
        {
            get { return m_bIgnoreAnchorPointForPosition; }
            set
            {
                if (value != m_bIgnoreAnchorPointForPosition)
                {
                    m_bIgnoreAnchorPointForPosition = value;
                    m_bIsTransformDirty = m_bIsInverseDirty = true;
                }
            }
        }

        public CCRect BoundingBox
        {
            get
            {
                var rect = new CCRect(0, 0, m_tContentSize.Width, m_tContentSize.Height);
                return CCAffineTransform.CCRectApplyAffineTransform(rect, NodeToParentTransform());
            }
        }

        public CCRect BoundingBoxInPixels
        {
            get
            {
                var rect = new CCRect(0, 0, ContentSizeInPixels.Width, ContentSizeInPixels.Height);
                return CCAffineTransform.CCRectApplyAffineTransform(rect, NodeToParentTransform());
            }
        }


        #region SelectorProtocol Members

        public virtual void Update(float dt)
        {
        }

        #endregion

        ~CCNode()
        {
            //unregisterScriptHandler();
            Cleanup();
        }

        public void GetPosition(out float x, out float y)
        {
            x = m_tPosition.X;
            y = m_tPosition.Y;
        }

        public void SetPosition(float x, float y)
        {
            m_tPosition.X = x;
            m_tPosition.Y = y;
            m_bIsTransformDirty = m_bIsInverseDirty = true;
        }

        public virtual void Cleanup()
        {
            // actions
            StopAllActions();

            // timers
            UnscheduleAllSelectors();

            if (m_pChildren != null && m_pChildren.count > 0)
            {
                CCNode[] elements = m_pChildren.Elements;
                for (int i = 0, count = m_pChildren.count; i < count; i++)
                {
                    elements[i].Cleanup();
                }
            }
        }

        public CCNode GetChildByTag(int tag)
        {
            Debug.Assert(tag != (int) NodeTag.kCCNodeTagInvalid, "Invalid tag");

            if (m_pChildren != null && m_pChildren.count > 0)
            {
                CCNode[] elements = m_pChildren.Elements;
                for (int i = 0, count = m_pChildren.count; i < count; i++)
                {
                    if (elements[i].m_nTag == tag)
                    {
                        return elements[i];
                    }
                }
            }

            return null;
        }

        public void AddChild(CCNode child)
        {
            Debug.Assert(child != null, "Argument must be no-null");
            AddChild(child, child.ZOrder, child.Tag);
        }

        public void AddChild(CCNode child, int zOrder)
        {
            Debug.Assert(child != null, "Argument must be no-null");
            AddChild(child, zOrder, child.Tag);
        }

        public virtual void AddChild(CCNode child, int zOrder, int tag)
        {
            Debug.Assert(child != null, "Argument must be non-null");
            Debug.Assert(child.m_pParent == null, "child already added. It can't be added again");

            if (m_pChildren == null)
            {
                m_pChildren = new RawList<CCNode>();
            }

            InsertChild(child, zOrder);

            child.m_nTag = tag;
            child.Parent = this;
            child.m_nOrderOfArrival = s_globalOrderOfArrival++;

            if (m_bIsRunning)
            {
                child.OnEnter();
                child.OnEnterTransitionDidFinish();
            }
        }

        public void RemoveFromParentAndCleanup(bool cleanup)
        {
            if (m_pParent != null)
            {
                m_pParent.RemoveChild(this, cleanup);
            }
        }

        public virtual void RemoveChild(CCNode child, bool cleanup)
        {
            // explicit nil handling
            if (m_pChildren == null)
            {
                return;
            }

            if (m_pChildren.Contains(child))
            {
                DetachChild(child, cleanup);
            }
        }

        public void RemoveChildByTag(int tag, bool cleanup)
        {
            Debug.Assert(tag != (int) NodeTag.kCCNodeTagInvalid, "Invalid tag");

            CCNode child = GetChildByTag(tag);

            if (child == null)
            {
                CCLog.Log("cocos2d: removeChildByTag: child not found!");
            }
            else
            {
                RemoveChild(child, cleanup);
            }
        }

        public virtual void RemoveAllChildrenWithCleanup(bool cleanup)
        {
            // not using detachChild improves speed here
            if (m_pChildren != null)
            {
                CCNode[] elements = m_pChildren.Elements;
                for (int i = 0, count = m_pChildren.count; i < count; i++)
                {
                    CCNode node = elements[i];

                    // IMPORTANT:
                    //  -1st do onExit
                    //  -2nd cleanup
                    if (m_bIsRunning)
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

                m_pChildren.Clear();
            }
        }

        private void DetachChild(CCNode child, bool doCleanup)
        {
            // IMPORTANT:
            //  -1st do onExit
            //  -2nd cleanup
            if (m_bIsRunning)
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

            m_pChildren.Remove(child);
        }

        private void InsertChild(CCNode child, int z)
        {
            m_bReorderChildDirty = true;
            m_pChildren.Add(child);
            //child.m_nOrderOfArrival = s_globalOrderOfArrival++;
            child.m_nZOrder = z;
        }

        public virtual void ReorderChild(CCNode child, int zOrder)
        {
            Debug.Assert(child != null, "Child must be non-null");

            m_bReorderChildDirty = true;
            child.m_nOrderOfArrival = s_globalOrderOfArrival++;
            child.m_nZOrder = zOrder;
        }

        public virtual void SortAllChildren()
        {
            if (m_bReorderChildDirty)
            {
                int i;
                int length = m_pChildren.count;
                CCNode[] x = m_pChildren.Elements;

                // insertion sort
                for (i = 1; i < length; i++)
                {
                    CCNode tempItem = x[i];
                    int j = i - 1;

                    //continue moving element downwards while zOrder is smaller or when zOrder is the same but mutatedIndex is smaller
                    while (j >= 0 &&
                           (tempItem.m_nZOrder < x[j].m_nZOrder ||
                            (tempItem.m_nZOrder == x[j].m_nZOrder && tempItem.m_nOrderOfArrival < x[j].m_nOrderOfArrival)))
                    {
                        x[j + 1] = x[j];
                        j = j - 1;
                    }
                    x[j + 1] = tempItem;
                }

                //don't need to check children recursively, that's done in visit of each child

                m_bReorderChildDirty = false;
            }
        }

        /// <summary>
        /// This is called from the Visit() method. This is where you DRAW your node. Only
        /// draw stuff from this method call.
        /// </summary>
        public virtual void Draw()
        {
            // Does nothing in the root node class.
        }

        /// <summary>
        /// This is called with every call to the MainLoop on the CCDirector class. In XNA, this is the same as the Draw() call.
        /// </summary>
        public virtual void Visit()
        {
            // quick return if not visible. children won't be drawn.
            if (!m_bIsVisible)
            {
                return;
            }

            CCDrawManager.PushMatrix();

            if (m_pGrid != null && m_pGrid.Active)
            {
                m_pGrid.BeforeDraw();
                //TransformAncestors();
            }
            else
            {
                Transform();
            }

            int i = 0;

            if ((m_pChildren != null) && (m_pChildren.count > 0))
            {
                SortAllChildren();

                CCNode[] elements = m_pChildren.Elements;
                int count = m_pChildren.count;

                // draw children zOrder < 0
                for (; i < count; ++i)
                {
                    if (elements[i].Visible && elements[i].m_nZOrder < 0)
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

            m_nOrderOfArrival = 0;

            if (m_pGrid != null && m_pGrid.Active)
            {
                m_pGrid.AfterDraw(this);
                Transform();
                m_pGrid.Blit();
            }

            //kmGLPopMatrix();
            CCDrawManager.PopMatrix();
        }

        public void TransformAncestors()
        {
            if (m_pParent != null)
            {
                m_pParent.TransformAncestors();
                m_pParent.Transform();
            }
        }

        public void Transform()
        {
            CCDrawManager.MultMatrix(NodeToParentTransform(), m_fVertexZ);

            // XXX: Expensive calls. Camera should be integrated into the cached affine matrix
            if (m_pCamera != null && !(m_pGrid != null && m_pGrid.Active))
            {
                bool translate = (m_tAnchorPointInPoints.X != 0.0f || m_tAnchorPointInPoints.Y != 0.0f);

                if (translate)
                {
                    CCDrawManager.Translate(m_tAnchorPointInPoints.X, m_tAnchorPointInPoints.Y, 0);
                }

                m_pCamera.Locate();

                if (translate)
                {
                    CCDrawManager.Translate(-m_tAnchorPointInPoints.X, -m_tAnchorPointInPoints.Y, 0);
                }
            }
        }

        public virtual void OnEnter()
        {
            if (m_pChildren != null && m_pChildren.count > 0)
            {
                CCNode[] elements = m_pChildren.Elements;
                for (int i = 0, count = m_pChildren.count; i < count; i++)
                {
                    elements[i].OnEnter();
                }
            }

            ResumeSchedulerAndActions();

            m_bIsRunning = true;

            /*
            if (m_nScriptHandler)
            {
                CCScriptEngineManager::sharedManager()->getScriptEngine()->executeFunctionWithIntegerData(m_nScriptHandler, kCCNodeOnEnter);
            }
            */
        }

        public virtual void OnEnterTransitionDidFinish()
        {
            if (m_pChildren != null && m_pChildren.count > 0)
            {
                CCNode[] elements = m_pChildren.Elements;
                for (int i = 0, count = m_pChildren.count; i < count; i++)
                {
                    elements[i].OnEnterTransitionDidFinish();
                }
            }
        }

        public virtual void OnExitTransitionDidStart()
        {
            if (m_pChildren != null && m_pChildren.count > 0)
            {
                CCNode[] elements = m_pChildren.Elements;
                for (int i = 0, count = m_pChildren.count; i < count; i++)
                {
                    elements[i].OnExitTransitionDidStart();
                }
            }
        }

        public virtual void OnExit()
        {
            PauseSchedulerAndActions();

            m_bIsRunning = false;

            /*
            if (m_nScriptHandler)
            {
                CCScriptEngineManager::sharedManager()->getScriptEngine()->executeFunctionWithIntegerData(m_nScriptHandler, kCCNodeOnExit);
            }
            */

            if (m_pChildren != null && m_pChildren.count > 0)
            {
                CCNode[] elements = m_pChildren.Elements;
                for (int i = 0, count = m_pChildren.count; i < count; i++)
                {
                    elements[i].OnExit();
                }
            }
        }

        #region Actions

        public CCActionManager ActionManager
        {
            get { return m_pActionManager; }
            set
            {
                if (value != m_pActionManager)
                {
                    StopAllActions();
                    m_pActionManager = value;
                }
            }
        }

        public CCAction RunAction(CCAction action)
        {
            Debug.Assert(action != null, "Argument must be non-nil");
            m_pActionManager.AddAction(action, this, !m_bIsRunning);
            return action;
        }

        public void StopAllActions()
        {
            m_pActionManager.RemoveAllActionsFromTarget(this);
        }

        public void StopAction(CCAction action)
        {
            m_pActionManager.RemoveAction(action);
        }

        public void StopActionByTag(int tag)
        {
            Debug.Assert(tag != (int) NodeTag.kCCNodeTagInvalid, "Invalid tag");
            m_pActionManager.RemoveActionByTag(tag, this);
        }

        public CCAction GetActionByTag(int tag)
        {
            Debug.Assert(tag != (int) NodeTag.kCCNodeTagInvalid, "Invalid tag");
            return m_pActionManager.GetActionByTag(tag, this);
        }

        public int NumberOfRunningActions()
        {
            return m_pActionManager.NumberOfRunningActionsInTarget(this);
        }

        #endregion

        #region CCNode - Callbacks

        public CCScheduler Scheduler
        {
            get { return m_pScheduler; }
            set
            {
                if (value != m_pScheduler)
                {
                    UnscheduleAllSelectors();
                    m_pScheduler = value;
                }
            }
        }

        public void ScheduleUpdate()
        {
            ScheduleUpdateWithPriority(0);
        }

        public void ScheduleUpdateWithPriority(int priority)
        {
            m_pScheduler.ScheduleUpdateForTarget(this, priority, !m_bIsRunning);
        }

        public void UnscheduleUpdate()
        {
            m_pScheduler.UnscheduleUpdateForTarget(this);
        }

        public void Schedule(SEL_SCHEDULE selector)
        {
            Schedule(selector, 0.0f, CCScheduler.kCCRepeatForever, 0.0f);
        }

        public void Schedule(SEL_SCHEDULE selector, float interval)
        {
            Schedule(selector, interval, CCScheduler.kCCRepeatForever, 0.0f);
        }

        public void Schedule(SEL_SCHEDULE selector, float interval, uint repeat, float delay)
        {
            Debug.Assert(selector != null, "Argument must be non-nil");
            Debug.Assert(interval >= 0, "Argument must be positive");

            m_pScheduler.ScheduleSelector(selector, this, interval, !m_bIsRunning, repeat, delay);
        }

        public void ScheduleOnce(SEL_SCHEDULE selector, float delay)
        {
            Schedule(selector, 0.0f, 0, delay);
        }

        public void Unschedule(SEL_SCHEDULE selector)
        {
            // explicit nil handling
            if (selector == null)
                return;

            m_pScheduler.UnscheduleSelector(selector, this);
        }

        public void UnscheduleAllSelectors()
        {
            m_pScheduler.UnscheduleAllSelectorsForTarget(this);
        }

        public void ResumeSchedulerAndActions()
        {
            m_pScheduler.ResumeTarget(this);
            m_pActionManager.ResumeTarget(this);
        }

        public void PauseSchedulerAndActions()
        {
            m_pScheduler.PauseTarget(this);
            m_pActionManager.PauseTarget(this);
        }

        #endregion

        #region Transformations

        public virtual CCAffineTransform NodeToParentTransform()
        {
            if (m_bIsTransformDirty)
            {
                // Translate values
                float x = m_tPosition.X;
                float y = m_tPosition.Y;

                if (m_bIgnoreAnchorPointForPosition)
                {
                    x += m_tAnchorPointInPoints.X;
                    y += m_tAnchorPointInPoints.Y;
                }

                // Rotation values
                float c = 1, s = 0;
                if (m_fRotation != 0.0f)
                {
                    float radians = -CCMacros.CCDegreesToRadians(m_fRotation);
                    c = (float) Math.Cos(radians);
                    s = (float) Math.Sin(radians);
                }

                bool needsSkewMatrix = (m_fSkewX != 0f || m_fSkewY != 0f);


                // optimization:
                // inline anchor point calculation if skew is not needed
                if (!needsSkewMatrix && !m_tAnchorPointInPoints.Equals(CCPoint.Zero))
                {
                    x += c * -m_tAnchorPointInPoints.X * m_fScaleX + -s * -m_tAnchorPointInPoints.Y * m_fScaleY;
                    y += s * -m_tAnchorPointInPoints.X * m_fScaleX + c * -m_tAnchorPointInPoints.Y * m_fScaleY;
                }

                // Build Transform Matrix
                //m_tTransform = new CCAffineTransform(
                //    c * m_fScaleX, s * m_fScaleX,
                //    -s * m_fScaleY, c * m_fScaleY,
                //    x, y);

                // Build Transform Matrix
                m_tTransform.a = c * m_fScaleX;
                m_tTransform.b = s * m_fScaleX;
                m_tTransform.c = -s * m_fScaleY;
                m_tTransform.d = c * m_fScaleY;
                m_tTransform.tx = x;
                m_tTransform.ty = y;

                // XXX: Try to inline skew
                // If skew is needed, apply skew and then anchor point
                if (needsSkewMatrix)
                {
                    var skewMatrix = new CCAffineTransform(
                        1.0f, (float) Math.Tan(CCMacros.CCDegreesToRadians(m_fSkewY)),
                        (float) Math.Tan(CCMacros.CCDegreesToRadians(m_fSkewX)), 1.0f,
                        0.0f, 0.0f);

                    m_tTransform = CCAffineTransform.CCAffineTransformConcat(skewMatrix, m_tTransform);

                    // adjust anchor point
                    if (!m_tAnchorPointInPoints.Equals(CCPoint.Zero))
                    {
                        m_tTransform = CCAffineTransform.CCAffineTransformTranslate(m_tTransform,
                                                                                    -m_tAnchorPointInPoints.X,
                                                                                    -m_tAnchorPointInPoints.Y);
                    }
                }

                m_bIsTransformDirty = false;
            }

            return m_tTransform;
        }

        public CCAffineTransform ParentToNodeTransform()
        {
            if (m_bIsInverseDirty)
            {
                m_tInverse = CCAffineTransform.CCAffineTransformInvert(NodeToParentTransform());
                m_bIsInverseDirty = false;
            }
            return m_tInverse;
        }

        public CCAffineTransform NodeToWorldTransform()
        {
            CCAffineTransform t = NodeToParentTransform();

            CCNode p = m_pParent;
            while (p != null)
            {
                t.Concat(p.NodeToParentTransform());
                p = p.Parent;
            }

            return t;
        }

        public CCAffineTransform WorldToNodeTransform()
        {
            return CCAffineTransform.CCAffineTransformInvert(NodeToWorldTransform());
        }

        #endregion

        #region ConvertToSpace

        public CCPoint ConvertToNodeSpace(CCPoint worldPoint)
        {
            return CCAffineTransform.CCPointApplyAffineTransform(worldPoint, WorldToNodeTransform());
        }

        public CCPoint ConvertToWorldSpace(CCPoint nodePoint)
        {
            return CCAffineTransform.CCPointApplyAffineTransform(nodePoint, NodeToWorldTransform());
        }

        public CCPoint ConvertToNodeSpaceAr(CCPoint worldPoint)
        {
            CCPoint nodePoint = ConvertToNodeSpace(worldPoint);
            return nodePoint - m_tAnchorPointInPoints;
        }

        public CCPoint ConvertToWorldSpaceAr(CCPoint nodePoint)
        {
            CCPoint pt = nodePoint + m_tAnchorPointInPoints;
            return ConvertToWorldSpace(pt);
        }

        public CCPoint ConvertToWindowSpace(CCPoint nodePoint)
        {
            CCPoint worldPoint = ConvertToWorldSpace(nodePoint);
            return CCDirector.SharedDirector.ConvertToUi(worldPoint);
        }

        public CCPoint ConvertTouchToNodeSpace(CCTouch touch)
        {
            CCPoint point = touch.LocationInView;
            point = CCDirector.SharedDirector.ConvertToGl(point);
            return ConvertToNodeSpace(point);
        }

        public CCPoint ConvertTouchToNodeSpaceAr(CCTouch touch)
        {
            CCPoint point = touch.LocationInView;
            point = CCDirector.SharedDirector.ConvertToGl(point);
            return ConvertToNodeSpaceAr(point);
        }

        #endregion

        /*
        void registerScriptHandler(int nHandler)
        {
            unregisterScriptHandler();
            m_nScriptHandler = nHandler;
            LUALOG("[LUA] Add CCNode event handler: %d", m_nScriptHandler);
        }

        void unregisterScriptHandler(void)
        {
            if (m_nScriptHandler)
            {
                CCScriptEngineManager::sharedManager().getScriptEngine().removeLuaHandler(m_nScriptHandler);
                LUALOG("[LUA] Remove CCNode event handler: %d", m_nScriptHandler);
                m_nScriptHandler = 0;
            }
        }
        */
    }
}