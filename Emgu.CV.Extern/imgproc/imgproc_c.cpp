//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "imgproc_c.h"

IplImage* cvGetImageSubRect(IplImage* image, CvRect* rect)
{
	IplImage* res = cvCreateImageHeader(cvSize(rect->width, rect->height), image->depth, image->nChannels);
	CvMat mat;
	cvGetSubRect(image, &mat, *rect);
	cvGetImage(&mat, res);
	return res;
}

//GrabCut
void cveGrabCut(cv::_InputArray* img, cv::_InputOutputArray* mask, cv::Rect* rect, cv::_InputOutputArray* bgdModel, cv::_InputOutputArray* fgdModel, int iterCount, int flag)
{
	cv::grabCut(*img, mask ? *mask : cv::noArray(), *rect, *bgdModel, *fgdModel, iterCount, flag);
}

void cveFilter2D(cv::_InputArray* src, cv::_OutputArray* dst, cv::_InputArray* kernel, CvPoint* anchor, double delta, int borderType)
{
	CV_Assert(src->size() == dst->size() && src->channels() == dst->channels());
	cv::filter2D(*src, *dst, dst->depth(), *kernel, *anchor, delta, borderType);
}

void cveSepFilter2D(cv::_InputArray* src, cv::_OutputArray* dst, int ddepth, cv::_InputArray* kernelX, cv::_InputArray* kernelY, CvPoint* anchor, double delta, int borderType)
{
	cv::sepFilter2D(*src, *dst, ddepth, *kernelX, *kernelY, *anchor, delta, borderType);
}

void cveBlendLinear(cv::_InputArray* src1, cv::_InputArray* src2, cv::_InputArray* weights1, cv::_InputArray* weights2, cv::_OutputArray* dst)
{
	cv::blendLinear(*src1, *src2, *weights1, *weights2, *dst);
}

void cveCLAHE(cv::_InputArray* src, double clipLimit, CvSize* tileGridSize, cv::_OutputArray* dst)
{
	cv::Size s(tileGridSize->width, tileGridSize->height);
	cv::Ptr<cv::CLAHE> clahe = cv::createCLAHE(clipLimit, s);
	clahe->apply(*src, *dst);
}

/*
void cveAdaptiveBilateralFilter(cv::_InputArray* src, cv::_OutputArray* dst, CvSize* ksize, double sigmaSpace, double maxSigmaColor, CvPoint* anchor, int borderType)
{
   cv::Size s(ksize->width, ksize->height);
   cv::adaptiveBilateralFilter(*src, *dst, s, sigmaSpace, maxSigmaColor, *anchor, borderType);
}*/

void cveErode(cv::_InputArray* src, cv::_OutputArray* dst, cv::_InputArray* kernel, CvPoint* anchor, int iterations, int borderType, CvScalar* borderValue)
{
	cv::erode(*src, *dst, kernel ? *kernel : (cv::InputArray) cv::noArray(), *anchor, iterations, borderType, *borderValue);
}

void cveDilate(cv::_InputArray* src, cv::_OutputArray* dst, cv::_InputArray* kernel, CvPoint* anchor, int iterations, int borderType, CvScalar* borderValue)
{
	cv::dilate(*src, *dst, kernel ? *kernel : (cv::InputArray) cv::noArray(), *anchor, iterations, borderType, *borderValue);
}
void cveGetStructuringElement(cv::Mat* mat, int shape, CvSize* ksize, CvPoint* anchor)
{
	cv::Size s(ksize->width, ksize->height);
	cv::Mat res = cv::getStructuringElement(shape, s, *anchor);
	cv::swap(*mat, res);
}
void cveMorphologyEx(cv::_InputArray* src, cv::_OutputArray* dst, int op, cv::_InputArray* kernel, CvPoint* anchor, int iterations, int borderType, CvScalar* borderValue)
{
	cv::morphologyEx(*src, *dst, op, *kernel, *anchor, iterations, borderType, *borderValue);
}

void cveSobel(cv::_InputArray* src, cv::_OutputArray* dst, int ddepth, int dx, int dy, int ksize, double scale, double delta, int borderType)
{
	cv::Sobel(*src, *dst, ddepth, dx, dy, ksize, scale, delta, borderType);
}

