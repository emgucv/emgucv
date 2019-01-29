//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2019 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "plot_c.h"

cv::plot::Plot2d* cvePlot2dCreateFrom(cv::_InputArray* data, cv::Ptr<cv::plot::Plot2d>** sharedPtr)
{
   cv::Ptr<cv::plot::Plot2d> plot = cv::plot::Plot2d::create(*data);
   *sharedPtr = new cv::Ptr<cv::plot::Plot2d>(plot);
   return plot.get();
}
cv::plot::Plot2d* cvePlot2dCreateFromXY(cv::_InputArray* dataX, cv::_InputArray* dataY, cv::Ptr<cv::plot::Plot2d>** sharedPtr)
{
   cv::Ptr<cv::plot::Plot2d> plot = cv::plot::Plot2d::create(*dataX, *dataY);
   *sharedPtr = new cv::Ptr<cv::plot::Plot2d>(plot);
   return plot.get();
}
void cvePlot2dRender(cv::plot::Plot2d* plot, cv::_OutputArray* result)
{
   plot->render(*result);
}
void cvePlot2dRelease(cv::plot::Plot2d** plot, cv::Ptr<cv::plot::Plot2d>** sharedPtr)
{
   delete *sharedPtr;
   *plot = 0;
   *sharedPtr = 0;
}

void cvePlot2dSetPlotLineColor(cv::plot::Plot2d* plot, CvScalar* plotLineColor)
{
   plot->setPlotLineColor(*plotLineColor);
}
void cvePlot2dSetPlotBackgroundColor(cv::plot::Plot2d* plot, CvScalar* plotBackgroundColor)
{
   plot->setPlotBackgroundColor(*plotBackgroundColor);
}
void cvePlot2dSetPlotAxisColor(cv::plot::Plot2d* plot, CvScalar* plotAxisColor)
{
   plot->setPlotAxisColor(*plotAxisColor);
}
void cvePlot2dSetPlotGridColor(cv::plot::Plot2d* plot, CvScalar* plotGridColor)
{
   plot->setPlotGridColor(*plotGridColor);
}
void cvePlot2dSetPlotTextColor(cv::plot::Plot2d* plot, CvScalar* plotTextColor)
{
   plot->setPlotTextColor(*plotTextColor);
}
void cvePlot2dSetPlotSize(cv::plot::Plot2d* plot, int plotSizeWidth, int plotSizeHeight)
{
   plot->setPlotSize(plotSizeWidth, plotSizeHeight);
}