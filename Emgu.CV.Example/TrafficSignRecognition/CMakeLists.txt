PROJECT(Example.TrafficSignRecognition)

FILE(GLOB_RECURSE SRC_Example_TrafficSignRecognition RELATIVE ${CMAKE_CURRENT_SOURCE_DIR} "*.cs")
SET(SRC_Example_TrafficSignRecognition ${SRC_Example_TrafficSignRecognition} ${CMAKE_CURRENT_SOURCE_DIR}/../../CommonAssemblyInfo.cs)

SET(CS_FLAGS ${CS_FLAGS} ${EMGU_CV_REFERENCE})

#copy the test image for this example
ADD_CS_FILE_TO_DEPLOY(${CMAKE_CURRENT_SOURCE_DIR}/stop-sign.jpg)
ADD_CS_FILE_TO_DEPLOY(${CMAKE_CURRENT_SOURCE_DIR}/stop-sign-model.png)

COMPILE_CS(${PROJECT_NAME} "winexe" "${SRC_Example_TrafficSignRecognition}" ALL)

ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV Emgu.CV.UI)

INSTALL(FILES ${LIBRARY_OUTPUT_PATH}/${PROJECT_NAME}.${CS_EXECUTABLE_EXTENSION} DESTINATION bin COMPONENT binary)
