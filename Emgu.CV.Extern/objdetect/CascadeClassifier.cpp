//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2022 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "objdetect_c.h"

cv::CascadeClassifier* cveCascadeClassifierCreate()
{
#ifdef HAVE_OPENCV_OBJDETECT
	return new cv::CascadeClassifier();
#else 
	throw_no_objdetect();
#endif
}
cv::CascadeClassifier* cveCascadeClassifierCreateFromFile(cv::String* fileName)
{
#ifdef HAVE_OPENCV_OBJDETECT
	return new cv::CascadeClassifier(*fileName);
#else 
	throw_no_objdetect();
#endif
}
bool cveCascadeClassifierRead(cv::CascadeClassifier* classifier, cv::FileNode* node)
{
#ifdef HAVE_OPENCV_OBJDETECT
	return classifier->read(*node);
#else 
	throw_no_objdetect();
#endif
}
void cveCascadeClassifierRelease(cv::CascadeClassifier** classifier)
{
#ifdef HAVE_OPENCV_OBJDETECT
	delete* classifier;
	*classifier = 0;
#else 
	throw_no_objdetect();
#endif
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
#ifdef HAVE_OPENCV_OBJDETECT
	classifier->detectMultiScale(*image, *objects, scaleFactor, minNeighbors, flags, *minSize, *maxSize);
#else 
	throw_no_objdetect();
#endif
}
bool cveCascadeClassifierIsOldFormatCascade(cv::CascadeClassifier* classifier)
{
#ifdef HAVE_OPENCV_OBJDETECT
	return classifier->isOldFormatCascade();
#else 
	throw_no_objdetect();
#endif
}

void cveCascadeClassifierGetOriginalWindowSize(cv::CascadeClassifier* classifier, CvSize* size)
{
#ifdef HAVE_OPENCV_OBJDETECT
	cv::Size s = classifier->getOriginalWindowSize();
	size->width = s.width;
	size->height = s.height;
#else 
	throw_no_objdetect();
#endif
}

void cveGroupRectangles1(std::vector< cv::Rect >* rectList, int groupThreshold, double eps)
{
#ifdef HAVE_OPENCV_OBJDETECT
	cv::groupRectangles(*rectList, groupThreshold, eps);
#else 
	throw_no_objdetect();
#endif
}
void cveGroupRectangles2(std::vector<cv::Rect>* rectList, std::vector<int>* weights, int groupThreshold, double eps)
{
#ifdef HAVE_OPENCV_OBJDETECT
	cv::groupRectangles(*rectList, *weights, groupThreshold, eps);
#else 
	throw_no_objdetect();
#endif
}
void cveGroupRectangles3(std::vector<cv::Rect>* rectList, int groupThreshold, double eps, std::vector<int>* weights, std::vector<double>* levelWeights)
{
#ifdef HAVE_OPENCV_OBJDETECT
	cv::groupRectangles(*rectList, groupThreshold, eps, weights, levelWeights);
#else 
	throw_no_objdetect();
#endif
}
void cveGroupRectangles4(std::vector<cv::Rect>* rectList, std::vector<int>* rejectLevels, std::vector<double>* levelWeights, int groupThreshold, double eps)
{
#ifdef HAVE_OPENCV_OBJDETECT
	cv::groupRectangles(*rectList, *rejectLevels, *levelWeights, groupThreshold, eps);
#else 
	throw_no_objdetect();
#endif
}
void cveGroupRectanglesMeanshift(std::vector<cv::Rect>* rectList, std::vector<double>* foundWeights, std::vector<double>* foundScales, double detectThreshold, CvSize* winDetSize)
{
#ifdef HAVE_OPENCV_OBJDETECT
	cv::groupRectangles_meanshift(*rectList, *foundWeights, *foundScales, detectThreshold, *winDetSize);
#else 
	throw_no_objdetect();
#endif
}
