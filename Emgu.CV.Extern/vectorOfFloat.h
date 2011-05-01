//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2011 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#include <vector>
#include "opencv2/core/core_c.h"

CVAPI(std::vector<float>*) VectorOfFloatCreate();

CVAPI(std::vector<float>*) VectorOfFloatCreateSize(int size);

CVAPI(int) VectorOfFloatGetSize(std::vector<float>* v);

CVAPI(void) VectorOfFloatPushMulti(std::vector<float>* v, float* values, int count);

CVAPI(void) VectorOfFloatClear(std::vector<float>* v);

CVAPI(void) VectorOfFloatRelease(std::vector<float>* v);

CVAPI(void) VectorOfFloatCopyData(std::vector<float>* v, float* data);

CVAPI(float*) VectorOfFloatGetStartAddress(std::vector<float>* v);