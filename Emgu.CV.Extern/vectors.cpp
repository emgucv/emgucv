//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2011 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#include "vectorOfByte.h"
#include "vectorOfFloat.h"
#include "vectorOfDMatch.h"
#include "vectorOfKeyPoint.h"

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
   for(int i=0; i < count; i++) v->push_back(*values++);
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
   memcpy(data, &(*v)[0], v->size() * sizeof(unsigned char));
}

unsigned char* VectorOfByteGetStartAddress(std::vector<unsigned char>* v)
{
   return v->empty() ? NULL : &(*v)[0];
}

//----------------------------------------------------------------------------
//
//  Vector of Byte
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
   for(int i=0; i < count; i++) v->push_back(*values++);
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

//implementation
void VectorOfDMatchPushMatrix(std::vector<cv::DMatch>* matches, const CvMat* trainIdx, const CvMat* distances, const CvMat* mask)
{
   cv::Mat trainIdxCPU = cv::cvarrToMat(trainIdx);
   const int* trainIdx_ptr = trainIdxCPU.ptr<int>();
   int nQuery = trainIdxCPU.cols;

   if (distances)
   {   
      cv::Mat distanceCPU = cv::cvarrToMat(distances);
      const float* distance_ptr = distanceCPU.ptr<float>();
      if (mask)
      {
         cv::Mat maskMat =  cv::cvarrToMat(mask);
         const unsigned char* mask_ptr = maskMat.ptr<unsigned char>();
         for (int queryIdx = 0; queryIdx < nQuery; ++queryIdx, ++trainIdx_ptr, ++distance_ptr, ++mask_ptr)
         {
            if (*mask_ptr)
            {
               int trainIdx = *trainIdx_ptr;
               if (trainIdx == -1)
                  continue;
               float distance = *distance_ptr;
               cv::DMatch m(queryIdx, trainIdx, 0, distance);
               matches->push_back(m);
            }
         }
      } else
      {
         for (int queryIdx = 0; queryIdx < nQuery; ++queryIdx, ++trainIdx_ptr, ++distance_ptr)
         {
            int trainIdx = *trainIdx_ptr;
            if (trainIdx == -1)
               continue;
            float distance = *distance_ptr;
            cv::DMatch m(queryIdx, trainIdx, 0, distance);
            matches->push_back(m);
         }
      }
   } else
   {
      if (mask)
      {
         cv::Mat maskMat =  cv::cvarrToMat(mask);
         const unsigned char* mask_ptr = maskMat.ptr<unsigned char>();
         for (int queryIdx = 0; queryIdx < nQuery; ++queryIdx, ++trainIdx_ptr, ++mask_ptr)
         {
            if (*mask_ptr)
            {
               int trainIdx = *trainIdx_ptr;
               if (trainIdx == -1)
                  continue;
               cv::DMatch m(queryIdx, trainIdx, 0, 0);
               matches->push_back(m);
            }
         }
      } else
      {
         for (int queryIdx = 0; queryIdx < nQuery; ++queryIdx, ++trainIdx_ptr)
         {
            int trainIdx = *trainIdx_ptr;
            if (trainIdx == -1)
               continue;
            cv::DMatch m(queryIdx, trainIdx, 0, 0);
            matches->push_back(m);
         }
      }
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
   for(int i=0; i < count; i++) v->push_back(*values++);
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
   memcpy(data, &(*v)[0], v->size() * sizeof(cv::DMatch));
}

cv::DMatch* VectorOfDMatchGetStartAddress(std::vector<cv::DMatch>* v)
{
   return v->empty() ? NULL : &(*v)[0];
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
   for(int i=0; i < count; i++) v->push_back(*values++);
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
   memcpy(data, &(*v)[0], v->size() * sizeof(cv::KeyPoint));
}

cv::KeyPoint* VectorOfKeyPointGetStartAddress(std::vector<cv::KeyPoint>* v)
{
   return v->empty() ? NULL : &(*v)[0];
}