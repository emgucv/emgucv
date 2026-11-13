# --------------------------------------------------------
#  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

CONFIGURE_FILE(${CMAKE_CURRENT_SOURCE_DIR}/Directory.Build.props.in ${CMAKE_CURRENT_SOURCE_DIR}/Directory.Build.props)

# Build the demo only in the WebAssembly build itself. HAVE_EMSCRIPTEN is also set by any other
# build that finds libs/webgl/cvextern.a, where the demo's pinned Emgu.CV.runtime.webassembly
# version usually doesn't exist yet and the restore fails (NU1102).
IF(IS_EMSCRIPTEN_BUILD)

  IF(DOTNET_FOUND)
    PROJECT(Example.HelloWorld.Blazor)

    BUILD_DOTNET_PROJ(${PROJECT_NAME} "${CMAKE_CURRENT_SOURCE_DIR}/HelloWorld.Blazor.csproj" "" ALL)

    SET_TARGET_PROPERTIES(${PROJECT_NAME} PROPERTIES FOLDER "Emgu.CV.Examples")

    IF(TARGET cvextern)
      ADD_DEPENDENCIES(${PROJECT_NAME} cvextern)
    ENDIF()
  ENDIF()

ENDIF()
