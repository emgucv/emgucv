//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_RAPID_C_H
#define EMGU_RAPID_C_H

#include "opencv2/core/core_c.h"

#ifdef HAVE_OPENCV_RAPID
#include "opencv2/rapid.hpp"
#else
static inline CV_NORETURN void throw_no_rapid() { CV_Error(cv::Error::StsBadFunc, "The library is compiled without rapid support"); }

namespace cv {
	namespace rapid {
		class Tracker {};
		class Rapid {};
		class OLSTracker {};
	}
}
#endif
CVAPI(void) cveDrawCorrespondencies(
	cv::_InputOutputArray* bundle,
	cv::_InputArray* cols,
	cv::_InputArray* colors);

CVAPI(void) cveDrawSearchLines(
	cv::_InputOutputArray* img,
	cv::_InputArray* locations,
	CvScalar* color);

CVAPI(void) cveDrawWireframe(
	cv::_InputOutputArray* img,
	cv::_InputArray* pts2d,
	cv::_InputArray* tris,
	CvScalar* color,
	int type,
	bool cullBackface);

CVAPI(void) cveExtractControlPoints(
	int num,
	int len,
	cv::_InputArray* pts3d,
	cv::_InputArray* rvec,
	cv::_InputArray* tvec,
	cv::_InputArray* K,
	CvSize* imsize,
	cv::_InputArray* tris,
	cv::_OutputArray* ctl2d,
	cv::_OutputArray* ctl3d);

CVAPI(void) cveExtractLineBundle(
	int len,
	cv::_InputArray* ctl2d,
	cv::_InputArray* img,
	cv::_OutputArray* bundle,
	cv::_OutputArray* srcLocations);

CVAPI(void) cveFindCorrespondencies(
	cv::_InputArray* bundle,
	cv::_OutputArray* cols,
	cv::_OutputArray* response);

CVAPI(void) cveConvertCorrespondencies(
	cv::_InputArray* cols,
	cv::_InputArray* srcLocations,
	cv::_OutputArray* pts2d,
	cv::_InputOutputArray* pts3d,
	cv::_InputArray* mask);

CVAPI(float) cveRapid(
	cv::_InputArray* img,
	int num,
	int len,
	cv::_InputArray* pts3d,
	cv::_InputArray* tris,
	cv::_InputArray* K,
	cv::_InputOutputArray* rvec,
	cv::_InputOutputArray* tvec,
	double* rmsd);

CVAPI(float) cveTrackerCompute(
	cv::rapid::Tracker* tracker,
	cv::_InputArray* img,
	int num,
	int len,
	cv::_InputArray* K,
	cv::_InputOutputArray* rvec,
	cv::_InputOutputArray* tvec,
	CvTermCriteria* termcrit);

CVAPI(void) cveTrackerClearState(cv::rapid::Tracker* tracker);

CVAPI(cv::rapid::Rapid*) cveRapidCreate(cv::_InputArray* pts3d, cv::_InputArray* tris, cv::rapid::Tracker** tracker, cv::Algorithm** algorithm, cv::Ptr<cv::rapid::Rapid>** sharedPtr);
CVAPI(void) cveRapidRelease(cv::Ptr<cv::rapid::Rapid>** sharedPtr);

CVAPI(cv::rapid::OLSTracker*) cveOLSTrackerCreate(cv::_InputArray* pts3d, cv::_InputArray* tris, int histBins, uchar sobelThesh, cv::rapid::Tracker** tracker, cv::Algorithm** algorithm, cv::Ptr<cv::rapid::OLSTracker>** sharedPtr);
CVAPI(void) cveOLSTrackerRelease(cv::Ptr<cv::rapid::OLSTracker>** sharedPtr);

#endif