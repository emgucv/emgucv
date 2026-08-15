# --------------------------------------------------------
#  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF(WIN32 AND HAVE_WINDESKTOP_X64 AND WITH_CUDNN)
  PROJECT(Emgu.runtime.windows.zlibwapi.nuget)

  BUILD_WINDOWS_RUNTIME_NUGET_PACKAGE(EMGU_ZLIBWAPI_ID EMGU_ZLIBWAPI_VERSION)

  ADD_DEPENDENCIES(${PROJECT_NAME} cvextern)
ENDIF()
