
m4_include([m4/ax_prog_doxygen.m4])

AC_PREREQ([2.59])
AC_INIT([libgeotiff], [1.7.4], [warmerdam@pobox.com])
AC_CANONICAL_BUILD

AC_CONFIG_MACRO_DIR([m4])

LIBGEOTIFF_MAJOR_VERSION=1
LIBGEOTIFF_MINOR_VERSION=7
LIBGEOTIFF_PATCH_VERSION=4
LIBGEOTIFF_REV_VERSION=0

LIBGEOTIFF_STRING_VERSION=${LIBGEOTIFF_MAJOR_VERSION}.${LIBGEOTIFF_MINOR_VERSION}.${LIBGEOTIFF_PATCH_VERSION}
LIBGEOTIFF_VERSION="${LIBGEOTIFF_MAJOR_VERSION}${LIBGEOTIFF_MINOR_VERSION}${LIBGEOTIFF_PATCH_VERSION}${LIBGEOTIFF_REV_VERSION}"

AC_SUBST(LIBGEOTIFF_MAJOR_VERSION)
AC_SUBST(LIBGEOTIFF_MINOR_VERSION)
AC_SUBST(LIBGEOTIFF_PATCH_VERSION)
AC_SUBST(LIBGEOTIFF_REV_VERSION)
AC_SUBST(LIBGEOTIFF_STRING_VERSION)
AC_SUBST(LIBGEOTIFF_VERSION)

AC_CONFIG_HEADERS([geo_config.h])
AC_CONFIG_FILES([geotiff.h])

dnl #########################################################################
dnl Checks for programs.
dnl #########################################################################

AM_INIT_AUTOMAKE
AM_MAINTAINER_MODE
AC_PROG_CC
AC_PROG_INSTALL
AC_PROG_LN_S
AC_PROG_MAKE_SET
AC_PROG_LIBTOOL

dnl #########################################################################
dnl Default compilation flags
dnl #########################################################################

m4_define([debug_default],[no])

CFLAGS="$CFLAGS"

dnl We want to honor the users wishes with regard to linking.
LIBS="$LDFLAGS $LIBS"

dnl #########################################################################
dnl Checks for header files.
dnl #########################################################################

dnl AC_CHECK_HEADERS([string.h],[HAVE_STRING_H=1], [AC_MSG_ERROR([cannot find string.h, bailing out])])
AC_CHECK_HEADERS([strings.h],[GEOTIFF_HAVE_STRINGS_H=1])
AC_DEFINE_UNQUOTED(GEOTIFF_HAVE_STRINGS_H, 1,
          [Define if you have the <strings.h> header file.])

AC_CHECK_HEADERS([stdio.h],, [AC_MSG_ERROR([cannot find stdio.h, bailing out])])
AC_CHECK_HEADERS([stdlib.h],, [AC_MSG_ERROR([cannot find stdlib.h, bailing out])])

AC_CHECK_HEADERS([locale.h])

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
    CFLAGS="$CFLAGS -g -DDEBUG -Wall"
    AC_MSG_RESULT(yes)
else
    CFLAGS="$CFLAGS -O3 -DNDEBUG"
    AC_MSG_RESULT(no)
fi


dnl Checks for libraries.
AC_CHECK_LIB(m,exp,,,)


dnl #########################################################################
dnl Add option to include libz in library list for use of libtiff.
dnl #########################################################################

AC_ARG_WITH([zip],
    AC_HELP_STRING([--with-zip=ARG], [zlib library to use (yes or path)]),,)

AC_ARG_WITH([zlib],
    AC_HELP_STRING([--with-zlib=ARG], [ alias for --with-zip]),,)

AC_ARG_WITH([libz],
    AC_HELP_STRING([--with-libz=ARG], [ alias for --with-zip]),,)

HAVE_ZIP="no"
ZIP_CONFIG="no"

# Implement aliasing...

if test "$with_zlib" != "" ; then
  with_zip=$with_zlib
fi
if test "$with_libz" != "" ; then
  with_zip=$with_libz
fi


