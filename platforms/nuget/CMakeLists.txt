# --------------------------------------------------------
#  Copyright (C) 2004-2019 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF (WIN32 AND (HAVE_WINDESKTOP_X86 OR HAVE_WINDESKTOP_X64))

ADD_SUBDIRECTORY("Cufft")
ADD_SUBDIRECTORY("CUDA")


PROJECT(Emgu.CV.nuget)
ADD_CUSTOM_TARGET(
   ${PROJECT_NAME} ALL
   COMMAND "${CMAKE_CURRENT_SOURCE_DIR}/../../miscellaneous/nuget.exe" pack "${CMAKE_CURRENT_SOURCE_DIR}/Package.nuspec" -OutputDirectory "${CMAKE_CURRENT_SOURCE_DIR}/"
   SOURCES "${CMAKE_CURRENT_SOURCE_DIR}/Package.nuspec"
)
ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.UI)

IF (WIN32 AND (HAVE_WINDESKTOP_X86 OR HAVE_WINDESKTOP_X64) AND WITH_CUDA)
  ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.CUDA.nuget)
ENDIF()

if(ENABLE_SOLUTION_FOLDERS)
  set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "NUGET")
endif() 


ENDIF()

IF(HAVE_ANDROID_ALL_ARCH AND WIN32)
  ADD_SUBDIRECTORY("Android")
ENDIF()

IF(HAVE_IOS AND WIN32)
  ADD_SUBDIRECTORY("iOS")
ENDIF()
