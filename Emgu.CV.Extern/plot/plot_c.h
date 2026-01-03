//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_PLOT_C_H
#define EMGU_PLOT_C_H

#include "opencv2/core/core.hpp"
#include "cvapi_compat.h"

#ifdef HAVE_OPENCV_PLOT
#include "opencv2/plot.hpp"
#else
static inline CV_NORETURN void throw_no_plot() { CV_Error(cv::Error::StsBadFunc, "The library is compiled without plot module support. To use this module, please switch to the full Emgu CV runtime."); }
namespace cv {
namespace plot {
class Plot2d {};
}
}
#endif

CVAPI(cv::plot::Plot2d*) cvePlot2dCreateFrom(cv::_InputArray* data, cv::Ptr<cv::plot::Plot2d>** sharedPtr);
CVAPI(cv::plot::Plot2d*) cvePlot2dCreateFromXY(cv::_InputArray* dataX, cv::_InputArray* dataY, cv::Ptr<cv::plot::Plot2d>** sharedPtr);
CVAPI(void) cvePlot2dRender(cv::plot::Plot2d* plot, cv::_OutputArray* result);
CVAPI(void) cvePlot2dRelease(cv::plot::Plot2d** plot, cv::Ptr<cv::plot::Plot2d>** sharedPtr);

CVAPI(void) cvePlot2dSetPlotLineColor(cv::plot::Plot2d* plot, cv::Scalar* plotLineColor);
CVAPI(void) cvePlot2dSetPlotBackgroundColor(cv::plot::Plot2d* plot, cv::Scalar* plotBackgroundColor);
CVAPI(void) cvePlot2dSetPlotAxisColor(cv::plot::Plot2d* plot, cv::Scalar* plotAxisColor);
CVAPI(void) cvePlot2dSetPlotGridColor(cv::plot::Plot2d* plot, cv::Scalar* plotGridColor);
CVAPI(void) cvePlot2dSetPlotTextColor(cv::plot::Plot2d* plot, cv::Scalar* plotTextColor);
CVAPI(void) cvePlot2dSetPlotSize(cv::plot::Plot2d* plot, int plotSizeWidth, int plotSizeHeight);

#endif