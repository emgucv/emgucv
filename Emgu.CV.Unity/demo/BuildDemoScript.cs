#if UNITY_EDITOR
﻿using UnityEngine;
using UnityEditor;
using System.Collections;

public class BuildDemoScript {

	static void PerformOSXBuild () {
		EditorUserBuildSettings.SwitchActiveBuildTarget (BuildTargetGroup.Standalone, BuildTarget.StandaloneOSX);
		string[] scenes = {"Assets/Emgu.CV/Emgu.CV.Demo/FaceDetectionScene.unity"};
		BuildPipeline.BuildPlayer (scenes, "Builds/OSX/osx.app", BuildTarget.StandaloneOSX, BuildOptions.None);
	}

	static void PerformIOSBuild () {
		EditorUserBuildSettings.SwitchActiveBuildTarget (BuildTargetGroup.iOS, BuildTarget.iOS);
		string[] scenes = {"Assets/Emgu.CV/Emgu.CV.Demo/FaceDetectionScene.unity"};
		BuildPipeline.BuildPlayer (scenes, "Builds/iOS", BuildTarget.iOS, BuildOptions.None);
	}

	static void PerformWinBuild () {
		EditorUserBuildSettings.SwitchActiveBuildTarget (BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows);
		string[] scenes = {"Assets/Emgu.CV/Emgu.CV.Demo/FaceDetectionScene.unity"};
		BuildPipeline.BuildPlayer (scenes, "Builds/Win/win.exe", BuildTarget.StandaloneWindows, BuildOptions.None);
	}

	static void PerformWin64Build () {
		EditorUserBuildSettings.SwitchActiveBuildTarget (BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows64);
		string[] scenes = {"Assets/Emgu.CV/Emgu.CV.Demo/FaceDetectionScene.unity"};
		BuildPipeline.BuildPlayer (scenes, "Builds/Win64/win64.exe", BuildTarget.StandaloneWindows64, BuildOptions.None);
	}

	static void PerformAndroidBuild () {
		EditorUserBuildSettings.SwitchActiveBuildTarget (BuildTargetGroup.Android, BuildTarget.Android);
		string[] scenes = {"Assets/Emgu.CV/Emgu.CV.Demo/FaceDetectionScene.unity"};
		BuildPipeline.BuildPlayer (scenes, "Builds/Android", BuildTarget.Android, BuildOptions.None);
	}


	static void PerformUniversalSDK81Build () {
		EditorUserBuildSettings.SwitchActiveBuildTarget (BuildTargetGroup.WSA, BuildTarget.WSAPlayer);
		//UnityEditor.EditorUserBuildSettings.wsaSDK = WSASDK.UniversalSDK81;
		string[] scenes = {"Assets/Emgu.CV/Emgu.CV.Demo/FaceDetectionScene.unity"};
		BuildPipeline.BuildPlayer (scenes, "Builds/UniversalSDK81", BuildTarget.WSAPlayer, BuildOptions.None);
	}

	static void PerformUWPBuild () {
		EditorUserBuildSettings.SwitchActiveBuildTarget (BuildTargetGroup.WSA, BuildTarget.WSAPlayer);
		//UnityEditor.EditorUserBuildSettings.wsaSDK = WSASDK.UWP;
		string[] scenes = {"Assets/Emgu.CV/Emgu.CV.Demo/FaceDetectionScene.unity"};
		BuildPipeline.BuildPlayer (scenes, "Builds/UWP", BuildTarget.WSAPlayer, BuildOptions.None);
	}
} 
#endif