# --------------------------------------------------------
#  Copyright (C) 2004-2013 by EMGU. All rights reserved.
# --------------------------------------------------------

IF((EMGU_CV_BUILD OR EMGU_CV_EXAMPLE_BUILD) AND NOT (ANDROID OR IOS))

PROJECT(Emgu.CV)

FILE(GLOB_RECURSE SRC_CV RELATIVE ${CMAKE_CURRENT_SOURCE_DIR} "*.cs")
LIST(APPEND SRC_CV ${CMAKE_CURRENT_SOURCE_DIR}/../CommonAssemblyInfo.cs)

IF (NETFX_CORE)
  #Remove android, or ios related files
  FILE(GLOB_RECURSE SRC_CV_EXCLUDES RELATIVE ${CMAKE_CURRENT_SOURCE_DIR} "PInvoke/iOS/*.cs" "PInvoke/Android/*.cs" "StringTable.Designer.cs" )
ELSE()
  #Remove android, ios or windows store library related files
  FILE(GLOB_RECURSE SRC_CV_EXCLUDES RELATIVE ${CMAKE_CURRENT_SOURCE_DIR} "PInvoke/iOS/*.cs" "PInvoke/Android/*.cs" "PInvoke/Windows.Store/*.cs")
ENDIF()

LIST(REMOVE_ITEM SRC_CV ${SRC_CV_EXCLUDES})

GENERATE_DOCUMENT(${LIBRARY_OUTPUT_PATH}/${PROJECT_NAME})
SIGN_ASSEMBLY(${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV.snk)
IF(NETFX_CORE)
  ADD_CS_REFERENCES("${LIBRARY_OUTPUT_PATH}/Emgu.Util.dll")
ELSE()
  ADD_CS_REFERENCES("System.Xml.dll;System.Drawing.dll;System.Data.dll;System.ServiceModel.dll;System.Core.dll;${LIBRARY_OUTPUT_PATH}/Emgu.Util.dll")
ENDIF()

ADD_CS_RESOURCES(${CMAKE_CURRENT_SOURCE_DIR}/Properties/StringTable.resx ${PROJECT_NAME}.Properties.StringTable.resources)

COMPILE_CS(${PROJECT_NAME} "library" "${SRC_CV}" ALL)

if(ENABLE_SOLUTION_FOLDERS)
    set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "emgu")
endif()

ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.Util cvextern)

INSTALL(
  FILES 
  ${LIBRARY_OUTPUT_PATH}/${PROJECT_NAME}.dll
  ${LIBRARY_OUTPUT_PATH}/${PROJECT_NAME}.xml
  ${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV.License.txt
  DESTINATION ${CPACK_PACKAGE_CLI_FOLDER}
  COMPONENT emgucv_binary)

IF(WIN32 OR APPLE)

IF(NETFX_CORE)
  SET(CSPROJ_FILE "${PROJECT_NAME}.Windows.Store.csproj")
ELSE()
  SET(CSPROJ_FILE "${PROJECT_NAME}.csproj")
ENDIF()
  
INSTALL(
  DIRECTORY
  ${CMAKE_CURRENT_SOURCE_DIR}
  DESTINATION .
    COMPONENT emgucv_source
  FILES_MATCHING 
  PATTERN "*.cs"
  PATTERN "${CSPROJ_FILE}"
  PATTERN "*.resx"
  PATTERN "README.txt"
  PATTERN ".svn" EXCLUDE
  PATTERN "obj" EXCLUDE
  PATTERN "CMakeFiles" EXCLUDE
  PATTERN "${PROJECT_NAME}.dir" EXCLUDE
  PATTERN "*Android*" EXCLUDE
  PATTERN "*iOS*" EXCLUDE
)
ENDIF()

ENDIF()
