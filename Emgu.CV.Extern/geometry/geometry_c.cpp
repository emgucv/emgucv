//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "geometry_c.h"

void cveMoments(cv::_InputArray* arr, bool binaryImage, cv::Moments* moments)
{
#ifdef HAVE_OPENCV_GEOMETRY
	cv::Moments m = cv::moments(*arr, binaryImage);
	memcpy(moments, &m, sizeof(cv::Moments));
#else
	throw_no_geometry();
#endif
}
void cveHuMoments(cv::Moments* moments, cv::_OutputArray* hu)
{
#ifdef HAVE_OPENCV_GEOMETRY
	cv::HuMoments(*moments, *hu);
#else
	throw_no_geometry();
#endif
}
void cveHuMoments2(cv::Moments* moments, double* hu)
{
#ifdef HAVE_OPENCV_GEOMETRY
	double hu_m[7];
	cv::HuMoments(*moments, hu_m);
	memcpy(hu, hu_m, sizeof(double) * 7);
#else
	throw_no_geometry();
#endif
}
double cveMatchShapes(cv::_InputArray* contour1, cv::_InputArray* contour2, int method, double parameter)
{
#ifdef HAVE_OPENCV_GEOMETRY
	return cv::matchShapes(*contour1, *contour2, method, parameter);
#else
	throw_no_geometry();
#endif
}

void cveGetAffineTransform(cv::_InputArray* src, cv::_InputArray* dst, cv::Mat* affine)
{
#ifdef HAVE_OPENCV_GEOMETRY
	cv::Mat result = cv::getAffineTransform(*src, *dst);
	cv::swap(result, *affine);
#else
	throw_no_geometry();
#endif
}
void cveGetPerspectiveTransform(cv::_InputArray* src, cv::_InputArray* dst, cv::Mat* perspective)
{
#ifdef HAVE_OPENCV_GEOMETRY
	cv::Mat result = cv::getPerspectiveTransform(*src, *dst);
	cv::swap(result, *perspective);
#else
	throw_no_geometry();
#endif
}
void cveInvertAffineTransform(cv::_InputArray* m, cv::_OutputArray* im)
{
#ifdef HAVE_OPENCV_GEOMETRY
	cv::invertAffineTransform(*m, *im);
#else
	throw_no_geometry();
#endif
}
void cveGetRotationMatrix2D(cv::Point2f* center, double angle, double scale, cv::_OutputArray* rotationMatrix2D)
{
#ifdef HAVE_OPENCV_GEOMETRY
	cv::Mat r = cv::getRotationMatrix2D(*center, angle, scale);
	if (rotationMatrix2D->empty() || r.type() == rotationMatrix2D->type())
		r.copyTo(*rotationMatrix2D);
	else
		r.convertTo(*rotationMatrix2D, rotationMatrix2D->type());
#else
	throw_no_geometry();
#endif
}

