//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "stitching_c.h"

cv::Stitcher* cveStitcherCreate(int mode, cv::Ptr<cv::Stitcher>** sharedPtr)
{
#ifdef HAVE_OPENCV_STITCHING
	cv::Ptr<cv::Stitcher> ptr = cv::Stitcher::create(static_cast<cv::Stitcher::Mode>(mode));
	*sharedPtr = new cv::Ptr<cv::Stitcher>(ptr);
	return ptr.get();
#else
	throw_no_stitching();
#endif
}

void cveStitcherRelease(cv::Ptr<cv::Stitcher>** sharedPtr)
{
#ifdef HAVE_OPENCV_STITCHING
	delete* sharedPtr;
	*sharedPtr = 0;
#else
	throw_no_stitching();
#endif
}

void cveStitcherSetFeaturesFinder(cv::Stitcher* stitcher, cv::Feature2D* finder)
{
#ifdef HAVE_OPENCV_STITCHING
	cv::Ptr<cv::Feature2D> p(finder, [](cv::Feature2D*) {});
	stitcher->setFeaturesFinder(p);
#else
	throw_no_stitching();
#endif
}

void cveStitcherSetWarper(cv::Stitcher* stitcher, cv::WarperCreator* creator)
{
#ifdef HAVE_OPENCV_STITCHING
	cv::Ptr<cv::WarperCreator> p(creator, [](cv::WarperCreator*) {});
	stitcher->setWarper(p);
#else
	throw_no_stitching();
#endif
}

void cveStitcherSetBlender(cv::Stitcher* stitcher, cv::detail::Blender* b)
{
#ifdef HAVE_OPENCV_STITCHING
	cv::Ptr<cv::detail::Blender> blender(b, [](cv::detail::Blender*) {});
	stitcher->setBlender(blender);
#else
	throw_no_stitching();
#endif
}

void cveStitcherSetExposureCompensator(cv::Stitcher* stitcher, cv::detail::ExposureCompensator* exposureComp)
{
#ifdef HAVE_OPENCV_STITCHING
	cv::Ptr<cv::detail::ExposureCompensator> p(exposureComp, [](cv::detail::ExposureCompensator*) {});
	stitcher->setExposureCompensator(p);
#else
	throw_no_stitching();
#endif
}

void cveStitcherSetBundleAdjuster(cv::Stitcher* stitcher, cv::detail::BundleAdjusterBase* bundleAdjuster)
{
#ifdef HAVE_OPENCV_STITCHING
	cv::Ptr<cv::detail::BundleAdjusterBase> p(bundleAdjuster, [](cv::detail::BundleAdjusterBase*) {});
	stitcher->setBundleAdjuster(p);
#else
	throw_no_stitching();
#endif
}

void cveStitcherSetSeamFinder(cv::Stitcher* stitcher, cv::detail::SeamFinder* seamFinder)
{
#ifdef HAVE_OPENCV_STITCHING
	cv::Ptr<cv::detail::SeamFinder> p(seamFinder, [](cv::detail::SeamFinder*) {});
	stitcher->setSeamFinder(p);
#else
	throw_no_stitching();
#endif
}

void cveStitcherSetEstimator(cv::Stitcher* stitcher, cv::detail::Estimator* estimator)
{
#ifdef HAVE_OPENCV_STITCHING
	cv::Ptr<cv::detail::Estimator> p(estimator, [](cv::detail::Estimator*) {});
	stitcher->setEstimator(p);
#else
	throw_no_stitching();
#endif
}

void cveStitcherSetFeaturesMatcher(cv::Stitcher* stitcher, cv::detail::FeaturesMatcher* featuresMatcher)
{
#ifdef HAVE_OPENCV_STITCHING
	cv::Ptr<cv::detail::FeaturesMatcher> p(featuresMatcher, [](cv::detail::FeaturesMatcher*) {});
	stitcher->setFeaturesMatcher(p);
#else
	throw_no_stitching();
#endif
}

void cveStitcherSetWaveCorrection(cv::Stitcher* stitcher, bool flag)
{
#ifdef HAVE_OPENCV_STITCHING
	stitcher->setWaveCorrection(flag);
#else
	throw_no_stitching();
#endif
}
bool cveStitcherGetWaveCorrection(cv::Stitcher* stitcher)
{
#ifdef HAVE_OPENCV_STITCHING
	return stitcher->waveCorrection();
#else
	throw_no_stitching();
#endif
}
void cveStitcherSetWaveCorrectionKind(cv::Stitcher* stitcher, int kind)
{
#ifdef HAVE_OPENCV_STITCHING
	stitcher->setWaveCorrectKind(static_cast<cv::detail::WaveCorrectKind>(kind));
#else
	throw_no_stitching();
#endif
}
int cveStitcherGetWaveCorrectionKind(cv::Stitcher* stitcher)
{
#ifdef HAVE_OPENCV_STITCHING
	return stitcher->waveCorrectKind();
#else
	throw_no_stitching();
#endif
}
void cveStitcherSetPanoConfidenceThresh(cv::Stitcher* stitcher, double confThresh)
{
#ifdef HAVE_OPENCV_STITCHING
	stitcher->setPanoConfidenceThresh(confThresh);
#else
	throw_no_stitching();
#endif
}
double cveStitcherGetPanoConfidenceThresh(cv::Stitcher* stitcher)
{
#ifdef HAVE_OPENCV_STITCHING
	return stitcher->panoConfidenceThresh();
#else
	throw_no_stitching();
#endif
}
void cveStitcherSetCompositingResol(cv::Stitcher* stitcher, double resolMpx)
{
#ifdef HAVE_OPENCV_STITCHING
	stitcher->setCompositingResol(resolMpx);
#else
	throw_no_stitching();
#endif
}
double cveStitcherGetCompositingResol(cv::Stitcher* stitcher)
{
#ifdef HAVE_OPENCV_STITCHING
	return stitcher->compositingResol();
#else
	throw_no_stitching();
#endif
}
void cveStitcherSetSeamEstimationResol(cv::Stitcher* stitcher, double resolMpx)
{
#ifdef HAVE_OPENCV_STITCHING
	stitcher->setSeamEstimationResol(resolMpx);
#else
	throw_no_stitching();
#endif
}
double cveStitcherGetSeamEstimationResol(cv::Stitcher* stitcher)
{
#ifdef HAVE_OPENCV_STITCHING
	return stitcher->seamEstimationResol();
#else
	throw_no_stitching();
#endif
}
void cveStitcherSetRegistrationResol(cv::Stitcher* stitcher, double resolMpx)
{
#ifdef HAVE_OPENCV_STITCHING
	stitcher->setRegistrationResol(resolMpx);
#else
	throw_no_stitching();
#endif
}
double cveStitcherGetRegistrationResol(cv::Stitcher* stitcher)
{
#ifdef HAVE_OPENCV_STITCHING
	return stitcher->registrationResol();
#else
	throw_no_stitching();
#endif
}

