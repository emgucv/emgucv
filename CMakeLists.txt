cmake_minimum_required(VERSION 2.6)

#set(DESIRED_CSHARP_COMPILER_VERSION 2)
set(CMAKE_MODULE_PATH ${CMAKE_SOURCE_DIR}/cmake/modules)
FIND_PACKAGE(CSharp REQUIRED)
INCLUDE (CSharpMacros)

#SET(CMAKE_INCLUDE_CURRENT_DIR ON)

project(emgucv)
set(LIBRARY_OUTPUT_PATH ${CMAKE_SOURCE_DIR}/bin)
set(LIBRARY_3RDPARTY_PATH ${CMAKE_SOURCE_DIR}/lib/3rdParty)
#set(CMAKE_CFG_INTDIR "")

add_subdirectory(Emgu.Util)
add_subdirectory(Emgu.CV)
add_subdirectory(Emgu.CV.ML)
add_subdirectory(Emgu.CV.UI)

set(CMAKE_SOURCE_DIR ${CMAKE_SOURCE_DIR}/opencv)
add_subdirectory(opencv)
add_subdirectory(Emgu.CV.Extern)

if(MSVC)
   set_target_properties(cvextern PROPERTIES PREFIX "../")
   set_target_properties(cxcore PROPERTIES PREFIX "../")
   set_target_properties(cv PROPERTIES PREFIX "../")
   set_target_properties(cvaux PROPERTIES PREFIX "../")
   set_target_properties(ml PROPERTIES PREFIX "../")
   set_target_properties(highgui PROPERTIES PREFIX "../")
   set_target_properties(opencv_ffmpeg PROPERTIES PREFIX "../")
endif()

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


