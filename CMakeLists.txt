# --------------------------------------------------------
# Root CMake file for Emgu CV
#
# From the off-tree build directory, invoke:
#   $ cmake <PATH_TO_EMGUCV_ROOT>
#
# --------------------------------------------------------

PROJECT(emgucv)
CMAKE_MINIMUM_REQUIRED(VERSION 2.6)

SET(CMAKE_MODULE_PATH ${CMAKE_SOURCE_DIR}/cmake/modules)
FIND_PACKAGE(CSharp REQUIRED)
INCLUDE (CSharpMacros)

SET(LIBRARY_OUTPUT_PATH ${CMAKE_SOURCE_DIR}/bin)
SET(LIBRARY_3RDPARTY_PATH ${CMAKE_SOURCE_DIR}/lib/3rdParty)

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
	IF(EMGUCV_SVNVERSION MATCHES "^$")
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
IF (WIN32)
  STRING(REGEX MATCH "Win64" EMGUCV_ARCH "${CMAKE_GENERATOR}" )
  IF(NOT EMGUCV_ARCH)
	SET(EMGUCV_ARCH win32)
  ELSE(NOT EMGUCV_ARCH)
    SET(EMGUCV_ARCH win64)
  ENDIF(NOT EMGUCV_ARCH)
ELSE(WIN32)
  SET(EMGUCV_ARCH ${CMAKE_SYSTEM_NAME})
ENDIF(WIN32)
MESSAGE(STATUS "BUILDING FOR ${EMGUCV_ARCH}")

# ----------------------------------------------------------------------------
#  FIND MSBUILD
# ----------------------------------------------------------------------------
FIND_PROGRAM(MSBUILD_PROGRAM
  NAMES MSBuild
  HINTS
  "$ENV{SYSTEMROOT}/Microsoft.NET/Framework/v3.5"
  "C:/WINDOWS/Microsoft.NET/Framework/v3.5"
)

# ----------------------------------------------------------------------------
#  Setup Package information
# ----------------------------------------------------------------------------
SET(PACKAGE "${PROJECT_NAME}")
SET(CPACK_PACKAGE_CONTACT "Canming Huang <support@emgu.com>")
SET(PACKAGE_BUGREPORT "${CPACK_PACKAGE_CONTACT}")
SET(PACKAGE_NAME "${PROJECT_NAME}")

SET(CPACK_PACKAGE_NAME "${PROJECT_NAME}")
SET(CPACK_PACKAGE_DESCRIPTION_SUMMARY "Emgu CV is a cross platform .Net wrapper to the Intel OpenCV image processing library.")

SET(CPACK_PACKAGE_VENDOR "Emgu")
SET(CPACK_PACKAGE_VERSION_MAJOR "2")
SET(CPACK_PACKAGE_VERSION_MINOR "1")
SET(CPACK_PACKAGE_VERSION_PATCH "0")
SET(CPACK_PACKAGE_VERSION "${CPACK_PACKAGE_VERSION_MAJOR}.${CPACK_PACKAGE_VERSION_MINOR}.${CPACK_PACKAGE_VERSION_PATCH}.${EMGUCV_SVNVERSION}")
SET(CPACK_PACKAGE_FILE_NAME "lib${PACKAGE_NAME}-${CPACK_PACKAGE_VERSION}-${EMGUCV_ARCH}")
SET(CPACK_RESOURCE_FILE_LICENSE "${CMAKE_SOURCE_DIR}/Emgu.CV.License.txt")

IF (WIN32)
SET(CPACK_PACKAGE_CLI_FOLDER bin)
ELSE (WIN32)
SET(CPACK_PACKAGE_CLI_FOLDER lib/cli/${PACKAGE_NAME}-${CPACK_PACKAGE_VERSION_MAJOR}.${CPACK_PACKAGE_VERSION_MINOR})
ENDIF()

