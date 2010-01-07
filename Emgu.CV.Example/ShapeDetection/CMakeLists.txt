PROJECT(Example.ShapeDetection)

FILE(GLOB_RECURSE SRC_Example_ShapeDetection RELATIVE ${CMAKE_CURRENT_SOURCE_DIR} "*.cs")
SET(SRC_Example_ShapeDetection ${SRC_Example_ShapeDetection} ${CMAKE_CURRENT_SOURCE_DIR}/../../CommonAssemblyInfo.cs)

SET(CS_FLAGS ${CS_FLAGS} ${EMGU_CV_REFERENCE})

#copy the test image for this example
ADD_CS_FILE_TO_DEPLOY(${CMAKE_CURRENT_SOURCE_DIR}/pic3.png)

COMPILE_CS(${PROJECT_NAME} "winexe" "${SRC_Example_ShapeDetection}" ALL)

ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV Emgu.CV.UI)

INSTALL(FILES ${LIBRARY_OUTPUT_PATH}/${PROJECT_NAME}.${CS_EXECUTABLE_EXTENSION} DESTINATION bin COMPONENT binary)
