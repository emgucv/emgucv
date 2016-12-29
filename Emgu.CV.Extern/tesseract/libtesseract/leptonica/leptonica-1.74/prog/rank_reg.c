/*====================================================================*
 -  Copyright (C) 2001 Leptonica.  All rights reserved.
 -
 -  Redistribution and use in source and binary forms, with or without
 -  modification, are permitted provided that the following conditions
 -  are met:
 -  1. Redistributions of source code must retain the above copyright
 -     notice, this list of conditions and the following disclaimer.
 -  2. Redistributions in binary form must reproduce the above
 -     copyright notice, this list of conditions and the following
 -     disclaimer in the documentation and/or other materials
 -     provided with the distribution.
 -
 -  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
 -  ``AS IS'' AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
 -  LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
 -  A PARTICULAR PURPOSE ARE DISCLAIMED.  IN NO EVENT SHALL ANY
 -  CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
 -  EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
 -  PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
 -  PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY
 -  OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
 -  NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
 -  SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 *====================================================================*/

/*
 * rank_reg.c
 *
 *   Tests grayscale rank functions:
 *      (1) pixRankFilterGray()
 *      (2) pixScaleGrayMinMax()
 *      (3) pixScaleGrayRankCascade()
 */

#include "allheaders.h"

static const l_int32   SIZE = 20;


int main(int    argc,
         char **argv)
{
l_int32       i, j, w, h, same;
l_float32     t, t1, t2;
GPLOT        *gplot;
NUMA         *nax, *nay1, *nay2;
PIX          *pixs, *pixd, *pix1, *pix2, *pix3, *pix4;
PIXA         *pixa;
static char   mainName[] = "rank_reg";

    if (argc != 1)
        return ERROR_INT(" Syntax: rank_reg", mainName, 1);

    lept_mkdir("lept/rank");

    if ((pixs = pixRead("lucasta.150.jpg")) == NULL)
        return ERROR_INT("pixs not made", mainName, 1);
    pixGetDimensions(pixs, &w, &h, NULL);

    startTimer();
    pixd = pixRankFilterGray(pixs, 15, 15, 0.4);
    t = stopTimer();
    fprintf(stderr, "Time =  %7.3f sec\n", t);
    fprintf(stderr, "MPix/sec: %7.3f\n", 0.000001 * w * h / t);
    pixDisplay(pixs, 0, 200);
    pixDisplay(pixd, 600, 200);
    pixWrite("/tmp/lept/rank/filter.png", pixd, IFF_PNG);
    pixDestroy(&pixd);


    /* ---------- Compare grayscale morph with rank operator ---------- */
        /* Get results for dilation */
    startTimer();
    pix1 = pixDilateGray(pixs, 15, 15);
    t = stopTimer();
    fprintf(stderr, "Dilation time =  %7.3f sec\n", t);

        /* Get results for erosion */
    pix2 = pixErodeGray(pixs, 15, 15);

        /* Get results using the rank filter for rank = 0.0 and 1.0.
         * Don't use 0.0 or 1.0, because those are dispatched
         * automatically to erosion and dilation! */
    pix3 = pixRankFilterGray(pixs, 15, 15, 0.0001);
    pix4 = pixRankFilterGray(pixs, 15, 15, 0.9999);

        /* Compare */
    pixEqual(pix1, pix4, &same);
    if (same)
        fprintf(stderr, "Correct: dilation results same as rank 1.0\n");
    else
        fprintf(stderr, "Error: dilation results differ from rank 1.0\n");
    pixEqual(pix2, pix3, &same);
    if (same)
        fprintf(stderr, "Correct: erosion results same as rank 0.0\n");
    else
        fprintf(stderr, "Error: erosion results differ from rank 0.0\n");
    pixDestroy(&pix1);
    pixDestroy(&pix2);
    pixDestroy(&pix3);
    pixDestroy(&pix4);


    /* ------------- Timing and filter size experiments --------- */
    fprintf(stderr, "\n----------------------------------------\n");
    fprintf(stderr, "The next part takes several seconds\n");
    fprintf(stderr, "----------------------------------------\n\n");

    nax = numaMakeSequence(1, 1, SIZE);
    nay1 = numaCreate(SIZE);
    nay2 = numaCreate(SIZE);
    gplot = gplotCreate("/tmp/lept/rank/plots", GPLOT_PNG,
                        "sec/MPix vs filter size", "size", "time");
    pixa = pixaCreate(20);
    for (i = 1; i <= SIZE; i++) {
        t1 = t2 = 0.0;
        for (j = 0; j < 5; j++) {
            startTimer();
            pix1 = pixRankFilterGray(pixs, i, SIZE + 1, 0.5);
            t1 += stopTimer();
            pixDestroy(&pix1);
            startTimer();
            pix1 = pixRankFilterGray(pixs, SIZE + 1, i, 0.5);
            t2 += stopTimer();
            if (j == 0)
                pixaAddPix(pixa, pix1, L_CLONE);
            pixDestroy(&pix1);
        }
        numaAddNumber(nay1, 1000000. * t1 / (5. * w * h));
        numaAddNumber(nay2, 1000000. * t2 / (5. * w * h));
    }
    gplotAddPlot(gplot, nax, nay1, GPLOT_LINES, "vertical");
    gplotAddPlot(gplot, nax, nay2, GPLOT_LINES, "horizontal");
    gplotMakeOutput(gplot);
    gplotDestroy(&gplot);
    pix1 = pixRead("/tmp/lept/rank/plots.png");
    pixDisplay(pix1, 100, 100);
    pixDestroy(&pix1);

        /* Display tiled */
    pixd = pixaDisplayTiledAndScaled(pixa, 8, 250, 5, 0, 25, 2);
    pixDisplay(pixd, 100, 600);
    pixWrite("/tmp/lept/rank/tiles.jpg", pixd, IFF_JFIF_JPEG);
    pixDestroy(&pixd);
    pixaDestroy(&pixa);
    pixDestroy(&pixs);

    /* ------------------     Gray tests    ------------------ */
    pixs = pixRead("test8.jpg");
    pixa = pixaCreate(4);
    for (i = 1; i <= 4; i++) {
        pix1 = pixScaleGrayRank2(pixs, i);
        pixaAddPix(pixa, pix1, L_INSERT);
    }
    pixd = pixaDisplayTiledInRows(pixa, 8, 1500, 1.0, 0, 20, 2);
    pixDisplay(pixd, 100, 100);
    pixWrite("/tmp/lept/rank/grayrank2.jpg", pixd, IFF_JFIF_JPEG);
    pixDestroy(&pixs);
    pixDestroy(&pixd);
    pixaDestroy(&pixa);

    pixs = pixRead("test24.jpg");
    pix1 = pixConvertRGBToLuminance(pixs);
    pix2 = pixScale(pix1, 1.5, 1.5);
    pixa = pixaCreate(5);
    for (i = 1; i <= 4; i++) {
        for (j = 1; j <= 4; j++) {
            pix3 = pixScaleGrayRankCascade(pix2, i, j, 0, 0);
            pixaAddPix(pixa, pix3, L_INSERT);
        }
    }
    pixd = pixaDisplayTiledInRows(pixa, 8, 1500, 0.7, 0, 20, 2);
    pixDisplay(pixd, 100, 700);
    pixWrite("/tmp/lept/rank/graycascade.jpg", pixd, IFF_JFIF_JPEG);
    pixDestroy(&pixs);
    pixDestroy(&pix1);
    pixDestroy(&pix2);
    pixDestroy(&pixd);
    pixaDestroy(&pixa);
    return 0;
}


