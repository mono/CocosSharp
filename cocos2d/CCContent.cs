using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;

namespace cocos2d.Framework
{
    public class CCContent
    {
        [ContentSerializer]
        public string Content { get; set; }
    }
}
namespace Cocos2D.Framework
{
    public class CCContent
    {
        [ContentSerializer]
        public string Content { get; set; }

        /// <summary>
        /// Helper static method to load the contents of a CCContent object.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string LoadContentFile(string file)
        {
            string content = null;
            try
            {
                var data = Cocos2D.CCApplication.SharedApplication.Content.Load<CCContent>(file);
                if (data != null && data.Content != null)
                {
                    content = data.Content;
                }
            }
            catch (Exception)
            {
            }
            if (content == null)
            {
                try
                {
                    var dx = Cocos2D.CCApplication.SharedApplication.Content.Load<cocos2d.Framework.CCContent>(file);
                    if (dx == null || dx.Content == null)
                    {
                        throw (new ContentLoadException("Could not load the contents of " + file + " as raw text."));
                    }
                    content = dx.Content;
                }
                catch (Exception ex)
                {
                    throw (new ContentLoadException("Could not load the contents of " + file + " as raw text.", ex));
                }
            }
            return (content);
        }
    }
}