void cveSpatialGradient(cv::_InputArray* src, cv::_OutputArray* dx, cv::_OutputArray* dy, int ksize, int borderType)
{
	cv::spatialGradient(*src, *dx, *dy, ksize, borderType);
}

void cveScharr(cv::_InputArray* src, cv::_OutputArray* dst, int ddepth, int dx, int dy, double scale, double delta, int borderType)
{
	cv::Scharr(*src, *dst, ddepth, dx, dy, scale, delta, borderType);
}

void cveLaplacian(cv::_InputArray* src, cv::_OutputArray* dst, int ddepth, int ksize, double scale, double delta, int borderType)
{
	cv::Laplacian(*src, *dst, ddepth, ksize, scale, delta, borderType);
}

void cvePyrUp(cv::_InputArray* src, cv::_OutputArray* dst, CvSize* size, int borderType)
{
	cv::Size s(size->width, size->height);
	cv::pyrUp(*src, *dst, s, borderType);
}
void cvePyrDown(cv::_InputArray* src, cv::_OutputArray* dst, CvSize* size, int borderType)
{
	cv::Size s(size->width, size->height);
	cv::pyrDown(*src, *dst, s, borderType);
}
void cveBuildPyramid(cv::_InputArray* src, cv::_OutputArray* dst, int maxlevel, int borderType)
{
	cv::buildPyramid(*src, *dst, maxlevel, borderType);
}

void cveCanny(cv::_InputArray* image, cv::_OutputArray* edges, double threshold1, double threshold2, int apertureSize, bool L2gradient)
{
	cv::Canny(*image, *edges, threshold1, threshold2, apertureSize, L2gradient);
}

void cveCanny2(cv::_InputArray* dx, cv::_InputArray* dy, cv::_OutputArray* edges, double threshold1, double threshold2, bool L2gradient)
{
	cv::Canny(*dx, *dy, *edges, threshold1, threshold2, L2gradient);
}

void cveCornerHarris(cv::_InputArray* src, cv::_OutputArray* dst, int blockSize, int ksize, double k, int borderType)
{
	cv::cornerHarris(*src, *dst, blockSize, ksize, k, borderType);
}

double cveThreshold(cv::_InputArray* src, cv::_OutputArray* dst, double thresh, double maxval, int type)
{
	return cv::threshold(*src, *dst, thresh, maxval, type);
}
void cveWatershed(cv::_InputArray* image, cv::_InputOutputArray* markers)
{
	cv::watershed(*image, *markers);
}
void cveAdaptiveThreshold(cv::_InputArray* src, cv::_OutputArray* dst, double maxValue, int adaptiveMethod, int thresholdType, int blockSize, double c)
{
	cv::adaptiveThreshold(*src, *dst, maxValue, adaptiveMethod, thresholdType, blockSize, c);
}
void cveCvtColor(cv::_InputArray* src, cv::_OutputArray* dst, int code, int dstCn)
{
	cv::cvtColor(*src, *dst, code, dstCn);
}
void cveCopyMakeBorder(cv::_InputArray* src, cv::_OutputArray* dst, int top, int bottom, int left, int right, int borderType, CvScalar* value)
{
	cv::copyMakeBorder(*src, *dst, top, bottom, left, right, borderType, *value);
}

void cveIntegral(cv::_InputArray* src, cv::_OutputArray* sum, cv::_OutputArray* sqsum, cv::_OutputArray* tilted, int sdepth, int sqdepth)
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

int cveFloodFill(cv::_InputOutputArray* image, cv::_InputOutputArray* mask, CvPoint* seedPoint, CvScalar* newVal, CvRect* rect, CvScalar* loDiff, CvScalar* upDiff, int flags)
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

void cvePyrMeanShiftFiltering(cv::_InputArray* src, cv::_OutputArray* dst, double sp, double sr, int maxLevel, CvTermCriteria* termCrit)
{
	cv::pyrMeanShiftFiltering(*src, *dst, sp, sr, maxLevel, *termCrit);
}

void cveMoments(cv::_InputArray* arr, bool binaryImage, cv::Moments* moments)
{
	cv::Moments m = cv::moments(*arr, binaryImage);
	memcpy(moments, &m, sizeof(cv::Moments));
}
void cveEqualizeHist(cv::_InputArray* src, cv::_OutputArray* dst)
{
	cv::equalizeHist(*src, *dst);
}

