# --------------------------------------------------------
#  Copyright (C) 2004-2023 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF(DOTNET_FOUND AND (NOT (IOS OR ANDROID OR NETFX_CORE)))

  PROJECT(Emgu.CV.Test.Net)

    SET(EXTRA_BUILD_FLAGS "/p:Platform=\"AnyCPU\"")
    
    #IF (NOT (WIN32 OR APPLE))
    #  #LINUX
    #  SET(EXTRA_BUILD_FLAGS ${EXTRA_BUILD_FLAGS} "/p:TargetFrameworks=\"netstandard2.0\"")
    #ENDIF()

    BUILD_DOTNET_PROJ(${PROJECT_NAME} "${CMAKE_CURRENT_SOURCE_DIR}/Emgu.CV.Test.Netcore/Emgu.CV.Test.Netcore.csproj" "${EXTRA_BUILD_FLAGS}" ALL)
    #BUILD_CSPROJ_IN_SOLUTION(${PROJECT_NAME} "${CMAKE_CURRENT_SOURCE_DIR}/../Solution/Windows.Desktop/Emgu.CV.Test.Net.sln" "Emgu.CV.Test.Netcore" "" ALL)
  
  if(ENABLE_SOLUTION_FOLDERS)
    set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "emgu")
  endif()
  IF (TARGET Emgu.CV)
    ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV)
  ENDIF()

ENDIF()
