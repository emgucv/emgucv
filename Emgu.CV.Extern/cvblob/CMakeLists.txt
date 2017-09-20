# --------------------------------------------------------
#  Copyright (C) 2004-2017 by EMGU Corporation. All rights reserved.
# 
#  CMake file for cvextern. See root CMakeLists.txt
#
# ----------------------------------------------------------------------------
project(cvblob)

INCLUDE_DIRECTORIES("${PROJECT_SOURCE_DIR}/../../opencv/include/opencv")
INCLUDE_DIRECTORIES("${PROJECT_SOURCE_DIR}/../../opencv/include")

SET(OPENCV_MODULES_DIR "${PROJECT_SOURCE_DIR}/../../opencv/modules")
INCLUDE_DIRECTORIES("${OPENCV_MODULES_DIR}/core/include")
INCLUDE_DIRECTORIES("${OPENCV_MODULES_DIR}/imgproc/include")
INCLUDE_DIRECTORIES("${OPENCV_MODULES_DIR}/ml/include")
INCLUDE_DIRECTORIES("${OPENCV_MODULES_DIR}/calib3d/include")
INCLUDE_DIRECTORIES("${OPENCV_MODULES_DIR}/objdetect/include")
INCLUDE_DIRECTORIES("${OPENCV_MODULES_DIR}/features2d/include")
INCLUDE_DIRECTORIES("${OPENCV_MODULES_DIR}/imgcodecs/include")
INCLUDE_DIRECTORIES("${OPENCV_MODULES_DIR}/videoio/include")
if (HAVE_opencv_highgui)
  INCLUDE_DIRECTORIES("${OPENCV_MODULES_DIR}/highgui/include")
ENDIF()
INCLUDE_DIRECTORIES("${OPENCV_MODULES_DIR}/flann/include")
INCLUDE_DIRECTORIES("${OPENCV_MODULES_DIR}/video/include")

############################### CVBLOB START ########################
file(GLOB CVBLOB_SRCS "${PROJECT_SOURCE_DIR}/libcvblob/cvBlob/*.cpp")
file(GLOB CVBLOB_HDRS "${PROJECT_SOURCE_DIR}/libcvblob/cvBlob/*.h*")

#LIST(APPEND cvblob_srcs ${CVBLOB_SRCS})
#LIST(APPEND cvblob_hdrs ${CVBLOB_HDRS})
############################### CVBLOB END ##########################

source_group("Src" FILES ${CVBLOB_SRCS})
source_group("Include" FILES ${CVBLOB_HDRS})
set(the_target ${PROJECT_NAME})

IF(ENABLE_WINRT_MODE)
  SET(CMAKE_C_FLAGS "${CMAKE_C_FLAGS} /wd4703")
  SET(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} /wd4703")
  add_definitions(-D_CRT_SECURE_NO_WARNINGS)
ENDIF()

add_library(${the_target} STATIC ${CVBLOB_SRCS} ${CVBLOB_HDRS})

if (NOT WIN32)
  set_target_properties(${the_target} PROPERTIES COMPILE_FLAGS "-fPIC")
ENDIF()

INSTALL(
  FILES
  "${CMAKE_CURRENT_SOURCE_DIR}/lgpl-3.0.txt"
  DESTINATION "lib/3rdParty"
  COMPONENT emgucv_source
  RENAME "cvblob.license.txt"
  )

# For dynamic link numbering convenions
set_target_properties(${the_target} PROPERTIES
    OUTPUT_NAME "${the_target}"
    )

IF(DEFINED EMGUCV_PLATFORM_TOOLSET)
  set_target_properties(${the_target} PROPERTIES PLATFORM_TOOLSET ${EMGUCV_PLATFORM_TOOLSET})
ENDIF()

if(ENABLE_SOLUTION_FOLDERS)
    set_target_properties(${the_target} PROPERTIES FOLDER "3rdparty")
endif() 

ADD_DEPENDENCIES(${the_target} 
  opencv_core opencv_calib3d opencv_features2d opencv_highgui opencv_imgproc opencv_ml opencv_objdetect)

#INSTALL(TARGETS ${the_target} 
#    ARCHIVE DESTINATION lib${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER} COMPONENT main)



