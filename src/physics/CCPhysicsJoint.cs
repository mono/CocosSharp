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
	/*
	 * @brief An PhysicsJoint object connects two physics bodies together.
	 */
	public class CCPhysicsJoint
	{

		#region PROTECTED PROPS

		public CCPhysicsBody _bodyA;
		public CCPhysicsBody _bodyB;
		public CCPhysicsWorld _world;
		internal CCPhysicsJointInfo _info;
		bool _enable;
		bool _collisionEnable;
		public bool _destoryMark;
		int _tag;

		#endregion

		public CCPhysicsJoint()
		{
			_bodyA = null;
			_bodyB = null;
			_world = null;
			_info = null;
			_enable = false;
			_collisionEnable = true;
			_destoryMark = false;
			_tag = 0;
		}

		~CCPhysicsJoint()
		{
			// reset the shapes collision group
			SetCollisionEnable(true);

		}


		#region PUBLIC FUNCTIONS

		public CCPhysicsBody GetBodyA() { return _bodyA; }
		public CCPhysicsBody GetBodyB() { return _bodyB; }
		public CCPhysicsWorld GetWorld() { return _world; }
		public int GetTag() { return _tag; }
		public void SetTag(int tag) { _tag = tag; }
		public bool IsEnabled() { return _enable; }
		/** Enable/Disable the joint */
		public void SetEnable(bool enable)
		{
			if (_enable != enable)
			{
				_enable = enable;

				if (_world != null)
				{
					if (enable)
					{
						_world.AddJointOrDelay(this);
					}
					else
					{
						_world.RemoveJointOrDelay(this);
					}
				}
			}
		}
		public bool IsCollisionEnabled() { return _collisionEnable; }
		/** Enable/disable the collision between two bodies */
		public void SetCollisionEnable(bool enable)
		{
			if (_collisionEnable != enable)
			{
				_collisionEnable = enable;
			}
		}
		/** Remove the joint from the world */
		public void RemoveFormWorld()
		{
			if (_world != null)
			{
				_world.RemoveJoint(this, false);
			}
		}
		/** Distory the joint*/
		public void Destroy()
		{
			// remove the joint and delete it.
			if (_world != null)
			{
				_world.RemoveJoint(this, true);
			}
			else
			{

				if (_bodyA != null)
				{
					_bodyA.RemoveJoint(this);
				}

				if (_bodyB != null)
				{
					_bodyB.RemoveJoint(this);
				}
			}

		}

		/** Set the max force between two bodies */
		public void SetMaxForce(float force)
		{
			foreach (cpConstraint joint in _info.getJoints())
			{
				joint.maxForce = force;
			}
		}

		/** Get the max force setting */
		public float GetMaxForce()
		{
			return _info.getJoints().FirstOrDefault().maxForce;
		}

		#endregion

		#region PROTECTED FUNCIONS

		protected bool Init(CCPhysicsBody a, CCPhysicsBody b)
		{
			cp.AssertWarn(a != null && b != null, "the body passed in is nil");
			cp.AssertWarn(a != b, "the two bodies are equal");
			_info = new CCPhysicsJointInfo(this);

			if (_info != null)
				return false;

			_bodyA = a;
			_bodyA._joints.Add(this);
			_bodyB = b;
			_bodyB._joints.Add(this);

			return true;
		}

		/**
		 * PhysicsShape is PhysicsBody's friend class, but all the subclasses isn't. so this method is use for subclasses to catch the bodyInfo from PhysicsBody.
		 */
		internal CCPhysicsBodyInfo GetBodyInfo(CCPhysicsBody body)
		{
			return body._info;
		}
		protected CCNode GetBodyNode(CCPhysicsBody body)
		{
			return body._node;
		}

		#endregion

	}

	/*
	 * @brief A fixed joint fuses the two bodies together at a reference point. Fixed joints are useful for creating complex shapes that can be broken apart later.
	 */
	public class CCPhysicsJointFixed : CCPhysicsJoint
	{

        public static CCPhysicsJointFixed Construct(CCPhysicsBody a, CCPhysicsBody b, CCPoint anchr)
		{
			CCPhysicsJointFixed joint = new CCPhysicsJointFixed();

            if (joint != null && joint.Init(a, b, anchr))
			{
				return joint;
			}

			// CC_SAFE_DELETE(joint);
			return null;
		}

		#region PROTECTED FUNC

        protected bool Init(CCPhysicsBody a, CCPhysicsBody b, CCPoint anchr)
		{

			if (!base.Init(a, b))
				return false;

			GetBodyNode(a).Position = anchr;
			GetBodyNode(b).Position = anchr;

			// add a pivot joint to fixed two body together
			//cpConstraint joint = cpPivotJoint.cpPivotJointNew(getBodyInfo(a).getBody(),
			//                                       getBodyInfo(b).getBody(),
			//                                      anchr);

            cpConstraint joint = new cpPivotJoint(GetBodyInfo(a).GetBody(), GetBodyInfo(b).GetBody(), PhysicsHelper.CCPointToCpVect(anchr));

			if (joint == null)
				return false;

			_info.Add(joint);


			// add a gear joint to make two body have the same rotation.
			joint = new cpGearJoint(GetBodyInfo(a).GetBody(), GetBodyInfo(b).GetBody(), 0, 1);

			if (joint == null)
				return false;

			_info.Add(joint);

			SetCollisionEnable(false);

			return true;
		}

		#endregion

	}

	/*
 * @brief A limit joint imposes a maximum distance between the two bodies, as if they were connected by a rope.
 */
	public class CCPhysicsJointLimit : CCPhysicsJoint
	{

		// static PhysicsJointFixed* ruct(PhysicsBody* a, PhysicsBody* b,  cpVect anchr);

		#region PUBLIC FUNC

		public static CCPhysicsJointLimit Construct(CCPhysicsBody a, CCPhysicsBody b, CCPoint anchr1, CCPoint anchr2, float min, float max)
		{
			CCPhysicsJointLimit joint = new CCPhysicsJointLimit();

			if (joint != null && joint.Init(a, b, anchr1, anchr2, min, max))
			{
				return joint;
			}

			return null;
		}

		public static CCPhysicsJointLimit Construct(CCPhysicsBody a, CCPhysicsBody b, CCPoint anchr1, CCPoint anchr2)
		{
			return Construct(a, b, anchr1, anchr2, 0,
                cpVect.cpvdist(b.Local2World(PhysicsHelper.CCPointToCpVect(anchr1)), a.Local2World(PhysicsHelper.CCPointToCpVect(anchr2))));
		}


		public CCPoint GetAnchr1()
		{
            return PhysicsHelper.cpVectToCCPoint(_info.getJoints().FirstOrDefault().GetAnchorA());
		}
		public void SetAnchr1(CCPoint anchr1)
		{
            _info.getJoints().FirstOrDefault().SetAnchorA(PhysicsHelper.CCPointToCpVect(anchr1));
		}
		public CCPoint GetAnchr2()
		{
			//getAnchr1
            return PhysicsHelper.cpVectToCCPoint(_info.getJoints().FirstOrDefault().GetAnchorB());
		}
		public void SetAnchr2(CCPoint anchr2)
		{
            _info.getJoints().FirstOrDefault().SetAnchorA(PhysicsHelper.CCPointToCpVect(anchr2));
		}
		public float GetMin()
		{
			return _info.getJoints().FirstOrDefault().GetMin();
		}
		public void SetMin(float min)
		{
			_info.getJoints().FirstOrDefault().SetMin(min);
		}

		public float GetMax()
		{
			return _info.getJoints().FirstOrDefault().GetMax();
		}
		public void SetMax(float max)
		{
			_info.getJoints().FirstOrDefault().SetMax(max);
		}
		#endregion


		#region PROTECTED FUNC

		protected bool Init(CCPhysicsBody a, CCPhysicsBody b, CCPoint anchr1, CCPoint anchr2, float min, float max)
		{

			if (!base.Init(a, b))
				return false;

			cpConstraint joint = new cpSlideJoint(GetBodyInfo(a).GetBody(), GetBodyInfo(b).GetBody(),
                PhysicsHelper.CCPointToCpVect(anchr1),
                PhysicsHelper.CCPointToCpVect(anchr2),
										  min,
										  max);

			if (joint == null)
				return false;

			_info.Add(joint);

			return true;

		}

		#endregion

	}


	/*
	 * @brief A pin joint allows the two bodies to independently rotate around the anchor point as if pinned together.
	 */
	public class CCPhysicsJointPin : CCPhysicsJoint
	{

		public static CCPhysicsJointPin Construct(CCPhysicsBody a, CCPhysicsBody b, CCPoint anchr)
		{
			CCPhysicsJointPin joint = new CCPhysicsJointPin();

			if (joint != null && joint.Init(a, b, anchr))
			{
				return joint;
			}

			return null;
		}

		//static PhysicsJointPin* ruct(PhysicsBody* a, PhysicsBody* b,  cpVect anchr);
		#region PROTECTED FUNC


        protected bool Init(CCPhysicsBody a, CCPhysicsBody b, CCPoint anchr)
		{
			if (!base.Init(a, b))
				return false;


			cpConstraint joint = new cpPivotJoint(GetBodyInfo(a).GetBody(), GetBodyInfo(b).GetBody(),
                PhysicsHelper.CCPointToCpVect(anchr));

			if (joint == null)
				return false;

			_info.Add(joint);

			return true;
		}

		#endregion

	}


	/** Set the fixed distance with two bodies */
	public class CCPhysicsJointDistance : CCPhysicsJoint
	{

		#region PUBLIC FUNC

		public float GetDistance()
		{
			return _info.getJoints().FirstOrDefault().GetDist();
		}
		public void SetDistance(float distance)
		{
			_info.getJoints().FirstOrDefault().SetDist(distance);
			//cpPinJointSetDist(_info->getJoints().front(), PhysicsHelper::float2cpfloat(distance));
		}
		#endregion

		public static CCPhysicsJointDistance Construct(CCPhysicsBody a, CCPhysicsBody b, CCPoint anchr1, CCPoint anchr2)
		{
			CCPhysicsJointDistance joint = new CCPhysicsJointDistance();
            if (joint != null && joint.Init(a, b, PhysicsHelper.CCPointToCpVect(anchr1), PhysicsHelper.CCPointToCpVect(anchr2)))
			{
				return joint;
			}
			return null;
		}

		#region PROTECTED FUNC

		bool Init(CCPhysicsBody a, CCPhysicsBody b, cpVect anchr1, cpVect anchr2)
		{

			if (!base.Init(a, b))
				return false;

			cpConstraint joint = new cpPinJoint(GetBodyInfo(a).GetBody(),
												GetBodyInfo(b).GetBody(),
											   anchr1,
											   anchr2);

			if (joint == null)
				return false;

			_info.Add(joint);

			return true;

		}
		#endregion

	}


	/** Connecting two physics bodies together with a spring. */
	public class CCPhysicsJointSpring : CCPhysicsJoint
	{
		#region PUBLIC FUNC
		public static CCPhysicsJointSpring Construct(CCPhysicsBody a, CCPhysicsBody b, CCPoint anchr1, CCPoint anchr2, float stiffness, float damping)
		{
			CCPhysicsJointSpring joint = new CCPhysicsJointSpring();

			if (joint != null && joint.Init(a, b, anchr1, anchr2, stiffness, damping))
			{
				return joint;
			}
			return null;
		}
		public CCPoint GetAnchr1()
		{
            return PhysicsHelper.cpVectToCCPoint(_info.getJoints().FirstOrDefault().GetAnchorA());
		}
		public void SetAnchr1(CCPoint anchr1)
		{
            _info.getJoints().FirstOrDefault().SetAnchorA(PhysicsHelper.CCPointToCpVect(anchr1));
		}
		public CCPoint GetAnchr2()
		{
            return PhysicsHelper.cpVectToCCPoint(_info.getJoints().FirstOrDefault().GetAnchorB());
		}
		public void SetAnchr2(CCPoint anchr2)
		{

                    _info.getJoints().FirstOrDefault().SetAnchorB(PhysicsHelper.CCPointToCpVect(anchr2));
		}
		public float GetRestLength()
		{
			return _info.getJoints().FirstOrDefault().GetRestLength();
		}

		public void SetRestLength(float restLength)
		{
			_info.getJoints().FirstOrDefault().SetRestLength(restLength);
		}

		public float GetStiffness()
		{
			return _info.getJoints().FirstOrDefault().GetStiffness();
		}
		public void SetStiffness(float stiffness)
		{
			_info.getJoints().FirstOrDefault().SetStiffness(stiffness);
		}

		public float GetDamping()
		{
			return _info.getJoints().FirstOrDefault().GetDamping();
		}

		public void SetDamping(float damping)
		{
			_info.getJoints().FirstOrDefault().SetDamping(damping);
		}
		#endregion


		#region PROTECTED FUNC

		protected bool Init(CCPhysicsBody a, CCPhysicsBody b, CCPoint anchr1, CCPoint anchr2, float stiffness, float damping)
		{
			if (!base.Init(a, b))
				return false;

            var anch1 = PhysicsHelper.CCPointToCpVect(anchr1);
            var anch2 = PhysicsHelper.CCPointToCpVect(anchr2);
			cpConstraint joint = new cpDampedSpring(GetBodyInfo(a).GetBody(),
													GetBodyInfo(b).GetBody(),
                anch1,
                anch2,
													cpVect.cpvdist(
                    _bodyB.Local2World(anch1), _bodyA.Local2World(anch2)),
													stiffness,
													damping);

			if (joint == null)
				return false;

			_info.Add(joint);

			return true;

		}

		#endregion

	}


	/** Attach body a to a line, and attach body b to a dot */
	public class CCPhysicsJointGroove : CCPhysicsJoint
	{


		#region PUBLIC FUNC

		public static CCPhysicsJointGroove Construct(CCPhysicsBody a, CCPhysicsBody b, CCPoint grooveA, CCPoint grooveB, CCPoint anchr2)
		{
			CCPhysicsJointGroove joint = new CCPhysicsJointGroove();

			if (joint != null && joint.Init(a, b, grooveA, grooveB, anchr2))
			{
				return joint;
			}

			return null;
		}

		public CCPoint GetGrooveA()
		{
            return PhysicsHelper.cpVectToCCPoint(_info.getJoints().FirstOrDefault().GetGrooveA());
		}
		public void SetGrooveA(CCPoint grooveA)
		{
            _info.getJoints().FirstOrDefault().SetGrooveA(PhysicsHelper.CCPointToCpVect(grooveA));
		}
		public CCPoint GetGrooveB()
		{
            return PhysicsHelper.cpVectToCCPoint(_info.getJoints().FirstOrDefault().GetGrooveB());
		}
		public void SetGrooveB(CCPoint grooveB)
		{
            _info.getJoints().FirstOrDefault().SetGrooveB(PhysicsHelper.CCPointToCpVect(grooveB));
		}
		public CCPoint GetAnchr2()
		{
            return PhysicsHelper.cpVectToCCPoint(_info.getJoints().FirstOrDefault().GetAnchorB());
		}
		public void SetAnchr2(CCPoint anchr2)
		{
            _info.getJoints().FirstOrDefault().SetAnchorB(PhysicsHelper.CCPointToCpVect(anchr2));
		}
		#endregion


		#region PROTECTED FUNC

		protected bool Init(CCPhysicsBody a, CCPhysicsBody b, CCPoint grooveA, CCPoint grooveB, CCPoint anchr)
		{

			if (!base.Init(a, b))
				return false;

			cpConstraint joint = new cpGrooveJoint(GetBodyInfo(a).GetBody(),
												   GetBodyInfo(b).GetBody(),
                PhysicsHelper.CCPointToCpVect(grooveA),
                PhysicsHelper.CCPointToCpVect(grooveB),
                PhysicsHelper.CCPointToCpVect(anchr));


			if (joint == null)
				return false;

			_info.Add(joint);


			return true;

		}

		#endregion

	}


	/** Likes a spring joint, but works with rotary */
	public class CCPhysicsJointRotarySpring : CCPhysicsJoint
	{
		#region PUBLIC FUNC

		public static CCPhysicsJointRotarySpring Construct(CCPhysicsBody a, CCPhysicsBody b, float stiffness, float damping)
		{
			CCPhysicsJointRotarySpring joint = new CCPhysicsJointRotarySpring();

			if (joint != null && joint.Init(a, b, stiffness, damping))
			{
				return joint;
			}

			return null;
		}

		public float GetRestAngle()
		{
			return _info.getJoints().FirstOrDefault().GetRestAngle();
		}
		public void SetRestAngle(float restAngle)
		{
			_info.getJoints().FirstOrDefault().SetRestAngle(restAngle);
		}
		public float GetStiffness()
		{
			return _info.getJoints().FirstOrDefault().GetStiffness();
		}
		public void SetStiffness(float stiffness)
		{
			_info.getJoints().FirstOrDefault().SetRestAngle(stiffness);
		}
		public float GetDamping()
		{
			return _info.getJoints().FirstOrDefault().GetDamping();
		}
		public void SetDamping(float damping)
		{
			_info.getJoints().FirstOrDefault().SetDamping(damping);
		}
		#endregion

		#region PROTECTED FUNC

		protected bool Init(CCPhysicsBody a, CCPhysicsBody b, float stiffness, float damping)
		{
			if (!base.Init(a, b))
				return false;

			cpConstraint joint = new cpDampedRotarySpring(GetBodyInfo(a).GetBody(),
												   GetBodyInfo(b).GetBody(),
													_bodyB.GetRotation() - _bodyA.GetRotation(),
													stiffness,
													damping);

			if (joint == null)
				return false;

			_info.Add(joint);


			return true;
		}

		#endregion
	}


	/** Likes a limit joint, but works with rotary */
	public class CCPhysicsJointRotaryLimit : CCPhysicsJoint
	{
		#region PUBLIC FUNC
		public static CCPhysicsJointRotaryLimit Construct(CCPhysicsBody a, CCPhysicsBody b, float min, float max)
		{
			CCPhysicsJointRotaryLimit joint = new CCPhysicsJointRotaryLimit();

			if (joint != null && joint.Init(a, b, min, max))
			{
				return joint;
			}

			return null;
		}
		public static CCPhysicsJointRotaryLimit Construct(CCPhysicsBody a, CCPhysicsBody b)
		{
			return Construct(a, b, 0.0f, 0.0f);
		}

		public float GetMin()
		{
			return _info.getJoints().FirstOrDefault().GetMin();
		}
		public void SetMin(float min)
		{
			_info.getJoints().FirstOrDefault().SetMin(min);
		}
		public float GetMax()
		{
			return _info.getJoints().FirstOrDefault().GetMax();
		}
		public void SetMax(float max)
		{
			_info.getJoints().FirstOrDefault().SetMax(max);
		}

		#endregion

		#region PROTECTED FUNC
		protected bool Init(CCPhysicsBody a, CCPhysicsBody b, float min, float max)
		{
			if (!base.Init(a, b))
				return false;

			cpConstraint joint = new cpRotaryLimitJoint(GetBodyInfo(a).GetBody(),
												   GetBodyInfo(b).GetBody(),
												   min, max);

			if (joint == null)
				return false;

			_info.Add(joint);


			return true;
		}


		#endregion
	}


	/** Works like a socket wrench. */
	public class CCPhysicsJointRatchet : CCPhysicsJoint
	{
		#region PUBLIC FUNC
		public static CCPhysicsJointRatchet Construct(CCPhysicsBody a, CCPhysicsBody b, float phase, float ratchet)
		{
			CCPhysicsJointRatchet joint = new CCPhysicsJointRatchet();

			if (joint != null && joint.Init(a, b, phase, ratchet))
			{
				return joint;
			}

			return null;
		}

		public float GetAngle()
		{
			return _info.getJoints().FirstOrDefault().GetAngle();
		}

		public void setAngle(float angle)
		{
			_info.getJoints().FirstOrDefault().SetAngle(angle);
		}

		public float GetPhase()
		{
			return _info.getJoints().FirstOrDefault().GetPhase();
		}
		public void SetPhase(float phase)
		{
			_info.getJoints().FirstOrDefault().SetPhase(phase);
		}

		public float GetRatchet()
		{
			return _info.getJoints().FirstOrDefault().GetRatchet();
		}

		public void SetRatchet(float ratchet)
		{
			_info.getJoints().FirstOrDefault().SetRatchet(ratchet);
		}


		#endregion

		#region PROTECTED FUNC

		protected bool Init(CCPhysicsBody a, CCPhysicsBody b, float phase, float ratchet)
		{
			if (!base.Init(a, b))
				return false;

			cpConstraint joint = new cpRatchetJoint(GetBodyInfo(a).GetBody(),
												   GetBodyInfo(b).GetBody(),
												   phase, ratchet);

			if (joint == null)
				return false;

			_info.Add(joint);


			return true;
		}

		#endregion
	}


	/** Keeps the angular velocity ratio of a pair of bodies constant. */
	public class CCPhysicsJointGear : CCPhysicsJoint
	{
		#region PUBLIC FUNC

		public static CCPhysicsJointGear Construct(CCPhysicsBody a, CCPhysicsBody b, float phase, float ratio)
		{
			CCPhysicsJointGear joint = new CCPhysicsJointGear();

			if (joint != null && joint.Init(a, b, phase, ratio))
			{
				return joint;
			}

			return null;
		}

		public float GetPhase()
		{
			return _info.getJoints().FirstOrDefault().GetPhase();
		}
		public void SetPhase(float phase)
		{
			_info.getJoints().FirstOrDefault().SetPhase(phase);
		}
		public float GetRatio()
		{
			return _info.getJoints().FirstOrDefault().GetRatio();
		}
		public void SetRatio(float ratchet)
		{
			_info.getJoints().FirstOrDefault().SetRatio(ratchet);
		}
		#endregion

		#region PROTECTED FUNC
		protected bool Init(CCPhysicsBody a, CCPhysicsBody b, float phase, float ratio)
		{
			if (!base.Init(a, b))
				return false;

			cpConstraint joint = new cpGearJoint(GetBodyInfo(a).GetBody(),
												   GetBodyInfo(b).GetBody(),
												   phase, ratio);

			if (joint == null)
				return false;

			_info.Add(joint);


			return true;
		}


		#endregion
	}

	/** Keeps the relative angular velocity of a pair of bodies constant */
	public class CCPhysicsJointMotor : CCPhysicsJoint
	{
		#region PUBLIC FUNC
		public static CCPhysicsJointMotor Construct(CCPhysicsBody a, CCPhysicsBody b, float rate)
		{
			CCPhysicsJointMotor joint = new CCPhysicsJointMotor();

			if (joint != null && joint.Init(a, b, rate))
			{
				return joint;
			}

			return null;
		}

		public float GetRate()
		{
			return _info.getJoints().FirstOrDefault().GetRate();
		}
		public void SetRate(float rate)
		{
			_info.getJoints().FirstOrDefault().SetRate(rate);
		}

		#endregion

		#region PROTECTED FUNC
		protected bool Init(CCPhysicsBody a, CCPhysicsBody b, float rate)
		{
			if (!base.Init(a, b))
				return false;

			cpConstraint joint = new cpSimpleMotor(GetBodyInfo(a).GetBody(),
												   GetBodyInfo(b).GetBody(),
												  rate);

			if (joint == null)
				return false;

			_info.Add(joint);


			return true;
		}


		#endregion
	}

}
#endif