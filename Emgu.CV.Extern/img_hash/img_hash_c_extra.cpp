//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "img_hash_c_extra.h"

void cveImgHashBaseCompute(cv::img_hash::ImgHashBase* imgHash, cv::_InputArray* inputArr, cv::_OutputArray* outputArr)
{
#ifdef HAVE_OPENCV_IMG_HASH
	imgHash->compute(*inputArr, *outputArr);
#else
	throw_no_img_hash();
#endif
}
double cveImgHashBaseCompare(cv::img_hash::ImgHashBase* imgHash, cv::_InputArray* hashOne, cv::_InputArray* hashTwo)
{
#ifdef HAVE_OPENCV_IMG_HASH
	return imgHash->compare(*hashOne, *hashTwo);
#else
	throw_no_img_hash();
#endif
}

//AverageHash
cv::img_hash::AverageHash* cveAverageHashCreate(cv::img_hash::ImgHashBase** imgHash, cv::Ptr<cv::img_hash::AverageHash>** sharedPtr)
{
#ifdef HAVE_OPENCV_IMG_HASH
	cv::Ptr<cv::img_hash::AverageHash> ptr = cv::img_hash::AverageHash::create();
	*imgHash = dynamic_cast<cv::img_hash::ImgHashBase*>(ptr.get());
	*sharedPtr = new cv::Ptr<cv::img_hash::AverageHash>(ptr);
	return ptr.get();
#else
	throw_no_img_hash();
#endif
}
CVAPI(void) cveAverageHashRelease(cv::img_hash::AverageHash** hash, cv::Ptr<cv::img_hash::AverageHash>** sharedPtr)
{
#ifdef HAVE_OPENCV_IMG_HASH
	delete *sharedPtr;
	*sharedPtr = 0;
	*hash = 0;
#else
	throw_no_img_hash();
#endif
}

//BlockMeanHash
cv::img_hash::BlockMeanHash* cveBlockMeanHashCreate(cv::img_hash::ImgHashBase** imgHash, int mode, cv::Ptr<cv::img_hash::BlockMeanHash>** sharedPtr)
{
#ifdef HAVE_OPENCV_IMG_HASH
	cv::Ptr<cv::img_hash::BlockMeanHash> ptr = cv::img_hash::BlockMeanHash::create(mode);
	*imgHash = dynamic_cast<cv::img_hash::ImgHashBase*>(ptr.get());
	*sharedPtr = new cv::Ptr<cv::img_hash::BlockMeanHash>(ptr);
	return ptr.get();
#else
	throw_no_img_hash();
#endif
}
void cveBlockMeanHashRelease(cv::img_hash::BlockMeanHash** hash, cv::Ptr<cv::img_hash::BlockMeanHash>** sharedPtr)
{
#ifdef HAVE_OPENCV_IMG_HASH
	delete *sharedPtr;
	*sharedPtr = 0;
	*hash = 0;
#else
	throw_no_img_hash();
#endif
}

//ColorMomentHash
cv::img_hash::ColorMomentHash* cveColorMomentHashCreate(cv::img_hash::ImgHashBase** imgHash, cv::Ptr<cv::img_hash::ColorMomentHash>** sharedPtr)
{
#ifdef HAVE_OPENCV_IMG_HASH
	cv::Ptr<cv::img_hash::ColorMomentHash> ptr = cv::img_hash::ColorMomentHash::create();
	*imgHash = dynamic_cast<cv::img_hash::ImgHashBase*>(ptr.get());
	*sharedPtr = new cv::Ptr<cv::img_hash::ColorMomentHash>(ptr);
	return ptr.get();
#else
	throw_no_img_hash();
#endif
}
CVAPI(void) cveColorMomentHashRelease(cv::img_hash::ColorMomentHash** hash, cv::Ptr<cv::img_hash::ColorMomentHash>** sharedPtr)
{
#ifdef HAVE_OPENCV_IMG_HASH
	delete *sharedPtr;
	*sharedPtr = 0;
	*hash = 0;
#else
	throw_no_img_hash();
#endif
}

//MarrHildrethHash
cv::img_hash::MarrHildrethHash* cveMarrHildrethHashCreate(cv::img_hash::ImgHashBase** imgHash, float alpha, float scale, cv::Ptr<cv::img_hash::MarrHildrethHash>** sharedPtr)
{
#ifdef HAVE_OPENCV_IMG_HASH
	cv::Ptr<cv::img_hash::MarrHildrethHash> ptr = cv::img_hash::MarrHildrethHash::create(alpha, scale);
	*imgHash = dynamic_cast<cv::img_hash::ImgHashBase*>(ptr.get());
	*sharedPtr = new cv::Ptr<cv::img_hash::MarrHildrethHash>(ptr);
	return ptr.get();
#else
	throw_no_img_hash();
#endif
}
void cveMarrHildrethHashRelease(cv::img_hash::MarrHildrethHash** hash, cv::Ptr<cv::img_hash::MarrHildrethHash>** sharedPtr)
{
#ifdef HAVE_OPENCV_IMG_HASH
	delete *sharedPtr;
	*sharedPtr = 0;
	*hash = 0;
#else
	throw_no_img_hash();
#endif
}

//PHash
cv::img_hash::PHash* cvePHashCreate(cv::img_hash::ImgHashBase** imgHash, cv::Ptr<cv::img_hash::PHash>** sharedPtr)
{
#ifdef HAVE_OPENCV_IMG_HASH
	cv::Ptr<cv::img_hash::PHash> ptr = cv::img_hash::PHash::create();
	*imgHash = dynamic_cast<cv::img_hash::ImgHashBase*>(ptr.get());
	*sharedPtr = new cv::Ptr<cv::img_hash::PHash>(ptr);
	return ptr.get();
#else
	throw_no_img_hash();
#endif
}

void cvePHashRelease(cv::img_hash::PHash** hash, cv::Ptr<cv::img_hash::PHash>** sharedPtr)
{
#ifdef HAVE_OPENCV_IMG_HASH
	delete *sharedPtr;
	*sharedPtr = 0;
	*hash = 0;
#else
	throw_no_img_hash();
#endif
}

//RadialVarianceHash
cv::img_hash::RadialVarianceHash* cveRadialVarianceHashCreate(cv::img_hash::ImgHashBase** imgHash, double sigma, int numOfAngleLine, cv::Ptr<cv::img_hash::RadialVarianceHash>** sharedPtr)
{
#ifdef HAVE_OPENCV_IMG_HASH
	cv::Ptr<cv::img_hash::RadialVarianceHash> ptr = cv::img_hash::RadialVarianceHash::create(sigma, numOfAngleLine);
	*imgHash = dynamic_cast<cv::img_hash::ImgHashBase*>(ptr.get());
	*sharedPtr = new cv::Ptr<cv::img_hash::RadialVarianceHash>(ptr);
	return ptr.get();
#else
	throw_no_img_hash();
#endif
}
void cveRadialVarianceHashRelease(cv::img_hash::RadialVarianceHash** hash, cv::Ptr<cv::img_hash::RadialVarianceHash>** sharedPtr)
{
#ifdef HAVE_OPENCV_IMG_HASH
	delete *sharedPtr;
	*sharedPtr = 0;
	*hash = 0;
#else
	throw_no_img_hash();
#endif
}