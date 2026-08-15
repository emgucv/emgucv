# --------------------------------------------------------
#  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
#
#  CMake file for cvextern_test. See root CMakeLists.txt
# --------------------------------------------------------

IF(NOT IS_DEBIAN_ARM_BUILD)

  PROJECT(cvextern_test)

  SET(OPENCV_MODULES_DIR "${OPENCV_SUBFOLDER}/modules")
  INCLUDE_DIRECTORIES("${OPENCV_SUBFOLDER}/include/opencv")
  INCLUDE_DIRECTORIES("${PROJECT_SOURCE_DIR}/../../Emgu.CV.Extern/")
  INCLUDE_DIRECTORIES("${PROJECT_SOURCE_DIR}/../../")

  INCLUDE_DIRECTORIES("${CMAKE_BINARY_DIR}")
  ############################### TBB START ##############################
  IF(WITH_TBB AND TBB_INCLUDE_DIR AND TBB_LIB_DIR)
    ADD_DEFINITIONS(-DHAVE_TBB)
    INCLUDE_DIRECTORIES(${TBB_INCLUDE_DIR})
    LINK_DIRECTORIES(${TBB_LIB_DIR})
  ENDIF()
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
    LINK_DIRECTORIES("${IE_INCLUDE_DIR}/../lib/intel64/")
  ENDIF()

  ############################### OPENMP START ##############################
  IF(WITH_OPENMP)
    FIND_PACKAGE(OpenMP REQUIRED)
    IF(OPENMP_FOUND)
      SET(CMAKE_C_FLAGS "${CMAKE_C_FLAGS} ${OpenMP_C_FLAGS}")
      SET(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} ${OpenMP_CXX_FLAGS}")
      SET(CMAKE_EXE_LINKER_FLAGS "${CMAKE_EXE_LINKER_FLAGS} ${OpenMP_EXE_LINKER_FLAGS}")
    ENDIF()
  ENDIF()
  ############################### OPENMP END ################################

  IF(CMAKE_COMPILER_IS_GNUCXX)
    SET(CMAKE_C_FLAGS "${CMAKE_C_FLAGS} -Wno-unused-function")
  ENDIF()

  FILE(GLOB cvextern_test_srcs "./*.cpp")
  SOURCE_GROUP("Src" FILES ${cvextern_test_srcs})
  FILE(GLOB cvextern_test_hdrs "./*.h*")
  SOURCE_GROUP("Include" FILES ${cvextern_test_hdrs})

  SET(the_target ${PROJECT_NAME})

  ADD_EXECUTABLE(${the_target} ${cvextern_test_srcs} ${cvextern_test_hdrs})

  # For dynamic link numbering convenions
  SET_TARGET_PROPERTIES(${the_target} PROPERTIES
    OUTPUT_NAME "${the_target}"
    )

  SET_TARGET_PROPERTIES(${PROJECT_NAME} PROPERTIES CXX_STANDARD 11)
  IF(ENABLE_SOLUTION_FOLDERS)
    SET_TARGET_PROPERTIES(${the_target} PROPERTIES FOLDER "emgu")
  ENDIF()

  # Additional target properties
  SET_TARGET_PROPERTIES(${the_target} PROPERTIES
    DEBUG_POSTFIX "${OPENCV_DEBUG_POSTFIX}"
    ARCHIVE_OUTPUT_DIRECTORY "${CMAKE_BINARY_DIR}/lib${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER}"
    RUNTIME_OUTPUT_DIRECTORY "${CMAKE_BINARY_DIR}/bin${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER}"
    )

  FOREACH(CVEXTERN_OPTIONAL_DEP ${OPENCV_MODULE_NAME_CANDIDATES})
    IF(HAVE_opencv_${CVEXTERN_OPTIONAL_DEP})
      IF((DEFINED opencv_${CVEXTERN_OPTIONAL_DEP}_SOURCE_DIR) AND (EXISTS ${opencv_${CVEXTERN_OPTIONAL_DEP}_SOURCE_DIR}/include))
        INCLUDE_DIRECTORIES("${opencv_${CVEXTERN_OPTIONAL_DEP}_SOURCE_DIR}/include")
        MESSAGE(STATUS "Include directory for cvextern_test:  ${opencv_${CVEXTERN_OPTIONAL_DEP}_SOURCE_DIR}/include")
      ELSEIF(EXISTS "${OPENCV_MODULES_DIR}/${CVEXTERN_OPTIONAL_DEP}/include")
        INCLUDE_DIRECTORIES("${OPENCV_MODULES_DIR}/${CVEXTERN_OPTIONAL_DEP}/include")
        MESSAGE(STATUS "Include directory for cvextern_test:  ${OPENCV_MODULES_DIR}/${CVEXTERN_OPTIONAL_DEP}/include")
      ELSEIF(EXISTS "${OPENCV_EXTRA_MODULES_PATH}/${CVEXTERN_OPTIONAL_DEP}/include")
        INCLUDE_DIRECTORIES("${OPENCV_EXTRA_MODULES_PATH}/${CVEXTERN_OPTIONAL_DEP}/include")
        MESSAGE(STATUS "Include directory for cvextern_test:  ${OPENCV_EXTRA_MODULES_PATH}/${CVEXTERN_OPTIONAL_DEP}/include")
      ENDIF()
    ENDIF()
  ENDFOREACH()

  FOREACH(CVEXTERN_OPTIONAL_DEP ${OPENCV_MODULE_NAMES})
    MESSAGE(STATUS "CVEXTERN_TEST dependency added:  opencv_${CVEXTERN_OPTIONAL_DEP}")
    ADD_DEPENDENCIES(${the_target} opencv_${CVEXTERN_OPTIONAL_DEP})
    TARGET_LINK_LIBRARIES(${the_target} opencv_${CVEXTERN_OPTIONAL_DEP})
  ENDFOREACH()

  IF(EMGU_ENABLE_SSE)
    ADD_DEFINITIONS(-D__EMGU_ENABLE_SSE__)
    IF(MSVC AND EMGUCV_ARCH MATCHES "win32" AND (NOT CV_ICC))
      SET(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} /arch:SSE2")        ## Optimization
    ENDIF()
    IF((NOT WIN32) AND NOT ("${CMAKE_SYSTEM_NAME}" STREQUAL "Emscripten"))
      SET(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} -latomic")
    ENDIF()
  ENDIF()

  # Add the required libraries for linking:
  TARGET_LINK_LIBRARIES(${the_target} ${OPENCV_LINKER_LIBS})

  IF(WITH_PNG AND BUILD_PNG)
    TARGET_LINK_LIBRARIES(${the_target} libpng)
  ENDIF()

  IF(WITH_TIFF AND BUILD_TIFF)
    TARGET_LINK_LIBRARIES(${the_target} libtiff)
  ENDIF()

  IF(WITH_JPEG AND BUILD_JPEG)
    TARGET_LINK_LIBRARIES(${the_target} libjpeg-turbo)
  ENDIF()

  IF("${CMAKE_SYSTEM_NAME}" STREQUAL "Emscripten")
    SET_TARGET_PROPERTIES(${the_target} PROPERTIES SUFFIX ".html")
  ENDIF()

  IF(DEFINED EMGUCV_PLATFORM_TOOLSET)
    SET_TARGET_PROPERTIES(${the_target} PROPERTIES PLATFORM_TOOLSET ${EMGUCV_PLATFORM_TOOLSET})
  ENDIF()

  INSTALL(TARGETS ${the_target}
    RUNTIME DESTINATION bin${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER} COMPONENT main
    LIBRARY DESTINATION lib${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER} COMPONENT main
    ARCHIVE DESTINATION lib${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER} COMPONENT main)

ENDIF()
