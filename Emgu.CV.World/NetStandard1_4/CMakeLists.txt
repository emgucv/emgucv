# --------------------------------------------------------
#  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF(EMGU_CV_BUILD OR EMGU_CV_EXAMPLE_BUILD)

  IF(WIN32 AND (MSVC_VERSION GREATER 1900)) #need MSVC to be at least 2017
    PROJECT(Emgu.CV.World.NetStandard1_4)
    BUILD_CSPROJ(${PROJECT_NAME} "${CMAKE_CURRENT_SOURCE_DIR}/Emgu.CV.World.NetStandard1_4.csproj" "" ALL)
    INSTALL(
      FILES 
      ${LIBRARY_OUTPUT_PATH}/${PROJECT_NAME}.xml
      ${LIBRARY_OUTPUT_PATH}/${PROJECT_NAME}.dll 
      DESTINATION ${CPACK_PACKAGE_CLI_FOLDER}
      COMPONENT emgucv_binary)	  
	ADD_CUSTOM_COMMAND(TARGET ${PROJECT_NAME} 
	  PRE_BUILD
	  COMMAND "${CMAKE_CURRENT_SOURCE_DIR}/../../miscellaneous/nuget.exe" restore "${CMAKE_CURRENT_SOURCE_DIR}/Emgu.CV.World.NetStandard1_4.csproj") 

    ADD_DEPENDENCIES(${PROJECT_NAME} cvextern)  

	INSTALL(
	  DIRECTORY
	  ${CMAKE_CURRENT_SOURCE_DIR}
	  DESTINATION .
	  COMPONENT emgucv_source
	  FILES_MATCHING 
	  PATTERN "Emgu.CV.World.NetStandard1_4.csproj"
	  PATTERN ".svn" EXCLUDE
	  PATTERN "obj" EXCLUDE
	  PATTERN "CMakeFiles" EXCLUDE
	  PATTERN "${PROJECT_NAME}.dir" EXCLUDE
	  )
	
	  set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "emgu")
  ENDIF()  
   
ENDIF()
