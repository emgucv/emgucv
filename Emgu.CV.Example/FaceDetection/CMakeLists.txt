# --------------------------------------------------------
#  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

PROJECT(Example.FaceDetection)

FILE(GLOB_RECURSE SRC_Example_FaceDetection RELATIVE ${CMAKE_CURRENT_SOURCE_DIR} "*.cs")
LIST(APPEND SRC_Example_FaceDetection ${CMAKE_CURRENT_SOURCE_DIR}/../../CommonAssemblyInfo.cs)

#IF(TARGET_ARCH_64)
#  LIST(APPEND CS_FLAGS -platform:x64)
#ELSE()
#  LIST(APPEND CS_FLAGS -platform:x86)
#ENDIF()

LIST(APPEND CS_FLAGS ${EMGU_CV_REFERENCE} ${EMGU_CV_CUDA_REFERENCE})

#copy the test image for this example
SET(EXAMPLE_FACE_DETECTION_DATA_FILES
  "${CMAKE_CURRENT_SOURCE_DIR}/lena.jpg"
  "${OPENCV_SUBFOLDER}/data/haarcascades/haarcascade_eye.xml"
  "${OPENCV_SUBFOLDER}/data/haarcascades/haarcascade_frontalface_default.xml")
 
FOREACH(EXAMPLE_FACE_DETECTION_DATA_FILE ${EXAMPLE_FACE_DETECTION_DATA_FILES})
ADD_CS_FILE_TO_DEPLOY(${EXAMPLE_FACE_DETECTION_DATA_FILE})
ENDFOREACH()

COMPILE_CS(${PROJECT_NAME} "winexe" "${SRC_Example_FaceDetection}" ALL)
set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "Emgu.CV.Examples")
ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.World Emgu.CV.UI)

INSTALL(FILES 
${LIBRARY_OUTPUT_PATH}/${PROJECT_NAME}.${CS_EXECUTABLE_EXTENSION} 
${EXAMPLE_FACE_DETECTION_DATA_FILES}
DESTINATION bin COMPONENT emgucv_example_binary)
