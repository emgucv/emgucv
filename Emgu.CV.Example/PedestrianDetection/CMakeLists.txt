# --------------------------------------------------------
#  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

PROJECT(Example.PedestrianDetection)

FILE(GLOB_RECURSE SRC_Example_PedestrianDetection RELATIVE ${CMAKE_CURRENT_SOURCE_DIR} "*.cs")
LIST(APPEND SRC_Example_PedestrianDetection ${CMAKE_CURRENT_SOURCE_DIR}/../../CommonAssemblyInfo.cs)

#IF(TARGET_ARCH_64)
#  LIST(APPEND CS_FLAGS -platform:x64)
#ELSE()
#  LIST(APPEND CS_FLAGS -platform:x86)
#ENDIF()

LIST(APPEND CS_FLAGS ${EMGU_CV_REFERENCE} ${EMGU_CV_CUDA_REFERENCE})

#copy the test image for this example
ADD_CS_FILE_TO_DEPLOY(${CMAKE_CURRENT_SOURCE_DIR}/pedestrian.png)

COMPILE_CS(${PROJECT_NAME} "winexe" "${SRC_Example_PedestrianDetection}" ALL)
set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "Emgu.CV.Examples")
ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.World Emgu.CV.UI)

INSTALL(FILES 
  "${LIBRARY_OUTPUT_PATH}/${PROJECT_NAME}.${CS_EXECUTABLE_EXTENSION}"
  "${CMAKE_CURRENT_SOURCE_DIR}/pedestrian.png"
  DESTINATION bin COMPONENT emgucv_example_binary)
