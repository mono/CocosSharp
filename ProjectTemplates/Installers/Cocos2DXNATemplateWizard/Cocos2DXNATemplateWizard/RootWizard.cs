using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using EnvDTE;
using EnvDTE80;
//using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TemplateWizard;
//using Microsoft.VisualStudio.Shell;
//using Microsoft.VisualStudio.Shell.Interop;
//using Microsoft.VisualStudio.Web.Silverlight;

// This assembly must be signed and either installed in the GAC
// or included as Content in a VSIX project.
// Then it is referenced from vstemplates.
// Custom replacement can be used in projects and files.

namespace Cocos2DXNATemplateWizard
{
    // Root wizard is used by root project vstemplate

    public class RootWizard : IWizard
    {
        // Use to communicate $saferootprojectname$ to ChildWizard
        public static Dictionary<string, string> GlobalDictionary =
            new Dictionary<string,string>();

        // Add global replacement parameters
        public void RunStarted(object automationObject, 
            Dictionary<string, string> replacementsDictionary, 
            WizardRunKind runKind, object[] customParams)
        {
            // Place "$saferootprojectname$ in the global dictionary.
            // Copy from $safeprojectname$ passed in my root vstemplate
            GlobalDictionary["$saferootprojectname$"] = replacementsDictionary["$safeprojectname$"];
        }

        public void RunFinished()
        {
        }

        public void BeforeOpeningFile(ProjectItem projectItem)
        {
        }

        public void ProjectFinishedGenerating(Project project)
        {
        }

        public bool ShouldAddProjectItem(string filePath)
        {
            return true;
        }

        public void ProjectItemFinishedGenerating(ProjectItem projectItem)
        {
        }
    }
}
