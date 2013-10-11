
makegeo_SOURCES = makegeo.c
geotifcp_SOURCES = geotifcp.c
listgeo_SOURCES = listgeo.c
applygeo_SOURCES = applygeo.c

bin_PROGRAMS = makegeo geotifcp listgeo applygeo

if TIFF_IS_CONFIG
TIFF_CFLAGS = @TIFF_INC@ -DHAVE_TIFF=1
endif

AM_CFLAGS = -I../ -I../libxtiff $(TIFF_CFLAGS) 

LDADD = ../libgeotiff.la

EXTRA_DIST =    getopt.c

