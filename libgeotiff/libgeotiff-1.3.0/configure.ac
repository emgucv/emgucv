

m4_define([VERSION_MAJOR], [1])
m4_define([VERSION_MINOR], [3])
m4_define([VERSION_POINT], [0])
m4_define([GEOTIFF_VERSION],
          [VERSION_MAJOR.VERSION_MINOR.VERSION_POINT])

m4_include([m4/ax_prog_doxygen.m4])

AC_PREREQ([2.59])
AC_INIT([libgeotiff], [GEOTIFF_VERSION], [warmerdam@pobox.com])
AC_CANONICAL_BUILD

AC_CONFIG_MACRO_DIR([m4])

RELEASE_VERSION=GEOTIFF_VERSION
AC_SUBST([RELEASE_VERSION])
AM_CONFIG_HEADER(geo_config.h)

dnl #########################################################################
dnl Checks for programs.
dnl #########################################################################

AM_INIT_AUTOMAKE
AM_MAINTAINER_MODE
AC_PROG_CC
AC_PROG_CXX
AC_PROG_CXXCPP
AC_PROG_INSTALL
AC_PROG_LN_S
AC_PROG_MAKE_SET
AC_PROG_LIBTOOL

dnl AC_PROG_CC
dnl AC_COMPILER_WFLAGS

dnl AC_PROG_RANLIB
dnl AC_COMPILER_PIC
dnl AC_LD_SHARED

dnl #########################################################################
dnl Default compilation flags
dnl #########################################################################

m4_define([debug_default],[no])

CFLAGS="$CFLAGS"
CXXFLAGS="$CXXFLAGS"

dnl We want to honor the users wishes with regard to linking.
LIBS="$LDFLAGS $LIBS"

dnl #########################################################################
dnl Checks for header files.
dnl #########################################################################

AC_CHECK_HEADERS([string.h],[HAVE_STRING_H=1], [AC_MSG_ERROR([cannot find string.h, bailing out])])
AC_CHECK_HEADERS([stdio.h],, [AC_MSG_ERROR([cannot find stdio.h, bailing out])])
AC_CHECK_HEADERS([stdlib.h],, [AC_MSG_ERROR([cannot find stdlib.h, bailing out])])


LIBS="${LIBS}"

dnl #########################################################################
dnl Definiion of custom Autoconf macros
dnl #########################################################################

AC_DEFUN([LOC_MSG],[
echo "$1"
])

dnl #########################################################################
dnl Build mode configuration (debug/optimized)
dnl #########################################################################

AC_ARG_ENABLE([debug],
    AC_HELP_STRING([--enable-debug=ARG], [Enable debug compilation mode @<:@yes|no@:>@, default=debug_default]),,)
    
AC_MSG_CHECKING([for debug enabled])

if test "x$enable_debug" = "xyes"; then
    CFLAGS="$CFLAGS -g -DDEBUG -Wall -Wno-long-long -pedantic -std=c99"
    CXXFLAGS="$CXXFLAGS -g -DDEBUG -Wall -Wno-long-long -pedantic -std=c++98"
    AC_MSG_RESULT(yes)
else
    CFLAGS="$CFLAGS -O3 -DNDEBUG"
    CXXFLAGS="$CXXFLAGS -O3 -DNDEBUG"
    AC_MSG_RESULT(no)
fi


dnl Checks for libraries.
AC_CHECK_LIB(m,exp,,,)


dnl #########################################################################
dnl Add option to include libz in library list for use of libtiff.
dnl #########################################################################

AC_ARG_WITH([zip],
    AC_HELP_STRING([--with-zip=ARG], [zlib library to use (yes or path)]),,)

AC_ARG_WITH([zip1],
    AC_HELP_STRING([--with-zlib=ARG], [ alias for --with-zip]),,)

AC_ARG_WITH([zip2],
    AC_HELP_STRING([--with-libz=ARG], [ alias for --with-zip]),,)

HAVE_ZIP="no"
ZIP_CONFIG="no"

if test "$with_zip" != "" -o "$with_zlib" != "" -o "$with_libz" != ""; then
    AC_CHECK_HEADERS([zlib.h],, [AC_MSG_ERROR([cannot find zlib.h, bailing out])])
    AC_SEARCH_LIBS([inflateInit_], [z],
                 [ZIP_CONFIG=yes], [ZIP_CONFIG=no],
                 [-L$with_zip/lib -lm])

else
    AC_MSG_CHECKING([for zlib])
    AC_MSG_RESULT([no])
fi

if test "${ZIP_CONFIG}" != "no" ; then

    if test "${ZIP_CONFIG}" = "yes" ; then
        if test -d ${with_zip}/include ; then
            ZIP_INC="-I$with_zip/include"
            AC_MSG_RESULT([using zlib from $with_zip/include]) 
            HAVE_ZLIB="yes"
            ZIP_CONFIG="yes"
        fi
    fi

fi

