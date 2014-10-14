function test-globalnpm {
    $result = $true
    try {
        $(& npm --version) | Out-Null
    }
    catch {
        $result = $false
    }
    
    return $result
}

function npm-install {
    param(
        [String]
        [parameter(Mandatory=$true)]
        $sourceDirectory
    )
    
    exec {
        npm install --prefix "$sourceDirectory"
    }
}