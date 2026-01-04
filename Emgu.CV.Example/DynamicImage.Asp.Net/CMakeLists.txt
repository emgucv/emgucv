# --------------------------------------------------------
#  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF (DOTNET_FOUND)
  PROJECT(Example.DynamicImage.Asp.Net)
  BUILD_DOTNET_PROJ(${PROJECT_NAME} "${CMAKE_CURRENT_SOURCE_DIR}/DynamicImage.Asp.Net.csproj" "" ALL)
  set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "Emgu.CV.Examples")
  IF (TARGET Emgu.CV)
	ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV)
  ENDIF()
ENDIF()
