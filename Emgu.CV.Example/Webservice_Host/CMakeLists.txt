# --------------------------------------------------------
#  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF(TARGET Emgu.CV.Platform.NetFramework)
  PROJECT(Example.Webservice_Host)
  MSBUILD_CSPROJ_IN_SOLUTION(${PROJECT_NAME} "${CMAKE_CURRENT_SOURCE_DIR}/../../Solution/Windows.Desktop/Emgu.CV.Example.sln" "Webservice_Host" "" ALL)
  SET_TARGET_PROPERTIES(${PROJECT_NAME} PROPERTIES FOLDER "Emgu.CV.Examples")
  ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.Platform.NetFramework)
ENDIF()
