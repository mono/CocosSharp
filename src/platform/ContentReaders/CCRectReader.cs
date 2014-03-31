using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework.Content;

[assembly: InternalsVisibleTo("CocosSharp.Content.Pipeline.Importers")]
[assembly: InternalsVisibleTo("Microsoft.Xna.Framework.Content")]

namespace CocosSharp
{
    internal class CCRectReader : ContentTypeReader<CCRect>
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

