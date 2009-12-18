IF(EMGU_CV_BUILD OR EMGU_CV_EXAMPLE_BUILD)

PROJECT(Emgu.CV)

FILE(GLOB_RECURSE SRC_CV RELATIVE ${CMAKE_CURRENT_SOURCE_DIR} "*.cs")
SET(SRC_CV ${SRC_CV} ${CMAKE_CURRENT_SOURCE_DIR}/../CommonAssemblyInfo.cs)

GENERATE_DOCUMENT(${LIBRARY_OUTPUT_PATH}/${PROJECT_NAME})
SIGN_ASSEMBLY(${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV.snk)
ADD_CS_REFERENCES("System.Xml.dll;System.Drawing.dll;System.Data.dll;${LIBRARY_OUTPUT_PATH}/Emgu.Util.dll;${LIBRARY_3RDPARTY_PATH}/zlib.net.dll")
IF(WIN32)
  ADD_CS_REFERENCES(C:/Program\ Files/Reference\ Assemblies/Microsoft/Framework/v3.0/System.ServiceModel.dll)
  ADD_CS_REFERENCES(C:/Program\ Files/Reference\ Assemblies/Microsoft/Framework/v3.5/System.Core.dll)
ELSE(WIN32)
  ADD_CS_REFERENCES(System.ServiceModel.dll)
  ADD_CS_REFERENCES(System.Core.dll)
ENDIF(WIN32)
ADD_CS_RESOURCES(${CMAKE_CURRENT_SOURCE_DIR}/Properties/StringTable.resx ${PROJECT_NAME}.Properties.StringTable.resources)

ADD_CS_FILE_TO_DEPLOY(${CMAKE_CURRENT_SOURCE_DIR}/../lib/3rdParty/zlib.net.dll)

ADD_CS_LIBRARY(${PROJECT_NAME} "${SRC_CV}" ALL)

ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.Util cvextern)

INSTALL(
  FILES 
  ${LIBRARY_OUTPUT_PATH}/${PROJECT_NAME}.dll
  ${LIBRARY_OUTPUT_PATH}/${PROJECT_NAME}.xml
  DESTINATION ${CPACK_PACKAGE_CLI_FOLDER}
  COMPONENT emgucv_binary)

IF(WIN32)
INSTALL(
  DIRECTORY
  ${CMAKE_CURRENT_SOURCE_DIR}
  DESTINATION ./
    COMPONENT emgucv_source
  FILES_MATCHING 
  PATTERN "*.cs"
  PATTERN "*.csproj"
  PATTERN "*.resx"
  PATTERN ".svn" EXCLUDE
  PATTERN "obj" EXCLUDE
  PATTERN "CMakeFiles" EXCLUDE
  PATTERN "${PROJECT_NAME}.dir" EXCLUDE
)
ENDIF()

ENDIF()
