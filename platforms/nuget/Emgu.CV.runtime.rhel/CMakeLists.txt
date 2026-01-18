# --------------------------------------------------------
#  Copyright (C) 2004-2024 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF(HAVE_RHEL_X64)
  PROJECT(Emgu.CV.runtime.rhel.nuget)
  
  SET(EMGUCV_RHEL_NUGET_TITLE "Emgu CV Native Runtime for RHEL")
  CONFIGURE_FILE(${CMAKE_CURRENT_SOURCE_DIR}/Package.nuspec.in ${CMAKE_CURRENT_SOURCE_DIR}/Package.nuspec)
  CONFIGURE_FILE(${EMGU_CV_SOURCE_DIR}/cmake/UseRidGraph.props.in ${CMAKE_CURRENT_SOURCE_DIR}/Emgu.CV.runtime.rhel.props)
  CONFIGURE_FILE(${CMAKE_CURRENT_SOURCE_DIR}/README.md.in ${CMAKE_CURRENT_SOURCE_DIR}/README.md)

  get_filename_component(NUGET_OUTPUT_DIR ${CMAKE_CURRENT_SOURCE_DIR} DIRECTORY)

  BUILD_NUGET_PACKAGE(
    ${PROJECT_NAME} 
	"${EMGU_CV_SOURCE_DIR}/Emgu.CV/NetStandard/Emgu.CV.csproj"  #csproj_file 
	"${CMAKE_CURRENT_SOURCE_DIR}/Package.nuspec" #nuspec_file 
	"${NUGET_OUTPUT_DIR}" #output_dir 
	"${CMAKE_CURRENT_SOURCE_DIR}" #working_dir
	) 
  
  ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV)
  
  IF (EMGU_NUGET_SIGN_FOUND)
    EMGU_SIGN_NUGET(${PROJECT_NAME} "${NUGET_OUTPUT_DIR}/${EMGUCV_UBUNTU_NUGET_ID}.${CPACK_PACKAGE_VERSION}.nupkg")
  ENDIF()

  if(ENABLE_SOLUTION_FOLDERS)
    set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "nuget")
  endif() 
ENDIF()

