//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#include "photo_edit.h"

#define BLENDING_WITH_LOOKUP

#ifdef BLENDING_WITH_LOOKUP
uchar blendingLookupTable[256*256];
bool blendingLookupTableInitialized;

void initializeBlendingLookupTable()
{
   float scalar = 1.0f/255.0f;
   for (int color = 0; color < 256; color++)
      for (int alpha = 0; alpha < 128; alpha++)
      {
         int idx0 = (color << 8 | alpha);
         int idx1 = (color << 8 | (255 - alpha));
         uchar c0 = (uchar) (color * alpha * scalar);
         uchar c1 = color - c0;
         blendingLookupTable[idx0] = c0; 
         blendingLookupTable[idx1] = c1;
      }
      blendingLookupTableInitialized = true;
}
#endif

void cvBlendBgraOverBgr(IplImage* bgra, IplImage* bgr, IplImage* dstBgr)
{
   CV_Assert(bgra->nChannels == 4 && bgr->nChannels == 3 && dstBgr->nChannels == 3);
   CV_Assert(bgra->width == bgr->width && bgra->height == bgr->height && bgr->width == dstBgr->width && bgr->height == dstBgr->height);

#ifdef BLENDING_WITH_LOOKUP
   if (!blendingLookupTableInitialized)
   {
      initializeBlendingLookupTable();
   }
#else
   float weight1, weight2;
   float scalar = 1.0f/255.0f;
#endif

   uchar* bgraPtr = (uchar*) bgra->imageData;
   uchar* bgrPtr = (uchar*) bgr->imageData;
   uchar* dstPtr = (uchar*) dstBgr->imageData;

   uchar* bgraRow = 0;
   uchar* bgrRow = 0;
   uchar* dstRow = 0;
   uchar alpha = 0;

   float val = 0;
   for (int i = 0; i < bgra->height; i++, bgraPtr += bgra->widthStep, bgrPtr += bgr->widthStep, dstPtr += dstBgr->widthStep)
   {
      bgraRow = bgraPtr;
      bgrRow = bgrPtr;
      dstRow = dstPtr;
      for (int j = 0; j < bgra->width; j++)
      {
#ifdef BLENDING_WITH_LOOKUP
         uchar alpha = *(bgraRow+3);
         uchar notAlpha = ~(alpha);  //255 - alpha;
         *dstRow++ = blendingLookupTable[(*bgraRow++) << 8 | alpha ] + blendingLookupTable[(*bgrRow++) << 8 | notAlpha ];
         *dstRow++ = blendingLookupTable[(*bgraRow++) << 8 | alpha ] + blendingLookupTable[(*bgrRow++) << 8 | notAlpha ];
         *dstRow++ = blendingLookupTable[(*bgraRow++) << 8 | alpha ] + blendingLookupTable[(*bgrRow++) << 8 | notAlpha ];
         bgraRow++;
#else
         alpha = *(bgraRow + 3);
         weight1 = alpha * scalar;
         weight2 = 1.0f - weight1;

         val = weight1 * (*bgraRow++) + weight2 * (*bgrRow++); //b value
         *dstRow++ = cv::saturate_cast<uchar>(val);
         val = weight1 * (*bgraRow++) + weight2 * (*bgrRow++); //g value
         *dstRow++ = cv::saturate_cast<uchar>(val);
         val = weight1 * (*bgraRow++) + weight2 * (*bgrRow++); //r value
         *dstRow++ = cv::saturate_cast<uchar>(val);
         bgraRow++;
#endif
      }
   }
}

