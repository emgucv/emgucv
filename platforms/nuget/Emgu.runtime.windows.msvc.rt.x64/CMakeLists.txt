# --------------------------------------------------------
#  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF(WIN32 AND HAVE_WINDESKTOP_X64)
  PROJECT(Emgu.runtime.windows.msvc.rt.x64.nuget)

  BUILD_WINDOWS_RUNTIME_NUGET_PACKAGE(EMGU_WINDOWS_MSVCRT_X64_NUGET_ID EMGU_WINDOWS_MSVCRT_NUGET_VERSION)

  ADD_DEPENDENCIES(${PROJECT_NAME} cvextern)

  IF(TARGET Emgu.CV)
    ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV)
  ENDIF()
ENDIF()
