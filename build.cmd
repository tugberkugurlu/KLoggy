@echo off
cd %~dp0

:run
@powershell -NoProfile -ExecutionPolicy unrestricted -command "&{ .\build.ps1 }"