void cveAccumulate(cv::_InputArray* src, cv::_InputOutputArray* dst, cv::_InputArray* mask)
{
	cv::accumulate(*src, *dst, mask ? *mask : (cv::InputArray) cv::noArray());
}
void cveAccumulateSquare(cv::_InputArray* src, cv::_InputOutputArray* dst, cv::_InputArray* mask)
{
	cv::accumulateSquare(*src, *dst, mask ? *mask : (cv::InputArray) cv::noArray());
}
void cveAccumulateProduct(cv::_InputArray* src1, cv::_InputArray* src2, cv::_InputOutputArray* dst, cv::_InputArray* mask)
{
	cv::accumulateProduct(*src1, *src2, *dst, mask ? *mask : (cv::InputArray) cv::noArray());
}
void cveAccumulateWeighted(cv::_InputArray* src, cv::_InputOutputArray* dst, double alpha, cv::_InputArray* mask)
{
	cv::accumulateWeighted(*src, *dst, alpha, mask ? *mask : (cv::InputArray) cv::noArray());
}
void cvePhaseCorrelate(cv::_InputArray* src1, cv::_InputArray* src2, cv::_InputArray* window, double* response, CvPoint2D64f* result)
{
	cv::Point2d pt = cv::phaseCorrelate(*src1, *src2, window ? *window : (cv::InputArray) cv::noArray(), response);
	result->x = pt.x; result->y = pt.y;
}
void cveCreateHanningWindow(cv::_OutputArray* dst, CvSize* winSize, int type)
{
	cv::createHanningWindow(*dst, *winSize, type);
}

void cveResize(cv::_InputArray* src, cv::_OutputArray* dst, CvSize* dsize, double fx, double fy, int interpolation)
{
	cv::resize(*src, *dst, *dsize, fx, fy, interpolation);
}
void cveWarpAffine(cv::_InputArray* src, cv::_OutputArray* dst, cv::_InputArray* m, CvSize* dsize, int flags, int borderMode, CvScalar* borderValue)
{
	cv::warpAffine(*src, *dst, *m, *dsize, flags, borderMode, *borderValue);
}
void cveWarpPerspective(cv::_InputArray* src, cv::_OutputArray* dst, cv::_InputArray* m, CvSize* dsize, int flags, int borderMode, CvScalar* borderValue)
{
	cv::warpPerspective(*src, *dst, *m, *dsize, flags, borderMode, *borderValue);
}

void cveLogPolar(cv::_InputArray* src, cv::_OutputArray* dst, CvPoint2D32f* center, double M, int flags)
{
	cv::logPolar(*src, *dst, *center, M, flags);
}
void cveLinearPolar(cv::_InputArray* src, cv::_OutputArray* dst, CvPoint2D32f* center, double maxRadius, int flags)
{
	cv::linearPolar(*src, *dst, *center, maxRadius, flags);
}
void cveRemap(cv::_InputArray* src, cv::_OutputArray* dst, cv::_InputArray* map1, cv::_InputArray* map2, int interpolation, int borderMode, CvScalar* borderValue)
{
	cv::remap(*src, *dst, *map1, *map2, interpolation, borderMode, *borderValue);
}
void cveRepeat(cv::_InputArray* src, int ny, int nx, cv::_OutputArray* dst)
{
	cv::repeat(*src, ny, nx, *dst);
}
void cveHoughCircles(cv::_InputArray* image, cv::_OutputArray* circles, int method, double dp, double minDist, double param1, double param2, int minRadius, int maxRadius)
{
	cv::HoughCircles(*image, *circles, method, dp, minDist, param1, param2, minRadius, maxRadius);
}
void cveHoughLines(cv::_InputArray* image, cv::_OutputArray* lines, double rho, double theta, int threshold, double srn, double stn)
{
	cv::HoughLines(*image, *lines, rho, theta, threshold, srn, stn);
}
void cveHoughLinesP(cv::_InputArray* image, cv::_OutputArray* lines, double rho, double theta, int threshold, double minLineLength, double maxGap)
{
	cv::HoughLinesP(*image, *lines, rho, theta, threshold, minLineLength, maxGap);
}

