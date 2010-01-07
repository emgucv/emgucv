IF(EMGU_CV_BUILD OR EMGU_CV_EXAMPLE_BUILD)

PROJECT(Emgu.CV.Test)

FILE(GLOB_RECURSE SRC_CV_TEST RELATIVE ${CMAKE_CURRENT_SOURCE_DIR} "*.cs")
SET(SRC_CV_TEST ${SRC_CV_TEST} ${CMAKE_CURRENT_SOURCE_DIR}/../CommonAssemblyInfo.cs)

#copy the test data for this example
FILE(GLOB_RECURSE SRC_CV_TEST_DATA RELATIVE ${CMAKE_CURRENT_SOURCE_DIR} "*.jpg" "*.png" "*.gif" "*.data" "*.xml" "*.avi")
SET(SRC_CV_TEST_DATA 
	${SRC_CV_TEST_DATA} 
	${CMAKE_CURRENT_SOURCE_DIR}/../opencv/data/haarcascades/haarcascade_frontalface_alt2.xml
	${CMAKE_CURRENT_SOURCE_DIR}/../opencv/data/haarcascades/haarcascade_eye.xml
	${CMAKE_CURRENT_SOURCE_DIR}/../lib/nunit.framework.dll)
FOREACH(FILE ${SRC_CV_TEST_DATA})
	ADD_CS_FILE_TO_DEPLOY(${FILE})
ENDFOREACH()

ADD_CS_REFERENCES("${LIBRARY_OUTPUT_PATH}/Emgu.CV.dll;${LIBRARY_OUTPUT_PATH}/Emgu.Util.dll;${LIBRARY_OUTPUT_PATH}/Emgu.CV.ML.dll;${LIBRARY_OUTPUT_PATH}/Emgu.CV.UI.dll;System.Drawing.dll;${LIBRARY_OUTPUT_PATH}/Emgu.UI.dll;System.Windows.Forms.dll;System.Core.dll;${CMAKE_CURRENT_SOURCE_DIR}/../lib/nunit.framework.dll")

COMPILE_CS(${PROJECT_NAME} "library" "${SRC_CV_TEST}")

ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.UI Emgu.CV.ML Emgu.UI)

ENDIF()
