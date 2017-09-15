//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2017 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_IMG_HASH_C_H
#define EMGU_IMG_HASH_C_H

#include "opencv2/img_hash.hpp"

CVAPI(void) cveImgHashBaseCompute(cv::img_hash::ImgHashBase* imgHash, cv::_InputArray* inputArr, cv::_OutputArray* outputArr);
CVAPI(double) cveImgHashBaseCompare(cv::img_hash::ImgHashBase* imgHash, cv::_InputArray* hashOne, cv::_InputArray* hashTwo);

//AverageHash
CVAPI(cv::img_hash::AverageHash*) cveAverageHashCreate(cv::img_hash::ImgHashBase** imgHash);
CVAPI(void) cveAverageHashRelease(cv::img_hash::AverageHash** hash);

//BlockMeanHash
CVAPI(cv::img_hash::BlockMeanHash*) cveBlockMeanHashCreate(cv::img_hash::ImgHashBase** imgHash, int mode);
CVAPI(void) cveBlockMeanHashRelease(cv::img_hash::BlockMeanHash** hash);

//ColorMomentHash
CVAPI(cv::img_hash::ColorMomentHash*) cveColorMomentHashCreate(cv::img_hash::ImgHashBase** imgHash);
CVAPI(void) cveColorMomentHashRelease(cv::img_hash::ColorMomentHash** hash);

//MarrHildrethHash
CVAPI(cv::img_hash::MarrHildrethHash*) cveMarrHildrethHashCreate(cv::img_hash::ImgHashBase** imgHash, float alpha, float scale);
CVAPI(void) cveMarrHildrethHashRelease(cv::img_hash::MarrHildrethHash** hash);

//PHash
CVAPI(cv::img_hash::PHash*) cvePHashCreate(cv::img_hash::ImgHashBase** imgHash);
CVAPI(void) cvePHashRelease(cv::img_hash::PHash** hash);

//RadialVarianceHash
CVAPI(cv::img_hash::RadialVarianceHash*) cveRadialVarianceHashCreate(cv::img_hash::ImgHashBase** imgHash, double sigma, int numOfAngleLine);
CVAPI(void) cveRadialVarianceHashRelease(cv::img_hash::RadialVarianceHash** hash);
#endif