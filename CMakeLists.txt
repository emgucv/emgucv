# --------------------------------------------------------
# Root CMake file for Emgu CV
#
# From the off-tree build directory, invoke:
#   $ cmake <PATH_TO_EMGUCV_ROOT>
#
# --------------------------------------------------------

PROJECT(emgucv)
CMAKE_MINIMUM_REQUIRED(VERSION 2.8.1)

SET(CMAKE_MODULE_PATH ${CMAKE_CURRENT_SOURCE_DIR}/cmake/modules)
#SET CSC 3.5 as the prefered compiler for EMGU CV
SET(CSC_PREFERRED_VERSION "3.5")
FIND_PACKAGE(CSharp REQUIRED)
INCLUDE (CSharpMacros)
INCLUDE (CheckTargetArch)

SET(LIBRARY_OUTPUT_PATH ${CMAKE_SOURCE_DIR}/bin)
#MESSAGE(STATUS "Changing library output path to '${LIBRARY_OUTPUT_PATH}'")

SET(LIBRARY_3RDPARTY_PATH ${CMAKE_CURRENT_SOURCE_DIR}/lib/3rdParty)

# ----------------------------------------------------------------------------
#  Autodetect if we are in a SVN repository
# ----------------------------------------------------------------------------
FIND_PROGRAM(SVNVERSION_PATH svnversion)
MARK_AS_ADVANCED(force SVNVERSION_PATH)
IF(SVNVERSION_PATH)
  MESSAGE(STATUS "Extracting Emgu CV svn version, please wait...")
  EXECUTE_PROCESS(COMMAND ${SVNVERSION_PATH} -n ${CMAKE_CURRENT_SOURCE_DIR} OUTPUT_VARIABLE SVNVERSION_RESULT)
  IF(SVNVERSION_RESULT MATCHES "exported")
    # This is NOT a svn repository:
    SET(EMGUCV_SVNVERSION "0")
  ELSE()
    STRING(REGEX REPLACE "^([0123456789]+).*" "\\1" EMGUCV_SVNVERSION "${SVNVERSION_RESULT}")
    IF(EMGUCV_SVNVERSION MATCHES "^$" OR EMGUCV_SVNVERSION MATCHES "^-")
      # This is NOT a svn repository:
      SET(EMGUCV_SVNVERSION "0")
    ENDIF()
  ENDIF()
  MESSAGE(STATUS "Emgu CV SVNVERSION: ${EMGUCV_SVNVERSION}")
ELSE()
  # We don't have svnversion:
  SET(EMGUCV_SVNVERSION "0")
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
  SET(CV_ICC_ADDITIONAL_FLAGS_RELEASE "/O3 /QaxSSE4.1,SSE3 /Qparallel /Qvec-report1 /Qpar-report1 /Qipo")
  STRING(REGEX REPLACE "/O2" ${CV_ICC_ADDITIONAL_FLAGS_RELEASE} CMAKE_C_FLAGS_RELEASE ${CMAKE_C_FLAGS_RELEASE})
  STRING(REGEX REPLACE "/O2" ${CV_ICC_ADDITIONAL_FLAGS_RELEASE} CMAKE_CXX_FLAGS_RELEASE ${CMAKE_CXX_FLAGS_RELEASE})
  SET(CMAKE_EXE_LINKER_FLAGS "${CMAKE_EXE_LINKER_FLAGS} /NODEFAULTLIB:\"libcmt\"")
  SET(CMAKE_SHARED_LINKER_FLAGS "${CMAKE_SHARED_LINKER_FLAGS} /NODEFAULTLIB:\"libcmt\"")
ENDIF()

# ----------------------------------------------------------------------------
#  Setup Package information
# ----------------------------------------------------------------------------
SET(PACKAGE "${PROJECT_NAME}")
SET(CPACK_PACKAGE_CONTACT "Canming Huang <support@emgu.com>")
SET(PACKAGE_BUGREPORT "${CPACK_PACKAGE_CONTACT}")
SET(PACKAGE_NAME "${PROJECT_NAME}")

SET(CPACK_PACKAGE_NAME "${PACKAGE_NAME}")
SET(CPACK_PACKAGE_DESCRIPTION_SUMMARY "Emgu CV is a cross platform .Net wrapper to the OpenCV image processing library.")

IF (WIN32)
  SET(CPACK_PACKAGE_NAME "${CPACK_PACKAGE_NAME}-windows") 
ENDIF()

IF(TARGET_ARCH_64)
  SET(CPACK_PACKAGE_NAME "${CPACK_PACKAGE_NAME}-x64") 
