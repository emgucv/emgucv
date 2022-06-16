# --------------------------------------------------------
#  Copyright (C) 2004-2022 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF (TARGET Emgu.CV.Wpf)
  PROJECT(Emgu.CV.Wpf.nuget)

  get_filename_component(NUGET_OUTPUT_DIR ${CMAKE_CURRENT_SOURCE_DIR} DIRECTORY)

  BUILD_NUGET_PACKAGE(
    ${PROJECT_NAME} 
	"${CMAKE_CURRENT_SOURCE_DIR}/../../../Emgu.CV.Platform/Emgu.CV.Wpf/Emgu.CV.Wpf.csproj"  #csproj_file 
	"${CMAKE_CURRENT_SOURCE_DIR}/Package.nuspec" #nuspec_file 
	"${NUGET_OUTPUT_DIR}" #output_dir 
	"${CMAKE_CURRENT_SOURCE_DIR}" #working_dir
	) 

  IF (EMGU_NUGET_SIGN_FOUND)
    EMGU_SIGN_NUGET(${PROJECT_NAME} "${NUGET_OUTPUT_DIR}/${EMGUCV_WPF_NUGET_ID}.${CPACK_PACKAGE_VERSION}.nupkg")
  ENDIF()

  IF (TARGET Emgu.CV.nuget)
    ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.nuget)
  ENDIF()

  ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.Wpf)

  if(ENABLE_SOLUTION_FOLDERS)
    set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "nuget")
  endif() 
  
ENDIF()
