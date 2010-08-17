include_HEADERS =   xtiffio.h 

if TIFF_IS_CONFIG
TIFF_CFLAGS = @TIFF_INC@ -DHAVE_TIFF=1
endif

AM_CFLAGS = -I../ $(TIFF_CFLAGS) 

libxtiff_la_SOURCES = xtiff.c

noinst_LTLIBRARIES = libxtiff.la
