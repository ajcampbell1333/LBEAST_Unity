# LBEAST Unity Compilation Testing

Automated compilation checking for Unity projects, designed for external tools, CI/CD pipelines, and AI assistants.

---

## 🎯 Purpose

This system allows external tools (like AI assistants, build servers, or scripts) to:
1. **Trigger Unity compilation** without opening the Editor GUI
2. **Capture all compilation errors** to a readable log file
3. **Verify project health** before pushing to source control

---

## 📁 Components

### **1. CompilationReporter.cs** (Editor Script)
- Automatically captures compilation errors/warnings
- Writes to `Temp/CompilationErrors.log`
- Runs automatically on every compilation (in Editor or Batch mode)

### **2. CompilationReporterCLI.cs** (Editor Script)
- Command-line interface for batch compilation
- Returns exit code 0 (success) or 1 (failure)
- Can be called from external tools

### **3. CompileProject.bat** (Windows Batch Script)
- **Double-click to run** compilation check
- Automatically finds Unity installation
- Displays results in console

### **4. CompileProject.ps1** (PowerShell Script)
- **More robust** version with better Unity detection
- Color-coded output
- Recommended for automation

---

## 🚀 Usage

### **Option 1: Manual Double-Click (Easiest)**

**Windows:**
```
Double-click CompileProject.bat
```

**Or run from command line:**
```batch
cd LBEAST_Unity
CompileProject.bat
```

### **Option 2: PowerShell (Recommended)**

```powershell
cd LBEAST_Unity
.\CompileProject.ps1
```

### **Option 3: Direct Unity Command (Advanced)**

```batch
Unity.exe -quit -batchmode -nographics ^
  -projectPath "F:\LBEAST\LBEAST_Unity" ^
  -executeMethod LBEAST.Editor.CompilationReporterCLI.CompileAndExit ^
  -logFile "Temp\UnityBatchCompile.log"
```

### **Option 4: From AI Assistant/External Tool**

1. **Run** `CompileProject.bat` or `CompileProject.ps1`
2. **Wait** for completion (1-2 minutes)
3. **Read** `Temp/CompilationErrors.log` for results

---

## 📊 Output Files

### **Temp/CompilationErrors.log**
Structured compilation report:
```
===========================================
LBEAST COMPILATION REPORT
===========================================
Started: 2025-10-26 14:30:15

Assembly: LBEAST.Runtime
-------------------------------------------
ERRORS: 2
  [Error] Assets/LBEAST/Runtime/Example.cs(42,5): CS1061: ...
  [Error] Assets/LBEAST/Runtime/Example.cs(43,5): CS0103: ...

===========================================
Finished: 2025-10-26 14:30:45
Status: FAILED - Compilation errors detected
===========================================
```

### **Temp/UnityBatchCompile.log**
Full Unity Editor log (verbose, includes all internal Unity messages)

---

## 🔧 Integration Examples

### **For AI Assistants**

```bash
# Run compilation
./CompileProject.bat

# Read results
cat Temp/CompilationErrors.log
```

### **For CI/CD (GitHub Actions)**

```yaml
- name: Compile Unity Project
  run: |
    cd LBEAST_Unity
    ./CompileProject.ps1
  
- name: Upload Compilation Report
  if: failure()
  uses: actions/upload-artifact@v3
  with:
    name: compilation-errors
    path: LBEAST_Unity/Temp/CompilationErrors.log
```

### **For Build Scripts**

```batch
@echo off
call CompileProject.bat
if %errorlevel% neq 0 (
    echo Build failed - see Temp\CompilationErrors.log
    exit /b 1
)
echo Build successful!
```

---

## 🎯 Exit Codes

| Code | Meaning |
|------|---------|
| `0` | Compilation successful, no errors |
| `1` | Compilation failed, errors present |

---

## ⚙️ Configuration

### **Changing Unity Version**

Edit `CompileProject.bat` or `CompileProject.ps1`:

```batch
REM CompileProject.bat
set UNITY_PATH="C:\Program Files\Unity\Hub\Editor\YOUR_VERSION\Editor\Unity.exe"
```

```powershell
# CompileProject.ps1
# Script automatically searches for installed Unity versions
# Or manually set:
$unityPath = "C:\Program Files\Unity\Hub\Editor\YOUR_VERSION\Editor\Unity.exe"
```

### **Compilation Timeout**

Unity batch mode has no built-in timeout. If compilation hangs:
- Check `Temp/UnityBatchCompile.log` for errors
- Kill the `Unity.exe` process manually
- Restart Unity Editor to fix corrupted state

---

## 🐛 Troubleshooting

### **"Unity executable not found"**
- Install Unity via Unity Hub
- Or edit the script to set `UNITY_PATH` manually

### **Report file not generated**
- Check `Temp/UnityBatchCompile.log` for errors
- Ensure CompilationReporter.cs compiled successfully
- Try opening Unity Editor manually to verify it works

### **Script hangs indefinitely**
- Unity may be waiting for user input (rare in batch mode)
- Check `Temp/UnityBatchCompile.log` for prompts
- Kill Unity.exe and check for project corruption

### **Exit code 0 but errors exist**
- CompilationReporterCLI may not have run
- Check that `LBEAST.Editor` namespace compiled
- Manually trigger via Unity menu: `LBEAST > CLI Test - Compile and Report`

---

## 📝 Notes

- **First run may take longer** (3-5 minutes) as Unity imports packages
- **Subsequent runs are faster** (1-2 minutes)
- **Report is overwritten** on each run
- **Unity log is append-only** and can grow large (safe to delete)
- **Compilation occurs on project load** - no changes needed
- **Works offline** - no internet connection required

---

## 🎉 Success Indicators

**In Console:**
```
✓ COMPILATION SUCCESSFUL
Exit Code: 0
```

**In Log File:**
```
Status: SUCCESS - No errors or warnings
```

---

**For questions or issues:** [GitHub Issues](https://github.com/ajcampbell1333/LBEAST/issues)



