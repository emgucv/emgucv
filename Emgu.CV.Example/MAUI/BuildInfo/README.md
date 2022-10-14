This is a basic MAUI project based on the dotnet MAUI template. 

When you click the button on the demo project, it should display the Emgu CV build information.

You will need setup a local nuget repository and put the following nuget packages in the repo:

1. Emgu.CV.runtime.maui.android
2. Emgu.CV.runtime.maui.ios
3. Emgu.CV.runtime.maui.macos
4. Emgu.CV.runtime.windows
5. Emgu.CV

To make sure you have dotnet MAUI workload installed, open the terminal and run this 
>> dotnet workload install maui

To restore the nuget package, in the terminal run this command:
>>> dotnet restore
 
To build the Mac OS MAUI App and run it, use this command:
>>> dotnet build -t:Run -f net6.0-maccatalyst

To build the iOS MAUI app and run it on a simulator, use this command:
>>> dotnet build -t:Run -f net6.0-ios

To build the iOS MAUI .ipa file for iOS device, use this command:
>>> dotnet publish -f net6.0-ios -c Release -r ios-arm64 --self-contained

To build the iOS MAUI app and run it on an iOS device, use this command:
>>> dotnet build -t:Run -f net6.0-ios -c Release -r ios-arm64 --self-contained -p:_DeviceName={YOUR_IOS_DEVICE_UUID}

For Windows and Android MAUI build, you can use Visual Studio on windows.