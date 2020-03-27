//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_IMG_HASH_C_H
#define EMGU_IMG_HASH_C_H

#include "opencv2/core/types_c.h"
#ifdef HAVE_OPENCV_IMG_HASH
#include "opencv2/img_hash.hpp"
#else

static inline CV_NORETURN void throw_no_img_hash() { CV_Error(cv::Error::StsBadFunc, "The library is compiled without img hash support"); }

namespace cv
{
	namespace img_hash
	{
		class ImgHashBase {};
		class AverageHash {};
		class ColorMomentHash {};
		class BlockMeanHash {};
		class MarrHildrethHash {};
		class PHash {};
		class RadialVarianceHash {};
		
	}
}

#endif

CVAPI(void) cveImgHashBaseCompute(cv::img_hash::ImgHashBase* imgHash, cv::_InputArray* inputArr, cv::_OutputArray* outputArr);
CVAPI(double) cveImgHashBaseCompare(cv::img_hash::ImgHashBase* imgHash, cv::_InputArray* hashOne, cv::_InputArray* hashTwo);

//AverageHash
CVAPI(cv::img_hash::AverageHash*) cveAverageHashCreate(cv::img_hash::ImgHashBase** imgHash, cv::Ptr<cv::img_hash::AverageHash>** sharedPtr);
CVAPI(void) cveAverageHashRelease(cv::img_hash::AverageHash** hash, cv::Ptr<cv::img_hash::AverageHash>** sharedPtr);

//BlockMeanHash
CVAPI(cv::img_hash::BlockMeanHash*) cveBlockMeanHashCreate(cv::img_hash::ImgHashBase** imgHash, int mode, cv::Ptr<cv::img_hash::BlockMeanHash>** sharedPtr);
CVAPI(void) cveBlockMeanHashRelease(cv::img_hash::BlockMeanHash** hash, cv::Ptr<cv::img_hash::BlockMeanHash>** sharedPtr);

//ColorMomentHash
CVAPI(cv::img_hash::ColorMomentHash*) cveColorMomentHashCreate(cv::img_hash::ImgHashBase** imgHash, cv::Ptr<cv::img_hash::ColorMomentHash>** sharedPtr);
CVAPI(void) cveColorMomentHashRelease(cv::img_hash::ColorMomentHash** hash, cv::Ptr<cv::img_hash::ColorMomentHash>** sharedPtr);

//MarrHildrethHash
CVAPI(cv::img_hash::MarrHildrethHash*) cveMarrHildrethHashCreate(cv::img_hash::ImgHashBase** imgHash, float alpha, float scale, cv::Ptr<cv::img_hash::MarrHildrethHash>** sharedPtr);
CVAPI(void) cveMarrHildrethHashRelease(cv::img_hash::MarrHildrethHash** hash, cv::Ptr<cv::img_hash::MarrHildrethHash>** sharedPtr);

//PHash
CVAPI(cv::img_hash::PHash*) cvePHashCreate(cv::img_hash::ImgHashBase** imgHash, cv::Ptr<cv::img_hash::PHash>** sharedPtr);
CVAPI(void) cvePHashRelease(cv::img_hash::PHash** hash, cv::Ptr<cv::img_hash::PHash>** sharedPtr);

//RadialVarianceHash
CVAPI(cv::img_hash::RadialVarianceHash*) cveRadialVarianceHashCreate(cv::img_hash::ImgHashBase** imgHash, double sigma, int numOfAngleLine, cv::Ptr<cv::img_hash::RadialVarianceHash>** sharedPtr);
CVAPI(void) cveRadialVarianceHashRelease(cv::img_hash::RadialVarianceHash** hash, cv::Ptr<cv::img_hash::RadialVarianceHash>** sharedPtr);
#endif