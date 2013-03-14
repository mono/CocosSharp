using System;
using Microsoft.Xna.Framework.Content;

namespace cocos2d
{
    public class KerningHashElementReader : ContentTypeReader<CCBMFontConfiguration.tKerningHashElement>
    {
        public KerningHashElementReader()
        {
        }
        
        protected override CCBMFontConfiguration.tKerningHashElement Read (ContentReader input, CCBMFontConfiguration.tKerningHashElement existingInstance)
        {
            var amount = input.ReadInt32 ();
            var key = input.ReadInt32 ();
            
            var objectSize = new CCBMFontConfiguration.tKerningHashElement();
            objectSize.amount = amount;
            objectSize.key = key;
            
            return objectSize;
        }
        
    }
}

