# --------------------------------------------------------
#  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF(EMGU_CV_EXAMPLE_BUILD)
  IF(NETFX_CORE)
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
    "SURFFeature/DrawMatches.cs"
	"SURFFeature/box.png"
	"SURFFeature/box_in_scene.png"
    DESTINATION "Emgu.CV.Example/SURFFeature"
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
    add_subdirectory(SURFFeature)
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