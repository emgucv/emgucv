/******************************************************************************
 * $Id: cpl_strtod.cpp 21653 2011-02-08 20:01:32Z warmerdam $
 *
 * Project:  CPL - Common Portability Library
 * Purpose:  Functions to convert ASCII string to floating point number.
 * Author:   Andrey Kiselev, dron@ak4719.spb.edu.
 *           Frank Warmerdam, warmerdam@pobox.com
 *
 * This file is derived from GDAL's port/cpl_strtod.cpp. 
 *
 ******************************************************************************
 * Copyright (c) 2006, Andrey Kiselev
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
 ****************************************************************************/

#include <locale.h>
#include <errno.h>
#include <stdlib.h>
#include "cpl_serv.h"

#ifndef NAN
#  ifdef HUGE_VAL
#    define NAN (HUGE_VAL * 0.0)
#  else

static float CPLNaN(void)
{
    float fNan;
    int nNan = 0x7FC00000;
    memcpy(&fNan, &nNan, 4);
    return fNan;
}

#    define NAN CPLNan()
#  endif
#endif

/************************************************************************/
/*                     _ReplacePointByLocalePoint()                     */
/************************************************************************/

static void _ReplacePointByLocalePoint(char* pszNumber, char point)
{
#if defined(WIN32CE) || defined(__ANDROID__)
    static char byPoint = 0;
    if (byPoint == 0)
    {
        char szBuf[16];
        sprintf(szBuf, "%.1f", 1.0);
        byPoint = szBuf[1];
    }
    if (point != byPoint)
    {
        int     i = 0;

        while ( pszNumber[i] )
        {
            if ( pszNumber[i] == point )
            {
                pszNumber[i] = byPoint;
                break;
            }
            i++;
        }
    }
#else
    struct lconv *poLconv = localeconv();
    if ( poLconv
         && poLconv->decimal_point
         && strlen(poLconv->decimal_point) > 0 )
    {
        int     i = 0;
        char    byPoint = poLconv->decimal_point[0];

        if (point != byPoint)
        {
            while ( pszNumber[i] )
            {
                if ( pszNumber[i] == point )
                {
                    pszNumber[i] = byPoint;
                    break;
                }
                i++;
            }
        }
    }
#endif
}

/************************************************************************/
/*                            _StrtodDelim()                            */
/************************************************************************/

static double _StrtodDelim(const char *nptr, char **endptr, char point)
{
   if (EQUAL(nptr,"nan") || EQUAL(nptr, "1.#QNAN") ||
       EQUAL(nptr, "-1.#QNAN") || EQUAL(nptr, "-1.#IND"))
       return NAN;

/* -------------------------------------------------------------------- */
/*  We are implementing a simple method here: copy the input string     */
/*  into the temporary buffer, replace the specified decimal delimiter  */
/*  with the one, taken from locale settings and use standard strtod()  */
/*  on that buffer.                                                     */
/* -------------------------------------------------------------------- */
    char        *pszNumber = CPLStrdup( nptr );
    double      dfValue;
    int         nError;

    _ReplacePointByLocalePoint(pszNumber, point);

    dfValue = strtod( pszNumber, endptr );
    nError = errno;

    if ( endptr )
        *endptr = (char *)nptr + (*endptr - pszNumber);

    CPLFree( pszNumber );

    errno = nError;
    return dfValue;
}

/************************************************************************/
/*                             GTIFStrtod()                             */
/************************************************************************/

double GTIFStrtod(const char *nptr, char **endptr)
{
    return _StrtodDelim(nptr, endptr, '.');
}

/************************************************************************/
/*                              GTIFAtof()                              */
/************************************************************************/

double GTIFAtof(const char *nptr)
{
    return GTIFStrtod(nptr, 0);
}

