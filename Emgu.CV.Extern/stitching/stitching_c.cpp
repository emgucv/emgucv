//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "stitching_c.h"
/*
cv::Stitcher* cveStitcherCreateDefault(bool tryUseGpu)
{
   cv::Ptr<cv::Stitcher> p = cv::createStitcher(tryUseGpu);
   p.addref();
   return p.get();
}*/

cv::Stitcher* cveStitcherCreate(int mode, cv::Ptr<cv::Stitcher>** sharedPtr)
{
	cv::Ptr<cv::Stitcher> ptr = cv::Stitcher::create(static_cast<cv::Stitcher::Mode>(mode));
	*sharedPtr = new cv::Ptr<cv::Stitcher>(ptr);

	return ptr.get();
}

void cveStitcherRelease(cv::Ptr<cv::Stitcher>** sharedPtr)
{
	delete* sharedPtr;
	*sharedPtr = 0;
}

void cveStitcherSetFeaturesFinder(cv::Stitcher* stitcher, cv::Feature2D* finder)
{
	cv::Ptr<cv::Feature2D> p(finder, [](cv::Feature2D*) {});
	stitcher->setFeaturesFinder(p);
}

void cveStitcherSetWarper(cv::Stitcher* stitcher, cv::WarperCreator* creator)
{
	cv::Ptr<cv::WarperCreator> p(creator, [](cv::WarperCreator*) {});
	stitcher->setWarper(p);
}

void cveStitcherSetBlender(cv::Stitcher* stitcher, cv::detail::Blender* b)
{
	cv::Ptr<cv::detail::Blender> blender(b, [](cv::detail::Blender*) {});
	stitcher->setBlender(blender);
}

void cveStitcherSetExposureCompensator(cv::Stitcher* stitcher, cv::detail::ExposureCompensator* exposureComp)
{
	cv::Ptr<cv::detail::ExposureCompensator> p(exposureComp, [](cv::detail::ExposureCompensator*) {});
	stitcher->setExposureCompensator(p);
}

void cveStitcherSetBundleAdjuster(cv::Stitcher* stitcher, cv::detail::BundleAdjusterBase* bundleAdjuster)
{
	cv::Ptr<cv::detail::BundleAdjusterBase> p(bundleAdjuster, [](cv::detail::BundleAdjusterBase*) {});
	stitcher->setBundleAdjuster(p);
}

void cveStitcherSetSeamFinder(cv::Stitcher* stitcher, cv::detail::SeamFinder* seamFinder)
{
	cv::Ptr<cv::detail::SeamFinder> p(seamFinder, [](cv::detail::SeamFinder*) {});
	stitcher->setSeamFinder(p);
}

void cveStitcherSetEstimator(cv::Stitcher* stitcher, cv::detail::Estimator* estimator)
{
	cv::Ptr<cv::detail::Estimator> p(estimator, [](cv::detail::Estimator*) {});
	stitcher->setEstimator(p);
}

void cveStitcherSetFeaturesMatcher(cv::Stitcher* stitcher, cv::detail::FeaturesMatcher* featuresMatcher)
{
	cv::Ptr<cv::detail::FeaturesMatcher> p(featuresMatcher, [](cv::detail::FeaturesMatcher*) {});
	stitcher->setFeaturesMatcher(p);
}

void cveStitcherSetWaveCorrection(cv::Stitcher* stitcher, bool flag)
{
	stitcher->setWaveCorrection(flag);
}
bool cveStitcherGetWaveCorrection(cv::Stitcher* stitcher)
{
	return stitcher->waveCorrection();
}
void cveStitcherSetWaveCorrectionKind(cv::Stitcher* stitcher, int kind)
{
	stitcher->setWaveCorrectKind(static_cast<cv::detail::WaveCorrectKind>(kind));
}
int cveStitcherGetWaveCorrectionKind(cv::Stitcher* stitcher)
{
	return stitcher->waveCorrectKind();
}
void cveStitcherSetPanoConfidenceThresh(cv::Stitcher* stitcher, double confThresh)
{
	stitcher->setPanoConfidenceThresh(confThresh);
}
double cveStitcherGetPanoConfidenceThresh(cv::Stitcher* stitcher)
{
	return stitcher->panoConfidenceThresh();
}
void cveStitcherSetCompositingResol(cv::Stitcher* stitcher, double resolMpx)
{
	stitcher->setCompositingResol(resolMpx);
}
double cveStitcherGetCompositingResol(cv::Stitcher* stitcher)
{
	return stitcher->compositingResol();
}
void cveStitcherSetSeamEstimationResol(cv::Stitcher* stitcher, double resolMpx)
{
	stitcher->setSeamEstimationResol(resolMpx);
}
double cveStitcherGetSeamEstimationResol(cv::Stitcher* stitcher)
{
	return stitcher->seamEstimationResol();
}
void cveStitcherSetRegistrationResol(cv::Stitcher* stitcher, double resolMpx)
{
	stitcher->setRegistrationResol(resolMpx);
}
double cveStitcherGetRegistrationResol(cv::Stitcher* stitcher)
{
	return stitcher->registrationResol();
}

