using System.IO;
using Microsoft.Xna.Framework.Content.Pipeline;

using TImport = System.String;

namespace CocosSharp.Content.Pipeline.Importers
{
    [ContentImporter(".*", DisplayName = "Text Importer", DefaultProcessor = "TextImporter")]
    public class TextImporter : ContentImporter<TImport>
    {
        public override TImport Import(string filename, ContentImporterContext context)
        {
            return File.ReadAllText(filename);
        }
    }
}