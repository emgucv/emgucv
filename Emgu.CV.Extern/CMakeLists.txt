# ----------------------------------------------------------------------------
#  CMake file for cvextern. See root CMakeLists.txt
#
# ----------------------------------------------------------------------------
project(cvextern)

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

  SET(IPP_BINARY_FILE_COMPONENT_NAMES 
	"ippcv" 
	"ipps" 
	"ippvm" 
	"ippi"
	"ippcc")
  LIST(APPEND IPP_BINARY_FILE "${IPP_PATH}/ippcore-7.0.dll")
  IF(CMAKE_CL_64)
    FOREACH(IPP_BINARY_FILE_COMPONENT_NAME ${IPP_BINARY_FILE_COMPONENT_NAMES})
      LIST(APPEND IPP_BINARY_FILE "${IPP_PATH}/${IPP_BINARY_FILE_COMPONENT_NAME}-7.0.dll")
  	  LIST(APPEND IPP_BINARY_FILE "${IPP_PATH}/${IPP_BINARY_FILE_COMPONENT_NAME}e9-7.0.dll")
  	  LIST(APPEND IPP_BINARY_FILE "${IPP_PATH}/${IPP_BINARY_FILE_COMPONENT_NAME}m7-7.0.dll")
	  LIST(APPEND IPP_BINARY_FILE "${IPP_PATH}/${IPP_BINARY_FILE_COMPONENT_NAME}n8-7.0.dll")
	  LIST(APPEND IPP_BINARY_FILE "${IPP_PATH}/${IPP_BINARY_FILE_COMPONENT_NAME}u8-7.0.dll")
	  LIST(APPEND IPP_BINARY_FILE "${IPP_PATH}/${IPP_BINARY_FILE_COMPONENT_NAME}y8-7.0.dll")
	ENDFOREACH()
  ELSE()
    FOREACH(IPP_BINARY_FILE_COMPONENT_NAME ${IPP_BINARY_FILE_COMPONENT_NAMES})
      LIST(APPEND IPP_BINARY_FILE "${IPP_PATH}/${IPP_BINARY_FILE_COMPONENT_NAME}-7.0.dll")
  	  LIST(APPEND IPP_BINARY_FILE "${IPP_PATH}/${IPP_BINARY_FILE_COMPONENT_NAME}g9-7.0.dll")
  	  LIST(APPEND IPP_BINARY_FILE "${IPP_PATH}/${IPP_BINARY_FILE_COMPONENT_NAME}p8-7.0.dll")
	  LIST(APPEND IPP_BINARY_FILE "${IPP_PATH}/${IPP_BINARY_FILE_COMPONENT_NAME}s8-7.0.dll")
	  LIST(APPEND IPP_BINARY_FILE "${IPP_PATH}/${IPP_BINARY_FILE_COMPONENT_NAME}v8-7.0.dll")
	  LIST(APPEND IPP_BINARY_FILE "${IPP_PATH}/${IPP_BINARY_FILE_COMPONENT_NAME}w7-7.0.dll")
	ENDFOREACH()
  ENDIF()


  INSTALL(
  FILES 
  ${IPP_BINARY_FILE}
  DESTINATION bin
  COMPONENT main)
ENDIF()
############################### IPP END ################################

############################### TBB START ##############################
if (WITH_TBB AND TBB_INCLUDE_DIR AND TBB_LIB_DIR)
  MESSAGE(STATUS "CVEXTERN: using TBB")
  add_definitions(-DHAVE_TBB)
  include_directories(${TBB_INCLUDE_DIR})
  link_directories(${TBB_LIB_DIR})
  SET(TBB_BINARY_FILE "${TBB_INCLUDE_DIR}/../../redist")
  IF(CMAKE_CL_64)
    SET(TBB_BINARY_FILE "${TBB_BINARY_FILE}/intel64/tbb")
  ELSE()
    SET(TBB_BINARY_FILE "${TBB_BINARY_FILE}/ia32/tbb")
  ENDIF()
  if (MSVC80)
	SET(TBB_BINARY_FILE "${TBB_BINARY_FILE}/vc8")
  elseif(MSVC90)
	SET(TBB_BINARY_FILE "${TBB_BINARY_FILE}/vc9")
  elseif(MSVC10)
	SET(TBB_BINARY_FILE "${TBB_BINARY_FILE}/vc10")
  endif()
  SET(TBB_BINARY_FILE "${TBB_BINARY_FILE}/tbb.dll")

  MESSAGE(STATUS "TBB_BINARY_FILE: ${TBB_BINARY_FILE}")

  INSTALL(
    FILES 
    ${TBB_BINARY_FILE}
    DESTINATION bin
    COMPONENT main)
endif()
############################### TBB END ################################

############################### GPU START ##############################
IF(WIN32 AND WITH_CUDA)

