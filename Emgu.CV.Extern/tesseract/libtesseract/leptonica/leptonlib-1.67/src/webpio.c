/*====================================================================*
 -  Copyright (C) 2001 Leptonica.  All rights reserved.
 -  This software is distributed in the hope that it will be
 -  useful, but with NO WARRANTY OF ANY KIND.
 -  No author or distributor accepts responsibility to anyone for the
 -  consequences of using this software, or for whether it serves any
 -  particular purpose or works at all, unless he or she says so in
 -  writing.  Everyone is granted permission to copy, modify and
 -  redistribute this source code, for commercial or non-commercial
 -  purposes, with the following restrictions: (1) the origin of this
 -  source code must not be misrepresented; (2) modified versions must
 -  be plainly marked as such; and (3) this notice may not be removed
 -  or altered from any source or modified source distribution.
 -  Author: krish@google.com (krish Chaudhury)
 *====================================================================*/

/*
 *  webpio.c
 *
 *    Read WebP from file
 *          PIX             *pixReadStreamWebP()
 *          l_int32          readHeaderWebP()
 *
 *    Write WebP to file
 *          l_int32          pixWriteWebP()  [ special top level ]
 *          l_int32          pixWriteStreamWebP()
 *
 *    Write WebP to file with target psnr
 *          l_int32          pixWriteWebPwithTargetPSNR
 *
 */

#include <math.h>
#include "allheaders.h"

#ifdef HAVE_CONFIG_H
#include "config_auto.h"
#endif  /* HAVE_CONFIG_H */

/* --------------------------------------------*/
#if  HAVE_LIBWEBP   /* defined in environ.h */
/* --------------------------------------------*/

#include "webpimg.h"

/*---------------------------------------------------------------------*
 *                              Reading WebP                            *
 *---------------------------------------------------------------------*/

/*!
 *  pixReadStreamWebP()
 *
 *      Input:  stream corresponding to WebP image
 *      Return: pix (32 bpp), or null on error
 *
 *  Notes:
 *      (1) Use 'free', and not leptonica's 'FREE', for all heap data
 *          that is returned from the WebP library.
 */
PIX *
pixReadStreamWebP(FILE  *fp)
{
l_int32    w, h, wpl, ret, nbytes;
l_uint8   *filedata;
l_uint8   *Y = NULL;
l_uint8   *U = NULL;
l_uint8   *V = NULL;
l_uint32  *data;
PIX       *pix;

    PROCNAME("pixReadStreamWebP");

    if (!fp)
        return (PIX *)ERROR_PTR("fp not defined", procName, NULL);

        /* Read data from file and decode into Y,U,V arrays */
    rewind(fp);
    if ((filedata = arrayReadStream(fp, &nbytes)) == NULL)
        return (PIX *)ERROR_PTR("filedata not read", procName, NULL);
    ret = WebPDecode(filedata, nbytes, &Y, &U, &V, &w, &h);
    FREE(filedata);
    if (ret != webp_success) {
        if (Y) free(Y);
        return (PIX *)ERROR_PTR("WebP decode failed", procName, NULL);
    }

        /* Write from Y,U,V arrays to pix data */
    pix = pixCreate(w, h, 32);
    wpl = pixGetWpl(pix);
    data = pixGetData(pix);
    YUV420toRGBA(Y, U, V, wpl, w, h, data);

    if (Y) free(Y);
    return pix;
}


/*!
 *  readHeaderWebP()
 *
 *      Input:  filename
 *              &width (<return>)
 *              &height (<return>)
 *      Return: 0 if OK, 1 on error
 */
l_int32
readHeaderWebP(const char *filename,
               l_int32    *pwidth,
               l_int32    *pheight)
{
l_uint8  data[10];
FILE    *fp;

    PROCNAME("readHeaderWebP");

    if (!filename)
        return ERROR_INT("filename not defined", procName, 1);
    if (!pwidth || !pheight)
        return ERROR_INT("input ptr(s) not defined", procName, 1);
    if ((fp = fopenReadStream(filename)) == NULL)
        return ERROR_INT("image file not found", procName, 1);
    if (fread((char *)data, 1, 10, fp) != 10)
        return ERROR_INT("failed to read 10 bytes of file", procName, 1);
    WebPGetInfo(data, 10, pwidth, pheight);
    fclose(fp);
    return 0;
}


/*---------------------------------------------------------------------*
 *                             Writing WebP                             *
 *---------------------------------------------------------------------*/
/*!
 *  pixWriteWebP()
 *
 *      Input:  filename
 *              pix
 *              quantparam (quantization parameter), controls quality of
 *              generated WebP, smaller quantparam == better quality.
 *              Send -1 to get default value.
 *      Return: 0 if OK, 1 on error
 */
