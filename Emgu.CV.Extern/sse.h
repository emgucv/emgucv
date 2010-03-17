#if defined __SSE2__ || _MSC_VER >= 1300
    #include "emmintrin.h"
    #define CV_SSE 1
    #define CV_SSE2 1
    #if defined __SSE3__ || _MSC_VER >= 1400
        #include "pmmintrin.h"
        #define CV_SSE3 1
    #endif
#else
    #define CV_SSE 0
    #define CV_SSE2 0
    #define CV_SSE3 0
#endif


#if CV_SSE2
inline double _dot_product(__m128d v0, __m128d v1)
{
   __m128d v = _mm_mul_pd(v0, v1);
   return v.m128d_f64[1] + v.m128d_f64[0];
}

inline double _cross_product(__m128d v0, __m128d v1)
{
   return v0.m128d_f64[1] * v1.m128d_f64[0] - v0.m128d_f64[0] * v1.m128d_f64[1];
}
#endif