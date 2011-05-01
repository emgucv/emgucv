//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2011 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#include <vector>
#include "opencv2/core/core_c.h"

CVAPI(std::vector<unsigned char>*) VectorOfByteCreate();

CVAPI(std::vector<unsigned char>*) VectorOfByteCreateSize(int size);

CVAPI(int) VectorOfByteGetSize(std::vector<unsigned char>* v);

CVAPI(void) VectorOfBytePushMulti(std::vector<unsigned char>* v, unsigned char* values, int count);

CVAPI(void) VectorOfByteClear(std::vector<unsigned char>* v);

CVAPI(void) VectorOfByteRelease(std::vector<unsigned char>* v);

CVAPI(void) VectorOfByteCopyData(std::vector<unsigned char>* v, unsigned char* data);

CVAPI(unsigned char*) VectorOfByteGetStartAddress(std::vector<unsigned char>* v);