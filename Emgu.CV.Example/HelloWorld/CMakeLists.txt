# --------------------------------------------------------
#  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF (DOTNET_FOUND)
  PROJECT(Example.HelloWorld)
  BUILD_DOTNET_PROJ(${PROJECT_NAME} "${CMAKE_CURRENT_SOURCE_DIR}/HelloWorld.NetCore.csproj"  "" ALL)
  set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "Emgu.CV.Examples")
  ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.Platform.NetStandard)
ENDIF()