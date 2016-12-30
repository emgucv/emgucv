//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2017 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_PLOT_C_H
#define EMGU_PLOT_C_H

#include "opencv2/core/core_c.h"
#include "opencv2/plot.hpp"

CVAPI(cv::plot::Plot2d*) cvePlot2dCreateFromX(cv::Mat* data);
CVAPI(cv::plot::Plot2d*) cvePlot2dCreateFromXY(cv::Mat* dataX, cv::Mat* dataY);
CVAPI(void) cvePlot2dRender(cv::plot::Plot2d* plot, cv::Mat* result);
CVAPI(void) cvePlot2dRelease(cv::plot::Plot2d** plot);

CVAPI(void) cvePlot2dSetPlotLineColor(cv::plot::Plot2d* plot, CvScalar* plotLineColor);
CVAPI(void) cvePlot2dSetPlotBackgroundColor(cv::plot::Plot2d* plot, CvScalar* plotBackgroundColor);
CVAPI(void) cvePlot2dSetPlotAxisColor(cv::plot::Plot2d* plot, CvScalar* plotAxisColor);
CVAPI(void) cvePlot2dSetPlotGridColor(cv::plot::Plot2d* plot, CvScalar* plotGridColor);
CVAPI(void) cvePlot2dSetPlotTextColor(cv::plot::Plot2d* plot, CvScalar* plotTextColor);
CVAPI(void) cvePlot2dSetPlotSize(cv::plot::Plot2d* plot, int plotSizeWidth, int plotSizeHeight);


#endif