#include "sse.h"
#include "doubleOps.h"
#include "stdio.h"
#include <iostream>
#include "quaternions.h"

#ifdef _MSC_VER
#include "windows.h"
#include "time.h"
#endif

using namespace std;

#define ERROR_EPS 1.0e-12
#define fequal(a, b) (fabs((a) - (b))< ( (fabs(a) + fabs(b)) / 2 * ERROR_EPS)) 
void Test_cross_product()
{
#if EMGU_SSE2
   __m128d v0 = _mm_set_pd(0.01, 0.02);
   __m128d v1 = _mm_set_pd(0.03, 0.04);

   double val0;
   _mm_store_sd(&val0, _cross_product(v1, v0));
   double val1 = 0.01 * 0.04 - 0.02 * 0.03;
   cout <<"Test cross product: " << (fequal(val0, val1) ? "Passed" : "Failed") << std::endl;
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
      bool equals = fequal(val1[i], (val0[i] * scale));
      if (!equals) cout << val1[i] << " != " << (val0[i] * scale) << std::endl;
      success &= equals;
   }

   memset(val1, 0, 3* sizeof(double));
   doubleOps::mulS(val0, scale, 2, val1);
   for (int i = 0; i < 2; i++)
   {
      bool equals = fequal(val1[i], (val0[i] * scale));
      if (!equals) cout << val1[i] << " != " << (val0[i] * scale) << std::endl;
      success &= equals;
   }

   cout <<"Test mulS: " << (success ? "Passed" : "Failed") << std::endl;
}

#ifdef _MSC_VER
void Test_quaternions_performance()
{
   LARGE_INTEGER begin;
   LARGE_INTEGER end;

   Quaternions q1, q2, q;

   /* initialize random seed: */
   srand ( time(NULL) );

   q1.w = rand(); q1.x = rand(); q1.y = rand(); q1.z = rand();
   q2.w = rand(); q2.x = rand(); q2.y = rand(); q2.z = rand();
   q1.renorm();
   q2.renorm();

   int count = 100000;
   {
      QueryPerformanceCounter(&begin); 
      for (int i = 0; i < count; i++)
      {
         //perform tasks
         q1.multiply(&q2, &q1); 
      }
      QueryPerformanceCounter(&end); 
      cout <<"Quaternions multiplication total CPU Cycle: " << (end.QuadPart - begin.QuadPart) << std::endl;
   }
   {
      QueryPerformanceCounter(&begin); 
      for (int i = 0; i < count; i++)
      {
         //perform tasks
         q1.renorm();
      }
      QueryPerformanceCounter(&end); 
      cout <<"Quaternions renorm total CPU Cycle: " << (end.QuadPart - begin.QuadPart) << std::endl;
   }
}
#endif

int main()
{
   char tmp;
   Test_cross_product();
   Test_double_MulS();
 
#ifdef _MSC_VER
   Test_quaternions_performance();
   cin >>tmp; //wait for input only if compiling with visual C++ 
#endif
}


