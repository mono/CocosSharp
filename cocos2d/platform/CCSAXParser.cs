/****************************************************************************
Copyright (c) 2010-2012 cocos2d-x.org
Copyright (c) 2008-2009 Jason Booth
Copyright (c) 2011-2012 openxlive.com

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
****************************************************************************/
using System;
using System.IO;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework.Content;

//using System.Xml.Linq;

namespace Cocos2D
{
    public class CCSAXParser
    {
        private ICCSAXDelegator m_pDelegator;

        public bool Init(string pszEncoding)
        {
            // nothing to do
            return true;
        }

        public bool ParseContent(string str)
        {
            TextReader textReader = new StringReader(str);
            return (ParseContent(textReader));
        }
        public bool ParseContent(TextReader sr)
        {
            var setting = new XmlReaderSettings();
#if !PSM
            setting.DtdProcessing = DtdProcessing.Ignore;
#endif
            XmlReader xmlReader = XmlReader.Create(sr, setting);
            int dataindex = 0;

            int Width = 0;
            int Height = 0;

            while (xmlReader.Read())
            {
                string name = xmlReader.Name;

                switch (xmlReader.NodeType)
                {
                    case XmlNodeType.Element:

                        string[] attrs = null;

                        if (name == "map")
                        {
                            Width = CCUtils.CCParseInt(xmlReader.GetAttribute("width"));
                            Height = CCUtils.CCParseInt(xmlReader.GetAttribute("height"));
                        }

                        if (xmlReader.HasAttributes)
                        {
                            attrs = new string[xmlReader.AttributeCount * 2];
                            xmlReader.MoveToFirstAttribute();
                            int i = 0;
                            attrs[0] = xmlReader.Name;
                            attrs[1] = xmlReader.Value;
                            i += 2;

                            while (xmlReader.MoveToNextAttribute())
                            {
                                attrs[i] = xmlReader.Name;
                                attrs[i + 1] = xmlReader.Value;
                                i += 2;
                            }

                            // Move the reader back to the element node.
                            xmlReader.MoveToElement();
                        }
                        StartElement(this, name, attrs);

                        byte[] buffer = null;

                        //read data content of tmx file
                        if (name == "data")
                        {
                            if (attrs != null)
                            {
                                string encoding = "";
                                for (int i = 0; i < attrs.Length; i++)
                                {
                                    if (attrs[i] == "encoding")
                                    {
                                        encoding = attrs[i + 1];
                                    }
                                }

                                if (encoding == "base64")
                                {
                                    int dataSize = (Width * Height * 4) + 1024;
                                    buffer = new byte[dataSize];
                                    xmlReader.ReadElementContentAsBase64(buffer, 0, dataSize);
                                }
                                else
                                {
                                    string value = xmlReader.ReadElementContentAsString();
                                    buffer = Encoding.UTF8.GetBytes(value);
                                }
                            }

                            TextHandler(this, buffer, buffer.Length);
                            EndElement(this, name);
                        }
                        else if (name == "key" || name == "integer" || name == "real" || name == "string")
                        {
                            string value = xmlReader.ReadElementContentAsString();
                            buffer = Encoding.UTF8.GetBytes(value);
                            TextHandler(this, buffer, buffer.Length);
                            EndElement(this, name);
                        }
                        else if (xmlReader.IsEmptyElement)
                        {
                            EndElement(this, name);
                        }
                        break;

                    case XmlNodeType.EndElement:
                        EndElement(this, xmlReader.Name);
                        dataindex++;
                        break;

                    default:
                        break;
                }
            }

            return true;
        }

        public bool ParseContentFile(string pszFile)
        {
            string content = CCContentManager.SharedContentManager.Load<string>(pszFile);
            return ParseContent(content);
        }

        public void SetDelegator(ICCSAXDelegator pDelegator)
        {
            m_pDelegator = pDelegator;
        }

        public static void StartElement(object ctx, string name, string[] atts)
        {
            ((CCSAXParser) (ctx)).m_pDelegator.StartElement(ctx, name, atts);
        }

        public static void EndElement(object ctx, string name)
        {
            ((CCSAXParser) (ctx)).m_pDelegator.EndElement(ctx, name);
        }

        public static void TextHandler(object ctx, byte[] ch, int len)
        {
            ((CCSAXParser) (ctx)).m_pDelegator.TextHandler(ctx, ch, len);
        }
    }
}