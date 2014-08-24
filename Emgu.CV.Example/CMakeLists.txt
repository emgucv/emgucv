# --------------------------------------------------------
#  Copyright (C) 2004-2014 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF(EMGU_CV_EXAMPLE_BUILD AND (NOT ANDROID))
  IF(NETFX_CORE)
  ELSE()
    SET(EMGU_CV_REFERENCE 
    -r:System.Xml.dll 
    -r:System.Drawing.dll 
    -r:System.Data.dll 
    -r:System.Windows.Forms.dll
    -r:"${LIBRARY_OUTPUT_PATH}/Emgu.CV.dll" 
    -r:"${LIBRARY_OUTPUT_PATH}/Emgu.Util.dll" 
    -r:"${LIBRARY_OUTPUT_PATH}/Emgu.CV.UI.dll")
  
    SET (EMGU_CV_CUDA_REFERENCE -r:"${LIBRARY_OUTPUT_PATH}/Emgu.CV.Cuda.dll")
    SET (EMGU_CV_CONTRIB_REFERENCE -r:"${LIBRARY_OUTPUT_PATH}/Emgu.CV.Contrib.dll")
	
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