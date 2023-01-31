# --------------------------------------------------------
#  Copyright (C) 2004-2023 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF(DOTNET_FOUND AND (NOT (IOS OR ANDROID OR NETFX_CORE)))

  PROJECT(Emgu.CV.Test.Net)

  BUILD_CSPROJ_IN_SOLUTION(${PROJECT_NAME} "${CMAKE_CURRENT_SOURCE_DIR}/../Solution/Windows.Desktop/Emgu.CV.Test.Net.sln" "Emgu.CV.Test.Netcore" "" ALL)
  
  if(ENABLE_SOLUTION_FOLDERS)
    set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "emgu")
  endif()

  ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.Platform.NetStandard)

ENDIF()
