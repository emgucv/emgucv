//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2017 by EMGU Corporation. All rights reserved.
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



CVAPI(cv::Stitcher*) cveStitcherCreateDefault(bool tryUseGpu);
CVAPI(cv::Stitcher*) cveStitcherCreate(int mode, bool tryUseGpu);

CVAPI(void) cveStitcherRelease(cv::Stitcher** stitcher);

CVAPI(void) cveStitcherSetFeaturesFinder(cv::Stitcher* stitcher, cv::detail::FeaturesFinder* finder);

CVAPI(void) cveStitcherSetWarper(cv::Stitcher* stitcher, cv::WarperCreator* creator);

CVAPI(void) cveStitcherSetWaveCorrection(cv::Stitcher* stitcher, bool flag);
CVAPI(bool) cveStitcherGetWaveCorrection(cv::Stitcher* stitcher);
CVAPI(void) cveStitcherSetWaveCorrectionKind(cv::Stitcher* stitcher, int kind);
CVAPI(int) cveStitcherGetWaveCorrectionKind(cv::Stitcher* stitcher);
CVAPI(void) cveStitcherSetPanoConfidenceThresh(cv::Stitcher* stitcher, double confThresh);
CVAPI(double) cveStitcherGetPanoConfidenceThresh(cv::Stitcher* stitcher);
CVAPI(void) cveStitcherSetCompositingResol(cv::Stitcher* stitcher, double resolMpx);
CVAPI(double) cveStitcherGetCompositingResol(cv::Stitcher* stitcher);
CVAPI(void) cveStitcherSetSeamEstimationResol(cv::Stitcher* stitcher, double resolMpx);
CVAPI(double) cveStitcherGetSeamEstimationResol(cv::Stitcher* stitcher);
CVAPI(void) cveStitcherSetRegistrationResol(cv::Stitcher* stitcher, double resolMpx);
CVAPI(double) cveStitcherGetRegistrationResol(cv::Stitcher* stitcher);

CVAPI(int) cveStitcherStitch(cv::Stitcher* stitcher, cv::_InputArray* images, cv::_OutputArray* pano);

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

CVAPI(cv::detail::AKAZEFeaturesFinder*) cveAKAZEFeaturesFinderCreate(
	int descriptorType,
	int descriptorSize,
	int descriptorChannels,
	float threshold,
	int nOctaves,
	int nOctaveLayers,
	int diffusivity, 
	cv::detail::FeaturesFinder** f);
CVAPI(void) cveAKAZEFeaturesFinderRelease(cv::detail::AKAZEFeaturesFinder** finder);


CVAPI(void) cveRotationWarperBuildMaps(cv::detail::RotationWarper* warper, CvSize* srcSize, cv::_InputArray* K, cv::_InputArray* R, cv::_OutputArray* xmap, cv::_OutputArray* ymap, CvRect* boundingBox);
CVAPI(void) cveRotationWarperWarp(cv::detail::RotationWarper* warper, cv::_InputArray* src, cv::_InputArray* K, cv::_InputArray* R, int interpMode, int borderMode, cv::_OutputArray* dst, CvPoint* corner);

CVAPI(cv::detail::PlaneWarper*) cvePlaneWarperCreate(float scale, cv::WarperCreator** creator, cv::detail::RotationWarper** rotationWarper);
CVAPI(void) cvePlaneWarperRelease(cv::detail::PlaneWarper** warper);

CVAPI(cv::detail::CylindricalWarper*) cveCylindricalWarperCreate(float scale, cv::WarperCreator** creator, cv::detail::RotationWarper** rotationWarper);
CVAPI(void) cveCylindricalWarperRelease(cv::detail::CylindricalWarper** warper);

CVAPI(cv::detail::SphericalWarper*) cveSphericalWarperCreate(float scale, cv::WarperCreator** creator, cv::detail::RotationWarper** rotationWarper);
CVAPI(void) cveSphericalWarperRelease(cv::detail::SphericalWarper** warper);

CVAPI(cv::detail::FisheyeWarper*) cveFisheyeWarperCreate(float scale, cv::WarperCreator** creator, cv::detail::RotationWarper** rotationWarper);
CVAPI(void) cveFisheyeWarperRelease(cv::detail::FisheyeWarper** warper);

CVAPI(cv::detail::StereographicWarper*) cveStereographicWarperCreate(float scale, cv::WarperCreator** creator, cv::detail::RotationWarper** rotationWarper);
CVAPI(void) cveStereographicWarperRelease(cv::detail::StereographicWarper** warper);

CVAPI(cv::detail::CompressedRectilinearWarper*) cveCompressedRectilinearWarperCreate(float scale, cv::WarperCreator** creator, cv::detail::RotationWarper** rotationWarper);
CVAPI(void) cveCompressedRectilinearWarperRelease(cv::detail::CompressedRectilinearWarper** warper);

CVAPI(cv::detail::PaniniWarper*) cvePaniniWarperCreate(float scale, cv::WarperCreator** creator, cv::detail::RotationWarper** rotationWarper);
CVAPI(void) cvePaniniWarperRelease(cv::detail::PaniniWarper** warper);

CVAPI(cv::detail::PaniniPortraitWarper*) cvePaniniPortraitWarperCreate(float scale, cv::WarperCreator** creator, cv::detail::RotationWarper** rotationWarper);
CVAPI(void) cvePaniniPortraitWarperRelease(cv::detail::PaniniPortraitWarper** warper);

CVAPI(cv::detail::MercatorWarper*) cveMercatorWarperCreate(float scale, cv::WarperCreator** creator, cv::detail::RotationWarper** rotationWarper);
CVAPI(void) cveMercatorWarperRelease(cv::detail::MercatorWarper** warper);

CVAPI(cv::detail::TransverseMercatorWarper*) cveTransverseMercatorWarperCreate(float scale, cv::WarperCreator** creator, cv::detail::RotationWarper** rotationWarper);
CVAPI(void) cveTransverseMercatorWarperRelease(cv::detail::TransverseMercatorWarper** warper);

#ifdef HAVE_OPENCV_CUDAWARPING
CVAPI(cv::detail::PlaneWarperGpu*) cvePlaneWarperGpuCreate(float scale, cv::WarperCreator** creator);
CVAPI(void) cvePlaneWarperGpuRelease(cv::detail::PlaneWarperGpu** warper);

CVAPI(cv::detail::CylindricalWarperGpu*) cveCylindricalWarperGpuCreate(float scale, cv::WarperCreator** creator);
CVAPI(void) cveCylindricalWarperGpuRelease(cv::detail::CylindricalWarperGpu** warper);

CVAPI(cv::detail::SphericalWarperGpu*) cveSphericalWarperGpuCreate(float scale, cv::WarperCreator** creator);
CVAPI(void) cveSphericalWarperGpuRelease(cv::detail::SphericalWarperGpu** warper);
#endif

#endif
