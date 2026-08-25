# --------------------------------------------------------
#  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
#
#  Defines the cvextern target itself: add_library(), TIFF/ZLIB/GeoTIFF
#  wiring, ADD_DEPENDENCIES/target_link_libraries for every OpenCV module
#  and optional component, and every platform-specific POST_BUILD step
#  (iOS lipo/xcframework assembly, Android strip, Ubuntu/Debian/RHEL
#  version-string + .so copy, Apple dylib bundling incl. libusb, and the
#  Emscripten LLVM-IR bitcode merge).
#  Included from Emgu.CV.Extern/CMakeLists.txt; CMAKE_CURRENT_SOURCE_DIR still
#  refers to Emgu.CV.Extern/ here (include() does not change it).
# ----------------------------------------------------------------------------

IF(IOS)
  add_library(${the_target} STATIC ${extern_srcs} ${extern_hdrs} )
ELSEIF(WIN32)
  #add the version information for windows binary
  add_library(${the_target} SHARED ${extern_srcs} ${extern_hdrs} ${PROJECT_SOURCE_DIR}/version.rc )
ELSEIF("${CMAKE_SYSTEM_NAME}" STREQUAL "Emscripten")
  add_library(${the_target} STATIC ${extern_srcs} ${extern_hdrs} )
  #set_target_properties(${the_target} PROPERTIES SUFFIX ".bc")    
ELSE()
  add_library(${the_target} SHARED ${extern_srcs} ${extern_hdrs} )
  IF (APPLE)
    set_target_properties(${the_target} PROPERTIES MACOSX_RPATH ON)    
    #SET(DYLIBBUNDLER_PATH "${PROJECT_SOURCE_DIR}/../platform/macos/dylibbundler")
    #MESSAGE(STATUS "DYLIBBUNDLER_PATH: ${DYLIBBUNDLER_PATH}")
  ENDIF()
ENDIF()  


IF(DEFINED CVEXTERN_DEPENDENCY_DLLS)
  FOREACH(CVEXTERN_DEPENDENCY_DLL ${CVEXTERN_DEPENDENCY_DLLS})
    LIST(APPEND CVEXTERN_DEPENDENCY_DLL_DEPLOY_COMMAND COMMAND ${CMAKE_COMMAND} -E copy "${CVEXTERN_DEPENDENCY_DLL}" "${CMAKE_RUNTIME_OUTPUT_DIRECTORY}")
    GET_FILENAME_COMPONENT(CVEXTERN_DEPENDENCT_DLL_NAME ${CVEXTERN_DEPENDENCY_DLL} NAME_WE)
    LIST(APPEND CVEXTERN_DEPENDENCY_DLL_NAMES ${CVEXTERN_DEPENDENCT_DLL_NAME})
  ENDFOREACH()
  
  #Promote this to parent scope such that cpack will know what dlls to be included in the package
  MESSAGE(STATUS "Copying CVEXTERN_DEPENDENCY_DLL_NAMES to parent scope: ${CVEXTERN_DEPENDENCY_DLL_NAMES}")
  SET(CVEXTERN_DEPENDENCY_DLL_NAMES ${CVEXTERN_DEPENDENCY_DLL_NAMES} PARENT_SCOPE)
  
  ADD_CUSTOM_COMMAND(
    TARGET ${the_target}
    POST_BUILD
    ${CVEXTERN_DEPENDENCY_DLL_DEPLOY_COMMAND}
    COMMENT "Copying ${CVEXTERN_DEPENDENCY_DLLS} to ${CMAKE_RUNTIME_OUTPUT_DIRECTORY}")
ENDIF()

IF(DEFINED CVEXTERN_CUDA_DEPENDENCY_DLLS)
  #Promote this to parent scope such that cpack will know what cuda dlls to be included in the package
  #MESSAGE("**************CVEXTERN_CUDA_DEPENDENCY_DLL_NAMES: ${CVEXTERN_CUDA_DEPENDENCY_DLL_NAMES}")
  SET(CVEXTERN_CUDA_DEPENDENCY_DLL_NAMES ${CVEXTERN_CUDA_DEPENDENCY_DLL_NAMES} PARENT_SCOPE)
ENDIF()

# For dynamic link numbering conventions
set_target_properties(${the_target} PROPERTIES
  OUTPUT_NAME "${the_target}"
  )

# Additional target properties
set_target_properties(${the_target} PROPERTIES
  DEBUG_POSTFIX "${OPENCV_DEBUG_POSTFIX}"
  ARCHIVE_OUTPUT_DIRECTORY "${CMAKE_SOURCE_DIR}/libs${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER}"
  RUNTIME_OUTPUT_DIRECTORY "${CMAKE_SOURCE_DIR}/libs${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER}"
  LIBRARY_OUTPUT_DIRECTORY "${CMAKE_SOURCE_DIR}/libs${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER}"
  DEFINE_SYMBOL "CVAPI_EXPORTS"
  )

#if (NOT (WIN32 OR ANDROID OR IOS))
#  set_target_properties(${the_target} PROPERTIES LINK_FLAGS "-fPIC")
#endif()  

if(ENABLE_SOLUTION_FOLDERS)
  set_target_properties(${the_target} PROPERTIES FOLDER "emgu")
endif() 

IF (WITH_TIFF)
  IF (BUILD_TIFF)
    ADD_DEPENDENCIES(${the_target} libtiff)
    SET(TIFF_INCLUDE_DIR "${OPENCV_SUBFOLDER}/3rdparty/libtiff")
    SET(TIFF_CONFIG_INCLUDE_DIR "${CMAKE_BINARY_DIR}/opencv/3rdparty/libtiff")
    SET(TIFF_LIBRARY "libtiff")
  ELSE()
    FIND_PACKAGE(TIFF)
    IF(NOT TIFF_FOUND)
      ADD_DEPENDENCIES(${the_target} libtiff)
      SET(TIFF_INCLUDE_DIR "${OPENCV_SUBFOLDER}/3rdparty/libtiff")
      SET(TIFF_CONFIG_INCLUDE_DIR "${CMAKE_BINARY_DIR}/opencv/3rdparty/libtiff")
      SET(TIFF_LIBRARY "libtiff")
    ENDIF()
  ENDIF()
  INCLUDE_DIRECTORIES(${TIFF_INCLUDE_DIR})
  INCLUDE_DIRECTORIES(${TIFF_CONFIG_INCLUDE_DIR})
ELSE()
  SET(TIFF_LIBRARY "")
ENDIF()

IF(WIN32 OR APPLE OR NETFX_CORE)
  #always build from source on windows / Mac
  MESSAGE(STATUS "Building zlib from source")
  ADD_DEPENDENCIES(${the_target} zlib)
  SET(ZLIB_INCLUDE_DIR "${OPENCV_SUBFOLDER}/3rdparty/zlib")
  SET(ZLIB_LIBRARY "zlib")
  INCLUDE_DIRECTORIES(${ZLIB_INCLUDE_DIR})
ELSE()
  FIND_PACKAGE(ZLIB)
  IF(ZLIB_FOUND)
    MESSAGE(STATUS "CVEXTERN: ZLIB found.")
  ELSE()
    MESSAGE(STATUS "CVEXTERN: ZLIB not found, building from source")
    ADD_DEPENDENCIES(${the_target} zlib)
    SET(ZLIB_INCLUDE_DIR "${OPENCV_SUBFOLDER}/3rdparty/zlib")
    SET(ZLIB_LIBRARY "zlib")
  ENDIF()
  INCLUDE_DIRECTORIES(${ZLIB_INCLUDE_DIR})
