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


        private readonly List<PrivateFontFileStream> _fontStreams = new List<PrivateFontFileStream>();
        private readonly List<PrivateFontFileEnumerator> _enumerators = new List<PrivateFontFileEnumerator>();
        readonly DataStream _keyStream;
        private readonly Factory _factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrivateFontLoader"/> class.
        /// This is to be implemented in each platform partial class implementaion
        /// </summary>
        /// <param name="factory">The factory.</param>
        //public PrivateFontLoader(Factory factory, string fontName)
        //{
        //}


        /// <summary>
        /// Gets the key used to identify the FontCollection as well as storing index for fonts.
        /// </summary>
        /// <value>The key.</value>
        public DataStream Key
        {
            get
            {
                return _keyStream;
            }
        }

        /// <summary>
        /// Creates a font file enumerator object that encapsulates a collection of font files. The font system calls back to this interface to create a font collection.
        /// </summary>
        /// <param name="factory">Pointer to the <see cref="SharpDX.DirectWrite.Factory"/> object that was used to create the current font collection.</param>
        /// <param name="collectionKey">A font collection key that uniquely identifies the collection of font files within the scope of the font collection loader being used. The buffer allocated for this key must be at least  the size, in bytes, specified by collectionKeySize.</param>
        /// <returns>
        /// a reference to the newly created font file enumerator.
        /// </returns>
        /// <unmanaged>HRESULT IDWriteFontCollectionLoader::CreateEnumeratorFromKey([None] IDWriteFactory* factory,[In, Buffer] const void* collectionKey,[None] int collectionKeySize,[Out] IDWriteFontFileEnumerator** fontFileEnumerator)</unmanaged>
        FontFileEnumerator FontCollectionLoader.CreateEnumeratorFromKey(Factory factory, DataPointer collectionKey)
        {
            var enumerator = new PrivateFontFileEnumerator(factory, this, collectionKey);
            _enumerators.Add(enumerator);

            return enumerator;
        }

        /// <summary>
        /// Creates a font file stream object that encapsulates an open file resource.
        /// </summary>
        /// <param name="fontFileReferenceKey">A reference to a font file reference key that uniquely identifies the font file resource within the scope of the font loader being used. The buffer allocated for this key must at least be the size, in bytes, specified by  fontFileReferenceKeySize.</param>
        /// <returns>
        /// a reference to the newly created <see cref="SharpDX.DirectWrite.FontFileStream"/> object.
        /// </returns>
        /// <remarks>
        /// The resource is closed when the last reference to fontFileStream is released.
        /// </remarks>
        /// <unmanaged>HRESULT IDWriteFontFileLoader::CreateStreamFromKey([In, Buffer] const void* fontFileReferenceKey,[None] int fontFileReferenceKeySize,[Out] IDWriteFontFileStream** fontFileStream)</unmanaged>
        FontFileStream FontFileLoader.CreateStreamFromKey(DataPointer fontFileReferenceKey)
        {
            var index = Utilities.Read<int>(fontFileReferenceKey.Pointer);
            return _fontStreams[index];
        }

    }
}

// These are based off the samples from SharpDX: https://github.com/sharpdx/SharpDX-Samples/tree/master/WindowsDesktop/DirectWrite/CustomFont
namespace CocosSharp
{
    /// <summary>
    /// This FontFileStream implem is reading data from a <see cref="DataStream"/>.
    /// </summary>
    internal class PrivateFontFileStream : CallbackBase, FontFileStream
    {
        private readonly DataStream _stream;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrivateFontFileStream"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public PrivateFontFileStream(DataStream stream)
        {
            this._stream = stream;
        }

        /// <summary>
        /// Reads a fragment from a font file.
        /// </summary>
        /// <param name="fragmentStart">When this method returns, contains an address of a  reference to the start of the font file fragment.  This parameter is passed uninitialized.</param>
        /// <param name="fileOffset">The offset of the fragment, in bytes, from the beginning of the font file.</param>
        /// <param name="fragmentSize">The size of the file fragment, in bytes.</param>
        /// <param name="fragmentContext">When this method returns, contains the address of</param>
        /// <remarks>
        /// Note that ReadFileFragment implementations must check whether the requested font file fragment is within the file bounds. Otherwise, an error should be returned from ReadFileFragment.   {{DirectWrite}} may invoke <see cref="SharpDX.DirectWrite.FontFileStream"/> methods on the same object from multiple threads simultaneously. Therefore, ReadFileFragment implementations that rely on internal mutable state must serialize access to such state across multiple threads. For example, an implementation that uses separate Seek and Read operations to read a file fragment must place the code block containing Seek and Read calls under a lock or a critical section.
        /// </remarks>
        /// <unmanaged>HRESULT IDWriteFontFileStream::ReadFileFragment([Out, Buffer] const void** fragmentStart,[None] __int64 fileOffset,[None] __int64 fragmentSize,[Out] void** fragmentContext)</unmanaged>
        void FontFileStream.ReadFileFragment(out IntPtr fragmentStart, long fileOffset, long fragmentSize, out IntPtr fragmentContext)
        {
            lock (this)
            {
                fragmentContext = IntPtr.Zero;
                _stream.Position = fileOffset;
                fragmentStart = _stream.PositionPointer;
            }
        }