void cvBlendBgrOverBgrWithAlpha(IplImage* bgrTop, IplImage* alpha, IplImage* bgr, IplImage* dstBgr)
{
   CV_Assert(bgrTop->nChannels == 3 && alpha->nChannels == 1 && bgr->nChannels == 3 && dstBgr->nChannels == 3);
   CV_Assert(bgrTop->width == alpha->width && bgrTop->height == alpha->height && bgrTop->width == bgr->width && bgrTop->height == bgr->height  && bgr->width == dstBgr->width && bgr->height == dstBgr->height);

#ifdef BLENDING_WITH_LOOKUP
   if (!blendingLookupTableInitialized)
   {
      initializeBlendingLookupTable();
   }
#else
   float weight1, weight2;
   float scalar = 1.0f/255.0f;
#endif

   uchar* bgrTopPtr = (uchar*) bgrTop->imageData;
   uchar* alphaPtr = (uchar*) alpha->imageData;
   uchar* bgrPtr = (uchar*) bgr->imageData;
   uchar* dstPtr = (uchar*) dstBgr->imageData;
   
   uchar* bgrTopRow = 0;
   uchar* alphaRow = 0;
   uchar* bgrRow = 0;
   uchar* dstRow = 0;

   float val = 0;
   for (int i = 0; i < bgrTop->height; i++, bgrTopPtr += bgrTop->widthStep, alphaPtr += alpha->widthStep, bgrPtr += bgr->widthStep, dstPtr += dstBgr->widthStep)
   {
      bgrTopRow = bgrTopPtr;
      alphaRow = alphaPtr;
      bgrRow = bgrPtr;
      dstRow = dstPtr;
      for (int j = 0; j < bgrTop->width; j++)
      {
#ifdef BLENDING_WITH_LOOKUP
         uchar notAlpha = ~(*alphaRow);  //255 - *alphaRow;
         *dstRow++ = blendingLookupTable[(*bgrTopRow++) << 8 | *alphaRow ] + blendingLookupTable[(*bgrRow++) << 8 | notAlpha ];
         *dstRow++ = blendingLookupTable[(*bgrTopRow++) << 8 | *alphaRow ] + blendingLookupTable[(*bgrRow++) << 8 | notAlpha ];
         *dstRow++ = blendingLookupTable[(*bgrTopRow++) << 8 | *alphaRow++ ] + blendingLookupTable[(*bgrRow++) << 8 | notAlpha ];

#else
         weight1 = (*alphaRow++) * scalar;
         weight2 = 1.0f - weight1;

         val = weight1 * (*bgrTopRow++) + weight2 * (*bgrRow++); //b value
         *dstRow++ = cv::saturate_cast<uchar>(val);
         val = weight1 * (*bgrTopRow++) + weight2 * (*bgrRow++); //g value
         *dstRow++ = cv::saturate_cast<uchar>(val);
         val = weight1 * (*bgrTopRow++) + weight2 * (*bgrRow++); //r value
         *dstRow++ = cv::saturate_cast<uchar>(val);
#endif    
      }
   }
}

void cvVignetteMaskCreate(IplImage* alphaMask, float centerWidth, float centerHeight, float fullColorRadius, float halfPowerRadius)
{
   CV_Assert(alphaMask->nChannels == 1);
   float centerW = alphaMask->width * centerWidth;
   float centerH = alphaMask->height * centerHeight;
   float widthFraction = 1.0f /  (float) alphaMask->width;
   float heightFraction = 1.0f / (float) alphaMask->height;
   float wDist = 0;
   float hDist = 0;
   float hDistSqr = 0;
   float dist = 0;
   float distSqr = 0;
   float fullColorRadiusSqr = fullColorRadius * fullColorRadius;
   uchar* maskRowPtr = (uchar*) alphaMask->imageData;
   uchar* maskRow = 0;

   float logHalf = log(0.5f);
   float distScale = logHalf / halfPowerRadius;
   float log255 = log(255.0f);
   float const1 = log255 - fullColorRadius *distScale;
   for (int h = 0; h < alphaMask->height; h++, maskRowPtr += alphaMask->widthStep)
   {
      maskRow = maskRowPtr;

      hDist = (h - centerH) * heightFraction;
      hDistSqr = hDist * hDist;

      for (int w = 0; w < alphaMask->width; w++)
      {
         wDist = (w - centerW) * widthFraction;
         distSqr = wDist * wDist + hDistSqr;
         
         if (distSqr <= fullColorRadiusSqr)
         {
            *maskRow++ = 0;
         }
         else 
         {
            //dist = exp( (sqrt(distSqr) - fullColorRadius) * distScale) * 255;
            //dist = exp( (sqrt(distSqr) - fullColorRadius) * distScale + log255 );
            //dist = exp( sqrt(distSqr) * distScale - fullColorRadius * distScale + log255);
            //dist = exp( sqrt(distSqr) * distScale + (log255 - fullColorRadius * distScale));
            dist = exp( sqrt(distSqr) * distScale + const1);
            *maskRow++ = ~ (cv::saturate_cast<uchar>(dist));
         }         
      }
   }
}

