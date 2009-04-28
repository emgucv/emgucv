PROJECT(Emgu.CV.Example.TrafficSignRecognition)

SET(SRC_Example_TrafficSignRecognition *.cs Properties/*.cs)

SET(CS_FLAGS ${CS_FLAGS} ${EMGU_CV_REFERENCE})

#copy the test image for this example
ADD_CS_FILE_TO_DEPLOY(${CMAKE_CURRENT_SOURCE_DIR}/stop-sign.jpg)
ADD_CS_FILE_TO_DEPLOY(${CMAKE_CURRENT_SOURCE_DIR}/stop-sign-model.png)

ADD_CS_EXECUTABLE(${PROJECT_NAME} "${SRC_Example_TrafficSignRecognition}")

ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV Emgu.CV.UI)

INSTALL(FILES ${LIBRARY_OUTPUT_PATH}/${PROJECT_NAME}.dll DESTINATION bin COMPONENT binary)