int cveStitcherGetInterpolationFlags(cv::Stitcher* stitcher)
{
	return stitcher->interpolationFlags();
}
void cveStitcherSetInterpolationFlags(cv::Stitcher* stitcher, int interpFlags)
{
	stitcher->setInterpolationFlags(static_cast<cv::InterpolationFlags>(interpFlags));
}

int cveStitcherStitch(cv::Stitcher* stitcher, cv::_InputArray* images, cv::_OutputArray* pano)
{
	return stitcher->stitch(*images, *pano);
}


int cveStitcherEstimateTransform(cv::Stitcher* stitcher, cv::_InputArray* images, cv::_InputArray* masks)
{
	return stitcher->estimateTransform(*images, masks ? *masks : (cv::InputArrayOfArrays) cv::noArray());
}

int cveStitcherComposePanorama1(cv::Stitcher* stitcher, cv::_OutputArray* pano)
{
	return stitcher->composePanorama(*pano);
}

int cveStitcherComposePanorama2(cv::Stitcher* stitcher, cv::_InputArray* images, cv::_OutputArray* pano)
{
	return stitcher->composePanorama(*images, *pano);
}

void cveRotationWarperBuildMaps(cv::detail::RotationWarper* warper, CvSize* srcSize, cv::_InputArray* K, cv::_InputArray* R, cv::_OutputArray* xmap, cv::_OutputArray* ymap, CvRect* boundingBox)
{
	*boundingBox = cvRect(warper->buildMaps(*srcSize, *K, *R, *xmap, *ymap));
}
void cveRotationWarperWarp(cv::detail::RotationWarper* warper, cv::_InputArray* src, cv::_InputArray* K, cv::_InputArray* R, int interpMode, int borderMode, cv::_OutputArray* dst, CvPoint* corner)
{
	*corner = cvPoint(warper->warp(*src, *K, *R, interpMode, borderMode, *dst));
}

cv::detail::PlaneWarper* cvePlaneWarperCreate(float scale, cv::WarperCreator** creator, cv::detail::RotationWarper** rotationWarper)
{
	cv::detail::PlaneWarper* ptr = new cv::detail::PlaneWarper(scale);
	*creator = dynamic_cast<cv::WarperCreator*>(ptr);
	*rotationWarper = dynamic_cast<cv::detail::RotationWarper*>(ptr);
	return ptr;
}
void cvePlaneWarperRelease(cv::detail::PlaneWarper** warper)
{
	delete* warper;
	*warper = 0;
}

cv::detail::CylindricalWarper* cveCylindricalWarperCreate(float scale, cv::WarperCreator** creator, cv::detail::RotationWarper** rotationWarper)
{
	cv::detail::CylindricalWarper* ptr = new cv::detail::CylindricalWarper(scale);
	*creator = dynamic_cast<cv::WarperCreator*>(ptr);
	*rotationWarper = dynamic_cast<cv::detail::RotationWarper*>(ptr);
	return ptr;
}
void cveCylindricalWarperRelease(cv::detail::CylindricalWarper** warper)
{
	delete* warper;
	*warper = 0;
}

cv::detail::SphericalWarper* cveSphericalWarperCreate(float scale, cv::WarperCreator** creator, cv::detail::RotationWarper** rotationWarper)
{
	cv::detail::SphericalWarper* ptr = new cv::detail::SphericalWarper(scale);
	*creator = dynamic_cast<cv::WarperCreator*>(ptr);
	*rotationWarper = dynamic_cast<cv::detail::RotationWarper*>(ptr);
	return ptr;
}
void cveSphericalWarperRelease(cv::detail::SphericalWarper** warper)
{
	delete* warper;
	*warper = 0;
}