int cveStitcherGetInterpolationFlags(cv::Stitcher* stitcher)
{
#ifdef HAVE_OPENCV_STITCHING
	return stitcher->interpolationFlags();
#else
	throw_no_stitching();
#endif
}
void cveStitcherSetInterpolationFlags(cv::Stitcher* stitcher, int interpFlags)
{
#ifdef HAVE_OPENCV_STITCHING
	stitcher->setInterpolationFlags(static_cast<cv::InterpolationFlags>(interpFlags));
#else
	throw_no_stitching();
#endif
}

int cveStitcherStitch(cv::Stitcher* stitcher, cv::_InputArray* images, cv::_OutputArray* pano)
{
#ifdef HAVE_OPENCV_STITCHING
	return stitcher->stitch(*images, *pano);
#else
	throw_no_stitching();
#endif
}


int cveStitcherEstimateTransform(cv::Stitcher* stitcher, cv::_InputArray* images, cv::_InputArray* masks)
{
#ifdef HAVE_OPENCV_STITCHING
	return stitcher->estimateTransform(*images, masks ? *masks : (cv::InputArrayOfArrays) cv::noArray());
#else
	throw_no_stitching();
#endif
}

int cveStitcherComposePanorama1(cv::Stitcher* stitcher, cv::_OutputArray* pano)
{
#ifdef HAVE_OPENCV_STITCHING
	return stitcher->composePanorama(*pano);
#else
	throw_no_stitching();
#endif
}

int cveStitcherComposePanorama2(cv::Stitcher* stitcher, cv::_InputArray* images, cv::_OutputArray* pano)
{
#ifdef HAVE_OPENCV_STITCHING
	return stitcher->composePanorama(*images, *pano);
#else
	throw_no_stitching();
#endif
}

void cveRotationWarperBuildMaps(cv::detail::RotationWarper* warper, CvSize* srcSize, cv::_InputArray* K, cv::_InputArray* R, cv::_OutputArray* xmap, cv::_OutputArray* ymap, CvRect* boundingBox)
{
#ifdef HAVE_OPENCV_STITCHING
	*boundingBox = cvRect(warper->buildMaps(*srcSize, *K, *R, *xmap, *ymap));
#else
	throw_no_stitching();
#endif
}
void cveRotationWarperWarp(cv::detail::RotationWarper* warper, cv::_InputArray* src, cv::_InputArray* K, cv::_InputArray* R, int interpMode, int borderMode, cv::_OutputArray* dst, CvPoint* corner)
{
#ifdef HAVE_OPENCV_STITCHING
	*corner = cvPoint(warper->warp(*src, *K, *R, interpMode, borderMode, *dst));
#else
	throw_no_stitching();
#endif
}

cv::detail::PlaneWarper* cveDetailPlaneWarperCreate(float scale, cv::detail::RotationWarper** rotationWarper)
{
#ifdef HAVE_OPENCV_STITCHING
	cv::detail::PlaneWarper* ptr = new cv::detail::PlaneWarper(scale);
	*rotationWarper = dynamic_cast<cv::detail::RotationWarper*>(ptr);
	return ptr;
#else
	throw_no_stitching();
#endif
}
void cveDetailPlaneWarperRelease(cv::detail::PlaneWarper** warper)
{
#ifdef HAVE_OPENCV_STITCHING
	delete* warper;
	*warper = 0;
#else
	throw_no_stitching();
#endif
}
cv::PlaneWarper* cvePlaneWarperCreate(cv::WarperCreator** warperCreator)
{
#ifdef HAVE_OPENCV_STITCHING
	cv::PlaneWarper* ptr = new cv::PlaneWarper();
	*warperCreator = dynamic_cast<cv::WarperCreator*>(ptr);
	return ptr;
#else
	throw_no_stitching();
#endif
}
void cvePlaneWarperRelease(cv::PlaneWarper** warper)
{
#ifdef HAVE_OPENCV_STITCHING
	delete* warper;
	*warper = 0;
#else
	throw_no_stitching();
#endif
}

