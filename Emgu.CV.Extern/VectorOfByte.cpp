//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2011 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#include <vector>
#include "opencv2/core/core_c.h"

CVAPI(std::vector<unsigned char>*) VectorOfByteCreate() 
{ 
   return new std::vector<unsigned char>(); 
}

CVAPI(std::vector<unsigned char>*) VectorOfByteCreateSize(int size) 
{ 
   return new std::vector<unsigned char>(size); 
}

CVAPI(int) VectorOfByteGetSize(std::vector<unsigned char>* v)
{
   return v->size();
}

CVAPI(void) VectorOfBytePushMulti(std::vector<unsigned char>* v, unsigned char* values, int count)
{
   for(int i=0; i < count; i++) v->push_back(*values++);
}

CVAPI(void) VectorOfByteClear(std::vector<unsigned char>* v)
{
   v->clear();
}

CVAPI(void) VectorOfByteRelease(std::vector<unsigned char>* v)
{
   delete v;
}

CVAPI(void) VectorOfByteCopyData(std::vector<unsigned char>* v, unsigned char* data)
{
   memcpy(data, &(*v)[0], v->size() * sizeof(unsigned char));
}

CVAPI(unsigned char*) VectorOfByteGetStartAddress(std::vector<unsigned char>* v)
{
   return v->empty() ? NULL : &(*v)[0];
}