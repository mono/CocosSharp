param($installPath, $toolsPath, $package, $project)

$allowedReferences = @("cocos2d-xna","box2d","ICSharpCode.SharpZipLib","MonoGame.Framework")

write-host "Running install.ps1 for $package"

# Full assembly name is required
Add-Type -AssemblyName 'Microsoft.Build, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'

$projectCollection = [Microsoft.Build.Evaluation.ProjectCollection]::GlobalProjectCollection

$allProjects = $projectCollection.GetLoadedProjects($project.Object.Project.FullName).GetEnumerator();

if($allProjects.MoveNext())
{
    $currentProject = $allProjects.Current

    foreach($Reference in $currentProject.GetItems('Reference') | ? {$allowedReferences -contains $_.Xml.Include.split(",")[0] })
    {
        $hintPath = $Reference.GetMetadataValue("HintPath")

        write-host "Matched againt $hintPath"

        #If it is x86 specific add condition 
        if ($hintPath -match '.*\\x86\\.*\.dll$')
        {
            write-host "X86 found"
            $Reference.Xml.Condition = " `$(Platform) == 'x86' "

            $condition = $Reference.Xml.Condition
            write-host "hintPath = $hintPath"
            write-host "condition = $condition"

            #Visual Studio doesnt allow the same reference twice (so try add friends)
            $matchingReferences = $currentProject.GetItems('Reference') | ? {($_.Xml.Include -eq $Reference.Xml.Include) -and ($_.GetMetadataValue("HintPath") -match ".*\\(ARM)\\.*\.dll$")}

            if (($matchingReferences | Measure-Object).Count -eq 0)
            {
                $ARM = $hintPath -replace '(.*\\)(x86)(\\.*\.dll)$', '$1ARM$3'
                $ARMPath = $ARM

                write-host "ARM $ARM"
                write-host "ARMPath $ARMPath"

                if (!(Test-Path $ARMPath)) {
                    #Add 
                    write-host "Adding reference to $ARM"

                    $metaData = new-object "System.Collections.Generic.Dictionary``2[[System.String, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089],[System.String, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]"
                    $metaData.Add("HintPath", $ARM)
                    $currentProject.AddItem('Reference', $Reference.Xml.Include, $metaData)

                    $newReference = $currentProject.GetItems('Reference') | ? {($_.Xml.Include -eq $Reference.Xml.Include) -and ($_.GetMetadataValue("HintPath") -eq $ARM)} | Select-Object -First 1

                    $newReference.Xml.Condition = " `$(Platform) != 'x86'"           
                }       
            }           
        }
    }
}