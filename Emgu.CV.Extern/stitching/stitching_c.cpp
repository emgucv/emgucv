//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "stitching_c.h"

cv::Stitcher* CvStitcherCreateDefault(bool tryUseGpu)
{
   cv::Ptr<cv::Stitcher> p = cv::createStitcher(tryUseGpu);
   p.addref();
   return p.get();
}

void CvStitcherRelease(cv::Stitcher** stitcher)
{
   delete *stitcher;
   *stitcher = 0;
}

void CvStitcherSetFeaturesFinder(cv::Stitcher* stitcher, cv::detail::FeaturesFinder* finder)
{
   cv::Ptr<cv::detail::FeaturesFinder> p(finder);
   p.addref();
   stitcher->setFeaturesFinder(p);
}

int CvStitcherStitch(cv::Stitcher* stitcher, cv::_InputArray* images, cv::_OutputArray* pano)
{
   return stitcher->stitch(*images, *pano);
}

#ifdef HAVE_OPENCV_NONFREE
cv::detail::SurfFeaturesFinder* cveSurfFeaturesFinderCreate(
   double hess_thresh, int num_octaves, int num_layers,
   int num_octaves_descr, int num_layers_descr, cv::detail::FeaturesFinder** f)
{
   cv::detail::SurfFeaturesFinder* finder = new cv::detail::SurfFeaturesFinder(hess_thresh, num_octaves, num_layers, num_octaves_descr, num_layers_descr);
   *f = dynamic_cast<cv::detail::FeaturesFinder*>(finder);
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
   *f = dynamic_cast<cv::detail::FeaturesFinder*>(finder);
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
   *f = dynamic_cast<cv::detail::FeaturesFinder*>(finder);
   return finder;
}
void cveOrbFeaturesFinderRelease(cv::detail::OrbFeaturesFinder** finder)
{
   delete *finder;
   *finder = 0;
}