double cvePointPolygonTest(cv::_InputArray* contour, cv::Point2f* pt, bool measureDist)
{
#ifdef HAVE_OPENCV_GEOMETRY
	return cv::pointPolygonTest(*contour, *pt, measureDist);
#else
	throw_no_geometry();
#endif
}
double cveContourArea(cv::_InputArray* contour, bool oriented)
{
#ifdef HAVE_OPENCV_GEOMETRY
	return cv::contourArea(*contour, oriented);
#else
	throw_no_geometry();
#endif
}
bool cveIsContourConvex(cv::_InputArray* contour)
{
#ifdef HAVE_OPENCV_GEOMETRY
	return cv::isContourConvex(*contour);
#else
	throw_no_geometry();
#endif
}
float cveIntersectConvexConvex(cv::_InputArray* p1, cv::_InputArray* p2, cv::_OutputArray* p12, bool handleNested)
{
#ifdef HAVE_OPENCV_GEOMETRY
	return cv::intersectConvexConvex(*p1, *p2, *p12, handleNested);
#else
	throw_no_geometry();
#endif
}
void cveBoundingRectangle(cv::_InputArray* points, cv::Rect* boundingRect)
{
#ifdef HAVE_OPENCV_GEOMETRY
	cv::Rect rect = cv::boundingRect(*points);
	boundingRect->x = rect.x;
	boundingRect->y = rect.y;
	boundingRect->width = rect.width;
	boundingRect->height = rect.height;
#else
	throw_no_geometry();
#endif
}
double cveArcLength(cv::_InputArray* curve, bool closed)
{
#ifdef HAVE_OPENCV_GEOMETRY
	return cv::arcLength(*curve, closed);
#else
	throw_no_geometry();
#endif
}
void cveMinAreaRect(cv::_InputArray* points, cv::RotatedRect* box)
{
#ifdef HAVE_OPENCV_GEOMETRY
	cv::RotatedRect rr = cv::minAreaRect(*points);
	box->center = rr.center;
	box->size = rr.size;
	box->angle = rr.angle;
#else
	throw_no_geometry();
#endif
}
void cveBoxPoints(cv::RotatedRect* box, cv::_OutputArray* points)
{
#ifdef HAVE_OPENCV_GEOMETRY
	cv::boxPoints(*box, *points);
#else
	throw_no_geometry();
#endif
}
double cveMinEnclosingTriangle(cv::_InputArray* points, cv::_OutputArray* triangle)
{
#ifdef HAVE_OPENCV_GEOMETRY
	return cv::minEnclosingTriangle(*points, *triangle);
#else
	throw_no_geometry();
#endif
}
void cveMinEnclosingCircle(cv::_InputArray* points, cv::Point2f* center, float* radius)
{
#ifdef HAVE_OPENCV_GEOMETRY
	cv::Point2f c; float r;
	cv::minEnclosingCircle(*points, c, r);
	center->x = c.x;
	center->y = c.y;
	*radius = r;
#else
	throw_no_geometry();
#endif
}
void cveFitEllipse(cv::_InputArray* points, cv::RotatedRect* box)
{
#ifdef HAVE_OPENCV_GEOMETRY
	cv::RotatedRect rect = cv::fitEllipse(*points);
	box->center = rect.center;
	box->size = rect.size;
	box->angle = rect.angle;
#else
	throw_no_geometry();
#endif
}
void cveFitEllipseAMS(cv::_InputArray* points, cv::RotatedRect* box)
{
#ifdef HAVE_OPENCV_GEOMETRY
	cv::RotatedRect rect = cv::fitEllipseAMS(*points);
	box->center = rect.center;
	box->size = rect.size;
	box->angle = rect.angle;
#else
	throw_no_geometry();
#endif
}
void cveFitEllipseDirect(cv::_InputArray* points, cv::RotatedRect* box)
{
#ifdef HAVE_OPENCV_GEOMETRY
	cv::RotatedRect rect = cv::fitEllipseDirect(*points);
	box->center = rect.center;
	box->size = rect.size;
	box->angle = rect.angle;
#else
	throw_no_geometry();
#endif
}
void cveGetClosestEllipsePoints(cv::RotatedRect* ellipseParams, cv::_InputArray* points, cv::_OutputArray* closestPts)
{
#ifdef HAVE_OPENCV_GEOMETRY
	cv::getClosestEllipsePoints(*ellipseParams, *points, *closestPts);
#else
	throw_no_geometry();
#endif
}
void cveFitLine(cv::_InputArray* points, cv::_OutputArray* line, int distType, double param, double reps, double aeps)
{
#ifdef HAVE_OPENCV_GEOMETRY
	cv::fitLine(*points, *line, distType, param, reps, aeps);
#else
	throw_no_geometry();
#endif
}
int cveRotatedRectangleIntersection(cv::RotatedRect* rect1, cv::RotatedRect* rect2, cv::_OutputArray* intersectingRegion)
{
#ifdef HAVE_OPENCV_GEOMETRY
	return cv::rotatedRectangleIntersection(*rect1, *rect2, *intersectingRegion);
#else
	throw_no_geometry();
#endif
}
void cveApproxPolyDP(cv::_InputArray* curve, cv::_OutputArray* approxCurve, double epsilon, bool closed)
{
#ifdef HAVE_OPENCV_GEOMETRY
	cv::approxPolyDP(*curve, *approxCurve, epsilon, closed);
#else
	throw_no_geometry();
#endif
}
void cveApproxPolyN(cv::_InputArray* curve, cv::_OutputArray* approxCurve, int nsides, float epsilonPercentage, bool ensureConvex)
{
#ifdef HAVE_OPENCV_GEOMETRY
	cv::approxPolyN(*curve, *approxCurve, nsides, epsilonPercentage, ensureConvex);
#else
	throw_no_geometry();
#endif
}
void cveConvexHull(cv::_InputArray* points, cv::_OutputArray* hull, bool clockwise, bool returnPoints)
{
#ifdef HAVE_OPENCV_GEOMETRY
	cv::convexHull(*points, *hull, clockwise, returnPoints);
#else
	throw_no_geometry();
#endif
}
void cveConvexityDefects(cv::_InputArray* contour, cv::_InputArray* convexhull, cv::_OutputArray* convexityDefects)
{
#ifdef HAVE_OPENCV_GEOMETRY
	cv::convexityDefects(*contour, *convexhull, *convexityDefects);
#else
	throw_no_geometry();
#endif
}

