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
    /// the minimum cvextern.a needs to link. The stock Unity default (32MB) is
    /// enough for the mini cvextern.a variant shipped in Plugins/WebGL, but not
    /// for the full (opencv_contrib) variant, whose larger static data segment
    /// fails to link with "wasm-ld: error: initial memory too small" otherwise.
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
