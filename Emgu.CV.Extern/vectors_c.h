//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2011 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_VECTORS_C_H
#define EMGU_VECTORS_C_H

#include <vector>
#include "opencv2/core/core_c.h"
#include "opencv2/features2d/features2d.hpp"

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

CVAPI(void) VectorOfDMatchToMat(std::vector<cv::DMatch>* matches, CvMat* trainIdx, CvMat* distance);

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

CVAPI(cv::KeyPoint*) VectorOfKeyPointGetStartAddress(std::vector<cv::KeyPoint>* v);

#endif