//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2019 by EMGU Corporation. All rights reserved.
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
	return new cv::ppf_match_3d::ICP(iterations, tolerence, rejectionScale, numLevels, sampleType, numMaxCorr);
}

int cveICPRegisterModelToScene(cv::ppf_match_3d::ICP* icp, cv::Mat* srcPC, cv::Mat* dstPC, double* residual, cv::Mat* pose)
{
	double r;
	cv::Matx44d p;
	int result = icp->registerModelToScene(*srcPC, *dstPC, r, p);
	*residual = r;
	cv::Mat matP(p);
	matP.copyTo(*pose);
	return result;
}

void cveICPRelease(cv::ppf_match_3d::ICP** icp)
{
	delete *icp;
	*icp = 0;
}

