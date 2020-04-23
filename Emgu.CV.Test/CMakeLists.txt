# --------------------------------------------------------
#  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF(DOTNET_FOUND AND (NOT (IOS OR ANDROID)))

  PROJECT(Emgu.CV.Test.Netcore)

#IF(APPLE)
#  BUILD_CSPROJ_IN_SOLUTION(${PROJECT_NAME} "${CMAKE_CURRENT_SOURCE_DIR}/../Solution/Windows.Desktop/Emgu.CV.Test.Netcore.sln" "" "" ALL)
#  ADD_CUSTOM_COMMAND(TARGET ${PROJECT_NAME} 
#    PRE_BUILD
#    #COMMAND mono "${CMAKE_CURRENT_SOURCE_DIR}/../miscellaneous/nuget.exe" restore "${CMAKE_CURRENT_SOURCE_DIR}/Emgu.CV.Test.Netcore/Emgu.CV.Test.Netcore.csproj"
#    COMMAND ${MSBUILD_EXECUTABLE} -t:restore 
#    WORKING_DIRECTORY "${CMAKE_CURRENT_SOURCE_DIR}/Emgu.CV.Test.Netcore")
#ELSE()
  BUILD_DOTNET_PROJ(${PROJECT_NAME} "${CMAKE_CURRENT_SOURCE_DIR}/Emgu.CV.Test.Netcore/Emgu.CV.Test.Netcore.csproj" "${EXTRA_BUILD_FLAGS}" ALL)
#ENDIF()

if(ENABLE_SOLUTION_FOLDERS)
    set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "emgu")
endif()

ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.Platform.NetStandard)

ENDIF()
