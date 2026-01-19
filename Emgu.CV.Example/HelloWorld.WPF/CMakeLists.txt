# --------------------------------------------------------
#  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF (DOTNET_FOUND AND WIN32 ) 
  PROJECT(Example.HelloWorld.WPF)
  
  CONFIGURE_FILE(${CMAKE_CURRENT_SOURCE_DIR}/Directory.Build.props.in ${CMAKE_CURRENT_SOURCE_DIR}/Directory.Build.props)
  
  BUILD_DOTNET_PROJ(${PROJECT_NAME} "${CMAKE_CURRENT_SOURCE_DIR}/HelloWorld.WPF.csproj"  "" ALL)
  
  set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "Emgu.CV.Examples")
  
  #IF (TARGET Emgu.CV)
  #  ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV)
  #ENDIF()
  
  IF (TARGET Emgu.CV.Wpf)
    ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.Wpf)
  ENDIF()
ENDIF()
