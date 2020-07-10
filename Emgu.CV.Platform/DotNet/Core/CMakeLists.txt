# --------------------------------------------------------
#  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF((EMGU_CV_BUILD OR EMGU_CV_EXAMPLE_BUILD) AND WIN32)
  
    PROJECT(Emgu.CV.Platform.NetCore)
    
    BUILD_CSPROJ_IN_SOLUTION(${PROJECT_NAME} "${CMAKE_CURRENT_SOURCE_DIR}/../../../Solution/Windows.Desktop/Emgu.CV.Example.sln" "${PROJECT_NAME}" "${EXTRA_BUILD_FLAGS}" ALL)	
    ADD_CUSTOM_COMMAND(TARGET ${PROJECT_NAME} 
      PRE_BUILD
      COMMAND ${MSBUILD_EXECUTABLE} -t:restore Emgu.CV.Example.sln
      WORKING_DIRECTORY "${CMAKE_CURRENT_SOURCE_DIR}/../../../Solution/Windows.Desktop")

    INSTALL(
      FILES 
      ${LIBRARY_OUTPUT_PATH}/${PROJECT_NAME}.xml
      ${LIBRARY_OUTPUT_PATH}/${PROJECT_NAME}.dll 
      DESTINATION ${CPACK_PACKAGE_CLI_FOLDER}
      COMPONENT emgucv_binary)
    
    if(ENABLE_SOLUTION_FOLDERS)
      set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "emgu")
    endif()
    
    ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.Bitmap)
  
ENDIF()
