###############################################################################
#
# CMake configuration file to build GeoTIFF utility programs
#
# Author: Mateusz Loskot <mateusz@loskot.net>
#
###############################################################################

INCLUDE_DIRECTORIES(
    .
    ${CMAKE_SOURCE_DIR})

IF(WIN32 AND MSVC)
    SET(GETOPT_SOURCE getopt.c)
ENDIF()
###############################################################################
# Collect programs to build

SET(GEOTIFF_UTILITIES makegeo listgeo applygeo)

MESSAGE(STATUS "Adding GeoTIFF utilities to build")

FOREACH(utility ${GEOTIFF_UTILITIES})
    ADD_EXECUTABLE(${utility} ${utility}.c ${GETOPT_SOURCE})
    TARGET_LINK_LIBRARIES(${utility} xtiff ${GEOTIFF_LIBRARY_TARGET})
ENDFOREACH()

ADD_EXECUTABLE(geotifcp geotifcp.c ${GETOPT_SOURCE})
TARGET_LINK_LIBRARIES(geotifcp
    xtiff
    ${GEOTIFF_LIBRARY_TARGET}
    ${JPEG_LIBRARIES}
    ${ZLIB_LIBRARIES})

SET(GEOTIFF_UTILITIES ${GEOTIFF_UTILITIES} geotifcp )

MESSAGE(STATUS "Adding GeoTIFF utilities to build - done: ${GEOTIFF_UTILITIES}")

###############################################################################
# Targets installation

INSTALL(TARGETS ${GEOTIFF_UTILITIES}
    RUNTIME DESTINATION ${GEOTIFF_BIN_DIR}
    LIBRARY DESTINATION ${GEOTIFF_LIB_DIR}
    ARCHIVE DESTINATION ${GEOTIFF_LIB_DIR})
