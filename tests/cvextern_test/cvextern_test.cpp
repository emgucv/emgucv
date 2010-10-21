#include "sse.h"
#include "doubleOps.h"
#include "stdio.h"
#include <iostream>
using namespace std;

void Test_cross_product()
{
#if EMGU_SSE2
   __m128d v0 = _mm_set_pd(0.01, 0.02);
   __m128d v1 = _mm_set_pd(0.03, 0.04);

   double val0 = _cross_product(v0, v1);
   double val1 = v0.m128d_f64[1] * v1.m128d_f64[0] - v0.m128d_f64[0] * v1.m128d_f64[1];
   cout <<"Test cross product: " << (val0 == val1 ? "Passed" : "Failed") << std::endl;
#endif
} 

void Test_double_MulS()
{
   double val0[]  = {0.1, 0.2, 0.3};
   
   double scale = 0.12345;

   double val1[3];

   bool success = true;
   doubleOps::mulS(val0, scale, 3, val1);
   for (int i = 0; i < 3; i++)
   {
      success &= (val1[i] == (val0[i] * scale));
   }

   memset(val1, 0, 3* sizeof(double));
   doubleOps::mulS(val0, scale, 2, val1);
   for (int i = 0; i < 2; i++)
   {
      success &= (val1[i] == (val0[i] * scale));
   }

   cout <<"Test mulS: " << (success ? "Passed" : "Failed") << std::endl;
}

int main()
{
   char tmp;
   Test_cross_product();
   Test_double_MulS();
 
#ifdef _MSC_VER
   cin >>tmp; //wait for input only if compiling with visual C++ 
#endif
}


