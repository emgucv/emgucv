/******************************************************************************
 * $Id: csv2html.c 1668 2009-10-09 16:17:07Z hobu $
 *
 * Project:  libgeotiff
 * Purpose:  CGI BIN to view CSV files on the web.
 * Author:   Frank Warmerdam, warmerda@home.com
 *
 ******************************************************************************
 * Copyright (c) 1999, Frank Warmerdam
 *
 * Permission is hereby granted, free of charge, to any person obtaining a
 * copy of this software and associated documentation files (the "Software"),
 * to deal in the Software without restriction, including without limitation
 * the rights to use, copy, modify, merge, publish, distribute, sublicense,
 * and/or sell copies of the Software, and to permit persons to whom the
 * Software is furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included
 * in all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
 * OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
 * THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
 * DEALINGS IN THE SOFTWARE.
 ******************************************************************************
 *
 * $Log$
 * Revision 1.3  2000/12/28 23:26:56  warmerda
 * Changed to default csv path.
 *
 * Revision 1.2  1999/07/28 22:11:31  warmerda
 * lots more changes
 *
 * Revision 1.1  1999/06/25 05:50:24  warmerda
 * New
 *
 */

/* #include "cpl_csv.h" */
#include "cpl_serv.h"

static void
CSV2HTML( const char * pszFilename, int nColumns, int * panColumns, char**,
          int );

/************************************************************************/
/*                                main()                                */
/************************************************************************/
int main( int nArgc, char ** papszArgv )

{
    int		nColumns = 0;
    int		*panColumnList = NULL;
    const char *pszTable = "horiz_cs";
    const char *pszTablePath = "/usr/local/share/epsg_csv";
    char	**papszOptions = NULL;
    char	szFilename[1024];
    int		i, bSingletons = FALSE;

    printf( "Content-type: text/html\n\n" );
    
/* -------------------------------------------------------------------- */
/*      Parse the PATH_INFO.                                            */
/* -------------------------------------------------------------------- */
    if( getenv( "PATH_INFO" ) != NULL )
    {
        papszOptions = CSLTokenizeStringComplex( getenv("PATH_INFO") + 1,
                                                 "/", TRUE, TRUE );
    }

/* -------------------------------------------------------------------- */
/*      Add commandline switch to the option list.                      */
/* -------------------------------------------------------------------- */
    for( i = 1; i < nArgc; i++ )
        papszOptions = CSLAddString( papszOptions, papszArgv[i] );

/* -------------------------------------------------------------------- */
/*      Process program options.                                        */
/* -------------------------------------------------------------------- */
    for( i = 0; papszOptions != NULL && papszOptions[i] != NULL; i++ )
    {
        if( EQUALN(papszOptions[i],"FIELDS=",7) )
        {
            char	**papszList;
            int		j;

            papszList = CSLTokenizeStringComplex( papszOptions[i]+7, ",",
                                                TRUE, TRUE );
            nColumns = CSLCount( papszList );
            panColumnList = (int *) CPLRealloc(panColumnList,
                                               sizeof(int) * nColumns);

            for( j = 0; j < nColumns; j++ )
                panColumnList[j] = atoi(papszList[j]);

            CSLDestroy( papszList );
        }

        else if( EQUALN(papszOptions[i],"TABLE=",6) )
        {
            pszTable = papszOptions[i] + 6;
        }

        else if( EQUALN(papszOptions[i],"CODE=",5) )
        {
            bSingletons = TRUE;
        }

        else if( EQUALN(papszOptions[i],"SINGLETON",6) )
        {
            bSingletons = TRUE;
        }
    }

/* -------------------------------------------------------------------- */
/*      Derive the full filename.                                       */
/* -------------------------------------------------------------------- */
    sprintf( szFilename, "%s/%s.csv", pszTablePath, pszTable );

/* -------------------------------------------------------------------- */
/*      Call function to translate to HTML.                             */
/* -------------------------------------------------------------------- */
    CSV2HTML( szFilename, nColumns, panColumnList, papszOptions,
              bSingletons );

    return 0;
}

/************************************************************************/
/*                              CSV2HTML()                              */
/*                                                                      */
/*      Translate file to HTML.                                         */
/************************************************************************/

static void
CSV2HTML( const char * pszFilename, int nColumns, int * panColumns,
          char ** papszOptions, int bSingletons )

