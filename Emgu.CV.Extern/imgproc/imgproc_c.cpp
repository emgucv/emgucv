//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "imgproc_c.h"

//GrabCut
void cveGrabCut(cv::_InputArray* img, cv::_InputOutputArray* mask, cv::Rect* rect, cv::_InputOutputArray* bgdModel, cv::_InputOutputArray* fgdModel, int iterCount, int flag)
{
	try
	{
		cv::grabCut(*img, mask ? *mask : cv::noArray(), *rect, *bgdModel, *fgdModel, iterCount, flag);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveFilter2D(cv::_InputArray* src, cv::_OutputArray* dst, cv::_InputArray* kernel, cv::Point* anchor, double delta, int borderType)
{
	try
	{
		CV_Assert(src->size() == dst->size() && src->channels() == dst->channels());
		cv::filter2D(*src, *dst, dst->depth(), *kernel, *anchor, delta, borderType);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveSepFilter2D(cv::_InputArray* src, cv::_OutputArray* dst, int ddepth, cv::_InputArray* kernelX, cv::_InputArray* kernelY, cv::Point* anchor, double delta, int borderType)
{
	try
	{
		cv::sepFilter2D(*src, *dst, ddepth, *kernelX, *kernelY, *anchor, delta, borderType);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveBlendLinear(cv::_InputArray* src1, cv::_InputArray* src2, cv::_InputArray* weights1, cv::_InputArray* weights2, cv::_OutputArray* dst)
{
	try
	{
		cv::blendLinear(*src1, *src2, *weights1, *weights2, *dst);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveCLAHE(cv::_InputArray* src, double clipLimit, cv::Size* tileGridSize, cv::_OutputArray* dst)
{
	try
	{
		cv::Size s(tileGridSize->width, tileGridSize->height);
		cv::Ptr<cv::CLAHE> clahe = cv::createCLAHE(clipLimit, s);
		clahe->apply(*src, *dst);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

/*
void cveAdaptiveBilateralFilter(cv::_InputArray* src, cv::_OutputArray* dst, CvSize* ksize, double sigmaSpace, double maxSigmaColor, CvPoint* anchor, int borderType)
{
   cv::Size s(ksize->width, ksize->height);
   cv::adaptiveBilateralFilter(*src, *dst, s, sigmaSpace, maxSigmaColor, *anchor, borderType);
}*/

void cveErode(cv::_InputArray* src, cv::_OutputArray* dst, cv::_InputArray* kernel, cv::Point* anchor, int iterations, int borderType, cv::Scalar* borderValue)
{
	try
	{
		cv::erode(*src, *dst, kernel ? *kernel : (cv::InputArray) cv::noArray(), *anchor, iterations, borderType, *borderValue);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveDilate(cv::_InputArray* src, cv::_OutputArray* dst, cv::_InputArray* kernel, cv::Point* anchor, int iterations, int borderType, cv::Scalar* borderValue)
{
	try
	{
		cv::dilate(*src, *dst, kernel ? *kernel : (cv::InputArray) cv::noArray(), *anchor, iterations, borderType, *borderValue);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveGetStructuringElement(cv::Mat* mat, int shape, cv::Size* ksize, cv::Point* anchor)
{
	try
	{
		cv::Size s(ksize->width, ksize->height);
		cv::Mat res = cv::getStructuringElement(shape, s, *anchor);
		cv::swap(*mat, res);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveMorphologyEx(cv::_InputArray* src, cv::_OutputArray* dst, int op, cv::_InputArray* kernel, cv::Point* anchor, int iterations, int borderType, cv::Scalar* borderValue)
{
	try
	{
		cv::morphologyEx(*src, *dst, op, *kernel, *anchor, iterations, borderType, *borderValue);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveSobel(cv::_InputArray* src, cv::_OutputArray* dst, int ddepth, int dx, int dy, int ksize, double scale, double delta, int borderType)
{
	try
	{
		cv::Sobel(*src, *dst, ddepth, dx, dy, ksize, scale, delta, borderType);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveSpatialGradient(cv::_InputArray* src, cv::_OutputArray* dx, cv::_OutputArray* dy, int ksize, int borderType)
{
	try
	{
		cv::spatialGradient(*src, *dx, *dy, ksize, borderType);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveScharr(cv::_InputArray* src, cv::_OutputArray* dst, int ddepth, int dx, int dy, double scale, double delta, int borderType)
{
	try
	{
		cv::Scharr(*src, *dst, ddepth, dx, dy, scale, delta, borderType);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveLaplacian(cv::_InputArray* src, cv::_OutputArray* dst, int ddepth, int ksize, double scale, double delta, int borderType)
{
	try
	{
		cv::Laplacian(*src, *dst, ddepth, ksize, scale, delta, borderType);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cvePyrUp(cv::_InputArray* src, cv::_OutputArray* dst, cv::Size* size, int borderType)
{
	try
	{
		cv::Size s(size->width, size->height);
		cv::pyrUp(*src, *dst, s, borderType);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cvePyrDown(cv::_InputArray* src, cv::_OutputArray* dst, cv::Size* size, int borderType)
{
	try
	{
		cv::Size s(size->width, size->height);
		cv::pyrDown(*src, *dst, s, borderType);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveBuildPyramid(cv::_InputArray* src, cv::_OutputArray* dst, int maxlevel, int borderType)
{
	try
	{
		cv::buildPyramid(*src, *dst, maxlevel, borderType);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveCanny(cv::_InputArray* image, cv::_OutputArray* edges, double threshold1, double threshold2, int apertureSize, bool L2gradient)
{
	try
	{
		cv::Canny(*image, *edges, threshold1, threshold2, apertureSize, L2gradient);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveCanny2(cv::_InputArray* dx, cv::_InputArray* dy, cv::_OutputArray* edges, double threshold1, double threshold2, bool L2gradient)
{
	try
	{
		cv::Canny(*dx, *dy, *edges, threshold1, threshold2, L2gradient);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveCornerEigenValsAndVecs(cv::_InputArray* src, cv::_OutputArray* dst, int blockSize, int ksize, int borderType)
{
	try
	{
		cv::cornerEigenValsAndVecs(*src, *dst, blockSize, ksize, borderType);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveCornerHarris(cv::_InputArray* src, cv::_OutputArray* dst, int blockSize, int ksize, double k, int borderType)
{
	try
	{
		cv::cornerHarris(*src, *dst, blockSize, ksize, k, borderType);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

double cveThreshold(
	cv::_InputArray* src, 
	cv::_OutputArray* dst, 
	double thresh, 
	double maxval, 
	int type)
{
	try
	{
		return cv::threshold(*src, *dst, thresh, maxval, type);
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
double cveThresholdWithMask(
	cv::_InputArray* src,
	cv::_InputOutputArray* dst,
	cv::_InputArray* mask,
	double thresh,
	double maxval,
	int type)
{
	try
	{
		return cv::thresholdWithMask(*src, *dst, *mask, thresh, maxval, type);
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

void cveWatershed(cv::_InputArray* image, cv::_InputOutputArray* markers)
{
	try
	{
		cv::watershed(*image, *markers);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveAdaptiveThreshold(cv::_InputArray* src, cv::_OutputArray* dst, double maxValue, int adaptiveMethod, int thresholdType, int blockSize, double c)
{
	try
	{
		cv::adaptiveThreshold(*src, *dst, maxValue, adaptiveMethod, thresholdType, blockSize, c);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveCvtColor(
	cv::_InputArray* src, 
	cv::_OutputArray* dst, 
	int code, 
	int dstCn, 
	int hint)
{
	try
	{
		cv::cvtColor(*src, *dst, code, dstCn, static_cast<cv::AlgorithmHint>(hint));
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveCvtColorTwoPlane(
	cv::_InputArray* src1, 
	cv::_InputArray* src2, 
	cv::_OutputArray* dst, 
	int code, 
	int hint)
{
	try
	{
		cv::cvtColorTwoPlane(*src1, *src2, *dst, code, static_cast<cv::AlgorithmHint>(hint));
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveDemosaicing(cv::_InputArray* src, cv::_OutputArray* dst, int code, int dstCn)
{
	try
	{
		cv::demosaicing(*src, *dst, code, dstCn);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveCopyMakeBorder(cv::_InputArray* src, cv::_OutputArray* dst, int top, int bottom, int left, int right, int borderType, cv::Scalar* value)
{
	try
	{
		cv::copyMakeBorder(*src, *dst, top, bottom, left, right, borderType, *value);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveIntegral(cv::_InputArray* src, cv::_OutputArray* sum, cv::_OutputArray* sqsum, cv::_OutputArray* tilted, int sdepth, int sqdepth)
{
	try
	{
		if (tilted)
		{
			cv::integral(*src, *sum, *sqsum, *tilted, sdepth, sqdepth);
		}
		else if (sqsum)
		{
			cv::integral(*src, *sum, *sqsum, sdepth, sqdepth);
		}
		else
		{
			cv::integral(*src, *sum, sdepth);
		}
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

int cveFloodFill(cv::_InputOutputArray* image, cv::_InputOutputArray* mask, cv::Point* seedPoint, cv::Scalar* newVal, cv::Rect* rect, cv::Scalar* loDiff, cv::Scalar* upDiff, int flags)
{
	try
	{
		cv::Rect r = *rect;
		int val = 0;
		if (mask)
			val = cv::floodFill(*image, *mask, *seedPoint, *newVal, &r, *loDiff, *upDiff, flags);
		else
			val = cv::floodFill(*image, *seedPoint, *newVal, &r, *loDiff, *upDiff, flags);

		rect->x = r.x;
		rect->y = r.y;
		rect->width = r.width;
		rect->height = r.height;
		return val;
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

void cvePyrMeanShiftFiltering(cv::_InputArray* src, cv::_OutputArray* dst, double sp, double sr, int maxLevel, cv::TermCriteria* termCrit)
{
	try
	{
		cv::pyrMeanShiftFiltering(*src, *dst, sp, sr, maxLevel, *termCrit);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveEqualizeHist(cv::_InputArray* src, cv::_OutputArray* dst)
{
	try
	{
		cv::equalizeHist(*src, *dst);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveAccumulate(cv::_InputArray* src, cv::_InputOutputArray* dst, cv::_InputArray* mask)
{
	try
	{
		cv::accumulate(*src, *dst, mask ? *mask : static_cast<cv::InputArray>(cv::noArray()));
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveAccumulateSquare(cv::_InputArray* src, cv::_InputOutputArray* dst, cv::_InputArray* mask)
{
	try
	{
		cv::accumulateSquare(*src, *dst, mask ? *mask : static_cast<cv::InputArray>(cv::noArray()));
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveAccumulateProduct(cv::_InputArray* src1, cv::_InputArray* src2, cv::_InputOutputArray* dst, cv::_InputArray* mask)
{
	try
	{
		cv::accumulateProduct(*src1, *src2, *dst, mask ? *mask : static_cast<cv::InputArray>(cv::noArray()));
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveAccumulateWeighted(cv::_InputArray* src, cv::_InputOutputArray* dst, double alpha, cv::_InputArray* mask)
{
	try
	{
		cv::accumulateWeighted(*src, *dst, alpha, mask ? *mask : static_cast<cv::InputArray>(cv::noArray()));
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cvePhaseCorrelate(cv::_InputArray* src1, cv::_InputArray* src2, cv::_InputArray* window, double* response, cv::Point2d* result)
{
	try
	{
		cv::Point2d pt = cv::phaseCorrelate(*src1, *src2, window ? *window : static_cast<cv::InputArray>(cv::noArray()), response);
		result->x = pt.x; result->y = pt.y;
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveCreateHanningWindow(cv::_OutputArray* dst, cv::Size* winSize, int type)
{
	try
	{
		cv::createHanningWindow(*dst, *winSize, type);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveResize(cv::_InputArray* src, cv::_OutputArray* dst, cv::Size* dsize, double fx, double fy, int interpolation)
{
	try
	{
		cv::resize(*src, *dst, *dsize, fx, fy, interpolation);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveWarpAffine(cv::_InputArray* src, cv::_OutputArray* dst, cv::_InputArray* m, cv::Size* dsize, int flags, int borderMode, cv::Scalar* borderValue)
{
	try
	{
		cv::warpAffine(*src, *dst, *m, *dsize, flags, borderMode, *borderValue);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveWarpPerspective(cv::_InputArray* src, cv::_OutputArray* dst, cv::_InputArray* m, cv::Size* dsize, int flags, int borderMode, cv::Scalar* borderValue)
{
	try
	{
		cv::warpPerspective(*src, *dst, *m, *dsize, flags, borderMode, *borderValue);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

/*
void cveLogPolar(cv::_InputArray* src, cv::_OutputArray* dst, cv::Point2f* center, double M, int flags)
{
	cv::logPolar(*src, *dst, *center, M, flags);
}
void cveLinearPolar(cv::_InputArray* src, cv::_OutputArray* dst, cv::Point2f* center, double maxRadius, int flags)
{
	cv::linearPolar(*src, *dst, *center, maxRadius, flags);
}
*/

void cveRemap(cv::_InputArray* src, cv::_OutputArray* dst, cv::_InputArray* map1, cv::_InputArray* map2, int interpolation, int borderMode, cv::Scalar* borderValue)
{
	try
	{
		cv::remap(*src, *dst, *map1, *map2, interpolation, borderMode, *borderValue);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveRepeat(cv::_InputArray* src, int ny, int nx, cv::_OutputArray* dst)
{
	try
	{
		cv::repeat(*src, ny, nx, *dst);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveHoughCircles(cv::_InputArray* image, cv::_OutputArray* circles, int method, double dp, double minDist, double param1, double param2, int minRadius, int maxRadius)
{
	try
	{
		cv::HoughCircles(*image, *circles, method, dp, minDist, param1, param2, minRadius, maxRadius);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveHoughLines(
	cv::_InputArray* image, 
	cv::_OutputArray* lines, 
	double rho, 
	double theta, 
	int threshold, 
	double srn, 
	double stn, 
	double minTheta,
	double maxTheta,
	bool useEdgeVal
	)
{
	try
	{
		cv::HoughLines(*image, *lines, rho, theta, threshold, srn, stn, minTheta, maxTheta, useEdgeVal);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveHoughLinesP(cv::_InputArray* image, cv::_OutputArray* lines, double rho, double theta, int threshold, double minLineLength, double maxGap)
{
	try
	{
		cv::HoughLinesP(*image, *lines, rho, theta, threshold, minLineLength, maxGap);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveMatchTemplate(cv::_InputArray* image, cv::_InputArray* templ, cv::_OutputArray* result, int method, cv::_InputArray* mask)
{
	try
	{
		cv::matchTemplate(*image, *templ, *result, method, mask ? *mask : static_cast<cv::InputArray>(cv::noArray()));
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveCornerSubPix(cv::_InputArray* image, cv::_InputOutputArray* corners, cv::Size* winSize, cv::Size* zeroZone, cv::TermCriteria* criteria)
{
	try
	{
		cv::cornerSubPix(*image, *corners, *winSize, *zeroZone, *criteria);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveConvertMaps(cv::_InputArray* map1, cv::_InputArray* map2, cv::_OutputArray* dstmap1, cv::_OutputArray* dstmap2, int dstmap1Type, bool nninterpolation)
{
	try
	{
		cv::convertMaps(
			*map1, 
			map2 ? *map2 : static_cast<cv::InputArray>(cv::noArray()), 
			*dstmap1, 
			dstmap2 ? *dstmap2 : static_cast<cv::OutputArray>(cv::noArray()), 
			dstmap1Type, 
			nninterpolation);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}


float cveEMD(cv::_InputArray* signature1, cv::_InputArray* signature2, int distType, cv::_InputArray* cost, float* lowerBound, cv::_OutputArray* flow)
{
	try
	{
		return cv::EMD(
			*signature1, 
			*signature2, 
			distType, 
			cost ? *cost : static_cast<cv::InputArray>(cv::noArray()), 
			lowerBound, flow ? *flow : static_cast<cv::OutputArray>(cv::noArray()));
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

void cveCalcHist(cv::_InputArray* images, const std::vector<int>* channels, cv::_InputArray* mask, cv::_OutputArray* hist, std::vector<int>* histSize, std::vector<float>* ranges, bool accumulate)
{
	try
	{
		cv::calcHist(
			*images, 
			*channels, 
			mask ? *mask : static_cast<cv::InputArray>(cv::noArray()), 
			*hist, 
			*histSize, 
			*ranges, 
			accumulate);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveCalcBackProject(cv::_InputArray* images, const std::vector<int>* channels, cv::_InputArray* hist, cv::_OutputArray* dst, const std::vector<float>* ranges, double scale)
{
	try
	{
		cv::calcBackProject(*images, *channels, *hist, *dst, *ranges, scale);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

double cveCompareHist(cv::_InputArray* h1, cv::_InputArray* h2, int method)
{
	try
	{
		return cv::compareHist(*h1, *h2, method);
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

void cveFindContours(cv::_InputOutputArray* image, cv::_OutputArray* contours, cv::_OutputArray* hierarchy, int mode, int method, cv::Point* offset)
{
	try
	{
		cv::findContours(*image, *contours, hierarchy ? *hierarchy : static_cast<cv::OutputArray>(cv::noArray()), mode, method, *offset);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveFindContoursLinkRuns(cv::_InputArray* image, cv::_OutputArray* contours, cv::_OutputArray* hierarchy)
{
	try
	{
		if (hierarchy)
		{
			cv::findContoursLinkRuns(*image, *contours);
		} else
		{
			cv::findContoursLinkRuns(*image, *contours, *hierarchy);
		}
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveDrawContours(
	cv::_InputOutputArray* image, 
	cv::_InputArray* contours, 
	int contourIdx,
	cv::Scalar* color, 
	int thickness, 
	int lineType, 
	cv::_InputArray* hierarchy,
	int maxLevel, 
	cv::Point* offset)
{
	try
	{
		cv::drawContours(
			*image, 
			*contours, 
			contourIdx, 
			*color, 
			thickness, 
			lineType, 
			hierarchy ? *hierarchy : (cv::_InputArray) cv::noArray(), 
			maxLevel, 
			*offset);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveGaussianBlur(
	cv::_InputArray* src, 
	cv::_OutputArray* dst, 
	cv::Size* ksize, 
	double sigmaX, 
	double sigmaY, 
	int borderType,
	int hint)
{
	try
	{
		cv::GaussianBlur(
			*src, 
			*dst, 
			*ksize, 
			sigmaX, 
			sigmaY, 
			borderType,
			static_cast<cv::AlgorithmHint>(hint));
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveBlur(cv::_InputArray* src, cv::_OutputArray* dst, cv::Size* kSize, cv::Point* anchor, int borderType)
{
	try
	{
		cv::blur(*src, *dst, *kSize, *anchor, borderType);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveStackBlur(cv::_InputArray* src, cv::_OutputArray* dst, cv::Size* ksize)
{
	try
	{
		cv::stackBlur(*src, *dst, *ksize);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveMedianBlur(cv::_InputArray* src, cv::_OutputArray* dst, int ksize)
{
	try
	{
		cv::medianBlur(*src, *dst, ksize);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveBoxFilter(cv::_InputArray* src, cv::_OutputArray* dst, int ddepth, cv::Size* ksize, cv::Point* anchor, bool normailize, int borderType)
{
	try
	{
		cv::boxFilter(*src, *dst, ddepth, *ksize, *anchor, normailize, borderType);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveSqrBoxFilter(cv::_InputArray* src, cv::_OutputArray* dst, int ddepth, cv::Size* ksize, cv::Point* anchor, bool normalize, int borderType)
{
	try
	{
		cv::sqrBoxFilter(*src, *dst, ddepth, *ksize, *anchor, normalize, borderType);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveBilateralFilter(cv::_InputArray* src, cv::_OutputArray* dst, int d, double sigmaColor, double sigmaSpace, int borderType)
{
	try
	{
		cv::bilateralFilter(*src, *dst, d, sigmaColor, sigmaSpace, borderType);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}


//LineIterator
cv::LineIterator* cveLineIteratorCreate(
	cv::Mat* img,
	cv::Point* pt1,
	cv::Point* pt2,
	int connectivity,
	bool leftToRight)
{
	try
	{
		return new cv::LineIterator(*img, *pt1, *pt2, connectivity, leftToRight);
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
uchar* cveLineIteratorGetDataPointer(cv::LineIterator* iterator)
{
	try
	{
		return *(*iterator);
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveLineIteratorPos(cv::LineIterator* iterator, cv::Point* pos)
{
	try
	{
		*pos = iterator->pos();
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveLineIteratorMoveNext(cv::LineIterator* iterator)
{
	try
	{
		++(*iterator);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveLineIteratorRelease(cv::LineIterator** iterator)
{
	try
	{
		delete* iterator;
		*iterator = 0;
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveLineIteratorSampleLine(
	cv::Mat* img,
	cv::Point* pt1,
	cv::Point* pt2,
	int connectivity,
	bool leftToRight,
	cv::Mat* result)
{
	try
	{
		cv::LineIterator li(*img, *pt1, *pt2, connectivity, leftToRight);
		result->create(li.count, 1, img->type());
		int elemSize = img->elemSize();
		for (int i = 0; i < li.count; ++li, i++)
		{
			memcpy(result->ptr(i), li.ptr, elemSize);
		}
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}


//Drawing
void cveLine(cv::_InputOutputArray* img, cv::Point* p1, cv::Point* p2, cv::Scalar* color, int thickness, int lineType, int shift)
{
	try
	{
		cv::line(*img, *p1, *p2, *color, thickness, lineType, shift);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveArrowedLine(cv::_InputOutputArray* img, cv::Point* pt1, cv::Point* pt2, cv::Scalar* color, int thickness, int lineType, int shift, double tipLength)
{
	try
	{
		cv::arrowedLine(*img, *pt1, *pt2, *color, thickness, lineType, shift, tipLength);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveRectangle(cv::_InputOutputArray* img, cv::Rect* rect, cv::Scalar* color, int thickness, int lineType, int shift)
{
	try
	{
		cv::Point p1(rect->x, rect->y);
		cv::Point p2(rect->x + rect->width, rect->y + rect->height);
		cv::rectangle(*img, p1, p2, *color, thickness, lineType, shift);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveCircle(cv::_InputOutputArray* img, cv::Point* center, int radius, cv::Scalar* color, int thickness, int lineType, int shift)
{
	try
	{
		cv::circle(*img, *center, radius, *color, thickness, lineType, shift);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cvePutText(cv::_InputOutputArray* img, cv::String* text, cv::Point* org, int fontFace, double fontScale, cv::Scalar* color, int thickness, int lineType, bool bottomLeftOrigin)
{
	try
	{
		cv::putText(*img, *text, *org, fontFace, fontScale, *color, thickness, lineType, bottomLeftOrigin);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveGetTextSize(cv::String* text, int fontFace, double fontScale, int thickness, int* baseLine, cv::Size* size)
{
	try
	{
		cv::Size s = cv::getTextSize(*text, fontFace, fontScale, thickness, baseLine);
		*size = s;
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveFillConvexPoly(cv::_InputOutputArray* img, cv::_InputArray* points, const cv::Scalar* color, int lineType, int shift)
{
	try
	{
		cv::fillConvexPoly(*img, *points, *color, lineType, shift);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveFillPoly(cv::_InputOutputArray* img, cv::_InputArray* pts, const cv::Scalar* color, int lineType, int shift, cv::Point* offset)
{
	try
	{
		cv::fillPoly(*img, *pts, *color, lineType, shift, *offset);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cvePolylines(cv::_InputOutputArray* img, cv::_InputArray* pts,
	bool isClosed, const cv::Scalar* color,
	int thickness, int lineType, int shift)
{
	try
	{
		cv::polylines(*img, *pts, isClosed, *color, thickness, lineType, shift);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveEllipse1(
	cv::_InputOutputArray* img,
	cv::Point* center,
	cv::Size* axes,
	double angle,
	double startAngle,
	double endAngle,
	const cv::Scalar* color,
	int thickness,
	int lineType,
	int shift)
{
	try
	{
		cv::ellipse(*img, *center, *axes, angle, startAngle, endAngle, *color, thickness, lineType, shift);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

CVAPI(void) cveEllipse2(
	cv::_InputOutputArray* img,
	cv::RotatedRect* box,
	cv::Scalar* color,
	int thickness,
	int lineType)
{
	try
	{
		cv::ellipse(*img, *box, *color, thickness, lineType);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveDrawMarker(
	cv::_InputOutputArray* img,
	cv::Point* position,
	cv::Scalar* color,
	int markerType,
	int markerSize,
	int thickness,
	int lineType)
{
	try
	{
		cv::drawMarker(*img, *position, *color, markerType, markerSize, thickness, lineType);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveApplyColorMap1(cv::_InputArray* src, cv::_OutputArray* dst, int colorMap)
{
	try
	{
		cv::applyColorMap(*src, *dst, colorMap);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveApplyColorMap2(cv::_InputArray* src, cv::_OutputArray* dst, cv::_InputArray userColorMap)
{
	try
	{
		cv::applyColorMap(*src, *dst, userColorMap);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}


void cveDistanceTransform(cv::_InputArray* src, cv::_OutputArray* dst, cv::_OutputArray* labels, int distanceType, int maskSize, int labelType)
{
	try
	{
		cv::distanceTransform(*src, *dst, labels ? *labels : cv::_OutputArray(), distanceType, maskSize, labelType);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}



void cveGetRectSubPix(cv::_InputArray* image, cv::Size* patchSize, cv::Point2f* center, cv::_OutputArray* patch, int patchType)
{
	try
	{
		cv::getRectSubPix(*image, *patchSize, *center, *patch, patchType);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

int cveConnectedComponents(cv::_InputArray* image, cv::_OutputArray* labels, int connectivity, int ltype, int ccltype)
{
	try
	{
		return cv::connectedComponents(*image, *labels, connectivity, ltype, ccltype);
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
int cveConnectedComponentsWithStats(cv::_InputArray* image, cv::_OutputArray* labels, cv::_OutputArray* stats, cv::_OutputArray* centroids, int connectivity, int ltype, int ccltype)
{
	try
	{
		return cv::connectedComponentsWithStats(*image, *labels, *stats, *centroids, connectivity, ltype, ccltype);
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

void cveGetGaussianKernel(
	int ksize,
	double sigma,
	int ktype,
	cv::Mat* result)
{
	try
	{
		cv::Mat m = cv::getGaussianKernel(
			ksize,
			sigma,
			ktype);
		cv::swap(m, *result);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveGetDerivKernels(
	cv::_OutputArray* kx,
	cv::_OutputArray* ky,
	int dx,
	int dy,
	int ksize,
	bool normalize,
	int ktype)
{
	try
	{
		cv::getDerivKernels(*kx, *ky, dx, dy, ksize, normalize, ktype);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveGetGaborKernel(
	cv::Size* ksize,
	double sigma,
	double theta,
	double lambd,
	double gamma,
	double psi,
	int ktype,
	cv::Mat* result)
{
	try
	{
		cv::Mat m = cv::getGaborKernel(*ksize, sigma, theta, lambd, gamma, psi, ktype);
		cv::swap(m, *result);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

cv::FontFace* cveFontFaceCreate(cv::String* fontPathOrName)
{
	try
	{
		return new cv::FontFace(*fontPathOrName);
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveFontFaceRelease(cv::FontFace** fontFace)
{
	try
	{
		delete *fontFace;
		*fontFace = 0;
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
bool cveFontFaceSet(cv::FontFace* fontFace, cv::String* fontPathOrName)
{
	try
	{
		return fontFace->set(*fontPathOrName);
	}
	CVAPI_CATCH_CV_ERRORS(false)
}
void cveFontFaceGetName(cv::FontFace* fontFace, cv::String* name)
{
	try
	{
		*name = fontFace->getName();
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
bool cveFontFaceSetInstance(cv::FontFace* fontFace, std::vector<int>* params)
{
	try
	{
		return fontFace->setInstance(*params);
	}
	CVAPI_CATCH_CV_ERRORS(false)
}
bool cveFontFaceGetInstance(cv::FontFace* fontFace, std::vector<int>* params)
{
	try
	{
		return fontFace->getInstance(*params);
	}
	CVAPI_CATCH_CV_ERRORS(false)
}

void cvePutTextFontFace(
	cv::_InputOutputArray* img,
	cv::String* text,
	cv::Point* org,
	cv::Scalar* color,
	cv::FontFace* fface,
	int size,
	int weight,
	int flags,
	cv::Range* wrap,
	cv::Point* result)
{
	try
	{
		*result = cv::putText(*img, *text, *org, *color, *fface, size, weight, static_cast<cv::PutTextFlags>(flags), *wrap);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveGetTextSizeFontFace(
	cv::Size* imgsize,
	cv::String* text,
	cv::Point* org,
	cv::FontFace* fface,
	int size,
	int weight,
	int flags,
	cv::Range* wrap,
	cv::Rect* result)
{
	try
	{
		*result = cv::getTextSize(*imgsize, *text, *org, *fface, size, weight, static_cast<cv::PutTextFlags>(flags), *wrap);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

