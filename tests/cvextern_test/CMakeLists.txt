# --------------------------------------------------------
#  Copyright (C) 2004-2017 by EMGU Corporation. All rights reserved.
#
#  CMake file for cvextern_test. See root CMakeLists.txt
# --------------------------------------------------------

project(cvextern_test)

SET(OPENCV_MODULES_DIR "${OPENCV_SUBFOLDER}/modules")
INCLUDE_DIRECTORIES("${OPENCV_MODULES_DIR}/core/include")
INCLUDE_DIRECTORIES("${OPENCV_MODULES_DIR}/imgproc/include")
INCLUDE_DIRECTORIES("${OPENCV_MODULES_DIR}/video/include")
INCLUDE_DIRECTORIES("${OPENCV_MODULES_DIR}/ml/include")
INCLUDE_DIRECTORIES("${OPENCV_MODULES_DIR}/calib3d/include")
INCLUDE_DIRECTORIES("${OPENCV_MODULES_DIR}/objdetect/include")
INCLUDE_DIRECTORIES("${OPENCV_MODULES_DIR}/features2d/include")
INCLUDE_DIRECTORIES("${OPENCV_MODULES_DIR}/hal/include")
INCLUDE_DIRECTORIES("${OPENCV_MODULES_DIR}/flann/include")
INCLUDE_DIRECTORIES("${OPENCV_SUBFOLDER}/include/opencv")
INCLUDE_DIRECTORIES("${PROJECT_SOURCE_DIR}/../../Emgu.CV.Extern/")
INCLUDE_DIRECTORIES("${PROJECT_SOURCE_DIR}/../../")

INCLUDE_DIRECTORIES("${CMAKE_BINARY_DIR}")
#INCLUDE_DIRECTORIES(${TIFF_INCLUDE_DIR})
#INCLUDE_DIRECTORIES(${ZLIB_INCLUDE_DIR})
#INCLUDE_DIRECTORIES(${GEOTIFF_INCLUDE_DIR})

############################### IPP START ##############################
#IF(WITH_IPP)
#include(${OPENCV_SUBFOLDER}/cmake/OpenCVFindIPP.cmake)
#  IF (IPP_FOUND)
#    message(STATUS "CVEXTERN: USING IPP: ${IPP_LIBRARY_DIRS} ")
#    add_definitions(-DHAVE_IPP)
#    include_directories(${IPP_INCLUDE_DIRS})
#    LINK_DIRECTORIES(${IPP_LIBRARY_DIRS})
#  ENDIF()
#ENDIF()
############################### IPP END ################################

############################### TBB START ##############################
if (WITH_TBB AND TBB_INCLUDE_DIR AND TBB_LIB_DIR)
  add_definitions(-DHAVE_TBB)
  include_directories(${TBB_INCLUDE_DIR})
  link_directories(${TBB_LIB_DIR})
endif()
############################### TBB END ################################

############################### GPU START ##############################
IF(WIN32 AND WITH_CUDA)
  IF(TARGET_ARCH_64)
    LINK_DIRECTORIES(${CUDA_TOOLKIT_ROOT_DIR}/lib/x64)
  ELSE()
    LINK_DIRECTORIES(${CUDA_TOOLKIT_ROOT_DIR}/lib/Win32)
  ENDIF()
ENDIF()
############################### GPU END ################################

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

if(ENABLE_SOLUTION_FOLDERS)
    set_target_properties(${the_target} PROPERTIES FOLDER "emgu")
endif()

# Additional target properties
set_target_properties(${the_target} PROPERTIES
	DEBUG_POSTFIX "${OPENCV_DEBUG_POSTFIX}"
	ARCHIVE_OUTPUT_DIRECTORY "${CMAKE_BINARY_DIR}/lib${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER}"
	RUNTIME_OUTPUT_DIRECTORY "${CMAKE_BINARY_DIR}/bin${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER}"
	)

ADD_DEPENDENCIES(${the_target} 
  cvextern opencv_core opencv_calib3d opencv_features2d opencv_highgui opencv_imgproc opencv_ml opencv_objdetect 
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
  cvextern opencv_core opencv_calib3d opencv_features2d opencv_highgui opencv_imgproc opencv_ml opencv_objdetect 
#  ${ZLIB_LIBRARY} 
#  ${TIFF_LIBRARY} 
#  ${GEOTIFF_LIBRARY}
)

IF(DEFINED EMGUCV_PLATFORM_TOOLSET)
  set_target_properties(${the_target} PROPERTIES PLATFORM_TOOLSET ${EMGUCV_PLATFORM_TOOLSET})
ENDIF()

INSTALL(TARGETS ${the_target} 
    RUNTIME DESTINATION bin${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER} COMPONENT main
    LIBRARY DESTINATION lib${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER} COMPONENT main
    ARCHIVE DESTINATION lib${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER} COMPONENT main)