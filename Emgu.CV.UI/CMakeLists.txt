PROJECT(Emgu.CV.UI)

SET(SRC_CV_UI *.cs Properties/*.cs )

#IF(WIN32)
#  SET(CS_FLAGS ${CS_FLAGS} -addmodule:${LIBRARY_OUTPUT_PATH}/Emgu.Util.netmodule -addmodule:${LIBRARY_OUTPUT_PATH}/Emgu.CV.netmodule -r:System.Drawing.dll -r:System.Data.dll -r:System.Windows.Forms.dll -r:${LIBRARY_3RDPARTY_PATH}/ZedGraph.dll)
#  ADD_CS_MODULE(${PROJECT_NAME} "${SRC_CV_UI}" ALL)
#ELSE(WIN32)
  SET(CS_FLAGS ${CS_FLAGS} -r:${LIBRARY_OUTPUT_PATH}/Emgu.Util.dll -r:${LIBRARY_OUTPUT_PATH}/Emgu.CV.dll -r:System.Drawing.dll -r:System.Data.dll -r:System.Windows.Forms.dll -r:${LIBRARY_3RDPARTY_PATH}/ZedGraph.dll)
  ADD_CS_LIBRARY(${PROJECT_NAME} "${SRC_CV_UI}" ALL)
#ENDIF(WIN32)

ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV Emgu.Util)
INSTALL(FILES ${LIBRARY_OUTPUT_PATH}/${PROJECT_NAME}.dll DESTINATION bin
COMPONENT binary)
