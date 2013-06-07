/****************************************************************************
Copyright (c) 2010-2011 cocos2d-x.org

http://www.cocos2d-x.org

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
// root name of xml

using System;
using Cocos2D;
using System.IO;
#if !WINDOWS && !MACOS && !LINUX && !NETFX_CORE
using System.IO.IsolatedStorage;
#endif
#if NETFX_CORE
using Microsoft.Xna.Framework.Storage;
#endif
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Cocos2D
{
    public class CCUserDefault 
    {

    	private static CCUserDefault m_spUserDefault = null;
    	private static string USERDEFAULT_ROOT_NAME = "userDefaultRoot";
    	private static string XML_FILE_NAME = "UserDefault.xml";

    #if !WINDOWS && !MACOS && !LINUX && !NETFX_CORE
    	private IsolatedStorageFile myIsolatedStorage;
    #elif NETFX_CORE
        private StorageContainer myIsolatedStorage;
        private StorageDevice myDevice;
    #endif
        private Dictionary<string, string> values = new Dictionary<string, string>();

        private bool parseXMLFile(Stream xmlFile)
    	{
    		values.Clear();

    		string key = "";

    		// Create an XmlReader
    		using (XmlReader reader = XmlReader.Create(xmlFile)) {
    				// Parse the file and display each of the nodes.
    				while (reader.Read()) {
    					switch (reader.NodeType) {
    						case XmlNodeType.Element:
    							key = reader.Name;
    							break;
    						case XmlNodeType.Text:
    							values.Add(key, reader.Value);
    							break;
    						case XmlNodeType.XmlDeclaration:
    						case XmlNodeType.ProcessingInstruction:
    							break;
    						case XmlNodeType.Comment:
    							break;
    						case XmlNodeType.EndElement:
    							break;
    					}
    				}
    		}
    		return true;
    	}

    	private string getValueForKey(string key)
    	{
    		string value = null;
    		if (! values.TryGetValue(key, out value)) {
    			value = null;
    		}

    		return value;
    	}

    	private void setValueForKey(string key, string value)
    	{
    		values[key] = value;
    	}

    #if NETFX_CORE
        private StorageDevice CheckStorageDevice() {
            if(myDevice != null) {
                return(myDevice);
            }
            IAsyncResult result = StorageDevice.BeginShowSelector(null, null);
            // Wait for the WaitHandle to become signaled.
            result.AsyncWaitHandle.WaitOne();
            myDevice = StorageDevice.EndShowSelector(result);
            if(myDevice != null) {
                result =
                    myDevice.BeginOpenContainer("Save Your Game...", null, null);
                // Wait for the WaitHandle to become signaled.
                result.AsyncWaitHandle.WaitOne();
                myIsolatedStorage = myDevice.EndOpenContainer(result);
                // Close the wait handle.
                result.AsyncWaitHandle.Dispose();
            }
            return(myDevice);
        }
    #endif

    	/**
    	 * implements of CCUserDefault
    	 */
    	private CCUserDefault()
    	{
    #if NETFX_CORE
            if(myIsolatedStorage == null) {
                CheckStorageDevice();
            }
            if(myIsolatedStorage != null) 
            {
                // only create xml file once if it doesnt exist
                if ((!isXMLFileExist()))
                {
                    createXMLFile();
                }
                using (Stream s = myIsolatedStorage.OpenFile(XML_FILE_NAME, FileMode.OpenOrCreate))
                {
                    parseXMLFile(s);
                }
            }
    #elif WINDOWS || MACOS || LINUX
    		// only create xml file once if it doesnt exist
    		if ((!isXMLFileExist())) {
    			createXMLFile();
    		}
    		using (FileStream fileStream = new FileInfo(XML_FILE_NAME).OpenRead()){
    			parseXMLFile(fileStream);
    		}

    #else
            myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication();

    		// only create xml file once if it doesnt exist
    		if ((!isXMLFileExist())) {
    			createXMLFile();
    		}

    		using (IsolatedStorageFileStream fileStream = myIsolatedStorage.OpenFile(XML_FILE_NAME, FileMode.Open, FileAccess.Read)) {
    			parseXMLFile(fileStream);
    		}
    #endif
        }

    	public void PurgeSharedUserDefault()
    	{
    		m_spUserDefault = null;
    	}

        public bool GetBoolForKey(string pKey)
        {
            return GetBoolForKey(pKey, false);
        }

    	public bool GetBoolForKey(string pKey, bool defaultValue)
    	{
    		string value = getValueForKey(pKey);
    		bool ret = defaultValue;

    		if (value != null)
    		{
    			ret = bool.Parse(value);
    		}

    		return ret;
    	}

        public int GetIntegerForKey(string pKey)
        {
            return GetIntegerForKey(pKey, 0);
        }

    	public int GetIntegerForKey(string pKey, int defaultValue)
    	{
    		string value = getValueForKey(pKey);
    		int ret = defaultValue;

    		if (value != null)
    		{
    			ret = CCUtils.CCParseInt(value);
    		}

    		return ret;
    	}

    	public float GetFloatForKey(string pKey, float defaultValue)
    	{
    		float ret = (float)GetDoubleForKey(pKey, (double)defaultValue);
     
    		return ret;
    	}

    	public double GetDoubleForKey(string pKey, double defaultValue)
    	{
    		string value = getValueForKey(pKey);
    		double ret = defaultValue;

    		if (value != null)
    		{
    			ret = double.Parse(value);
    		}

    		return ret;
    	}

    	public string GetStringForKey(string pKey, string defaultValue)
    	{
    		string value = getValueForKey(pKey);
    		string ret = defaultValue;

    		if (value != null)
    		{
    			ret = value;
    		}

    		return ret;
    	}

    	public void SetBoolForKey(string pKey, bool value)
    	{
    		// check key
    		if (pKey == null) {
    			return;
    		}

    		// save bool value as string
    		SetStringForKey(pKey, value.ToString());
    	}

    	public void SetIntegerForKey(string pKey, int value)
    	{
    		// check key
    		if (pKey == null)
    		{
    			return;
    		}

    		// convert to string
    		setValueForKey(pKey, value.ToString());
    	}

    	public void SetFloatForKey(string pKey, float value)
    	{
    		SetDoubleForKey(pKey, value);
    	}

    	public void SetDoubleForKey(string pKey, double value)
    	{
    		// check key
    		if (pKey == null)
    		{
    			return;
    		}

    		// convert to string
    		setValueForKey(pKey, value.ToString());
    	}

    	public void SetStringForKey(string pKey, string value)
    	{
    		// check key
    		if (pKey == null)
    		{
    			return;
    		}

    		// convert to string
    		setValueForKey(pKey, value.ToString());
    	}

    	public static CCUserDefault SharedUserDefault
    	{
            get {
        		if (m_spUserDefault == null)
        		{
        			m_spUserDefault = new CCUserDefault();
        		}

        		return m_spUserDefault;
            }
    	}

    	private bool isXMLFileExist()
    	{
    		bool bRet = false;
    #if NETFX_CORE
            // use the StorageContainer to determine if the file exists.
            if (myIsolatedStorage.FileExists(XML_FILE_NAME))
            {
                bRet = true;
            }
    #elif WINDOWS || LINUX || MACOS
    		if (new FileInfo(XML_FILE_NAME).Exists) 
    		{
    			bRet = true;
    		}
    #else
            if (myIsolatedStorage.FileExists(XML_FILE_NAME)) 
    		{
    			bRet = true;
    		}
    #endif
    		return bRet;
    	}

    	// create new xml file
    	private bool createXMLFile()
    	{
    		bool bRet = false;

    #if NETFX_CORE
            using (StreamWriter writeFile = new StreamWriter(myIsolatedStorage.OpenFile(XML_FILE_NAME, FileMode.OpenOrCreate)))
    #elif WINDOWS || LINUX || MACOS
    		using (StreamWriter writeFile = new StreamWriter(XML_FILE_NAME)) 
    #else
            using (StreamWriter writeFile = new StreamWriter(new IsolatedStorageFileStream(XML_FILE_NAME, FileMode.Create, FileAccess.Write, myIsolatedStorage)))
    #endif
            {
                string someTextData = "<?xml version=\"1.0\" encoding=\"utf-8\"?><userDefaultRoot>";
                writeFile.WriteLine(someTextData);
                // Do not write anything here. This just creates the temporary xml save file.
                writeFile.WriteLine("</userDefaultRoot>");
            }
    		return bRet;
    	}

    	public void Flush()
    	{
    #if NETFX_CORE
            using (Stream stream = myIsolatedStorage.OpenFile(XML_FILE_NAME, FileMode.OpenOrCreate))
    #elif WINDOWS || LINUX || MACOS
    		using (StreamWriter stream = new StreamWriter(XML_FILE_NAME)) 
    #else
    		using (StreamWriter stream = new StreamWriter(new IsolatedStorageFileStream(XML_FILE_NAME, FileMode.Create, FileAccess.Write, myIsolatedStorage))) 
    #endif
    		{
    			//create xml doc
    			XmlWriterSettings ws = new XmlWriterSettings();
    			ws.Encoding = Encoding.UTF8;
    			ws.Indent = true;
    			using (XmlWriter writer = XmlWriter.Create(stream, ws)) 
    			{
    				writer.WriteStartDocument();
    				writer.WriteStartElement(USERDEFAULT_ROOT_NAME);
    				foreach (KeyValuePair<string, string> pair in values) 
    				{
    					writer.WriteStartElement(pair.Key);
    					writer.WriteString(pair.Value);
    					writer.WriteEndElement();
    				}
    				writer.WriteEndElement();
    				writer.WriteEndDocument();
    			}
    		}
    	}

    }
}
