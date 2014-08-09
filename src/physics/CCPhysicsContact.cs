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

	public enum EventCode
	{
		NONE = 1,
		BEGIN = 2,
		PRESOLVE = 3,
		POSTSOLVE = 4,
		SEPERATE = 5
	};

	public class CCPhysicsContactData
	{
		public const int POINT_MAX = 4;
		public List<cpVect> points = new List<cpVect>();
		public int count
		{
			get
			{
				return (points != null) ? points.Count : 0;
			}
		}
		public cpVect normal;
	};

	/**  * @brief Contact infomation. it will created automatically when two shape contact with each other. and it will destoried automatically when two shape separated. */
	public class CCPhysicsContact : CCEventCustom
	{

		public const string PHYSICSCONTACT_EVENT_NAME = "PhysicsContactEvent";


		#region PRIVATE PROPS

		CCPhysicsWorld _world;
		CCPhysicsShape _shapeA;
		CCPhysicsShape _shapeB;
		EventCode _eventCode;
		CCPhysicsContactInfo _info;
		bool _notificationEnable;
		bool _result;

		object _data;
		public object _contactInfo;
		CCPhysicsContactData _contactData;
		CCPhysicsContactData _preContactData;

		#endregion

		public CCPhysicsContact()
			: base(PHYSICSCONTACT_EVENT_NAME)
		{
			_world = null;
			_shapeA = null;
			_shapeB = null;
			_eventCode = EventCode.NONE;
			_info = null;
			_notificationEnable = true;
			_result = true;
			_data = null;
			_contactInfo = null;
			_contactData = null;
			_preContactData = null;
		}

		public CCPhysicsContact(CCPhysicsShape a, CCPhysicsShape b)
			: this()
		{
			Init(a, b);
		}

		#region PUBLIC METHODS

		/** get contact shape A. */
		public CCPhysicsShape GetShapeA() { return _shapeA; }
		/** get contact shape B. */
		public CCPhysicsShape GetShapeB() { return _shapeB; }
		/** get contact data */
		public CCPhysicsContactData GetContactData() { return _contactData; }
		/** get previous contact data */
		public CCPhysicsContactData GetPreContactData() { return _preContactData; }
		/** get data. */
		public object GetData() { return _data; }
		/**
		 * @brief set data to contact. you must manage the memory yourself, Generally you can set data at contact begin, and distory it at contact seperate.
		 */
		public void SetData(object data) { _data = data; }
		/** get the event code */
		public EventCode GetEventCode() { return _eventCode; }

		#endregion

		#region PRIVATE METHODS

		//public static PhysicsContact Construct(PhysicsShape a, PhysicsShape b)
		//{
		//	PhysicsContact contact = new PhysicsContact();
		//	if (contact != null && contact.Init(a, b))
		//	{
		//		return contact;
		//	}
		//	return null;
		//}

		bool Init(CCPhysicsShape a, CCPhysicsShape b)
		{
			if (a == null || b == null)
				return false;

			_info = new CCPhysicsContactInfo(this);

			if (_info != null)
				return false;

			_shapeA = a;
			_shapeB = b;

			return true;
		}

		public void SetEventCode(EventCode eventCode) { _eventCode = eventCode; }
		public bool IsNotificationEnabled() { return _notificationEnable; }
		public void SetNotificationEnable(bool enable) { _notificationEnable = enable; }
		CCPhysicsWorld GetWorld() { return _world; }
		public void SetWorld(CCPhysicsWorld world) { _world = world; }
		public void SetResult(bool result) { _result = result; }
		public bool ResetResult() { bool ret = _result; _result = true; return ret; }

		public void GenerateContactData()
		{

			if (_contactInfo == null)
			{
				return;
			}

			cpArbiter arb = (cpArbiter)_contactInfo;

			_preContactData = _contactData;
			_contactData = new CCPhysicsContactData();

			for (int i = 0; i < _contactData.count && i < CCPhysicsContactData.POINT_MAX; ++i)
			{
				_contactData.points[i] = arb.GetPointA(i);
			}

			_contactData.normal = _contactData.count > 0 ? arb.GetNormal() : cpVect.Zero;
		}

		#endregion

	}

	/* * @brief presolve value generated when onContactPreSolve called. */
	public class CCPhysicsContactPreSolve
	{
		private object _contactInfo;
		public CCPhysicsContactPreSolve(object contactInfo)
		{
			_contactInfo = contactInfo;
		}

		#region PUBLIC METHODS

		/** get restitution between two bodies*/
		public float GetRestitution()
		{
			return (_contactInfo as cpArbiter).e;
		}
		/** get friction between two bodies*/
		public float GetFriction()
		{
			return (_contactInfo as cpArbiter).u;
		}
		/** get surface velocity between two bodies*/
		public cpVect GetSurfaceVelocity()
		{
			return (_contactInfo as cpArbiter).surface_vr;
		}
		/** set the restitution*/
		public void SetRestitution(float restitution)
		{
			(_contactInfo as cpArbiter).e = restitution;
		}
		/** set the friction*/
		public void SetFriction(float friction)
		{
			(_contactInfo as cpArbiter).u = friction;
		}
		/** set the surface velocity*/
		public void SetSurfaceVelocity(cpVect velocity)
		{
			(_contactInfo as cpArbiter).surface_vr = velocity;
		}
		/** ignore the rest of the contact presolve and postsolve callbacks */
		public void Ignore()
		{
			(_contactInfo as cpArbiter).Ignore();
		}

		#endregion



	}

	/*  * @brief postsolve value generated when onContactPostSolve called. */
	public class CCPhysicsContactPostSolve
	{

		public CCPhysicsContactPostSolve(object contactInfo)
		{
			_contactInfo = contactInfo;
		}


		private object _contactInfo;

		#region PUBLIC METHODS

		/** get restitution between two bodies*/
		float GetRestitution()
		{
			return (_contactInfo as cpArbiter).e;
		}
		/** get friction between two bodies*/
		float GetFriction()
		{
			return (_contactInfo as cpArbiter).u;
		}
		/** get surface velocity between two bodies*/
		cpVect GetSurfaceVelocity()
		{

			return (_contactInfo as cpArbiter).surface_vr;

		}

		#endregion

	}

	/* contact listener. it will recive all the contact callbacks. */
	public class EventListenerPhysicsContact : CCEventListenerCustom
	{

		public EventListenerPhysicsContact()
			: base(CCPhysicsContact.PHYSICSCONTACT_EVENT_NAME, null)
		{
			onContactBegin = null;
			onContactPreSolve = null;
			onContactPostSolve = null;
			onContactSeperate = null;

			OnCustomEvent = e =>
			{
				OnEvent(e);
			};


		}

		//public EventListenerPhysicsContact(Action<CCEventCustom> eCustom) :
		//	base(PhysicsContact.PHYSICSCONTACT_EVENT_NAME, eCustom)
		//{

		//	//TODO: NOT CLEAR ?¿
		//	this.OnCustomEvent = (eventC) =>
		//	{
		//		OnEvent(eventC);
		//	};

		//}



		#region PUBLIC PROPS

		//public delegate bool OnContactDelegate(PhysicsContact contact);
		//public delegate bool OnContactPresolveDelegate(PhysicsContact contact, PhysicsContactPreSolve solve);
		//public delegate bool OnContactPostsolveDelegate(PhysicsContact contact, PhysicsContactPostSolve solve);


		/*
	 * @brief it will called at two shapes start to contact, and only call it once.
	 */
		public event Func<CCPhysicsContact, bool> onContactBegin;
		/*
		 * @brief Two shapes are touching during this step. Return false from the callback to make world ignore the collision this step or true to process it normally. Additionally, you may override collision values, restitution, or surface velocity values.
		 */
		public event Func<CCPhysicsContact, CCPhysicsContactPreSolve, bool> onContactPreSolve;
		/*
		 * @brief Two shapes are touching and their collision response has been processed. You can retrieve the collision impulse or kinetic energy at this time if you want to use it to calculate sound volumes or damage amounts. See cpArbiter for more info
		 */

		public event Func<CCPhysicsContact, CCPhysicsContactPostSolve, bool> onContactPostSolve;
		/*
		 * @brief it will called at two shapes separated, and only call it once.
		 * onContactBegin and onContactSeperate will called in pairs.
		 */
		public event Func<CCPhysicsContact, bool> onContactSeperate;

		#endregion

		#region PUBLIC METHODS
		public EventListenerPhysicsContact Clone()
		{

			EventListenerPhysicsContact obj = new EventListenerPhysicsContact();
			if (obj != null)
			{
				obj.onContactBegin = onContactBegin;
				obj.onContactPreSolve = onContactPreSolve;
				obj.onContactPostSolve = onContactPostSolve;
				obj.onContactSeperate = onContactSeperate;
				return obj;
			}

			return null;
		}

		/** create the listener */
		public bool CheckAvailable()
		{
			if (onContactBegin == null && onContactPreSolve == null
				&& onContactPostSolve == null && onContactSeperate == null)
			{
				cp.AssertWarn(false, "Invalid PhysicsContactListener.");
				return false;
			}

			return true;
		}


		#endregion

		#region PROTECTED METHODS

		protected void OnEvent(CCEventCustom eventC)
		{

			CCPhysicsContact contact = (CCPhysicsContact)eventC.UserData;
			// PhysicsContact) contact = dynamic_cast<PhysicsContact*>(event);


			if (contact == null)
			{
				return;
			}

			switch (contact.GetEventCode())
			{
				case EventCode.BEGIN:
					{
						bool ret = true;

						if (onContactBegin != null
							&& HitTest(contact.GetShapeA(), contact.GetShapeB()))
						{
							contact.GenerateContactData();
							ret = onContactBegin(contact);
						}

						contact.SetResult(ret);
						break;
					}
				case EventCode.PRESOLVE:
					{
						bool ret = true;

						if (onContactPreSolve != null
							&& HitTest(contact.GetShapeA(), contact.GetShapeB()))
						{
							CCPhysicsContactPreSolve solve = new CCPhysicsContactPreSolve(contact._contactInfo);
							contact.GenerateContactData();

							ret = onContactPreSolve(contact, solve);
						}

						contact.SetResult(ret);
						break;
					}
				case EventCode.POSTSOLVE:
					{
						if (onContactPostSolve != null
							&& HitTest(contact.GetShapeA(), contact.GetShapeB()))
						{
							CCPhysicsContactPostSolve solve = new CCPhysicsContactPostSolve(contact._contactInfo);
							onContactPostSolve(contact, solve);
						}
						break;
					}
				case EventCode.SEPERATE:
					{
						if (onContactSeperate != null
							&& HitTest(contact.GetShapeA(), contact.GetShapeB()))
						{
							onContactSeperate(contact);
						}
						break;
					}
				default:
					break;
			}

		}
		#endregion

		public virtual bool HitTest(CCPhysicsShape shapeA, CCPhysicsShape shapeB)
		{
			//CC_UNUSED_PARAM(shapeA);
			//CC_UNUSED_PARAM(shapeB);
			return true;
		}

	}

	/** this event listener only be called when bodyA and bodyB have contacts */
	public class EventListenerPhysicsContactWithBodies : EventListenerPhysicsContact
	{
		#region PROPTECTED PROPS

		protected CCPhysicsBody _a;
		protected CCPhysicsBody _b;

		#endregion

		public EventListenerPhysicsContactWithBodies(CCPhysicsBody bodyA, CCPhysicsBody bodyB)
		{
			_a = bodyA;
			_b = bodyB;
		}

		#region PUBLIC METHODS

		public EventListenerPhysicsContactWithBodies Clone()
		{

			EventListenerPhysicsContactWithBodies obj = new EventListenerPhysicsContactWithBodies(_a, _b);
			if (obj != null)
				return obj;

			return null;
		}

		public override bool HitTest(CCPhysicsShape shapeA, CCPhysicsShape shapeB)
		{
			if ((shapeA.GetBody() == _a && shapeB.GetBody() == _b)
	  || (shapeA.GetBody() == _b && shapeB.GetBody() == _a))
			{
				return true;
			}

			return false;
		}

		#endregion
	}

	/** this event listener only be called when shapeA and shapeB have contacts */
	public class EventListenerPhysicsContactWithShapes : EventListenerPhysicsContact
	{
		#region PROPTECTED PROPS

		public CCPhysicsShape _a;
		public CCPhysicsShape _b;

		#endregion

		public EventListenerPhysicsContactWithShapes(CCPhysicsShape shapeA, CCPhysicsShape shapeB)
		{
			_a = shapeA;
			_b = shapeB;
		}

		#region PUBLIC METHODS

		public override bool HitTest(CCPhysicsShape shapeA, CCPhysicsShape shapeB)
		{
			if ((shapeA == _a && shapeB == _b)
	   || (shapeA == _b && shapeB == _a))
			{
				return true;
			}

			return false;
		}
		public EventListenerPhysicsContactWithShapes Clone()
		{
			EventListenerPhysicsContactWithShapes obj = new EventListenerPhysicsContactWithShapes(_a, _b);
			if (obj != null)
				return obj;

			return null;
		}

		#endregion
	}

	/** this event listener only be called when shapeA or shapeB is in the group your specified */
	public class EventListenerPhysicsContactWithGroup : EventListenerPhysicsContact
	{
		#region PROPTECTED PROPS

		public int _group;

		#endregion


		public EventListenerPhysicsContactWithGroup()
			: this(cp.NO_GROUP)
		{
		}

		public EventListenerPhysicsContactWithGroup(int group)
		{
			_group = group;
		}


		#region PUBLIC METHODS

		public override bool HitTest(CCPhysicsShape shapeA, CCPhysicsShape shapeB)
		{
			if (shapeA.GetGroup() == _group || shapeB.GetGroup() == _group)
			{
				return true;
			}

			return false;
		}
		public EventListenerPhysicsContactWithGroup Clone()
		{
			EventListenerPhysicsContactWithGroup obj = new EventListenerPhysicsContactWithGroup(_group);
			if (obj != null)
				return obj;
			return null;
		}

		#endregion
	}

}