ENDIF()

SET(GEOTIFF_LIBRARY)
IF (ANDROID OR IOS)
  SET(EMGU_CV_WITH_TIFF OFF CACHE BOOL "BUILD TIFF wrapper")
  MESSAGE(STATUS "Skipping tiff")
ELSE()
  SET(EMGU_CV_WITH_TIFF ON CACHE BOOL "BUILD TIFF wrapper")
ENDIF()

IF(EMGU_CV_WITH_TIFF)
  SET(CMAKE_MODULE_PATH "${CMAKE_CURRENT_SOURCE_DIR}/libgeotiff/cmake")
  FIND_PACKAGE(GeoTIFF)
  IF(GEOTIFF_FOUND)
    MESSAGE(STATUS "CVEXTERN: GEOTIFF found. INCLUDE DIR: ${GEOTIFF_INCLUDE_DIR}")
  ENDIF()
  IF (NOT GEOTIFF_FOUND)
    MESSAGE(STATUS "CVEXTERN: GEOTIFF not found. Building PROJ and libgeotiff from source")
    # ----------------------------------------------------------------------------
    #  Build PROJ (required by libgeotiff 1.7.4)
    # ----------------------------------------------------------------------------
    include(FetchContent)
    FetchContent_Declare(
        proj_src
        URL "https://download.osgeo.org/proj/proj-9.5.1.tar.gz"
        URL_HASH SHA256=a8395f9696338ffd46b0feb603edbb730fad6746fba77753c77f7f997345e3d3
        DOWNLOAD_EXTRACT_TIMESTAMP TRUE
        PATCH_COMMAND ${CMAKE_COMMAND}
            -DPROJ_SRC_DIR=<SOURCE_DIR>
            -P "${CMAKE_CURRENT_SOURCE_DIR}/cmake/patch_proj_uninstall.cmake"
    )
    set(_save_BUILD_SHARED_LIBS ${BUILD_SHARED_LIBS})
    set(BUILD_SHARED_LIBS OFF CACHE BOOL "" FORCE)
    set(BUILD_TESTING     OFF CACHE BOOL "" FORCE)
    set(BUILD_APPS        OFF CACHE BOOL "" FORCE)
    set(ENABLE_CURL       OFF CACHE BOOL "" FORCE)
    set(ENABLE_TIFF       OFF CACHE BOOL "" FORCE)
    FetchContent_MakeAvailable(proj_src)
    set(BUILD_SHARED_LIBS ${_save_BUILD_SHARED_LIBS} CACHE BOOL "" FORCE)
    # ----------------------------------------------------------------------------
    #  Download libgeotiff source and build via our custom CMakeLists.txt
    # ----------------------------------------------------------------------------
    FetchContent_Declare(
        libgeotiff_src
        URL "https://github.com/OSGeo/libgeotiff/releases/download/1.7.4/libgeotiff-1.7.4.tar.gz"
        URL_HASH SHA256=c598d04fdf2ba25c4352844dafa81dde3f7fd968daa7ad131228cd91e9d3dc47
        DOWNLOAD_EXTRACT_TIMESTAMP TRUE
    )
    if(POLICY CMP0169)
        cmake_policy(SET CMP0169 OLD)
    endif()
    FetchContent_GetProperties(libgeotiff_src)
    if(NOT libgeotiff_src_POPULATED)
        FetchContent_Populate(libgeotiff_src)
    endif()
    SET(LIBGEOTIFF_DIR "${libgeotiff_src_SOURCE_DIR}")
    ADD_SUBDIRECTORY(libgeotiff)
    SET(GEOTIFF_INCLUDE_DIR ${TIFF_INCLUDE_DIR} ${LIBGEOTIFF_DIR} "${LIBGEOTIFF_DIR}/libxtiff")
    SET(GEOTIFF_LIBRARY geotiff_archive xtiff proj)
  ENDIF()
  ADD_DEFINITIONS(-DEMGU_CV_WITH_TIFF)
  INCLUDE_DIRECTORIES(${GEOTIFF_INCLUDE_DIR})
ENDIF()

IF (OPENCL_PROJ)
  ADD_DEPENDENCIES(${the_target} ${OPENCL_PROJ})
ENDIF()

#IF (ZLIB_LIBRARY)
#  ADD_DEPENDENCIES(${the_target} ${ZLIB_LIBRARY})
#ENDIF()

IF (TIFF_LIBRARY)
  IF (TARGET TIFF_LIBRARY)
    ADD_DEPENDENCIES(${the_target} ${TIFF_LIBRARY})
  ENDIF()
ENDIF()

IF (GEOTIFF_LIBRARY)
  IF (TARGET GEOTIFF_LIBRARY)
    ADD_DEPENDENCIES(${the_target} ${GEOTIFF_LIBRARY})
  ENDIF()
ENDIF()

#IF (CVBLOB_LIBRARY)
#  ADD_DEPENDENCIES(${the_target} ${CVBLOB_LIBRARY})
#ENDIF()

#ADD_DEPENDENCIES(${the_target} 
#  ${OPENCL_PROJ}
  # ${ZLIB_LIBRARY} 
  #  xtiff
#  ${TIFF_LIBRARY} ${GEOTIFF_LIBRARY} ${CVBLOB_LIBRARY} )

#IF(WITH_CUDA)
#  FIND_PACKAGE(CUDA)
#  ADD_DEPENDENCIES(${the_target}  ${CUDA_LIBRARIES})
#ENDIF()

IF (TARGET opencv_dnn_openvino)
  ADD_DEPENDENCIES(${the_target} opencv_dnn_openvino)
ENDIF()

IF(EMGU_CV_WITH_TESSERACT)
  IF(TESSERACT_FOUND)
    #MESSAGE("TESSERACT found, no need to add dependency")
    #ADD_DEPENDENCIES(${TESSERACT_LIBRARIES})
  ELSE()
    #MESSAGE("TESSERACT not found, add dependency to ${TESSERACT_PROJECTS}")
    ADD_DEPENDENCIES(${the_target} ${TESSERACT_PROJECTS})
  ENDIF()
ENDIF()

IF(EMGU_CV_WITH_DEPTHAI)
  target_link_libraries(${the_target} depthai-core) 
ENDIF()

FOREACH(CVEXTERN_OPTIONAL_DEP ${OPENCV_MODULE_NAMES})
  MESSAGE(STATUS "CVEXTERN dependency added:  opencv_${CVEXTERN_OPTIONAL_DEP}")
  ADD_DEPENDENCIES(${the_target} opencv_${CVEXTERN_OPTIONAL_DEP})
  target_link_libraries(${the_target} opencv_${CVEXTERN_OPTIONAL_DEP})
ENDFOREACH()

IF(EMGU_ENABLE_SSE)
  ADD_DEFINITIONS(-D__EMGU_ENABLE_SSE__)
  IF(MSVC AND (NOT CV_ICC) AND (NOT TARGET_ARCH_64)) 
    SET(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} /arch:SSE2")        ## Optimization
  ENDIF()
ENDIF()

IF(ANDROID)
  ADD_DEFINITIONS(-DANDROID)
  TARGET_LINK_LIBRARIES(${the_target} c++)
  target_link_options(${the_target} PRIVATE "-Wl,-z,max-page-size=16384")
  
  # For Android, tell the linker to transform all the symbols in the static libraries to hidden.
  # This can significantly reduce the binary size
  #set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} -Wl,--exclude-libs,ALL")
  #set(CMAKE_C_FLAGS "${CMAKE_CXX_FLAGS} -Wl,--exclude-libs,ALL")
