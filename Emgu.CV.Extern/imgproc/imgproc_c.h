//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_IMGPROC_C_H
#define EMGU_IMGPROC_C_H

#include "opencv2/core/core_c.h"
#include "opencv2/imgproc/imgproc.hpp"
#include "opencv2/imgproc/imgproc_c.h"
#include "opencv2/imgproc/types_c.h"
#include "emgu_c.h"

CVAPI(IplImage*) cvGetImageSubRect(IplImage* image, CvRect* rect);

//GrabCut
CVAPI(void) cveGrabCut(cv::_InputArray* img, cv::_InputOutputArray* mask, cv::Rect* rect, cv::_InputOutputArray* bgdModel, cv::_InputOutputArray* fgdModel, int iterCount, int flag);

CVAPI(void) cveFilter2D(cv::_InputArray* src, cv::_OutputArray* dst, cv::_InputArray* kernel, CvPoint* anchor, double delta, int borderType );

CVAPI(void) cveSepFilter2D(cv::_InputArray* src, cv::_OutputArray* dst, int ddepth,	cv::_InputArray* kernelX, cv::_InputArray* kernelY, CvPoint* anchor,	double delta, int borderType);

CVAPI(void) cveBlendLinear(cv::_InputArray* src1, cv::_InputArray* src2, cv::_InputArray* weights1, cv::_InputArray* weights2, cv::_OutputArray* dst);

CVAPI(void) cveCLAHE(cv::_InputArray* src, double clipLimit, CvSize* tileGridSize, cv::_OutputArray* dst);

//CVAPI(void) cveAdaptiveBilateralFilter(cv::_InputArray* src, cv::_OutputArray* dst, CvSize* ksize, double sigmaSpace, double maxSigmaColor, CvPoint* anchor, int borderType);

CVAPI(void) cveErode(cv::_InputArray* src, cv::_OutputArray* dst, cv::_InputArray* kernel, CvPoint* anchor, int iterations, int borderType, CvScalar* borderValue);

CVAPI(void) cveDilate(cv::_InputArray* src, cv::_OutputArray* dst, cv::_InputArray* kernel, CvPoint* anchor, int iterations, int borderType, CvScalar* borderValue);
CVAPI(void) cveGetStructuringElement(cv::Mat* mat, int shape, CvSize* ksize, CvPoint* anchor);
CVAPI(void) cveMorphologyEx(cv::_InputArray* src, cv::_OutputArray* dst, int op, cv::_InputArray* kernel, CvPoint* anchor, int iterations, int borderType, CvScalar* borderValue);

CVAPI(void) cveSobel(cv::_InputArray* src, cv::_OutputArray* dst, int ddepth, int dx, int dy, int ksize, double scale, double delta, int borderType);
CVAPI(void) cveSpatialGradient(cv::_InputArray* src, cv::_OutputArray* dx, cv::_OutputArray* dy, int ksize, int borderType);
CVAPI(void) cveScharr(cv::_InputArray* src, cv::_OutputArray* dst, int ddepth, int dx, int dy, double scale, double delta, int borderType);
CVAPI(void) cveLaplacian(cv::_InputArray* src, cv::_OutputArray* dst, int ddepth, int ksize, double scale, double delta, int borderType);
CVAPI(void) cvePyrUp(cv::_InputArray* src, cv::_OutputArray* dst, CvSize* size, int borderType);
CVAPI(void) cvePyrDown(cv::_InputArray* src, cv::_OutputArray* dst, CvSize* size, int borderType);
CVAPI(void) cveBuildPyramid(cv::_InputArray* src, cv::_OutputArray* dst, int maxlevel, int borderType);

CVAPI(void) cveCanny(cv::_InputArray* image, cv::_OutputArray* edges, double threshold1, double threshold2, int apertureSize, bool L2gradient);
CVAPI(void) cveCanny2(cv::_InputArray* dx, cv::_InputArray* dy, cv::_OutputArray* edges, double threshold1, double threshold2, bool L2gradient);
CVAPI(void) cveCornerHarris(cv::_InputArray* src, cv::_OutputArray* dst, int blockSize, int ksize, double k, int borderType);
CVAPI(double) cveThreshold(cv::_InputArray* src, cv::_OutputArray* dst, double thresh, double maxval, int type);
CVAPI(void) cveWatershed(cv::_InputArray* image, cv::_InputOutputArray* markers);
CVAPI(void) cveAdaptiveThreshold(cv::_InputArray* src, cv::_OutputArray* dst, double maxValue, int adaptiveMethod, int thresholdType, int blockSize, double c);
CVAPI(void) cveCvtColor(cv::_InputArray* src, cv::_OutputArray* dst, int code, int dstCn);
CVAPI(void) cveCopyMakeBorder(cv::_InputArray* src, cv::_OutputArray* dst, int top, int bottom, int left, int right, int borderType, CvScalar* value);

