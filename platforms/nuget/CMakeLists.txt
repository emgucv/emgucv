# --------------------------------------------------------
#  Copyright (C) 2004-2019 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------



ADD_SUBDIRECTORY("Cufft")
ADD_SUBDIRECTORY("Cudnn")
ADD_SUBDIRECTORY("CUDA")

#PROJECT(Emgu.CV.nuget)
#ADD_CUSTOM_TARGET(
#   ${PROJECT_NAME} ALL
#   COMMAND "${CMAKE_CURRENT_SOURCE_DIR}/../../miscellaneous/nuget.exe" pack "${CMAKE_CURRENT_SOURCE_DIR}/Package.nuspec" -OutputDirectory "${CMAKE_CURRENT_SOURCE_DIR}/"
#   SOURCES "${CMAKE_CURRENT_SOURCE_DIR}/Package.nuspec"
#)
#ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.UI)



#if(ENABLE_SOLUTION_FOLDERS)
#  set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "nuget")
#endif() 



ADD_SUBDIRECTORY("runtime.android.Emgu.CV")


IF(HAVE_IOS AND WIN32)
  ADD_SUBDIRECTORY("iOS")
ENDIF()
