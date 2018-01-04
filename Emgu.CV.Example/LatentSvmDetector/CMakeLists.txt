# --------------------------------------------------------
#  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

PROJECT(Example.LatenSvmDetector)

FILE(GLOB_RECURSE SRC_Example_LatenSvmDetector RELATIVE ${CMAKE_CURRENT_SOURCE_DIR} "*.cs")
LIST(APPEND SRC_Example_LatenSvmDetector ${CMAKE_CURRENT_SOURCE_DIR}/../../CommonAssemblyInfo.cs)

#IF(TARGET_ARCH_64)
#  LIST(APPEND CS_FLAGS -platform:x64)
#ELSE()
#  LIST(APPEND CS_FLAGS -platform:x86)
#ENDIF()

LIST(APPEND CS_FLAGS ${EMGU_CV_REFERENCE})

#copy the test image for this example
ADD_CS_FILE_TO_DEPLOY(${CMAKE_CURRENT_SOURCE_DIR}/cat.jpg)
ADD_CS_FILE_TO_DEPLOY(${CMAKE_CURRENT_SOURCE_DIR}/cat.xml)

COMPILE_CS(${PROJECT_NAME} "winexe" "${SRC_Example_LatenSvmDetector}" ALL)
set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "Emgu.CV.Examples")
ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV Emgu.CV.UI)

INSTALL(FILES 
  "${LIBRARY_OUTPUT_PATH}/${PROJECT_NAME}.${CS_EXECUTABLE_EXTENSION}"
  "${CMAKE_CURRENT_SOURCE_DIR}/cat.jpg"
  "${CMAKE_CURRENT_SOURCE_DIR}/cat.xml"
  DESTINATION bin COMPONENT emgucv_example_binary)
