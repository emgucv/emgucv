# --------------------------------------------------------
#  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF (WIN32 AND (HAVE_WINDESKTOP_X86 OR HAVE_WINDESKTOP_X64) AND WITH_CUDA)

PROJECT(Emgu.CV.CUDA.nuget)
ADD_CUSTOM_TARGET(
   ${PROJECT_NAME} ALL
   COMMAND "${CMAKE_CURRENT_SOURCE_DIR}/../../../miscellaneous/nuget.exe" pack "${CMAKE_CURRENT_SOURCE_DIR}/Package.nuspec"
   SOURCES "${CMAKE_CURRENT_SOURCE_DIR}/Package.nuspec"
)
ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.UI)

ENDIF()
