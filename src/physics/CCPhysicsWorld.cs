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


		public CCPhysicsShape shape;
		public CCPoint start;
		public CCPoint end;              //< in lua, it's name is "ended"
		public CCPoint contact;
		public CCPoint normal;
		public float fraction;
		public object data;

		public CCPhysicsRayCastInfo(CCPhysicsShape shape,
								  CCPoint start,
								  CCPoint end,
								  CCPoint contact,
								  CCPoint normal,
								  float fraction,
								  object data)
		{
			this.shape = shape;
			this.start = start;
			this.end = end;          //< in lua, it's name is "ended"
			this.contact = contact;
			this.normal = normal;
			this.fraction = fraction;
			this.data = data;
		}
	};

	public struct CCRayCastCallbackInfo
	{
		public CCPhysicsWorld world;
		public Func<CCPhysicsWorld, CCPhysicsRayCastInfo, object, bool> func;
		public CCPoint p1;
		public CCPoint p2;
		public object data;

		public CCRayCastCallbackInfo(CCPhysicsWorld world, Func<CCPhysicsWorld, CCPhysicsRayCastInfo, object, bool> func, CCPoint p1, CCPoint p2, object data)
		{
			this.world = world;
			this.func = func;
			this.p1 = p1;
			this.p2 = p2;
			this.data = data;

		}
	}

	public class CCRectQueryCallbackInfo
	{
		public CCPhysicsWorld world;
		public Func<CCPhysicsWorld, CCPhysicsShape, object, bool> func;
		public object data;
	};

	public class CCPointQueryCallbackInfo
	{
		public CCPhysicsWorld world;
		public Func<CCPhysicsWorld, CCPhysicsShape, object, bool> func;
		public object data;

		public CCPointQueryCallbackInfo(CCPhysicsWorld w, Func<CCPhysicsWorld, CCPhysicsShape, object, bool> f, object d)
		{
			world = w; func = f; data = d;
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
			if (!continues)
			{
				return;
			}

			CCPhysicsShapeInfo it;

			cp.AssertWarn(!CCPhysicsShapeInfo.Map.TryGetValue(shape, out it));

			CCPhysicsRayCastInfo callbackInfo = new CCPhysicsRayCastInfo(
		it.Shape,
		info.p1,
		info.p2,
		new CCPoint(info.p1.X + (info.p2.X - info.p1.X) * t, info.p1.Y + (info.p2.Y - info.p1.Y) * t),
		 new CCPoint(n.Y, n.Y),
		t, null
	);

			continues = info.func(info.world, callbackInfo, info.data);
		}

		public static void QueryRectCallbackFunc(cpShape shape, CCRectQueryCallbackInfo info)
		{
			CCPhysicsShapeInfo it;

			cp.AssertWarn(!CCPhysicsShapeInfo.Map.TryGetValue(shape, out it));

			if (!continues)
			{
				return;
			}

			continues = info.func(info.world, it.Shape, info.data);

		}
		public static void QueryPointFunc(cpShape shape, float distance, CCPoint point, ref CCPointQueryCallbackInfo info)
		{
			CCPhysicsShapeInfo it;

			cp.AssertWarn(!CCPhysicsShapeInfo.Map.TryGetValue(shape, out it));

			continues = info.func(info.world, it.Shape, info.data);
		}
		public static void GetShapesAtPointFunc(cpShape shape, float distance, CCPoint point, ref List<CCPhysicsShape> arr)
		{
			CCPhysicsShapeInfo it;

			cp.AssertWarn(!CCPhysicsShapeInfo.Map.TryGetValue(shape, out it));

			arr.Add(it.Shape);  // (it.second.getShape());

		}

		public static bool continues = true;

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
		internal CCPhysicsWorldInfo _info;
		protected float _speed;
		protected int _updateRate;
		protected int _updateRateCount;
		protected float _updateTime;

		protected List<CCPhysicsBody> _bodies = new List<CCPhysicsBody>();
		protected List<CCPhysicsJoint> _joints = new List<CCPhysicsJoint>();
		protected CCScene _scene;

		protected bool _delayDirty;
		public PhysicsDebugDraw _debugDraw;
		private int _debugDrawMask;

		protected List<CCPhysicsBody> _delayAddBodies = new List<CCPhysicsBody>();
		protected List<CCPhysicsBody> _delayRemoveBodies = new List<CCPhysicsBody>();
		protected List<CCPhysicsJoint> _delayAddJoints = new List<CCPhysicsJoint>();
		protected List<CCPhysicsJoint> _delayRemoveJoints = new List<CCPhysicsJoint>();

		#endregion
		public cpSpace Space
		{
			get
			{
				return _info.Space;
			}
		}

		public CCPhysicsWorld(CCScene scene)
		{

			_gravity = new CCPoint(0.0f, -98.0f);
			_speed = 1.0f;
			_updateRate = 1;
			_updateRateCount = 0;
			_updateTime = 0.0f;
			_info = null;
			_scene = null;
			_delayDirty = false;

			_info = new CCPhysicsWorldInfo();

			_scene = scene;

            _info.Gravity = PhysicsHelper.CCPointToCpVect(_gravity);

			var spc = _info.Space;

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

			if (_debugDraw != null && _bodies.Count > 0)
			{
				if (_debugDraw.Begin())
				{
					if ((_debugDrawMask & DEBUGDRAW_SHAPE) > 0)
					{
						foreach (CCPhysicsBody body in _bodies)
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

					if ((_debugDrawMask & DEBUGDRAW_JOINT) > 0)
					{
						foreach (CCPhysicsJoint joint in _joints)
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
			_joints.Add(joint);
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

			_joints.Remove(joint);
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
				if (_delayRemoveJoints.Exists(j => j == joint))
				{
					joint._destoryMark = true;
				}
			}
		}

		/** Remove all joints from physics world.*/
		public virtual void RemoveAllJoints(bool destroy = true)
		{
			foreach (var joint in _joints)
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
					if (_delayRemoveJoints.Exists(j => j == joint))
						joint._destoryMark = true;
				}
			}

			_joints.Clear();
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


				if (_delayRemoveJoints.Exists(j => j == joint))
					joint._destoryMark = true;

			}

			body._joints.Clear();

			RemoveBodyOrDelay(body);
			_bodies.Remove(body);
			body._world = null;

		}

		/** Remove body by tag. */
		public virtual void RemoveBody(int tag)
		{
			foreach (var body in _bodies)
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
			foreach (var child in _bodies)
			{
				RemoveBodyOrDelay(child);
				child._world = null;
			}

			_bodies.Clear();
		}


		/** Searches for physics shapes that intersects the ray. */
		public void RayCast(Func<CCPhysicsWorld, CCPhysicsRayCastInfo, object, bool> func, CCPoint point1, CCPoint point2, object data)
		{
			cp.AssertWarn(func != null, "func shouldn't be nullptr");

			if (func != null)
			{
				CCRayCastCallbackInfo info = new CCRayCastCallbackInfo(this, func, point1, point2, data);
				//Action<cpShape, cpVect, cpVect, float, object> func
				CCPhysicsWorldCallback.continues = true;

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
						world = this,
						func = func,
						data = data
					};

				CCPhysicsWorldCallback.continues = true;

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

				CCPhysicsWorldCallback.continues = true;

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

		/** Get all the bodys that in the physics world. */
		public List<CCPhysicsBody> GetAllBodies()
		{
			return _bodies;
		}

		/** Get body by tag */
		public CCPhysicsBody GetBody(int tag)
		{
			foreach (var body in _bodies)
			{
				if (body.Tag == tag)
				{
					return body;
				}
			}

			return null;
		}

		/** Get scene contain this physics world */
		public CCScene Scene { get { return _scene; } }

		/** get the gravity value */
		public CCPoint Gravity
		{
			get { return _gravity; }
			set
			{
				if (_bodies.Count > 0)
				{
					foreach (var body in _bodies)
					{
						// reset gravity for body
						if (!body.IsGravityEnabled())
						{
							body.ApplyForce(PhysicsHelper.CCPointToCpVect((_gravity - value)) * body.GetMass());
						}
					}
				}

				_gravity = value;
				_info.Gravity = PhysicsHelper.CCPointToCpVect(value);
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

		public int DebugDrawMask { get { return _debugDrawMask; } set { _debugDrawMask = value; } }


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
			_bodies.Add(body);
			body._world = this;

			return body;
		}

		public virtual CCPhysicsShape AddShape(CCPhysicsShape shape)
		{
			if (shape != null)
			{
				_info.AddShape(shape._info);
				return shape;
			}
			return null;
		}

		public virtual void RemoveShape(CCPhysicsShape shape)
		{
			if (shape != null)
			{
				_info.RemoveShape(shape._info);
			}
		}

		public virtual void Update(float delta)
		{
			while (_delayDirty)
			{
				// the updateJoints must run before the updateBodies.
				UpdateJoints();
				UpdateBodies();

				_delayDirty = !(_delayAddBodies.Count == 0 && _delayRemoveBodies.Count == 0 && _delayAddJoints.Count == 0 && _delayRemoveJoints.Count == 0);
			}

			_updateTime += delta;
			if (++_updateRateCount >= _updateRate)
			{
				_info.Step(_updateTime * _speed);
				foreach (var body in _bodies)
				{
					body.Update(_updateTime * _speed);
				}
				_updateRateCount = 0;
				_updateTime = 0.0f;
			}

			if (_debugDrawMask != DEBUGDRAW_NONE)
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

				if (!_joints.Exists(j => j == joint))
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

				_scene.DispatchEvent(contact);
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
			_scene.DispatchEvent(contact);

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
			_scene.DispatchEvent(contact);
		}

		public virtual void CollisionSeparateCallback(CCPhysicsContact contact)
		{
			if (!contact.IsNotificationEnabled())
			{
				return;
			}

			contact.SetEventCode(EventCode.SEPERATE);
			contact.SetWorld(this);
			_scene.DispatchEvent(contact);
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
					_info.AddBody(body._info);
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
			_info.RemoveBody(body._info);
		}

		public virtual void DoAddJoint(CCPhysicsJoint joint)
		{
			if (joint == null || joint._info == null)
			{
				return;
			}

			_info.AddJoint(joint._info);
		}

		public virtual void DoRemoveJoint(CCPhysicsJoint joint)
		{
			_info.RemoveJoint(joint._info);
		}

		public virtual CCPhysicsBody AddBodyOrDelay(CCPhysicsBody body)
		{
			CCPhysicsBody removeBodyIter = _delayRemoveBodies.Find(b => b == body);
			if (removeBodyIter != null)
			{
				_delayRemoveBodies.Remove(removeBodyIter);
				return null;
			}

			if (_info.IsLocked())
			{
				if (_delayAddBodies.Exists(b => b == body))
				{
					_delayAddBodies.Add(body);
					_delayDirty = true;
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
			if (_delayAddBodies.Exists(b => b == body))
			{
				_delayAddBodies.Remove(body);
				return;
			}

			if (_info.IsLocked())
			{
				if (_delayRemoveBodies.Exists(b => b == body))
				{
					_delayRemoveBodies.Add(body);
					_delayDirty = true;
				}
			}
			else
			{
				DoRemoveBody(body);
			}
		}

		public virtual void AddJointOrDelay(CCPhysicsJoint joint)
		{

			var it = _delayRemoveJoints.Find(j => j == joint);
			if (it != null)
			{
				_delayRemoveJoints.Remove(it);
				return;
			}

			if (_info.IsLocked())
			{

				if (!_delayAddJoints.Exists(j => j == joint))
				{
					_delayAddJoints.Add(joint);
					_delayDirty = true;
				}

			}
			else
			{
				DoAddJoint(joint);
			}
		}

		public virtual void RemoveJointOrDelay(CCPhysicsJoint joint)
		{
			var it = _delayAddJoints.Find(j => j == joint);
			if (it != null)
			{
				_delayAddJoints.Remove(it);
			}
			if (_info.IsLocked())
			{

				if (!_delayAddJoints.Exists(j => j == joint))
				{
					_delayRemoveJoints.Add(joint);
					_delayDirty = true;
				}
			}
			else
			{
				DoRemoveJoint(joint);
			}
		}

		public virtual void UpdateBodies()
		{
			if (_info.IsLocked())
			{
				return;
			}

			// Fixed: netonjm >  issue #4944, contact callback will be invoked when add/remove body, _delayAddBodies maybe changed, so we need make a copy.

			CCPhysicsBody[] addCopy = new CCPhysicsBody[_delayAddBodies.Count];
			_delayAddBodies.CopyTo(addCopy);
			_delayAddBodies.Clear();

			foreach (var body in addCopy)
			{
				DoAddBody(body);
			}

			CCPhysicsBody[] removeCopy = new CCPhysicsBody[_delayRemoveBodies.Count];
			_delayRemoveBodies.CopyTo(removeCopy);

			_delayRemoveBodies.Clear();

			foreach (var body in removeCopy)
			{
				DoRemoveBody(body);
			}
		}

		public virtual void UpdateJoints()
		{
			if (_info.IsLocked())
			{
				return;
			}

			CCPhysicsJoint[] addCopy = new CCPhysicsJoint[_delayAddJoints.Count];

			_delayAddJoints.CopyTo(addCopy);

			_delayAddJoints.Clear();

			foreach (var joint in addCopy)
			{
				DoAddJoint(joint);
			}

			CCPhysicsJoint[] removeCopy = new CCPhysicsJoint[_delayRemoveJoints.Count];
			_delayRemoveJoints.CopyTo(removeCopy);

			_delayRemoveJoints.Clear();
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




		public void SetGravity(cpVect gravity)
		{
			_info.SetGravity(gravity);
		}

		public void SetSleepTimeThreshold(float sleepTimeThreshold)
		{
			_info.SetSleepTimeThreshold(sleepTimeThreshold);
		}

		public void SetIterations(int iterations)
		{
			_info.SetIterations(iterations);
		}
	}

}
#endif