using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework.Content;

[assembly: InternalsVisibleTo("CocosSharp.Content.Pipeline.Importers")]
[assembly: InternalsVisibleTo("Microsoft.Xna.Framework.Content")]


namespace CocosSharp
{
    internal class CCSizeReader : ContentTypeReader<CCSize>
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

