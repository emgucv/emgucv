# --------------------------------------------------------
#  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF(EMGU_CV_EXAMPLE_BUILD)
  #XamarinForms.Core 
  IF (HAVE_ANDROID OR HAVE_IOS OR HAVE_OSX OR HAVE_WINSTORE_81_X86 OR HAVE_WINSTORE_81_X64 OR HAVE_WINPHONE_81_X86 OR HAVE_WINPHONE_81_ARM OR HAVE_WINSTORE_10_X86 OR HAVE_WINSTORE_10_X64 OR HAVE_WINSTORE_10_ARM) 
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
  IF(HAVE_OSX)
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
  
  IF(HAVE_WINSTORE_10_X86 OR HAVE_WINSTORE_10_X64 OR HAVE_WINSTORE_10_ARM)
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
  
  IF(HAVE_WINSTORE_81_X86 OR HAVE_WINSTORE_81_X64 OR HAVE_WINPHONE_81_X86 OR HAVE_WINPHONE_81_ARM)
    INSTALL(
      DIRECTORY
      "${CMAKE_CURRENT_SOURCE_DIR}/XamarinForms/WindowsUniversal81/Shared/"
      DESTINATION Emgu.CV.Example/XamarinForms/WindowsUniversal81/Shared/
      COMPONENT emgucv_example_source
      FILES_MATCHING 
      PATTERN "*.cs"
      PATTERN "Properties/*"
      PATTERN "*.projitems"
      PATTERN "*.shproj"
      PATTERN "*.xaml"
      PATTERN "*.json"
      PATTERN "*.pfx"  
      PATTERN ".git" EXCLUDE
      PATTERN "obj" EXCLUDE
      PATTERN "CMake*" EXCLUDE
      PATTERN "Release" EXCLUDE
      PATTERN "Debug" EXCLUDE
      PATTERN "RelWithDebInfo" EXCLUDE
      PATTERN "*.dir" EXCLUDE
      PATTERN "bin" EXCLUDE
      )
    
    IF(HAVE_WINSTORE_81_X86 OR HAVE_WINSTORE_81_X64)
      # ----------------------------------------------------------------------------
      #  The example files
      # ----------------------------------------------------------------------------
      INSTALL(
	DIRECTORY
	"${CMAKE_CURRENT_SOURCE_DIR}/XamarinForms/WindowsUniversal81/Windows/"
	DESTINATION Emgu.CV.Example/XamarinForms/WindowsUniversal81/Windows/
	COMPONENT emgucv_example_source
	FILES_MATCHING 
	PATTERN "*.cs"
	PATTERN "*.csproj"
	PATTERN "*.config"
	PATTERN "*.shproj"
	PATTERN "*.Xamarin"
	PATTERN "*.appxmanifest"
	PATTERN "*.projitems"
	PATTERN "*.axml"
	PATTERN "*.xaml"
	PATTERN "*.xml"
	PATTERN "*.txt"
	PATTERN "*.png"
	PATTERN "*.dll"
	PATTERN "*.json"
	PATTERN ".git" EXCLUDE
	PATTERN "obj" EXCLUDE
	PATTERN "CMake*" EXCLUDE
	PATTERN "Release" EXCLUDE
	PATTERN "Debug" EXCLUDE
	PATTERN "RelWithDebInfo" EXCLUDE
	PATTERN "*.dir" EXCLUDE
	)
    ENDIF()
    IF(HAVE_WINPHONE_81_X86 OR HAVE_WINPHONE_81_ARM)
      # ----------------------------------------------------------------------------
      #  The example files
      # ----------------------------------------------------------------------------
      INSTALL(
	DIRECTORY
	"${CMAKE_CURRENT_SOURCE_DIR}/XamarinForms/WindowsUniversal81/WindowsPhone/"
	DESTINATION Emgu.CV.Example/XamarinForms/WindowsUniversal81/WindowsPhone/
	COMPONENT emgucv_example_source
	FILES_MATCHING 
	PATTERN "*.cs"
	PATTERN "*.csproj"
	PATTERN "*.config"
	PATTERN "*.shproj"
	PATTERN "*.Xamarin"
	PATTERN "*.appxmanifest"
	PATTERN "*.projitems"
	PATTERN "*.axml"
	PATTERN "*.xaml"
	PATTERN "*.xml"
	PATTERN "*.txt"
	PATTERN "*.png"
	PATTERN "*.dll"
	PATTERN "*.json"
	PATTERN ".git" EXCLUDE
	PATTERN "obj" EXCLUDE
	PATTERN "CMake*" EXCLUDE
	PATTERN "Release" EXCLUDE
	PATTERN "Debug" EXCLUDE
	PATTERN "RelWithDebInfo" EXCLUDE
	PATTERN "*.dir" EXCLUDE
	)
    ENDIF()
  ENDIF()
  
  IF (HAVE_ANDROID OR HAVE_WINSTORE_81_X86 OR HAVE_WINSTORE_81_X64 OR HAVE_WINPHONE_81_X86 OR HAVE_WINPHONE_81_ARM OR HAVE_WINSTORE_10_X86 OR HAVE_WINSTORE_10_X64 OR HAVE_WINSTORE_10_ARM) 
    INSTALL(
      FILES
      "PlanarSubdivision/DrawSubdivision.cs"
      DESTINATION "Emgu.CV.Example/PlanarSubdivision"
      COMPONENT emgucv_example_source
      )
    INSTALL(
      DIRECTORY
      "${CMAKE_CURRENT_SOURCE_DIR}/FaceDetection/"
      DESTINATION Emgu.CV.Example/FaceDetection/
      COMPONENT emgucv_example_source
      FILES_MATCHING 
      PATTERN "DetectFace.cs" 
      PATTERN "lena.jpg"  
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
      "${CMAKE_CURRENT_SOURCE_DIR}/FeatureMatching/"
      DESTINATION Emgu.CV.Example/FeatureMatching/
      COMPONENT emgucv_example_source
      FILES_MATCHING 
      PATTERN "DrawMatches.cs" 
      PATTERN "box.png"  
      PATTERN "box_in_scene.png"  
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
      "${CMAKE_CURRENT_SOURCE_DIR}/PedestrianDetection/"
      DESTINATION Emgu.CV.Example/PedestrianDetection/
      COMPONENT emgucv_example_source
      FILES_MATCHING 
      PATTERN "FindPedestrian.cs" 
      PATTERN "pedestrian.png" 
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
    INSTALL(
      FILES
      "LicensePlateRecognition/LicensePlateDetector.cs"
      "LicensePlateRecognition/license-plate.jpg"
      DESTINATION "Emgu.CV.Example/LicensePlateRecognition"
      COMPONENT emgucv_example_source
      )
    INSTALL(
      FILES
      "TrafficSignRecognition/StopSignDetector.cs"
      "TrafficSignRecognition/stop-sign-model.png"
      "TrafficSignRecognition/stop-sign.jpg"
      DESTINATION "Emgu.CV.Example/TrafficSignRecognition"
      COMPONENT emgucv_example_source
      )
  ENDIF()

  IF(NOT (NETFX_CORE OR ANDROID OR (APPLE AND (NOT IOS))))
    SET(EMGU_CV_REFERENCE 
      -r:System.Xml.dll 
      -r:System.Drawing.dll 
      -r:System.Data.dll 
      -r:System.Windows.Forms.dll
	  -r:System.ServiceModel.dll
      -r:"${LIBRARY_OUTPUT_PATH}/Emgu.CV.World.dll" 
      -r:"${LIBRARY_OUTPUT_PATH}/Emgu.CV.UI.dll")
    
    add_subdirectory(HelloWorld)
    add_subdirectory(CameraCapture)
    add_subdirectory(PlanarSubdivision)
    add_subdirectory(ShapeDetection)
    add_subdirectory(FeatureMatching)
    add_subdirectory(TrafficSignRecognition)
    add_subdirectory(MotionDetection)
    add_subdirectory(VideoSurveilance)
    add_subdirectory(FaceDetection)
    add_subdirectory(Webservice_Host)
    add_subdirectory(Webservice_Client)
    add_subdirectory(PedestrianDetection)
    add_subdirectory(LicensePlateRecognition)
    add_subdirectory(OCR)
    add_subdirectory(Aruco)
    #add_subdirectory(LatentSvmDetector)
    add_subdirectory(Simple3DReconstruction)
    IF(WIN32)
      add_subdirectory(WebDynamicImage)
    ENDIF()
  ENDIF()
ENDIF()
