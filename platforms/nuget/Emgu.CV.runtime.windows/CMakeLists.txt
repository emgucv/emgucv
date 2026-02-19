# --------------------------------------------------------
#  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF ( (TARGET Emgu.CV)
  AND (HAVE_WINDESKTOP) 
  AND (NOT ("${EMGU_WINDOWS_MSVCRT_VERSION}" STREQUAL "")) )
  
  PROJECT(Emgu.CV.runtime.windows.nuget)

  SET(EMGUCV_WINDOWS_NUGET_FILE_LIST "")
  IF(HAVE_WINDESKTOP_X86)
	SET(EMGUCV_WINDOWS_NUGET_FILE_LIST "${EMGUCV_WINDOWS_NUGET_FILE_LIST} <file src=\"..\\..\\..\\libs\\runtimes\\win-x86\\native\\*.dll\" exclude=\"..\\..\\..\\libs\\runtimes\\win-x86\\native\\cu*.dll;..\\..\\..\\libs\\runtimes\\win-x86\\native\\npp*.dll;..\\..\\..\\libs\\runtimes\\win-x86\\native\\concrt*.dll;..\\..\\..\\libs\\runtimes\\win-x86\\native\\msvcp*.dll;..\\..\\..\\libs\\runtimes\\win-x86\\native\\vcruntime*.dll;..\\..\\..\\libs\\runtimes\\win-x86\\native\\vccorlib*.dll\" target=\"runtimes\\win-x86\\native\" />")
	SET(EMGUCV_WINDOWS_NUGET_FILE_LIST "${EMGUCV_WINDOWS_NUGET_FILE_LIST} <file src=\"..\\..\\..\\libs\\runtimes\\win-x86\\native\\version_string.inc\" target=\"docs\\build_info_x86.inc\" />")
  ENDIF()
  IF(HAVE_WINDESKTOP_X64)
	SET(EMGUCV_WINDOWS_NUGET_FILE_LIST "${EMGUCV_WINDOWS_NUGET_FILE_LIST} <file src=\"..\\..\\..\\libs\\runtimes\\win-x64\\native\\*.dll\" exclude=\"..\\..\\..\\libs\\runtimes\\win-x64\\native\\cu*.dll;..\\..\\..\\libs\\runtimes\\win-x64\\native\\npp*.dll;..\\..\\..\\libs\\runtimes\\win-x64\\native\\concrt*.dll;..\\..\\..\\libs\\runtimes\\win-x64\\native\\msvcp*.dll;..\\..\\..\\libs\\runtimes\\win-x64\\native\\vcruntime*.dll;..\\..\\..\\libs\\runtimes\\win-x64\\native\\vccorlib*.dll;..\\..\\..\\libs\\runtimes\\win-x64\\native\\zlibwapi.dll\" target=\"runtimes\\win-x64\\native\" />")
	#Also added configuration files from OpenVino
	SET(EMGUCV_WINDOWS_NUGET_FILE_LIST "${EMGUCV_WINDOWS_NUGET_FILE_LIST} <file src=\"..\\..\\..\\libs\\runtimes\\win-x64\\native\\*.xml\" target=\"runtimes\\win-x64\\native\" />")
	SET(EMGUCV_WINDOWS_NUGET_FILE_LIST "${EMGUCV_WINDOWS_NUGET_FILE_LIST} <file src=\"..\\..\\..\\libs\\runtimes\\win-x64\\native\\*.json\" target=\"runtimes\\win-x64\\native\" />")
	SET(EMGUCV_WINDOWS_NUGET_FILE_LIST "${EMGUCV_WINDOWS_NUGET_FILE_LIST} <file src=\"..\\..\\..\\libs\\runtimes\\win-x64\\native\\*.elf\" target=\"runtimes\\win-x64\\native\" />")
	SET(EMGUCV_WINDOWS_NUGET_FILE_LIST "${EMGUCV_WINDOWS_NUGET_FILE_LIST} <file src=\"..\\..\\..\\libs\\runtimes\\win-x64\\native\\*.mvcmd\" target=\"runtimes\\win-x64\\native\" />")
	SET(EMGUCV_WINDOWS_NUGET_FILE_LIST "${EMGUCV_WINDOWS_NUGET_FILE_LIST} <file src=\"..\\..\\..\\libs\\runtimes\\win-x64\\native\\version_string.inc\" target=\"docs\\build_info_x64.inc\" />")
  ENDIF()
  IF(HAVE_WINDESKTOP_ARM64)
	SET(EMGUCV_WINDOWS_NUGET_FILE_LIST "${EMGUCV_WINDOWS_NUGET_FILE_LIST} 
	   <file src=\"..\\..\\..\\libs\\runtimes\\win-arm64\\native\\*.dll\" exclude=\"..\\..\\..\\libs\\runtimes\\win-arm64\\native\\cu*.dll;..\\..\\..\\libs\\runtimes\\win-arm64\\native\\npp*.dll;..\\..\\..\\libs\\runtimes\\win-arm64\\native\\concrt*.dll;..\\..\\..\\libs\\runtimes\\win-arm64\\native\\msvcp*.dll;..\\..\\..\\libs\\runtimes\\win-arm64\\native\\vcruntime*.dll;..\\..\\..\\libs\\runtimes\\win-arm64\\native\\vccorlib*.dll;..\\..\\..\\libs\\runtimes\\win-arm64\\native\\opencv_videoio_ffmpeg*.dll\" target=\"runtimes\\win-arm64\\native\" />
	   <file src=\"..\\..\\..\\libs\\runtimes\\win-arm64\\native\\version_string.inc\" target=\"docs\\build_info_arm64.inc\" />")
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

  IF (EMGU_NUGET_SIGN_FOUND)
    EMGU_SIGN_NUGET(${PROJECT_NAME} "${NUGET_OUTPUT_DIR}/${EMGUCV_WINDOWS_NUGET_ID}.${CPACK_PACKAGE_VERSION}.nupkg")
  ENDIF()

  if(ENABLE_SOLUTION_FOLDERS)
    set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "nuget")
  endif() 

  ADD_DEPENDENCIES(${PROJECT_NAME} cvextern)
  
  IF (TARGET Emgu.CV)
    ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV)
  ENDIF()
  
  IF (WIN32 AND HAVE_WINDESKTOP_X64 AND WITH_CUDA)
  	ADD_DEPENDENCIES(${PROJECT_NAME} 
	  Emgu.runtime.windows.cuda.fft.nuget 
	  Emgu.runtime.windows.cuda.dnn.cnn.nuget 
	  Emgu.runtime.windows.cuda.dnn.engines.nuget 
	  Emgu.runtime.windows.cuda.dnn.ops.nuget 
	  Emgu.runtime.windows.cuda.dnn.adv.nuget 
	  Emgu.runtime.windows.cuda.blas.nuget
      Emgu.runtime.windows.cuda.blas.lt.nuget	  
	  Emgu.runtime.windows.cuda.npp.nuget)
  ENDIF()

  IF (TARGET Emgu.runtime.windows.msvc.rt.x64.nuget)
  	ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.runtime.windows.msvc.rt.x64.nuget)
  ENDIF()

  IF (TARGET Emgu.runtime.windows.msvc.rt.x86.nuget)
  	ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.runtime.windows.msvc.rt.x86.nuget)
  ENDIF()

  IF (TARGET Emgu.runtime.windows.msvc.rt.arm64.nuget)
  	ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.runtime.windows.msvc.rt.arm64.nuget)
  ENDIF()

#  IF (TARGET Emgu.CV.Platform.NetFramework)
#    ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.Platform.NetFramework)
#  ENDIF()

#  IF (TARGET Emgu.CV.Platform.NetCore)
#    ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.Platform.NetCore)
#  ENDIF()

ENDIF()
