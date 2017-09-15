//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2017 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "img_hash_c_extra.h"

void cveImgHashBaseCompute(cv::img_hash::ImgHashBase* imgHash, cv::_InputArray* inputArr, cv::_OutputArray* outputArr)
{
	imgHash->compute(*inputArr, *outputArr);
}
double cveImgHashBaseCompare(cv::img_hash::ImgHashBase* imgHash, cv::_InputArray* hashOne, cv::_InputArray* hashTwo)
{
	return imgHash->compare(*hashOne, *hashTwo);
}

//AverageHash
cv::img_hash::AverageHash* cveAverageHashCreate(cv::img_hash::ImgHashBase** imgHash)
{
	cv::Ptr<cv::img_hash::AverageHash> ptr = cv::img_hash::AverageHash::create();
	ptr.addref();
	*imgHash = dynamic_cast<cv::img_hash::ImgHashBase*>(ptr.get());
	return ptr.get();
}
CVAPI(void) cveAverageHashRelease(cv::img_hash::AverageHash** hash)
{
	delete *hash;
	*hash = 0;
}

//BlockMeanHash
cv::img_hash::BlockMeanHash* cveBlockMeanHashCreate(cv::img_hash::ImgHashBase** imgHash, int mode)
{
	cv::Ptr<cv::img_hash::BlockMeanHash> ptr = cv::img_hash::BlockMeanHash::create(mode);
	ptr.addref();
	*imgHash = dynamic_cast<cv::img_hash::ImgHashBase*>(ptr.get());
	return ptr.get();
}
void cveBlockMeanHashRelease(cv::img_hash::BlockMeanHash** hash)
{
	delete *hash;
	*hash = 0;
}

//ColorMomentHash
cv::img_hash::ColorMomentHash* cveColorMomentHashCreate(cv::img_hash::ImgHashBase** imgHash)
{
	cv::Ptr<cv::img_hash::ColorMomentHash> ptr = cv::img_hash::ColorMomentHash::create();
	ptr.addref();
	*imgHash = dynamic_cast<cv::img_hash::ImgHashBase*>(ptr.get());
	return ptr.get();
}
CVAPI(void) cveColorMomentHashRelease(cv::img_hash::ColorMomentHash** hash)
{
	delete *hash;
	*hash = 0;
}

//MarrHildrethHash
cv::img_hash::MarrHildrethHash* cveMarrHildrethHashCreate(cv::img_hash::ImgHashBase** imgHash, float alpha, float scale)
{
	cv::Ptr<cv::img_hash::MarrHildrethHash> ptr = cv::img_hash::MarrHildrethHash::create(alpha, scale);
	ptr.addref();
	*imgHash = dynamic_cast<cv::img_hash::ImgHashBase*>(ptr.get());
	return ptr.get();
}
void cveMarrHildrethHashRelease(cv::img_hash::MarrHildrethHash** hash)
{
	delete *hash;
	*hash = 0;
}

//PHash
cv::img_hash::PHash* cvePHashCreate(cv::img_hash::ImgHashBase** imgHash)
{
	cv::Ptr<cv::img_hash::PHash> ptr = cv::img_hash::PHash::create();
	ptr.addref();
	*imgHash = dynamic_cast<cv::img_hash::ImgHashBase*>(ptr.get());
	return ptr.get();
}

void cvePHashRelease(cv::img_hash::PHash** hash)
{
	delete *hash;
	*hash = 0;
}

//RadialVarianceHash
cv::img_hash::RadialVarianceHash* cveRadialVarianceHashCreate(cv::img_hash::ImgHashBase** imgHash, double sigma, int numOfAngleLine)
{
	cv::Ptr<cv::img_hash::RadialVarianceHash> ptr = cv::img_hash::RadialVarianceHash::create(sigma, numOfAngleLine);
	ptr.addref();
	*imgHash = dynamic_cast<cv::img_hash::ImgHashBase*>(ptr.get());
	return ptr.get();
}
void cveRadialVarianceHashRelease(cv::img_hash::RadialVarianceHash** hash)
{
	delete *hash;
	*hash = 0;
}