void cvCovertBgr2Bgr_Gray(IplImage* bgr, IplImage* gray_3channel)
{
   CV_Assert(bgr->nChannels == 3 && gray_3channel->nChannels == 3);
   CV_Assert(bgr->width == gray_3channel->width && bgr->height == gray_3channel->height);

   uchar* bgrPtr = (uchar*) bgr->imageData;
   uchar* dstPtr = (uchar*) gray_3channel->imageData;

   uchar* bgrRow = 0;
   uchar* dstRow = 0;
   float coeffR = 0.299f, coeffG = 0.587f, coeffB = 0.114f;

   for (int i = 0; i < bgr->height; i++, bgrPtr += bgr->widthStep, dstPtr += gray_3channel->widthStep)
   {
      bgrRow = bgrPtr;
      dstRow = dstPtr;
      for (int j = 0; j < bgr->width; j++)
      {
         uchar b = *bgrRow++;
         uchar g = *bgrRow++;
         uchar r = *bgrRow++;
         uchar gray = cv::saturate_cast<uchar>(coeffR * r + coeffG * g + coeffB * b);
         *dstRow++ = gray;
         *dstRow++ = gray;
         *dstRow++ = gray;
      }
   }
}

//returns hue that is in the range of [0, 60)
float bgr2Hue(uchar b, uchar g, uchar r)
{
   uchar min = b < g ? b : g;
   min = min < r ? min : r;

   uchar max = b > g ? b : g;
   max = max > r ? max : r;
   uchar delta = max - min;
   if (delta == 0)
   {
      //r == g == b == 0
      return -1.0f;
   }

   float hue;
   float deltaF = (float) delta;
   if( r == max )
   {
      // between yellow & magenta
      hue = ( g - b ) / deltaF;	
      if (g == min)
         hue += 6.0f;
   }
   else if( g == max )
   {
      hue = 2.0f + ( b - r ) / deltaF; // between cyan & yellow
   }
   else
      hue = 4.0f + ( r - g ) / deltaF;	// between magenta & cyan

	
   return hue * 60;			
}

bool hueInRange(float hue, float start, float end)
{
   if (end > start)
   {
      return hue >= start && hue <= end;
   } else
   {
      return hue >= start || hue <= end;
   }
}

float normal_pdf(float x, float mean, float sigma)
{
    static const float inv_sqrt_2pi = 0.3989422804014327f;
    float a = (x - mean) / sigma;
    return inv_sqrt_2pi / sigma * std::exp(-0.5f * a * a);
}


void cvSelectiveColor(IplImage* bgr, IplImage* result, float start, float end)
{
   CV_Assert(bgr->nChannels == 3 && result->nChannels == 3);
   CV_Assert(bgr->width == result->width && bgr->height == result->height);

   uchar* bgrPtr = (uchar*) bgr->imageData;
   uchar* dstPtr = (uchar*) result->imageData;

   uchar* bgrRow = 0;
   uchar* dstRow = 0;
   float coeffR = 0.299f, coeffG = 0.587f, coeffB = 0.114f;

   /*
   //scale start and end value such that it matches the scale of the return value of bgr2Hue
   start = start / 60.0f;
   end = end / 60.0f;
   */

   for (int i = 0; i < bgr->height; i++, bgrPtr += bgr->widthStep, dstPtr += result->widthStep)
   {
      bgrRow = bgrPtr;
      dstRow = dstPtr;
      for (int j = 0; j < bgr->width; j++)
      {
         uchar b = *bgrRow++;
         uchar g = *bgrRow++;
         uchar r = *bgrRow++;
         
         float hue = bgr2Hue(b, g, r);
         if (hue == -1.0f || hueInRange(hue, start, end))
         {
            //it is already gray color or it is the color in range
            //just copy over
            *dstRow++ = b;
            *dstRow++ = g;
            *dstRow++ = r;
         } else
         {
            uchar gray = cv::saturate_cast<uchar>(coeffR * r + coeffG * g + coeffB * b);
            *dstRow++ = gray;
            *dstRow++ = gray;
            *dstRow++ = gray;
         }
      }
   }
}

