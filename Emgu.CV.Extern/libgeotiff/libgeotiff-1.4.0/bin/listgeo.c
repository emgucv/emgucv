/*
 * listgeo.c -- example client code for LIBGEO geographic
 *     TIFF tag support. Dumps info to GeoTIFF metadata file.
 *
 *  Author: Niles D. Ritter
 *
 */

#include "geotiff.h"
#include "xtiffio.h"
#include "geo_normalize.h"
#include "geo_simpletags.h"
#include "geovalues.h"
#include "tiffio.h"
#include "cpl_serv.h"
#include <stdio.h>

static void WriteTFWFile( GTIF * gtif, const char * tif_filename );
static void GTIFPrintCorners( GTIF *, GTIFDefn *, FILE *, int, int, int, int );
static const char *CSVFileOverride( const char * );
static const char *CSVDirName = NULL;
static TIFF *st_setup_test_info();

void Usage()

{
    printf( 
        "%s", 
        "Usage: listgeo [-d] [-tfw] [-proj4] [-no_norm] [-t tabledir] filename\n"
        "\n"
        "  -d: report lat/long corners in decimal degrees instead of DMS.\n"
        "  -tfw: Generate a .tfw (ESRI TIFF World) file for the target file.\n"
        "  -proj4: Report PROJ.4 equivelent projection definition.\n"
        "  -no_norm: Don't report 'normalized' parameter values.\n"
        "  filename: Name of the GeoTIFF file to report on.\n" );
        
    exit( 1 );
}

int main(int argc, char *argv[])
{
    char	*fname = NULL;
    TIFF 	*tif=(TIFF*)0;  /* TIFF-level descriptor */
    GTIF	*gtif=(GTIF*)0; /* GeoKey-level descriptor */
    int		i, norm_print_flag = 1, proj4_print_flag = 0;
    int		tfw_flag = 0, inv_flag = 0, dec_flag = 0;
    int         st_test_flag = 0;

    /*
     * Handle command line options.
     */
    for( i = 1; i < argc; i++ )
    {
        if( strcmp(argv[i],"-no_norm") == 0 )
            norm_print_flag = 0;
        else if( strcmp(argv[i],"-t") == 0 )
        {
            CSVDirName = argv[++i];
            SetCSVFilenameHook( CSVFileOverride );
        }
        else if( strcmp(argv[i],"-tfw") == 0 )
            tfw_flag = 1;
        else if( strcmp(argv[i],"-proj4") == 0 )
            proj4_print_flag = 1;
        else if( strcmp(argv[i],"-i") == 0 )
            inv_flag = 1;
        else if( strcmp(argv[i],"-d") == 0 )
            dec_flag = 1;
        else if( strcmp(argv[i],"-st_test") == 0 )
        {
            st_test_flag = 1;
            norm_print_flag = 0;
        }
        else if( fname == NULL && argv[i][0] != '-' )
            fname = argv[i];
        else
        {
            Usage();
        }
    }

    if( fname == NULL && !st_test_flag )
        Usage();

    /*
     * Open the file, read the GeoTIFF information, and print to stdout. 
     */

    if( st_test_flag )
    {
        tif = st_setup_test_info();
        gtif = GTIFNewSimpleTags( tif );
    }
    else
    {
        tif=XTIFFOpen(fname,"r");
        if (!tif) goto failure;
	
        gtif = GTIFNew(tif);
        if (!gtif)
        {
            fprintf(stderr,"failed in GTIFNew\n");
            goto failure;
        }
    }

    if( tfw_flag )
    {
        WriteTFWFile( gtif, fname );

        goto Success;
    }
	
    /* dump the GeoTIFF metadata to std out */

    GTIFPrint(gtif,0,0);

    /*
     * Capture, and report normalized information if requested.
     */

    if( norm_print_flag )
    {
        GTIFDefn	defn;
        
        if( GTIFGetDefn( gtif, &defn ) )
        {
            int		xsize, ysize;
            
            printf( "\n" );
            GTIFPrintDefn( &defn, stdout );

            if( proj4_print_flag )
            {
                printf( "\n" );
                printf( "PROJ.4 Definition: %s\n", GTIFGetProj4Defn(&defn));
            }
            
            TIFFGetField( tif, TIFFTAG_IMAGEWIDTH, &xsize );
            TIFFGetField( tif, TIFFTAG_IMAGELENGTH, &ysize );
            GTIFPrintCorners( gtif, &defn, stdout, xsize, ysize, inv_flag, dec_flag );
        }

    }

  Success:
    GTIFFree(gtif);
    if( st_test_flag )
        ST_Destroy( (ST_TIFF *) tif );
    else
        XTIFFClose(tif);
    GTIFDeaccessCSV();
    return 0;
		
  failure:
    fprintf(stderr,"failure in listgeo\n");
    if (tif) XTIFFClose(tif);
    if (gtif) GTIFFree(gtif);
    GTIFDeaccessCSV();
    return 1;
}