if test "$with_zip" != "" -a "x$with_zip" != "xno" ; then
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
            LIBS="-L${with_zip}/lib -lz $LIBS"
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
            LIBS="-L$with_jpeg/lib $LIBS"
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
  PKG_CHECK_MODULES(LIBTIFF, libtiff-4,
                    [TIFF_INC="$LIBTIFF_CFLAGS" LIBS="$LIBS $LIBTIFF_LIBS" TIFF_CONFIG="yes"],
                    [AC_CHECK_LIB(tiff,TIFFOpen,[TIFF_CONFIG=yes],
                                  AC_MSG_ERROR([You will need to substantially rewrite libxtiff to
build libgeotiff without libtiff]),-lm)
                    LIBS_SAVED="$LIBS"
                    AC_CHECK_LIB(tiff,TIFFMergeFieldInfo,[TIFF_CONFIG=yes],
                                 AC_MSG_ERROR([Libtiff 3.6.0 Beta or later required for this version of
libgeotiff. Please upgrade libtiff or use an older version of libgeotiff.]),-lm)
                    LIBS="$LIBS -ltiff"
                    TIFF_INC=
                    TIFF_CONFIG="yes"])
fi


AC_SUBST([TIFF_INC])
AC_SUBST([TIFF_PREFIX])
AM_CONDITIONAL([TIFF_IS_CONFIG], [test ! x$TIFF_CONFIG = xno])

dnl ---------------------------------------------------------------------------
dnl PROJ.6 related stuff
dnl ---------------------------------------------------------------------------

AC_MSG_CHECKING([for PROJ >= 6 library])

AC_ARG_WITH(proj,[  --with-proj=ARG Compile with PROJ.x (ARG=yes or path)],,)

PROJ_INCLUDE=""
if test "x$with_proj" = "xno" ; then

  AC_MSG_ERROR([PROJ 6 is a required dependency.])

