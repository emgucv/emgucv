###############################################################################
#
# CMake main configuration file to build GeoTIFF library and utilities.
#
# Author: Mateusz Loskot <mateusz@loskot.net>
#
###############################################################################
CMAKE_MINIMUM_REQUIRED(VERSION 3.13)

PROJECT(GeoTIFF)

include(GNUInstallDirs)

SET(GEOTIFF_LIB_NAME geotiff)
SET(GEOTIFF_LIBRARY_TARGET geotiff_library)

##############################################################################
# CMake settings
SET(CMAKE_COLOR_MAKEFILE ON)

# Version information
set(LIBGEOTIFF_MAJOR_VERSION 1)
set(LIBGEOTIFF_MINOR_VERSION 7)
set(LIBGEOTIFF_PATCH_VERSION 4)
set(LIBGEOTIFF_REV_VERSION 0)

# Set library version to match that of autoconf:
# libgeotiff.so -> libgeotiff.so.5
# libgeotiff.so.5 -> libgeotiff.so.5.2.4
# libgeotiff.so.5.2.4
set(LINK_SOVERSION "5")
set(LINK_VERSION "5.2.4")

set(PROJECT_VERSION_MAJOR ${LIBGEOTIFF_MAJOR_VERSION})
set(PROJECT_VERSION_MINOR ${LIBGEOTIFF_MINOR_VERSION})
set(PROJECT_VERSION_PATCH ${LIBGEOTIFF_PATCH_VERSION})
set(PROJECT_VERSION
  "${PROJECT_VERSION_MAJOR}.${PROJECT_VERSION_MINOR}.${PROJECT_VERSION_PATCH}")

set(LIBGEOTIFF_STRING_VERSION "${PROJECT_VERSION}")
set(LIBGEOTIFF_VERSION "${LIBGEOTIFF_MAJOR_VERSION}${LIBGEOTIFF_MINOR_VERSION}${LIBGEOTIFF_PATCH_VERSION}${LIBGEOTIFF_REV_VERSION}")

string (TOLOWER ${PROJECT_NAME} PROJECT_NAME_LOWER)
string (TOUPPER ${PROJECT_NAME} PROJECT_NAME_UPPER)

configure_file(geotiff.h.in ${CMAKE_CURRENT_BINARY_DIR}/geotiff.h @ONLY)

# Allow advanced users to generate Makefiles printing detailed commands
MARK_AS_ADVANCED(CMAKE_VERBOSE_MAKEFILE)

# Path to additional CMake modules
SET(CMAKE_MODULE_PATH ${CMAKE_SOURCE_DIR}/cmake ${CMAKE_MODULE_PATH})

###############################################################################
# General build settings

option(BUILD_SHARED_LIBS "Set ON to build shared library" ON)
option(BUILD_MAN "Set ON to build man pages" ON)
option(BUILD_DOC "Set ON to build doc files" ON)

IF(NOT CMAKE_BUILD_TYPE)
    SET(CMAKE_BUILD_TYPE Debug CACHE STRING
        "Choose the type of build, options are: None Debug Release RelWithDebInfo MinSizeRel"
        FORCE)
ENDIF()

SET(GEOTIFF_BUILD_PEDANTIC FALSE CACHE BOOL "Choose compilation in pedantic or relaxed mode")
IF(CMAKE_BUILD_TYPE MATCHES Debug)
    SET(GEOTIFF_BUILD_PEDANTIC TRUE)
ENDIF()

# TODO: Still testing the output paths --mloskot
SET(GEOTIFF_BUILD_OUTPUT_DIRECTORY ${PROJECT_BINARY_DIR}/bin)

# Output directory in which to build RUNTIME target files.
# SET(CMAKE_RUNTIME_OUTPUT_DIRECTORY ${GEOTIFF_BUILD_OUTPUT_DIRECTORY})
SET(CMAKE_RUNTIME_OUTPUT_DIRECTORY ${PROJECT_BINARY_DIR}/bin)

