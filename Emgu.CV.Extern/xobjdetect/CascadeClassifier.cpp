//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "xobjdetect_c.h"

cv::CascadeClassifier* cveCascadeClassifierCreate()
{
#ifdef HAVE_OPENCV_XOBJDETECT
	return new cv::CascadeClassifier();
#else 
	throw_no_xobjdetect();
#endif
}
cv::CascadeClassifier* cveCascadeClassifierCreateFromFile(cv::String* fileName)
{
#ifdef HAVE_OPENCV_XOBJDETECT
	return new cv::CascadeClassifier(*fileName);
#else 
	throw_no_xobjdetect();
#endif
}
bool cveCascadeClassifierRead(cv::CascadeClassifier* classifier, cv::FileNode* node)
{
#ifdef HAVE_OPENCV_XOBJDETECT
	return classifier->read(*node);
#else 
	throw_no_xobjdetect();
#endif
}
void cveCascadeClassifierRelease(cv::CascadeClassifier** classifier)
{
#ifdef HAVE_OPENCV_XOBJDETECT
	delete* classifier;
	*classifier = 0;
#else 
	throw_no_xobjdetect();
#endif
}
void cveCascadeClassifierDetectMultiScale(
	cv::CascadeClassifier* classifier,
	cv::_InputArray* image,
	std::vector<cv::Rect>* objects,
	double scaleFactor,
	int minNeighbors, int flags,
	cv::Size* minSize,
	cv::Size* maxSize)
{
#ifdef HAVE_OPENCV_XOBJDETECT
	classifier->detectMultiScale(*image, *objects, scaleFactor, minNeighbors, flags, *minSize, *maxSize);
#else 
	throw_no_xobjdetect();
#endif
}
bool cveCascadeClassifierIsOldFormatCascade(cv::CascadeClassifier* classifier)
{
#ifdef HAVE_OPENCV_XOBJDETECT
	return classifier->isOldFormatCascade();
#else 
	throw_no_xobjdetect();
#endif
}

void cveCascadeClassifierGetOriginalWindowSize(cv::CascadeClassifier* classifier, cv::Size* size)
{
#ifdef HAVE_OPENCV_XOBJDETECT
	cv::Size s = classifier->getOriginalWindowSize();
	size->width = s.width;
	size->height = s.height;
#else 
	throw_no_xobjdetect();
#endif
}

void cveGroupRectangles1(std::vector< cv::Rect >* rectList, int groupThreshold, double eps)
{
#ifdef HAVE_OPENCV_XOBJDETECT
	cv::groupRectangles(*rectList, groupThreshold, eps);
#else 
	throw_no_xobjdetect();
#endif
}
void cveGroupRectangles2(std::vector<cv::Rect>* rectList, std::vector<int>* weights, int groupThreshold, double eps)
{
#ifdef HAVE_OPENCV_XOBJDETECT
	cv::groupRectangles(*rectList, *weights, groupThreshold, eps);
#else 
	throw_no_xobjdetect();
#endif
}
void cveGroupRectangles3(std::vector<cv::Rect>* rectList, int groupThreshold, double eps, std::vector<int>* weights, std::vector<double>* levelWeights)
{
#ifdef HAVE_OPENCV_XOBJDETECT
	cv::groupRectangles(*rectList, groupThreshold, eps, weights, levelWeights);
#else 
	throw_no_xobjdetect();
#endif
}
void cveGroupRectangles4(std::vector<cv::Rect>* rectList, std::vector<int>* rejectLevels, std::vector<double>* levelWeights, int groupThreshold, double eps)
{
#ifdef HAVE_OPENCV_XOBJDETECT
	cv::groupRectangles(*rectList, *rejectLevels, *levelWeights, groupThreshold, eps);
#else 
	throw_no_xobjdetect();
#endif
}
void cveGroupRectanglesMeanshift(std::vector<cv::Rect>* rectList, std::vector<double>* foundWeights, std::vector<double>* foundScales, double detectThreshold, cv::Size* winDetSize)
{
#ifdef HAVE_OPENCV_XOBJDETECT
	cv::groupRectangles_meanshift(*rectList, *foundWeights, *foundScales, detectThreshold, *winDetSize);
#else 
	throw_no_xobjdetect();
#endif
}
