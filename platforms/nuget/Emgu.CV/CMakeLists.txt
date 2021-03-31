# --------------------------------------------------------
#  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF ((TARGET Emgu.CV.Platform.NetStandard) AND (TARGET Emgu.CV.Platform.NetStandard.IOS))
  PROJECT(Emgu.CV.nuget)

  SET(EMGU_CV_NUGET_PROJECT_FILE "${CMAKE_CURRENT_SOURCE_DIR}/../../../Emgu.CV.Platform/NetStandard/Emgu.CV.Platform.NetStandard.csproj")
  
#  IF (TARGET Emgu.CV.Platform.NetStandard.IOS)
#    SET(EMGU_CV_NUGET_PROJECT_FILE "${CMAKE_CURRENT_SOURCE_DIR}/../../../Emgu.CV.Platform/NetStandard_iOS/Emgu.CV.Platform.NetStandard.IOS.csproj")
#  ENDIF()

  ADD_CUSTOM_TARGET(
    ${PROJECT_NAME} ALL
    COMMAND ${DOTNET_EXECUTABLE} pack -c ${DEFAULT_CS_CONFIG} ${EMGU_CV_NUGET_PROJECT_FILE} -p:NuspecFile="${CMAKE_CURRENT_SOURCE_DIR}/Package.nuspec" -o "${CMAKE_CURRENT_SOURCE_DIR}/../"
    WORKING_DIRECTORY "${CMAKE_CURRENT_SOURCE_DIR}"
    )

  ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.Platform.NetStandard)  
  ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.Platform.NetStandard.IOS)
  
  if(ENABLE_SOLUTION_FOLDERS)
    set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "nuget")
  endif() 
  
ENDIF()
