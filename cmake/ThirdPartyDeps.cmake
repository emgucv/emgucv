# --------------------------------------------------------
#  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
#
#  FreeType/HarfBuzz font rendering support and OpenVINO dependency probing.
#  Included from the root CMakeLists.txt; CMAKE_CURRENT_SOURCE_DIR still
#  refers to the repository root here (include() does not change it).
# ----------------------------------------------------------------------------

IF(IS_UBUNTU OR IS_RHEL)
  #USE system freetype, do nothing here.
ELSEIF (EMGU_CV_WITH_FREETYPE)
  MESSAGE(STATUS "Setting up FREETYPE")
  find_package(FREETYPE CONFIG)
  if(NOT FREETYPE_FOUND)
    message(STATUS "FREETYPE:   NO")
  else()
    message(STATUS "FREETYPE:   ${FREETYPE_DIR} (ver ${FREETYPE_VERSION})")
    SET(FREETYPE_INCLUDE_DIRS "${FREETYPE_DIR}/../../../include/freetype2")
    SET(FREETYPE_LIBRARY freetype)
    get_target_property(FREETYPE_LIBRARIES freetype IMPORTED_LOCATION_RELEASE)
    SET(FREETYPE_LINK_LIBRARIES ${FREETYPE_LIBRARIES})
    MESSAGE(STATUS "FREETYPE_LIBRARIES: ${FREETYPE_LIBRARIES}")
  endif()
  find_package(HARFBUZZ CONFIG)
  if(NOT HARFBUZZ_FOUND)
    message(STATUS "HARFBUZZ:    NO")
  else()
    message(STATUS "HARFBUZZ:    ${HARFBUZZ_DIR} (ver ${HARFBUZZ_VERSION})")
    SET(HARFBUZZ_INCLUDE_DIRS "${HARFBUZZ_DIR}/../../../include/harfbuzz")
    get_target_property(HARFBUZZ_LIBRARY harfbuzz::harfbuzz IMPORTED_LOCATION_RELEASE)
    SET(HARFBUZZ_LIBRARIES ${HARFBUZZ_LIBRARY})
    SET(HARFBUZZ_LINK_LIBRARIES ${HARFBUZZ_LIBRARY})
    MESSAGE(STATUS "HARFBUZZ_LIBRARIES: ${HARFBUZZ_LIBRARIES}")
    include_directories(${HARFBUZZ_INCLUDE_DIRS})
  endif()
ELSE()
  SET(FREETYPE_LIBRARY "")
ENDIF()

IF(WITH_OPENVINO)
  find_package(TBB CONFIG)
  find_package(OpenVINO CONFIG)
  find_package(ngraph CONFIG)
ENDIF()
