# --------------------------------------------------------
#  Copyright (C) 2004-2019 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF(DOTNET_FOUND)

PROJECT(Emgu.CV.Test.Netcore)

BUILD_CSPROJ_IN_SOLUTION(${PROJECT_NAME} "${CMAKE_CURRENT_SOURCE_DIR}/../Solution/Windows.Desktop/Emgu.CV.Test.Netcore.sln" "${PROJECT_NAME}" "${EXTRA_BUILD_FLAGS}" ALL)
#  ADD_CUSTOM_COMMAND(TARGET ${PROJECT_NAME} 
#	PRE_BUILD
#	COMMAND ${DOTNET_EXECUTABLE} restore ${PROJECT_NAME}.csproj
#	WORKING_DIRECTORY "${CMAKE_CURRENT_SOURCE_DIR}")

if(ENABLE_SOLUTION_FOLDERS)
    set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "emgu")
endif()

ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.UI Emgu.CV.World.Netstandard)

ENDIF()
