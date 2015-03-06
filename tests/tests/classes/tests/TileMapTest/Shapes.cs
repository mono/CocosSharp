using System;
using CocosSharp;

namespace tests
{
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
}
