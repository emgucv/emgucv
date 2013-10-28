//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_VECTORS_C_H
#define EMGU_VECTORS_C_H

#include <vector>
#include "opencv2/core/core_c.h"
#include "opencv2/features2d/features2d.hpp"
#include "opencv2/objdetect/objdetect.hpp"
#include "opencv2/objdetect/objdetect_c.h"

template <class dataType> 
void VectorPushMulti(std::vector<dataType>* v, dataType* values, int count)
{
   if (count > 0)
   {
      size_t oldSize = v->size();
      v->resize(oldSize + count);
      memcpy(&(*v)[oldSize], values, count * sizeof(dataType));
   }
}

template <class dataType> 
void VectorCopyData(std::vector<dataType>* v, dataType* data)
{
   if (!v->empty())
      memcpy(data, &(*v)[0], v->size() * sizeof(dataType));
}


//----------------------------------------------------------------------------
//
//  Vector of Byte
//
//----------------------------------------------------------------------------
CVAPI(std::vector<unsigned char>*) VectorOfByteCreate();

CVAPI(std::vector<unsigned char>*) VectorOfByteCreateSize(int size);

CVAPI(int) VectorOfByteGetSize(std::vector<unsigned char>* v);

CVAPI(void) VectorOfBytePushMulti(std::vector<unsigned char>* v, unsigned char* values, int count);

CVAPI(void) VectorOfByteClear(std::vector<unsigned char>* v);

CVAPI(void) VectorOfByteRelease(std::vector<unsigned char>* v);

CVAPI(void) VectorOfByteCopyData(std::vector<unsigned char>* v, unsigned char* data);

CVAPI(unsigned char*) VectorOfByteGetStartAddress(std::vector<unsigned char>* v);

//----------------------------------------------------------------------------
//
//  Vector of Float
//
//----------------------------------------------------------------------------
CVAPI(std::vector<float>*) VectorOfFloatCreate();

CVAPI(std::vector<float>*) VectorOfFloatCreateSize(int size);

CVAPI(int) VectorOfFloatGetSize(std::vector<float>* v);

CVAPI(void) VectorOfFloatPushMulti(std::vector<float>* v, float* values, int count);

CVAPI(void) VectorOfFloatClear(std::vector<float>* v);

CVAPI(void) VectorOfFloatRelease(std::vector<float>* v);

CVAPI(void) VectorOfFloatCopyData(std::vector<float>* v, float* data);

CVAPI(float*) VectorOfFloatGetStartAddress(std::vector<float>* v);

//----------------------------------------------------------------------------
//
//  Vector of DMatch
//
//----------------------------------------------------------------------------
CVAPI(std::vector<cv::DMatch>*) VectorOfDMatchCreate();

CVAPI(void) VectorOfDMatchPushMatrix(std::vector<cv::DMatch>* matches, const CvMat* trainIdx, const CvMat* distance = 0, const CvMat* mask = 0);

CVAPI(std::vector<cv::DMatch>*) VectorOfDMatchCreateSize(int size);

CVAPI(int) VectorOfDMatchGetSize(std::vector<cv::DMatch>* v);

CVAPI(void) VectorOfDMatchPushMulti(std::vector<cv::DMatch>* v, cv::DMatch* values, int count);

CVAPI(void) VectorOfDMatchClear(std::vector<cv::DMatch>* v);

CVAPI(void) VectorOfDMatchRelease(std::vector<cv::DMatch>* v);

CVAPI(void) VectorOfDMatchCopyData(std::vector<cv::DMatch>* v, cv::DMatch* data);

CVAPI(cv::DMatch*) VectorOfDMatchGetStartAddress(std::vector<cv::DMatch>* v);

CVAPI(void) VectorOfDMatchToMat(std::vector< std::vector<cv::DMatch> >* matches, CvMat* trainIdx, CvMat* distance);

//----------------------------------------------------------------------------
//
//  Vector of KeyPoint
//
//----------------------------------------------------------------------------
CVAPI(std::vector<cv::KeyPoint>*) VectorOfKeyPointCreate();

CVAPI(std::vector<cv::KeyPoint>*) VectorOfKeyPointCreateSize(int size);

CVAPI(int) VectorOfKeyPointGetSize(std::vector<cv::KeyPoint>* v);

CVAPI(void) VectorOfKeyPointPushMulti(std::vector<cv::KeyPoint>* v, cv::KeyPoint* values, int count);

CVAPI(void) VectorOfKeyPointClear(std::vector<cv::KeyPoint>* v);

CVAPI(void) VectorOfKeyPointRelease(std::vector<cv::KeyPoint>* v);

CVAPI(void) VectorOfKeyPointCopyData(std::vector<cv::KeyPoint>* v, cv::KeyPoint* data);

CVAPI(void) VectorOfKeyPointFilterByImageBorder( std::vector<cv::KeyPoint>* keypoints, CvSize imageSize, int borderSize );

CVAPI(void) VectorOfKeyPointFilterByKeypointSize( std::vector<cv::KeyPoint>* keypoints, float minSize, float maxSize);

CVAPI(void) VectorOfKeyPointFilterByPixelsMask( std::vector<cv::KeyPoint>* keypoints, CvMat* mask );

CVAPI(cv::KeyPoint*) VectorOfKeyPointGetStartAddress(std::vector<cv::KeyPoint>* v);

CVAPI(void) VectorOfKeyPointGetItem(std::vector<cv::KeyPoint>* keypoints, int index, cv::KeyPoint* keypoint);

//----------------------------------------------------------------------------
//
//  Vector of DataMatrixCode
//
//----------------------------------------------------------------------------
CVAPI(std::vector<CvDataMatrixCode>*) VectorOfDataMatrixCodeCreate();

