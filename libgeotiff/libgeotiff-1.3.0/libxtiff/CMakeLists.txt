###############################################################################
#
# CMake configuration file to build libxtiff library
#
# Author: Canming
#
###############################################################################
SET(LIBGEOTIFF_DIR "${CMAKE_CURRENT_SOURCE_DIR}/..")
ADD_LIBRARY(xtiff STATIC "${LIBGEOTIFF_DIR}/libxtiff/xtiff.c")
include_directories("${LIBGEOTIFF_DIR}")