else

  if test "x$with_proj" = "xyes" -o "x$with_proj" = "x"; then
    PKG_CHECK_MODULES(PROJ, proj, [LIBS="$LIBS $PROJ_LIBS" PROJ_FOUND=yes], [PROJ_FOUND=no])
    if test "$PROJ_FOUND" = "no"; then
        ORIG_LIBS="$LIBS"
        LIBS="-lproj $ORIG_LIBS"
        AC_CHECK_LIB(proj,proj_create_from_wkt,PROJ_FOUND=yes,PROJ_FOUND=no,)
    fi
    if test "$PROJ_FOUND" = "no"; then
        AC_CHECK_LIB(proj,internal_proj_create_from_wkt,PROJ_FOUND=yes,PROJ_FOUND=no,)
        if test "$PROJ_FOUND" = "yes"; then
            PROJ_INCLUDE="-DPROJ_RENAME_SYMBOLS"
        fi
    fi
    if test "$PROJ_FOUND" = "no"; then
        LIBS="-linternalproj $ORIG_LIBS"
        AC_CHECK_LIB(internalproj,internal_proj_create_from_wkt,PROJ_FOUND=yes,PROJ_FOUND=no,)
        if test "$PROJ_FOUND" = "yes"; then
            PROJ_INCLUDE="-DPROJ_RENAME_SYMBOLS"
        fi
    fi
    if test "$PROJ_FOUND" = "no"; then
        AC_MSG_ERROR([PROJ 6 symbols not found])
    fi
    AC_CHECK_HEADERS([proj.h])
    if test "$ac_cv_header_proj_h" = "yes" ; then
        AC_MSG_NOTICE([proj.h found])
    else
        AC_MSG_ERROR([proj.h not found])
    fi
  else

    ORIG_LIBS="$LIBS"
    LIBS="-L$with_proj/lib -lproj $ORIG_LIBS"
    AC_CHECK_LIB(proj,proj_create_from_wkt,PROJ_FOUND=yes,PROJ_FOUND=no,)
    if test "$PROJ_FOUND" = "no"; then
        LIBS="-L$with_proj/lib -lproj -lsqlite3 $ORIG_LIBS"
        unset ac_cv_lib_proj_proj_create_from_wkt
        AC_CHECK_LIB(proj,proj_create_from_wkt,PROJ_FOUND=yes,PROJ_FOUND=no,)
    fi
    if test "$PROJ_FOUND" = "no"; then
        LIBS="-L$with_proj/lib -lproj $ORIG_LIBS"
        AC_CHECK_LIB(proj,internal_proj_create_from_wkt,PROJ_FOUND=yes,PROJ_FOUND=no,)
        if test "$PROJ_FOUND" = "no"; then
            LIBS="-L$with_proj/lib -lproj -lsqlite3 $ORIG_LIBS"
            unset ac_cv_lib_proj_internal_proj_create_from_wkt
            AC_CHECK_LIB(proj,internal_proj_create_from_wkt,PROJ_FOUND=yes,PROJ_FOUND=no,)
        fi
        if test "$PROJ_FOUND" = "yes"; then
            PROJ_INCLUDE="-DPROJ_RENAME_SYMBOLS"
        fi
    fi
    if test "$PROJ_FOUND" = "no"; then
        LIBS="-L$with_proj/lib -linternalproj $ORIG_LIBS"
        AC_CHECK_LIB(internalproj,internal_proj_create_from_wkt,PROJ_FOUND=yes,PROJ_FOUND=no,)
        if test "$PROJ_FOUND" = "no"; then
            LIBS="-L$with_proj/lib -linternalproj -lsqlite3 $ORIG_LIBS"
            unset ac_cv_lib_internal_proj_internal_proj_create_from_wkt
            AC_CHECK_LIB(internalproj,internal_proj_create_from_wkt,PROJ_FOUND=yes,PROJ_FOUND=no,)
        fi
        if test "$PROJ_FOUND" = "yes"; then
            PROJ_INCLUDE="-DPROJ_RENAME_SYMBOLS"
        fi
    fi
    if test "$PROJ_FOUND" = "no"; then
        AC_MSG_ERROR([PROJ 6 symbols not found])
    fi
    if test -r "$with_proj/include/proj.h" ; then
        PROJ_INCLUDE="$PROJ_INCLUDE -I$with_proj/include"
        AC_MSG_NOTICE([proj.h found])
    else
        AC_MSG_ERROR([proj.h not found])
    fi
  fi
fi

AC_SUBST(PROJ_INCLUDE)


AC_ARG_ENABLE(towgs84, [  --disable-towgs84       Disable WGS84 parameters for binary compatibility with pre-1.4.1], AC_DEFINE([GEO_NORMALIZE_DISABLE_TOWGS84], [], [Disable WGS84 parameters]))

AC_MSG_CHECKING([if libtool needs -no-undefined flag to build shared libraries])
case "${host}" in
   *-*-cygwin* | *-*-mingw*|*-*-aix*)
    ## Add in the -no-undefined flag to LDFLAGS for libtool.
    AC_MSG_RESULT([yes])
    NOUNDEFINED=" -no-undefined"
    ;;
  *)
    ## Don't add in anything.
    AC_MSG_RESULT([no])
    ;;
esac
AC_SUBST([NOUNDEFINED])

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
    man/Makefile
    man/man1/Makefile
    cmake/Makefile
    test/Makefile
    libgeotiff.pc
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

LOC_MSG([  Debugging support........: ${enable_debug}])
LOC_MSG()

LOC_MSG([  zlib support......: ${ZIP_CONFIG}])
LOC_MSG([  jpeg support......: ${JPEG_CONFIG}])
LOC_MSG([  TIFF support......: ${TIFF_CONFIG}])
LOC_MSG([    -INCLUDE .......: ${TIFF_INC}])
LOC_MSG([    -PREFIX ........: ${TIFF_PREFIX}])
LOC_MSG([  PROJ support......: yes])
LOC_MSG([    -INCLUDE .......: ${PROJ_INCLUDE}])

LOC_MSG([  LIBS.....................: ${LIBS}])
LOC_MSG()
LOC_MSG([  libgeotiff - http://trac.osgeo.org/geotiff])
LOC_MSG()

dnl EOF
