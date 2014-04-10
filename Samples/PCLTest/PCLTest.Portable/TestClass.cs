using System;
using CocosSharp;

namespace PCLTest
{
	public static class TestClass
	{
		public static CCLabelTtf PCLLabel(string message)
		{
			return new CCLabelTtf(message, "MarkerFelt", 22);
		}
	}
}

