# --------------------------------------------------------
# Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
# 
# Root CMake file for Emgu CV
#
# From the off-tree build directory, invoke:
#   $ cmake <PATH_TO_EMGUCV_ROOT>
#
# --------------------------------------------------------

CMAKE_MINIMUM_REQUIRED(VERSION 3.16)

SET(CMAKE_MODULE_PATH ${CMAKE_MODULE_PATH} "${CMAKE_CURRENT_SOURCE_DIR}/cmake/modules")
SET(EMGU_CV_SOURCE_DIR "${CMAKE_CURRENT_SOURCE_DIR}")
INCLUDE (CheckPlatform)

SET(EMGU_CV_WITH_DEPTHAI OFF CACHE BOOL "Build support for OAK-1 and OAK-D")

IF(EMGU_CV_WITH_DEPTHAI)
  # Always build depthai-core as a static library
  SET(BUILD_SHARED_LIBS_CACHE ${BUILD_SHARED_LIBS})
  SET(BUILD_SHARED_LIBS OFF)
  add_subdirectory(Emgu.CV.Extern/depthai-core EXCLUDE_FROM_ALL)
  SET(BUILD_SHARED_LIBS ${BUILD_SHARED_LIBS_CACHE})
ENDIF()


PROJECT(emgucv)

if(POLICY CMP0022)
  #cmake_policy(SET CMP0022 NEW)
  SET(CMAKE_POLICY_DEFAULT_CMP0022 NEW)
endif()
if(POLICY CMP0069)
  #cmake_policy(SET CMP0069 NEW)
  SET(CMAKE_POLICY_DEFAULT_CMP0069 NEW)
endif()


IF( WIN32 OR ANDROID OR IOS OR NETFX_CORE)
  SET(EMGU_CV_WITH_FREETYPE ON CACHE BOOL "BUILD FREETYPE")
ELSE() 
  SET(EMGU_CV_WITH_FREETYPE OFF CACHE BOOL "BUILD FREETYPE")
ENDIF()


INCLUDE("${CMAKE_CURRENT_SOURCE_DIR}/cmake/ThirdPartyDeps.cmake")

IF("${CMAKE_SYSTEM_NAME}" STREQUAL "Emscripten")
  SET(IS_EMSCRIPTEN_BUILD TRUE)
ELSE()
  SET(IS_EMSCRIPTEN_BUILD FALSE)
ENDIF()

MESSAGE(STATUS "IS_EMSCRIPTEN_BUILD: ${IS_EMSCRIPTEN_BUILD}")

INCLUDE("${CMAKE_CURRENT_SOURCE_DIR}/cmake/EmscriptenBuildFlags.cmake")

SET(OPENCV_SUBFOLDER ${CMAKE_CURRENT_SOURCE_DIR}/opencv)
SET(OPENCV_CONTRIB_SUBFOLDER ${CMAKE_CURRENT_SOURCE_DIR}/opencv_contrib)
SET(OPENCV_EXTRA_SUBFOLDER ${CMAKE_CURRENT_SOURCE_DIR}/opencv_extra)


  
SET(CSC_PREFERRED_VERSION "4.0")
IF((NOT NETFX_CORE) AND MSVC) 
  #SET CSC 3.5 as the prefered compiler for EMGU CV
  SET(CSC_TARGET_FRAMEWORK "3.5" CACHE STRING "C#  target framework")
ELSE()
  #SET CSC 4.5 (in the .Net framework 4.0 folder) as the prefered compiler for EMGU CV
  SET(CSC_TARGET_FRAMEWORK "" CACHE STRING "C# target framework")
ENDIF()
  
FIND_PACKAGE(CSharp REQUIRED)
INCLUDE (CSharpMacros)
INCLUDE (DebianRuntimeMacros)

IF (NOT (DOTNET_FOUND))
  IF (IS_UBUNTU)
    MESSAGE(FATAL_ERROR "dotnet sdk is required to build Emgu CV on Ubuntu, please follow the instructions to install dotnet sdk on Ubuntu: https://learn.microsoft.com/en-ca/dotnet/core/install/linux-ubuntu-install?tabs=dotnet9")  
  ELSEIF(IS_DEBIAN OR IS_RHEL)
    MESSAGE(FATAL_ERROR "dotnet sdk is required to build Emgu CV")  
  ENDIF()
