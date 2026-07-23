/* applygeo.c */
#include <stdint.h>
#include <stdlib.h>
#include "geotiff.h"
#include "xtiffio.h"

static int
InstallGeoTIFF(const char *geofile, const char *tiffile)
{
    TIFF *tif = XTIFFOpen(tiffile, "r+");  /* TIFF-level descriptor */
    if (!tif)
    {
        perror(tiffile);
        fprintf(stderr, "Cannot open TIFF file %s (does not exist or not a valid TIFF file)\n", tiffile);
        return(-1);
    }

    /* If we have existing geokeys, try to wipe them
    by writing a dummy geokey directory. (#2546) */

    uint16_t *panVI = NULL;
    uint16_t nKeyCount;
    if( TIFFGetField( tif, TIFFTAG_GEOKEYDIRECTORY,
                      &nKeyCount, &panVI ) )
    {
        uint16_t anGKVersionInfo[4] = { 1, 1, 0, 0 };
        double  adfDummyDoubleParams[1] = { 0.0 };

        TIFFSetField( tif, TIFFTAG_GEOKEYDIRECTORY,
                      4, anGKVersionInfo );
        TIFFSetField( tif, TIFFTAG_GEODOUBLEPARAMS,
                      1, adfDummyDoubleParams );
        TIFFSetField( tif, TIFFTAG_GEOASCIIPARAMS, "" );
    }

    GTIF *gtif = GTIFNew(tif);  /* GeoKey-level descriptor */
    if (!gtif)
    {
        fprintf(stderr, "Internal error (GTIFNew)\n");
        return(-2);
    }

    /* Read GeoTIFF projection information from geofile */
    FILE *fp = fopen(geofile, "r");
    if( fp == NULL )
    {
        perror( geofile );
        fprintf(stderr, "Cannot open projection definition file %s\n", geofile);
        return(-3);
    }
    if (!GTIFImport(gtif, 0, fp))
    {
        fprintf(stderr,"Projection definition file is not valid (%s)\n", geofile);
        return(-4);
    }
    fclose(fp);

    /* Install GeoTIFF keys into the TIFF file */
    GTIFWriteKeys(gtif);

    /* Clean up */
    GTIFFree(gtif);
    TIFFRewriteDirectory(tif);
    XTIFFClose(tif);
    return(0);
}

int
main(int argc, char *argv[])
{
    char *usage = "usage: %s file.geo file.tiff\n"
        "geo\tfile containing projection (eg. from listgeo)\n"
        "tiff\tTIFF file into which the projection is written\n";
    if( argc != 3 )
    {
        fprintf(stderr, usage, "applygeo");
        exit(1);
    }

    char *prog = argv[0];
    char *geofile = argv[1];
    char *tiffile = argv[2];

    if (!geofile || !tiffile)
    {
        fprintf(stderr, usage, prog);
        exit(1);
    }

    const int rc = InstallGeoTIFF(geofile, tiffile);
    if (rc)
    {
        fprintf(stderr, "%s: error %d applying projection from %s into TIFF %s\n", prog, rc, geofile, tiffile);
        exit(2);
    }

    return(0);
}