static const char *CSVFileOverride( const char * pszInput )

{
    static char		szPath[1024];

#ifdef WIN32
    sprintf( szPath, "%s\\%s", CSVDirName, pszInput );
#else    
    sprintf( szPath, "%s/%s", CSVDirName, pszInput );
#endif    

    return( szPath );
}

/*
 * Report the file(s) corner coordinates in projected coordinates, and
 * if possible lat/long.
 */

static int GTIFReportACorner( GTIF *gtif, GTIFDefn *defn, FILE * fp_out,
                              const char * corner_name,
                              double x, double y, int inv_flag, int dec_flag )

{
    double	x_saved, y_saved;

    /* Try to transform the coordinate into PCS space */
    if( !GTIFImageToPCS( gtif, &x, &y ) )
        return FALSE;
    
    x_saved = x;
    y_saved = y;

    fprintf( fp_out, "%-13s ", corner_name );

    if( defn->Model == ModelTypeGeographic )
    {
	if (dec_flag) 
	{
	    fprintf( fp_out, "(%.7f,", x );
	    fprintf( fp_out, "%.7f)\n", y );
	} 
	else 
	{
	    fprintf( fp_out, "(%s,", GTIFDecToDMS( x, "Long", 2 ) );
	    fprintf( fp_out, "%s)\n", GTIFDecToDMS( y, "Lat", 2 ) );
	}
    }
    else
    {
        fprintf( fp_out, "(%12.3f,%12.3f)", x, y );

        if( GTIFProj4ToLatLong( defn, 1, &x, &y ) )
        {
	    if (dec_flag) 
	    {
                fprintf( fp_out, "  (%.7f,", x );
                fprintf( fp_out, "%.7f)", y );
	    } 
	    else 
	    {
		fprintf( fp_out, "  (%s,", GTIFDecToDMS( x, "Long", 2 ) );
		fprintf( fp_out, "%s)", GTIFDecToDMS( y, "Lat", 2 ) );
	    }
        }

        fprintf( fp_out, "\n" );
    }

    if( inv_flag && GTIFPCSToImage( gtif, &x_saved, &y_saved ) )
    {
        fprintf( fp_out, "      inverse (%11.3f,%11.3f)\n", x_saved, y_saved );
    }
    
    return TRUE;
}

static void GTIFPrintCorners( GTIF *gtif, GTIFDefn *defn, FILE * fp_out,
                              int xsize, int ysize, int inv_flag, int dec_flag )

