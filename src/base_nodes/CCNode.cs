using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

	public class CCNode : ICCUpdatable, ICCFocusable, ICCKeypadDelegate, IComparer<CCNode>, IComparable<CCNode>
    {
		public const int TagInvalid = -1;        					// Use this to determine if a tag has been set on the node.
		static uint globalOrderOfArrival = 1;

		public CCAffineTransform AffineTransform;

		bool isWorldTransformDirty;
		bool isAdditionalTransformDirty;
		bool ignoreAnchorPointForPosition;
		bool isCleaned = false;
		bool keypadEnabled;         								// input variables
		bool inverseDirty;

		int tag;
		int zOrder;

		float rotationX;
		float rotationY;
		float scaleX;
		float scaleY;
		float skewX;
		float skewY;

		CCPoint anchorPoint;
		CCPoint anchorPointInPoints;
		CCPoint position;

		CCSize contentSize;

		CCAffineTransform additionalTransform;
		CCAffineTransform inverse;
		CCAffineTransform nodeToWorldTransform;

		CCCamera camera;
		CCScheduler scheduler;
		CCActionManager actionManager;

		Dictionary<int, List<CCNode>> childrenByTag;


		#region Properties

		// Auto-implemented properties

		public bool IsRunning { get; protected set; }
		public virtual bool HasFocus { get; set; }
		public virtual bool Visible { get; set; }
		public virtual bool IsSerializable { get; protected set; } 		// If this is true, the screen will be recorded into the director's state
		public int LocalZOrder { get; set; }
		public float GlobalZOrder { get; set; }
		public virtual float VertexZ { get; set; }
		public object UserData { get; set; }
		public object UserObject { get; set; }
		public string Name { get; set; }
		public CCEventDispatcher EventDispatcher { get; set; }
		public CCRawList<CCNode> Children { get; protected set; }
		public CCGridBase Grid { get; set; }
		public CCNode Parent { get; set; }

		internal protected uint OrderOfArrival { get; internal set; }
		internal protected CCDirector Director { get; private set; }

		protected bool IsTransformDirty { get; set; }
		protected bool IsReorderChildDirty { get; set; }

		// Not auto-implemented properties

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
					SetTransformIsDirty();
				}
			}
		}

		public virtual bool KeypadEnabled
		{
			get { return keypadEnabled; }
			set
			{
				if (value != keypadEnabled)
				{
					keypadEnabled = value;

					if (IsRunning)
					{
						if (value)
						{
							Director.KeypadDispatcher.AddDelegate(this);
						}
						else
						{
							Director.KeypadDispatcher.RemoveDelegate(this);
						}
					}
				}
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

		public virtual float SkewX
		{
			get { return skewX; }
			set
			{
				skewX = value;
				SetTransformIsDirty();
			}
		}

		public virtual float SkewY
		{
			get { return skewY; }
			set
			{
				skewY = value;
				SetTransformIsDirty();
			}
		}

		// 2D rotation of the node relative to the 0,1 vector in a clock-wise orientation.
		public virtual float Rotation
		{
			set
			{
				rotationX = rotationY = value;
				SetTransformIsDirty();
			}
		}

		public virtual float RotationX
		{
			get { return rotationX; }
			set
			{
				rotationX = value;
				SetTransformIsDirty();
			}
		}

		public virtual float RotationY
		{
			get { return rotationY; }
			set
			{
				rotationY = value;
				SetTransformIsDirty();
			}
		}
			
		// The general scale that applies to both X and Y directions.
		public virtual float Scale
		{
			set
			{
				scaleX = scaleY = value;
				SetTransformIsDirty();
			}
		}

		public virtual float ScaleX
		{
			get { return scaleX; }
			set
			{
				scaleX = value;
				SetTransformIsDirty();
			}
		}

		public virtual float ScaleY
		{
			get { return scaleY; }
			set
			{
				scaleY = value;
				SetTransformIsDirty();
			}
		}

		public float PositionX
		{
			get { return position.X; }
			set { SetPosition(value, position.Y); }
		}

		public float PositionY
		{
			get { return position.Y; }
			set { SetPosition(position.X, value); }
		}

		// Sets and gets the position of the node. For Menus, this is the center of the menu. For layers,
		// this is the lower left corner of the layer.
		public virtual CCPoint Position
		{
			get { return position; }
			set
			{
				position = value;
				SetTransformIsDirty();
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
					anchorPointInPoints = new CCPoint(contentSize.Width * anchorPoint.X,
						contentSize.Height * anchorPoint.Y);
					SetTransformIsDirty();
				}
			}
		}
			
		// Returns the content size with the scale applied.
		public virtual CCSize ContentSizeInPixels
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
					SetTransformIsDirty();
				}
			}
		}

		// Returns the bounding box of this node in world coordinates
		public CCRect WorldBoundingBox
		{
			get
			{
				var rect = new CCRect(0, 0, contentSize.Width, contentSize.Height);
				return CCAffineTransform.Transform(rect, NodeToWorldTransform);
			}
		}

		// Returns the bounding box of this node in the coordinate system of its parent.
		public CCRect BoundingBox
		{
			get
			{
				var rect = new CCRect(0, 0, contentSize.Width, contentSize.Height);
				return CCAffineTransform.Transform(rect, NodeToParentTransform());
			}
		}
			
		// Returns the bounding box of this node, in the coordinate system of its parent,
		// with the scale, content scale, and other display transforms applied to return
		// the per-pixel bounding box.
		public CCRect BoundingBoxInPixels
		{
			get
			{
				var rect = new CCRect(0, 0, ContentSizeInPixels.Width, ContentSizeInPixels.Height);
				return CCAffineTransform.Transform(rect, NodeToParentTransform());
			}
		}

		public CCAffineTransform AdditionalTransform
		{
			get { return additionalTransform; }
			set
			{
				additionalTransform = value;
				IsTransformDirty = true;
				isAdditionalTransformDirty = true;
			}
		}

		public CCAffineTransform ParentToNodeTransform
		{
			get 
			{
				if (inverseDirty) {
					inverse = CCAffineTransform.Invert (NodeToParentTransform ());
					inverseDirty = false;
				}
				return inverse;
			}
		}

		public CCAffineTransform NodeToWorldTransform
		{
			get 
			{
				CCAffineTransform t = NodeToParentTransform ();

				// CCLog.Log("{0}.NodeToWorld: woldIsDirty={1}, transformIsDirty={2}", GetType().Name, m_bWorldTransformIsDirty, m_bTransformDirty);

				if (!isWorldTransformDirty) {
					return (nodeToWorldTransform);
				}
				if (Parent != null) {
					CCAffineTransform n2p = Parent.NodeToWorldTransform;
					t.Concat (ref n2p);
				}
				isWorldTransformDirty = false;
				nodeToWorldTransform = t;
				return t;
			}
		}

		public CCAffineTransform WorldToNodeTransform
		{
			get 
			{
				return CCAffineTransform.Invert (NodeToWorldTransform);
			}
		}

		public CCCamera Camera
		{
			get { return camera ?? (camera = new CCCamera()); }
		}

		public CCScheduler Scheduler
		{
			get { return scheduler; }
			set
			{
				if (value != scheduler)
				{
					UnscheduleAll();
					scheduler = value;
				}
			}
		}

		public CCActionManager ActionManager
		{
			get { return actionManager; }
			set
			{
				if (value != actionManager)
				{
					StopAllActions();
					actionManager = value;
				}
			}
		}

		public CCNode this[int tag]
		{
			get { return GetChildByTag(tag); }
		}

		#endregion Properties


		#region Constructors

		// Eventually remove optional argument, director cannot be null
		public CCNode(CCDirector director=null)
        {
			if (director == null) {
				director = CCDirector.SharedDirector;
			}

			scaleX = 1.0f;
            scaleY = 1.0f;
            Visible = true;
            tag = TagInvalid;

			nodeToWorldTransform = CCAffineTransform.Identity;
            AffineTransform = CCAffineTransform.Identity;
            inverseDirty = true;
			isWorldTransformDirty = true;

			HasFocus = false;

			IsSerializable = true;

            // set default scheduler and actionManager
			Director = director;
            actionManager = director.ActionManager;
            scheduler = director.Scheduler;
			EventDispatcher = director.EventDispatcher;
        }

		#endregion Constructors


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

			//eventDispatcher.RemoveEventListeners (this);

			// actions
			StopAllActions();

			// timers
			UnscheduleAll();

			if (EventDispatcher != null)
				EventDispatcher.RemoveEventListeners (this);

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
            CCSerialization.SerializeData(inverseDirty, sw);
            CCSerialization.SerializeData(IsRunning, sw);
            CCSerialization.SerializeData(IsTransformDirty, sw);
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
            inverseDirty = CCSerialization.DeSerializeBool(sr);
            IsRunning = CCSerialization.DeSerializeBool(sr);
            IsTransformDirty = CCSerialization.DeSerializeBool(sr);
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

        // Returns the given point, which is assumed to be in this node's coordinate
        // system, as a point in the given target's coordinate system.
        public CCPoint ConvertPointTo(ref CCPoint ptInNode, CCNode target)
        {
            CCPoint pt = NodeToWorldTransform.Transform(ptInNode);
            pt = target.WorldToNodeTransform.Transform(pt);
            return (pt);
        }

        // Returns this node's bounding box in the coordinate system of the given target.
        public CCRect GetBoundingBox(CCNode target)
        {
            CCRect rect = WorldBoundingBox;
            rect = target.WorldToNodeTransform.Transform(rect);
            return (rect);
        }
			
        // Sets all of the transform indictators to dirty so that the visual transforms
        // are recomputed.
        public virtual void ForceTransformRefresh()
        {
            IsTransformDirty = true;
            isWorldTransformDirty = true;
            isAdditionalTransformDirty = true;
            inverseDirty = true;
        }

		public void GetPosition(out float x, out float y)
		{
			x = position.X;
			y = position.Y;
		}

		public void SetPosition(float x, float y)
		{
			position.X = x;
			position.Y = y;
			SetTransformIsDirty();
		}

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


		public virtual void Update(float dt)
		{
		}

        protected virtual void Draw()
        {
            // Does nothing in the root node class.
        }
			
        // This is called with every call to the MainLoop on the CCDirector class. In XNA, this is the same as the Draw() call.
        public virtual void Visit()
        {
            // quick return if not visible. children won't be drawn.
            if (!Visible)
            {
                return;
            }

            CCDrawManager.PushMatrix();

            if (Grid != null && Grid.Active)
            {
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
                Transform();
                Grid.Blit();
            }

            CCDrawManager.PopMatrix();
        }

        public void TransformAncestors()
        {
            if (Parent != null)
            {
                Parent.TransformAncestors();
                Parent.Transform();
            }
        }

        public void Transform()
        {
            CCDrawManager.MultMatrix(NodeToParentTransform(), VertexZ);

            // XXX: Expensive calls. Camera should be integrated into the cached affine matrix
            if (camera != null && !(Grid != null && Grid.Active))
            {
                bool translate = (anchorPointInPoints.X != 0.0f || anchorPointInPoints.Y != 0.0f);

                if (translate)
                {
                    CCDrawManager.Translate(anchorPointInPoints.X, anchorPointInPoints.Y, 0);
                }

                camera.Locate();

                if (translate)
                {
                    CCDrawManager.Translate(-anchorPointInPoints.X, -anchorPointInPoints.Y, 0);
                }
            }
        }


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

            // add this node to concern the kaypad msg
            if (keypadEnabled)
            {
				Director.KeypadDispatcher.AddDelegate(this);
            }
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
            if (keypadEnabled)
            {
				Director.KeypadDispatcher.RemoveDelegate(this);
            }

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

		public void AddAction (CCAction action, bool paused = false)
		{
			ActionManager.AddAction(action, this, paused);
		}

		public void AddActions (bool paused, params CCFiniteTimeAction[] actions)
		{
			ActionManager.AddAction(new CCSequence(actions), this, paused);
		}

        public CCActionState Repeat (uint times, params CCFiniteTimeAction[] actions)
		{
			return RunAction (new CCRepeat (new CCSequence(actions), times));

		}

        public CCActionState Repeat (uint times, CCActionInterval action)
		{
			return RunAction (new CCRepeat (action, times));
		}

        public CCActionState RepeatForever (params CCFiniteTimeAction[] actions)
		{
			return RunAction (new CCRepeatForever (actions));

		}

        public CCActionState RepeatForever(CCActionInterval action)
		{
			return RunAction (new CCRepeatForever (action));
		}

        public CCActionState RunAction(CCAction action)
        {
            Debug.Assert(action != null, "Argument must be non-nil");
            CCActionState actionState = actionManager.AddAction(action, this, !IsRunning);
            return actionState;
        }

        public CCActionState RunActions(params CCFiniteTimeAction[] actions)
		{
			Debug.Assert(actions != null, "Argument must be non-nil");
			var action = new CCSequence(actions);
            CCActionState actionState = actionManager.AddAction(action, this, !IsRunning);
            return actionState;
		}

        public void StopAllActions()
        {
            actionManager.RemoveAllActionsFromTarget(this);
        }

        public void StopAction(CCAction action)
        {
            actionManager.RemoveAction(action);
        }

        public void StopActionByTag(int tag)
        {
            Debug.Assert(tag != (int) CCNodeTag.Invalid, "Invalid tag");
            actionManager.RemoveActionByTag(tag, this);
        }

        public CCAction GetActionByTag(int tag)
        {
            Debug.Assert(tag != (int) CCNodeTag.Invalid, "Invalid tag");
            return actionManager.GetActionByTag(tag, this);
        }

        public int NumberOfRunningActions()
        {
            return actionManager.NumberOfRunningActionsInTarget(this);
        }

		#endregion Actions


		#region Scheduling

		public void Schedule ()
		{
			Schedule (0);
		}

		public void Schedule (int priority)
		{
			scheduler.Schedule (this, priority, !IsRunning);
		}

		public void Unschedule ()
		{
			scheduler.Unschedule (this);
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

			scheduler.Schedule (selector, this, interval, repeat, delay, !IsRunning);
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

			scheduler.Unschedule (selector, this);
		}

		public void UnscheduleAll ()
		{
			scheduler.UnscheduleAll (this);
		}

        public void Resume()
        {
            scheduler.Resume(this);
            actionManager.ResumeTarget(this);
			EventDispatcher.Resume (this);
        }

        public void Pause()
        {
            scheduler.PauseTarget(this);
            actionManager.PauseTarget(this);
			if (EventDispatcher != null)
				EventDispatcher.Pause (this);
        }

		#endregion Scheduling


        #region Transformations

        public virtual CCAffineTransform NodeToParentTransform()
        {
            if (IsTransformDirty)
            {
                // Translate values
                float x = position.X;
                float y = position.Y;

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
				AffineTransform.A = cy * scaleX;
				AffineTransform.B = sy * scaleX;
				AffineTransform.C = -sx * scaleY;
				AffineTransform.D = cx * scaleY;
				AffineTransform.Tx = x;
				AffineTransform.Ty = y;

                // XXX: Try to inline skew
                // If skew is needed, apply skew and then anchor point
                if (needsSkewMatrix)
                {
                    var skewMatrix = new CCAffineTransform(
                        1.0f, (float) Math.Tan(CCMacros.CCDegreesToRadians(skewY)),
                        (float) Math.Tan(CCMacros.CCDegreesToRadians(skewX)), 1.0f,
                        0.0f, 0.0f);

                    AffineTransform = CCAffineTransform.Concat(skewMatrix, AffineTransform);

                    // adjust anchor point
                    if (!anchorPointInPoints.Equals(CCPoint.Zero))
                    {
                        AffineTransform = CCAffineTransform.Translate(AffineTransform,
                                                                                    -anchorPointInPoints.X,
                                                                                    -anchorPointInPoints.Y);
                    }
                }

                if (isAdditionalTransformDirty)
                {
                    AffineTransform.Concat(ref additionalTransform);
                    isAdditionalTransformDirty = false;
                }

                IsTransformDirty = false;
            }

            return AffineTransform;
        }
			
        // Set my transform to be dirty and recursively set my children's transform to be dirty.
        protected virtual void SetTransformIsDirty()
        {
            IsTransformDirty = true;
            inverseDirty = true;
            // Me and all of my children have dirty world transforms now.
            SetWorldTransformIsDirty();
        }

        protected virtual void SetWorldTransformIsDirty()
        {
            isWorldTransformDirty = true;
            if (Children != null && Children.Count > 0)
            {
                CCNode[] elements = Children.Elements;
                for (int i = 0, count = Children.Count; i < count; i++)
                {
                    elements[i].SetWorldTransformIsDirty();
                }
            }
        }

        public virtual void UpdateTransform()
        {
            // Recursively iterate over children
            if (Children != null && Children.Count > 0)
            {
                CCNode[] elements = Children.Elements;
                for (int i = 0, count = Children.Count; i < count; i++)
                {
                    elements[i].UpdateTransform();
                }
            }
        }

		#endregion Transformations


        #region ConvertToSpace

        public CCPoint ConvertToNodeSpace(CCPoint worldPoint)
        {
            return CCAffineTransform.Transform(worldPoint, WorldToNodeTransform);
        }

        public CCPoint ConvertToWorldSpace(CCPoint nodePoint)
        {
            return CCAffineTransform.Transform(nodePoint, NodeToWorldTransform);
        }

        public CCPoint ConvertToNodeSpaceAr(CCPoint worldPoint)
        {
            CCPoint nodePoint = ConvertToNodeSpace(worldPoint);
            return nodePoint - anchorPointInPoints;
        }

        public CCPoint ConvertToWorldSpaceAr(CCPoint nodePoint)
        {
            CCPoint pt = nodePoint + anchorPointInPoints;
            return ConvertToWorldSpace(pt);
        }

        public CCPoint ConvertToWindowSpace(CCPoint nodePoint)
        {
            CCPoint worldPoint = ConvertToWorldSpace(nodePoint);
			return Director.ConvertToUi(worldPoint);
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

		#endregion ConvertToSpace


        public virtual void KeyBackClicked()
        {
        }

        public virtual void KeyMenuClicked()
        {
        }
    }
}