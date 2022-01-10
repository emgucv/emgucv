# --------------------------------------------------------
#  Copyright (C) 2004-2022 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF(TARGET Emgu.CV.Platform.NetFramework)
  PROJECT(Example.CameraCapture)
  MSBUILD_CSPROJ_IN_SOLUTION(${PROJECT_NAME} "${CMAKE_CURRENT_SOURCE_DIR}/../../Solution/Windows.Desktop/Emgu.CV.Example.sln" "CameraCapture" "" ALL)
  set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "Emgu.CV.Examples")
  ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.Platform.NetFramework)
ENDIF()

