using System;
using Microsoft.Xna.Framework.Content;

namespace CocosSharp
{
    public class CCSizeReader : ContentTypeReader<CCSize>
    {
        public CCSizeReader()
        {
        }
        
        protected override CCSize Read (ContentReader input, CCSize existingInstance)
        {
            var width = input.ReadSingle ();
            var height = input.ReadSingle ();
            
            var objectSize = new CCSize(width, height);
            
            return objectSize;
        }
        
    }
}

