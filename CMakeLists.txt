# --------------------------------------------------------
# Copyright (C) 2004-2013 by EMGU. All rights reserved.
# 
# Root CMake file for Emgu CV
#
# From the off-tree build directory, invoke:
#   $ cmake <PATH_TO_EMGUCV_ROOT>
#
# --------------------------------------------------------

PROJECT(emgucv)
CMAKE_MINIMUM_REQUIRED(VERSION 2.8.1)

SET(OPENCV_SUBFOLDER ${CMAKE_CURRENT_SOURCE_DIR}/opencv)

SET(CMAKE_MODULE_PATH ${CMAKE_MODULE_PATH} "${CMAKE_CURRENT_SOURCE_DIR}/cmake/modules")
IF (NOT ANDROID)
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
ENDIF()

INCLUDE (CheckTargetArch)
INCLUDE (OpenCVVersion)

SET(LIBRARY_3RDPARTY_PATH ${CMAKE_CURRENT_SOURCE_DIR}/lib/3rdParty)

# ----------------------------------------------------------------------------
#  Autodetect if we are in a GIT repository
# ----------------------------------------------------------------------------
FIND_PROGRAM(GIT_PATH git)
FIND_PROGRAM(WC_PATH wc)
MARK_AS_ADVANCED(force GIT_PATH WC_PATH)
IF(GIT_PATH AND WC_PATH)
  MESSAGE(STATUS "Extracting Emgu CV git version, please wait...")
  EXECUTE_PROCESS(
    WORKING_DIRECTORY  ${CMAKE_CURRENT_SOURCE_DIR}
    COMMAND ${GIT_PATH} log --oneline 
    COMMAND ${WC_PATH} -l
    OUTPUT_VARIABLE GITVERSION_RESULT)
  #MESSAGE(STATUS "COMMAND: ${GIT_PATH} log --oneline | ${WC_PATH} -l")
  STRING(REGEX REPLACE "^([0123456789]+).*" "\\1" EMGUCV_GITVERSION "${GITVERSION_RESULT}")
  STRING(STRIP "${EMGUCV_GITVERSION}" EMGUCV_GITVERSION)
  IF(EMGUCV_GITVERSION MATCHES "^$" OR EMGUCV_GITVERSION MATCHES "^-")
    # This is NOT a git repository:
    SET(EMGUCV_GITVERSION "0")
  ENDIF()
  MESSAGE(STATUS "Emgu CV GIT VERSION: ${EMGUCV_GITVERSION}")
ELSE()
  # We don't have git or wc:
  SET(EMGUCV_GITVERSION "0")
ENDIF()

# ----------------------------------------------------------------------------
#  Check if 32bit or 64-bit
# ----------------------------------------------------------------------------
CHECK_TARGET_ARCH()
IF (TARGET_ARCH_64)
  MESSAGE(STATUS "Building 64bit library")
ELSE()
  MESSAGE(STATUS "Building 32bit library")
ENDIF()

IF (WIN32)
  IF(TARGET_ARCH_64)
    SET(EMGUCV_ARCH win64)
  ELSE()
    SET(EMGUCV_ARCH win32)
  ENDIF()
ELSE(WIN32)
  SET(EMGUCV_ARCH ${CMAKE_SYSTEM_NAME})
ENDIF(WIN32)

IF(WIN32 AND CV_ICC)
  SET(CV_ICC_ADDITIONAL_FLAGS_RELEASE "/O3 /QaxAVX,SSE4.1,SSE3 /Qparallel /Qvec-report1 /Qpar-report1 /Qipo /Qunroll-aggressive")
  STRING(REGEX REPLACE "/O2" ${CV_ICC_ADDITIONAL_FLAGS_RELEASE} CMAKE_C_FLAGS_RELEASE ${CMAKE_C_FLAGS_RELEASE})
  STRING(REGEX REPLACE "/O2" ${CV_ICC_ADDITIONAL_FLAGS_RELEASE} CMAKE_CXX_FLAGS_RELEASE ${CMAKE_CXX_FLAGS_RELEASE})
  SET(CMAKE_EXE_LINKER_FLAGS "${CMAKE_EXE_LINKER_FLAGS} /NODEFAULTLIB:\"libcmt\"")
  SET(CMAKE_SHARED_LINKER_FLAGS "${CMAKE_SHARED_LINKER_FLAGS} /NODEFAULTLIB:\"libcmt\"")
  set(EMGUCV_PLATFORM_TOOLSET "Intel C++ Compiler XE 14.0" CACHE STRING "Platform toolset for Visual Studio")
ENDIF()

IF(WIN32 AND NOT MINGW)
  SET(NETFX_CORE OFF CACHE BOOL "Build Windows Store App")
  IF(NETFX_CORE)
    add_definitions(-DWINAPI_FAMILY=WINAPI_FAMILY_APP -DWINVER=0x0602 -D_WIN32_WINNT=0x0602) 
	add_definitions(-D_UNICODE)
  ENDIF()
ENDIF()

SET(LIBRARY_OUTPUT_PATH ${CMAKE_SOURCE_DIR}/bin)
IF(ANDROID)
  SET(UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER "/${ANDROID_ABI}")
ELSEIF(WIN32)
  IF (TARGET_ARCH_64)
    SET(UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER "/x64")
	#Check for x86 binary as well...
	FILE(GLOB UNMANAGED_DLL_32BIT "${CMAKE_CURRENT_SOURCE_DIR}/bin/x86/*.dll")
    LIST(LENGTH UNMANAGED_DLL_32BIT UNMANAGED_DLL_32BIT_LENGTH)
	IF(UNMANAGED_DLL_32BIT_LENGTH GREATER 0)
	   SET(UNIVERSAL_BUILD ON)
	   MESSAGE(STATUS "Performing a universal build.")	
	ENDIF()
  ELSE()
    SET(UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER "/x86")
  ENDIF()
#ELSEIF(APPLE)
#  SET(UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER "/lib")
ELSEIF()
  SET(UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER "")
ENDIF()

SET(UNMANAGED_LIBRARY_OUTPUT_PATH "${LIBRARY_OUTPUT_PATH}${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER}")
IF(ANDROID)
  SET(LIBRARY_OUTPUT_PATH ${UNMANAGED_LIBRARY_OUTPUT_PATH})
ENDIF()
FILE(MAKE_DIRECTORY ${UNMANAGED_LIBRARY_OUTPUT_PATH})

#MESSAGE(STATUS "Changing library output path to '${LIBRARY_OUTPUT_PATH}'")

# ----------------------------------------------------------------------------
#  Setup Package information
# ----------------------------------------------------------------------------
SET(PACKAGE "${PROJECT_NAME}")
SET(CPACK_PACKAGE_CONTACT "Canming Huang <support@emgu.com>")
SET(PACKAGE_BUGREPORT "${CPACK_PACKAGE_CONTACT}")
SET(PACKAGE_NAME "${PROJECT_NAME}")

SET(CPACK_PACKAGE_NAME "${PACKAGE_NAME}")
SET(CPACK_PACKAGE_DESCRIPTION_SUMMARY "Emgu CV is a cross platform .Net wrapper to the OpenCV image processing library.")

IF(ANDROID)
  SET(CPACK_PACKAGE_NAME "${CPACK_PACKAGE_NAME}-android")
ELSEIF (APPLE)
  SET(CPACK_PACKAGE_NAME "${CPACK_PACKAGE_NAME}-osx")
ELSEIF (WIN32)
  IF (NETFX_CORE)
    SET(CPACK_PACKAGE_NAME "${CPACK_PACKAGE_NAME}-windows-store") 
  ELSE()
    SET(CPACK_PACKAGE_NAME "${CPACK_PACKAGE_NAME}-windows") 
  ENDIF()
ENDIF()

IF(ANDROID)
  SET(CPACK_PACKAGE_NAME "${CPACK_PACKAGE_NAME}-${ANDROID_ABI}")
ELSEIF(TARGET_ARCH_64)
  IF(DEFINED UNIVERSAL_BUILD)
    SET(CPACK_PACKAGE_NAME "${CPACK_PACKAGE_NAME}-universal") 
  ELSE()
    SET(CPACK_PACKAGE_NAME "${CPACK_PACKAGE_NAME}-x64") 
  ENDIF()
ELSE()
  SET(CPACK_PACKAGE_NAME "${CPACK_PACKAGE_NAME}-x86")
ENDIF()

IF(WITH_CUDA)
  SET(CPACK_PACKAGE_NAME "${CPACK_PACKAGE_NAME}-cuda")
ENDIF()

IF(WITH_OPENNI) 
  SET(CPACK_PACKAGE_NAME "${CPACK_PACKAGE_NAME}-openni")
ENDIF()
IF(WITH_TBB)
  SET(CPACK_PACKAGE_NAME "${CPACK_PACKAGE_NAME}-tbb")
ENDIF()
IF(WITH_IPP)
  SET(CPACK_PACKAGE_NAME "${CPACK_PACKAGE_NAME}-ipp")
ENDIF()
IF(CV_ICC)
  SET(CPACK_PACKAGE_NAME "${CPACK_PACKAGE_NAME}-icc")
ENDIF()

SET(CPACK_PACKAGE_VENDOR "Emgu")
SET(CPACK_PACKAGE_VERSION_MAJOR ${OPENCV_VERSION_MAJOR})
SET(CPACK_PACKAGE_VERSION_MINOR ${OPENCV_VERSION_MINOR})
SET(CPACK_PACKAGE_VERSION_PATCH ${OPENCV_VERSION_PATCH})
SET(CPACK_PACKAGE_VERSION "${CPACK_PACKAGE_VERSION_MAJOR}.${CPACK_PACKAGE_VERSION_MINOR}.${CPACK_PACKAGE_VERSION_PATCH}.${EMGUCV_GITVERSION}")
IF(ANDROID)
  SET(CPACK_PACKAGE_FILE_NAME "lib${CPACK_PACKAGE_NAME}")
ELSE()
  SET(CPACK_PACKAGE_FILE_NAME "lib${CPACK_PACKAGE_NAME}-${CPACK_PACKAGE_VERSION}")
