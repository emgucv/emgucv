IF(EMGU_CV_BUILD OR EMGU_CV_EXAMPLE_BUILD)

PROJECT(Emgu.CV.Test)

SET(SRC_CV_TEST 
  *.cs 
  Properties/*.cs)

ADD_CS_REFERENCES("${LIBRARY_OUTPUT_PATH}/Emgu.CV.dll;${LIBRARY_OUTPUT_PATH}/Emgu.Util.dll;${LIBRARY_OUTPUT_PATH}/Emgu.CV.ML.dll;${LIBRARY_OUTPUT_PATH}/Emgu.CV.UI.dll;System.Drawing.dll;${LIBRARY_OUTPUT_PATH}/Emgu.UI.dll;System.Windows.Forms.dll;nunit.framework.dll")

ADD_CS_LIBRARY(${PROJECT_NAME} "${SRC_CV_TEST}")

ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV.UI Emgu.CV.ML Emgu.UI)

ENDIF()

