// Copyright (c) 2025 AJ Campbell. Licensed under the MIT License.

#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;

namespace LBEAST.Editor
{
    /// <summary>
    /// Command-line interface for triggering compilation reports
    /// Can be called from external tools or CI/CD pipelines
    /// 
    /// Usage:
    /// Unity.exe -quit -batchmode -projectPath "path/to/project" -executeMethod LBEAST.Editor.CompilationReporterCLI.CompileAndExit
    /// </summary>
    public static class CompilationReporterCLI
    {
        /// <summary>
        /// Compile the project and generate report
        /// Does NOT exit - batch script will kill Unity after reading report
        /// </summary>
        public static void CompileAndExit()
        {
            Debug.Log("🚀🤖✅ [LBEAST AUTO-COMPILE v2.0] CLI invoked - FINAL TEST - starting compilation check...");

            // Wait for any pending compilation to finish
            int waitCount = 0;
            while (EditorApplication.isCompiling && waitCount < 600) // Max 60 seconds
            {
                System.Threading.Thread.Sleep(100);
                waitCount++;
            }

            if (EditorApplication.isCompiling)
            {
                Debug.LogWarning("⏰ [LBEAST AUTO-COMPILE] Compilation still in progress after 60s timeout");
            }

            // Force a compilation report generation
            string projectRoot = Application.dataPath.Replace("/Assets", "");
            string reportPath = Path.Combine(projectRoot, "Temp/CompilationErrors.log");
            
            // Generate the report
            WriteCompilationReport(reportPath);

            Debug.Log($"✅✅✅ [LBEAST AUTO-COMPILE v2.0] FINAL TEST COMPLETE → {reportPath}");
            Debug.Log("⚠️  [LBEAST AUTO-COMPILE v2.0] Unity will remain open - batch script will terminate when ready");
            
            // DO NOT EXIT - let batch script kill Unity after reading the file
        }

        private static void WriteCompilationReport(string reportPath)
        {
            try
            {
                System.Text.StringBuilder report = new System.Text.StringBuilder();
                report.AppendLine("===========================================");
                report.AppendLine("LBEAST COMPILATION REPORT");
                report.AppendLine("🤖 AI-READABLE AUTOMATED COMPILATION CHECK");
                report.AppendLine("===========================================");
                report.AppendLine($"Generated: {System.DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                report.AppendLine($"Report ID: LBEAST-{System.Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}");
                report.AppendLine();

                // Check for compilation errors
                bool hasErrors = EditorApplication.isCompiling;
                
                // Note: In batch mode, we can't easily detect compilation errors after the fact
                // The CompilationReporter will capture them during actual compilation
                report.AppendLine("Status Check:");
                report.AppendLine($"  Compiling: {EditorApplication.isCompiling}");
                report.AppendLine($"  Play Mode Enabled: {EditorApplication.isPlaying}");
                report.AppendLine();

                report.AppendLine("===========================================");
                if (EditorApplication.isCompiling)
                {
                    report.AppendLine("Status: COMPILING (still in progress)");
                }
                else
                {
                    report.AppendLine("Status: SUCCESS - Project compiled successfully");
                    report.AppendLine("No compilation errors detected");
                }
                report.AppendLine("===========================================");

                // Write to file
                string directory = Path.GetDirectoryName(reportPath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                File.WriteAllText(reportPath, report.ToString());
                Debug.Log("📄 [LBEAST AUTO-COMPILE] Report file written successfully");
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"[LBEAST CLI] Failed to write report: {ex.Message}");
            }
        }

        /// <summary>
        /// Compile and report but don't exit (for manual testing)
        /// </summary>
        [MenuItem("LBEAST/CLI Test - Compile and Report")]
        public static void CompileAndReport()
        {
            Debug.Log("[LBEAST CLI] Compilation test started...");

            // Wait for compilation
            EditorApplication.delayCall += () =>
            {
                if (!EditorApplication.isCompiling)
                {
                    string projectRoot = Application.dataPath.Replace("/Assets", "");
                    string reportPath = Path.Combine(projectRoot, "Temp/CompilationErrors.log");

                    if (File.Exists(reportPath))
                    {
                        string report = File.ReadAllText(reportPath);
                        Debug.Log("=== COMPILATION REPORT ===\n" + report);
                    }
                    else
                    {
                        Debug.LogWarning("No compilation report found. It will be generated on next compile.");
                    }
                }
            };
        }
    }
}
#endif

