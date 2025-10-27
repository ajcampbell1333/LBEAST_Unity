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
        /// Compile the project and exit
        /// Exit code 0 = success, 1 = compilation errors
        /// </summary>
        public static void CompileAndExit()
        {
            Debug.Log("[LBEAST CLI] Starting compilation...");

            // Wait for any pending compilation to finish
            while (EditorApplication.isCompiling)
            {
                System.Threading.Thread.Sleep(100);
            }

            // Check for compilation errors
            string projectRoot = Application.dataPath.Replace("/Assets", "");
            string reportPath = Path.Combine(projectRoot, "Temp/CompilationErrors.log");

            if (File.Exists(reportPath))
            {
                string report = File.ReadAllText(reportPath);
                Debug.Log("[LBEAST CLI] Compilation Report:\n" + report);

                if (report.Contains("Status: FAILED"))
                {
                    Debug.LogError("[LBEAST CLI] Compilation FAILED");
                    EditorApplication.Exit(1);
                    return;
                }
            }

            Debug.Log("[LBEAST CLI] Compilation SUCCESS");
            EditorApplication.Exit(0);
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

