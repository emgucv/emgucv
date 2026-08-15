# --------------------------------------------------------
#  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF(WIN32 AND HAVE_WINDESKTOP_X64 AND WITH_CUDA)
  PROJECT(Emgu.runtime.windows.cuda.npp.nuget)

  BUILD_WINDOWS_RUNTIME_NUGET_PACKAGE(EMGU_NPP_NUGET_ID EMGU_NPP_VERSION)

  ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.runtime.windows.cuda.rt.nuget)
ENDIF()
