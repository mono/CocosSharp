using System;
using Microsoft.Xna.Framework.Content;

namespace Cocos2D
{
    public class KerningHashElementReader : ContentTypeReader<CCBMFontConfiguration.CCKerningHashElement>
    {
        public KerningHashElementReader()
        {
        }
        
        protected override CCBMFontConfiguration.CCKerningHashElement Read (ContentReader input, CCBMFontConfiguration.CCKerningHashElement existingInstance)
        {
            var amount = input.ReadInt32 ();
            var key = input.ReadInt32 ();
            
            var objectSize = new CCBMFontConfiguration.CCKerningHashElement();
            objectSize.amount = amount;
            objectSize.key = key;
            
            return objectSize;
        }
        
    }
}

