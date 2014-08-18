#if USE_PHYSICS
/****************************************************************************
 Copyright (c) 2013 Chukong Technologies Inc. ported by Jose Medrano (@netonjm)
 
 http://www.cocos2d-x.org
 
 Permission is hereby granted, free of charge, to any person obtaining a copy
 of this software and associated documentation files (the "Software"), to deal
 in the Software without restriction, including without limitation the rights
 to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 copies of the Software, and to permit persons to whom the Software is
 furnished to do so, subject to the following conditions:
 
 The above copyright notice and this permission notice shall be included in
 all copies or substantial portions of the Software.
 
 THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 THE SOFTWARE.
 ****************************************************************************/

using ChipmunkSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace CocosSharp
{

	public struct CCPhysicsRayCastInfo
	{


        public CCPhysicsShape Shape { get; set; }
        public CCPoint Start { get; set; }
        public CCPoint End { get; set; }              //< in lua, it's name is "ended"
        public CCPoint Contact { get; set; }
        public CCPoint Normal { get; set; }
        public float Fraction { get; set; }
        public object Data { get; set; }

		public CCPhysicsRayCastInfo(CCPhysicsShape shape,
								  CCPoint start,
								  CCPoint end,
								  CCPoint contact,
								  CCPoint normal,
								  float fraction,
                                    object data) : this()
		{
			this.Shape = shape;
			this.Start = start;
			this.End = end;          //< in lua, it's name is "ended"
			this.Contact = contact;
			this.Normal = normal;
			this.Fraction = fraction;
			this.Data = data;
		}
	};

	public struct CCRayCastCallbackInfo
	{
        public CCPhysicsWorld World { get; set; }
        public Func<CCPhysicsWorld, CCPhysicsRayCastInfo, object, bool> Function { get; set; }
        public CCPoint Pont1 { get; set; }
        public CCPoint Point2 { get; set; }
        public object Data { get; set; }

		public CCRayCastCallbackInfo(CCPhysicsWorld world, Func<CCPhysicsWorld, CCPhysicsRayCastInfo, object, bool> func, CCPoint p1, CCPoint p2, object data)
            : this()
		{
			this.World = world;
			this.Function = func;
			this.Pont1 = p1;
			this.Point2 = p2;
			this.Data = data;

		}
	}

	public class CCRectQueryCallbackInfo
	{
        public CCPhysicsWorld World { get; set; }
        public Func<CCPhysicsWorld, CCPhysicsShape, object, bool> Function { get; set; }
        public object Data { get; set; }
	};

	public class CCPointQueryCallbackInfo
	{
        public CCPhysicsWorld World { get; set; }
        public Func<CCPhysicsWorld, CCPhysicsShape, object, bool> Function { get; set; }
        public object Data { get; set; }

		public CCPointQueryCallbackInfo(CCPhysicsWorld w, Func<CCPhysicsWorld, CCPhysicsShape, object, bool> f, object d)
		{
			World = w; Function = f; Data = d;
		}

	}


	/**
 * @brief Called for each fixture found in the query. You control how the ray cast
 * proceeds by returning a float:
 * return true: continue
 * return false: terminate the ray cast
 * @param fixture the fixture hit by the ray
 * @param point the point of initial intersection
 * @param normal the normal vector at the point of intersection
 * @return true to continue, false to terminate
 */

	internal class CCPhysicsWorldCallback
	{

		public static bool CollisionBeginCallbackFunc(cpArbiter arb, cpSpace space, CCPhysicsWorld world)
		{

			cpShape a, b;
			arb.GetShapes(out a, out b);

			CCPhysicsShapeInfo ita = null, itb = null;
			cp.AssertWarn(CCPhysicsShapeInfo.Map.TryGetValue(a, out ita) && CCPhysicsShapeInfo.Map.TryGetValue(b, out itb));
			if (ita == null || itb == null)
				return false;

			CCPhysicsContact contact = new CCPhysicsContact(ita.Shape, itb.Shape);
			arb.data = contact;
			contact._contactInfo = arb;

			return world.CollisionBeginCallback(contact);
		}

		public static bool CollisionPreSolveCallbackFunc(cpArbiter arb, cpSpace space, CCPhysicsWorld world)
		{
			return world.CollisionPreSolveCallback((CCPhysicsContact)(arb.data));
		}

		public static void CollisionPostSolveCallbackFunc(cpArbiter arb, cpSpace space, CCPhysicsWorld world)
		{
			world.CollisionPostSolveCallback((CCPhysicsContact)(arb.data));
		}

		public static void CollisionSeparateCallbackFunc(cpArbiter arb, cpSpace space, CCPhysicsWorld world)
		{

			CCPhysicsContact contact = (CCPhysicsContact)(arb.data);
			if (contact != null)
				world.CollisionSeparateCallback(contact);
			//delete contact;
		}


		public static void RayCastCallbackFunc(cpShape shape, float t, CCPoint n, ref CCRayCastCallbackInfo info)
		{
			if (!Continues)
			{
				return;
			}

			CCPhysicsShapeInfo it;

			cp.AssertWarn(!CCPhysicsShapeInfo.Map.TryGetValue(shape, out it));

			CCPhysicsRayCastInfo callbackInfo = new CCPhysicsRayCastInfo(
        		it.Shape,
        		info.Pont1,
        		info.Point2,
        		new CCPoint(info.Pont1.X + (info.Point2.X - info.Pont1.X) * t, info.Pont1.Y + (info.Point2.Y - info.Pont1.Y) * t),
        		 new CCPoint(n.Y, n.Y),
        		t, null
        	);

			Continues = info.Function(info.World, callbackInfo, info.Data);
		}

		public static void QueryRectCallbackFunc(cpShape shape, CCRectQueryCallbackInfo info)
		{
			CCPhysicsShapeInfo it;

			cp.AssertWarn(!CCPhysicsShapeInfo.Map.TryGetValue(shape, out it));

			if (!Continues)
			{
				return;
			}

			Continues = info.Function(info.World, it.Shape, info.Data);

		}
		public static void QueryPointFunc(cpShape shape, float distance, CCPoint point, ref CCPointQueryCallbackInfo info)
		{
			CCPhysicsShapeInfo it;

			cp.AssertWarn(!CCPhysicsShapeInfo.Map.TryGetValue(shape, out it));

			Continues = info.Function(info.World, it.Shape, info.Data);
		}
		public static void GetShapesAtPointFunc(cpShape shape, float distance, CCPoint point, ref List<CCPhysicsShape> arr)
		{
			CCPhysicsShapeInfo it;

			cp.AssertWarn(!CCPhysicsShapeInfo.Map.TryGetValue(shape, out it));

			arr.Add(it.Shape);  // (it.second.getShape());

		}

		public static bool Continues = true;

	};


	/**
 * @brief Called for each fixture found in the query. You control how the ray cast
 * proceeds by returning a float:
 * return true: continue
 * return false: terminate the ray cast
 * @param fixture the fixture hit by the ray
 * @param point the point of initial intersection
 * @param normal the normal vector at the point of intersection
 * @return true to continue, false to terminate
 */

	/**
 * @brief An PhysicsWorld object simulates collisions and other physical properties. You do not create PhysicsWorld objects directly; instead, you can get it from an Scene object.
 */
	public class CCPhysicsWorld
	{

		public const int DEBUGDRAW_NONE = 0x00;        ///< draw nothing
		public const int DEBUGDRAW_SHAPE = 0x01;       ///< draw shapes
		public const int DEBUGDRAW_JOINT = 0x02;
		public const int DEBUGDRAW_CONTACT = 0x04;
		public const int DEBUGDRAW_ALL = DEBUGDRAW_SHAPE | DEBUGDRAW_JOINT | DEBUGDRAW_CONTACT;


		public string DEFAULT_FONT = "fonts/MarkerFelt-22";

		#region PROTECTED



		protected CCPoint _gravity;
        internal CCPhysicsWorldInfo Info { get; set; }
		protected float _speed;
		protected int _updateRate;
		protected int _updateRateCount;
		protected float _updateTime;

        public List<CCPhysicsBody> Bodies { get; protected set; }
        protected List<CCPhysicsJoint> Joints { get; set; }
        public CCScene Scene  { get; protected set; }

        protected bool DelayDirty { get; set; }
        public int DebugDrawMask { get; set; }
		public PhysicsDebugDraw _debugDraw;

        protected List<CCPhysicsBody> DelayAddBodies { get; set; }
        protected List<CCPhysicsBody> DelayRemoveBodies { get; set; }
        protected List<CCPhysicsJoint> DelayAddJoints { get; set; }
        protected List<CCPhysicsJoint> DelayRemoveJoints { get; set; }

		#endregion
		public cpSpace Space
		{
			get
			{
				return Info.Space;
			}
		}

		public CCPhysicsWorld(CCScene scene)
		{
            DelayAddBodies = new List<CCPhysicsBody>();
            DelayRemoveBodies = new List<CCPhysicsBody>();
            DelayAddJoints = new List<CCPhysicsJoint>();
            DelayRemoveJoints = new List<CCPhysicsJoint>();

            Bodies = new List<CCPhysicsBody>();
            Joints = new List<CCPhysicsJoint>();

			_gravity = new CCPoint(0.0f, -98.0f);
			_speed = 1.0f;
			_updateRate = 1;
			_updateRateCount = 0;
			_updateTime = 0.0f;
			Info = null;
			Scene = null;
			DelayDirty = false;

			Info = new CCPhysicsWorldInfo();

			Scene = scene;

            Info.Gravity = PhysicsHelper.CCPointToCpVect(_gravity);

			var spc = Info.Space;

			spc.defaultHandler = new cpCollisionHandler()
			{
				beginFunc = (a, s, o) => CCPhysicsWorldCallback.CollisionBeginCallbackFunc(a as cpArbiter, s, this),
				preSolveFunc = (a, s, o) => CCPhysicsWorldCallback.CollisionPreSolveCallbackFunc(a as cpArbiter, s, this),
				postSolveFunc = (a, s, o) => CCPhysicsWorldCallback.CollisionPostSolveCallbackFunc(a as cpArbiter, s, this),
				separateFunc = (a, s, o) => CCPhysicsWorldCallback.CollisionSeparateCallbackFunc(a as cpArbiter, s, this)
			};

		}

#if USE_PHYSICS

		public void DebugDraw()
		{

			if (_debugDraw == null)
			{

				_debugDraw = new PhysicsDebugDraw(this);

				_debugDraw.Flags = PhysicsDrawFlags.All;
			}

			if (_debugDraw != null && Bodies.Count > 0)
			{
				if (_debugDraw.Begin())
				{
					if ((DebugDrawMask & DEBUGDRAW_SHAPE) > 0)
					{
						foreach (CCPhysicsBody body in Bodies)
						{
							//hysicsBody body = dynamic_cast<PhysicsBody*>(obj);

							if (!body.IsEnabled())
							{
								continue;
							}

							foreach (CCPhysicsShape shape in body.GetShapes())
							{
								_debugDraw.DrawShape(shape);
							}
						}
					}

					if ((DebugDrawMask & DEBUGDRAW_JOINT) > 0)
					{
						foreach (CCPhysicsJoint joint in Joints)
						{
							_debugDraw.DrawJoint(joint);
						}
					}

					_debugDraw.End();
				}
			}
		}

#endif


		~CCPhysicsWorld()
		{
			RemoveAllJoints(true);
			RemoveAllBodies();
		}

		#region PUBLIC


		/** Adds a joint to the physics world.*/
		public virtual void AddJoint(CCPhysicsJoint joint)
		{
			if (joint.World != null && joint.World != this)
			{
				joint.RemoveFormWorld();
			}

			AddJointOrDelay(joint);
			Joints.Add(joint);
			joint._world = this;
		}
		/** Remove a joint from physics world.*/

		public virtual void RemoveJoint(CCPhysicsJoint joint, bool destroy = true)
		{
			if (joint.World != this)
			{
				if (destroy)
				{
					cp.AssertWarn("physics warnning: the joint is not in this world, it won't be destoried utill the body it conntect is destoried");
				}
				return;
			}

			RemoveJointOrDelay(joint);

			Joints.Remove(joint);
			joint._world = null;

			// clean the connection to this joint
			if (destroy)
			{
				if (joint.BodyA != null)
				{
					joint.BodyA.RemoveJoint(joint);
				}

				if (joint.BodyB != null)
				{
					joint.BodyB.RemoveJoint(joint);
				}

				// test the distraction is delaied or not
				if (DelayRemoveJoints.Exists(j => j == joint))
				{
					joint._destoryMark = true;
				}
			}
		}

		/** Remove all joints from physics world.*/
		public virtual void RemoveAllJoints(bool destroy = true)
		{
			foreach (var joint in Joints)
			{
				RemoveJointOrDelay(joint);
				joint._world = null;

				// clean the connection to this joint
				if (destroy)
				{
					if (joint.BodyA != null)
					{
						joint.BodyA.RemoveJoint(joint);
					}

					if (joint.BodyB != null)
					{
						joint.BodyB.RemoveJoint(joint);
					}

					// test the distraction is delaied or not
					if (DelayRemoveJoints.Exists(j => j == joint))
						joint._destoryMark = true;
				}
			}

			Joints.Clear();
		}

		/** Remove a body from physics world. */
		public virtual void RemoveBody(CCPhysicsBody body)
		{

			if (body.GetWorld() != this)
			{
				cp.AssertWarn("Physics Warnning: this body doesn't belong to this world");
				return;
			}

			// destory the body's joints
			foreach (var joint in body._joints)
			{
				// set destroy param to false to keep the iterator available
				RemoveJoint(joint, false);

				CCPhysicsBody other = (joint.BodyA == body ? joint.BodyB : joint.BodyA);
				other.RemoveJoint(joint);


				if (DelayRemoveJoints.Exists(j => j == joint))
					joint._destoryMark = true;

			}

			body._joints.Clear();

			RemoveBodyOrDelay(body);
			Bodies.Remove(body);
			body._world = null;

		}

		/** Remove body by tag. */
		public virtual void RemoveBody(int tag)
		{
			foreach (var body in Bodies)
			{
				if (body.Tag == tag)
				{
					RemoveBody(body);
					return;
				}
			}
		}

		/** Remove all bodies from physics world. */
		public virtual void RemoveAllBodies()
		{
			foreach (var child in Bodies)
			{
				RemoveBodyOrDelay(child);
				child._world = null;
			}

			Bodies.Clear();
		}


		/** Searches for physics shapes that intersects the ray. */
		public void RayCast(Func<CCPhysicsWorld, CCPhysicsRayCastInfo, object, bool> func, CCPoint point1, CCPoint point2, object data)
		{
			cp.AssertWarn(func != null, "func shouldn't be nullptr");

			if (func != null)
			{
				CCRayCastCallbackInfo info = new CCRayCastCallbackInfo(this, func, point1, point2, data);
				//Action<cpShape, cpVect, cpVect, float, object> func
				CCPhysicsWorldCallback.Continues = true;

				this.Space.SegmentQuery(
                    PhysicsHelper.CCPointToCpVect(point1),
                    PhysicsHelper.CCPointToCpVect(point2), 1f,
									new cpShapeFilter(cp.NO_GROUP, cp.ALL_LAYERS, cp.ALL_LAYERS),
                    (shape, v1, v2, f, o) => CCPhysicsWorldCallback.RayCastCallbackFunc(shape, f, PhysicsHelper.cpVectToCCPoint(v1), ref info), data
									);
			}
		}

		/** Searches for physics shapes that contains in the rect. */
		public void QueryRect(Func<CCPhysicsWorld, CCPhysicsShape, object, bool> func, CCRect rect, object data)
		{
			cp.AssertWarn(func != null, "func shouldn't be nullptr");
			if (func != null)
			{
				CCRectQueryCallbackInfo info = new CCRectQueryCallbackInfo()
					{
						World = this,
						Function = func,
						Data = data
					};

				CCPhysicsWorldCallback.Continues = true;

				this.Space.BBQuery(
									PhysicsHelper.rect2cpbb(rect),
									new cpShapeFilter(cp.NO_GROUP, cp.ALL_LAYERS, cp.ALL_LAYERS),
									(s, o) => CCPhysicsWorldCallback.QueryRectCallbackFunc(s, info),
									data
									);
			}
		}

		/** Searches for physics shapes that contains the point. */
		public void QueryPoint(Func<CCPhysicsWorld, CCPhysicsShape, object, bool> func, CCPoint point, object data)
		{
			cp.AssertWarn(func != null, "func shouldn't be nullptr");

			if (func != null)
			{
				//CCPointQueryCallbackInfo info = new CCPointQueryCallbackInfo(this, func, data);

				CCPointQueryCallbackInfo info = new CCPointQueryCallbackInfo(this, func, data);

				CCPhysicsWorldCallback.Continues = true;

				this.Space.PointQuery(
                    PhysicsHelper.CCPointToCpVect(point), 0f,
					new cpShapeFilter(cp.NO_GROUP, cp.ALL_LAYERS, cp.ALL_LAYERS),
					(s, v, f1, f2, o) => CCPhysicsWorldCallback.QueryPointFunc(s, 0f, point, ref info),
					data
								);
			}
		}

		/** Get phsyics shapes that contains the point. */
		public List<CCPhysicsShape> GetShapes(CCPoint point)
		{
			List<CCPhysicsShape> arr = new List<CCPhysicsShape>();

			this.Space.PointQuery(
                PhysicsHelper.CCPointToCpVect(point), 0, new cpShapeFilter(cp.NO_GROUP, cp.ALL_LAYERS, cp.ALL_LAYERS),
                (s, v1, f, v2, o) => CCPhysicsWorldCallback.GetShapesAtPointFunc(s, f, PhysicsHelper.cpVectToCCPoint(v1), ref arr),
									null
									 );

			return arr;
		}

		/** return physics shape that contains the point. */
		public CCPhysicsShape GetShape(CCPoint point)
		{

			cpShape shape = null;

			this.Space.PointQuery(
                PhysicsHelper.CCPointToCpVect(point), 0, new cpShapeFilter(cp.NO_GROUP, cp.ALL_LAYERS, cp.ALL_LAYERS),
								  (s, v1, f, v2, o) => { shape = s; }, null);

			if (shape == null)
				return null;

			CCPhysicsShapeInfo dev;
			if (CCPhysicsShapeInfo.Map.TryGetValue(shape, out dev))
				return dev.Shape;

			return null;

		}

		/** Get body by tag */
		public CCPhysicsBody GetBody(int tag)
		{
			foreach (var body in Bodies)
			{
				if (body.Tag == tag)
				{
					return body;
				}
			}

			return null;
		}

		/** get the gravity value */
		public CCPoint Gravity
		{
			get { return _gravity; }
			set
			{
				if (Bodies.Count > 0)
				{
					foreach (var body in Bodies)
					{
						// reset gravity for body
						if (!body.IsGravityEnabled())
						{
							body.ApplyForce(PhysicsHelper.CCPointToCpVect((_gravity - value)) * body.GetMass());
						}
					}
				}

				_gravity = value;
				Info.Gravity = PhysicsHelper.CCPointToCpVect(value);
			}
		}

		
		/** get the speed of physics world */
		public float Speed { get { return _speed; } set { if (value >= 0.0f) { _speed = value; } } }

		/** 
		 * set the update rate of physics world, update rate is the value of EngineUpdateTimes/PhysicsWorldUpdateTimes.
		 * set it higher can improve performance, set it lower can improve accuracy of physics world simulation.
		 * default value is 1.0
		 */
		public int UpdateRate { get { return _updateRate; } set { if (value > 0) { _updateRate = value; } } }

		#endregion

		#region PROTECTED

		public virtual CCPhysicsBody AddBody(CCPhysicsBody body)
		{
			cp.AssertWarn(body != null, "the body can not be nullptr");

			if (body.GetWorld() == this)
				return null;

			if (body.GetWorld() != null)
			{
				body.RemoveFromWorld();
			}

			AddBodyOrDelay(body);
			Bodies.Add(body);
			body._world = this;

			return body;
		}

		public virtual CCPhysicsShape AddShape(CCPhysicsShape shape)
		{
			if (shape != null)
			{
				Info.AddShape(shape._info);
				return shape;
			}
			return null;
		}

		public virtual void RemoveShape(CCPhysicsShape shape)
		{
			if (shape != null)
			{
				Info.RemoveShape(shape._info);
			}
		}

		public virtual void Update(float delta)
		{
			while (DelayDirty)
			{
				// the updateJoints must run before the updateBodies.
				UpdateJoints();
				UpdateBodies();

				DelayDirty = !(DelayAddBodies.Count == 0 && DelayRemoveBodies.Count == 0 && DelayAddJoints.Count == 0 && DelayRemoveJoints.Count == 0);
			}

			_updateTime += delta;
			if (++_updateRateCount >= _updateRate)
			{
				Info.Step(_updateTime * _speed);
				foreach (var body in Bodies)
				{
					body.Update(_updateTime * _speed);
				}
				_updateRateCount = 0;
				_updateTime = 0.0f;
			}

			if (DebugDrawMask != DEBUGDRAW_NONE)
			{
				DebugDraw();
			}


		}
	

		public virtual bool CollisionBeginCallback(CCPhysicsContact contact)
		{
			bool ret = true;

			CCPhysicsShape shapeA = contact.GetShapeA();
			CCPhysicsShape shapeB = contact.GetShapeB();

			CCPhysicsBody bodyA = shapeA.Body;
			CCPhysicsBody bodyB = shapeB.Body;

			List<CCPhysicsJoint> jointsA = bodyA.GetJoints();

			// check the joint is collision enable or not
			foreach (CCPhysicsJoint joint in jointsA)
			{

				if (!Joints.Exists(j => j == joint))
					continue;

				if (!joint.IsCollisionEnabled())
				{
					CCPhysicsBody body = joint.BodyA == bodyA ? joint.BodyB : joint.BodyA;

					if (body == bodyB)
					{
						contact.SetNotificationEnable(false);
						return false;
					}
				}
			}

			// bitmask check
			if ((shapeA.CategoryBitmask & shapeB.ContactTestBitmask) == 0
				|| (shapeA.ContactTestBitmask & shapeB.CategoryBitmask) == 0)
			{
				contact.SetNotificationEnable(false);
			}

			if (shapeA.Group != 0 && shapeA.Group == shapeB.Group)
			{
				ret = shapeA.Group > 0;
			}
			else
			{
				if ((shapeA.CategoryBitmask & shapeB.CollisionBitmask) == 0
					|| (shapeB.CategoryBitmask & shapeA.CollisionBitmask) == 0)
				{
					ret = false;
				}
			}

			if (contact.IsNotificationEnabled())
			{
				contact.SetEventCode(EventCode.BEGIN);
				contact.SetWorld(this);

				Scene.DispatchEvent(contact);
			}


			return ret ? contact.ResetResult() : false;

		}

		public virtual bool CollisionPreSolveCallback(CCPhysicsContact contact)
		{
			if (!contact.IsNotificationEnabled())
			{
				(contact._contactInfo as cpArbiter).Ignore();
				return true;
			}

			contact.SetEventCode(EventCode.PRESOLVE);
			contact.SetWorld(this);
			Scene.DispatchEvent(contact);

			return contact.ResetResult() ? true : false;
		}

		public virtual void CollisionPostSolveCallback(CCPhysicsContact contact)
		{
			if (!contact.IsNotificationEnabled())
			{
				return;
			}

			contact.SetEventCode(EventCode.POSTSOLVE);
			contact.SetWorld(this);
			Scene.DispatchEvent(contact);
		}

		public virtual void CollisionSeparateCallback(CCPhysicsContact contact)
		{
			if (!contact.IsNotificationEnabled())
			{
				return;
			}

			contact.SetEventCode(EventCode.SEPERATE);
			contact.SetWorld(this);
			Scene.DispatchEvent(contact);
		}

		public virtual void DoAddBody(CCPhysicsBody body)
		{
			if (body.IsEnabled())
			{
				//is gravity enable
				if (!body.IsGravityEnabled())
				{
					body.ApplyForce(-_gravity * body.GetMass());
				}

				// add body to space
				if (body.IsDynamic())
				{
					Info.AddBody(body._info);
				}

				// add shapes to space
				foreach (CCPhysicsShape shape in body.GetShapes())
				{
					AddShape(shape);
				}
			}
		}

		public virtual void DoRemoveBody(CCPhysicsBody body)
		{
			cp.AssertWarn(body != null, "the body can not be nullptr");

			// reset the gravity
			if (!body.IsGravityEnabled())
			{
				body.ApplyForce(_gravity * body.GetMass());
			}

			// remove shaps
			foreach (var shape in body.GetShapes())
			{
				RemoveShape(shape);
			}

			// remove body
			Info.RemoveBody(body._info);
		}

		public virtual void DoAddJoint(CCPhysicsJoint joint)
		{
			if (joint == null || joint._info == null)
			{
				return;
			}

			Info.AddJoint(joint._info);
		}

		public virtual void DoRemoveJoint(CCPhysicsJoint joint)
		{
			Info.RemoveJoint(joint._info);
		}

		public virtual CCPhysicsBody AddBodyOrDelay(CCPhysicsBody body)
		{
			CCPhysicsBody removeBodyIter = DelayRemoveBodies.Find(b => b == body);
			if (removeBodyIter != null)
			{
				DelayRemoveBodies.Remove(removeBodyIter);
				return null;
			}

			if (Info.IsLocked())
			{
				if (DelayAddBodies.Exists(b => b == body))
				{
					DelayAddBodies.Add(body);
					DelayDirty = true;
				}
			}
			else
			{
				DoAddBody(body);
			}

			return body;
		}

		public virtual void RemoveBodyOrDelay(CCPhysicsBody body)
		{
			if (DelayAddBodies.Exists(b => b == body))
			{
				DelayAddBodies.Remove(body);
				return;
			}

			if (Info.IsLocked())
			{
				if (DelayRemoveBodies.Exists(b => b == body))
				{
					DelayRemoveBodies.Add(body);
					DelayDirty = true;
				}
			}
			else
			{
				DoRemoveBody(body);
			}
		}

		public virtual void AddJointOrDelay(CCPhysicsJoint joint)
		{

			var it = DelayRemoveJoints.Find(j => j == joint);
			if (it != null)
			{
				DelayRemoveJoints.Remove(it);
				return;
			}

			if (Info.IsLocked())
			{

				if (!DelayAddJoints.Exists(j => j == joint))
				{
					DelayAddJoints.Add(joint);
					DelayDirty = true;
				}

			}
			else
			{
				DoAddJoint(joint);
			}
		}

		public virtual void RemoveJointOrDelay(CCPhysicsJoint joint)
		{
			var it = DelayAddJoints.Find(j => j == joint);
			if (it != null)
			{
				DelayAddJoints.Remove(it);
			}
			if (Info.IsLocked())
			{

				if (!DelayAddJoints.Exists(j => j == joint))
				{
					DelayRemoveJoints.Add(joint);
					DelayDirty = true;
				}
			}
			else
			{
				DoRemoveJoint(joint);
			}
		}

		public virtual void UpdateBodies()
		{
			if (Info.IsLocked())
			{
				return;
			}

			// Fixed: netonjm >  issue #4944, contact callback will be invoked when add/remove body, _delayAddBodies maybe changed, so we need make a copy.

			CCPhysicsBody[] addCopy = new CCPhysicsBody[DelayAddBodies.Count];
			DelayAddBodies.CopyTo(addCopy);
			DelayAddBodies.Clear();

			foreach (var body in addCopy)
			{
				DoAddBody(body);
			}

			CCPhysicsBody[] removeCopy = new CCPhysicsBody[DelayRemoveBodies.Count];
			DelayRemoveBodies.CopyTo(removeCopy);

			DelayRemoveBodies.Clear();

			foreach (var body in removeCopy)
			{
				DoRemoveBody(body);
			}
		}

		public virtual void UpdateJoints()
		{
			if (Info.IsLocked())
			{
				return;
			}

			CCPhysicsJoint[] addCopy = new CCPhysicsJoint[DelayAddJoints.Count];

			DelayAddJoints.CopyTo(addCopy);

			DelayAddJoints.Clear();

			foreach (var joint in addCopy)
			{
				DoAddJoint(joint);
			}

			CCPhysicsJoint[] removeCopy = new CCPhysicsJoint[DelayRemoveJoints.Count];
			DelayRemoveJoints.CopyTo(removeCopy);

			DelayRemoveJoints.Clear();
			foreach (var joint in removeCopy)
			{
				DoRemoveJoint(joint);

				if (joint._destoryMark)
				{
					//delete joint;
				}
			}
		}

		#endregion

      
        public int Iterations { get { return Info.Iterations; } set { Info.Iterations = value; } }

        public float SleepTimeThreshold { get { return Info.SleepTimeThreshold; } set { Info.SleepTimeThreshold = value; } }

        public float CurrentTimeStep { get { return Info.CurrentTimeStep; } }

        public float Damping { get { return Info.Damping; } set { Info.Damping = value; } }

        public float IdleSpeedThreshold { get { return Info.IdleSpeedThreshold; } set { Info.IdleSpeedThreshold = value; } }

        public cpBody StaticBody { get { return Info.StaticBody; } }
        
	}

}
#endif