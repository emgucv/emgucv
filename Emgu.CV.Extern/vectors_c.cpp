//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2011 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#include "vectors_c.h"

//----------------------------------------------------------------------------
//
//  Vector of Byte
//
//----------------------------------------------------------------------------

std::vector<unsigned char>* VectorOfByteCreate() 
{ 
   return new std::vector<unsigned char>(); 
}

std::vector<unsigned char>* VectorOfByteCreateSize(int size) 
{ 
   return new std::vector<unsigned char>(size); 
}

int VectorOfByteGetSize(std::vector<unsigned char>* v)
{
   return v->size();
}

void VectorOfBytePushMulti(std::vector<unsigned char>* v, unsigned char* values, int count)
{
   if (count > 0)
   {
      size_t oldSize = v->size();
      v->resize(oldSize + count);
      memcpy(&(*v)[oldSize], values, count * sizeof(unsigned char));
   }
}

void VectorOfByteClear(std::vector<unsigned char>* v)
{
   v->clear();
}

void VectorOfByteRelease(std::vector<unsigned char>* v)
{
   delete v;
}

void VectorOfByteCopyData(std::vector<unsigned char>* v, unsigned char* data)
{
   if (!v->empty())
      memcpy(data, &(*v)[0], v->size() * sizeof(unsigned char));
}

unsigned char* VectorOfByteGetStartAddress(std::vector<unsigned char>* v)
{
   return v->empty() ? NULL : &(*v)[0];
}

//----------------------------------------------------------------------------
//
//  Vector of Float
//
//----------------------------------------------------------------------------
std::vector<float>* VectorOfFloatCreate() 
{ 
   return new std::vector<float>(); 
}

std::vector<float>* VectorOfFloatCreateSize(int size) 
{ 
   return new std::vector<float>(size); 
}

int VectorOfFloatGetSize(std::vector<float>* v)
{
   return v->size();
}

void VectorOfFloatPushMulti(std::vector<float>* v, float* values, int count)
{
   if (count > 0)
   {
      size_t oldSize = v->size();
      v->resize(oldSize + count);
      memcpy(&(*v)[oldSize], values, count * sizeof(float));
   }
}

void VectorOfFloatClear(std::vector<float>* v)
{
   v->clear();
}

void VectorOfFloatRelease(std::vector<float>* v)
{
   delete v;
}

void VectorOfFloatCopyData(std::vector<float>* v, float* data)
{
   if (!v->empty())
      memcpy(data, &(*v)[0], v->size() * sizeof(float));
}

float* VectorOfFloatGetStartAddress(std::vector<float>* v)
{
   return v->empty() ? NULL : &(*v)[0];
}

//----------------------------------------------------------------------------
//
//  Vector of DMatch
//
//----------------------------------------------------------------------------
std::vector<cv::DMatch>* VectorOfDMatchCreate() 
{ 
   return new std::vector<cv::DMatch>(); 
}

void VectorOfDMatchPushMatrix(std::vector<cv::DMatch>* matches, const CvMat* trainIdx, const CvMat* distances, const CvMat* mask)
{
   CV_Assert( trainIdx->step == trainIdx->cols * sizeof(int));
   CV_Assert( !distances || (distances->step == distances->cols * sizeof(float)));
   CV_Assert( !mask || (mask->step == mask->cols * sizeof(unsigned char)));

   const int* trainIdx_ptr = trainIdx->data.i;
   const float* distance_ptr = distances ? distances->data.fl : 0;
   const unsigned char* mask_ptr = mask ? mask->data.ptr : 0;

   for (int queryIdx = 0; queryIdx <  trainIdx->rows; ++queryIdx, ++trainIdx_ptr)
   {
      if (*trainIdx_ptr != -1 && (mask_ptr == 0 || *mask_ptr))
      {
         cv::DMatch m(queryIdx, *trainIdx_ptr, 0, distance_ptr ? *distance_ptr : -1);
         matches->push_back(m);
      }

      if (mask_ptr) 
         ++mask_ptr;

      if (distance_ptr)
         ++distance_ptr;
   }
}

std::vector<cv::DMatch>* VectorOfDMatchCreateSize(int size) 
{ 
   return new std::vector<cv::DMatch>(size); 
}

int VectorOfDMatchGetSize(std::vector<cv::DMatch>* v)
{
   return v->size();
}

