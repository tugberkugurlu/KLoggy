param(
    [String]
    [parameter(Mandatory=$true)]
    $configuration
)

function Clean-Solution ($solutionRoot) {
    
    $directories = $(Get-ChildItem "$solutionRoot\artifacts"),`
        $(Get-ChildItem "$solutionRoot\**\**\bin"),`
        $(Get-ChildItem "$solutionRoot\**\**\node_modules"),` 
        $(Get-ChildItem "$solutionRoot\**\**\bower_components")
    
    $directories | foreach ($_) { Remove-Item $_.FullName -Force -Recurse }
}

function Build-K ($projectFile, $configuration, $outputDirectory) {
    kpm build $projectFile --configuration $configuration --out $outputDirectory
}

$scriptRoot = (Split-Path -parent $MyInvocation.MyCommand.Definition)
$solutionRoot = (get-item $scriptRoot).parent.fullname
$artifactsRoot = "$solutionRoot\artifacts"
$artifactsBuildRoot = "$artifactsRoot\build"
$artifactsTestRoot = "$artifactsRoot\test"
$projectFileName = "project.json"
$artifactsWebPackRoot = "$artifactsRoot\_WebSites"
$srcRoot = "$solutionRoot\src"
$testsRoot = "$solutionRoot\test"
$appProjects = Get-ChildItem "$srcRoot\**\$projectFileName" | foreach { $_.FullName }
$testProjects = Get-ChildItem "$testsRoot\**\$projectFileName" | foreach { $_.FullName }

. $ScriptRoot\_Common.ps1

if(test-globalnpm -eq $false) { throw "npm doesn't exists globally" }
if(test-globalbower -eq $false) { throw "bower doesn't exists globally" }
if(test-globalkpm -eq $false) { throw "kpm doesn't exists globally" }
if(test-globalgulp -eq $false) { throw "gulp doesn't exists globally" }

Clean-Solution -solutionRoot $solutionRoot
$appProjects | foreach { Build-K -projectFile $_ -configuration $configuration -outputDirectory $artifactsBuildRoot }
$testProjects | foreach { Build-K -projectFile $_ -configuration $configuration -outputDirectory $artifactsTestRoot }