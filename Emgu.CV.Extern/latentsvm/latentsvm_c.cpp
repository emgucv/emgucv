//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "latentsvm_c.h"

inline void copyStringVector(std::vector<cv::String>& vSrc, std::vector<std::string>& vDst)
{
   if (!vSrc.empty())
      for (std::vector<cv::String>::iterator it = vSrc.begin(); it != vSrc.end(); it++)
         vDst.push_back(*it);
}

inline void copyStringVector(std::vector<std::string>& vSrc, std::vector<cv::String>& vDst)
{
   if (!vSrc.empty())
      for (std::vector<std::string>::iterator it = vSrc.begin(); it != vSrc.end(); it++)
         vDst.push_back(*it);
}

//Latent svm
cv::lsvm::LSVMDetector* cveLSVMDetectorCreate(std::vector<cv::String>* filenames, std::vector<cv::String>* classNames)
{
   std::vector<std::string> fnv;
   copyStringVector(*filenames, fnv);
   std::vector<std::string> cnv;
   copyStringVector(*classNames, cnv);
   cv::Ptr<cv::lsvm::LSVMDetector> ptr = cv::lsvm::LSVMDetector::create(fnv, cnv);
   ptr.addref();
   return ptr.get();
}

int cveLSVMGetClassCount(cv::lsvm::LSVMDetector* detector)
{
   return static_cast<int>(detector->getClassCount());
}
void cveLSVMGetClassNames(cv::lsvm::LSVMDetector* detector, std::vector<cv::String>* classNames)
{
   std::vector<std::string> strs = detector->getClassNames();
   copyStringVector(strs, *classNames);
}

void cveLSVMDetectorDetect(cv::lsvm::LSVMDetector* detector, cv::Mat* image, std::vector<cv::lsvm::LSVMDetector::ObjectDetection>* objects, float overlapThreshold)
{
   detector->detect(*image, *objects, overlapThreshold);
}

void cveLSVMDetectorRelease(cv::lsvm::LSVMDetector** detector)
{
   delete *detector;
   *detector = 0;
}