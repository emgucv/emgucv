//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.
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
//  Vector of DMatch
//
//----------------------------------------------------------------------------

CVAPI(void) VectorOfDMatchPushMatrix(std::vector<cv::DMatch>* matches, const CvMat* trainIdx, const CvMat* distance = 0, const CvMat* mask = 0);

CVAPI(void) VectorOfDMatchToMat(std::vector< std::vector<cv::DMatch> >* matches, CvMat* trainIdx, CvMat* distance);

//----------------------------------------------------------------------------
//
//  Vector of KeyPoint
//
//----------------------------------------------------------------------------
CVAPI(void) VectorOfKeyPointFilterByImageBorder( std::vector<cv::KeyPoint>* keypoints, CvSize imageSize, int borderSize );

CVAPI(void) VectorOfKeyPointFilterByKeypointSize( std::vector<cv::KeyPoint>* keypoints, float minSize, float maxSize);

CVAPI(void) VectorOfKeyPointFilterByPixelsMask( std::vector<cv::KeyPoint>* keypoints, CvMat* mask );

/*
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

CVAPI(void) VectorOfDataMatrixCodeRelease(std::vector<CvDataMatrixCode>** v);

CVAPI(CvDataMatrixCode*) VectorOfDataMatrixCodeGetStartAddress(std::vector<CvDataMatrixCode>* v);

CVAPI(CvDataMatrixCode*) VectorOfDataMatrixCodeGetItem(std::vector<CvDataMatrixCode>* v, int index);

CVAPI(void) VectorOfDataMatrixCodeFind(std::vector<CvDataMatrixCode>* v, IplImage* image);

CVAPI(void) VectorOfDataMatrixCodeDraw(std::vector<CvDataMatrixCode>* v, IplImage* image);
*/
#endif