# Output directory in which to build LIBRARY target files
# SET(CMAKE_LIBRARY_OUTPUT_DIRECTORY ${GEOTIFF_BUILD_OUTPUT_DIRECTORY})
SET(CMAKE_LIBRARY_OUTPUT_DIRECTORY ${PROJECT_BINARY_DIR}/lib)

# Output directory in which to build ARCHIVE target files.
# SET(CMAKE_ARCHIVE_OUTPUT_DIRECTORY ${GEOTIFF_BUILD_OUTPUT_DIRECTORY})
SET(CMAKE_ARCHIVE_OUTPUT_DIRECTORY ${PROJECT_BINARY_DIR}/lib)



# Check required standard headers
INCLUDE(CheckIncludeFiles)
CHECK_INCLUDE_FILES(strings.h GEOTIFF_HAVE_STRINGS_H)

###############################################################################
# User-defined build settings

option(WITH_UTILITIES "Choose if GeoTIFF utilities should be built" ON)

if (MSVC)
    SET(INTERFACE_LIB_PREFIX _i CACHE STRING "Windows interface library suffix")
endif (MSVC)

###############################################################################
# Search for dependencies


# TIFF support - required, default=ON
option(WITH_TIFF "Choose if TIFF support should be built" ON)

FIND_PACKAGE(PROJ NO_MODULE QUIET)
if (NOT PROJ_FOUND)
  FIND_PACKAGE(PROJ REQUIRED)
endif ()

# Zlib support - optional, default=OFF
option(WITH_ZLIB "Choose if zlib support should be built" OFF)

# JPEG support - optional, default=OFF
option(WITH_JPEG  "Choose if JPEG support should be built" OFF)


# Turn off TOWGS84 support
option(WITH_TOWGS84 "Build with TOWGS84 support" ON)
IF (NOT WITH_TOWGS84)
   SET(GEO_NORMALIZE_DISABLE_TOWGS84 1)
endif()

###############################################################################
# Generate geo_config.h with compile-time configuration

MESSAGE(STATUS "Generating geo_config.h header")

CONFIGURE_FILE (${CMAKE_CURRENT_SOURCE_DIR}/cmake/geo_config.h.in
  geo_config.h)
INCLUDE_DIRECTORIES(BEFORE ${CMAKE_CURRENT_BINARY_DIR})

MESSAGE(STATUS "Generating geo_config.h header - done")


###############################################################################
# Installation settings

SET(GEOTIFF_LIB_HEADERS
    ${CMAKE_CURRENT_BINARY_DIR}/geotiff.h
    geotiffio.h
    geovalues.h
    geonames.h
    geokeys.h
    ${CMAKE_CURRENT_BINARY_DIR}/geo_config.h
    geo_tiffp.h
    geo_keyp.h
    geo_normalize.h
    cpl_serv.h
    geo_simpletags.h
    epsg_datum.inc
    epsg_gcs.inc
    epsg_pm.inc
    epsg_units.inc
    geo_ctrans.inc
    epsg_ellipse.inc
    epsg_pcs.inc
    epsg_proj.inc
    epsg_vertcs.inc
    geokeys.inc
    geokeys_v1_1.inc
    libxtiff/xtiffio.h)

SET(GEOTIFF_MAN_PAGES
    man/man1/applygeo.1
    man/man1/geotifcp.1
    man/man1/listgeo.1)

# Install doc files
if(BUILD_DOC)
  INSTALL(FILES
    AUTHORS ChangeLog COPYING LICENSE README README_BIN README.WIN
    DESTINATION ${CMAKE_INSTALL_DOCDIR})
endif ()

# Install man pages
if(BUILD_MAN)
  INSTALL(FILES ${GEOTIFF_MAN_PAGES} DESTINATION ${CMAKE_INSTALL_MANDIR}/man1)
endif ()

# Install header files for development distribution
INSTALL(FILES ${GEOTIFF_LIB_HEADERS} DESTINATION ${CMAKE_INSTALL_INCLUDEDIR})


