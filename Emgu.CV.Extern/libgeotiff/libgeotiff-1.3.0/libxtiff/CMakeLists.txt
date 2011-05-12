###############################################################################
#
# Copyright (C) 2004-2011 by EMGU. All rights reserved.
#
# CMake configuration file to build libxtiff library
#
# Author: Canming
#
###############################################################################
PROJECT(xtiff)

SET(LIBGEOTIFF_DIR "${CMAKE_CURRENT_SOURCE_DIR}/..")
ADD_LIBRARY(${PROJECT_NAME} STATIC "${LIBGEOTIFF_DIR}/libxtiff/xtiff.c")
set_target_properties(${PROJECT_NAME} PROPERTIES FOLDER "3rdparty")
include_directories("${LIBGEOTIFF_DIR}")
