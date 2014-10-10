param(

    [String]
    [parameter(Mandatory=$true)]
    $configuration
)

## When build.cmd is called, we are always inside the ./ path.
## Assume that we will always be.

$scriptRoot = (Split-Path -parent $MyInvocation.MyCommand.Definition)
$solutionRoot = (get-item $scriptRoot).parent.fullname
$artifactsRoot = "$solutionRoot\artifacts"
$packagesLocalRoot = "$solutionRoot\packages"
$srcRoot = "$solutionRoot\src"
$kloggyWebRoot = "$srcRoot\KLoggy.Web"

## load dependencies
. $ScriptRoot\_Common.ps1

function Remove-Dir($path) {
    if((Test-Path $path) -eq $true) {
        Remove-Item -Force -Recurse $path
    }
}

## clean up
Remove-Dir -path "$artifactsRoot\"
Remove-Dir -path "$packagesLocalRoot\"
Remove-Dir -path "$kloggyWebRoot\bin\"
Remove-Dir -path "$kloggyWebRoot\node_modules\"
Remove-Dir -path "$kloggyWebRoot\bower_components\"

## install npm components
if(check-globalnpm -eq $true) {
    
    Write-Output "Installing npm modules..."
    npm install --prefix $kloggyWebRoot
    
} else {
    
    // TODO: Possibly install npm?
    throw "npm doesn't exists globally"
}

## install bower components
if(check-globalbower -eq $true) {

    Write-Output "Installing bower components..."
    bower install
    
} else {

    // TODO: Possibly install bower?
    throw "bower doesn't exists globally"
}

## kpm restore
if(check-globalkpm -eq $true) {
    
    Write-Output "Restoring k packages..."
    kpm restore "$solutionRoot\src" --packages $packagesLocalRoot
 
} else {
    
    // TODO: Possibly install kpm?
    throw "kpm doesn't exists globally"
}

## Write-Output "Building the projects..."
## Write-Output "running the tests..."
## Write-Output "Running the gulp tasks..."
## Write-Output "Packing the web application..."