//Subdiv2D
cv::Subdiv2D* cveSubdiv2DCreate(cv::Rect* rect)
{
#ifdef HAVE_OPENCV_GEOMETRY
	return new cv::Subdiv2D(*rect);
#else
	throw_no_geometry();
#endif
}
void cveSubdiv2DRelease(cv::Subdiv2D** subdiv)
{
#ifdef HAVE_OPENCV_GEOMETRY
	delete* subdiv;
	*subdiv = 0;
#else
	throw_no_geometry();
#endif
}
void cveSubdiv2DInsertMulti(cv::Subdiv2D* subdiv, std::vector<cv::Point2f>* points)
{
#ifdef HAVE_OPENCV_GEOMETRY
	subdiv->insert(*points);
#else
	throw_no_geometry();
#endif
}
int cveSubdiv2DInsertSingle(cv::Subdiv2D* subdiv, cv::Point2f* pt)
{
#ifdef HAVE_OPENCV_GEOMETRY
	return subdiv->insert(*pt);
#else
	throw_no_geometry();
#endif
}
void cveSubdiv2DGetTriangleList(cv::Subdiv2D* subdiv, std::vector<cv::Vec6f>* triangleList)
{
#ifdef HAVE_OPENCV_GEOMETRY
	subdiv->getTriangleList(*triangleList);
#else
	throw_no_geometry();
#endif
}
void cveSubdiv2DGetVoronoiFacetList(cv::Subdiv2D* subdiv, std::vector<int>* idx, std::vector< std::vector< cv::Point2f> >* facetList, std::vector< cv::Point2f >* facetCenters)
{
#ifdef HAVE_OPENCV_GEOMETRY
	subdiv->getVoronoiFacetList(*idx, *facetList, *facetCenters);
#else
	throw_no_geometry();
#endif
}
int cveSubdiv2DFindNearest(cv::Subdiv2D* subdiv, cv::Point2f* pt, cv::Point2f* nearestPt)
{
#ifdef HAVE_OPENCV_GEOMETRY
	cv::Point2f np;
	int result = subdiv->findNearest(*pt, &np);
	*nearestPt = np;
	return result;
#else
	throw_no_geometry();
#endif
}
int cveSubdiv2DLocate(cv::Subdiv2D* subdiv, cv::Point2f* pt, int* edge, int* vertex)
{
#ifdef HAVE_OPENCV_GEOMETRY
	int e = 0, v = 0;
	int result = subdiv->locate(*pt, e, v);
	*edge = e;
	*vertex = v;
	return result;
#else
	throw_no_geometry();
#endif
}
