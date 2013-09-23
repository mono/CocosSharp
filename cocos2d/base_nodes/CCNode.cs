using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Microsoft.Xna.Framework.Input;

namespace Cocos2D
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

    public class CCNode : ICCSelectorProtocol, ICCFocusable, ICCTargetedTouchDelegate, ICCStandardTouchDelegate, ICCKeypadDelegate, ICCKeyboardDelegate, IComparer<CCNode>
    {
        /// <summary>
        /// Use this to determine if a tag has been set on the node.
        /// </summary>
        public const int kCCNodeTagInvalid = -1;

        private static uint s_globalOrderOfArrival = 1;
        protected bool m_bIgnoreAnchorPointForPosition;

        // transform
        public CCAffineTransform m_sTransform;
        protected bool m_bInverseDirty;
        protected bool m_bRunning;
        public bool m_bTransformDirty;
        protected bool m_bVisible;
        protected bool m_bReorderChildDirty;
        protected float m_fRotationX;
        protected float m_fRotationY;
        protected float m_fScaleX;
        protected float m_fScaleY;

        //protected int m_nScriptHandler;

        protected float m_fSkewX;
        protected float m_fSkewY;
        protected float m_fVertexZ;
        internal protected uint m_uOrderOfArrival;
        private int m_nTag;
        internal int m_nZOrder;
        protected CCActionManager m_pActionManager;
        protected CCCamera m_pCamera;
        protected CCRawList<CCNode> m_pChildren;
        protected Dictionary<int, List<CCNode>> m_pChildrenByTag;
        protected CCGridBase m_pGrid;
        protected CCNode m_pParent;
        protected CCScheduler m_pScheduler;

        protected object m_pUserData;
        protected CCPoint m_obAnchorPoint;
        protected CCPoint m_obAnchorPointInPoints;
        protected CCSize m_obContentSize;
        private CCAffineTransform m_sInverse;
        protected CCPoint m_obPosition;
        private bool m_bHasFocus = false;

        private bool m_bAdditionalTransformDirty;
        private CCAffineTransform m_sAdditionalTransform;

        private string m_sName;

        // input variables
        private bool m_bKeypadEnabled;
		private bool m_bKeyboardEnabled;
        private bool m_bGamePadEnabled;
        private bool m_bTouchEnabled;
        private CCTouchMode m_eTouchMode = CCTouchMode.OneByOne;
		private CCKeyboardMode m_eKeyboardMode = CCKeyboardMode.All;
        private int m_nTouchPriority;
        private bool m_bGamePadDelegatesInited;

        public enum CCTouchMode
        {
            AllAtOnce,
            OneByOne
        }

        public CCNode()
        {
            m_fScaleX = 1.0f;
            m_fScaleY = 1.0f;
            m_bVisible = true;
            m_nTag = kCCNodeTagInvalid;

            m_sTransform = CCAffineTransform.Identity;
            m_bInverseDirty = true;

            // set default scheduler and actionManager
            CCDirector director = CCDirector.SharedDirector;
            m_pActionManager = director.ActionManager;
            m_pScheduler = director.Scheduler;

        }

        public virtual bool Init()
        {
          return true;
        }

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
            CCSerialization.SerializeData(m_bVisible, sw);
            CCSerialization.SerializeData(m_fRotationX, sw);
            CCSerialization.SerializeData(m_fRotationY, sw);
            CCSerialization.SerializeData(m_fScaleX, sw);
            CCSerialization.SerializeData(m_fScaleY, sw);
            CCSerialization.SerializeData(m_fSkewX, sw);
            CCSerialization.SerializeData(m_fSkewY, sw);
            CCSerialization.SerializeData(m_fVertexZ, sw);
            CCSerialization.SerializeData(m_bIgnoreAnchorPointForPosition, sw);
            CCSerialization.SerializeData(m_bInverseDirty, sw);
            CCSerialization.SerializeData(m_bRunning, sw);
            CCSerialization.SerializeData(m_bTransformDirty, sw);
            CCSerialization.SerializeData(m_bReorderChildDirty, sw);
            CCSerialization.SerializeData(m_uOrderOfArrival, sw);
            CCSerialization.SerializeData(m_nTag, sw);
            CCSerialization.SerializeData(m_nZOrder, sw);
            CCSerialization.SerializeData(m_obAnchorPoint, sw);
            CCSerialization.SerializeData(m_obContentSize, sw);
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
            m_bVisible = CCSerialization.DeSerializeBool(sr); 
            m_fRotationX = CCSerialization.DeSerializeFloat(sr);
            m_fRotationY = CCSerialization.DeSerializeFloat(sr);
            m_fScaleX = CCSerialization.DeSerializeFloat(sr);
            m_fScaleY = CCSerialization.DeSerializeFloat(sr);
            m_fSkewX = CCSerialization.DeSerializeFloat(sr);
            m_fSkewY = CCSerialization.DeSerializeFloat(sr);
            m_fVertexZ = CCSerialization.DeSerializeFloat(sr);
            m_bIgnoreAnchorPointForPosition = CCSerialization.DeSerializeBool(sr);
            m_bInverseDirty = CCSerialization.DeSerializeBool(sr);
            m_bRunning = CCSerialization.DeSerializeBool(sr);
            m_bTransformDirty = CCSerialization.DeSerializeBool(sr);
            m_bReorderChildDirty = CCSerialization.DeSerializeBool(sr);
            m_uOrderOfArrival = (uint)CCSerialization.DeSerializeInt(sr);
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
            set
            {
                if (m_nTag != value)
                {
                    if (Parent != null)
                    {
                        Parent.ChangedChildTag(this, m_nTag, value);
                    }
                    m_nTag = value;
                }
            }
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
                m_bTransformDirty = m_bInverseDirty = true;
            }
        }

        public virtual float SkewY
        {
            get { return m_fSkewY; }
            set
            {
                m_fSkewY = value;
                m_bTransformDirty = m_bInverseDirty = true;
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
            get
            {
                Debug.Assert(m_fRotationX == m_fRotationY, "CCNode#rotation. RotationX != RotationY. Don't know which one to return");
                return m_fRotationX;
            }
            set
            {
                m_fRotationX = m_fRotationY = value;
                m_bTransformDirty = m_bInverseDirty = true;
            }
        }

        public virtual float RotationX
        {
            get { return m_fRotationX; }
            set
            {
                m_fRotationX = value;
                m_bTransformDirty = m_bInverseDirty = true;
            }
        }

        public virtual float RotationY
        {
            get { return m_fRotationY; }
            set
            {
                m_fRotationY = value;
                m_bTransformDirty = m_bInverseDirty = true;
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
                m_bTransformDirty = m_bInverseDirty = true;
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
                m_bTransformDirty = m_bInverseDirty = true;
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
                m_bTransformDirty = m_bInverseDirty = true;
            }
        }

        /// <summary>
        /// Sets and gets the position of the node. For Menus, this is the center of the menu. For layers,
        /// this is the lower left corner of the layer.
        /// </summary>
        public virtual CCPoint Position
        {
            get { return m_obPosition; }
            set
            {
                m_obPosition = value;
                m_bTransformDirty = m_bInverseDirty = true;
            }
        }

        public float PositionX
        {
            get { return m_obPosition.X; }
            set { SetPosition(value, m_obPosition.Y); }
        }

        public float PositionY
        {
            get { return m_obPosition.Y; }
            set { SetPosition(m_obPosition.X, value); }
        }

        public CCRawList<CCNode> Children
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
            get { return m_bVisible; }
            set { m_bVisible = value; }
        }

        /// <summary>
        /// Returns the anchor point in pixels, AnchorPoint * ContentSize. This does not use
        /// the scale factor of the node.
        /// </summary>
        public virtual CCPoint AnchorPointInPoints
        {
            get { return m_obAnchorPointInPoints; }
        }

        /// <summary>
        /// returns the Anchor Point of the node as a value [0,1], where 1 is 100% of the dimension and 0 is 0%.
        /// </summary>
        public virtual CCPoint AnchorPoint
        {
            get { return m_obAnchorPoint; }
            set
            {
                if (!value.Equals(m_obAnchorPoint))
                {
                    m_obAnchorPoint = value;
                    m_obAnchorPointInPoints = new CCPoint(m_obContentSize.Width * m_obAnchorPoint.X,
                                                                  m_obContentSize.Height * m_obAnchorPoint.Y);
                    m_bTransformDirty = m_bInverseDirty = true;
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
            get { return m_obContentSize; }
            set
            {
                if (!CCSize.Equal(ref value, ref m_obContentSize))
                {
                    m_obContentSize = value;
                    m_obAnchorPointInPoints = new CCPoint(m_obContentSize.Width * m_obAnchorPoint.X,
                                                                  m_obContentSize.Height * m_obAnchorPoint.Y);
                    m_bTransformDirty = m_bInverseDirty = true;
                }
            }
        }

        public bool IsRunning
        {
            // read only
            get { return m_bRunning; }
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
                    m_bTransformDirty = m_bInverseDirty = true;
                }
            }
        }

        public CCRect BoundingBox
        {
            get
            {
                var rect = new CCRect(0, 0, m_obContentSize.Width, m_obContentSize.Height);
                return CCAffineTransform.Transform(rect, NodeToParentTransform());
            }
        }

        public CCRect BoundingBoxInPixels
        {
            get
            {
                var rect = new CCRect(0, 0, ContentSizeInPixels.Width, ContentSizeInPixels.Height);
                return CCAffineTransform.Transform(rect, NodeToParentTransform());
            }
        }


        public uint OrderOfArrival
        {
            get { return m_uOrderOfArrival; }
            set { m_uOrderOfArrival = value; }
        }

        public CCAffineTransform AdditionalTransform
        {
            get { return m_sAdditionalTransform; }
            set
            {
                m_sAdditionalTransform = value;
                m_bTransformDirty = true;
                m_bAdditionalTransformDirty = true;
            }
        }

        public string Name 
        {
            get { return m_sName; }
            set { m_sName = value; }
        }

        #region SelectorProtocol Members

        public virtual void Update(float dt)
        {
            /*
            if (m_nUpdateScriptHandler)
            {
                CCScriptEngineManager::sharedManager()->getScriptEngine()->executeSchedule(m_nUpdateScriptHandler, fDelta, this);
            }
    
            if (m_pComponentContainer && !m_pComponentContainer->isEmpty())
            {
                m_pComponentContainer->visit(fDelta);
            }
            */
        }

        #endregion

        private bool m_bCleaned = false;

        ~CCNode()
        {
            //unregisterScriptHandler();
            Cleanup();
            if(m_pChildren != null)
                m_pChildren.Clear();
            if(m_pChildrenByTag != null)
                m_pChildrenByTag.Clear();
        }

        public void GetPosition(out float x, out float y)
        {
            x = m_obPosition.X;
            y = m_obPosition.Y;
        }

        public void SetPosition(float x, float y)
        {
            m_obPosition.X = x;
            m_obPosition.Y = y;
            m_bTransformDirty = m_bInverseDirty = true;
        }

        protected virtual void ResetCleanState()
        {
            m_bCleaned = false;
            if (m_pChildren != null && m_pChildren.count > 0)
            {
                CCNode[] elements = m_pChildren.Elements;
                for (int i = 0, count = m_pChildren.count; i < count; i++)
                {
                    elements[i].ResetCleanState();
                }
            }
        }

        public virtual void Cleanup()
        {
            if (m_bCleaned == true)
            {
                return;
            }
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
            m_bCleaned = true;
        }

        public CCNode GetChildByTag(int tag)
        {
            Debug.Assert(tag != (int) CCNodeTag.Invalid, "Invalid tag");

            if (m_pChildrenByTag != null && m_pChildrenByTag.Count > 0)
            {
                Debug.Assert(m_pChildren != null && m_pChildren.count > 0);

                List<CCNode> list;
                if (m_pChildrenByTag.TryGetValue(tag, out list))
                {
                    if (list.Count > 0)
                    {
                        return list[0];
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
            Debug.Assert(child != this, "Can not add myself to myself.");

            if (m_pChildren == null)
            {
                m_pChildren = new CCRawList<CCNode>();
            }

            InsertChild(child, zOrder, tag);

            child.Parent = this;
            child.m_nTag = tag;
            child.m_uOrderOfArrival = s_globalOrderOfArrival++;
            if (child.m_bCleaned)
            {
                child.ResetCleanState();
            }

            if (m_bRunning)
            {
                child.OnEnter();
                child.OnEnterTransitionDidFinish();
            }
        }

        public void RemoveFromParent()
        {
            RemoveFromParentAndCleanup(true);
        }

        public void RemoveFromParentAndCleanup(bool cleanup)
        {
            if (m_pParent != null)
            {
                m_pParent.RemoveChild(this, cleanup);
            }
        }

        public void RemoveChild(CCNode child)
        {
            RemoveChild(child, true);
        }

        public virtual void RemoveChild(CCNode child, bool cleanup)
        {
            // explicit nil handling
            if (m_pChildren == null)
            {
                return;
            }

            ChangedChildTag(child, child.Tag, kCCNodeTagInvalid);

            if (m_pChildren.Contains(child))
            {
                DetachChild(child, cleanup);
            }
        }

        public void RemoveChildByTag(int tag)
        {
            RemoveChildByTag(tag, true);
        }

        public void RemoveChildByTag(int tag, bool cleanup)
        {
            Debug.Assert(tag != (int) CCNodeTag.Invalid, "Invalid tag");

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

        public virtual void RemoveAllChildren()
        {
            RemoveAllChildrenWithCleanup(true);
        }

        public virtual void RemoveAllChildrenWithCleanup(bool cleanup)
        {
            // not using detachChild improves speed here
            if (m_pChildren != null && m_pChildren.Count > 0)
            {
                if (m_pChildrenByTag != null)
                {
                    m_pChildrenByTag.Clear();
                }
                
                CCNode[] elements = m_pChildren.Elements;
                for (int i = 0, count = m_pChildren.count; i < count; i++)
                {
                    CCNode node = elements[i];

                    // IMPORTANT:
                    //  -1st do onExit
                    //  -2nd cleanup
                    if (m_bRunning)
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
            if (m_bRunning)
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

        private void ChangedChildTag(CCNode child, int oldTag, int newTag)
        {
            List<CCNode> list;

            if (m_pChildrenByTag != null && oldTag != kCCNodeTagInvalid)
            {
                if (m_pChildrenByTag.TryGetValue(oldTag, out list))
                {
                    list.Remove(child);
                }
            }

            if (newTag != kCCNodeTagInvalid)
            {
                if (m_pChildrenByTag == null)
                {
                    m_pChildrenByTag = new Dictionary<int, List<CCNode>>();
                }

                if (!m_pChildrenByTag.TryGetValue(newTag, out list))
                {
                    list = new List<CCNode>();
                    m_pChildrenByTag.Add(newTag, list);
                }

                list.Add(child);
            }
        }

        private void InsertChild(CCNode child, int z, int tag)
        {
            m_bReorderChildDirty = true;
            m_pChildren.Add(child);

            ChangedChildTag(child, kCCNodeTagInvalid, tag);

            //child.m_nOrderOfArrival = s_globalOrderOfArrival++;
            child.m_nZOrder = z;
        }

        public virtual void ReorderChild(CCNode child, int zOrder)
        {
            Debug.Assert(child != null, "Child must be non-null");

            m_bReorderChildDirty = true;
            child.m_uOrderOfArrival = s_globalOrderOfArrival++;
            child.m_nZOrder = zOrder;
        }
        
        #region Child Sorting

        int IComparer<CCNode>.Compare(CCNode n1, CCNode n2)
        {
            if (n1.m_nZOrder < n2.m_nZOrder || (n1.m_nZOrder == n2.m_nZOrder && n1.m_uOrderOfArrival < n2.m_uOrderOfArrival))
            {
                return -1;
            }

            if (n1 == n2)
            {
                return 0;
            }

            return 1;
        }

        public virtual void SortAllChildren()
        {
            if (m_bReorderChildDirty)
            {
                Array.Sort(m_pChildren.Elements, 0, m_pChildren.count, this);
                m_bReorderChildDirty = false;
            }
        }

        #endregion

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
            if (!m_bVisible)
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

            m_uOrderOfArrival = 0;

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
                bool translate = (m_obAnchorPointInPoints.X != 0.0f || m_obAnchorPointInPoints.Y != 0.0f);

                if (translate)
                {
                    CCDrawManager.Translate(m_obAnchorPointInPoints.X, m_obAnchorPointInPoints.Y, 0);
                }

                m_pCamera.Locate();

                if (translate)
                {
                    CCDrawManager.Translate(-m_obAnchorPointInPoints.X, -m_obAnchorPointInPoints.Y, 0);
                }
            }
        }

        public virtual void OnEnter()
        {

            // register 'parent' nodes first
            // since events are propagated in reverse order
            if (m_bTouchEnabled)
            {
                RegisterWithTouchDispatcher();
            }

            if (m_pChildren != null && m_pChildren.count > 0)
            {
                CCNode[] elements = m_pChildren.Elements;
                for (int i = 0, count = m_pChildren.count; i < count; i++)
                {
                    elements[i].OnEnter();
                }
            }

            ResumeSchedulerAndActions();

            m_bRunning = true;

            CCDirector director = CCDirector.SharedDirector;

            // add this node to concern the kaypad msg
            if (m_bKeypadEnabled)
            {
                director.KeypadDispatcher.AddDelegate(this);
            }

			// tell the director that this node is interested in Keyboard message
			if (m_bKeyboardEnabled)
			{
				director.KeyboardDispatcher.AddDelegate(this);
			}


            if (GamePadEnabled && director.GamePadEnabled)
            {
                if (!m_bGamePadDelegatesInited)
                {
                    m_OnGamePadButtonUpdateDelegate = new CCGamePadButtonDelegate(OnGamePadButtonUpdate);
                    m_OnGamePadConnectionUpdateDelegate = new CCGamePadConnectionDelegate(OnGamePadConnectionUpdate);
                    m_OnGamePadDPadUpdateDelegate = new CCGamePadDPadDelegate(OnGamePadDPadUpdate);
                    m_OnGamePadStickUpdateDelegate = new CCGamePadStickUpdateDelegate(OnGamePadStickUpdate);
                    m_OnGamePadTriggerUpdateDelegate = new CCGamePadTriggerDelegate(OnGamePadTriggerUpdate);
                    m_bGamePadDelegatesInited = true;
                }

                CCApplication application = CCApplication.SharedApplication;

                application.GamePadButtonUpdate += m_OnGamePadButtonUpdateDelegate;
                application.GamePadConnectionUpdate += m_OnGamePadConnectionUpdateDelegate;
                application.GamePadDPadUpdate += m_OnGamePadDPadUpdateDelegate;
                application.GamePadStickUpdate += m_OnGamePadStickUpdateDelegate;
                application.GamePadTriggerUpdate += m_OnGamePadTriggerUpdateDelegate;
            }
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

            CCDirector director = CCDirector.SharedDirector;

            if (m_bTouchEnabled)
            {
                director.TouchDispatcher.RemoveDelegate(this);
                //unregisterScriptTouchHandler();
            }

            if (m_bKeypadEnabled)
            {
                director.KeypadDispatcher.RemoveDelegate(this);
            }

			if (m_bKeyboardEnabled)
			{
				director.KeyboardDispatcher.RemoveDelegate(this);
			}

            if (GamePadEnabled && director.GamePadEnabled)
            {
                CCApplication application = CCApplication.SharedApplication;
                application.GamePadButtonUpdate -= m_OnGamePadButtonUpdateDelegate;
                application.GamePadConnectionUpdate -= m_OnGamePadConnectionUpdateDelegate;
                application.GamePadDPadUpdate -= m_OnGamePadDPadUpdateDelegate;
                application.GamePadStickUpdate -= m_OnGamePadStickUpdateDelegate;
                application.GamePadTriggerUpdate -= m_OnGamePadTriggerUpdateDelegate;
            }

            PauseSchedulerAndActions();

            m_bRunning = false;

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
            m_pActionManager.AddAction(action, this, !m_bRunning);
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
            Debug.Assert(tag != (int) CCNodeTag.Invalid, "Invalid tag");
            m_pActionManager.RemoveActionByTag(tag, this);
        }

        public CCAction GetActionByTag(int tag)
        {
            Debug.Assert(tag != (int) CCNodeTag.Invalid, "Invalid tag");
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
            m_pScheduler.ScheduleUpdateForTarget(this, priority, !m_bRunning);
        }

        public void UnscheduleUpdate()
        {
            m_pScheduler.UnscheduleUpdateForTarget(this);
        }

        public void Schedule(Action<float> selector)
        {
            Schedule(selector, 0.0f, CCScheduler.kCCRepeatForever, 0.0f);
        }

        public void Schedule(Action<float> selector, float interval)
        {
            Schedule(selector, interval, CCScheduler.kCCRepeatForever, 0.0f);
        }

        public void Schedule(Action<float> selector, float interval, uint repeat, float delay)
        {
            Debug.Assert(selector != null, "Argument must be non-nil");
            Debug.Assert(interval >= 0, "Argument must be positive");

            m_pScheduler.ScheduleSelector(selector, this, interval, repeat, delay, !m_bRunning);
        }

        public void ScheduleOnce(Action<float> selector, float delay)
        {
            Schedule(selector, 0.0f, 0, delay);
        }

        public void Unschedule(Action<float> selector)
        {
            // explicit nil handling
            if (selector == null)
                return;

            m_pScheduler.UnscheduleSelector(selector, this);
        }

        public void UnscheduleAllSelectors()
        {
            m_pScheduler.UnscheduleAllForTarget(this);
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
            if (m_bTransformDirty)
            {
                // Translate values
                float x = m_obPosition.X;
                float y = m_obPosition.Y;

                if (m_bIgnoreAnchorPointForPosition)
                {
                    x += m_obAnchorPointInPoints.X;
                    y += m_obAnchorPointInPoints.Y;
                }

                // Rotation values
                // Change rotation code to handle X and Y
                // If we skew with the exact same value for both x and y then we're simply just rotating
                float cx = 1, sx = 0, cy = 1, sy = 0;
                if (m_fRotationX != 0 || m_fRotationY != 0)
                {
                    float radiansX = -CCMacros.CCDegreesToRadians(m_fRotationX);
                    float radiansY = -CCMacros.CCDegreesToRadians(m_fRotationY);
                    cx = (float)Math.Cos(radiansX);
                    sx = (float)Math.Sin(radiansX);
                    cy = (float)Math.Cos(radiansY);
                    sy = (float)Math.Sin(radiansY);
                }

                bool needsSkewMatrix = (m_fSkewX != 0f || m_fSkewY != 0f);

                // optimization:
                // inline anchor point calculation if skew is not needed
                if (!needsSkewMatrix && !m_obAnchorPointInPoints.Equals(CCPoint.Zero))
                {
                    x += cy * -m_obAnchorPointInPoints.X * m_fScaleX + -sx * -m_obAnchorPointInPoints.Y * m_fScaleY;
                    y += sy * -m_obAnchorPointInPoints.X * m_fScaleX + cx * -m_obAnchorPointInPoints.Y * m_fScaleY;
                }

                // Build Transform Matrix
                // Adjusted transform calculation for rotational skew
                m_sTransform.a = cy * m_fScaleX;
                m_sTransform.b = sy * m_fScaleX;
                m_sTransform.c = -sx * m_fScaleY;
                m_sTransform.d = cx * m_fScaleY;
                m_sTransform.tx = x;
                m_sTransform.ty = y;

                // XXX: Try to inline skew
                // If skew is needed, apply skew and then anchor point
                if (needsSkewMatrix)
                {
                    var skewMatrix = new CCAffineTransform(
                        1.0f, (float) Math.Tan(CCMacros.CCDegreesToRadians(m_fSkewY)),
                        (float) Math.Tan(CCMacros.CCDegreesToRadians(m_fSkewX)), 1.0f,
                        0.0f, 0.0f);

                    m_sTransform = CCAffineTransform.Concat(skewMatrix, m_sTransform);

                    // adjust anchor point
                    if (!m_obAnchorPointInPoints.Equals(CCPoint.Zero))
                    {
                        m_sTransform = CCAffineTransform.Translate(m_sTransform,
                                                                                    -m_obAnchorPointInPoints.X,
                                                                                    -m_obAnchorPointInPoints.Y);
                    }
                }

                if (m_bAdditionalTransformDirty)
                {
                    m_sTransform.Concat(ref m_sAdditionalTransform);
                    m_bAdditionalTransformDirty = false;
                }

                m_bTransformDirty = false;
            }

            return m_sTransform;
        }

        public virtual void UpdateTransform()
        {
            // Recursively iterate over children
            if (m_pChildren != null && m_pChildren.count > 0)
            {
                CCNode[] elements = m_pChildren.Elements;
                for (int i = 0, count = m_pChildren.count; i < count; i++)
                {
                    elements[i].UpdateTransform();
                }
            }
        }

        public CCAffineTransform ParentToNodeTransform()
        {
            if (m_bInverseDirty)
            {
                m_sInverse = CCAffineTransform.Invert(NodeToParentTransform());
                m_bInverseDirty = false;
            }
            return m_sInverse;
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
            return CCAffineTransform.Invert(NodeToWorldTransform());
        }

        #endregion

        #region ConvertToSpace

        public CCPoint ConvertToNodeSpace(CCPoint worldPoint)
        {
            return CCAffineTransform.Transform(worldPoint, WorldToNodeTransform());
        }

        public CCPoint ConvertToWorldSpace(CCPoint nodePoint)
        {
            return CCAffineTransform.Transform(nodePoint, NodeToWorldTransform());
        }

        public CCPoint ConvertToNodeSpaceAr(CCPoint worldPoint)
        {
            CCPoint nodePoint = ConvertToNodeSpace(worldPoint);
            return nodePoint - m_obAnchorPointInPoints;
        }

        public CCPoint ConvertToWorldSpaceAr(CCPoint nodePoint)
        {
            CCPoint pt = nodePoint + m_obAnchorPointInPoints;
            return ConvertToWorldSpace(pt);
        }

        public CCPoint ConvertToWindowSpace(CCPoint nodePoint)
        {
            CCPoint worldPoint = ConvertToWorldSpace(nodePoint);
            return CCDirector.SharedDirector.ConvertToUi(worldPoint);
        }

        public CCPoint ConvertTouchToNodeSpace(CCTouch touch)
        {
            var point = touch.Location;
            return ConvertToNodeSpace(point);
        }

        public CCPoint ConvertTouchToNodeSpaceAr(CCTouch touch)
        {
            var point = touch.Location;
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

        public virtual void RegisterWithTouchDispatcher()
        {
            CCTouchDispatcher pDispatcher = CCDirector.SharedDirector.TouchDispatcher;

            /*
            if (m_pScriptHandlerEntry)
            {
                if (m_pScriptHandlerEntry->isMultiTouches())
                {
                    pDispatcher->addStandardDelegate(this, 0);
                    LUALOG("[LUA] Add multi-touches event handler: %d", m_pScriptHandlerEntry->getHandler());
                }
                else
                {
                    pDispatcher->addTargetedDelegate(this,
                                         m_pScriptHandlerEntry->getPriority(),
                                         m_pScriptHandlerEntry->getSwallowsTouches());
                    LUALOG("[LUA] Add touch event handler: %d", m_pScriptHandlerEntry->getHandler());
                }
                return;
            }
            */
            if (m_eTouchMode == CCTouchMode.AllAtOnce)
            {
                pDispatcher.AddStandardDelegate(this, 0);
            }
            else
            {
                pDispatcher.AddTargetedDelegate(this, m_nTouchPriority, true);
            }
        }

        public CCTouchMode TouchMode
        {
            get { return m_eTouchMode; }
            set
            {
                if (m_eTouchMode != value)
                {
                    m_eTouchMode = value;

                    if (m_bTouchEnabled)
                    {
                        TouchEnabled = false;
                        TouchEnabled = true;
                    }
                }
            }
        }

        public virtual bool TouchEnabled
        {
            get { return m_bTouchEnabled; }
            set
            {
                if (m_bTouchEnabled != value)
                {
                    m_bTouchEnabled = value;

                    if (m_bRunning)
                    {
                        if (value)
                        {
                            RegisterWithTouchDispatcher();
                        }
                        else
                        {
                            CCDirector.SharedDirector.TouchDispatcher.RemoveDelegate(this);
                        }
                    }
                }
            }
        }

        public virtual int TouchPriority
        {
            get { return m_nTouchPriority; }
            set
            {
                if (m_nTouchPriority != value)
                {
                    m_nTouchPriority = value;

                    if (m_bRunning)
                    {
                        TouchEnabled = false;
                        TouchEnabled = true;
                    }
                }
            }
        }

        public virtual bool KeypadEnabled
        {
            get { return m_bKeypadEnabled; }
            set
            {
                if (value != m_bKeypadEnabled)
                {
                    m_bKeypadEnabled = value;

                    if (m_bRunning)
                    {
                        if (value)
                        {
                            CCDirector.SharedDirector.KeypadDispatcher.AddDelegate(this);
                        }
                        else
                        {
                            CCDirector.SharedDirector.KeypadDispatcher.RemoveDelegate(this);
                        }
                    }
                }
            }
        }

		public virtual bool KeyboardEnabled
		{
			get { return m_bKeyboardEnabled; }
			set
			{
				if (value != m_bKeyboardEnabled)
				{
					m_bKeyboardEnabled = value;

					if (m_bRunning)
					{
						if (value)
						{
							CCDirector.SharedDirector.KeyboardDispatcher.AddDelegate(this);
						}
						else
						{
							CCDirector.SharedDirector.KeyboardDispatcher.RemoveDelegate(this);
						}
					}
				}
			}
		}

		public virtual CCKeyboardMode KeyboardMode
		{
			get { return m_eKeyboardMode; }
			set
			{
				if (m_eKeyboardMode != value)
				{
					m_eKeyboardMode = value;
				}
			}
		}

        public virtual bool GamePadEnabled
        {
            get { return (m_bGamePadEnabled); }
            set
            {
                if (value != m_bGamePadEnabled)
                {
                    m_bGamePadEnabled = value;
                }
                if (value && !CCDirector.SharedDirector.GamePadEnabled)
                {
                    CCDirector.SharedDirector.GamePadEnabled = true;
                }
            }
        }

        #region touches

        #region ICCStandardTouchDelegate Members

        public virtual void TouchesBegan(List<CCTouch> touches)
        {
        }

        public virtual void TouchesMoved(List<CCTouch> touches)
        {
        }

        public virtual void TouchesEnded(List<CCTouch> touches)
        {
        }

        public virtual void TouchesCancelled(List<CCTouch> touches)
        {
        }

        #endregion

        #region ICCTargetedTouchDelegate Members

        public virtual bool TouchBegan(CCTouch touch)
        {
            return true;
        }

        public virtual void TouchMoved(CCTouch touch)
        {
        }

        public virtual void TouchEnded(CCTouch touch)
        {
        }

        public virtual void TouchCancelled(CCTouch touch)
        {
        }

        #endregion

        #endregion


        public virtual void KeyBackClicked()
        {
        }

        public virtual void KeyMenuClicked()
        {
        }

		#region Keyboard Support

		public virtual void KeyPressed (Keys key) 
		{ }

		public virtual void KeyReleased (Keys key)
		{ }

		public virtual void KeyboardCurrentState (KeyboardState currentState)
		{ }

		#endregion

        #region GamePad Support
        private CCGamePadButtonDelegate m_OnGamePadButtonUpdateDelegate;
        private CCGamePadConnectionDelegate m_OnGamePadConnectionUpdateDelegate;
        private CCGamePadDPadDelegate m_OnGamePadDPadUpdateDelegate;
        private CCGamePadStickUpdateDelegate m_OnGamePadStickUpdateDelegate;
        private CCGamePadTriggerDelegate m_OnGamePadTriggerUpdateDelegate;

        protected virtual void OnGamePadTriggerUpdate(float leftTriggerStrength, float rightTriggerStrength, Microsoft.Xna.Framework.PlayerIndex player)
        {
        }

        protected virtual void OnGamePadStickUpdate(CCGameStickStatus leftStick, CCGameStickStatus rightStick, Microsoft.Xna.Framework.PlayerIndex player)
        {
        }

        protected virtual void OnGamePadDPadUpdate(CCGamePadButtonStatus leftButton, CCGamePadButtonStatus upButton, CCGamePadButtonStatus rightButton, CCGamePadButtonStatus downButton, Microsoft.Xna.Framework.PlayerIndex player)
        {
            if (!HasFocus)
            {
                return;
            }
        }

        protected virtual void OnGamePadConnectionUpdate(Microsoft.Xna.Framework.PlayerIndex player, bool IsConnected)
        {
        }

        protected virtual void OnGamePadButtonUpdate(CCGamePadButtonStatus backButton, CCGamePadButtonStatus startButton, CCGamePadButtonStatus systemButton, CCGamePadButtonStatus aButton, CCGamePadButtonStatus bButton, CCGamePadButtonStatus xButton, CCGamePadButtonStatus yButton, CCGamePadButtonStatus leftShoulder, CCGamePadButtonStatus rightShoulder, Microsoft.Xna.Framework.PlayerIndex player)
        {
        }
        #endregion
    }
}