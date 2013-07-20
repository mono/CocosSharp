//
// Taken from PodSleuth (http://git.gnome.org/cgit/podsleuth)
//  
// Author:
//       Aaron Bockover <abockover@novell.com>
// 
// Copyright (c) 2007-2009 Novell, Inc. (http://www.novell.com)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.ComponentModel;
using System.Xml;
using System.IO;
using System.Diagnostics;
using Microsoft.Xna.Framework.Content;
#if NETFX_CORE
using Win8StoreIOUtility = Cocos2D.Win8StoreIOUtility;
#endif

namespace Cocos2D
{
#if IOS
    [MonoTouch.Foundation.Preserve (AllMembers = true)]
#endif
    public class PlistDocument : PlistObjectBase
    {
        private const string version = "1.0";

        private PlistObjectBase root;

        public PlistDocument(string data)
        {
            LoadFromXml(data);
        }

        /// <summary>
        /// Load the plist from a stream. On XNA platforms you shoul use Game.TitleContainer.OpenStream() to get
        /// a handle on your resource as a stream as it exists in the isolated title container space.
        /// </summary>
        /// <param name="data"></param>
        public PlistDocument(Stream data)
        {
            LoadFromXmlFile(data);
        }

        public PlistDocument()
        {
        }

        public PlistDocument(PlistObjectBase root)
        {
            this.root = root;
        }

        public void LoadFromXmlFile(Stream data)
        {
            //allow DTD but not try to resolve it from web
            var settings = new XmlReaderSettings()
            {
#if !PSM
                DtdProcessing = DtdProcessing.Ignore,
#endif
                //ProhibitDtd = false,
#if !NETFX_CORE
                XmlResolver = null,
#endif
            };
            using (var reader = XmlReader.Create(data, settings))
                LoadFromXml(reader);
        }

        public void LoadFromXmlFile(string path)
        {
            //allow DTD but not try to resolve it from web
            var settings = new XmlReaderSettings()
                {
#if !PSM
                    DtdProcessing = DtdProcessing.Ignore,
#endif
				//ProhibitDtd = false,
#if !NETFX_CORE
                    XmlResolver = null,
#endif
                };
            using (var reader = XmlReader.Create(path, settings))
                LoadFromXml(reader);
        }

        public void LoadFromXml(string data)
        {
            //allow DTD but not try to resolve it from web
            var settings = new XmlReaderSettings()
                {
                    CloseInput = true,
#if !PSM
                    DtdProcessing = DtdProcessing.Ignore,
#endif
				//ProhibitDtd = false,
#if !NETFX_CORE
                    XmlResolver = null,
#endif
                };
            using (var reader = XmlReader.Create(new StringReader(data), settings))
            {
                LoadFromXml(reader);
            }
        }

        public void LoadFromXml(XmlReader reader)
        {
            reader.ReadToDescendant("plist");
            while (reader.Read() && reader.NodeType != XmlNodeType.Element) ;
            if (!reader.EOF)
                root = LoadFromNode(reader);
        }

        private PlistObjectBase LoadFromNode(XmlReader reader)
        {
            Debug.Assert(reader.NodeType == XmlNodeType.Element);
            bool isEmpty = reader.IsEmptyElement;
            switch (reader.LocalName)
            {
                case "dict":
                    var dict = new PlistDictionary(true);
                    if (!isEmpty)
                    {
                        if (reader.ReadToDescendant("key"))
                            dict = LoadDictionaryContents(reader, dict);
                        reader.ReadEndElement();
                    }
                    return dict;

                case "array":
                    if (isEmpty)
                        return new PlistArray();

                    //advance to first node
                    reader.ReadStartElement();
                    while (reader.Read() && reader.NodeType != XmlNodeType.Element) ;

                    // HACK: plist data in iPods is not even valid in some cases! Way to go Apple!
                    // This hack checks to see if they really meant for this array to be a dict.
                    if (reader.LocalName == "key")
                    {
                        var ret = LoadDictionaryContents(reader, new PlistDictionary(true));
                        reader.ReadEndElement();
                        return ret;
                    }

                    var arr = new PlistArray();
                    do
                    {
                        if (reader.NodeType == XmlNodeType.Element)
                        {
                            var val = LoadFromNode(reader);
                            if (val != null)
                                arr.Add(val);
                        }
                    } while (reader.Read() && reader.NodeType != XmlNodeType.EndElement);
                    reader.ReadEndElement();
                    return arr;

                case "string":
                    return new PlistString(reader.ReadElementContentAsString());
                case "integer":
                    return new PlistInteger(reader.ReadElementContentAsInt());
                case "real":
                    return new PlistReal(reader.ReadElementContentAsFloat());
                case "false":
                    reader.ReadStartElement();
                    if (!isEmpty)
                        reader.ReadEndElement();
                    return new PlistBoolean(false);
                case "true":
                    reader.ReadStartElement();
                    if (!isEmpty)
                        reader.ReadEndElement();
                    return new PlistBoolean(true);
                case "data":
                    return new PlistData(reader.ReadElementContentAsString());
                case "date":
#if NETFX_CORE
                    return new PlistDate(DateTime.Parse(reader.ReadElementContentAsString()));
#else
                    return new PlistDate(reader.ReadElementContentAsDateTime());
#endif               
                default:
                    throw new XmlException(String.Format("Plist Node `{0}' is not supported", reader.LocalName));
            }
        }

