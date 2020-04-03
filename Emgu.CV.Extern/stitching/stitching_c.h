//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_STITCHING_C_H
#define EMGU_STITCHING_C_H

#include "opencv2/core/core_c.h"
#ifdef HAVE_OPENCV_STITCHING

#include "opencv2/stitching.hpp"

#ifndef HAVE_OPENCV_CUDAWARPING
static inline CV_NORETURN void throw_no_cudawarping() { CV_Error(cv::Error::StsBadFunc, "The library is compiled without CUDA Warping support"); }
namespace cv
{
	class PlaneWarperGpu {};
	class CylindricalWarperGpu {};
	class SphericalWarperGpu {};
}
#endif

#else
static inline CV_NORETURN void throw_no_stitching() { CV_Error(cv::Error::StsBadFunc, "The library is compiled without stitching support"); }

namespace cv
{
	class Stitcher {};
	class Feature2D {};
	class WarperCreator {};
	
	namespace detail
	{
		class Blender {};
		class ExposureCompensator {};
		class BundleAdjusterBase {};
		class Estimator {};
		class SeamFinder {};
		class FeaturesMatcher {};
		class RotationWarper {};
		class PlaneWarper {};
		class CylindricalWarper {};
		class SphericalWarper {};
		class FisheyeWarper{};
		class StereographicWarper {};
		class CompressedRectilinearWarper {};
		class PaniniWarper {};
		class PaniniPortraitWarper {};
		class MercatorWarper {};
		class TransverseMercatorWarper {};
		class FeatherBlender {};
		class MultiBandBlender {};
		class NoExposureCompensator {};
		class GainCompensator {};
		class ChannelsCompensator {};
		class BlocksGainCompensator {};
		class BlocksChannelsCompensator {};
		class NoBundleAdjuster {};
		class BundleAdjusterReproj {};
		class BundleAdjusterRay {};
		class BundleAdjusterAffine {};
		class BundleAdjusterAffinePartial {};
		class NoSeamFinder {};
		class VoronoiSeamFinder {};
		class DpSeamFinder {};
		class GraphCutSeamFinder {};
		class HomographyBasedEstimator {};
		class AffineBasedEstimator {};
		class BestOf2NearestMatcher {};
		class BestOf2NearestRangeMatcher {};
		class AffineBestOf2NearestMatcher {};
		class PlaneWarperGpu {};
		class CylindricalWarperGpu {};
		class SphericalWarperGpu {};
	}
}
#endif



CVAPI(cv::Stitcher*) cveStitcherCreate(int mode, cv::Ptr<cv::Stitcher>** sharedPtr);

CVAPI(void) cveStitcherRelease(cv::Ptr<cv::Stitcher>** sharedPtr);

CVAPI(void) cveStitcherSetFeaturesFinder(cv::Stitcher* stitcher, cv::Feature2D* finder);

CVAPI(void) cveStitcherSetWarper(cv::Stitcher* stitcher, cv::WarperCreator* creator);

CVAPI(void) cveStitcherSetBlender(cv::Stitcher* stitcher, cv::detail::Blender* b);

CVAPI(void) cveStitcherSetExposureCompensator(cv::Stitcher* stitcher, cv::detail::ExposureCompensator* exposureComp);

CVAPI(void) cveStitcherSetBundleAdjuster(cv::Stitcher* stitcher, cv::detail::BundleAdjusterBase* bundleAdjuster);

CVAPI(void) cveStitcherSetSeamFinder(cv::Stitcher* stitcher, cv::detail::SeamFinder* seamFinder);

CVAPI(void) cveStitcherSetEstimator(cv::Stitcher* stitcher, cv::detail::Estimator* estimator);

CVAPI(void) cveStitcherSetFeaturesMatcher(cv::Stitcher* stitcher, cv::detail::FeaturesMatcher* featuresMatcher);

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

CVAPI(cv::detail::PlaneWarper*) cveDetailPlaneWarperCreate(float scale, cv::detail::RotationWarper** rotationWarper);
CVAPI(void) cveDetailPlaneWarperRelease(cv::detail::PlaneWarper** warper);
CVAPI(cv::PlaneWarper*) cvePlaneWarperCreate(cv::WarperCreator** warperCreator);
CVAPI(void) cvePlaneWarperRelease(cv::PlaneWarper** warper);

