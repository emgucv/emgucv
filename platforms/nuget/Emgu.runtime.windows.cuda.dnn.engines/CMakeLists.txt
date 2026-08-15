# --------------------------------------------------------
#  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF(WIN32 AND HAVE_WINDESKTOP_X64 AND WITH_CUDNN)
  PROJECT(Emgu.runtime.windows.cuda.dnn.engines.nuget)

  BUILD_WINDOWS_RUNTIME_NUGET_PACKAGE(EMGU_CUDNN_ENGINES_NUGET_ID EMGU_CUDNN_VERSION)

  ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.runtime.windows.cuda.dnn.nuget)
ENDIF()
