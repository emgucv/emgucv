# --------------------------------------------------------
#  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF(HAVE_LINUX_ARM)

  PROJECT(Emgu.CV.runtime.debian-arm.nuget)

  SET(EMGUCV_DEBIAN_ARM_NUGET_TITLE "Emgu CV Native ARM Runtime for Debian (including Raspberry Pi OS)")
  
  SET(EMGUCV_DEBIAN_ARM_NUGET_FILE_LIST "")
  SET(EMGUCV_DEBIAN_ARM_NUGET_FILE_LIST "${EMGUCV_DEBIAN_ARM_NUGET_FILE_LIST} 
      <file src=\"..\\..\\..\\libs\\runtimes\\linux-arm\\native\\*.so\" target=\"runtimes\\linux-arm\\native\" />")
  SET(EMGUCV_DEBIAN_ARM_NUGET_FILE_LIST "${EMGUCV_DEBIAN_ARM_NUGET_FILE_LIST} 
	  <file src=\"..\\..\\..\\libs\\runtimes\\linux-arm\\native\\debian_arm_version_string.inc\" target=\"docs\\build_info_arm.inc\" />")
  SET(EMGUCV_DEBIAN_ARM_NUGET_FILE_LIST "${EMGUCV_DEBIAN_ARM_NUGET_FILE_LIST} 
      <file src=\"..\\..\\..\\miscellaneous\\nuget_icon.png\" target=\"icon.png\" />
      <file src=\"..\\..\\..\\${EMGUCV_LICENSE_FILE}\" target=\"\" />
	  <file src=\"README.md\" target=\"docs\\\" />
	  <file src=\"Emgu.CV.runtime.debian-arm.props\" target=\"build\\\" />")
	  
  CONFIGURE_FILE(${EMGU_CV_SOURCE_DIR}/cmake/UseRidGraph.props.in ${CMAKE_CURRENT_SOURCE_DIR}/Emgu.CV.runtime.debian-arm.props)
  CONFIGURE_FILE(${CMAKE_CURRENT_SOURCE_DIR}/Package.nuspec.in ${CMAKE_CURRENT_SOURCE_DIR}/Package.nuspec)
  CONFIGURE_FILE(${CMAKE_CURRENT_SOURCE_DIR}/README.md.in ${CMAKE_CURRENT_SOURCE_DIR}/README.md)
  
  get_filename_component(NUGET_OUTPUT_DIR ${CMAKE_CURRENT_SOURCE_DIR} DIRECTORY)

  BUILD_NUGET_PACKAGE(
    ${PROJECT_NAME} 
	"${EMGU_CV_SOURCE_DIR}/Emgu.CV/NetStandard/Emgu.CV.csproj"  #csproj_file 
	"${CMAKE_CURRENT_SOURCE_DIR}/Package.nuspec" #nuspec_file 
	"${NUGET_OUTPUT_DIR}" #output_dir 
	"${CMAKE_CURRENT_SOURCE_DIR}" #working_dir
	) 

  IF (EMGU_NUGET_SIGN_FOUND)
    EMGU_SIGN_NUGET(${PROJECT_NAME} "${NUGET_OUTPUT_DIR}/${EMGUCV_DEBIAN_ARM_NUGET_ID}.${CPACK_PACKAGE_VERSION}.nupkg")
  ENDIF()

  ADD_DEPENDENCIES(${PROJECT_NAME} cvextern)

  IF(TARGET Emgu.CV)
    ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV)
  ENDIF()

  if(ENABLE_SOLUTION_FOLDERS)
    set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "nuget")
  endif() 

ENDIF()

