# --------------------------------------------------------
#  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF ((TARGET Emgu.CV.Models.NetStandard) OR (TARGET Emgu.CV.Models.IOS))
  PROJECT(Emgu.CV.Models.nuget)
  ADD_CUSTOM_TARGET(
    ${PROJECT_NAME} ALL
    COMMAND ${DOTNET_EXECUTABLE} pack -c ${DEFAULT_CS_CONFIG} "${CMAKE_CURRENT_SOURCE_DIR}/../../../Emgu.CV.Models/Emgu.CV.Models.NetStandard/Emgu.CV.Models.NetStandard.csproj" -p:NuspecFile="${CMAKE_CURRENT_SOURCE_DIR}/Package.nuspec" -o "${CMAKE_CURRENT_SOURCE_DIR}/../"
    WORKING_DIRECTORY "${CMAKE_CURRENT_SOURCE_DIR}"
    )
  
  IF (TARGET Emgu.CV.Models.NetStandard)
    ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.Models.NetStandard)
  ENDIF()
  
  IF (TARGET Emgu.CV.Models.IOS)
    ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.Models.IOS)
  ENDIF()

  if(ENABLE_SOLUTION_FOLDERS)
    set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "nuget")
  endif() 
ENDIF()
