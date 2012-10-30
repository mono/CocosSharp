using System;
using System.IO;
using Microsoft.Xna.Framework.Content.Pipeline;
using TImport = System.String;

namespace cocos2d.Content.Pipeline.Importers.TMX
{
    [ContentImporter(".TMX", DisplayName = "TMX Importer", DefaultProcessor = "TMX Processor")]
    public class TMXImporter : ContentImporter<TImport>
    {
        public override TImport Import(string filename, ContentImporterContext context)
        {
            return File.ReadAllText(filename);
            throw new NotImplementedException();
        }
    }
}