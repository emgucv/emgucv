# --------------------------------------------------------
#  Copyright (C) 2004-2011 by EMGU. All rights reserved.
# 
#  CMake file for cvextern. See root CMakeLists.txt
#
# ----------------------------------------------------------------------------
project(cvextern)

INCLUDE_DIRECTORIES("${PROJECT_SOURCE_DIR}/../opencv/modules/core/include")
INCLUDE_DIRECTORIES("${PROJECT_SOURCE_DIR}/../opencv/modules/imgproc/include")
INCLUDE_DIRECTORIES("${PROJECT_SOURCE_DIR}/../opencv/modules/ml/include")
INCLUDE_DIRECTORIES("${PROJECT_SOURCE_DIR}/../opencv/modules/calib3d/include")
INCLUDE_DIRECTORIES("${PROJECT_SOURCE_DIR}/../opencv/modules/objdetect/include")
INCLUDE_DIRECTORIES("${PROJECT_SOURCE_DIR}/../opencv/modules/features2d/include")
INCLUDE_DIRECTORIES("${PROJECT_SOURCE_DIR}/../opencv/modules/legacy/include")
INCLUDE_DIRECTORIES("${PROJECT_SOURCE_DIR}/../opencv/modules/contrib/include")
INCLUDE_DIRECTORIES("${PROJECT_SOURCE_DIR}/../opencv/modules/highgui/include")
INCLUDE_DIRECTORIES("${PROJECT_SOURCE_DIR}/../opencv/modules/flann/include")
INCLUDE_DIRECTORIES("${PROJECT_SOURCE_DIR}/../opencv/include/opencv")

############################### IPP START ##############################
IF(WITH_IPP)
include(${PROJECT_SOURCE_DIR}/../opencv/OpenCVFindIPP.cmake)

IF (IPP_FOUND)
  message(STATUS "CVEXTERN: USING IPP: ${IPP_LIBRARY_DIRS} ")
  add_definitions(-DHAVE_IPP)
  include_directories(${IPP_INCLUDE_DIRS})
  LINK_DIRECTORIES(${IPP_LIBRARY_DIRS})
  
  IF(WIN32)
  IF(TARGET_ARCH_64)
    SET(IPP_ARCH "intel64")
  ELSE()
    SET(IPP_ARCH "ia32")
  ENDIF()

  SET(IPP_BINARY_FILE_COMPONENT_NAMES 
	"ippcv" 
	"ipps" 
	"ippvm" 
	"ippi"
	"ippcc")
  LIST(APPEND IPP_BINARY_FILE "ippcore-7.0")
  IF(TARGET_ARCH_64)
    FOREACH(IPP_BINARY_FILE_COMPONENT_NAME ${IPP_BINARY_FILE_COMPONENT_NAMES})
     LIST(APPEND IPP_BINARY_FILE "${IPP_BINARY_FILE_COMPONENT_NAME}-7.0")
  	  LIST(APPEND IPP_BINARY_FILE "${IPP_BINARY_FILE_COMPONENT_NAME}e9-7.0")
  	  LIST(APPEND IPP_BINARY_FILE "${IPP_BINARY_FILE_COMPONENT_NAME}m7-7.0")
	  LIST(APPEND IPP_BINARY_FILE "${IPP_BINARY_FILE_COMPONENT_NAME}n8-7.0")
	  LIST(APPEND IPP_BINARY_FILE "${IPP_BINARY_FILE_COMPONENT_NAME}u8-7.0")
	  LIST(APPEND IPP_BINARY_FILE "${IPP_BINARY_FILE_COMPONENT_NAME}y8-7.0")
	ENDFOREACH()
  ELSE()
    FOREACH(IPP_BINARY_FILE_COMPONENT_NAME ${IPP_BINARY_FILE_COMPONENT_NAMES})
     LIST(APPEND IPP_BINARY_FILE "${IPP_BINARY_FILE_COMPONENT_NAME}-7.0")
  	  LIST(APPEND IPP_BINARY_FILE "${IPP_BINARY_FILE_COMPONENT_NAME}g9-7.0")
  	  LIST(APPEND IPP_BINARY_FILE "${IPP_BINARY_FILE_COMPONENT_NAME}p8-7.0")
	  LIST(APPEND IPP_BINARY_FILE "${IPP_BINARY_FILE_COMPONENT_NAME}s8-7.0")
	  LIST(APPEND IPP_BINARY_FILE "${IPP_BINARY_FILE_COMPONENT_NAME}v8-7.0")
	  LIST(APPEND IPP_BINARY_FILE "${IPP_BINARY_FILE_COMPONENT_NAME}w7-7.0")
	ENDFOREACH()
  ENDIF()
  
  LIST(APPEND CVEXTERN_DEPENDENCY_DLL_NAMES ${IPP_BINARY_FILE})
  FOREACH(IPP_FILE ${IPP_BINARY_FILE})
	LIST(APPEND CVEXTERN_DEPENDENCY_DLLS "${IPP_ROOT_DIR}/../redist/${IPP_ARCH}/ipp/${IPP_FILE}.dll")
  ENDFOREACH()
  ENDIF()
