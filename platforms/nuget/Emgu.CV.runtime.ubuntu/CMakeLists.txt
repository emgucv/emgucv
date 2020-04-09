# --------------------------------------------------------
#  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

PROJECT(Emgu.CV.runtime.ubuntu.nuget)
IF(WIN32)
ADD_CUSTOM_TARGET(
   ${PROJECT_NAME} ALL
   COMMAND "${CMAKE_CURRENT_SOURCE_DIR}/../../../miscellaneous/nuget.exe" pack Package.nuspec -OutputDirectory "${CMAKE_CURRENT_SOURCE_DIR}/../"
   WORKING_DIRECTORY "${CMAKE_CURRENT_SOURCE_DIR}"
)
ELSE()
ADD_CUSTOM_TARGET(
   ${PROJECT_NAME} ALL
   COMMAND ${DOTNET_EXECUTABLE} pack "${CMAKE_CURRENT_SOURCE_DIR}/../../../Emgu.CV.Platform/NetStandard/Emgu.CV.Platform.NetStandard.csproj" -c ${DEFAULT_CS_CONFIG} --no-build -p:NuspecFile="${CMAKE_CURRENT_SOURCE_DIR}/Package.nuspec" -o "${CMAKE_CURRENT_SOURCE_DIR}/../"
   WORKING_DIRECTORY "${CMAKE_CURRENT_SOURCE_DIR}"
)
ENDIF()

ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.nuget)

if(ENABLE_SOLUTION_FOLDERS)
  set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "nuget")
endif() 