CVAPI(void) cveIntegral(cv::_InputArray* src, cv::_OutputArray* sum, cv::_OutputArray* sqsum, cv::_OutputArray* tilted, int sdepth, int sqdepth);
CVAPI(int) cveFloodFill(cv::_InputOutputArray* image, cv::_InputOutputArray* mask, CvPoint* seedPoint, CvScalar* newVal, CvRect* rect, CvScalar* loDiff, CvScalar* upDiff, int flags);
CVAPI(void) cvePyrMeanShiftFiltering(cv::_InputArray* src, cv::_OutputArray* dst, double sp, double sr, int maxLevel, CvTermCriteria* termCrit);

CVAPI(void) cveMoments(cv::_InputArray* arr, bool binaryImage, cv::Moments* moments);

CVAPI(void) cveEqualizeHist(cv::_InputArray* src, cv::_OutputArray* dst);

CVAPI(void) cveAccumulate(cv::_InputArray* src, cv::_InputOutputArray* dst, cv::_InputArray* mask);
CVAPI(void) cveAccumulateSquare(cv::_InputArray* src, cv::_InputOutputArray* dst, cv::_InputArray* mask);
CVAPI(void) cveAccumulateProduct(cv::_InputArray* src1, cv::_InputArray* src2, cv::_InputOutputArray* dst, cv::_InputArray* mask);
CVAPI(void) cveAccumulateWeighted(cv::_InputArray* src, cv::_InputOutputArray* dst, double alpha, cv::_InputArray* mask);
CVAPI(void) cvePhaseCorrelate(cv::_InputArray* src1, cv::_InputArray* src2, cv::_InputArray* window, double* response, CvPoint2D64f* result);
CVAPI(void) cveCreateHanningWindow(cv::_OutputArray* dst, CvSize* winSize, int type);

CVAPI(void) cveResize(cv::_InputArray* src, cv::_OutputArray* dst, CvSize* dsize, double fx, double fy, int interpolation);
CVAPI(void) cveWarpAffine(cv::_InputArray* src, cv::_OutputArray* dst, cv::_InputArray* m, CvSize* dsize, int flags, int borderMode, CvScalar* borderValue);
CVAPI(void) cveWarpPerspective(cv::_InputArray* src, cv::_OutputArray* dst, cv::_InputArray* m, CvSize* dsize, int flags, int borderMode, CvScalar* borderValue);

CVAPI(void) cveLogPolar(cv::_InputArray* src, cv::_OutputArray* dst, CvPoint2D32f* center, double M, int flags);
CVAPI(void) cveLinearPolar(cv::_InputArray* src, cv::_OutputArray* dst, CvPoint2D32f* center, double maxRadius, int flags);
CVAPI(void) cveRemap(cv::_InputArray* src, cv::_OutputArray* dst, cv::_InputArray* map1, cv::_InputArray* map2, int interpolation, int borderMode, CvScalar* borderValue);
CVAPI(void) cveRepeat(cv::_InputArray* src, int ny, int nx, cv::_OutputArray* dst);
CVAPI(void) cveHoughCircles(cv::_InputArray* image, cv::_OutputArray* circles, int method, double dp, double minDist, double param1, double param2, int minRadius, int maxRadius);
CVAPI(void) cveHoughLines(cv::_InputArray* image, cv::_OutputArray* lines, double rho, double theta, int threshold, double srn, double stn);
CVAPI(void) cveHoughLinesP(cv::_InputArray* image, cv::_OutputArray* lines, double rho, double theta, int threshold, double minLineLength, double maxGap);

CVAPI(void) cveMatchTemplate(cv::_InputArray* image, cv::_InputArray* templ, cv::_OutputArray* result, int method, cv::_InputArray* mask);
CVAPI(void) cveCornerSubPix(cv::_InputArray* image, cv::_InputOutputArray* corners, CvSize* winSize, CvSize* zeroZone, CvTermCriteria* criteria);

