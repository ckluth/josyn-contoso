@echo off
CHCP 1252
setlocal

set "LOCAL_BUILD=%~dp0"

echo.
echo === contoso-adapter ===
call "%LOCAL_BUILD%..\contoso-adapter\.local-build\clean.cmd" NOPAUSE
if %ERRORLEVEL% neq 0 exit /b %ERRORLEVEL%

echo.
echo [OK] Clean abgeschlossen.
exit /b 0
