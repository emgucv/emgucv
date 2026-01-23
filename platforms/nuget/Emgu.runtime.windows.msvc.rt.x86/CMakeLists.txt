# --------------------------------------------------------
#  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF (WIN32 AND HAVE_WINDESKTOP_X86)

PROJECT(Emgu.runtime.windows.msvc.rt.x86.nuget)

  #SET(EMGU_WINDOWS_MSVCRT_X86_NUGET_TITLE "MSVC x86 runtime for windows")
  #SET(EMGU_WINDOWS_MSVCRT_X86_NUGET_FILE_LIST "${EMGU_WINDOWS_MSVCRT_X86_NUGET_FILE_LIST} 
#	<file src=\"..\\..\\..\\libs\\runtimes\\win-x86\\native\\concrt*.dll\" target=\"runtimes\\win-x86\\native\" /> 
#	<file src=\"..\\..\\..\\libs\\runtimes\\win-x86\\native\\msvcp*.dll\" target=\"runtimes\\win-x86\\native\" /> 
#	<file src=\"..\\..\\..\\libs\\runtimes\\win-x86\\native\\vcruntime*.dll\" target=\"runtimes\\win-x86\\native\" />     
#	<file src=\"..\\..\\..\\libs\\runtimes\\win-x86\\native\\vccorlib*.dll\" target=\"runtimes\\win-x86\\native\" />     
#	<file src=\"..\\..\\..\\miscellaneous\\nuget_icon.png\" target=\"icon.png\" />
#   <file src=\"..\\..\\..\\${EMGUCV_LICENSE_FILE}\" target=\"\" />
#	<file src=\"README.md\" target=\"docs\\\" />")

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

  IF (EMGU_NUGET_SIGN_FOUND)
    EMGU_SIGN_NUGET(${PROJECT_NAME} "${NUGET_OUTPUT_DIR}/${EMGU_WINDOWS_MSVCRT_X86_NUGET_ID}.${EMGU_WINDOWS_MSVCRT_NUGET_VERSION}.nupkg")
  ENDIF()

  ADD_DEPENDENCIES(${PROJECT_NAME} cvextern)

  IF (TARGET Emgu.CV)
    ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV)
  ENDIF()

  if(ENABLE_SOLUTION_FOLDERS)
    set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "nuget")
  endif() 

ENDIF()
