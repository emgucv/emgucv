# --------------------------------------------------------
#  Copyright (C) 2004-2025 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF ( (TARGET Emgu.CV)
  AND (HAVE_WINDESKTOP_X86 OR HAVE_WINDESKTOP_X64 OR HAVE_WINDESKTOP_ARM64) 
  AND (NOT ("${EMGU_WINDOWS_MSVCRT_VERSION}" STREQUAL "")) )
  
  PROJECT(Emgu.CV.runtime.windows.nuget)

  get_filename_component(NUGET_OUTPUT_DIR ${CMAKE_CURRENT_SOURCE_DIR} DIRECTORY)

  BUILD_NUGET_PACKAGE(
    ${PROJECT_NAME} 
	"${EMGU_CV_SOURCE_DIR}/Emgu.CV/NetStandard/Emgu.CV.csproj"  #csproj_file 
	"${CMAKE_CURRENT_SOURCE_DIR}/Package.nuspec" #nuspec_file 
	"${NUGET_OUTPUT_DIR}" #output_dir 
	"${CMAKE_CURRENT_SOURCE_DIR}" #working_dir
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
