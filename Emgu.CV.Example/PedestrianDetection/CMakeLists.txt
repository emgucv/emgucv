PROJECT(Example.PedestrianDetection)

FILE(GLOB_RECURSE SRC_Example_PedestrianDetection RELATIVE ${CMAKE_CURRENT_SOURCE_DIR} "*.cs")
LIST(APPEND SRC_Example_PedestrianDetection ${CMAKE_CURRENT_SOURCE_DIR}/../../CommonAssemblyInfo.cs)

LIST(APPEND CS_FLAGS ${EMGU_CV_GPU_REFERENCE})

#copy the test image for this example
ADD_CS_FILE_TO_DEPLOY(${CMAKE_CURRENT_SOURCE_DIR}/pedestrian.png)

COMPILE_CS(${PROJECT_NAME} "winexe" "${SRC_Example_PedestrianDetection}" ALL)

ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV Emgu.CV.UI, Emgu.CV.GPU)

INSTALL(FILES ${LIBRARY_OUTPUT_PATH}/${PROJECT_NAME}.${CS_EXECUTABLE_EXTENSION} DESTINATION bin COMPONENT binary)
