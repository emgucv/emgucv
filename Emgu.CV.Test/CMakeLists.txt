# --------------------------------------------------------
#  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF((EMGU_CV_BUILD OR EMGU_CV_EXAMPLE_BUILD) AND NOT ANDROID AND NOT NETFX_CORE)

PROJECT(Emgu.CV.Test)

FILE(GLOB_RECURSE SRC_CV_TEST RELATIVE ${CMAKE_CURRENT_SOURCE_DIR} "*.cs")
LIST(APPEND SRC_CV_TEST ${CMAKE_CURRENT_SOURCE_DIR}/../CommonAssemblyInfo.cs)

#Remove android or ios specific files
FILE(GLOB_RECURSE SRC_CV_TEST_EXCLUDES RELATIVE ${CMAKE_CURRENT_SOURCE_DIR} 
"Class1.cs" 
"Emgu.CV.Test.Android/*.cs" "Emgu.CV.Test.Android/Resources/*.cs" "Emgu.CV.Test.Android/Properties/*.cs" 
"Emgu.CV.Test.iOS/*.cs"
"Emgu.CV.Test.Windows.Phone/Properties/*.cs" 
"Emgu.CV.Test.Windows.Store/Properties/*.cs")
LIST(REMOVE_ITEM SRC_CV_TEST ${SRC_CV_TEST_EXCLUDES})

#copy the test data for this example
FILE(GLOB_RECURSE SRC_CV_TEST_DATA RELATIVE ${CMAKE_CURRENT_SOURCE_DIR} "*.jpg" "*.png" "*.gif" "*.data" "*.bmp" "*.xml" "*.avi")
LIST(APPEND
	SRC_CV_TEST_DATA 
	${CMAKE_CURRENT_SOURCE_DIR}/../opencv/data/haarcascades/haarcascade_frontalface_alt2.xml
	${CMAKE_CURRENT_SOURCE_DIR}/../opencv/data/haarcascades/haarcascade_eye.xml
	${CMAKE_CURRENT_SOURCE_DIR}/../opencv/samples/data/left01.jpg
	${CMAKE_CURRENT_SOURCE_DIR}/../opencv/samples/data/board.jpg
	${CMAKE_CURRENT_SOURCE_DIR}/../opencv/samples/data/box.png
	${CMAKE_CURRENT_SOURCE_DIR}/../opencv/samples/data/box_in_scene.png
    ${CMAKE_CURRENT_SOURCE_DIR}/../opencv/samples/data/lena.jpg
	${CMAKE_CURRENT_SOURCE_DIR}/../opencv/samples/data/stuff.jpg
	${CMAKE_CURRENT_SOURCE_DIR}/../opencv/samples/data/aloeL.jpg
	${CMAKE_CURRENT_SOURCE_DIR}/../opencv/samples/data/aloeR.jpg
	${CMAKE_CURRENT_SOURCE_DIR}/../opencv/samples/data/tree.avi
	${CMAKE_CURRENT_SOURCE_DIR}/../opencv_contrib/modules/text/samples/scenetext01.jpg
	${CMAKE_CURRENT_SOURCE_DIR}/../opencv/samples/data/letter-recognition.data
	${CMAKE_CURRENT_SOURCE_DIR}/../lib/nunit.framework.dll
	)
FOREACH(FILE ${SRC_CV_TEST_DATA})
	ADD_CS_FILE_TO_DEPLOY(${FILE})
ENDFOREACH()

SET_CS_TARGET_FRAMEWORK(${CSC_TARGET_FRAMEWORK})
ADD_CS_REFERENCES("${LIBRARY_OUTPUT_PATH}/Emgu.CV.World.dll;${LIBRARY_OUTPUT_PATH}/Emgu.CV.UI.dll;${LIBRARY_OUTPUT_PATH}/Emgu.UI.dll;${LIBRARY_OUTPUT_PATH}/Emgu.CV.UI.GL.dll;${CMAKE_CURRENT_SOURCE_DIR}/../lib/nunit.framework.dll")
ADD_CS_FRAMEWORK_REFERENCES("${CSC_TARGET_FRAMEWORK}" "System.Windows.Forms.dll")
COMPILE_CS(${PROJECT_NAME} "library" "${SRC_CV_TEST}")

if(ENABLE_SOLUTION_FOLDERS)
    set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "emgu")
endif()

ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.UI Emgu.CV.World Emgu.CV.UI.GL Emgu.UI)

ENDIF()
