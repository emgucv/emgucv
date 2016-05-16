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