void VectorOfDMatchPushMulti(std::vector<cv::DMatch>* v, cv::DMatch* values, int count)
{
   if (count > 0)
   {
      size_t oldSize = v->size();
      v->resize(oldSize + count);
      memcpy(&(*v)[oldSize], values, count * sizeof(cv::DMatch));
   }
}

void VectorOfDMatchClear(std::vector<cv::DMatch>* v)
{
   v->clear();
}

void VectorOfDMatchRelease(std::vector<cv::DMatch>* v)
{
   delete v;
}

void VectorOfDMatchCopyData(std::vector<cv::DMatch>* v, cv::DMatch* data)
{
   if (!v->empty())
      memcpy(data, &(*v)[0], v->size() * sizeof(cv::DMatch));
}

cv::DMatch* VectorOfDMatchGetStartAddress(std::vector<cv::DMatch>* v)
{
   return v->empty() ? NULL : &(*v)[0];
}

void VectorOfDMatchToMat(std::vector< std::vector<cv::DMatch> >* matches, CvMat* trainIdx, CvMat* distance)
{
   CV_Assert(trainIdx->rows == (int) matches->size() && trainIdx->step == trainIdx->cols * sizeof(int));
   CV_Assert(distance->rows == (int) matches->size() && distance->step == distance->cols * sizeof(float));

   int k = trainIdx->cols;
   float* distance_ptr = distance->data.fl;
   int* trainIdx_ptr = trainIdx->data.i;
   for(std::vector< std::vector<cv::DMatch> >::iterator v = matches->begin(); v != matches->end(); ++v)
   {
      int idx = 0;
      if (!v->empty())
      {
         for (std::vector< cv::DMatch >::iterator m = v->begin(); m != v->end() && idx < k; ++m, ++idx)
         {
            *distance_ptr++ = m->distance;
            *trainIdx_ptr++ = m->trainIdx;
         }
      }
      for (; idx < k; ++idx)
      {
         *trainIdx_ptr++ = -1;
         *distance_ptr++ = -1;
      }
   }
}

//----------------------------------------------------------------------------
//
//  Vector of KeyPoint
//
//----------------------------------------------------------------------------
std::vector<cv::KeyPoint>* VectorOfKeyPointCreate() 
{ 
   return new std::vector<cv::KeyPoint>(); 
}

std::vector<cv::KeyPoint>* VectorOfKeyPointCreateSize(int size) 
{ 
   return new std::vector<cv::KeyPoint>(size); 
}

int VectorOfKeyPointGetSize(std::vector<cv::KeyPoint>* v)
{
   return v->size();
}

void VectorOfKeyPointPushMulti(std::vector<cv::KeyPoint>* v, cv::KeyPoint* values, int count)
{
   if (count > 0)
   {
      size_t oldSize = v->size();
      v->resize(oldSize + count);
      memcpy(&(*v)[oldSize], values, count * sizeof(cv::KeyPoint));
   }
}

void VectorOfKeyPointClear(std::vector<cv::KeyPoint>* v)
{
   v->clear();
}

void VectorOfKeyPointRelease(std::vector<cv::KeyPoint>* v)
{
   delete v;
}

void VectorOfKeyPointCopyData(std::vector<cv::KeyPoint>* v, cv::KeyPoint* data)
{
   if (!v->empty())
      memcpy(data, &(*v)[0], v->size() * sizeof(cv::KeyPoint));
}

cv::KeyPoint* VectorOfKeyPointGetStartAddress(std::vector<cv::KeyPoint>* v)
{
   return v->empty() ? NULL : &(*v)[0];
}

void VectorOfKeyPointFilterByImageBorder( std::vector<cv::KeyPoint>* keypoints, CvSize imageSize, int borderSize )
{
   cv::KeyPointsFilter::runByImageBorder(*keypoints, imageSize, borderSize);
}

void VectorOfKeyPointFilterByKeypointSize( std::vector<cv::KeyPoint>* keypoints, float minSize, float maxSize)
{
   cv::KeyPointsFilter::runByKeypointSize(*keypoints, minSize, maxSize);
}

void VectorOfKeyPointFilterByPixelsMask( std::vector<cv::KeyPoint>* keypoints, CvMat* mask )
{
   cv::Mat m = cv::cvarrToMat(mask);
   cv::KeyPointsFilter::runByPixelsMask(*keypoints, m);
}

void VectorOfKeyPointGetItem(std::vector<cv::KeyPoint>* keypoints, int index, cv::KeyPoint* keypoint)
{
   *keypoint = keypoints->at(index);
}