cv::detail::CylindricalWarper* cveDetailCylindricalWarperCreate(float scale, cv::detail::RotationWarper** rotationWarper)
{
#ifdef HAVE_OPENCV_STITCHING
	cv::detail::CylindricalWarper* ptr = new cv::detail::CylindricalWarper(scale);	
	*rotationWarper = dynamic_cast<cv::detail::RotationWarper*>(ptr);
	return ptr;
#else
	throw_no_stitching();
#endif
}
void cveDetailCylindricalWarperRelease(cv::detail::CylindricalWarper** warper)
{
#ifdef HAVE_OPENCV_STITCHING
	delete* warper;
	*warper = 0;
#else
	throw_no_stitching();
#endif
}
cv::CylindricalWarper* cveCylindricalWarperCreate(cv::WarperCreator** warperCreator)
{
#ifdef HAVE_OPENCV_STITCHING
	cv::CylindricalWarper* ptr = new cv::CylindricalWarper();
	*warperCreator = dynamic_cast<cv::WarperCreator*>(ptr);
	return ptr;
#else
	throw_no_stitching();
#endif
}
void cveCylindricalWarperRelease(cv::CylindricalWarper** warper)
{
#ifdef HAVE_OPENCV_STITCHING
	delete* warper;
	*warper = 0;
#else
	throw_no_stitching();
#endif
}

cv::detail::SphericalWarper* cveDetailSphericalWarperCreate(float scale, cv::detail::RotationWarper** rotationWarper)
{
#ifdef HAVE_OPENCV_STITCHING
	cv::detail::SphericalWarper* ptr = new cv::detail::SphericalWarper(scale);
	*rotationWarper = dynamic_cast<cv::detail::RotationWarper*>(ptr);
	return ptr;
#else
	throw_no_stitching();
#endif
}
void cveDetailSphericalWarperRelease(cv::detail::SphericalWarper** warper)
{
#ifdef HAVE_OPENCV_STITCHING
	delete* warper;
	*warper = 0;
#else
	throw_no_stitching();
#endif
}

cv::SphericalWarper* cveSphericalWarperCreate(cv::WarperCreator** warperCreator)
{
#ifdef HAVE_OPENCV_STITCHING
	cv::SphericalWarper* ptr = new cv::SphericalWarper();
	*warperCreator = dynamic_cast<cv::WarperCreator*>(ptr);
	return ptr;
#else
	throw_no_stitching();
#endif
}
void cveSphericalWarperRelease(cv::SphericalWarper** warperCreator)
{
#ifdef HAVE_OPENCV_STITCHING
	delete* warperCreator;
	*warperCreator = 0;
#else
	throw_no_stitching();
#endif
}

cv::detail::FisheyeWarper* cveDetailFisheyeWarperCreate(float scale, cv::detail::RotationWarper** rotationWarper)
{
#ifdef HAVE_OPENCV_STITCHING
	cv::detail::FisheyeWarper* ptr = new cv::detail::FisheyeWarper(scale);
	*rotationWarper = dynamic_cast<cv::detail::RotationWarper*>(ptr);
	return ptr;
#else
	throw_no_stitching();
#endif
}
void cveDetailFisheyeWarperRelease(cv::detail::FisheyeWarper** warper)
{
#ifdef HAVE_OPENCV_STITCHING
	delete* warper;
	*warper = 0;
#else
	throw_no_stitching();
#endif
}
cv::FisheyeWarper* cveFisheyeWarperCreate(cv::WarperCreator** warperCreator)
{
#ifdef HAVE_OPENCV_STITCHING
	cv::FisheyeWarper* ptr = new cv::FisheyeWarper();
	*warperCreator = dynamic_cast<cv::WarperCreator*>(ptr);
	return ptr;
#else
	throw_no_stitching();
#endif	
}
void cveFisheyeWarperRelease(cv::FisheyeWarper** warperCreator)
{
#ifdef HAVE_OPENCV_STITCHING
	delete* warperCreator;
	*warperCreator = 0;
#else
	throw_no_stitching();
#endif	
}

cv::detail::StereographicWarper* cveDetailStereographicWarperCreate(float scale, cv::detail::RotationWarper** rotationWarper)
{
#ifdef HAVE_OPENCV_STITCHING
	cv::detail::StereographicWarper* ptr = new cv::detail::StereographicWarper(scale);
	*rotationWarper = dynamic_cast<cv::detail::RotationWarper*>(ptr);
	return ptr;
#else
	throw_no_stitching();
#endif
}
void cveDetailStereographicWarperRelease(cv::detail::StereographicWarper** warper)
{
#ifdef HAVE_OPENCV_STITCHING
	delete* warper;
	*warper = 0;
#else
	throw_no_stitching();
#endif
}
cv::StereographicWarper* cveStereographicWarperCreate(cv::WarperCreator** warperCreator)
{
#ifdef HAVE_OPENCV_STITCHING
	cv::StereographicWarper* ptr = new cv::StereographicWarper();
	*warperCreator = dynamic_cast<cv::WarperCreator*>(ptr);
	return ptr;
#else
	throw_no_stitching();
#endif	
}
void cveStereographicWarperRelease(cv::StereographicWarper** warperCreator)
{
#ifdef HAVE_OPENCV_STITCHING
	delete* warperCreator;
	*warperCreator = 0;
#else
	throw_no_stitching();
#endif	
}

cv::detail::CompressedRectilinearWarper* cveDetailCompressedRectilinearWarperCreate(float scale, cv::detail::RotationWarper** rotationWarper)
{
#ifdef HAVE_OPENCV_STITCHING
	cv::detail::CompressedRectilinearWarper* ptr = new cv::detail::CompressedRectilinearWarper(scale);
	*rotationWarper = dynamic_cast<cv::detail::RotationWarper*>(ptr);
	return ptr;
#else
	throw_no_stitching();
#endif
}
void cveDetailCompressedRectilinearWarperRelease(cv::detail::CompressedRectilinearWarper** warper)
{
#ifdef HAVE_OPENCV_STITCHING
	delete* warper;
	*warper = 0;
#else
	throw_no_stitching();
#endif
}
cv::CompressedRectilinearWarper* cveCompressedRectilinearWarperCreate(cv::WarperCreator** warperCreator)
{
#ifdef HAVE_OPENCV_STITCHING
	cv::CompressedRectilinearWarper* ptr = new cv::CompressedRectilinearWarper();
	*warperCreator = dynamic_cast<cv::WarperCreator*>(ptr);
	return ptr;
#else
	throw_no_stitching();
#endif
}
void cveCompressedRectilinearWarperRelease(cv::CompressedRectilinearWarper** warperCreator)
{
#ifdef HAVE_OPENCV_STITCHING
	delete* warperCreator;
	*warperCreator = 0;
#else
	throw_no_stitching();
#endif	
}