###############################################################################
# Build libgeotiff library

SET(GEOTIFF_LIB_SOURCES
    cpl_serv.c
    geo_extra.c
    geo_free.c
    geo_get.c
    geo_names.c
    geo_new.c
    geo_normalize.c
    geo_print.c
    geo_set.c
    geo_simpletags.c
    geo_tiffp.c
    geo_trans.c
    geo_write.c
    geotiff_proj4.c)

SET(XTIFF_SOURCES libxtiff/xtiff.c)

if (MSVC OR CMAKE_CONFIGURATION_TYPES)
  # For multi-config systems and for Visual Studio, the debug versions
  # of the libraries have a _d suffix.
  set (CMAKE_DEBUG_POSTFIX _d)
endif ()

ADD_LIBRARY(${GEOTIFF_LIBRARY_TARGET}
    ${GEOTIFF_LIB_SOURCES} ${XTIFF_SOURCES})

set_property(TARGET ${GEOTIFF_LIBRARY_TARGET} PROPERTY CXX_STANDARD 98)
set_property(TARGET ${GEOTIFF_LIBRARY_TARGET} PROPERTY C_STANDARD 99)
set_property(TARGET ${GEOTIFF_LIBRARY_TARGET} PROPERTY POSITION_INDEPENDENT_CODE ON)
set_property(TARGET ${GEOTIFF_LIBRARY_TARGET} PROPERTY OUTPUT_NAME ${GEOTIFF_LIB_NAME})

IF(WITH_JPEG)
    FIND_PACKAGE(JPEG NO_MODULE QUIET)
    if (NOT JPEG_FOUND)
      FIND_PACKAGE(JPEG)
    endif ()

    IF(JPEG_FOUND)
        SET(HAVE_JPEG 1)
        TARGET_INCLUDE_DIRECTORIES(${GEOTIFF_LIBRARY_TARGET} PRIVATE ${JPEG_INCLUDE_DIR})
        target_compile_definitions(${GEOTIFF_LIBRARY_TARGET} PRIVATE -DHAVE_JPEG=${HAVE_JPEG})
    ENDIF()
ENDIF()


IF(WITH_TIFF)
    FIND_PACKAGE(TIFF NO_MODULE QUIET)
    if (NOT TIFF_FOUND)
      FIND_PACKAGE(TIFF REQUIRED)
    endif ()

    IF(TIFF_FOUND)
        # Confirm required API is available
        INCLUDE(CheckFunctionExists)
        SET(CMAKE_REQUIRED_LIBRARIES ${TIFF_LIBRARIES})

        CHECK_FUNCTION_EXISTS(TIFFOpen HAVE_TIFFOPEN)
        IF(NOT HAVE_TIFFOPEN)
            SET(TIFF_FOUND) # ReSET to NOT found for TIFF library
            MESSAGE(FATAL_ERROR "Failed to link with libtiff - TIFFOpen function not found")
        ENDIF()

        CHECK_FUNCTION_EXISTS(TIFFMergeFieldInfo HAVE_TIFFMERGEFIELDINFO)
        IF(NOT HAVE_TIFFMERGEFIELDINFO)
            SET(TIFF_FOUND) # ReSET to NOT found for TIFF library
            MESSAGE(FATAL_ERROR "Failed to link with libtiff - TIFFMergeFieldInfo function not found. libtiff 3.6.0 Beta or later required. Please upgrade or use an older version of libgeotiff")
        ENDIF()

        TARGET_INCLUDE_DIRECTORIES(${GEOTIFF_LIBRARY_TARGET} PRIVATE ${TIFF_INCLUDE_DIR})
        TARGET_COMPILE_DEFINITIONS(${GEOTIFF_LIBRARY_TARGET} PRIVATE -DHAVE_TIFF=1)
    ENDIF(TIFF_FOUND)
ENDIF(WITH_TIFF)