# ----------------------------------------------------------------------------
#  Build opencv and Emgu.CV.Extern
# ----------------------------------------------------------------------------
SET(CMAKE_SOURCE_DIR ${CMAKE_SOURCE_DIR}/opencv)
ADD_SUBDIRECTORY(opencv)
ADD_SUBDIRECTORY(Emgu.CV.Extern)

IF(MSVC)
  SET(extra_bin_prefix "../")
  SET_TARGET_PROPERTIES(cvextern PROPERTIES PREFIX ${extra_bin_prefix})
  SET_TARGET_PROPERTIES(cxcore PROPERTIES PREFIX ${extra_bin_prefix})
  SET_TARGET_PROPERTIES(cv PROPERTIES PREFIX ${extra_bin_prefix})
  SET_TARGET_PROPERTIES(cvaux PROPERTIES PREFIX ${extra_bin_prefix})
  SET_TARGET_PROPERTIES(ml PROPERTIES PREFIX ${extra_bin_prefix})

  SET_TARGET_PROPERTIES(highgui PROPERTIES PREFIX ${extra_bin_prefix})
  IF(opencv_ffmpeg)
    SET_TARGET_PROPERTIES(opencv_ffmpeg PROPERTIES PREFIX ${extra_bin_prefix})
  ENDIF()
ENDIF()

