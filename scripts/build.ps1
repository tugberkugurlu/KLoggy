$scriptDir = (Split-Path -parent $MyInvocation.MyCommand.Definition)
$solutionDir = (get-item $scriptDir).parent.fullname
$packagesDir = "$solutionDir\packages"
$psakeToolsDir = "$packagesDir\psake\tools"

## How Do I run psake? https://github.com/psake/psake/wiki/How-do-I-run-psake%3F
## How can I access functions that are in other script files from within psake? https://github.com/psake/psake/wiki/How-can-I-access-functions-that-are-in-other-script-files-from-within-psake%3F

Import-Module (join-path $psakeToolsDir "psake.psm1") -force
Invoke-psake "$scriptDir\default.ps1"