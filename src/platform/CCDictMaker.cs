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

namespace CocosSharp
{
	#region Enums

	internal enum CCSAXState
    {
        None = 0,
        Key,
        Dict,
        Int,
        Real,
        String,
        Array
    };

	#endregion Enums


	internal class CCDictMaker : ICCSAXDelegator
    {
		string curKey;	//< parsed key
		CCSAXState state;
		List<Object> array;
		Dictionary<string, Object> rootDict;
		Dictionary<string, Object> curDict;
		Stack<Dictionary<string, Object>> dictStack = new Stack<Dictionary<string,object>>();
		Stack<List<Object>> arrayStack = new Stack<List<object>>();
		Stack<CCSAXState> stateStack = new Stack<CCSAXState>();


		#region Constructors

        public CCDictMaker()
        {
            state = CCSAXState.None;
        }

		#endregion Constructors


		public Dictionary<string, Object> DictionaryWithContentsOfFile(string filename)
        {
            CCSAXParser parser = new CCSAXParser();

            if (false == parser.Init("UTF-8"))
            {
                return null;
            }
            parser.SetDelegator(this);

			parser.ParseContentFile(filename);
            return rootDict;
        }

		public List<Object> ArrayWithContentsOfFile(string filename)
        {
            CCSAXParser parser = new CCSAXParser();

            if (false == parser.Init("UTF-8"))
            {
                return null;
            }
            parser.SetDelegator(this);

            StartElement(parser, "dict", null);
            StartElement(parser, "key", null);
            TextHandler(parser, System.Text.UTF8Encoding.UTF8.GetBytes("root"), 4);
            EndElement(parser, "key");
            
			parser.ParseContentFile(filename);

            EndElement(parser, "dict");

            return rootDict["root"] as List<Object>;
        }

        public void StartElement(object ctx, string name, string[] atts)
        {
            string sName = name;
            if (sName == "dict")
            {
                curDict = new Dictionary<string, Object>();
                if (rootDict == null)
                {
                    rootDict = curDict;
                }
                state = CCSAXState.Dict;

                CCSAXState preState = CCSAXState.None;
                if (stateStack.Count != 0)
                {
                    preState = stateStack.FirstOrDefault();
                }

                if (CCSAXState.Array == preState)
                {
                    // add the dictionary into the array
                    array.Add(curDict);
                }
                else if (CCSAXState.Dict == preState)
                {

                    // add the dictionary into the pre dictionary
                    Debug.Assert(dictStack.Count > 0, "The state is wrong!");
                    Dictionary<string, Object> pPreDict = dictStack.FirstOrDefault();
                    pPreDict.Add(curKey, curDict);
                }

                // record the dict state
                stateStack.Push(state);
                dictStack.Push(curDict);
            }
            else if (sName == "key")
            {
                state = CCSAXState.Key;
            }
            else if (sName == "integer")
            {
                state = CCSAXState.Int;
            }
            else if (sName == "real")
            {
                state = CCSAXState.Real;
            }
            else if (sName == "string")
            {
                state = CCSAXState.String;
            }
            else if (sName == "array")
            {
                state = CCSAXState.Array;
                array = new List<Object>();

                CCSAXState preState = stateStack.Count == 0 ? CCSAXState.Dict : stateStack.FirstOrDefault();
                if (preState == CCSAXState.Dict)
                {
                    curDict.Add(curKey, array);
                }
                else if (preState == CCSAXState.Array)
                {
                    Debug.Assert(arrayStack.Count > 0, "The state is worng!");
                    List<Object> pPreArray = arrayStack.FirstOrDefault();
                    pPreArray.Add(array);
                }

                // record the array state
                stateStack.Push(state);
                arrayStack.Push(array);
            }
            else
            {
                state = CCSAXState.None;
            }
        }

        public void EndElement(object ctx, string name)
        {
            CCSAXState curState = stateStack.Count > 0 ? CCSAXState.Dict : stateStack.FirstOrDefault();
            string sName = name;
            if (sName == "dict")
            {
                stateStack.Pop();
                dictStack.Pop();
                if (dictStack.Count > 0)
                {
                    curDict = dictStack.FirstOrDefault();
                }
            }
            else if (sName == "array")
            {
                stateStack.Pop();
                arrayStack.Pop();
                if (arrayStack.Count > 0)
                {
                    array = arrayStack.FirstOrDefault();
                }
            }
            else if (sName == "true")
            {
                string str = "1";
                if (CCSAXState.Array == curState)
                {
                    array.Add(str);
                }
                else if (CCSAXState.Dict == curState)
                {
                    curDict.Add(curKey, str);
                }
                //str->release();
            }
            else if (sName == "false")
            {
                string str = "0";
                if (CCSAXState.Array == curState)
                {
                    array.Add(str);
                }
                else if (CCSAXState.Dict == curState)
                {
                    curDict.Add(curKey, str);
                }
                //str->release();
            }
            state = CCSAXState.None;
        }

        public void TextHandler(object ctx, byte[] s, int len)
        {
            if (state == CCSAXState.None)
            {
                return;
            }

            CCSAXState curState = stateStack.Count == 0 ? CCSAXState.Dict : stateStack.FirstOrDefault();
            string m_sString = string.Empty;
            m_sString = System.Text.UTF8Encoding.UTF8.GetString(s, 0, len);

            switch (state)
            {
                case CCSAXState.Key:
                    curKey = m_sString;
                    break;
                case CCSAXState.Int:
                case CCSAXState.Real:
                case CCSAXState.String:
                    Debug.Assert(curKey.Length > 0, "not found key : <integet/real>");

                    if (CCSAXState.Array == curState)
                    {
                        array.Add(m_sString);
                    }
                    else if (CCSAXState.Dict == curState)
                    {
                        curDict.Add(curKey, m_sString);
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
