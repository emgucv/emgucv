//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_VIZ_C_H
#define EMGU_VIZ_C_H

#include "opencv2/opencv_modules.hpp"
#include "opencv2/core/core.hpp"
#include "cvapi_compat.h"

#ifdef HAVE_OPENCV_VIZ
#include "opencv2/viz.hpp"
#else
static inline CV_NORETURN void throw_no_viz() { CV_Error(cv::Error::StsBadFunc, "The library is compiled without Viz support. To use this module, please switch to the full Emgu CV runtime."); }

namespace cv
{
	namespace viz
	{
		class Viz3d
		{
		};

		class WText
		{
		};

		class WCoordinateSystem
		{
			
		};

		class WCloud
		{
			
		};

		class WCube
		{
			
		};

		class WCylinder
		{
			
		};

		class WCircle
		{
			
		};

		class WCone
		{
			
		};

		class WArrow
		{
			
		};

		class Widget
		{
			
		};

		class Widget2D
		{
			
		};

		class Widget3D
		{

		};
		
	}
}

namespace cv
{
	class Affine3d
	{

	};
}

#endif

CVAPI(cv::viz::Viz3d*) cveViz3dCreate(cv::String* s);
CVAPI(void) cveViz3dShowWidget(cv::viz::Viz3d* viz, cv::String* id, cv::viz::Widget* widget, cv::Affine3d* pose);
CVAPI(void) cveViz3dSetWidgetPose(cv::viz::Viz3d* viz, cv::String* id, cv::Affine3d* pose);
CVAPI(void) cveViz3dRemoveWidget(cv::viz::Viz3d* viz, cv::String* id);
CVAPI(void) cveViz3dSetBackgroundMeshLab(cv::viz::Viz3d* viz);

CVAPI(void) cveViz3dSpin(cv::viz::Viz3d* viz);
CVAPI(void) cveViz3dSpinOnce(cv::viz::Viz3d* viz, int time, bool forceRedraw);
CVAPI(bool) cveViz3dWasStopped(cv::viz::Viz3d* viz);
CVAPI(void) cveViz3dRelease(cv::viz::Viz3d** viz);

CVAPI(cv::viz::WText*) cveWTextCreate(cv::String* text, cv::Point* pos, int fontSize, cv::Scalar* color, cv::viz::Widget2D** widget2D, cv::viz::Widget** widget);
CVAPI(void) cveWTextRelease(cv::viz::WText** text);

CVAPI(cv::viz::WCoordinateSystem*) cveWCoordinateSystemCreate(double scale, cv::viz::Widget3D** widget3d, cv::viz::Widget** widget);
CVAPI(void) cveWCoordinateSystemRelease(cv::viz::WCoordinateSystem** system);

CVAPI(cv::viz::WCloud*) cveWCloudCreateWithColorArray(cv::_InputArray* cloud, cv::_InputArray* color, cv::viz::Widget3D** widget3d, cv::viz::Widget** widget);
CVAPI(cv::viz::WCloud*) cveWCloudCreateWithColor(cv::_InputArray* cloud, cv::Scalar* color, cv::viz::Widget3D** widget3d, cv::viz::Widget** widget);
CVAPI(void) cveWCloudRelease(cv::viz::WCloud** cloud);

CVAPI(void) cveWriteCloud(cv::String* file, cv::_InputArray* cloud, cv::_InputArray* colors, cv::_InputArray*, bool binary);
CVAPI(void) cveReadCloud(cv::String* file, cv::Mat* cloud, cv::_OutputArray* colors, cv::_OutputArray* normals);

CVAPI(cv::viz::WCube*) cveWCubeCreate(cv::Point3d* minPoint, cv::Point3d* maxPoint, bool wireFrame, cv::Scalar* color, cv::viz::Widget3D** widget3d, cv::viz::Widget** widget);
CVAPI(void) cveWCubeRelease(cv::viz::WCube** cube);

CVAPI(cv::viz::WCylinder*) cveWCylinderCreate(cv::Point3d* axisPoint1, cv::Point3d* axisPoint2, double radius, int numsides, cv::Scalar* color, cv::viz::Widget3D** widget3d, cv::viz::Widget** widget);
CVAPI(void) cveWCylinderRelease(cv::viz::WCylinder** cylinder);

CVAPI(cv::viz::WCircle*) cveWCircleCreateAtOrigin(double radius, double thickness, cv::Scalar* color, cv::viz::Widget3D** widget3d, cv::viz::Widget** widget);
CVAPI(cv::viz::WCircle*) cveWCircleCreate(double radius, cv::Point3d* center, cv::Point3d* normal, double thickness, cv::Scalar* color, cv::viz::Widget3D** widget3d, cv::viz::Widget** widget);
CVAPI(void) cveWCircleRelease(cv::viz::WCircle** circle);

CVAPI(cv::viz::WCone*) cveWConeCreateAtOrigin(double length, double radius, int resolution, cv::Scalar* color, cv::viz::Widget3D** widget3d, cv::viz::Widget** widget);
CVAPI(cv::viz::WCone*) cveWConeCreate(double radius, cv::Point3d* center, cv::Point3d* tip, int resolution, cv::Scalar* color, cv::viz::Widget3D** widget3d, cv::viz::Widget** widget);
CVAPI(void) cveWConeRelease(cv::viz::WCone** cone);

CVAPI(cv::viz::WArrow*) cveWArrowCreate(cv::Point3d* pt1, cv::Point3d* pt2, double thickness, cv::Scalar* color, cv::viz::Widget3D** widget3d, cv::viz::Widget** widget);
CVAPI(void) cveWArrowRelease(cv::viz::WArrow** arrow);
#endif