IF(EMGU_CV_WITH_TESSERACT)
PROJECT(Example.LicensePlateRecognition)

FILE(GLOB_RECURSE SRC_Example_LicensePlateRecognition RELATIVE ${CMAKE_CURRENT_SOURCE_DIR} "*.cs")
LIST(APPEND SRC_Example_LicensePlateRecognition ${CMAKE_CURRENT_SOURCE_DIR}/../../CommonAssemblyInfo.cs)

IF(TARGET_ARCH_64)
  LIST(APPEND CS_FLAGS -platform:x64)
ELSE()
  LIST(APPEND CS_FLAGS -platform:x86)
ENDIF()

LIST(APPEND CS_FLAGS -r:"${LIBRARY_OUTPUT_PATH}/Emgu.CV.OCR.dll")
LIST(APPEND CS_FLAGS ${EMGU_CV_REFERENCE})

#copy the test image for this example
ADD_CS_FILE_TO_DEPLOY(${CMAKE_CURRENT_SOURCE_DIR}/license-plate.jpg)

#Make sure the destination folder exist
GET_CS_LIBRARY_TARGET_DIR()
LIST(APPEND
  CS_PREBUILD_COMMAND  
  COMMAND ${CMAKE_COMMAND} -E make_directory "${COMPILE_CS_TARGET_DIR}/tessdata"
  )
ADD_CS_FILE_TO_DEPLOY(${CMAKE_CURRENT_SOURCE_DIR}/tessdata/eng.traineddata tessdata)

COMPILE_CS(${PROJECT_NAME} "winexe" "${SRC_Example_LicensePlateRecognition}" ALL)

ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV Emgu.CV.UI Emgu.CV.OCR)

INSTALL(FILES 
  "${LIBRARY_OUTPUT_PATH}/${PROJECT_NAME}.${CS_EXECUTABLE_EXTENSION}"
  "${CMAKE_CURRENT_SOURCE_DIR}/license-plate.jpg"
  "${CMAKE_CURRENT_SOURCE_DIR}/tessdata/eng.traineddata"
  DESTINATION bin COMPONENT emgucv_example_binary)
ENDIF()