        private PlistDictionary LoadDictionaryContents(XmlReader reader, PlistDictionary dict)
        {
            Debug.Assert(reader.NodeType == XmlNodeType.Element && reader.LocalName == "key");
            while (!reader.EOF && reader.NodeType == XmlNodeType.Element)
            {
                //string key = reader.ReadElementString ();
                string key = reader.ReadElementContentAsString();
                while (reader.NodeType != XmlNodeType.Element && reader.Read())
                    if (reader.NodeType == XmlNodeType.EndElement)
                        throw new Exception(String.Format("No value found for key {0}", key));
                PlistObjectBase result = LoadFromNode(reader);
                if (result != null)
                    dict.Add(key, result);
                
                // when there is no whitespace between nodes, we might already be at
                // the next key element, so reading to next sibling would jump over
                // the next (current) key element
                if (!"key".Equals(reader.Name))
                    reader.ReadToNextSibling("key");                
            }
            return dict;
        }

        public PlistObjectBase Root
        {
            get { return root; }
            set { root = value; }
        }

        public override void Write(System.Xml.XmlWriter writer)
        {
            writer.WriteStartDocument();
            writer.WriteDocType("plist", "-//Apple Computer//DTD PLIST 1.0//EN", "http://www.apple.com/DTDs/PropertyList-1.0.dtd", null);
            writer.WriteStartElement("plist");
            writer.WriteAttributeString("version", version);
            root.Write(writer);
            writer.WriteEndDocument();
        }

        public override byte[] AsBinary
        {
            get { throw new NotImplementedException(); }
        }

        public override int AsInt
        {
            get { throw new NotImplementedException(); }
        }

        public override float AsFloat
        {
            get { throw new NotImplementedException(); }
        }

        public override string AsString
        {
            get { throw new NotImplementedException(); }
        }

        public override DateTime AsDate
        {
            get { throw new NotImplementedException(); }
        }

        public override bool AsBool
        {
            get { throw new NotImplementedException(); }
        }

        public override PlistArray AsArray
        {
            get { throw new NotImplementedException(); }
        }

        public override PlistDictionary AsDictionary
        {
            get { throw new NotImplementedException(); }
        }

        public void WriteToFile(string filename)
        {
#if NETFX_CORE
            Stream writeStreamFromFileName = Win8StoreIOUtility.GetWriteStreamFromFileName(filename);
            using (StreamWriter streamWriter = new StreamWriter(writeStreamFromFileName, System.Text.Encoding.UTF8))
#else
            using (var streamWriter = new StreamWriter(filename, false, System.Text.Encoding.UTF8))
#endif
            {
                var settings = new XmlWriterSettings()
                    {
                        Indent = true
                    };

                using (var writer = XmlWriter.Create(streamWriter, settings))
                {
                    Write(writer);
                }
            }
        }

        public enum ValueType : byte
        {
            Array,
            Bool,
            Data,
            Date,
            Dictionary,
            Integer,
            Null,
            Real,
            String
        }

        public class PlistDocumentReader : ContentTypeReader<PlistDocument>
        {
            private string[] _stringPool;

            protected override PlistDocument Read(ContentReader input, PlistDocument existingInstance)
            {
                if (existingInstance == null)
                {
                    existingInstance = new PlistDocument();
                }

                _stringPool = input.ReadObject<string[]>();

                existingInstance.root = ReadValue(input);
                
                return existingInstance;
            }

            protected PlistObjectBase ReadValue(ContentReader input)
            {
                var type = (ValueType) input.ReadByte();

                switch (type)
                {
                    case ValueType.Array:
                        var count = input.ReadInt32();
                        var array = new PlistArray(count);
                        for (int i = 0; i < count; i++)
                        {
                            array.Add(ReadValue(input));
                        }
                        return array;

                    case ValueType.Bool:
                        return new PlistBoolean(input.ReadBoolean());

                    case ValueType.Data:
                        count = input.ReadInt32();
                        return new PlistData(input.ReadBytes(count));

                    case ValueType.Date:
                        return new PlistDate(input.ReadObject<DateTime>());

                    case ValueType.Dictionary:
                        count = input.ReadInt32();
                        var dict = new PlistDictionary();
                        for (int i = 0; i < count; i++)
                        {
                            string key = _stringPool[input.ReadInt32()];
                            dict.Add(key, ReadValue(input));
                        }
                        return dict;

                    case ValueType.Integer:
                        return new PlistInteger(input.ReadInt32());

                    case ValueType.Null:
                        return new PlistNull();

                    case ValueType.Real:
                        return new PlistReal(input.ReadSingle());

                    case ValueType.String:
                        return new PlistString(_stringPool[input.ReadInt32()]);

                    default:
                        throw new InvalidOperationException();
                }
            }
        }
    }
}
