using System;
using CocosSharp;

namespace CocosSharpPCLTest
{
	public static class TestClass
	{
		public static CCLabelTtf PCLLabel(string message)
		{
			var label = new CCLabelTtf(message, "MarkerFelt", 22);
			label.Color = CCColor3B.White;
			label.Position = CCDrawManager.VisibleSize.Center;
			return label;
		}
	}
}