cv::detail::FisheyeWarper* cveFisheyeWarperCreate(float scale, cv::WarperCreator** creator, cv::detail::RotationWarper** rotationWarper)
{
	cv::detail::FisheyeWarper* ptr = new cv::detail::FisheyeWarper(scale);
	*creator = dynamic_cast<cv::WarperCreator*>(ptr);
	*rotationWarper = dynamic_cast<cv::detail::RotationWarper*>(ptr);
	return ptr;
}
void cveFisheyeWarperRelease(cv::detail::FisheyeWarper** warper)
{
	delete* warper;
	*warper = 0;
}

cv::detail::StereographicWarper* cveStereographicWarperCreate(float scale, cv::WarperCreator** creator, cv::detail::RotationWarper** rotationWarper)
{
	cv::detail::StereographicWarper* ptr = new cv::detail::StereographicWarper(scale);
	*creator = dynamic_cast<cv::WarperCreator*>(ptr);
	*rotationWarper = dynamic_cast<cv::detail::RotationWarper*>(ptr);
	return ptr;
}
void cveStereographicWarperRelease(cv::detail::StereographicWarper** warper)
{
	delete* warper;
	*warper = 0;
}

cv::detail::CompressedRectilinearWarper* cveCompressedRectilinearWarperCreate(float scale, cv::WarperCreator** creator, cv::detail::RotationWarper** rotationWarper)
{
	cv::detail::CompressedRectilinearWarper* ptr = new cv::detail::CompressedRectilinearWarper(scale);
	*creator = dynamic_cast<cv::WarperCreator*>(ptr);
	*rotationWarper = dynamic_cast<cv::detail::RotationWarper*>(ptr);
	return ptr;
}
void cveCompressedRectilinearWarperRelease(cv::detail::CompressedRectilinearWarper** warper)
{
	delete* warper;
	*warper = 0;
}

cv::detail::PaniniWarper* cvePaniniWarperCreate(float scale, cv::WarperCreator** creator, cv::detail::RotationWarper** rotationWarper)
{
	cv::detail::PaniniWarper* ptr = new cv::detail::PaniniWarper(scale);
	*creator = dynamic_cast<cv::WarperCreator*>(ptr);
	*rotationWarper = dynamic_cast<cv::detail::RotationWarper*>(ptr);
	return ptr;
}
void cvePaniniWarperRelease(cv::detail::PaniniWarper** warper)
{
	delete* warper;
	*warper = 0;
}

cv::detail::PaniniPortraitWarper* cvePaniniPortraitWarperCreate(float scale, cv::WarperCreator** creator, cv::detail::RotationWarper** rotationWarper)
{
	cv::detail::PaniniPortraitWarper* ptr = new cv::detail::PaniniPortraitWarper(scale);
	*creator = dynamic_cast<cv::WarperCreator*>(ptr);
	*rotationWarper = dynamic_cast<cv::detail::RotationWarper*>(ptr);
	return ptr;
}
void cvePaniniPortraitWarperRelease(cv::detail::PaniniPortraitWarper** warper)
{
	delete* warper;
	*warper = 0;
}

cv::detail::MercatorWarper* cveMercatorWarperCreate(float scale, cv::WarperCreator** creator, cv::detail::RotationWarper** rotationWarper)
{
	cv::detail::MercatorWarper* ptr = new cv::detail::MercatorWarper(scale);
	*creator = dynamic_cast<cv::WarperCreator*>(ptr);
	*rotationWarper = dynamic_cast<cv::detail::RotationWarper*>(ptr);
	return ptr;
}
void cveMercatorWarperRelease(cv::detail::MercatorWarper** warper)
{
	delete* warper;
	*warper = 0;
}

cv::detail::TransverseMercatorWarper* cveTransverseMercatorWarperCreate(float scale, cv::WarperCreator** creator, cv::detail::RotationWarper** rotationWarper)
{
	cv::detail::TransverseMercatorWarper* ptr = new cv::detail::TransverseMercatorWarper(scale);
	*creator = dynamic_cast<cv::WarperCreator*>(ptr);
	*rotationWarper = dynamic_cast<cv::detail::RotationWarper*>(ptr);
	return ptr;
}
void cveTransverseMercatorWarperRelease(cv::detail::TransverseMercatorWarper** warper)
{
	delete* warper;
	*warper = 0;
}

