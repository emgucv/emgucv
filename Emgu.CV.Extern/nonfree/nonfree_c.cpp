//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
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