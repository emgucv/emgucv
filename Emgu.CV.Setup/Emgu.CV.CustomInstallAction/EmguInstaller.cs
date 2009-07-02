using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using System.ComponentModel;
using System.Configuration.Install;
using Microsoft.Win32;

namespace Emgu.CV.CustomInstallAction
{

    [RunInstaller(true)]
    public class EmguInstaller : Installer
    {
        private static List<String> foldersToDeploy = new List<string>()
        {
            "zh-CN"
        };
        private static List<String> filesToDeploy = new List<string>()
        {
            @"zh-CN\Emgu.CV.UI.resources.dll",
            @"zh-CN\ZedGraph.resources.dll",
            //"cv110.dll",
            //"cxcore110.dll",
            //"highgui110.dll",
            "Emgu.CV.dll",
            "Emgu.Util.dll",
            "Emgu.CV.UI.dll",
            "Emgu.CV.DebuggerVisualizers.dll",
            "ZedGraph.dll",
            "zlib.net.dll"
        };

        private static void CleanUp()
        {
            foreach (String vsFolder in GetVisualStudioInstallationPath())
            {
                String deploymentFolder = Path.Combine(vsFolder, @"..\Packages\Debugger\Visualizers");
                if (Directory.Exists(deploymentFolder))
                {
                    foreach (String f in filesToDeploy)
                    {
                        String file = Path.Combine(deploymentFolder, f);
                        if (File.Exists(file))
                            File.Delete(file);
                    }

                    foreach (String f in foldersToDeploy)
                    {
                        String folder = Path.Combine(deploymentFolder, f);
                        if (Directory.Exists(folder))
                            Directory.Delete(folder, false);
                    }
                }
                    
            }
        }

        public override void Install(System.Collections.IDictionary state)
        {
            base.Install(state);

            String executablePath = Path.Combine(ExecutingAssemblyPath, "bin");

            foreach (String vsFolder in GetVisualStudioInstallationPath())
            {
                String deploymentFolder = Path.Combine(vsFolder, @"..\Packages\Debugger\Visualizers");
                if (Directory.Exists(deploymentFolder))
                {
                    foreach (String folder in foldersToDeploy)
                    {
                        String f = Path.Combine(deploymentFolder, folder);
                        if (!Directory.Exists(f))
                            Directory.CreateDirectory(f);
                    }

                    foreach (String file in filesToDeploy)
                    {
                        File.Copy(
                            Path.Combine(executablePath, file),
                            Path.Combine(deploymentFolder, file),
                            true);
                    }
                }
            }
            /*
            using (RegistryKey reg = GetPathRegKey(true))
            {
                string curPath = (string)reg.GetValue("Path", "", RegistryValueOptions.DoNotExpandEnvironmentNames);
                state.Add("OldPath", curPath);
                List<String> paths = curPath.Split(';').ToList<string>();
                paths.Add(executablePath);
                reg.SetValue("Path", String.Join(";", paths.ToArray()), RegistryValueKind.ExpandString);
            }*/
        }

        public override void Uninstall(System.Collections.IDictionary state)
        {
            base.Uninstall(state);
            CleanUp();
            /*
            using (RegistryKey reg = GetPathRegKey(true))
            {
                string executablePath = Path.Combine(ExecutingAssemblyPath, "bin");
                string curPath = (string)reg.GetValue("Path", "", RegistryValueOptions.DoNotExpandEnvironmentNames);
                
                state.Add("OldPath", curPath);
                reg.SetValue(
                    "PATH", 
                    String.Join(";", (from s in curPath.Split(';') where (!s.Equals(executablePath)) select s).ToArray<String>()),
                    RegistryValueKind.ExpandString);
            }*/
        }

        public override void Rollback(System.Collections.IDictionary savedState)
        {
            base.Rollback(savedState);
            CleanUp();
            
            /*
            if (savedState.Contains("OldPath"))
            using (RegistryKey reg = GetPathRegKey(true))
            {
                reg.SetValue(
                    "PATH",
                    savedState["OldPath"],
                    RegistryValueKind.ExpandString);
            }*/
        }

        private static IEnumerable<string> GetVisualStudioInstallationPath()
        {
            String vs2008 = Registry.LocalMachine
                .OpenSubKey(@"SOFTWARE\Microsoft\VisualStudio\9.0", false)
                .GetValue("InstallDir", null)
                as String;
            if (!String.IsNullOrEmpty(vs2008))
                yield return vs2008;

            String vs2005 = Registry.LocalMachine
                .OpenSubKey(@"SOFTWARE\Microsoft\VisualStudio\8.0", false)
                .GetValue("InstallDir", null)
                as String;
            if (!String.IsNullOrEmpty(vs2005))
                yield return vs2005;
        }

        private static string ExecutingAssemblyPath
        {
            get
            {
                return System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            }
        }
        
        private static RegistryKey GetPathRegKey(bool writable)
        {
            // for the user-specific path...
            //return Registry.CurrentUser.OpenSubKey("Environment", writable);

            // for the system-wide path...
            return Registry.LocalMachine.OpenSubKey(
                @"SYSTEM\CurrentControlSet\Control\Session Manager\Environment", writable);
        }

    }
}
