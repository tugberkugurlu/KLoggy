properties {
    $configuration = 'Release'
}

Include ".\core\utils.ps1"
Include ".\core\k.ps1"
Include ".\core\npm.ps1"
Include ".\core\bower.ps1"
Include ".\core\gulp.ps1"

$scriptRoot = (split-path -parent $MyInvocation.MyCommand.Definition)
$solutionRoot = (get-item $scriptRoot).parent.fullname
$artifactsRoot = "$solutionRoot\artifacts"
$artifactsBuildRoot = "$artifactsRoot\build"
$artifactsTestRoot = "$artifactsRoot\test"
$projectFileName = "project.json"
$artifactsAppsRoot = "$artifactsRoot\apps"
$srcRoot = "$solutionRoot\src"
$testsRoot = "$solutionRoot\test"
$appProjects = Get-ChildItem "$srcRoot\**\$projectFileName" | foreach { $_.FullName }
$testProjects = Get-ChildItem "$testsRoot\**\$projectFileName" | foreach { $_.FullName }

task default -depends Check, Run-Gulp, Pack

task Run-Gulp -depends Clean, Run-Npm, Run-Bower {
    $gulpProjects = $appProjects | where {
        $sourceDirectory = Split-Path $_
        $gulpFile = Join-Path $sourceDirectory "gulpfile.js"
        Test-Path $gulpFile
    }
    
    $gulpProjects | foreach {
        $sourceDirectory = Split-Path $_
        gulp-run $sourceDirectory
    }
}

task Run-Bower -depends Clean {
    $bowerProjects = $appProjects | where {
        $sourceDirectory = Split-Path $_
        $bowerJsonFile = Join-Path $sourceDirectory "bower.json"
        Test-Path $bowerJsonFile
    }
    
    $bowerProjects | foreach {
        $sourceDirectory = Split-Path $_
        bower-install $sourceDirectory
    }
}

task Run-Npm -depends Clean {
    $npmProjects = $appProjects | where {
        $sourceDirectory = Split-Path $_
        $packageJsonFile = Join-Path $sourceDirectory "package.json"
        Test-Path $packageJsonFile
    }
    
    $npmProjects | foreach {
        $sourceDirectory = Split-Path $_
        npm-install $sourceDirectory
    }
}

task Pack -depends Build, Test {
    $packableProjects = $appProjects |  
        where { 
            $projFile = $_;
            $projObj = (get-content $projFile) -join "`n" | ConvertFrom-Json
            $projObj | Get-Member | where { $_.MemberType -eq "NoteProperty" } | Test-Any { $_.Name -eq "commands" }
        }

    $packableProjects | foreach {
        $sourceDirectory = Split-Path $_
        $projName = Split-Path $sourceDirectory -Leaf
        $packDir = Join-Path $artifactsAppsRoot $projName
        k-pack -sourceDirectory $sourceDirectory -configuration $configuration -outputDirectory $packDir
    }
}

task Test -depends Build, Clean { 
    $testProjects | foreach {
        Write-Host $_
        k-run-test -projectFile $_
    }
}

task Check {
    if($(test-globalnpm) -eq $false) { throw "npm doesn't exists globally" }
    if($(test-globalbower) -eq $false) { throw "bower doesn't exists globally" }
    if($(test-globalkpm) -eq $false) { throw "kpm doesn't exists globally" }
    if($(test-globalgulp) -eq $false) { throw "gulp doesn't exists globally" }
}

task Build -depends Clean, Restore { 
    $appProjects | foreach {
        k-build -projectFile $_ -configuration $configuration -outputDirectory $artifactsBuildRoot
    }
    
    $testProjects | foreach {
        k-build -projectFile $_ -configuration $configuration -outputDirectory $artifactsTestRoot
    }
}

task Restore {
    @($srcRoot, $testsRoot) | foreach {
        k-restore $_
    }
}

task Clean {
    $directories = $(Get-ChildItem "$solutionRoot\artifacts*"),`
        $(Get-ChildItem "$solutionRoot\**\**\bin"),`
        $(Get-ChildItem "$solutionRoot\**\**\node_modules"),` 
        $(Get-ChildItem "$solutionRoot\**\**\bower_components")

    $directories | foreach ($_) { Remove-Item $_.FullName -Force -Recurse }
}

task ? -Description "Helper to display task info" {
    Write-Documentation
}