AC_SUBST([ZIP_INC])
AC_SUBST([ZIP_PREFIX])
AM_CONDITIONAL([ZIP_IS_CONFIG], [test ! x$ZIP_CONFIG = xno])



dnl #########################################################################
dnl Add option to include libjpeg in library list for use of libtiff.
dnl #########################################################################

AC_ARG_WITH([jpeg],
    AC_HELP_STRING([--with-jpeg=ARG], [jpeg library to use (yes or path)]),,)

HAVE_JPEG="no"
JPEG_CONFIG="no"

if test "$with_jpeg" = "no" -o "$with_jpeg" = "" ; then
    AC_MSG_CHECKING([for jpeg])
    AC_MSG_RESULT([no])
else
    AC_SEARCH_LIBS([jinit_compress_master], [jpeg],
                 [JPEG_CONFIG=yes], [JPEG_CONFIG=no],
                 [-L$with_jpeg/lib -lm])
fi

if test "${JPEG_CONFIG}" != "no" ; then

    if test "${JPEG_CONFIG}" = "yes" ; then
        if test -d ${with_jpeg}/include ; then
            JPEG_INC="-I$with_jpeg/include"
            AC_MSG_RESULT([using jpeg from $with_jpeg/include]) 
        fi
    fi
    HAVE_JPEG="yes"
    JPEG_CONFIG="yes"
fi

AC_SUBST([JPEG_INC])
AC_SUBST([JPEG_PREFIX])
AM_CONDITIONAL([JPEG_IS_CONFIG], [test ! x$JPEG_CONFIG = xno])


dnl #########################################################################
dnl Special logic to try and find a user specified libtiff tree.  If not
dnl available check if the system directories have the library, _and_ the
dnl required private include files.  Normally these private include files
dnl (tiffiop.h) are not installed with the public interface.
dnl #########################################################################

TIFF_CONFIG="no"

AC_ARG_WITH(libtiff, [  --with-libtiff=dir      Use libtiff in directory dir],,)

if test "$with_libtiff" != "" ; then
  if test -f $with_libtiff/include/tiffio.h -a -d $with_libtiff/lib ; then
    LIBS="-L$with_libtiff/lib -ltiff $LIBS"
    TIFF_INC=-I$with_libtiff/include
  else
    LIBS="-L$with_libtiff -ltiff $LIBS"
    TIFF_INC=-I$with_libtiff
  fi
  LIBS_SAVED="$LIBS"
  AC_CHECK_LIB(tiff,TIFFOpen,[TIFF_CONFIG=yes],
               AC_MSG_ERROR([failed to link with -ltiff to find TIFFOpen]),-lm)
  AC_CHECK_LIB(tiff,TIFFMergeFieldInfo,[TIFF_CONFIG=yes],
               AC_MSG_ERROR([Libtiff 3.6.0 Beta or later required for this version of
libgeotiff. Please upgrade or use an older version of libgeotiff.]),-lm)
  LIBS="$LIBS_SAVED"
else
  AC_CHECK_LIB(tiff,TIFFOpen,[TIFF_CONFIG=yes],
               AC_MSG_ERROR([You will need to substantially rewrite libxtiff to
build libgeotiff without libtiff]),-lm)
  LIBS_SAVED="$LIBS"
  AC_CHECK_LIB(tiff,TIFFMergeFieldInfo,[TIFF_CONFIG=yes],
               AC_MSG_ERROR([Libtiff 3.6.0 Beta or later required for this version of
libgeotiff. Please upgrade libtiff or use an older version of libgeotiff.]),-lm)
  LIBS="$LIBS -ltiff"
  TIFF_INC=
  TIFF_CONFIG="yes"
fi


AC_SUBST([TIFF_INC])
AC_SUBST([TIFF_PREFIX])
AM_CONDITIONAL([TIFF_IS_CONFIG], [test ! x$TIFF_CONFIG = xno])

dnl
dnl Try and find libproj.a, and projects.h
dnl

PROJ_CONFIG="no"
PROJECTS_H_CONFIG="no"

AC_ARG_WITH(proj, [  --with-proj[=ARG]       Use PROJ.4 library (ARG=no or path)],[PROJ_HOME=$withval],[PROJ_HOME=])

if test "$PROJ_HOME" != "" -a "$PROJ_HOME" != "no" ; then
  if test -d "$PROJ_HOME/src/pj_init.c" ; then
    PROJ_HOME=$PROJ_HOME/src
  fi

  dnl Old PROJ.4 build tree, without automake
  if test -f "$PROJ_HOME/libproj.a" -a -f "$PROJ_HOME/projects.h" ; then
    PROJ_LIB=$PROJ_HOME/libproj.a
    PROJ_INC=$PROJ_HOME
    PROJ_CONFIG="yes"

  dnl New PROJ.4 build tree, with automake
  elif test -f "$PROJ_HOME/.libs/libproj.a" -a -f "$PROJ_HOME/projects.h" ; then
    PROJ_LIB=$PROJ_HOME/.libs/libproj.a
    PROJ_INC=$PROJ_HOME

    PROJ_CONFIG="yes"
    
  dnl Installed tree in an odd place
  elif test \( -f "$PROJ_HOME/lib/libproj.a" -o -f "$PROJ_HOME/lib/libproj.so" \) -a -f "$PROJ_HOME/include/proj_api.h"; then
    PROJ_LIB="-L$PROJ_HOME/lib -lproj"
    PROJ_INC=$PROJ_HOME/include 

    PROJ_CONFIG="yes"
  else
    echo "PROJ_HOME=$PROJ_HOME, but either projects.h or libproj.a not found!"
  fi
