# --------------------------------------------------------
#  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF (HAVE_WINSTORE_10)

  PROJECT(Emgu.CV.runtime.uwp.nuget)

  SET(EMGUCV_UWP_NUGET_ID "${EMGUCV_NUGET_ID}.runtime${EMGUCV_RUNTIME_EXTRA_TAG}.uwp")

  SET(EMGUCV_UWP_NUGET_TITLE "Emgu CV for UWP Native Runtime")
  
  SET(EMGUCV_UWP_NUGET_FILE_LIST "")
  IF(HAVE_WINSTORE_10_X86)
	SET(EMGUCV_UWP_NUGET_FILE_LIST "${EMGUCV_UWP_NUGET_FILE_LIST} <file src=\"..\\..\\..\\libs\\runtimes\\win10-x86\\native\\*.dll\" target=\"runtimes\\win10-x86\\native\" />")
  ENDIF()
  IF(HAVE_WINSTORE_10_X64)
	SET(EMGUCV_UWP_NUGET_FILE_LIST "${EMGUCV_UWP_NUGET_FILE_LIST} <file src=\"..\\..\\..\\libs\\runtimes\\win10-x64\\native\\*.dll\" target=\"runtimes\\win10-x64\\native\" />")
  ENDIF()
  IF(HAVE_WINSTORE_10_ARM)
	SET(EMGUCV_UWP_NUGET_FILE_LIST "${EMGUCV_UWP_NUGET_FILE_LIST} <file src=\"..\\..\\..\\libs\\runtimes\\win10-arm\\native\\*.dll\" target=\"runtimes\\win10-arm\\native\" />")
  ENDIF()
  IF(HAVE_WINSTORE_10_ARM64)
	SET(EMGUCV_UWP_NUGET_FILE_LIST "${EMGUCV_UWP_NUGET_FILE_LIST} <file src=\"..\\..\\..\\libs\\runtimes\\win10-arm64\\native\\*.dll\" target=\"runtimes\\win10-arm64\\native\" />")
  ENDIF()
  
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

  TARGET_SOURCES(${PROJECT_NAME} PRIVATE
	"${CMAKE_CURRENT_SOURCE_DIR}/Package.nuspec.in"
	"${CMAKE_CURRENT_SOURCE_DIR}/Package.nuspec"
	"${CMAKE_CURRENT_SOURCE_DIR}/README.md.in"
	"${CMAKE_CURRENT_SOURCE_DIR}/README.md"
  )

  source_group("Template" FILES 
	"${CMAKE_CURRENT_SOURCE_DIR}/Package.nuspec.in"
	"${CMAKE_CURRENT_SOURCE_DIR}/README.md.in")
	
  source_group("Nuget" FILES 
	"${CMAKE_CURRENT_SOURCE_DIR}/Package.nuspec"
	"${CMAKE_CURRENT_SOURCE_DIR}/README.md"
  )
	
  ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV cvextern)
  
  IF (EMGU_NUGET_SIGN_FOUND)
    EMGU_SIGN_NUGET(${PROJECT_NAME} "${NUGET_OUTPUT_DIR}/${EMGUCV_UWP_NUGET_ID}.${CPACK_PACKAGE_VERSION}.nupkg")
  ENDIF()

  if(ENABLE_SOLUTION_FOLDERS)
    set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "nuget")
  endif() 
  
ENDIF()
