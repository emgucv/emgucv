# --------------------------------------------------------
#  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF(WIN32)
  PROJECT(Example.Webservice_Client)

  BUILD_CSPROJ_IN_SOLUTION(${PROJECT_NAME} "${CMAKE_CURRENT_SOURCE_DIR}/../../Solution/Windows.Desktop/Emgu.CV.Example.sln" "Webservice_Client" "" ALL)
  set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "Emgu.CV.Examples")
  ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.Platform.NetFramework)
ENDIF()