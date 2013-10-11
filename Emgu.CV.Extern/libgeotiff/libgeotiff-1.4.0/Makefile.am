SUBDIRS = libxtiff . bin man cmake

AUTOMAKE_OPTIONS =	dist-zip
ACLOCAL_AMFLAGS = -I m4

include $(top_srcdir)/m4/doxygen.am

PROJ_CFLAGS=
if PROJ_IS_CONFIG
PROJ_CFLAGS += @PROJ_INC@ -DHAVE_LIBPROJ=1
endif

if PROJECTS_H_IS_CONFIG
PROJ_CFLAGS += -DHAVE_PROJECTS_H=1
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
			csv/compdcs.csv \
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
			csv/datum_shift.csv \
			csv/datum_shift_pref.csv \
			csv/deprecation.csv \
			csv/ellipsoid.csv \
			csv/esri_datum_override.csv \
			csv/gcs.csv \
			csv/gcs.override.csv \
			csv/geoccs.csv \
			csv/naming_system.csv \
			csv/pcs.csv \
			csv/pcs.override.csv \
			csv/prime_meridian.csv \
			csv/projop_wparm.csv \
			csv/stateplane.csv \
			csv/supersession.csv \
			csv/unit_of_measure.csv \
			csv/version_history.csv \
			csv/vertcs.csv \
			csv/vertcs.override.csv 

include_HEADERS =   geotiff.h \
                    geo_config.h \
                    geotiffio.h \
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
                        geo_strtod.c \
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

libgeotiff_la_LDFLAGS = -version-info 3:0:1

libgeotiff_la_LIBADD = libxtiff/libxtiff.la

lib_LTLIBRARIES = libgeotiff.la

EXTRA_DIST =    makefile.vc \
                geo_config.h.vc \
                README \
                README.WIN \
                README_BIN \
                LICENSE \
                INSTALL \
                CMakeLists.txt \
                libxtiff/CMakeLists.txt \
                bin/CMakeLists.txt 

MOSTLYCLEANFILES = $(DX_CLEANFILES)
