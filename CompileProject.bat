@echo off
REM LBEAST Unity Compilation Check
REM Launches Unity in batch mode, compiles project, and outputs errors to log
REM Copyright (c) 2025 AJ Campbell. Licensed under the MIT License.

echo ========================================
echo LBEAST Unity Compilation Check
echo ========================================
echo.

REM Find Unity executable
set UNITY_PATH=""
if exist "C:\Program Files\Unity\Hub\Editor\6.0.27f1\Editor\Unity.exe" (
    set UNITY_PATH="C:\Program Files\Unity\Hub\Editor\6.0.27f1\Editor\Unity.exe"
) else if exist "C:\Program Files\Unity\Hub\Editor\2022.3.54f1\Editor\Unity.exe" (
    set UNITY_PATH="C:\Program Files\Unity\Hub\Editor\2022.3.54f1\Editor\Unity.exe"
) else (
    echo ERROR: Unity executable not found
    echo Please edit this script to set UNITY_PATH manually
    echo.
    echo Common paths:
    echo   C:\Program Files\Unity\Hub\Editor\[VERSION]\Editor\Unity.exe
    echo   C:\Program Files\Unity\Editor\Unity.exe
    echo.
    pause
    exit /b 1
)

echo Unity Path: %UNITY_PATH%
echo Project Path: %~dp0
echo.

REM Get project path (remove trailing backslash)
set PROJECT_PATH=%~dp0
set PROJECT_PATH=%PROJECT_PATH:~0,-1%

REM Run Unity in batch mode to compile
echo Starting Unity compilation (this may take 1-2 minutes)...
echo Please wait...
echo.

%UNITY_PATH% -quit -batchmode -nographics -projectPath "%PROJECT_PATH%" -executeMethod LBEAST.Editor.CompilationReporterCLI.CompileAndExit -logFile "%PROJECT_PATH%\Temp\UnityBatchCompile.log"

set UNITY_EXIT_CODE=%errorlevel%

echo.
echo ========================================
echo Unity Compilation Complete
echo ========================================
echo Exit Code: %UNITY_EXIT_CODE%
echo.

REM Check if compilation report was generated
if exist "%PROJECT_PATH%\Temp\CompilationErrors.log" (
    echo Compilation Report:
    echo -------------------------------------------
    type "%PROJECT_PATH%\Temp\CompilationErrors.log"
    echo -------------------------------------------
    echo.
    echo Full report saved to: Temp\CompilationErrors.log
    echo Unity log saved to: Temp\UnityBatchCompile.log
) else (
    echo WARNING: Compilation report not found
    echo Check Unity log for details: Temp\UnityBatchCompile.log
)

echo.
if %UNITY_EXIT_CODE% EQU 0 (
    echo ✓ COMPILATION SUCCESSFUL
) else (
    echo ✗ COMPILATION FAILED
)
echo.

pause
exit /b %UNITY_EXIT_CODE%

