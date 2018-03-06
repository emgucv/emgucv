//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_SSE_H
#define EMGU_SSE_H

#include "opencv2/core/core_c.h"
#include "opencv2/core/core.hpp"

#if __EMGU_ENABLE_SSE__
   #if defined __SSE2__ || _MSC_VER >= 1300
       #include "emmintrin.h"
       #define EMGU_SSE 1
       #define EMGU_SSE2 1
       #if defined __SSE3__ || _MSC_VER >= 1500 //Visual Studio 2008
            #include "pmmintrin.h"
            #define EMGU_SSE3 1
            #if defined __SSE4_1__ || _MSC_VER >= 1500 //Visual Studio 2008
               #include "smmintrin.h"
               #define EMGU_SSE4_1 1
            #endif
       #endif
   #else
       #define EMGU_SSE 0
       #define EMGU_SSE2 0
       #define EMGU_SSE3 0
       #define EMGU_SSE4_1 0
   #endif

   #if EMGU_SSE2
   const bool simdSSE4_1 = cv::checkHardwareSupport(CV_CPU_SSE4_1);

   inline __m128d _dot_product(__m128d v0, __m128d v1)
   {
   #if EMGU_SSE4_1
      if(simdSSE4_1) return _mm_dp_pd(v0, v1, 0x33); 
   #endif 
      __m128d v = _mm_mul_pd(v0, v1);
      return _mm_add_pd(v, _mm_shuffle_pd(v, v, 1));
   }

   //returns a1: v0 x v1; a2: v1 x v0;
   inline __m128d _cross_product(__m128d v0, __m128d v1)
   {
      __m128d val = _mm_mul_pd(v0, _mm_shuffle_pd(v1, v1, 1));
      return _mm_sub_pd(val, _mm_shuffle_pd(val, val, 1));
   }
   #endif

#endif
#endif