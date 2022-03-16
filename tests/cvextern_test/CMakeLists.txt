# --------------------------------------------------------
#  Copyright (C) 2004-2022 by EMGU Corporation. All rights reserved.
#
#  CMake file for cvextern_test. See root CMakeLists.txt
# --------------------------------------------------------

IF(NOT IS_DEBIAN_ARM_BUILD) 

project(cvextern_test)

SET(OPENCV_MODULES_DIR "${OPENCV_SUBFOLDER}/modules")
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

IF(WITH_INF_ENGINE)
	#SET(INF_ENGINE_LIB_DIRS )
	#link_directories("${INF_ENGINE_LIB_DIRS}/Release")
	link_directories("${IE_INCLUDE_DIR}/../lib/intel64/")
ENDIF()

############################### OPENMP START ##############################
IF(WITH_OPENMP)
  FIND_PACKAGE( OpenMP REQUIRED)
  if(OPENMP_FOUND)
    #MESSAGE(STATUS " -- Compiling cvextern with OPENMP")
    set(CMAKE_C_FLAGS "${CMAKE_C_FLAGS} ${OpenMP_C_FLAGS}")
    set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} ${OpenMP_CXX_FLAGS}")
    set(CMAKE_EXE_LINKER_FLAGS "${CMAKE_EXE_LINKER_FLAGS} ${OpenMP_EXE_LINKER_FLAGS}")
  endif()
ENDIF()
############################### OPENMP END ################################

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

set_target_properties(${PROJECT_NAME} PROPERTIES CXX_STANDARD 11)
if(ENABLE_SOLUTION_FOLDERS)
    set_target_properties(${the_target} PROPERTIES FOLDER "emgu")
endif()

# Additional target properties
set_target_properties(${the_target} PROPERTIES
	DEBUG_POSTFIX "${OPENCV_DEBUG_POSTFIX}"
	ARCHIVE_OUTPUT_DIRECTORY "${CMAKE_BINARY_DIR}/lib${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER}"
	RUNTIME_OUTPUT_DIRECTORY "${CMAKE_BINARY_DIR}/bin${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER}"
	)

FOREACH(CVEXTERN_OPTIONAL_DEP ${OPENCV_MODULE_NAME_CANDIDATES})
  IF (HAVE_opencv_${CVEXTERN_OPTIONAL_DEP})
    IF ( (DEFINED opencv_${CVEXTERN_OPTIONAL_DEP}_SOURCE_DIR) AND (EXISTS ${opencv_${CVEXTERN_OPTIONAL_DEP}_SOURCE_DIR}/include) )
      INCLUDE_DIRECTORIES("${opencv_${CVEXTERN_OPTIONAL_DEP}_SOURCE_DIR}/include")
      MESSAGE(STATUS "Include directory for cvextern_test:  ${opencv_${CVEXTERN_OPTIONAL_DEP}_SOURCE_DIR}/include")
    ELSEIF (EXISTS "${OPENCV_MODULES_DIR}/${CVEXTERN_OPTIONAL_DEP}/include")
      INCLUDE_DIRECTORIES("${OPENCV_MODULES_DIR}/${CVEXTERN_OPTIONAL_DEP}/include")
      MESSAGE(STATUS "Include directory for cvextern_test:  ${OPENCV_MODULES_DIR}/${CVEXTERN_OPTIONAL_DEP}/include")
    ELSEIF (EXISTS "${OPENCV_EXTRA_MODULES_PATH}/${CVEXTERN_OPTIONAL_DEP}/include")
      INCLUDE_DIRECTORIES("${OPENCV_EXTRA_MODULES_PATH}/${CVEXTERN_OPTIONAL_DEP}/include")
      MESSAGE(STATUS "Include directory for cvextern_test:  ${OPENCV_EXTRA_MODULES_PATH}/${CVEXTERN_OPTIONAL_DEP}/include")
    ENDIF()
  ENDIF()
ENDFOREACH()

FOREACH(CVEXTERN_OPTIONAL_DEP ${OPENCV_MODULE_NAMES}) 
  #  INCLUDE_DIRECTORIES("${OPENCV_MODULES_DIR}/${CVEXTERN_OPTIONAL_DEP}/include")
  MESSAGE(STATUS "CVEXTERN_TEST dependency added:  opencv_${CVEXTERN_OPTIONAL_DEP}")
  ADD_DEPENDENCIES(${the_target} opencv_${CVEXTERN_OPTIONAL_DEP})
  target_link_libraries(${the_target} opencv_${CVEXTERN_OPTIONAL_DEP})
ENDFOREACH()

#ADD_DEPENDENCIES(${the_target} 
#  cvextern 
#)

IF(EMGU_ENABLE_SSE)
  ADD_DEFINITIONS(-D__EMGU_ENABLE_SSE__)
  IF(MSVC AND EMGUCV_ARCH MATCHES "win32" AND (NOT CV_ICC)) 
    SET(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} /arch:SSE2")        ## Optimization
  ENDIF()
  IF ((NOT WIN32) AND NOT ("${CMAKE_SYSTEM_NAME}" STREQUAL "Emscripten"))
    SET(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} -latomic")
  ENDIF()
ENDIF()

# Add the required libraries for linking:
target_link_libraries(${the_target} ${OPENCV_LINKER_LIBS} 
  #cvextern 
  #opencv_core opencv_calib3d opencv_features2d opencv_highgui opencv_imgproc opencv_ml opencv_objdetect 
#  ${ZLIB_LIBRARY} 
#  ${TIFF_LIBRARY} 
#  ${GEOTIFF_LIBRARY}
)

#IF(IS_DEBIAN_ARM_BUILD)
#  MESSAGE(STATUS "----------------------------- Linking with atomic on arm debian build ---------------------------")
#  #target_link_libraries(${the_target} atomic)
#  target_link_libraries(${the_target} -latomic)
#  target_link_options(${the_target} PRIVATE -latomic)
#  SET(CMAKE_EXE_LINKER_FLAGS ${CMAKE_EXE_LINKER_FLAGS} -latomic)
#ENDIF()

#IF(IS_DEBIAN_ARM64_BUILD)
#  MESSAGE(STATUS "----------------------------- DISABLE NEON on arm64 debian build ---------------------------")
#  ADD_DEFINITIONS(-DPNG_ARM_NEON_OPT=0)
#ENDIF()

if(WITH_PNG AND BUILD_PNG)
  target_link_libraries(${the_target} libpng)
endif()

if(WITH_TIFF AND BUILD_TIFF)
  target_link_libraries(${the_target} libtiff)
endif()

if(WITH_JPEG AND BUILD_JPEG)
  target_link_libraries(${the_target} libjpeg-turbo)
  endif()

IF("${CMAKE_SYSTEM_NAME}" STREQUAL "Emscripten")
  set_target_properties(${the_target} PROPERTIES SUFFIX ".html")
ENDIF()

IF(DEFINED EMGUCV_PLATFORM_TOOLSET)
  set_target_properties(${the_target} PROPERTIES PLATFORM_TOOLSET ${EMGUCV_PLATFORM_TOOLSET})
ENDIF()

INSTALL(TARGETS ${the_target} 
    RUNTIME DESTINATION bin${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER} COMPONENT main
    LIBRARY DESTINATION lib${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER} COMPONENT main
    ARCHIVE DESTINATION lib${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER} COMPONENT main)

ENDIF()