cv::detail::PaniniWarper* cveDetailPaniniWarperCreate(float scale, cv::detail::RotationWarper** rotationWarper)
{
#ifdef HAVE_OPENCV_STITCHING
	cv::detail::PaniniWarper* ptr = new cv::detail::PaniniWarper(scale);
	*rotationWarper = dynamic_cast<cv::detail::RotationWarper*>(ptr);
	return ptr;
#else
	throw_no_stitching();
#endif
}
void cveDetailPaniniWarperRelease(cv::detail::PaniniWarper** warper)
{
#ifdef HAVE_OPENCV_STITCHING
	delete* warper;
	*warper = 0;
#else
	throw_no_stitching();
#endif
}
cv::PaniniWarper* cvePaniniWarperCreate(cv::WarperCreator** warperCreator)
{
#ifdef HAVE_OPENCV_STITCHING
	cv::PaniniWarper* ptr = new cv::PaniniWarper();
	*warperCreator = dynamic_cast<cv::WarperCreator*>(ptr);
	return ptr;
#else
	throw_no_stitching();
#endif
}
void cvePaniniWarperRelease(cv::PaniniWarper** warperCreator)
{
#ifdef HAVE_OPENCV_STITCHING
	delete* warperCreator;
	*warperCreator = 0;
#else
	throw_no_stitching();
#endif	
}

cv::detail::PaniniPortraitWarper* cveDetailPaniniPortraitWarperCreate(float scale, cv::detail::RotationWarper** rotationWarper)
{
#ifdef HAVE_OPENCV_STITCHING
	cv::detail::PaniniPortraitWarper* ptr = new cv::detail::PaniniPortraitWarper(scale);
	*rotationWarper = dynamic_cast<cv::detail::RotationWarper*>(ptr);
	return ptr;
#else
	throw_no_stitching();
#endif
}
void cveDetailPaniniPortraitWarperRelease(cv::detail::PaniniPortraitWarper** warper)
{
#ifdef HAVE_OPENCV_STITCHING
	delete* warper;
	*warper = 0;
#else
	throw_no_stitching();
#endif
}
cv::PaniniPortraitWarper* cvePaniniPortraitWarperCreate(cv::WarperCreator** warperCreator)
{
#ifdef HAVE_OPENCV_STITCHING
	cv::PaniniPortraitWarper* ptr = new cv::PaniniPortraitWarper();
	*warperCreator = dynamic_cast<cv::WarperCreator*>(ptr);
	return ptr;
#else
	throw_no_stitching();
#endif
}
void cvePaniniPortraitWarperRelease(cv::PaniniPortraitWarper** warperCreator)
{
#ifdef HAVE_OPENCV_STITCHING
	delete* warperCreator;
	*warperCreator = 0;
#else
	throw_no_stitching();
#endif	
}

cv::detail::MercatorWarper* cveDetailMercatorWarperCreate(float scale, cv::detail::RotationWarper** rotationWarper)
{
#ifdef HAVE_OPENCV_STITCHING
	cv::detail::MercatorWarper* ptr = new cv::detail::MercatorWarper(scale);
	*rotationWarper = dynamic_cast<cv::detail::RotationWarper*>(ptr);
	return ptr;
#else
	throw_no_stitching();
#endif
}
void cveDetailMercatorWarperRelease(cv::detail::MercatorWarper** warper)
{
#ifdef HAVE_OPENCV_STITCHING
	delete* warper;
	*warper = 0;
#else
	throw_no_stitching();
#endif
}
cv::MercatorWarper* cveMercatorWarperCreate(cv::WarperCreator** warperCreator)
{
#ifdef HAVE_OPENCV_STITCHING
	cv::MercatorWarper* ptr = new cv::MercatorWarper();
	*warperCreator = dynamic_cast<cv::WarperCreator*>(ptr);
	return ptr;
#else
	throw_no_stitching();
#endif
}
void cveMercatorWarperRelease(cv::MercatorWarper** warperCreator)
{
#ifdef HAVE_OPENCV_STITCHING
	delete* warperCreator;
	*warperCreator = 0;
#else
	throw_no_stitching();
#endif	
}

cv::detail::TransverseMercatorWarper* cveDetailTransverseMercatorWarperCreate(float scale, cv::detail::RotationWarper** rotationWarper)
{
#ifdef HAVE_OPENCV_STITCHING
	cv::detail::TransverseMercatorWarper* ptr = new cv::detail::TransverseMercatorWarper(scale);
	*rotationWarper = dynamic_cast<cv::detail::RotationWarper*>(ptr);
	return ptr;
#else
	throw_no_stitching();
#endif
}
void cveDetailTransverseMercatorWarperRelease(cv::detail::TransverseMercatorWarper** warper)
{
#ifdef HAVE_OPENCV_STITCHING
	delete* warper;
	*warper = 0;
#else
	throw_no_stitching();
#endif
}
cv::TransverseMercatorWarper* cveTransverseMercatorWarperCreate(cv::WarperCreator** warperCreator)
{
#ifdef HAVE_OPENCV_STITCHING
	cv::TransverseMercatorWarper * ptr = new cv::TransverseMercatorWarper();
	*warperCreator = dynamic_cast<cv::WarperCreator*>(ptr);
	return ptr;
#else
	throw_no_stitching();
#endif
}
void cveTransverseMercatorWarperRelease(cv::TransverseMercatorWarper** warperCreator)
{
#ifdef HAVE_OPENCV_STITCHING
	delete* warperCreator;
	*warperCreator = 0;
#else
	throw_no_stitching();
#endif	
}

