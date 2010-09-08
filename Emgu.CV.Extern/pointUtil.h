#pragma once
#ifndef POINT_UTIL_H
#define POINT_UTIL_H

#include "opencv2/core/core.hpp"

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
   return p1->x * p2->x + p1->y * p2->y + p1->z * p2->z;
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
   return p1->x * p2->x + p1->y * p2->y + p1->z * p2->z;
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
   crossProduct->x = p1->y * p2->z - p1->z * p2->y;
   crossProduct->y = p1->z * p2->x - p1->x * p2->z;
   crossProduct->z = p1->x * p2->y - p1->y * p2->x;
};

inline void cvPoint3D64fSub(const CvPoint3D64f* p1, const CvPoint3D64f* p2, CvPoint3D64f* result)
{
   result->x = p1->x - p2->x;
   result->y = p1->y - p2->y;
   result->z = p1->z - p2->z;
}

inline void cvPoint3D64fAdd(const CvPoint3D64f* p1, const CvPoint3D64f* p2, CvPoint3D64f* result)
{
   result->x = p1->x + p2->x;
   result->y = p1->y + p2->y;
   result->z = p1->z + p2->z;
}

inline void cvPoint3D64fMul(const CvPoint3D64f* p1, double scale, CvPoint3D64f* result)
{
   result->x = p1->x * scale;
   result->y = p1->y * scale;
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