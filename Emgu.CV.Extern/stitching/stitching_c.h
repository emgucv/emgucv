//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_STITCHING_C_H
#define EMGU_STITCHING_C_H

#include "opencv2/core/core_c.h"
#include "opencv2/stitching.hpp"

namespace cv {

   class CV_EXPORTS StitcherWrapper 
   {
   public:
      StitcherWrapper(bool tryUseGpu)
         : stitcher(Stitcher::createDefault(tryUseGpu))
      {
      }

      Stitcher stitcher;
   };
}

CVAPI(cv::StitcherWrapper*) CvStitcherCreateDefault(bool tryUseGpu);

CVAPI(void) CvStitcherRelease(cv::StitcherWrapper** stitcherWrapper);

CVAPI(bool) CvStitcherStitch(cv::StitcherWrapper* stitcherWrapper, cv::_InputArray* images, cv::_OutputArray* pano);
#endif