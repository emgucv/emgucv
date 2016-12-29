//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "xphoto_c.h"

void cveWhiteBalancerBalanceWhite(cv::xphoto::WhiteBalancer* whiteBalancer, cv::_InputArray* src, cv::_OutputArray* dst)
{
	whiteBalancer->balanceWhite(*src, *dst);
}

cv::xphoto::SimpleWB* cveSimpleWBCreate(cv::xphoto::WhiteBalancer** whiteBalancer)
{
	cv::Ptr<cv::xphoto::SimpleWB> ptr = cv::xphoto::createSimpleWB();
	ptr.addref();
	*whiteBalancer = dynamic_cast<cv::xphoto::WhiteBalancer*>(ptr.get());
	return ptr.get();
}
void cveSimpleWBRelease(cv::xphoto::SimpleWB** whiteBalancer)
{
	delete *whiteBalancer;
	*whiteBalancer = 0;
}

cv::xphoto::GrayworldWB* cveGrayworldWBCreate(cv::xphoto::WhiteBalancer** whiteBalancer)
{
	cv::Ptr<cv::xphoto::GrayworldWB> ptr = cv::xphoto::createGrayworldWB();
	ptr.addref();
	*whiteBalancer = dynamic_cast<cv::xphoto::WhiteBalancer*>(ptr.get());
	return ptr.get();
}
void cveGrayworldWBRelease(cv::xphoto::GrayworldWB** whiteBalancer)
{
	delete *whiteBalancer;
	*whiteBalancer = 0;
}

cv::xphoto::LearningBasedWB* cveLearningBasedWBCreate(cv::xphoto::WhiteBalancer** whiteBalancer)
{
	cv::Ptr<cv::xphoto::LearningBasedWB> ptr = cv::xphoto::createLearningBasedWB();
	ptr.addref();
	*whiteBalancer = dynamic_cast<cv::xphoto::WhiteBalancer*>(ptr.get());
	return ptr.get();
}

void cveLearningBasedWBRelease(cv::xphoto::LearningBasedWB** whiteBalancer)
{
	delete *whiteBalancer;
	*whiteBalancer = 0;
}

void cveDctDenoising(const cv::Mat* src, cv::Mat* dst, const double sigma, const int psize)
{
   cv::xphoto::dctDenoising(*src, *dst, sigma, psize);
}

void cveXInpaint(const cv::Mat* src, const cv::Mat* mask, cv::Mat* dst, const int algorithmType)
{
   cv::xphoto::inpaint(*src, *mask, *dst, algorithmType);
}