ENDIF()

#disable warnings
IF(MSVC)
  ADD_DEFINITIONS(-wd4251 -D_CRT_SECURE_NO_WARNINGS)
ENDIF()

if(NETFX_CORE)
  if((CMAKE_SYSTEM_VERSION MATCHES 10.0) OR (CMAKE_SYSTEM_VERSION MATCHES 8.1))
    set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} /ZW")
  endif()
endif()

if(WITH_PNG AND BUILD_PNG)
  target_link_libraries(${the_target} libpng)
endif()

if(WITH_TIFF AND BUILD_TIFF)
  target_link_libraries(${the_target} libtiff)
endif()

if(WITH_JPEG AND BUILD_JPEG)
  target_link_libraries(${the_target} libjpeg-turbo)
endif()

# Add the required libraries for linking:
target_link_libraries(${the_target}
  ${OPENCV_LINKER_LIBS}
  ${ZLIB_LIBRARY})

IF(HAVE_ONNXRUNTIME AND ONNX_LIBRARIES)
  target_link_libraries(${the_target} ${ONNX_LIBRARIES})
ENDIF()
  #  xtiff 
  #${CVBLOB_LIBRARY})

#IF (APPLE)
	set_target_properties(${PROJECT_NAME} PROPERTIES CXX_STANDARD 17)
#ELSE()
#	set_target_properties(${PROJECT_NAME} PROPERTIES CXX_STANDARD 11)
#ENDIF()

IF(DEFINED EMGUCV_PLATFORM_TOOLSET)
  set_target_properties(${the_target} PROPERTIES PLATFORM_TOOLSET ${EMGUCV_PLATFORM_TOOLSET})
ENDIF()

# OpenCV sets /DELAYLOAD flags inside its own subdirectory scope via
# ocv_register_modules(), but cmake does not propagate CMAKE_SHARED_LINKER_FLAGS
# changes from a subdirectory back to the parent scope. Replicate the same logic
# here using target_link_options() so cvextern.dll also delay-loads OpenCV DLLs.
if(MSVC AND BUILD_SHARED_LIBS AND ENABLE_DELAYLOAD AND NOT BUILD_opencv_world)
  set(_cvextern_delay_flags "/IGNORE:4199")
  foreach(_mod ${OPENCV_MODULES_BUILD})
    if(NOT _mod STREQUAL "opencv_core" AND NOT _mod MATCHES "bindings_generator|python")
      list(APPEND _cvextern_delay_flags
        "/DELAYLOAD:${_mod}${OPENCV_VERSION_MAJOR}${OPENCV_VERSION_MINOR}${OPENCV_VERSION_PATCH}.dll")
    endif()
  endforeach()
  target_link_libraries(${the_target} PRIVATE delayimp)
  target_link_options(${the_target} PRIVATE ${_cvextern_delay_flags})
  message(STATUS "cvextern: DELAYLOAD enabled for ${OPENCV_VERSION_MAJOR}${OPENCV_VERSION_MINOR}${OPENCV_VERSION_PATCH} OpenCV modules")
endif()

#IF(WITH_IPP)
#  target_link_libraries(${the_target} ippdc_l)
#ENDIF()  

IF(EMGU_CV_WITH_TESSERACT AND TESSERACT_OPENCL)
  #SET(CMAKE_MODULE_PATH ${CMAKE_MODULE_PATH} "${OPENCV_SUBFOLDER}/cmake/")
  #include(${OPENCV_SUBFOLDER}/cmake/OpenCVDetectOpenCL.cmake)
  #MESSAGE(STATUS "OPENCVL_LIBRARIES: ${OPENCV_LIBRARIES}")
  target_link_libraries(${the_target} OpenCL)
ENDIF()

#IF(WITH_INF_ENGINE)
#  target_link_libraries(${the_target} inference_engine)
#ENDIF()

#message(STATUS "EMGU_CV_WITH_FREETYPE: ${EMGU_CV_WITH_FREETYPE}")
GET_TARGET_PROPERTY(CVEXTERN_TARGET_TYPE ${the_target} TYPE)
IF (CVEXTERN_TARGET_TYPE STREQUAL STATIC_LIBRARY)
  # Do not need to link FREETYPE or HARFBUZZ if static linking
  # e.g. IOS will use FREETYPE and HARFBUZZ framework
ELSEIF (TARGET opencv_freetype)
  target_link_libraries(${the_target} ${FREETYPE_LIBRARY} ${HARFBUZZ_LIBRARY})
ENDIF()

IF (TARGET opencv_hdf)
    message(STATUS "LINK CVEXTERN with HDF5_LIBRARIES: ${HDF5_LIBRARIES}")
	target_link_libraries(${the_target} ${HDF5_LIBRARIES})
ENDIF()

IF(ANDROID)
  IF(EMGU_CV_WITH_TESSERACT)
    #NOT SURE WHY THE FOLLOWING IS NEEDED, BUT SEEMS TO MAKE THE PROBLEM GOES AWAY
    target_link_libraries(${the_target} ${TESSERACT_PROJECTS} tesseract_ccstruct tesseract_dict tesseract_classify tesseract_ccutil)
  ENDIF()
  INSTALL(TARGETS ${the_target} 
	RUNTIME DESTINATION libs${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER} COMPONENT libs
    LIBRARY DESTINATION libs${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER} COMPONENT libs
	ARCHIVE DESTINATION libs${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER} COMPONENT libs)
	
ELSEIF(APPLE)
  IF(EMGU_CV_WITH_TESSERACT)
    IF(NOT TESSERACT_FOUND)
      target_link_libraries(${the_target} ${TESSERACT_PROJECTS})
    ENDIF()
  ENDIF()
  IF (EMGU_CV_WITH_TIFF AND NOT IOS)
    target_link_libraries(${the_target} ${GEOTIFF_LIBRARY})
  ENDIF()
ELSE()
  target_link_libraries(${the_target} ${TIFF_LIBRARY})
  IF (EMGU_CV_WITH_TIFF)
    target_link_libraries(${the_target} ${GEOTIFF_LIBRARY})
  ENDIF()
  IF(EMGU_CV_WITH_TESSERACT)
    IF(NOT TESSERACT_FOUND)
      target_link_libraries(${the_target} ${TESSERACT_PROJECTS})
    ENDIF()
    IF(WIN32)
      #additional linkage required for tesseract built on windows
      target_link_libraries(${the_target} Ws2_32)
    ENDIF()
  ENDIF()
  INSTALL(TARGETS ${the_target} 
    RUNTIME DESTINATION libs${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER} COMPONENT libs
    LIBRARY DESTINATION libs${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER} COMPONENT libs
    ARCHIVE DESTINATION libs${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER} COMPONENT libs)
ENDIF()


SET(OPENCV_BUILD_INFO_VERSION_STRING ${CMAKE_BINARY_DIR}/opencv/modules/core/version_string.inc)
IF(BUILD_opencv_world)
  SET(OPENCV_BUILD_INFO_VERSION_STRING ${CMAKE_BINARY_DIR}/opencv/modules/world/version_string.inc)
ENDIF()

