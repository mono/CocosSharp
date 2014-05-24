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
using CocosSharp;
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

// Note: Something to use here http://msdn.microsoft.com/en-us/library/hh582102.aspx

namespace CocosSharp
{
    public class CCUserDefault 
    {
    	static CCUserDefault UserDefault = null;
    	static string USERDEFAULT_ROOT_NAME = "userDefaultRoot";
    	static string XML_FILE_NAME = "UserDefault.xml";

    #if !WINDOWS && !MACOS && !LINUX && !NETFX_CORE
    	IsolatedStorageFile myIsolatedStorage;
    #elif NETFX_CORE
        StorageContainer myIsolatedStorage;
        StorageDevice myDevice;
    #endif
        Dictionary<string, string> values = new Dictionary<string, string>();


		#region Properties

		public static CCUserDefault SharedUserDefault
		{
			get {
				if (UserDefault == null)
				{
					UserDefault = new CCUserDefault();
				}

				return UserDefault;
			}
		}
			
		#endregion Properties


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

		#region Constructors

    	CCUserDefault()
    	{
    #if NETFX_CORE
            if(myIsolatedStorage == null) {
                CheckStorageDevice();
            }
            if(myIsolatedStorage != null) 
            {
                // only create xml file once if it doesnt exist
                if ((!IsXMLFileExist()))
                {
                    createXMLFile();
                }
                using (Stream s = myIsolatedStorage.OpenFile(XML_FILE_NAME, FileMode.OpenOrCreate))
                {
                    ParseXMLFile(s);
                }
            }
    #elif WINDOWS || MACOS || LINUX
    		// only create xml file once if it doesnt exist
    		if ((!IsXMLFileExist())) {
				CreateXMLFile();
    		}
    		using (FileStream fileStream = new FileInfo(XML_FILE_NAME).OpenRead()){
				ParseXMLFile(fileStream);
    		}

    #else

            myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication();

            // only create xml file once if it doesnt exist
    		if ((!IsXMLFileExist())) {
    			createXMLFile();
    		}

    		using (IsolatedStorageFileStream fileStream = myIsolatedStorage.OpenFile(XML_FILE_NAME, FileMode.Open, FileAccess.Read)) {
    			ParseXMLFile(fileStream);
    		}
    #endif
        }

		#endregion Constructors


    	public void PurgeSharedUserDefault()
    	{
    		UserDefault = null;
    	}

		bool ParseXMLFile(Stream xmlFile)
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

		string GetValueForKey(string key)
		{
			string value = null;
			if (! values.TryGetValue(key, out value)) {
				value = null;
			}

			return value;
		}

		void SetValueForKey(string key, string value)
		{
			values[key] = value;
		}

		public bool GetBoolForKey(string key, bool defaultValue=false)
    	{
			string value = GetValueForKey(key);
    		bool ret = defaultValue;

    		if (value != null)
    		{
    			ret = bool.Parse(value);
    		}

    		return ret;
    	}

		public int GetIntegerForKey(string key, int defaultValue=0)
    	{
			string value = GetValueForKey(key);
    		int ret = defaultValue;

    		if (value != null)
    		{
    			ret = CCUtils.CCParseInt(value);
    		}

    		return ret;
    	}

		public float GetFloatForKey(string key, float defaultValue)
    	{
			float ret = (float)GetDoubleForKey(key, (double)defaultValue);
     
    		return ret;
    	}

		public double GetDoubleForKey(string key, double defaultValue)
    	{
			string value = GetValueForKey(key);
    		double ret = defaultValue;

    		if (value != null)
    		{
    			ret = double.Parse(value);
    		}

    		return ret;
    	}

		public string GetStringForKey(string key, string defaultValue)
    	{
			string value = GetValueForKey(key);
    		string ret = defaultValue;

    		if (value != null)
    		{
    			ret = value;
    		}

    		return ret;
    	}

		public void SetBoolForKey(string key, bool value)
    	{
    		// check key
			if (key == null) {
    			return;
    		}

    		// save bool value as string
			SetStringForKey(key, value.ToString());
    	}

		public void SetIntegerForKey(string key, int value)
    	{
    		// check key
			if (key == null)
    		{
    			return;
    		}

    		// convert to string
			SetValueForKey(key, value.ToString());
    	}

		public void SetFloatForKey(string key, float value)
    	{
			SetDoubleForKey(key, value);
    	}

		public void SetDoubleForKey(string key, double value)
    	{
    		// check key
			if (key == null)
    		{
    			return;
    		}

    		// convert to string
			SetValueForKey(key, value.ToString());
    	}

		public void SetStringForKey(string key, string value)
    	{
    		// check key
			if (key == null)
    		{
    			return;
    		}

    		// convert to string
			SetValueForKey(key, value.ToString());
    	}

		bool IsXMLFileExist()
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

		bool CreateXMLFile()
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
