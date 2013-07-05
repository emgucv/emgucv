# --------------------------------------------------------
#  Copyright (C) 2004-2013 by EMGU. All rights reserved.
# --------------------------------------------------------

IF((EMGU_CV_BUILD OR EMGU_CV_EXAMPLE_BUILD) AND NOT ANDROID AND NOT NETFX_CORE)

PROJECT(Emgu.CV.Test)

FILE(GLOB_RECURSE SRC_CV_TEST RELATIVE ${CMAKE_CURRENT_SOURCE_DIR} "*.cs")
LIST(APPEND SRC_CV_TEST ${CMAKE_CURRENT_SOURCE_DIR}/../CommonAssemblyInfo.cs)

#Remove android or ios specific files
FILE(GLOB_RECURSE SRC_CV_TEST_EXCLUDES RELATIVE ${CMAKE_CURRENT_SOURCE_DIR} "Class1.cs")
LIST(REMOVE_ITEM SRC_CV_TEST ${SRC_CV_TEST_EXCLUDES})

#copy the test data for this example
FILE(GLOB_RECURSE SRC_CV_TEST_DATA RELATIVE ${CMAKE_CURRENT_SOURCE_DIR} "*.jpg" "*.png" "*.gif" "*.data" "*.xml" "*.avi")
LIST(APPEND
	SRC_CV_TEST_DATA 
	${CMAKE_CURRENT_SOURCE_DIR}/../opencv/data/haarcascades/haarcascade_frontalface_alt2.xml
	${CMAKE_CURRENT_SOURCE_DIR}/../opencv/data/haarcascades/haarcascade_eye.xml
	${CMAKE_CURRENT_SOURCE_DIR}/../opencv/samples/cpp/left01.jpg
	${CMAKE_CURRENT_SOURCE_DIR}/../opencv/samples/c/airplane.jpg
	${CMAKE_CURRENT_SOURCE_DIR}/../opencv/samples/c/box.png
	${CMAKE_CURRENT_SOURCE_DIR}/../opencv/samples/c/box_in_scene.png
    ${CMAKE_CURRENT_SOURCE_DIR}/../opencv/samples/c/lena.jpg
	${CMAKE_CURRENT_SOURCE_DIR}/../opencv/samples/c/stuff.jpg
	${CMAKE_CURRENT_SOURCE_DIR}/../opencv/samples/c/scene_l.bmp
	${CMAKE_CURRENT_SOURCE_DIR}/../opencv/samples/c/scene_r.bmp
	${CMAKE_CURRENT_SOURCE_DIR}/../opencv/samples/c/tree.avi
	${CMAKE_CURRENT_SOURCE_DIR}/../opencv/samples/c/agaricus-lepiota.data
	${CMAKE_CURRENT_SOURCE_DIR}/../opencv/samples/cpp/letter-recognition.data
	${CMAKE_CURRENT_SOURCE_DIR}/../lib/nunit.framework.dll
	)
FOREACH(FILE ${SRC_CV_TEST_DATA})
	ADD_CS_FILE_TO_DEPLOY(${FILE})
ENDFOREACH()

ADD_CS_REFERENCES("${LIBRARY_OUTPUT_PATH}/Emgu.CV.dll;${LIBRARY_OUTPUT_PATH}/Emgu.Util.dll;${LIBRARY_OUTPUT_PATH}/Emgu.CV.ML.dll;${LIBRARY_OUTPUT_PATH}/Emgu.CV.UI.dll;${LIBRARY_OUTPUT_PATH}/Emgu.CV.OCR.dll;${LIBRARY_OUTPUT_PATH}/Emgu.CV.GPU.dll;System.Drawing.dll;${LIBRARY_OUTPUT_PATH}/Emgu.UI.dll;System.Windows.Forms.dll;System.Core.dll;${CMAKE_CURRENT_SOURCE_DIR}/../lib/nunit.framework.dll")

COMPILE_CS(${PROJECT_NAME} "library" "${SRC_CV_TEST}")

if(ENABLE_SOLUTION_FOLDERS)
    set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "emgu")
endif()

ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.UI Emgu.CV.ML Emgu.UI Emgu.CV.OCR Emgu.CV.GPU)

ENDIF()
