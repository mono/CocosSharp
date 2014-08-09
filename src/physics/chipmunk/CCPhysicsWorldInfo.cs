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

	public class CCPhysicsWorldInfo
	{

		private cpSpace _space;


		#region PUBLIC METHODS

		public cpSpace getSpace() { return _space; }

		public void addShape(CCPhysicsShapeInfo shape)
		{

			foreach (var item in shape.getShapes())
			{
				_space.AddShape(item);
			}

		}
		public void removeShape(CCPhysicsShapeInfo shapeInf)
		{
			foreach (var shape in shapeInf.getShapes())
			{
				if (_space.ContainsShape(shape))
					_space.RemoveShape(shape);
			}

		}

		public void addBody(CCPhysicsBodyInfo bodyInf)
		{
			cpBody body = bodyInf.GetBody();

			if (!_space.ContainsBody(body))
			{
				_space.AddBody(body);
			}
		}

		public void removeBody(CCPhysicsBodyInfo bodyInf)
		{
			var body = bodyInf.GetBody();
			if (_space.ContainsBody(body))
			{
				_space.RemoveBody(body);
			}
		}

		public void AddJoint(CCPhysicsJointInfo joint)
		{
			foreach (cpConstraint subjoint in joint.getJoints())
			{
				_space.AddConstraint(subjoint);
			}
		}

		public void removeJoint(CCPhysicsJointInfo joint)
		{
			foreach (var subjoint in joint.getJoints())
			{
				_space.RemoveConstraint(subjoint);
			}
		}

		public void SetGravity(cpVect gravity)
		{
			_space.SetGravity(gravity);
		}

		public bool isLocked() { return _space.IsLocked; } // 0 == _space.locked ? false : true; }
		public void Step(float delta) { _space.Step(delta); }


		#endregion

		public CCPhysicsWorldInfo()
		{
			_space = new cpSpace();

		}

		//public void SetDebugDraw(cpDebugDraw debug)
		//{
		//	_space.SetDebugDraw(debug);
		//}


	}
}