###############################################################################
#
# CMake main configuration file to build GeoTIFF library and utilities.
#
# Author: Mateusz Loskot <mateusz@loskot.net>
#
###############################################################################
PROJECT(GeoTIFF)

SET(GEOTIFF_LIB_NAME geotiff)
SET(GEOTIFF_LIBRARY_TARGET geotiff_library)
SET(GEOTIFF_ARCHIVE_TARGET geotiff_archive)

##############################################################################
# CMake settings
CMAKE_MINIMUM_REQUIRED(VERSION 2.6.0)

SET(CMAKE_COLOR_MAKEFILE ON)

# Allow advanced users to generate Makefiles printing detailed commands
MARK_AS_ADVANCED(CMAKE_VERBOSE_MAKEFILE)

# Path to additional CMake modules
SET(CMAKE_MODULE_PATH ${CMAKE_SOURCE_DIR}/cmake ${CMAKE_MODULE_PATH})

###############################################################################
# General build settings

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
SET(CMAKE_RUNTIME_OUTPUT_DIRECTORY ${GEOTIFF_BUILD_OUTPUT_DIRECTORY})

# Output directory in which to build LIBRARY target files
SET(CMAKE_LIBRARY_OUTPUT_DIRECTORY ${GEOTIFF_BUILD_OUTPUT_DIRECTORY})

# Output directory in which to build ARCHIVE target files.
SET(CMAKE_ARCHIVE_OUTPUT_DIRECTORY ${GEOTIFF_BUILD_OUTPUT_DIRECTORY}) 

###############################################################################
# Platform and compiler specific settings

IF(WIN32)
    IF(MSVC) 
        ADD_DEFINITIONS(-DBUILD_AS_DLL=1)
        ADD_DEFINITIONS(/DW4)

        IF(MSVC80)
            ADD_DEFINITIONS(-D_CRT_SECURE_NO_WARNINGS)
            ADD_DEFINITIONS(-D_CRT_NONSTDC_NO_WARNING)
            ADD_DEFINITIONS(-D_CRT_SECURE_NO_DEPRECATE)
        ENDIF()
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

SET(GEOTIFF_CSV_NAMES area.csv codes.csv datum.csv gcs.csv pcs.csv)
FIND_PATH(GEOTIFF_CSV_DATA_DIR NAMES ${GEOTIFF_CSV_NAMES}
    PATHS "${CMAKE_SOURCE_DIR}/csv"
    DOC "Default location of GeoTIFF CSV files"
    NO_DEFAULT_PATH)

IF(IS_DIRECTORY ${GEOTIFF_CSV_DATA_DIR} AND EXISTS "${GEOTIFF_CSV_DATA_DIR}/gcs.csv")
    MESSAGE(STATUS "Found GeoTIFF CSV files in: ${GEOTIFF_CSV_DATA_DIR}")
    ADD_DEFINITIONS(-DCSV_DATA_DIR="${GEOTIFF_CSV_DATA_DIR}")
ELSE()
    MESSAGE(FATAL_ERROR "Failed to find GeoTIFF CSV files in: ${GEOTIFF_CSV_DATA_DIR}")
ENDIF()

# Has the user requested "incode" EPSG tables, overriding the default
# use of EPSG tables in csv files?
SET(GEOTIFF_ENABLE_INCODE_EPSG FALSE CACHE BOOL
    "Choose if C code EPSG tables should be used")
MESSAGE(STATUS "Enable in-code GeoTIFF EPSG tables: ${GEOTIFF_ENABLE_INCODE_EPSG}")

SET(WITH_UTILITIES TRUE CACHE BOOL "Choose if GeoTIFF utilities should be built")

###############################################################################
# Search for dependencies

# TIFF support - required, default=ON
SET(WITH_TIFF TRUE CACHE BOOL "Choose if TIFF support should be built")

IF(WITH_TIFF)
    FIND_PACKAGE(TIFF REQUIRED)

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
        
        INCLUDE_DIRECTORIES(${TIFF_INCLUDE_DIR})
        ADD_DEFINITIONS(-DHAVE_TIFF=1)
    ENDIF(TIFF_FOUND)
ENDIF(WITH_TIFF)

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
SET(WITH_ZLIB FALSE CACHE BOOL "Choose if zlib support should be built")

IF(WITH_ZLIB)
    FIND_PACKAGE(ZLIB)

    IF(ZLIB_FOUND)
        SET(HAVE_ZIP 1)
        INCLUDE_DIRECTORIES(${ZLIB_INCLUDE_DIR})
        ADD_DEFINITIONS(-DHAVE_ZIP=${HAVE_ZIP})
    ENDIF()
