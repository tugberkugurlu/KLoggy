function test-globalkpm {
    $result = $true
    try {
        $(& kpm --version) | Out-Null
    }
    catch {
        $result = $false
    }
    
    return $result
}

function k-build {
    param(    
        [String]
        [parameter(Mandatory=$true)]
        $projectFile,
        
        [String]
        [parameter(Mandatory=$true)]
        $configuration, 
        
        [String]
        [parameter(Mandatory=$true)]
        $outputDirectory
    )
    
    exec {
        kpm build $projectFile --configuration $configuration --out "$outputDirectory"
    }
}

function k-restore {
    param(
        [String]
        [parameter(Mandatory=$true)]
        $sourceDirectory
    )
    
    exec {
        kpm restore $sourceDirectory
    }
}

function k-run {
    param(
        [String]
        [parameter(Mandatory=$true)]
        $command,
        
        [String]
        [parameter(Mandatory=$true)]
        $sourceDirectory
    )

    exec {
        $env:K_APPBASE = "$sourceDirectory"
        k $command
    }
}

function k-pack {
    param(
        [String]
        [parameter(Mandatory=$true)]
        $sourceDirectory,
        
        [String]
        [parameter(Mandatory=$true)]
        $configuration,
        
        [String]
        [parameter(Mandatory=$true)]
        $outputDirectory
    )
    
    exec {
        kpm pack "$sourceDirectory" --configuration $configuration --out "$outputDirectory"
    }
}

## inspired by _k-test.shade from KoreBuild
function k-run-test {
    param(
        [String]
        [parameter(Mandatory=$true)]
        $projectFile
    )
    
    if((test-path $projectFile) -eq $false) 
    {
        throw "projectFile doesn't exists. projectFile: $projectFile"
    }
    
    $testProjectDir = (split-path $projectFile)
    $projectObj = (get-content $projectFile) -join "`n" | ConvertFrom-Json
    $hasCommands = $projectObj | 
        Get-Member | 
        where { $_.MemberType -eq "NoteProperty" } | 
        Test-Any { $_.Name -eq "commands" }
    
    if($hasCommands -eq $true) 
    {
        $hasTestCommand = $projectObj.commands | 
            Get-Member | 
            where { $_.MemberType -eq "NoteProperty" } | 
            Test-Any { $_.Name -eq "test" }
    
        if($hasTestCommand -eq $true)
        {
            $hasFrameworks = $projectObj | 
                Get-Member | 
                where { $_.MemberType -eq "NoteProperty" } | 
                Test-Any { $_.Name -eq "frameworks" }
        
            if($hasFrameworks -eq $true) 
            {
                $supportedFrameworkPrefixes = @("net", "aspnet")
                $isSupported = $projectObj.frameworks | 
                    Get-Member | 
                    where { $_.MemberType -eq "NoteProperty" } | 
                    Test-Any { 
                        $property = $_; 
                        $supportedFrameworkPrefixes | Test-Any { 
                            $property.Name.StartsWith($_)  
                        }
                    }
                    
                if($isSupported -eq $true) 
                {
                    k-run "test" "$testProjectDir"
                }
                else 
                {
                    Write-Output "Not supported env for running the tests. Skipping..."
                }
            }
            else 
            {
                throw "TODO: Not sure. Is it possible for a project to not to have frameworks array?"
            }
        }
    }
}