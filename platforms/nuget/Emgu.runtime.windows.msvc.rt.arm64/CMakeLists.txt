# --------------------------------------------------------
#  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF(WIN32 AND HAVE_WINDESKTOP_ARM64)
  PROJECT(Emgu.runtime.windows.msvc.rt.arm64.nuget)

  BUILD_WINDOWS_RUNTIME_NUGET_PACKAGE(EMGU_WINDOWS_MSVCRT_ARM64_NUGET_ID EMGU_WINDOWS_MSVCRT_NUGET_VERSION)

  ADD_DEPENDENCIES(${PROJECT_NAME} cvextern)

  IF(TARGET Emgu.CV)
    ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV)
  ENDIF()
ENDIF()
