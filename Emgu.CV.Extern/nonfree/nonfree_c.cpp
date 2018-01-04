//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
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
cv::SURF* CvSURFDetectorCreate(double hessianThresh, int nOctaves, int nOctaveLayers, bool extended, bool upright, cv::FeatureDetector** featureDetector, cv::DescriptorExtractor** descriptorExtractor)
{
   cv::SURF* surf = new cv::SURF(hessianThresh, nOctaves, nOctaveLayers, extended, upright);
   *featureDetector = static_cast<cv::FeatureDetector*>(surf);
   *descriptorExtractor = static_cast<cv::DescriptorExtractor*>(surf);
   return surf;
}

void CvSURFDetectorRelease(cv::SURF** detector)
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
cv::cuda::VIBE_GPU* gpuVibeCreate(unsigned long rngSeed, cv::cuda::GpuMat* firstFrame, cv::cuda::Stream* stream)
{
   cv::cuda::VIBE_GPU* vibe = new cv::cuda::VIBE_GPU(rngSeed);
   vibe->initialize(*firstFrame, stream ? *stream : cv::cuda::Stream::Null());
   return vibe;
}
void gpuVibeCompute(cv::cuda::VIBE_GPU* vibe, cv::cuda::GpuMat* frame, cv::cuda::GpuMat* fgMask, cv::cuda::Stream* stream)
{
   (*vibe)(*frame, *fgMask, stream ? *stream : cv::cuda::Stream::Null());
}
void gpuVibeRelease(cv::cuda::VIBE_GPU** vibe)
{
   (*vibe)->release();
   delete *vibe;
   *vibe = 0;
}*/

/*
cv::ocl::SURF_OCL* oclSURFDetectorCreate(double _hessianThreshold, int _nOctaves, int _nOctaveLayers, bool _extended, float _keypointsRatio, bool _upright)
{
   return new cv::ocl::SURF_OCL(_hessianThreshold, _nOctaves, _nOctaveLayers, _extended, _keypointsRatio, _upright);
}

void oclSURFDetectorRelease(cv::ocl::SURF_OCL** detector)
{
   delete *detector;
   *detector = 0;
}

void oclSURFDetectorDetectKeyPoints(cv::ocl::SURF_OCL* detector, const cv::ocl::oclMat* img, const cv::ocl::oclMat* mask, cv::ocl::oclMat* keypoints)
{
   (*detector)(*img, mask ? *mask : cv::ocl::oclMat() , *keypoints);
}

void oclSURFDownloadKeypoints(cv::ocl::SURF_OCL* detector, const cv::ocl::oclMat* keypointsOcl, std::vector<cv::KeyPoint>* keypoints)
{
   detector->downloadKeypoints(*keypointsOcl, *keypoints);
}

void oclSURFUploadKeypoints(cv::ocl::SURF_OCL* detector, const std::vector<cv::KeyPoint>* keypoints, cv::ocl::oclMat* keypointsOcl)
{
   detector->uploadKeypoints(*keypoints, *keypointsOcl);
}

void oclSURFDetectorCompute(
   cv::ocl::SURF_OCL* detector, 
   const cv::ocl::oclMat* img, 
   const cv::ocl::oclMat* mask, 
   cv::ocl::oclMat* keypoints, 
   cv::ocl::oclMat* descriptors, 
   bool useProvidedKeypoints)
{
   (*detector)(
      *img, 
      mask? *mask : cv::ocl::oclMat(), 
      *keypoints,
      *descriptors,
      useProvidedKeypoints);
}

int oclSURFDetectorGetDescriptorSize(cv::ocl::SURF_OCL* detector)
{
   return detector->descriptorSize();
}*/