using System;
using System.IO;
using CocosSharp;
using Microsoft.Xna.Framework.Graphics;
using Spine;

namespace CocosSharp.Spine
{
	public class CocosSharpTextureLoader : TextureLoader 
	{
		public CocosSharpTextureLoader ()
		{
		}

		Texture2D texture;

		public void Load (AtlasPage page, String path) 
		{
            var ccTexture = CCTextureCache.Instance.AddImage(path);

            if (texture == null)
                using (Stream stream = CCFileUtils.GetFileStream(path))
                {
                    texture = Util.LoadTexture(CCDrawManager.GraphicsDevice, stream);
                }
            else
                texture = ccTexture.XNATexture;

            page.rendererObject = texture;
            page.width = texture.Width;
            page.height = texture.Height;
		}

		public void Unload (Object texture) {
			((CCTexture2D)texture).Dispose();
		}
	}
}

