# ----------------------------------------------------------------------------
#  CMake file for cvextern. See root CMakeLists.txt
#
# ----------------------------------------------------------------------------
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
INCLUDE_DIRECTORIES("${PROJECT_SOURCE_DIR}/../../opencv/include/opencv")
INCLUDE_DIRECTORIES("${PROJECT_SOURCE_DIR}/../../Emgu.CV.Extern/")

#INCLUDE_DIRECTORIES(${TIFF_INCLUDE_DIR})
#INCLUDE_DIRECTORIES(${ZLIB_INCLUDE_DIR})
#INCLUDE_DIRECTORIES(${GEOTIFF_INCLUDE_DIR})

############################### IPP START ##############################
IF (USE_IPP AND IPP_PATH)
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

############################### TESSERACT START ########################
IF(WIN32 AND NOT CMAKE_CL_64)
  LINK_DIRECTORIES("${PROJECT_SOURCE_DIR}/../../Emgu.CV.Extern/tesseract/lib/x86")
  #SET(TESSERACT_LIBRARY ccmain ccstruct ccutil classify cube cutil dict image neural_networks tesseract textord viewer wordrec leptonlib Ws2_32)
ENDIF()
############################### TESSERACT END ##########################

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
