//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.
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
   VectorPushMulti<unsigned char>(v, values, count);
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
   VectorCopyData<unsigned char>(v, data);
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
   VectorPushMulti<float>(v, values, count);
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
   VectorCopyData<float>(v, data);
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
   VectorPushMulti<cv::DMatch>(v, values, count);
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
   VectorCopyData<cv::DMatch>(v, data);
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
   VectorPushMulti<cv::KeyPoint>(v, values, count);
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
   VectorCopyData<cv::KeyPoint>(v, data);
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


//----------------------------------------------------------------------------
//
//  Vector of DataMatrixCode
//
//----------------------------------------------------------------------------
std::vector<CvDataMatrixCode>* VectorOfDataMatrixCodeCreate()
{
   return new std::vector<CvDataMatrixCode>();
}

std::vector<CvDataMatrixCode>* VectorOfDataMatrixCodeCreateSize(int size)
{
   return new std::vector<CvDataMatrixCode>();
}

int VectorOfDataMatrixCodeGetSize(std::vector<CvDataMatrixCode>* v)
{
   return v->size();
}

void VectorOfDataMatrixCodeClear(std::vector<CvDataMatrixCode>* v)
{
   v->clear();
}

void VectorOfDataMatrixCodeRelease(std::vector<CvDataMatrixCode>* v)
{
   delete v;
}

CvDataMatrixCode* VectorOfDataMatrixCodeGetStartAddress(std::vector<CvDataMatrixCode>* v)
{
   return v->empty()? NULL : &(*v)[0];
}

CvDataMatrixCode* VectorOfDataMatrixCodeGetItem(std::vector<CvDataMatrixCode>* v, int index)
{
   return &(*v)[index];
}

/*
void VectorOfDataMatrixCodeFind(std::vector<CvDataMatrixCode>* v, IplImage* image)
{
   cv::Mat m = cv::cvarrToMat(image);
   cv::findDataMatrix(m, *v);
}

void VectorOfDataMatrixCodeDraw(std::vector<CvDataMatrixCode>* v, IplImage* image)
{
   cv::Mat m = cv::
   (image);
   cv::drawDataMatrixCodes(*v, m);
}*/

//----------------------------------------------------------------------------
//
//  Vector of Mat
//
//----------------------------------------------------------------------------
std::vector<cv::Mat>* VectorOfMatCreate()
{
   return new std::vector<cv::Mat>();
}

int VectorOfMatGetSize(std::vector<cv::Mat>* v)
{
   return v->size();
}

void VectorOfMatPush(std::vector<cv::Mat>* v, cv::Mat* value)
{
   v->push_back(*value);
}

void VectorOfMatClear(std::vector<cv::Mat>* v)
{
   v->clear();
}

void VectorOfMatRelease(std::vector<cv::Mat>* v)
{
   delete v;
}

cv::Mat* VectorOfMatGetItem(std::vector<cv::Mat>* v, int index)
{
   return &(*v)[index];
}

//----------------------------------------------------------------------------
//
//  Vector of Point
//
//----------------------------------------------------------------------------
std::vector<cv::Point>* VectorOfPointCreate() 
{ 
   CV_Assert(sizeof(cv::Point) == 2*sizeof(int));
   return new std::vector<cv::Point>(); 
}

std::vector<cv::Point>* VectorOfPointCreateSize(int size) 
{ 
   CV_Assert(sizeof(cv::Point) == 2*sizeof(int));
   return new std::vector<cv::Point>(size); 
}

int VectorOfPointGetSize(std::vector<cv::Point>* v)
{
   return v->size();
}

void VectorOfPointPushMulti(std::vector<cv::Point>* v, cv::Point* values, int count)
{
   VectorPushMulti<cv::Point>(v, values, count);
}

void VectorOfPointClear(std::vector<cv::Point>* v)
{
   v->clear();
}

void VectorOfPointRelease(std::vector<cv::Point>* v)
{
   delete v;
}

void VectorOfPointCopyData(std::vector<cv::Point>* v, cv::Point* data)
{
   VectorCopyData<cv::Point>(v, data);
}