fi


if test "$PROJ_HOME" = "no" ; then
  echo "PROJ.4 support disabled."

elif test "$PROJ_LIB" != "" -a "$PROJ_INC" != "" ; then
  echo "Found $PROJ_LIB"
  echo "Found $PROJ_INC/projects.h"

  LIBS="$PROJ_LIB $LIBS"
  PROJ_INC=-I$PROJ_INC

  AC_SUBST(PROJ_INC,$PROJ_INC)
  AC_DEFINE(HAVE_PROJECTS_H)
  AC_DEFINE(HAVE_LIBPROJ)
  AC_SUBST([HAVE_LIBPROJ])
  PROJ_CONFIG="yes"
  PROJECTS_H_CONFIG="yes"

else
  AC_CHECK_LIB(proj,pj_init,,,-lm)
  AC_CHECK_HEADERS(projects.h,,)
  PROJ_CONFIG="yes"
fi

AM_CONDITIONAL([PROJ_IS_CONFIG], [test ! x$PROJ_CONFIG = xno])
AM_CONDITIONAL([PROJECTS_H_IS_CONFIG], [test ! x$PROJECTS_H_CONFIG = xno])


dnl
dnl Has the user requested "incode" EPSG tables, overriding the
dnl default use of EPSG tables in csv files?  Temporarily unvailable.
dnl

CSV_CONFIG="no"
AC_ARG_ENABLE(incode-epsg, [  --enable-incode-epsg    Use C code EPSG tables],, enable_incode_epsg=no)

if test "$enable_incode_epsg" != "no"  -o "$enable_incode_epsg" = ""  ; then

    AC_MSG_CHECKING([if EPSG tables will be read from .csv files])
    AC_MSG_RESULT([yes])
    CSV_CONFIG="yes"
else
    AC_MSG_CHECKING([if EPSG tables will be read from .csv files])
    AC_MSG_RESULT([no])

fi
AM_CONDITIONAL([CSV_IS_CONFIG], [test ! x$CSV_CONFIG = xno])

dnl #########################################################################
dnl Doxygen settings
dnl #########################################################################

DX_HTML_FEATURE(ON)
DX_CHM_FEATURE(OFF)
DX_CHI_FEATURE(OFF)
DX_MAN_FEATURE(ON)
DX_RTF_FEATURE(OFF)
DX_XML_FEATURE(OFF)
DX_PDF_FEATURE(OFF)
DX_PS_FEATURE(OFF)

DX_INIT_DOXYGEN($PACKAGE_NAME, docs/Doxyfile, docs/output)



dnl #########################################################################
dnl Generate makefiles
dnl #########################################################################

AC_CONFIG_FILES([
    Makefile
    libxtiff/Makefile
    bin/Makefile
])

AC_OUTPUT

dnl #########################################################################
dnl Print configuration summary
dnl #########################################################################

LOC_MSG()
LOC_MSG([libgeotiff configuration summary:])
LOC_MSG()
LOC_MSG([  Version..................: ${RELEASE_VERSION}])
LOC_MSG([  Installation directory...: ${prefix}])
LOC_MSG([  C compiler...............: ${CC} ${CFLAGS}])
LOC_MSG([  C++ compiler.............: ${CXX} ${CXXFLAGS}])

LOC_MSG([  Debugging support........: ${enable_debug}])
LOC_MSG()

LOC_MSG([  zlib support......: ${ZIP_CONFIG}])
LOC_MSG([  jpeg support......: ${JPEG_CONFIG}])
LOC_MSG([  TIFF support......: ${TIFF_CONFIG}])
LOC_MSG([    -INCLUDE .......: ${TIFF_INC}])
LOC_MSG([    -PREFIX ........: ${TIFF_PREFIX}])
LOC_MSG([  PROJ support......: ${PROJ_CONFIG}])
LOC_MSG([    -INCLUDE .......: ${PROJ_INC}])
LOC_MSG([    -PREFIX ........: ${PROJ_PREFIX}])
LOC_MSG([  Incode CSV support: ${CSV_CONFIG}])

LOC_MSG([  LIBS.....................: ${LIBS}])
LOC_MSG()
LOC_MSG([  libgeotiff - http://trac.osgeo.org/geotiff])
LOC_MSG()

dnl EOF