void cveMatchTemplate(cv::_InputArray* image, cv::_InputArray* templ, cv::_OutputArray* result, int method, cv::_InputArray* mask)
{
	cv::matchTemplate(*image, *templ, *result, method, mask ? *mask : (cv::InputArray) cv::noArray());
}
void cveCornerSubPix(cv::_InputArray* image, cv::_InputOutputArray* corners, CvSize* winSize, CvSize* zeroZone, CvTermCriteria* criteria)
{
	cv::cornerSubPix(*image, *corners, *winSize, *zeroZone, *criteria);
}

void cveConvertMaps(cv::_InputArray* map1, cv::_InputArray* map2, cv::_OutputArray* dstmap1, cv::_OutputArray* dstmap2, int dstmap1Type, bool nninterpolation)
{
	cv::convertMaps(*map1, map2 ? *map2 : (cv::InputArray) cv::noArray(), *dstmap1, dstmap2 ? *dstmap2 : (cv::OutputArray) cv::noArray(), dstmap1Type, nninterpolation);
}


void cveGetAffineTransform(cv::_InputArray* src, cv::_InputArray* dst, cv::Mat* affine)
{
	cv::Mat result = cv::getAffineTransform(*src, *dst);
	cv::swap(result, *affine);
}

void cveGetPerspectiveTransform(cv::_InputArray* src, cv::_InputArray* dst, cv::Mat* perspective)
{
	cv::Mat result = cv::getPerspectiveTransform(*src, *dst);
	cv::swap(result, *perspective);
}

void cveInvertAffineTransform(cv::_InputArray* m, cv::_OutputArray* im)
{
	cv::invertAffineTransform(*m, *im);
}

void cveEMD(cv::_InputArray* signature1, cv::_InputArray* signature2, int distType, cv::_InputArray* cost, float* lowerBound, cv::_OutputArray* flow)
{
	cv::EMD(
		*signature1, 
		*signature2, 
		distType, 
		cost ? *cost : static_cast<cv::InputArray>(cv::noArray()), 
		lowerBound, flow ? *flow : static_cast<cv::OutputArray>(cv::noArray()));
}

void cveCalcHist(cv::_InputArray* images, const std::vector<int>* channels, cv::_InputArray* mask, cv::_OutputArray* hist, std::vector<int>* histSize, std::vector<float>* ranges, bool accumulate)
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

void cveCalcBackProject(cv::_InputArray* images, const std::vector<int>* channels, cv::_InputArray* hist, cv::_OutputArray* dst, const std::vector<float>* ranges, double scale)
{
	cv::calcBackProject(*images, *channels, *hist, *dst, *ranges, scale);
}

double cveCompareHist(cv::_InputArray* h1, cv::_InputArray* h2, int method)
{
	return cv::compareHist(*h1, *h2, method);
}

void cveGetRotationMatrix2D(CvPoint2D32f* center, double angle, double scale, cv::_OutputArray* rotationMatrix2D)
{
	cv::Mat r = cv::getRotationMatrix2D(*center, angle, scale);
	if (rotationMatrix2D->empty() || r.type() == rotationMatrix2D->type())
		r.copyTo(*rotationMatrix2D);
	else
		r.convertTo(*rotationMatrix2D, rotationMatrix2D->type());
}

void cveFindContours(cv::_InputOutputArray* image, cv::_OutputArray* contours, cv::_OutputArray* hierarchy, int mode, int method, CvPoint* offset)
{
	cv::findContours(*image, *contours, hierarchy ? *hierarchy : (cv::OutputArray) cv::noArray(), mode, method, *offset);
}

double cvePointPolygonTest(cv::_InputArray* contour, CvPoint2D32f* pt, bool measureDist)
{
	return cv::pointPolygonTest(*contour, *pt, measureDist);
}

double cveContourArea(cv::_InputArray* contour, bool oriented)
{
	return cv::contourArea(*contour, oriented);
}

