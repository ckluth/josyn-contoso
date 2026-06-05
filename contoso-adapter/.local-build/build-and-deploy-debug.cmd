@echo off
CHCP 1252
setlocal

set target=DEBUG

set "SRC=C:\Temp\VS.OUT\Contoso\Contoso.Josyn.Adapter\bin\%TARGET%"
set "DEPLOY=C:\Temp\VS.OUT\JOSYN\JOSYN.Jap.JAPServer\bin\%TARGET%\adapters"

echo.
echo [1/3] Build...
cd /d "%~dp0.."
dotnet build --configuration %TARGET% --no-restore
if %ERRORLEVEL% neq 0 ( echo   BUILD FEHLER & exit /b %ERRORLEVEL% )
echo   OK

echo.
echo [2/3] Erstelle adapters-Ordner falls nicht vorhanden...
if not exist "%DEPLOY%" mkdir "%DEPLOY%"
if %ERRORLEVEL% neq 0 ( echo   FEHLER beim Erstellen von %DEPLOY% & exit /b %ERRORLEVEL% )
echo   OK

echo.
echo [3/3] Deploy nach %DEPLOY%...
copy /y "%SRC%\Contoso.Josyn.Adapter.dll" "%DEPLOY%\Contoso.Josyn.Adapter.dll"
if %ERRORLEVEL% neq 0 ( echo   FEHLER beim Kopieren & exit /b %ERRORLEVEL% )
echo   OK

echo.
echo [OK] Contoso-Adapter gebaut und deployed.
