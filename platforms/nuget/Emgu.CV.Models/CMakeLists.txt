# --------------------------------------------------------
#  Copyright (C) 2004-2024 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF ((TARGET Emgu.CV.Models.NetStandard) AND (TARGET Emgu.CV.Models.IOS))
  PROJECT(Emgu.CV.Models.nuget)

  get_filename_component(NUGET_OUTPUT_DIR ${CMAKE_CURRENT_SOURCE_DIR} DIRECTORY)

  BUILD_NUGET_PACKAGE(
    ${PROJECT_NAME} 
	"${CMAKE_CURRENT_SOURCE_DIR}/../../../Emgu.CV.Models/Emgu.CV.Models.NetStandard/Emgu.CV.Models.NetStandard.csproj"  #csproj_file 
	"${CMAKE_CURRENT_SOURCE_DIR}/Package.nuspec" #nuspec_file 
	"${NUGET_OUTPUT_DIR}" #output_dir 
	"${CMAKE_CURRENT_SOURCE_DIR}" #working_dir
	)
	
  IF (EMGU_NUGET_SIGN_FOUND)
    EMGU_SIGN_NUGET(${PROJECT_NAME} "${NUGET_OUTPUT_DIR}/${EMGUCV_MODELS_NUGET_ID}.${CPACK_PACKAGE_VERSION}.nupkg")
  ENDIF()

  IF (APPLE)
    add_custom_command(TARGET ${PROJECT_NAME}
      PRE_BUILD
      COMMAND dotnet build -c ${DEFAULT_CS_CONFIG} "${CMAKE_CURRENT_SOURCE_DIR}/../../../Emgu.CV.Models/Emgu.CV.Models.NetStandard/Emgu.CV.Models.NetStandard.csproj"
      COMMAND dotnet build -c ${DEFAULT_CS_CONFIG} "${CMAKE_CURRENT_SOURCE_DIR}/../../../Emgu.CV.Models/Emgu.CV.Models.IOS/Emgu.CV.Models.IOS.csproj"
      COMMENT "Building binary for Emgu.CV.Models.nuget")
  ENDIF()
  

  ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.Models.NetStandard)
  ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.Models.IOS)

  IF (TARGET Emgu.CV.nuget)
    ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.nuget)
  ENDIF()
  
  if(ENABLE_SOLUTION_FOLDERS)
    set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "nuget")
  endif() 
ENDIF()
