#include "weighted_sum.h"

void weightedSum(double* d1, double* d2, int elementCount, double w1, double w2, double* r)
{
#if EMGU_SSE2
   __m128d f = _mm_set_pd(w1, w2);
   double* end = r + elementCount;
   while(r < end)
      *r++ = _dot_product(_mm_set_pd(*d1++, *d2++), f);
#else
   for (int i = 0; i < elementCount; i++)
      *r++ = *d1++ * w1 + *d2++ * w2;
#endif
}