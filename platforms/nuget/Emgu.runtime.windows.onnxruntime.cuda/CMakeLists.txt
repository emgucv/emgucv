# --------------------------------------------------------
#  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF(WIN32 AND HAVE_WINDESKTOP_X64 AND WITH_CUDA AND HAVE_ONNXRUNTIME)
  PROJECT(Emgu.runtime.windows.onnxruntime.cuda.nuget)

  BUILD_WINDOWS_RUNTIME_NUGET_PACKAGE(EMGU_ONNXRUNTIME_CUDA_NUGET_ID EMGU_ONNXRUNTIME_CUDA_VERSION)

  ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV cvextern)
ENDIF()