cv::Point* VectorOfPointGetStartAddress(std::vector<cv::Point>* v)
{
   return v->empty() ? NULL : &(*v)[0];
}

void VectorOfPointGetItem(std::vector<cv::Point>* points, int index, cv::Point* point)
{
   *point = points->at(index);
}

//----------------------------------------------------------------------------
//
//  Vector of Point
//
//----------------------------------------------------------------------------
std::vector<cv::Point2f>* VectorOfPointFCreate() 
{ 
   CV_Assert(sizeof(cv::Point2f) == 2*sizeof(float));
   return new std::vector<cv::Point2f>(); 
}

std::vector<cv::Point2f>* VectorOfPointFCreateSize(int size) 
{ 
   CV_Assert(sizeof(cv::Point) == 2*sizeof(int));
   return new std::vector<cv::Point2f>(size); 
}

int VectorOfPointFGetSize(std::vector<cv::Point2f>* v)
{
   return v->size();
}

void VectorOfPointFPushMulti(std::vector<cv::Point2f>* v, cv::Point2f* values, int count)
{
   VectorPushMulti<cv::Point2f>(v, values, count);
}

void VectorOfPointFClear(std::vector<cv::Point2f>* v)
{
   v->clear();
}

void VectorOfPointFRelease(std::vector<cv::Point2f>* v)
{
   delete v;
}

void VectorOfPointFCopyData(std::vector<cv::Point2f>* v, cv::Point2f* data)
{
   VectorCopyData<cv::Point2f>(v, data);
}

cv::Point2f* VectorOfPointFGetStartAddress(std::vector<cv::Point2f>* v)
{
   return v->empty() ? NULL : &(*v)[0];
}

void VectorOfPointFGetItem(std::vector<cv::Point2f>* points, int index, cv::Point2f* point)
{
   *point = points->at(index);
}

//----------------------------------------------------------------------------
//
//  Vector of Vector of Point
//
//----------------------------------------------------------------------------
std::vector< std::vector<cv::Point> >* VectorOfVectorOfPointCreate()
{
   CV_Assert(sizeof(cv::Point) == 2*sizeof(int));
   return new std::vector< std::vector<cv::Point> >();
}

int VectorOfVectorOfPointGetSize(std::vector< std::vector<cv::Point> >* v)
{
   return v->size();
}

void VectorOfVectorOfPointClear(std::vector< std::vector<cv::Point> >* v)
{
   v->clear();
}

void VectorOfVectorOfPointRelease(std::vector< std::vector<cv::Point> >* v)
{
   delete v;
}

std::vector<cv::Point>* VectorOfVectorOfPointGetItem(std::vector< std::vector<cv::Point> >* points, int index)
{
   return &(*points)[index];
}

//----------------------------------------------------------------------------
//
//  Vector of CvRect
//
//----------------------------------------------------------------------------
std::vector<cv::Rect>* VectorOfRectCreate()
{
   CV_Assert(sizeof(cv::Rect) == 4 * sizeof(int));
   return new std::vector<cv::Rect>();
}

std::vector<cv::Rect>* VectorOfRectCreateSize(int size)
{
   CV_Assert(sizeof(cv::Rect) == 4 * sizeof(int));
   return new std::vector<cv::Rect>(size);
}

int VectorOfRectGetSize(std::vector<cv::Rect>* v)
{
   return v->size();
}

void VectorOfRectPushMulti(std::vector<cv::Rect>* v, cv::Rect* values, int count)
{
   VectorPushMulti<cv::Rect>(v, values, count);
}

void VectorOfRectClear(std::vector<cv::Rect>* v)
{
   v->clear();
}

void VectorOfRectRelease(std::vector<cv::Rect>* v)
{
   delete v;
}

void VectorOfRectCopyData(std::vector<cv::Rect>* v, cv::Rect* data)
{
   VectorCopyData<cv::Rect>(v, data);
}

cv::Rect* VectorOfRectGetStartAddress(std::vector<cv::Rect>* v)
{
   return v->empty() ? NULL : &(*v)[0];
}

void VectorOfRectGetItem(std::vector<cv::Rect>* points, int index, cv::Rect* point)
{
   *point = points->at(index);
}
