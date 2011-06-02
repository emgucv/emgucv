# --------------------------------------------------------
#  Copyright (C) 2004-2011 by EMGU. All rights reserved.
#
#  CMake file for cvextern_test. See root CMakeLists.txt
# --------------------------------------------------------

project(cvextern_test)

INCLUDE_DIRECTORIES("${PROJECT_SOURCE_DIR}/../../opencv/modules/core/include")
INCLUDE_DIRECTORIES("${PROJECT_SOURCE_DIR}/../../opencv/modules/imgproc/include")
INCLUDE_DIRECTORIES("${PROJECT_SOURCE_DIR}/../../opencv/modules/video/include")
INCLUDE_DIRECTORIES("${PROJECT_SOURCE_DIR}/../../opencv/modules/ml/include")
INCLUDE_DIRECTORIES("${PROJECT_SOURCE_DIR}/../../opencv/modules/calib3d/include")
INCLUDE_DIRECTORIES("${PROJECT_SOURCE_DIR}/../../opencv/modules/objdetect/include")
INCLUDE_DIRECTORIES("${PROJECT_SOURCE_DIR}/../../opencv/modules/features2d/include")
INCLUDE_DIRECTORIES("${PROJECT_SOURCE_DIR}/../../opencv/modules/legacy/include")
INCLUDE_DIRECTORIES("${PROJECT_SOURCE_DIR}/../../opencv/modules/contrib/include")
INCLUDE_DIRECTORIES("${PROJECT_SOURCE_DIR}/../../opencv/modules/flann/include")
INCLUDE_DIRECTORIES("${PROJECT_SOURCE_DIR}/../../opencv/include/opencv")
INCLUDE_DIRECTORIES("${PROJECT_SOURCE_DIR}/../../Emgu.CV.Extern/")

#INCLUDE_DIRECTORIES(${TIFF_INCLUDE_DIR})
#INCLUDE_DIRECTORIES(${ZLIB_INCLUDE_DIR})
#INCLUDE_DIRECTORIES(${GEOTIFF_INCLUDE_DIR})

############################### IPP START ##############################
IF(WITH_IPP)
include(${PROJECT_SOURCE_DIR}/../../opencv/OpenCVFindIPP.cmake)
  IF (IPP_FOUND)
    message(STATUS "CVEXTERN: USING IPP: ${IPP_LIBRARY_DIRS} ")
    add_definitions(-DHAVE_IPP)
    include_directories(${IPP_INCLUDE_DIRS})
    LINK_DIRECTORIES(${IPP_LIBRARY_DIRS})
  ENDIF()
ENDIF()
############################### IPP END ################################

############################### TBB START ##############################
if (WITH_TBB AND TBB_INCLUDE_DIR AND TBB_LIB_DIR)
  add_definitions(-DHAVE_TBB)
  include_directories(${TBB_INCLUDE_DIR})
  link_directories(${TBB_LIB_DIR})
endif()
############################### TBB END ################################

if(CMAKE_COMPILER_IS_GNUCXX)
  set(CMAKE_C_FLAGS "${CMAKE_C_FLAGS} -Wno-unused-function")
endif()

file(GLOB cvextern_test_srcs "./*.cpp")
source_group("Src" FILES ${cvextern_test_srcs})
file(GLOB cvextern_test_hdrs "./*.h*")
source_group("Include" FILES ${cvextern_test_hdrs})

set(the_target ${PROJECT_NAME})

add_executable(${the_target} ${cvextern_test_srcs} ${cvextern_test_hdrs})

# For dynamic link numbering convenions
set_target_properties(${the_target} PROPERTIES
    OUTPUT_NAME "${the_target}"
    )

# Additional target properties
set_target_properties(${the_target} PROPERTIES
	DEBUG_POSTFIX "${OPENCV_DEBUG_POSTFIX}"
	ARCHIVE_OUTPUT_DIRECTORY "${CMAKE_BINARY_DIR}/lib/"
	RUNTIME_OUTPUT_DIRECTORY "${CMAKE_BINARY_DIR}/bin/"
	#DEFINE_SYMBOL "CVAPI_EXPORTS"
	)

ADD_DEPENDENCIES(${the_target} 
  cvextern opencv_core opencv_calib3d opencv_contrib opencv_features2d opencv_highgui opencv_imgproc opencv_legacy opencv_ml opencv_objdetect 
#  ${ZLIB_LIBRARY} 
#  ${TIFF_LIBRARY} ${GEOTIFF_LIBRARY}
)

IF(EMGU_ENABLE_SSE)
  ADD_DEFINITIONS(-D__EMGU_ENABLE_SSE__)
  IF(MSVC AND EMGUCV_ARCH MATCHES "win32" AND (NOT CV_ICC)) 
	SET(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} /arch:SSE2")        ## Optimization
  ENDIF()
ENDIF()

# Add the required libraries for linking:
target_link_libraries(${the_target} ${OPENCV_LINKER_LIBS} 
  cvextern opencv_core opencv_calib3d opencv_contrib opencv_features2d opencv_highgui opencv_imgproc opencv_legacy opencv_ml opencv_objdetect 
#  ${ZLIB_LIBRARY} 
#  ${TIFF_LIBRARY} 
#  ${GEOTIFF_LIBRARY}
)

INSTALL(TARGETS ${the_target} 
    RUNTIME DESTINATION bin COMPONENT main
    LIBRARY DESTINATION lib COMPONENT main
    ARCHIVE DESTINATION lib COMPONENT main)