l_int32
pixWriteWebP(const char  *filename,
             PIX         *pix,
             l_int32      quantparam)
{
FILE  *fp;

    PROCNAME("pixWriteWebP");

    if (!pix)
        return ERROR_INT("pix not defined", procName, 1);
    if (!filename)
        return ERROR_INT("filename not defined", procName, 1);

    if ((fp = fopen(filename, "wb+")) == NULL)
        return ERROR_INT("stream not opened", procName, 1);

    if (pixWriteStreamWebP(fp, pix, quantparam) != 0) {
        fclose(fp);
        return ERROR_INT("pix not written to stream", procName, 1);
    }

    fclose(fp);
    return 0;
}


/*!
 *  pixWriteStreampWebP()
 *
 *      Input:  stream
 *              pix  (32 bpp)
 *              quantparam (quantization parameter; use -1 for default)
 *      Return: 0 if OK, 1 on error
 *
 *  Notes:
 *      (1) The @quantparam controls the quality of the generated WebP;
 *          a smaller quantparam gives better quality.  The following
 *          table shows a rough correspondence between @quantparam
 *          and the jpeg quality parameter:
 *
 *              quantparam               jpeg quality
 *              ----------               ------------
 *              20                         60
 *              15                         75
 *              12                         90
 */
l_int32
pixWriteStreamWebP(FILE    *fp,
                   PIX     *pixs,
                   l_int32  quantparam)
{
l_int32    w, h, d, wpl, uv_width, uv_height, nbytes, ret;
l_uint8   *Y = NULL;
l_uint8   *U = NULL;
l_uint8   *V = NULL;
l_uint8   *filedata = NULL;
l_uint32  *data;
PIX       *pix = NULL;

    PROCNAME("pixWriteStreamWebP");

    if (!fp)
        return ERROR_INT("stream not open", procName, 1);
    if (!pixs)
        return ERROR_INT("pixs not defined", procName, 1);
    if (quantparam <= 0) quantparam = 20;

    if ((pix = pixRemoveColormap(pixs, REMOVE_CMAP_TO_FULL_COLOR)) == NULL) {
        return ERROR_INT("cannot remove color map", procName, 1);
    }
    pixGetDimensions(pix, &w, &h, &d);
    wpl = pixGetWpl(pix);
    data = pixGetData(pix);
    if (d != 32 || w <= 0 || h <= 0 || wpl <= 0 || !data) {
        pixDestroy(&pix);
        return ERROR_INT("bad or empty input pix", procName, 1);
    }

        /* Read data into Y,U,V arrays */
    uv_width = (w + 1) >> 1;
    uv_height = (h + 1) >> 1;
    nbytes = w * h + 2 * uv_width * uv_height;
    if ((Y = (l_uint8 *)CALLOC(nbytes, sizeof(l_uint8))) == NULL) {
        pixDestroy(&pix);
        return ERROR_INT("YUV buffer alloc failed", procName, 1);
    }
    U = Y + w * h;
    V = U + uv_width * uv_height;
    RGBAToYUV420(data, wpl, w, h, Y, U, V);

        /* Encode Y,U,V and write data to file */
    ret = WebPEncode(Y, U, V, w, h, w, uv_width, uv_height, uv_width,
                     quantparam, &filedata, &nbytes, NULL);
    FREE(Y);
    if (ret != webp_success) {
        if (filedata) free(filedata);
        pixDestroy(&pix);
        return ERROR_INT("WebPEncode failed", procName, 1);
    }

    rewind(fp);
    if (fwrite(filedata, 1, nbytes, fp) != nbytes) {
        pixDestroy(&pix);
        return ERROR_INT("Write error", procName, 1);
    }
    free(filedata);
    pixDestroy(&pix);

    return 0;
}


/*!
 *  pixWriteWebPwithTargetPSNR()
 *
 *      Input:  filename
 *              pix  (32 bpp rgb)
 *              target_psnr (target psnr to control the quality [1 ... 99])
 *              pqp (<optional return> final qp value used to obtain
 *                   the target_psnr; can be null)
 *      Return: 0 if OK, 1 on error
 *
 *  Notes:
 *      (1) The parameter to control quality while encoding WebP is qp.
 *          This function does a line search over the qp values between
 *          MIN_QP and MAX_QP to achieve the target PSNR as closely as
 *          possible.
 */
