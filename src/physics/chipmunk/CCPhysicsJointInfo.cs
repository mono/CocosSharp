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
	internal class CCPhysicsJointInfo
	{

		protected List<cpConstraint> _joints;
		static Dictionary<cpConstraint, CCPhysicsJointInfo> _map;


		protected CCPhysicsJoint _joint;

		protected static Dictionary<cpConstraint, CCPhysicsJointInfo> Map { get { return _map; } }

		public CCPhysicsJoint getJoint() { return _joint; }
		public List<cpConstraint> getJoints() { return _joints; }

		public CCPhysicsJointInfo(CCPhysicsJoint joint)
		{
			_joints = new List<cpConstraint>();
			_map = new Dictionary<cpConstraint, CCPhysicsJointInfo>();


			_joint = joint;
		}

		public void Add(cpConstraint joint)
		{
			if (joint == null) return;
			_joints.Add(joint);
			_map.Add(joint, this);
		}

		public void Remove(cpConstraint joint)
		{
			if (joint == null) return;

			var it = _joints.Find((c) => c == joint);
			if (it != null)
			{
				_joints.Remove(it);

				CCPhysicsJointInfo tmp;
				if (_map.TryGetValue(joint, out tmp))
				{
					_map.Remove(joint);
				}

			}
		}

		public void RemoveAll()
		{
			foreach (cpConstraint joint in _joints)
			{
				CCPhysicsJointInfo mit;
				if (_map.TryGetValue(joint, out mit))
				{
					_map.Remove(joint);
				}
			}

			_joints.Clear();
		}


		//     PhysicsJointInfo(PhysicsJoint joint);
		//~PhysicsJointInfo();

		~CCPhysicsJointInfo()
		{
			//foreach (var item in _joints)
			//{

			//}
		}


	}
}
#endif