//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once

#ifndef EMGU_DOUBLE_OPS_H
#define EMGU_DOUBLE_OPS_H

#include "sse.h"
namespace doubleOps
{
   inline void weightedSum(const double* d1, const double* d2, int elementCount, double w1, double w2, double* r)
   {
      double* end = r + elementCount;
#if EMGU_SSE2
      __m128d f = _mm_set_pd(w1, w2);

#if EMGU_SSE4_1
      if(simdSSE4_1) 
      {
         while(r < end) _mm_store_sd(r++, _mm_dp_pd(_mm_set_pd(*d1++, *d2++), f, 0x33));
         return;
      }
#endif 
      while (r < end)
      {
         __m128d v = _mm_mul_pd(_mm_set_pd(*d1++, *d2++), f);
         _mm_store_sd(r++, _mm_add_pd(v, _mm_shuffle_pd(v, v, 1)));
      }
#else
      while( r < end) 
         *r++ = *d1++ * w1 + *d2++ * w2;
#endif
   }

   inline void mulS(const double* d, double scale, int length, double* result)
   {
      const double* current = d;
      const double* end = d+length;
#if EMGU_SSE2
      __m128d _scale = _mm_set1_pd(scale);
      for(const double* stop = d + (length & -2); current < stop; current+=2, result+=2)
         _mm_storeu_pd(result,_mm_mul_pd(_mm_loadu_pd(current), _scale));
#endif
      while (current < end)
      {
         *result++ = (*current++) * scale;
      }
   }

   inline void add(const double* d1, const double* d2, int length, double* result)
   {
      const double* current1 = d1;
      const double* current2 = d2;
      const double* end1 = d1 + length;
#if EMGU_SSE2
      for(const double* stop1 = d1 + (length &-2); current1 < stop1; current1+=2, current2 +=2, result +=2)
         _mm_storeu_pd(result, _mm_add_pd(_mm_loadu_pd(current1), _mm_loadu_pd(current2)));
#endif
      while(current1 < end1)
      {
         *result++ = (*current1++) + (*current2++);
      }
   }

   inline bool containsNaN(const double* d, int length)
   {
      for (int i = 0; i < length; i++)
         if (d[i] != d[i]) return true;
      return false;
   }
}
#endif