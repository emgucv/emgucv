//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "alphamat_c.h"

void cveAlphamatInfoFlow(cv::_InputArray* image, cv::_InputArray* tmap, cv::_OutputArray* result)
{
	try
	{
#ifdef HAVE_OPENCV_ALPHAMAT
		// cv::alphamat::infoFlow does not validate its inputs: it loops using
		// image's row/col counts but indexes tmap with them too, so a
		// mismatched tmap size is an out-of-bounds Mat access (heap
		// corruption), not a catchable cv::Exception. Check here instead.
		CV_Assert(image->size() == tmap->size());
		CV_Assert(tmap->channels() == 1);
		cv::alphamat::infoFlow(*image, *tmap, *result);
#else
		throw_no_alphamat();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
