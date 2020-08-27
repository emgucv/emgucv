//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "surface_matching_c.h"

cv::ppf_match_3d::ICP* cveICPCreate(
	int iterations, 
	float tolerence, 
	float rejectionScale, 
	int numLevels, 
	int sampleType, 
	int numMaxCorr)
{
#ifdef HAVE_OPENCV_SURFACE_MATCHING
	return new cv::ppf_match_3d::ICP(iterations, tolerence, rejectionScale, numLevels, sampleType, numMaxCorr);
#else
	throw_no_surface_matching();
#endif
}

int cveICPRegisterModelToScene(cv::ppf_match_3d::ICP* icp, cv::Mat* srcPC, cv::Mat* dstPC, double* residual, cv::Mat* pose)
{
#ifdef HAVE_OPENCV_SURFACE_MATCHING
	double r;
	cv::Matx44d p;
	int result = icp->registerModelToScene(*srcPC, *dstPC, r, p);
	*residual = r;
	cv::Mat matP(p);
	matP.copyTo(*pose);
	return result;
#else
	throw_no_surface_matching();
#endif
}

void cveICPRelease(cv::ppf_match_3d::ICP** icp)
{
#ifdef HAVE_OPENCV_SURFACE_MATCHING
	delete *icp;
	*icp = 0;
#else
	throw_no_surface_matching();
#endif
}

