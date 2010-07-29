#include "opencv2/core/core.hpp"

inline float cvPoint3D32fDotProduct(const CvPoint3D32f* p1, const CvPoint3D32f* p2)
{
   return p1->x * p2->x + p1->y * p2->y + p1->z * p2->z;
};

inline double cvPoint3D64fDotProduct(const CvPoint3D64f* p1, const CvPoint3D64f* p2)
{
   return p1->x * p2->x + p1->y * p2->y + p1->z * p2->z;
};

inline void cvPoint3D64fCrossProduct(const CvPoint3D64f* p1, const CvPoint3D64f* p2, CvPoint3D64f* crossProduct)
{
   crossProduct->x = p1->y * p2->z - p1->z * p2->y;
   crossProduct->y = p1->z * p2->x - p1->x * p2->z;
   crossProduct->z = p1->x * p2->y - p1->y * p2->x;
};

inline void cvPoint3d64fSubstract(const CvPoint3D64f* p1, const CvPoint3D64f* p2, CvPoint3D64f* result)
{
   result->x = p1->x - p2->x;
   result->y = p1->y - p2->y;
   result->z = p1->z - p2->z;
}

//return true if the two points are equal
inline bool cvPoint3D64Equals(CvPoint3D64f* p1, CvPoint3D64f* p2)
{
   return p1->x == p2->x && p1->y == p2->y && p1->z == p2->z;
}

