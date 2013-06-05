//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#include "nonfree_c.h"

//SIFTDetector
cv::SIFT* CvSIFTDetectorCreate(
   int nFeatures, int nOctaveLayers, 
   double contrastThreshold, double edgeThreshold, 
   double sigma, cv::FeatureDetector** featureDetector, cv::DescriptorExtractor** descriptorExtractor)
{
   cv::SIFT* sift = new cv::SIFT(nFeatures, nOctaveLayers, contrastThreshold, edgeThreshold, sigma);
   *featureDetector = static_cast<cv::FeatureDetector*>(sift);
   *descriptorExtractor = static_cast<cv::DescriptorExtractor*>(sift);
   return sift;
}

void CvSIFTDetectorRelease(cv::SIFT** detector)
{
   delete *detector;
   *detector = 0;
}

/*
int CvSIFTDetectorGetDescriptorSize(cv::SIFT* detector)
{
   return detector->descriptorSize();
}

void CvSIFTDetectorDetectFeature(cv::SIFT* detector, IplImage* image, IplImage* mask, std::vector<cv::KeyPoint>* keypoints, std::vector<float>* descriptors)
{
   cv::Mat mat = cv::cvarrToMat(image);
   cv::Mat maskMat;
   if (mask) maskMat = cv::cvarrToMat(mask);
   cv::Mat descriptorsMat;
   (*detector)(mat, maskMat, *keypoints, descriptorsMat, false);

   descriptors->resize(keypoints->size()*detector->descriptorSize());

   if (keypoints->size() > 0)
      memcpy(&(*descriptors)[0], descriptorsMat.ptr<float>(), sizeof(float)* descriptorsMat.rows * descriptorsMat.cols);
}

void CvSIFTDetectorComputeDescriptors(cv::SIFT* detector, IplImage* image, std::vector<cv::KeyPoint>* keypoints, CvMat* descriptors)
{
   if (keypoints->size() <= 0) return;
   cv::Mat mat = cv::cvarrToMat(image);
   cv::Mat maskMat;

   cv::Mat descriptorsMat = cv::cvarrToMat(descriptors);

   if (mat.channels() == 1)
      (*detector)(mat, maskMat, *keypoints, descriptorsMat, true);
   else //opponent color
   {
      cv::Ptr<cv::DescriptorExtractor> siftExtractor(detector);
      siftExtractor.addref(); //add reference such that the detector will not be released when the smart pointer went out of scope.
      cv::OpponentColorDescriptorExtractor colorDetector(siftExtractor);
      colorDetector.compute(mat, *keypoints, descriptorsMat);
   }
}*/

//SURFDetector
CVAPI(cv::SURF*) CvSURFDetectorCreate(CvSURFParams* detector, cv::FeatureDetector** featureDetector, cv::DescriptorExtractor** descriptorExtractor)
{
   cv::SURF* surf = new cv::SURF(detector->hessianThreshold, detector->nOctaves, detector->nOctaveLayers, detector->extended != 0, detector->upright != 0);
   *featureDetector = static_cast<cv::FeatureDetector*>(surf);
   *descriptorExtractor = static_cast<cv::DescriptorExtractor*>(surf);
   return surf;
}

CVAPI(void) CvSURFDetectorRelease(cv::SURF** detector)
{
   delete *detector;
   *detector = 0;
}

/*
void CvSURFDetectorDetectFeature(cv::SURF* detector, IplImage* image, IplImage* mask, std::vector<cv::KeyPoint>* keypoints, std::vector<float>* descriptors)
{
   cv::Mat mat = cv::cvarrToMat(image);
   cv::Mat maskMat;
   if (mask) maskMat = cv::cvarrToMat(mask);
   (*detector)(mat, maskMat, *keypoints, *descriptors, false);
}

void CvSURFDetectorComputeDescriptors(cv::SURF* detector, IplImage* image, std::vector<cv::KeyPoint>* keypoints, CvMat* descriptors)
{
   if (keypoints->size() <= 0) return;

   cv::Mat img = cv::cvarrToMat(image);
   cv::Mat maskMat;

   if (img.channels() == 1)
   {
      //std::vector<float> desc;
      cv::Mat descriptorsMat = cv::cvarrToMat(descriptors);
      (*detector)(img, maskMat, *keypoints, descriptorsMat, true);
      //CV_Assert(desc.size() == descriptors->width * descriptors->height);
      //memcpy(descriptors->data.ptr, &desc[0], desc.size() * sizeof(float));
   } else //opponent SURF
   {
      cv::Mat descriptorsMat = cv::cvarrToMat(descriptors);
      cv::Ptr<cv::DescriptorExtractor> surfExtractor(detector);
      surfExtractor.addref();
      cv::OpponentColorDescriptorExtractor colorDetector(surfExtractor);
      colorDetector.compute(img, *keypoints, descriptorsMat);
   }
}

//----------------------------------------------------------------------------
//
//  VIBE GPU
//
//----------------------------------------------------------------------------
cv::gpu::VIBE_GPU* gpuVibeCreate(unsigned long rngSeed, cv::gpu::GpuMat* firstFrame, cv::gpu::Stream* stream)
{
   cv::gpu::VIBE_GPU* vibe = new cv::gpu::VIBE_GPU(rngSeed);
   vibe->initialize(*firstFrame, stream ? *stream : cv::gpu::Stream::Null());
   return vibe;
}
void gpuVibeCompute(cv::gpu::VIBE_GPU* vibe, cv::gpu::GpuMat* frame, cv::gpu::GpuMat* fgMask, cv::gpu::Stream* stream)
{
   (*vibe)(*frame, *fgMask, stream ? *stream : cv::gpu::Stream::Null());
}
void gpuVibeRelease(cv::gpu::VIBE_GPU** vibe)
{
   (*vibe)->release();
   delete *vibe;
   *vibe = 0;
}*/