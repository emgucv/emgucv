//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_GEOMETRY_C_H
#define EMGU_GEOMETRY_C_H

#include "opencv2/core.hpp"
#include "cvapi_compat.h"

#ifdef HAVE_OPENCV_GEOMETRY
#include "opencv2/geometry.hpp"
#else
static inline CV_NORETURN void throw_no_geometry() { CV_Error(cv::Error::StsBadFunc, "The library is compiled without geometry support. To use this module, please switch to the full Emgu CV runtime."); }
#endif

CVAPI(void) cveMoments(cv::_InputArray* arr, bool binaryImage, cv::Moments* moments);
CVAPI(void) cveHuMoments(cv::Moments* moments, cv::_OutputArray* hu);
CVAPI(void) cveHuMoments2(cv::Moments* moments, double* hu);
CVAPI(double) cveMatchShapes(cv::_InputArray* contour1, cv::_InputArray* contour2, int method, double parameter);

CVAPI(void) cveGetAffineTransform(cv::_InputArray* src, cv::_InputArray* dst, cv::Mat* affine);
CVAPI(void) cveGetPerspectiveTransform(cv::_InputArray* src, cv::_InputArray* dst, cv::Mat* perspective);
CVAPI(void) cveInvertAffineTransform(cv::_InputArray* m, cv::_OutputArray* im);
CVAPI(void) cveGetRotationMatrix2D(cv::Point2f* center, double angle, double scale, cv::_OutputArray* rotationMatrix2D);

CVAPI(double) cvePointPolygonTest(cv::_InputArray* contour, cv::Point2f* pt, bool measureDist);
CVAPI(double) cveContourArea(cv::_InputArray* contour, bool oriented);
CVAPI(bool) cveIsContourConvex(cv::_InputArray* contour);
CVAPI(float) cveIntersectConvexConvex(cv::_InputArray* p1, cv::_InputArray* p2, cv::_OutputArray* p12, bool handleNested);
CVAPI(void) cveBoundingRectangle(cv::_InputArray* points, cv::Rect* boundingRect);
CVAPI(double) cveArcLength(cv::_InputArray* curve, bool closed);
CVAPI(void) cveMinAreaRect(cv::_InputArray* points, cv::RotatedRect* box);
CVAPI(void) cveBoxPoints(cv::RotatedRect* box, cv::_OutputArray* points);
CVAPI(double) cveMinEnclosingTriangle(cv::_InputArray* points, cv::_OutputArray* triangle);
CVAPI(void) cveMinEnclosingCircle(cv::_InputArray* points, cv::Point2f* center, float* radius);
CVAPI(void) cveFitEllipse(cv::_InputArray* points, cv::RotatedRect* box);
CVAPI(void) cveFitEllipseAMS(cv::_InputArray* points, cv::RotatedRect* box);
CVAPI(void) cveFitEllipseDirect(cv::_InputArray* points, cv::RotatedRect* box);
CVAPI(void) cveGetClosestEllipsePoints(cv::RotatedRect* ellipseParams, cv::_InputArray* points, cv::_OutputArray* closestPts);
CVAPI(void) cveFitLine(cv::_InputArray* points, cv::_OutputArray* line, int distType, double param, double reps, double aeps);
CVAPI(int) cveRotatedRectangleIntersection(cv::RotatedRect* rect1, cv::RotatedRect* rect2, cv::_OutputArray* intersectingRegion);
CVAPI(void) cveApproxPolyDP(cv::_InputArray* curve, cv::_OutputArray* approxCurve, double epsilon, bool closed);
CVAPI(void) cveApproxPolyN(cv::_InputArray* curve, cv::_OutputArray* approxCurve, int nsides, float epsilonPercentage, bool ensureConvex);
CVAPI(void) cveConvexHull(cv::_InputArray* points, cv::_OutputArray* hull, bool clockwise, bool returnPoints);
CVAPI(void) cveConvexityDefects(cv::_InputArray* contour, cv::_InputArray* convexhull, cv::_OutputArray* convexityDefects);

//Subdiv2D
CVAPI(cv::Subdiv2D*) cveSubdiv2DCreate(cv::Rect* rect);
CVAPI(void) cveSubdiv2DRelease(cv::Subdiv2D** subdiv);
CVAPI(void) cveSubdiv2DInsertMulti(cv::Subdiv2D* subdiv, std::vector<cv::Point2f>* points);
CVAPI(int) cveSubdiv2DInsertSingle(cv::Subdiv2D* subdiv, cv::Point2f* pt);
CVAPI(void) cveSubdiv2DGetTriangleList(cv::Subdiv2D* subdiv, std::vector<cv::Vec6f>* triangleList);
CVAPI(void) cveSubdiv2DGetVoronoiFacetList(cv::Subdiv2D* subdiv, std::vector<int>* idx, std::vector< std::vector< cv::Point2f> >* facetList, std::vector< cv::Point2f >* facetCenters);
CVAPI(int) cveSubdiv2DFindNearest(cv::Subdiv2D* subdiv, cv::Point2f* pt, cv::Point2f* nearestPt);
CVAPI(int) cveSubdiv2DLocate(cv::Subdiv2D* subdiv, cv::Point2f* pt, int* edge, int* vertex);

#endif
