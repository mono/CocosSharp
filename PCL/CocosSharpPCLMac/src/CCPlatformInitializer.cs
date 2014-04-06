using System;

namespace CocosSharp
{
	public static class CCPlatformInitializer
	{
		static bool connectedToPCL;

		public static void ConnectToPCL()
		{
			Console.WriteLine("Connect to PCL");

			if (!connectedToPCL) 
			{
				CCUtils.PlatformHandler = new CCUtilsPlatformHandlerMac();

				connectedToPCL = true;
			}
		}
	}
}

