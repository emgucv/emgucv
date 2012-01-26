//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2011 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#include "stitching_c.h"

cv::StitcherWrapper* CvStitcherCreateDefault(bool tryUseGpu)
{
   return new cv::StitcherWrapper(tryUseGpu);
}

void CvStitcherRelease(cv::StitcherWrapper** stitcherWrapper)
{
   delete *stitcherWrapper;
   *stitcherWrapper = 0;
}

IplImage* CvStitcherStitch(cv::StitcherWrapper* stitcherWrapper, IplImage** images, int imgCount)
{
   cv::Mat pano;
   std::vector<cv::Mat> imgs(imgCount);
   for (int i = 0; i < imgs.size(); i++)
   {
      imgs[i] = cv::cvarrToMat(images[i]);
   }
   if ( stitcherWrapper->stitcher.stitch(imgs, pano) != cv::Stitcher::OK)
      return 0;

   IplImage* result = cvCreateImage(pano.size(), IPL_DEPTH_8U, 3);
   IplImage tmp = (IplImage) pano;
   cvCopy(&tmp, result);
   //cv::Mat resultMat = cv::cvarrToMat(result);
   //pano.copyTo(resultMat);
   return result;
}