CVAPI(cv::detail::CylindricalWarper*) cveDetailCylindricalWarperCreate(float scale, cv::detail::RotationWarper** rotationWarper);
CVAPI(void) cveDetailCylindricalWarperRelease(cv::detail::CylindricalWarper** warper);
CVAPI(cv::CylindricalWarper*) cveCylindricalWarperCreate(cv::WarperCreator** warperCreator);
CVAPI(void) cveCylindricalWarperRelease(cv::CylindricalWarper** warper);


CVAPI(cv::detail::SphericalWarper*) cveDetailSphericalWarperCreate(float scale, cv::detail::RotationWarper** rotationWarper);
CVAPI(void) cveDetailSphericalWarperRelease(cv::detail::SphericalWarper** warper);
CVAPI(cv::SphericalWarper*) cveSphericalWarperCreate(cv::WarperCreator** warperCreator);
CVAPI(void) cveSphericalWarperRelease(cv::SphericalWarper** warperCreator);

CVAPI(cv::detail::FisheyeWarper*) cveDetailFisheyeWarperCreate(float scale, cv::detail::RotationWarper** rotationWarper);
CVAPI(void) cveDetailFisheyeWarperRelease(cv::detail::FisheyeWarper** warper);
CVAPI(cv::FisheyeWarper*) cveFisheyeWarperCreate(cv::WarperCreator** warperCreator);
CVAPI(void) cveFisheyeWarperRelease(cv::FisheyeWarper** warperCreator);

CVAPI(cv::detail::StereographicWarper*) cveDetailStereographicWarperCreate(float scale, cv::detail::RotationWarper** rotationWarper);
CVAPI(void) cveDetailStereographicWarperRelease(cv::detail::StereographicWarper** warper);
CVAPI(cv::StereographicWarper*) cveStereographicWarperCreate(cv::WarperCreator** warperCreator);
CVAPI(void) cveStereographicWarperRelease(cv::StereographicWarper** warperCreator);

CVAPI(cv::detail::CompressedRectilinearWarper*) cveDetailCompressedRectilinearWarperCreate(float scale, cv::detail::RotationWarper** rotationWarper);
CVAPI(void) cveDetailCompressedRectilinearWarperRelease(cv::detail::CompressedRectilinearWarper** warper);
CVAPI(cv::CompressedRectilinearWarper*) cveCompressedRectilinearWarperCreate(cv::WarperCreator** warperCreator);
CVAPI(void) cveCompressedRectilinearWarperRelease(cv::CompressedRectilinearWarper** warperCreator);

CVAPI(cv::detail::PaniniWarper*) cveDetailPaniniWarperCreate(float scale, cv::detail::RotationWarper** rotationWarper);
CVAPI(void) cveDetailPaniniWarperRelease(cv::detail::PaniniWarper** warper);
CVAPI(cv::PaniniWarper*) cvePaniniWarperCreate(cv::WarperCreator** warperCreator);
CVAPI(void) cvePaniniWarperRelease(cv::PaniniWarper** warperCreator);

CVAPI(cv::detail::PaniniPortraitWarper*) cveDetailPaniniPortraitWarperCreate(float scale, cv::detail::RotationWarper** rotationWarper);
CVAPI(void) cveDetailPaniniPortraitWarperRelease(cv::detail::PaniniPortraitWarper** warper);
CVAPI(cv::PaniniPortraitWarper*) cvePaniniPortraitWarperCreate(cv::WarperCreator** warperCreator);
CVAPI(void) cvePaniniPortraitWarperRelease(cv::PaniniPortraitWarper** warperCreator);

CVAPI(cv::detail::MercatorWarper*) cveDetailMercatorWarperCreate(float scale, cv::detail::RotationWarper** rotationWarper);
CVAPI(void) cveDetailMercatorWarperRelease(cv::detail::MercatorWarper** warper);
CVAPI(cv::MercatorWarper*) cveMercatorWarperCreate(cv::WarperCreator** warperCreator);
CVAPI(void) cveMercatorWarperRelease(cv::MercatorWarper** warperCreator);

CVAPI(cv::detail::TransverseMercatorWarper*) cveDetailTransverseMercatorWarperCreate(float scale, cv::detail::RotationWarper** rotationWarper);
CVAPI(void) cveDetailTransverseMercatorWarperRelease(cv::detail::TransverseMercatorWarper** warper);
CVAPI(cv::TransverseMercatorWarper*) cveTransverseMercatorWarperCreate(cv::WarperCreator** warperCreator);
CVAPI(void) cveTransverseMercatorWarperRelease(cv::TransverseMercatorWarper** warperCreator);

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