cv::detail::FeatherBlender* cveFeatherBlenderCreate(float sharpness, cv::detail::Blender** blender)
{
#ifdef HAVE_OPENCV_STITCHING
	cv::detail::FeatherBlender* ptr = new cv::detail::FeatherBlender(sharpness);
	*blender = dynamic_cast<cv::detail::Blender*>(ptr);
	return ptr;
#else
	throw_no_stitching();
#endif
}
void cveFeatherBlenderRelease(cv::detail::FeatherBlender** blender)
{
#ifdef HAVE_OPENCV_STITCHING
	delete* blender;
	*blender = 0;
#else
	throw_no_stitching();
#endif
}

cv::detail::MultiBandBlender* cveMultiBandBlenderCreate(int tryGpu, int numBands, int weightType, cv::detail::Blender** blender)
{
#ifdef HAVE_OPENCV_STITCHING
	cv::detail::MultiBandBlender* ptr = new cv::detail::MultiBandBlender(tryGpu, numBands, weightType);
	*blender = dynamic_cast<cv::detail::Blender*>(ptr);
	return ptr;
#else
	throw_no_stitching();
#endif
}
void cveMultiBandBlenderRelease(cv::detail::MultiBandBlender** blender)
{
#ifdef HAVE_OPENCV_STITCHING
	delete* blender;
	*blender = 0;
#else
	throw_no_stitching();
#endif
}

cv::detail::NoExposureCompensator* cveNoExposureCompensatorCreate(cv::detail::ExposureCompensator** exposureCompensatorPtr)
{
#ifdef HAVE_OPENCV_STITCHING
	cv::detail::NoExposureCompensator* ptr = new cv::detail::NoExposureCompensator();
	*exposureCompensatorPtr = dynamic_cast<cv::detail::ExposureCompensator*>(ptr);
	return ptr;
#else
	throw_no_stitching();
#endif
}
void cveNoExposureCompensatorRelease(cv::detail::NoExposureCompensator** compensator)
{
#ifdef HAVE_OPENCV_STITCHING
	delete* compensator;
	*compensator = 0;
#else
	throw_no_stitching();
#endif
}

cv::detail::GainCompensator* cveGainCompensatorCreate(int nrFeeds, cv::detail::ExposureCompensator** exposureCompensatorPtr)
{
#ifdef HAVE_OPENCV_STITCHING
	cv::detail::GainCompensator* ptr = new cv::detail::GainCompensator(nrFeeds);
	*exposureCompensatorPtr = dynamic_cast<cv::detail::ExposureCompensator*>(ptr);
	return ptr;
#else
	throw_no_stitching();
#endif
}
void cveGainCompensatorRelease(cv::detail::GainCompensator** compensator)
{
#ifdef HAVE_OPENCV_STITCHING
	delete* compensator;
	*compensator = 0;
#else
	throw_no_stitching();
#endif
}

cv::detail::ChannelsCompensator* cveChannelsCompensatorCreate(int nrFeeds, cv::detail::ExposureCompensator** exposureCompensatorPtr)
{
#ifdef HAVE_OPENCV_STITCHING
	cv::detail::ChannelsCompensator* ptr = new cv::detail::ChannelsCompensator(nrFeeds);
	*exposureCompensatorPtr = dynamic_cast<cv::detail::ExposureCompensator*>(ptr);
	return ptr;
#else
	throw_no_stitching();
#endif
}
void cveChannelsCompensatorRelease(cv::detail::ChannelsCompensator** compensator)
{
#ifdef HAVE_OPENCV_STITCHING
	delete* compensator;
	*compensator = 0;
#else
	throw_no_stitching();
#endif
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
#ifdef HAVE_OPENCV_STITCHING
	cv::detail::BlocksGainCompensator* ptr = new cv::detail::BlocksGainCompensator(blWidth, blHeight, nrFeeds);
	*exposureCompensatorPtr = dynamic_cast<cv::detail::ExposureCompensator*>(ptr);
	return ptr;
#else
	throw_no_stitching();
#endif
}
void cveBlocksGainCompensatorRelease(cv::detail::BlocksGainCompensator** compensator)
{
#ifdef HAVE_OPENCV_STITCHING
	delete* compensator;
	*compensator = 0;
#else
	throw_no_stitching();
#endif
}

cv::detail::BlocksChannelsCompensator* cveBlocksChannelsCompensatorCreate(int blWidth, int blHeight, int nrFeeds, cv::detail::ExposureCompensator** exposureCompensatorPtr)
{
#ifdef HAVE_OPENCV_STITCHING
	cv::detail::BlocksChannelsCompensator* ptr = new cv::detail::BlocksChannelsCompensator(blWidth, blHeight, nrFeeds);
	*exposureCompensatorPtr = dynamic_cast<cv::detail::ExposureCompensator*>(ptr);
	return ptr;
#else
	throw_no_stitching();
#endif
}
void cveBlocksChannelsCompensatorRelease(cv::detail::BlocksChannelsCompensator** compensator)
{
#ifdef HAVE_OPENCV_STITCHING
	delete* compensator;
	*compensator = 0;
#else
	throw_no_stitching();
#endif
}