ELSE()
  SET(CPACK_PACKAGE_NAME "${CPACK_PACKAGE_NAME}-x86")
ENDIF()

IF(WITH_CUDA) 
  SET(CPACK_PACKAGE_NAME "${CPACK_PACKAGE_NAME}-gpu")
ENDIF()
IF(WITH_TBB)
  SET(CPACK_PACKAGE_NAME "${CPACK_PACKAGE_NAME}-tbb")
ENDIF()
IF(USE_IPP)
  SET(CPACK_PACKAGE_NAME "${CPACK_PACKAGE_NAME}-ipp")
ENDIF()
IF(CV_ICC)
  SET(CPACK_PACKAGE_NAME "${CPACK_PACKAGE_NAME}-icc")
ENDIF()

SET(CPACK_PACKAGE_VENDOR "Emgu")
SET(CPACK_PACKAGE_VERSION_MAJOR "2")
SET(CPACK_PACKAGE_VERSION_MINOR "3")
SET(CPACK_PACKAGE_VERSION_PATCH "0")
SET(CPACK_PACKAGE_VERSION "${CPACK_PACKAGE_VERSION_MAJOR}.${CPACK_PACKAGE_VERSION_MINOR}.${CPACK_PACKAGE_VERSION_PATCH}.${EMGUCV_SVNVERSION}")
SET(CPACK_PACKAGE_FILE_NAME "lib${CPACK_PACKAGE_NAME}-${CPACK_PACKAGE_VERSION}")

SET(CPACK_RESOURCE_FILE_LICENSE "${CMAKE_CURRENT_SOURCE_DIR}/Emgu.CV.License.txt")

IF (WIN32)
  SET(CPACK_PACKAGE_CLI_FOLDER bin)
ELSE (WIN32)
  SET(CPACK_PACKAGE_CLI_FOLDER lib/cli/${PACKAGE_NAME}-${CPACK_PACKAGE_VERSION_MAJOR}.${CPACK_PACKAGE_VERSION_MINOR})
ENDIF()

# ----------------------------------------------------------------------------
#  Build opencv and Emgu.CV.Extern
# ----------------------------------------------------------------------------

#IF(MSVC)
SET( CMAKE_RUNTIME_OUTPUT_DIRECTORY "${LIBRARY_OUTPUT_PATH}" )
SET( CMAKE_RUNTIME_OUTPUT_DIRECTORY_RELEASE "${CMAKE_RUNTIME_OUTPUT_DIRECTORY}")
SET( CMAKE_RUNTIME_OUTPUT_DIRECTORY_DEBUG "${CMAKE_RUNTIME_OUTPUT_DIRECTORY}")
#ENDIF()

SET(CMAKE_OLD_SOURCE_DIR ${CMAKE_SOURCE_DIR})
SET(CMAKE_SOURCE_DIR ${CMAKE_CURRENT_SOURCE_DIR}/opencv)
ADD_SUBDIRECTORY(opencv)
SET(CMAKE_SOURCE_DIR ${CMAKE_OLD_SOURCE_DIR})

IF (TARGET_ARCH_64)
  SET(EMGU_ENABLE_SSE OFF CACHE BOOL "ENABLE SSE for Emgu CV")
ELSE()
  SET(EMGU_ENABLE_SSE ON CACHE BOOL "ENABLE SSE for Emgu CV")
ENDIF()
ADD_SUBDIRECTORY(Emgu.CV.Extern)

SET(EMGU_CV_BUILD ON CACHE BOOL "BUILD Emgu CV")
SET(EMGU_CV_EXAMPLE_BUILD ON CACHE BOOL "Build Emgu CV Example")

