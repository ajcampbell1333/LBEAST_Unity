# LBEAST Unity Compilation Check (PowerShell)
# Launches Unity in batch mode, compiles project, and outputs errors to log
# Copyright (c) 2025 AJ Campbell. Licensed under the MIT License.

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "LBEAST Unity Compilation Check" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Find Unity executable
$unityPath = $null
$commonPaths = @(
    "C:\Program Files\Unity\Hub\Editor\6.0.27f1\Editor\Unity.exe",
    "C:\Program Files\Unity\Hub\Editor\2022.3.54f1\Editor\Unity.exe",
    "C:\Program Files\Unity\Hub\Editor\2023.2.20f1\Editor\Unity.exe",
    "C:\Program Files\Unity\Editor\Unity.exe"
)

foreach ($path in $commonPaths) {
    if (Test-Path $path) {
        $unityPath = $path
        break
    }
}

# Search Unity Hub for any installed editor
if (-not $unityPath) {
    $hubEditorPath = "C:\Program Files\Unity\Hub\Editor"
    if (Test-Path $hubEditorPath) {
        $versions = Get-ChildItem $hubEditorPath | Where-Object { $_.PSIsContainer }
        foreach ($version in $versions) {
            $exePath = Join-Path $version.FullName "Editor\Unity.exe"
            if (Test-Path $exePath) {
                $unityPath = $exePath
                Write-Host "Found Unity $($version.Name)" -ForegroundColor Green
                break
            }
        }
    }
}

if (-not $unityPath) {
    Write-Host "ERROR: Unity executable not found" -ForegroundColor Red
    Write-Host ""
    Write-Host "Please edit this script or install Unity via Unity Hub" -ForegroundColor Yellow
    Write-Host "Common paths checked:" -ForegroundColor Yellow
    foreach ($path in $commonPaths) {
        Write-Host "  - $path" -ForegroundColor Gray
    }
    Write-Host ""
    Read-Host "Press Enter to exit"
    exit 1
}

Write-Host "Unity Path: $unityPath" -ForegroundColor Green
$projectPath = $PSScriptRoot
Write-Host "Project Path: $projectPath" -ForegroundColor Green
Write-Host ""

# Run Unity in batch mode
Write-Host "Starting Unity compilation (this may take 1-2 minutes)..." -ForegroundColor Yellow
Write-Host "Please wait..." -ForegroundColor Yellow
Write-Host ""

$logFile = Join-Path $projectPath "Temp\UnityBatchCompile.log"
$reportFile = Join-Path $projectPath "Temp\CompilationErrors.log"

$arguments = @(
    "-quit",
    "-batchmode",
    "-nographics",
    "-projectPath", "`"$projectPath`"",
    "-executeMethod", "LBEAST.Editor.CompilationReporterCLI.CompileAndExit",
    "-logFile", "`"$logFile`""
)

$process = Start-Process -FilePath $unityPath -ArgumentList $arguments -Wait -PassThru -NoNewWindow

$exitCode = $process.ExitCode

Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Unity Compilation Complete" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Exit Code: $exitCode" -ForegroundColor $(if ($exitCode -eq 0) { "Green" } else { "Red" })
Write-Host ""

# Display compilation report
if (Test-Path $reportFile) {
    Write-Host "Compilation Report:" -ForegroundColor Cyan
    Write-Host "-------------------------------------------" -ForegroundColor Gray
    Get-Content $reportFile | Write-Host
    Write-Host "-------------------------------------------" -ForegroundColor Gray
    Write-Host ""
    Write-Host "Full report saved to: Temp\CompilationErrors.log" -ForegroundColor Gray
    Write-Host "Unity log saved to: Temp\UnityBatchCompile.log" -ForegroundColor Gray
} else {
    Write-Host "WARNING: Compilation report not found" -ForegroundColor Yellow
    Write-Host "Check Unity log for details: Temp\UnityBatchCompile.log" -ForegroundColor Yellow
}

Write-Host ""
if ($exitCode -eq 0) {
    Write-Host "✓ COMPILATION SUCCESSFUL" -ForegroundColor Green
} else {
    Write-Host "✗ COMPILATION FAILED" -ForegroundColor Red
    Write-Host ""
    Write-Host "Check the logs above for error details" -ForegroundColor Yellow
}
Write-Host ""

Read-Host "Press Enter to exit"
exit $exitCode




