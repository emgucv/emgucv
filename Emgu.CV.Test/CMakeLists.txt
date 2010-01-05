IF(EMGU_CV_BUILD OR EMGU_CV_EXAMPLE_BUILD)

PROJECT(Emgu.CV.Test)

FILE(GLOB_RECURSE SRC_CV_TEST RELATIVE ${CMAKE_CURRENT_SOURCE_DIR} "*.cs")
SET(SRC_CV_TEST ${SRC_CV_TEST} ${CMAKE_CURRENT_SOURCE_DIR}/../CommonAssemblyInfo.cs)

#copy the test data for this example
FILE(GLOB_RECURSE SRC_CV_TEST_DATA RELATIVE ${CMAKE_CURRENT_SOURCE_DIR} "*.jpg" "*.png" "*.gif" "*.data" "*.xml" "*.avi")
FOREACH(FILE ${SRC_CV_TEST_DATA})
	ADD_CS_FILE_TO_DEPLOY(${FILE})
ENDFOREACH()
#copy the haarcascade data for this example
FILE(GLOB_RECURSE SRC_CV_TEST_DATA RELATIVE ${CMAKE_CURRENT_SOURCE_DIR} "*.xml")
FOREACH(FILE ${SRC_CV_TEST_DATA})
	ADD_CS_FILE_TO_DEPLOY(${FILE} haarcascades)
ENDFOREACH()


ADD_CS_REFERENCES("${LIBRARY_OUTPUT_PATH}/Emgu.CV.dll;${LIBRARY_OUTPUT_PATH}/Emgu.Util.dll;${LIBRARY_OUTPUT_PATH}/Emgu.CV.ML.dll;${LIBRARY_OUTPUT_PATH}/Emgu.CV.UI.dll;System.Drawing.dll;${LIBRARY_OUTPUT_PATH}/Emgu.UI.dll;System.Windows.Forms.dll")
IF(WIN32)
  ADD_CS_REFERENCES(${CMAKE_CURRENT_SOURCE_DIR}/../lib/nunit.framework.dll)
  ADD_CS_REFERENCES(C:/Program\ Files/Reference\ Assemblies/Microsoft/Framework/v3.5/System.Core.dll)
ELSE(WIN32)
  #ADD_CS_REFERENCES(nunit.framework.dll)
  ADD_CS_PACKAGE_REFERENCES(nunit-2.4)
  ADD_CS_REFERENCES(System.Core.dll)
ENDIF(WIN32)

ADD_CS_LIBRARY(${PROJECT_NAME} "${SRC_CV_TEST}")

ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.UI Emgu.CV.ML Emgu.UI)

ENDIF()
