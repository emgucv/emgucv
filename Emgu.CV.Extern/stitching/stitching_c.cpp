//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2014 by EMGU Corporation. All rights reserved.
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

bool CvStitcherStitch(cv::StitcherWrapper* stitcherWrapper, cv::_InputArray* images, cv::_OutputArray* pano)
{
   return stitcherWrapper->stitcher.stitch(*images, *pano) == cv::Stitcher::OK;
}
