PROJECT(Emgu.CV.ML)

SET(SRC_CV_ML *.cs PInvoke/*.cs PInvoke/CvType/*.cs)
SET(SRC_CV_ML ${SRC_CV_ML} Properties/*.cs)

GENERATE_DOCUMENT(${LIBRARY_OUTPUT_PATH}/${PROJECT_NAME})

ADD_CS_REFERENCES("${LIBRARY_OUTPUT_PATH}/Emgu.CV.dll;${LIBRARY_OUTPUT_PATH}/Emgu.Util.dll;System.Drawing.dll")

SIGN_ASSEMBLY(ML.snk)

#FOR Linux, copy the dll mapping file such that mono can find the unmanaged libraries
IF (WIN32)
ELSE (WIN32)
  ADD_CS_FILE_TO_DEPLOY(${PROJECT_NAME}.dll.config)
ENDIF(WIN32)

ADD_CS_LIBRARY(${PROJECT_NAME} "${SRC_CV_ML}")

ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV)
INSTALL(FILES ${LIBRARY_OUTPUT_PATH}/${PROJECT_NAME}.dll ${LIBRARY_OUTPUT_PATH}/${PROJECT_NAME}.XML DESTINATION bin
COMPONENT binary)

