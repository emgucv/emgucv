# --------------------------------------------------------
#  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF ((TARGET Emgu.CV.Models.NetStandard) AND (TARGET Emgu.CV.Models.IOS))
  PROJECT(Emgu.CV.Models.nuget)

  BUILD_NUGET_PACKAGE(
    ${PROJECT_NAME} 
	"${CMAKE_CURRENT_SOURCE_DIR}/../../../Emgu.CV.Models/Emgu.CV.Models.NetStandard/Emgu.CV.Models.NetStandard.csproj"  #csproj_file 
	"${CMAKE_CURRENT_SOURCE_DIR}/Package.nuspec" #nuspec_file 
	"${CMAKE_CURRENT_SOURCE_DIR}/../" #output_dir 
	"${CMAKE_CURRENT_SOURCE_DIR}" #working_dir
	) 
  
  ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.Models.NetStandard)
  ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.Models.IOS)

  IF (TARGET Emgu.CV.nuget)
    ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.nuget)
  ENDIF()
  
  if(ENABLE_SOLUTION_FOLDERS)
    set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "nuget")
  endif() 
ENDIF()
