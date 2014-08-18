#if USE_PHYSICS
using CocosSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChipmunkSharp
{

	[Flags]
	public enum PhysicsDrawFlags
	{

		None = 1 << 0,


		/// <summary>
		/// Draw shapes.
		/// </summary>
		Shapes = 1 << 1,

		/// <summary>
		/// Draw joint connections.
		/// </summary>
		Joints = 1 << 2,

		/// <summary>
		/// Draw contact points.
		/// </summary>
		ContactPoints = 1 << 3,

		/// <summary>
		/// Draw polygon BB.
		/// </summary>
		BB = 1 << 4,

		/// <summary>
		/// Draw All connections.
		/// </summary>
		All = 1 << 10,

	}

	public class PhysicsDebugDraw : CCDrawNode
	{

		public static cpColor CONSTRAINT_COLOR = cpColor.Grey; //new cpColor(0, 1, 0, 0.5f);
		public static cpColor TRANSPARENT_COLOR = new cpColor(0, 0, 0, 0.0f);

		cpVect[] springPoints = new cpVect[]{
	new cpVect(0.00f, 0.0f),
	new cpVect(0.20f, 0.0f),
	new cpVect(0.25f, 3.0f),
	new cpVect(0.30f, -6.0f),
	new cpVect(0.35f, 6.0f),
	new cpVect(0.40f, -6.0f),
	new cpVect(0.45f, 6.0f),
	new cpVect(0.50f, -6.0f),
	new cpVect(0.55f, 6.0f),
	new cpVect(0.60f, -6.0f),
	new cpVect(0.65f, 6.0f),
	new cpVect(0.70f, -3.0f),
	new cpVect(0.75f, 6.0f),
	new cpVect(0.80f, 0.0f),
	new cpVect(1.00f, 0.0f)
		};


		public PhysicsDrawFlags Flags = PhysicsDrawFlags.None;



		static CCPoint cpVert2Point(cpVect vert)
		{
			return new CCPoint(vert.x, vert.y);
		}


		static CCPoint[] cpVertArray2ccpArrayN(cpVect[] cpVertArray, int count)
		{
			if (count == 0)
				return null;

			CCPoint[] pPoints = new CCPoint[count];

			for (int i = 0; i < count; ++i)
			{
				pPoints[i].X = cpVertArray[i].x;
				pPoints[i].Y = cpVertArray[i].y;
			}
			return pPoints;
		}

#if USE_PHYSICS
		CCPhysicsWorld _world;
#endif
		cpSpace _space;
		cpBody _body;

		//bool ignoreBodyRotation = false;



#if USE_PHYSICS

		public PhysicsDebugDraw(CCPhysicsWorld world)
		{
			_world = world;
			_space = world.Info.Space;
			SelectFont("weblysleeku", 22);
			_world.Scene.AddChild(this); // getScene().addChild(_drawNode);

		}

#else

			public PhysicsDebugDraw(cpSpace space)
		{

	_space = space;

			SelectFont("weblysleeku", 22);

		}

#endif




		public cpBody Body
		{
			get
			{

				return _body;

			}
			set
			{

				_body = value;

			}
		}

		public override CCPoint Position
		{
			get
			{
				return cpVert2Point(_body.GetPosition());
			}

			set
			{
				_body.SetPosition(new cpVect(value.X, value.Y));//();
			}
		}



		/// <summary>
		/// Append flags to the current flags.
		/// </summary>
		public void AppendFlags(params PhysicsDrawFlags[] flags)
		{
			foreach (var item in flags)
			{
				Flags |= item;
			}

		}

		/// <summary>
		/// Clear flags from the current flags.
		/// </summary>
		public void ClearFlags(params PhysicsDrawFlags[] flags)
		{
			foreach (var item in flags)
			{
				Flags &= ~item;

			}
		}



		public void DebugDraw()
		{

			if (_space == null)
			{
				return;
			}

			DrawString(15, 15, string.Format("Step: {0}", _space.stamp));
			DrawString(15, 50, string.Format("Bodies : {0}/{1}", _space.dynamicBodies.Count + _space.staticBodies.Count, _space.dynamicBodies.Capacity));
			DrawString(15, 80, string.Format("Arbiters: {0}/{1}", _space.arbiters.Count, _space.arbiters.Capacity));

			if (Flags.HasFlag(PhysicsDrawFlags.All) || Flags.HasFlag(PhysicsDrawFlags.BB) || Flags.HasFlag(PhysicsDrawFlags.Shapes))
			{
				_space.EachShape(DrawShape);
			}

			if (Flags.HasFlag(PhysicsDrawFlags.Joints) || Flags.HasFlag(PhysicsDrawFlags.All))
			{
				_space.EachConstraint(DrawConstraint);
			}

			var contacts = 0;

			if (Flags.HasFlag(PhysicsDrawFlags.All) || Flags.HasFlag(PhysicsDrawFlags.ContactPoints))
			{
				for (var i = 0; i < _space.arbiters.Count; i++)
				{
					for (int j = 0; j < _space.arbiters[i].contacts.Count; j++)
					{
						Draw(_space.arbiters[i].contacts[j]);
					}
					contacts += _space.arbiters[i].contacts.Count;
				}

			}

			DrawString(15, 110, "Contact points: " + contacts);
			DrawString(15, 140, string.Format("Nodes:{1} Leaf:{0} Pairs:{2}", cp.numLeaves, cp.numNodes, cp.numPairs));

			base.Draw();
			base.Clear();
		}

		public void DrawShape(cpShape shape)
		{
			cpBody body = shape.body;
			cpColor color = cp.GetShapeColor(shape); ;// ColorForBody(body);


			switch (shape.shapeType)
			{
				case cpShapeType.Circle:
					{

						cpCircleShape circle = (cpCircleShape)shape;

						if (Flags.HasFlag(PhysicsDrawFlags.BB) || Flags.HasFlag(PhysicsDrawFlags.All))
							Draw(circle.bb);

						if (Flags.HasFlag(PhysicsDrawFlags.Shapes) || Flags.HasFlag(PhysicsDrawFlags.All))
							Draw(circle, color);

					}
					break;
				case cpShapeType.Segment:
					{

						cpSegmentShape seg = (cpSegmentShape)shape;

						if (Flags.HasFlag(PhysicsDrawFlags.BB) || Flags.HasFlag(PhysicsDrawFlags.All))
							Draw(seg.bb);

						if (Flags.HasFlag(PhysicsDrawFlags.Shapes) || Flags.HasFlag(PhysicsDrawFlags.All))
						{
							Draw(seg, color);
						}



					}
					break;
				case cpShapeType.Polygon:
					{
						cpPolyShape poly = (cpPolyShape)shape;


						if (Flags.HasFlag(PhysicsDrawFlags.BB) || Flags.HasFlag(PhysicsDrawFlags.All))
							Draw(poly.bb);

						if (Flags.HasFlag(PhysicsDrawFlags.Shapes) || Flags.HasFlag(PhysicsDrawFlags.All))
						{
							Draw(poly, color);
						}


					}
					break;
				default:
					cp.AssertHard(false, "Bad assertion in DrawShape()");
					break;
			}
		}

		public void DrawConstraint(cpConstraint constraint)
		{
			Type klass = constraint.GetType();

			if (klass == typeof(cpPinJoint))
			{
				Draw((cpPinJoint)constraint);
			}
			else if (klass == typeof(cpSlideJoint))
			{
				Draw((cpSlideJoint)constraint);

			}
			else if (klass == typeof(cpPivotJoint))
			{
				Draw((cpPivotJoint)constraint);
			}
			else if (klass == typeof(cpGrooveJoint))
			{
				Draw((cpGrooveJoint)constraint);
			}
			else if (klass == typeof(cpDampedSpring))
			{

				Draw((cpDampedSpring)constraint);
				// TODO
			}
			else if (klass == typeof(cpDampedRotarySpring))
			{

				Draw((cpDampedRotarySpring)constraint);

			}
			else if (klass == typeof(cpSimpleMotor))
			{

				Draw((cpSimpleMotor)constraint);

			}
			else
			{
				//		printf("Cannot draw constraint\n");
			}
		}



		public void DrawSpring(cpVect a, cpVect b, cpColor cpColor)
		{

			DrawDot(a, 5, CONSTRAINT_COLOR);
			DrawDot(b, 5, CONSTRAINT_COLOR);

			cpVect delta = cpVect.cpvsub(b, a);
			float cos = delta.x;
			float sin = delta.y;
			float s = 1.0f / cpVect.cpvlength(delta);

			cpVect r1 = cpVect.cpv(cos, -sin * s);
			cpVect r2 = cpVect.cpv(sin, cos * s);

			cpVect[] verts = new cpVect[springPoints.Length];
			for (int i = 0; i < springPoints.Length; i++)
			{
				cpVect v = springPoints[i];
				verts[i] = new cpVect(cpVect.cpvdot(v, r1) + a.x, cpVect.cpvdot(v, r2) + a.y);
			}

			for (int i = 0; i < springPoints.Length - 1; i++)
			{
				DrawSegment(verts[i], verts[i + 1], 1, cpColor.Grey);
			}

		}

		#region DRAW SHAPES

		public void DrawCircle(cpVect center, float radius, cpColor color)
		{
			var centerPoint = center.ToCCPoint();
			var colorOutline = color.ToCCColor4B();
			var colorFill = colorOutline * 0.5f;
			base.DrawCircle(centerPoint, radius, colorOutline);
			base.DrawSolidCircle(centerPoint, radius, colorFill);
		}

		public void DrawSolidCircle(cpVect center, float radius, cpColor color)
		{
			base.DrawCircle(center.ToCCPoint(), radius, color.ToCCColor4B());
		}

		public void DrawCircle(cpVect center, float radius, float angle, int segments, cpColor color)
		{
			base.DrawCircle(center.ToCCPoint(), radius, angle, segments, color.ToCCColor4B());
		}
		public void DrawDot(cpVect pos, float radius, cpColor color)
		{
			//base.DrawDot(pos.ToCCPoint(), radius, color.ToCCColor4F());
			base.DrawSolidCircle(pos.ToCCPoint(), radius, color.ToCCColor4B());
		}
		public void DrawPolygon(cpVect[] verts, int count, cpColor fillColor, float borderWidth, cpColor borderColor)
		{
			base.DrawPolygon(cpVertArray2ccpArrayN(verts, verts.Length), count, fillColor.ToCCColor4F(), borderWidth, borderColor.ToCCColor4F());
		}
		public void DrawRect(CCRect rect, cpColor color)
		{
			base.DrawRect(rect, color.ToCCColor4B());
		}
		public void DrawSegment(cpVect from, cpVect to, float radius, cpColor color)
		{
			base.DrawSegment(from.ToCCPoint(), to.ToCCPoint(), radius, color.ToCCColor4F());
		}

		public void Draw(cpPolyShape poly, cpColor color)
		{
			cpColor fill = new cpColor(color);
			fill.a = cp.cpflerp(color.a, 1.0f, 0.5f);
			DrawPolygon(poly.GetVertices(), poly.Count, fill, 0.5f, color);
		}

		public void Draw(cpBB bb)
		{
			Draw(bb, cpColor.CyanBlue);
		}

		public void Draw(cpBB bb, cpColor color)
		{
			DrawPolygon(new cpVect[] { 
 
						new cpVect(bb.r, bb.b),
					new cpVect(bb.r, bb.t),
					new cpVect(bb.l, bb.t),
					new cpVect(bb.l, bb.b)
				
				}, 4, TRANSPARENT_COLOR, 1, color);

		}

		public void Draw(cpContact contact)
		{
			DrawDot(contact.r1, 0.5f, cpColor.Red);
			DrawDot(contact.r2, 0.5f, cpColor.Red);
		}

		public void Draw(cpCircleShape circle, cpColor color)
		{
			cpVect center = circle.tc;
			float radius = circle.r;
			cpVect To = cpVect.cpvadd(cpVect.cpvmult(circle.body.GetRotation(), circle.r), (circle.tc));
			DrawCircle(center, cp.cpfmax(radius, 1.0f), color);
			DrawSegment(center, To, 0.5f, cpColor.Grey);
		}

		private void Draw(cpSegmentShape seg, cpColor color)
		{
			DrawFatSegment(seg.ta, seg.tb, seg.r, color);
		}

		private void DrawFatSegment(cpVect ta, cpVect tb, float r, cpColor color)
		{
			cpColor fill = new cpColor(color);
			fill.a = cp.cpflerp(color.a, 1.0f, 0.5f);

			DrawSegment(ta, tb, Math.Max(1, r), fill);
		}

		public void Draw(cpVect point)
		{
			Draw(point, 0.5f);
		}

		public void Draw(cpVect point, cpColor color)
		{
			Draw(point, 0.5f, color);
		}

		public void Draw(cpVect point, float radius)
		{
			DrawDot(point, radius, cpColor.Red);
		}

		public void Draw(cpVect point, float radius, cpColor color)
		{
			DrawDot(point, radius, color);
		}


		#endregion

		#region DRAW CONSTRAINT

		private void Draw(cpDampedRotarySpring constraint)
		{
			//Not used
		}

		private void Draw(cpDampedSpring constraint)
		{

			var a = constraint.a.LocalToWorld(constraint.GetAnchorA());
			var b = constraint.b.LocalToWorld(constraint.GetAnchorB());

			DrawSpring(a, b, CONSTRAINT_COLOR);

		}

		public void Draw(cpSimpleMotor cpSimpleMotor)
		{
			//Not used

		}

		private void Draw(cpGrooveJoint constraint)
		{

			var a = constraint.a.LocalToWorld(constraint.grv_a);
			var b = constraint.a.LocalToWorld(constraint.grv_b);
			var c = constraint.b.LocalToWorld(constraint.anchorB);

			DrawSegment(a, b, 1, CONSTRAINT_COLOR);
			DrawCircle(c, 5f, CONSTRAINT_COLOR);
		}

		private void Draw(cpPivotJoint constraint)
		{

			cpVect a = cpTransform.Point(constraint.a.transform, constraint.GetAnchorA());
			cpVect b = cpTransform.Point(constraint.b.transform, constraint.GetAnchorB());

			//DrawSegment(a, b, 1, cpColor.Grey);
			DrawDot(a, 3, CONSTRAINT_COLOR);
			DrawDot(b, 3, CONSTRAINT_COLOR);

		}

		public void Draw(cpSlideJoint constraint)
		{

			cpVect a = cpTransform.Point(constraint.a.transform, constraint.GetAnchorA());
			cpVect b = cpTransform.Point(constraint.b.transform, constraint.GetAnchorB());

			DrawSegment(a, b, 1, cpColor.Grey);
			DrawDot(a, 5, CONSTRAINT_COLOR);
			DrawDot(b, 5, CONSTRAINT_COLOR);

		}

		public void Draw(cpPinJoint constraint)
		{

			cpVect a = cpTransform.Point(constraint.a.transform, constraint.GetAnchorA());
			cpVect b = cpTransform.Point(constraint.b.transform, constraint.GetAnchorB());

			DrawSegment(a, b, 1, cpColor.Grey);
			DrawDot(a, 5, CONSTRAINT_COLOR);
			DrawDot(b, 5, CONSTRAINT_COLOR);


		}

		#endregion

#if USE_PHYSICS

		#region DRAW PHYSICS
		public void DrawShape(CCPhysicsShape shape)
		{
			foreach (cpShape item in shape._info.GetShapes())
				DrawShape(item);
		}

		public void DrawJoint(CCPhysicsJoint joint)
		{
			foreach (cpConstraint item in joint._info.getJoints())
				DrawConstraint(item);
		}

		#endregion

#endif

		public bool Begin()
		{
			base.Clear();
			return true;
		}

		public void End()
		{

		}


	}
}

#endif