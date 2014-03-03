using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework.Content;

[assembly: InternalsVisibleTo("CocosSharp.Content.Pipeline.Importers")]
[assembly: InternalsVisibleTo("Microsoft.Xna.Framework.Content")]

namespace CocosSharp
{
    internal class KerningHashElementReader : ContentTypeReader<CCBMFontConfiguration.CCKerningHashElement>
    {
        public KerningHashElementReader()
        {
        }
        
        protected override CCBMFontConfiguration.CCKerningHashElement Read (ContentReader input, CCBMFontConfiguration.CCKerningHashElement existingInstance)
        {
            var amount = input.ReadInt32 ();
            var key = input.ReadInt32 ();
            
            var objectSize = new CCBMFontConfiguration.CCKerningHashElement();
            objectSize.Amount = amount;
            objectSize.Key = key;
            
            return objectSize;
        }
        
    }
}

