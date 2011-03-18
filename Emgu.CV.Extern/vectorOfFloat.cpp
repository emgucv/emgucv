//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2011 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#include <vector>
#include "opencv2/core/core_c.h"

CVAPI(std::vector<float>*) VectorOfFloatCreate() 
{ 
   return new std::vector<float>(); 
}

CVAPI(std::vector<float>*) VectorOfFloatCreateSize(int size) 
{ 
   return new std::vector<float>(size); 
}

CVAPI(int) VectorOfFloatGetSize(std::vector<float>* v)
{
   return v->size();
}

CVAPI(void) VectorOfFloatPushMulti(std::vector<float>* v, float* values, int count)
{
   for(int i=0; i < count; i++) v->push_back(*values++);
}

CVAPI(void) VectorOfFloatClear(std::vector<float>* v)
{
   v->clear();
}

CVAPI(void) VectorOfFloatRelease(std::vector<float>* v)
{
   delete v;
}

CVAPI(void) VectorOfFloatCopyData(std::vector<float>* v, float* data)
{
   memcpy(data, &(*v)[0], v->size() * sizeof(float));
}

CVAPI(float*) VectorOfFloatGetStartAddress(std::vector<float>* v)
{
   return &(*v)[0];
}