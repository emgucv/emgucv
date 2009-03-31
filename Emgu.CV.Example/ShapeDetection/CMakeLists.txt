PROJECT(Emgu.CV.Example.ShapeDetection)

SET(SRC_Example_ShapeDetection *.cs Properties/*.cs)

SET(CS_FLAGS ${CS_FLAGS} ${EMGU_CV_REFERENCE})

ADD_CS_EXECUTABLE(${PROJECT_NAME} "${SRC_Example_ShapeDetection}")

#copy the test image for this example
ADD_CUSTOM_COMMAND(
    TARGET ${PROJECT_NAME}
    POST_BUILD
    COMMAND ${CMAKE_COMMAND} -E copy ARGS pic3.png ${EXECUTABLE_OUTPUT_PATH}/
)

ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV Emgu.CV.UI)

INSTALL(FILES ${LIBRARY_OUTPUT_PATH}/${PROJECT_NAME}.dll DESTINATION bin COMPONENT binary)
