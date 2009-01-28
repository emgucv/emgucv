PROJECT(Emgu.CV.UI)

SET(CS_FLAGS -r:${LIBRARY_OUTPUT_PATH}/Emgu.CV
-r:${LIBRARY_OUTPUT_PATH}/Emgu.Util -r:System.Drawing -r:System.Data
-r:System.Windows.Forms -r:${LIBRARY_3RDPARTY_PATH}/ZedGraph.dll)
SET(SRC_CV_UI *.cs Properties/*.cs )
ADD_CS_LIBRARY(Emgu.CV.UI "${SRC_CV_UI}" ALL)

ADD_DEPENDENCIES(Emgu.CV.UI Emgu.CV)
INSTALL(FILES ${LIBRARY_OUTPUT_PATH}/Emgu.CV.UI.dll DESTINATION bin
COMPONENT binary)
