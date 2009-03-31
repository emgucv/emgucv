PROJECT(Emgu.CV.UI)

SET(SRC_CV_UI *.cs Properties/*.cs)

GENERATE_DOCUMENT(${LIBRARY_OUTPUT_PATH}/${PROJECT_NAME})
SET(CS_FLAGS ${CS_FLAGS} -r:${LIBRARY_OUTPUT_PATH}/Emgu.Util.dll -r:${LIBRARY_OUTPUT_PATH}/Emgu.CV.dll -r:System.Drawing.dll -r:System.Data.dll -r:System.Windows.Forms.dll -r:${LIBRARY_3RDPARTY_PATH}/ZedGraph.dll)
SET(CS_FLAGS ${CS_FLAGS} -resource:${PROJECT_NAME}.Properties.StringTable.resources)
SIGN_ASSEMBLY(CV.UI.snk)
ADD_CS_LIBRARY(${PROJECT_NAME} "${SRC_CV_UI}" ALL)

ADD_CUSTOM_COMMAND(
    TARGET ${PROJECT_NAME}
    PRE_BUILD 
    COMMAND ${RESGEN_EXECUTABLE} -usesourcepath Properties/StringTable.resx ${PROJECT_NAME}.Properties.StringTable.resources
)

ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV Emgu.Util)
INSTALL(FILES ${LIBRARY_OUTPUT_PATH}/${PROJECT_NAME}.dll DESTINATION bin
COMPONENT binary)
