//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "plot_c.h"

cv::plot::Plot2d* cvePlot2dCreateFrom(cv::_InputArray* data, cv::Ptr<cv::plot::Plot2d>** sharedPtr)
{
#ifdef HAVE_OPENCV_PLOT
   cv::Ptr<cv::plot::Plot2d> plot = cv::plot::Plot2d::create(*data);
   *sharedPtr = new cv::Ptr<cv::plot::Plot2d>(plot);
   return plot.get();
#else
	throw_no_plot();
#endif
}
cv::plot::Plot2d* cvePlot2dCreateFromXY(cv::_InputArray* dataX, cv::_InputArray* dataY, cv::Ptr<cv::plot::Plot2d>** sharedPtr)
{
#ifdef HAVE_OPENCV_PLOT
   cv::Ptr<cv::plot::Plot2d> plot = cv::plot::Plot2d::create(*dataX, *dataY);
   *sharedPtr = new cv::Ptr<cv::plot::Plot2d>(plot);
   return plot.get();
#else
	throw_no_plot();
#endif
}
void cvePlot2dRender(cv::plot::Plot2d* plot, cv::_OutputArray* result)
{
#ifdef HAVE_OPENCV_PLOT
   plot->render(*result);
#else
	throw_no_plot();
#endif
}
void cvePlot2dRelease(cv::plot::Plot2d** plot, cv::Ptr<cv::plot::Plot2d>** sharedPtr)
{
#ifdef HAVE_OPENCV_PLOT
   delete *sharedPtr;
   *plot = 0;
   *sharedPtr = 0;
#else
	throw_no_plot();
#endif
}

void cvePlot2dSetPlotLineColor(cv::plot::Plot2d* plot, CvScalar* plotLineColor)
{
#ifdef HAVE_OPENCV_PLOT
   plot->setPlotLineColor(*plotLineColor);
#else
	throw_no_plot();
#endif
}
void cvePlot2dSetPlotBackgroundColor(cv::plot::Plot2d* plot, CvScalar* plotBackgroundColor)
{
#ifdef HAVE_OPENCV_PLOT
   plot->setPlotBackgroundColor(*plotBackgroundColor);
#else
	throw_no_plot();
#endif
}
void cvePlot2dSetPlotAxisColor(cv::plot::Plot2d* plot, CvScalar* plotAxisColor)
{
#ifdef HAVE_OPENCV_PLOT
   plot->setPlotAxisColor(*plotAxisColor);
#else
	throw_no_plot();
#endif
}
void cvePlot2dSetPlotGridColor(cv::plot::Plot2d* plot, CvScalar* plotGridColor)
{
#ifdef HAVE_OPENCV_PLOT
   plot->setPlotGridColor(*plotGridColor);
#else
	throw_no_plot();
#endif
}
void cvePlot2dSetPlotTextColor(cv::plot::Plot2d* plot, CvScalar* plotTextColor)
{
#ifdef HAVE_OPENCV_PLOT
   plot->setPlotTextColor(*plotTextColor);
#else
	throw_no_plot();
#endif
}
void cvePlot2dSetPlotSize(cv::plot::Plot2d* plot, int plotSizeWidth, int plotSizeHeight)
{
#ifdef HAVE_OPENCV_PLOT
   plot->setPlotSize(plotSizeWidth, plotSizeHeight);
#else
	throw_no_plot();
#endif
}
