SUBDIRS = libxtiff . bin man cmake test

AUTOMAKE_OPTIONS = dist-zip subdir-objects
ACLOCAL_AMFLAGS = -I m4

include $(top_srcdir)/m4/doxygen.am

PROJ_CFLAGS= @PROJ_INCLUDE@

if TIFF_IS_CONFIG
TIFF_CFLAGS = @TIFF_INC@ -DHAVE_TIFF=1
endif

AM_CFLAGS = -I$(srcdir)/libxtiff $(PROJ_CFLAGS) $(TIFF_CFLAGS)

include_HEADERS =   geotiffio.h \
                    geovalues.h \
                    geonames.h \
                    geokeys.h \
                    geo_tiffp.h \
                    geo_keyp.h \
                    geo_normalize.h \
                    cpl_serv.h \
                    geo_simpletags.h \
                    epsg_datum.inc \
                    epsg_gcs.inc \
                    epsg_pm.inc \
                    epsg_units.inc \
                    geo_ctrans.inc \
                    epsg_ellipse.inc \
                    epsg_pcs.inc \
                    epsg_proj.inc \
                    epsg_vertcs.inc \
                    geokeys.inc \
                    geokeys_v1_1.inc

nodist_include_HEADERS = geotiff.h \
                         geo_config.h

libgeotiff_la_SOURCES = cpl_serv.c \
                        geo_extra.c \
                        geo_free.c \
                        geo_get.c \
                        geo_names.c \
                        geo_new.c \
                        geo_normalize.c \
                        geo_print.c \
                        geo_set.c \
                        geo_simpletags.c \
                        geo_tiffp.c \
                        geo_trans.c \
                        geo_write.c \
                        geo_strtod.c \
                        geotiff_proj4.c

libgeotiff_la_LDFLAGS = -version-info 7:3:2 ${NOUNDEFINED}

libgeotiff_la_LIBADD = libxtiff/libxtiff.la

lib_LTLIBRARIES = libgeotiff.la

pkgconfig_DATA = libgeotiff.pc
pkgconfigdir = $(libdir)/pkgconfig

EXTRA_DIST =    autogen.sh \
                geo_config.h.vc \
                README \
                README.WIN \
                README_BIN \
                LICENSE \
                CMakeLists.txt \
                libxtiff/CMakeLists.txt \
                bin/CMakeLists.txt \
                docs/Doxyfile \
                docs/geotifcp.html \
                docs/index.dox \
                docs/listgeo.html \
                docs/manual.txt

MOSTLYCLEANFILES = $(DX_CLEANFILES)
