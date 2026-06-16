@echo off
CHCP 1252
setlocal

set "LOCAL_BUILD=%~dp0"

echo.
echo [INFO] Kein packbares Projekt in diesem Repo (Contoso.IdentityAdapter ist ein EXE, kein NuGet-Paket).
echo [OK] Pack abgeschlossen.
exit /b 0