cv::detail::FeatherBlender* cveFeatherBlenderCreate(float sharpness, cv::detail::Blender** blender)
{
	cv::detail::FeatherBlender* ptr = new cv::detail::FeatherBlender(sharpness);
	*blender = dynamic_cast<cv::detail::Blender*>(ptr);
	return ptr;
}
void cveFeatherBlenderRelease(cv::detail::FeatherBlender** blender)
{
	delete* blender;
	*blender = 0;
}

cv::detail::MultiBandBlender* cveMultiBandBlenderCreate(int tryGpu, int numBands, int weightType, cv::detail::Blender** blender)
{
	cv::detail::MultiBandBlender* ptr = new cv::detail::MultiBandBlender(tryGpu, numBands, weightType);
	*blender = dynamic_cast<cv::detail::Blender*>(ptr);
	return ptr;
}
void cveMultiBandBlenderRelease(cv::detail::MultiBandBlender** blender)
{
	delete* blender;
	*blender = 0;
}

cv::detail::NoExposureCompensator* cveNoExposureCompensatorCreate(cv::detail::ExposureCompensator** exposureCompensatorPtr)
{
	cv::detail::NoExposureCompensator* ptr = new cv::detail::NoExposureCompensator();
	*exposureCompensatorPtr = dynamic_cast<cv::detail::ExposureCompensator*>(ptr);
	return ptr;
}
void cveNoExposureCompensatorRelease(cv::detail::NoExposureCompensator** compensator)
{
	delete* compensator;
	*compensator = 0;
}

cv::detail::GainCompensator* cveGainCompensatorCreate(int nrFeeds, cv::detail::ExposureCompensator** exposureCompensatorPtr)
{
	cv::detail::GainCompensator* ptr = new cv::detail::GainCompensator(nrFeeds);
	*exposureCompensatorPtr = dynamic_cast<cv::detail::ExposureCompensator*>(ptr);
	return ptr;
}
void cveGainCompensatorRelease(cv::detail::GainCompensator** compensator)
{
	delete* compensator;
	*compensator = 0;
}

cv::detail::ChannelsCompensator* cveChannelsCompensatorCreate(int nrFeeds, cv::detail::ExposureCompensator** exposureCompensatorPtr)
{
	cv::detail::ChannelsCompensator* ptr = new cv::detail::ChannelsCompensator(nrFeeds);
	*exposureCompensatorPtr = dynamic_cast<cv::detail::ExposureCompensator*>(ptr);
	return ptr;
}
void cveChannelsCompensatorRelease(cv::detail::ChannelsCompensator** compensator)
{
	delete* compensator;
	*compensator = 0;
}

//cv::detail::BlocksCompensator* cveBlocksCompensatorCreate(int blWidth, int blHeight, int nrFeeds, cv::detail::ExposureCompensator** exposureCompensatorPtr)
//{
//    cv::detail::BlocksCompensator* ptr = new cv::detail::BlocksCompensator(blWidth, blHeight, nrFeeds);
//	*exposureCompensatorPtr = dynamic_cast<cv::detail::ExposureCompensator*>(ptr);
//    return ptr;
//}
//void cveBlocksCompensatorRelease(cv::detail::BlocksCompensator** compensator)
//{
//    delete* compensator;
//    *compensator = 0;
//}

cv::detail::BlocksGainCompensator* cveBlocksGainCompensatorCreate(int blWidth, int blHeight, int nrFeeds, cv::detail::ExposureCompensator** exposureCompensatorPtr)
{
	cv::detail::BlocksGainCompensator* ptr = new cv::detail::BlocksGainCompensator(blWidth, blHeight, nrFeeds);
	*exposureCompensatorPtr = dynamic_cast<cv::detail::ExposureCompensator*>(ptr);
	return ptr;
}
void cveBlocksGainCompensatorRelease(cv::detail::BlocksGainCompensator** compensator)
{
	delete* compensator;
	*compensator = 0;
}

