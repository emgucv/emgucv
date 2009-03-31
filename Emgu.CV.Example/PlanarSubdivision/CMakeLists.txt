PROJECT(Emgu.CV.Example.PlanarSubdivision)

SET(SRC_Example_PlanarSubdivision *.cs Properties/*.cs)

SET(CS_FLAGS ${CS_FLAGS} ${EMGU_CV_REFERENCE})

ADD_CS_EXECUTABLE(${PROJECT_NAME} "${SRC_Example_PlanarSubdivision}")

ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV Emgu.CV.UI)

INSTALL(FILES ${LIBRARY_OUTPUT_PATH}/${PROJECT_NAME}.dll DESTINATION bin COMPONENT binary)
