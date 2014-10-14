function test-globalgulp {
    $result = $true
    try {
        $(& gulp --version) | Out-Null
    }
    catch {
        $result = $false
    }
    
    return $result
}

function gulp-run {
    param(
        [String]
        [parameter(Mandatory=$true)]
        $sourceDirectory
    )
    
    exec {
        gulp --cwd "$sourceDirectory"
    }
}