ENDIF()
ENDIF()
############################### IPP END ################################

############################### TBB START ##############################
if (WITH_TBB AND TBB_INCLUDE_DIR AND TBB_LIB_DIR)
  MESSAGE(STATUS "CVEXTERN: using TBB")
  add_definitions(-DHAVE_TBB)
  include_directories(${TBB_INCLUDE_DIR})
  link_directories(${TBB_LIB_DIR})
  
  IF(WIN32)
  SET(TBB_BINARY_FILE_DIR "${TBB_INCLUDE_DIR}/../../redist")
  IF(TARGET_ARCH_64)
    SET(TBB_BINARY_FILE_DIR "${TBB_BINARY_FILE_DIR}/intel64/tbb")
  ELSE()
    SET(TBB_BINARY_FILE_DIR "${TBB_BINARY_FILE_DIR}/ia32/tbb")
  ENDIF()
  if (MSVC80)
	SET(TBB_BINARY_FILE_DIR "${TBB_BINARY_FILE_DIR}/vc8")
  elseif(MSVC90)
	SET(TBB_BINARY_FILE_DIR "${TBB_BINARY_FILE_DIR}/vc9")
  elseif(MSVC10)
	SET(TBB_BINARY_FILE_DIR "${TBB_BINARY_FILE_DIR}/vc10")
  endif()

  LIST(APPEND CVEXTERN_DEPENDENCY_DLL_NAMES tbb)
  LIST(APPEND CVEXTERN_DEPENDENCY_DLLS "${TBB_BINARY_FILE_DIR}/tbb.dll")
  ENDIF()
endif()
############################### TBB END ################################

############################### OPENNI START ##############################
IF(WIN32 AND WITH_OPENNI)
  IF(TARGET_ARCH_64)
    SET(OPENNI_BINARY_FILE "${OPENNI_LIB_DIR}/../Bin64/OpenNI64.dll")
    LIST(APPEND CVEXTERN_DEPENDENCY_DLL_NAMES OpenNI64)
  ELSE()
    SET(OPENNI_BINARY_FILE "${OPENNI_LIB_DIR}/../Bin/OpenNI.dll")
    LIST(APPEND CVEXTERN_DEPENDENCY_DLL_NAMES OpenNI)
  ENDIF()
  
LIST(APPEND CVEXTERN_DEPENDENCY_DLLS ${OPENNI_BINARY_FILE})   
ENDIF()
############################### OPENNI END ################################

IF(DEFINED CVEXTERN_DEPENDENCY_DLLS)
  INSTALL(
    FILES 
    ${CVEXTERN_DEPENDENCY_DLLS}
    DESTINATION bin
    COMPONENT main)
ENDIF()

file(GLOB extern_srcs "./*.cpp")
file(GLOB extern_hdrs "./*.h*")

############################### TESSERACT START ########################
IF(WIN32)
  SET(EMGU_CV_WITH_TESSERACT ON CACHE BOOL "BUILD OCR")
  IF(EMGU_CV_WITH_TESSERACT)
    ADD_SUBDIRECTORY(tesseract/libtesseract)
    SET(TESSERACT_INCLUDE_DIRS "${PROJECT_SOURCE_DIR}/tesseract/libtesseract/tesseract-ocr/ccutil" "${PROJECT_SOURCE_DIR}/tesseract/libtesseract/tesseract-ocr/api" "${PROJECT_SOURCE_DIR}/tesseract/libtesseract/tesseract-ocr/ccmain" "${PROJECT_SOURCE_DIR}/tesseract/libtesseract/tesseract-ocr/ccstruct" "${PROJECT_SOURCE_DIR}/tesseract")
    SET(TESSERACT_LIBRARY tesseract_ccstruct tesseract_ccutil tesseract_classify tesseract_cube tesseract_cutil tesseract_dict tesseract_image tesseract_neural_networks tesseract_api tesseract_textord tesseract_viewer tesseract_wordrec libleptonica tesseract_ccmain)
  ENDIF()
