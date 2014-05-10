//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2014 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "nonfree_ocl_c.h"

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
   (*detector)(*img, mask ? *mask : cv::ocl::oclMat(), *keypoints);
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