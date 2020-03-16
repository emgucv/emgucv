# --------------------------------------------------------
#  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF (HAVE_WINSTORE_10)
  PROJECT(Emgu.CV.runtime.uwp.nuget)

  ADD_CUSTOM_TARGET(
    ${PROJECT_NAME} ALL
    COMMAND "${CMAKE_CURRENT_SOURCE_DIR}/../../../miscellaneous/nuget.exe" pack Package.nuspec -OutputDirectory "${CMAKE_CURRENT_SOURCE_DIR}/../"
    WORKING_DIRECTORY "${CMAKE_CURRENT_SOURCE_DIR}"
    )
  ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.nuget)
  
  if(ENABLE_SOLUTION_FOLDERS)
    set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "nuget")
  endif() 
  
ENDIF()
