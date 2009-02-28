PROJECT(Emgu.CV.UI)

SET(CS_FLAGS -r:${LIBRARY_OUTPUT_PATH}/Emgu.CV.dll
-r:${LIBRARY_OUTPUT_PATH}/Emgu.Util.dll -r:System.Drawing.dll -r:System.Data.dll
-r:System.Windows.Forms.dll -r:${LIBRARY_3RDPARTY_PATH}/ZedGraph.dll)
SET(SRC_CV_UI *.cs Properties/*.cs )
ADD_CS_LIBRARY(Emgu.CV.UI "${SRC_CV_UI}" ALL)

ADD_DEPENDENCIES(Emgu.CV.UI Emgu.CV)
INSTALL(FILES ${LIBRARY_OUTPUT_PATH}/Emgu.CV.UI.dll DESTINATION bin
COMPONENT binary)
