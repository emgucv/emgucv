//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "plot_c.h"

cv::plot::Plot2d* cvePlot2dCreateFromX(cv::Mat* data)
{
   cv::Ptr<cv::plot::Plot2d> plot = cv::plot::createPlot2d(*data);
   plot.addref();
   return plot.get();
}
cv::plot::Plot2d* cvePlot2dCreateFromXY(cv::Mat* dataX, cv::Mat* dataY)
{
   cv::Ptr<cv::plot::Plot2d> plot = cv::plot::createPlot2d(*dataX, *dataY);
   plot.addref();
   return plot.get();
}
void cvePlot2dRender(cv::plot::Plot2d* plot, cv::Mat* result)
{
   plot->render(*result);
}
void cvePlot2dRelease(cv::plot::Plot2d** plot)
{
   delete *plot;
   *plot = 0;
}