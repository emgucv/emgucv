# --------------------------------------------------------
#  Copyright (C) 2004-2024 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF(EMGU_CV_EXAMPLE_BUILD)
   
  IF (HAVE_ANDROID OR HAVE_IOS OR HAVE_MACOS OR HAVE_WINDESKTOP_X86 OR HAVE_WINDESKTOP_X64 OR HAVE_WINSTORE_10) 
    INSTALL(
      DIRECTORY
      "${CMAKE_CURRENT_SOURCE_DIR}/MAUI/MauiDemoApp/"
      DESTINATION Emgu.CV.Example/MAUI/MauiDemoApp/
      COMPONENT emgucv_example_source
      FILES_MATCHING 
      PATTERN "*.cs"
      PATTERN "*.csproj"
      PATTERN "*.config"
      PATTERN "*.shproj"
      PATTERN "*.projitems"
      PATTERN "*.axml"
      PATTERN "*.xaml"
      PATTERN "*.xml"
      PATTERN "*.txt"
      PATTERN "*.png"
	  PATTERN "*.jpg"
	  PATTERN "*.svg"
      PATTERN "*.dll"
      PATTERN "*.json"
	  PATTERN "*.manifest"
	  PATTERN "*.appxmanifest"
	  PATTERN "*.plist"
	  PATTERN "*.ttf"
      PATTERN ".git" EXCLUDE
      PATTERN "Debug" EXCLUDE
      PATTERN "Release" EXCLUDE  
      )	  
  ENDIF()
  
  #Windows only demos
  IF (HAVE_WINDESKTOP_X86 OR HAVE_WINDESKTOP_X64)
    add_subdirectory(CameraCapture)
    add_subdirectory(Webservice_Host)
    add_subdirectory(Webservice_Client)
    add_subdirectory(OCR)
    INSTALL(
      DIRECTORY
      "${CMAKE_CURRENT_SOURCE_DIR}/CameraCapture/"
      DESTINATION Emgu.CV.Example/CameraCapture/
      COMPONENT emgucv_example_source
      FILES_MATCHING 
      PATTERN "*.cs"
      PATTERN "*.csproj"
      PATTERN "*.config"
      PATTERN "*.shproj"
      PATTERN "*.Xamarin"
      PATTERN "*.projitems"
      PATTERN "*.axml"
      PATTERN "*.xaml"
      PATTERN "*.xml"
      PATTERN "*.txt"
      PATTERN "*.png"
      PATTERN "*.dll"
      PATTERN "*.json"
	  PATTERN "*.resx"
      PATTERN ".git" EXCLUDE
      PATTERN "Debug" EXCLUDE
      PATTERN "Release" EXCLUDE  
      )
    INSTALL(
      DIRECTORY
      "${CMAKE_CURRENT_SOURCE_DIR}/WPF.NetCore/"
      DESTINATION Emgu.CV.Example/WPF.NetCore/
      COMPONENT emgucv_example_source
      FILES_MATCHING 
      PATTERN "*.cs"
      PATTERN "*.csproj"
      PATTERN "*.config"
      PATTERN "*.shproj"
      PATTERN "*.Xamarin"
      PATTERN "*.projitems"
      PATTERN "*.axml"
      PATTERN "*.xaml"
      PATTERN "*.xml"
      PATTERN "*.txt"
      PATTERN "*.png"
      PATTERN "*.dll"
      PATTERN "*.json"
      PATTERN ".git" EXCLUDE
      PATTERN "Debug" EXCLUDE
      PATTERN "Release" EXCLUDE  
      )
	INSTALL(
      DIRECTORY
      "${CMAKE_CURRENT_SOURCE_DIR}/TF.Interop/"
      DESTINATION Emgu.CV.Example/TF.Interop/
      COMPONENT emgucv_example_source
      FILES_MATCHING 
      PATTERN "*.cs"
      PATTERN "*.csproj"
      PATTERN "*.config"
      PATTERN "*.shproj"
      PATTERN "*.Xamarin"
      PATTERN "*.projitems"
      PATTERN "*.axml"
      PATTERN "*.xaml"
      PATTERN "*.xml"
      PATTERN "*.txt"
      PATTERN "*.png"
      PATTERN "*.dll"
      PATTERN "*.json"
      PATTERN ".git" EXCLUDE
      PATTERN "Debug" EXCLUDE
      PATTERN "Release" EXCLUDE  
      )
	INSTALL(
      DIRECTORY
      "${CMAKE_CURRENT_SOURCE_DIR}/TF.Lite.Interop/"
      DESTINATION Emgu.CV.Example/TF.Lite.Interop/
      COMPONENT emgucv_example_source
      FILES_MATCHING 
      PATTERN "*.cs"
      PATTERN "*.csproj"
      PATTERN "*.config"
      PATTERN "*.shproj"
      PATTERN "*.Xamarin"
      PATTERN "*.projitems"
      PATTERN "*.axml"
      PATTERN "*.xaml"
      PATTERN "*.xml"
      PATTERN "*.txt"
      PATTERN "*.png"
      PATTERN "*.dll"
      PATTERN "*.json"
      PATTERN ".git" EXCLUDE
      PATTERN "Debug" EXCLUDE
      PATTERN "Release" EXCLUDE  
      )
    INSTALL(
      DIRECTORY
      "${CMAKE_CURRENT_SOURCE_DIR}/OCR/"
      DESTINATION Emgu.CV.Example/OCR/
      COMPONENT emgucv_example_source
      FILES_MATCHING 
      PATTERN "*.cs"
      PATTERN "*.csproj"
      PATTERN "*.config"
      PATTERN "*.shproj"
      PATTERN "*.Xamarin"
      PATTERN "*.projitems"
      PATTERN "*.axml"
      PATTERN "*.xaml"
      PATTERN "*.xml"
      PATTERN "*.txt"
      PATTERN "*.png"
      PATTERN "*.dll"
      PATTERN "*.json"
      PATTERN ".git" EXCLUDE
      PATTERN "Debug" EXCLUDE
      PATTERN "Release" EXCLUDE  
      )
	INSTALL(
      DIRECTORY
      "${CMAKE_CURRENT_SOURCE_DIR}/Stitching/"
      DESTINATION Emgu.CV.Example/Stitching/
      COMPONENT emgucv_example_source
      FILES_MATCHING 
      PATTERN "*.cs"
	  PATTERN "*.resx"
      PATTERN "*.csproj"
      PATTERN "*.config"
      PATTERN "*.shproj"
      PATTERN "*.Xamarin"
      PATTERN "*.projitems"
      PATTERN "*.axml"
      PATTERN "*.xaml"
      PATTERN "*.xml"
      PATTERN "*.txt"
      PATTERN "*.png"
      PATTERN "*.dll"
      PATTERN "*.json"
      PATTERN ".git" EXCLUDE
      PATTERN "Debug" EXCLUDE
      PATTERN "Release" EXCLUDE  
      )
	INSTALL(
      DIRECTORY
      "${CMAKE_CURRENT_SOURCE_DIR}/Aruco/"
      DESTINATION Emgu.CV.Example/Aruco/
      COMPONENT emgucv_example_source
      FILES_MATCHING 
      PATTERN "*.cs"
	  PATTERN "*.resx"
      PATTERN "*.csproj"
      PATTERN "*.config"
      PATTERN "*.shproj"
      PATTERN "*.projitems"
      PATTERN "*.axml"
      PATTERN "*.xaml"
      PATTERN "*.xml"
      PATTERN "*.txt"
      PATTERN "*.png"
      PATTERN "*.dll"
      PATTERN "*.json"
      PATTERN ".git" EXCLUDE
      PATTERN "Debug" EXCLUDE
      PATTERN "Release" EXCLUDE  
      )
	INSTALL(
      DIRECTORY
      "${CMAKE_CURRENT_SOURCE_DIR}/Webservice_Client/"
      DESTINATION Emgu.CV.Example/Webservice_Client/
      COMPONENT emgucv_example_source
      FILES_MATCHING 
      PATTERN "*.cs"
      PATTERN "*.csproj"
      PATTERN "*.config"
      PATTERN "*.shproj"
      PATTERN "*.Xamarin"
      PATTERN "*.projitems"
      PATTERN "*.axml"
      PATTERN "*.xaml"
      PATTERN "*.xml"
      PATTERN "*.txt"
      PATTERN "*.png"
      PATTERN "*.dll"
      PATTERN "*.json"
	  PATTERN "*.resx"
      PATTERN ".git" EXCLUDE
      PATTERN "Debug" EXCLUDE
      PATTERN "Release" EXCLUDE  
      )
	INSTALL(
      DIRECTORY
      "${CMAKE_CURRENT_SOURCE_DIR}/Webservice_Host/"
      DESTINATION Emgu.CV.Example/Webservice_Host/
      COMPONENT emgucv_example_source
      FILES_MATCHING 
      PATTERN "*.cs"
      PATTERN "*.csproj"
      PATTERN "*.config"
      PATTERN "*.shproj"
      PATTERN "*.Xamarin"
      PATTERN "*.projitems"
      PATTERN "*.axml"
      PATTERN "*.xaml"
      PATTERN "*.xml"
      PATTERN "*.txt"
      PATTERN "*.png"
      PATTERN "*.dll"
      PATTERN "*.json"
      PATTERN ".git" EXCLUDE
      PATTERN "Debug" EXCLUDE
      PATTERN "Release" EXCLUDE  
      )
	INSTALL(
      DIRECTORY
      "${CMAKE_CURRENT_SOURCE_DIR}/DepthAI/"
      DESTINATION Emgu.CV.Example/DepthAI/
      COMPONENT emgucv_example_source
      FILES_MATCHING 
      PATTERN "*.cs"
      PATTERN "*.csproj"
      PATTERN "*.config"
      PATTERN "*.shproj"
      PATTERN "*.Xamarin"
      PATTERN "*.projitems"
      PATTERN "*.axml"
      PATTERN "*.xaml"
      PATTERN "*.xml"
      PATTERN "*.txt"
      PATTERN "*.png"
      PATTERN "*.dll"
      PATTERN "*.json"
      PATTERN ".git" EXCLUDE
      PATTERN "Debug" EXCLUDE
      PATTERN "Release" EXCLUDE  
      )
	INSTALL(
      DIRECTORY
      "${CMAKE_CURRENT_SOURCE_DIR}/FaceDetectionVB/"
      DESTINATION Emgu.CV.Example/FaceDetectionVB/
      COMPONENT emgucv_example_source
      FILES_MATCHING 
      PATTERN "*.vb"
      PATTERN "*.config"
      PATTERN "*.vbproj"
      PATTERN "*.projitems"
      PATTERN "*.axml"
      PATTERN "*.xaml"
      PATTERN "*.xml"
      PATTERN "*.txt"
      PATTERN "*.jpg"
      PATTERN "*.dll"
      PATTERN "*.json"
	  PATTERN "*.resx"
      PATTERN ".git" EXCLUDE
      PATTERN "Debug" EXCLUDE
      PATTERN "Release" EXCLUDE  
      )
  ENDIF()
  IF(HAVE_ANDROID)
    INSTALL(
      DIRECTORY
      "${CMAKE_CURRENT_SOURCE_DIR}/Android/"
      DESTINATION Emgu.CV.Example/Android/
      COMPONENT emgucv_example_source
      FILES_MATCHING 
      PATTERN "*.cs"
      PATTERN "*.csproj"
      PATTERN "*.axml"
      PATTERN "*.xml"
      PATTERN "*.txt"
      PATTERN "*.png"
      PATTERN "*.dll"
	  PATTERN "*.config"
      PATTERN ".git" EXCLUDE
      PATTERN "Debug" EXCLUDE
      PATTERN "Release" EXCLUDE  
      )
  ENDIF()
  
  IF(HAVE_WINSTORE_10)
    INSTALL(
      DIRECTORY
      "${CMAKE_CURRENT_SOURCE_DIR}/HelloWorld.UWP/"
      DESTINATION Emgu.CV.Example/HelloWorld.UWP/
      COMPONENT emgucv_example_source
      FILES_MATCHING 
      PATTERN "*.cs"
      PATTERN "Properties/*"
      PATTERN "*.csproj"
      PATTERN "*.appxmanifest"
      PATTERN "*.xaml"
	  PATTERN "*.xml"
      PATTERN "*.json"
      PATTERN "*.pfx"
      PATTERN "Assets/*.png"  
      PATTERN ".git" EXCLUDE
      PATTERN "obj" EXCLUDE
      PATTERN "CMake*" EXCLUDE
      PATTERN "Release" EXCLUDE
      PATTERN "Debug" EXCLUDE
      PATTERN "RelWithDebInfo" EXCLUDE
      PATTERN "*.dir" EXCLUDE
      PATTERN "bin" EXCLUDE
      )
    INSTALL(
      DIRECTORY
      "${CMAKE_CURRENT_SOURCE_DIR}/RealtimeCamera.UWP/"
      DESTINATION Emgu.CV.Example/RealtimeCamera.UWP/
      COMPONENT emgucv_example_source
      FILES_MATCHING 
      PATTERN "*.cs"
      PATTERN "Properties/*"
      PATTERN "*.csproj"
      PATTERN "*.appxmanifest"
      PATTERN "*.xaml"
	  PATTERN "*.xml"
      PATTERN "*.json"
      PATTERN "*.pfx"
      PATTERN "Assets/*.png"  
      PATTERN ".git" EXCLUDE
      PATTERN "obj" EXCLUDE
      PATTERN "CMake*" EXCLUDE
      PATTERN "Release" EXCLUDE
      PATTERN "Debug" EXCLUDE
      PATTERN "RelWithDebInfo" EXCLUDE
      PATTERN "*.dir" EXCLUDE
      PATTERN "bin" EXCLUDE
      )

  ENDIF()
   
  IF(HAVE_WINDESKTOP_X86 OR HAVE_WINDESKTOP_X64 OR HAVE_MACOS OR HAVE_LINUX_ARM OR HAVE_LINUX_X64 OR HAVE_LINUX_X86)
    add_subdirectory(HelloWorld)
	add_subdirectory(BuildInfo.Console)
	add_subdirectory(DynamicImage.Asp.Net)
	INSTALL(
      DIRECTORY
      "${CMAKE_CURRENT_SOURCE_DIR}/HelloWorld/"
      DESTINATION Emgu.CV.Example/HelloWorld/
      COMPONENT emgucv_example_source
      FILES_MATCHING 
      PATTERN "*.cs"
      PATTERN "*.csproj"
      PATTERN "*.config"
      PATTERN "*.shproj"
      PATTERN "*.json"
      PATTERN "*.projitems"
      PATTERN "*.axml"
      PATTERN "*.xaml"
      PATTERN "*.xml"
      PATTERN "*.txt"
      PATTERN "*.png"
	  PATTERN "*.jpg"
      PATTERN "*.dll"
      PATTERN "*.json"
	  PATTERN "*.Build.props"
      PATTERN "bin" EXCLUDE
      PATTERN "obj" EXCLUDE
      )	
	INSTALL(
      DIRECTORY
      "${CMAKE_CURRENT_SOURCE_DIR}/BuildInfo.Console/"
      DESTINATION Emgu.CV.Example/BuildInfo.Console/
      COMPONENT emgucv_example_source
      FILES_MATCHING 
      PATTERN "*.cs"
      PATTERN "*.csproj"
      PATTERN "*.config"
      PATTERN "*.shproj"
      PATTERN "*.json"
      PATTERN "*.projitems"
      PATTERN "*.axml"
      PATTERN "*.xaml"
      PATTERN "*.xml"
      PATTERN "*.txt"
      PATTERN "*.png"
	  PATTERN "*.jpg"
      PATTERN "*.dll"
      PATTERN "*.json"
	  PATTERN "*.Build.props"
      PATTERN "bin" EXCLUDE
      PATTERN "obj" EXCLUDE
      )	
    INSTALL(
      DIRECTORY
      "${CMAKE_CURRENT_SOURCE_DIR}/DynamicImage.Asp.Net/"
      DESTINATION Emgu.CV.Example/DynamicImage.Asp.Net/
      COMPONENT emgucv_example_source
      FILES_MATCHING 
      PATTERN "*.cs"
      PATTERN "*.csproj"
      PATTERN "*.config"
      PATTERN "*.shproj"
      PATTERN "*.json"
      PATTERN "*.projitems"
      PATTERN "*.axml"
      PATTERN "*.xaml"
      PATTERN "*.xml"
      PATTERN "*.txt"
      PATTERN "*.png"
	  PATTERN "*.jpg"
      PATTERN "*.dll"
      PATTERN "*.json"
	  PATTERN "*.Build.props"
      PATTERN "bin" EXCLUDE
      PATTERN "obj" EXCLUDE
      )	
  ENDIF()
  

ENDIF()
