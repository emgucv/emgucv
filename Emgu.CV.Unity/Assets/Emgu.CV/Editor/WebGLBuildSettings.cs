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
    ///
    /// Also enables WebGL exception support if it is currently "None". cvextern.a
    /// is compiled without -fwasm-exceptions for this platform (see
    /// cmake/EmscriptenBuildFlags.cmake's EMGU_CV_EMSCRIPTEN_WASM_EXCEPTIONS),
    /// matching Unity's own legacy JS-exception model -- but that model only
    /// actually catches exceptions if PlayerSettings.WebGL.exceptionSupport is
    /// "Explicit Only" or higher; a new project defaults to "None"
    /// (DISABLE_EXCEPTION_CATCHING=1), under which ANY exception thrown inside
    /// cvextern.a -- including ones OpenCV throws and catches internally via
    /// CVAPI_CATCH_CV_ERRORS -- surfaces instead as an uncaught, message-less
    /// wasm trap ("Uncaught undefined") on literally the first native call made,
    /// even a trivial `new Mat(64, 64, DepthType.Cv8U, 3)`.
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

            if (PlayerSettings.WebGL.exceptionSupport == WebGLExceptionSupport.None)
            {
                PlayerSettings.WebGL.exceptionSupport = WebGLExceptionSupport.FullWithStacktrace;
                Debug.Log(
                    "Emgu CV: raised PlayerSettings.WebGL.exceptionSupport from None to FullWithStacktrace " +
                    "(required for cvextern.a's internal exception handling to work at all on WebGL).");
            }
        }
    }
}
#endif
