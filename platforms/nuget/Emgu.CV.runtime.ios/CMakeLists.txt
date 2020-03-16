# --------------------------------------------------------
#  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF(HAVE_IOS AND WIN32)

PROJECT(Emgu.CV.runtime.ios.nuget)
IF (APPLE)
ADD_CUSTOM_TARGET(
   ${PROJECT_NAME} ALL
   COMMAND nuget pack "${CMAKE_CURRENT_SOURCE_DIR}/Package.nuspec" -OutputDirectory "${CMAKE_CURRENT_SOURCE_DIR}/../"
)
ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.Platform.IOS)
ELSE()
ADD_CUSTOM_TARGET(
   ${PROJECT_NAME} ALL
   COMMAND "${CMAKE_CURRENT_SOURCE_DIR}/../../../miscellaneous/nuget.exe" pack "${CMAKE_CURRENT_SOURCE_DIR}/Package.nuspec" -OutputDirectory "${CMAKE_CURRENT_SOURCE_DIR}/../"
)
ENDIF()


if(ENABLE_SOLUTION_FOLDERS)
  set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "nuget")
endif() 

ENDIF()
