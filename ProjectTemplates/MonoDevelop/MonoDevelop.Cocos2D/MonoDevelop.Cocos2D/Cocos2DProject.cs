using System;
using MonoDevelop.Projects;
using System.Xml;
using MonoDevelop.Core.Assemblies;

namespace MonoDevelop.Cocos2D
{	
	public static class Cocos2DBuildAction
	{
		public static readonly string Shader;
		
		public static bool IsCocos2DBuildAction(string action)
		{
			return action == Shader;
		}
		
		static Cocos2DBuildAction ()
		{
			Shader = "Cocos2DShader";
		}
	}
	
	public class Cocos2DProject :  DotNetAssemblyProject
	{
		public Cocos2DProject ()
		{
			Init ();
		}
		
		public Cocos2DProject (string languageName)
			: base (languageName)
		{
			Init ();
		}
		
		public Cocos2DProject (string languageName, ProjectCreateInformation info, XmlElement projectOptions)
			: base (languageName, info, projectOptions)
		{
			Init ();
		}
		
		private void Init()
		{
		}
		
		public override SolutionItemConfiguration CreateConfiguration (string name)
		{
			var conf = new Cocos2DProjectConfiguration (name);
			conf.CopyFrom (base.CreateConfiguration (name));
			return conf;
		}
			
		public override bool SupportsFormat (FileFormat format)
		{
			return format.Id == "MSBuild10";
		}
		
		public override TargetFrameworkMoniker GetDefaultTargetFrameworkForFormat (FileFormat format)
		{
			return new TargetFrameworkMoniker("4.0");
		}
		
		public override bool SupportsFramework (MonoDevelop.Core.Assemblies.TargetFramework framework)
		{
			if (!framework.IsCompatibleWithFramework (MonoDevelop.Core.Assemblies.TargetFrameworkMoniker.NET_4_0))
				return false;
			else
				return base.SupportsFramework (framework);
		}
		
		protected override System.Collections.Generic.IList<string> GetCommonBuildActions ()
		{			
			var actions = new System.Collections.Generic.List<string>(base.GetCommonBuildActions());
			actions.Add(Cocos2DBuildAction.Shader);
			return actions;
		}
		
		public override string GetDefaultBuildAction (string fileName)
		{
			if (System.IO.Path.GetExtension(fileName) == ".fx")
			{
				return Cocos2DBuildAction.Shader;
			}
			return base.GetDefaultBuildAction (fileName);
		}        

        protected override void PopulateSupportFileList(FileCopySet list, ConfigurationSelector solutionConfiguration)
        {
            base.PopulateSupportFileList(list, solutionConfiguration);
            //HACK: workaround for MD not local-copying package references
            foreach (var projectReference in References)
            {
                if (projectReference.Package != null && projectReference.Package.Name == "Cocos2D")
                {
                    if (projectReference.ReferenceType == ReferenceType.Gac)
                    {
                        foreach (var assem in projectReference.Package.Assemblies)
                        {
                            list.Add(assem.Location);
                            var cfg = (Cocos2DProjectConfiguration)solutionConfiguration.GetConfiguration(this);
                            if (cfg.DebugMode)
                            {
                                var mdbFile = TargetRuntime.GetAssemblyDebugInfoFile(assem.Location);
                                if (System.IO.File.Exists(mdbFile))
                                    list.Add(mdbFile);
                            }
                        }
                    }
                    break;
                }
            }
        }
				
	}
	
	public class Cocos2DBuildExtension : ProjectServiceExtension
	{
		
		protected override BuildResult Build (MonoDevelop.Core.IProgressMonitor monitor, SolutionEntityItem item, ConfigurationSelector configuration)
		{
#if DEBUG			
			monitor.Log.WriteLine("Cocos2D Extension Build Called");	
#endif			
			try
			{
			  return base.Build (monitor, item, configuration);
			}
			finally
			{
#if DEBUG				
			   monitor.Log.WriteLine("Cocos2D Extension Build Ended");	
#endif				
			}
		}
		
		protected override BuildResult Compile (MonoDevelop.Core.IProgressMonitor monitor, SolutionEntityItem item, BuildData buildData)
		{
#if DEBUG			
			monitor.Log.WriteLine("Cocos2D Extension Compile Called");	
#endif			
			try
			{				
				var proj = item as Cocos2DProject;
				if (proj == null)
				{
				   return base.Compile (monitor, item, buildData);
				}
				var results = new System.Collections.Generic.List<BuildResult>();
				foreach(var file in proj.Files)
				{
					if (Cocos2DBuildAction.IsCocos2DBuildAction(file.BuildAction))					
					{												
						buildData.Items.Add(file);
						var buildResult = Cocos2DContentProcessor.Compile(file, monitor, buildData);
						results.Add(buildResult);
					}
				}
				return base.Compile (monitor, item, buildData).Append(results);
			}
			finally
			{
#if DEBUG				
				monitor.Log.WriteLine("Cocos2D Extension Compile Ended");	
#endif				
			}
		}
	}
	
	public class Cocos2DProjectBinding : IProjectBinding
	{
		public Project CreateProject (ProjectCreateInformation info, System.Xml.XmlElement projectOptions)
		{ 
			string lang = projectOptions.GetAttribute ("language");
			return new Cocos2DProject (lang, info, projectOptions);
		}
	
		public Project CreateSingleFileProject (string sourceFile)
		{
			throw new InvalidOperationException ();
		}
		
		public bool CanCreateSingleFileProject (string sourceFile)
		{
			return false;
		}
		
		public string Name {
			get { return "Cocos2D"; }
		}
	}
	
	public class Cocos2DProjectConfiguration : DotNetProjectConfiguration
	{
		public Cocos2DProjectConfiguration () : base ()
		{
		}
		
		public Cocos2DProjectConfiguration (string name) : base (name)
		{
		}		
		
		public override void CopyFrom (ItemConfiguration configuration)
		{
			base.CopyFrom (configuration);
		}
	}
	
	public class Cocos2DContentProcessor 
	{		
		
		public static BuildResult Compile(ProjectFile file,MonoDevelop.Core.IProgressMonitor monitor,BuildData buildData)
		{			
			switch (file.BuildAction) {
			case "Cocos2DShader" :
				var result = new BuildResult();
				monitor.Log.WriteLine("Compiling Shader");					
				monitor.Log.WriteLine("Shader : "+buildData.Configuration.OutputDirectory);
				monitor.Log.WriteLine("Shader : "+file.FilePath);
				monitor.Log.WriteLine("Shader : "+file.ToString());
				return result;
			default:
				return new BuildResult();
			}
			
		}
	}
}