SET(EMGU_CV_BUILD ON CACHE BOOL "BUILD Emgu CV")
SET(EMGU_CV_EXAMPLE_BUILD ON CACHE BOOL "Build Emgu CV Example")
SET(EMGU_ENABLE_SSE OFF CACHE BOOL "ENABLE SSE for Emgu CV")

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
[assembly: AssemblyCopyright(\"Copyright © ${CPACK_PACKAGE_VENDOR} 2010\")]
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
GET_TARGET_PROPERTY(CXCORE_OUTPUT_NAME cxcore OUTPUT_NAME)
GET_TARGET_PROPERTY(CV_OUTPUT_NAME cv OUTPUT_NAME)
GET_TARGET_PROPERTY(HIGHGUI_OUTPUT_NAME highgui OUTPUT_NAME)
GET_TARGET_PROPERTY(ML_OUTPUT_NAME ml OUTPUT_NAME)
GET_TARGET_PROPERTY(CVAUX_OUTPUT_NAME cvaux OUTPUT_NAME)
GET_TARGET_PROPERTY(CVEXTERN_OUTPUT_NAME cvextern OUTPUT_NAME)


FILE(WRITE ${CMAKE_CURRENT_SOURCE_DIR}/Emgu.CV/PInvoke/CvInvokeEntryPoints.cs
  "//This file is automatically generate by CMAKE. DO NOT MODIFY.
using System;

namespace Emgu.CV
{
   public partial class CvInvoke
   {
      /// <summary>
      /// The file name of the cxcore library
      /// </summary>
      public const string CXCORE_LIBRARY = \"${CXCORE_OUTPUT_NAME}\";
      /// <summary>
      /// The file name of the cv library
      /// </summary>
      public const string CV_LIBRARY = \"${CV_OUTPUT_NAME}\";
      /// <summary>
      /// The file name of the highgui library
      /// </summary>
      public const string HIGHGUI_LIBRARY = \"${HIGHGUI_OUTPUT_NAME}\";
      /// <summary>
      /// The file name of the cvaux library
      /// </summary>
      public const string CVAUX_LIBRARY = \"${CVAUX_OUTPUT_NAME}\";
      /// <summary>
      /// The file name of the cvextern library
      /// </summary>
      public const string EXTERN_LIBRARY = \"${CVEXTERN_OUTPUT_NAME}\";
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

IF (WIN32)
  IF(EMGUCV_ARCH STREQUAL "win32")
    SET(CPACK_GENERATOR NSIS;ZIP)
  ELSE()
    SET(CPACK_GENERATOR ZIP)
  ENDIF()
  
  SET(CPACK_NSIS_MODIFY_PATH ON)
  
  ADD_SUBDIRECTORY(Emgu.CV.DebuggerVisualizers)
  
  # ----------------------------------------------------------------------------
  #  Bundle MSCVRT into deployment package
  # ----------------------------------------------------------------------------
  IF("${EMGUCV_ARCH}" STREQUAL "win32")
    INSTALL(
	FILES
	  "${CMAKE_CURRENT_SOURCE_DIR}/miscellaneous/vcredist_x86.exe"
	  DESTINATION ./
	  COMPONENT emgucv_binary 
	  )
	SET(CPACK_NSIS_EXTRA_INSTALL_COMMANDS "${CPACK_NSIS_EXTRA_INSTALL_COMMANDS}\nExecWait '$INSTDIR\\\\vcredist_x86.exe'")
  ELSE()
    IF("${EMGUCV_ARCH}" STREQUAL "win64")
	  INSTALL(
	  FILES
	    "${CMAKE_CURRENT_SOURCE_DIR}/miscellaneous/vcredist_x64.exe"
	    DESTINATION ./
	    COMPONENT emgucv_binary 
	    )
	  SET(CPACK_NSIS_EXTRA_INSTALL_COMMANDS "${CPACK_NSIS_EXTRA_INSTALL_COMMANDS}\nExecWait '$INSTDIR\\\\vcredist_x64.exe'")
	ELSE()
	ENDIF()
  ENDIF()
  
  # ----------------------------------------------------------------------------
  #  Install Debugger Visualizer
  # ----------------------------------------------------------------------------
  SET(DEBUGER_VISUALIZER_FILES 
	"Emgu.Util"
	"Emgu.CV"
	"Emgu.CV.UI"
	"Emgu.CV.DebuggerVisualizers"
	${CXCORE_OUTPUT_NAME}
	${CV_OUTPUT_NAME}
    ${CVAUX_OUTPUT_NAME}
	${HIGHGUI_OUTPUT_NAME}
	${CVEXTERN_OUTPUT_NAME})
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
installDebuggerVisualizer4${vs_version}Done:
")
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
uninstallDebuggerVisualizer4${vs_version}Done:
")
  ENDMACRO()
  
  INSTALL_DEBUGER_VISUALIZER("VS2005" "Visual Studio 2005" "SOFTWARE\\\\\\\\Microsoft\\\\\\\\VisualStudio\\\\\\\\8.0")
  SET(CPACK_NSIS_EXTRA_INSTALL_COMMANDS "${CPACK_NSIS_EXTRA_INSTALL_COMMANDS}\n${INSTALL_DEBUGER_VISUALIZER_COMMAND}")

  INSTALL_DEBUGER_VISUALIZER("VS2008" "Visual Studio 2008" "SOFTWARE\\\\\\\\Microsoft\\\\\\\\VisualStudio\\\\\\\\9.0")
  SET(CPACK_NSIS_EXTRA_INSTALL_COMMANDS "${CPACK_NSIS_EXTRA_INSTALL_COMMANDS}\n${INSTALL_DEBUGER_VISUALIZER_COMMAND}")
  
  INSTALL_DEBUGER_VISUALIZER("VS2010" "Visual Studio 2010" "SOFTWARE\\\\\\\\Microsoft\\\\\\\\VisualStudio\\\\\\\\10.0")
  SET(CPACK_NSIS_EXTRA_INSTALL_COMMANDS "${CPACK_NSIS_EXTRA_INSTALL_COMMANDS}\n${INSTALL_DEBUGER_VISUALIZER_COMMAND}")

  UNINSTALL_DEBUGER_VISUALIZER("VS2005" "SOFTWARE\\\\\\\\Microsoft\\\\\\\\VisualStudio\\\\\\\\8.0")
  SET(CPACK_NSIS_EXTRA_UNINSTALL_COMMANDS "${CPACK_NSIS_EXTRA_UNINSTALL_COMMANDS}\n${UNINSTALL_DEBUGER_VISUALIZER_COMMAND}")
  
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
  DESTINATION ./
  COMPONENT emgucv_source
  )

INSTALL(
  DIRECTORY
  ${CMAKE_CURRENT_SOURCE_DIR}/lib
  DESTINATION ./
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
)
  
INSTALL(
  DIRECTORY
  ${CMAKE_CURRENT_SOURCE_DIR}/Solution
  DESTINATION ./
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
  DESTINATION ./
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
  DESTINATION ./
  COMPONENT emgucv_example_source
  FILES_MATCHING 
  PATTERN "Emgu.CV.Example.sln"
  PATTERN ".svn" EXCLUDE
  PATTERN "bin" EXCLUDE
)

INSTALL(
  DIRECTORY
  ${CMAKE_CURRENT_SOURCE_DIR}/opencv/include/opencv
  DESTINATION ./opencv/include
  COMPONENT emgucv_example_source
  FILES_MATCHING 
  PATTERN "*.h"
  PATTERN "*.hpp"
  PATTERN ".svn" EXCLUDE
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
set(CPACK_NSIS_MENU_LINKS "doc/opencv.pdf" "Open CV Documentation")

IF(EMGU_CV_DOCUMENTATION_BUILD)
set(CPACK_COMPONENTS_ALL ${CPACK_COMPONENTS_ALL} emgucv_document)
set(CPACK_NSIS_MENU_LINKS ${CPACK_NSIS_MENU_LINKS} "Emgu.CV.Documentation.chm" "Emgu CV Documentation")
ENDIF()
	
set(CPACK_COMPONENT_MAIN_DISPLAY_NAME "OpenCV Library")
set(CPACK_COMPONENT_MAIN_REQUIRED ON)
set(CPACK_COMPONENT_EMGUCV_BINARY_DISPLAY_NAME "Emgu CV (Binary)")
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
    "/usr/${CPACK_PACKAGE_CLI_FOLDER}/Emgu.Util.dll\n/usr/${CPACK_PACKAGE_CLI_FOLDER}/Emgu.CV.dll\n/usr/${CPACK_PACKAGE_CLI_FOLDER}/Emgu.CV.ML.dll\n/usr/${CPACK_PACKAGE_CLI_FOLDER}/Emgu.CV.UI.dll\n/usr/${CPACK_PACKAGE_CLI_FOLDER}/ZedGraph.dll\n/usr/${CPACK_PACKAGE_CLI_FOLDER}/zlib.net.dll")
  INSTALL(
    FILES
    ${LINUX_CONTROL_FILE_FOLDER}/clifilelist.txt
    DESTINATION ${CPACK_PACKAGE_CLI_FOLDER}
    COMPONENT binary)

#creating the pkg-config file
    FILE(WRITE ${LINUX_CONTROL_FILE_FOLDER}/${CPACK_PACKAGE_NAME}.pc
      "prefix=/usr\nexec_prefix=\${prefix}\nlibdir=\${exec_prefix}/${CPACK_PACKAGE_CLI_FOLDER}\napidir=\${exec_prefix}/${CPACK_PACKAGE_CLI_FOLDER}\n\nName: ${CPACK_PACKAGE_NAME}\nDescription: ${CPACK_PACKAGE_DESCRIPTION_SUMMARY}\nVersion: ${CPACK_PACKAGE_VERSION}\nLibs: -r:\${libdir}/Emgu.CV.dll -r:\${libdir}/Emgu.Util.dll -r:\${libdir}/Emgu.CV.ML.dll -r:\${libdir}/Emgu.CV.UI.dll -r:\${libdir}/ZedGraph.dll -r:\${libdir}/zlib.net.dll\n")
    INSTALL(
      FILES
      ${LINUX_CONTROL_FILE_FOLDER}/${CPACK_PACKAGE_NAME}.pc
      DESTINATION /usr/lib/pkgconfig
      COMPONENT binary)

  SET(CPACK_STRIP_FILES "")
  MESSAGE(STATUS "Checking for CPACK build type...")
  FIND_PROGRAM(DPKG_CMD dpkg)
  IF (DPKG_CMD)
    # ----------------------------------------------------------------------------
    #  Building .deb package for Debian 
    # ----------------------------------------------------------------------------
    MESSAGE(STATUS "FOUND dpkg: ${DPKG_CMD}")
    SET(CPACK_GENERATOR DEB)

    SET(DEBIAN_CONTROL_FILE_FOLDER "${CMAKE_CURRENT_SOURCE_DIR}/cmake/debian")

    FILE(WRITE ${DEBIAN_CONTROL_FILE_FOLDER}/postinst
      "#!/bin/sh\nset -e\nif [ \"$1\" = \"configure\" ]; then\n  ldconfig\n  ${MONO_GACUTIL_EXEC} -il /usr/${CPACK_PACKAGE_CLI_FOLDER}/clifilelist.txt -package ${CPACK_PACKAGE_NAME}\nfi\n")
    FILE(WRITE ${DEBIAN_CONTROL_FILE_FOLDER}/prerm
      "#!/bin/sh\nset -e\nif [ \"$1\" = \"remove\" ] || [ \"$1\" = \"upgrade\" ] && [ -x /usr/share/cli-common/gac-package-remove ]; then\nfor dll in Emgu.CV.UI Emgu.CV.ML Emgu.CV Emgu.Util ZedGraph zlib.net; do ${MONO_GACUTIL_EXEC} -u $dll; done\nfi\n")
    
    SET(DEBIAN_PACKAGE_PRIORITY "optional")
    SET(CPACK_DEBIAN_PACKAGE_CONTROL_EXTRA "${DEBIAN_CONTROL_FILE_FOLDER}/postinst;${DEBIAN_CONTROL_FILE_FOLDER}/prerm;${DEBIAN_CONTROL_FILE_FOLDER}/postrm")
    SET(CPACK_DEBIAN_PACKAGE_DEPENDS "mono-runtime (>> 2.4.2), mono-2.0-gac, libmono-wcf3.0-cil")
        
    
  ELSE()  
    FIND_PROGRAM(RPMBUILD_CMD rpmbuild)
    IF(RPMBUILD_CMD)
      # ----------------------------------------------------------------------------
      #  Building .rpm package  
      # ----------------------------------------------------------------------------
      MESSAGE(STATUS "FOUND rpmbuild: ${RPMBUILD_CMD}")
      SET(CPACK_GENERATOR RPM)
      SET(CPACK_RPM_PACKAGE_LICENSE "GPL v3")
      SET(CPACK_RPM_PACKAGE_REQUIRES "mono-core >= 2.4,mono-wcf")

      SET(RPM_CONTROL_FILE_FOLDER "${CMAKE_CURRENT_SOURCE_DIR}/cmake/rpm")
      
      FILE(WRITE ${RPM_CONTROL_FILE_FOLDER}/postinst
	"ldconfig\n${MONO_GACUTIL_EXEC} -il /usr/${CPACK_PACKAGE_CLI_FOLDER}/clifilelist.txt -package ${CPACK_PACKAGE_NAME}\n")
      FILE(WRITE ${RPM_CONTROL_FILE_FOLDER}/prerm
	"for dll in Emgu.CV.UI Emgu.CV.ML Emgu.CV Emgu.Util ZedGraph zlib.net; do ${MONO_GACUTIL_EXEC} -u $dll; done\n")

      SET(CPACK_RPM_POST_INSTALL_SCRIPT_FILE "${RPM_CONTROL_FILE_FOLDER}/postinst")
      SET(CPACK_RPM_PRE_UNINSTALL_SCRIPT_FILE "${RPM_CONTROL_FILE_FOLDER}/prerm")
      SET(CPACK_RPM_POST_UNINSTALL_SCRIPT_FILE "${RPM_CONTROL_FILE_FOLDER}/postrm")
    ENDIF()	
  ENDIF()  
  
ENDIF()

MESSAGE(STATUS "CPACK_GENERATOR: ${CPACK_GENERATOR}")
INCLUDE (CPack)
