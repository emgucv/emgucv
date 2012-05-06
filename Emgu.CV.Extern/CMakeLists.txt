# --------------------------------------------------------
#  Copyright (C) 2004-2012 by EMGU. All rights reserved.
# 
#  CMake file for cvextern. See root CMakeLists.txt
#
# ----------------------------------------------------------------------------
project(cvextern)

INCLUDE_DIRECTORIES("${PROJECT_SOURCE_DIR}/../opencv/include/opencv")
INCLUDE_DIRECTORIES("${PROJECT_SOURCE_DIR}/../opencv/include")

SET(OPENCV_MODULES_DIR "${PROJECT_SOURCE_DIR}/../opencv/modules")
INCLUDE_DIRECTORIES("${OPENCV_MODULES_DIR}/core/include")
INCLUDE_DIRECTORIES("${OPENCV_MODULES_DIR}/imgproc/include")
INCLUDE_DIRECTORIES("${OPENCV_MODULES_DIR}/ml/include")
INCLUDE_DIRECTORIES("${OPENCV_MODULES_DIR}/calib3d/include")
INCLUDE_DIRECTORIES("${OPENCV_MODULES_DIR}/objdetect/include")
INCLUDE_DIRECTORIES("${OPENCV_MODULES_DIR}/features2d/include")
INCLUDE_DIRECTORIES("${OPENCV_MODULES_DIR}/legacy/include")
INCLUDE_DIRECTORIES("${OPENCV_MODULES_DIR}/contrib/include")
INCLUDE_DIRECTORIES("${OPENCV_MODULES_DIR}/highgui/include")
INCLUDE_DIRECTORIES("${OPENCV_MODULES_DIR}/flann/include")
INCLUDE_DIRECTORIES("${OPENCV_MODULES_DIR}/video/include")
INCLUDE_DIRECTORIES("${OPENCV_MODULES_DIR}/nonfree/include")

IF(WIN32 AND CV_ICC)
  STRING(REGEX REPLACE "/Qipo" "" CMAKE_C_FLAGS_RELEASE ${CMAKE_C_FLAGS_RELEASE})
  STRING(REGEX REPLACE "/Qipo" "" CMAKE_CXX_FLAGS_RELEASE ${CMAKE_CXX_FLAGS_RELEASE})
ENDIF()

############################### IPP START ##############################
IF(WITH_IPP)
  include(${PROJECT_SOURCE_DIR}/../opencv/cmake/OpenCVFindIPP.cmake)
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

############################### INTEL ICL START ##############################
IF(WIN32 AND CV_ICC)
  IF(TARGET_ARCH_64)
    SET(INTEL_ICC_REDIST_PATH "$ENV{ICPP_COMPILER12}redist/intel64/compiler/")
  ELSE()
    SET(INTEL_ICC_REDIST_PATH "$ENV{ICPP_COMPILER12}redist/ia32/compiler/")
  ENDIF()
  STRING(REGEX REPLACE "\\\\" "/" INTEL_ICC_REDIST_PATH ${INTEL_ICC_REDIST_PATH})
  LIST(APPEND CVEXTERN_DEPENDENCY_DLL_NAMES libomp5md libmmd svml_dispmd)
  LIST(APPEND CVEXTERN_DEPENDENCY_DLLS ${INTEL_ICC_REDIST_PATH}libiomp5md.dll ${INTEL_ICC_REDIST_PATH}libmmd.dll ${INTEL_ICC_REDIST_PATH}svml_dispmd.dll)
ENDIF()
############################### INTEL ICL START ##############################

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
    DESTINATION bin${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER}
    COMPONENT main)
ENDIF()

file(GLOB extern_srcs "${PROJECT_SOURCE_DIR}/*.cpp")
file(GLOB extern_hdrs "${PROJECT_SOURCE_DIR}/*.h*")

IF(IOS)
  LIST(APPEND extern_srcs "ios/divmodsi4.c")
ENDIF()

############################### TESSERACT START ########################
IF(ANDROID)
  SET(EMGU_CV_WITH_TESSERACT ON CACHE BOOL "BUILD OCR")
  IF(EMGU_CV_WITH_TESSERACT)
    ADD_SUBDIRECTORY(tesseract/libtesseract)
    SET(TESSERACT_INCLUDE_DIRS "${PROJECT_SOURCE_DIR}/tesseract/libtesseract/tesseract-ocr/ccutil" "${PROJECT_SOURCE_DIR}/tesseract/libtesseract/tesseract-ocr/api" "${PROJECT_SOURCE_DIR}/tesseract/libtesseract/tesseract-ocr/ccmain" "${PROJECT_SOURCE_DIR}/tesseract/libtesseract/tesseract-ocr/ccstruct" "${PROJECT_SOURCE_DIR}/tesseract")
    SET(TESSERACT_LIBRARY tesseract_ccstruct tesseract_ccutil tesseract_classify tesseract_cube tesseract_cutil tesseract_dict tesseract_image tesseract_neural_networks tesseract_api tesseract_textord tesseract_viewer tesseract_wordrec libleptonica tesseract_ccmain)
  ENDIF()
