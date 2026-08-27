using UnityEditor;
using UnityEditor.Build.Reporting;

public static class HelloWorldMacOSBuild
{
    public static void Build()
    {
        BuildPlayerOptions options = new BuildPlayerOptions
        {
            scenes = new[] { "Assets/Emgu.CV/Demo/HelloWorldScene.unity" },
            locationPathName = "Build/HelloWorldMacOS/Emgu.CV.UnityDemo.app",
            target = BuildTarget.StandaloneOSX,
            options = BuildOptions.None
        };

        BuildReport report = BuildPipeline.BuildPlayer(options);
        if (report.summary.result != BuildResult.Succeeded)
        {
            EditorApplication.Exit(1);
        }
    }
}
