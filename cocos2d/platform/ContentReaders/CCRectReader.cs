using System;
using Microsoft.Xna.Framework.Content;

namespace Cocos2D
{
    public class CCRectReader : ContentTypeReader<CCRect>
    {
        public CCRectReader()
        {
        }

        protected override CCRect Read (ContentReader input, CCRect existingInstance)
        {
            var x = input.ReadSingle ();
            var y = input.ReadSingle ();
            var width = input.ReadSingle ();
            var height = input.ReadSingle ();

            var objectRect = new CCRect(x, y, width, height);

            return objectRect;
        }

    }
}