CVAPI(std::vector<CvDataMatrixCode>*) VectorOfDataMatrixCodeCreateSize(int size);

CVAPI(int) VectorOfDataMatrixCodeGetSize(std::vector<CvDataMatrixCode>* v);

CVAPI(void) VectorOfDataMatrixCodePushMulti(std::vector<CvDataMatrixCode>* v, CvDataMatrixCode* values, int count);

CVAPI(void) VectorOfDataMatrixCodeClear(std::vector<CvDataMatrixCode>* v);

CVAPI(void) VectorOfDataMatrixCodeRelease(std::vector<CvDataMatrixCode>* v);

CVAPI(CvDataMatrixCode*) VectorOfDataMatrixCodeGetStartAddress(std::vector<CvDataMatrixCode>* v);

CVAPI(CvDataMatrixCode*) VectorOfDataMatrixCodeGetItem(std::vector<CvDataMatrixCode>* v, int index);

CVAPI(void) VectorOfDataMatrixCodeFind(std::vector<CvDataMatrixCode>* v, IplImage* image);

CVAPI(void) VectorOfDataMatrixCodeDraw(std::vector<CvDataMatrixCode>* v, IplImage* image);


//----------------------------------------------------------------------------
//
//  Vector of Mat
//
//----------------------------------------------------------------------------
CVAPI(std::vector<cv::Mat>*) VectorOfMatCreate();

CVAPI(int) VectorOfMatGetSize(std::vector<cv::Mat>* v);

CVAPI(void) VectorOfMatPush(std::vector<cv::Mat>* v, cv::Mat* value);

CVAPI(void) VectorOfMatClear(std::vector<cv::Mat>* v);

CVAPI(void) VectorOfMatRelease(std::vector<cv::Mat>* v);

CVAPI(cv::Mat*) VectorOfMatGetItem(std::vector<cv::Mat>* v, int index);

//----------------------------------------------------------------------------
//
//  Vector of Point
//
//----------------------------------------------------------------------------
CVAPI(std::vector<cv::Point>*) VectorOfPointCreate();

CVAPI(std::vector<cv::Point>*) VectorOfPointCreateSize(int size);

CVAPI(int) VectorOfPointGetSize(std::vector<cv::Point>* v);

CVAPI(void) VectorOfPointPushMulti(std::vector<cv::Point>* v, cv::Point* values, int count);

CVAPI(void) VectorOfPointClear(std::vector<cv::Point>* v);

CVAPI(void) VectorOfPointRelease(std::vector<cv::Point>* v);

CVAPI(void) VectorOfPointCopyData(std::vector<cv::Point>* v, cv::Point* data);

CVAPI(cv::Point*) VectorOfPointGetStartAddress(std::vector<cv::Point>* v);

CVAPI(void) VectorOfPointGetItem(std::vector<cv::Point>* points, int index, cv::Point* point);

//----------------------------------------------------------------------------
//
//  Vector of PointF
//
//----------------------------------------------------------------------------
CVAPI(std::vector<cv::Point2f>*) VectorOfPointFCreate();

CVAPI(std::vector<cv::Point2f>*) VectorOfPointFCreateSize(int size);

CVAPI(int) VectorOfPointFGetSize(std::vector<cv::Point2f>* v);

CVAPI(void) VectorOfPointFPushMulti(std::vector<cv::Point2f>* v, cv::Point2f* values, int count);

CVAPI(void) VectorOfPointFClear(std::vector<cv::Point2f>* v);

CVAPI(void) VectorOfPointFRelease(std::vector<cv::Point2f>* v);

CVAPI(void) VectorOfPointFCopyData(std::vector<cv::Point2f>* v, cv::Point2f* data);

CVAPI(cv::Point2f*) VectorOfPointFGetStartAddress(std::vector<cv::Point2f>* v);

CVAPI(void) VectorOfPointFGetItem(std::vector<cv::Point2f>* points, int index, cv::Point2f* point);

//----------------------------------------------------------------------------
//
//  Vector of Vector of Point
//
//----------------------------------------------------------------------------
CVAPI(std::vector< std::vector<cv::Point> >*) VectorOfVectorOfPointCreate();

CVAPI(int) VectorOfVectorOfPointGetSize(std::vector< std::vector<cv::Point> >* v);

CVAPI(void) VectorOfVectorOfPointClear(std::vector< std::vector<cv::Point> >* v);

CVAPI(void) VectorOfVectorOfPointRelease(std::vector< std::vector<cv::Point> >* v);

CVAPI(std::vector<cv::Point>*) VectorOfVectorOfPointGetItem(std::vector< std::vector<cv::Point> >* points, int index);

//----------------------------------------------------------------------------
//
//  Vector of CvRect
//
//----------------------------------------------------------------------------
CVAPI(std::vector<cv::Rect>*) VectorOfRectCreate();

CVAPI(std::vector<cv::Rect>*) VectorOfRectCreateSize(int size);

CVAPI(int) VectorOfRectGetSize(std::vector<cv::Rect>* v);

CVAPI(void) VectorOfRectPushMulti(std::vector<cv::Rect>* v, cv::Rect* values, int count);

CVAPI(void) VectorOfRectClear(std::vector<cv::Rect>* v);

CVAPI(void) VectorOfRectRelease(std::vector<cv::Rect>* v);

CVAPI(void) VectorOfRectCopyData(std::vector<cv::Rect>* v, cv::Rect* data);

CVAPI(cv::Rect*) VectorOfRectGetStartAddress(std::vector<cv::Rect>* v);

CVAPI(void) VectorOfRectGetItem(std::vector<cv::Rect>* points, int index, cv::Rect* point);

#endif