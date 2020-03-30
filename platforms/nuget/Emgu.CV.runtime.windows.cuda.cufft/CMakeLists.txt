# --------------------------------------------------------
#  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF (WIN32 AND (HAVE_WINDESKTOP_X86 OR HAVE_WINDESKTOP_X64) AND WITH_CUDA)

PROJECT(Emgu.CV.runtime.windows.cuda.cufft.nuget)
ADD_CUSTOM_TARGET(
   ${PROJECT_NAME}
   COMMAND "${CMAKE_CURRENT_SOURCE_DIR}/../../../miscellaneous/nuget.exe" pack "${CMAKE_CURRENT_SOURCE_DIR}/Package.nuspec" -OutputDirectory "${CMAKE_CURRENT_SOURCE_DIR}/../"
)
ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.nuget cvextern)

if(ENABLE_SOLUTION_FOLDERS)
  set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "nuget")
endif() 

ENDIF()
