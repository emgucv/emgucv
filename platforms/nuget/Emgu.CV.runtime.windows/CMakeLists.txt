# --------------------------------------------------------
#  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF ((TARGET Emgu.CV.Platform.NetStandard) AND (HAVE_WINDESKTOP_X86 OR HAVE_WINDESKTOP_X64))
  PROJECT(Emgu.CV.runtime.windows.nuget)

  BUILD_NUGET_PACKAGE(
    ${PROJECT_NAME} 
	"${CMAKE_CURRENT_SOURCE_DIR}/../../../Emgu.CV.Platform/NetStandard/Emgu.CV.Platform.NetStandard.csproj"  #csproj_file 
	"${CMAKE_CURRENT_SOURCE_DIR}/Package.nuspec" #nuspec_file 
	"${CMAKE_CURRENT_SOURCE_DIR}/../" #output_dir 
	"${CMAKE_CURRENT_SOURCE_DIR}" #working_dir
	) 

  if(ENABLE_SOLUTION_FOLDERS)
    set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "nuget")
  endif() 

  ADD_DEPENDENCIES(${PROJECT_NAME} cvextern)
  
  IF (TARGET Emgu.CV.nuget)
    ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.nuget)
  ENDIF()
  
  IF (WIN32 AND HAVE_WINDESKTOP_X64 AND WITH_CUDA)
  	ADD_DEPENDENCIES(${PROJECT_NAME} 
	  Emgu.runtime.windows.cuda.fft.nuget 
	  Emgu.runtime.windows.cuda.dnn.cnn.infer.nuget 
	  Emgu.runtime.windows.cuda.dnn.cnn.train.nuget 
	  Emgu.runtime.windows.cuda.dnn.ops.nuget 
	  Emgu.runtime.windows.cuda.dnn.adv.nuget 
	  Emgu.runtime.windows.cuda.blas.nuget 
	  Emgu.runtime.windows.cuda.npp.nuget)
  ENDIF()

  IF (TARGET Emgu.runtime.windows.msvc.rt.x64.nuget)
  	ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.runtime.windows.msvc.rt.x64.nuget)
  ENDIF()

  IF (TARGET Emgu.runtime.windows.msvc.rt.x86.nuget)
  	ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.runtime.windows.msvc.rt.x86.nuget)
  ENDIF()

  IF (TARGET Emgu.CV.Platform.NetFramework)
    ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.Platform.NetFramework)
  ENDIF()

  IF (TARGET Emgu.CV.Platform.NetCore)
    ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.Platform.NetCore)
  ENDIF()

ENDIF()
