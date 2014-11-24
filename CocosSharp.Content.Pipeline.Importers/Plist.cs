using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using CocosSharp;

namespace CocosSharp.Content.Pipeline.Importers
{
    [ContentImporter(".plist", DisplayName = "CocosSharp - Plist", DefaultProcessor = "CocosPListProcessor")]
    public class CocosPListImporter : ContentImporter<String>
    {
        public override String Import(string filename, ContentImporterContext context)
        {
            return filename;
        }
    }

    [ContentProcessor(DisplayName = "CocosSharp - Plist")]
    public class CocosPListProcessor : ContentProcessor<string, PlistDocument>
    {
        public override PlistDocument Process(string fileName, ContentProcessorContext context)
        {
            string data = File.ReadAllText(fileName);
            var plist = new PlistDocument();
            plist.LoadFromXml(data);
            return plist;
        }
    }

    [ContentTypeWriter]
    public class PlistDocumentWriter : ContentTypeWriter<PlistDocument>
    {
        Dictionary<string, int> _stringPool = new Dictionary<string, int>();

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(PlistDocument.PlistDocumentReader).AssemblyQualifiedName;
        }

        private void PrepareStringPool(PlistObjectBase value)
        {
            if (value is PlistArray)
            {
                var array = (PlistArray)value;
                for (int i = 0; i < array.Count; i++)
                {
                    PrepareStringPool(array[i]);
                }
            }
            else if (value is PlistDictionary)
            {
                var dict = (PlistDictionary)value;
                foreach (var pair in dict)
                {
                    if (!_stringPool.ContainsKey(pair.Key))
                    {
                        _stringPool.Add(pair.Key, _stringPool.Count);
                    }
                    PrepareStringPool(pair.Value);
                }
            }
            else if (value is PlistString)
            {
                if (!_stringPool.ContainsKey(value.AsString))
                {
                    _stringPool.Add(value.AsString, _stringPool.Count);
                }
            }
        }

        protected override void Write(ContentWriter output, PlistDocument value)
        {
            _stringPool.Clear();

            PrepareStringPool(value.Root);

            var stringArray = new string[_stringPool.Count];
            foreach (var pair in _stringPool)
            {
                stringArray[pair.Value] = pair.Key;
            }

            output.WriteObject(stringArray);

            WriteValue(output, value.Root);
        }

        protected void WriteValue(ContentWriter output, PlistObjectBase value)
        {
            if (value is PlistArray)
            {
                output.Write((byte) PlistDocument.ValueType.Array);

                var array = (PlistArray) value;
                output.Write(array.Count);
                for (int i = 0; i < array.Count; i++)
                {
                    WriteValue(output, array[i]);
                }
            }
            else if (value is PlistBoolean)
            {
                output.Write((byte) PlistDocument.ValueType.Bool);
                output.Write(value.AsBool);
            }
            else if (value is PlistData)
            {
                output.Write((byte) PlistDocument.ValueType.Data);
                output.Write(value.AsBinary);
            }
            else if (value is PlistDate)
            {
                output.Write((byte) PlistDocument.ValueType.Date);
                output.WriteObject(value.AsDate);
            }
            else if (value is PlistDictionary)
            {
                output.Write((byte) PlistDocument.ValueType.Dictionary);

                var dict = (PlistDictionary) value;
                output.Write(dict.Count);
                foreach (var pair in dict)
                {
                    int index = _stringPool[pair.Key];
                    output.Write(index);
                    WriteValue(output, pair.Value);
                }
            }
            else if (value is PlistInteger)
            {
                output.Write((byte) PlistDocument.ValueType.Integer);
                output.Write(value.AsInt);
            }
            else if (value is PlistNull)
            {
                output.Write((byte) PlistDocument.ValueType.Null);
            }
            else if (value is PlistReal)
            {
                output.Write((byte) PlistDocument.ValueType.Real);
                output.Write(value.AsFloat);
            }
            else if (value is PlistString)
            {
                int index = _stringPool[value.AsString];
                output.Write((byte) PlistDocument.ValueType.String);
                output.Write(index);
            }
        }
    }
}