bool cveIsContourConvex(cv::_InputArray* contour)
{
	return cv::isContourConvex(*contour);
}
float cveIntersectConvexConvex(cv::_InputArray* p1, cv::_InputArray* p2, cv::_OutputArray* p12, bool handleNested)
{
	return cv::intersectConvexConvex(*p1, *p2, *p12, handleNested);
}
void cveBoundingRectangle(cv::_InputArray* points, CvRect* boundingRect)
{
	cv::Rect rect = cv::boundingRect(*points);
	boundingRect->x = rect.x;
	boundingRect->y = rect.y;
	boundingRect->width = rect.width;
	boundingRect->height = rect.height;
}
double cveArcLength(cv::_InputArray* curve, bool closed)
{
	return cv::arcLength(*curve, closed);
}
void cveMinAreaRect(cv::_InputArray* points, CvBox2D* box)
{
	cv::RotatedRect rr = cv::minAreaRect(*points);
	box->center = cvPoint2D32f(rr.center.x, rr.center.y);
	box->size = cvSize2D32f(rr.size.width, rr.size.height);
	box->angle = rr.angle;
}
void cveBoxPoints(CvBox2D* box, cv::_OutputArray* points)
{
	cv::boxPoints(*box, *points);
}
double cveMinEnclosingTriangle(cv::_InputArray* points, cv::_OutputArray* triangle)
{
	return cv::minEnclosingTriangle(*points, *triangle);
}
void cveMinEnclosingCircle(cv::_InputArray* points, CvPoint2D32f* center, float* radius)
{
	cv::Point2f c; float r;
	cv::minEnclosingCircle(*points, c, r);
	center->x = c.x;
	center->y = c.y;
	*radius = r;
}
double cveMatchShapes(cv::_InputArray* contour1, cv::_InputArray* contour2, int method, double parameter)
{
	return cv::matchShapes(*contour1, *contour2, method, parameter);
}
void cveFitEllipse(cv::_InputArray* points, CvBox2D* box)
{
	cv::RotatedRect rect = cv::fitEllipse(*points);
	box->center = cvPoint2D32f(rect.center.x, rect.center.y);
	box->size = cvSize2D32f(rect.size.width, rect.size.height);
	box->angle = rect.angle;
}
void cveFitEllipseAMS(cv::_InputArray* points, CvBox2D* box)
{
	cv::RotatedRect rect = cv::fitEllipseAMS(*points);
	box->center = cvPoint2D32f(rect.center.x, rect.center.y);
	box->size = cvSize2D32f(rect.size.width, rect.size.height);
	box->angle = rect.angle;
}
void cveFitEllipseDirect(cv::_InputArray* points, CvBox2D* box)
{
	cv::RotatedRect rect = cv::fitEllipseDirect(*points);
	box->center = cvPoint2D32f(rect.center.x, rect.center.y);
	box->size = cvSize2D32f(rect.size.width, rect.size.height);
	box->angle = rect.angle;
}

void cveFitLine(cv::_InputArray* points, cv::_OutputArray* line, int distType, double param, double reps, double aeps)
{
	cv::fitLine(*points, *line, distType, param, reps, aeps);
}
int cveRotatedRectangleIntersection(CvBox2D* rect1, CvBox2D* rect2, cv::_OutputArray* intersectingRegion)
{
	cv::RotatedRect r1 = *rect1;
	cv::RotatedRect r2 = *rect2;
	return cv::rotatedRectangleIntersection(r1, r2, *intersectingRegion);
}
void cveDrawContours(
	cv::_InputOutputArray* image, cv::_InputArray* contours, int contourIdx,
	CvScalar* color, int thickness, int lineType, cv::_InputArray* hierarchy,
	int maxLevel, CvPoint* offset)
{
	cv::drawContours(*image, *contours, contourIdx, *color, thickness, lineType, hierarchy ? *hierarchy : (cv::_InputArray) cv::noArray(), maxLevel, *offset);
}
void cveApproxPolyDP(cv::_InputArray* curve, cv::_OutputArray* approxCurve, double epsilon, bool closed)
{
	cv::approxPolyDP(*curve, *approxCurve, epsilon, closed);
}
void cveConvexHull(cv::_InputArray* points, cv::_OutputArray* hull, bool clockwise, bool returnPoints)
{
	cv::convexHull(*points, *hull, clockwise, returnPoints);
}
void cveConvexityDefects(cv::_InputArray* contour, cv::_InputArray* convexhull, cv::_OutputArray* convexityDefects)
{
	cv::convexityDefects(*contour, *convexhull, *convexityDefects);
}

