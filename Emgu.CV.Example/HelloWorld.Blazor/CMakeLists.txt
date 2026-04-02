# --------------------------------------------------------
#  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF(HAVE_EMSCRIPTEN)

  IF (DOTNET_FOUND)
    PROJECT(Example.HelloWorld.Blazor)

    BUILD_DOTNET_PROJ(${PROJECT_NAME} "${CMAKE_CURRENT_SOURCE_DIR}/HelloWorld.Blazor.csproj" "" ALL)

    set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "Emgu.CV.Examples")

    IF (TARGET cvextern)
      ADD_DEPENDENCIES(${PROJECT_NAME} cvextern)
    ENDIF()
  ENDIF()

ENDIF()
