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

using cocos2d;
using System.IO;
using System.IO.IsolatedStorage;
using System.Collections.Generic;
using System.Text;
using System.Xml;

public class CCUserDefault {
	private static CCUserDefault m_spUserDefault = null;
	private static string USERDEFAULT_ROOT_NAME = "userDefaultRoot";
	private static string XML_FILE_NAME = "UserDefault.xml";

	private IsolatedStorageFile myIsolatedStorage;
	private Dictionary<string, string> values = new Dictionary<string, string>();

	private bool parseXMLFile(IsolatedStorageFileStream xmlFile)
	{
		values.Clear();

		string key = "";

		// Create an XmlReader
		using (XmlReader reader = XmlReader.Create(xmlFile)) {
			XmlWriterSettings ws = new XmlWriterSettings();
			ws.Indent = false;
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


	/**
	 * implements of CCUserDefault
	 */
	private CCUserDefault()
	{
		myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication();

		// only create xml file once if it doesnt exist
		if ((!isXMLFileExist())) {
			createXMLFile();
		}

		using (IsolatedStorageFileStream fileStream = myIsolatedStorage.OpenFile(XML_FILE_NAME, FileMode.Open, FileAccess.Read)) {
			parseXMLFile(fileStream);
		}
	}

	public void purgeSharedUserDefault()
	{
		m_spUserDefault = null;
	}

	public bool getBoolForKey(string pKey, bool defaultValue)
	{
		string value = getValueForKey(pKey);
		bool ret = defaultValue;

		if (value != null)
		{
			ret = bool.Parse(value);
		}

		return ret;
	}

	public int getIntegerForKey(string pKey, int defaultValue)
	{
		string value = getValueForKey(pKey);
		int ret = defaultValue;

		if (value != null)
		{
			ret = ccUtils.ccParseInt(value);
		}

		return ret;
	}

	public float getFloatForKey(string pKey, float defaultValue)
	{
		float ret = (float)getDoubleForKey(pKey, (double)defaultValue);
 
		return ret;
	}

	public double getDoubleForKey(string pKey, double defaultValue)
	{
		string value = getValueForKey(pKey);
		double ret = defaultValue;

		if (value != null)
		{
			ret = double.Parse(value);
		}

		return ret;
	}

	public string getStringForKey(string pKey, string defaultValue)
	{
		string value = getValueForKey(pKey);
		string ret = defaultValue;

		if (value != null)
		{
			ret = value;
		}

		return ret;
	}

	public void setBoolForKey(string pKey, bool value)
	{
		// check key
		if (pKey == null) {
			return;
		}

		// save bool value as string
		setStringForKey(pKey, value.ToString());
	}

	public void setIntegerForKey(string pKey, int value)
	{
		// check key
		if (pKey == null)
		{
			return;
		}

		// convert to string
		setValueForKey(pKey, value.ToString());
	}

	public void setFloatForKey(string pKey, float value)
	{
		setDoubleForKey(pKey, value);
	}

	public void setDoubleForKey(string pKey, double value)
	{
		// check key
		if (pKey == null)
		{
			return;
		}

		// convert to string
		setValueForKey(pKey, value.ToString());
	}

	public void setStringForKey(string pKey, string value)
	{
		// check key
		if (pKey == null)
		{
			return;
		}

		// convert to string
		setValueForKey(pKey, value.ToString());
	}

	public static CCUserDefault sharedUserDefault()
	{
		if (m_spUserDefault == null)
		{
			m_spUserDefault = new CCUserDefault();
		}

		return m_spUserDefault;
	}

	private bool isXMLFileExist()
	{
		bool bRet = false;

		if (myIsolatedStorage.FileExists(XML_FILE_NAME)) 
		{
			bRet = true;
		}

		return bRet;
	}

	// create new xml file
	private bool createXMLFile()
	{
		bool bRet = false;

		using (StreamWriter writeFile = new StreamWriter(new IsolatedStorageFileStream(XML_FILE_NAME, FileMode.Create, FileAccess.Write, myIsolatedStorage))) 
		{
			string someTextData = "<?xml version=\"1.0\" encoding=\"utf-8\"?><userDefaultRoot></userDefaultRoot>";
			writeFile.WriteLine(someTextData);
			writeFile.Close();
		}
		return bRet;
	}

	public void flush()
	{
		using (StreamWriter stream = new StreamWriter(new IsolatedStorageFileStream(XML_FILE_NAME, FileMode.Create, FileAccess.Write, myIsolatedStorage))) 
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