IF(WITH_ZLIB)
    FIND_PACKAGE(ZLIB REQUIRED )
    IF(ZLIB_FOUND)
        SET(HAVE_ZIP 1)
        TARGET_INCLUDE_DIRECTORIES(${GEOTIFF_LIBRARY_TARGET} PRIVATE ${ZLIB_INCLUDE_DIR})
        TARGET_COMPILE_DEFINITIONS(${GEOTIFF_LIBRARY_TARGET} PRIVATE -DHAVE_ZIP=${HAVE_ZIP})
    ENDIF()
ENDIF()

TARGET_INCLUDE_DIRECTORIES(${GEOTIFF_LIBRARY_TARGET} PUBLIC
                            $<BUILD_INTERFACE:${PROJECT_SOURCE_DIR}/>
                            $<BUILD_INTERFACE:${PROJECT_SOURCE_DIR}/libxtiff>
                            $<INSTALL_INTERFACE:${CMAKE_INSTALL_INCLUDEDIR}>)
IF(WIN32 AND MSVC)
    SET_TARGET_PROPERTIES(${GEOTIFF_LIBRARY_TARGET} PROPERTIES IMPORT_SUFFIX "${INTERFACE_LIB_PREFIX}.lib")
ENDIF(WIN32 AND MSVC)

# Unix, linux:
if(UNIX)
   SET_TARGET_PROPERTIES(
      ${GEOTIFF_LIBRARY_TARGET}
      PROPERTIES
      VERSION ${LINK_VERSION}
      SOVERSION ${LINK_SOVERSION}
      CLEAN_DIRECT_OUTPUT 1 )
   if (APPLE)
      set_target_properties(
        ${GEOTIFF_LIBRARY_TARGET}
        PROPERTIES
        INSTALL_NAME_DIR "${CMAKE_INSTALL_PREFIX}/${CMAKE_INSTALL_LIBDIR}")
   endif()
endif()

###############################################################################
# Platform and compiler specific settings

IF(WIN32)
    IF(BUILD_SHARED_LIBS)
        target_compile_definitions(${GEOTIFF_LIBRARY_TARGET} PRIVATE -DBUILD_AS_DLL=1)
    ENDIF()
    IF(MSVC)
        target_compile_definitions(${GEOTIFF_LIBRARY_TARGET} PRIVATE /DW4)
        if (NOT (MSVC_VERSION VERSION_LESS 1400))
            target_compile_definitions(${GEOTIFF_LIBRARY_TARGET} PRIVATE -D_CRT_SECURE_NO_DEPRECATE)
            target_compile_definitions(${GEOTIFF_LIBRARY_TARGET} PRIVATE -D_CRT_SECURE_NO_WARNINGS)
            target_compile_definitions(${GEOTIFF_LIBRARY_TARGET} PRIVATE -D_CRT_NONSTDC_NO_WARNING)
            target_compile_definitions(${GEOTIFF_LIBRARY_TARGET} PRIVATE -D_SCL_SECURE_NO_WARNINGS)
        endif()
    ENDIF(MSVC)
ENDIF()

IF(CMAKE_COMPILER_IS_GNUCC OR CMAKE_COMPILER_IS_GNUCXX)
    target_compile_options(${GEOTIFF_LIBRARY_TARGET} PRIVATE -Wall -Wno-long-long )
    IF(GEOTIFF_BUILD_PEDANTIC)
        target_compile_options(${GEOTIFF_LIBRARY_TARGET} PRIVATE -pedantic)
    ENDIF()
ENDIF()


set(CONFIG_PUBLIC_DEPENDENCIES "")
set(CONFIG_PRIVATE_DEPENDENCIES "")
if(TARGET TIFF::TIFF)
    set(TIFF_LIBRARIES TIFF::TIFF)
    string(APPEND CONFIG_PUBLIC_DEPENDENCIES "find_dependency(TIFF)\n")
endif()
target_link_libraries(${GEOTIFF_LIBRARY_TARGET} PUBLIC
    ${TIFF_LIBRARIES})

if(TARGET PROJ::proj)
    set(PROJ_LIBRARIES PROJ::proj)
    string(APPEND CONFIG_PRIVATE_DEPENDENCIES "  find_dependency(PROJ)\n")
