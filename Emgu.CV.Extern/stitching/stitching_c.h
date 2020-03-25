//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
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


//CVAPI(cv::Stitcher*) cveStitcherCreateDefault(bool tryUseGpu);

CVAPI(cv::Stitcher*) cveStitcherCreate(int mode, cv::Ptr<cv::Stitcher>** sharedPtr);

CVAPI(void) cveStitcherRelease(cv::Ptr<cv::Stitcher>** sharedPtr);

CVAPI(void) cveStitcherSetFeaturesFinder(cv::Stitcher* stitcher, cv::Feature2D* finder);

CVAPI(void) cveStitcherSetWarper(cv::Stitcher* stitcher, cv::WarperCreator* creator);

CVAPI(void) cveStitcherSetBlender(cv::Stitcher* stitcher, cv::detail::Blender* b);

CVAPI(void) cveStitcherSetExposureCompensator(cv::Stitcher* stitcher, cv::detail::ExposureCompensator* exposureComp);

CVAPI(void) cveStitcherSetBundleAdjuster(cv::Stitcher* stitcher, cv::detail::BundleAdjusterBase* bundleAdjuster);

CVAPI(void) cveStitcherSetSeamFinder(cv::Stitcher* stitcher, cv::detail::SeamFinder* seamFinder);

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

CVAPI(int) cveStitcherGetInterpolationFlags(cv::Stitcher* stitcher);
CVAPI(void) cveStitcherSetInterpolationFlags(cv::Stitcher* stitcher, int interpFlags);

CVAPI(int) cveStitcherStitch(cv::Stitcher* stitcher, cv::_InputArray* images, cv::_OutputArray* pano);

CVAPI(int) cveStitcherEstimateTransform(cv::Stitcher* stitcher, cv::_InputArray* images, cv::_InputArray* masks);
CVAPI(int) cveStitcherComposePanorama1(cv::Stitcher* stitcher, cv::_OutputArray* pano);
CVAPI(int) cveStitcherComposePanorama2(cv::Stitcher* stitcher, cv::_InputArray* images, cv::_OutputArray* pano);

/*
#ifdef OPENCV_ENABLE_NONFREE
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
*/

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


CVAPI(cv::detail::FeatherBlender*) cveFeatherBlenderCreate(float sharpness, cv::detail::Blender** blender);
CVAPI(void) cveFeatherBlenderRelease(cv::detail::FeatherBlender** blender);

CVAPI(cv::detail::MultiBandBlender*) cveMultiBandBlenderCreate(int tryGpu, int numBands, int weightType, cv::detail::Blender** blender);
CVAPI(void) cveMultiBandBlenderRelease(cv::detail::MultiBandBlender** blender);

CVAPI(cv::detail::NoExposureCompensator*) cveNoExposureCompensatorCreate(cv::detail::ExposureCompensator** exposureCompensatorPtr);
CVAPI(void) cveNoExposureCompensatorRelease(cv::detail::NoExposureCompensator** compensator);

CVAPI(cv::detail::GainCompensator*) cveGainCompensatorCreate(int nrFeeds, cv::detail::ExposureCompensator** exposureCompensatorPtr);
CVAPI(void) cveGainCompensatorRelease(cv::detail::GainCompensator** compensator);

CVAPI(cv::detail::ChannelsCompensator*) cveChannelsCompensatorCreate(int nrFeeds, cv::detail::ExposureCompensator** exposureCompensatorPtr);
CVAPI(void) cveChannelsCompensatorRelease(cv::detail::ChannelsCompensator** compensator);

//CVAPI(cv::detail::BlocksCompensator*) cveBlocksCompensatorCreate(int blWidth, int blHeight, int nrFeeds, cv::detail::ExposureCompensator** exposureCompensatorPtr);
//CVAPI(void) cveBlocksCompensatorRelease(cv::detail::BlocksCompensator** compensator);

CVAPI(cv::detail::BlocksGainCompensator*) cveBlocksGainCompensatorCreate(int blWidth, int blHeight, int nrFeeds, cv::detail::ExposureCompensator** exposureCompensatorPtr);
CVAPI(void) cveBlocksGainCompensatorRelease(cv::detail::BlocksGainCompensator** compensator);

