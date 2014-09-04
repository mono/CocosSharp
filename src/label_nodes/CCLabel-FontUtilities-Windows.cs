using System;
using System.Collections.Generic;
using SharpDX;
using SharpDX.DirectWrite;

// These are based off the samples from SharpDX: https://github.com/sharpdx/SharpDX-Samples/tree/master/WindowsDesktop/DirectWrite/CustomFont
namespace CocosSharp
{
    /// <summary>
    /// ResourceFont main loader. This classes implements FontCollectionLoader and FontFileLoader.
    /// It reads all fonts embedded as resource in the current assembly and expose them.
    /// </summary>
    internal partial class PrivateFontLoader : CallbackBase, FontCollectionLoader, FontFileLoader
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="PrivateFontLoader"/> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        public PrivateFontLoader(Factory factory)
        {
            _factory = factory;


            foreach (var name in System.Reflection.Assembly.GetEntryAssembly().GetManifestResourceNames())
            {
                if (name.ToLower().EndsWith(".ttf"))
                {
                    var fontBytes = Utilities.ReadStream(System.Reflection.Assembly.GetEntryAssembly().GetManifestResourceStream(name));
                    var stream = new DataStream(fontBytes.Length, true, true);
                    stream.Write(fontBytes, 0, fontBytes.Length);
                    stream.Position = 0;
                    _fontStreams.Add(new PrivateFontFileStream(stream));
                }
            }
            // Build a Key storage that stores the index of the font
            _keyStream = new DataStream(sizeof(int) * _fontStreams.Count, true, true);
            for (int i = 0; i < _fontStreams.Count; i++)
                _keyStream.Write((int)i);
            _keyStream.Position = 0;

            // Register the 
            _factory.RegisterFontFileLoader(this);
            _factory.RegisterFontCollectionLoader(this);
        }

    }
}

