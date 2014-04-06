using Microsoft.Xna.Framework.Content.Pipeline;
using TInput = System.String;
using TOutput = CocosSharp.CCContent;

namespace CocosSharp.Content.Pipeline.Importers.TMX
{
    [ContentProcessor(DisplayName = "TMX Processor")]
    internal sealed class TMXProcessor : ContentProcessor<TInput, TOutput>
    {
        public override TOutput Process(TInput input, ContentProcessorContext context)
        {
            var output = new TOutput();
            output.Content = input;
            return output;
        }
    }
}