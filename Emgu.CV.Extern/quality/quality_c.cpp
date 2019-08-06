//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2019 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "quality_c.h"

void cveQualityBaseCompute(cv::quality::QualityBase* qualityBase, cv::_InputArray* cmpImgs, CvScalar* score)
{
	cv::Scalar s = qualityBase->compute(*cmpImgs);
	*score = s;
}

void cveQualityBaseGetQualityMap(cv::quality::QualityBase* qualityBase, cv::_OutputArray* dst)
{
	qualityBase->getQualityMap(*dst);
}

cv::quality::QualityMSE* cveQualityMSECreate(
	cv::_InputArray* refImgs,
	cv::quality::QualityBase** qualityBase,
	cv::Algorithm** algorithm,
	cv::Ptr<cv::quality::QualityMSE>** sharedPtr)
{
	cv::Ptr<cv::quality::QualityMSE> quality = cv::quality::QualityMSE::create(*refImgs);
	*sharedPtr = new cv::Ptr<cv::quality::QualityMSE>(quality);
	cv::quality::QualityMSE* ptr = (*sharedPtr)->get();
	*qualityBase = dynamic_cast<cv::quality::QualityBase*>(ptr);
	*algorithm = dynamic_cast<cv::Algorithm*>(ptr);
	return ptr;
}

void cveQualityMSERelease(cv::Ptr<cv::quality::QualityMSE>** sharedPtr)
{
	delete *sharedPtr;
	*sharedPtr = 0;
}

cv::quality::QualityBRISQUE* cveQualityBRISQUECreate(
	cv::String* modelFilePath,
	cv::String* rangeFilePath,
	cv::quality::QualityBase** qualityBase,
	cv::Algorithm** algorithm,
	cv::Ptr<cv::quality::QualityBRISQUE>** sharedPtr)
{
	cv::Ptr<cv::quality::QualityBRISQUE> quality = cv::quality::QualityBRISQUE::create(*modelFilePath, *rangeFilePath);
	*sharedPtr = new cv::Ptr<cv::quality::QualityBRISQUE>(quality);
	cv::quality::QualityBRISQUE* ptr = (*sharedPtr)->get();
	*qualityBase = dynamic_cast<cv::quality::QualityBase*>(ptr);
	*algorithm = dynamic_cast<cv::Algorithm*>(ptr);
	return ptr;
}

void cveQualityBRISQUERelease(cv::Ptr<cv::quality::QualityBRISQUE>** sharedPtr)
{
	delete *sharedPtr;
	*sharedPtr = 0;
}

cv::quality::QualityPSNR* cveQualityPSNRCreate(
	cv::_InputArray* refImgs,
	double maxPixelValue,
	cv::quality::QualityBase** qualityBase,
	cv::Algorithm** algorithm,
	cv::Ptr<cv::quality::QualityPSNR>** sharedPtr)
{
	cv::Ptr<cv::quality::QualityPSNR> quality = cv::quality::QualityPSNR::create(*refImgs, maxPixelValue);
	*sharedPtr = new cv::Ptr<cv::quality::QualityPSNR>(quality);
	cv::quality::QualityPSNR* ptr = (*sharedPtr)->get();
	*qualityBase = dynamic_cast<cv::quality::QualityBase*>(ptr);
	*algorithm = dynamic_cast<cv::Algorithm*>(ptr);
	return ptr;
}

void cveQualityPSNRRelease(cv::Ptr<cv::quality::QualityPSNR>** sharedPtr)
{
	delete *sharedPtr;
	*sharedPtr = 0;
}

cv::quality::QualitySSIM* cveQualitySSIMCreate(
	cv::_InputArray* refImgs,
	cv::quality::QualityBase** qualityBase,
	cv::Algorithm** algorithm,
	cv::Ptr<cv::quality::QualitySSIM>** sharedPtr)
{
	cv::Ptr<cv::quality::QualitySSIM> quality = cv::quality::QualitySSIM::create(*refImgs);
	*sharedPtr = new cv::Ptr<cv::quality::QualitySSIM>(quality);
	cv::quality::QualitySSIM* ptr = (*sharedPtr)->get();
	*qualityBase = dynamic_cast<cv::quality::QualityBase*>(ptr);
	*algorithm = dynamic_cast<cv::Algorithm*>(ptr);
	return ptr;
}

void cveQualitySSIMRelease(cv::Ptr<cv::quality::QualitySSIM>** sharedPtr)
{
	delete *sharedPtr;
	*sharedPtr = 0;
}

cv::quality::QualityGMSD* cveQualityGMSDCreate(
	cv::_InputArray* refImgs,
	cv::quality::QualityBase** qualityBase,
	cv::Algorithm** algorithm,
	cv::Ptr<cv::quality::QualityGMSD>** sharedPtr)
{
	cv::Ptr<cv::quality::QualityGMSD> quality = cv::quality::QualityGMSD::create(*refImgs);
	*sharedPtr = new cv::Ptr<cv::quality::QualityGMSD>(quality);
	cv::quality::QualityGMSD* ptr = (*sharedPtr)->get();
	*qualityBase = dynamic_cast<cv::quality::QualityBase*>(ptr);
	*algorithm = dynamic_cast<cv::Algorithm*>(ptr);
	return ptr;
}

void cveQualityGMSDRelease(cv::Ptr<cv::quality::QualityGMSD>** sharedPtr)
{
	delete *sharedPtr;
	*sharedPtr = 0;
}