using System;
using System.Linq;
using System.Collections.Generic;

using EnvDTE;
using Microsoft.VisualStudio.TemplateWizard;

namespace Cocos2DXNATemplateWizard
{
    // Child wizard is used by child project vstemplates

    public class ChildWizard : IWizard
    {
        // Retrieve global replacement parameters
        public void RunStarted(object automationObject, 
            Dictionary<string, string> replacementsDictionary, 
            WizardRunKind runKind, object[] customParams)
        {
            // Add custom parameters.
            replacementsDictionary.Add("$saferootprojectname$",
                RootWizard.GlobalDictionary["$saferootprojectname$"]);
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
