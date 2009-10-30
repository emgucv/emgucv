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

CVAPI(vectorOfFloat*) VectorOfFloatCreate() 
{ 
   return new vectorOfFloat(); 
}

CVAPI(vectorOfFloat*) VectorOfFloatCreateSize(int size) 
{ 
   return new vectorOfFloat(size); 
}

CVAPI(int) VectorOfFloatGetSize(vectorOfFloat* v)
{
   return v->data.size();
}

CVAPI(void) VectorOfFloatRelease(vectorOfFloat* v)
{
   delete v;
}

CVAPI(void) VectorOfFloatCopyData(vectorOfFloat* v, float* data)
{
   memcpy(data, &v->data[0], v->data.size());
}

CVAPI(float*) VectorOfFloatGetStartAddress(vectorOfFloat* v)
{
   return &v->data[0];
}