CVAPI(cv::detail::BlocksChannelsCompensator*) cveBlocksChannelsCompensatorCreate(int blWidth, int blHeight, int nrFeeds, cv::detail::ExposureCompensator** exposureCompensatorPtr);
CVAPI(void) cveBlocksChannelsCompensatorRelease(cv::detail::BlocksChannelsCompensator** compensator);

CVAPI(cv::detail::NoBundleAdjuster*) cveNoBundleAdjusterCreate(cv::detail::BundleAdjusterBase** bundleAdjusterBasePtr);
CVAPI(void) cveNoBundleAdjusterRelease(cv::detail::NoBundleAdjuster** bundleAdjuster);

CVAPI(cv::detail::BundleAdjusterReproj*) cveBundleAdjusterReprojCreate(cv::detail::BundleAdjusterBase** bundleAdjusterBasePtr);
CVAPI(void) cveBundleAdjusterReprojRelease(cv::detail::BundleAdjusterReproj** bundleAdjuster);

CVAPI(cv::detail::BundleAdjusterRay*) cveBundleAdjusterRayCreate(cv::detail::BundleAdjusterBase** bundleAdjusterBasePtr);
CVAPI(void) cveBundleAdjusterRayRelease(cv::detail::BundleAdjusterRay** bundleAdjuster);

CVAPI(cv::detail::BundleAdjusterAffine*) cveBundleAdjusterAffineCreate(cv::detail::BundleAdjusterBase** bundleAdjusterBasePtr);
CVAPI(void) cveBundleAdjusterAffineRelease(cv::detail::BundleAdjusterAffine** bundleAdjuster);

CVAPI(cv::detail::BundleAdjusterAffinePartial*) cveBundleAdjusterAffinePartialCreate(cv::detail::BundleAdjusterBase** bundleAdjusterBasePtr);
CVAPI(void) cveBundleAdjusterAffinePartialRelease(cv::detail::BundleAdjusterAffinePartial** bundleAdjuster);


CVAPI(cv::detail::NoSeamFinder*) cveNoSeamFinderCreate(cv::detail::SeamFinder** seamFinderPtr);
CVAPI(void) cveNoSeamFinderRelease(cv::detail::NoSeamFinder** seamFinder);

//CVAPI(cv::detail::PairwiseSeamFinder*) cvePairwiseSeamFinderCreate(cv::detail::SeamFinder** seamFinderPtr);
//CVAPI(void) cvePairwiseSeamFinderRelease(cv::detail::PairwiseSeamFinder** seamFinder);

CVAPI(cv::detail::VoronoiSeamFinder*) cveVoronoiSeamFinderCreate(cv::detail::SeamFinder** seamFinderPtr);
CVAPI(void) cveVoronoiSeamFinderRelease(cv::detail::VoronoiSeamFinder** seamFinder);

CVAPI(cv::detail::DpSeamFinder*) cveDpSeamFinderCreate(int costFunc, cv::detail::SeamFinder** seamFinderPtr);
CVAPI(void) cveDpSeamFinderRelease(cv::detail::DpSeamFinder** seamFinder);

CVAPI(cv::detail::GraphCutSeamFinder*) cveGraphCutSeamFinderCreate(
	int costType, 
	float terminalCost,
	float badRegionPenalty,
	cv::detail::SeamFinder** seamFinderPtr);
CVAPI(void) cveGraphCutSeamFinderRelease(cv::detail::GraphCutSeamFinder** seamFinder);

#ifdef HAVE_OPENCV_CUDAWARPING
CVAPI(cv::detail::PlaneWarperGpu*) cvePlaneWarperGpuCreate(float scale, cv::WarperCreator** creator);
CVAPI(void) cvePlaneWarperGpuRelease(cv::detail::PlaneWarperGpu** warper);

CVAPI(cv::detail::CylindricalWarperGpu*) cveCylindricalWarperGpuCreate(float scale, cv::WarperCreator** creator);
CVAPI(void) cveCylindricalWarperGpuRelease(cv::detail::CylindricalWarperGpu** warper);

CVAPI(cv::detail::SphericalWarperGpu*) cveSphericalWarperGpuCreate(float scale, cv::WarperCreator** creator);
CVAPI(void) cveSphericalWarperGpuRelease(cv::detail::SphericalWarperGpu** warper);
#endif

#endif