endif()

if(TARGET ZLIB::zlib)
    set(ZLIB_LIBRARIES ZLIB::zlib)
    string(APPEND CONFIG_PRIVATE_DEPENDENCIES "  find_dependency(ZLIB)\n")
endif()

target_link_libraries(${GEOTIFF_LIBRARY_TARGET} PRIVATE
                      ${PROJ_LIBRARIES}
                      ${TIFF_LIBRARIES}
                      ${ZLIB_LIBRARIES})

target_include_directories( ${GEOTIFF_LIBRARY_TARGET} PUBLIC
            $<BUILD_INTERFACE:${PROJECT_SOURCE_DIR}>
            $<INSTALL_INTERFACE:${CMAKE_INSTALL_INCLUDEDIR}>)

INSTALL( TARGETS ${GEOTIFF_LIBRARY_TARGET}
     EXPORT depends
     RUNTIME DESTINATION ${CMAKE_INSTALL_BINDIR}
     LIBRARY DESTINATION ${CMAKE_INSTALL_LIBDIR}
     PUBLIC_HEADER DESTINATION ${CMAKE_INSTALL_INCLUDEDIR}
     ARCHIVE DESTINATION ${CMAKE_INSTALL_LIBDIR} )

IF (MSVC)
IF (BUILD_SHARED_LIBS)
    INSTALL(FILES $<TARGET_PDB_FILE:${GEOTIFF_LIBRARY_TARGET}> DESTINATION ${CMAKE_INSTALL_BINDIR} OPTIONAL)
ENDIF (BUILD_SHARED_LIBS)
ENDIF (MSVC)

# Install libgeotiff.pc
set(prefix "${CMAKE_INSTALL_PREFIX}")
set(exec_prefix "\${prefix}")
if(IS_ABSOLUTE "${CMAKE_INSTALL_LIBDIR}")
    set(libdir "${CMAKE_INSTALL_LIBDIR}")
else()
    set(libdir "\${exec_prefix}/${CMAKE_INSTALL_LIBDIR}")
endif()
if(IS_ABSOLUTE "${CMAKE_INSTALL_INCLUDEDIR}")
    set(includedir "${CMAKE_INSTALL_INCLUDEDIR}")
else()
    set(includedir "\${prefix}/${CMAKE_INSTALL_INCLUDEDIR}")
endif()
SET(PACKAGE_NAME ${PROJECT_NAME})
SET(VERSION ${PROJECT_VERSION})
configure_file(libgeotiff.pc.in ${CMAKE_CURRENT_BINARY_DIR}/libgeotiff.pc @ONLY)

INSTALL(
    FILES ${CMAKE_CURRENT_BINARY_DIR}/libgeotiff.pc
    DESTINATION "${CMAKE_INSTALL_LIBDIR}/pkgconfig")

# Define grouping of source files in PROJECT file (e.g. Visual Studio)
SOURCE_GROUP("CMake Files" FILES CMakeLists.txt)
SOURCE_GROUP("Header Files" FILES ${GEOTIFF_LIB_HEADERS})
SOURCE_GROUP("Source Files" FILES ${GEOTIFF_LIB_SOURCES})

###############################################################################
# Build GeoTIFF utilities

IF(WITH_UTILITIES)
    ADD_SUBDIRECTORY(bin)
ENDIF()

ADD_SUBDIRECTORY(cmake)

message(STATUS "
################################
Summary of build options:
   Build shared library:     ${BUILD_SHARED_LIBS}
   Build man pages:          ${BUILD_MAN}
   Build doc files:          ${BUILD_DOC}
   Build GeoTIFF utilities:  ${WITH_UTILITIES}
   Build TIFF support:       ${WITH_TIFF}
   Build zlib support:       ${WITH_ZLIB}
   Build JPEG support:       ${WITH_JPEG}
   Build TOWGS84 support:    ${WITH_TOWGS84}
################################")
