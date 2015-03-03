using System;
using System.Collections.Generic;
using CocosSharp;

namespace tests
{
	public class TmxMapRectangle : CCNode
	{
		public CCRect Rect { get; set; }

		protected override void Draw()
		{
			base.Draw();

			var color = new CCColor4B( Color.R, Color.G, Color.B );

			CCDrawingPrimitives.Begin();
			CCDrawingPrimitives.LineWidth = 2.0f;
			CCDrawingPrimitives.DrawRect(Rect, color);
			CCDrawingPrimitives.End();
		}
	}

	public class TmxMapEllipse : CCNode
	{
		public CCRect Rect { get; set; }

		protected override void Draw()
		{
			base.Draw();

			var color = new CCColor4B( Color.R, Color.G, Color.B );

			CCDrawingPrimitives.Begin();
			CCDrawingPrimitives.LineWidth = 2.0f;
			CCDrawingPrimitives.DrawEllipse(Rect, color);
			CCDrawingPrimitives.End();
		}
	}

	public class TmxMapPolygon : CCNode
	{
		public List<CCPoint> Points { get; set; }

		protected override void Draw()
		{
			base.Draw();

			var color = new CCColor4B( Color.R, Color.G, Color.B );

			CCDrawingPrimitives.Begin();
			CCDrawingPrimitives.LineWidth = 2.0f;
			CCDrawingPrimitives.DrawPoly(Points.ToArray(), color, true);
			CCDrawingPrimitives.End();
		}
	}

	public class TmxMapPolyline : CCNode
	{
		public List<CCPoint> Points { get; set; }

		protected override void Draw()
		{
			base.Draw();

			var color = new CCColor4B( Color.R, Color.G, Color.B );

			CCDrawingPrimitives.Begin();
			CCDrawingPrimitives.LineWidth = 2.0f;
			for ( int i = 0; i < Points.Count-1; i++ )
			{
				CCDrawingPrimitives.DrawLine(Points[i], Points[i+1], color);
			}
			CCDrawingPrimitives.End();
		}
	}

}