ENDIF()

INCLUDE (CheckTargetArch)
INCLUDE (OpenCVVersion)

IF(WIN32)
  INCLUDE (FindCodeSign)
ENDIF()

# ----------------------------------------------------------------------------
#  Check if 32bit or 64-bit
# ----------------------------------------------------------------------------
CHECK_TARGET_ARCH()

SET(EMGUCV_ARCH ${TARGET_ARCH_NAME})
MESSAGE(STATUS "Building for ${EMGUCV_ARCH}")

IF ( TARGET_ARCH_64 )
  IF (TARGET_ARM)
    SET(NUGET_NATIVE_PLATFORM_ID_ARCH "arm64")
  ELSE()
    SET(NUGET_NATIVE_PLATFORM_ID_ARCH "x64")
  ENDIF()
ELSE()
  IF (TARGET_ARM)
    SET(NUGET_NATIVE_PLATFORM_ID_ARCH "arm")
  ELSE()
    SET(NUGET_NATIVE_PLATFORM_ID_ARCH "x86")
  ENDIF()
ENDIF()

IF(WIN32 AND CV_ICC)
  #  SET(CV_ICC_ADDITIONAL_FLAGS_RELEASE "/O3 /QaxCORE-AVX2,AVX,SSE4.1,SSE3 /Qparallel /Qopt-report:1 /Qopt-report-phase:vec /Qopt-report-phase:par /Qopt-report-file:stderr /Qipo /Qunroll-aggressive")
  SET(CV_ICC_ADDITIONAL_FLAGS_RELEASE "/O3 /QaxCORE-AVX2,AVX,SSE4.1,SSE3 /Qparallel /Qopt-report:1 /Qopt-report-phase:vec /Qopt-report-phase:par /Qipo /Qrestrict /Ot")
  STRING(REGEX REPLACE "/O2" ${CV_ICC_ADDITIONAL_FLAGS_RELEASE} CMAKE_C_FLAGS_RELEASE ${CMAKE_C_FLAGS_RELEASE})
  STRING(REGEX REPLACE "/O2" ${CV_ICC_ADDITIONAL_FLAGS_RELEASE} CMAKE_CXX_FLAGS_RELEASE ${CMAKE_CXX_FLAGS_RELEASE})
  SET(CMAKE_EXE_LINKER_FLAGS "${CMAKE_EXE_LINKER_FLAGS} /NODEFAULTLIB:\"libcmt\"")
  SET(CMAKE_SHARED_LINKER_FLAGS "${CMAKE_SHARED_LINKER_FLAGS} /NODEFAULTLIB:\"libcmt\"")
  set(EMGUCV_PLATFORM_TOOLSET "Intel C++ Compiler 19.1" CACHE STRING "Platform toolset for Visual Studio")
ENDIF()

IF(WIN32 AND NOT MINGW)
  SET(NETFX_CORE OFF CACHE BOOL "Build Windows Store App")
  #  IF(NETFX_CORE)
  #    add_definitions(-DWINAPI_FAMILY=WINAPI_FAMILY_APP -DWINVER=0x0602 -D_WIN32_WINNT=0x0602) 
  #	add_definitions(-D_UNICODE)
  #  ENDIF()
ENDIF()

INCLUDE("${CMAKE_CURRENT_SOURCE_DIR}/cmake/PlatformDetection.cmake")

INCLUDE("${CMAKE_CURRENT_SOURCE_DIR}/cmake/PackageNaming.cmake")

# ----------------------------------------------------------------------------
#  Build opencv and Emgu.CV.Extern
# ----------------------------------------------------------------------------
SET(CMAKE_RUNTIME_OUTPUT_DIRECTORY "${UNMANAGED_LIBRARY_OUTPUT_PATH}" )
SET(CMAKE_RUNTIME_OUTPUT_DIRECTORY_RELEASE "${CMAKE_RUNTIME_OUTPUT_DIRECTORY}")
SET(CMAKE_RUNTIME_OUTPUT_DIRECTORY_DEBUG "${CMAKE_RUNTIME_OUTPUT_DIRECTORY}")


SET(ENTRYPOINT_CS_STRING "")
SET(ENTRYPOINT_MODULES_STRING "")


