# --------------------------------------------------------
#  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF(WIN32)
PROJECT(Example.Webservice_Client)

FILE(GLOB_RECURSE SRC_Example_Webservice_Client RELATIVE ${CMAKE_CURRENT_SOURCE_DIR} "*.cs")
LIST(APPEND SRC_Example_Webservice_Client 
  ${CMAKE_CURRENT_SOURCE_DIR}/../Webservice_Host/IImageService.cs
  ${CMAKE_CURRENT_SOURCE_DIR}/../../CommonAssemblyInfo.cs)

#IF(TARGET_ARCH_64)
#  LIST(APPEND CS_FLAGS -platform:x64)
#ELSE()
#  LIST(APPEND CS_FLAGS -platform:x86)
#ENDIF()

LIST(APPEND CS_FLAGS ${EMGU_CV_REFERENCE})

ADD_CS_REFERENCES(System.ServiceModel.dll)

GET_CS_EXECUTABLE_EXTENSION()

COMPILE_CS(${PROJECT_NAME} "winexe" "${SRC_Example_Webservice_Client}" ALL)
set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "Emgu.CV.Examples")
ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.World Emgu.CV.UI)

INSTALL(FILES ${LIBRARY_OUTPUT_PATH}/${PROJECT_NAME}.${CS_EXECUTABLE_EXTENSION} DESTINATION bin COMPONENT binary)
ENDIF()