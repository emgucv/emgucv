# --------------------------------------------------------
#  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF (WIN32 AND HAVE_WINDESKTOP_X64 AND WITH_CUDNN)

  PROJECT(Emgu.runtime.windows.zlibwapi.nuget)
  
  #SET(EMGU_WINDOWS_ZLIBWAPI_X64_NUGET_FILE_LIST "${EMGU_WINDOWS_ZLIBWAPI_X64_NUGET_FILE_LIST}
#	<file src=\"..\\..\\..\\libs\\runtimes\\win-x64\\native\\zlibwapi.dll\" target=\"runtimes\\win-x64\\native\" /> 
#	<file src=\"..\\..\\..\\miscellaneous\\nuget_icon.png\" target=\"icon.png\" />
#	<file src=\"zlib_license.txt\" target=\"\" />
#	<file src=\"README.md\" target=\"docs\\\" />")
  
  CONFIGURE_FILE(${CMAKE_CURRENT_SOURCE_DIR}/Package.nuspec.in ${CMAKE_CURRENT_SOURCE_DIR}/Package.nuspec)
  CONFIGURE_FILE(${CMAKE_CURRENT_SOURCE_DIR}/README.md.in ${CMAKE_CURRENT_SOURCE_DIR}/README.md)
  
  get_filename_component(NUGET_OUTPUT_DIR ${CMAKE_CURRENT_SOURCE_DIR} DIRECTORY)
  
  BUILD_NUGET_PACKAGE(
      ${PROJECT_NAME} 
	  "${EMGU_CV_SOURCE_DIR}/Emgu.CV/NetStandard/Emgu.CV.csproj"  #csproj_file 
	  "${CMAKE_CURRENT_SOURCE_DIR}/Package.nuspec" #nuspec_file 
	  "${CMAKE_CURRENT_SOURCE_DIR}/../" #output_dir 
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
	  
  IF (EMGU_NUGET_SIGN_FOUND)
    EMGU_SIGN_NUGET(${PROJECT_NAME} "${NUGET_OUTPUT_DIR}/${EMGU_ZLIBWAPI_ID}.${EMGU_ZLIBWAPI_VERSION}.nupkg")
  ENDIF()
	
  if(ENABLE_SOLUTION_FOLDERS)
    set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "nuget")
  endif() 

  ADD_DEPENDENCIES(${PROJECT_NAME} cvextern)
ENDIF()
