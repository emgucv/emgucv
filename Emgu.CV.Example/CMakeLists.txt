# --------------------------------------------------------
#  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF(EMGU_CV_EXAMPLE_BUILD)
  IF(NETFX_CORE)
  
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
		"${CMAKE_CURRENT_SOURCE_DIR}/PlanarSubdivision/"
		DESTINATION Emgu.CV.Example/PlanarSubdivision/
		COMPONENT emgucv_example_source
		FILES_MATCHING 
		PATTERN "DrawSubdivision.cs" 
		PATTERN "PedestrianDetection/FindPedestrian.cs" 
		PATTERN "PedestrianDetection/pedestrian.png" 
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
	  IF("${CMAKE_SYSTEM_NAME}" STREQUAL "WindowsPhone")
	    # ----------------------------------------------------------------------------
		#  The example files
		# ----------------------------------------------------------------------------

		INSTALL(
		DIRECTORY
		"${CMAKE_CURRENT_SOURCE_DIR}/WindowsPhoneApp/"
		DESTINATION Emgu.CV.Example/WindowsPhoneApp/
		COMPONENT emgucv_example_source
		FILES_MATCHING 
		PATTERN "*.cs"
		PATTERN "Common/*.xaml"
		PATTERN "Common/*.txt"
		PATTERN "Common/*.cs"
		PATTERN "DataModel/*.json"
		PATTERN "DataModel/*.cs"
		PATTERN "Strings/en-US/*.resw"
		PATTERN "*.csproj"
		PATTERN "*.appxmanifest"
		PATTERN "*.xaml"
		PATTERN "*.xml"
		PATTERN "packages.config"
		PATTERN "Assets/*.png"  
		PATTERN ".git" EXCLUDE
		PATTERN "obj" EXCLUDE
		PATTERN "CMake*" EXCLUDE
		PATTERN "Release" EXCLUDE
		PATTERN "Debug" EXCLUDE
		PATTERN "RelWithDebInfo" EXCLUDE
		PATTERN "*.dir" EXCLUDE
		PATTERN "bin" EXCLUDE
		PATTERN "Android" EXCLUDE
		PATTERN "iOS" EXCLUDE
		PATTERN "Windows.Store" EXCLUDE
		PATTERN "VS2013-2015" EXCLUDE
		PATTERN "CrossPlatform" EXCLUDE
		)
		INSTALL(
		DIRECTORY
		${CMAKE_CURRENT_SOURCE_DIR}/XamarinForms/
		DESTINATION Emgu.CV.Example/XamarinForms
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
		PATTERN ".git" EXCLUDE
		PATTERN "obj" EXCLUDE
		PATTERN "CMake*" EXCLUDE
		PATTERN "Release" EXCLUDE
		PATTERN "Debug" EXCLUDE
		PATTERN "RelWithDebInfo" EXCLUDE
		PATTERN "*.dir" EXCLUDE
		PATTERN "XamarinForms.Droid" EXCLUDE
		#PATTERN "WindowsUniversal/*" EXCLUDE
		PATTERN "WindowsUniversal81/Windows" EXCLUDE
		PATTERN "XamarinForms.iOS" EXCLUDE
		)
	  ELSE() #windows store
		# ----------------------------------------------------------------------------
		#  The example files
		# ----------------------------------------------------------------------------
		INSTALL(
		DIRECTORY
		${CMAKE_CURRENT_SOURCE_DIR}/WindowsStoreCameraApp/
		DESTINATION Emgu.CV.Example/WindowsStoreCameraApp
		COMPONENT emgucv_example_source
		FILES_MATCHING 
		PATTERN "*.cs"
		PATTERN "Properties/*.cs"
		PATTERN "Common/*.xaml"
		PATTERN "*.csproj"
		PATTERN "*.appxmanifest"
		PATTERN "*.xaml"
		PATTERN "packages.config"
		PATTERN "Assets/*.png"  
		PATTERN ".git" EXCLUDE
		PATTERN "obj" EXCLUDE
		PATTERN "CMake*" EXCLUDE
		PATTERN "Release" EXCLUDE
		PATTERN "Debug" EXCLUDE
		PATTERN "RelWithDebInfo" EXCLUDE
		PATTERN "*.dir" EXCLUDE
		)
		INSTALL(
		DIRECTORY
		${CMAKE_CURRENT_SOURCE_DIR}/XamarinForms/
		DESTINATION Emgu.CV.Example/XamarinForms
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
		PATTERN ".git" EXCLUDE
		PATTERN "obj" EXCLUDE
		PATTERN "CMake*" EXCLUDE
		PATTERN "Release" EXCLUDE
		PATTERN "Debug" EXCLUDE
		PATTERN "RelWithDebInfo" EXCLUDE
		PATTERN "*.dir" EXCLUDE
		PATTERN "XamarinForms.Droid" EXCLUDE
		PATTERN "WindowsPhone" EXCLUDE
		PATTERN "XamarinForms.iOS" EXCLUDE
		)
	  ENDIF()
  ELSEIF(ANDROID)
    INSTALL(
  DIRECTORY
  "${CMAKE_CURRENT_SOURCE_DIR}/Android"
  DESTINATION Emgu.CV.Example
  COMPONENT main
  FILES_MATCHING 
  PATTERN "*.cs"
  PATTERN "*.csproj"
  PATTERN "*.axml"
  PATTERN "*.xml"
  PATTERN "*.txt"
  PATTERN "*.png"
  PATTERN "*.dll"
  PATTERN ".git" EXCLUDE
  )
  INSTALL(
  DIRECTORY
  "${CMAKE_CURRENT_SOURCE_DIR}/XamarinForms"
  DESTINATION Emgu.CV.Example
  COMPONENT main
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
  PATTERN ".git" EXCLUDE
  PATTERN "Debug" EXCLUDE
  PATTERN "Release" EXCLUDE
  PATTERN "WindowsUniversal" EXCLUDE
  PATTERN "XamarinForms.iOS" EXCLUDE
  )
  INSTALL(
    FILES
    "PlanarSubdivision/DrawSubdivision.cs"
    DESTINATION "Emgu.CV.Example/PlanarSubdivision"
    COMPONENT main
    )
  INSTALL(
    FILES
    "FaceDetection/DetectFace.cs"
	"FaceDetection/lena.jpg"
    DESTINATION "Emgu.CV.Example/FaceDetection"
    COMPONENT main
    )
  INSTALL(
    FILES
    "${OPENCV_SUBFOLDER}/data/haarcascades/haarcascade_eye.xml"
	"${OPENCV_SUBFOLDER}/data/haarcascades/haarcascade_frontalface_default.xml"
    DESTINATION "opencv/data/haarcascades"
    COMPONENT main
    )
  INSTALL(
    FILES
    "FeatureMatching/DrawMatches.cs"
	"FeatureMatching/box.png"
	"FeatureMatching/box_in_scene.png"
    DESTINATION "Emgu.CV.Example/FeatureMatching"
    COMPONENT main
    )
  INSTALL(
    FILES
    "PedestrianDetection/FindPedestrian.cs"
	"PedestrianDetection/pedestrian.png"
    DESTINATION "Emgu.CV.Example/PedestrianDetection"
    COMPONENT main
    )
  INSTALL(
    FILES
    "LicensePlateRecognition/LicensePlateDetector.cs"
	"LicensePlateRecognition/license-plate.jpg"
    DESTINATION "Emgu.CV.Example/LicensePlateRecognition"
    COMPONENT main
    )
  INSTALL(
    FILES
    "TrafficSignRecognition/StopSignDetector.cs"
	"TrafficSignRecognition/stop-sign-model.png"
	"TrafficSignRecognition/stop-sign.jpg"
    DESTINATION "Emgu.CV.Example/TrafficSignRecognition"
    COMPONENT main
    )
  ELSE()
    SET(EMGU_CV_REFERENCE 
    -r:System.Xml.dll 
    -r:System.Drawing.dll 
    -r:System.Data.dll 
    -r:System.Windows.Forms.dll
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
    #add_subdirectory(LatentSvmDetector)
    IF(WIN32)
      add_subdirectory(WebDynamicImage)
      add_subdirectory(Simple3DReconstruction)
    ENDIF()
  ENDIF()
ENDIF()