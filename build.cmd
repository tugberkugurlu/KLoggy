@echo off
cd %~dp0

IF "%1" == "" (

  @powershell -NoProfile -ExecutionPolicy unrestricted -File "scripts\build.ps1" "-configuration" "Release"
  
) else (

  @powershell -NoProfile -ExecutionPolicy unrestricted -File "scripts\build.ps1" -configuration "%1%"
)