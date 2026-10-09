//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "viz_c.h"

cv::viz::Viz3d* cveViz3dCreate(cv::String* s)
{
	try
	{
#ifdef HAVE_OPENCV_VIZ
		cv::viz::Viz3d* viz3d = new cv::viz::Viz3d(*s);
		return viz3d;
#else
		throw_no_viz();
#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveViz3dShowWidget(cv::viz::Viz3d* viz, cv::String* id, cv::viz::Widget* widget, cv::Affine3d* pose)
{
	try
	{
#ifdef HAVE_OPENCV_VIZ
		viz->showWidget(*id, *widget, pose ? *pose : cv::Affine3d::Identity());
#else
		throw_no_viz();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveViz3dSetWidgetPose(cv::viz::Viz3d* viz, cv::String* id, cv::Affine3d* pose)
{
	try
	{
#ifdef HAVE_OPENCV_VIZ
		viz->setWidgetPose(*id, *pose);
#else
		throw_no_viz();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveViz3dRemoveWidget(cv::viz::Viz3d* viz, cv::String* id)
{
	try
	{
#ifdef HAVE_OPENCV_VIZ
		viz->removeWidget(*id);
#else
		throw_no_viz();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveViz3dSetBackgroundMeshLab(cv::viz::Viz3d* viz)
{
	try
	{
#ifdef HAVE_OPENCV_VIZ
		viz->setBackgroundMeshLab();
#else
		throw_no_viz();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveViz3dSpin(cv::viz::Viz3d* viz)
{
	try
	{
#ifdef HAVE_OPENCV_VIZ
		viz->spin();
#else
		throw_no_viz();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveViz3dSpinOnce(cv::viz::Viz3d* viz, int time, bool forceRedraw)
{
	try
	{
#ifdef HAVE_OPENCV_VIZ
		viz->spinOnce(time, forceRedraw);
#else
		throw_no_viz();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
bool cveViz3dWasStopped(cv::viz::Viz3d* viz)
{
	try
	{
#ifdef HAVE_OPENCV_VIZ
		return viz->wasStopped();
#else
		throw_no_viz();
#endif
	}
	CVAPI_CATCH_CV_ERRORS(false)
}
void cveViz3dRelease(cv::viz::Viz3d** viz)
{
	try
	{
#ifdef HAVE_OPENCV_VIZ
		delete* viz;
		*viz = 0;
#else
		throw_no_viz();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

cv::viz::WText* cveWTextCreate(cv::String* text, cv::Point* pos, int fontSize, cv::Scalar* color, cv::viz::Widget2D** widget2D, cv::viz::Widget** widget)
{
	try
	{
#ifdef HAVE_OPENCV_VIZ
		cv::viz::Color c = cv::viz::Color(*color);
		cv::viz::WText* t = new cv::viz::WText(*text, *pos, fontSize, c);
		*widget2D = dynamic_cast<cv::viz::Widget2D*>(t);
		*widget = dynamic_cast<cv::viz::Widget*>(t);
		return t;
#else
		throw_no_viz();
#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveWTextRelease(cv::viz::WText** text)
{
	try
	{
#ifdef HAVE_OPENCV_VIZ
		delete* text;
		*text = 0;
#else
		throw_no_viz();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

cv::viz::WCoordinateSystem* cveWCoordinateSystemCreate(double scale, cv::viz::Widget3D** widget3d, cv::viz::Widget** widget)
{
	try
	{
#ifdef HAVE_OPENCV_VIZ
		cv::viz::WCoordinateSystem* system = new cv::viz::WCoordinateSystem(scale);
		*widget3d = dynamic_cast<cv::viz::Widget3D*>(system);
		*widget = dynamic_cast<cv::viz::Widget*>(system);
		return system;
#else
		throw_no_viz();
#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveWCoordinateSystemRelease(cv::viz::WCoordinateSystem** system)
{
	try
	{
#ifdef HAVE_OPENCV_VIZ
		delete* system;
		*system = 0;
#else
		throw_no_viz();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

cv::viz::WCloud* cveWCloudCreateWithColorArray(cv::_InputArray* cloud, cv::_InputArray* color, cv::viz::Widget3D** widget3d, cv::viz::Widget** widget)
{
	try
	{
#ifdef HAVE_OPENCV_VIZ
		cv::viz::WCloud* wcloud = new cv::viz::WCloud(*cloud, *color);
		*widget3d = dynamic_cast<cv::viz::Widget3D*>(wcloud);
		*widget = dynamic_cast<cv::viz::Widget*>(wcloud);
		return wcloud;
#else
		throw_no_viz();
#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::viz::WCloud* cveWCloudCreateWithColor(cv::_InputArray* cloud, cv::Scalar* color, cv::viz::Widget3D** widget3d, cv::viz::Widget** widget)
{
	try
	{
#ifdef HAVE_OPENCV_VIZ
		cv::viz::Color c = cv::viz::Color(*color);
		cv::viz::WCloud* wcloud = new cv::viz::WCloud(*cloud, c);
		*widget3d = dynamic_cast<cv::viz::Widget3D*>(wcloud);
		*widget = dynamic_cast<cv::viz::Widget*>(wcloud);
		return wcloud;
#else
		throw_no_viz();
#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveWCloudRelease(cv::viz::WCloud** cloud)
{
	try
	{
#ifdef HAVE_OPENCV_VIZ
		delete* cloud;
		*cloud = 0;
#else
		throw_no_viz();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveWriteCloud(cv::String* file, cv::_InputArray* cloud, cv::_InputArray* colors, cv::_InputArray* normals, bool binary)
{
	try
	{
#ifdef HAVE_OPENCV_VIZ
		cv::viz::writeCloud(*file, *cloud, colors ? *colors : (cv::InputArray) cv::noArray(), normals ? *normals : (cv::InputArray) cv::noArray(), binary);
#else
		throw_no_viz();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveReadCloud(cv::String* file, cv::Mat* cloud, cv::_OutputArray* colors, cv::_OutputArray* normals)
{
	try
	{
#ifdef HAVE_OPENCV_VIZ
		cv::Mat r = cv::viz::readCloud(*file, colors ? *colors : (cv::OutputArray) cv::noArray(), normals ? *normals : (cv::OutputArray) cv::noArray());
		cv::swap(r, *cloud);
#else
		throw_no_viz();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

cv::viz::WCube* cveWCubeCreate(cv::Point3d* minPoint, cv::Point3d* maxPoint, bool wireFrame, cv::Scalar* color, cv::viz::Widget3D** widget3d, cv::viz::Widget** widget)
{
	try
	{
#ifdef HAVE_OPENCV_VIZ
		cv::viz::Color c = cv::viz::Color(*color);
		cv::Point3d minp = cv::Point3d(minPoint->x, minPoint->y, minPoint->z);
		cv::Point3d maxp = cv::Point3d(maxPoint->x, maxPoint->y, maxPoint->z);
		cv::viz::WCube* cube = new cv::viz::WCube(minp, maxp, wireFrame, c);
		*widget3d = dynamic_cast<cv::viz::Widget3D*>(cube);
		*widget = dynamic_cast<cv::viz::Widget*>(cube);
		return cube;
#else
		throw_no_viz();
#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveWCubeRelease(cv::viz::WCube** cube)
{
	try
	{
#ifdef HAVE_OPENCV_VIZ
		delete* cube;
		*cube = 0;
#else
		throw_no_viz();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

cv::viz::WCylinder* cveWCylinderCreate(cv::Point3d* axisPoint1, cv::Point3d* axisPoint2, double radius, int numsides, cv::Scalar* color, cv::viz::Widget3D** widget3d, cv::viz::Widget** widget)
{
	try
	{
#ifdef HAVE_OPENCV_VIZ
		cv::viz::Color c = cv::viz::Color(*color);
		cv::Point3d ap1 = cv::Point3d(axisPoint1->x, axisPoint1->y, axisPoint1->z);
		cv::Point3d ap2 = cv::Point3d(axisPoint2->x, axisPoint2->y, axisPoint2->z);
		cv::viz::WCylinder* cylinder = new cv::viz::WCylinder(ap1, ap2, radius, numsides, c);
		*widget3d = dynamic_cast<cv::viz::Widget3D*>(cylinder);
		*widget = dynamic_cast<cv::viz::Widget*>(cylinder);
		return cylinder;
#else
		throw_no_viz();
#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

void cveWCylinderRelease(cv::viz::WCylinder** cylinder)
{
	try
	{
#ifdef HAVE_OPENCV_VIZ
		delete* cylinder;
		*cylinder = 0;
#else
		throw_no_viz();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

cv::viz::WCircle* cveWCircleCreateAtOrigin(double radius, double thickness, cv::Scalar* color, cv::viz::Widget3D** widget3d, cv::viz::Widget** widget)
{
	try
	{
#ifdef HAVE_OPENCV_VIZ
		cv::viz::Color c = cv::viz::Color(*color);
		cv::viz::WCircle* circle = new cv::viz::WCircle(radius, thickness, c);
		*widget3d = dynamic_cast<cv::viz::Widget3D*>(circle);
		*widget = dynamic_cast<cv::viz::Widget*>(circle);
		return circle;
#else
		throw_no_viz();
#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::viz::WCircle* cveWCircleCreate(double radius, cv::Point3d* center, cv::Point3d* normal, double thickness, cv::Scalar* color, cv::viz::Widget3D** widget3d, cv::viz::Widget** widget)
{
	try
	{
#ifdef HAVE_OPENCV_VIZ
		cv::viz::Color c = cv::viz::Color(*color);
		cv::Point3d cp = cv::Point3d(center->x, center->y, center->z);
		cv::Point3d n = cv::Point3d(normal->x, normal->y, normal->z);
		cv::viz::WCircle* circle = new cv::viz::WCircle(radius, cp, n, thickness, c);
		*widget3d = dynamic_cast<cv::viz::Widget3D*>(circle);
		*widget = dynamic_cast<cv::viz::Widget*>(circle);
		return circle;
#else
		throw_no_viz();
#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveWCircleRelease(cv::viz::WCircle** circle)
{
	try
	{
#ifdef HAVE_OPENCV_VIZ
		delete* circle;
		*circle = 0;
#else
		throw_no_viz();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

cv::viz::WCone* cveWConeCreateAtOrigin(double length, double radius, int resolution, cv::Scalar* color, cv::viz::Widget3D** widget3d, cv::viz::Widget** widget)
{
	try
	{
#ifdef HAVE_OPENCV_VIZ
		cv::viz::Color c = cv::viz::Color(*color);
		cv::viz::WCone* cone = new cv::viz::WCone(length, radius, resolution, c);
		*widget3d = dynamic_cast<cv::viz::Widget3D*>(cone);
		*widget = dynamic_cast<cv::viz::Widget*>(cone);
		return cone;
#else
		throw_no_viz();
#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::viz::WCone* cveWConeCreate(double radius, cv::Point3d* center, cv::Point3d* tip, int resolution, cv::Scalar* color, cv::viz::Widget3D** widget3d, cv::viz::Widget** widget)
{
	try
	{
#ifdef HAVE_OPENCV_VIZ
		cv::viz::Color c = cv::viz::Color(*color);
		cv::Point3d cp = cv::Point3d(center->x, center->y, center->z);
		cv::Point3d tp = cv::Point3d(tip->x, tip->y, tip->z);
		cv::viz::WCone* cone = new cv::viz::WCone(radius, cp, tp, resolution, c);
		*widget3d = dynamic_cast<cv::viz::Widget3D*>(cone);
		*widget = dynamic_cast<cv::viz::Widget*>(cone);
		return cone;
#else
		throw_no_viz();
#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveWConeRelease(cv::viz::WCone** cone)
{
	try
	{
#ifdef HAVE_OPENCV_VIZ
		delete* cone;
		*cone = 0;
#else
		throw_no_viz();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

cv::viz::WArrow* cveWArrowCreate(cv::Point3d* pt1, cv::Point3d* pt2, double thickness, cv::Scalar* color, cv::viz::Widget3D** widget3d, cv::viz::Widget** widget)
{
	try
	{
#ifdef HAVE_OPENCV_VIZ
		cv::viz::Color c = cv::viz::Color(*color);
		cv::Point3d p1 = cv::Point3d(pt1->x, pt1->y, pt1->z);
		cv::Point3d p2 = cv::Point3d(pt2->x, pt2->y, pt2->z);
		cv::viz::WArrow* arrow = new cv::viz::WArrow(p1, p2, thickness, c);
		*widget3d = dynamic_cast<cv::viz::Widget3D*>(arrow);
		*widget = dynamic_cast<cv::viz::Widget*>(arrow);
		return arrow;
#else
		throw_no_viz();
#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveWArrowRelease(cv::viz::WArrow** arrow)
{
	try
	{
#ifdef HAVE_OPENCV_VIZ
		delete* arrow;
		*arrow = 0;
#else
		throw_no_viz();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