void cveGaussianBlur(cv::_InputArray* src, cv::_OutputArray* dst, CvSize* ksize, double sigmaX, double sigmaY, int borderType)
{
	cv::GaussianBlur(*src, *dst, *ksize, sigmaX, sigmaY, borderType);
}
void cveBlur(cv::_InputArray* src, cv::_OutputArray* dst, CvSize* kSize, CvPoint* anchor, int borderType)
{
	cv::blur(*src, *dst, *kSize, *anchor, borderType);
}
void cveMedianBlur(cv::_InputArray* src, cv::_OutputArray* dst, int ksize)
{
	cv::medianBlur(*src, *dst, ksize);
}
void cveBoxFilter(cv::_InputArray* src, cv::_OutputArray* dst, int ddepth, CvSize* ksize, CvPoint* anchor, bool normailize, int borderType)
{
	cv::boxFilter(*src, *dst, ddepth, *ksize, *anchor, normailize, borderType);
}
void cveSqrBoxFilter(cv::_InputArray* src, cv::_OutputArray* dst, int ddepth, CvSize* ksize, CvPoint* anchor, bool normalize, int borderType)
{
	cv::sqrBoxFilter(*src, *dst, ddepth, *ksize, *anchor, normalize, borderType);
}
void cveBilateralFilter(cv::_InputArray* src, cv::_OutputArray* dst, int d, double sigmaColor, double sigmaSpace, int borderType)
{
	cv::bilateralFilter(*src, *dst, d, sigmaColor, sigmaSpace, borderType);
}


//Subdiv2D
cv::Subdiv2D* cveSubdiv2DCreate(CvRect* rect)
{
	return new cv::Subdiv2D(*rect);
}
void cveSubdiv2DRelease(cv::Subdiv2D** subdiv)
{
	delete* subdiv;
	*subdiv = 0;
}
void cveSubdiv2DInsertMulti(cv::Subdiv2D* subdiv, std::vector<cv::Point2f>* points)
{
	subdiv->insert(*points);
}
int cveSubdiv2DInsertSingle(cv::Subdiv2D* subdiv, CvPoint2D32f* pt)
{
	return subdiv->insert(*pt);
}
void cveSubdiv2DGetTriangleList(cv::Subdiv2D* subdiv, std::vector<cv::Vec6f>* triangleList)
{
	subdiv->getTriangleList(*triangleList);
}
void cveSubdiv2DGetVoronoiFacetList(cv::Subdiv2D* subdiv, std::vector<int>* idx, std::vector< std::vector< cv::Point2f> >* facetList, std::vector< cv::Point2f >* facetCenters)
{
	subdiv->getVoronoiFacetList(*idx, *facetList, *facetCenters);
}
int cveSubdiv2DFindNearest(cv::Subdiv2D* subdiv, CvPoint2D32f* pt, CvPoint2D32f* nearestPt)
{
	cv::Point2f np;
	int result = subdiv->findNearest(*pt, &np);
	*nearestPt = cvPoint2D32f(np);
	return result;
}
int cveSubdiv2DLocate(cv::Subdiv2D* subdiv, CvPoint2D32f* pt, int* edge, int* vertex)
{
	int e = 0, v = 0;
	int result = subdiv->locate(*pt, e, v);
	*edge = e;
	*vertex = v;
	return result;
}

//LineIterator
cv::LineIterator* cveLineIteratorCreate(
	cv::Mat* img,
	CvPoint* pt1,
	CvPoint* pt2,
	int connectivity,
	bool leftToRight)
{
	return new cv::LineIterator(*img, *pt1, *pt2, connectivity, leftToRight);
}
uchar* cveLineIteratorGetDataPointer(cv::LineIterator* iterator)
{
	return *(*iterator);
}
void cveLineIteratorPos(cv::LineIterator* iterator, CvPoint* pos)
{
	*pos = cvPoint(iterator->pos());
}
void cveLineIteratorMoveNext(cv::LineIterator* iterator)
{
	++(*iterator);
}
void cveLineIteratorRelease(cv::LineIterator** iterator)
{
	delete* iterator;
	*iterator = 0;
}

void cveLineIteratorSampleLine(
	cv::Mat* img,
	CvPoint* pt1,
	CvPoint* pt2,
	int connectivity,
	bool leftToRight,
	cv::Mat* result)
{
	cv::LineIterator li(*img, *pt1, *pt2, connectivity, leftToRight);
	result->create(li.count, 1, img->type());
	int elemSize = img->elemSize();
	for (int i = 0; i < li.count; ++li, i++)
	{
		memcpy(result->ptr(i), li.ptr, elemSize);
	}
}


