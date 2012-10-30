using System;
using System.IO;
using Microsoft.Xna.Framework.Content.Pipeline;

namespace cocos2d.Content.Pipeline.Importers
{
    [ContentImporter(".fnt", DisplayName = "Cocos2D - Font", DefaultProcessor = "CocosFontProcessor")]
    public class CocosFontImporter : ContentImporter<String>
    {
        public override String Import(string filename, ContentImporterContext context)
        {
            return filename;
        }
    }

    [ContentProcessor(DisplayName = "Cocos2D - Font")]
    public class CocosFontProcessor : ContentProcessor<string, CCBMFontConfiguration>
    {
        public override CCBMFontConfiguration Process(string fileName, ContentProcessorContext context)
        {
            //System.Diagnostics.Debugger.Launch(); 

            var result = new CCBMFontConfiguration();

            var data = File.ReadAllText(fileName);

            var relativePath = context.OutputFilename.Substring(context.OutputDirectory.Length);
            relativePath = Path.GetDirectoryName(relativePath);

            result.InitWithString(data, Path.Combine(relativePath, Path.GetFileName(fileName)));

            return result;
        }
    }

}