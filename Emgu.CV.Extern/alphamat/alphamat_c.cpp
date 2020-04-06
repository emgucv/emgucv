//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "alphamat_c.h"

void cveAlphamatInfoFlow(cv::_InputArray* image, cv::_InputArray* tmap, cv::_OutputArray* result)
{
#ifdef HAVE_OPENCV_ALPHAMAT
	cv::alphamat::infoFlow(*image, *tmap, *result);
#else
	throw_no_alphamat();
#endif
}