IF(MSVC)
  set(ENABLE_SOLUTION_FOLDERS ON CACHE BOOL "Solution folder in Visual Studio")
  # On GitHub Actions windows-latest runners, Strawberry Perl ships MinGW's
  # libpthread.a to C:/Strawberry/c/lib/. find_package(Threads) finds it and
  # sets CMAKE_THREAD_LIBS_INIT="-lpthread" / CMAKE_USE_PTHREADS_INIT=1.
  # This causes every cmake target that links Threads::Threads (abseil, openjpeg,
  # depthai, ...) to propagate pthread.lib into cvextern's link line → LNK1181.
  #
  # Fix: create stub targets BEFORE any subdirectory can call find_package(Threads).
  #
  #  - "Threads::Threads" INTERFACE IMPORTED GLOBAL: FindThreads.cmake only creates
  #    this target when (NOT TARGET Threads::Threads), so our empty stub prevents
  #    it from ever setting INTERFACE_LINK_LIBRARIES = "-lpthread".
  #
  #  - "pthread" INTERFACE: intercepts the raw string "pthread" when code calls
  #    target_link_libraries(X PRIVATE pthread) — cmake sees a known target and
  #    treats it as a no-op, so "pthread.lib" never reaches the linker.
  #
  # Also pre-set the check_library_exists cache variables so the probe is skipped.
  if(NOT TARGET Threads::Threads)
    add_library(Threads::Threads INTERFACE IMPORTED GLOBAL)
  endif()
  if(NOT TARGET pthread)
    add_library(pthread INTERFACE IMPORTED GLOBAL)
  endif()
  set(CMAKE_HAVE_PTHREADS_CREATE "" CACHE INTERNAL "" FORCE)
  set(CMAKE_HAVE_PTHREAD_CREATE   "" CACHE INTERNAL "" FORCE)
  set(Threads_FOUND TRUE CACHE BOOL "" FORCE)
ENDIF()
ADD_SUBDIRECTORY(${OPENCV_SUBFOLDER})

SET (OPENCV_MODULE_NAME_CANDIDATES
  core geometry ptcloud imgproc video flann ml imgcodecs videoio highgui objdetect features calib 3d
  cudaarithm cudawarping cudafilters cudaimgproc cudalegacy cudaoptflow 
  cudastereo cudabgsegm cudafeatures2d cudacodec cudaobjdetect
  photo superres  
  videostab shape
  cudev  
  face ximgproc xobjdetect xfeatures2d xphoto bgsegm optflow
  stitching plot
  dnn dnn_objdetect dnn_superres fuzzy viz bioinspired text tracking
  dpm
  saliency
  img_hash
  phase_unwrapping
  freetype
  hfs
  quality
  stereo
  surface_matching
  alphamat
  intensity_transform
  signal
  rapid
  line_descriptor
  hdf
  gapi
  wechat_qrcode
  structured_light
  rgbd
  reg
  xstereo
  )
  
IF(BUILD_opencv_world)
  LIST (APPEND OPENCV_MODULE_NAME_CANDIDATES world)
ENDIF()

FOREACH(OPENCV_MODULE_NAME ${OPENCV_MODULE_NAME_CANDIDATES})
  IF(TARGET opencv_${OPENCV_MODULE_NAME})
    LIST(APPEND OPENCV_MODULE_NAMES ${OPENCV_MODULE_NAME})  
  ENDIF() 
ENDFOREACH()

# ----------------------------------------------------------------------------
#  Autodetect if we are in a GIT repository
# ----------------------------------------------------------------------------

IF(GIT_EXECUTABLE)
  MESSAGE(STATUS "Extracting Emgu CV git version, please wait...")
  EXECUTE_PROCESS(
    WORKING_DIRECTORY  ${CMAKE_CURRENT_SOURCE_DIR}
    COMMAND ${GIT_EXECUTABLE} rev-list HEAD --count
    OUTPUT_VARIABLE GITVERSION_RESULT)
  STRING(REGEX REPLACE "^([0123456789]+).*" "\\1" EMGUCV_GITVERSION "${GITVERSION_RESULT}")
  STRING(STRIP "${EMGUCV_GITVERSION}" EMGUCV_GITVERSION)
  IF(EMGUCV_GITVERSION MATCHES "^$" OR EMGUCV_GITVERSION MATCHES "^-")
    # This is NOT a git repository:
    SET(EMGUCV_GITVERSION "1")
  ENDIF()
  MESSAGE(STATUS "Emgu CV GIT VERSION: ${EMGUCV_GITVERSION}")
