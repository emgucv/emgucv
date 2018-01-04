# --------------------------------------------------------
#  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

#IF(WIN32)

PROJECT(Example.Simple3DReconstruction)

FILE(GLOB_RECURSE SRC_Example_Simple3DReconstruction RELATIVE ${CMAKE_CURRENT_SOURCE_DIR} "*.cs")
LIST(APPEND SRC_Example_Simple3DReconstruction ${CMAKE_CURRENT_SOURCE_DIR}/../../CommonAssemblyInfo.cs)

#Add emgucv references
LIST(APPEND CS_FLAGS ${EMGU_CV_REFERENCE})

#SET(OPENTK_DLL_DIRECTORY "${CMAKE_CURRENT_SOURCE_DIR}/../../lib/3rdParty")

#Add Tao components
#SET(OPENTK_DLL "OpenTK.dll" "OpenTK.GLControl.dll")
#FOREACH(DLL_FILE ${OPENTK_DLL})
#  LIST(APPEND CS_PREBUILD_COMMAND
#     COMMAND ${CMAKE_COMMAND} -E copy "${OPENTK_DLL_DIRECTORY}/${DLL_FILE}" ${CMAKE_CURRENT_SOURCE_DIR})
#     ADD_CS_FILE_TO_DEPLOY(${CMAKE_CURRENT_SOURCE_DIR}/${DLL_FILE})
#  LIST(APPEND CS_FLAGS -r:"${CMAKE_CURRENT_SOURCE_DIR}/${DLL_FILE}")
#ENDFOREACH()     

#copy the test image for this example
ADD_CS_FILE_TO_DEPLOY(${CMAKE_CURRENT_SOURCE_DIR}/imL.png)
ADD_CS_FILE_TO_DEPLOY(${CMAKE_CURRENT_SOURCE_DIR}/imR.png)

COMPILE_CS(${PROJECT_NAME} "winexe" "${SRC_Example_Simple3DReconstruction}" ALL)
set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "Emgu.CV.Examples")
ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.World Emgu.CV.UI)

INSTALL(FILES ${LIBRARY_OUTPUT_PATH}/${PROJECT_NAME}.${CS_EXECUTABLE_EXTENSION} DESTINATION bin COMPONENT emgucv_example_binary)

#ENDIF()
