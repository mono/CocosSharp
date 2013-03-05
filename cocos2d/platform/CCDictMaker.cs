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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace cocos2d
{
    public enum CCSAXState
    {
        SAX_NONE = 0,
        SAX_KEY,
        SAX_DICT,
        SAX_INT,
        SAX_REAL,
        SAX_STRING,
        SAX_ARRAY
    };
    public class CCDictMaker : ICCSAXDelegator
    {
        public Dictionary<string, Object> m_pRootDict;
        public Dictionary<string, Object> m_pCurDict;
        public Stack<Dictionary<string, Object>> m_tDictStack=new Stack<Dictionary<string,object>>();
        public string m_sCurKey;///< parsed key
        public CCSAXState m_tState;
        public List<Object> m_pArray;

        Stack<List<Object>> m_tArrayStack = new Stack<List<object>>();
        Stack<CCSAXState> m_tStateStack = new Stack<CCSAXState>();

        public CCDictMaker()
        {
            m_tState = CCSAXState.SAX_NONE;
        }

        public Dictionary<string, Object> DictionaryWithContentsOfFile(string pFileName)
        {
            CCSAXParser parser = new CCSAXParser();

            if (false == parser.Init("UTF-8"))
            {
                return null;
            }
            parser.SetDelegator(this);

            parser.Parse(pFileName);
            return m_pRootDict;
        }

        public List<Object> ArrayWithContentsOfFile(string pFileName)
        {
            CCSAXParser parser = new CCSAXParser();

            if (false == parser.Init("UTF-8"))
            {
                return null;
            }
            parser.SetDelegator(this);

            //byte[] root;

            StartElement(parser, "dict", null);
            StartElement(parser, "key", null);
            TextHandler(parser, System.Text.UTF8Encoding.UTF8.GetBytes("root"), 4);
            EndElement(parser, "key");
            
            parser.Parse(pFileName);

            EndElement(parser, "dict");

            return m_pRootDict["root"] as List<Object>;
        }

        public void StartElement(object ctx, string name, string[] atts)
        {

            string sName = name;
            if (sName == "dict")
            {
                m_pCurDict = new Dictionary<string, Object>();
                if (m_pRootDict == null)
                {
                    m_pRootDict = m_pCurDict;
                }
                m_tState = CCSAXState.SAX_DICT;

                CCSAXState preState = CCSAXState.SAX_NONE;
                if (m_tStateStack.Count != 0)
                {
                    preState = m_tStateStack.FirstOrDefault();
                }

                if (CCSAXState.SAX_ARRAY == preState)
                {
                    // add the dictionary into the array
                    m_pArray.Add(m_pCurDict);
                }
                else if (CCSAXState.SAX_DICT == preState)
                {

                    // add the dictionary into the pre dictionary
                    Debug.Assert(m_tDictStack.Count > 0, "The state is wrong!");
                    Dictionary<string, Object> pPreDict = m_tDictStack.FirstOrDefault();
                    pPreDict.Add(m_sCurKey, m_pCurDict);
                }
                //m_pCurDict->autorelease();

                // record the dict state
                m_tStateStack.Push(m_tState);
                m_tDictStack.Push(m_pCurDict);
            }
            else if (sName == "key")
            {
                m_tState = CCSAXState.SAX_KEY;
            }
            else if (sName == "integer")
            {
                m_tState = CCSAXState.SAX_INT;
            }
            else if (sName == "real")
            {
                m_tState = CCSAXState.SAX_REAL;
            }
            else if (sName == "string")
            {
                m_tState = CCSAXState.SAX_STRING;
            }
            else if (sName == "array")
            {
                m_tState = CCSAXState.SAX_ARRAY;
                m_pArray = new List<Object>();

                CCSAXState preState = m_tStateStack.Count == 0 ? CCSAXState.SAX_DICT : m_tStateStack.FirstOrDefault();
                if (preState == CCSAXState.SAX_DICT)
                {
                    m_pCurDict.Add(m_sCurKey, m_pArray);
                }
                else if (preState == CCSAXState.SAX_ARRAY)
                {
                    Debug.Assert(m_tArrayStack.Count > 0, "The state is worng!");
                    List<Object> pPreArray = m_tArrayStack.FirstOrDefault();
                    pPreArray.Add(m_pArray);
                }
                //m_pArray->release();
                // record the array state
                m_tStateStack.Push(m_tState);
                m_tArrayStack.Push(m_pArray);
            }
            else
            {
                m_tState = CCSAXState.SAX_NONE;
            }
        }

        public void EndElement(object ctx, string name)
        {
            CCSAXState curState = m_tStateStack.Count > 0 ? CCSAXState.SAX_DICT : m_tStateStack.FirstOrDefault();
            string sName = name;
            if (sName == "dict")
            {
                m_tStateStack.Pop();
                m_tDictStack.Pop();
                if (m_tDictStack.Count > 0)
                {
                    m_pCurDict = m_tDictStack.FirstOrDefault();
                }
            }
            else if (sName == "array")
            {
                m_tStateStack.Pop();
                m_tArrayStack.Pop();
                if (m_tArrayStack.Count > 0)
                {
                    m_pArray = m_tArrayStack.FirstOrDefault();
                }
            }
            else if (sName == "true")
            {
                string str = "1";
                if (CCSAXState.SAX_ARRAY == curState)
                {
                    m_pArray.Add(str);
                }
                else if (CCSAXState.SAX_DICT == curState)
                {
                    m_pCurDict.Add(m_sCurKey, str);
                }
                //str->release();
            }
            else if (sName == "false")
            {
                string str = "0";
                if (CCSAXState.SAX_ARRAY == curState)
                {
                    m_pArray.Add(str);
                }
                else if (CCSAXState.SAX_DICT == curState)
                {
                    m_pCurDict.Add(m_sCurKey, str);
                }
                //str->release();
            }
            m_tState = CCSAXState.SAX_NONE;
        }

        public void TextHandler(object ctx, byte[] s, int len)
        {
            if (m_tState == CCSAXState.SAX_NONE)
            {
                return;
            }

            CCSAXState curState = m_tStateStack.Count == 0 ? CCSAXState.SAX_DICT : m_tStateStack.FirstOrDefault();
            string m_sString = string.Empty;
            m_sString = System.Text.UTF8Encoding.UTF8.GetString(s, 0, len);

            switch (m_tState)
            {
                case CCSAXState.SAX_KEY:
                    m_sCurKey = m_sString;
                    break;
                case CCSAXState.SAX_INT:
                case CCSAXState.SAX_REAL:
                case CCSAXState.SAX_STRING:
                    Debug.Assert(m_sCurKey.Length > 0, "not found key : <integet/real>");

                    if (CCSAXState.SAX_ARRAY == curState)
                    {
                        m_pArray.Add(m_sString);
                    }
                    else if (CCSAXState.SAX_DICT == curState)
                    {
                        m_pCurDict.Add(m_sCurKey, m_sString);
                    }
                    break;
                default:
                    break;
            }
            //pText->release();
        }
    }
}
