PROJECT(Example.Webservice_Host)

FILE(GLOB_RECURSE SRC_Example_Webservice_Host RELATIVE ${CMAKE_CURRENT_SOURCE_DIR} "*.cs")
SET(SRC_Example_Webservice_Host ${SRC_Example_Webservice_Host} ${CMAKE_CURRENT_SOURCE_DIR}/../../CommonAssemblyInfo.cs)

ADD_CS_REFERENCES(System.ServiceModel.dll)

SET(CS_FLAGS ${CS_FLAGS} ${EMGU_CV_REFERENCE})

COMPILE_CS(${PROJECT_NAME} "exe" "${SRC_Example_Webservice_Host}" ALL)

ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV Emgu.CV.UI)

INSTALL(FILES ${LIBRARY_OUTPUT_PATH}/${PROJECT_NAME}.${CS_EXECUTABLE_EXTENSION} DESTINATION bin COMPONENT binary)
