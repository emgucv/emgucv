//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_POINT_UTIL_H
#define EMGU_POINT_UTIL_H

#include "opencv2/core/core.hpp"
#include "sse.h"

/**
 * @fn   inline float cvPoint3D32fDotProduct(const CvPoint3D32f* p1, const CvPoint3D32f* p2)
 *
 * @brief   Compute the dot product of two 3D vector
 *
 * @author  Canming Huang
 * @date 8/31/2010
 *
 * @param   p1 The first const CvPoint3D32f*. 
 * @param   p2 The second const CvPoint3D32f*. 
 *
 * @return  The dot product of two 3D vector  
**/
inline float cvPoint3D32fDotProduct(const CvPoint3D32f* p1, const CvPoint3D32f* p2)
{
   /*
#if EMGU_SSE
   __m128 ps1 = _mm_set_ps(p1->x, p1->y, p1->z, 0);
   __m128 ps2 = _mm_set_ps(p2->x, p2->y, p2->z, 0);
   __m128 product = _mm_mul_ps(ps1, ps2);
   return product.m128_f32[3] + product.m128_f32[2] + product.m128_f32[1];
#else*/
   return p1->x * p2->x + p1->y * p2->y + p1->z * p2->z;
//#endif
};

/**
 * @fn   inline double cvPoint3D64fDotProduct(const CvPoint3D64f* p1, const CvPoint3D64f* p2)
 *
 * @brief   Compute the dot product of two 3D vector
 *
 * @author  Canming Huang
 * @date 8/31/2010
 *
 * @param   p1 The first const CvPoint3D64f*. 
 * @param   p2 The second const CvPoint3D64f*. 
 *
 * @return  The dot product of two 3D vector 
**/
inline double cvPoint3D64fDotProduct(const CvPoint3D64f* p1, const CvPoint3D64f* p2)
{
#if EMGU_SSE2
   double tmp;
   _mm_store_sd(&tmp, _dot_product(_mm_loadu_pd(&p1->x), _mm_loadu_pd(&p2->x)));
   return tmp + p1->z * p2->z;
#else
   return p1->x * p2->x + p1->y * p2->y + p1->z * p2->z;
#endif
};

/**
 * @fn   inline void cvPoint3D64fCrossProduct(const CvPoint3D64f* p1, const CvPoint3D64f* p2,
 * CvPoint3D64f* crossProduct)
 *
 * @brief   Compute the cross product of two 3D vector
 *
 * @author  Canming Huang
 * @date 8/31/2010
 *
 * @param   p1                      The first const CvPoint3D64f*. 
 * @param   p2                      The second const CvPoint3D64f*. 
 * @param [in, out]   crossProduct   If non-null, the cross product. 
**/
inline void cvPoint3D64fCrossProduct(const CvPoint3D64f* p1, const CvPoint3D64f* p2, CvPoint3D64f* crossProduct)
{
#if EMGU_SSE2
   _mm_store_sd(&crossProduct->x, _cross_product(_mm_loadu_pd(&p1->y), _mm_loadu_pd(&p2->y))); 
   _mm_store_sd(&crossProduct->y, _cross_product(_mm_set_pd(p1->x, p1->z), _mm_set_pd(p2->x, p2->z)));
   _mm_store_sd(&crossProduct->z, _cross_product(_mm_loadu_pd(&p1->x), _mm_loadu_pd(&p2->x))); 
#else
   crossProduct->x = p1->y * p2->z - p1->z * p2->y;
   crossProduct->y = p1->z * p2->x - p1->x * p2->z;
   crossProduct->z = p1->x * p2->y - p1->y * p2->x;
#endif
};

inline void cvPoint3D64fSub(const CvPoint3D64f* p1, const CvPoint3D64f* p2, CvPoint3D64f* result)
{
#if EMGU_SSE2
   _mm_storeu_pd((double*)result, _mm_sub_pd(_mm_loadu_pd((const double*) p1), _mm_loadu_pd((const double*) p2)));
#else
   result->x = p1->x - p2->x;
   result->y = p1->y - p2->y;
#endif
   result->z = p1->z - p2->z;
}

inline void cvPoint3D64fAdd(const CvPoint3D64f* p1, const CvPoint3D64f* p2, CvPoint3D64f* result)
{
#if EMGU_SSE2
   _mm_storeu_pd((double*)result, _mm_add_pd(_mm_loadu_pd((const double*) p1), _mm_loadu_pd((const double*) p2)));
#else
   result->x = p1->x + p2->x;
   result->y = p1->y + p2->y;
#endif
   result->z = p1->z + p2->z;
}

inline void cvPoint3D64fMul(const CvPoint3D64f* p1, double scale, CvPoint3D64f* result)
{
#if EMGU_SSE2
   _mm_storeu_pd((double*)result, _mm_mul_pd(_mm_loadu_pd((const double*) p1), _mm_set1_pd(scale)));
#else
   result->x = p1->x * scale;
   result->y = p1->y * scale;
#endif
   result->z = p1->z * scale;
}

/**
 * @fn   inline bool cvPoint3D64Equals(const CvPoint3D64f* p1, const CvPoint3D64f* p2)
 *
 * @brief   Check if the two point equals. 
 *
 * @author  Canming Huang
 * @date 9/01/2010
 *
 * @param   p1 The first CvPoint3D64f. 
 * @param   p2 The second CvPoint3D64f. 
 *
 * @return  true if the two point equals, false otherwise. 
**/
inline bool cvPoint3D64Equals(const CvPoint3D64f* p1, const CvPoint3D64f* p2)
{
   return memcmp(p1, p2, sizeof(CvPoint3D64f)) == 0;
}

#endif