if(EXISTS ${CUDA_NPP_INCLUDES}/nppversion.h)
    file( STRINGS ${CUDA_NPP_INCLUDES}/nppversion.h npp_major REGEX "#define NPP_VERSION_MAJOR.*")
    file( STRINGS ${CUDA_NPP_INCLUDES}/nppversion.h npp_minor REGEX "#define NPP_VERSION_MINOR.*")
    file( STRINGS ${CUDA_NPP_INCLUDES}/nppversion.h npp_build REGEX "#define NPP_VERSION_BUILD.*")

    string( REGEX REPLACE "#define NPP_VERSION_MAJOR[ \t]+|//.*" "" npp_major ${npp_major})
    string( REGEX REPLACE "#define NPP_VERSION_MINOR[ \t]+|//.*" "" npp_minor ${npp_minor})
    string( REGEX REPLACE "#define NPP_VERSION_BUILD[ \t]+|//.*" "" npp_build ${npp_build})

    string( REGEX MATCH "[0-9]+" npp_major ${npp_major} ) 
    string( REGEX MATCH "[0-9]+" npp_minor ${npp_minor} ) 
    string( REGEX MATCH "[0-9]+" npp_build ${npp_build} ) 	
endif()

IF(CMAKE_CL_64) 
  SET(NPP_BINARY_FILE "${CUDA_NPP_LIBRARY_ROOT_DIR}/bin/x64/npp64_${npp_major}${npp_minor}_${npp_build}.dll")
  LIST(APPEND NPP_BINARY_FILE "${CUDA_SDK_ROOT_DIR}/bin/cudart64_${npp_major}${npp_minor}_${npp_build}.dll")
ELSE()
  SET(NPP_BINARY_FILE "${CUDA_NPP_LIBRARY_ROOT_DIR}/bin/win32/npp32_${npp_major}${npp_minor}_${npp_build}.dll")
  LIST(APPEND NPP_BINARY_FILE "${CUDA_SDK_ROOT_DIR}/bin/cudart32_${npp_major}${npp_minor}_${npp_build}.dll")
ENDIF()

MESSAGE(STATUS "NPP_BINARY_FILE: ${NPP_BINARY_FILE}")

INSTALL(
  FILES 
  ${NPP_BINARY_FILE}
  DESTINATION bin
  COMPONENT main)

ENDIF()
############################### GPU END ################################

file(GLOB extern_srcs "./*.cpp")
source_group("Src" FILES ${extern_srcs})
file(GLOB extern_hdrs "./*.h*")
source_group("Include" FILES ${extern_hdrs})

set(the_target ${PROJECT_NAME})
add_library(${the_target} ${extern_srcs} ${extern_hdrs})

############################### GPU START ##############################
IF(WIN32 AND WITH_CUDA)
  FOREACH(NPP_FILE ${NPP_BINARY_FILE})
    LIST(APPEND NPP_FILE_DEPLOY_COMMAND COMMAND ${CMAKE_COMMAND} -E copy ${NPP_FILE} ${LIBRARY_OUTPUT_PATH})
  ENDFOREACH()
  
  ADD_CUSTOM_COMMAND(
    TARGET ${the_target}
    POST_BUILD
    ${NPP_FILE_DEPLOY_COMMAND}
    COMMENT "Copying ${NPP_BINARY_FILE} to ${LIBRARY_OUTPUT_PATH}")
ENDIF()  
############################### GPU END ################################

############################### TBB START ##############################
IF (WITH_TBB AND TBB_INCLUDE_DIR AND TBB_LIB_DIR)
  ADD_CUSTOM_COMMAND(
    TARGET ${the_target}
    POST_BUILD
    COMMAND ${CMAKE_COMMAND} -E copy ${TBB_BINARY_FILE} ${LIBRARY_OUTPUT_PATH}
    COMMENT "Copying ${TBB_BINARY_FILE} to ${LIBRARY_OUTPUT_PATH}")
ENDIF()
############################### TBB END ################################

############################### IPP START ##############################
IF (USE_IPP AND IPP_PATH)
  FOREACH(IPP_FILE ${IPP_BINARY_FILE})
    LIST(APPEND IPP_FILE_DEPLOY_COMMAND COMMAND ${CMAKE_COMMAND} -E copy ${IPP_FILE} ${LIBRARY_OUTPUT_PATH})
  ENDFOREACH()
  
  ADD_CUSTOM_COMMAND(
    TARGET ${the_target}
    POST_BUILD
    ${IPP_FILE_DEPLOY_COMMAND}
    COMMENT "Copying ${IPP_BINARY_FILE} to ${LIBRARY_OUTPUT_PATH}")
ENDIF()
############################### IPP END ################################

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
  IF(MSVC AND (NOT CV_ICC) AND (NOT CMAKE_CL_64)) 
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
