using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using MonoMac.OpenGL;

namespace CocosSharp
{
	internal class CCUtilsPlatformHandlerMac : CCUtilsPlatformHandler
	{
		List<string> glExtensions = null;

		internal override List<string> PlatformGLExtensions 
		{ 
			get
			{
				// Setup extensions.
				if(glExtensions == null) 
				{
					glExtensions = new List<string>();

					// for right now there are errors with GL before we even get here so the 
					// CheckGLError for MACOS is throwing errors even though the extensions are read
					// correctly.  Placed this here for now so that we can continue the processing
					// until we find the real error.
					var extstring = GL.GetString(StringName.Extensions);

					if (!string.IsNullOrEmpty(extstring))
					{
						glExtensions.AddRange(extstring.Split(' '));
						/*CCLog.Log("Supported GL extensions:");
						foreach (string extension in extensions)
							CCLog.Log(extension);
							*/
					}
				}

				return glExtensions;
			}
		}

	}
}

