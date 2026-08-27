using UnityEditor;
using UnityEditor.Build.Reporting;

public static class HelloWorldIOSBuild
{
    public static void Build()
    {
        PlayerSettings.iOS.cameraUsageDescription = "Used by the Emgu CV demo for camera-based image processing.";
        PlayerSettings.iOS.sdkVersion = iOSSdkVersion.SimulatorSDK;
        PlayerSettings.iOS.simulatorSdkArchitecture = AppleMobileArchitectureSimulator.ARM64;

        BuildPlayerOptions options = new BuildPlayerOptions
        {
            scenes = new[] { "Assets/Emgu.CV/Demo/HelloWorldScene.unity" },
            locationPathName = "Build/HelloWorldIOS",
            target = BuildTarget.iOS,
            options = BuildOptions.None
        };

        BuildReport report = BuildPipeline.BuildPlayer(options);
        if (report.summary.result != BuildResult.Succeeded)
        {
            EditorApplication.Exit(1);
        }
    }
}