//Drawing
void cveLine(cv::_InputOutputArray* img, CvPoint* p1, CvPoint* p2, CvScalar* color, int thickness, int lineType, int shift)
{
	cv::line(*img, *p1, *p2, *color, thickness, lineType, shift);
}

void cveArrowedLine(cv::_InputOutputArray* img, CvPoint* pt1, CvPoint* pt2, CvScalar* color, int thickness, int lineType, int shift, double tipLength)
{
	cv::arrowedLine(*img, *pt1, *pt2, *color, thickness, lineType, shift, tipLength);
}

void cveRectangle(cv::_InputOutputArray* img, CvRect* rect, CvScalar* color, int thickness, int lineType, int shift)
{
	cv::Point p1(rect->x, rect->y);
	cv::Point p2(rect->x + rect->width, rect->y + rect->height);
	cv::rectangle(*img, p1, p2, *color, thickness, lineType, shift);
}

void cveCircle(cv::_InputOutputArray* img, CvPoint* center, int radius, CvScalar* color, int thickness, int lineType, int shift)
{
	cv::circle(*img, *center, radius, *color, thickness, lineType, shift);
}

void cvePutText(cv::_InputOutputArray* img, cv::String* text, CvPoint* org, int fontFace, double fontScale, CvScalar* color, int thickness, int lineType, bool bottomLeftOrigin)
{
	cv::putText(*img, *text, *org, fontFace, fontScale, *color, thickness, lineType, bottomLeftOrigin);
}

void cveGetTextSize(cv::String* text, int fontFace, double fontScale, int thickness, int* baseLine, CvSize* size)
{
	cv::Size s = cv::getTextSize(*text, fontFace, fontScale, thickness, baseLine);
	*size = cvSize(s);
}

void cveFillConvexPoly(cv::_InputOutputArray* img, cv::_InputArray* points, const CvScalar* color, int lineType, int shift)
{
	cv::fillConvexPoly(*img, *points, *color, lineType, shift);
}

void cveFillPoly(cv::_InputOutputArray* img, cv::_InputArray* pts, const CvScalar* color, int lineType, int shift, CvPoint* offset)
{
	cv::fillPoly(*img, *pts, *color, lineType, shift, *offset);
}

void cvePolylines(cv::_InputOutputArray* img, cv::_InputArray* pts,
	bool isClosed, const CvScalar* color,
	int thickness, int lineType, int shift)
{
	cv::polylines(*img, *pts, isClosed, *color, thickness, lineType, shift);
}

void cveEllipse(cv::_InputOutputArray* img, CvPoint* center, CvSize* axes,
	double angle, double startAngle, double endAngle,
	const CvScalar* color, int thickness, int lineType, int shift)
{
	cv::ellipse(*img, *center, *axes, angle, startAngle, endAngle, *color, thickness, lineType, shift);
}

void cveDrawMarker(
	cv::_InputOutputArray* img,
	CvPoint* position,
	CvScalar* color,
	int markerType,
	int markerSize,
	int thickness,
	int lineType)
{
	cv::drawMarker(*img, *position, *color, markerType, markerSize, thickness, lineType);
}

void cveApplyColorMap1(cv::_InputArray* src, cv::_OutputArray* dst, int colorMap)
{
	cv::applyColorMap(*src, *dst, colorMap);
}

void cveApplyColorMap2(cv::_InputArray* src, cv::_OutputArray* dst, cv::_InputArray userColorMap)
{
	cv::applyColorMap(*src, *dst, userColorMap);
}


void cveDistanceTransform(cv::_InputArray* src, cv::_OutputArray* dst, cv::_OutputArray* labels, int distanceType, int maskSize, int labelType)
{
	cv::distanceTransform(*src, *dst, labels ? *labels : cv::_OutputArray(), distanceType, maskSize, labelType);
}



void cveGetRectSubPix(cv::_InputArray* image, CvSize* patchSize, CvPoint2D32f* center, cv::_OutputArray* patch, int patchType)
{
	cv::getRectSubPix(*image, *patchSize, *center, *patch, patchType);
}

/*
int cveSampleLine(const void* _img, CvPoint* pt1, CvPoint* pt2, void* _buffer, int connectivity)
{
   return cvSampleLine(_img, *pt1, *pt2, _buffer, connectivity);
}*/


