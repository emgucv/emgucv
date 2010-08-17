SUBDIRS = libxtiff . bin

AUTOMAKE_OPTIONS =	dist-zip
ACLOCAL_AMFLAGS = -I m4

include $(top_srcdir)/m4/doxygen.am

if PROJ_IS_CONFIG
PROJ_CFLAGS = @PROJ_INC@ -DHAVE_LIBPROJ=1
endif

if PROJECTS_H_IS_CONFIG
PROJ_CFLAGS = -DHAVE_PROJECTS_H=1 -DHAVE_LIBPROJ=1 @PROJ_INC@
endif

if TIFF_IS_CONFIG
TIFF_CFLAGS = @TIFF_INC@ -DHAVE_TIFF=1
endif

AM_CFLAGS = -I./libxtiff $(PROJ_CFLAGS) $(TIFF_CFLAGS) \
			-DCSV_DATA_DIR=\"$(datadir)/epsg_csv\"

csvdir = $(datadir)/epsg_csv

dist_csv_DATA = 	csv/alias.csv \
			csv/area.csv \
			csv/change.csv \
			csv/codes.csv \
			csv/coordinate_axis.csv \
			csv/coordinate_axis_name.csv \
			csv/coordinate_operation.csv \
			csv/coordinate_operation_method.csv \
			csv/coordinate_operation_parameter.csv \
			csv/coordinate_operation_parameter_value.csv \
			csv/coordinate_operation_path.csv \
			csv/coordinate_reference_system.csv \
			csv/coordinate_system.csv \
			csv/datum.csv \
			csv/ellipsoid.csv \
			csv/esri_datum_override.csv \
			csv/gcs.csv \
			csv/gcs.override.csv \
			csv/naming_system.csv \
			csv/pcs.csv \
			csv/pcs.override.csv \
			csv/prime_meridian.csv \
			csv/projop_wparm.csv \
			csv/stateplane.csv \
			csv/unit_of_measure.csv \
			csv/version_history.csv 
			
include_HEADERS =   geotiff.h \
                    geotiffio.h \
                    geovalues.h \
                    geonames.h \
                    geokeys.h \
                    geo_config.h \
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
                    geo_incode_defs.h


libgeotiff_la_SOURCES = cpl_serv.c \
                        cpl_csv.c \
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
                        geotiff_proj4.c 

if CSV_IS_CONFIG
    libgeotiff_la_SOURCES += csv/datum.c \
                             csv/ellipsoid.c \
                             csv/gcs.c \
                             csv/pcs.c \
                             csv/prime_meridian.c \
                             csv/projop_wparm.c \
                             csv/unit_of_measure.c 
endif

libgeotiff_la_LDFLAGS = -version-info 2:0:0

libgeotiff_la_LIBADD = libxtiff/libxtiff.la

lib_LTLIBRARIES = libgeotiff.la

EXTRA_DIST =    makefile.vc \
                README \
                README.WIN \
                README_BIN \
                LICENSE \
                INSTALL

MOSTLYCLEANFILES = $(DX_CLEANFILES)