ENDIF()

SET(CPACK_RESOURCE_FILE_LICENSE "${CMAKE_CURRENT_SOURCE_DIR}/Emgu.CV.License.txt")

IF (WIN32 OR APPLE)
  SET(CPACK_PACKAGE_CLI_FOLDER bin)
ELSE ()
  SET(CPACK_PACKAGE_CLI_FOLDER lib/cli/${PACKAGE_NAME}-${CPACK_PACKAGE_VERSION_MAJOR}.${CPACK_PACKAGE_VERSION_MINOR})
ENDIF()



# ----------------------------------------------------------------------------
#  Build opencv and Emgu.CV.Extern
# ----------------------------------------------------------------------------
SET(CMAKE_RUNTIME_OUTPUT_DIRECTORY "${UNMANAGED_LIBRARY_OUTPUT_PATH}" )
SET(CMAKE_RUNTIME_OUTPUT_DIRECTORY_RELEASE "${CMAKE_RUNTIME_OUTPUT_DIRECTORY}")
SET(CMAKE_RUNTIME_OUTPUT_DIRECTORY_DEBUG "${CMAKE_RUNTIME_OUTPUT_DIRECTORY}")

#SET(CMAKE_ARCHIEVE_OUTPUT_DIRECTORY "${CMAKE_RUNTIME_OUTPUT_DIRECTORY}")
#SET(CMAKE_ARCHIEVE_OUTPUT_DIRECTORY_DEBUG "${CMAKE_ARCHIEVE_OUTPUT_DIRECTORY}")
#SET(CMAKE_ARCHIEVE_OUTPUT_DIRECTORY_RELEASE "${CMAKE_ARCHIEVE_OUTPUT_DIRECTORY}")

#SET(CMAKE_INSTALL_PREFIX_COPY ${CMAKE_INSTALL_PREFIX})
#SET(CMAKE_INSTALL_PREFIX ${CMAKE_INSTALL_PREFIX}${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER})
#SET(CPACK_PACKAGING_INSTALL_PREFIX ${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER})
#SET(DESTDIR ${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER})
#SET(CPACK_OUTPUT_FILE_PREFIX "x64")

SET(ENTRYPOINT_CS_STRING "")
SET(ENTRYPOINT_MODULES_STRING "")

