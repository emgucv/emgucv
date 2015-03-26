###############################################################################
#
# CMake main configuration file to build GeoTIFF library and utilities.
#
# Author: Mateusz Loskot <mateusz@loskot.net>
#
# Modified by Canming:
#               Changed all CMAKE_SOURCE_DIR to CMAKE_CURRENT_SOURCE_DIR
#               Build only geotiff_archieve for emgucv
# Dec 9, 2010   Not to include source or header file for emgucv installation
#
###############################################################################



###############################################################################
# Build libxtiff library
PROJECT(xtiff)
include_directories( ${TIFF_INCLUDE_DIR})
ADD_LIBRARY(${PROJECT_NAME} STATIC "${LIBGEOTIFF_DIR}/libxtiff/xtiff.c")
IF(DEFINED EMGUCV_PLATFORM_TOOLSET)
  set_target_properties(${PROJECT_NAME} PROPERTIES PLATFORM_TOOLSET ${EMGUCV_PLATFORM_TOOLSET})
ENDIF()
set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "3rdparty")

PROJECT(GeoTIFF)
SET(GEOTIFF_LIB_NAME geotiff)
#SET(GEOTIFF_LIBRARY_TARGET geotiff_library)
SET(GEOTIFF_ARCHIVE_TARGET geotiff_archive)

##############################################################################
# CMake settings
CMAKE_MINIMUM_REQUIRED(VERSION 2.6.0)

SET(CMAKE_COLOR_MAKEFILE ON)

# Allow advanced users to generate Makefiles printing detailed commands
MARK_AS_ADVANCED(CMAKE_VERBOSE_MAKEFILE)

# Path to additional CMake modules
SET(CMAKE_MODULE_PATH ${LIBGEOTIFF_DIR}/../cmake ${CMAKE_MODULE_PATH})

INCLUDE_DIRECTORIES("${LIBGEOTIFF_DIR}")

###############################################################################
# General build settings

