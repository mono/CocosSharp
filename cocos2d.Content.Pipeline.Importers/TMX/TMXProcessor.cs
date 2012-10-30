using Microsoft.Xna.Framework.Content.Pipeline;
using TInput = System.String;
using TOutput = cocos2d.Framework.CCContent;

namespace cocos2d.Content.Pipeline.Importers.TMX
{
    [ContentProcessor(DisplayName = "TMX Processor")]
    public class TMXProcessor : ContentProcessor<TInput, TOutput>
    {
        public override TOutput Process(TInput input, ContentProcessorContext context)
        {
            var output = new TOutput();
            output.Content = input;
            return output;
        }
    }
}