# --------------------------------------------------------
#  Copyright (C) 2004-2024 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF (DOTNET_FOUND AND NOT(APPLE AND ("${CMAKE_HOST_SYSTEM_PROCESSOR}" STREQUAL "arm64")) ) #DO not compile demo on Apple silicon
  PROJECT(Example.BuildInfo.NetCore.Console)
  BUILD_DOTNET_PROJ(${PROJECT_NAME} "${CMAKE_CURRENT_SOURCE_DIR}/BuildInfo.NetCore.Console.csproj"  "" ALL)
  set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "Emgu.CV.Examples")
  
  IF (TARGET Emgu.CV)
    ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV)
  ENDIF()
ENDIF()
