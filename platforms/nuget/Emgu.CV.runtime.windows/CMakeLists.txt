# --------------------------------------------------------
#  Copyright (C) 2004-2019 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF (TARGET Emgu.CV.Netstandard)
  PROJECT(Emgu.CV.runtime.windows.nuget)

  ADD_CUSTOM_TARGET(
    ${PROJECT_NAME} ALL
    COMMAND ${DOTNET_EXECUTABLE} pack -c ${DEFAULT_CS_CONFIG} "${CMAKE_CURRENT_SOURCE_DIR}/../../../Emgu.CV.World/Netstandard/Emgu.CV.Netstandard.csproj" -p:NuspecFile="${CMAKE_CURRENT_SOURCE_DIR}/Package.nuspec" -o "${CMAKE_CURRENT_SOURCE_DIR}/../"
    WORKING_DIRECTORY "${CMAKE_CURRENT_SOURCE_DIR}"
    )
  ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.Netstandard)
  
  if(ENABLE_SOLUTION_FOLDERS)
    set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "nuget")
  endif() 
  
  IF (WIN32 AND (HAVE_WINDESKTOP_X64_CUDA) )
  	ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.TF.Dnn.nuget)
  	ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.TF.Cufft.nuget)
  ENDIF()
ENDIF()