cv::detail::NoBundleAdjuster* cveNoBundleAdjusterCreate(cv::detail::BundleAdjusterBase** bundleAdjusterBasePtr)
{
#ifdef HAVE_OPENCV_STITCHING
	cv::detail::NoBundleAdjuster* ptr = new cv::detail::NoBundleAdjuster();
	*bundleAdjusterBasePtr = dynamic_cast<cv::detail::BundleAdjusterBase*>(ptr);
	return ptr;
#else
	throw_no_stitching();
#endif
}
void cveNoBundleAdjusterRelease(cv::detail::NoBundleAdjuster** bundleAdjuster)
{
#ifdef HAVE_OPENCV_STITCHING
	delete* bundleAdjuster;
	*bundleAdjuster = 0;
#else
	throw_no_stitching();
#endif
}

cv::detail::BundleAdjusterReproj* cveBundleAdjusterReprojCreate(cv::detail::BundleAdjusterBase** bundleAdjusterBasePtr)
{
#ifdef HAVE_OPENCV_STITCHING
	cv::detail::BundleAdjusterReproj* ptr = new cv::detail::BundleAdjusterReproj();
	*bundleAdjusterBasePtr = dynamic_cast<cv::detail::BundleAdjusterBase*>(ptr);
	return ptr;
#else
	throw_no_stitching();
#endif
}
void cveBundleAdjusterReprojRelease(cv::detail::BundleAdjusterReproj** bundleAdjuster)
{
#ifdef HAVE_OPENCV_STITCHING
	delete* bundleAdjuster;
	*bundleAdjuster = 0;
#else
	throw_no_stitching();
#endif
}

cv::detail::BundleAdjusterRay* cveBundleAdjusterRayCreate(cv::detail::BundleAdjusterBase** bundleAdjusterBasePtr)
{
#ifdef HAVE_OPENCV_STITCHING
	cv::detail::BundleAdjusterRay* ptr = new cv::detail::BundleAdjusterRay();
	*bundleAdjusterBasePtr = dynamic_cast<cv::detail::BundleAdjusterBase*>(ptr);
	return ptr;
#else
	throw_no_stitching();
#endif
}
void cveBundleAdjusterRayRelease(cv::detail::BundleAdjusterRay** bundleAdjuster)
{
#ifdef HAVE_OPENCV_STITCHING
	delete* bundleAdjuster;
	*bundleAdjuster = 0;
#else
	throw_no_stitching();
#endif
}

cv::detail::BundleAdjusterAffine* cveBundleAdjusterAffineCreate(cv::detail::BundleAdjusterBase** bundleAdjusterBasePtr)
{
#ifdef HAVE_OPENCV_STITCHING
	cv::detail::BundleAdjusterAffine* ptr = new cv::detail::BundleAdjusterAffine();
	*bundleAdjusterBasePtr = dynamic_cast<cv::detail::BundleAdjusterBase*>(ptr);
	return ptr;
#else
	throw_no_stitching();
#endif
}
void cveBundleAdjusterAffineRelease(cv::detail::BundleAdjusterAffine** bundleAdjuster)
{
#ifdef HAVE_OPENCV_STITCHING
	delete* bundleAdjuster;
	*bundleAdjuster = 0;
#else
	throw_no_stitching();
#endif
}

cv::detail::BundleAdjusterAffinePartial* cveBundleAdjusterAffinePartialCreate(cv::detail::BundleAdjusterBase** bundleAdjusterBasePtr)
{
#ifdef HAVE_OPENCV_STITCHING
	cv::detail::BundleAdjusterAffinePartial* ptr = new cv::detail::BundleAdjusterAffinePartial();
	*bundleAdjusterBasePtr = dynamic_cast<cv::detail::BundleAdjusterBase*>(ptr);
	return ptr;
#else
	throw_no_stitching();
#endif
}
void cveBundleAdjusterAffinePartialRelease(cv::detail::BundleAdjusterAffinePartial** bundleAdjuster)
{
#ifdef HAVE_OPENCV_STITCHING
	delete* bundleAdjuster;
	*bundleAdjuster = 0;
#else
	throw_no_stitching();
#endif
}

cv::detail::NoSeamFinder* cveNoSeamFinderCreate(cv::detail::SeamFinder** seamFinderPtr)
{
#ifdef HAVE_OPENCV_STITCHING
	cv::detail::NoSeamFinder* ptr = new cv::detail::NoSeamFinder();
	*seamFinderPtr = dynamic_cast<cv::detail::SeamFinder*>(ptr);
	return ptr;
#else
	throw_no_stitching();
#endif
}
void cveNoSeamFinderRelease(cv::detail::NoSeamFinder** seamFinder)
{
#ifdef HAVE_OPENCV_STITCHING
	delete* seamFinder;
	*seamFinder = 0;
#else
	throw_no_stitching();
#endif
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
#ifdef HAVE_OPENCV_STITCHING
	cv::detail::VoronoiSeamFinder* ptr = new cv::detail::VoronoiSeamFinder();
	*seamFinderPtr = dynamic_cast<cv::detail::SeamFinder*>(ptr);
	return ptr;
#else
	throw_no_stitching();
#endif
}
void cveVoronoiSeamFinderRelease(cv::detail::VoronoiSeamFinder** seamFinder)
{
#ifdef HAVE_OPENCV_STITCHING
	delete* seamFinder;
	*seamFinder = 0;
#else
	throw_no_stitching();
#endif
}

cv::detail::DpSeamFinder* cveDpSeamFinderCreate(int costFunc, cv::detail::SeamFinder** seamFinderPtr)
{
#ifdef HAVE_OPENCV_STITCHING
	cv::detail::DpSeamFinder* ptr = new cv::detail::DpSeamFinder(static_cast<cv::detail::DpSeamFinder::CostFunction>(costFunc));
	*seamFinderPtr = dynamic_cast<cv::detail::SeamFinder*>(ptr);
	return ptr;
#else
	throw_no_stitching();
#endif
}
void cveDpSeamFinderRelease(cv::detail::DpSeamFinder** seamFinder)
{
#ifdef HAVE_OPENCV_STITCHING
	delete* seamFinder;
	*seamFinder = 0;
#else
	throw_no_stitching();
#endif
}