ENDIF()

# JPEG support - optional, default=OFF
SET(WITH_JPEG FALSE CACHE BOOL "Choose if JPEG support should be built")

IF(WITH_JPEG)
    FIND_PACKAGE(JPEG)

    IF(JPEG_FOUND)
        SET(HAVE_JPEG 1)
        INCLUDE_DIRECTORIES(${JPEG_INCLUDE_DIR})
        ADD_DEFINITIONS(-DHAVE_JPEG=${HAVE_JPEG})
    ENDIF()
ENDIF()

###############################################################################
# Generate geo_config.h with compile-time configuration

MESSAGE(STATUS "Generating geo_config.h header")

CONFIGURE_FILE(${CMAKE_CURRENT_SOURCE_DIR}/cmake/geo_config.h.in
    ${CMAKE_CURRENT_SOURCE_DIR}/geo_config.h)

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
INSTALL(FILES
    AUTHORS ChangeLog COPYING INSTALL LICENSE README README_BIN README.WIN
    DESTINATION ${GEOTIFF_DATA_DIR}/doc)

# Install CSV data files
INSTALL(FILES ${GEOTIFF_CSV_DATA} DESTINATION ${GEOTIFF_DATA_DIR}/epsg_csv)

# Install header files for development distribution
INSTALL(FILES ${GEOTIFF_LIB_HEADERS} DESTINATION ${GEOTIFF_INCLUDE_DIR})

###############################################################################
# Build libxtiff library

ADD_SUBDIRECTORY(libxtiff)

###############################################################################
# Build libgeotiff library

SET(GEOTIFF_LIB_SOURCES
    cpl_serv.c
    cpl_csv.c
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

IF (GEOTIFF_ENABLE_INCODE_EPSG)
    SET(GEOTIFF_LIB_CSV_SOURCES
        csv/datum.c
        csv/ellipsoid.c
        csv/gcs.c
        csv/pcs.c
        csv/prime_meridian.c
        csv/projop_wparm.c
        csv/unit_of_measure.c)
    SOURCE_GROUP("CSV Source Files" FILES ${GEOTIFF_LIB_CSV_SOURCES})
ENDIF()

# Static libgeotiff archive 
ADD_LIBRARY(${GEOTIFF_ARCHIVE_TARGET} STATIC
    ${GEOTIFF_LIB_SOURCES} ${GEOTIFF_LIB_CSV_SOURCES})
SET_TARGET_PROPERTIES(${GEOTIFF_ARCHIVE_TARGET} PROPERTIES
    OUTPUT_NAME ${GEOTIFF_LIB_NAME})

# Shared libgeotiff library
ADD_LIBRARY(${GEOTIFF_LIBRARY_TARGET} SHARED
    ${GEOTIFF_LIB_SOURCES} ${GEOTIFF_LIB_CSV_SOURCES})

IF(WIN32 AND MSVC)
    SET_TARGET_PROPERTIES(${GEOTIFF_LIBRARY_TARGET} PROPERTIES IMPORT_SUFFIX "_i.lib")
ENDIF()

SET_TARGET_PROPERTIES(${GEOTIFF_LIBRARY_TARGET} PROPERTIES
    OUTPUT_NAME ${GEOTIFF_LIB_NAME})

TARGET_LINK_LIBRARIES(${GEOTIFF_LIBRARY_TARGET}
	${TIFF_LIBRARIES}
	${PROJ4_LIBRARIES}
	${ZLIB_LIBRARIES}
	${JPEG_LIBRARIES})

INSTALL(TARGETS ${GEOTIFF_ARCHIVE_TARGET} ${GEOTIFF_LIBRARY_TARGET}
    RUNTIME DESTINATION ${GEOTIFF_BIN_DIR}
    LIBRARY DESTINATION ${GEOTIFF_LIB_DIR}
    ARCHIVE DESTINATION ${GEOTIFF_LIB_DIR})

# Define grouping of source files in PROJECT file (e.g. Visual Studio)
SOURCE_GROUP("CMake Files" FILES CMakeLists.txt)
SOURCE_GROUP("Header Files" FILES ${GEOTIFF_LIB_HEADERS})
SOURCE_GROUP("Source Files" FILES ${GEOTIFF_LIB_SOURCES})

###############################################################################
# Build GeoTIFF utilities

IF(WITH_UTILITIES)
    ADD_SUBDIRECTORY(bin)
ENDIF()
