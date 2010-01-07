PROJECT(Example.Webservice_Client)

FILE(GLOB_RECURSE SRC_Example_Webservice_Client RELATIVE ${CMAKE_CURRENT_SOURCE_DIR} "*.cs")
SET(SRC_Example_Webservice_Client ${SRC_Example_Webservice_Client} ${CMAKE_CURRENT_SOURCE_DIR}/../../CommonAssemblyInfo.cs)

SET(CS_FLAGS ${CS_FLAGS} ${EMGU_CV_REFERENCE})

ADD_CS_REFERENCES(System.ServiceModel.dll)

GET_CS_EXECUTABLE_EXTENSION()
ADD_CS_REFERENCES(${LIBRARY_OUTPUT_PATH}/Example.Webservice_Host.${CS_EXECUTABLE_EXTENSION})

COMPILE_CS(${PROJECT_NAME} "winexe" "${SRC_Example_Webservice_Client}" ALL)

ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV Emgu.CV.UI Example.Webservice_Host)

INSTALL(FILES ${LIBRARY_OUTPUT_PATH}/${PROJECT_NAME}.${CS_EXECUTABLE_EXTENSION} DESTINATION bin COMPONENT binary)