ELSE()
  # We don't have git:
  SET(EMGUCV_GITVERSION "1")
ENDIF()


SET(CPACK_PACKAGE_VENDOR "Emgu Corporation")
SET(CPACK_PACKAGE_VERSION_MAJOR ${OPENCV_VERSION_MAJOR})
SET(CPACK_PACKAGE_VERSION_MINOR ${OPENCV_VERSION_MINOR})
SET(CPACK_PACKAGE_VERSION_PATCH ${OPENCV_VERSION_PATCH})
SET(CPACK_PACKAGE_VERSION "${CPACK_PACKAGE_VERSION_MAJOR}.${CPACK_PACKAGE_VERSION_MINOR}.${CPACK_PACKAGE_VERSION_PATCH}.${EMGUCV_GITVERSION}")
IF(ANDROID)
  SET(CPACK_PACKAGE_FILE_NAME "lib${CPACK_PACKAGE_NAME}")
ELSE()
  SET(CPACK_PACKAGE_FILE_NAME "lib${CPACK_PACKAGE_NAME}-${CPACK_PACKAGE_VERSION}")
ENDIF()

MESSAGE(STATUS "Package name will be ${CPACK_PACKAGE_NAME}")
FILE(WRITE "emgucv.version.txt" "${CPACK_PACKAGE_VERSION}")

MATH(EXPR CPACK_PACKAGE_VERSION_PATCH_PLUS_ONE "${CPACK_PACKAGE_VERSION_PATCH}+1")
SET(CPACK_PACKAGE_DEPENDENCY_VERSION "${CPACK_PACKAGE_VERSION}")
# Use the full 4-part version: "nuget pack" normalizes a floating version such
# as "4.13.0.*" down to its 3-part minimum "4.13.0", which is not a published
# package version and makes every consumer emit warning NU1603 (issue #1012).
SET(NUGET_PACKAGE_DEPENDENCY_VERSION "${CPACK_PACKAGE_VERSION}")

SET(EMGU_CV_WITH_TESSERACT ON CACHE BOOL "BUILD TESSERACT OCR")

IF (TARGET_ARCH_64)
  SET(EMGU_ENABLE_SSE OFF CACHE BOOL "ENABLE SSE for Emgu CV")
ELSE()
  SET(EMGU_ENABLE_SSE ON CACHE BOOL "ENABLE SSE for Emgu CV")
ENDIF()
ADD_SUBDIRECTORY(Emgu.CV.Extern)
ADD_SUBDIRECTORY(Emgu.CV.NativeImage)

IF(NOT (ANDROID OR IOS OR NETFX_CORE OR APPLE OR IS_EMSCRIPTEN_BUILD))
  ENABLE_TESTING()
  ADD_SUBDIRECTORY(tests/cvextern_test)
ENDIF()

SET(CMAKE_RUNTIME_OUTPUT_DIRECTORY "${LIBRARY_OUTPUT_PATH}" )
SET(CMAKE_RUNTIME_OUTPUT_DIRECTORY_RELEASE "${CMAKE_RUNTIME_OUTPUT_DIRECTORY}")
SET(CMAKE_RUNTIME_OUTPUT_DIRECTORY_DEBUG "${CMAKE_RUNTIME_OUTPUT_DIRECTORY}")

SET(EMGU_CV_BUILD ON CACHE BOOL "BUILD Emgu CV")
SET(EMGU_CV_EXAMPLE_BUILD ON CACHE BOOL "Build Emgu CV Example")

# ----------------------------------------------------------------------------
#  Generate required common assembly file for Emgu CV project
# ----------------------------------------------------------------------------
INCLUDE(Today)
TODAY() #set the variable CURRENT_YEAR

CONFIGURE_FILE(${CMAKE_CURRENT_SOURCE_DIR}/cmake/CommonAssemblyInfo.cs.in ${CMAKE_CURRENT_SOURCE_DIR}/CommonAssemblyInfo.cs)

IF(EMGU_CV_WITH_DEPTHAI)
  SET(EMGU_CV_WITH_DEPTHAI_CS "true")
ELSE()
  SET(EMGU_CV_WITH_DEPTHAI_CS "false")
ENDIF()

