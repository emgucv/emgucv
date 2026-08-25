//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Emgu.CV
{
    /// <summary>
    /// Raises the WebGL initial memory size before a WebGL build if it is below
    /// the minimum cvextern.a needs to link. The full (opencv_contrib) cvextern.a
    /// variant shipped in Plugins/WebGL has a larger static data segment than
    /// the stock Unity default (32MB) allows, and fails to link with
    /// "wasm-ld: error: initial memory too small" otherwise. This is a no-op if
    /// you swap in the smaller mini variant instead, which doesn't need it.
    /// Only raises the value -- never lowers a project's own deliberate setting.
    /// </summary>
    public class WebGLBuildSettings : IPreprocessBuildWithReport
    {
        private const int MinimumInitialMemorySizeMb = 256;

        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report)
        {
            if (report.summary.platform != BuildTarget.WebGL)
                return;

            if (PlayerSettings.WebGL.initialMemorySize < MinimumInitialMemorySizeMb)
            {
                PlayerSettings.WebGL.initialMemorySize = MinimumInitialMemorySizeMb;
                Debug.Log(string.Format(
                    "Emgu CV: raised PlayerSettings.WebGL.initialMemorySize to {0}MB.",
                    MinimumInitialMemorySizeMb));
            }
        }
    }
}
#endif
