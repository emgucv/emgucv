//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#include "objdetect_c.h"

//#include "stdio.h"
void CvHOGDescriptorPeopleDetectorCreate(CvSeq* seq) 
{   
   std::vector<float> v = cv::HOGDescriptor::getDefaultPeopleDetector();  
   cvSeqPushMulti(seq, &v.front(), v.size()); 
}
cv::HOGDescriptor* CvHOGDescriptorCreateDefault() { return new cv::HOGDescriptor; }

cv::HOGDescriptor* CvHOGDescriptorCreate(
   CvSize* _winSize, 
   CvSize* _blockSize, 
   CvSize* _blockStride,
   CvSize* _cellSize, 
   int _nbins, 
   int _derivAperture, 
   double _winSigma,
   int _histogramNormType, 
   double _L2HysThreshold, 
   bool _gammaCorrection)
{
   return new cv::HOGDescriptor(*_winSize, *_blockSize, *_blockStride, *_cellSize, _nbins, _derivAperture, _winSigma, _histogramNormType, _L2HysThreshold, _gammaCorrection);
}

void CvHOGSetSVMDetector(cv::HOGDescriptor* descriptor, std::vector<float>* vector) 
{ 
   descriptor->setSVMDetector(*vector); 
}

void CvHOGDescriptorRelease(cv::HOGDescriptor* descriptor) { delete descriptor; }


void CvHOGDescriptorDetectMultiScale(
   cv::HOGDescriptor* descriptor, 
   CvArr* img, 
   CvSeq* foundLocations,
   double hitThreshold, 
   CvSize* winStride,
   CvSize* padding, 
   double scale,
   double finalThreshold, 
   bool useMeanshiftGrouping)
{
   cvClearSeq(foundLocations);

   std::vector<cv::Rect> rects;
   std::vector<double> weights;
   cv::Mat mat = cv::cvarrToMat(img);
   descriptor->detectMultiScale(mat, rects, weights, hitThreshold, *winStride, *padding, scale, finalThreshold, useMeanshiftGrouping );
   //CV_Assert(rects.size() == weights.size());

   //char message[2000];
   //sprintf(message, "rect.size() = %d; weights.size() = %d", rects.size(), weights.size());
   //CV_Error(rects.size() == weights.size(), message);

   for (unsigned int i = 0; i < rects.size(); ++i)
   {
      CvObjectDetection d;
      d.rect = rects[i];
      //The implementation without meanshift is not producing the right weight.
      //This is due to the group_rectangle function call do not pass the weights for recalculation.
      d.score = useMeanshiftGrouping ? (float) weights[i] : 0;  
      cvSeqPush(foundLocations, &d);
   }
}

void CvHOGDescriptorCompute(
    cv::HOGDescriptor *descriptor,
    CvArr *img, 
    std::vector<float> *descriptors,
    CvSize* winStride,
    CvSize* padding,
    CvSeq* locationSeq) 
{
    cv::Mat mat = cv::cvarrToMat(img);
    std::vector<cv::Point> location(0);
    if (locationSeq)
    {
       location.resize(locationSeq->total);
       cvSeqPopMulti(locationSeq, &location[0], locationSeq->total);
    }
    
    descriptor->compute(
       mat, 
       *descriptors,
       *winStride,
       *padding,
       location); 
}


/*
void CvHOGDescriptorDetect(
   cv::HOGDescriptor* descriptor, 
   CvArr* img, 
   CvSeq* foundLocations,
   double hitThreshold, 
   CvSize winStride,
   CvSize padding)
{
   cvClearSeq(foundLocations);

   std::vector<cv::Rect> rects;
   cv::Mat mat = cv::cvarrToMat(img);
   descriptor->detect(mat, rects, hitThreshold, winStride, padding);
   if (rects.size() > 0)
      cvSeqPushMulti(foundLocations, &rects[0], rects.size());
}*/

unsigned int CvHOGDescriptorGetDescriptorSize(cv::HOGDescriptor* descriptor)
{
   return (unsigned int) descriptor->getDescriptorSize();
}