CONFIGURE_FILE(${CMAKE_CURRENT_SOURCE_DIR}/cmake/Directory.Build.props.in ${CMAKE_CURRENT_SOURCE_DIR}/Directory.Build.props)

CONFIGURE_FILE(${CMAKE_CURRENT_SOURCE_DIR}/cmake/version.rc.in ${CMAKE_CURRENT_SOURCE_DIR}/Emgu.CV.Extern/version.rc)

SET (EMGUCV_LICENSE_FILE "${LICENSE_FILE_NAME}.txt")
CONFIGURE_FILE(${CMAKE_CURRENT_SOURCE_DIR}/${LICENSE_FILE_NAME} ${CMAKE_CURRENT_SOURCE_DIR}/${EMGUCV_LICENSE_FILE} COPYONLY)

MACRO(CHECK_LICENSE_TYPE)
	MESSAGE(STATUS "LICENSE FILE: ${CMAKE_CURRENT_SOURCE_DIR}/${LICENSE_FILE_NAME}")
    IF (WIN32)
        EXECUTE_PROCESS(COMMAND "findstr" "gpl" "${LICENSE_FILE_NAME}" WORKING_DIRECTORY ${CMAKE_CURRENT_SOURCE_DIR} OUTPUT_VARIABLE TMP)
	ELSE()	
	    EXECUTE_PROCESS(COMMAND "grep" "gpl" "${LICENSE_FILE_NAME}" WORKING_DIRECTORY ${CMAKE_CURRENT_SOURCE_DIR} OUTPUT_VARIABLE TMP)
	ENDIF()
	
	STRING(STRIP "${TMP}" TMP)
	STRING(LENGTH "${TMP}" GPL_LICENSE_LENGTH)
	SET (LICENSE_TYPE "Dual License")
	
	IF(${GPL_LICENSE_LENGTH} EQUAL 0)
		SET(LICENSE_TYPE "Commercial")		
	ENDIF()
	
	MESSAGE(STATUS "LICENSE_TYPE: ${LICENSE_TYPE}")
ENDMACRO()
CHECK_LICENSE_TYPE()

INCLUDE("${CMAKE_CURRENT_SOURCE_DIR}/cmake/NugetIds.cmake")

# ----------------------------------------------------------------------------
#  Generate readme file for Emgu CV project
# ----------------------------------------------------------------------------
CONFIGURE_FILE(${EMGU_CV_SOURCE_DIR}/cmake/README.txt.in ${EMGU_CV_SOURCE_DIR}/Emgu.CV/README.txt)
INSTALL(FILES "${CMAKE_CURRENT_SOURCE_DIR}/Emgu.CV/README.txt" DESTINATION "." COMPONENT emgucv_source)

# ----------------------------------------------------------------------------
#  Include license file & nuget icon into package
# ----------------------------------------------------------------------------
INSTALL(FILES "${CMAKE_CURRENT_SOURCE_DIR}/LICENSE.txt" DESTINATION "." COMPONENT emgucv_source)
INSTALL(FILES "${CMAKE_CURRENT_SOURCE_DIR}/miscellaneous/nuget_icon.png" DESTINATION "miscellaneous/" COMPONENT emgucv_source)

INCLUDE("${CMAKE_CURRENT_SOURCE_DIR}/cmake/CvInvokeEntryPoints.cmake")

# ----------------------------------------------------------------------------
#  Build the managed dlls of Emgu CV
# ----------------------------------------------------------------------------
ADD_SUBDIRECTORY(Emgu.Util)
ADD_SUBDIRECTORY(Emgu.CV)
ADD_SUBDIRECTORY(Emgu.CV.Bitmap)
ADD_SUBDIRECTORY(Emgu.CV.Wpf)
ADD_SUBDIRECTORY(Emgu.CV.WinForms)

ADD_SUBDIRECTORY(Emgu.CV.Test)
ADD_SUBDIRECTORY(Emgu.CV.Cuda)

ADD_SUBDIRECTORY(Emgu.CV.OCR)
 
ADD_SUBDIRECTORY(Emgu.CV.Contrib)
ADD_SUBDIRECTORY(Emgu.CV.Models)

ADD_SUBDIRECTORY(platforms/nuget)

# MAUI will need to use some of the nuget packages. It need to be build after nuget packages is build.
ADD_SUBDIRECTORY(Emgu.CV.Runtime)