CVAPI(void) cveConvertMaps(cv::_InputArray* map1, cv::_InputArray* map2, cv::_OutputArray* dstmap1, cv::_OutputArray* dstmap2, int dstmap1Type, bool nninterpolation);

CVAPI(void) cveGetAffineTransform(cv::_InputArray* src, cv::_InputArray* dst, cv::Mat* affine);
CVAPI(void) cveGetPerspectiveTransform(cv::_InputArray* src, cv::_InputArray* dst, cv::Mat* perspective);
CVAPI(void) cveInvertAffineTransform(cv::_InputArray* m, cv::_OutputArray* im);


CVAPI(void) cveEMD(cv::_InputArray* signature1, cv::_InputArray* signature2, int distType, cv::_InputArray* cost, float* lowerBound, cv::_OutputArray* flow);

CVAPI(void) cveCalcHist( cv::_InputArray* images, const std::vector<int>* channels, cv::_InputArray* mask, cv::_OutputArray* hist, std::vector<int>* histSize, std::vector<float>* ranges, bool accumulate );
CVAPI(void) cveCalcBackProject(cv::_InputArray* images, const std::vector<int>* channels, cv::_InputArray* hist, cv::_OutputArray* dst, const std::vector<float>* ranges, double scale );
CVAPI(double) cveCompareHist(cv::_InputArray* h1, cv::_InputArray* h2, int method);

CVAPI(void) cveGetRotationMatrix2D(CvPoint2D32f* center, double angle, double scale, cv::_OutputArray* rotationMatrix2D);

CVAPI(void) cveFindContours(cv::_InputOutputArray* image, cv::_OutputArray* contours, cv::_OutputArray* hierarchy, int mode, int method, CvPoint* offset);
CVAPI(double) cvePointPolygonTest(cv::_InputArray* contour, CvPoint2D32f* pt, bool measureDist);

CVAPI(double) cveContourArea(cv::_InputArray* contour, bool oriented);
CVAPI(bool) cveIsContourConvex(cv::_InputArray* contour);
CVAPI(float) cveIntersectConvexConvex(cv::_InputArray* p1, cv::_InputArray* p2,	cv::_OutputArray* p12, bool handleNested);
CVAPI(void) cveBoundingRectangle(cv::_InputArray* points, CvRect* boundingRect);
CVAPI(double) cveArcLength(cv::_InputArray* curve, bool closed);
CVAPI(void) cveMinAreaRect(cv::_InputArray* points, CvBox2D* box);
CVAPI(void) cveBoxPoints(CvBox2D* box, cv::_OutputArray* points);
CVAPI(double) cveMinEnclosingTriangle(cv::_InputArray* points, cv::_OutputArray* triangle);
CVAPI(void) cveMinEnclosingCircle(cv::_InputArray* points, CvPoint2D32f* center, float* radius);
CVAPI(double) cveMatchShapes(cv::_InputArray* contour1, cv::_InputArray* contour2, int method, double parameter);
CVAPI(void) cveFitEllipse(cv::_InputArray* points, CvBox2D* box);
CVAPI(void) cveFitEllipseAMS(cv::_InputArray* points, CvBox2D* box);
CVAPI(void) cveFitEllipseDirect(cv::_InputArray* points, CvBox2D* box);

CVAPI(void) cveFitLine(cv::_InputArray* points, cv::_OutputArray* line, int distType, double param, double reps, double aeps);
CVAPI(int) cveRotatedRectangleIntersection(CvBox2D* rect1, CvBox2D* rect2, cv::_OutputArray* intersectingRegion);
CVAPI(void) cveDrawContours(
   cv::_InputOutputArray* image, cv::_InputArray* contours, int contourIdx, 
   CvScalar* color, int thickness, int lineType, cv::_InputArray* hierarchy, 
   int maxLevel, CvPoint* offset);
CVAPI(void) cveApproxPolyDP(cv::_InputArray* curve, cv::_OutputArray* approxCurve, double epsilon, bool closed);
CVAPI(void) cveConvexHull(cv::_InputArray* points, cv::_OutputArray* hull, bool clockwise, bool returnPoints);
CVAPI(void) cveConvexityDefects(cv::_InputArray* contour, cv::_InputArray* convexhull, cv::_OutputArray* convexityDefects); 