#add_subdirectory(gpu)
IF (WIN32)
  IF (EMGU_SIGN_FOUND)
    EMGU_SIGN_BINARY(${the_target} ${CMAKE_SOURCE_DIR}/libs${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER}/cvextern.dll)
  ENDIF()
  add_custom_command(TARGET ${the_target}
    POST_BUILD
    COMMAND ${CMAKE_COMMAND} -E copy_if_different ${OPENCV_BUILD_INFO_VERSION_STRING} ${CMAKE_SOURCE_DIR}/libs${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER}/version_string.inc
    WORKING_DIRECTORY "${CMAKE_BINARY_DIR}"
    COMMENT "Copying build information from ${OPENCV_BUILD_INFO_VERSION_STRING} to ${CMAKE_SOURCE_DIR}/libs${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER}/version_string.inc")
  #INSTALL(FILES
  #    "${CMAKE_SOURCE_DIR}/libs${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER}/version_string.inc"
  #    DESTINATION "libs${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER}/"
  #    COMPONENT emgucv_binary)
ELSEIF (IOS)
  SET(IOS_RELEASE_FOLDER "")
  IF (IPHONESIMULATOR)
    SET(IOS_RELEASE_FOLDER "Release-iphonesimulator")
    SET(IOS_RELEASE_FILENAME "libcvextern_simulator_${IOS_ARCH}.a")
	add_custom_command(TARGET ${the_target}
      POST_BUILD
      COMMAND ${CMAKE_COMMAND} -E copy_if_different ${OPENCV_BUILD_INFO_VERSION_STRING} ${CMAKE_SOURCE_DIR}/libs/iOS/simulator_${IOS_ARCH}_version_string.inc
      WORKING_DIRECTORY "${CMAKE_BINARY_DIR}"
      COMMENT "Copying build information from ${OPENCV_BUILD_INFO_VERSION_STRING} to ${CMAKE_SOURCE_DIR}/libs/iOS/simulator_${IOS_ARCH}_version_string.inc")
	#INSTALL(FILES
	#  "${CMAKE_SOURCE_DIR}/libs/iOS/simulator_${IOS_ARCH}_version_string.inc"
	#  DESTINATION "libs/iOS/"
	#  COMPONENT emgucv_binary)   
  ELSEIF(IPHONEOS)
    SET(IOS_RELEASE_FOLDER "Release-iphoneos")
    SET(IOS_RELEASE_FILENAME "libcvextern_iphoneos_${IOS_ARCH}.a")
	add_custom_command(TARGET ${the_target}
      POST_BUILD
      COMMAND ${CMAKE_COMMAND} -E copy_if_different ${OPENCV_BUILD_INFO_VERSION_STRING} ${CMAKE_SOURCE_DIR}/libs/iOS/iphoneos_${IOS_ARCH}_version_string.inc
      WORKING_DIRECTORY "${CMAKE_BINARY_DIR}"
      COMMENT "Copying build information from ${OPENCV_BUILD_INFO_VERSION_STRING} to ${CMAKE_SOURCE_DIR}/libs/iOS/iphoneos_${IOS_ARCH}_version_string.inc")
	#INSTALL(FILES
	#  "${CMAKE_SOURCE_DIR}/libs/iOS/iphoneos_${IOS_ARCH}_version_string.inc"
	#  DESTINATION "libs/iOS/"
	#  COMPONENT emgucv_binary) 
  ELSEIF(MAC_CATALYST)
    SET(IOS_RELEASE_FOLDER "Release-catalyst")
    SET(IOS_RELEASE_FILENAME "libcvextern_catalyst_${IOS_ARCH}.a")
    add_custom_command(TARGET ${the_target}
      POST_BUILD
      COMMAND ${CMAKE_COMMAND} -E copy_if_different ${OPENCV_BUILD_INFO_VERSION_STRING} ${CMAKE_SOURCE_DIR}/libs/iOS/catalyst_${IOS_ARCH}_version_string.inc
      WORKING_DIRECTORY "${CMAKE_BINARY_DIR}"
      COMMENT "Copying build information from ${OPENCV_BUILD_INFO_VERSION_STRING} to ${CMAKE_SOURCE_DIR}/libs/iOS/catalyst_${IOS_ARCH}_version_string.inc")
	#INSTALL(FILES
	#  "${CMAKE_SOURCE_DIR}/libs/iOS/catalyst_${IOS_ARCH}_version_string.inc"
	#  DESTINATION "libs/iOS/"
	#  COMPONENT emgucv_binary) 	 
  ENDIF()
  #MESSAGE(STATUS "<<<<<< IOS_RELEASE_FOLDER: ${IOS_RELEASE_FOLDER} >>>>>>") 
  GET_TARGET_PROPERTY(CVEXTERN_TARGET_TYPE ${the_target} TYPE)
  SET(LIBTOOL_FREETYPE_ARGS "")
  SET(LIBTOOL_HARFBUZZ_ARGS "")
  IF (NOT CVEXTERN_TARGET_TYPE STREQUAL STATIC_LIBRARY)
    IF (TARGET opencv_freetype)
      SET(LIBTOOL_FREETYPE_ARGS "${FREETYPE_LIBRARIES}")
      SET(LIBTOOL_HARFBUZZ_ARGS "${HARFBUZZ_LIBRARIES}")
    ENDIF()
  ENDIF()
  add_custom_command(TARGET ${the_target}
    POST_BUILD
    COMMAND ${CMAKE_COMMAND}
      -DARCH=${IOS_ARCH}
      -DOUTPUT=${CMAKE_SOURCE_DIR}/libs/iOS/${IOS_RELEASE_FILENAME}
      -DBIN_DIR=${CMAKE_BINARY_DIR}/bin${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER}/Release
      -DOPENCV_MODULES_DIR=${CMAKE_BINARY_DIR}/opencv/lib/Release
      -DOPENCV_3RDPARTY_DIR=${CMAKE_BINARY_DIR}/opencv/3rdparty/lib/Release
      -DRUNTIME_DIR=${CMAKE_RUNTIME_OUTPUT_DIRECTORY}/Release
      -DWITH_TESSERACT=${EMGU_CV_WITH_TESSERACT}
      -DTESSERACT_DIR=${CMAKE_BINARY_DIR}/libs/Release
      "-DFREETYPE_LIBS=${LIBTOOL_FREETYPE_ARGS}"
      "-DHARFBUZZ_LIBS=${LIBTOOL_HARFBUZZ_ARGS}"
      -P ${CMAKE_SOURCE_DIR}/cmake/libtool_link_ios.cmake
    WORKING_DIRECTORY "${CMAKE_BINARY_DIR}"
    COMMENT "Linking static library for ${IOS_ARCH}")
    
  SET(IPHONEOS_STATIC_LIB_FILE ${CMAKE_SOURCE_DIR}/libs/iOS/libcvextern_iphoneos.a)
  SET(IPHONESIMULATOR_STATIC_LIB_FILE ${CMAKE_SOURCE_DIR}/libs/iOS/libcvextern_simulator.a)
  SET(IPHONE_UNIVERSAL_STATIC_LIB_FILE ${CMAKE_SOURCE_DIR}/libs/iOS/libcvextern_universal.a)
  SET(LIPO_INPUT_FILES)
  SET(XCFRAMEWORK_INPUT_FILES)
  SET(IOS_SIMULATOR_x86_64_RELEASE_FILE_NAME ${CMAKE_SOURCE_DIR}/libs/iOS/libcvextern_simulator_x86_64.a)
  IF (IPHONESIMULATOR)
    # Only add the x86_64 simulator file to the static library
    IF(("${IOS_ARCH}" STREQUAL "x86_64") OR (EXISTS ${IOS_SIMULATOR_x86_64_RELEASE_FILE_NAME}))
      LIST(APPEND LIPO_INPUT_FILES ${IOS_SIMULATOR_x86_64_RELEASE_FILE_NAME})
    ENDIF()
    LIST(APPEND XCFRAMEWORK_INPUT_FILES ${IPHONESIMULATOR_STATIC_LIB_FILE})
    IF (EXISTS ${IPHONEOS_STATIC_LIB_FILE})
      LIST(APPEND LIPO_INPUT_FILES ${IPHONEOS_STATIC_LIB_FILE})
      LIST(APPEND XCFRAMEWORK_INPUT_FILES ${IPHONEOS_STATIC_LIB_FILE})
    ENDIF()
    add_custom_command(TARGET ${the_target}
      POST_BUILD
      COMMAND lipo -create -output ${IPHONESIMULATOR_STATIC_LIB_FILE} ${CMAKE_SOURCE_DIR}/libs/iOS/libcvextern_simulator_*.a
      WORKING_DIRECTORY "${CMAKE_SOURCE_DIR}"
      COMMENT "Linking multiple arch into a single file for iOS simulator") 
  ELSEIF(IPHONEOS)
    LIST(APPEND LIPO_INPUT_FILES ${IPHONEOS_STATIC_LIB_FILE})
    IF (EXISTS ${IOS_SIMULATOR_x86_64_RELEASE_FILE_NAME})
      LIST(APPEND LIPO_INPUT_FILES "${IOS_SIMULATOR_x86_64_RELEASE_FILE_NAME}")
    ENDIF()
    LIST(APPEND XCFRAMEWORK_INPUT_FILES ${IPHONEOS_STATIC_LIB_FILE})
    IF (EXISTS ${IPHONESIMULATOR_STATIC_LIB_FILE})
      LIST(APPEND XCFRAMEWORK_INPUT_FILES ${IPHONESIMULATOR_STATIC_LIB_FILE})
    ENDIF()

    add_custom_command(TARGET ${the_target}
      POST_BUILD
      COMMAND lipo -create -output ${IPHONEOS_STATIC_LIB_FILE} ${CMAKE_SOURCE_DIR}/libs/iOS/libcvextern_iphoneos_*.a
      WORKING_DIRECTORY "${CMAKE_SOURCE_DIR}"
      COMMENT "Linking multiple arch into a single file for iOS device")
  ENDIF()
  
  LIST(LENGTH XCFRAMEWORK_INPUT_FILES XCFRAMEWORK_INPUT_FILES_LENGTH)
  IF (XCFRAMEWORK_INPUT_FILES_LENGTH GREATER 0)
      SET(CVEXTERN_CXFRAMEWORK_LIBRARIES)
	  FOREACH(XCFRAMEWORK_INPUT_FILE ${XCFRAMEWORK_INPUT_FILES}) 
		LIST(APPEND CVEXTERN_XCFRAMEWORK_LIBRARIES -library ${XCFRAMEWORK_INPUT_FILE})
	  ENDFOREACH()
      add_custom_command(TARGET ${the_target}
      POST_BUILD
      COMMAND rm -rf ${CMAKE_SOURCE_DIR}/libs/iOS/libcvextern_ios.xcframework
      COMMAND xcodebuild -create-xcframework ${CVEXTERN_XCFRAMEWORK_LIBRARIES} -output ${CMAKE_SOURCE_DIR}/libs/iOS/libcvextern_ios.xcframework
      WORKING_DIRECTORY "${CMAKE_SOURCE_DIR}/libs/iOS"
      COMMENT "Creating xcframework for iOS using files from: ${XCFRAMEWORK_INPUT_FILES}")
  ENDIF()

  IF(MAC_CATALYST)
    SET(CATALYST_FAT_LIB ${CMAKE_SOURCE_DIR}/libs/iOS/libcvextern_catalyst.a)
    add_custom_command(TARGET ${the_target}
      POST_BUILD
      COMMAND lipo -create -output ${CATALYST_FAT_LIB} ${CMAKE_SOURCE_DIR}/libs/iOS/libcvextern_catalyst_*.a
      WORKING_DIRECTORY "${CMAKE_SOURCE_DIR}"
      COMMENT "Merging Mac Catalyst static libraries into ${CATALYST_FAT_LIB}")
    add_custom_command(TARGET ${the_target}
      POST_BUILD
      COMMAND rm -rf ${CMAKE_SOURCE_DIR}/libs/iOS/libcvextern_catalyst.xcframework
      COMMAND xcodebuild -create-xcframework -library ${CATALYST_FAT_LIB} -output ${CMAKE_SOURCE_DIR}/libs/iOS/libcvextern_catalyst.xcframework
      WORKING_DIRECTORY "${CMAKE_SOURCE_DIR}/libs/iOS"
      COMMENT "Creating xcframework for Mac Catalyst")
  ENDIF()

  LIST(LENGTH LIPO_INPUT_FILES LIPO_INPUT_FILES_LENGTH)
  IF (LIPO_INPUT_FILES_LENGTH EQUAL 1)
    add_custom_command(TARGET ${the_target}
      POST_BUILD
      COMMAND ${CMAKE_COMMAND} -E copy ${LIPO_INPUT_FILES} ${IPHONE_UNIVERSAL_STATIC_LIB_FILE} 
      WORKING_DIRECTORY "${CMAKE_SOURCE_DIR}"
      COMMENT "Linking simulator and iphoneos static library into a universal file for iOS: ${LIPO_INPUT_FILES}")
   INSTALL(FILES
      "${IPHONE_UNIVERSAL_STATIC_LIB_FILE}"
      DESTINATION "libs/iOS/"
      COMPONENT emgucv_binary)    
  ELSEIF (LIPO_INPUT_FILES_LENGTH GREATER 1)
    add_custom_command(TARGET ${the_target}
      POST_BUILD
      COMMAND lipo -create -output ${IPHONE_UNIVERSAL_STATIC_LIB_FILE} ${LIPO_INPUT_FILES}
      WORKING_DIRECTORY "${CMAKE_SOURCE_DIR}"
      COMMENT "Linking multiple arch into a single file for iOS device: ${LIPO_INPUT_FILES}")
   INSTALL(FILES
      "${IPHONE_UNIVERSAL_STATIC_LIB_FILE}"
      DESTINATION "libs/iOS/"
      COMPONENT emgucv_binary)
  ENDIF()
  
  IF(IPHONESIMULATOR OR IPHONEOS)
    INSTALL(FILES
      "${IPHONEOS_STATIC_LIB_FILE}"
      DESTINATION "libs/iOS/"
      COMPONENT emgucv_binary)
    INSTALL(FILES
      "${IPHONESIMULATOR_STATIC_LIB_FILE}"
      DESTINATION "libs/iOS/"
      COMPONENT emgucv_binary)
    INSTALL(FILES
      "${IPHONE_UNIVERSAL_STATIC_LIB_FILE}"
      DESTINATION "libs/iOS/"
      COMPONENT emgucv_binary)
    INSTALL(DIRECTORY
      "${CMAKE_SOURCE_DIR}/libs/iOS/libcvextern_ios.xcframework"
      DESTINATION "libs/iOS/"
      COMPONENT emgucv_binary)  

	#IF(IPHONESIMULATOR AND (EXISTS "${CMAKE_SOURCE_DIR}/libs/iOS/iphoneos_arm64_version_string.inc"))
	#    INSTALL(FILES
	#	  "${CMAKE_SOURCE_DIR}/libs/iOS/iphoneos_arm64_version_string.inc"
	#	  DESTINATION "libs/iOS/"
	#	  COMPONENT emgucv_binary)
	#ENDIF()
	#IF(IPHONEOS AND (EXISTS "${CMAKE_SOURCE_DIR}/libs/iOS/simulator_x86_64_version_string.inc"))
	#    INSTALL(FILES
	#	  "${CMAKE_SOURCE_DIR}/libs/iOS/simulator_x86_64_version_string.inc"
	#	  DESTINATION "libs/iOS/"
	#	  COMPONENT emgucv_binary)
	#ENDIF()
  ELSEIF(MAC_CATALYST)
    INSTALL(FILES
      "${CMAKE_SOURCE_DIR}/libs/iOS/libcvextern_catalyst_${IOS_ARCH}.a"
      DESTINATION "libs/iOS/"
      COMPONENT emgucv_binary)
    IF(("${IOS_ARCH}" STREQUAL "x86_64") AND (EXISTS "${CMAKE_SOURCE_DIR}/libs/iOS/libcvextern_catalyst_arm64.a"))
      INSTALL(FILES
		"${CMAKE_SOURCE_DIR}/libs/iOS/libcvextern_catalyst_arm64.a"
		#"${CMAKE_SOURCE_DIR}/libs/iOS/catalyst_arm64_version_string.inc"
		DESTINATION "libs/iOS/"
		COMPONENT emgucv_binary)
    ELSEIF(("${IOS_ARCH}" STREQUAL "arm64") AND (EXISTS "${CMAKE_SOURCE_DIR}/libs/iOS/libcvextern_catalyst_x86_64.a"))
      INSTALL(FILES
		"${CMAKE_SOURCE_DIR}/libs/iOS/libcvextern_catalyst_x86_64.a"
		#"${CMAKE_SOURCE_DIR}/libs/iOS/catalyst_x86_64_version_string.inc"
		DESTINATION "libs/iOS/"
		COMPONENT emgucv_binary)
    ENDIF()
    INSTALL(DIRECTORY
      "${CMAKE_SOURCE_DIR}/libs/iOS/libcvextern_catalyst.xcframework"
      DESTINATION "libs/iOS/"
      COMPONENT emgucv_binary)
  ENDIF()

