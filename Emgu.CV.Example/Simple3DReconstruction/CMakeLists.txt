# --------------------------------------------------------
#  Copyright (C) 2004-2011 by EMGU. All rights reserved.
# --------------------------------------------------------

IF(WIN32)

PROJECT(Example.Simple3DReconstruction)

FILE(GLOB_RECURSE SRC_Example_Simple3DReconstruction RELATIVE ${CMAKE_CURRENT_SOURCE_DIR} "*.cs")
LIST(APPEND SRC_Example_Simple3DReconstruction ${CMAKE_CURRENT_SOURCE_DIR}/../../CommonAssemblyInfo.cs)

#Add emgucv references
IF(TARGET_ARCH_64)
  LIST(APPEND CS_FLAGS -platform:x64)
ELSE()
  LIST(APPEND CS_FLAGS -platform:x86)
ENDIF()
LIST(APPEND CS_FLAGS ${EMGU_CV_REFERENCE})

IF(TARGET_ARCH_64)
  SET(OSG_DLL_DIRECTORY "${CMAKE_CURRENT_SOURCE_DIR}/../../lib/3rdParty/x86_64")
ELSE()
  SET(OSG_DLL_DIRECTORY "${CMAKE_CURRENT_SOURCE_DIR}/../../lib/3rdParty/x86")
ENDIF()
SET(TAO_DLL_DIRECTORY "${CMAKE_CURRENT_SOURCE_DIR}/../../lib/3rdParty")

#Add Tao components
SET(TAO_DLL "Tao.OpenGl.dll" "Tao.Platform.Windows.dll")
FOREACH(DLL_FILE ${TAO_DLL})
  LIST(APPEND CS_PREBUILD_COMMAND
     COMMAND ${CMAKE_COMMAND} -E copy "${TAO_DLL_DIRECTORY}/${DLL_FILE}" ${CMAKE_CURRENT_SOURCE_DIR})
     ADD_CS_FILE_TO_DEPLOY(${CMAKE_CURRENT_SOURCE_DIR}/${DLL_FILE})
  LIST(APPEND CS_FLAGS -r:"${CMAKE_CURRENT_SOURCE_DIR}/${DLL_FILE}")
ENDFOREACH()     

#Add OSG components
SET(OSG_DLL "OpenThreads.dll" "osg.dll" "osgDB.dll" "osgFX.dll" "osgGA.dll" "osgManipulator.dll" 
"osgText.dll" "osgUtil.dll" "osgViewer.dll")
FOREACH (DLL_FILE ${OSG_DLL})
   LIST(APPEND CS_PREBUILD_COMMAND
   COMMAND ${CMAKE_COMMAND} -E copy "${OSG_DLL_DIRECTORY}/${DLL_FILE}" ${CMAKE_CURRENT_SOURCE_DIR})
   ADD_CS_FILE_TO_DEPLOY(${CMAKE_CURRENT_SOURCE_DIR}/${DLL_FILE})
ENDFOREACH()

#Add OSG DOTNET components
SET(OSG_DOT_NET_DLL "OsgUtil.Net.dll" "OsgViewer.NET.dll" "Osg.NET.dll" "OsgDB.NET.dll"
"OsgFX.NET.dll" "OsgGA.NET.dll" "OsgManipulator.NET.dll" "OsgText.NET.dll")
FOREACH (DLL_FILE ${OSG_DOT_NET_DLL})
   LIST(APPEND CS_PREBUILD_COMMAND
   COMMAND ${CMAKE_COMMAND} -E copy "${OSG_DLL_DIRECTORY}/${DLL_FILE}" ${CMAKE_CURRENT_SOURCE_DIR})
   ADD_CS_FILE_TO_DEPLOY(${CMAKE_CURRENT_SOURCE_DIR}/${DLL_FILE})
   LIST(APPEND CS_FLAGS -r:"${CMAKE_CURRENT_SOURCE_DIR}/${DLL_FILE}")
ENDFOREACH()

COMPILE_CS(${PROJECT_NAME} "winexe" "${SRC_Example_Simple3DReconstruction}" ALL)
set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "Emgu.CV.Examples")
ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV Emgu.CV.UI)

#INSTALL(FILES ${LIBRARY_OUTPUT_PATH}/${PROJECT_NAME}.${CS_EXECUTABLE_EXTENSION} DESTINATION bin COMPONENT emgucv_example_binary)

ENDIF()