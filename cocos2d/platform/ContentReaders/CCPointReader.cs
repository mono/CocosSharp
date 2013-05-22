using System;
using Microsoft.Xna.Framework.Content;

namespace Cocos2D
{
    public class CCPointReader : ContentTypeReader<CCPoint>
    {
        public CCPointReader()
        {
        }
        
        protected override CCPoint Read (ContentReader input, CCPoint existingInstance)
        {
            var x = input.ReadSingle ();
            var y = input.ReadSingle ();

            var objectPoint = new CCPoint(x, y);
            
            return objectPoint;
        }
        
    }
}

