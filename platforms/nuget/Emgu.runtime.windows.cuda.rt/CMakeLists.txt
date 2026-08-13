# --------------------------------------------------------
#  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF (WIN32 AND HAVE_WINDESKTOP_X64 AND WITH_CUDA)
  PROJECT(Emgu.runtime.windows.cuda.rt.nuget)

  BUILD_WINDOWS_RUNTIME_NUGET_PACKAGE(EMGU_CUDART_NUGET_ID EMGU_CUDART_VERSION)

  ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV cvextern)
ENDIF()
