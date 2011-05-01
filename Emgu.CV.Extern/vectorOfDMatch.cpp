//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2011 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#include "vectorOfDMatch.h"

CVAPI(std::vector<cv::DMatch>*) VectorOfDMatchCreate() 
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

CVAPI(std::vector<cv::DMatch>*) VectorOfDMatchCreateSize(int size) 
{ 
   return new std::vector<cv::DMatch>(size); 
}

CVAPI(int) VectorOfDMatchGetSize(std::vector<cv::DMatch>* v)
{
   return v->size();
}

CVAPI(void) VectorOfDMatchPushMulti(std::vector<cv::DMatch>* v, cv::DMatch* values, int count)
{
   for(int i=0; i < count; i++) v->push_back(*values++);
}

CVAPI(void) VectorOfDMatchClear(std::vector<cv::DMatch>* v)
{
   v->clear();
}

CVAPI(void) VectorOfDMatchRelease(std::vector<cv::DMatch>* v)
{
   delete v;
}

CVAPI(void) VectorOfDMatchCopyData(std::vector<cv::DMatch>* v, cv::DMatch* data)
{
   memcpy(data, &(*v)[0], v->size() * sizeof(cv::DMatch));
}

CVAPI(cv::DMatch*) VectorOfDMatchGetStartAddress(std::vector<cv::DMatch>* v)
{
   return v->empty() ? NULL : &(*v)[0];
}