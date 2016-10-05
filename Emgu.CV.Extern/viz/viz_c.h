//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.
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

#endif