# Example may need to use some of the Emgu.CV.Runtime library. It need to be defined after Emgu.CV.Runtime
ADD_SUBDIRECTORY(Emgu.CV.Example)

INSTALL(
  FILES
  "opencv/LICENSE"
  DESTINATION "lib"
  COMPONENT emgucv_example_source
  RENAME "opencv.license.txt"
  )    

IF(HAVE_MAUI_ANDROID)
  INSTALL(
    FILES
    "Solution/Android/Emgu.CV.Android.Example.sln"
    "Solution/Android/Emgu.CV.Android.sln"
    DESTINATION "Solution/Android"
    COMPONENT emgucv_example_source
    )

ENDIF()

IF(HAVE_IOS)
  INSTALL(
    FILES
    "Solution/iOS/Emgu.CV.iOS.Example.sln"
    "Solution/iOS/Emgu.CV.iOS.Maui.sln"
    DESTINATION "Solution/iOS"
    COMPONENT emgucv_example_source
    )
ENDIF()

IF(HAVE_MACOS)
  INSTALL(
    FILES
    "Solution/Mac/Emgu.CV.Mac.Example.sln"
    DESTINATION "Solution/Mac"
    COMPONENT emgucv_example_source
    )
  IF (NOT IS_MACOS_BUILD)
    INSTALL(
      FILES
      "libs/runtimes/osx/native/libcvextern.dylib"
      DESTINATION "libs/runtimes/osx/native"
      COMPONENT emgucv_binary
      )
  ENDIF()
ENDIF()

IF(HAVE_MAC_CATALYST)
  INSTALL(
    FILES
    "Solution/Mac/Emgu.CV.Mac.Maui.sln"
    DESTINATION "Solution/Mac"
    COMPONENT emgucv_example_source
    )
  IF (HAVE_MAC_CATALYST_ARM64)
    INSTALL(
      FILES
      "libs/iOS/libcvextern_catalyst_arm64.a"
      DESTINATION "libs/iOS"
      COMPONENT emgucv_binary
      )
  ENDIF()
  IF (HAVE_MAC_CATALYST_X86_64)
    INSTALL(
      FILES
      "libs/iOS/libcvextern_catalyst_x86_64.a"
      DESTINATION "libs/iOS"
      COMPONENT emgucv_binary
      )
  ENDIF()
ENDIF()

IF (TARGET Emgu.CV.runtime.maui)
  INSTALL(
    FILES
    "Solution/CrossPlatform/Emgu.CV.CrossPlatform.sln"
	"Solution/CrossPlatform/Emgu.CV.sln"
    DESTINATION "Solution/CrossPlatform"
    COMPONENT emgucv_example_source
    )
ENDIF()

INSTALL(
  FILES
  "${CMAKE_CURRENT_SOURCE_DIR}/Emgu.CV.snk"
  "${CMAKE_CURRENT_SOURCE_DIR}/CommonAssemblyInfo.cs"
  "${CMAKE_CURRENT_SOURCE_DIR}/Directory.Build.props"
  "${CMAKE_CURRENT_SOURCE_DIR}/${LICENSE_FILE_NAME}"
  DESTINATION .
  COMPONENT emgucv_source
  )
  

IF (ANDROID)
  SET(CPACK_GENERATOR ZIP)
ELSEIF (WIN32)
INCLUDE("${CMAKE_CURRENT_SOURCE_DIR}/cmake/CPackWindows.cmake")
 
  
ELSEIF (APPLE)
  
INCLUDE("${CMAKE_CURRENT_SOURCE_DIR}/cmake/CPackApple.cmake")
  
ELSE() #LINUX
  SET(MONO_GACUTIL_EXEC "gacutil")
  SET(EMGUCV_MANAGED_DLLS "Emgu.CV")

  IF(NOT CPACK_GENERATOR)
    MESSAGE(STATUS "Checking for CPACK build type...")
    FIND_PROGRAM(ZIP_CMD zip)
    IF (ZIP_CMD)
      MESSAGE(STATUS "FOUND zip: ${ZIP_CMD}")
      LIST(APPEND CPACK_GENERATOR ZIP)
    ENDIF()
  ENDIF()
ENDIF()

MESSAGE(STATUS "CPACK_GENERATOR: ${CPACK_GENERATOR}")
INCLUDE (CPack) 
