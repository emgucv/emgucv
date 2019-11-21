# --------------------------------------------------------
#  Copyright (C) 2004-2019 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF (TARGET Emgu.CV.World.Netstandard)
  PROJECT(Emgu.CV.nuget)

  ADD_CUSTOM_TARGET(
    ${PROJECT_NAME} ALL
    COMMAND ${DOTNET_EXECUTABLE} pack -c ${DEFAULT_CS_CONFIG} "${CMAKE_CURRENT_SOURCE_DIR}/../../../Emgu.CV.World/Netstandard/Emgu.CV.World.Netstandard.csproj" -p:NuspecFile="${CMAKE_CURRENT_SOURCE_DIR}/Package.nuspec" -o "${CMAKE_CURRENT_SOURCE_DIR}/../"
    WORKING_DIRECTORY "${CMAKE_CURRENT_SOURCE_DIR}"
    )
  ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.World.Netstandard)
  
  if(ENABLE_SOLUTION_FOLDERS)
    set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "nuget")
  endif() 
  
ENDIF()