void cveHuMoments(cv::Moments* moments, cv::_OutputArray* hu)
{
	cv::HuMoments(*moments, *hu);
}
void cveHuMoments2(cv::Moments* moments, double* hu)
{
	double hu_m[7];
	cv::HuMoments(*moments, hu_m);
	memcpy(hu, hu_m, sizeof(double) * 7);
}

/*
double cveGetSpatialMoment(CvMoments* moments, int xOrder, int yOrder)
{
   return cvGetSpatialMoment(moments, xOrder, yOrder);
}

double cveGetCentralMoment(CvMoments* moments, int xOrder, int yOrder)
{
   return cvGetCentralMoment(moments, xOrder, yOrder);
}

double cveGetNormalizedCentralMoment(CvMoments* moments, int xOrder, int yOrder)
{
   return cvGetNormalizedCentralMoment(moments, xOrder, yOrder);
}
*/
void cveMaxRect(CvRect* rect1, CvRect* rect2, CvRect* result)
{
	*result = cvMaxRect(rect1, rect2);
}

int cveConnectedComponents(cv::_InputArray* image, cv::_OutputArray* labels, int connectivity, int ltype, int ccltype)
{
	return cv::connectedComponents(*image, *labels, connectivity, ltype, ccltype);
}
int cveConnectedComponentsWithStats(cv::_InputArray* image, cv::_OutputArray* labels, cv::_OutputArray* stats, cv::_OutputArray* centroids, int connectivity, int ltype, int ccltype)
{
	return cv::connectedComponentsWithStats(*image, *labels, *stats, *centroids, connectivity, ltype, ccltype);
}

cv::segmentation::IntelligentScissorsMB* cveIntelligentScissorsMBCreate()
{
	return new cv::segmentation::IntelligentScissorsMB();
}
void cveIntelligentScissorsMBRelease(cv::segmentation::IntelligentScissorsMB** ptr)
{
	delete* ptr;
	*ptr = 0;
}
void cveIntelligentScissorsMBSetWeights(
	cv::segmentation::IntelligentScissorsMB* ptr,
	float weightNonEdge,
	float weightGradientDirection,
	float weightGradientMagnitude)
{
	ptr->setWeights(weightNonEdge, weightGradientDirection, weightGradientMagnitude);
}
void cveIntelligentScissorsMBSetEdgeFeatureCannyParameters(
	cv::segmentation::IntelligentScissorsMB* ptr,
	double threshold1,
	double threshold2,
	int apertureSize,
	bool L2gradient)
{
	ptr->setEdgeFeatureCannyParameters(threshold1, threshold2, apertureSize, L2gradient);
}
void cveIntelligentScissorsMBApplyImage(cv::segmentation::IntelligentScissorsMB* ptr, cv::_InputArray* image)
{
	ptr->applyImage(*image);
}
void cveIntelligentScissorsMBApplyImageFeatures(
	cv::segmentation::IntelligentScissorsMB* ptr,
	cv::_InputArray* nonEdge,
	cv::_InputArray* gradientDirection,
	cv::_InputArray* gradientMagnitude,
	cv::_InputArray* image)
{
	ptr->applyImageFeatures(*nonEdge, *gradientDirection, *gradientMagnitude, image ? *image : static_cast<cv::InputArray>(cv::noArray()));
}
void cveIntelligentScissorsMBBuildMap(cv::segmentation::IntelligentScissorsMB* ptr, CvPoint* sourcePt)
{
	ptr->buildMap(*sourcePt);
}
void cveIntelligentScissorsMBGetContour(
	cv::segmentation::IntelligentScissorsMB* ptr,
	CvPoint* targetPt,
	cv::_OutputArray* contour,
	bool backward)
{
	ptr->getContour(*targetPt, *contour, backward);
}

void cveGetGaussianKernel(
	int ksize,
	double sigma,
	int ktype,
	cv::Mat* result)
{
	cv::Mat m = cv::getGaussianKernel(
		ksize,
		sigma,
		ktype);
	cv::swap(m, *result);
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
	cv::getDerivKernels(*kx, *ky, dx, dy, ksize, normalize, ktype);
}

void cveGetGaborKernel(
	CvSize* ksize,
	double sigma,
	double theta,
	double lambd,
	double gamma,
	double psi,
	int ktype,
	cv::Mat* result)
{
	cv::Mat m = cv::getGaborKernel(*ksize, sigma, theta, lambd, gamma, psi, ktype);
	cv::swap(m, *result);
}