ELSE()
IF(WIN32 OR APPLE)
  SET(EMGU_CV_WITH_TESSERACT ON CACHE BOOL "BUILD OCR")
  IF(EMGU_CV_WITH_TESSERACT)
    ADD_SUBDIRECTORY(tesseract/libtesseract)
    SET(TESSERACT_INCLUDE_DIRS "${PROJECT_SOURCE_DIR}/tesseract/libtesseract/tesseract-ocr/ccutil" "${PROJECT_SOURCE_DIR}/tesseract/libtesseract/tesseract-ocr/api" "${PROJECT_SOURCE_DIR}/tesseract/libtesseract/tesseract-ocr/ccmain" "${PROJECT_SOURCE_DIR}/tesseract/libtesseract/tesseract-ocr/ccstruct" "${PROJECT_SOURCE_DIR}/tesseract")
    SET(TESSERACT_LIBRARY tesseract_ccstruct tesseract_ccutil tesseract_classify tesseract_cube tesseract_cutil tesseract_dict tesseract_image tesseract_neural_networks tesseract_api tesseract_textord tesseract_viewer tesseract_wordrec libleptonica tesseract_ccmain)
    ADD_DEFINITIONS(-D__MSW32__)
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
ENDIF()

IF(EMGU_CV_WITH_TESSERACT)
  INCLUDE_DIRECTORIES(${TESSERACT_INCLUDE_DIRS})
  LIST(APPEND extern_hdrs "${PROJECT_SOURCE_DIR}/tesseract/tesseract_c.h")
  LIST(APPEND extern_srcs "${PROJECT_SOURCE_DIR}/tesseract/tesseract.cpp")
ENDIF()
############################### TESSERACT END ##########################

############################### CVBLOB START ########################
SET(EMGU_CV_WITH_CVBLOB ON CACHE BOOL "BUILD with CVBLOB")
MESSAGE(STATUS "WITH CVBLOB: ${EMGU_CV_WITH_CVBLOB}")
IF(EMGU_CV_WITH_CVBLOB)
  ADD_SUBDIRECTORY(cvblob)
  SET(CVBLOB_LIBRARY cvblob)
ELSE()
  FILE(GLOB extern_srcs_excludes "${PROJECT_SOURCE_DIR}/cvblob_c.cpp")
  LIST(REMOVE_ITEM extern_srcs ${extern_srcs_excludes})
  FILE(GLOB extern_hdrs_excludes "${PROJECT_SOURCE_DIR}/cvblob_c.h")
  LIST(REMOVE_ITEM extern_hdrs ${extern_hdrs_excludes})
ENDIF()
############################### CVBLOB END ##########################

IF(ANDROID OR APPLE)
  FILE(GLOB extern_srcs_excludes "${PROJECT_SOURCE_DIR}/tiffio.cpp")
  LIST(REMOVE_ITEM extern_srcs ${extern_srcs_excludes})
  #MESSAGE(STATUS "extern_srcs: ${extern_srcs}")
  FILE(GLOB extern_hdrs_excludes "${PROJECT_SOURCE_DIR}/tiffio_c.h")
  LIST(REMOVE_ITEM extern_hdrs ${extern_hdrs_excludes})
ENDIF()

source_group("Src" FILES ${extern_srcs})
source_group("Include" FILES ${extern_hdrs})
set(the_target ${PROJECT_NAME})
add_library(${the_target} ${extern_srcs} ${extern_hdrs})

IF(DEFINED CVEXTERN_DEPENDENCY_DLLS)
  FOREACH(CVEXTERN_DEPENDENCY_DLL ${CVEXTERN_DEPENDENCY_DLLS})
    LIST(APPEND CVEXTERN_DEPENDENCY_DLL_DEPLOY_COMMAND COMMAND ${CMAKE_COMMAND} -E copy "${CVEXTERN_DEPENDENCY_DLL}" "${CMAKE_RUNTIME_OUTPUT_DIRECTORY}")
  ENDFOREACH()
  #Promote this to parent scope such that cpack will know what dlls to be included in the package
  SET(CVEXTERN_DEPENDENCY_DLL_NAMES ${CVEXTERN_DEPENDENCY_DLL_NAMES} PARENT_SCOPE)

  ADD_CUSTOM_COMMAND(
    TARGET ${the_target}
    POST_BUILD
    ${CVEXTERN_DEPENDENCY_DLL_DEPLOY_COMMAND}
    COMMENT "Copying ${CVEXTERN_DEPENDENCY_DLLS} to ${CMAKE_RUNTIME_OUTPUT_DIRECTORY}")
ENDIF()

# For dynamic link numbering convenions
set_target_properties(${the_target} PROPERTIES
    OUTPUT_NAME "${the_target}"
    )

