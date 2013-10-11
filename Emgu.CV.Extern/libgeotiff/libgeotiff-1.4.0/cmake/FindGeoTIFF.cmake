###############################################################################
#
# CMake module to search for GeoTIFF library
#
# On success, the macro sets the following variables:
# GEOTIFF_FOUND       = if the library found
# GEOTIFF_LIBRARIES   = full path to the library
# GEOTIFF_INCLUDE_DIR = where to find the library headers 
# also defined, but not for general use are
# GEOTIFF_LIBRARY, where to find the PROJ.4 library.
#
# Copyright (c) 2009 Mateusz Loskot <mateusz@loskot.net>
#
# Module source: http://github.com/mloskot/workshop/tree/master/cmake/
#
# Redistribution and use is allowed according to the terms of the BSD license.
# For details see the accompanying COPYING-CMAKE-SCRIPTS file.
#
###############################################################################

SET(GEOTIFF_NAMES geotiff)

IF(WIN32)

    IF(MINGW)
        FIND_PATH(GEOTIFF_INCLUDE_DIR
            geotiff.h
            PATH_PREFIXES geotiff
            PATHS
            /usr/local/include
            /usr/include
            c:/msys/local/include)

        FIND_LIBRARY(GEOTIFF_LIBRARY
            NAMES ${GEOTIFF_NAMES}
            PATHS
            /usr/local/lib
            /usr/lib
            c:/msys/local/lib)
    ENDIF(MINGW)

    IF(MSVC)
        SET(GEOTIFF_INCLUDE_DIR "$ENV{LIB_DIR}/include" CACHE STRING INTERNAL)

        SET(GEOTIFF_NAMES ${GEOTIFF_NAMES} geotiff_i)
        FIND_LIBRARY(GEOTIFF_LIBRARY NAMES 
            NAMES ${GEOTIFF_NAMES}
            PATHS
            "$ENV{LIB_DIR}/lib"
            /usr/lib
            c:/msys/local/lib)
    ENDIF(MSVC)
  
ELSEIF(UNIX)

    FIND_PATH(GEOTIFF_INCLUDE_DIR geotiff.h PATH_PREFIXES geotiff)

    FIND_LIBRARY(GEOTIFF_LIBRARY NAMES ${GEOTIFF_NAMES})

ELSE()
    MESSAGE("FindGeoTIFF.cmake: unrecognized or unsupported operating system")
ENDIF()

IF(GEOTIFF_FOUND)
  SET(GEOTIFF_LIBRARIES ${GEOTIFF_LIBRARY})
ENDIF()

# Handle the QUIETLY and REQUIRED arguments and set SPATIALINDEX_FOUND to TRUE
# if all listed variables are TRUE
INCLUDE(FindPackageHandleStandardArgs)
FIND_PACKAGE_HANDLE_STANDARD_ARGS(GEOTIFF DEFAULT_MSG GEOTIFF_LIBRARY GEOTIFF_INCLUDE_DIR)
