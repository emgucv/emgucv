#ifndef EMGU_SSE_H
#define EMGU_SSE_H

#if __EMGU_ENABLE_SSE__
   #if defined __SSE2__ || _MSC_VER >= 1300
       #include "emmintrin.h"
       #define EMGU_SSE 1
       #define EMGU_SSE2 1
       #if defined __SSE3__ || _MSC_VER >= 1400
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
   inline double _dot_product(__m128d v0, __m128d v1)
   {
   #if EMGU_SSE4_1
      return _mm_dp_pd(v0, v1, 0x31).m128d_f64[0];
   #else
      __m128d v = _mm_mul_pd(v0, v1);
      return v.m128d_f64[1] + v.m128d_f64[0];
   #endif
   }

   inline double _cross_product(__m128d v0, __m128d v1)
   {
      return v0.m128d_f64[1] * v1.m128d_f64[0] - v0.m128d_f64[0] * v1.m128d_f64[1];
   }
   #endif

#endif
#endif