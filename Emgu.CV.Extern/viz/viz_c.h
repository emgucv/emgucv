//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_VIZ_C_H
#define EMGU_VIZ_C_H

#include "opencv2/core/core_c.h"
#include "opencv2/viz.hpp"

CVAPI(cv::viz::Viz3d*) cveViz3dCreate(cv::String* s);
CVAPI(void) cveViz3dShowWidget(cv::viz::Viz3d* viz, cv::String* id, cv::viz::Widget* widget, cv::Affine3d* pose);
CVAPI(void) cveViz3dSetWidgetPose(cv::viz::Viz3d* viz, cv::String* id, cv::Affine3d* pose);
CVAPI(void) cveViz3dRemoveWidget(cv::viz::Viz3d* viz, cv::String* id);
CVAPI(void) cveViz3dSetBackgroundMeshLab(cv::viz::Viz3d* viz);

CVAPI(void) cveViz3dSpin(cv::viz::Viz3d* viz);
CVAPI(void) cveViz3dSpinOnce(cv::viz::Viz3d* viz, int time, bool forceRedraw);
CVAPI(bool) cveViz3dWasStopped(cv::viz::Viz3d* viz);
CVAPI(void) cveViz3dRelease(cv::viz::Viz3d** viz);

CVAPI(cv::viz::WText*) cveWTextCreate(cv::String* text, CvPoint* pos, int fontSize, CvScalar* color, cv::viz::Widget2D** widget2D, cv::viz::Widget** widget);
CVAPI(void) cveWTextRelease(cv::viz::WText** text);

CVAPI(cv::viz::WCoordinateSystem*) cveWCoordinateSystemCreate(double scale, cv::viz::Widget3D** widget3d, cv::viz::Widget** widget);
CVAPI(void) cveWCoordinateSystemRelease(cv::viz::WCoordinateSystem** system);

CVAPI(cv::viz::WCloud*) cveWCloudCreateWithColorArray(cv::_InputArray* cloud, cv::_InputArray* color, cv::viz::Widget3D** widget3d, cv::viz::Widget** widget);
CVAPI(cv::viz::WCloud*) cveWCloudCreateWithColor(cv::_InputArray* cloud, CvScalar* color, cv::viz::Widget3D** widget3d, cv::viz::Widget** widget);
CVAPI(void) cveWCloudRelease(cv::viz::WCloud** cloud);

CVAPI(void) cveWriteCloud(cv::String* file, cv::_InputArray* cloud, cv::_InputArray* colors, cv::_InputArray*, bool binary);
CVAPI(void) cveReadCloud(cv::String* file, cv::Mat* cloud, cv::_OutputArray* colors, cv::_OutputArray* normals);

CVAPI(cv::viz::WCube*) cveWCubeCreate(CvPoint3D64f* minPoint, CvPoint3D64f* maxPoint, bool wireFrame, CvScalar* color, cv::viz::Widget3D** widget3d, cv::viz::Widget** widget);
CVAPI(void) cveWCubeRelease(cv::viz::WCube** cube);

CVAPI(cv::viz::WCylinder*) cveWCylinderCreate(CvPoint3D64f* axisPoint1, CvPoint3D64f* axisPoint2, double radius, int numsides, CvScalar* color, cv::viz::Widget3D** widget3d, cv::viz::Widget** widget);
CVAPI(void) cveWCylinderRelease(cv::viz::WCylinder** cylinder);

CVAPI(cv::viz::WCircle*) cveWCircleCreateAtOrigin(double radius, double thickness, CvScalar* color, cv::viz::Widget3D** widget3d, cv::viz::Widget** widget);
CVAPI(cv::viz::WCircle*) cveWCircleCreate(double radius, CvPoint3D64f* center, CvPoint3D64f* normal, double thickness, CvScalar* color, cv::viz::Widget3D** widget3d, cv::viz::Widget** widget);
CVAPI(void) cveWCircleRelease(cv::viz::WCircle** circle);

CVAPI(cv::viz::WCone*) cveWConeCreateAtOrigin(double length, double radius, int resolution, CvScalar* color, cv::viz::Widget3D** widget3d, cv::viz::Widget** widget);
CVAPI(cv::viz::WCone*) cveWConeCreate(double radius, CvPoint3D64f* center, CvPoint3D64f* tip, int resolution, CvScalar* color, cv::viz::Widget3D** widget3d, cv::viz::Widget** widget);
CVAPI(void) cveWConeRelease(cv::viz::WCone** cone);

CVAPI(cv::viz::WArrow*) cveWArrowCreate(CvPoint3D64f* pt1, CvPoint3D64f* pt2, double thickness, CvScalar* color, cv::viz::Widget3D** widget3d, cv::viz::Widget** widget);
CVAPI(void) cveWArrowRelease(cv::viz::WArrow** arrow);
#endif