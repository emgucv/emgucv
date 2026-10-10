//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "geometry_c.h"

void cveMoments(cv::_InputArray* arr, bool binaryImage, cv::Moments* moments)
{
	try
	{
#ifdef HAVE_OPENCV_GEOMETRY
		cv::Moments m = cv::moments(*arr, binaryImage);
		memcpy(moments, &m, sizeof(cv::Moments));
#else
		throw_no_geometry();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveHuMoments(cv::Moments* moments, cv::_OutputArray* hu)
{
	try
	{
#ifdef HAVE_OPENCV_GEOMETRY
		cv::HuMoments(*moments, *hu);
#else
		throw_no_geometry();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveHuMoments2(cv::Moments* moments, double* hu)
{
	try
	{
#ifdef HAVE_OPENCV_GEOMETRY
		double hu_m[7];
		cv::HuMoments(*moments, hu_m);
		memcpy(hu, hu_m, sizeof(double) * 7);
#else
		throw_no_geometry();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
double cveMatchShapes(cv::_InputArray* contour1, cv::_InputArray* contour2, int method, double parameter)
{
	try
	{
#ifdef HAVE_OPENCV_GEOMETRY
		return cv::matchShapes(*contour1, *contour2, method, parameter);
#else
		throw_no_geometry();
#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

void cveGetAffineTransform(cv::_InputArray* src, cv::_InputArray* dst, cv::Mat* affine)
{
	try
	{
#ifdef HAVE_OPENCV_GEOMETRY
		cv::Mat result = cv::getAffineTransform(*src, *dst);
		cv::swap(result, *affine);
#else
		throw_no_geometry();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveGetPerspectiveTransform(cv::_InputArray* src, cv::_InputArray* dst, cv::Mat* perspective)
{
	try
	{
#ifdef HAVE_OPENCV_GEOMETRY
		cv::Mat result = cv::getPerspectiveTransform(*src, *dst);
		cv::swap(result, *perspective);
#else
		throw_no_geometry();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveInvertAffineTransform(cv::_InputArray* m, cv::_OutputArray* im)
{
	try
	{
#ifdef HAVE_OPENCV_GEOMETRY
		cv::invertAffineTransform(*m, *im);
#else
		throw_no_geometry();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveGetRotationMatrix2D(cv::Point2f* center, double angle, double scale, cv::_OutputArray* rotationMatrix2D)
{
	try
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
	CVAPI_CATCH_CV_ERRORS_VOID
}

double cvePointPolygonTest(cv::_InputArray* contour, cv::Point2f* pt, bool measureDist)
{
	try
	{
#ifdef HAVE_OPENCV_GEOMETRY
		return cv::pointPolygonTest(*contour, *pt, measureDist);
#else
		throw_no_geometry();
#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
double cveContourArea(cv::_InputArray* contour, bool oriented)
{
	try
	{
#ifdef HAVE_OPENCV_GEOMETRY
		return cv::contourArea(*contour, oriented);
#else
		throw_no_geometry();
#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
bool cveIsContourConvex(cv::_InputArray* contour)
{
	try
	{
#ifdef HAVE_OPENCV_GEOMETRY
		return cv::isContourConvex(*contour);
#else
		throw_no_geometry();
#endif
	}
	CVAPI_CATCH_CV_ERRORS(false)
}
float cveIntersectConvexConvex(cv::_InputArray* p1, cv::_InputArray* p2, cv::_OutputArray* p12, bool handleNested)
{
	try
	{
#ifdef HAVE_OPENCV_GEOMETRY
		return cv::intersectConvexConvex(*p1, *p2, *p12, handleNested);
#else
		throw_no_geometry();
#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveBoundingRectangle(cv::_InputArray* points, cv::Rect* boundingRect)
{
	try
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
	CVAPI_CATCH_CV_ERRORS_VOID
}
double cveArcLength(cv::_InputArray* curve, bool closed)
{
	try
	{
#ifdef HAVE_OPENCV_GEOMETRY
		return cv::arcLength(*curve, closed);
#else
		throw_no_geometry();
#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveMinAreaRect(cv::_InputArray* points, cv::RotatedRect* box)
{
	try
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
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveBoxPoints(cv::RotatedRect* box, cv::_OutputArray* points)
{
	try
	{
#ifdef HAVE_OPENCV_GEOMETRY
		cv::boxPoints(*box, *points);
#else
		throw_no_geometry();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
double cveMinEnclosingTriangle(cv::_InputArray* points, cv::_OutputArray* triangle)
{
	try
	{
#ifdef HAVE_OPENCV_GEOMETRY
		return cv::minEnclosingTriangle(*points, *triangle);
#else
		throw_no_geometry();
#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveMinEnclosingCircle(cv::_InputArray* points, cv::Point2f* center, float* radius)
{
	try
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
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveFitEllipse(cv::_InputArray* points, cv::RotatedRect* box)
{
	try
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
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveFitEllipseAMS(cv::_InputArray* points, cv::RotatedRect* box)
{
	try
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
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveFitEllipseDirect(cv::_InputArray* points, cv::RotatedRect* box)
{
	try
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
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveGetClosestEllipsePoints(cv::RotatedRect* ellipseParams, cv::_InputArray* points, cv::_OutputArray* closestPts)
{
	try
	{
#ifdef HAVE_OPENCV_GEOMETRY
		cv::getClosestEllipsePoints(*ellipseParams, *points, *closestPts);
#else
		throw_no_geometry();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveFitLine(cv::_InputArray* points, cv::_OutputArray* line, int distType, double param, double reps, double aeps)
{
	try
	{
#ifdef HAVE_OPENCV_GEOMETRY
		cv::fitLine(*points, *line, distType, param, reps, aeps);
#else
		throw_no_geometry();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
int cveRotatedRectangleIntersection(cv::RotatedRect* rect1, cv::RotatedRect* rect2, cv::_OutputArray* intersectingRegion)
{
	try
	{
#ifdef HAVE_OPENCV_GEOMETRY
		return cv::rotatedRectangleIntersection(*rect1, *rect2, *intersectingRegion);
#else
		throw_no_geometry();
#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveApproxPolyDP(cv::_InputArray* curve, cv::_OutputArray* approxCurve, double epsilon, bool closed)
{
	try
	{
#ifdef HAVE_OPENCV_GEOMETRY
		cv::approxPolyDP(*curve, *approxCurve, epsilon, closed);
#else
		throw_no_geometry();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveApproxPolyN(cv::_InputArray* curve, cv::_OutputArray* approxCurve, int nsides, float epsilonPercentage, bool ensureConvex)
{
	try
	{
#ifdef HAVE_OPENCV_GEOMETRY
		cv::approxPolyN(*curve, *approxCurve, nsides, epsilonPercentage, ensureConvex);
#else
		throw_no_geometry();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveConvexHull(cv::_InputArray* points, cv::_OutputArray* hull, bool clockwise, bool returnPoints)
{
	try
	{
#ifdef HAVE_OPENCV_GEOMETRY
		cv::convexHull(*points, *hull, clockwise, returnPoints);
#else
		throw_no_geometry();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveConvexityDefects(cv::_InputArray* contour, cv::_InputArray* convexhull, cv::_OutputArray* convexityDefects)
{
	try
	{
#ifdef HAVE_OPENCV_GEOMETRY
		cv::convexityDefects(*contour, *convexhull, *convexityDefects);
#else
		throw_no_geometry();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

//Subdiv2D
cv::Subdiv2D* cveSubdiv2DCreate(cv::Rect* rect)
{
	try
	{
#ifdef HAVE_OPENCV_GEOMETRY
		return new cv::Subdiv2D(*rect);
#else
		throw_no_geometry();
#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveSubdiv2DRelease(cv::Subdiv2D** subdiv)
{
	try
	{
#ifdef HAVE_OPENCV_GEOMETRY
		delete* subdiv;
		*subdiv = 0;
#else
		throw_no_geometry();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveSubdiv2DInsertMulti(cv::Subdiv2D* subdiv, std::vector<cv::Point2f>* points)
{
	try
	{
#ifdef HAVE_OPENCV_GEOMETRY
		subdiv->insert(*points);
#else
		throw_no_geometry();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
int cveSubdiv2DInsertSingle(cv::Subdiv2D* subdiv, cv::Point2f* pt)
{
	try
	{
#ifdef HAVE_OPENCV_GEOMETRY
		return subdiv->insert(*pt);
#else
		throw_no_geometry();
#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveSubdiv2DGetTriangleList(cv::Subdiv2D* subdiv, std::vector<cv::Vec6f>* triangleList)
{
	try
	{
#ifdef HAVE_OPENCV_GEOMETRY
		subdiv->getTriangleList(*triangleList);
#else
		throw_no_geometry();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveSubdiv2DGetVoronoiFacetList(cv::Subdiv2D* subdiv, std::vector<int>* idx, std::vector< std::vector< cv::Point2f> >* facetList, std::vector< cv::Point2f >* facetCenters)
{
	try
	{
#ifdef HAVE_OPENCV_GEOMETRY
		subdiv->getVoronoiFacetList(*idx, *facetList, *facetCenters);
#else
		throw_no_geometry();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
int cveSubdiv2DFindNearest(cv::Subdiv2D* subdiv, cv::Point2f* pt, cv::Point2f* nearestPt)
{
	try
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
	CVAPI_CATCH_CV_ERRORS(0)
}
int cveSubdiv2DLocate(cv::Subdiv2D* subdiv, cv::Point2f* pt, int* edge, int* vertex)
{
	try
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
	CVAPI_CATCH_CV_ERRORS(-2) // cv::Subdiv2D::PTLOC_ERROR
}
