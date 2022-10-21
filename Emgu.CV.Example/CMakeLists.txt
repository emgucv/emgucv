# --------------------------------------------------------
#  Copyright (C) 2004-2022 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF(EMGU_CV_EXAMPLE_BUILD)
   
  IF (HAVE_ANDROID OR HAVE_IOS OR HAVE_MACOS OR HAVE_WINDESKTOP_X86 OR HAVE_WINDESKTOP_X64 OR HAVE_WINSTORE_10) 
    INSTALL(
      DIRECTORY
      "${CMAKE_CURRENT_SOURCE_DIR}/XamarinForms/Core/"
      DESTINATION Emgu.CV.Example/XamarinForms/Core/
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
	  PATTERN "*.jpg"
      PATTERN "*.dll"
      PATTERN "*.json"
      PATTERN ".git" EXCLUDE
      PATTERN "Debug" EXCLUDE
      PATTERN "Release" EXCLUDE  
      )
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
      PATTERN "*.Xamarin"
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
    INSTALL(
      FILES
      "${OPENCV_EXTRA_SUBFOLDER}/testdata/dnn/mask_rcnn_inception_v2_coco_2018_01_28.pbtxt"
      DESTINATION "opencv_extra/testdata/dnn/"
      COMPONENT emgucv_example_source
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
      "${CMAKE_CURRENT_SOURCE_DIR}/XamarinForms/WPF/"
      DESTINATION Emgu.CV.Example/XamarinForms/WPF/
      COMPONENT emgucv_example_source
      FILES_MATCHING 
      PATTERN "*.cs"
      PATTERN "Properties/*"
      PATTERN "*.csproj"
      PATTERN "*.appxmanifest"
      PATTERN "*.xaml"
      PATTERN "*.json"
      PATTERN "*.pfx"
	  PATTERN "*.config"
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
  ENDIF()
  IF(HAVE_ANDROID)
    INSTALL(
      DIRECTORY
      "${CMAKE_CURRENT_SOURCE_DIR}/XamarinForms/Android/"
      DESTINATION Emgu.CV.Example/XamarinForms/Android/
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
  
  IF(HAVE_IOS)
    INSTALL(
      DIRECTORY
      "${CMAKE_CURRENT_SOURCE_DIR}/XamarinForms/iOS/"
      DESTINATION Emgu.CV.Example/XamarinForms/iOS/
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
      PATTERN "*.plist"
      PATTERN "iTunesArtwork*"
	  PATTERN "*.storyboard"
      PATTERN ".git" EXCLUDE
      PATTERN "Debug" EXCLUDE
      PATTERN "Release" EXCLUDE  
      )
    
    INSTALL(
      DIRECTORY
      "${CMAKE_CURRENT_SOURCE_DIR}/iOS/"
      DESTINATION Emgu.CV.Example/iOS/
      COMPONENT emgucv_example_source
      FILES_MATCHING 
      PATTERN "*.cs"
      PATTERN "*.csproj"
      PATTERN "*.axml"
      PATTERN "*.xml"
      PATTERN "*.txt"
      PATTERN "*.png"
      PATTERN "*.dll"
      PATTERN "*.plist"
      PATTERN ".git" EXCLUDE
      PATTERN "bin" EXCLUDE
      PATTERN "obj" EXCLUDE
      )
  ENDIF()
  IF(HAVE_MACOS)
    INSTALL(
      DIRECTORY
      "${CMAKE_CURRENT_SOURCE_DIR}/Mac/"
      DESTINATION Emgu.CV.Example/Mac/
      COMPONENT emgucv_example_source
      FILES_MATCHING 
      PATTERN "*.cs"
      PATTERN "*.csproj"
      PATTERN "*.axml"
      PATTERN "*.xml"
      PATTERN "*.txt"
      PATTERN "*.png"
      PATTERN "*.dll"
      PATTERN "*.plist"
      PATTERN "*.json"
      PATTERN "*.storyboard"
      PATTERN ".git" EXCLUDE
      PATTERN "bin" EXCLUDE
      PATTERN "obj" EXCLUDE
      )

    INSTALL(
      DIRECTORY
      "${CMAKE_CURRENT_SOURCE_DIR}/XamarinForms/Mac/"
      DESTINATION Emgu.CV.Example/XamarinForms/Mac/
      COMPONENT emgucv_example_source
      FILES_MATCHING
      PATTERN "*.cs"
      PATTERN "*.csproj"
      PATTERN "*.config"
      PATTERN "*.Xamarin"
      PATTERN "*.axml"
      PATTERN "*.xaml"
      PATTERN "*.xml"
      PATTERN "*.txt"
      PATTERN "*.png"
      PATTERN "*.dll"
      PATTERN "*.json"
      PATTERN "*.plist"
      PATTERN "iTunesArtwork*"
      PATTERN "*.storyboard"
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
    INSTALL(
      DIRECTORY
      "${CMAKE_CURRENT_SOURCE_DIR}/XamarinForms/UWP/"
      DESTINATION Emgu.CV.Example/XamarinForms/UWP/
      COMPONENT emgucv_example_source
      FILES_MATCHING 
      PATTERN "*.cs"
      PATTERN "Properties/*"
      PATTERN "*.csproj"
      PATTERN "*.appxmanifest"
      PATTERN "*.xaml"
      PATTERN "*.json"
      PATTERN "*.pfx"
	  PATTERN "*.config"
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
 

  IF(HAVE_ANDROID)    
    INSTALL(
      FILES
      "${OPENCV_SUBFOLDER}/data/haarcascades/haarcascade_eye.xml"
      "${OPENCV_SUBFOLDER}/data/haarcascades/haarcascade_frontalface_default.xml"
      DESTINATION "opencv/data/haarcascades"
      COMPONENT emgucv_example_source
      )
  ENDIF()
  
  IF(HAVE_WINDESKTOP_X86 OR HAVE_WINDESKTOP_X64 OR HAVE_MACOS OR HAVE_LINUX_ARM OR HAVE_LINUX_X64 OR HAVE_LINUX_X86)
    add_subdirectory(HelloWorld)
	add_subdirectory(BuildInfo.NetCore.Console)
	add_subdirectory(DynamicImage.Asp.NetCore)
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
      PATTERN "bin" EXCLUDE
      PATTERN "obj" EXCLUDE
      )	
	INSTALL(
      DIRECTORY
      "${CMAKE_CURRENT_SOURCE_DIR}/BuildInfo.NetCore.Console/"
      DESTINATION Emgu.CV.Example/BuildInfo.NetCore.Console/
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
      PATTERN "bin" EXCLUDE
      PATTERN "obj" EXCLUDE
      )	
    INSTALL(
      DIRECTORY
      "${CMAKE_CURRENT_SOURCE_DIR}/DynamicImage.Asp.NetCore/"
      DESTINATION Emgu.CV.Example/DynamicImage.Asp.NetCore/
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
      PATTERN "bin" EXCLUDE
      PATTERN "obj" EXCLUDE
      )	
  ENDIF()
  

ENDIF()