{
    FILE	*fp;
    char	**papszFields, **papszFieldNames;
    int		iCol, nColCount;

/* -------------------------------------------------------------------- */
/*      Open the source file.                                           */
/* -------------------------------------------------------------------- */
    fp = VSIFOpen( pszFilename, "rt" );

    if( fp == NULL )
    {
        perror( "fopen" );
        return;
    }

/* -------------------------------------------------------------------- */
/*      Read and emit the title line specially.                         */
/* -------------------------------------------------------------------- */
    papszFieldNames = CSVReadParseLine( fp );
    nColCount = CSLCount( papszFieldNames );

    if( nColumns == 0 )
    {
        nColumns = nColCount;
        panColumns = (int *) CPLMalloc(sizeof(int) * nColCount);
        for( iCol = 0; iCol < nColCount; iCol++ )
            panColumns[iCol] = iCol;
    }

    printf( "<table border>\n" );

    if( !bSingletons )
    {
        for( iCol = 0; iCol < nColumns; iCol++ )
        {
            if( panColumns[iCol] < nColCount )
            {
                printf( "<th>%s\n", papszFieldNames[panColumns[iCol]] );
            }
        }
        printf( "<tr>\n" );
    }

/* -------------------------------------------------------------------- */
/*      Read and emit normal records.                                   */
/* -------------------------------------------------------------------- */
    while( (papszFields = CSVReadParseLine( fp )) != NULL )
    {
        int	bDisplay=TRUE, i;
        
        nColCount = CSLCount( papszFields );

        for( i = 0; papszOptions != NULL && papszOptions[i] != NULL; i++ )
        {
            if( EQUALN(papszOptions[i],"CODE=",5) )
            {
                if( atoi(papszOptions[i]+5) != atoi(papszFields[0]) )
                    bDisplay = FALSE;
            }
            else if( EQUALN(papszOptions[i],"CODE<",5) )
            {
                if( atoi(papszOptions[i]+5) <= atoi(papszFields[0]) )
                    bDisplay = FALSE;
            }
            else if( EQUALN(papszOptions[i],"CODE>",5) )
            {
                if( atoi(papszOptions[i]+5) >= atoi(papszFields[0]) )
                    bDisplay = FALSE;
            }
            else if( EQUALN(papszOptions[i],"NAMEKEY=",8) )
            {
                if( strstr(papszFields[1],papszOptions[i]+8) == NULL )
                    bDisplay = FALSE;
            }
        }

        if( bDisplay )
        {
            for( iCol = 0; iCol < nColumns; iCol++ )
            {
                const char	*pszSubTable = NULL;
                const char  *pszFieldName;
                
                if( panColumns[iCol] < 0
                    || panColumns[iCol] >= nColCount )
                    continue;
                
                pszFieldName = papszFieldNames[panColumns[iCol]];

                if( bSingletons )
                {
                    printf( "<td>%s\n", pszFieldName );
                }
                

                if( EQUAL(pszFieldName,"PRIME_MERIDIAN_CODE") )
                    pszSubTable = "p_meridian";
                else if( EQUAL(pszFieldName,"GEOD_DATUM_CODE") )
                    pszSubTable = "geod_datum";
                else if( EQUAL(pszFieldName,"UOM_LENGTH_CODE") )
                    pszSubTable = "uom_length";
                else if( EQUAL(pszFieldName,"UOM_ANGLE_CODE") )
                    pszSubTable = "uom_angle";
                else if( EQUAL(pszFieldName,"SOURCE_GEOGCS_CODE") )
                    pszSubTable = "horiz_cs";
                else if( EQUAL(pszFieldName,"PROJECTION_TRF_CODE") )
                    pszSubTable = "trf_nonpolynomial";
                else if( EQUAL(pszFieldName,"ELLIPSOID_CODE") )
                    pszSubTable = "ellipsoid";
                else if( EQUAL(pszFieldName,"COORD_TRF_METHOD_CODE") )
                    pszSubTable = "trf_method";
                
                if( pszSubTable != NULL )
                    printf( "<td><a href="
                            "\"/cgi-bin/csv2html/TABLE=%s/CODE=%s/\">"
                            "%s</a>\n",
                            pszSubTable,papszFields[panColumns[iCol]],
                            papszFields[panColumns[iCol]] );
                else
                    printf( "<td>%s\n", papszFields[panColumns[iCol]] );

                if( bSingletons )
                    printf( "<tr>\n" );
            }

            if( !bSingletons )
                printf( "<tr>\n" );
        }

        CSLDestroy( papszFields );
    }

    printf( "</table>\n" );

    CSLDestroy( papszFieldNames );

    VSIFClose( fp );
}