ELSE()
  SET(CMAKE_MODULE_PATH "${PROJECT_SOURCE_DIR}/../cmake/modules")
  FIND_PACKAGE(TESSERACT)
  IF(TESSERACT_FOUND)
    SET(EMGU_CV_WITH_TESSERACT ON CACHE BOOL "BUILD OCR")
  ELSE()
    SET(EMGU_CV_WITH_TESSERACT OFF CACHE BOOL "BUILD OCR")
    SET(TESSERACT_INCLUDE_DIRS "")
    SET(TESSERACT_LIBRARY "")
  ENDIF()
ENDIF()

IF(EMGU_CV_WITH_TESSERACT)
  INCLUDE_DIRECTORIES(${TESSERACT_INCLUDE_DIRS})
  LIST(APPEND extern_srcs "${PROJECT_SOURCE_DIR}/tesseract/tesseract.cpp")
ENDIF()
############################### TESSERACT END ##########################

source_group("Src" FILES ${extern_srcs})
source_group("Include" FILES ${extern_hdrs})
set(the_target ${PROJECT_NAME})
add_library(${the_target} ${extern_srcs} ${extern_hdrs})

IF(DEFINED CVEXTERN_DEPENDENCY_DLLS)
  FOREACH(CVEXTERN_DEPENDENCY_DLL ${CVEXTERN_DEPENDENCY_DLLS})
    LIST(APPEND CVEXTERN_DEPENDENCY_DLL_DEPLOY_COMMAND COMMAND ${CMAKE_COMMAND} -E copy "${CVEXTERN_DEPENDENCY_DLL}" ${LIBRARY_OUTPUT_PATH})
  ENDFOREACH()
  #Promote this to parent scope such that cpack will know what dlls to be included in the package
  SET(CVEXTERN_DEPENDENCY_DLL_NAMES ${CVEXTERN_DEPENDENCY_DLL_NAMES} PARENT_SCOPE)

  ADD_CUSTOM_COMMAND(
    TARGET ${the_target}
    POST_BUILD
    ${CVEXTERN_DEPENDENCY_DLL_DEPLOY_COMMAND}
    COMMENT "Copying ${CVEXTERN_DEPENDENCY_DLLS} to ${LIBRARY_OUTPUT_PATH}")
ENDIF()

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
  SET(TIFF_INCLUDE_DIR "${PROJECT_SOURCE_DIR}/../opencv/3rdparty/libtiff")
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
  SET(ZLIB_INCLUDE_DIR "${PROJECT_SOURCE_DIR}/../opencv/3rdparty/zlib")
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
  SET(GEOTIFF_INCLUDE_DIR "${PROJECT_SOURCE_DIR}/../opencv/3rdparty/libtiff" "${PROJECT_SOURCE_DIR}/libgeotiff/libgeotiff-1.3.0" "${PROJECT_SOURCE_DIR}/libgeotiff/libgeotiff-1.3.0/libxtiff")
  SET(GEOTIFF_LIBRARY geotiff_archive xtiff)
ENDIF()
INCLUDE_DIRECTORIES(${GEOTIFF_INCLUDE_DIR})

ADD_DEPENDENCIES(${the_target} 
  opencv_core opencv_calib3d opencv_contrib opencv_features2d opencv_highgui opencv_imgproc opencv_legacy opencv_ml opencv_objdetect opencv_gpu
  ${ZLIB_LIBRARY} 
#  xtiff
  ${TIFF_LIBRARY} ${GEOTIFF_LIBRARY} ${TESSERACT_LIBRARY})

IF(EMGU_ENABLE_SSE)
  ADD_DEFINITIONS(-D__EMGU_ENABLE_SSE__)
  IF(MSVC AND (NOT CV_ICC) AND (NOT TARGET_ARCH_64)) 
	SET(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} /arch:SSE2")        ## Optimization
  ENDIF()
ENDIF()

#disable warnings
IF(MSVC)
  ADD_DEFINITIONS(-wd4251 -D_CRT_SECURE_NO_WARNINGS)
ENDIF()

# Add the required libraries for linking:
target_link_libraries(${the_target} ${OPENCV_LINKER_LIBS} 
  opencv_core opencv_calib3d opencv_contrib opencv_features2d opencv_highgui opencv_imgproc opencv_legacy opencv_ml opencv_objdetect
  ${ZLIB_LIBRARY} 
#  xtiff 
  ${TIFF_LIBRARY} ${GEOTIFF_LIBRARY} ${TESSERACT_LIBRARY})
  
IF(WIN32 AND EMGU_CV_WITH_TESSERACT)
#additional linkage required for tesseract built on windows
  target_link_libraries(${the_target}  Ws2_32)
ENDIF()
  
INSTALL(TARGETS ${the_target} 
    RUNTIME DESTINATION bin COMPONENT main
    LIBRARY DESTINATION lib COMPONENT main
    ARCHIVE DESTINATION lib COMPONENT main)

add_subdirectory(gpu)
