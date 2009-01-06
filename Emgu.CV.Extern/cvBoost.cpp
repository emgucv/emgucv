#include "cvextern.h"

CvBoost* CvBoostCreate()
{
   return new CvBoost();
}

void CvBoostRelease(CvBoost* model)
{
   model->~CvBoost();
}