CVAPI(cv::detail::HomographyBasedEstimator*) cveHomographyBasedEstimatorCreate(bool isFocalsEstimated, cv::detail::Estimator** estimatorPtr);
CVAPI(void) cveHomographyBasedEstimatorRelease(cv::detail::HomographyBasedEstimator** estimator);

CVAPI(cv::detail::AffineBasedEstimator*) cveAffineBasedEstimatorCreate(cv::detail::Estimator** estimatorPtr);
CVAPI(void) cveAffineBasedEstimatorRelease(cv::detail::AffineBasedEstimator** estimator);

CVAPI(cv::detail::BestOf2NearestMatcher*) cveBestOf2NearestMatcherCreate(
	bool tryUseGpu,
	float matchConf,
	int numMatchesThresh1,
	int numMatchesThresh2,
	cv::detail::FeaturesMatcher** featuresMatcher);
CVAPI(void) cveBestOf2NearestMatcherRelease(cv::detail::BestOf2NearestMatcher** featuresMatcher);

CVAPI(cv::detail::BestOf2NearestRangeMatcher*) cveBestOf2NearestRangeMatcherCreate(
	int rangeWidth,
	bool tryUseGpu,
	float matchConf,
	int numMatchesThresh1,
	int numMatchesThresh2,
	cv::detail::FeaturesMatcher** featuresMatcher);
CVAPI(void) cveBestOf2NearestRangeMatcherRelease(cv::detail::BestOf2NearestRangeMatcher** featuresMatcher);

CVAPI(cv::detail::AffineBestOf2NearestMatcher*) cveAffineBestOf2NearestMatcherCreate(
	bool fullAffine, 
	bool tryUseGpu,
	float matchConf, 
	int numMatchesThresh1,
	cv::detail::FeaturesMatcher** featuresMatcher);
CVAPI(void) cveAffineBestOf2NearestMatcherRelease(cv::detail::AffineBestOf2NearestMatcher** featuresMatcher);

/*
CVAPI(cv::detail::BestOf2NearestRangeMatcher*) cveBestOf2NearestRangeMatcherCreate(
	int rangeWidth, 
	bool tryUseGpu, 
	float matchConf,
	int numMatchesThresh1, 
	int numMatchesThresh2,
	cv::detail::FeaturesMatcher** featuresMatcher);
CVAPI(void) cveBestOf2NearestRangeMatcherRelease(cv::detail::BestOf2NearestRangeMatcher** featuresMatcher);
*/

CVAPI(cv::detail::PlaneWarperGpu*) cveDetailPlaneWarperGpuCreate(float scale, cv::detail::RotationWarper** rotationWarper);
CVAPI(void) cveDetailPlaneWarperGpuRelease(cv::detail::PlaneWarperGpu** warper);
CVAPI(cv::PlaneWarperGpu*) cvePlaneWarperGpuCreate(cv::WarperCreator** warperCreator);
CVAPI(void) cvePlaneWarperGpuRelease(cv::PlaneWarperGpu** warperCreator);

CVAPI(cv::detail::CylindricalWarperGpu*) cveDetailCylindricalWarperGpuCreate(float scale, cv::detail::RotationWarper** rotationWarper);
CVAPI(void) cveDetailCylindricalWarperGpuRelease(cv::detail::CylindricalWarperGpu** warper);
CVAPI(cv::CylindricalWarperGpu*) cveCylindricalWarperGpuCreate(cv::WarperCreator** warperCreator);
CVAPI(void) cveCylindricalWarperGpuRelease(cv::CylindricalWarperGpu** warperCreator);

CVAPI(cv::detail::SphericalWarperGpu*) cveDetailSphericalWarperGpuCreate(float scale, cv::detail::RotationWarper** rotationWarper);
CVAPI(void) cveDetailSphericalWarperGpuRelease(cv::detail::SphericalWarperGpu** warper);
CVAPI(cv::SphericalWarperGpu*) cveSphericalWarperGpuCreate(cv::WarperCreator** warperCreator);
CVAPI(void) cveSphericalWarperGpuRelease(cv::SphericalWarperGpu** warper);

#endif
