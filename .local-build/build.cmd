@echo off
CHCP 1252
setlocal

set "CONFIGURATION=%~1"
if not defined CONFIGURATION set "CONFIGURATION=Release"

if /i "%CONFIGURATION%" neq "Release" if /i "%CONFIGURATION%" neq "Debug" (
    echo [FEHLER] Unbekannte Konfiguration: "%CONFIGURATION%"
    echo          Erlaubt: Release, Debug
    exit /b 1
)

set "LOCAL_BUILD=%~dp0"

echo.
echo === contoso-adapter ===
call "%LOCAL_BUILD%..\contoso-adapter\.local-build\build.cmd" %CONFIGURATION%
if %ERRORLEVEL% neq 0 exit /b %ERRORLEVEL%

echo.
echo === contoso-demo-job ===
call "%LOCAL_BUILD%..\contoso-demo-job\.local-build\build.cmd" %CONFIGURATION%
if %ERRORLEVEL% neq 0 exit /b %ERRORLEVEL%

echo.
echo [OK] Alle Projekte erfolgreich gebaut (%CONFIGURATION%).
exit /b 0
