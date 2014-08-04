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

void CvStitcherSetFeaturesFinder(cv::StitcherWrapper* stitcherWrapper, cv::detail::FeaturesFinder* finder)
{
   cv::Ptr<cv::detail::FeaturesFinder> p(finder);
   p.addref();
   stitcherWrapper->stitcher.setFeaturesFinder(p);
}

bool CvStitcherStitch(cv::StitcherWrapper* stitcherWrapper, cv::_InputArray* images, cv::_OutputArray* pano)
{
   return stitcherWrapper->stitcher.stitch(*images, *pano) == cv::Stitcher::OK;
}

#ifdef HAVE_OPENCV_NONFREE
cv::detail::SurfFeaturesFinder* cveSurfFeaturesFinderCreate(
   double hess_thresh, int num_octaves, int num_layers,
   int num_octaves_descr, int num_layers_descr, cv::detail::FeaturesFinder** f)
{
   cv::detail::SurfFeaturesFinder* finder = new cv::detail::SurfFeaturesFinder(hess_thresh, num_octaves, num_layers, num_octaves_descr, num_layers_descr);
   *f = (cv::detail::FeaturesFinder*)(finder);
   return finder;
}

void cveSurfFeaturesFinderRelease(cv::detail::SurfFeaturesFinder** finder)
{
   delete *finder;
   *finder = 0;
}

cv::detail::SurfFeaturesFinderGpu* cveSurfFeaturesFinderGpuCreate(
   double hess_thresh, int num_octaves, int num_layers,
   int num_octaves_descr, int num_layers_descr, cv::detail::FeaturesFinder** f)
{
   cv::detail::SurfFeaturesFinderGpu* finder = new cv::detail::SurfFeaturesFinderGpu(hess_thresh, num_octaves, num_layers, num_octaves_descr, num_layers_descr);
   *f = (cv::detail::FeaturesFinder*)(finder);
   return finder;
}

void cveSurfFeaturesFinderGpuRelease(cv::detail::SurfFeaturesFinderGpu** finder)
{
   delete *finder;
   *finder = 0;
}
#endif

cv::detail::OrbFeaturesFinder* cveOrbFeaturesFinderCreate(CvSize* grid_size, int nfeaturea, float scaleFactor, int nlevels, cv::detail::FeaturesFinder** f)
{
   cv::detail::OrbFeaturesFinder* finder = new cv::detail::OrbFeaturesFinder(*grid_size, nfeaturea, scaleFactor, nlevels);
   *f = (cv::detail::FeaturesFinder*)(finder);
   return finder;
}
void cveOrbFeaturesFinderRelease(cv::detail::OrbFeaturesFinder** finder)
{
   delete *finder;
   *finder = 0;
}