        /// <summary>
        /// Releases a fragment from a file.
        /// </summary>
        /// <param name="fragmentContext">A reference to the client-defined context of a font fragment returned from {{ReadFileFragment}}.</param>
        /// <unmanaged>void IDWriteFontFileStream::ReleaseFileFragment([None] void* fragmentContext)</unmanaged>
        void FontFileStream.ReleaseFileFragment(IntPtr fragmentContext)
        {
            // Nothing to release. No context are used
        }

        /// <summary>
        /// Obtains the total size of a file.
        /// </summary>
        /// <returns>the total size of the file.</returns>
        /// <remarks>
        /// Implementing GetFileSize() for asynchronously loaded font files may require downloading the complete file contents. Therefore, this method should be used only for operations that either require a complete font file to be loaded (for example, copying a font file) or that need to make decisions based on the value of the file size (for example, validation against a persisted file size).
        /// </remarks>
        /// <unmanaged>HRESULT IDWriteFontFileStream::GetFileSize([Out] __int64* fileSize)</unmanaged>
        long FontFileStream.GetFileSize()
        {
            return _stream.Length;
        }

        /// <summary>
        /// Obtains the last modified time of the file.
        /// </summary>
        /// <returns>
        /// the last modified time of the file in the format that represents the number of 100-nanosecond intervals since January 1, 1601 (UTC).
        /// </returns>
        /// <remarks>
        /// The "last modified time" is used by DirectWrite font selection algorithms to determine whether one font resource is more up to date than another one.
        /// </remarks>
        /// <unmanaged>HRESULT IDWriteFontFileStream::GetLastWriteTime([Out] __int64* lastWriteTime)</unmanaged>
        long FontFileStream.GetLastWriteTime()
        {
            return 0;
        }
    }
}

// These are based off the samples from SharpDX: https://github.com/sharpdx/SharpDX-Samples/tree/master/WindowsDesktop/DirectWrite/CustomFont
namespace CocosSharp
{
    /// <summary>
    /// Resource FontFileEnumerator.
    /// </summary>
    internal  class PrivateFontFileEnumerator : CallbackBase, FontFileEnumerator
    {
        private Factory _factory;
        private FontFileLoader _loader;
        internal DataStream keyStream;
        private FontFile _currentFontFile;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrivateFontFileEnumerator"/> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="loader">The loader.</param>
        /// <param name="key">The key.</param>
        public PrivateFontFileEnumerator(Factory factory, FontFileLoader loader, DataPointer key)
        {
            _factory = factory;
            _loader = loader;
            keyStream = new DataStream(key.Pointer, key.Size, true, false);
        }

        //public bool MoveNext2()
        //{
        //    return FontFileEnumerator.MoveNext();
        //}

        /// <summary>
        /// Advances to the next font file in the collection. When it is first created, the enumerator is positioned before the first element of the collection and the first call to MoveNext advances to the first file.
        /// </summary>
        /// <returns>
        /// the value TRUE if the enumerator advances to a file; otherwise, FALSE if the enumerator advances past the last file in the collection.
        /// </returns>
        /// <unmanaged>HRESULT IDWriteFontFileEnumerator::MoveNext([Out] BOOL* hasCurrentFile)</unmanaged>
        bool FontFileEnumerator.MoveNext()
        {
            bool moveNext = keyStream.RemainingLength != 0;
            if (moveNext)
            {
                if (_currentFontFile != null)
                    _currentFontFile.Dispose();

                _currentFontFile = new FontFile(_factory, keyStream.PositionPointer, 4, _loader);
                keyStream.Position += 4;
            }
            return moveNext;
        }

        /// <summary>
        /// Gets a reference to the current font file.
        /// </summary>
        /// <value></value>
        /// <returns>a reference to the newly created <see cref="SharpDX.DirectWrite.FontFile"/> object.</returns>
        /// <unmanaged>HRESULT IDWriteFontFileEnumerator::GetCurrentFontFile([Out] IDWriteFontFile** fontFile)</unmanaged>
        FontFile FontFileEnumerator.CurrentFontFile
        {
            get
            {
                ((IUnknown) _currentFontFile).AddReference();
                return _currentFontFile;
            }
        }
    }
}