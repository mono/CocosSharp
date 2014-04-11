using System;
using CocosSharp;

namespace PCLTest
{
	public static class TestClass
	{
		public static CCLabelTtf PCLLabel(string message)
		{

			var label = new CCLabelTtf(message, "MarkerFelt", 22);
			label.Position = CCDrawManager.VisibleSize.Center;
			return label;
		}
	}
}

