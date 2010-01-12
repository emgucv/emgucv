#include <cvaux.h>
#include "vectorOfFloat.h"

vectorOfFloat::vectorOfFloat()
:data()
{
}

vectorOfFloat::vectorOfFloat(int size)
:data(size)
{
}

CVAPI(vector<float>*) VectorOfFloatCreate() 
{ 
   return new vector<float>(); 
}

CVAPI(vector<float>*) VectorOfFloatCreateSize(int size) 
{ 
   return new vector<float>(size); 
}

CVAPI(int) VectorOfFloatGetSize(vector<float>* v)
{
   return v->size();
}

CVAPI(void) VectorOfFloatPushMulti(vector<float>* v, float* values, int count)
{
   for(int i=0; i < count; i++) v->push_back(*values++);
}

CVAPI(void) VectorOfFloatClear(vector<float>* v)
{
   v->clear();
}

CVAPI(void) VectorOfFloatRelease(vector<float>* v)
{
   delete v;
}

CVAPI(void) VectorOfFloatCopyData(vector<float>* v, float* data)
{
   memcpy(data, &v->front(), v->size());
}

CVAPI(float*) VectorOfFloatGetStartAddress(vector<float>* v)
{
   return &v->front();
}