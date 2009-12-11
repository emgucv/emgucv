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
    ENDIF()
    MESSAGE(STATUS "Emgu CV SVNVERSION: ${EMGUCV_SVNVERSION}")
ELSE()
    # We don't have svnversion:
    SET(EMGUCV_SVNVERSION "0")
ENDIF()

# ----------------------------------------------------------------------------
#  Check if 32bit or 64-bit
# ----------------------------------------------------------------------------
IF (CMAKE_SIZEOF_VOID_P MATCHES "4")
  SET(EMGUCV_ARCH "x86")
ELSE ()
  SET(EMGUCV_ARCH "x86-64")
ENDIF()

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
  SET_TARGET_PROPERTIES(opencv_ffmpeg PROPERTIES PREFIX ${extra_bin_prefix})
ENDIF()

#IF (WIN32)
  #As default, do not build EMGU on Windows. It can be build using Visual Studio
  #SET(EMGU_CV_BUILD OFF CACHE BOOL "Build Emgu CV")
  #SET(EMGU_CV_EXAMPLE_BUILD OFF CACHE BOOL "Build Emgu CV Example")
#ELSE (WIN32)
  #As default, build Emgu CV on Linux
  SET(EMGU_CV_BUILD ON CACHE BOOL "BUILD Emgu CV")
  SET(EMGU_CV_EXAMPLE_BUILD ON CACHE BOOL "Build Emgu CV Example")
#ENDIF ()

