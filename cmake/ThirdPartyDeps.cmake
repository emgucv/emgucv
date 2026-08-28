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
    # opencv_contrib's freetype module detects us via ocv_check_modules(FREETYPE
    # freetype2), which -- because no pkg-config executable exists on this
    # machine -- skips pkg_check_modules() entirely and just inherits the
    # FREETYPE_FOUND/FREETYPE_LIBRARIES already set above. But that same macro
    # unconditionally re-derives FREETYPE_LIBRARIES from FREETYPE_LDFLAGS
    # afterwards (OpenCVUtils.cmake's ocv_check_modules, the
    # "${define}_FOUND AND ${define}_LIBRARIES" block), rebuilding it from an
    # LDFLAGS-flag parse loop. Leaving FREETYPE_LDFLAGS unset makes that loop
    # iterate over nothing, silently overwriting our correct FREETYPE_LIBRARIES
    # with an empty CACHE INTERNAL value right before the module's own
    # ocv_target_link_libraries() call -- reproduced directly: freetype.lib
    # and harfbuzz.lib were absent from opencv_world.vcxproj's
    # AdditionalDependencies, causing LNK2019 for every FT_*/hb_* symbol at
    # opencv_world link time (BUILD_opencv_world is forced on for -Cuda
    # builds). The loop treats an IS_ABSOLUTE flag as a literal library path,
    # so pre-seeding FREETYPE_LDFLAGS with the same absolute .lib path makes
    # the re-derivation reproduce the identical value instead of wiping it.
    SET(FREETYPE_LDFLAGS ${FREETYPE_LIBRARIES})
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
    # Same ocv_check_modules(HARFBUZZ harfbuzz) re-derivation gap as FREETYPE_LDFLAGS above.
    SET(HARFBUZZ_LDFLAGS ${HARFBUZZ_LIBRARIES})
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
