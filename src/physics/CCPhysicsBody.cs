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

	public class CCPhysicsBody
	{

		public CCPhysicsMaterial PHYSICSBODY_MATERIAL_DEFAULT
		{
			get
			{
				return new CCPhysicsMaterial(0.1f, 0.5f, 0.5f);
			}
		}

		public cpBody Body { get { return _info.GetBody(); } }


		public const float MASS_DEFAULT = 1.0f;
		public const float MOMENT_DEFAULT = 200;

		#region PROTECTED PARAMETERS

		public CCNode _node;
		public List<CCPhysicsJoint> _joints = new List<CCPhysicsJoint>();
		protected List<CCPhysicsShape> _shapes = new List<CCPhysicsShape>();
		public CCPhysicsWorld _world;
		internal CCPhysicsBodyInfo _info;
		internal bool _dynamic;
		internal bool _enabled;
		internal bool _rotationEnabled;
		internal bool _gravityEnabled;
		internal bool _massDefault;
		internal bool _momentDefault;

		internal float _elasticity;
		internal float _friction;

		internal float _mass;
		internal float _area;
		internal float _density;
		internal float _moment;
		internal bool _isDamping;
		internal float _linearDamping;
		internal float _angularDamping;
		internal int _tag;

		internal int _categoryBitmask;
		internal int _collisionBitmask;
		internal int _contactTestBitmask;
		internal int _group;

		internal bool _positionResetTag;     /// To avoid reset the body position when body invoke Node::setPosition().
		internal bool _rotationResetTag;     /// To avoid reset the body rotation when body invoke Node::setRotation().
		internal cpVect _positionOffset;
		internal float _rotationOffset;



		#endregion

		public cpBodyType BodyType { get { return _info.GetBody().bodyType; } set { _info.GetBody().bodyType = value; } }


		/**
 * A body affect by physics.
 * it can attach one or more shapes.
 * if you create body with createXXX, it will automatically compute mass and moment with density your specified(which is PHYSICSBODY_MATERIAL_DEFAULT by default, and the density value is 0.1f), and it based on the formular: mass = density * area.
 * if you create body with createEdgeXXX, the mass and moment will be PHYSICS_INFINITY by default. and it's a static body.
 * you can change mass and moment with setMass() and setMoment(). and you can change the body to be dynamic or static by use function setDynamic().
 */
		public CCPhysicsBody()
			: this(MASS_DEFAULT, MOMENT_DEFAULT)
		{
		}

		public CCPhysicsBody(float mass)
			: this(mass, MOMENT_DEFAULT)
		{

		}

		public CCPhysicsBody(float mass, float moment)
            : this(mass, MOMENT_DEFAULT, CCPoint.Zero)
		{

		}



		public CCPhysicsBody(float mass, float moment, CCPoint offset)
		{
            var offsetcp = new cpVect(offset.X, offset.Y); ;
			_positionOffset = offsetcp != null ? offsetcp : cpVect.Zero;
			_node = null;
			_world = null;
			_info = null;
			_dynamic = true;
			_enabled = true;
			_rotationEnabled = true;
			_gravityEnabled = true;
			_massDefault = mass == MASS_DEFAULT;
			_momentDefault = moment == MOMENT_DEFAULT;
			_mass = mass;
			_area = 0.0f;
			_density = 0.0f;
			_moment = moment;
			_isDamping = false;
			_linearDamping = 0.0f;
			_angularDamping = 0.0f;
			_tag = 0;
			_categoryBitmask = int.MaxValue;//(UINT_MAX)
			_collisionBitmask = 0;
			_contactTestBitmask = int.MaxValue;
			_group = 0;
			_positionResetTag = false;
			_rotationResetTag = false;
			_rotationOffset = 0;

			_info = new CCPhysicsBodyInfo();
			_info.SetBody(new cpBody(_mass, _moment));

		}

		

		public CCPhysicsBody(CCPhysicsShape shape)
            : this(0f, 0f, CCPoint.Zero)
		{
			AddShape(shape);
		}


		public CCPhysicsBody(List<CCPhysicsShape> shapes)
            : this(0f, 0f, CCPoint.Zero)
		{

			foreach (var item in shapes)
			{
				AddShape(item);
			}

		}

		~CCPhysicsBody()
		{
			foreach (var joint in _joints)
			{
				CCPhysicsBody other = joint.GetBodyA() == this ? joint.GetBodyB() : joint.GetBodyA();
				other.RemoveJoint(joint);
			}
		}

		#region PROTECTED METHODS


		protected virtual void SetRotation(float rotation)
		{
			_info.GetBody().SetAngle(-((rotation + _rotationOffset) * (CCMathHelper.Pi / 180.0f)));
		}

		public void Update(float delta)
		{
			if (_node != null)
			{
				CCNode parent = _node.Parent;
				CCScene scene = _world.GetScene();
                var vec = new cpVect(Position.X, Position.Y);

				CCPoint parentPosition = new CCPoint((float)vec.x, (float)vec.y);   //ConvertToNodeSpace(scene.ConvertToWorldSpace(new CCPoint((float)vec.x, (float)vec.y)));

				cpVect position = parent != scene ?
					new cpVect(parentPosition.X, parentPosition.Y)
					: vec;

				//->convertToNodeSpace(scene->convertToWorldSpace(getPosition())) : getPosition();
				float rotation = GetRotation();
				for (; parent != scene; parent = parent.Parent)
				{
					rotation -= parent.RotationX;
				}

				_positionResetTag = true;
				_rotationResetTag = true;
				_node.Position = new CCPoint(position.x, position.y);
				_node.Rotation = (float)rotation;
				_positionResetTag = false;
				_rotationResetTag = false;

				// damping compute
				if (_isDamping && _dynamic && !IsResting())
				{
					_info.GetBody().v.x *= cp.cpfclamp(1.0f - delta * _linearDamping, 0.0f, 1.0f);
					_info.GetBody().v.y *= cp.cpfclamp(1.0f - delta * _linearDamping, 0.0f, 1.0f);
					_info.GetBody().w *= cp.cpfclamp(1.0f - delta * _angularDamping, 0.0f, 1.0f);
				}
			}
		}

		public void RemoveJoint(CCPhysicsJoint joint)
		{
			var it = _joints.Find((j) => j == joint);
			if (it != null)
				_joints.Remove(it);
		}

		protected void UpdateDamping() { _isDamping = _linearDamping != 0.0f || _angularDamping != 0.0f; }
		protected void UpdateMass(float oldMass, float newMass)
		{
          
			if (_dynamic && !_gravityEnabled && _world != null && oldMass != cp.PHYSICS_INFINITY)
			{

				ApplyForce(_world.GetGravity() * oldMass);
			}
			_info.GetBody().SetMass(newMass);

			if (_dynamic && !_gravityEnabled && _world != null && newMass != cp.PHYSICS_INFINITY)
			{
				ApplyForce(-_world.GetGravity() * newMass);
			}
		}

		#endregion

		#region PUBLIC METHODS


		/** Create a body contains a circle shape. */
		public static CCPhysicsBody CreateCircle(float radius, CCPhysicsMaterial material, CCPoint offset)
		{

			CCPhysicsBody body = new CCPhysicsBody();
			body.SetMass(MASS_DEFAULT);
            body.AddShape(new CCPhysicsShapeCircle(material, radius, offset));
			return body;
		}


		public static CCPhysicsBody CreateCircle(float radius, CCPoint offset)
		{
			return CreateCircle(radius, CCPhysicsMaterial.PHYSICSSHAPE_MATERIAL_DEFAULT, offset);
		}
		/** Create a body contains a box shape. */
		public static CCPhysicsBody CreateBox(CCSize size, CCPhysicsMaterial material, float radius)
		{
			CCPhysicsBody body = new CCPhysicsBody();
			body.AddShape(new CCPhysicsShapeBox(size, material, radius));
			return body;
		}


		public static CCPhysicsBody CreateBox(CCSize size, float radius)
		{

			return CreateBox(size, CCPhysicsMaterial.PHYSICSSHAPE_MATERIAL_DEFAULT, radius);
		}

		/**
		 * @brief Create a body contains a polygon shape.
		 * points is an array of cpVect structs defining a convex hull with a clockwise winding.
		 */
		public static CCPhysicsBody CreatePolygon(CCPoint[] points, int count, CCPhysicsMaterial material, float radius)
		{

			CCPhysicsBody body = new CCPhysicsBody();
			body.AddShape(new CCPhysicsShapePolygon(points, count, material, radius));
			return body;
		}

		public static CCPhysicsBody CreatePolygon(CCPoint[] points, int count, float radius)
		{
            return CreatePolygon(points, count, CCPhysicsMaterial.PHYSICSSHAPE_MATERIAL_DEFAULT, radius);
		}


		/** Create a body contains a EdgeSegment shape. */
		public static CCPhysicsBody CreateEdgeSegment(CCPoint a, CCPoint b, CCPhysicsMaterial material, float border = 1)
		{

			CCPhysicsBody body = new CCPhysicsBody();

            body.AddShape(new CCPhysicsShapeEdgeSegment(a, b, material, border));
			body._dynamic = false;
			return body;

		}

		public static CCPhysicsBody CreateEdgeSegment(CCPoint a, CCPoint b, float border = 1)
		{
            return CreateEdgeSegment(a, b , CCPhysicsMaterial.PHYSICSSHAPE_MATERIAL_DEFAULT, border);
		}



		/** Create a body contains a EdgeBox shape. */
		public static CCPhysicsBody CreateEdgeBox(CCSize size, CCPhysicsMaterial material, float border, CCPoint offset)
		{
			CCPhysicsBody body = new CCPhysicsBody();
            body.AddShape(new CCPhysicsShapeEdgeBox(size, material, offset, border));
			body._dynamic = false;
			return body;
		}

		public static CCPhysicsBody CreateEdgeBox(CCSize size, float border, CCPoint offset)
		{
            return CreateEdgeBox(size, CCPhysicsMaterial.PHYSICSSHAPE_MATERIAL_DEFAULT, border, offset);
		}


		/** Create a body contains a EdgePolygon shape. */
		public static CCPhysicsBody CreateEdgePolygon(CCPoint[] points, int count, CCPhysicsMaterial material, float border = 1)
		{
			CCPhysicsBody body = new CCPhysicsBody();
            body.AddShape(new CCPhysicsShapeEdgePolygon(points, count, material, border));
			body._dynamic = false;
			return body;
		}


        public static CCPhysicsBody CreateEdgePolygon(CCPoint[] points, int count, float border = 1)
		{
            return CreateEdgePolygon(points, count, CCPhysicsMaterial.PHYSICSSHAPE_MATERIAL_DEFAULT, border);
		}

		/** Create a body contains a EdgeChain shape. */
		public static CCPhysicsBody CreateEdgeChain(CCPoint[] points, int count, CCPhysicsMaterial material, float border = 1)
		{
			CCPhysicsBody body = new CCPhysicsBody();
            body.AddShape(new CCPhysicsShapeEdgeChain(points, count, material, border));
			body._dynamic = false;
			return body;
		}


		public static CCPhysicsBody CreateEdgeChain(CCPoint[] points, int count, float border = 1)
		{
            return CreateEdgeChain(points, count, CCPhysicsMaterial.PHYSICSSHAPE_MATERIAL_DEFAULT, border);
		}

		/*
		 * @brief add a shape to body
		 * @param shape the shape to be added
		 * @param addMassAndMoment if this is true, the shape's mass and moment will be added to body. the default is true
		 */
		public virtual CCPhysicsShape AddShape(CCPhysicsShape shape, bool addMassAndMoment = true)
		{
			if (shape == null) return null;

			// add shape to body
			if (!_shapes.Exists((s) => s == shape))
			{
				shape.SetBody(this);

				// calculate the area, mass, and desity
				// area must update before mass, because the density changes depend on it.
				if (addMassAndMoment)
				{
					_area += shape.GetArea();
					AddMass(shape.GetMass());
					AddMoment(shape.GetMoment());
				}

				if (_world != null)
				{
					_world.AddShape(shape);
				}

				_shapes.Add(shape);

				if (_group != cp.NO_GROUP && shape.GetGroup() == cp.NO_GROUP)
				{
					shape.SetGroup(_group);
				}
			}

			return shape;
		}
		/*
		 * @brief remove a shape from body
		 * @param shape the shape to be removed
		 * @param reduceMassAndMoment if this is true, the body mass and moment will be reduced by shape. the default is true
		 */
		public void RemoveShape(CCPhysicsShape shape, bool reduceMassAndMoment = true)
		{
			if (_shapes.Exists((s) => s == shape))
			{
				// deduce the area, mass and moment
				// area must update before mass, because the density changes depend on it.
				if (reduceMassAndMoment)
				{
					_area -= shape.GetArea();
					AddMass(-shape.GetMass());
					AddMoment(-shape.GetMoment());
				}

				//remove
				if (_world != null)
				{
					_world.RemoveShape(shape);
				}

				// set shape->_body = nullptr make the shape->setBody will not trigger the _body->removeShape function call.
				shape._body = null;
				shape.SetBody(null);

				_shapes.Remove(shape);
			}


		}
		/*
		 * @brief remove a shape from body
		 * @param tag the tag of the shape to be removed
		 * @param reduceMassAndMoment if this is true, the body mass and moment will be reduced by shape. the default is true
		 */
		public void RemoveShape(int tag, bool reduceMassAndMoment = true)
		{
			foreach (var shape in _shapes)
			{
				if (shape.GetTag() == tag)
				{
					RemoveShape(shape, reduceMassAndMoment);
					return;
				}
			}
		}
		/* remove all shapes */
		public void RemoveAllShapes(bool reduceMassAndMoment = true)
		{
			// CCPhysicsShape shape;
			foreach (CCPhysicsShape shape in _shapes)
			{
				// area must update before mass, because the density changes depend on it.
				if (reduceMassAndMoment)
				{
					_area -= shape.GetArea();
					AddMass(-shape.GetMass());
					AddMoment(-shape.GetMoment());
				}

				if (_world != null)
				{
					_world.RemoveShape(shape);
				}

				// set shape->_body = nullptr make the shape->setBody will not trigger the _body->removeShape function call.
				shape._body = null;
				shape.SetBody(null);
			}
			_shapes.Clear();
		}
		/* get the body shapes. */
		public List<CCPhysicsShape> GetShapes() { return _shapes; }
		/* get the first shape of the body shapes. */
		public CCPhysicsShape GetFirstShape() { return _shapes.FirstOrDefault(); }
		/* get the shape of the body. */
		public CCPhysicsShape GetShape(int tag)
		{

			foreach (var shape in _shapes)
			{
				if (shape.GetTag() == tag)
				{
					return shape;
				}
			}

			return null;

		}

		/** Applies a immediate force to body. */
		public virtual void ApplyForce(CCPoint force)
		{
            ApplyForce(force, CCPoint.Zero);
		}
		/** Applies a immediate force to body. */
        public virtual void ApplyForce(CCPoint force, CCPoint offset)
		{
            ApplyForce(new cpVect(force.X,force.Y), new cpVect(offset.X,offset.Y));
		}

        /** Applies a immediate force to body. */
        internal void ApplyForce(cpVect force)
        {
            ApplyForce(force, cpVect.Zero);
        }

        /** Applies a immediate force to body. */
        internal void ApplyForce(cpVect force, cpVect offset)
        {
            if (_dynamic && _mass != cp.PHYSICS_INFINITY)
            {
                _info.GetBody().ApplyForce(force, offset);
            }
        }

		/** reset all the force applied to body. */
		public virtual void ResetForces()
		{
			_info.GetBody().ResetForces();
			// if _gravityEnabled is false, add a reverse of gravity force to body
			if (_world != null && _dynamic && !_gravityEnabled && _mass != cp.PHYSICS_INFINITY)
			{
				ApplyForce(-_world.GetGravity() * _mass);
			}
		}

        /** Applies a continuous force to body. */
		public virtual void ApplyImpulse(CCPoint impulse)
		{
            ApplyImpulse(impulse, CCPoint.Zero);
		}
		/** Applies a continuous force to body. */
		public virtual void ApplyImpulse(CCPoint impulse, CCPoint offset)
		{
			// cpBodyApplyImpulse(_info->getBody(), PhysicsHelper::point2cpv(impulse), PhysicsHelper::point2cpv(offset));
            ApplyImpulse(new cpVect(impulse.X,impulse.Y), new cpVect(offset.X,offset.Y));
		}

        /** Applies a continuous force to body. */
        internal void ApplyImpulse(cpVect impulse)
        {
            ApplyImpulse(impulse, cpVect.Zero);
        }
        /** Applies a continuous force to body. */
        internal void ApplyImpulse(cpVect impulse, cpVect offset)
        {
            // cpBodyApplyImpulse(_info->getBody(), PhysicsHelper::point2cpv(impulse), PhysicsHelper::point2cpv(offset));
            _info.GetBody().ApplyImpulse(impulse, offset);
        }


		/** Applies a torque force to body. */
		public virtual void ApplyTorque(float torque)
		{
			_info.GetBody().SetTorque(torque);
		}

		/** set the velocity of a body */
        public virtual void SetVelocity(CCPoint velocity)
		{
			if (!_dynamic)
			{
				cp.AssertWarn("physics warning: your can't set velocity for a static body.");
				return;
			}
            _info.GetBody().SetPosition(new cpVect(velocity.X,velocity.Y));
			//cpBodySetVel(_info->getBody(), PhysicsHelper::point2cpv(velocity));
		}

		/** get the velocity of a body */
		public virtual CCPoint GetVelocity()
		{
            var velocity = _info.GetBody().GetVelocity();// getVel();
            return new CCPoint(velocity.x, velocity.y);
		}
		/** set the angular velocity of a body */
		public virtual void SetAngularVelocity(float velocity)
		{
			if (!_dynamic)
			{
				cp.AssertWarn("physics warning: your can't set angular velocity for a static body.");
				return;
			}

			_info.GetBody().SetAngularVelocity(velocity);
			//cpBodySetAngVel(_info->getBody(), PhysicsHelper::float2cpfloat(velocity));
		}
		/** get the angular velocity of a body at a local point */
		public virtual CCPoint GetVelocityAtLocalPoint(CCPoint point)
		{
            var cpVec = GetVelocityAtLocalPoint(new cpVect(point.X,point.Y));
            return new CCPoint(cpVec.x, cpVec.y);
		}
		/** get the angular velocity of a body at a world point */
		public virtual CCPoint GetVelocityAtWorldPoint(CCPoint point)
		{
            var cpVec = GetVelocityAtWorldPoint(new cpVect(point.X,point.Y));
            return new CCPoint(cpVec.x, cpVec.y);
		}

        /** get the angular velocity of a body at a local point */
        internal cpVect GetVelocityAtLocalPoint(cpVect point)
        {
            return _info.GetBody().GetVelocityAtLocalPoint(point);
        }
        /** get the angular velocity of a body at a world point */
        internal cpVect GetVelocityAtWorldPoint(cpVect point)
        {
            return _info.GetBody().GetVelocityAtWorldPoint(point);
        }

		/** get the angular velocity of a body */
		public virtual float GetAngularVelocity()
		{
			return _info.GetBody().GetAngularVelocity(); //GetAngVel();
		}
		/** set the max of velocity */
		//public virtual void SetVelocityLimit(float limit)
		//{
		//	_info.GetBody(). VelLimit = limit;// SetVelLimit(limit);
		//}
		/** get the max of velocity */
		//public virtual float GetVelocityLimit()
		//{
		//	return _info.GetBody().VelLimit; // getVelLimit();
		//}
		/** set the max of angular velocity */
		//public virtual void SetAngularVelocityLimit(float limit)
		//{
		//	_info.GetBody().AngVelLimit = limit; // SetAngVelLimit(limit);
		//}
		/** get the max of angular velocity */
		//public virtual float GetAngularVelocityLimit()
		//{
		//	return _info.GetBody().AngVelLimit; // getAngVelLimit();
		//}

		/** remove the body from the world it added to */
		public void RemoveFromWorld()
		{
			if (_world != null)
			{
				_world.RemoveBody(this);
			}
		}

		/** get the world body added to. */
		public CCPhysicsWorld GetWorld() { return _world; }
		/** get all joints the body have */
		public List<CCPhysicsJoint> GetJoints() { return _joints; }

		/** get the sprite the body set to. */
		public CCNode GetNode() { return _node; }

		/**
		 * A mask that defines which categories this physics body belongs to.
		 * Every physics body in a scene can be assigned to up to 32 different categories, each corresponding to a bit in the bit mask. You define the mask values used in your game. In conjunction with the collisionBitMask and contactTestBitMask properties, you define which physics bodies interact with each other and when your game is notified of these interactions.
		 * The default value is 0xFFFFFFFF (all bits set).
		 */
		public void SetCategoryBitmask(int bitmask)
		{

			_categoryBitmask = bitmask;

			foreach (var shape in _shapes)
			{
				shape.SetCategoryBitmask(bitmask);
			}

		}
		/** 
		 * A mask that defines which categories of bodies cause intersection notifications with this physics body.
		 * When two bodies share the same space, each body’s category mask is tested against the other body’s contact mask by performing a logical AND operation. If either comparison results in a non-zero value, an PhysicsContact object is created and passed to the physics world’s delegate. For best performance, only set bits in the contacts mask for interactions you are interested in.
		 * The default value is 0x00000000 (all bits cleared).
		 */
		public void SetContactTestBitmask(int bitmask)
		{
			_contactTestBitmask = bitmask;

			foreach (var shape in _shapes)
			{
				shape.SetContactTestBitmask(bitmask);
			}
		}
		/**
		 * A mask that defines which categories of physics bodies can collide with this physics body.
		 * When two physics bodies contact each other, a collision may occur. This body’s collision mask is compared to the other body’s category mask by performing a logical AND operation. If the result is a non-zero value, then this body is affected by the collision. Each body independently chooses whether it wants to be affected by the other body. For example, you might use this to avoid collision calculations that would make negligible changes to a body’s velocity.
		 * The default value is 0xFFFFFFFF (all bits set).
		 */
		public void SetCollisionBitmask(int bitmask)
		{
			_collisionBitmask = bitmask;

			foreach (var shape in _shapes)
			{
				shape.SetCollisionBitmask(bitmask);
			}
		}
		/** get the category bit mask */
		public int GetCategoryBitmask() { return _categoryBitmask; }
		/** get the contact test bit mask */
		public int GetContactTestBitmask() { return _contactTestBitmask; }
		/** get the collision bit mask */
		public int GetCollisionBitmask() { return _collisionBitmask; }

		/** 
		 * set the group of body
		 * Collision groups let you specify an integral group index. You can have all fixtures with the same group index always collide (positive index) or never collide (negative index)
		 * it have high priority than bit masks
		 */
		public void SetGroup(int group)
		{
			foreach (var shape in _shapes)
			{
				shape.SetGroup(group);
			}
		}
		/** get the group of body */
		public int GetGroup() { return _group; }


		/** get the body position. */
        public CCPoint Position
		{
			get
			{
				var vec = _info.GetBody().GetPosition();
                var vecp = vec - _positionOffset;
                return new CCPoint(vecp.x, vecp.y);

			}
			set
			{
                var newpos = new cpVect(value.X, value.Y);
				_info.GetBody().SetPosition(newpos + _positionOffset);
			}
		}

		/** get the body rotation. */
		public float GetRotation()
		{
			return -(_info.GetBody().GetAngle() * 180f / cp.M_PI) - _rotationOffset;
			//return -(_info.GetBody().GetAngle() * (180.0 / CCMathHelper.Pi)) - _rotationOffset;
		}

		/** set body position offset, it's the position witch relative to node */
        public void setPositionOffset(CCPoint position)
		{
            setPositionOffset(new cpVect(position.X, position.Y));
		}

        /** set body position offset, it's the position witch relative to node */
        internal void setPositionOffset(cpVect position)
        {
            if (!_positionOffset.Equals(position))
            {
                var pos = Position;
                _positionOffset = position;
                Position = pos;// setPosition(pos);
            }
        }

		/** get body position offset. */
		public CCPoint GetPositionOffset()
		{
            var posOff = new CCPoint(_positionOffset.x, _positionOffset.y);
			return posOff;
		}

        /** set body rotation offset, it's the rotation witch relative to node */
		public void SetRotationOffset(float rotation)
		{
			if (Math.Abs(_rotationOffset - rotation) > 0.5f)
			{
				float rot = GetRotation();
				_rotationOffset = rotation;
				SetRotation(rot);
			}
		}
		/** set the body rotation offset */
		public float GetRotationOffset()
		{
			return _rotationOffset;
		}

		/**
		 * @brief test the body is dynamic or not.
		 * a dynamic body will effect with gravity.
		 */
		public bool IsDynamic() { return _dynamic; }
		/**
		 * @brief set dynamic to body.
		 * a dynamic body will effect with gravity.
		 */
		public void SetDynamic(bool dynamic)
		{
			if (dynamic != _dynamic)
			{
				_dynamic = dynamic;
				if (dynamic)
				{
					_info.GetBody().SetMass(_mass);
					_info.GetBody().SetMoment(_moment);

					if (_world != null)
					{
						// reset the gravity enable
						if (IsGravityEnabled())
						{
							_gravityEnabled = false;
							SetGravityEnable(true);
						}

						// _world._info
						_world._info.Space.AddBody(_info.GetBody());
					}
				}
				else
				{
					if (_world != null)
					{
						_world._info.Space.RemoveBody(_info.GetBody());
						// cpSpaceRemoveBody(_world->_info->getSpace(), _info->getBody());
					}

					// avoid incorrect collion simulation.
					var body = _info.GetBody();
					body.SetMass(CCPhysicsBody.MASS_DEFAULT);
					body.SetMoment(CCPhysicsBody.MOMENT_DEFAULT);
					body.SetVelocity(cpVect.Zero);
					body.SetAngularVelocity(0.0f);

					ResetForces();
				}

			}
		}

		/**
		 * @brief set the body mass.
		 * @note if you need add/subtract mass to body, don't use setMass(getMass() +/- mass), because the mass of body may be equal to PHYSICS_INFINITY, it will cause some unexpected result, please use addMass() instead.
		 */
		public void SetMass(float mass)
		{
			if (mass <= 0)
			{
				return;
			}

			float oldMass = _mass;
			_mass = mass;
			_massDefault = false;

			// update density
			if (_mass == cp.PHYSICS_INFINITY)
			{
				_density = cp.PHYSICS_INFINITY;
			}
			else
			{
				if (_area > 0)
				{
					_density = _mass / _area;
				}
				else
				{
					_density = 0;
				}
			}

			// the static body's mass and moment is always infinity
			if (_dynamic)
			{
				UpdateMass(oldMass, _mass);
			}
		}
		/** get the body mass. */
		public float GetMass() { return _mass; }
		/**
		 * @brief add mass to body.
		 * if _mass(mass of the body) == PHYSICS_INFINITY, it remains.
		 * if mass == PHYSICS_INFINITY, _mass will be PHYSICS_INFINITY.
		 * if mass == -PHYSICS_INFINITY, _mass will not change.
		 * if mass + _mass <= 0, _mass will equal to MASS_DEFAULT(1.0)
		 * other wise, mass = mass + _mass;
		 */
		public void AddMass(float mass)
		{
			float oldMass = _mass;

			if (mass == cp.PHYSICS_INFINITY)
			{
				_mass = cp.PHYSICS_INFINITY;
				_massDefault = false;
				_density = cp.PHYSICS_INFINITY;
			}
			else if (mass == -cp.PHYSICS_INFINITY)
			{
				return;
			}
			else
			{
				if (_massDefault)
				{
					_mass = 0;
					_massDefault = false;
				}

				if (_mass + mass > 0)
				{
					_mass += mass;
				}
				else
				{
					_mass = MASS_DEFAULT;
					_massDefault = true;
				}

				if (_area > 0)
				{
					_density = _mass / _area;
				}
				else
				{
					_density = 0;
				}
			}

			// the static body's mass and moment is always infinity
			if (_dynamic)
			{
				UpdateMass(oldMass, _mass);
			}
		}

		/**
		 * @brief set the body moment of inertia.
		 * @note if you need add/subtract moment to body, don't use setMoment(getMoment() +/- moment), because the moment of body may be equal to PHYSICS_INFINITY, it will cause some unexpected result, please use addMoment() instead.
		 */
		public void SetMoment(float moment)
		{
			_moment = moment;
			_momentDefault = false;

			// the static body's mass and moment is always infinity
			if (_rotationEnabled && _dynamic)
			{
				_info.GetBody().SetMoment(moment); //   cpBodySetMoment(_info->getBody(), PhysicsHelper::float2cpfloat(_moment));
			}

		}
		/** get the body moment of inertia. */
		public float GetMoment() { return _moment; }
		/**
		 * @brief add moment of inertia to body.
		 * if _moment(moment of the body) == PHYSICS_INFINITY, it remains.
		 * if moment == PHYSICS_INFINITY, _moment will be PHYSICS_INFINITY.
		 * if moment == -PHYSICS_INFINITY, _moment will not change.
		 * if moment + _moment <= 0, _moment will equal to MASS_DEFAULT(1.0)
		 * other wise, moment = moment + _moment;
		 */
		public void AddMoment(float moment)
		{

			if (moment == cp.PHYSICS_INFINITY)
			{
				// if moment is PHYSICS_INFINITY, the moment of the body will become PHYSICS_INFINITY
				_moment = (float)cp.PHYSICS_INFINITY;
				_momentDefault = false;
			}
			else if (moment == -cp.PHYSICS_INFINITY)
			{
				return;
			}
			else
			{
				// if moment of the body is PHYSICS_INFINITY is has no effect
				if (_moment != cp.PHYSICS_INFINITY)
				{
					if (_momentDefault)
					{
						_moment = 0;
						_momentDefault = false;
					}

					if (_moment + moment > 0)
					{
						_moment += moment;
					}
					else
					{
						_moment = MOMENT_DEFAULT;
						_momentDefault = true;
					}
				}
			}

			// the static body's mass and moment is always infinity
			if (_rotationEnabled && _dynamic)
			{
				//cpBodySetMoment(, PhysicsHelper::float2cpfloat(_moment));
				_info.GetBody().SetMoment(_moment);
			}

		}
		/** get linear damping. */
		public float GetLinearDamping() { return _linearDamping; }
		/** 
		 * set linear damping.
		 * it is used to simulate fluid or air friction forces on the body. 
		 * the value is 0.0f to 1.0f. 
		 */
		public void SetLinearDamping(float damping) { _linearDamping = damping; UpdateDamping(); }
		/** get angular damping. */
		public float GetAngularDamping() { return _angularDamping; }
		/**
		 * set angular damping.
		 * it is used to simulate fluid or air friction forces on the body.
		 * the value is 0.0f to 1.0f.
		 */
		public void SetAngularDamping(float damping) { _angularDamping = damping; UpdateDamping(); }

		/** whether the body is at rest */
		public bool IsResting()
		{
			//TODO: it's not clear  new cpBody(0, 0) = ((cpBody*)0);
			return _info.GetBody().nodeRoot != null; //  (_info->getBody()->node).root != ((cpBody*)0);
		}
		/** set body to rest */
		public void SetResting(bool rest)
		{
			if (rest && !IsResting())
			{
				_info.GetBody().Sleep();
			}
			else if (!rest && IsResting())
			{
				_info.GetBody().Activate();
			}
		}
		/** 
		 * whether the body is enabled
		 * if the body it isn't enabled, it will not has simulation by world
		 */
		public bool IsEnabled() { return _enabled; }
		/**
		 * set the enable value.
		 * if the body it isn't enabled, it will not has simulation by world
		 */
		public void SetEnable(bool enable)
		{
			if (_enabled != enable)
			{
				_enabled = enable;

				if (_world != null)
				{
					if (enable)
					{
						_world.AddBodyOrDelay(this);
					}
					else
					{
						_world.RemoveBodyOrDelay(this);
					}
				}
			}
		}

		/** whether the body can rotation */
		public bool IsRotationEnabled() { return _rotationEnabled; }
		/** set the body is allow rotation or not */
		public void SetRotationEnable(bool enable)
		{
			if (_rotationEnabled != enable)
			{
				_info.GetBody().SetMoment(enable ? _moment : cp.Infinity);
				_rotationEnabled = enable;
			}
		}

		/** whether this physics body is affected by the physics world’s gravitational force. */
		public bool IsGravityEnabled() { return _gravityEnabled; }
		/** set the body is affected by the physics world's gravitational force or not. */
		public void SetGravityEnable(bool enable)
		{
			if (_gravityEnabled != enable)
			{
				_gravityEnabled = enable;

				if (_world != null)
				{
					if (enable)
					{
						ApplyForce(_world.GetGravity() * _mass);
					}
					else
					{
						ApplyForce(-_world.GetGravity() * _mass);
					}
				}
			}
		}

		/** get the body's tag */
		public int GetTag() { return _tag; }
		/** set the body's tag */
		public void setTag(int tag) { _tag = tag; }

		/** convert the world point to local */
		public CCPoint World2Local(CCPoint point)
		{
            var pp = World2Local(new cpVect(point.X, point.Y));
            return new CCPoint(pp.x, pp.y);
			//return PhysicsHelper::cpv2point(cpBodyWorld2Local(_info->getBody(), PhysicsHelper::point2cpv(point)));
		}

		/** convert the local point to world */
		public CCPoint Local2World(CCPoint point)
		{
            var pp = Local2World(new cpVect(point.X, point.Y));
            return new CCPoint(pp.x, pp.y);
		}

        /** convert the world point to local */
        internal cpVect World2Local(cpVect point)
        {
            return _info.GetBody().WorldToLocal(point);
            //return PhysicsHelper::cpv2point(cpBodyWorld2Local(_info->getBody(), PhysicsHelper::point2cpv(point)));
        }
        /** convert the local point to world */
        internal cpVect Local2World(cpVect point)
        {
            return _info.GetBody().LocalToWorld(point);
        }
        #endregion


		public void SetScale(float scale)
		{
			foreach (var shape in _shapes)
			{
				shape.SetScale(scale);
			}
		}

		public void SetScale(float scaleX, float scaleY)
		{
			foreach (var shape in _shapes)
			{
				shape.SetScale(scaleX, scaleY);
			}
		}

		public void SetScaleX(float scaleX)
		{
			foreach (var shape in _shapes)
			{
				shape.SetScaleX(scaleX);
			}
		}
		public void SetScaleY(float scaleY)
		{
			foreach (var shape in _shapes)
			{
				shape.SetScaleY(scaleY);
			}
		}


		public void SetType(cpBodyType type)
		{
			_info.GetBody().SetBodyType(type);
		}

		public float GetElasticity()
		{
			return _elasticity;
		}

		public void SetElasticity(float elasticity)
		{
			_elasticity = elasticity;
			foreach (var shape in _shapes)
			{
				shape.SetElasticity(elasticity);
			}
		}


		public float GetFriction()
		{
			return _friction;
		}

		public void SetFriction(float friction)
		{
			_friction = friction;
			foreach (var shape in _shapes)
			{
				shape.SetFriction(friction);
			}
		}

		public void SetFilter(cpShapeFilter filter)
		{
			SetGroup(filter.group);
			SetCategoryBitmask(filter.categories);
			SetCollisionBitmask(filter.mask);
		}

		public void SetAngle(float angle)
		{
			_info.GetBody().SetAngle(angle);
		}

		public void SetVelocityUpdateFunc(Action<cpVect, float, float> velocityFunc)
		{
			_info.GetBody().SetVelocityUpdateFunc(velocityFunc);
		}


	
	}
}
#endif