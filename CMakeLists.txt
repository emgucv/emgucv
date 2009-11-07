# --------------------------------------------------------
# Root CMake file for Emgu CV
#
# From the off-tree build directory, invoke:
#   $ cmake <PATH_TO_EMGUCV_ROOT>
#
# --------------------------------------------------------

project(emgucv)
cmake_minimum_required(VERSION 2.6)

set(CMAKE_MODULE_PATH ${CMAKE_SOURCE_DIR}/cmake/modules)
FIND_PACKAGE(CSharp REQUIRED)
INCLUDE (CSharpMacros)

set(LIBRARY_OUTPUT_PATH ${CMAKE_SOURCE_DIR}/bin)
set(LIBRARY_3RDPARTY_PATH ${CMAKE_SOURCE_DIR}/lib/3rdParty)

SET(PACKAGE "${PROJECT_NAME}")
SET(PACKAGE_BUGREPORT "emgucv@gmail.com")
SET(PACKAGE_NAME "${PROJECT_NAME}")

SET(CPACK_PACKAGE_NAME "${PROJECT_NAME}")
SET(CPACK_PACKAGE_DESCRIPTION_SUMMARY "Emgu CV is a cross platform .Net wrapper to the Intel OpenCV image processing library.")

SET(CPACK_PACKAGE_VENDOR "Emgu")
SET(CPACK_PACKAGE_VERSION_MAJOR "2")
SET(CPACK_PACKAGE_VERSION_MINOR "1")
SET(CPACK_PACKAGE_VERSION_PATCH "0")
SET(CPACK_PACKAGE_VERSION "${CPACK_PACKAGE_VERSION_MAJOR}.${CPACK_PACKAGE_VERSION_MINOR}.${CPACK_PACKAGE_VERSION_PATCH}")
SET(CPACK_PACKAGE_FILE_NAME "${PACKAGE_NAME}_${CPACK_PACKAGE_VERSION}")
SET(CPACK_PACKAGE_CONTACT "Canming Huang <emgucv@gmail.com>")
SET(CPACK_PACKAGE_CLI_FOLDER lib/cli/${PACKAGE_NAME}-${CPACK_PACKAGE_VERSION_MAJOR}.${CPACK_PACKAGE_VERSION_MINOR})

#IF(MSVC)
#	MESSAGE(STATUS "---------------------- TRYING TO FIND MD FLAG FROM ${CMAKE_CXX_FLAGS_RELEASE} -----------------")
#	IF (${CMAKE_CXX_FLAGS_RELEASE} MATCHES "/MD")
#		MESSAGE(STATUS "FOUND MD FLAG")
#		string(REGEX REPLACE "/MD" "/MT" CMAKE_CXX_FLAGS_RELEASE ${CMAKE_CXX_FLAGS_RELEASE})
#		SET(CMAKE_SHARED_LINKER_FLAGS_RELEASE "${CMAKE_SHARED_LINKER_FLAGS_RELEASE} /NODEFAULTLIB:\"LIBCMT\"" ) 
#            MESSAGE(STATUS "FLAG: ${CMAKE_SHARED_LINKER_FLAGS_RELEASE}" )
#      ENDIF()
#ENDIF()

#Build opencv and Emgu.CV.Extern
set(CMAKE_SOURCE_DIR ${CMAKE_SOURCE_DIR}/opencv)
add_subdirectory(opencv)
add_subdirectory(Emgu.CV.Extern)


if(MSVC)
set(extra_bin_prefix "../")
else(MSVC)
set(extra_bin_prefix "../bin/")
endif()

set_target_properties(cvextern PROPERTIES PREFIX ${extra_bin_prefix})
set_target_properties(cxcore PROPERTIES PREFIX ${extra_bin_prefix})
set_target_properties(cv PROPERTIES PREFIX ${extra_bin_prefix})
set_target_properties(cvaux PROPERTIES PREFIX ${extra_bin_prefix})
set_target_properties(ml PROPERTIES PREFIX ${extra_bin_prefix})

if(highgui)
  set_target_properties(highgui PROPERTIES PREFIX ${extra_bin_prefix})
endif()

if(opencv_ffmpeg)
  set_target_properties(opencv_ffmpeg PROPERTIES PREFIX ${extra_bin_prefix})
endif ()

#End building opencv and Emgu.CV.Extern

if (WIN32)
  #As default, do not build EMGU on Windows. It can be build using Visual Studio
  set(EMGU_CV_BUILD OFF CACHE BOOL "Build Emgu CV")
  set(EMGU_CV_EXAMPLE_BUILD OFF CACHE BOOL "Build Emgu CV Example")
else (WIN32)
  #As default, build Emgu CV on Linux
  set(EMGU_CV_BUILD ON CACHE BOOL "BUILD Emgu CV")
  set(EMGU_CV_EXAMPLE_BUILD ON CACHE BOOL "Build Emgu CV Example")
endif ()

add_subdirectory(Emgu.Util)
add_subdirectory(Emgu.CV)
add_subdirectory(Emgu.CV.UI)
add_subdirectory(Emgu.CV.ML)

add_subdirectory(Emgu.CV.Example)


#SET(CPACK_GENERATOR "TGZ")
#SET(CPACK_SOURCE_GENERATOR "TGZ")

#Debian specific info
SET(DEBIAN_PACKAGE_PRIORITY "optional")
SET(DEBIAN_CONTROL_FILE_FOLDER "${CMAKE_CURRENT_SOURCE_DIR}/cmake/debian")
SET(CPACK_DEBIAN_PACKAGE_CONTROL_EXTRA "${DEBIAN_CONTROL_FILE_FOLDER}/postinst;${DEBIAN_CONTROL_FILE_FOLDER}/postrm")
SET(CPACK_DEBIAN_PACKAGE_DEPENDS "mono-runtime (>> 2.4.2), libmono-wcf3.0-cil")

INCLUDE (CPack)