# Additional target properties
set_target_properties(${the_target} PROPERTIES
	DEBUG_POSTFIX "${OPENCV_DEBUG_POSTFIX}"
	ARCHIVE_OUTPUT_DIRECTORY "${CMAKE_BINARY_DIR}/lib"
	RUNTIME_OUTPUT_DIRECTORY "${CMAKE_BINARY_DIR}/bin"
	DEFINE_SYMBOL "CVAPI_EXPORTS"
	)

if(ENABLE_SOLUTION_FOLDERS)
    set_target_properties(${the_target} PROPERTIES FOLDER "emgu")
endif() 

FIND_PACKAGE(TIFF)
IF(NOT TIFF_FOUND)
  ADD_DEPENDENCIES(${the_target} libtiff)
  SET(TIFF_INCLUDE_DIR "${CMAKE_CURRENT_BINARY_DIR}/../opencv/3rdparty/libtiff" "${PROJECT_SOURCE_DIR}/../opencv/3rdparty/libtiff")
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

IF (ANDROID OR APPLE OR WIN32)
  SET(EMGU_CV_WITH_TIFF OFF CACHE BOOL "BUILD TIFF wrapper")
  MESSAGE(STATUS "Skipping tiff")
  SET(GEOTIFF_LIBRARY "")
ELSE()
  SET(EMGU_CV_WITH_TIFF ON CACHE BOOL "BUILD TIFF wrapper")
ENDIF()

IF(EMGU_CV_WITH_TIFF)
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
    SET(LIBGEOTIFF_DIR "${CMAKE_CURRENT_SOURCE_DIR}/libgeotiff/libgeotiff-1.4.0")
    ADD_SUBDIRECTORY(libgeotiff)
	#MESSAGE(STATUS "CURRENT Binary DIR: ${CMAKE_CURRENT_BINARY_DIR}")
    SET(GEOTIFF_INCLUDE_DIR ${TIFF_INCLUDE_DIR} ${LIBGEOTIFF_DIR} "${LIBGEOTIFF_DIR}/libxtiff")
    SET(GEOTIFF_LIBRARY geotiff_archive xtiff)
  ENDIF()
  ADD_DEFINITIONS(-DEMGU_CV_WITH_TIFF)
  INCLUDE_DIRECTORIES(${GEOTIFF_INCLUDE_DIR})
ENDIF()

ADD_DEPENDENCIES(${the_target} 
  opencv_core opencv_calib3d opencv_contrib opencv_features2d opencv_highgui opencv_imgproc opencv_legacy opencv_ml opencv_objdetect opencv_nonfree 
  ${ZLIB_LIBRARY} 
#  xtiff
  ${TIFF_LIBRARY} ${GEOTIFF_LIBRARY} ${TESSERACT_LIBRARY} ${CVBLOB_LIBRARY})

IF(EMGU_ENABLE_SSE)
  ADD_DEFINITIONS(-D__EMGU_ENABLE_SSE__)
  IF(MSVC AND (NOT CV_ICC) AND (NOT TARGET_ARCH_64)) 
	SET(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} /arch:SSE2")        ## Optimization
  ENDIF()
ENDIF()

IF(ANDROID)
  ADD_DEFINITIONS(-DANDROID)
ENDIF()

#disable warnings
IF(MSVC)
  ADD_DEFINITIONS(-wd4251 -D_CRT_SECURE_NO_WARNINGS)
ENDIF()

# Add the required libraries for linking:
target_link_libraries(${the_target} ${OPENCV_LINKER_LIBS} 
  opencv_core opencv_calib3d opencv_contrib opencv_features2d opencv_highgui opencv_imgproc opencv_legacy opencv_ml opencv_objdetect opencv_nonfree 
  ${ZLIB_LIBRARY} 
#  xtiff 
  ${TESSERACT_LIBRARY} ${CVBLOB_LIBRARY})

#IF(WITH_IPP)
#  target_link_libraries(${the_target} ippdc_l)
#ENDIF()  

IF(ANDROID)
  IF(EMGU_CV_WITH_TESSERACT)
    #NOT SURE WHY THE FOLLOWING IS NEEDED, BUT SEEMS TO MAKE THE PROBLEM GOES AWAY
    target_link_libraries(${the_target} ${TESSERACT_LIBRARY} tesseract_ccstruct tesseract_dict tesseract_classify tesseract_ccutil)
  ENDIF()
  INSTALL(TARGETS ${the_target} 
    LIBRARY DESTINATION libs${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER} COMPONENT main)
ELSE()
  target_link_libraries(${the_target} ${TIFF_LIBRARY} ${GEOTIFF_LIBRARY})
  IF(WIN32 AND EMGU_CV_WITH_TESSERACT)
    #additional linkage required for tesseract built on windows
    target_link_libraries(${the_target} Ws2_32)
  ENDIF()
  INSTALL(TARGETS ${the_target} 
    RUNTIME DESTINATION bin${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER} COMPONENT main
    LIBRARY DESTINATION lib${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER} COMPONENT main
    ARCHIVE DESTINATION lib${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER} COMPONENT main)
ENDIF()

add_subdirectory(gpu)
