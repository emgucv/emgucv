//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "viz_c.h"

cv::viz::Viz3d* cveViz3dCreate(cv::String* s)
{
   cv::viz::Viz3d* viz3d = new cv::viz::Viz3d(*s);
   return viz3d;
}
void cveViz3dShowWidget(cv::viz::Viz3d* viz, cv::String* id, cv::viz::Widget* widget)
{
   viz->showWidget(*id, *widget);
}
void cveViz3dSetBackgroundMeshLab(cv::viz::Viz3d* viz)
{
   viz->setBackgroundMeshLab();
}
void cveViz3dSpin(cv::viz::Viz3d* viz)
{
   viz->spin();
}
void cveViz3dRelease(cv::viz::Viz3d** viz)
{
   delete *viz;
   *viz = 0;
}

cv::viz::WText* cveWTextCreate(cv::String* text, CvPoint* pos, int fontSize, CvScalar* color, cv::viz::Widget2D** widget2D, cv::viz::Widget** widget)
{
   cv::viz::Color c = cv::viz::Color(*color);
   cv::viz::WText* t = new cv::viz::WText(*text, *pos, fontSize, c);
   *widget2D = dynamic_cast<cv::viz::Widget2D*>(t);
   *widget = dynamic_cast<cv::viz::Widget*>(t);
   return t;
}
void cveWTextRelease(cv::viz::WText** text)
{
   delete *text;
   *text = 0;
}

cv::viz::WCoordinateSystem* cveWCoordinateSystemCreate(double scale, cv::viz::Widget3D** widget3d, cv::viz::Widget** widget)
{
   cv::viz::WCoordinateSystem* system = new cv::viz::WCoordinateSystem(scale);
   *widget3d = dynamic_cast<cv::viz::Widget3D*>(system);
   *widget = dynamic_cast<cv::viz::Widget*>(system);
   return system;
}
void cveWCoordinateSystemRelease(cv::viz::WCoordinateSystem** system)
{
   delete *system;
   *system = 0;
}

cv::viz::WCloud* cveWCloudCreateWithColorArray(cv::_InputArray* cloud, cv::_InputArray* color, cv::viz::Widget3D** widget3d, cv::viz::Widget** widget)
{
   cv::viz::WCloud* wcloud = new cv::viz::WCloud(*cloud, *color);
   *widget3d = dynamic_cast<cv::viz::Widget3D*>(wcloud);
   *widget = dynamic_cast<cv::viz::Widget*>(wcloud);
   return wcloud;
}
cv::viz::WCloud* cveWCloudCreateWithColor(cv::_InputArray* cloud, CvScalar* color, cv::viz::Widget3D** widget3d, cv::viz::Widget** widget)
{
   cv::viz::Color c = cv::viz::Color(*color);
   cv::viz::WCloud* wcloud = new cv::viz::WCloud(*cloud, c);
   *widget3d = dynamic_cast<cv::viz::Widget3D*>(wcloud);
   *widget = dynamic_cast<cv::viz::Widget*>(wcloud);
   return wcloud;
}
void cveWCloudRelease(cv::viz::WCloud** cloud)
{
   delete *cloud;
   *cloud = 0;
}