PROJECT(Emgu.CV.ML)

SET(SRC_CV_ML *.cs PInvoke/*.cs PInvoke/CvType/*.cs)
SET(SRC_CV_ML ${SRC_CV_ML} Properties/*.cs)

#IF(WIN32)
#  SET(CS_FLAGS ${CS_FLAGS} -doc:${LIBRARY_OUTPUT_PATH}/${PROJECT_NAME}.XML -addmodule:${LIBRARY_OUTPUT_PATH}/Emgu.CV.netmodule -addmodule:${LIBRARY_OUTPUT_PATH}/Emgu.Util.netmodule -r:System.Drawing.dll)
#  ADD_CS_MODULE(${PROJECT_NAME} "${SRC_CV_ML}" ALL)
#ELSE(WIN32)
  SET(CS_FLAGS ${CS_FLAGS} -doc:${LIBRARY_OUTPUT_PATH}/${PROJECT_NAME}.XML -r:${LIBRARY_OUTPUT_PATH}/Emgu.CV.dll -r:${LIBRARY_OUTPUT_PATH}/Emgu.Util.dll -r:System.Drawing.dll)
  ADD_CS_LIBRARY(${PROJECT_NAME} "${SRC_CV_ML}")
#ENDIF(WIN32)

#IF (WIN32)
#SET(LINK_EXECUTABLE "C:/Program Files/Microsoft Visual Studio 8/VC/bin/link.exe")
#ADD_CUSTOM_COMMAND(
#    TARGET ${PROJECT_NAME}
#    POST_BUILD
#    COMMAND ${LINK_EXECUTABLE} -LTCG -DLL ${LIBRARY_OUTPUT_PATH}/Emgu.Util.netmodule ${LIBRARY_OUTPUT_PATH}/Emgu.CV.ML.netmodule ${LIBRARY_OUTPUT_PATH}/Emgu.CV.netmodule -assemblyresource:${LIBRARY_OUTPUT_PATH}/cvextern.dll -out:${LIBRARY_OUTPUT_PATH}/Emgu.CV.Bundle.dll
#)
#ENDIF()

ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV)
INSTALL(FILES ${LIBRARY_OUTPUT_PATH}/${PROJECT_NAME}.dll ${LIBRARY_OUTPUT_PATH}/${PROJECT_NAME}.XML DESTINATION bin
COMPONENT binary)

