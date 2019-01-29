//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2019 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "xphoto_c.h"

void cveWhiteBalancerBalanceWhite(cv::xphoto::WhiteBalancer* whiteBalancer, cv::_InputArray* src, cv::_OutputArray* dst)
{
	whiteBalancer->balanceWhite(*src, *dst);
}

cv::xphoto::SimpleWB* cveSimpleWBCreate(cv::xphoto::WhiteBalancer** whiteBalancer, cv::Ptr<cv::xphoto::SimpleWB>** sharedPtr)
{
	cv::Ptr<cv::xphoto::SimpleWB> ptr = cv::xphoto::createSimpleWB();
	*sharedPtr = new cv::Ptr<cv::xphoto::SimpleWB>(ptr);
	*whiteBalancer = dynamic_cast<cv::xphoto::WhiteBalancer*>(ptr.get());
	return ptr.get();
}
void cveSimpleWBRelease(cv::xphoto::SimpleWB** whiteBalancer, cv::Ptr<cv::xphoto::SimpleWB>** sharedPtr)
{
	delete *sharedPtr;
	*whiteBalancer = 0;
	*sharedPtr = 0;
}

cv::xphoto::GrayworldWB* cveGrayworldWBCreate(cv::xphoto::WhiteBalancer** whiteBalancer, cv::Ptr<cv::xphoto::GrayworldWB>** sharedPtr)
{
	cv::Ptr<cv::xphoto::GrayworldWB> ptr = cv::xphoto::createGrayworldWB();
	*sharedPtr = new cv::Ptr<cv::xphoto::GrayworldWB>(ptr);
	*whiteBalancer = dynamic_cast<cv::xphoto::WhiteBalancer*>(ptr.get());
	return ptr.get();
}
void cveGrayworldWBRelease(cv::xphoto::GrayworldWB** whiteBalancer, cv::Ptr<cv::xphoto::GrayworldWB>** sharedPtr)
{
	delete *sharedPtr;
	*whiteBalancer = 0;
	*sharedPtr = 0;
}

cv::xphoto::LearningBasedWB* cveLearningBasedWBCreate(cv::xphoto::WhiteBalancer** whiteBalancer, cv::Ptr<cv::xphoto::LearningBasedWB>** sharedPtr)
{
	cv::Ptr<cv::xphoto::LearningBasedWB> ptr = cv::xphoto::createLearningBasedWB();
	*sharedPtr = new cv::Ptr<cv::xphoto::LearningBasedWB>(ptr);
	*whiteBalancer = dynamic_cast<cv::xphoto::WhiteBalancer*>(ptr.get());
	return ptr.get();
}

void cveLearningBasedWBRelease(cv::xphoto::LearningBasedWB** whiteBalancer, cv::Ptr<cv::xphoto::LearningBasedWB>** sharedPtr)
{
	delete *sharedPtr;
	*whiteBalancer = 0;
	*sharedPtr = 0;
}

void cveApplyChannelGains(cv::_InputArray* src, cv::_OutputArray* dst, float gainB, float gainG, float gainR)
{
	cv::xphoto::applyChannelGains(*src, *dst, gainB, gainG, gainR);
}

void cveDctDenoising(const cv::Mat* src, cv::Mat* dst, const double sigma, const int psize)
{
   cv::xphoto::dctDenoising(*src, *dst, sigma, psize);
}

void cveXInpaint(const cv::Mat* src, const cv::Mat* mask, cv::Mat* dst, const int algorithmType)
{
   cv::xphoto::inpaint(*src, *mask, *dst, algorithmType);
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
	cv::xphoto::bm3dDenoising(
		*src, *dstStep1, *dstStep2,
		h, templateWindowSize, searchWindowSize, blockMatchingStep1, blockMatchingStep2,
		groupSize, slidingStep, beta, normType, step, transformType);
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
	cv::xphoto::bm3dDenoising(
		*src, *dst, h, templateWindowSize, searchWindowSize, blockMatchingStep1, blockMatchingStep2,
		groupSize, slidingStep, beta, normType, step, transformType);
}


void cveOilPainting(
	cv::_InputArray* src,
	cv::_OutputArray* dst,
	int size,
	int dynRatio,
	int code)
{
	cv::xphoto::oilPainting(*src, *dst, size, dynRatio, code);
}


cv::xphoto::TonemapDurand* cveTonemapDurandCreate(
	float gamma, float contrast, float saturation, float sigmaSpace, float sigmaColor,
	cv::Tonemap** tonemap, cv::Algorithm** algorithm,
	cv::Ptr<cv::xphoto::TonemapDurand>** sharedPtr)
{
	cv::Ptr<cv::xphoto::TonemapDurand> t = cv::xphoto::createTonemapDurand(gamma, contrast, saturation, sigmaSpace, sigmaColor);
	*sharedPtr = new cv::Ptr<cv::xphoto::TonemapDurand>(t);
	*tonemap = dynamic_cast<cv::Tonemap*>(t.get());
	*algorithm = dynamic_cast<cv::Algorithm*>(t.get());
	return t.get();
}
void cveTonemapDurandRelease(cv::xphoto::TonemapDurand** tonemap, cv::Ptr<cv::xphoto::TonemapDurand>** sharedPtr)
{
	delete *sharedPtr;
	*tonemap = 0;
	*sharedPtr = 0;
}