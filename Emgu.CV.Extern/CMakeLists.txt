# ----------------------------------------------------------------------------
#  CMake file for cvextern. See root CMakeLists.txt
#
# ----------------------------------------------------------------------------
project(cvextern)

#INCLUDE_DIRECTORIES("${PROJECT_SOURCE_DIR}/../opencv/modules/core/include/opencv2/core")
INCLUDE_DIRECTORIES("${PROJECT_SOURCE_DIR}/../opencv/modules/core/include")
INCLUDE_DIRECTORIES("${PROJECT_SOURCE_DIR}/../opencv/modules/imgproc/include")
INCLUDE_DIRECTORIES("${PROJECT_SOURCE_DIR}/../opencv/modules/video/include")
INCLUDE_DIRECTORIES("${PROJECT_SOURCE_DIR}/../opencv/modules/ml/include")
INCLUDE_DIRECTORIES("${PROJECT_SOURCE_DIR}/../opencv/modules/calib3d/include")
INCLUDE_DIRECTORIES("${PROJECT_SOURCE_DIR}/../opencv/modules/objdetect/include")
INCLUDE_DIRECTORIES("${PROJECT_SOURCE_DIR}/../opencv/modules/features2d/include")
INCLUDE_DIRECTORIES("${PROJECT_SOURCE_DIR}/../opencv/modules/legacy/include")
INCLUDE_DIRECTORIES("${PROJECT_SOURCE_DIR}/../opencv/modules/contrib/include")
INCLUDE_DIRECTORIES("${PROJECT_SOURCE_DIR}/../opencv/include/opencv")
file(GLOB extern_srcs "./*.cpp")
source_group("Src" FILES ${extern_srcs})
file(GLOB extern_hdrs "./*.h*")
source_group("Include" FILES ${extern_hdrs})

set(the_target ${PROJECT_NAME})

add_library(${the_target} ${extern_srcs} ${extern_hdrs})

# For dynamic link numbering convenions
set_target_properties(${the_target} PROPERTIES
    OUTPUT_NAME "${the_target}"
    )

# Additional target properties
set_target_properties(${the_target} PROPERTIES
	DEBUG_POSTFIX "${OPENCV_DEBUG_POSTFIX}"
	ARCHIVE_OUTPUT_DIRECTORY "${CMAKE_BINARY_DIR}/lib/"
	RUNTIME_OUTPUT_DIRECTORY "${CMAKE_BINARY_DIR}/bin/"
	DEFINE_SYMBOL "CVAPI_EXPORTS"
	)

add_dependencies(${the_target} opencv_core opencv_calib3d opencv_contrib opencv_features2d opencv_highgui opencv_imgproc opencv_legacy opencv_ml opencv_objdetect)

IF(EMGU_ENABLE_SSE)
  ADD_DEFINITIONS(-D__EMGU_ENABLE_SSE__)
  IF(MSVC AND EMGUCV_ARCH MATCHES "win32") 
	SET(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} /arch:SSE2")        ## Optimization
  ENDIF()
ENDIF()

# Add the required libraries for linking:
target_link_libraries(${the_target} ${OPENCV_LINKER_LIBS} opencv_core opencv_calib3d opencv_contrib opencv_features2d opencv_highgui opencv_imgproc opencv_legacy opencv_ml opencv_objdetect)

INSTALL(TARGETS ${the_target} 
    RUNTIME DESTINATION bin COMPONENT main
    LIBRARY DESTINATION lib COMPONENT main
    ARCHIVE DESTINATION lib COMPONENT main)
  
