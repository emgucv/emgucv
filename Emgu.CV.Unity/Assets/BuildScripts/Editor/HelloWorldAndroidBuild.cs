using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

public static class HelloWorldAndroidBuild
{
    public static void Build()
    {
        // Deliberately not overriding AndroidExternalToolsSettings: Unity's own
        // Android module bundles a matching SDK/NDK/OpenJDK under
        // <Editor>/PlaybackEngines/AndroidPlayer/{SDK,NDK,OpenJDK}, and this
        // Editor version requires exactly NDK r27c -- pointing at a system NDK
        // (e.g. one installed for a different purpose via Android Studio) can
        // easily mismatch and fail with "is not valid Android NDK path".

        // The project's active build target was last switched to StandaloneOSX
        // (by the macOS build script) -- PlayerSettings.Android.* changes don't
        // reliably stick until Android is the active target/build target group.
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);

        // ARM64 on Android requires the IL2CPP scripting backend (Mono doesn't
        // support arm64) -- the project has apparently never configured Android
        // settings before (no AndroidTargetArchitectures/backend keys existed),
        // so this almost certainly defaulted to Mono2x, which silently surfaces
        // as Unity's generic "Target architecture not specified" build error
        // rather than a clearer backend-mismatch message.
        PlayerSettings.SetScriptingBackend(NamedBuildTarget.Android, ScriptingImplementation.IL2CPP);
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;
        EditorUserBuildSettings.androidBuildSystem = AndroidBuildSystem.Gradle;
        EditorUserBuildSettings.buildAppBundle = false;
        AssetDatabase.SaveAssets();

        BuildPlayerOptions options = new BuildPlayerOptions
        {
            scenes = new[] { "Assets/Emgu.CV/Demo/HelloWorldScene.unity" },
            locationPathName = "Build/HelloWorldAndroid/Emgu.CV.UnityDemo.apk",
            target = BuildTarget.Android,
            options = BuildOptions.None
        };

        BuildReport report = BuildPipeline.BuildPlayer(options);
        if (report.summary.result != BuildResult.Succeeded)
        {
            EditorApplication.Exit(1);
        }
    }
}
