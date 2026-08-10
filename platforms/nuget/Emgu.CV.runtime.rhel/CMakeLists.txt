# --------------------------------------------------------
#  Copyright (C) 2004-2024 by EMGU Corporation. All rights reserved.
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

  #Build the RID list actually included in this package, for the README's publish section.
  #HAVE_RHEL_X86/X64/ARM64 already account for "already built OR currently
  #building this arch" (see CHECK_BINARY_EXIST in the root CMakeLists.txt),
  #so no extra fallback is needed here, unlike the macOS package. Note that
  #only HAVE_RHEL_X64 is currently wired up to CHECK_BINARY_EXIST in the
  #root CMakeLists.txt -- HAVE_RHEL_X86/ARM64 are referenced but never
  #actually set, so in practice this list will only ever contain rhel-x64
  #until that gap is addressed.
  SET(EMGUCV_RHEL_RID_LIST "")
  IF(HAVE_RHEL_X64)
    LIST(APPEND EMGUCV_RHEL_RID_LIST "rhel-x64")
  ENDIF()
  IF(HAVE_RHEL_X86)
    LIST(APPEND EMGUCV_RHEL_RID_LIST "rhel-x86")
  ENDIF()
  IF(HAVE_RHEL_ARM64)
    LIST(APPEND EMGUCV_RHEL_RID_LIST "rhel-arm64")
  ENDIF()
  LIST(LENGTH EMGUCV_RHEL_RID_LIST EMGUCV_RHEL_RID_COUNT)
  IF(EMGUCV_RHEL_RID_COUNT GREATER 0)
    LIST(GET EMGUCV_RHEL_RID_LIST 0 EMGUCV_RHEL_PRIMARY_RID)
  ELSE()
    SET(EMGUCV_RHEL_PRIMARY_RID "rhel-x64")
  ENDIF()
  IF(EMGUCV_RHEL_RID_COUNT GREATER 1)
    SET(EMGUCV_RHEL_RID_PLURAL_S "s")
  ELSE()
    SET(EMGUCV_RHEL_RID_PLURAL_S "")
  ENDIF()
  SET(EMGUCV_RHEL_RID_LIST_MD "")
  FOREACH(EMGUCV_RHEL_RID_ITEM ${EMGUCV_RHEL_RID_LIST})
    LIST(APPEND EMGUCV_RHEL_RID_LIST_MD "`${EMGUCV_RHEL_RID_ITEM}`")
  ENDFOREACH()
  LIST(JOIN EMGUCV_RHEL_RID_LIST_MD " and " EMGUCV_RHEL_RID_LIST_TEXT)

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
    EMGU_SIGN_NUGET(${PROJECT_NAME} "${NUGET_OUTPUT_DIR}/${EMGUCV_RHEL_NUGET_ID}.${CPACK_PACKAGE_VERSION}.nupkg")
  ENDIF()

  if(ENABLE_SOLUTION_FOLDERS)
    set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "nuget")
  endif() 
ENDIF()