cv::detail::BlocksChannelsCompensator* cveBlocksChannelsCompensatorCreate(int blWidth, int blHeight, int nrFeeds, cv::detail::ExposureCompensator** exposureCompensatorPtr)
{
	cv::detail::BlocksChannelsCompensator* ptr = new cv::detail::BlocksChannelsCompensator(blWidth, blHeight, nrFeeds);
	*exposureCompensatorPtr = dynamic_cast<cv::detail::ExposureCompensator*>(ptr);
	return ptr;
}
void cveBlocksChannelsCompensatorRelease(cv::detail::BlocksChannelsCompensator** compensator)
{
	delete* compensator;
	*compensator = 0;
}

cv::detail::NoBundleAdjuster* cveNoBundleAdjusterCreate(cv::detail::BundleAdjusterBase** bundleAdjusterBasePtr)
{
	cv::detail::NoBundleAdjuster* ptr = new cv::detail::NoBundleAdjuster();
	*bundleAdjusterBasePtr = dynamic_cast<cv::detail::BundleAdjusterBase*>(ptr);
	return ptr;
}
void cveNoBundleAdjusterRelease(cv::detail::NoBundleAdjuster** bundleAdjuster)
{
	delete* bundleAdjuster;
	*bundleAdjuster = 0;
}

cv::detail::BundleAdjusterReproj* cveBundleAdjusterReprojCreate(cv::detail::BundleAdjusterBase** bundleAdjusterBasePtr)
{
	cv::detail::BundleAdjusterReproj* ptr = new cv::detail::BundleAdjusterReproj();
	*bundleAdjusterBasePtr = dynamic_cast<cv::detail::BundleAdjusterBase*>(ptr);
	return ptr;
}
void cveBundleAdjusterReprojRelease(cv::detail::BundleAdjusterReproj** bundleAdjuster)
{
	delete* bundleAdjuster;
	*bundleAdjuster = 0;
}

cv::detail::BundleAdjusterRay* cveBundleAdjusterRayCreate(cv::detail::BundleAdjusterBase** bundleAdjusterBasePtr)
{
	cv::detail::BundleAdjusterRay* ptr = new cv::detail::BundleAdjusterRay();
	*bundleAdjusterBasePtr = dynamic_cast<cv::detail::BundleAdjusterBase*>(ptr);
	return ptr;
}
void cveBundleAdjusterRayRelease(cv::detail::BundleAdjusterRay** bundleAdjuster)
{
	delete* bundleAdjuster;
	*bundleAdjuster = 0;
}

cv::detail::BundleAdjusterAffine* cveBundleAdjusterAffineCreate(cv::detail::BundleAdjusterBase** bundleAdjusterBasePtr)
{
	cv::detail::BundleAdjusterAffine* ptr = new cv::detail::BundleAdjusterAffine();
	*bundleAdjusterBasePtr = dynamic_cast<cv::detail::BundleAdjusterBase*>(ptr);
	return ptr;
}
void cveBundleAdjusterAffineRelease(cv::detail::BundleAdjusterAffine** bundleAdjuster)
{
	delete* bundleAdjuster;
	*bundleAdjuster = 0;
}

cv::detail::BundleAdjusterAffinePartial* cveBundleAdjusterAffinePartialCreate(cv::detail::BundleAdjusterBase** bundleAdjusterBasePtr)
{
	cv::detail::BundleAdjusterAffinePartial* ptr = new cv::detail::BundleAdjusterAffinePartial();
	*bundleAdjusterBasePtr = dynamic_cast<cv::detail::BundleAdjusterBase*>(ptr);
	return ptr;
}
void cveBundleAdjusterAffinePartialRelease(cv::detail::BundleAdjusterAffinePartial** bundleAdjuster)
{
	delete* bundleAdjuster;
	*bundleAdjuster = 0;
}

cv::detail::NoSeamFinder* cveNoSeamFinderCreate(cv::detail::SeamFinder** seamFinderPtr)
{
	cv::detail::NoSeamFinder* ptr = new cv::detail::NoSeamFinder();
	*seamFinderPtr = dynamic_cast<cv::detail::SeamFinder*>(ptr);
	return ptr;
}
void cveNoSeamFinderRelease(cv::detail::NoSeamFinder** seamFinder)
{
	delete* seamFinder;
	*seamFinder = 0;
}