CVAPI(void) cveGaussianBlur(cv::_InputArray* src, cv::_OutputArray* dst, CvSize* ksize, double sigmaX, double sigmaY, int borderType);
CVAPI(void) cveBlur(cv::_InputArray* src, cv::_OutputArray* dst, CvSize* kSize, CvPoint* anchor, int borderType);
CVAPI(void) cveMedianBlur(cv::_InputArray* src, cv::_OutputArray* dst, int ksize);
CVAPI(void) cveBoxFilter(cv::_InputArray* src, cv::_OutputArray* dst, int ddepth, CvSize* ksize, CvPoint* anchor, bool normailize, int borderType);
CVAPI(void) cveSqrBoxFilter(cv::_InputArray* _src, cv::_OutputArray* _dst, int ddepth, CvSize* ksize, CvPoint* anchor, bool normalize, int borderType);
CVAPI(void) cveBilateralFilter(cv::_InputArray* src, cv::_OutputArray* dst, int d, double sigmaColor, double sigmaSpace, int borderType);

//Subdiv2D
CVAPI(cv::Subdiv2D*) cveSubdiv2DCreate(CvRect* rect);
CVAPI(void) cveSubdiv2DRelease(cv::Subdiv2D** subdiv);
CVAPI(void) cveSubdiv2DInsertMulti(cv::Subdiv2D* subdiv, std::vector<cv::Point2f>* points);
CVAPI(int) cveSubdiv2DInsertSingle(cv::Subdiv2D* subdiv, CvPoint2D32f* pt);
CVAPI(void) cveSubdiv2DGetTriangleList(cv::Subdiv2D* subdiv, std::vector<cv::Vec6f>* triangleList);
CVAPI(void) cveSubdiv2DGetVoronoiFacetList(cv::Subdiv2D* subdiv, std::vector<int>* idx, std::vector< std::vector< cv::Point2f> >* facetList, std::vector< cv::Point2f >* facetCenters);
CVAPI(int) cveSubdiv2DFindNearest(cv::Subdiv2D* subdiv, CvPoint2D32f* pt,  CvPoint2D32f* nearestPt);
CVAPI(int) cveSubdiv2DLocate(cv::Subdiv2D* subdiv, CvPoint2D32f* pt, int* edge, int* vertex);

//LineIterator
CVAPI(cv::LineIterator*) cveLineIteratorCreate(
	cv::Mat* img, 
	CvPoint* pt1, 
	CvPoint* pt2,
	int connectivity, 
	bool leftToRight);
CVAPI(uchar*) cveLineIteratorGetDataPointer(cv::LineIterator* iterator);
CVAPI(void) cveLineIteratorPos(cv::LineIterator* iterator, CvPoint* pos);
CVAPI(void) cveLineIteratorMoveNext(cv::LineIterator* iterator);
CVAPI(void) cveLineIteratorRelease(cv::LineIterator** iterator);
CVAPI(void) cveLineIteratorSampleLine(
	cv::Mat* img,
	CvPoint* pt1,
	CvPoint* pt2,
	int connectivity,
	bool leftToRight, 
	cv::Mat* result);

//Drawing
CVAPI(void) cveLine(cv::_InputOutputArray* img, CvPoint* p1, CvPoint* p2, CvScalar* color, int thickness, int lineType, int shift);

CVAPI(void) cveArrowedLine(cv::_InputOutputArray* img, CvPoint* pt1, CvPoint* pt2, CvScalar* color, int thickness, int lineType, int shift, double tipLength);

CVAPI(void) cveRectangle(cv::_InputOutputArray* img, CvRect* rect, CvScalar* color, int thickness, int lineType, int shift);

CVAPI(void) cveCircle(cv::_InputOutputArray* img, CvPoint* center, int radius, CvScalar* color, int thickness, int lineType, int shift);

CVAPI(void) cvePutText(cv::_InputOutputArray* img, cv::String* text, CvPoint* org, int fontFace, double fontScale, CvScalar* color, int thickness, int lineType, bool bottomLeftOrigin);

CVAPI(void) cveGetTextSize(cv::String* text, int fontFace, double fontScale, int thickness, int* baseLine, CvSize* size);

CVAPI(void) cveFillConvexPoly(cv::_InputOutputArray* img, cv::_InputArray* points, const CvScalar* color, int lineType, int shift);

CVAPI(void) cveFillPoly(cv::_InputOutputArray* img, cv::_InputArray* pts, const CvScalar* color, int lineType, int shift, CvPoint* offset);

CVAPI(void) cvePolylines(cv::_InputOutputArray* img, cv::_InputArray* pts,
                   bool isClosed, const CvScalar* color,
                   int thickness, int lineType, int shift );

