function Update-Environment {
6 
7    $locations = 'HKLM:\SYSTEM\CurrentControlSet\Control\Session Manager\Environment',
8                 'HKCU:\Environment'
9 
10    $locations | ForEach-Object {
11        $k = Get-Item $_
12        $k.GetValueNames() | ForEach-Object {
13            $name  = $_
14            $value = $k.GetValue($_)
15 
16            if ($userLocation -and $name -ieq 'PATH') {
17                $env:path += ";$value"
18            } else {
19                Set-Item -Path Env:\$name -Value $value
20            }
21        }
22 
23        $userLocation = $true
24    }
25}
26 
27iex ((new-object net.webclient).DownloadString('https://raw.githubusercontent.com/aspnet/Home/master/kvminstall.ps1'))
28Update-Environment
29kvm install latest -r CLR
30npm install bower -g
31npm install gulp -g