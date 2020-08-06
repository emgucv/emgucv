//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "rapid_c.h"

void cveDrawCorrespondencies(
	cv::_InputOutputArray* bundle,
	cv::_InputArray* cols,
	cv::_InputArray* colors)
{
	cv::rapid::drawCorrespondencies(*bundle, *cols, colors ? *colors: static_cast<cv::InputArray>(cv::noArray()));
}

void cveDrawSearchLines(
	cv::_InputOutputArray* img,
	cv::_InputArray* locations,
	CvScalar* color)
{
	cv::rapid::drawSearchLines(*img, *locations, *color);
}

void cveDrawWireframe(
	cv::_InputOutputArray* img,
	cv::_InputArray* pts2d,
	cv::_InputArray* tris,
	CvScalar* color,
	int type,
	bool cullBackface)
{
	cv::rapid::drawWireframe(*img, *pts2d, *tris, *color, type, cullBackface);
}

void cveExtractControlPoints(
	int num,
	int len,
	cv::_InputArray* pts3d,
	cv::_InputArray* rvec,
	cv::_InputArray* tvec,
	cv::_InputArray* K,
	CvSize* imsize,
	cv::_InputArray* tris,
	cv::_OutputArray* ctl2d,
	cv::_OutputArray* ctl3d)
{
	cv::rapid::extractControlPoints(num, len, *pts3d, *rvec, *tvec, *K, *imsize, *tris, *ctl2d, *ctl3d);
}

void cveExtractLineBundle(
	int len,
	cv::_InputArray* ctl2d,
	cv::_InputArray* img,
	cv::_OutputArray* bundle,
	cv::_OutputArray* srcLocations)
{
	cv::rapid::extractLineBundle(len, *ctl2d, *img, *bundle, *srcLocations);
}

void cveFindCorrespondencies(
	cv::_InputArray* bundle,
	cv::_OutputArray* cols,
	cv::_OutputArray* response)
{
	cv::rapid::findCorrespondencies(*bundle, *cols, response ? *response : static_cast<cv::OutputArray>(cv::noArray()));
}

void cveConvertCorrespondencies(
	cv::_InputArray* cols,
	cv::_InputArray* srcLocations,
	cv::_OutputArray* pts2d,
	cv::_InputOutputArray* pts3d,
	cv::_InputArray* mask)
{
	cv::rapid::convertCorrespondencies(
		*cols,
		*srcLocations,
		*pts2d,
		pts3d ? *pts3d : static_cast<cv::InputOutputArray>(cv::noArray()),
		mask ? *mask : static_cast<cv::InputArray>(cv::noArray()));
}

float cveRapid(
	cv::_InputArray* img,
	int num,
	int len,
	cv::_InputArray* pts3d,
	cv::_InputArray* tris,
	cv::_InputArray* K,
	cv::_InputOutputArray* rvec,
	cv::_InputOutputArray* tvec,
	double* rmsd)
{
	return cv::rapid::rapid(*img, num, len, *pts3d, *tris, *K, *rvec, *tvec, rmsd);
}

float cveTrackerCompute(
	cv::rapid::Tracker* tracker,
	cv::_InputArray* img,
	int num,
	int len,
	cv::_InputArray* K,
	cv::_InputOutputArray* rvec,
	cv::_InputOutputArray* tvec,
	CvTermCriteria* termcrit)
{
	return tracker->compute(*img, num, len, *K, *rvec, *tvec, *termcrit);
}

void cveTrackerClearState(cv::rapid::Tracker* tracker)
{
	tracker->clearState();
}

cv::rapid::Rapid* cveRapidCreate(cv::_InputArray* pts3d, cv::_InputArray* tris, cv::rapid::Tracker** tracker, cv::Algorithm** algorithm, cv::Ptr<cv::rapid::Rapid>** sharedPtr)
{
	cv::Ptr<cv::rapid::Rapid> rapid = cv::rapid::Rapid::create(*pts3d, *tris);
	*sharedPtr = new cv::Ptr<cv::rapid::Rapid>(rapid);
	*tracker = dynamic_cast<cv::rapid::Tracker*>((*sharedPtr)->get());
	*algorithm = dynamic_cast<cv::Algorithm*>((*sharedPtr)->get());
	return (*sharedPtr)->get();
}
void cveRapidRelease(cv::Ptr<cv::rapid::Rapid>** sharedPtr)
{
	delete* sharedPtr;
	*sharedPtr = 0;
}

cv::rapid::OLSTracker* cveOLSTrackerCreate(
	cv::_InputArray* pts3d, 
	cv::_InputArray* tris, 
	int histBins, 
	uchar sobelThesh, 
	cv::rapid::Tracker** tracker, 
	cv::Algorithm** algorithm, 
	cv::Ptr<cv::rapid::OLSTracker>** sharedPtr)
{
	cv::Ptr<cv::rapid::OLSTracker> olsTracker = cv::rapid::OLSTracker::create(*pts3d, *tris, histBins, sobelThesh);
	*sharedPtr = new cv::Ptr<cv::rapid::OLSTracker>(olsTracker);
	*tracker = dynamic_cast<cv::rapid::Tracker*>((*sharedPtr)->get());
	*algorithm = dynamic_cast<cv::Algorithm*>((*sharedPtr)->get());
	return (*sharedPtr)->get();
}
void cveOLSTrackerRelease(cv::Ptr<cv::rapid::OLSTracker>** sharedPtr)
{
	delete* sharedPtr;
	*sharedPtr = 0;
}