ELSEIF (ANDROID)
    add_custom_command(TARGET ${the_target}
      POST_BUILD
      COMMAND ${CMAKE_STRIP} libcvextern.so 
      WORKING_DIRECTORY "${CMAKE_SOURCE_DIR}/libs${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER}"
      COMMENT "Stripping libcvextern.so from ${CMAKE_SOURCE_DIR}/libs${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER}")
	
	#GET_FILENAME_COMPONENT(ANDROID_NATIVE_LIBRARRY_PATH "${ZLIB_LIBRARY_RELEASE}" DIRECTORY)
	#SET(CPP_SHARED_LIBRARY_RELEASE "${ANDROID_NATIVE_LIBRARRY_PATH}/../libc++_shared.so") 
	#add_custom_command(TARGET ${the_target}
    #  POST_BUILD
    #  COMMAND ${CMAKE_COMMAND} -E copy_if_different ${CPP_SHARED_LIBRARY_RELEASE} ${CMAKE_SOURCE_DIR}/libs${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER}/libc++_shared.so
    #  WORKING_DIRECTORY "${CMAKE_BINARY_DIR}"
    #  COMMENT "Copying libc++_shared.so from ${CPP_SHARED_LIBRARY_RELEASE} to ${CMAKE_SOURCE_DIR}/libs${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER}/libc++_shared.so")
	  
	add_custom_command(TARGET ${the_target}
      POST_BUILD
      COMMAND ${CMAKE_COMMAND} -E copy_if_different ${OPENCV_BUILD_INFO_VERSION_STRING} ${CMAKE_SOURCE_DIR}/libs${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER}/${ANDROID_ABI}_version_string.inc
      WORKING_DIRECTORY "${CMAKE_BINARY_DIR}"
      COMMENT "Copying build information from ${OPENCV_BUILD_INFO_VERSION_STRING} to ${CMAKE_SOURCE_DIR}/libs${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER}/${ANDROID_ABI}_version_string.inc")
	#INSTALL(FILES
	#	"${CMAKE_SOURCE_DIR}/libs${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER}/${ANDROID_ABI}_version_string.inc"
	#	DESTINATION "libs${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER}"
	#	COMPONENT emgucv_binary)  
