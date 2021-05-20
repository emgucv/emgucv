# --------------------------------------------------------
#  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF (WIN32 AND (HAVE_WINDESKTOP_X86 OR HAVE_WINDESKTOP_X64) AND WITH_CUDNN)

PROJECT(Emgu.runtime.windows.cuda.dnn.cnn.infer.nuget)
  BUILD_NUGET_PACKAGE(
    ${PROJECT_NAME} 
	"${CMAKE_CURRENT_SOURCE_DIR}/../../../Emgu.CV.Platform/NetStandard/Emgu.CV.Platform.NetStandard.csproj"  #csproj_file 
	"${CMAKE_CURRENT_SOURCE_DIR}/Package.nuspec" #nuspec_file 
	"${CMAKE_CURRENT_SOURCE_DIR}/../" #output_dir 
	"${CMAKE_CURRENT_SOURCE_DIR}" #working_dir
	) 
ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.runtime.windows.cuda.dnn.nuget)

if(ENABLE_SOLUTION_FOLDERS)
  set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "nuget")
endif() 

ENDIF()
