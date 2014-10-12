Write-Output "Installing kvm from aspnet/home/dev..."
iex ((new-object net.webclient).DownloadString('https://raw.githubusercontent.com/aspnet/Home/dev/kvminstall.ps1'))

Write-Output "Installing latest kre..."
## TODO: This is bad but no choice :s
. "$($env:USERPROFILE)\.kre\bin\kvm.cmd" upgrade

Write-Output "Installing bower globally..."
npm install bower -g

Write-Output "Installing gulp globally..."
npm install gulp -g