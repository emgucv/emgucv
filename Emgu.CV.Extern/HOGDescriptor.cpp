#include "cvaux.h"
#include "vectorOfFloat.h"

CVAPI(void) CvHOGDescriptorPeopleDetectorCreate(CvSeq* seq) 
{   
   std::vector<float> v = cv::HOGDescriptor::getDefaultPeopleDetector();  
   cvSeqPushMulti(seq, &v.front(), v.size()); 
}
CVAPI(cv::HOGDescriptor*) CvHOGDescriptorCreateDefault() { return new cv::HOGDescriptor; }

CVAPI(cv::HOGDescriptor*) CvHOGDescriptorCreate(
   cv::Size* _winSize, 
   cv::Size* _blockSize, 
   cv::Size* _blockStride,
   cv::Size* _cellSize, 
   int _nbins, 
   int _derivAperture, 
   double _winSigma,
   int _histogramNormType, 
   double _L2HysThreshold, 
   bool _gammaCorrection)
{
   return new cv::HOGDescriptor(*_winSize, *_blockSize, *_blockStride, *_cellSize, _nbins, _derivAperture, _winSigma, _histogramNormType, _L2HysThreshold, _gammaCorrection);
}
CVAPI(void) CvHOGSetSVMDetector(cv::HOGDescriptor* descriptor, vectorOfFloat* vector) 
{ 
   descriptor->setSVMDetector(vector->data); 
}

CVAPI(void) CvHOGDescriptorRelease(cv::HOGDescriptor* descriptor) { delete descriptor; }

CVAPI(void) CvHOGDescriptorDetectMultiScale(
   cv::HOGDescriptor* descriptor, 
   CvArr* img, 
   CvSeq* foundLocations,
   double hitThreshold, 
   CvSize winStride,
   CvSize padding, 
   double scale,
   int groupThreshold)
{
   cvClearSeq(foundLocations);

   std::vector<cv::Rect> rects;
   cv::Mat mat = cv::cvarrToMat(img);
   descriptor->detectMultiScale(mat, rects, hitThreshold, winStride, padding, scale, groupThreshold);
   if (rects.size() > 0)
      cvSeqPushMulti(foundLocations, &rects[0], rects.size());
}

/*
CVAPI(void) cvHOGDescriptorDetect(
   cv::HOGDescriptor* descriptor, 
   CvArr* img, 
   CvSeq* foundLocations,
   double hitThreshold, 
   CvSize winStride,
   CvSize padding)
{
   cvClearSeq(foundLocations);

   std::vector<cv::Point> hits;
   cv::Mat mat = cv::cvarrToMat(img);
   descriptor->detect(mat, hits, hitThreshold, winStride, padding);
   cvSeqPushMulti(foundLocations, &hits.front(), hits.size());
}*/
