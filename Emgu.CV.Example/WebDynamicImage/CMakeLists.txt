# --------------------------------------------------------
#  Copyright (C) 2004-2012 by EMGU. All rights reserved.
# --------------------------------------------------------

PROJECT(Example.WebDynamicImage)

SET(WEB_DYNAMIC_IMAGE_BIN_FOLDER "${CMAKE_CURRENT_SOURCE_DIR}/Bin${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER}")

SET(TMP opencv_core opencv_imgproc opencv_highgui opencv_objdetect opencv_ml opencv_calib3d opencv_contrib opencv_features2d opencv_legacy opencv_video opencv_flann cvextern)
FOREACH(tmp_component ${TMP})
   ###################################################################
   # Work around for GET_TARGET_PROPERTY LOCATION bug
   ###################################################################
   GET_TARGET_PROPERTY(TMP_LOCATION ${tmp_component} LOCATION)
   GET_FILENAME_COMPONENT(TMP_LOCATION ${TMP_LOCATION} NAME)
   LIST(APPEND WEB_DYNAMIC_IMAGE_COPY_COMMAND 
     COMMAND ${CMAKE_COMMAND} -E copy "${UNMANAGED_LIBRARY_OUTPUT_PATH}/${TMP_LOCATION}" "${WEB_DYNAMIC_IMAGE_BIN_FOLDER}")
ENDFOREACH()

###################################################################
# Create the Bin folder if it is not already exists
###################################################################
IF(NOT(EXISTS ${WEB_DYNAMIC_IMAGE_BIN_FOLDER} AND IS_DIRECTORY ${WEB_DYNAMIC_IMAGE_BIN_FOLDER}))
   FILE(MAKE_DIRECTORY ${WEB_DYNAMIC_IMAGE_BIN_FOLDER})
ENDIF()
#MESSAGE("${WEB_DYNAMIC_IMAGE_COPY_COMMAND}")
ADD_CUSTOM_TARGET(
   ${PROJECT_NAME} ALL
   ${WEB_DYNAMIC_IMAGE_COPY_COMMAND}
)
set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "Emgu.CV.Examples")
ADD_DEPENDENCIES(${PROJECT_NAME} Emgu.CV Emgu.CV.UI ${TMP})


