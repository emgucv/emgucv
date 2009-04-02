project(emgucv)
cmake_minimum_required(VERSION 2.6)

set(CMAKE_MODULE_PATH ${CMAKE_SOURCE_DIR}/cmake/modules)
FIND_PACKAGE(CSharp REQUIRED)
INCLUDE (CSharpMacros)

set(LIBRARY_OUTPUT_PATH ${CMAKE_SOURCE_DIR}/bin)
set(LIBRARY_3RDPARTY_PATH ${CMAKE_SOURCE_DIR}/lib/3rdParty)

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
set_target_properties(highgui PROPERTIES PREFIX ${extra_bin_prefix})

IF(WIN32)
set_target_properties(opencv_ffmpeg PROPERTIES PREFIX ${extra_bin_prefix})
ENDIF()

#End building opencv and Emgu.CV.Extern

add_subdirectory(Emgu.Util)
add_subdirectory(Emgu.CV)
add_subdirectory(Emgu.CV.ML)
add_subdirectory(Emgu.CV.UI)

add_subdirectory(Emgu.CV.Example)

SET(CPACK_PACKAGE_DESCRIPTION_SUMMARY "Emgu CV")
SET(CPACK_GENERATOR "TGZ")
SET(CPACK_PACKAGE_VENDOR "Emgu")
SET(CPACK_PACKAGE_VERSION_MAJOR "2")
SET(CPACK_PACKAGE_VERSION_MINOR "0")
SET(CPACK_PACKAGE_VERSION_PATCH "0")
SET(CPACK_PACKAGE_FILE_NAME "Emgu.CV")
SET(CPACK_SOURCE_GENERATOR "TGZ")

INCLUDE (CPack)


