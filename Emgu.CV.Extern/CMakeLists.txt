# ----------------------------------------------------------------------------
#  CMake file for cvextern. See root CMakeLists.txt
#
# ----------------------------------------------------------------------------
project(cvextern)

#INCLUDE_DIRECTORIES("${PROJECT_SOURCE_DIR}/../opencv/modules/core/include/opencv2/core")
#INCLUDE_DIRECTORIES("${PROJECT_SOURCE_DIR}/../opencv/3rdparty/include")
INCLUDE_DIRECTORIES("${PROJECT_SOURCE_DIR}/../opencv/modules/core/include")
INCLUDE_DIRECTORIES("${PROJECT_SOURCE_DIR}/../opencv/modules/imgproc/include")
INCLUDE_DIRECTORIES("${PROJECT_SOURCE_DIR}/../opencv/modules/video/include")
INCLUDE_DIRECTORIES("${PROJECT_SOURCE_DIR}/../opencv/modules/ml/include")
INCLUDE_DIRECTORIES("${PROJECT_SOURCE_DIR}/../opencv/modules/calib3d/include")
INCLUDE_DIRECTORIES("${PROJECT_SOURCE_DIR}/../opencv/modules/objdetect/include")
INCLUDE_DIRECTORIES("${PROJECT_SOURCE_DIR}/../opencv/modules/features2d/include")
INCLUDE_DIRECTORIES("${PROJECT_SOURCE_DIR}/../opencv/modules/legacy/include")
INCLUDE_DIRECTORIES("${PROJECT_SOURCE_DIR}/../opencv/modules/contrib/include")
INCLUDE_DIRECTORIES("${PROJECT_SOURCE_DIR}/../opencv/modules/flann/include")
INCLUDE_DIRECTORIES("${PROJECT_SOURCE_DIR}/../opencv/modules/gpu/include")
INCLUDE_DIRECTORIES("${PROJECT_SOURCE_DIR}/../opencv/include/opencv")

############################### IPP START ##############################
IF (USE_IPP AND IPP_PATH)
  message(STATUS "CVEXTERN: USING IPP: ${IPP_PATH} ")
  add_definitions(-DHAVE_IPP)
  include_directories("${IPP_PATH}/../include")
  LINK_DIRECTORIES("${IPP_PATH}/../lib")

  IF(WIN32 AND NOT CMAKE_CL_64)
	link_directories(
	"${IPP_PATH}/../../../ipp/lib/ia32"
	"${IPP_PATH}/../../../compiler/lib/ia32")
  ELSEIF(WIN32 AND CMAKE_CL_64)
	link_directories(
	"${IPP_PATH}/../../../ipp/lib/intel64"
	"${IPP_PATH}/../../../compiler/lib/intel64")
  ENDIF()
ENDIF()
############################### IPP END ################################

############################### TBB START ##############################
if (WITH_TBB AND TBB_INCLUDE_DIR AND TBB_LIB_DIR)
  MESSAGE(STATUS "CVEXTERN: using TBB")
  add_definitions(-DHAVE_TBB)
  include_directories(${TBB_INCLUDE_DIR})
  link_directories(${TBB_LIB_DIR})
endif()
############################### TBB END ################################

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

FIND_PACKAGE(TIFF)
IF(NOT TIFF_FOUND)
  ADD_DEPENDENCIES(${the_target} libtiff)
  SET(TIFF_INCLUDE_DIR "${PROJECT_SOURCE_DIR}/../opencv/3rdparty/include")
  SET(TIFF_LIBRARY "libtiff")
ENDIF()
INCLUDE_DIRECTORIES(${TIFF_INCLUDE_DIR})

FIND_PACKAGE(ZLIB)
IF(ZLIB_FOUND)
  MESSAGE(STATUS "CVEXTERN: ZLIB found.")
ENDIF()
IF(NOT ZLIB_FOUND)
  MESSAGE(STATUS "CVEXTERN: ZLIB not found, building from source")
  ADD_DEPENDENCIES(${the_target} zlib)
  SET(ZLIB_INCLUDE_DIR "${PROJECT_SOURCE_DIR}/../opencv/3rdparty/include")
  SET(ZLIB_LIBRARY "zlib")
ENDIF()
INCLUDE_DIRECTORIES(${ZLIB_INCLUDE_DIR})

SET(CMAKE_MODULE_PATH "${PROJECT_SOURCE_DIR}/libgeotiff/cmake")
FIND_PACKAGE(GeoTIFF)
IF(GEOTIFF_FOUND)
  MESSAGE(STATUS "CVEXTERN: GEOTIFF found. INCLUDE DIR: ${GEOTIFF_INCLUDE_DIR}")
ENDIF()
IF (NOT GEOTIFF_FOUND)
  MESSAGE(STATUS "CVEXTERN: GEOTIFF not found. Building from source")
  # ----------------------------------------------------------------------------
  #  Build libgeotiff 
  # ----------------------------------------------------------------------------
  ADD_SUBDIRECTORY(libgeotiff/libgeotiff-1.3.0)
  SET(GEOTIFF_INCLUDE_DIR "${PROJECT_SOURCE_DIR}/libgeotiff/libgeotiff-1.3.0" "${PROJECT_SOURCE_DIR}/libgeotiff/libgeotiff-1.3.0/libxtiff")
  SET(GEOTIFF_LIBRARY geotiff_archive xtiff)
ENDIF()
INCLUDE_DIRECTORIES(${GEOTIFF_INCLUDE_DIR})

ADD_DEPENDENCIES(${the_target} 
  opencv_core opencv_calib3d opencv_contrib opencv_features2d opencv_highgui opencv_imgproc opencv_legacy opencv_ml opencv_objdetect opencv_gpu
  ${ZLIB_LIBRARY} 
#  xtiff
  ${TIFF_LIBRARY} ${GEOTIFF_LIBRARY})

IF(EMGU_ENABLE_SSE)
  ADD_DEFINITIONS(-D__EMGU_ENABLE_SSE__)
  IF(MSVC AND EMGUCV_ARCH MATCHES "win32" AND (NOT CV_ICC)) 
	SET(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} /arch:SSE2")        ## Optimization
  ENDIF()
ENDIF()

# Add the required libraries for linking:
target_link_libraries(${the_target} ${OPENCV_LINKER_LIBS} 
  opencv_core opencv_calib3d opencv_contrib opencv_features2d opencv_highgui opencv_imgproc opencv_legacy opencv_ml opencv_objdetect opencv_gpu
  ${ZLIB_LIBRARY} 
#  xtiff 
  ${TIFF_LIBRARY} ${GEOTIFF_LIBRARY})

INSTALL(TARGETS ${the_target} 
    RUNTIME DESTINATION bin COMPONENT main
    LIBRARY DESTINATION lib COMPONENT main
    ARCHIVE DESTINATION lib COMPONENT main)