#Add opencl
IF(WIN32 AND (NOT NETFX_CORE) )
  IF(WITH_OPENCL)
    MESSAGE(STATUS "Open CL build included")
	SET(OPENCL_SOURCE_FOLDER "${CMAKE_CURRENT_SOURCE_DIR}/lib/opencl-icd-1.2.11.0/icd")
	ADD_SUBDIRECTORY(${OPENCL_SOURCE_FOLDER})
	#GET_TARGET_PROPERTY(OPENCL_OUTPUT_NAME OpenCL OUTPUT_NAME)
	
	set(OPENCL_FOUND YES)
	set(OPENCL_LIBRARY "${CMAKE_CURRENT_SOURCE_DIR}/bin/Release/opencl.lib" CACHE STRING "OpenCL library")
	set(OPENCL_INCLUDE_DIR "${OPENCL_SOURCE_FOLDER}/inc" CACHE STRING "OpenCL include directory")
	mark_as_advanced(OPENCL_INCLUDE_DIR OPENCL_LIBRARY)
	
	SET(ENTRYPOINT_MODULES_STRING "${ENTRYPOINT_MODULES_STRING}
	     OpenclLibrary,")
    SET(ENTRYPOINT_CS_STRING "${ENTRYPOINT_CS_STRING} 
      /// <summary>
      /// The file name of the OpenCL library
      /// </summary>
      public const string OpenclLibrary = \"OpenCL\";")
  ELSE()
    SET(OPENCL_OUTPUT_NAME "")
	MESSAGE(STATUS "Open CL build not included")
  ENDIF()
ENDIF()

SET(CMAKE_OLD_SOURCE_DIR ${CMAKE_SOURCE_DIR})
SET(CMAKE_SOURCE_DIR ${OPENCV_SUBFOLDER})
IF(MSVC)
   set(ENABLE_SOLUTION_FOLDERS ON CACHE BOOL "Solution folder in Visual Studio")
ENDIF()
ADD_SUBDIRECTORY(${OPENCV_SUBFOLDER})
SET(CMAKE_SOURCE_DIR ${CMAKE_OLD_SOURCE_DIR})

SET (OPENCV_MODULE_NAME_CANDIDATES
  core imgproc video flann ml highgui objdetect features2d calib3d
  legacy contrib 
  cudalegacy cudaarithm cudawarping cuda cudafilters cudaimgproc cudaoptflow 
  cudastereo cudabgsegm cudafeatures2d 
  ocl
  photo bioinspired superres nonfree stitching 
  videostab  optim softcascade shape
  cudev  cudacodec 
  )
  
FOREACH(OPENCV_MODULE_NAME ${OPENCV_MODULE_NAME_CANDIDATES})
IF(TARGET opencv_${OPENCV_MODULE_NAME})
  LIST(APPEND OPENCV_MODULE_NAMES ${OPENCV_MODULE_NAME})
ENDIF() 
ENDFOREACH()

IF (TARGET_ARCH_64)
  SET(EMGU_ENABLE_SSE OFF CACHE BOOL "ENABLE SSE for Emgu CV")
ELSE()
  SET(EMGU_ENABLE_SSE ON CACHE BOOL "ENABLE SSE for Emgu CV")
ENDIF()
ADD_SUBDIRECTORY(Emgu.CV.Extern)

IF(NOT (ANDROID OR IOS))
  ENABLE_TESTING()
  ADD_SUBDIRECTORY(tests/cvextern_test)
ENDIF()

SET(CMAKE_RUNTIME_OUTPUT_DIRECTORY "${LIBRARY_OUTPUT_PATH}" )
SET(CMAKE_RUNTIME_OUTPUT_DIRECTORY_RELEASE "${CMAKE_RUNTIME_OUTPUT_DIRECTORY}")
SET(CMAKE_RUNTIME_OUTPUT_DIRECTORY_DEBUG "${CMAKE_RUNTIME_OUTPUT_DIRECTORY}")
#SET(CMAKE_INSTALL_PREFIX ${CMAKE_INSTALL_PREFIX_COPY})
#SET(CPACK_PACKAGING_INSTALL_PREFIX "")
#SET(DESTDIR "")
#SET(CPACK_OUTPUT_FILE_PREFIX "")

SET(EMGU_CV_BUILD ON CACHE BOOL "BUILD Emgu CV")
SET(EMGU_CV_EXAMPLE_BUILD ON CACHE BOOL "Build Emgu CV Example")

# ----------------------------------------------------------------------------
#  Generate required common assembly file for Emgu CV project
# ----------------------------------------------------------------------------
INCLUDE(Today)
TODAY() #set the variable CURRENT_YEAR
CONFIGURE_FILE(${CMAKE_CURRENT_SOURCE_DIR}/cmake/CommonAssemblyInfo.cs.in ${CMAKE_CURRENT_SOURCE_DIR}/CommonAssemblyInfo.cs)

# ----------------------------------------------------------------------------
#  Generate readme file for Emgu CV project
# ----------------------------------------------------------------------------
CONFIGURE_FILE(${CMAKE_CURRENT_SOURCE_DIR}/cmake/README.txt.in ${CMAKE_CURRENT_SOURCE_DIR}/Emgu.CV/README.txt)
INSTALL(FILES "${CMAKE_CURRENT_SOURCE_DIR}/Emgu.CV/README.txt" DESTINATION "." COMPONENT main)

# ----------------------------------------------------------------------------
#  Generate CvInvokeEntryPoints.cs file for Emgu CV project
# ----------------------------------------------------------------------------
SET(OPENCV_LIBRARY_OUTPUT_NAME_PREFIX "")
IF (APPLE)
  SET(OPENCV_LIBRARY_OUTPUT_NAME_POSTFIX ".${OPENCV_VERSION_MAJOR}.${OPENCV_VERSION_MINOR}.${OPENCV_VERSION_PATCH}")
ELSE()
  SET(OPENCV_LIBRARY_OUTPUT_NAME_POSTFIX "")
ENDIF()


SET(OPENCV_LIBRARY_OUTPUT_NAMES )

FOREACH(OPENCV_MODULE_NAME ${OPENCV_MODULE_NAME_CANDIDATES})
  STRING (TOUPPER "${OPENCV_MODULE_NAME}" OPENCV_MODULE_NAME_UPPER)
  
  STRING (SUBSTRING "${OPENCV_MODULE_NAME}" 0 1 OPENCV_MODULE_NAME_FIRST_LETTER)
  STRING (TOUPPER "${OPENCV_MODULE_NAME_FIRST_LETTER}" OPENCV_MODULE_NAME_FIRST_LETTER_UPPER)
  STRING (SUBSTRING "${OPENCV_MODULE_NAME}" 1 -1 OPENCV_MODULE_NAME_REMAINDER)
  SET(OPENCV_MODULE_NAME_FIRST_LETTER_UPPER "${OPENCV_MODULE_NAME_FIRST_LETTER_UPPER}${OPENCV_MODULE_NAME_REMAINDER}")
  
  IF(NOT (TARGET opencv_${OPENCV_MODULE_NAME}))
    SET(OPENCV_${OPENCV_MODULE_NAME_UPPER}_OUTPUT_NAME "")
  ELSE()
    GET_TARGET_PROPERTY(OPENCV_OUTPUT_NAME_TMP opencv_${OPENCV_MODULE_NAME} OUTPUT_NAME)
    SET(OPENCV_${OPENCV_MODULE_NAME_UPPER}_OUTPUT_NAME  ${OPENCV_LIBRARY_OUTPUT_NAME_PREFIX}${OPENCV_OUTPUT_NAME_TMP}${OPENCV_LIBRARY_OUTPUT_NAME_POSTFIX})
	LIST(APPEND OPENCV_LIBRARY_OUTPUT_NAMES ${OPENCV_${OPENCV_MODULE_NAME_UPPER}_OUTPUT_NAME})
	SET(ENTRYPOINT_MODULES_STRING "${ENTRYPOINT_MODULES_STRING}
	     Opencv${OPENCV_MODULE_NAME_FIRST_LETTER_UPPER}Library,")
  ENDIF()

  IF(IOS)
    SET(ENTRYPOINT_CS_STRING "${ENTRYPOINT_CS_STRING} 
      /// <summary>
      /// The file name of the ${OPENCV_MODULE_NAME} library
      /// </summary>
      public const string Opencv${OPENCV_MODULE_NAME_FIRST_LETTER_UPPER}Library = \"__Internal\";")
  ELSE()
    SET(ENTRYPOINT_CS_STRING "${ENTRYPOINT_CS_STRING} 
      /// <summary>
      /// The file name of the ${OPENCV_MODULE_NAME} library
      /// </summary>
      public const string Opencv${OPENCV_MODULE_NAME_FIRST_LETTER_UPPER}Library = \"${OPENCV_${OPENCV_MODULE_NAME_UPPER}_OUTPUT_NAME}\";")
  ENDIF()
ENDFOREACH()

SET(ENTRYPOINT_MODULES_STRING "public static List<String> OpenCVModuleList = new List<String>
	  {
        OpencvFFMpegLibrary,${ENTRYPOINT_MODULES_STRING}
        ExternLibrary
      };
")

GET_TARGET_PROPERTY(CVEXTERN_OUTPUT_NAME cvextern OUTPUT_NAME)
LIST(APPEND OPENCV_LIBRARY_OUTPUT_NAMES ${CVEXTERN_OUTPUT_NAME})

if(WIN32 AND WITH_FFMPEG)
  if(TARGET_ARCH_64)
    set(FFMPEG_SUFFIX _64)
  endif()
  STRING(LENGTH "${OPENCV_CORE_OUTPUT_NAME}" temp)

  MATH(EXPR temp "${temp}-3")
  STRING(SUBSTRING "${OPENCV_CORE_OUTPUT_NAME}" ${temp} -1 OPENCV_DLLVERSION)
  set(OPENCV_FFMPEG_OUTPUT_NAME "opencv_ffmpeg${OPENCV_DLLVERSION}${FFMPEG_SUFFIX}")
  #set(OPENCV_FFMPEG_OUTPUT_NAME "opencv_ffmpeg${FFMPEG_SUFFIX}")
  MESSAGE(STATUS "OPENCV_FFMPEG_OUTPUT_NAME: ${OPENCV_FFMPEG_OUTPUT_NAME}")
  LIST(APPEND OPENCV_LIBRARY_OUTPUT_NAMES ${OPENCV_FFMPEG_OUTPUT_NAME})
endif()
IF(TARGET_ARCH_64)
  SET(UNMANAGED_CODE_BITNESS "64")
ELSE()
  SET(UNMANAGED_CODE_BITNESS "32")
ENDIF()

#IF(ANDROID)
#  SET(CVINVOKE_ENTRYPOINTS_FILE_SUBFOLDER "Android/")
#ELSE()
SET(CVINVOKE_ENTRYPOINTS_FILE_SUBFOLDER "")
#ENDIF()

IF(IOS)
  SET(CVEXTERN_OUTPUT_NAME "__Internal")
ENDIF()
CONFIGURE_FILE(${CMAKE_CURRENT_SOURCE_DIR}/cmake/CvInvokeEntryPoints.cs.in ${CMAKE_CURRENT_SOURCE_DIR}/Emgu.CV/PInvoke/${CVINVOKE_ENTRYPOINTS_FILE_SUBFOLDER}CvInvokeEntryPoints.cs)

# ----------------------------------------------------------------------------
#  Build the managed dlls of Emgu CV
# ----------------------------------------------------------------------------
IF(NOT (ANDROID OR IOS)) 
ADD_SUBDIRECTORY(Emgu.Util)
ADD_SUBDIRECTORY(Emgu.CV)
ADD_SUBDIRECTORY(Emgu.CV.UI)
ADD_SUBDIRECTORY(Emgu.CV.ML)
ADD_SUBDIRECTORY(Emgu.CV.Example)
ADD_SUBDIRECTORY(Emgu.UI)
ADD_SUBDIRECTORY(Emgu.CV.Test)
ADD_SUBDIRECTORY(Emgu.CV.Cuda)
ADD_SUBDIRECTORY(Emgu.CV.OCR)
ADD_SUBDIRECTORY(Emgu.CV.Stitching)
ADD_SUBDIRECTORY(Emgu.CV.VideoStab)
#ADD_SUBDIRECTORY(Emgu.CV.OpenCL)
ADD_SUBDIRECTORY(Emgu.CV.Nonfree)
ADD_SUBDIRECTORY(Emgu.CV.Superres)
ADD_SUBDIRECTORY(Emgu.CV.Shape)
ADD_SUBDIRECTORY(Emgu.CV.Softcascade)
IF(NOT NETFX_CORE)
  ADD_SUBDIRECTORY(Emgu.CV.UI.GL)
ENDIF()
ENDIF()

IF (ANDROID)
  SET(CPACK_GENERATOR ZIP)
  SET(ANDROID_SOURCE_FOLDERS Emgu.Util Emgu.CV Emgu.CV.ML Emgu.CV.Cuda Emgu.CV.Stitching Emgu.CV.VideoStab Emgu.CV.Nonfree Emgu.CV.Superres Emgu.CV.Shape Emgu.CV.Softcascade)
  IF(EMGU_CV_WITH_TESSERACT)
    LIST(APPEND ANDROID_SOURCE_FOLDERS Emgu.CV.OCR)
  ENDIF()
  FOREACH(ANDROID_SOURCE_FOLDER ${ANDROID_SOURCE_FOLDERS})
  INSTALL(
  DIRECTORY
  "${CMAKE_CURRENT_SOURCE_DIR}/${ANDROID_SOURCE_FOLDER}"
  DESTINATION .
  COMPONENT main
  FILES_MATCHING 
  PATTERN "*.cs"
  PATTERN "*.Android.csproj"
  PATTERN "*.txt"
  PATTERN "*.xml"
  PATTERN "*.resx"
  PATTERN "CMakeLists.txt" EXCLUDE
  PATTERN ".git" EXCLUDE
  )
  ENDFOREACH()
  
  INSTALL(
  DIRECTORY
  "${CMAKE_CURRENT_SOURCE_DIR}/Emgu.CV.Example/Android"
  DESTINATION Emgu.CV.Example
  COMPONENT main
  FILES_MATCHING 
  PATTERN "*.cs"
  PATTERN "*.csproj"
  PATTERN "*.axml"
  PATTERN "*.xml"
  PATTERN "*.txt"
  PATTERN "*.png"
  PATTERN "*.dll"
  PATTERN ".git" EXCLUDE
  )
  INSTALL(
    FILES
    "Emgu.CV.Example/PlanarSubdivision/DrawSubdivision.cs"
    DESTINATION "Emgu.CV.Example/PlanarSubdivision"
    COMPONENT main
    )
  INSTALL(
    FILES
    "Emgu.CV.Example/FaceDetection/DetectFace.cs"
	"Emgu.CV.Example/FaceDetection/lena.jpg"
    DESTINATION "Emgu.CV.Example/FaceDetection"
    COMPONENT main
    )
  INSTALL(
    FILES
    "${OPENCV_SUBFOLDER}/data/haarcascades/haarcascade_eye.xml"
    "${OPENCV_SUBFOLDER}/data/haarcascades/haarcascade_eye.xml"
	"${OPENCV_SUBFOLDER}/data/haarcascades/haarcascade_frontalface_default.xml"
    DESTINATION "opencv/data/haarcascades"
    COMPONENT main
    )
  INSTALL(
    FILES
    "Emgu.CV.Example/SURFFeature/DrawMatches.cs"
	"Emgu.CV.Example/SURFFeature/box.png"
	"Emgu.CV.Example/SURFFeature/box_in_scene.png"
    DESTINATION "Emgu.CV.Example/SURFFeature"
    COMPONENT main
    )
  INSTALL(
    FILES
    "Emgu.CV.Example/PedestrianDetection/FindPedestrian.cs"
	"Emgu.CV.Example/PedestrianDetection/pedestrian.png"
    DESTINATION "Emgu.CV.Example/PedestrianDetection"
    COMPONENT main
    )
  INSTALL(
    FILES
    "Emgu.CV.Example/LicensePlateRecognition/LicensePlateDetector.cs"
	"Emgu.CV.Example/LicensePlateRecognition/license-plate.jpg"
    DESTINATION "Emgu.CV.Example/LicensePlateRecognition"
    COMPONENT main
    )
  INSTALL(
    DIRECTORY "Emgu.CV.Extern/tesseract/libtesseract/tesseract-ocr/tessdata"
	DESTINATION "Emgu.CV.Extern/tesseract/libtesseract/tesseract-ocr"
    COMPONENT main
    FILES_MATCHING 
    PATTERN "*eng.*"
    )
  INSTALL(
    FILES
    "Emgu.CV.Example/TrafficSignRecognition/StopSignDetector.cs"
	"Emgu.CV.Example/TrafficSignRecognition/stop-sign-model.png"
	"Emgu.CV.Example/TrafficSignRecognition/stop-sign.jpg"
    DESTINATION "Emgu.CV.Example/TrafficSignRecognition"
    COMPONENT main
    )
  INSTALL(
    FILES
    "Solution/Android/Emgu.CV.Android.sln"
    DESTINATION "Solution/Android"
    COMPONENT main
    )
  INSTALL(
    FILES
    "CommonAssemblyInfo.cs" 
	"Emgu.CV.snk"
	"Emgu.CV.License.txt"
    DESTINATION .
    COMPONENT main
    )
ELSEIF (WIN32)
  IF(NETFX_CORE)
    SET(CPACK_GENERATOR ZIP)
  ELSE()
    SET(CPACK_GENERATOR NSIS;ZIP)
  ENDIF()
  
  SET(CPACK_ARCHIVE_COMPONENT_INSTALL ON) #enable components install for zip
  IF(CPACK_GENERATOR MATCHES "ZIP")
    SET(CPACK_COMPONENTS_ALL_IN_ONE_PACKAGE 1)
  ENDIF()
  SET(CPACK_NSIS_MODIFY_PATH OFF)
  SET(CPACK_NSIS_INSTALL_ROOT "C:\\\\Emgu")
  
  IF(NOT NETFX_CORE)
    ADD_SUBDIRECTORY(Emgu.CV.DebuggerVisualizers)
  ENDIF()
  
  # ----------------------------------------------------------------------------
  #  Bundle MSCVRT into deployment package
  # ----------------------------------------------------------------------------
  IF(MSVC)
    IF(MSVC12)
	  SET(MSVC_REDIST_FOLDER "${CMAKE_CURRENT_SOURCE_DIR}/miscellaneous/msvc12_redist")
    ELSEIF(MSVC11)
	  SET(MSVC_REDIST_FOLDER "${CMAKE_CURRENT_SOURCE_DIR}/miscellaneous/msvc11_redist")
    ELSEIF(MSVC10)
      SET(MSVC_REDIST_FOLDER "${CMAKE_CURRENT_SOURCE_DIR}/miscellaneous/msvc10_redist")
    ELSEIF(MSVC90)
      SET(MSVC_REDIST_FOLDER "${CMAKE_CURRENT_SOURCE_DIR}/miscellaneous/msvc9_redist")
    ENDIF()

    SET(VCRT_REDIST_NAME "vcredist_x86.exe")
    IF(MSVC_REDIST_FOLDER AND (DEFINED UNIVERSAL_BUILD OR (NOT DEFINED TARGET_ARCH_64)))    
	  MESSAGE(STATUS "Including ${MSVC_REDIST_FOLDER}/${VCRT_REDIST_NAME} in package")
      INSTALL(
        FILES
        "${MSVC_REDIST_FOLDER}/${VCRT_REDIST_NAME}"
        DESTINATION .
        COMPONENT emgucv_binary 
        )
      SET(CPACK_NSIS_EXTRA_INSTALL_COMMANDS "${CPACK_NSIS_EXTRA_INSTALL_COMMANDS}\nExecWait '$INSTDIR\\\\${VCRT_REDIST_NAME} /passive'")
    ENDIF()
	
    IF(TARGET_ARCH_64)
      SET(VCRT_REDIST_NAME "vcredist_x64.exe")
   
      IF(MSVC_REDIST_FOLDER)
        MESSAGE(STATUS "Including ${MSVC_REDIST_FOLDER}/${VCRT_REDIST_NAME} in package")
        INSTALL(
        FILES
        "${MSVC_REDIST_FOLDER}/${VCRT_REDIST_NAME}"
        DESTINATION .
        COMPONENT emgucv_binary 
        )
        SET(CPACK_NSIS_EXTRA_INSTALL_COMMANDS 
		"${CPACK_NSIS_EXTRA_INSTALL_COMMANDS}\n!include 'x64.nsh'\n${If} ${RunningX64}\nExecWait '$INSTDIR\\\\${VCRT_REDIST_NAME} /passive'\n${EndIf}")
	  ELSE()
	    MESSAGE(FATAL_ERROR "Unable to find MSVC reistributable. Please verify that the following file exist: ${MSVC_REDIST_FOLDER}/${VCRT_REDIST_NAME}")
      ENDIF()
	ENDIF()
  ENDIF()

  IF(DEFINED UNIVERSAL_BUILD)
    MESSAGE(STATUS "Including the following files in package: ${UNMANAGED_DLL_32BIT}")
	INSTALL(
	   FILES
	   ${UNMANAGED_DLL_32BIT}
	   DESTINATION bin/x86
	   COMPONENT emgucv_binary
	   )
  ENDIF()
  
  IF(NOT NETFX_CORE)
    # ----------------------------------------------------------------------------
    #  Install Debugger Visualizer
    # ----------------------------------------------------------------------------
    SET(DEBUGER_VISUALIZER_FILES 
      "Emgu.Util"
      "Emgu.CV"
      "Emgu.CV.UI")
    SET(DEBUGGER_VISUALIZER_UNMANAGED_FILES
      #    "Emgu.CV.DebuggerVisualizers"
      ${OPENCV_LIBRARY_OUTPUT_NAMES}
      ${CVEXTERN_DEPENDENCY_DLL_NAMES})
  
    IF (${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER} MATCHES "^$")
      SET(DEBUGGER_VISUALIZER_UNMANAGED_FILES_SUBFOLDER "")
    ELSE()
      #MESSAGE(STATUS "UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER: ${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER}")
      #remove leading slashes
      STRING(REGEX REPLACE "^/" "" DEBUGGER_VISUALIZER_UNMANAGED_FILES_SUBFOLDER ${UNMANAGED_LIBRARY_OUTPUT_SUBFOLDER})
      #add tailing slashes
      SET(DEBUGGER_VISUALIZER_UNMANAGED_FILES_SUBFOLDER "${DEBUGGER_VISUALIZER_UNMANAGED_FILES_SUBFOLDER}\\")
      #replace single slashes with quard slashes
      STRING(REGEX REPLACE "\\\\" "\\\\\\\\" DEBUGGER_VISUALIZER_UNMANAGED_FILES_SUBFOLDER ${DEBUGGER_VISUALIZER_UNMANAGED_FILES_SUBFOLDER})
	  #MESSAGE(STATUS "DEBUGGER_VISUALIZER_UNMANAGED_FILES_SUBFOLDER: ${DEBUGGER_VISUALIZER_UNMANAGED_FILES_SUBFOLDER}")
    ENDIF()
	
    #FOREACH(UNMANAGE_DLL_NAME ${CVEXTERN_DEPENDENCY_DLL_NAMES})
    #  LIST(APPEND DEBUGGER_VISUALIZER_FILES "${DEBUGGER_VISUALIZER_UNMANAGED_FILES_SUBFOLDER}${UNMANAGE_DLL_NAME}")
    #ENDFOREACH()
    #MESSAGE(STATUS "DEBUGGER VISUALIZER FILES: ${DEBUGER_VISUALIZER_FILES}")
  
    MACRO(INSTALL_DEBUGER_VISUALIZER vs_version vs_description hklm_key)
      SET(INSTALL_DEBUGER_VISUALIZER_COMMAND "installDebuggerVisualizer4${vs_version}:
ClearErrors
ReadRegStr \\\$R1 HKLM '${hklm_key}' 'InstallDir'
IfErrors installDebuggerVisualizer4${vs_version}Done
MessageBox MB_YESNO 'Install Debugger Visualizer for ${vs_description}?' IDNO installDebuggerVisualizer4${vs_version}Done")
  FOREACH (file ${DEBUGER_VISUALIZER_FILES})
    SET(INSTALL_DEBUGER_VISUALIZER_COMMAND
      "${INSTALL_DEBUGER_VISUALIZER_COMMAND}
CopyFiles /SILENT '\\\$INSTDIR\\\\bin\\\\${file}.dll' '\\\$R1..\\\\Packages\\\\Debugger\\\\Visualizers\\\\'")
  ENDFOREACH()
  
    SET(INSTALL_DEBUGER_VISUALIZER_COMMAND
      "${INSTALL_DEBUGER_VISUALIZER_COMMAND}
CreateDirectory '\\\$R1..\\\\Packages\\\\Debugger\\\\Visualizers\\\\${DEBUGGER_VISUALIZER_UNMANAGED_FILES_SUBFOLDER}'")
  FOREACH (file ${DEBUGGER_VISUALIZER_UNMANAGED_FILES})
    SET(INSTALL_DEBUGER_VISUALIZER_COMMAND
      "${INSTALL_DEBUGER_VISUALIZER_COMMAND}
CopyFiles /SILENT '\\\$INSTDIR\\\\bin\\\\${DEBUGGER_VISUALIZER_UNMANAGED_FILES_SUBFOLDER}${file}.dll' '\\\$R1..\\\\Packages\\\\Debugger\\\\Visualizers\\\\${DEBUGGER_VISUALIZER_UNMANAGED_FILES_SUBFOLDER}'")
  ENDFOREACH()

  IF(DEFINED UNIVERSAL_BUILD)
    SET(INSTALL_DEBUGER_VISUALIZER_COMMAND
      "${INSTALL_DEBUGER_VISUALIZER_COMMAND}
CreateDirectory '\\\$R1..\\\\Packages\\\\Debugger\\\\Visualizers\\\\x86\\\\'")
  FOREACH (file ${UNMANAGED_DLL_32BIT})
    GET_FILENAME_COMPONENT(UNMANAGED_FILE_NAME_TO_DEPLOY ${file} NAME)
    MESSAGE(STATUS "Adding ${UNMANAGED_FILE_NAME_TO_DEPLOY} to debuger visualizer")
    SET(INSTALL_DEBUGER_VISUALIZER_COMMAND
      "${INSTALL_DEBUGER_VISUALIZER_COMMAND}
CopyFiles /SILENT '\\\$INSTDIR\\\\bin\\\\x86\\\\${UNMANAGED_FILE_NAME_TO_DEPLOY}' '\\\$R1..\\\\Packages\\\\Debugger\\\\Visualizers\\\\x86\\\\'")
  ENDFOREACH()
  ENDIF()

  SET(INSTALL_DEBUGER_VISUALIZER_COMMAND
    "${INSTALL_DEBUGER_VISUALIZER_COMMAND}
CopyFiles /SILENT '\\\$INSTDIR\\\\bin\\\\Emgu.CV.DebuggerVisualizers.${vs_version}.dll' '\\\$R1..\\\\Packages\\\\Debugger\\\\Visualizers\\\\'
installDebuggerVisualizer4${vs_version}Done:")

  ENDMACRO()

  MACRO(UNINSTALL_DEBUGER_VISUALIZER vs_version hklm_key)
    SET(UNINSTALL_DEBUGER_VISUALIZER_COMMAND "uninstallDebuggerVisualizer4${vs_version}:
ClearErrors
ReadRegStr \\\$R1 HKLM '${hklm_key}' 'InstallDir'
IfErrors uninstallDebuggerVisualizer4${vs_version}Done")
  FOREACH (file ${DEBUGER_VISUALIZER_FILES})
    SET(UNINSTALL_DEBUGER_VISUALIZER_COMMAND
      "${UNINSTALL_DEBUGER_VISUALIZER_COMMAND}
Delete '\\\$R1..\\\\Packages\\\\Debugger\\\\Visualizers\\\\${file}.dll'")
  ENDFOREACH()
  FOREACH (file ${DEBUGGER_VISUALIZER_UNMANAGED_FILES})
    SET(UNINSTALL_DEBUGER_VISUALIZER_COMMAND
      "${UNINSTALL_DEBUGER_VISUALIZER_COMMAND}
Delete '\\\$R1..\\\\Packages\\\\Debugger\\\\Visualizers\\\\${DEBUGGER_VISUALIZER_UNMANAGED_FILES_SUBFOLDER}${file}.dll'")
  ENDFOREACH()
  IF(DEFINED UNIVERSAL_BUILD)
  FOREACH (file ${UNMANAGED_DLL_32BIT})
    GET_FILENAME_COMPONENT(UNMANAGED_FILE_NAME_TO_DEPLOY ${file} NAME)
    SET(UNINSTALL_DEBUGER_VISUALIZER_COMMAND
      "${UNINSTALL_DEBUGER_VISUALIZER_COMMAND}
Delete '\\\$R1..\\\\Packages\\\\Debugger\\\\Visualizers\\\\x86\\\\${UNMANAGED_FILE_NAME_TO_DEPLOY}'")
  ENDFOREACH()
  ENDIF()

  SET(UNINSTALL_DEBUGER_VISUALIZER_COMMAND
    "${UNINSTALL_DEBUGER_VISUALIZER_COMMAND}
Delete '\\\$R1..\\\\Packages\\\\Debugger\\\\Visualizers\\\\Emgu.CV.DebuggerVisualizers.${vs_version}.dll'
uninstallDebuggerVisualizer4${vs_version}Done:
")
  ENDMACRO()

  INSTALL_DEBUGER_VISUALIZER("VS2008" "Visual Studio 2008" "SOFTWARE\\\\\\\\Microsoft\\\\\\\\VisualStudio\\\\\\\\9.0")
  SET(CPACK_NSIS_EXTRA_INSTALL_COMMANDS "${CPACK_NSIS_EXTRA_INSTALL_COMMANDS}\n${INSTALL_DEBUGER_VISUALIZER_COMMAND}")

  INSTALL_DEBUGER_VISUALIZER("VS2010" "Visual Studio 2010" "SOFTWARE\\\\\\\\Microsoft\\\\\\\\VisualStudio\\\\\\\\10.0")
  SET(CPACK_NSIS_EXTRA_INSTALL_COMMANDS "${CPACK_NSIS_EXTRA_INSTALL_COMMANDS}\n${INSTALL_DEBUGER_VISUALIZER_COMMAND}")

  INSTALL_DEBUGER_VISUALIZER("VS2012" "Visual Studio 2012" "SOFTWARE\\\\\\\\Microsoft\\\\\\\\VisualStudio\\\\\\\\11.0")
  SET(CPACK_NSIS_EXTRA_INSTALL_COMMANDS "${CPACK_NSIS_EXTRA_INSTALL_COMMANDS}\n${INSTALL_DEBUGER_VISUALIZER_COMMAND}")
  
  INSTALL_DEBUGER_VISUALIZER("VS2013" "Visual Studio 2013" "SOFTWARE\\\\\\\\Microsoft\\\\\\\\VisualStudio\\\\\\\\12.0")
  SET(CPACK_NSIS_EXTRA_INSTALL_COMMANDS "${CPACK_NSIS_EXTRA_INSTALL_COMMANDS}\n${INSTALL_DEBUGER_VISUALIZER_COMMAND}")

  UNINSTALL_DEBUGER_VISUALIZER("VS2008" "SOFTWARE\\\\\\\\Microsoft\\\\\\\\VisualStudio\\\\\\\\9.0")
  SET(CPACK_NSIS_EXTRA_UNINSTALL_COMMANDS "${CPACK_NSIS_EXTRA_UNINSTALL_COMMANDS}\n${UNINSTALL_DEBUGER_VISUALIZER_COMMAND}")

  UNINSTALL_DEBUGER_VISUALIZER("VS2010" "SOFTWARE\\\\\\\\Microsoft\\\\\\\\VisualStudio\\\\\\\\10.0")
  SET(CPACK_NSIS_EXTRA_UNINSTALL_COMMANDS "${CPACK_NSIS_EXTRA_UNINSTALL_COMMANDS}\n${UNINSTALL_DEBUGER_VISUALIZER_COMMAND}")

  UNINSTALL_DEBUGER_VISUALIZER("VS2012" "SOFTWARE\\\\\\\\Microsoft\\\\\\\\VisualStudio\\\\\\\\11.0")
  SET(CPACK_NSIS_EXTRA_UNINSTALL_COMMANDS "${CPACK_NSIS_EXTRA_UNINSTALL_COMMANDS}\n${UNINSTALL_DEBUGER_VISUALIZER_COMMAND}")

  UNINSTALL_DEBUGER_VISUALIZER("VS2013" "SOFTWARE\\\\\\\\Microsoft\\\\\\\\VisualStudio\\\\\\\\12.0")
  SET(CPACK_NSIS_EXTRA_UNINSTALL_COMMANDS "${CPACK_NSIS_EXTRA_UNINSTALL_COMMANDS}\n${UNINSTALL_DEBUGER_VISUALIZER_COMMAND}")

ENDIF()

# ----------------------------------------------------------------------------
#  Build the documents of Emgu CV
# ----------------------------------------------------------------------------
SET(EMGU_CV_DOCUMENTATION_BUILD OFF CACHE BOOL "Build Emgu CV Documentation")
IF(EMGU_CV_DOCUMENTATION_BUILD)
  ADD_SUBDIRECTORY(miscellaneous)
ENDIF()

# ----------------------------------------------------------------------------
#  Build the extra components of Emgu CV
# ----------------------------------------------------------------------------
SET(EMGU_CV_EXTRA_BUILD OFF CACHE BOOL "Build Emgu CV Extra")
IF(EMGU_CV_EXTRA_BUILD)
  ADD_SUBDIRECTORY(Emgu.RPC)
ENDIF()

# ----------------------------------------------------------------------------
#  The source files
# ----------------------------------------------------------------------------
INSTALL(
  FILES
  "${CMAKE_CURRENT_SOURCE_DIR}/Emgu.CV.snk"
  "${CMAKE_CURRENT_SOURCE_DIR}/CommonAssemblyInfo.cs"
  "${CMAKE_CURRENT_SOURCE_DIR}/Emgu.CV.License.txt"
  DESTINATION .
  COMPONENT emgucv_source
  )

INSTALL(
  DIRECTORY
  ${CMAKE_CURRENT_SOURCE_DIR}/lib
  DESTINATION .
  COMPONENT emgucv_source
  FILES_MATCHING 
  PATTERN "*.dll"
  PATTERN "*.txt"
  PATTERN "*.xml"
  PATTERN ".git" EXCLUDE
  PATTERN "obj" EXCLUDE
  PATTERN "CMake*" EXCLUDE
  PATTERN "Release" EXCLUDE
  PATTERN "${PROJECT_NAME}.dir" EXCLUDE
  )

IF(NETFX_CORE)    
  INSTALL(
    DIRECTORY
    ${CMAKE_CURRENT_SOURCE_DIR}/Solution
    DESTINATION .
    COMPONENT emgucv_source
    FILES_MATCHING 
    PATTERN "Emgu.CV.Windows.Store.sln"
    PATTERN ".git" EXCLUDE
    PATTERN "bin" EXCLUDE
  )  
  INSTALL(
    DIRECTORY
    ${CMAKE_CURRENT_SOURCE_DIR}/Solution
    DESTINATION .
    COMPONENT emgucv_example_source
    FILES_MATCHING 
    PATTERN "Emgu.CV.Windows.Store.Example.sln"
    PATTERN ".git" EXCLUDE
    PATTERN "bin" EXCLUDE
    )
	
  # ----------------------------------------------------------------------------
  #  The example files
  # ----------------------------------------------------------------------------
  INSTALL(
    DIRECTORY
    ${CMAKE_CURRENT_SOURCE_DIR}/Emgu.CV.Example
    DESTINATION .
    COMPONENT emgucv_example_source
    FILES_MATCHING 
    PATTERN "WindowsStoreCameraApp/*.cs"
	PATTERN "WindowsStoreCameraApp/Properties/*.cs"
	PATTERN "WindowsStoreCameraApp/Common/*.xaml"
    PATTERN "WindowsStoreCameraApp/*.csproj"
    PATTERN "WindowsStoreCameraApp/*.appxmanifest"
    PATTERN "WindowsStoreCameraApp/*.xaml"
    PATTERN "WindowsStoreCameraApp/Assets/*.png"  
    PATTERN ".git" EXCLUDE
    PATTERN "obj" EXCLUDE
    PATTERN "CMake*" EXCLUDE
    PATTERN "Release" EXCLUDE
    PATTERN "Debug" EXCLUDE
    PATTERN "*.dir" EXCLUDE
  )
ELSE()
  INSTALL(
    DIRECTORY
    ${CMAKE_CURRENT_SOURCE_DIR}/Solution
    DESTINATION .
    COMPONENT emgucv_source
    FILES_MATCHING 
    PATTERN "Emgu.CV.sln"
    PATTERN "Emgu.CV.DebuggerVisualizers.sln"
    PATTERN ".git" EXCLUDE
    PATTERN "bin" EXCLUDE
  )
  INSTALL(
    DIRECTORY
    ${CMAKE_CURRENT_SOURCE_DIR}/Emgu.CV.WPF
    DESTINATION .
    COMPONENT emgucv_source
    FILES_MATCHING 
    PATTERN "*.cs"
    PATTERN ".git" EXCLUDE
  )
  INSTALL(
    DIRECTORY
    ${CMAKE_CURRENT_SOURCE_DIR}/Solution
    DESTINATION .
    COMPONENT emgucv_example_source
    FILES_MATCHING 
    PATTERN "Emgu.CV.Example.sln"
    PATTERN ".git" EXCLUDE
    PATTERN "bin" EXCLUDE
    )
	
  # ----------------------------------------------------------------------------
  #  The example files
  # ----------------------------------------------------------------------------
  INSTALL(
    DIRECTORY
    ${CMAKE_CURRENT_SOURCE_DIR}/Emgu.CV.Example
    DESTINATION .
    COMPONENT emgucv_example_source
    FILES_MATCHING 
    PATTERN "*.cs"
    PATTERN "*.csproj"
    PATTERN "*.resx"
    PATTERN "*.h"
    PATTERN "*.cpp"
    PATTERN "*.resX"
    PATTERN "*.ico"
    PATTERN "*.rc"
    PATTERN "CPlusPlus/*.vcproj"
    PATTERN "CPlusPlus/*.vcxproj"
    PATTERN "*.vb"
    PATTERN "*.vbproj"
    PATTERN "*.aspx" 
    PATTERN "*.dll"
    PATTERN "*.txt"
    PATTERN "*.xml"
    PATTERN "*.xaml"
    PATTERN "*.jpg"
    PATTERN "*.png"  
    PATTERN "*.settings"
    PATTERN "*.config"
    PATTERN "tessdata/*"
    PATTERN ".git" EXCLUDE
    PATTERN "obj" EXCLUDE
    PATTERN "CMake*" EXCLUDE
    PATTERN "Release" EXCLUDE
    PATTERN "Debug" EXCLUDE
    PATTERN "*.dir" EXCLUDE
    PATTERN "Android" EXCLUDE
    PATTERN "iOS" EXCLUDE
    PATTERN "WindowsStore" EXCLUDE
  )
ENDIF()

INSTALL(
  FILES
  "${OPENCV_SUBFOLDER}/data/haarcascades/haarcascade_frontalface_default.xml"
  "${OPENCV_SUBFOLDER}/data/haarcascades/haarcascade_eye.xml"
  DESTINATION opencv/data/haarcascades
  COMPONENT emgucv_example_source
  )

# ----------------------------------------------------------------------------
#  Build the package
# ----------------------------------------------------------------------------

set(CPACK_COMPONENTS_ALL 
  main #opencv components 
  emgucv_binary 
  emgucv_source
  emgucv_example_source
  )

set(CPACK_PACKAGE_EXECUTABLES "" "") #http://public.kitware.com/Bug/view.php?id=7828

SET(CPACK_NSIS_CONTACT "support@emgu.com")

# Define MUI_TEMP that will be used for uninstalling menulinks
SET(CPACK_NSIS_EXTRA_UNINSTALL_COMMANDS "${CPACK_NSIS_EXTRA_UNINSTALL_COMMANDS}\n !insertmacro MUI_STARTMENU_GETFOLDER Application $MUI_TEMP")

# ----------------------------------------------------------------------------
#  Add menu link for documentations
# ----------------------------------------------------------------------------
SET(CPACK_NSIS_EXTRA_INSTALL_COMMANDS "${CPACK_NSIS_EXTRA_INSTALL_COMMANDS}\nCreateDirectory \\\"$SMPROGRAMS\\\\$STARTMENU_FOLDER\\\\Documentation\\\" ")

SET(CPACK_NSIS_EXTRA_INSTALL_COMMANDS "${CPACK_NSIS_EXTRA_INSTALL_COMMANDS}\nCreateShortCut \\\"$SMPROGRAMS\\\\$STARTMENU_FOLDER\\\\Documentation\\\\Open CV Documentation.lnk\\\"  \\\"$INSTDIR\\\\doc\\\\opencv2refman.pdf\\\" ")	
SET(CPACK_NSIS_EXTRA_UNINSTALL_COMMANDS "${CPACK_NSIS_EXTRA_UNINSTALL_COMMANDS}\n Delete \\\"$SMPROGRAMS\\\\$MUI_TEMP\\\\Documentation\\\\Open CV Documentation.lnk\\\" ")

IF(EMGU_CV_DOCUMENTATION_BUILD)
  LIST(APPEND CPACK_COMPONENTS_ALL emgucv_document)
  set(CPACK_COMPONENT_EMGUCV_DOCUMENT_DISPLAY_NAME "Emgu CV Documentation")
  set(CPACK_COMPONENT_EMGUCV_DOCUMENT_DEPENDS emgucv_binary)
  SET(CPACK_NSIS_EXTRA_INSTALL_COMMANDS "${CPACK_NSIS_EXTRA_INSTALL_COMMANDS}\nCreateShortCut \\\"$SMPROGRAMS\\\\$STARTMENU_FOLDER\\\\Documentation\\\\Emgu CV Documentation.lnk\\\"  \\\"$INSTDIR\\\\Emgu.CV.Documentation.chm\\\" ")	
  SET(CPACK_NSIS_EXTRA_UNINSTALL_COMMANDS "${CPACK_NSIS_EXTRA_UNINSTALL_COMMANDS}\n Delete \\\"$SMPROGRAMS\\\\$MUI_TEMP\\\\Documentation\\\\Emgu CV Documentation.lnk\\\" ")
ENDIF()

SET(CPACK_NSIS_EXTRA_UNINSTALL_COMMANDS "${CPACK_NSIS_EXTRA_UNINSTALL_COMMANDS}\nRMDir  \\\"$SMPROGRAMS\\\\$MUI_TEMP\\\\Documentation\\\" ")

# ----------------------------------------------------------------------------
#  Add menu link for web sites
# ----------------------------------------------------------------------------
LIST(APPEND CPACK_NSIS_MENU_LINKS "http://www.emgu.com" "Emgu CV wiki")
LIST(APPEND CPACK_NSIS_MENU_LINKS "http://www.emgu.com/forum" "Emgu CV Discussion Forums")
LIST(APPEND CPACK_NSIS_MENU_LINKS "http://www.emgu.com/bugs/buglist.cgi?query_format=advanced&short_desc_type=allwordssubstr&short_desc=&product=Emgu+CV&long_desc_type=substring&long_desc=&bug_file_loc_type=allwordssubstr&bug_file_loc=&deadlinefrom=&deadlineto=&bug_status=UNCONFIRMED&bug_status=NEW&bug_status=ASSIGNED&bug_status=REOPENED&bug_status=RESOLVED&bug_status=VERIFIED&bug_status=CLOSED&emailassigned_to1=1&emailtype1=substring&email1=&emailassigned_to2=1&emailreporter2=1&emailcc2=1&emailtype2=substring&email2=&bugidtype=include&bug_id=&chfieldfrom=&chfieldto=Now&chfieldvalue=&cmdtype=doit&order=Reuse+same+sort+as+last+time&field0-0-0=noop&type0-0-0=noop&value0-0-0=" "Emgu CV Bug Tracking")

# ----------------------------------------------------------------------------
#  Add menu link for Visual Studio solutions 
# ----------------------------------------------------------------------------
SET(CPACK_NSIS_EXTRA_INSTALL_COMMANDS "${CPACK_NSIS_EXTRA_INSTALL_COMMANDS}\nCreateDirectory \\\"$SMPROGRAMS\\\\$STARTMENU_FOLDER\\\\Visual Studio Solution\\\" ")

SET(CPACK_NSIS_EXTRA_INSTALL_COMMANDS "${CPACK_NSIS_EXTRA_INSTALL_COMMANDS}\nCreateShortCut \\\"$SMPROGRAMS\\\\$STARTMENU_FOLDER\\\\Visual Studio Solution\\\\Visual Studio 2008 Examples.lnk\\\"  \\\"$INSTDIR\\\\Solution\\\\VS2008\\\\Emgu.CV.Example.sln\\\" ")	
SET(CPACK_NSIS_EXTRA_INSTALL_COMMANDS "${CPACK_NSIS_EXTRA_INSTALL_COMMANDS}\nCreateShortCut \\\"$SMPROGRAMS\\\\$STARTMENU_FOLDER\\\\Visual Studio Solution\\\\Visual Studio 2010 2012 2013 Examples.lnk\\\"  \\\"$INSTDIR\\\\Solution\\\\VS2010_2012_2013\\\\Emgu.CV.Example.sln\\\" ")	

SET(CPACK_NSIS_EXTRA_UNINSTALL_COMMANDS "${CPACK_NSIS_EXTRA_UNINSTALL_COMMANDS}\n Delete \\\"$SMPROGRAMS\\\\$MUI_TEMP\\\\Visual Studio Solution\\\\Visual Studio 2008 Examples.lnk\\\" ")
SET(CPACK_NSIS_EXTRA_UNINSTALL_COMMANDS "${CPACK_NSIS_EXTRA_UNINSTALL_COMMANDS}\n Delete \\\"$SMPROGRAMS\\\\$MUI_TEMP\\\\Visual Studio Solution\\\\Visual Studio 2010 2012 2013 Examples.lnk\\\" ")

SET(CPACK_NSIS_EXTRA_UNINSTALL_COMMANDS "${CPACK_NSIS_EXTRA_UNINSTALL_COMMANDS}\nRMDir  \\\"$SMPROGRAMS\\\\$MUI_TEMP\\\\Visual Studio Solution\\\" ")

# ----------------------------------------------------------------------------
#  Add menu link for Licenses 
# ----------------------------------------------------------------------------
SET(CPACK_NSIS_EXTRA_INSTALL_COMMANDS "${CPACK_NSIS_EXTRA_INSTALL_COMMANDS}\nCreateDirectory \\\"$SMPROGRAMS\\\\$STARTMENU_FOLDER\\\\License\\\" ")

SET(CPACK_NSIS_EXTRA_INSTALL_COMMANDS "${CPACK_NSIS_EXTRA_INSTALL_COMMANDS}\nCreateShortCut \\\"$SMPROGRAMS\\\\$STARTMENU_FOLDER\\\\License\\\\Emgu CV License.lnk\\\"  \\\"$INSTDIR\\\\Emgu.CV.License.txt\\\" ")	
SET(CPACK_NSIS_EXTRA_INSTALL_COMMANDS "${CPACK_NSIS_EXTRA_INSTALL_COMMANDS}\nCreateShortCut \\\"$SMPROGRAMS\\\\$STARTMENU_FOLDER\\\\License\\\\Open CV License.lnk\\\"  \\\"$INSTDIR\\\\doc\\\\license.txt\\\" ")
SET(CPACK_NSIS_EXTRA_INSTALL_COMMANDS "${CPACK_NSIS_EXTRA_INSTALL_COMMANDS}\nCreateShortCut \\\"$SMPROGRAMS\\\\$STARTMENU_FOLDER\\\\License\\\\ZedGraph License.lnk\\\"  \\\"$INSTDIR\\\\bin\\\\ZedGraph.license.txt\\\" ")	
SET(CPACK_NSIS_EXTRA_INSTALL_COMMANDS "${CPACK_NSIS_EXTRA_INSTALL_COMMANDS}\nCreateShortCut \\\"$SMPROGRAMS\\\\$STARTMENU_FOLDER\\\\License\\\\Tesseract OCR License.lnk\\\"  \\\"$INSTDIR\\\\lib\\\\3rdParty\\\\tesseract-ocr.license.txt\\\" ")	
SET(CPACK_NSIS_EXTRA_INSTALL_COMMANDS "${CPACK_NSIS_EXTRA_INSTALL_COMMANDS}\nCreateShortCut \\\"$SMPROGRAMS\\\\$STARTMENU_FOLDER\\\\License\\\\CvBlob License.lnk\\\"  \\\"$INSTDIR\\\\lib\\\\3rdParty\\\\cvblob.license.txt\\\" ")	

SET(CPACK_NSIS_EXTRA_UNINSTALL_COMMANDS "${CPACK_NSIS_EXTRA_UNINSTALL_COMMANDS}\n Delete \\\"$SMPROGRAMS\\\\$MUI_TEMP\\\\License\\\\Emgu CV License.lnk\\\" ")
SET(CPACK_NSIS_EXTRA_UNINSTALL_COMMANDS "${CPACK_NSIS_EXTRA_UNINSTALL_COMMANDS}\n Delete \\\"$SMPROGRAMS\\\\$MUI_TEMP\\\\License\\\\Open CV License.lnk\\\" ")
SET(CPACK_NSIS_EXTRA_UNINSTALL_COMMANDS "${CPACK_NSIS_EXTRA_UNINSTALL_COMMANDS}\n Delete \\\"$SMPROGRAMS\\\\$MUI_TEMP\\\\License\\\\ZedGraph License.lnk\\\" ")
SET(CPACK_NSIS_EXTRA_UNINSTALL_COMMANDS "${CPACK_NSIS_EXTRA_UNINSTALL_COMMANDS}\n Delete \\\"$SMPROGRAMS\\\\$MUI_TEMP\\\\License\\\\Tesseract OCR License.lnk\\\" ")
SET(CPACK_NSIS_EXTRA_UNINSTALL_COMMANDS "${CPACK_NSIS_EXTRA_UNINSTALL_COMMANDS}\n Delete \\\"$SMPROGRAMS\\\\$MUI_TEMP\\\\License\\\\CvBlob License.lnk\\\" ")
SET(CPACK_NSIS_EXTRA_UNINSTALL_COMMANDS "${CPACK_NSIS_EXTRA_UNINSTALL_COMMANDS}\nRMDir  \\\"$SMPROGRAMS\\\\$MUI_TEMP\\\\License\\\" ")

IF(EMGU_CV_EXAMPLE_BUILD AND NOT NETFX_CORE)
  LIST(APPEND CPACK_COMPONENTS_ALL emgucv_example_binary)
  set(CPACK_COMPONENT_EMGUCV_EXAMPLE_BINARY_DISPLAY_NAME "Emgu CV Examples (Binary)")
  set(CPACK_COMPONENT_EMGUCV_EXAMPLE_BINARY_DEPENDS emgucv_binary)
ENDIF()

set(CPACK_COMPONENT_MAIN_DISPLAY_NAME "OpenCV Library ${EMGUCV_ARCH}")
set(CPACK_COMPONENT_MAIN_REQUIRED ON)
set(CPACK_COMPONENT_EMGUCV_BINARY_DISPLAY_NAME "Emgu CV (${EMGUCV_ARCH} Binary)")
set(CPACK_COMPONENT_EMGUCV_BINARY_REQUIRED ON)
set(CPACK_COMPONENT_EMGUCV_BINARY_DEPENDS main)
set(CPACK_COMPONENT_EMGUCV_SOURCE_DISPLAY_NAME "Emgu CV (Source)")
set(CPACK_COMPONENT_EMGUCV_SOURCE_DEPENDS main)
set(CPACK_COMPONENT_EMGUCV_EXAMPLE_SOURCE_DISPLAY_NAME "Emgu CV Examples (Source)")
set(CPACK_COMPONENT_EMGUCV_EXAMPLE_SOURCE_DEPENDS emgucv_source)

ELSEIF (APPLE)
  SET(CPACK_GENERATOR ZIP)
  SET(CPACK_BUNDLE_NAME ${CPACK_PACKAGE_NAME})

# ----------------------------------------------------------------------------
#  The source files
# ----------------------------------------------------------------------------
INSTALL(
  FILES
  "${CMAKE_CURRENT_SOURCE_DIR}/Emgu.CV.snk"
  "${CMAKE_CURRENT_SOURCE_DIR}/CommonAssemblyInfo.cs"
  "${CMAKE_CURRENT_SOURCE_DIR}/Emgu.CV.License.txt"
  DESTINATION .
  COMPONENT emgucv_source
  )

INSTALL(
  DIRECTORY
  ${CMAKE_CURRENT_SOURCE_DIR}/lib
  DESTINATION .
  COMPONENT emgucv_source
  FILES_MATCHING 
  PATTERN "*.dll"
  PATTERN "*.txt"
  PATTERN "*.xml"
  PATTERN ".git" EXCLUDE
  PATTERN "obj" EXCLUDE
  PATTERN "CMake*" EXCLUDE
  PATTERN "Release" EXCLUDE
  PATTERN "${PROJECT_NAME}.dir" EXCLUDE
  )

INSTALL(
  DIRECTORY
  ${CMAKE_CURRENT_SOURCE_DIR}/Solution
  DESTINATION .
  COMPONENT emgucv_source
  FILES_MATCHING 
  PATTERN "Emgu.CV.sln"
  PATTERN "Emgu.CV.DebuggerVisualizers.sln"
  PATTERN ".git" EXCLUDE
  PATTERN "bin" EXCLUDE
  )
  
INSTALL(
  DIRECTORY
  ${CMAKE_CURRENT_SOURCE_DIR}/Emgu.CV.WPF
  DESTINATION .
  COMPONENT emgucv_source
  FILES_MATCHING 
  PATTERN "*.cs"
  PATTERN ".git" EXCLUDE
  )
  
# ----------------------------------------------------------------------------
#  The example files
# ----------------------------------------------------------------------------
INSTALL(
  DIRECTORY
  ${CMAKE_CURRENT_SOURCE_DIR}/Emgu.CV.Example
  DESTINATION .
  COMPONENT emgucv_example_source
  FILES_MATCHING 
  PATTERN "*.cs"
  PATTERN "*.csproj"
  PATTERN "*.resx"
  PATTERN "*.h"
  PATTERN "*.cpp"
  PATTERN "*.resX"
  PATTERN "*.ico"
  PATTERN "*.rc"
  PATTERN "CPlusPlus/*.vcproj"
  PATTERN "CPlusPlus/*.vcxproj"
  PATTERN "*.vb"
  PATTERN "*.vbproj"
  PATTERN "*.aspx" 
  PATTERN "*.dll"
  PATTERN "*.txt"
  PATTERN "*.xml"
  PATTERN "*.xaml"
  PATTERN "*.jpg"
  PATTERN "*.png"  
  PATTERN "*.settings"
  PATTERN "*.config"
  PATTERN "tessdata/*"
  PATTERN ".git" EXCLUDE
  PATTERN "obj" EXCLUDE
  PATTERN "CMake*" EXCLUDE
  PATTERN "Release" EXCLUDE
  PATTERN "Debug" EXCLUDE
  PATTERN "*.dir" EXCLUDE
  PATTERN "Android" EXCLUDE
  PATTERN "iOS" EXCLUDE
  )
INSTALL(
  DIRECTORY
  ${CMAKE_CURRENT_SOURCE_DIR}/Solution
  DESTINATION .
  COMPONENT emgucv_example_source
  FILES_MATCHING 
  PATTERN "Emgu.CV.Example.sln"
  PATTERN ".git" EXCLUDE
  PATTERN "bin" EXCLUDE
  )

INSTALL(
  FILES
  "${OPENCV_SUBFOLDER}/data/haarcascades/haarcascade_frontalface_default.xml"
  "${OPENCV_SUBFOLDER}/data/haarcascades/haarcascade_eye.xml"
  DESTINATION opencv/data/haarcascades
  COMPONENT emgucv_example_source
  )


# ----------------------------------------------------------------------------
#  Generate required common assembly file for Emgu CV project
# ----------------------------------------------------------------------------
  SET(CPACK_BUNDLE_PLIST ${CMAKE_CURRENT_SOURCE_DIR}/CMake/osx/Info.plist)
  FILE(WRITE ${CPACK_BUNDLE_PLIST}
  "<?xml version=\"1.0\" encoding=\"UTF-8\"?>
<!DOCTYPE plist PUBLIC \"-//Apple//DTD PLIST 1.0//EN\" \"http://www.apple.com/DTDs/PropertyList-1.0.dtd\">
<plist version=\"1.0\">
<dict>
   <key>CFBundleIdentifier</key>
   <string>com.emgu.emgucv</string>
   <key>CFBundleName</key>
   <string>${CPACK_BUNDLE_NAME}</string>
   <key>CFBundleVersion</key>
   <string>1</string>
   <key>LSMinimumSystemVersion</key>
   <string>10.6</string>
</dict>
</plist>")

ELSE() #LINUX
  SET(MONO_GACUTIL_EXEC "gacutil")
  SET(LINUX_CONTROL_FILE_FOLDER "${CMAKE_CURRENT_SOURCE_DIR}/cmake/linux")
  SET(EMGUCV_MANAGED_DLLS "Emgu.Util" "Emgu.CV" "Emgu.CV.ML" "Emgu.CV.Cuda" "Emgu.CV.UI" "Emgu.CV.Stitching" "Emgu.CV.VideoStab" "Emgu.CV.Superres" "Emgu.CV.Nonfree" "Emgu.CV.Shape" "Emgu.CV.Softcascade" "ZedGraph")
  IF(EMGU_CV_WITH_TESSERACT)
    LIST(APPEND EMGUCV_MANAGED_DLLS "Emgu.CV.OCR")
  ENDIF()
  SET(CLIFILELIST_CONTENT "")
  FOREACH(MANAGED_DLL ${EMGUCV_MANAGED_DLLS})
    SET(CLIFILELIST_CONTENT "${CLIFILELIST_CONTENT}/usr/${CPACK_PACKAGE_CLI_FOLDER}/${MANAGED_DLL}.dll\n")
  ENDFOREACH()
  FILE(WRITE ${LINUX_CONTROL_FILE_FOLDER}/clifilelist.txt ${CLIFILELIST_CONTENT})
  INSTALL(
    FILES
    ${LINUX_CONTROL_FILE_FOLDER}/clifilelist.txt
    DESTINATION ${CPACK_PACKAGE_CLI_FOLDER}
    COMPONENT emgucv_binary)
  
  #creating the pkg-config file
  SET(PACKAGE_CFG_REF_CONTENT "")
  FOREACH(MANAGED_DLL ${EMGUCV_MANAGED_DLLS})
    SET(PACKAGE_CFG_REF_CONTENT "${PACKAGE_CFG_REF_CONTENT} -r:\${libdir}/${MANAGED_DLL}.dll")
  ENDFOREACH()
  
  FILE(WRITE ${LINUX_CONTROL_FILE_FOLDER}/${CPACK_PACKAGE_NAME}.pc
    "prefix=/usr\nexec_prefix=\${prefix}\nlibdir=\${exec_prefix}/${CPACK_PACKAGE_CLI_FOLDER}\napidir=\${exec_prefix}/${CPACK_PACKAGE_CLI_FOLDER}\n\nName: ${CPACK_PACKAGE_NAME}\nDescription: ${CPACK_PACKAGE_DESCRIPTION_SUMMARY}\nVersion: ${CPACK_PACKAGE_VERSION}\nLibs: ${PACKAGE_CFG_REF_CONTENT}\n")
  INSTALL(
    FILES
    ${LINUX_CONTROL_FILE_FOLDER}/${CPACK_PACKAGE_NAME}.pc
    DESTINATION /usr/lib/pkgconfig
    COMPONENT emgucv_binary)
  
  SET(REMOVE_EMGU_FROM_GAC_SCRIPT "")
  FOREACH(MANAGED_DLL ${EMGUCV_MANAGED_DLLS})
    SET(REMOVE_EMGU_FROM_GAC_SCRIPT "${REMOVE_EMGU_FROM_GAC_SCRIPT} ${MANAGED_DLL}")
  ENDFOREACH()
  SET(REMOVE_EMGU_FROM_GAC_SCRIPT "for dll in${REMOVE_EMGU_FROM_GAC_SCRIPT}; do ${MONO_GACUTIL_EXEC} -u $dll; done")

  SET(CPACK_STRIP_FILES "")
  
  IF(NOT CPACK_GENERATOR)
    MESSAGE(STATUS "Checking for CPACK build type...")
    FIND_PROGRAM(DPKG_CMD dpkg)
    IF (DPKG_CMD)
      # ----------------------------------------------------------------------------
      #  Building .deb package for Debian 
      # ----------------------------------------------------------------------------
      MESSAGE(STATUS "FOUND dpkg: ${DPKG_CMD}")
      LIST(APPEND CPACK_GENERATOR DEB)
    ENDIF()
    
    FIND_PROGRAM(RPMBUILD_CMD rpmbuild)
    IF(RPMBUILD_CMD)
      # ----------------------------------------------------------------------------
      #  Building .rpm package  
      # ----------------------------------------------------------------------------
      MESSAGE(STATUS "FOUND rpmbuild: ${RPMBUILD_CMD}")
      LIST(APPEND CPACK_GENERATOR RPM)
    ENDIF()
  ENDIF()
   
  SET(PREINST_SCRIPT "#!/bin/sh\nmkdir -p /usr/lib/pkgconfig /usr/${CPACK_PACKAGE_CLI_FOLDER} /usr/share/opencv/haarcascades /usr/share/opencv/lbpcascades /usr/share/opencv/doc /usr/share/opencv/doc/vidsurv /usr/share/opencv/doc/papers /usr/include/opencv2/flann /usr/include/opencv2/core /usr/include/opencv2/contrib /usr/include/opencv2/features2d /usr/include/opencv2/ffmpeg /usr/include/opencv2/gpu /usr/include/opencv2/haartraining /usr/include/opencv2/highgui /usr/include/opencv2/imgproc /usr/include/opencv2/legacy /usr/include/opencv2/ml /usr/include/opencv2/objdetect /usr/include/opencv2/calib3d /usr/include/opencv2/video /usr/include/opencv\n")
  LIST(FIND CPACK_GENERATOR "DEB" TEMP)
  IF(NOT (${TEMP} EQUAL -1))
    SET(DEBIAN_CONTROL_FILE_FOLDER "${CMAKE_CURRENT_SOURCE_DIR}/cmake/debian")
    FILE(WRITE ${DEBIAN_CONTROL_FILE_FOLDER}/preinst ${PREINST_SCRIPT}) 
    FILE(WRITE ${DEBIAN_CONTROL_FILE_FOLDER}/postinst
      "#!/bin/sh\nset -e\nif [ \"$1\" = \"configure\" ]; then\n  ldconfig\n  ${MONO_GACUTIL_EXEC} -il /usr/${CPACK_PACKAGE_CLI_FOLDER}/clifilelist.txt -package ${CPACK_PACKAGE_NAME}\nfi\n")
    FILE(WRITE ${DEBIAN_CONTROL_FILE_FOLDER}/prerm
      "#!/bin/sh\nset -e\nif [ \"$1\" = \"remove\" ] || [ \"$1\" = \"upgrade\" ] && [ -x /usr/share/cli-common/gac-package-remove ]; then\n${REMOVE_EMGU_FROM_GAC_SCRIPT}\nfi\n")
    
    SET(DEBIAN_PACKAGE_PRIORITY "optional")
    SET(CPACK_DEBIAN_PACKAGE_CONTROL_EXTRA "${DEBIAN_CONTROL_FILE_FOLDER}/preinst;${DEBIAN_CONTROL_FILE_FOLDER}/postinst;${DEBIAN_CONTROL_FILE_FOLDER}/prerm;${DEBIAN_CONTROL_FILE_FOLDER}/postrm")
    SET(CPACK_DEBIAN_PACKAGE_DEPENDS "mono-runtime (>> 2.4.2), mono-2.0-gac, libtiff5, libgeotiff2, gtk2-engines, libgstreamer0.10-0, libavcodec54, libswscale2, libavformat54, libopenexr6, libjasper1, libdc1394-22, libv4l-0, libqt4-opengl")
    IF(EMGU_CV_WITH_TESSERACT)
      SET(CPACK_DEBIAN_PACKAGE_DEPENDS "${CPACK_DEBIAN_PACKAGE_DEPENDS}, libtesseract3")
    ENDIF()
    IF(WITH_TBB)
      SET(CPACK_DEBIAN_PACKAGE_DEPENDS "${CPACK_DEBIAN_PACKAGE_DEPENDS}, libtbb2")
    ENDIF()
  ENDIF()
  
  LIST(FIND CPACK_GENERATOR "RPM" TEMP)
  IF(NOT (${TEMP} EQUAL -1))  
    SET(CPACK_RPM_PACKAGE_LICENSE "GPL v3")
    SET(CPACK_RPM_PACKAGE_REQUIRES "mono-core >= 2.6, mono-wcf, jasper, libgeotiff, gtk2, OpenEXR, libv4l, libdc1394, qt")
    IF (WITH_TBB)
      SET(CPACK_RPM_PACKAGE_REQUIRES ", tbb")
    ENDIF()    
    SET(RPM_CONTROL_FILE_FOLDER "${CMAKE_CURRENT_SOURCE_DIR}/cmake/rpm")
    FILE(WRITE ${RPM_CONTROL_FILE_FOLDER}/preinst ${PREINST_SCRIPT})
    FILE(WRITE ${RPM_CONTROL_FILE_FOLDER}/postinst
      "ldconfig\n${MONO_GACUTIL_EXEC} -il /usr/${CPACK_PACKAGE_CLI_FOLDER}/clifilelist.txt -package ${CPACK_PACKAGE_NAME}\n")
    FILE(WRITE ${RPM_CONTROL_FILE_FOLDER}/prerm
      "${REMOVE_EMGU_FROM_GAC_SCRIPT}\n")
    SET(CPACK_RPM_PRE_INSTALL_SCRIPT_FILE "${RPM_CONTROL_FILE_FOLDER}/preinst")
    SET(CPACK_RPM_POST_INSTALL_SCRIPT_FILE "${RPM_CONTROL_FILE_FOLDER}/postinst")
    SET(CPACK_RPM_PRE_UNINSTALL_SCRIPT_FILE "${RPM_CONTROL_FILE_FOLDER}/prerm")
    SET(CPACK_RPM_POST_UNINSTALL_SCRIPT_FILE "${RPM_CONTROL_FILE_FOLDER}/postrm")
  ENDIF()	
ENDIF()  

MESSAGE(STATUS "CPACK_GENERATOR: ${CPACK_GENERATOR}")
INCLUDE (CPack) 
