#include "vectorOfFloat.h"

CVAPI(VectorOfFloat*) VectorOfFloatCreate() 
{ 
   return new VectorOfFloat(); 
}

CVAPI(VectorOfFloat*) VectorOfFloatCreateSize(int size) 
{ 
   return new VectorOfFloat(size); 
}

CVAPI(int) VectorOfFloatGetSize(VectorOfFloat* v)
{
   return v->data.size();
}

CVAPI(void) VectorOfFloatPushMulti(VectorOfFloat* v, float* values, int count)
{
   for(int i=0; i < count; i++) v->data.push_back(*values++);
}

CVAPI(void) VectorOfFloatClear(VectorOfFloat* v)
{
   v->data.clear();
}

CVAPI(void) VectorOfFloatRelease(VectorOfFloat* v)
{
   delete v;
}

CVAPI(void) VectorOfFloatCopyData(VectorOfFloat* v, float* data)
{
   memcpy(data, &v->data[0], v->data.size());
}

CVAPI(float*) VectorOfFloatGetStartAddress(VectorOfFloat* v)
{
   return &v->data[0];
}