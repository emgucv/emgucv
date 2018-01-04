# --------------------------------------------------------
#  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
# --------------------------------------------------------

IF((EMGU_CV_BUILD OR EMGU_CV_EXAMPLE_BUILD) AND NOT ANDROID AND NOT NETFX_CORE)
  
  PROJECT(Emgu.UI)
  
  FILE(GLOB_RECURSE SRC_EMGU_UI RELATIVE ${CMAKE_CURRENT_SOURCE_DIR} "*.cs")
  LIST(APPEND SRC_EMGU_UI ${CMAKE_CURRENT_SOURCE_DIR}/../CommonAssemblyInfo.cs)
  
  GENERATE_DOCUMENT(${LIBRARY_OUTPUT_PATH}/${PROJECT_NAME})
  SIGN_ASSEMBLY(${CMAKE_CURRENT_SOURCE_DIR}/../Emgu.CV.snk)
  ADD_CS_REFERENCES("System.Drawing.dll;System.Windows.Forms.dll")
  COMPILE_CS(${PROJECT_NAME} "library" "${SRC_EMGU_UI}")
  
  if(ENABLE_SOLUTION_FOLDERS)
    set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "emgu")
  endif()

ENDIF()