{
    printf( "\nCorner Coordinates:\n" );
    if( !GTIFReportACorner( gtif, defn, fp_out,
                            "Upper Left", 0.0, 0.0, inv_flag, dec_flag ) )
    {
        printf( " ... unable to transform points between pixel/line and PCS space\n" );
        return;
    }

    GTIFReportACorner( gtif, defn, fp_out, "Lower Left", 0.0, ysize, 
                       inv_flag, dec_flag );
    GTIFReportACorner( gtif, defn, fp_out, "Upper Right", xsize, 0.0,
                       inv_flag, dec_flag );
    GTIFReportACorner( gtif, defn, fp_out, "Lower Right", xsize, ysize,
                       inv_flag, dec_flag );
    GTIFReportACorner( gtif, defn, fp_out, "Center", xsize/2.0, ysize/2.0,
                       inv_flag, dec_flag );
}

/*
 * Write the defining matrix for this file to a .tfw file with the same
 * basename.
 */

static void WriteTFWFile( GTIF * gtif, const char * tif_filename )

{
    char	tfw_filename[1024];
    int		i;
    double	adfCoeff[6], x, y;
    FILE	*fp;

    /*
     * form .tfw filename
     */
    strncpy( tfw_filename, tif_filename, sizeof(tfw_filename)-4 );
    for( i = strlen(tfw_filename)-1; i > 0; i-- )
    {
        if( tfw_filename[i] == '.' )
        {
            strcpy( tfw_filename + i, ".tfw" );
            break;
        }
    }

    if( i <= 0 )
        strcat( tfw_filename, ".tfw" );

    /*
     * Compute the coefficients.
     */
    x = 0.5;
    y = 0.5;
    if( !GTIFImageToPCS( gtif, &x, &y ) )
    {
        fprintf( stderr, "Unable to translate image to PCS coordinates.\n" );
        return;
    }
    adfCoeff[4] = x;
    adfCoeff[5] = y;

    x = 1.5;
    y = 0.5;
    if( !GTIFImageToPCS( gtif, &x, &y ) )
        return;
    adfCoeff[0] = x - adfCoeff[4];
    adfCoeff[1] = y - adfCoeff[5];

    x = 0.5;
    y = 1.5;
    if( !GTIFImageToPCS( gtif, &x, &y ) )
        return;
    adfCoeff[2] = x - adfCoeff[4];
    adfCoeff[3] = y - adfCoeff[5];

    /*
     * Write out the coefficients.
     */

    fp = fopen( tfw_filename, "wt" );
    if( fp == NULL )
    {
        perror( "fopen" );
        fprintf( stderr, "Failed to open TFW file `%s'\n", tfw_filename );
        return;
    }

    for( i = 0; i < 6; i++ )
        fprintf( fp, "%24.10f\n", adfCoeff[i] );

    fclose( fp );

    fprintf( stderr, "World file written to '%s'.\n", tfw_filename); 
}

/************************************************************************/
/*                         st_setup_test_info()                         */
/*                                                                      */
/*      Setup a ST_TIFF structure for a simulated TIFF file.  This      */
/*      is just a hack to test the ST_ interface.                       */
/************************************************************************/

static TIFF *st_setup_test_info()

{
    ST_TIFF *st;
    double dbl_data[100];
    unsigned short  shrt_data[] = 
        { 1,1,0,6,1024,0,1,1,1025,0,1,1,1026,34737,17,0,2052,0,1,9001,2054,0,1,9102,3072,0,1,26711 };
    char *ascii_data = "UTM    11 S E000|";

    st = ST_Create();

    dbl_data[0] = 60;
    dbl_data[1] = 60;
    dbl_data[2] = 0;
    
    ST_SetKey( st, 33550, 3, STT_DOUBLE, dbl_data );

    dbl_data[0] = 0;
    dbl_data[1] = 0;
    dbl_data[2] = 0;
    dbl_data[3] = 440720;
    dbl_data[4] = 3751320;
    dbl_data[5] = 0;
    ST_SetKey( st, 33922, 6, STT_DOUBLE, dbl_data );

    ST_SetKey( st, 34735, sizeof(shrt_data)/2, STT_SHORT, shrt_data );
    ST_SetKey( st, 34737, strlen(ascii_data)+1, STT_ASCII, ascii_data );
    
    return (TIFF *) st;
}
