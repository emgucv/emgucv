//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "xphoto_c.h"

void cveWhiteBalancerBalanceWhite(cv::xphoto::WhiteBalancer* whiteBalancer, cv::_InputArray* src, cv::_OutputArray* dst)
{
#ifdef HAVE_OPENCV_XPHOTO
	whiteBalancer->balanceWhite(*src, *dst);
#else
	throw_no_xphoto();
#endif
}

cv::xphoto::SimpleWB* cveSimpleWBCreate(cv::xphoto::WhiteBalancer** whiteBalancer, cv::Ptr<cv::xphoto::SimpleWB>** sharedPtr)
{
#ifdef HAVE_OPENCV_XPHOTO
	cv::Ptr<cv::xphoto::SimpleWB> ptr = cv::xphoto::createSimpleWB();
	*sharedPtr = new cv::Ptr<cv::xphoto::SimpleWB>(ptr);
	*whiteBalancer = dynamic_cast<cv::xphoto::WhiteBalancer*>(ptr.get());
	return ptr.get();
#else
	throw_no_xphoto();
#endif
}
void cveSimpleWBRelease(cv::Ptr<cv::xphoto::SimpleWB>** sharedPtr)
{
#ifdef HAVE_OPENCV_XPHOTO
	delete *sharedPtr;
	*sharedPtr = 0;
#else
	throw_no_xphoto();
#endif
}

cv::xphoto::GrayworldWB* cveGrayworldWBCreate(cv::xphoto::WhiteBalancer** whiteBalancer, cv::Ptr<cv::xphoto::GrayworldWB>** sharedPtr)
{
#ifdef HAVE_OPENCV_XPHOTO
	cv::Ptr<cv::xphoto::GrayworldWB> ptr = cv::xphoto::createGrayworldWB();
	*sharedPtr = new cv::Ptr<cv::xphoto::GrayworldWB>(ptr);
	*whiteBalancer = dynamic_cast<cv::xphoto::WhiteBalancer*>(ptr.get());
	return ptr.get();
#else
	throw_no_xphoto();
#endif
}
void cveGrayworldWBRelease(cv::Ptr<cv::xphoto::GrayworldWB>** sharedPtr)
{
#ifdef HAVE_OPENCV_XPHOTO
	delete *sharedPtr;
	*sharedPtr = 0;
#else
	throw_no_xphoto();
#endif
}

cv::xphoto::LearningBasedWB* cveLearningBasedWBCreate(cv::xphoto::WhiteBalancer** whiteBalancer, cv::Ptr<cv::xphoto::LearningBasedWB>** sharedPtr)
{
#ifdef HAVE_OPENCV_XPHOTO
	cv::Ptr<cv::xphoto::LearningBasedWB> ptr = cv::xphoto::createLearningBasedWB();
	*sharedPtr = new cv::Ptr<cv::xphoto::LearningBasedWB>(ptr);
	*whiteBalancer = dynamic_cast<cv::xphoto::WhiteBalancer*>(ptr.get());
	return ptr.get();
#else
	throw_no_xphoto();
#endif
}

void cveLearningBasedWBRelease(cv::Ptr<cv::xphoto::LearningBasedWB>** sharedPtr)
{
#ifdef HAVE_OPENCV_XPHOTO
	delete *sharedPtr;
	*sharedPtr = 0;
#else
	throw_no_xphoto();
#endif
}

void cveApplyChannelGains(cv::_InputArray* src, cv::_OutputArray* dst, float gainB, float gainG, float gainR)
{
#ifdef HAVE_OPENCV_XPHOTO
	cv::xphoto::applyChannelGains(*src, *dst, gainB, gainG, gainR);
#else
	throw_no_xphoto();
#endif
}

void cveDctDenoising(const cv::Mat* src, cv::Mat* dst, const double sigma, const int psize)
{
#ifdef HAVE_OPENCV_XPHOTO
   cv::xphoto::dctDenoising(*src, *dst, sigma, psize);
#else
	throw_no_xphoto();
#endif
}

void cveXInpaint(const cv::Mat* src, const cv::Mat* mask, cv::Mat* dst, const int algorithmType)
{
#ifdef HAVE_OPENCV_XPHOTO
   cv::xphoto::inpaint(*src, *mask, *dst, algorithmType);
#else
	throw_no_xphoto();
#endif
}

void cveBm3dDenoising1(
	cv::_InputArray* src,
	cv::_InputOutputArray* dstStep1,
	cv::_OutputArray* dstStep2,
	float h,
	int templateWindowSize,
	int searchWindowSize,
	int blockMatchingStep1,
	int blockMatchingStep2,
	int groupSize,
	int slidingStep,
	float beta,
	int normType,
	int step,
	int transformType)
{
#ifdef HAVE_OPENCV_XPHOTO
	cv::xphoto::bm3dDenoising(
		*src, *dstStep1, *dstStep2,
		h, templateWindowSize, searchWindowSize, blockMatchingStep1, blockMatchingStep2,
		groupSize, slidingStep, beta, normType, step, transformType);
#else
	throw_no_xphoto();
#endif
}

void cveBm3dDenoising2(
	cv::_InputArray* src,
	cv::_OutputArray* dst,
	float h,
	int templateWindowSize,
	int searchWindowSize,
	int blockMatchingStep1,
	int blockMatchingStep2,
	int groupSize,
	int slidingStep,
	float beta,
	int normType,
	int step,
	int transformType)
{
#ifdef HAVE_OPENCV_XPHOTO
	cv::xphoto::bm3dDenoising(
		*src, *dst, h, templateWindowSize, searchWindowSize, blockMatchingStep1, blockMatchingStep2,
		groupSize, slidingStep, beta, normType, step, transformType);
#else
	throw_no_xphoto();
#endif
}


void cveOilPainting(
	cv::_InputArray* src,
	cv::_OutputArray* dst,
	int size,
	int dynRatio,
	int code)
{
#ifdef HAVE_OPENCV_XPHOTO
	cv::xphoto::oilPainting(*src, *dst, size, dynRatio, code);
#else
	throw_no_xphoto();
#endif
}


cv::xphoto::TonemapDurand* cveTonemapDurandCreate(
	float gamma, float contrast, float saturation, float sigmaSpace, float sigmaColor,
	cv::Tonemap** tonemap, cv::Algorithm** algorithm,
	cv::Ptr<cv::xphoto::TonemapDurand>** sharedPtr)
{
#ifdef HAVE_OPENCV_XPHOTO
	cv::Ptr<cv::xphoto::TonemapDurand> t = cv::xphoto::createTonemapDurand(gamma, contrast, saturation, sigmaSpace, sigmaColor);
	*sharedPtr = new cv::Ptr<cv::xphoto::TonemapDurand>(t);
	*tonemap = dynamic_cast<cv::Tonemap*>(t.get());
	*algorithm = dynamic_cast<cv::Algorithm*>(t.get());
	return t.get();
#else
	throw_no_xphoto();
#endif
}
void cveTonemapDurandRelease(cv::Ptr<cv::xphoto::TonemapDurand>** sharedPtr)
{
#ifdef HAVE_OPENCV_XPHOTO
	delete *sharedPtr;
	*sharedPtr = 0;
#else
	throw_no_xphoto();
#endif
}