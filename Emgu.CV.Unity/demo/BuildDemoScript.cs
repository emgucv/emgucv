#if UNITY_EDITOR
﻿using UnityEngine;
using UnityEditor;
using System.Collections;

public class BuildDemoScript {

	static void PerformOSXBuild () {
		EditorUserBuildSettings.SwitchActiveBuildTarget (BuildTarget.StandaloneOSXUniversal);
		string[] scenes = {"Assets/Emgu.CV/Emgu.CV.Demo/FaceDetectionScene.unity"};
		BuildPipeline.BuildPlayer (scenes, "Builds/OSX/osx.app", BuildTarget.StandaloneOSXUniversal, BuildOptions.None);
	}

	static void PerformIOSBuild () {
		EditorUserBuildSettings.SwitchActiveBuildTarget (BuildTarget.iOS);
		string[] scenes = {"Assets/Emgu.CV/Emgu.CV.Demo/FaceDetectionScene.unity"};
		BuildPipeline.BuildPlayer (scenes, "Builds/iOS", BuildTarget.iOS, BuildOptions.None);
	}

	static void PerformWinBuild () {
		EditorUserBuildSettings.SwitchActiveBuildTarget (BuildTarget.StandaloneWindows);
		string[] scenes = {"Assets/Emgu.CV/Emgu.CV.Demo/FaceDetectionScene.unity"};
		BuildPipeline.BuildPlayer (scenes, "Builds/Win/win.exe", BuildTarget.StandaloneWindows, BuildOptions.None);
	}

	static void PerformWin64Build () {
		EditorUserBuildSettings.SwitchActiveBuildTarget (BuildTarget.StandaloneWindows64);
		string[] scenes = {"Assets/Emgu.CV/Emgu.CV.Demo/FaceDetectionScene.unity"};
		BuildPipeline.BuildPlayer (scenes, "Builds/Win64/win64.exe", BuildTarget.StandaloneWindows64, BuildOptions.None);
	}

	static void PerformAndroidBuild () {
		EditorUserBuildSettings.SwitchActiveBuildTarget (BuildTarget.Android);
		string[] scenes = {"Assets/Emgu.CV/Emgu.CV.Demo/FaceDetectionScene.unity"};
		BuildPipeline.BuildPlayer (scenes, "Builds/Android", BuildTarget.Android, BuildOptions.None);
	}

	static void PerformUniversalSDK81Build () {
		EditorUserBuildSettings.SwitchActiveBuildTarget (BuildTarget.WSAPlayer);
		UnityEditor.EditorUserBuildSettings.wsaSDK = WSASDK.UniversalSDK81;
		string[] scenes = {"Assets/Emgu.CV/Emgu.CV.Demo/FaceDetectionScene.unity"};
		BuildPipeline.BuildPlayer (scenes, "Builds/UniversalSDK81", BuildTarget.WSAPlayer, BuildOptions.None);
	}

	static void PerformUWPBuild () {
		EditorUserBuildSettings.SwitchActiveBuildTarget (BuildTarget.WSAPlayer);
		UnityEditor.EditorUserBuildSettings.wsaSDK = WSASDK.UWP;
		string[] scenes = {"Assets/Emgu.CV/Emgu.CV.Demo/FaceDetectionScene.unity"};
		BuildPipeline.BuildPlayer (scenes, "Builds/UWP", BuildTarget.WSAPlayer, BuildOptions.None);
	}
} 
#endif