cv::detail::GraphCutSeamFinder* cveGraphCutSeamFinderCreate(
	int costType,
	float terminalCost,
	float badRegionPenalty,
	cv::detail::SeamFinder** seamFinderPtr)
{
#ifdef HAVE_OPENCV_STITCHING
	cv::detail::GraphCutSeamFinder* ptr = new cv::detail::GraphCutSeamFinder(
		costType, terminalCost, badRegionPenalty );
	*seamFinderPtr = dynamic_cast<cv::detail::SeamFinder*>(ptr);
	return ptr;
#else
	throw_no_stitching();
#endif
}
void cveGraphCutSeamFinderRelease(cv::detail::GraphCutSeamFinder** seamFinder)
{
#ifdef HAVE_OPENCV_STITCHING
	delete* seamFinder;
	*seamFinder = 0;
#else
	throw_no_stitching();
#endif
}

cv::detail::HomographyBasedEstimator* cveHomographyBasedEstimatorCreate(bool isFocalsEstimated, cv::detail::Estimator** estimatorPtr)
{
#ifdef HAVE_OPENCV_STITCHING
	cv::detail::HomographyBasedEstimator* ptr = new cv::detail::HomographyBasedEstimator(isFocalsEstimated);
	*estimatorPtr = dynamic_cast<cv::detail::Estimator*>(ptr);
	return ptr;
#else
	throw_no_stitching();
#endif
}
void cveHomographyBasedEstimatorRelease(cv::detail::HomographyBasedEstimator** estimator)
{
#ifdef HAVE_OPENCV_STITCHING
	delete* estimator;
	*estimator = 0;
#else
	throw_no_stitching();
#endif
}

cv::detail::AffineBasedEstimator* cveAffineBasedEstimatorCreate(cv::detail::Estimator** estimatorPtr)
{
#ifdef HAVE_OPENCV_STITCHING
	cv::detail::AffineBasedEstimator* ptr = new cv::detail::AffineBasedEstimator();
	*estimatorPtr = dynamic_cast<cv::detail::Estimator*>(ptr);
	return ptr;
#else
	throw_no_stitching();
#endif
}
void cveAffineBasedEstimatorRelease(cv::detail::AffineBasedEstimator** estimator)
{
#ifdef HAVE_OPENCV_STITCHING
	delete* estimator;
	*estimator = 0;
#else
	throw_no_stitching();
#endif
}

cv::detail::BestOf2NearestMatcher* cveBestOf2NearestMatcherCreate(
	bool tryUseGpu,
	float matchConf,
	int numMatchesThresh1,
	int numMatchesThresh2,
	cv::detail::FeaturesMatcher** featuresMatcher)
{
#ifdef HAVE_OPENCV_STITCHING
	cv::detail::BestOf2NearestMatcher* ptr = new cv::detail::BestOf2NearestMatcher(tryUseGpu, matchConf, numMatchesThresh1, numMatchesThresh2);
	*featuresMatcher = dynamic_cast<cv::detail::FeaturesMatcher*>(ptr);
	return ptr;
#else
	throw_no_stitching();
#endif
}
void cveBestOf2NearestMatcherRelease(cv::detail::BestOf2NearestMatcher** featuresMatcher)
{
#ifdef HAVE_OPENCV_STITCHING
	delete* featuresMatcher;
	*featuresMatcher = 0;
#else
	throw_no_stitching();
#endif
}

cv::detail::BestOf2NearestRangeMatcher* cveBestOf2NearestRangeMatcherCreate(
	int rangeWidth,
	bool tryUseGpu,
	float matchConf,
	int numMatchesThresh1,
	int numMatchesThresh2,
	cv::detail::FeaturesMatcher** featuresMatcher)
{
#ifdef HAVE_OPENCV_STITCHING
	cv::detail::BestOf2NearestRangeMatcher* ptr = new cv::detail::BestOf2NearestRangeMatcher(
		rangeWidth,
		tryUseGpu, 
		matchConf, 
		numMatchesThresh1, 
		numMatchesThresh2);
	*featuresMatcher = dynamic_cast<cv::detail::FeaturesMatcher*>(ptr);
	return ptr;
#else
	throw_no_stitching();
#endif
}
void cveBestOf2NearestRangeMatcherRelease(cv::detail::BestOf2NearestRangeMatcher** featuresMatcher)
{
#ifdef HAVE_OPENCV_STITCHING
	delete* featuresMatcher;
	*featuresMatcher = 0;
#else
	throw_no_stitching();
#endif
}

cv::detail::AffineBestOf2NearestMatcher* cveAffineBestOf2NearestMatcherCreate(
	bool fullAffine,
	bool tryUseGpu,
	float matchConf,
	int numMatchesThresh1,
	cv::detail::FeaturesMatcher** featuresMatcher)
{
#ifdef HAVE_OPENCV_STITCHING
	cv::detail::AffineBestOf2NearestMatcher* ptr = new cv::detail::AffineBestOf2NearestMatcher(
		fullAffine,
		tryUseGpu,
		matchConf,
		numMatchesThresh1);
	*featuresMatcher = dynamic_cast<cv::detail::FeaturesMatcher*>(ptr);
	return ptr;
#else
	throw_no_stitching();
#endif
}
void cveAffineBestOf2NearestMatcherRelease(cv::detail::AffineBestOf2NearestMatcher** featuresMatcher)
{
#ifdef HAVE_OPENCV_STITCHING
	delete* featuresMatcher;
	*featuresMatcher = 0;
#else
	throw_no_stitching();
#endif
}


