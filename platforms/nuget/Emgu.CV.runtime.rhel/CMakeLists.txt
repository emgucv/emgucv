# --------------------------------------------------------
#  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF(HAVE_RHEL_X64 OR HAVE_RHEL_X86 OR HAVE_RHEL_ARM64)
  PROJECT(Emgu.CV.runtime.rhel.nuget)
  
  SET(EMGUCV_RHEL_NUGET_ID "${EMGUCV_NUGET_ID}.runtime${EMGUCV_RUNTIME_EXTRA_TAG}.${NUGET_NATIVE_PLATFORM_ID_RHEL}")

  SET(EMGUCV_RHEL_NUGET_TITLE "Emgu CV Native Runtime for RHEL")
  
  SET(EMGUCV_RHEL_NUGET_FILE_LIST "")
  IF(HAVE_RHEL_X64)
    SET(EMGUCV_RHEL_NUGET_FILE_LIST "${EMGUCV_RHEL_NUGET_FILE_LIST} <file src=\"..\\..\\..\\libs\\runtimes\\rhel-x64\\native\\*.so\" target=\"runtimes\\rhel-x64\\native\" />")
	SET(EMGUCV_RHEL_NUGET_FILE_LIST "${EMGUCV_RHEL_NUGET_FILE_LIST} <file src=\"..\\..\\..\\libs\\runtimes\\rhel-x64\\native\\rhel_x64_version_string.inc\" target=\"docs\\build_info_x64.inc\" /> ")
  ENDIF()
  IF(HAVE_RHEL_X86)
    SET(EMGUCV_RHEL_NUGET_FILE_LIST "${EMGUCV_RHEL_NUGET_FILE_LIST} <file src=\"..\\..\\..\\libs\\runtimes\\rhel-x86\\native\\*.so\" target=\"runtimes\\rhel-x86\\native\" />")
	SET(EMGUCV_RHEL_NUGET_FILE_LIST "${EMGUCV_RHEL_NUGET_FILE_LIST} <file src=\"..\\..\\..\\libs\\runtimes\\rhel-x86\\native\\rhel_x86_version_string.inc\" target=\"docs\\build_info_x86.inc\" /> ")
  ENDIF()
  IF(HAVE_RHEL_ARM64)
    SET(EMGUCV_RHEL_NUGET_FILE_LIST "${EMGUCV_RHEL_NUGET_FILE_LIST} <file src=\"..\\..\\..\\libs\\runtimes\\rhel-arm64\\native\\*.so\" target=\"runtimes\\rhel-arm64\\native\" />")
	SET(EMGUCV_RHEL_NUGET_FILE_LIST "${EMGUCV_RHEL_NUGET_FILE_LIST} <file src=\"..\\..\\..\\libs\\runtimes\\rhel-arm64\\native\\rhel_arm64_version_string.inc\" target=\"docs\\build_info_arm64.inc\" /> ")
  ENDIF()
	
  #SET(EMGUCV_RHEL_NUGET_FILE_LIST "${EMGUCV_RHEL_NUGET_FILE_LIST} 
  #  <file src=\"..\\..\\..\\miscellaneous\\nuget_icon.png\" target=\"icon.png\" />
  #  <file src=\"..\\..\\..\\${EMGUCV_LICENSE_FILE}\" target=\"\" />
  #  <file src=\"README.md\" target=\"docs\\\" />
#	<file src=\"Emgu.CV.runtime.rhel.props\" target=\"build\\\" />")
  
  CONFIGURE_FILE(${CMAKE_CURRENT_SOURCE_DIR}/Package.nuspec.in ${CMAKE_CURRENT_SOURCE_DIR}/Package.nuspec)
  CONFIGURE_FILE(${EMGU_CV_SOURCE_DIR}/cmake/UseRidGraph.props.in ${CMAKE_CURRENT_SOURCE_DIR}/${EMGUCV_RHEL_NUGET_ID}.props)
  CONFIGURE_FILE(${CMAKE_CURRENT_SOURCE_DIR}/README.md.in ${CMAKE_CURRENT_SOURCE_DIR}/README.md)

  get_filename_component(NUGET_OUTPUT_DIR ${CMAKE_CURRENT_SOURCE_DIR} DIRECTORY)

  BUILD_NUGET_PACKAGE(
    ${PROJECT_NAME} 
	"${EMGU_CV_SOURCE_DIR}/Emgu.CV/NetStandard/Emgu.CV.csproj"  #csproj_file 
	"${CMAKE_CURRENT_SOURCE_DIR}/Package.nuspec" #nuspec_file 
	"${NUGET_OUTPUT_DIR}" #output_dir 
	"${CMAKE_CURRENT_SOURCE_DIR}" #working_dir
	) 
  
  TARGET_SOURCES(${PROJECT_NAME} PRIVATE
    "${CMAKE_CURRENT_SOURCE_DIR}/${EMGUCV_RHEL_NUGET_ID}.props"
	"${CMAKE_CURRENT_SOURCE_DIR}/Package.nuspec.in"
	"${CMAKE_CURRENT_SOURCE_DIR}/Package.nuspec"
	"${CMAKE_CURRENT_SOURCE_DIR}/README.md.in"
	"${CMAKE_CURRENT_SOURCE_DIR}/README.md"
  )

  source_group("Template" FILES 
	"${CMAKE_CURRENT_SOURCE_DIR}/Package.nuspec.in"
	"${CMAKE_CURRENT_SOURCE_DIR}/README.md.in")
	
  source_group("Nuget" FILES 
    "${CMAKE_CURRENT_SOURCE_DIR}/${EMGUCV_RHEL_NUGET_ID}.props"
	"${CMAKE_CURRENT_SOURCE_DIR}/Package.nuspec"
	"${CMAKE_CURRENT_SOURCE_DIR}/README.md"
  )

  ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV)
  
  IF (EMGU_NUGET_SIGN_FOUND)
    EMGU_SIGN_NUGET(${PROJECT_NAME} "${NUGET_OUTPUT_DIR}/${EMGUCV_UBUNTU_NUGET_ID}.${CPACK_PACKAGE_VERSION}.nupkg")
  ENDIF()

  if(ENABLE_SOLUTION_FOLDERS)
    set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "nuget")
  endif() 
ENDIF()