/*
cv::detail::PairwiseSeamFinder* cvePairwiseSeamFinderCreate(cv::detail::SeamFinder** seamFinderPtr)
{
	cv::detail::PairwiseSeamFinder* ptr = new cv::detail::PairwiseSeamFinder();
	*seamFinderPtr = dynamic_cast<cv::detail::SeamFinder*>(ptr);
	return ptr;
}
void cvePairwiseSeamFinderRelease(cv::detail::PairwiseSeamFinder** seamFinder)
{
	delete* seamFinder;
	*seamFinder = 0;
}
*/

cv::detail::VoronoiSeamFinder* cveVoronoiSeamFinderCreate(cv::detail::SeamFinder** seamFinderPtr)
{
	cv::detail::VoronoiSeamFinder* ptr = new cv::detail::VoronoiSeamFinder();
	*seamFinderPtr = dynamic_cast<cv::detail::SeamFinder*>(ptr);
	return ptr;
}
void cveVoronoiSeamFinderRelease(cv::detail::VoronoiSeamFinder** seamFinder)
{
	delete* seamFinder;
	*seamFinder = 0;
}

cv::detail::DpSeamFinder* cveDpSeamFinderCreate(int costFunc, cv::detail::SeamFinder** seamFinderPtr)
{
	cv::detail::DpSeamFinder* ptr = new cv::detail::DpSeamFinder(static_cast<cv::detail::DpSeamFinder::CostFunction>(costFunc));
	*seamFinderPtr = dynamic_cast<cv::detail::SeamFinder*>(ptr);
	return ptr;
}
void cveDpSeamFinderRelease(cv::detail::DpSeamFinder** seamFinder)
{
	delete* seamFinder;
	*seamFinder = 0;
}

cv::detail::GraphCutSeamFinder* cveGraphCutSeamFinderCreate(
	int costType,
	float terminalCost,
	float badRegionPenalty,
	cv::detail::SeamFinder** seamFinderPtr)
{
	cv::detail::GraphCutSeamFinder* ptr = new cv::detail::GraphCutSeamFinder(
		costType, terminalCost, badRegionPenalty );
	*seamFinderPtr = dynamic_cast<cv::detail::SeamFinder*>(ptr);
	return ptr;
}
void cveGraphCutSeamFinderRelease(cv::detail::GraphCutSeamFinder** seamFinder)
{
	delete* seamFinder;
	*seamFinder = 0;
}

cv::detail::HomographyBasedEstimator* cveHomographyBasedEstimatorCreate(bool isFocalsEstimated, cv::detail::Estimator** estimatorPtr)
{
	cv::detail::HomographyBasedEstimator* ptr = new cv::detail::HomographyBasedEstimator(isFocalsEstimated);
	*estimatorPtr = dynamic_cast<cv::detail::Estimator*>(ptr);
	return ptr;
}
void cveHomographyBasedEstimatorRelease(cv::detail::HomographyBasedEstimator** estimator)
{
	delete* estimator;
	*estimator = 0;
}

cv::detail::AffineBasedEstimator* cveAffineBasedEstimatorCreate(cv::detail::Estimator** estimatorPtr)
{
	cv::detail::AffineBasedEstimator* ptr = new cv::detail::AffineBasedEstimator();
	*estimatorPtr = dynamic_cast<cv::detail::Estimator*>(ptr);
	return ptr;
}
void cveAffineBasedEstimatorRelease(cv::detail::AffineBasedEstimator** estimator)
{
	delete* estimator;
	*estimator = 0;
}

cv::detail::BestOf2NearestMatcher* cveBestOf2NearestMatcherCreate(
	bool tryUseGpu,
	float matchConf,
	int numMatchesThresh1,
	int numMatchesThresh2,
	cv::detail::FeaturesMatcher** featuresMatcher)
{
	cv::detail::BestOf2NearestMatcher* ptr = new cv::detail::BestOf2NearestMatcher(tryUseGpu, matchConf, numMatchesThresh1, numMatchesThresh2);
	*featuresMatcher = dynamic_cast<cv::detail::FeaturesMatcher*>(ptr);
	return ptr;
}
void cveBestOf2NearestMatcherRelease(cv::detail::BestOf2NearestMatcher** featuresMatcher)
{
	delete* featuresMatcher;
	*featuresMatcher = 0;
}