ELSEIF (IS_UBUNTU OR IS_DEBIAN OR IS_RHEL)

  IF (WITH_CUDA)
    add_custom_command(TARGET ${the_target}
      POST_BUILD
      COMMAND cp -fP ${CMAKE_BINARY_DIR}/opencv/lib/*.so ${CMAKE_BINARY_DIR}/opencv/lib/*.so.* ${CMAKE_SOURCE_DIR}/libs${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER}
      #WORKING_DIRECTORY "${UNMANAGED_LIBRARY_OUTPUT_PATH}"
      COMMENT "Copying file to ${CMAKE_SOURCEDIR}/libs${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER}")
  ENDIF()
  
  IF (IS_UBUNTU)
    SET(VERSION_STR_FILE ${CMAKE_SOURCE_DIR}/libs${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER}/${EMGUCV_ARCH}_${UBUNTU_VERSION}_version_string.inc)
  ELSEIF(IS_DEBIAN)
    SET(VERSION_STR_FILE ${CMAKE_SOURCE_DIR}/libs${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER}/${EMGUCV_ARCH}_${DEBIAN_VERSION}_version_string.inc)
  ELSE()
    SET(VERSION_STR_FILE ${CMAKE_SOURCE_DIR}/libs${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER}/${EMGUCV_ARCH}_version_string.inc)
  ENDIF()
  
  add_custom_command(TARGET ${the_target}
    POST_BUILD
    COMMAND ${CMAKE_COMMAND} -E copy_if_different ${OPENCV_BUILD_INFO_VERSION_STRING} ${VERSION_STR_FILE}
    WORKING_DIRECTORY "${CMAKE_BINARY_DIR}"
    COMMENT "Copying build information from ${OPENCV_BUILD_INFO_VERSION_STRING} to ${VERSION_STR_FILE}")
  
  INSTALL(FILES
    "${VERSION_STR_FILE}"
    DESTINATION "libs${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER}"
  	COMPONENT emgucv_binary)  
	  
ELSEIF (APPLE)
  SET_TARGET_PROPERTIES(${the_target}
    PROPERTIES
    XCODE_ATTRIBUTE_COPY_PHASE_STRIP "YES"
    XCODE_ATTRIBUTE_STRIP_INSTALLED_PRODUCT "YES"
    XCODE_ATTRIBUTE_STRIP_STYLE "non-global"
    XCODE_ATTRIBUTE_STRIPFLAGS "-x -u -r"
    XCODE_ATTRIBUTE_DEAD_CODE_STRIPPING "YES"
  )

  add_custom_command(TARGET ${the_target}
    POST_BUILD
    COMMAND mkdir -p ${UNMANAGED_LIBRARY_OUTPUT_PATH}/../arch
    COMMENT "Creating arch folder: ${UNMANAGED_LIBRARY_OUTPUT_PATH}/../arch")

  IF(EMGU_CV_WITH_DEPTHAI)  
    SET(LIBUSB_FILE_NAME "libusb_${CMAKE_SYSTEM_PROCESSOR}.dylib")
    IF ("${EMGUCV_ARCH}" STREQUAL "arm64")
       # Apple silicon
       SET(LIBUSB_PATH /opt/homebrew/opt/libusb/lib/libusb-1.0.0.dylib)
    ELSE()
       # Intel Mac
       SET(LIBUSB_PATH /usr/local/opt/libusb/lib/libusb-1.0.0.dylib)
    ENDIF()
    IF(EXISTS ${LIBUSB_PATH})
    add_custom_command(TARGET ${the_target}
      POST_BUILD
      COMMAND cp -f ${LIBUSB_PATH} ${UNMANAGED_LIBRARY_OUTPUT_PATH}/../arch/${LIBUSB_FILE_NAME}
      COMMENT "Copying file to ${UNMANAGED_LIBRARY_OUTPUT_PATH}/../arch/${LIBUSB_FILE_NAME}")
    add_custom_command(TARGET ${the_target}
      POST_BUILD
      COMMAND lipo ${UNMANAGED_LIBRARY_OUTPUT_PATH}/../arch/libusb*.dylib -output ${UNMANAGED_LIBRARY_OUTPUT_PATH}/../libusb-1.0.0.dylib -create
      COMMENT "Linking target for all architectures and ouputing to ${UNMANAGED_LIBRARY_OUTPUT_PATH}/../libusb-1.0.0.dylib")
    INSTALL(FILES
      ${UNMANAGED_LIBRARY_OUTPUT_PATH}/../libusb-1.0.0.dylib
      DESTINATION "libs/runtimes/osx/native/"
      COMPONENT emgucv_binary)
    add_custom_command(TARGET ${the_target}
      POST_BUILD
      COMMAND install_name_tool -change ${LIBUSB_PATH} @rpath/libusb-1.0.0.dylib ${UNMANAGED_LIBRARY_OUTPUT_PATH}/../libcvextern.dylib 
      COMMENT "Altering dependency for ${UNMANAGED_LIBRARY_OUTPUT_PATH}/../libcvextern.dylib")
    add_custom_command(TARGET ${the_target}
      POST_BUILD
      COMMAND install_name_tool -id "@rpath/libusb-1.0.0.dylib" ${UNMANAGED_LIBRARY_OUTPUT_PATH}/../libusb-1.0.0.dylib
      COMMAND chmod a+x ${UNMANAGED_LIBRARY_OUTPUT_PATH}/../libusb-1.0.0.dylib
      COMMENT "Altering id for ${UNMANAGED_LIBRARY_OUTPUT_PATH}/../libusb-1.0.0.dylib")
    ENDIF()
  ENDIF()

  add_custom_command(TARGET ${the_target}
    POST_BUILD
    COMMAND ${CMAKE_COMMAND} -E copy_if_different ${OPENCV_BUILD_INFO_VERSION_STRING} ${CMAKE_SOURCE_DIR}/libs${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER}/../${EMGUCV_ARCH}_version_string.inc
    WORKING_DIRECTORY "${CMAKE_BINARY_DIR}"
    COMMENT "Copying MacOS build information from ${OPENCV_BUILD_INFO_VERSION_STRING} to ${CMAKE_SOURCE_DIR}/libs${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER}/../${EMGUCV_ARCH}_version_string.inc")

  # Each architecture build also writes its build information next to the thin
  # per-architecture binary, so the packaged .inc always matches the dylib it
  # sits beside (issue #978).
  IF("${CMAKE_SYSTEM_PROCESSOR}" STREQUAL "arm64")
    SET(EMGUCV_OSX_RID "osx-arm64")
  ELSE()
    SET(EMGUCV_OSX_RID "osx-x64")
  ENDIF()
  add_custom_command(TARGET ${the_target}
    POST_BUILD
    COMMAND ${CMAKE_COMMAND} -E make_directory ${CMAKE_SOURCE_DIR}/libs/runtimes/${EMGUCV_OSX_RID}/native
    COMMAND ${CMAKE_COMMAND} -E copy_if_different ${OPENCV_BUILD_INFO_VERSION_STRING} ${CMAKE_SOURCE_DIR}/libs/runtimes/${EMGUCV_OSX_RID}/native/${EMGUCV_ARCH}_version_string.inc
    WORKING_DIRECTORY "${CMAKE_BINARY_DIR}"
    COMMENT "Copying MacOS build information to libs/runtimes/${EMGUCV_OSX_RID}/native/${EMGUCV_ARCH}_version_string.inc")
  
    #INSTALL(FILES
	#	"${CMAKE_SOURCE_DIR}/libs${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER}/../${EMGUCV_ARCH}_version_string.inc"
	#	DESTINATION "libs/runtimes/osx/native"
	#	COMPONENT emgucv_binary)
		
    #IF(("${EMGUCV_ARCH}" STREQUAL "x86_64") AND (EXISTS "${CMAKE_SOURCE_DIR}/libs${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER}/../arm64_version_string.inc"))
    #  INSTALL(FILES
	#	"${CMAKE_SOURCE_DIR}/libs${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER}/../arm64_version_string.inc"
	#	DESTINATION "libs/runtimes/osx/native"
	#	COMPONENT emgucv_binary)
    #ELSEIF(("${EMGUCV_ARCH}" STREQUAL "arm64") AND (EXISTS "${CMAKE_SOURCE_DIR}/libs${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER}/../x86_64_version_string.inc"))
    #  INSTALL(FILES
	#	"${CMAKE_SOURCE_DIR}/libs${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER}/../x86_64_version_string.inc"
	#	DESTINATION "libs/runtimes/osx/native"
	#	COMPONENT emgucv_binary)
    #ENDIF()

  SET(CVEXTERN_AND_DEPENDENCY_DLLS ${CVEXTERN_DEPENDENCY_DLLS} "libcvextern.dylib")
  

  FOREACH(CVEXTERN_DEPENDENCY_DLL ${CVEXTERN_AND_DEPENDENCY_DLLS})
    GET_FILENAME_COMPONENT(CVEXTERN_DEPENDENCY_DLL_NAME ${CVEXTERN_DEPENDENCY_DLL} NAME_WE)
    GET_FILENAME_COMPONENT(CVEXTERN_DEPENDENCY_DLL_EXT ${CVEXTERN_DEPENDENCY_DLL} EXT)
    STRING(TOLOWER ${CVEXTERN_DEPENDENCY_DLL_EXT} CVEXTERN_DEPENDENCY_DLL_EXT_LOWER)
    
    add_custom_command(TARGET ${the_target}
      POST_BUILD
      COMMAND rm -f ${UNMANAGED_LIBRARY_OUTPUT_PATH}/../${CVEXTERN_DEPENDENCY_DLL_NAME}${CVEXTERN_DEPENDENCY_DLL_EXT}
      COMMENT "Removing file ${UNMANAGED_LIBRARY_OUTPUT_PATH}/../${CVEXTERN_DEPENDENCY_DLL_NAME}${CVEXTERN_DEPENDENCY_DLL_EXT}")
    
    IF( (${CVEXTERN_DEPENDENCY_DLL_EXT_LOWER} STREQUAL ".dylib") OR (${CVEXTERN_DEPENDENCY_DLL_EXT_LOWER} STREQUAL ".so") ) 
      add_custom_command(TARGET ${the_target}
	POST_BUILD
	COMMAND cp -f ${UNMANAGED_LIBRARY_OUTPUT_PATH}/${CVEXTERN_DEPENDENCY_DLL_NAME}${CVEXTERN_DEPENDENCY_DLL_EXT} ${UNMANAGED_LIBRARY_OUTPUT_PATH}/../arch/${CVEXTERN_DEPENDENCY_DLL_NAME}_${CMAKE_SYSTEM_PROCESSOR}${CVEXTERN_DEPENDENCY_DLL_EXT}
	COMMENT "Copying file to ${UNMANAGED_LIBRARY_OUTPUT_PATH}/../arch/${CVEXTERN_DEPENDENCY_DLL_NAME}_${CMAKE_SYSTEM_PROCESSOR}${CVEXTERN_DEPENDENCY_DLL_EXT}")
      add_custom_command(TARGET ${the_target}
	POST_BUILD
	COMMAND lipo ${UNMANAGED_LIBRARY_OUTPUT_PATH}/../arch/${CVEXTERN_DEPENDENCY_DLL_NAME}_*${CVEXTERN_DEPENDENCY_DLL_EXT} -output ${UNMANAGED_LIBRARY_OUTPUT_PATH}/../${CVEXTERN_DEPENDENCY_DLL_NAME}${CVEXTERN_DEPENDENCY_DLL_EXT} -create
	COMMENT "Linking target for all architectures and outputing to ${UNMANAGED_LIBRARY_OUTPUT_PATH}/../${CVEXTERN_DEPENDENCY_DLL_NAME}${CVEXTERN_DEPENDENCY_DLL_EXT}")
      add_custom_command(TARGET ${the_target}
	POST_BUILD
	COMMAND chmod a+x ${UNMANAGED_LIBRARY_OUTPUT_PATH}/../${CVEXTERN_DEPENDENCY_DLL_NAME}${CVEXTERN_DEPENDENCY_DLL_EXT}
	COMMENT "Add execute permission to ${UNMANAGED_LIBRARY_OUTPUT_PATH}/../${CVEXTERN_DEPENDENCY_DLL_NAME}${CVEXTERN_DEPENDENCY_DLL_EXT}")
      # Thin per-architecture copies under runtimes/osx-<arch>/native. The nuget
      # packages ship these instead of the universal binary so that a published
      # application only carries its own architecture (issue #978).
      add_custom_command(TARGET ${the_target}
	POST_BUILD
	COMMAND bash -c "[ ! -f '${UNMANAGED_LIBRARY_OUTPUT_PATH}/../arch/${CVEXTERN_DEPENDENCY_DLL_NAME}_arm64${CVEXTERN_DEPENDENCY_DLL_EXT}' ] || ( mkdir -p '${UNMANAGED_LIBRARY_OUTPUT_PATH}/../../../osx-arm64/native' && cp -f '${UNMANAGED_LIBRARY_OUTPUT_PATH}/../arch/${CVEXTERN_DEPENDENCY_DLL_NAME}_arm64${CVEXTERN_DEPENDENCY_DLL_EXT}' '${UNMANAGED_LIBRARY_OUTPUT_PATH}/../../../osx-arm64/native/${CVEXTERN_DEPENDENCY_DLL_NAME}${CVEXTERN_DEPENDENCY_DLL_EXT}' && chmod a+x '${UNMANAGED_LIBRARY_OUTPUT_PATH}/../../../osx-arm64/native/${CVEXTERN_DEPENDENCY_DLL_NAME}${CVEXTERN_DEPENDENCY_DLL_EXT}' )"
	COMMAND bash -c "[ ! -f '${UNMANAGED_LIBRARY_OUTPUT_PATH}/../arch/${CVEXTERN_DEPENDENCY_DLL_NAME}_x86_64${CVEXTERN_DEPENDENCY_DLL_EXT}' ] || ( mkdir -p '${UNMANAGED_LIBRARY_OUTPUT_PATH}/../../../osx-x64/native' && cp -f '${UNMANAGED_LIBRARY_OUTPUT_PATH}/../arch/${CVEXTERN_DEPENDENCY_DLL_NAME}_x86_64${CVEXTERN_DEPENDENCY_DLL_EXT}' '${UNMANAGED_LIBRARY_OUTPUT_PATH}/../../../osx-x64/native/${CVEXTERN_DEPENDENCY_DLL_NAME}${CVEXTERN_DEPENDENCY_DLL_EXT}' && chmod a+x '${UNMANAGED_LIBRARY_OUTPUT_PATH}/../../../osx-x64/native/${CVEXTERN_DEPENDENCY_DLL_NAME}${CVEXTERN_DEPENDENCY_DLL_EXT}' )"
	VERBATIM
	COMMENT "Populating thin per-architecture copies under runtimes/osx-<arch>/native")
    ELSE()
      add_custom_command(TARGET ${the_target}
	POST_BUILD
	COMMAND cp -f ${UNMANAGED_LIBRARY_OUTPUT_PATH}/${CVEXTERN_DEPENDENCY_DLL_NAME}${CVEXTERN_DEPENDENCY_DLL_EXT} ${UNMANAGED_LIBRARY_OUTPUT_PATH}/../${CVEXTERN_DEPENDENCY_DLL_NAME}${CVEXTERN_DEPENDENCY_DLL_EXT}
	COMMENT "Copying file to ${UNMANAGED_LIBRARY_OUTPUT_PATH}/../${CVEXTERN_DEPENDENCY_DLL_NAME}${CVEXTERN_DEPENDENCY_DLL_EXT}")
    ENDIF()
    
    INSTALL(FILES
      ${UNMANAGED_LIBRARY_OUTPUT_PATH}/../${CVEXTERN_DEPENDENCY_DLL}
      DESTINATION "libs/runtimes/osx/native/"
      COMPONENT emgucv_binary)
    
  ENDFOREACH()
  
  #INSTALL(FILES ${the_target} 
  #  RUNTIME DESTINATION libs${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER} COMPONENT libs
  #  LIBRARY DESTINATION libs${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER} COMPONENT libs
  #  ARCHIVE DESTINATION libs${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER} COMPONENT libs)
  
ENDIF()

IF(IS_EMSCRIPTEN_BUILD AND EMGU_CV_EMSCRIPTEN_LLVM_AR_PATH)
  IF(NOT DEFINED EMGU_CV_EMSCRIPTEN_OUTPUT_DIR OR EMGU_CV_EMSCRIPTEN_OUTPUT_DIR STREQUAL "")
    SET(EMGU_CV_EMSCRIPTEN_OUTPUT_DIR "webgl")
  ENDIF()
  add_custom_command(TARGET ${the_target}
    POST_BUILD
    COMMAND ${CMAKE_COMMAND}
      -DLLVM_AR=${EMGU_CV_EMSCRIPTEN_LLVM_AR_PATH}
      -DBUILD_DIR=${CMAKE_BINARY_DIR}
      -DSOURCE_DIR=${CMAKE_SOURCE_DIR}
      -DOBJ_DIR=${CMAKE_CURRENT_BINARY_DIR}/CMakeFiles/${the_target}.dir
      "-DOUTPUT_SUFFIX=${EMGU_CV_EMSCRIPTEN_OUTPUT_SUFFIX}"
      "-DOUTPUT_SUBDIR=${EMGU_CV_EMSCRIPTEN_OUTPUT_DIR}"
      -P ${CMAKE_CURRENT_SOURCE_DIR}/cmake/merge_emscripten_libs.cmake
    COMMENT "Merging LLVM IR bitcode archives into libs/${EMGU_CV_EMSCRIPTEN_OUTPUT_DIR}/cvextern${EMGU_CV_EMSCRIPTEN_OUTPUT_SUFFIX}.a")
ENDIF()