# ----------------------------------------------------------------------------
#  Generate required .cs files for Emgu CV project
# ----------------------------------------------------------------------------
FILE(WRITE ${CMAKE_CURRENT_SOURCE_DIR}/CommonAssemblyInfo.cs
  "using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
[assembly: AssemblyVersion(\"${CPACK_PACKAGE_VERSION}\")]
[assembly: AssemblyFileVersion(\"${CPACK_PACKAGE_VERSION}\")]
[assembly: AssemblyConfiguration(\"\")]
[assembly: AssemblyCompany(\"Emgu\")]
[assembly: AssemblyProduct(\"Emgu.CV\")]
[assembly: AssemblyCopyright(\"Copyright © Emgu 2009\")]
[assembly: AssemblyTrademark(\"\")]
[assembly: AssemblyCulture(\"\")]
")

GET_TARGET_PROPERTY(CXCORE_OUTPUT_NAME cxcore OUTPUT_NAME)
GET_TARGET_PROPERTY(CV_OUTPUT_NAME cv OUTPUT_NAME)
GET_TARGET_PROPERTY(HIGHGUI_OUTPUT_NAME highgui OUTPUT_NAME)
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
ADD_SUBDIRECTORY(lib)

if (WIN32)
ADD_SUBDIRECTORY(Emgu.RPC)
ENDIF()

# ----------------------------------------------------------------------------
#  Build the documents of Emgu CV
# ----------------------------------------------------------------------------
ADD_SUBDIRECTORY(miscellaneous)


# ----------------------------------------------------------------------------
#  Build the package
# ----------------------------------------------------------------------------
IF (WIN32)
  SET(CPACK_GENERATOR TBZ2)
ELSE (WIN32)
  FIND_PROGRAM(DPKG_CMD dpkg)
  IF (DPKG_CMD)
    # ----------------------------------------------------------------------------
    #  Building .deb package for Debian 
    # ----------------------------------------------------------------------------
    SET(CPACK_GENERATOR DEB)
    SET(DEBIAN_PACKAGE_PRIORITY "optional")
    SET(DEBIAN_CONTROL_FILE_FOLDER "${CMAKE_CURRENT_SOURCE_DIR}/cmake/debian")
    SET(CPACK_DEBIAN_PACKAGE_CONTROL_EXTRA "${DEBIAN_CONTROL_FILE_FOLDER}/postinst;${DEBIAN_CONTROL_FILE_FOLDER}/prerm;${DEBIAN_CONTROL_FILE_FOLDER}/postrm")
    SET(CPACK_DEBIAN_PACKAGE_DEPENDS "mono-runtime (>> 2.4.2), mono-2.0-gac, libmono-wcf3.0-cil")
    
    FILE(WRITE ${DEBIAN_CONTROL_FILE_FOLDER}/clifilelist.txt
      "/usr/${CPACK_PACKAGE_CLI_FOLDER}/Emgu.Util.dll\n/usr/${CPACK_PACKAGE_CLI_FOLDER}/Emgu.CV.dll\n/usr/${CPACK_PACKAGE_CLI_FOLDER}/Emgu.CV.ML.dll\n/usr/${CPACK_PACKAGE_CLI_FOLDER}/Emgu.CV.UI.dll\n/usr/${CPACK_PACKAGE_CLI_FOLDER}/ZedGraph.dll\n/usr/${CPACK_PACKAGE_CLI_FOLDER}/zlib.net.dll")
    INSTALL(
      FILES
      ${DEBIAN_CONTROL_FILE_FOLDER}/clifilelist.txt
      DESTINATION ${CPACK_PACKAGE_CLI_FOLDER}
      COMPONENT binary)
    
    SET(DEBIAN_GACUTIL_EXEC "gacutil")

    FILE(WRITE ${DEBIAN_CONTROL_FILE_FOLDER}/postinst
      "#!/bin/sh\nset -e\nif [ \"$1\" = \"configure\" ]; then\n  ldconfig\n  ${DEBIAN_GACUTIL_EXEC} -il ${DEBIAN_CONTROL_FILE_FOLDER}/clifilelist.txt -package ${CPACK_PACKAGE_NAME}\nfi\n")
    FILE(WRITE ${DEBIAN_CONTROL_FILE_FOLDER}/prerm
      "#!/bin/sh\nset -e\nif [ \"$1\" = \"remove\" ] || [ \"$1\" = \"upgrade\" ] && [ -x /usr/share/cli-common/gac-package-remove ]; then\nfor dll in Emgu.CV.UI Emgu.CV.ML Emgu.CV Emgu.Util ZedGraph zlib.net; do ${DEBIAN_GACUTIL_EXEC} -u $dll; done\nfi\n")
    FILE(WRITE ${DEBIAN_CONTROL_FILE_FOLDER}/${CPACK_PACKAGE_NAME}.pc
#      "prefix=/usr\nexec_prefix=\${prefix}\nlibdir=\${exec_prefix}/${CPACK_PACKAGE_CLI_FOLDER}\napidir=\${exec_prefix}/${CPACK_PACKAGE_CLI_FOLDER}\n\nName: ${CPACK_PACKAGE_NAME}\nDescription: ${CPACK_PACKAGE_DESCRIPTION_SUMMARY}\nVersion: ${CPACK_PACKAGE_VERSION}\nRequires: libmono-wcf3.0-cil\nCflags: -I:\${apidir}/Emgu.CV.xml -I:\${apidir}/Emgu.Util.xml -I:\${apidir}/Emgu.CV.ML.xml -I:\${apidir}/Emgu.CV.UI.xml -I:\${apidir}/ZedGraph.xml \nLibs: -r:\${libdir}/Emgu.CV.dll -r:\${libdir}/Emgu.Util.dll -r:\${libdir}/Emgu.CV.ML.dll -r:\${libdir}/Emgu.CV.UI.dll -r:\${libdir}/ZedGraph.dll -r:\${libdir}/zlib.net.dll\n")
      "prefix=/usr\nexec_prefix=\${prefix}\nlibdir=\${exec_prefix}/${CPACK_PACKAGE_CLI_FOLDER}\napidir=\${exec_prefix}/${CPACK_PACKAGE_CLI_FOLDER}\n\nName: ${CPACK_PACKAGE_NAME}\nDescription: ${CPACK_PACKAGE_DESCRIPTION_SUMMARY}\nVersion: ${CPACK_PACKAGE_VERSION}\nLibs: -r:\${libdir}/Emgu.CV.dll -r:\${libdir}/Emgu.Util.dll -r:\${libdir}/Emgu.CV.ML.dll -r:\${libdir}/Emgu.CV.UI.dll -r:\${libdir}/ZedGraph.dll -r:\${libdir}/zlib.net.dll\n")
    INSTALL(
      FILES
      ${DEBIAN_CONTROL_FILE_FOLDER}/${CPACK_PACKAGE_NAME}.pc
      DESTINATION /usr/lib/pkgconfig
      COMPONENT binary)

  ENDIF()  
  
ENDIF()

INCLUDE (CPack)
