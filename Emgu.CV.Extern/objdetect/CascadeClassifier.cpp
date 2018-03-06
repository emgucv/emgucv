//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "objdetect_c.h"

cv::CascadeClassifier* cveCascadeClassifierCreate()
{
   return new cv::CascadeClassifier();
}
cv::CascadeClassifier* cveCascadeClassifierCreateFromFile(cv::String* fileName)
{
   return new cv::CascadeClassifier(*fileName);
}
bool cveCascadeClassifierRead(cv::CascadeClassifier* classifier, cv::FileNode* node)
{
   return classifier->read(*node);
}
void cveCascadeClassifierRelease(cv::CascadeClassifier** classifier)
{
   delete *classifier;
   *classifier = 0;
}
void cveCascadeClassifierDetectMultiScale( 
   cv::CascadeClassifier* classifier,
   cv::_InputArray* image,
   std::vector<cv::Rect>* objects,
   double scaleFactor,
   int minNeighbors, int flags,
   CvSize* minSize,
   CvSize* maxSize)
{
   classifier->detectMultiScale(*image, *objects, scaleFactor, minNeighbors, flags, *minSize, *maxSize);
}
bool cveCascadeClassifierIsOldFormatCascade(cv::CascadeClassifier* classifier)
{
   return classifier->isOldFormatCascade();
}

void cveCascadeClassifierGetOriginalWindowSize(cv::CascadeClassifier* classifier, CvSize* size)
{
   cv::Size s = classifier->getOriginalWindowSize();
   size->width = s.width;
   size->height = s.height;
}

void cveGroupRectangles1(std::vector< cv::Rect >* rectList, int groupThreshold, double eps)
{
	cv::groupRectangles(*rectList, groupThreshold, eps);
}
void cveGroupRectangles2(std::vector<cv::Rect>* rectList, std::vector<int>* weights, int groupThreshold, double eps)
{
	cv::groupRectangles(*rectList, *weights, groupThreshold, eps);
}
void cveGroupRectangles3(std::vector<cv::Rect>* rectList, int groupThreshold, double eps, std::vector<int>* weights, std::vector<double>* levelWeights)
{
	cv::groupRectangles(*rectList, groupThreshold, eps, weights, levelWeights);
}
void cveGroupRectangles4(std::vector<cv::Rect>* rectList, std::vector<int>* rejectLevels, std::vector<double>* levelWeights, int groupThreshold, double eps)
{
	cv::groupRectangles(*rectList, *rejectLevels, *levelWeights, groupThreshold, eps);
}
void cveGroupRectanglesMeanshift(std::vector<cv::Rect>* rectList, std::vector<double>* foundWeights, std::vector<double>* foundScales, double detectThreshold, CvSize* winDetSize)
{
	cv::groupRectangles_meanshift(*rectList, *foundWeights, *foundScales, detectThreshold, *winDetSize);
}
