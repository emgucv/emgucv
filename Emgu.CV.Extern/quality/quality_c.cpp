//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "quality_c.h"

void cveQualityBaseCompute(cv::quality::QualityBase* qualityBase, cv::_InputArray* cmpImgs, CvScalar* score)
{
#ifdef HAVE_OPENCV_QUALITY
	cv::Scalar s = qualityBase->compute(*cmpImgs);
	*score = cvScalar(s);
#else
	throw_no_quality();
#endif
}

void cveQualityBaseGetQualityMap(cv::quality::QualityBase* qualityBase, cv::_OutputArray* dst)
{
#ifdef HAVE_OPENCV_QUALITY
	qualityBase->getQualityMap(*dst);
#else
	throw_no_quality();
#endif
}

cv::quality::QualityMSE* cveQualityMSECreate(
	cv::_InputArray* refImgs,
	cv::quality::QualityBase** qualityBase,
	cv::Algorithm** algorithm,
	cv::Ptr<cv::quality::QualityMSE>** sharedPtr)
{
#ifdef HAVE_OPENCV_QUALITY
	cv::Ptr<cv::quality::QualityMSE> quality = cv::quality::QualityMSE::create(*refImgs);
	*sharedPtr = new cv::Ptr<cv::quality::QualityMSE>(quality);
	cv::quality::QualityMSE* ptr = (*sharedPtr)->get();
	*qualityBase = dynamic_cast<cv::quality::QualityBase*>(ptr);
	*algorithm = dynamic_cast<cv::Algorithm*>(ptr);
	return ptr;
#else
	throw_no_quality();
#endif
}

void cveQualityMSERelease(cv::Ptr<cv::quality::QualityMSE>** sharedPtr)
{
#ifdef HAVE_OPENCV_QUALITY
	delete *sharedPtr;
	*sharedPtr = 0;
#else
	throw_no_quality();
#endif
}

cv::quality::QualityBRISQUE* cveQualityBRISQUECreate(
	cv::String* modelFilePath,
	cv::String* rangeFilePath,
	cv::quality::QualityBase** qualityBase,
	cv::Algorithm** algorithm,
	cv::Ptr<cv::quality::QualityBRISQUE>** sharedPtr)
{
#ifdef HAVE_OPENCV_QUALITY
	cv::Ptr<cv::quality::QualityBRISQUE> quality = cv::quality::QualityBRISQUE::create(*modelFilePath, *rangeFilePath);
	*sharedPtr = new cv::Ptr<cv::quality::QualityBRISQUE>(quality);
	cv::quality::QualityBRISQUE* ptr = (*sharedPtr)->get();
	*qualityBase = dynamic_cast<cv::quality::QualityBase*>(ptr);
	*algorithm = dynamic_cast<cv::Algorithm*>(ptr);
	return ptr;
#else
	throw_no_quality();
#endif
}

void cveQualityBRISQUERelease(cv::Ptr<cv::quality::QualityBRISQUE>** sharedPtr)
{
#ifdef HAVE_OPENCV_QUALITY
	delete *sharedPtr;
	*sharedPtr = 0;
#else
	throw_no_quality();
#endif
}

cv::quality::QualityPSNR* cveQualityPSNRCreate(
	cv::_InputArray* refImgs,
	double maxPixelValue,
	cv::quality::QualityBase** qualityBase,
	cv::Algorithm** algorithm,
	cv::Ptr<cv::quality::QualityPSNR>** sharedPtr)
{
#ifdef HAVE_OPENCV_QUALITY
	cv::Ptr<cv::quality::QualityPSNR> quality = cv::quality::QualityPSNR::create(*refImgs, maxPixelValue);
	*sharedPtr = new cv::Ptr<cv::quality::QualityPSNR>(quality);
	cv::quality::QualityPSNR* ptr = (*sharedPtr)->get();
	*qualityBase = dynamic_cast<cv::quality::QualityBase*>(ptr);
	*algorithm = dynamic_cast<cv::Algorithm*>(ptr);
	return ptr;
#else
	throw_no_quality();
#endif
}

void cveQualityPSNRRelease(cv::Ptr<cv::quality::QualityPSNR>** sharedPtr)
{
#ifdef HAVE_OPENCV_QUALITY
	delete *sharedPtr;
	*sharedPtr = 0;
#else
	throw_no_quality();
#endif
}

cv::quality::QualitySSIM* cveQualitySSIMCreate(
	cv::_InputArray* refImgs,
	cv::quality::QualityBase** qualityBase,
	cv::Algorithm** algorithm,
	cv::Ptr<cv::quality::QualitySSIM>** sharedPtr)
{
#ifdef HAVE_OPENCV_QUALITY
	cv::Ptr<cv::quality::QualitySSIM> quality = cv::quality::QualitySSIM::create(*refImgs);
	*sharedPtr = new cv::Ptr<cv::quality::QualitySSIM>(quality);
	cv::quality::QualitySSIM* ptr = (*sharedPtr)->get();
	*qualityBase = dynamic_cast<cv::quality::QualityBase*>(ptr);
	*algorithm = dynamic_cast<cv::Algorithm*>(ptr);
	return ptr;
#else
	throw_no_quality();
#endif
}

void cveQualitySSIMRelease(cv::Ptr<cv::quality::QualitySSIM>** sharedPtr)
{
#ifdef HAVE_OPENCV_QUALITY
	delete *sharedPtr;
	*sharedPtr = 0;
#else
	throw_no_quality();
#endif
}

cv::quality::QualityGMSD* cveQualityGMSDCreate(
	cv::_InputArray* refImgs,
	cv::quality::QualityBase** qualityBase,
	cv::Algorithm** algorithm,
	cv::Ptr<cv::quality::QualityGMSD>** sharedPtr)
{
#ifdef HAVE_OPENCV_QUALITY
	cv::Ptr<cv::quality::QualityGMSD> quality = cv::quality::QualityGMSD::create(*refImgs);
	*sharedPtr = new cv::Ptr<cv::quality::QualityGMSD>(quality);
	cv::quality::QualityGMSD* ptr = (*sharedPtr)->get();
	*qualityBase = dynamic_cast<cv::quality::QualityBase*>(ptr);
	*algorithm = dynamic_cast<cv::Algorithm*>(ptr);
	return ptr;
#else
	throw_no_quality();
#endif
}

void cveQualityGMSDRelease(cv::Ptr<cv::quality::QualityGMSD>** sharedPtr)
{
#ifdef HAVE_OPENCV_QUALITY
	delete *sharedPtr;
	*sharedPtr = 0;
#else
	throw_no_quality();
#endif
}