IF(NOT CMAKE_BUILD_TYPE)
    SET(CMAKE_BUILD_TYPE Release CACHE STRING
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
SET(CMAKE_RUNTIME_OUTPUT_DIRECTORY ${GEOTIFF_BUILD_OUTPUT_DIRECTORY})

# Output directory in which to build LIBRARY target files
SET(CMAKE_LIBRARY_OUTPUT_DIRECTORY ${GEOTIFF_BUILD_OUTPUT_DIRECTORY})

# Output directory in which to build ARCHIVE target files.
SET(CMAKE_ARCHIVE_OUTPUT_DIRECTORY ${GEOTIFF_BUILD_OUTPUT_DIRECTORY}) 

###############################################################################
# Platform and compiler specific settings

        IF(NETFX_CORE)
          MESSAGE(STATUS "Disabling depreciation")
          ADD_DEFINITIONS(-D_CRT_SECURE_NO_WARNINGS)
          #ADD_DEFINITIONS(/sdl-)
          #target_compile_options(geotiff_archive PRIVATE -sdl-)
          #SET(CMAKE_C_FLAGS "${CMAKE_C_FLAGS} -sdl-")
          #SET(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} -sdl-")
        ENDIF()

IF(WIN32)
    IF(MSVC) 
        ADD_DEFINITIONS(-DBUILD_AS_DLL=1)
        ADD_DEFINITIONS(-W0)

#        IF(MSVC80)
#            ADD_DEFINITIONS(-D_CRT_SECURE_NO_WARNINGS)
#            ADD_DEFINITIONS(-D_CRT_NONSTDC_NO_WARNING)
#            ADD_DEFINITIONS(-D_CRT_SECURE_NO_DEPRECATE)
#        ENDIF()
    ENDIF(MSVC)
ENDIF()

IF(CMAKE_COMPILER_IS_GNUCC OR CMAKE_COMPILER_IS_GNUCXX)
    SET(COMPILE_FLAGS "-fPIC -Wall -Wno-long-long")
    SET(CMAKE_C_FLAGS "${CMAKE_C_FLAGS} ${COMPILE_FLAGS} -std=c99")
    SET(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} ${COMPILE_FLAGS} -std=c++98")
    IF(GEOTIFF_BUILD_PEDANTIC)
        SET(CMAKE_C_FLAGS "${CMAKE_C_FLAGS} -pedantic")
        SET(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} -pedantic")
    ENDIF()
ENDIF()

# Check required standard headers
INCLUDE(CheckIncludeFiles)
CHECK_INCLUDE_FILES(stdio.h HAVE_STDIO_H)
CHECK_INCLUDE_FILES(stdlib.h HAVE_STDLIB_H)
CHECK_INCLUDE_FILES(string.h HAVE_STRING_H)

###############################################################################
# User-defined build settings

#SET(GEOTIFF_CSV_NAMES area.csv datum.csv gcs.csv pcs.csv)
#FIND_PATH(GEOTIFF_CSV_DATA_DIR NAMES ${GEOTIFF_CSV_NAMES}
#    PATHS "${LIBGEOTIFF_DIR}/csv"
#    DOC "Default location of GeoTIFF CSV files"
#    NO_DEFAULT_PATH)
#MESSAGE("LIBGEOTIFF_DIR: ${LIBGEOTIFF_DIR}/csv")
#MESSAGE("GEOTIFF_CSV_DATA_DIR: ${GEOTIFF_CSV_DATA_DIR}")
	
#IF(IS_DIRECTORY ${GEOTIFF_CSV_DATA_DIR} AND EXISTS "${GEOTIFF_CSV_DATA_DIR}/gcs.csv")
#    MESSAGE(STATUS "Found GeoTIFF CSV files in: ${GEOTIFF_CSV_DATA_DIR}")
#    ADD_DEFINITIONS(-DCSV_DATA_DIR="${GEOTIFF_CSV_DATA_DIR}")
#ELSE()
#    MESSAGE(FATAL_ERROR "Failed to find GeoTIFF CSV files in: ${GEOTIFF_CSV_DATA_DIR}")
#ENDIF()

# Has the user requested "incode" EPSG tables, overriding the default
# use of EPSG tables in csv files?
SET(GEOTIFF_ENABLE_INCODE_EPSG FALSE CACHE BOOL
    "Choose if C code EPSG tables should be used")
MESSAGE(STATUS "Enable in-code GeoTIFF EPSG tables: ${GEOTIFF_ENABLE_INCODE_EPSG}")

###############################################################################
# Search for dependencies
INCLUDE_DIRECTORIES(${TIFF_INCLUDE_DIR})
ADD_DEFINITIONS(-DHAVE_TIFF=1)

INCLUDE_DIRECTORIES("${LIBGEOTIFF_DIR}/libxtiff")

# PROJ.4 support - optional, default=ON
SET(WITH_PROJ4 TRUE CACHE BOOL "Choose if PROJ.4 support should be built")

IF(WITH_PROJ4)
    FIND_PACKAGE(PROJ4)

    IF(PROJ4_FOUND)
        ADD_DEFINITIONS(-DHAVE_LIBPROJ=1)
        INCLUDE_DIRECTORIES(${PROJ4_INCLUDE_DIR})

        IF(EXISTS "${PROJ4_INCLUDE_DIR}/projects.h")
            MESSAGE(STATUS "Looking for projects.h header from PROJ.4 library - found")
            ADD_DEFINITIONS(-DHAVE_PROJECTS_H=1)
            INCLUDE_DIRECTORIES(${PROJ4_INCLUDE_DIR_2})
        ELSE()
            MESSAGE(FATAL_ERROR "Looking for projects.h from PROJ.4 library - not found")
        ENDIF()
    ENDIF()
ENDIF()

# Zlib support - optional, default=OFF
FIND_PACKAGE(ZLIB)
IF(NOT ZLIB_FOUND)
SET(ZLIB_INCLUDE_DIR "${LIBGEOTIFF_DIR}/../../opencv/3rdparty/include")
ENDIF()
SET(HAVE_ZIP 1)
INCLUDE_DIRECTORIES(${ZLIB_INCLUDE_DIR})
ADD_DEFINITIONS(-DHAVE_ZIP=${HAVE_ZIP})

# JPEG support - optional, default=OFF
FIND_PACKAGE(JPEG)
IF(NOT JPEG_FOUND)
  SET(JPEG_INCLUDE_DIR "${LIBGEOTIFF_DIR}/../../opencv/3rdparty/include")
ENDIF()
SET(HAVE_JPEG 1)
INCLUDE_DIRECTORIES(${JPEG_INCLUDE_DIR})
ADD_DEFINITIONS(-DHAVE_JPEG=${HAVE_JPEG})


###############################################################################
# Generate geo_config.h with compile-time configuration

MESSAGE(STATUS "Generating geo_config.h header")

CONFIGURE_FILE(${LIBGEOTIFF_DIR}/../cmake/geo_config.h.in
    ${LIBGEOTIFF_DIR}/geo_config.h)

MESSAGE(STATUS "Generating geo_config.h header - done")


###############################################################################
# Installation settings

SET(GEOTIFF_CSV_DATA
    csv/alias.csv
    csv/area.csv
    csv/change.csv
    csv/codes.csv
    csv/coordinate_axis.csv
    csv/coordinate_axis_name.csv
    csv/coordinate_operation.csv
    csv/coordinate_operation_method.csv
    csv/coordinate_operation_parameter.csv
    csv/coordinate_operation_parameter_value.csv
    csv/coordinate_operation_path.csv
    csv/coordinate_reference_system.csv
    csv/coordinate_system.csv
    csv/datum.csv
    csv/ellipsoid.csv
    csv/esri_datum_override.csv
    csv/gcs.csv
    csv/gcs.override.csv
    csv/naming_system.csv
    csv/pcs.csv
    csv/pcs.override.csv
    csv/prime_meridian.csv
    csv/projop_wparm.csv
    csv/stateplane.csv
    csv/unit_of_measure.csv
    csv/version_history.csv)

SET(GEOTIFF_LIB_HEADERS
    geotiff.h
    geotiffio.h
    geovalues.h
    geonames.h
    geokeys.h
    geo_config.h
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
    geokeys.inc)

IF(WIN32)
    SET(DEFAULT_LIB_SUBDIR lib)
    SET(DEFAULT_DATA_SUBDIR .)
    SET(DEFAULT_INCLUDE_SUBDIR include)

    IF(MSVC)
        SET(DEFAULT_BIN_SUBDIR bin)
    ELSE()
        SET(DEFAULT_BIN_SUBDIR .)
    ENDIF()
ELSE()
    # Common locatoins for Unix and Mac OS X
    SET(DEFAULT_BIN_SUBDIR bin)
    SET(DEFAULT_LIB_SUBDIR lib)
    SET(DEFAULT_DATA_SUBDIR share)
    SET(DEFAULT_INCLUDE_SUBDIR include)
ENDIF()

# Locations are changeable by user to customize layout of GeoTIFF installation
# (default values are platform-specIFic)
SET(GEOTIFF_BIN_SUBDIR ${DEFAULT_BIN_SUBDIR} CACHE STRING
    "Subdirectory where executables will be installed")
SET(GEOTIFF_LIB_SUBDIR ${DEFAULT_LIB_SUBDIR} CACHE STRING
    "Subdirectory where libraries will be installed")
SET(GEOTIFF_INCLUDE_SUBDIR ${DEFAULT_INCLUDE_SUBDIR} CACHE STRING
    "Subdirectory where header files will be installed")
SET(GEOTIFF_DATA_SUBDIR ${DEFAULT_DATA_SUBDIR} CACHE STRING
    "Subdirectory where data will be installed")

# Mark *_SUBDIR variables as advanced and dedicated to use by power-users only.
MARK_AS_ADVANCED(GEOTIFF_BIN_SUBDIR GEOTIFF_LIB_SUBDIR GEOTIFF_INCLUDE_SUBDIR GEOTIFF_DATA_SUBDIR)

# Full paths for the installation
SET(GEOTIFF_BIN_DIR ${GEOTIFF_BIN_SUBDIR})
SET(GEOTIFF_LIB_DIR ${GEOTIFF_LIB_SUBDIR})
SET(GEOTIFF_INCLUDE_DIR ${GEOTIFF_INCLUDE_SUBDIR})
SET(GEOTIFF_DATA_DIR ${GEOTIFF_DATA_SUBDIR})

# Install doc files
#INSTALL(FILES
#    AUTHORS ChangeLog COPYING INSTALL LICENSE README README_BIN README.WIN
#    DESTINATION ${GEOTIFF_DATA_DIR}/doc)

# Install CSV data files
#INSTALL(FILES ${GEOTIFF_CSV_DATA} DESTINATION ${GEOTIFF_DATA_DIR}/epsg_csv)

# Install header files for development distribution
#INSTALL(FILES ${GEOTIFF_LIB_HEADERS} DESTINATION ${GEOTIFF_INCLUDE_DIR})

###############################################################################
# Build libgeotiff library

SET(GEOTIFF_LIB_SOURCES
    "${LIBGEOTIFF_DIR}/cpl_serv.c"
    "${LIBGEOTIFF_DIR}/cpl_csv.c"
    "${LIBGEOTIFF_DIR}/geo_extra.c"
    "${LIBGEOTIFF_DIR}/geo_free.c"
    "${LIBGEOTIFF_DIR}/geo_get.c"
    "${LIBGEOTIFF_DIR}/geo_names.c"
    "${LIBGEOTIFF_DIR}/geo_new.c"
    "${LIBGEOTIFF_DIR}/geo_normalize.c"
    "${LIBGEOTIFF_DIR}/geo_print.c"
    "${LIBGEOTIFF_DIR}/geo_set.c"
    "${LIBGEOTIFF_DIR}/geo_simpletags.c"
    "${LIBGEOTIFF_DIR}/geo_tiffp.c"
    "${LIBGEOTIFF_DIR}/geo_trans.c"
    "${LIBGEOTIFF_DIR}/geo_write.c"
    "${LIBGEOTIFF_DIR}/geotiff_proj4.c")

IF (GEOTIFF_ENABLE_INCODE_EPSG)
    SET(GEOTIFF_LIB_CSV_SOURCES
        csv/datum.c
        csv/ellipsoid.c
        csv/gcs.c
        csv/pcs.c
        csv/prime_meridian.c
        csv/projop_wparm.c
        csv/unit_of_measure.c)
    #SOURCE_GROUP("CSV Source Files" FILES ${GEOTIFF_LIB_CSV_SOURCES})
ENDIF()

# Static libgeotiff archive 
ADD_LIBRARY(${GEOTIFF_ARCHIVE_TARGET} STATIC
    ${GEOTIFF_LIB_SOURCES} ${GEOTIFF_LIB_CSV_SOURCES})
SET_TARGET_PROPERTIES(${GEOTIFF_ARCHIVE_TARGET} PROPERTIES
    OUTPUT_NAME ${GEOTIFF_LIB_NAME})
IF(DEFINED EMGUCV_PLATFORM_TOOLSET)
  set_target_properties(${GEOTIFF_ARCHIVE_TARGET} PROPERTIES PLATFORM_TOOLSET ${EMGUCV_PLATFORM_TOOLSET})
ENDIF()
set_target_properties(${GEOTIFF_ARCHIVE_TARGET} PROPERTIES FOLDER "3rdparty")

# Shared libgeotiff library
#ADD_LIBRARY(${GEOTIFF_LIBRARY_TARGET} SHARED
#    ${GEOTIFF_LIB_SOURCES} ${GEOTIFF_LIB_CSV_SOURCES})

#IF(WIN32 AND MSVC)
#    SET_TARGET_PROPERTIES(${GEOTIFF_LIBRARY_TARGET} PROPERTIES IMPORT_SUFFIX "_i.lib")
#ENDIF()

#SET_TARGET_PROPERTIES(${GEOTIFF_LIBRARY_TARGET} PROPERTIES
#    OUTPUT_NAME ${GEOTIFF_LIB_NAME})

#TARGET_LINK_LIBRARIES(${GEOTIFF_LIBRARY_TARGET}
#	libtiff
#	${PROJ4_LIBRARIES}
#	${ZLIB_LIBRARIES}
#	${JPEG_LIBRARIES})

#INSTALL(TARGETS ${GEOTIFF_ARCHIVE_TARGET} #${GEOTIFF_LIBRARY_TARGET}
#    RUNTIME DESTINATION ${GEOTIFF_BIN_DIR}
#    LIBRARY DESTINATION ${GEOTIFF_LIB_DIR}
#    ARCHIVE DESTINATION ${GEOTIFF_LIB_DIR})

# Define grouping of source files in PROJECT file (e.g. Visual Studio)
#SOURCE_GROUP("CMake Files" FILES CMakeLists.txt)
#SOURCE_GROUP("Header Files" FILES ${GEOTIFF_LIB_HEADERS})
#SOURCE_GROUP("Source Files" FILES ${GEOTIFF_LIB_SOURCES})