cv::detail::PlaneWarperGpu* cveDetailPlaneWarperGpuCreate(float scale, cv::detail::RotationWarper** rotationWarper)
{
#ifdef HAVE_OPENCV_STITCHING
#ifdef HAVE_OPENCV_CUDAWARPING
	cv::detail::PlaneWarperGpu* ptr = new cv::detail::PlaneWarperGpu(scale);
	*rotationWarper = dynamic_cast<cv::detail::RotationWarper*>(ptr);
	return ptr;
#else
	throw_no_cudawarping();
#endif
#else
	throw_no_stitching();
#endif
}
void cveDetailPlaneWarperGpuRelease(cv::detail::PlaneWarperGpu** warper)
{
#ifdef HAVE_OPENCV_STITCHING
#ifdef HAVE_OPENCV_CUDAWARPING
	delete* warper;
	*warper = 0;
#else
	throw_no_cudawarping();
#endif
#else
	throw_no_stitching();
#endif
}
cv::PlaneWarperGpu* cvePlaneWarperGpuCreate(cv::WarperCreator** warperCreator)
{
#ifdef HAVE_OPENCV_STITCHING
#ifdef HAVE_OPENCV_CUDAWARPING
	cv::PlaneWarperGpu* ptr = new cv::PlaneWarperGpu();
	*warperCreator = dynamic_cast<cv::WarperCreator*>(ptr);
	return ptr;
#else
	throw_no_cudawarping();
#endif
#else
	throw_no_stitching();
#endif
}
void cvePlaneWarperGpuRelease(cv::PlaneWarperGpu** warperCreator)
{
#ifdef HAVE_OPENCV_STITCHING
#ifdef HAVE_OPENCV_CUDAWARPING
	delete* warperCreator;
	*warperCreator = 0;
#else
	throw_no_cudawarping();
#endif
#else
	throw_no_stitching();
#endif
}

cv::detail::CylindricalWarperGpu* cveDetailCylindricalWarperGpuCreate(float scale, cv::detail::RotationWarper** rotationWarper)
{
#ifdef HAVE_OPENCV_STITCHING
#ifdef HAVE_OPENCV_CUDAWARPING
	cv::detail::CylindricalWarperGpu* ptr = new cv::detail::CylindricalWarperGpu(scale);
	*rotationWarper = dynamic_cast<cv::detail::RotationWarper*>(ptr);
	return ptr;
#else
	throw_no_cudawarping();
#endif
#else
	throw_no_stitching();
#endif
}
void cveDetailCylindricalWarperGpuRelease(cv::detail::CylindricalWarperGpu** warper)
{
#ifdef HAVE_OPENCV_STITCHING
#ifdef HAVE_OPENCV_CUDAWARPING
	delete* warper;
	*warper = 0;
#else
	throw_no_cudawarping();
#endif
#else
	throw_no_stitching();
#endif
}
cv::CylindricalWarperGpu* cveCylindricalWarperGpuCreate(cv::WarperCreator** warperCreator)
{
#ifdef HAVE_OPENCV_STITCHING
#ifdef HAVE_OPENCV_CUDAWARPING
	cv::CylindricalWarperGpu* ptr = new cv::CylindricalWarperGpu();
	*warperCreator = dynamic_cast<cv::WarperCreator*>(ptr);
	return ptr;
#else
	throw_no_cudawarping();
#endif
#else
	throw_no_stitching();
#endif
}
void cveCylindricalWarperGpuRelease(cv::CylindricalWarperGpu** warperCreator)
{
#ifdef HAVE_OPENCV_STITCHING
#ifdef HAVE_OPENCV_CUDAWARPING
	delete* warperCreator;
	*warperCreator = 0;
#else
	throw_no_cudawarping();
#endif
#else
	throw_no_stitching();
#endif
}

cv::detail::SphericalWarperGpu* cveDetailSphericalWarperGpuCreate(float scale, cv::detail::RotationWarper** rotationWarper)
{
#ifdef HAVE_OPENCV_STITCHING
#ifdef HAVE_OPENCV_CUDAWARPING
	cv::detail::SphericalWarperGpu* ptr = new cv::detail::SphericalWarperGpu(scale);
	*rotationWarper = dynamic_cast<cv::detail::RotationWarper*>(ptr);
	return ptr;
#else
	throw_no_cudawarping();
#endif
#else
	throw_no_stitching();
#endif
}
void cveDetailSphericalWarperGpuRelease(cv::detail::SphericalWarperGpu** warper)
{
#ifdef HAVE_OPENCV_STITCHING
#ifdef HAVE_OPENCV_CUDAWARPING
	delete* warper;
	*warper = 0;
#else
	throw_no_cudawarping();
#endif
#else
	throw_no_stitching();
#endif
}

cv::SphericalWarperGpu* cveSphericalWarperGpuCreate(cv::WarperCreator** warperCreator)
{
#ifdef HAVE_OPENCV_STITCHING
#ifdef HAVE_OPENCV_CUDAWARPING
	cv::SphericalWarperGpu* ptr = new cv::SphericalWarperGpu();
	*warperCreator = dynamic_cast<cv::WarperCreator*>(ptr);
	return ptr;
#else
	throw_no_cudawarping();
#endif
#else
	throw_no_stitching();
#endif
}
void cveSphericalWarperGpuRelease(cv::SphericalWarperGpu** warper)
{
#ifdef HAVE_OPENCV_STITCHING
#ifdef HAVE_OPENCV_CUDAWARPING
	delete* warper;
	*warper = 0;
#else
	throw_no_cudawarping();
#endif
#else
	throw_no_stitching();
#endif	
}