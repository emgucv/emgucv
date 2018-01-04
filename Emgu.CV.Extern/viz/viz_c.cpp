//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "viz_c.h"

cv::viz::Viz3d* cveViz3dCreate(cv::String* s)
{
   cv::viz::Viz3d* viz3d = new cv::viz::Viz3d(*s);
   return viz3d;
}
void cveViz3dShowWidget(cv::viz::Viz3d* viz, cv::String* id, cv::viz::Widget* widget, cv::Affine3d* pose)
{
   viz->showWidget(*id, *widget, pose ? *pose : cv::Affine3d::Identity());
}
void cveViz3dSetWidgetPose(cv::viz::Viz3d* viz, cv::String* id, cv::Affine3d* pose)
{
   viz->setWidgetPose(*id, *pose);
}
void cveViz3dRemoveWidget(cv::viz::Viz3d* viz, cv::String* id)
{
   viz->removeWidget(*id);
}
void cveViz3dSetBackgroundMeshLab(cv::viz::Viz3d* viz)
{
   viz->setBackgroundMeshLab();
}
void cveViz3dSpin(cv::viz::Viz3d* viz)
{
   viz->spin();
}
void cveViz3dSpinOnce(cv::viz::Viz3d* viz, int time, bool forceRedraw)
{
   viz->spinOnce(time, forceRedraw);
}
bool cveViz3dWasStopped(cv::viz::Viz3d* viz)
{
   return viz->wasStopped();
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

void cveWriteCloud(cv::String* file, cv::_InputArray* cloud, cv::_InputArray* colors, cv::_InputArray* normals, bool binary)
{
   cv::viz::writeCloud(*file, *cloud, colors ? *colors : (cv::InputArray) cv::noArray(), normals ? *normals : (cv::InputArray) cv::noArray(), binary);
}
void cveReadCloud(cv::String* file, cv::Mat* cloud, cv::_OutputArray* colors, cv::_OutputArray* normals)
{
   cv::Mat r = cv::viz::readCloud(*file, colors ? *colors : (cv::OutputArray) cv::noArray(), normals ? *normals : (cv::OutputArray) cv::noArray());
   cv::swap(r, *cloud);
}

cv::viz::WCube* cveWCubeCreate(CvPoint3D64f* minPoint, CvPoint3D64f* maxPoint, bool wireFrame, CvScalar* color, cv::viz::Widget3D** widget3d, cv::viz::Widget** widget)
{
   cv::viz::Color c = cv::viz::Color(*color);
   cv::Point3d minp = cv::Point3d(minPoint->x, minPoint->y, minPoint->z);
   cv::Point3d maxp = cv::Point3d(maxPoint->x, maxPoint->y, maxPoint->z);
   cv::viz::WCube* cube = new cv::viz::WCube(minp, maxp, wireFrame, c);
   *widget3d = dynamic_cast<cv::viz::Widget3D*>(cube);
   *widget = dynamic_cast<cv::viz::Widget*>(cube);
   return cube;
}
void cveWCubeRelease(cv::viz::WCube** cube)
{
   delete *cube;
   *cube = 0;
}

cv::viz::WCylinder* cveWCylinderCreate(CvPoint3D64f* axisPoint1, CvPoint3D64f* axisPoint2, double radius, int numsides, CvScalar* color, cv::viz::Widget3D** widget3d, cv::viz::Widget** widget)
{
   cv::viz::Color c = cv::viz::Color(*color);
   cv::Point3d ap1 = cv::Point3d(axisPoint1->x, axisPoint1->y, axisPoint1->z);
   cv::Point3d ap2 = cv::Point3d(axisPoint2->x, axisPoint2->y, axisPoint2->z);
   cv::viz::WCylinder* cylinder = new cv::viz::WCylinder(ap1, ap2, radius, numsides, c);
   *widget3d = dynamic_cast<cv::viz::Widget3D*>(cylinder);
   *widget = dynamic_cast<cv::viz::Widget*>(cylinder);
   return cylinder;
}

void cveWCylinderRelease(cv::viz::WCylinder** cylinder)
{
   delete *cylinder;
   *cylinder = 0;
}

cv::viz::WCircle* cveWCircleCreateAtOrigin(double radius, double thickness, CvScalar* color, cv::viz::Widget3D** widget3d, cv::viz::Widget** widget)
{
   cv::viz::Color c = cv::viz::Color(*color);
   cv::viz::WCircle* circle = new cv::viz::WCircle(radius, thickness, c);
   *widget3d = dynamic_cast<cv::viz::Widget3D*>(circle);
   *widget = dynamic_cast<cv::viz::Widget*>(circle);
   return circle;
}
cv::viz::WCircle* cveWCircleCreate(double radius, CvPoint3D64f* center, CvPoint3D64f* normal, double thickness, CvScalar* color, cv::viz::Widget3D** widget3d, cv::viz::Widget** widget)
{
   cv::viz::Color c = cv::viz::Color(*color);
   cv::Point3d cp = cv::Point3d(center->x, center->y, center->z);
   cv::Point3d n = cv::Point3d(normal->x, normal->y, normal->z);
   cv::viz::WCircle* circle = new cv::viz::WCircle(radius, cp, n, thickness, c);
   *widget3d = dynamic_cast<cv::viz::Widget3D*>(circle);
   *widget = dynamic_cast<cv::viz::Widget*>(circle);
   return circle;
}
void cveWCircleRelease(cv::viz::WCircle** circle)
{
   delete *circle;
   *circle = 0;
}

cv::viz::WCone* cveWConeCreateAtOrigin(double length, double radius, int resolution, CvScalar* color, cv::viz::Widget3D** widget3d, cv::viz::Widget** widget)
{
   cv::viz::Color c = cv::viz::Color(*color);
   cv::viz::WCone* cone = new cv::viz::WCone(length, radius, resolution, c);
   *widget3d = dynamic_cast<cv::viz::Widget3D*>(cone);
   *widget = dynamic_cast<cv::viz::Widget*>(cone);
   return cone;
}
cv::viz::WCone* cveWConeCreate(double radius, CvPoint3D64f* center, CvPoint3D64f* tip, int resolution, CvScalar* color, cv::viz::Widget3D** widget3d, cv::viz::Widget** widget)
{
   cv::viz::Color c = cv::viz::Color(*color);
   cv::Point3d cp = cv::Point3d(center->x, center->y, center->z);
   cv::Point3d tp = cv::Point3d(tip->x, tip->y, tip->z);
   cv::viz::WCone* cone = new cv::viz::WCone( radius, cp, tp,  resolution, c);
   *widget3d = dynamic_cast<cv::viz::Widget3D*>(cone);
   *widget = dynamic_cast<cv::viz::Widget*>(cone);
   return cone;
}
void cveWConeRelease(cv::viz::WCone** cone)
{
   delete *cone;
   *cone = 0;
}

cv::viz::WArrow* cveWArrowCreate(CvPoint3D64f* pt1, CvPoint3D64f* pt2, double thickness, CvScalar* color, cv::viz::Widget3D** widget3d, cv::viz::Widget** widget)
{
   cv::viz::Color c = cv::viz::Color(*color);
   cv::Point3d p1 = cv::Point3d(pt1->x, pt1->y, pt1->z);
   cv::Point3d p2 = cv::Point3d(pt2->x, pt2->y, pt2->z);
   cv::viz::WArrow* arrow = new cv::viz::WArrow(p1, p2, thickness, c);
   *widget3d = dynamic_cast<cv::viz::Widget3D*>(arrow);
   *widget = dynamic_cast<cv::viz::Widget*>(arrow);
   return arrow;
}
void cveWArrowRelease(cv::viz::WArrow** arrow)
{
   delete *arrow;
   *arrow = 0;
}