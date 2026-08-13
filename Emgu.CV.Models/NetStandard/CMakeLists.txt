# --------------------------------------------------------
#  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF(EMGU_CV_BUILD OR EMGU_CV_EXAMPLE_BUILD)
  PROJECT(Emgu.CV.Models)
  
  SET(EXTRA_BUILD_FLAGS "")
  #generate nuget config file for Emgu.CV.Models
  CONFIGURE_FILE(${CMAKE_CURRENT_SOURCE_DIR}/Directory.Build.props.in ${CMAKE_CURRENT_SOURCE_DIR}/Directory.Build.props)
  IF(EMGU_SIGN_FOUND AND WIN32)
    CONFIGURE_FILE(${EMGU_CV_SOURCE_DIR}/cmake/SignBinary.Targets.in ${EMGU_CV_SOURCE_DIR}/Emgu.CV.Models/NetStandard/SignBinary.Targets)
  ENDIF()
  IF(EMGU_NUGET_SIGN_FOUND AND WIN32)
    CONFIGURE_FILE(${EMGU_CV_SOURCE_DIR}/cmake/SignPkg.Targets.in ${EMGU_CV_SOURCE_DIR}/Emgu.CV.Models/NetStandard/SignPkg.Targets)
  ENDIF()
  CONFIGURE_FILE(${CMAKE_CURRENT_SOURCE_DIR}/README.md.in ${CMAKE_CURRENT_SOURCE_DIR}/docs/README.md)

  IF(DOTNET_FOUND)
    # dotnet msbuild seems to have a bug on Mac OS, it do not respect the assembly name.
    # It output Emgu.CV.Model.NetStandard.dll instead of Emgu.CV.Model.dll specified in
    # the csproj file. dotnet seems to work just fine.
    BUILD_DOTNET_PROJ(${PROJECT_NAME} "${CMAKE_CURRENT_SOURCE_DIR}/Emgu.CV.Models.csproj" "${EXTRA_BUILD_FLAGS}" ALL)
  ELSE()
    MESSAGE(FATAL_ERROR "dotnet command not found, unable to build Emgu.CV.Models project")
  ENDIF()
  
  INSTALL(
    DIRECTORY 
    ${EMGU_CV_SOURCE_DIR}/libs/Build/Emgu.CV.Models
    DESTINATION ${CPACK_PACKAGE_CLI_FOLDER}/Build
    COMPONENT emgucv_binary)
    
  IF (TARGET Emgu.CV)
    ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV)  
  ENDIF()
  
  if(ENABLE_SOLUTION_FOLDERS)
    set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "emgu")
  endif()
ENDIF()