# ----------------------------------------------------------------------------
#  Generate required common assembly file for Emgu CV project
# ----------------------------------------------------------------------------
FILE(WRITE ${CMAKE_CURRENT_SOURCE_DIR}/CommonAssemblyInfo.cs
  "using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
[assembly: AssemblyVersion(\"${CPACK_PACKAGE_VERSION}\")]
[assembly: AssemblyFileVersion(\"${CPACK_PACKAGE_VERSION}\")]
[assembly: AssemblyConfiguration(\"\")]
[assembly: AssemblyCompany(\"${CPACK_PACKAGE_VENDOR}\")]
[assembly: AssemblyProduct(\"Emgu.CV\")]
[assembly: AssemblyCopyright(\"Copyright ${CPACK_PACKAGE_VENDOR} 2011\")]
[assembly: AssemblyTrademark(\"\")]
[assembly: AssemblyCulture(\"\")]
")

# ----------------------------------------------------------------------------
#  Generate readme file for Emgu CV project
# ----------------------------------------------------------------------------
FILE(WRITE ${CMAKE_CURRENT_SOURCE_DIR}/Emgu.CV/README.txt
  "Version ${CPACK_PACKAGE_VERSION}
==================================================================

For online documentation please visit
http://www.emgu.com/wiki/files/${CPACK_PACKAGE_VERSION_MAJOR}.${CPACK_PACKAGE_VERSION_MINOR}.${CPACK_PACKAGE_VERSION_PATCH}.0/Index.html

For tutorials and helps please visit the Emgu CV wiki 
http://www.emgu.com/wiki/

For questions and discussions please visit the Emgu CV forum
http://www.emgu.com/forum/

To report a bug, please visit
http://www.emgu.com/bugs/
")

# ----------------------------------------------------------------------------
#  Generate CvInvokeEntryPoints.cs file for Emgu CV project
# ----------------------------------------------------------------------------
GET_TARGET_PROPERTY(OPENCV_CORE_OUTPUT_NAME opencv_core OUTPUT_NAME)
GET_TARGET_PROPERTY(OPENCV_IMGPROC_OUTPUT_NAME opencv_imgproc OUTPUT_NAME)
GET_TARGET_PROPERTY(OPENCV_HIGHGUI_OUTPUT_NAME opencv_highgui OUTPUT_NAME)
GET_TARGET_PROPERTY(OPENCV_ML_OUTPUT_NAME opencv_ml OUTPUT_NAME)
GET_TARGET_PROPERTY(OPENCV_CALIB3D_OUTPUT_NAME opencv_calib3d OUTPUT_NAME)
GET_TARGET_PROPERTY(OPENCV_FEATURES2D_OUTPUT_NAME opencv_features2d OUTPUT_NAME)
GET_TARGET_PROPERTY(OPENCV_OBJDETECT_OUTPUT_NAME opencv_objdetect OUTPUT_NAME)
GET_TARGET_PROPERTY(OPENCV_LEGACY_OUTPUT_NAME opencv_legacy OUTPUT_NAME)
GET_TARGET_PROPERTY(OPENCV_CALIB3D_OUTPUT_NAME opencv_calib3d OUTPUT_NAME)
GET_TARGET_PROPERTY(OPENCV_VIDEO_OUTPUT_NAME opencv_video OUTPUT_NAME)
GET_TARGET_PROPERTY(OPENCV_FLANN_OUTPUT_NAME opencv_flann OUTPUT_NAME)
GET_TARGET_PROPERTY(OPENCV_GPU_OUTPUT_NAME opencv_gpu OUTPUT_NAME)
GET_TARGET_PROPERTY(CVEXTERN_OUTPUT_NAME cvextern OUTPUT_NAME)

IF(TARGET opencv_ffmpeg)
  GET_TARGET_PROPERTY(OPENCV_FFMPEG_OUTPUT_NAME opencv_ffmpeg OUTPUT_NAME)
ELSE()
  SET(OPENCV_FFMPEG_OUTPUT_NAME "")
ENDIF()

SET(OPENCV_LIBRARY_OUTPUT_NAMES 
  ${OPENCV_CORE_OUTPUT_NAME}
  ${OPENCV_IMGPROC_OUTPUT_NAME}
  ${OPENCV_HIGHGUI_OUTPUT_NAME}
  ${OPENCV_ML_OUTPUT_NAME}
  ${OPENCV_CALIB3D_OUTPUT_NAME}
  ${OPENCV_FEATURES2D_OUTPUT_NAME}
  ${OPENCV_OBJDETECT_OUTPUT_NAME}
  ${OPENCV_LEGACY_OUTPUT_NAME}
  ${OPENCV_CALIB3D_OUTPUT_NAME}
  ${OPENCV_VIDEO_OUTPUT_NAME}
  ${OPENCV_FFMPEG_OUTPUT_NAME}
  ${OPENCV_FLANN_OUTPUT_NAME}
  ${OPENCV_GPU_OUTPUT_NAME}
  ${CVEXTERN_OUTPUT_NAME})

IF(TARGET opencv_ffmpeg)
ELSE()
  SET(OPENCV_FFMPEG_OUTPUT_NAME "unknown")
ENDIF()

IF(TARGET_ARCH_64)
  SET(UNMANAGED_CODE_BITNESS "64")
ELSE()
  SET(UNMANAGED_CODE_BITNESS "32")
ENDIF()

FILE(WRITE ${CMAKE_CURRENT_SOURCE_DIR}/Emgu.CV/PInvoke/CvInvokeEntryPoints.cs
  "//This file is automatically generate by CMAKE. DO NOT MODIFY.
using System;

namespace Emgu.CV
{
   public partial class CvInvoke
   {
      /// <summary>
      /// The file name of the core library
      /// </summary>
      public const string OPENCV_CORE_LIBRARY = \"${OPENCV_CORE_OUTPUT_NAME}\";
      /// <summary>
      /// The file name of the imgproc library
      /// </summary>
      public const string OPENCV_IMGPROC_LIBRARY = \"${OPENCV_IMGPROC_OUTPUT_NAME}\";
      /// <summary>
      /// The file name of the highgui library
      /// </summary>
      public const string OPENCV_HIGHGUI_LIBRARY = \"${OPENCV_HIGHGUI_OUTPUT_NAME}\";
      /// <summary>
      /// The file name of the feature2d library
      /// </summary>
      public const string OPENCV_FEATURES2D_LIBRARY = \"${OPENCV_FEATURES2D_OUTPUT_NAME}\";
      /// <summary>
      /// The file name of the objdetect library
      /// </summary>
      public const string OPENCV_OBJDETECT_LIBRARY = \"${OPENCV_OBJDETECT_OUTPUT_NAME}\";
      /// <summary>
      /// The file name of the calib3d library
      /// </summary>
      public const string OPENCV_CALIB3D_LIBRARY = \"${OPENCV_CALIB3D_OUTPUT_NAME}\";
      /// <summary>
      /// The file name of the legacy library
      /// </summary>
      public const string OPENCV_LEGACY_LIBRARY = \"${OPENCV_LEGACY_OUTPUT_NAME}\";
      /// <summary>
      /// The file name of the video library
      /// </summary>
      public const string OPENCV_VIDEO_LIBRARY = \"${OPENCV_VIDEO_OUTPUT_NAME}\";
      /// <summary>
      /// The file name of the gpu library
      /// </summary>
      public const string OPENCV_GPU_LIBRARY = \"${OPENCV_GPU_OUTPUT_NAME}\";
      /// <summary>
      /// The file name of the cvextern library
      /// </summary>
      public const string EXTERN_LIBRARY = \"${CVEXTERN_OUTPUT_NAME}\";
	  
      /// <summary>
      /// The file name of the opencv_ffmpeg library
      /// </summary>
      public const string OPENCV_FFMPEG_LIBRARY = \"${OPENCV_FFMPEG_OUTPUT_NAME}\";

      /// <summary>
      /// The bit-ness of the unmanaged code (either 32 or 64) 
      /// </summary>
      public const uint UnmanagedCodeBitness = ${UNMANAGED_CODE_BITNESS};
   }
}")

# ----------------------------------------------------------------------------
#  Build the managed dlls of Emgu CV
# ----------------------------------------------------------------------------
ADD_SUBDIRECTORY(Emgu.Util)
ADD_SUBDIRECTORY(Emgu.CV)
ADD_SUBDIRECTORY(Emgu.CV.UI)
ADD_SUBDIRECTORY(Emgu.CV.ML)
ADD_SUBDIRECTORY(Emgu.CV.Example)
ADD_SUBDIRECTORY(Emgu.UI)
ADD_SUBDIRECTORY(Emgu.CV.Test)
ADD_SUBDIRECTORY(Emgu.CV.GPU)
ADD_SUBDIRECTORY(Emgu.CV.GPU.Test)
ADD_SUBDIRECTORY(Emgu.CV.OCR)

IF (WIN32)
  IF(TARGET_ARCH_64)
    SET(CPACK_GENERATOR ZIP)
  ELSE()
    SET(CPACK_GENERATOR NSIS;ZIP)
  ENDIF()
  
  SET(CPACK_NSIS_MODIFY_PATH OFF)
  SET(CPACK_NSIS_INSTALL_ROOT "C:\\\\Emgu")
  ADD_SUBDIRECTORY(Emgu.CV.DebuggerVisualizers)
  
  # ----------------------------------------------------------------------------
  #  Bundle MSCVRT into deployment package
  # ----------------------------------------------------------------------------
  IF(MSVC)
  IF(MSVC100)
    SET(MSVC_REDIST_FOLDER "${CMAKE_CURRENT_SOURCE_DIR}/miscellaneous/msvc10_redist")
  ENDIF()
  IF(MSVC90)
    SET(MSVC_REDIST_FOLDER "${CMAKE_CURRENT_SOURCE_DIR}/miscellaneous/msvc9_redist")
  ENDIF()
  
  SET(VCRT_REDIST_NAME "vcredist_x86.exe")
  IF(TARGET_ARCH_64)
      SET(VCRT_REDIST_NAME "vcredist_x64.exe")
  ENDIF()
  
  IF(MSVC_REDIST_FOLDER)
  INSTALL(
    FILES
    "${MSVC_REDIST_FOLDER}/${VCRT_REDIST_NAME}"
    DESTINATION .
    COMPONENT emgucv_binary 
    )
  SET(CPACK_NSIS_EXTRA_INSTALL_COMMANDS "${CPACK_NSIS_EXTRA_INSTALL_COMMANDS}\nExecWait '$INSTDIR\\\\${VCRT_REDIST_NAME} /passive'")
  ENDIF()
  ENDIF()

  # ----------------------------------------------------------------------------
  #  Bundle intel c++ compiler xe 11 redistributable into deployment package
  # ----------------------------------------------------------------------------
  IF(WIN32 AND CV_ICC)
    SET(CV_ICC_REDIST_NAME "w_ccompxe_redist_ia32_2011.1.127.msi")
    IF(TARGET_ARCH_64)
      SET(CV_ICC_REDIST_NAME "w_ccompxe_redist_intel64_2011.1.127.msi")
    ENDIF()
	MESSAGE(STATUS "${CV_ICC_REDIST_NAME} will be included in deployment package")
	INSTALL(
	  FILES 
	  "${CMAKE_CURRENT_SOURCE_DIR}/miscellaneous/intel_ccompxe_redist/${CV_ICC_REDIST_NAME}"
	  DESTINATION .
	  COMPONENT emgucv_binary
	  )
	SET(CPACK_NSIS_EXTRA_INSTALL_COMMANDS "${CPACK_NSIS_EXTRA_INSTALL_COMMANDS}\nExecWait '$INSTDIR\\\\${CV_ICC_REDIST_NAME} /passive'")
  ENDIF()    
  # ----------------------------------------------------------------------------
  #  Install Debugger Visualizer
  # ----------------------------------------------------------------------------
  SET(DEBUGER_VISUALIZER_FILES 
    "Emgu.Util"
    "Emgu.CV"
    "Emgu.CV.UI"
    #    "Emgu.CV.DebuggerVisualizers"
    ${OPENCV_LIBRARY_OUTPUT_NAMES}
	${CVEXTERN_DEPENDENCY_DLL_NAMES})
  MESSAGE(STATUS "DEBUGGER VISUALIZER FILES: ${DEBUGER_VISUALIZER_FILES}")
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

SET(UNINSTALL_DEBUGER_VISUALIZER_COMMAND
  "${UNINSTALL_DEBUGER_VISUALIZER_COMMAND}
Delete '\\\$R1..\\\\Packages\\\\Debugger\\\\Visualizers\\\\Emgu.CV.DebuggerVisualizers.${vs_version}.dll'
uninstallDebuggerVisualizer4${vs_version}Done:
")
ENDMACRO()

#  INSTALL_DEBUGER_VISUALIZER("VS2005" "Visual Studio 2005" "SOFTWARE\\\\\\\\Microsoft\\\\\\\\VisualStudio\\\\\\\\8.0")
#  SET(CPACK_NSIS_EXTRA_INSTALL_COMMANDS "${CPACK_NSIS_EXTRA_INSTALL_COMMANDS}\n${INSTALL_DEBUGER_VISUALIZER_COMMAND}")

INSTALL_DEBUGER_VISUALIZER("VS2008" "Visual Studio 2008" "SOFTWARE\\\\\\\\Microsoft\\\\\\\\VisualStudio\\\\\\\\9.0")
SET(CPACK_NSIS_EXTRA_INSTALL_COMMANDS "${CPACK_NSIS_EXTRA_INSTALL_COMMANDS}\n${INSTALL_DEBUGER_VISUALIZER_COMMAND}")

INSTALL_DEBUGER_VISUALIZER("VS2010" "Visual Studio 2010" "SOFTWARE\\\\\\\\Microsoft\\\\\\\\VisualStudio\\\\\\\\10.0")
SET(CPACK_NSIS_EXTRA_INSTALL_COMMANDS "${CPACK_NSIS_EXTRA_INSTALL_COMMANDS}\n${INSTALL_DEBUGER_VISUALIZER_COMMAND}")

#  UNINSTALL_DEBUGER_VISUALIZER("VS2005" "SOFTWARE\\\\\\\\Microsoft\\\\\\\\VisualStudio\\\\\\\\8.0")
#  SET(CPACK_NSIS_EXTRA_UNINSTALL_COMMANDS "${CPACK_NSIS_EXTRA_UNINSTALL_COMMANDS}\n${UNINSTALL_DEBUGER_VISUALIZER_COMMAND}")

UNINSTALL_DEBUGER_VISUALIZER("VS2008" "SOFTWARE\\\\\\\\Microsoft\\\\\\\\VisualStudio\\\\\\\\9.0")
SET(CPACK_NSIS_EXTRA_UNINSTALL_COMMANDS "${CPACK_NSIS_EXTRA_UNINSTALL_COMMANDS}\n${UNINSTALL_DEBUGER_VISUALIZER_COMMAND}")

UNINSTALL_DEBUGER_VISUALIZER("VS2010" "SOFTWARE\\\\\\\\Microsoft\\\\\\\\VisualStudio\\\\\\\\10.0")
SET(CPACK_NSIS_EXTRA_UNINSTALL_COMMANDS "${CPACK_NSIS_EXTRA_UNINSTALL_COMMANDS}\n${UNINSTALL_DEBUGER_VISUALIZER_COMMAND}")


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
  PATTERN ".svn" EXCLUDE
  PATTERN "obj" EXCLUDE
  PATTERN "CMake*" EXCLUDE
  PATTERN "Release" EXCLUDE
  PATTERN "${PROJECT_NAME}.dir" EXCLUDE
  PATTERN "3rdParty/x86" EXCLUDE #exclude 32bit osg binary
  PATTERN "3rdParty/x86_64" EXCLUDE #exclude 64bit osg binary
  )

INSTALL(
  DIRECTORY
  ${CMAKE_CURRENT_SOURCE_DIR}/Solution
  DESTINATION .
  COMPONENT emgucv_source
  FILES_MATCHING 
  PATTERN "Emgu.CV.sln"
  PATTERN "Emgu.CV.DebuggerVisualizers.sln"
  PATTERN ".svn" EXCLUDE
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
  PATTERN ".svn" EXCLUDE
  PATTERN "obj" EXCLUDE
  PATTERN "CMake*" EXCLUDE
  PATTERN "Release" EXCLUDE
  PATTERN "Debug" EXCLUDE
  PATTERN "*.dir" EXCLUDE
  )
INSTALL(
  DIRECTORY
  ${CMAKE_CURRENT_SOURCE_DIR}/Solution
  DESTINATION .
  COMPONENT emgucv_example_source
  FILES_MATCHING 
  PATTERN "Emgu.CV.Example.sln"
  PATTERN ".svn" EXCLUDE
  PATTERN "bin" EXCLUDE
  )

INSTALL(
  DIRECTORY
  ${CMAKE_CURRENT_SOURCE_DIR}/opencv/data/haarcascades
  DESTINATION ./opencv/data
  COMPONENT emgucv_example_source
  FILES_MATCHING 
  PATTERN "*.xml"
  PATTERN ".svn" EXCLUDE
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

SET(CPACK_NSIS_EXTRA_INSTALL_COMMANDS "${CPACK_NSIS_EXTRA_INSTALL_COMMANDS}\nCreateShortCut \\\"$SMPROGRAMS\\\\$STARTMENU_FOLDER\\\\Documentation\\\\Open CV Documentation.lnk\\\"  \\\"$INSTDIR\\\\doc\\\\opencv.pdf\\\" ")	
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

SET(CPACK_NSIS_EXTRA_INSTALL_COMMANDS "${CPACK_NSIS_EXTRA_INSTALL_COMMANDS}\nCreateShortCut \\\"$SMPROGRAMS\\\\$STARTMENU_FOLDER\\\\Visual Studio Solution\\\\Visual Studio 2005 Examples.lnk\\\"  \\\"$INSTDIR\\\\Solution\\\\VS2005_MonoDevelop\\\\Emgu.CV.Example.sln\\\" ")	
SET(CPACK_NSIS_EXTRA_INSTALL_COMMANDS "${CPACK_NSIS_EXTRA_INSTALL_COMMANDS}\nCreateShortCut \\\"$SMPROGRAMS\\\\$STARTMENU_FOLDER\\\\Visual Studio Solution\\\\Visual Studio 2008 Examples.lnk\\\"  \\\"$INSTDIR\\\\Solution\\\\VS2008\\\\Emgu.CV.Example.sln\\\" ")	
SET(CPACK_NSIS_EXTRA_INSTALL_COMMANDS "${CPACK_NSIS_EXTRA_INSTALL_COMMANDS}\nCreateShortCut \\\"$SMPROGRAMS\\\\$STARTMENU_FOLDER\\\\Visual Studio Solution\\\\Visual Studio 2010 Examples.lnk\\\"  \\\"$INSTDIR\\\\Solution\\\\VS2010\\\\Emgu.CV.Example.sln\\\" ")	

SET(CPACK_NSIS_EXTRA_UNINSTALL_COMMANDS "${CPACK_NSIS_EXTRA_UNINSTALL_COMMANDS}\n Delete \\\"$SMPROGRAMS\\\\$MUI_TEMP\\\\Visual Studio Solution\\\\Visual Studio 2005 Examples.lnk\\\" ")
SET(CPACK_NSIS_EXTRA_UNINSTALL_COMMANDS "${CPACK_NSIS_EXTRA_UNINSTALL_COMMANDS}\n Delete \\\"$SMPROGRAMS\\\\$MUI_TEMP\\\\Visual Studio Solution\\\\Visual Studio 2008 Examples.lnk\\\" ")
SET(CPACK_NSIS_EXTRA_UNINSTALL_COMMANDS "${CPACK_NSIS_EXTRA_UNINSTALL_COMMANDS}\n Delete \\\"$SMPROGRAMS\\\\$MUI_TEMP\\\\Visual Studio Solution\\\\Visual Studio 2010 Examples.lnk\\\" ")

SET(CPACK_NSIS_EXTRA_UNINSTALL_COMMANDS "${CPACK_NSIS_EXTRA_UNINSTALL_COMMANDS}\nRMDir  \\\"$SMPROGRAMS\\\\$MUI_TEMP\\\\Visual Studio Solution\\\" ")

# ----------------------------------------------------------------------------
#  Add menu link for Licenses 
# ----------------------------------------------------------------------------
SET(CPACK_NSIS_EXTRA_INSTALL_COMMANDS "${CPACK_NSIS_EXTRA_INSTALL_COMMANDS}\nCreateDirectory \\\"$SMPROGRAMS\\\\$STARTMENU_FOLDER\\\\License\\\" ")

SET(CPACK_NSIS_EXTRA_INSTALL_COMMANDS "${CPACK_NSIS_EXTRA_INSTALL_COMMANDS}\nCreateShortCut \\\"$SMPROGRAMS\\\\$STARTMENU_FOLDER\\\\License\\\\Emgu CV License.lnk\\\"  \\\"$INSTDIR\\\\Emgu.CV.License.txt\\\" ")	
SET(CPACK_NSIS_EXTRA_INSTALL_COMMANDS "${CPACK_NSIS_EXTRA_INSTALL_COMMANDS}\nCreateShortCut \\\"$SMPROGRAMS\\\\$STARTMENU_FOLDER\\\\License\\\\Open CV License.lnk\\\"  \\\"$INSTDIR\\\\doc\\\\license.txt\\\" ")
SET(CPACK_NSIS_EXTRA_INSTALL_COMMANDS "${CPACK_NSIS_EXTRA_INSTALL_COMMANDS}\nCreateShortCut \\\"$SMPROGRAMS\\\\$STARTMENU_FOLDER\\\\License\\\\ZedGraph License.lnk\\\"  \\\"$INSTDIR\\\\bin\\\\ZedGraph.license.txt\\\" ")	

SET(CPACK_NSIS_EXTRA_UNINSTALL_COMMANDS "${CPACK_NSIS_EXTRA_UNINSTALL_COMMANDS}\n Delete \\\"$SMPROGRAMS\\\\$MUI_TEMP\\\\License\\\\Emgu CV License.lnk\\\" ")
SET(CPACK_NSIS_EXTRA_UNINSTALL_COMMANDS "${CPACK_NSIS_EXTRA_UNINSTALL_COMMANDS}\n Delete \\\"$SMPROGRAMS\\\\$MUI_TEMP\\\\License\\\\Open CV License.lnk\\\" ")
SET(CPACK_NSIS_EXTRA_UNINSTALL_COMMANDS "${CPACK_NSIS_EXTRA_UNINSTALL_COMMANDS}\n Delete \\\"$SMPROGRAMS\\\\$MUI_TEMP\\\\License\\\\ZedGraph License.lnk\\\" ")

SET(CPACK_NSIS_EXTRA_UNINSTALL_COMMANDS "${CPACK_NSIS_EXTRA_UNINSTALL_COMMANDS}\nRMDir  \\\"$SMPROGRAMS\\\\$MUI_TEMP\\\\License\\\" ")

IF(EMGU_CV_EXAMPLE_BUILD)
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

ELSE (WIN32)
  SET(MONO_GACUTIL_EXEC "gacutil")
  SET(LINUX_CONTROL_FILE_FOLDER "${CMAKE_CURRENT_SOURCE_DIR}/cmake/linux")
  
  FILE(WRITE ${LINUX_CONTROL_FILE_FOLDER}/clifilelist.txt
    "/usr/${CPACK_PACKAGE_CLI_FOLDER}/Emgu.Util.dll\n/usr/${CPACK_PACKAGE_CLI_FOLDER}/Emgu.CV.dll\n/usr/${CPACK_PACKAGE_CLI_FOLDER}/Emgu.CV.ML.dll\n/usr/${CPACK_PACKAGE_CLI_FOLDER}/Emgu.CV.GPU.dll\n/usr/${CPACK_PACKAGE_CLI_FOLDER}/Emgu.CV.UI.dll\n/usr/${CPACK_PACKAGE_CLI_FOLDER}/ZedGraph.dll")
  INSTALL(
    FILES
    ${LINUX_CONTROL_FILE_FOLDER}/clifilelist.txt
    DESTINATION ${CPACK_PACKAGE_CLI_FOLDER}
    COMPONENT emgucv_binary)
  
  #creating the pkg-config file
  FILE(WRITE ${LINUX_CONTROL_FILE_FOLDER}/${CPACK_PACKAGE_NAME}.pc
    "prefix=/usr\nexec_prefix=\${prefix}\nlibdir=\${exec_prefix}/${CPACK_PACKAGE_CLI_FOLDER}\napidir=\${exec_prefix}/${CPACK_PACKAGE_CLI_FOLDER}\n\nName: ${CPACK_PACKAGE_NAME}\nDescription: ${CPACK_PACKAGE_DESCRIPTION_SUMMARY}\nVersion: ${CPACK_PACKAGE_VERSION}\nLibs: -r:\${libdir}/Emgu.CV.dll -r:\${libdir}/Emgu.Util.dll -r:\${libdir}/Emgu.CV.ML.dll -r:\${libdir}/Emgu.CV.GPU.dll -r:\${libdir}/Emgu.CV.UI.dll -r:\${libdir}/ZedGraph.dll\n")
  INSTALL(
    FILES
    ${LINUX_CONTROL_FILE_FOLDER}/${CPACK_PACKAGE_NAME}.pc
    DESTINATION /usr/lib/pkgconfig
    COMPONENT emgucv_binary)
  
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
      "#!/bin/sh\nset -e\nif [ \"$1\" = \"remove\" ] || [ \"$1\" = \"upgrade\" ] && [ -x /usr/share/cli-common/gac-package-remove ]; then\nfor dll in Emgu.CV.UI Emgu.CV.ML Emgu.CV Emgu.Util Emgu.CV.GPU ZedGraph; do ${MONO_GACUTIL_EXEC} -u $dll; done\nfi\n")
    
    SET(DEBIAN_PACKAGE_PRIORITY "optional")
    SET(CPACK_DEBIAN_PACKAGE_CONTROL_EXTRA "${DEBIAN_CONTROL_FILE_FOLDER}/preinst;${DEBIAN_CONTROL_FILE_FOLDER}/postinst;${DEBIAN_CONTROL_FILE_FOLDER}/prerm;${DEBIAN_CONTROL_FILE_FOLDER}/postrm")
    SET(CPACK_DEBIAN_PACKAGE_DEPENDS "mono-runtime (>> 2.4.2), mono-2.0-gac, libmono-wcf3.0-cil, libtiff4, libgeotiff1.2, gtk2-engines, libgstreamer0.10-0, libavcodec52, libswscale0, libavformat52, libopenexr6, libjasper1, libdc1394-22, libv4l-0, libqt4-opengl")
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
      "for dll in Emgu.CV.UI Emgu.CV.ML Emgu.CV Emgu.Util Emgu.CV.GPU ZedGraph; do ${MONO_GACUTIL_EXEC} -u $dll; done\n")
    SET(CPACK_RPM_PRE_INSTALL_SCRIPT_FILE "${RPM_CONTROL_FILE_FOLDER}/preinst")
    SET(CPACK_RPM_POST_INSTALL_SCRIPT_FILE "${RPM_CONTROL_FILE_FOLDER}/postinst")
    SET(CPACK_RPM_PRE_UNINSTALL_SCRIPT_FILE "${RPM_CONTROL_FILE_FOLDER}/prerm")
    SET(CPACK_RPM_POST_UNINSTALL_SCRIPT_FILE "${RPM_CONTROL_FILE_FOLDER}/postrm")
  ENDIF()	
ENDIF()  

MESSAGE(STATUS "CPACK_GENERATOR: ${CPACK_GENERATOR}")
INCLUDE (CPack)

ENABLE_TESTING()
ADD_SUBDIRECTORY(tests/cvextern_test)

