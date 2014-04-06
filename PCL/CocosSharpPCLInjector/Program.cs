using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;
using MethodAttributes = Mono.Cecil.MethodAttributes;

namespace CocosSharpPCLInjector
{
	class MainClass
	{
		static AssemblyDefinition AssemblyPlatform { get; set; }
		static AssemblyDefinition AssemblyShared { get; set; }

		public static void Main (string[] args)
		{
			Debug.Assert(args.Length == 2, 
				"Expecting only 2 arguments - the path of the shared and platform assembly respectively relative to working directory\n");

			string assemblyFileShared = args[0];
			string assemblyFilePlatform = args[1];

			string typeName = "CCPlatformInitializer";
			string methodName = "ConnectToPCL";

			AssemblyPlatform = ReadAssembly(assemblyFilePlatform);
			AssemblyShared = ReadAssembly(assemblyFileShared);

			TypeDefinition type = AssemblyPlatform.MainModule.Types.FirstOrDefault(t => t.Name == typeName);
			MethodReference callee = type.Methods.FirstOrDefault(m => m.Name == methodName);
			InjectInitializer(AssemblyShared, callee);

			WriteAssembly(AssemblyShared, assemblyFileShared);
		}

		static AssemblyDefinition ReadAssembly(string assemblyPath)
		{
			Debug.Assert(File.Exists(assemblyPath), "Could not find assembly at path: {0}\n", assemblyPath);

			var readParams = new ReaderParameters(ReadingMode.Immediate);
			AssemblyDefinition assemblyDef = AssemblyDefinition.ReadAssembly(assemblyPath, readParams);

			Debug.Write("Reading assembly at path: {0}\n", assemblyPath);
			Debug.Assert(assemblyDef != null, String.Format("Could not read assembly at path: {0}\n", assemblyDef));

			return assemblyDef;
		}

		static void InjectInitializer(AssemblyDefinition assemblyDef, MethodReference callee)
		{
			TypeReference voidRef = assemblyDef.MainModule.Import(callee.ReturnType);
			const MethodAttributes attributes = MethodAttributes.Static
				| MethodAttributes.SpecialName
				| MethodAttributes.RTSpecialName;

			// Get the definition for the module initializer function .cctor
			var cctor = new MethodDefinition(".cctor", attributes, voidRef);

			// Insert into the body the method call CCPlatformInitializer.ConnectToPCL()
			// Note we importing this method from the platform-dependent assembly
			cctor.Body.Instructions.Add(Instruction.Create(OpCodes.Call, AssemblyShared.MainModule.Import(callee)));
			cctor.Body.Instructions.Add(Instruction.Create(OpCodes.Ret));

			TypeDefinition moduleClass = assemblyDef.MainModule.Types.FirstOrDefault(t => t.Name == "<Module>");
			Debug.Assert(moduleClass != null, "Found no module class in PCL\n");

			Debug.Write("Injecting platform specific code into PCL\n");
			moduleClass.Methods.Add(cctor);
		}

		static void WriteAssembly(AssemblyDefinition assemblyDef, string assemblyFile)
		{
			var writeParams = new WriterParameters();
			assemblyDef.Write(assemblyFile, writeParams);
			Debug.Write("Saved changed to PCL\n");
		}
	}
}
