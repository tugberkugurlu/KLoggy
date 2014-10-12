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
$artifactsWebPackRoot = "$artifactsRoot\_WebSites"
$artifactsKloggyWebRoot = "$artifactsWebPackRoot\approot\src\KLoggy.Web"
$packagesLocalRoot = "$solutionRoot\packages"
$srcRoot = "$solutionRoot\src"
$kloggyWebRoot = "$srcRoot\KLoggy.Web"
$kloggyDomainRoot = "$srcRoot\KLoggy.Domain"

## Refresh the process path env variable
## further info: http://stackoverflow.com/questions/17794507/reload-the-path-in-powershell
$env:PATH += ";$([System.Environment]::GetEnvironmentVariable("Path","USER"))"

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
if(test-globalnpm -eq $true) {
    
    Write-Output "Installing npm modules..."
    npm install --prefix $kloggyWebRoot
    
} else {
    
    ## TODO: Possibly install npm?
    throw "npm doesn't exists globally"
}

## install bower components
if(test-globalbower -eq $true) {

    Write-Output "Installing bower components..."
    bower install
    
} else {

    ## TODO: Possibly install bower?
    throw "bower doesn't exists globally"
}

## kpm restore
if(test-globalkpm -eq $true) {
    
    Write-Output "Restoring k packages..."
    kpm restore "$solutionRoot\src"
 
    if ($lastexitcode -ne 0) {
        throw ""
    }
 
    Write-Output "building the domin application..."
    kpm build "$kloggyDomainRoot" --configuration $configuration --out $artifactsRoot

    if ($lastexitcode -ne 0) {
        throw ""
    }

    Write-Output "building the web application..."
    kpm build "$kloggyWebRoot" --configuration $configuration --out $artifactsRoot
 
    if ($lastexitcode -ne 0) {
        throw ""
    }
 
} else {
    
    ## TODO: Possibly install kpm?
    throw "kpm doesn't exists globally"
}

## TODO: Write-Output "running the tests..."

if(test-globalgulp -eq $true) {

    Write-Output "Running the gulp tasks..."
    gulp --cwd $kloggyWebRoot
    
} else {
    
    ## TODO: Possibly use local gulp?
    throw "gulp doesn't exists globally"
}

## At this stage, we know that kpm exists globally
Write-Output "Packing the web application..."
kpm pack "$kloggyWebRoot" --configuration $configuration --out $artifactsWebPackRoot

if ($lastexitcode -ne 0) {
    throw ""
}

## Copy release configs
Copy-Item "$solutionRoot\config\config.release.ini" "$artifactsKloggyWebRoot\"