CVAPI(void) cveEllipse(cv::_InputOutputArray* img, CvPoint* center, CvSize* axes,
              double angle, double startAngle, double endAngle,
              const CvScalar* color, int thickness, int lineType, int shift );

CVAPI(void) cveDrawMarker(
	cv::_InputOutputArray* img, 
	CvPoint* position, 
	CvScalar* color,
	int markerType, 
	int markerSize, 
	int thickness,
	int lineType);

//color map
CVAPI(void) cveApplyColorMap1(cv::_InputArray* src, cv::_OutputArray* dst, int colorMap);
CVAPI(void) cveApplyColorMap2(cv::_InputArray* src, cv::_OutputArray* dst, cv::_InputArray userColorMap);

CVAPI(void) cveDistanceTransform(cv::_InputArray* src, cv::_OutputArray* dst, cv::_OutputArray* labels, int distanceType, int maskSize, int labelType);



CVAPI(void) cveGetRectSubPix(cv::_InputArray* image, CvSize* patchSize, CvPoint2D32f* center, cv::_OutputArray* patch, int patchType);


//CVAPI(int) cveSampleLine(const void* _img, CvPoint* pt1, CvPoint* pt2, void* _buffer, int connectivity);
CVAPI(void) cveHuMoments(cv::Moments* moments, cv::_OutputArray* hu);
CVAPI(void) cveHuMoments2(cv::Moments* moments, double* hu);
/*
CVAPI(double) cveGetSpatialMoment(CvMoments* moments, int xOrder, int yOrder);
CVAPI(double) cveGetCentralMoment(CvMoments* moments, int xOrder, int yOrder);
CVAPI(double) cveGetNormalizedCentralMoment(CvMoments* moments, int xOrder, int yOrder);
*/
CVAPI(void) cveMaxRect(CvRect* rect1, CvRect* rect2, CvRect* result);

CVAPI(int) cveConnectedComponents(cv::_InputArray* image, cv::_OutputArray* labels, int connectivity, int ltype, int ccltype);
CVAPI(int) cveConnectedComponentsWithStats(cv::_InputArray* image, cv::_OutputArray* labels, cv::_OutputArray* stats, cv::_OutputArray* centroids, int connectivity, int ltype, int ccltype);

CVAPI(cv::segmentation::IntelligentScissorsMB*) cveIntelligentScissorsMBCreate();
CVAPI(void) cveIntelligentScissorsMBRelease(cv::segmentation::IntelligentScissorsMB** ptr);
CVAPI(void) cveIntelligentScissorsMBSetWeights(
	cv::segmentation::IntelligentScissorsMB* ptr, 
	float weightNonEdge, 
	float weightGradientDirection, 
	float weightGradientMagnitude);
CVAPI(void) cveIntelligentScissorsMBSetEdgeFeatureCannyParameters(
	cv::segmentation::IntelligentScissorsMB* ptr,
	double threshold1, 
	double threshold2,
	int apertureSize, 
	bool L2gradient);
CVAPI(void) cveIntelligentScissorsMBApplyImage(cv::segmentation::IntelligentScissorsMB* ptr, cv::_InputArray* image);
CVAPI(void) cveIntelligentScissorsMBApplyImageFeatures(
	cv::segmentation::IntelligentScissorsMB* ptr,
	cv::_InputArray* nonEdge, 
	cv::_InputArray* gradientDirection, 
	cv::_InputArray* gradientMagnitude,
	cv::_InputArray* image);
CVAPI(void) cveIntelligentScissorsMBBuildMap(cv::segmentation::IntelligentScissorsMB* ptr, CvPoint* sourcePt);
CVAPI(void) cveIntelligentScissorsMBGetContour(
	cv::segmentation::IntelligentScissorsMB* ptr, 
	CvPoint* targetPt, 
	cv::_OutputArray* contour, 
	bool backward);

CVAPI(void) cveGetGaussianKernel(
	int ksize, 
	double sigma, 
	int ktype, 
	cv::Mat* result);

CVAPI(void) cveGetDerivKernels(
	cv::_OutputArray* kx, 
	cv::_OutputArray* ky,
	int dx, 
	int dy, 
	int ksize,
	bool normalize, 
	int ktype);

CVAPI(void) cveGetGaborKernel(
	CvSize* ksize, 
	double sigma, 
	double theta, 
	double lambd,
	double gamma, 
	double psi, 
	int ktype,
	cv::Mat* result);

#endif
