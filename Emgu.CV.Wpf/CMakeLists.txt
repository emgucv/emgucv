# --------------------------------------------------------
#  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF (TARGET Emgu.CV)
  IF (DOTNET_FOUND AND WIN32)
    # Only build Emgu.CV.Wpf for Windows
    PROJECT(Emgu.CV.Wpf)
      SET(EXTRA_BUILD_FLAGS "")
      #IF(WIN32)
      #  SET(EXTRA_BUILD_FLAGS "/p:Platform=\"Any CPU\"")
      #ENDIF()

    #generate nuget config file for Emgu.CV.Wpf
    CONFIGURE_FILE(${EMGU_CV_SOURCE_DIR}/cmake/Emgu.CV.Wpf.Directory.Build.props.in ${EMGU_CV_SOURCE_DIR}/Emgu.CV.Wpf/Directory.Build.props)
    IF(EMGU_SIGN_FOUND AND WIN32)
      CONFIGURE_FILE(${EMGU_CV_SOURCE_DIR}/cmake/SignBinary.Targets.in ${EMGU_CV_SOURCE_DIR}/Emgu.CV.Wpf/SignBinary.Targets)
    ENDIF()
    IF(EMGU_NUGET_SIGN_FOUND AND WIN32)
      CONFIGURE_FILE(${EMGU_CV_SOURCE_DIR}/cmake/SignPkg.Targets.in ${EMGU_CV_SOURCE_DIR}/Emgu.CV.Wpf/SignPkg.Targets)
    ENDIF()
    CONFIGURE_FILE(${EMGU_CV_SOURCE_DIR}/cmake/Emgu.CV.Wpf.Package.README.md.in ${EMGU_CV_SOURCE_DIR}/Emgu.CV.Wpf/docs/README.md)

    BUILD_DOTNET_PROJ(${PROJECT_NAME} "${CMAKE_CURRENT_SOURCE_DIR}/Emgu.CV.Wpf.csproj" "${EXTRA_BUILD_FLAGS}" ALL)
	
    #IF (EMGU_SIGN_FOUND)
    #  EMGU_SIGN_BINARY(${PROJECT_NAME} ${CMAKE_CURRENT_SOURCE_DIR}/../../libs/${PROJECT_NAME}.dll)
    #ENDIF()

    INSTALL(
      DIRECTORY 
      ${LIBRARY_OUTPUT_PATH}/Build/Emgu.CV.Wpf
      DESTINATION ${CPACK_PACKAGE_CLI_FOLDER}/Build
      COMPONENT emgucv_binary)	  
  
    if(ENABLE_SOLUTION_FOLDERS)
      set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "emgu")
    endif()
    ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV Emgu.CV.Bitmap)
  ENDIF()
  
  INSTALL(
      DIRECTORY ${CMAKE_CURRENT_SOURCE_DIR}
      DESTINATION .
      COMPONENT emgucv_source
      FILES_MATCHING 
      PATTERN "Emgu.CV.Wpf.csproj"
	  PATTERN "Directory.Build.props"
	  PATTERN "README.md"
	  #PATTERN "*.png"
	  PATTERN "*.cs"
      PATTERN ".svn" EXCLUDE
      PATTERN "obj" EXCLUDE
      PATTERN "CMakeFiles" EXCLUDE
      PATTERN "${PROJECT_NAME}.dir" EXCLUDE
      )
	  
ENDIF()
