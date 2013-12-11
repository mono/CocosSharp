using CocosSharp;

namespace tests.Extensions
{
	public class CustomTableViewCell : CCTableViewCell
	{
		public override void Draw()
		{
			base.Draw();
			
			// draw bounding box
			
			//var pos = position;
			//var size = new CCSize(178, 200);
			//var vertices = new CCPoint[]
			//{
			//    new CCPoint(pos.x + 1, pos.y + 1),
			//    new CCPoint(pos.x + size.width - 1, pos.y + 1),
			//    new CCPoint(pos.x + size.width - 1, pos.y+size.height - 1),
			//    new CCPoint(pos.x + 1, pos.y + size.height - 1),
			//};

			//CCDrawingPrimitives.ccDrawColor4B(0, 0, 255, 255);
			//CCDrawingPrimitives.ccDrawPoly(vertices, 4, true);
		}
	}
}

