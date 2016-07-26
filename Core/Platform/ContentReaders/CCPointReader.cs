using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework.Content;

[assembly: InternalsVisibleTo("CocosSharp.Content.Pipeline.Importers")]
[assembly: InternalsVisibleTo("Microsoft.Xna.Framework.Content")]

namespace CocosSharp
{
    internal class CCPointReader : ContentTypeReader<CCPoint>
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

