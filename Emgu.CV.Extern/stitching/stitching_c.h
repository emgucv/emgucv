//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_STITCHING_C_H
#define EMGU_STITCHING_C_H

#include "opencv2/core/core_c.h"
#include "opencv2/stitching.hpp"

/*
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
}*/



CVAPI(cv::Stitcher*) CvStitcherCreateDefault(bool tryUseGpu);

CVAPI(void) CvStitcherRelease(cv::Stitcher** stitcher);

CVAPI(void) CvStitcherSetFeaturesFinder(cv::Stitcher* stitcher, cv::detail::FeaturesFinder* finder);

CVAPI(int) CvStitcherStitch(cv::Stitcher* stitcher, cv::_InputArray* images, cv::_OutputArray* pano);

#ifdef HAVE_OPENCV_NONFREE
CVAPI(cv::detail::SurfFeaturesFinder*) cveSurfFeaturesFinderCreate(
   double hess_thresh, int num_octaves, int num_layers,
   int num_octaves_descr, int num_layers_descr, cv::detail::FeaturesFinder** f);

CVAPI(void) cveSurfFeaturesFinderRelease(cv::detail::SurfFeaturesFinder** finder);

CVAPI(cv::detail::SurfFeaturesFinderGpu*) cveSurfFeaturesFinderGpuCreate(
   double hess_thresh, int num_octaves, int num_layers,
   int num_octaves_descr, int num_layers_descr, cv::detail::FeaturesFinder** f); 

CVAPI(void) cveSurfFeaturesFinderGpuRelease(cv::detail::SurfFeaturesFinderGpu** finder);
#endif

CVAPI(cv::detail::OrbFeaturesFinder*) cveOrbFeaturesFinderCreate(CvSize* grid_size, int nfeaturea, float scaleFactor, int nlevels, cv::detail::FeaturesFinder** f);
CVAPI(void) cveOrbFeaturesFinderRelease(cv::detail::OrbFeaturesFinder** finder);
#endif