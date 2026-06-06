@echo off
CHCP 1252 >nul

if "%~1"=="" (
    echo.
    echo  Diese Datei ist nicht zum direkten Aufruf gedacht.
    echo  Bitte eine der arguments-*.cmd Dateien in diesem Ordner verwenden.
    echo.
    pause
    exit /b 1
)

set JOSYN_CLI=C:\ProgramData\JOSYN\CLI\JOSYN.Backend.CLI.exe
set JOB_NAME=Contoso.DemoProduct.DemoJob

echo.
echo  Job:   %JOB_NAME%
echo  Args:  %~nx1
echo.

set /p CONFIRM="ENTER zum Starten, CTRL-C zum Abbrechen... "

echo.
"%JOSYN_CLI%" run-job "%JOB_NAME%" "%~1"
exit /b %ERRORLEVEL%
