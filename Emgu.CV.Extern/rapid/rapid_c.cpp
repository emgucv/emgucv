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
#ifdef HAVE_OPENCV_RAPID
	cv::rapid::drawCorrespondencies(*bundle, *cols, colors ? *colors: static_cast<cv::InputArray>(cv::noArray()));
#else
	throw_no_rapid();
#endif
}

void cveDrawSearchLines(
	cv::_InputOutputArray* img,
	cv::_InputArray* locations,
	CvScalar* color)
{
#ifdef HAVE_OPENCV_RAPID
	cv::rapid::drawSearchLines(*img, *locations, *color);
#else
	throw_no_rapid();
#endif
}

void cveDrawWireframe(
	cv::_InputOutputArray* img,
	cv::_InputArray* pts2d,
	cv::_InputArray* tris,
	CvScalar* color,
	int type,
	bool cullBackface)
{
#ifdef HAVE_OPENCV_RAPID
	cv::rapid::drawWireframe(*img, *pts2d, *tris, *color, type, cullBackface);
#else
	throw_no_rapid();
#endif
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
#ifdef HAVE_OPENCV_RAPID
	cv::rapid::extractControlPoints(num, len, *pts3d, *rvec, *tvec, *K, *imsize, *tris, *ctl2d, *ctl3d);
#else
	throw_no_rapid();
#endif
}

void cveExtractLineBundle(
	int len,
	cv::_InputArray* ctl2d,
	cv::_InputArray* img,
	cv::_OutputArray* bundle,
	cv::_OutputArray* srcLocations)
{
#ifdef HAVE_OPENCV_RAPID
	cv::rapid::extractLineBundle(len, *ctl2d, *img, *bundle, *srcLocations);
#else
	throw_no_rapid();
#endif
}

void cveFindCorrespondencies(
	cv::_InputArray* bundle,
	cv::_OutputArray* cols,
	cv::_OutputArray* response)
{
#ifdef HAVE_OPENCV_RAPID
	cv::rapid::findCorrespondencies(*bundle, *cols, response ? *response : static_cast<cv::OutputArray>(cv::noArray()));
#else
	throw_no_rapid();
#endif
}

void cveConvertCorrespondencies(
	cv::_InputArray* cols,
	cv::_InputArray* srcLocations,
	cv::_OutputArray* pts2d,
	cv::_InputOutputArray* pts3d,
	cv::_InputArray* mask)
{
#ifdef HAVE_OPENCV_RAPID
	cv::rapid::convertCorrespondencies(
		*cols,
		*srcLocations,
		*pts2d,
		pts3d ? *pts3d : static_cast<cv::InputOutputArray>(cv::noArray()),
		mask ? *mask : static_cast<cv::InputArray>(cv::noArray()));
#else
	throw_no_rapid();
#endif
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
#ifdef HAVE_OPENCV_RAPID
	return cv::rapid::rapid(*img, num, len, *pts3d, *tris, *K, *rvec, *tvec, rmsd);
#else
	throw_no_rapid();
#endif
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
#ifdef HAVE_OPENCV_RAPID
	return tracker->compute(*img, num, len, *K, *rvec, *tvec, *termcrit);
#else
	throw_no_rapid();
#endif
}

void cveTrackerClearState(cv::rapid::Tracker* tracker)
{
#ifdef HAVE_OPENCV_RAPID
	tracker->clearState();
#else
	throw_no_rapid();
#endif
}

cv::rapid::Rapid* cveRapidCreate(cv::_InputArray* pts3d, cv::_InputArray* tris, cv::rapid::Tracker** tracker, cv::Algorithm** algorithm, cv::Ptr<cv::rapid::Rapid>** sharedPtr)
{
#ifdef HAVE_OPENCV_RAPID
	cv::Ptr<cv::rapid::Rapid> rapid = cv::rapid::Rapid::create(*pts3d, *tris);
	*sharedPtr = new cv::Ptr<cv::rapid::Rapid>(rapid);
	*tracker = dynamic_cast<cv::rapid::Tracker*>((*sharedPtr)->get());
	*algorithm = dynamic_cast<cv::Algorithm*>((*sharedPtr)->get());
	return (*sharedPtr)->get();
#else
	throw_no_rapid();
#endif
}
void cveRapidRelease(cv::Ptr<cv::rapid::Rapid>** sharedPtr)
{
#ifdef HAVE_OPENCV_RAPID
	delete* sharedPtr;
	*sharedPtr = 0;
#else
	throw_no_rapid();
#endif
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
#ifdef HAVE_OPENCV_RAPID
	cv::Ptr<cv::rapid::OLSTracker> olsTracker = cv::rapid::OLSTracker::create(*pts3d, *tris, histBins, sobelThesh);
	*sharedPtr = new cv::Ptr<cv::rapid::OLSTracker>(olsTracker);
	*tracker = dynamic_cast<cv::rapid::Tracker*>((*sharedPtr)->get());
	*algorithm = dynamic_cast<cv::Algorithm*>((*sharedPtr)->get());
	return (*sharedPtr)->get();
#else
	throw_no_rapid();
#endif
}
void cveOLSTrackerRelease(cv::Ptr<cv::rapid::OLSTracker>** sharedPtr)
{
#ifdef HAVE_OPENCV_RAPID
	delete* sharedPtr;
	*sharedPtr = 0;
#else
	throw_no_rapid();
#endif
}
