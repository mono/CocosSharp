using System;
using System.IO;
using Microsoft.Xna.Framework.Content.Pipeline;
using TImport = System.String;

namespace CocosSharp.Content.Pipeline.Importers.TMX
{
    [ContentImporter(".tmx",".TMX", DisplayName = "Tilemap Importer - CocosSharp", DefaultProcessor = "TMXProcessor")]
    public class TMXImporter : ContentImporter<TImport>
    {
        public override TImport Import(string filename, ContentImporterContext context)
        {
            return File.ReadAllText(filename);
        }
    }
}