l_int32
pixWriteWebPwithTargetPSNR(const char  *filename,
                           PIX         *pixs,
                           l_float64    target_psnr,
                           l_int32     *pqp)
{
l_uint8   *Y = NULL;
l_uint8   *U = NULL;
l_uint8   *V = NULL;
l_uint8   *filedata = NULL;
l_uint8   *tmp_filedata = NULL;
l_int32    MIN_QP = 10;  /* min allowed value of qp */
l_int32    MAX_QP = 63;  /* max allowed value of qp */
l_int32    w, h, d, wpl, uv_width, uv_height, nbytes, ret;
l_int32    qp, delta_qp, qp_test, accept;
l_int32    tmp_nbytes = 0;
l_uint32  *data;
l_float64  psnr, psnr_test;
FILE      *fp;
PIX       *pix = NULL;

    PROCNAME("pixWriteWebPwithTargetPSNR");

    if (!filename)
        return ERROR_INT("filename not defined", procName, 1);
    if (!pixs)
        return ERROR_INT("pixs not defined", procName, 1);
    if (target_psnr <= 0 || target_psnr >= 100)
        return ERROR_INT("Target psnr out of range", procName, 1);

    if ((pix = pixRemoveColormap(pixs, REMOVE_CMAP_TO_FULL_COLOR)) == NULL) {
        return ERROR_INT("cannot remove color map", procName, 1);
    }
    pixGetDimensions(pix, &w, &h, &d);
    wpl = pixGetWpl(pix);
    data = pixGetData(pix);
    if (d != 32 || w <= 0 || h <= 0 || wpl <= 0 || !data) {
        pixDestroy(&pix);
        return ERROR_INT("bad or empty input pix", procName, 1);
    }

        /* Set the initial value of the QP parameter.  In each iteration
         * it will then increase or decrease the QP value, based on
         * whether the achieved psnr is higher or lower than the target_psnr */
    qp = 30;

        /* Read data into Y,U,V arrays */
    uv_width = (w + 1) >> 1;
    uv_height = (h + 1) >> 1;
    nbytes = w * h + 2 * uv_width * uv_height;
    if ((Y = (l_uint8 *)CALLOC(nbytes, sizeof(l_uint8))) == NULL) {
        pixDestroy(&pix);
        return ERROR_INT("YUV buffer alloc failed", procName, 1);
    }
    U = Y + w * h;
    V = U + uv_width * uv_height;
    RGBAToYUV420(data, wpl, w, h, Y, U, V);

        /* Encode Y,U,V and write data to file */
    ret = WebPEncode(Y, U, V, w, h, w, uv_width, uv_height, uv_width, qp,
                     &filedata, &nbytes, &psnr);
    if (ret != webp_success) {
        FREE(Y);
        if (filedata) free(filedata);
        pixDestroy(&pix);
        return ERROR_INT("WebPEncode failed", procName, 1);
    }

        /* Rationale about the delta_qp being limited: we expect the optimal
         * qp to be not too far from target qp in practice. So instead of a full
         * dichotomy for the whole [MIN_QP, MAX_QP] range we cap |delta_qp|
         * to only explore quickly around the starting value and maximize the
         * return in investment. */
    delta_qp = (psnr > target_psnr) ? L_MAX((MAX_QP - qp) / 4, 1) :
        L_MIN((MIN_QP - qp) / 4, -1);

    while (delta_qp != 0) {
            /* Advance qp and clip to valid range */
        qp_test = L_MIN(L_MAX(qp + delta_qp, MIN_QP), MAX_QP);
            /* Re-adjust delta value after QP-clipping. */
        delta_qp = qp_test - qp;

        ret = WebPEncode(Y, U, V, w, h, w, uv_width, uv_height, uv_width,
                         qp_test, &tmp_filedata, &tmp_nbytes, &psnr_test);

        if (ret != webp_success) {
            FREE(Y);
            free(filedata);
            if (tmp_filedata) free(tmp_filedata);
            pixDestroy(&pix);
            return ERROR_INT("WebPEncode failed", procName, 1);
        }

            /* Accept or reject new settings */
        accept = (psnr_test > target_psnr) ^ (delta_qp < 0);
        if (accept) {
            free(filedata);
            filedata = tmp_filedata;
            nbytes = tmp_nbytes;
            qp = qp_test;
            psnr = psnr_test;
        }
        else {
            delta_qp /= 2;
            free(tmp_filedata);
        }
    }
    if (pqp) *pqp = qp;
    FREE(Y);

    if ((fp = fopen(filename, "wb+")) == NULL) {
        free(filedata);
        pixDestroy(&pix);
        return ERROR_INT("stream not opened", procName, 1);
    }
    ret = (fwrite(filedata, 1, nbytes, fp) != nbytes);
    fclose(fp);
    free(filedata);
    if (ret) {
        pixDestroy(&pix);
        return ERROR_INT("Write error", procName, 1);
    }

    pixDestroy(&pix);
    return 0;
}

/* --------------------------------------------*/
#endif  /* HAVE_LIBWEBP */
/* --------------------------------------------*/
