using System;
using System.IO;
using System.Reflection;
using System.Xml;
namespace Cocos2D
{
	public static class Win8IOExtensions
	{
		public static void Close(this Stream stream)
		{
            //stream.Flush();
			stream.Close();
		}
		public static MethodInfo GetMethod(this Type type, string name)
		{
			return type.GetTypeInfo().GetDeclaredMethod(name);
		}
		public static FieldInfo GetField(this Type type, string name)
		{
			return type.GetTypeInfo().GetDeclaredField(name);
		}
		public static AssemblyName GetAssemblyName(this Type type)
		{
			return type.GetTypeInfo().Assembly.GetName();
		}
		public static void Close(this XmlWriter writer)
		{
            //writer.Flush();
			writer.Dispose();
		}
	}
}