cv::detail::BestOf2NearestRangeMatcher* cveBestOf2NearestRangeMatcherCreate(
	int rangeWidth,
	bool tryUseGpu,
	float matchConf,
	int numMatchesThresh1,
	int numMatchesThresh2,
	cv::detail::FeaturesMatcher** featuresMatcher)
{
	cv::detail::BestOf2NearestRangeMatcher* ptr = new cv::detail::BestOf2NearestRangeMatcher(
		rangeWidth,
		tryUseGpu, 
		matchConf, 
		numMatchesThresh1, 
		numMatchesThresh2);
	*featuresMatcher = dynamic_cast<cv::detail::FeaturesMatcher*>(ptr);
	return ptr;
}
void cveBestOf2NearestRangeMatcherRelease(cv::detail::BestOf2NearestRangeMatcher** featuresMatcher)
{
	delete* featuresMatcher;
	*featuresMatcher = 0;
}

cv::detail::AffineBestOf2NearestMatcher* cveAffineBestOf2NearestMatcherCreate(
	bool fullAffine,
	bool tryUseGpu,
	float matchConf,
	int numMatchesThresh1,
	cv::detail::FeaturesMatcher** featuresMatcher)
{
	cv::detail::AffineBestOf2NearestMatcher* ptr = new cv::detail::AffineBestOf2NearestMatcher(
		fullAffine,
		tryUseGpu,
		matchConf,
		numMatchesThresh1);
	*featuresMatcher = dynamic_cast<cv::detail::FeaturesMatcher*>(ptr);
	return ptr;
}
void cveAffineBestOf2NearestMatcherRelease(cv::detail::AffineBestOf2NearestMatcher** featuresMatcher)
{
	delete* featuresMatcher;
	*featuresMatcher = 0;
}


cv::detail::PlaneWarperGpu* cvePlaneWarperGpuCreate(float scale, cv::WarperCreator** creator, cv::detail::RotationWarper** rotationWarper)
{
#ifdef HAVE_OPENCV_CUDAWARPING
	cv::detail::PlaneWarperGpu* ptr = new cv::detail::PlaneWarperGpu(scale);
	*creator = dynamic_cast<cv::WarperCreator*>(ptr);
	*rotationWarper = dynamic_cast<cv::detail::RotationWarper*>(ptr);
	return ptr;
#else
	throw_no_cudawarping();
#endif
}
void cvePlaneWarperGpuRelease(cv::detail::PlaneWarperGpu** warper)
{
#ifdef HAVE_OPENCV_CUDAWARPING
	delete* warper;
	*warper = 0;
#else
	throw_no_cudawarping();
#endif
}

cv::detail::CylindricalWarperGpu* cveCylindricalWarperGpuCreate(float scale, cv::WarperCreator** creator, cv::detail::RotationWarper** rotationWarper)
{
#ifdef HAVE_OPENCV_CUDAWARPING
	cv::detail::CylindricalWarperGpu* ptr = new cv::detail::CylindricalWarperGpu(scale);
	*creator = dynamic_cast<cv::WarperCreator*>(ptr);
	*rotationWarper = dynamic_cast<cv::detail::RotationWarper*>(ptr);
	return ptr;
#else
	throw_no_cudawarping();
#endif
}
void cveCylindricalWarperGpuRelease(cv::detail::CylindricalWarperGpu** warper)
{
#ifdef HAVE_OPENCV_CUDAWARPING
	delete* warper;
	*warper = 0;
#else
	throw_no_cudawarping();
#endif
}

cv::detail::SphericalWarperGpu* cveSphericalWarperGpuCreate(float scale, cv::WarperCreator** creator, cv::detail::RotationWarper** rotationWarper)
{
#ifdef HAVE_OPENCV_CUDAWARPING
	cv::detail::SphericalWarperGpu* ptr = new cv::detail::SphericalWarperGpu(scale);
	*creator = dynamic_cast<cv::WarperCreator*>(ptr);
	*rotationWarper = dynamic_cast<cv::detail::RotationWarper*>(ptr);
	return ptr;
#else
	throw_no_cudawarping();
#endif
}
void cveSphericalWarperGpuRelease(cv::detail::SphericalWarperGpu** warper)
{
#ifdef HAVE_OPENCV_CUDAWARPING
	delete* warper;
	*warper = 0;
#else
	throw_no_cudawarping();
#endif
}

