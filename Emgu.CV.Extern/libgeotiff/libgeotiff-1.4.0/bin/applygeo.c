/* applygeo.c */
#include <stdlib.h>
#include "geotiff.h"
#include "xtiffio.h"
 
static int
InstallGeoTIFF(const char *geofile, const char *tiffile)
{
    TIFF *tif = (TIFF*)0; /* TIFF-level descriptor */
    GTIF *gtif=(GTIF*)0; /* GeoKey-level descriptor */
    FILE *fp;

    uint16 *panVI = NULL;
    uint16 nKeyCount;
     
    tif = XTIFFOpen(tiffile, "r+");
    if (!tif)
    {
        perror(tiffile);
        fprintf(stderr, "Cannot open TIFF file %s (does not exist or not a valid TIFF file)\n", tiffile);
        return(-1);
    }

    /* If we have existing geokeys, try to wipe them
    by writing a dummy geokey directory. (#2546) */


    if( TIFFGetField( tif, TIFFTAG_GEOKEYDIRECTORY, 
                      &nKeyCount, &panVI ) )
    {
        uint16 anGKVersionInfo[4] = { 1, 1, 0, 0 };
        double  adfDummyDoubleParams[1] = { 0.0 };

        TIFFSetField( tif, TIFFTAG_GEOKEYDIRECTORY, 
                      4, anGKVersionInfo );
        TIFFSetField( tif, TIFFTAG_GEODOUBLEPARAMS, 
                      1, adfDummyDoubleParams );
        TIFFSetField( tif, TIFFTAG_GEOASCIIPARAMS, "" );
    }

    gtif = GTIFNew(tif);
    if (!gtif)
    {
        fprintf(stderr, "Internal error (GTIFNew)\n");
        return(-2);
    }
 
    /* Read GeoTIFF projection information from geofile */
    fp = fopen(geofile, "r");
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
    char *prog;
    char *geofile;
    char *tiffile;
    int rc;
 
    prog = argv[0];
    geofile = argv[1];
    tiffile = argv[2];
 
    if (!geofile || !tiffile)
    {
        fprintf(stderr, usage, prog);
        exit(1);
    }
 
    rc = InstallGeoTIFF(geofile, tiffile);
    if (rc)
    {
        fprintf(stderr, "%s: error %d applying projection from %s into TIFF %s\n", prog, rc, geofile, tiffile);
        exit(2);
    }
 
    return(0);
}
