function test-globalbower {
    $result = $true
    try {
        $(& bower --version) | Out-Null
    }
    catch {
        $result = $false
    }
    
    return $result
}

function bower-install {
    param(
        [String]
        [parameter(Mandatory=$true)]
        $sourceDirectory
    )
    
    exec {
        ## rely on .bowerrc file for now. config.cwd is so weird. ref: https://github.com/bower/bower/issues/1561
        ## bower install --config.cwd "$sourceDirectory"
        bower install
    }
}