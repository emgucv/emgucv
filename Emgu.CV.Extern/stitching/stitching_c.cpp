//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "stitching_c.h"

cv::Stitcher* cveStitcherCreate(int mode, cv::Ptr<cv::Stitcher>** sharedPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		cv::Ptr<cv::Stitcher> ptr = cv::Stitcher::create(static_cast<cv::Stitcher::Mode>(mode));
		*sharedPtr = new cv::Ptr<cv::Stitcher>(ptr);
		return ptr.get();
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

void cveStitcherRelease(cv::Ptr<cv::Stitcher>** sharedPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		delete* sharedPtr;
		*sharedPtr = 0;
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveStitcherSetFeaturesFinder(cv::Stitcher* stitcher, cv::Feature2D* finder)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		cv::Ptr<cv::Feature2D> p(finder, [](cv::Feature2D*) {});
		stitcher->setFeaturesFinder(p);
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveStitcherSetWarper(cv::Stitcher* stitcher, cv::WarperCreator* creator)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		cv::Ptr<cv::WarperCreator> p(creator, [](cv::WarperCreator*) {});
		stitcher->setWarper(p);
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveStitcherSetBlender(cv::Stitcher* stitcher, cv::detail::Blender* b)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		cv::Ptr<cv::detail::Blender> blender(b, [](cv::detail::Blender*) {});
		stitcher->setBlender(blender);
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveStitcherSetExposureCompensator(cv::Stitcher* stitcher, cv::detail::ExposureCompensator* exposureComp)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		cv::Ptr<cv::detail::ExposureCompensator> p(exposureComp, [](cv::detail::ExposureCompensator*) {});
		stitcher->setExposureCompensator(p);
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveStitcherSetBundleAdjuster(cv::Stitcher* stitcher, cv::detail::BundleAdjusterBase* bundleAdjuster)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		cv::Ptr<cv::detail::BundleAdjusterBase> p(bundleAdjuster, [](cv::detail::BundleAdjusterBase*) {});
		stitcher->setBundleAdjuster(p);
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveStitcherSetSeamFinder(cv::Stitcher* stitcher, cv::detail::SeamFinder* seamFinder)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		cv::Ptr<cv::detail::SeamFinder> p(seamFinder, [](cv::detail::SeamFinder*) {});
		stitcher->setSeamFinder(p);
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveStitcherSetEstimator(cv::Stitcher* stitcher, cv::detail::Estimator* estimator)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		cv::Ptr<cv::detail::Estimator> p(estimator, [](cv::detail::Estimator*) {});
		stitcher->setEstimator(p);
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveStitcherSetFeaturesMatcher(cv::Stitcher* stitcher, cv::detail::FeaturesMatcher* featuresMatcher)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		cv::Ptr<cv::detail::FeaturesMatcher> p(featuresMatcher, [](cv::detail::FeaturesMatcher*) {});
		stitcher->setFeaturesMatcher(p);
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveStitcherSetWaveCorrection(cv::Stitcher* stitcher, bool flag)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		stitcher->setWaveCorrection(flag);
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
bool cveStitcherGetWaveCorrection(cv::Stitcher* stitcher)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		return stitcher->waveCorrection();
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(false)
}
void cveStitcherSetWaveCorrectionKind(cv::Stitcher* stitcher, int kind)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		stitcher->setWaveCorrectKind(static_cast<cv::detail::WaveCorrectKind>(kind));
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
int cveStitcherGetWaveCorrectionKind(cv::Stitcher* stitcher)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		return stitcher->waveCorrectKind();
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveStitcherSetPanoConfidenceThresh(cv::Stitcher* stitcher, double confThresh)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		stitcher->setPanoConfidenceThresh(confThresh);
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
double cveStitcherGetPanoConfidenceThresh(cv::Stitcher* stitcher)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		return stitcher->panoConfidenceThresh();
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveStitcherSetCompositingResol(cv::Stitcher* stitcher, double resolMpx)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		stitcher->setCompositingResol(resolMpx);
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
double cveStitcherGetCompositingResol(cv::Stitcher* stitcher)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		return stitcher->compositingResol();
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveStitcherSetSeamEstimationResol(cv::Stitcher* stitcher, double resolMpx)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		stitcher->setSeamEstimationResol(resolMpx);
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
double cveStitcherGetSeamEstimationResol(cv::Stitcher* stitcher)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		return stitcher->seamEstimationResol();
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveStitcherSetRegistrationResol(cv::Stitcher* stitcher, double resolMpx)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		stitcher->setRegistrationResol(resolMpx);
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
double cveStitcherGetRegistrationResol(cv::Stitcher* stitcher)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		return stitcher->registrationResol();
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

int cveStitcherGetInterpolationFlags(cv::Stitcher* stitcher)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		return stitcher->interpolationFlags();
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveStitcherSetInterpolationFlags(cv::Stitcher* stitcher, int interpFlags)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		stitcher->setInterpolationFlags(static_cast<cv::InterpolationFlags>(interpFlags));
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

int cveStitcherStitch(cv::Stitcher* stitcher, cv::_InputArray* images, cv::_OutputArray* pano)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		return stitcher->stitch(*images, *pano);
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}


int cveStitcherEstimateTransform(cv::Stitcher* stitcher, cv::_InputArray* images, cv::_InputArray* masks)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		return stitcher->estimateTransform(*images, masks ? *masks : (cv::InputArrayOfArrays) cv::noArray());
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

int cveStitcherComposePanorama1(cv::Stitcher* stitcher, cv::_OutputArray* pano)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		return stitcher->composePanorama(*pano);
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

int cveStitcherComposePanorama2(cv::Stitcher* stitcher, cv::_InputArray* images, cv::_OutputArray* pano)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		return stitcher->composePanorama(*images, *pano);
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

void cveStitcherCameras(cv::Stitcher* stitcher, std::vector< cv::detail::CameraParams >* cameraParams)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		* cameraParams = stitcher->cameras();
	#else
		throw_no_stitching();
	#endif	
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveStitcherComponent(cv::Stitcher* stitcher, std::vector< int >* component)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		* component = stitcher->component();
	#else
		throw_no_stitching();
	#endif		
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

int cveStitcherSetTransform(
	cv::Stitcher* stitcher,
	cv::_InputArray* images,
	const std::vector< cv::detail::CameraParams >* cameras,
	const std::vector< int >* component)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		if (component)
			return stitcher->setTransform(*images, *cameras, *component);
		else
			return stitcher->setTransform(*images, *cameras);
	#else
		throw_no_stitching();
	#endif			
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

void cveStitcherGetResultMask(
	cv::Stitcher* stitcher,
	cv::_OutputArray* resultMask)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		cv::UMat m = stitcher->resultMask();
		m.copyTo(*resultMask);
	#else
		throw_no_stitching();
	#endif				
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}


void cveRotationWarperBuildMaps(cv::detail::RotationWarper* warper, cv::Size* srcSize, cv::_InputArray* K, cv::_InputArray* R, cv::_OutputArray* xmap, cv::_OutputArray* ymap, cv::Rect* boundingBox)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		*boundingBox = warper->buildMaps(*srcSize, *K, *R, *xmap, *ymap);
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveRotationWarperWarp(cv::detail::RotationWarper* warper, cv::_InputArray* src, cv::_InputArray* K, cv::_InputArray* R, int interpMode, int borderMode, cv::_OutputArray* dst, cv::Point* corner)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		*corner = warper->warp(*src, *K, *R, interpMode, borderMode, *dst);
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

cv::detail::PlaneWarper* cveDetailPlaneWarperCreate(float scale, cv::detail::RotationWarper** rotationWarper)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		cv::detail::PlaneWarper* ptr = new cv::detail::PlaneWarper(scale);
		*rotationWarper = dynamic_cast<cv::detail::RotationWarper*>(ptr);
		return ptr;
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveDetailPlaneWarperRelease(cv::detail::PlaneWarper** warper)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		delete* warper;
		*warper = 0;
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
cv::PlaneWarper* cvePlaneWarperCreate(cv::WarperCreator** warperCreator)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		cv::PlaneWarper* ptr = new cv::PlaneWarper();
		*warperCreator = dynamic_cast<cv::WarperCreator*>(ptr);
		return ptr;
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cvePlaneWarperRelease(cv::PlaneWarper** warper)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		delete* warper;
		*warper = 0;
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

cv::detail::CylindricalWarper* cveDetailCylindricalWarperCreate(float scale, cv::detail::RotationWarper** rotationWarper)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		cv::detail::CylindricalWarper* ptr = new cv::detail::CylindricalWarper(scale);	
		*rotationWarper = dynamic_cast<cv::detail::RotationWarper*>(ptr);
		return ptr;
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveDetailCylindricalWarperRelease(cv::detail::CylindricalWarper** warper)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		delete* warper;
		*warper = 0;
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
cv::CylindricalWarper* cveCylindricalWarperCreate(cv::WarperCreator** warperCreator)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		cv::CylindricalWarper* ptr = new cv::CylindricalWarper();
		*warperCreator = dynamic_cast<cv::WarperCreator*>(ptr);
		return ptr;
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveCylindricalWarperRelease(cv::CylindricalWarper** warper)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		delete* warper;
		*warper = 0;
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

cv::detail::SphericalWarper* cveDetailSphericalWarperCreate(float scale, cv::detail::RotationWarper** rotationWarper)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		cv::detail::SphericalWarper* ptr = new cv::detail::SphericalWarper(scale);
		*rotationWarper = dynamic_cast<cv::detail::RotationWarper*>(ptr);
		return ptr;
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveDetailSphericalWarperRelease(cv::detail::SphericalWarper** warper)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		delete* warper;
		*warper = 0;
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

cv::SphericalWarper* cveSphericalWarperCreate(cv::WarperCreator** warperCreator)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		cv::SphericalWarper* ptr = new cv::SphericalWarper();
		*warperCreator = dynamic_cast<cv::WarperCreator*>(ptr);
		return ptr;
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveSphericalWarperRelease(cv::SphericalWarper** warperCreator)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		delete* warperCreator;
		*warperCreator = 0;
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

cv::detail::FisheyeWarper* cveDetailFisheyeWarperCreate(float scale, cv::detail::RotationWarper** rotationWarper)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		cv::detail::FisheyeWarper* ptr = new cv::detail::FisheyeWarper(scale);
		*rotationWarper = dynamic_cast<cv::detail::RotationWarper*>(ptr);
		return ptr;
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveDetailFisheyeWarperRelease(cv::detail::FisheyeWarper** warper)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		delete* warper;
		*warper = 0;
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
cv::FisheyeWarper* cveFisheyeWarperCreate(cv::WarperCreator** warperCreator)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		cv::FisheyeWarper* ptr = new cv::FisheyeWarper();
		*warperCreator = dynamic_cast<cv::WarperCreator*>(ptr);
		return ptr;
	#else
		throw_no_stitching();
	#endif	
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveFisheyeWarperRelease(cv::FisheyeWarper** warperCreator)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		delete* warperCreator;
		*warperCreator = 0;
	#else
		throw_no_stitching();
	#endif	
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

cv::detail::StereographicWarper* cveDetailStereographicWarperCreate(float scale, cv::detail::RotationWarper** rotationWarper)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		cv::detail::StereographicWarper* ptr = new cv::detail::StereographicWarper(scale);
		*rotationWarper = dynamic_cast<cv::detail::RotationWarper*>(ptr);
		return ptr;
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveDetailStereographicWarperRelease(cv::detail::StereographicWarper** warper)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		delete* warper;
		*warper = 0;
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
cv::StereographicWarper* cveStereographicWarperCreate(cv::WarperCreator** warperCreator)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		cv::StereographicWarper* ptr = new cv::StereographicWarper();
		*warperCreator = dynamic_cast<cv::WarperCreator*>(ptr);
		return ptr;
	#else
		throw_no_stitching();
	#endif	
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveStereographicWarperRelease(cv::StereographicWarper** warperCreator)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		delete* warperCreator;
		*warperCreator = 0;
	#else
		throw_no_stitching();
	#endif	
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

cv::detail::CompressedRectilinearWarper* cveDetailCompressedRectilinearWarperCreate(float scale, cv::detail::RotationWarper** rotationWarper)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		cv::detail::CompressedRectilinearWarper* ptr = new cv::detail::CompressedRectilinearWarper(scale);
		*rotationWarper = dynamic_cast<cv::detail::RotationWarper*>(ptr);
		return ptr;
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveDetailCompressedRectilinearWarperRelease(cv::detail::CompressedRectilinearWarper** warper)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		delete* warper;
		*warper = 0;
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
cv::CompressedRectilinearWarper* cveCompressedRectilinearWarperCreate(cv::WarperCreator** warperCreator)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		cv::CompressedRectilinearWarper* ptr = new cv::CompressedRectilinearWarper();
		*warperCreator = dynamic_cast<cv::WarperCreator*>(ptr);
		return ptr;
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveCompressedRectilinearWarperRelease(cv::CompressedRectilinearWarper** warperCreator)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		delete* warperCreator;
		*warperCreator = 0;
	#else
		throw_no_stitching();
	#endif	
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

cv::detail::PaniniWarper* cveDetailPaniniWarperCreate(float scale, cv::detail::RotationWarper** rotationWarper)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		cv::detail::PaniniWarper* ptr = new cv::detail::PaniniWarper(scale);
		*rotationWarper = dynamic_cast<cv::detail::RotationWarper*>(ptr);
		return ptr;
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveDetailPaniniWarperRelease(cv::detail::PaniniWarper** warper)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		delete* warper;
		*warper = 0;
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
cv::PaniniWarper* cvePaniniWarperCreate(cv::WarperCreator** warperCreator)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		cv::PaniniWarper* ptr = new cv::PaniniWarper();
		*warperCreator = dynamic_cast<cv::WarperCreator*>(ptr);
		return ptr;
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cvePaniniWarperRelease(cv::PaniniWarper** warperCreator)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		delete* warperCreator;
		*warperCreator = 0;
	#else
		throw_no_stitching();
	#endif	
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

cv::detail::PaniniPortraitWarper* cveDetailPaniniPortraitWarperCreate(float scale, cv::detail::RotationWarper** rotationWarper)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		cv::detail::PaniniPortraitWarper* ptr = new cv::detail::PaniniPortraitWarper(scale);
		*rotationWarper = dynamic_cast<cv::detail::RotationWarper*>(ptr);
		return ptr;
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveDetailPaniniPortraitWarperRelease(cv::detail::PaniniPortraitWarper** warper)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		delete* warper;
		*warper = 0;
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
cv::PaniniPortraitWarper* cvePaniniPortraitWarperCreate(cv::WarperCreator** warperCreator)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		cv::PaniniPortraitWarper* ptr = new cv::PaniniPortraitWarper();
		*warperCreator = dynamic_cast<cv::WarperCreator*>(ptr);
		return ptr;
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cvePaniniPortraitWarperRelease(cv::PaniniPortraitWarper** warperCreator)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		delete* warperCreator;
		*warperCreator = 0;
	#else
		throw_no_stitching();
	#endif	
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

cv::detail::MercatorWarper* cveDetailMercatorWarperCreate(float scale, cv::detail::RotationWarper** rotationWarper)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		cv::detail::MercatorWarper* ptr = new cv::detail::MercatorWarper(scale);
		*rotationWarper = dynamic_cast<cv::detail::RotationWarper*>(ptr);
		return ptr;
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveDetailMercatorWarperRelease(cv::detail::MercatorWarper** warper)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		delete* warper;
		*warper = 0;
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
cv::MercatorWarper* cveMercatorWarperCreate(cv::WarperCreator** warperCreator)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		cv::MercatorWarper* ptr = new cv::MercatorWarper();
		*warperCreator = dynamic_cast<cv::WarperCreator*>(ptr);
		return ptr;
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveMercatorWarperRelease(cv::MercatorWarper** warperCreator)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		delete* warperCreator;
		*warperCreator = 0;
	#else
		throw_no_stitching();
	#endif	
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

cv::detail::TransverseMercatorWarper* cveDetailTransverseMercatorWarperCreate(float scale, cv::detail::RotationWarper** rotationWarper)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		cv::detail::TransverseMercatorWarper* ptr = new cv::detail::TransverseMercatorWarper(scale);
		*rotationWarper = dynamic_cast<cv::detail::RotationWarper*>(ptr);
		return ptr;
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveDetailTransverseMercatorWarperRelease(cv::detail::TransverseMercatorWarper** warper)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		delete* warper;
		*warper = 0;
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
cv::TransverseMercatorWarper* cveTransverseMercatorWarperCreate(cv::WarperCreator** warperCreator)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		cv::TransverseMercatorWarper * ptr = new cv::TransverseMercatorWarper();
		*warperCreator = dynamic_cast<cv::WarperCreator*>(ptr);
		return ptr;
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveTransverseMercatorWarperRelease(cv::TransverseMercatorWarper** warperCreator)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		delete* warperCreator;
		*warperCreator = 0;
	#else
		throw_no_stitching();
	#endif	
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveBlenderPrepare(cv::detail::Blender* blender, std::vector< cv::Point >* corners, const std::vector< cv::Size >* sizes)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		blender->prepare(*corners, *sizes);
	#else
		throw_no_stitching();
	#endif		
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveBlenderPrepare2(cv::detail::Blender* blender, cv::Rect* dstRoi)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		blender->prepare(*dstRoi);
	#else
		throw_no_stitching();
	#endif	
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveBlenderFeed(cv::detail::Blender* blender, cv::_InputArray* img, cv::_InputArray* mask, cv::Point* tl)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		blender->feed(*img, *mask, *tl);
	#else
		throw_no_stitching();
	#endif	
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveBlenderBlend(cv::detail::Blender* blender, cv::_InputOutputArray* dst, cv::_InputOutputArray* dstMask)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		blender->blend(*dst, *dstMask);
	#else
		throw_no_stitching();
	#endif	
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}


cv::detail::FeatherBlender* cveFeatherBlenderCreate(float sharpness, cv::detail::Blender** blender)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		cv::detail::FeatherBlender* ptr = new cv::detail::FeatherBlender(sharpness);
		*blender = dynamic_cast<cv::detail::Blender*>(ptr);
		return ptr;
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveFeatherBlenderRelease(cv::detail::FeatherBlender** blender)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		delete* blender;
		*blender = 0;
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

cv::detail::MultiBandBlender* cveMultiBandBlenderCreate(int tryGpu, int numBands, int weightType, cv::detail::Blender** blender)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		cv::detail::MultiBandBlender* ptr = new cv::detail::MultiBandBlender(tryGpu, numBands, weightType);
		*blender = dynamic_cast<cv::detail::Blender*>(ptr);
		return ptr;
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveMultiBandBlenderRelease(cv::detail::MultiBandBlender** blender)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		delete* blender;
		*blender = 0;
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

cv::detail::NoExposureCompensator* cveNoExposureCompensatorCreate(cv::detail::ExposureCompensator** exposureCompensatorPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		cv::detail::NoExposureCompensator* ptr = new cv::detail::NoExposureCompensator();
		*exposureCompensatorPtr = dynamic_cast<cv::detail::ExposureCompensator*>(ptr);
		return ptr;
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveNoExposureCompensatorRelease(cv::detail::NoExposureCompensator** compensator)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		delete* compensator;
		*compensator = 0;
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

cv::detail::GainCompensator* cveGainCompensatorCreate(int nrFeeds, cv::detail::ExposureCompensator** exposureCompensatorPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		cv::detail::GainCompensator* ptr = new cv::detail::GainCompensator(nrFeeds);
		*exposureCompensatorPtr = dynamic_cast<cv::detail::ExposureCompensator*>(ptr);
		return ptr;
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveGainCompensatorRelease(cv::detail::GainCompensator** compensator)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		delete* compensator;
		*compensator = 0;
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

cv::detail::ChannelsCompensator* cveChannelsCompensatorCreate(int nrFeeds, cv::detail::ExposureCompensator** exposureCompensatorPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		cv::detail::ChannelsCompensator* ptr = new cv::detail::ChannelsCompensator(nrFeeds);
		*exposureCompensatorPtr = dynamic_cast<cv::detail::ExposureCompensator*>(ptr);
		return ptr;
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveChannelsCompensatorRelease(cv::detail::ChannelsCompensator** compensator)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		delete* compensator;
		*compensator = 0;
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
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
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		cv::detail::BlocksGainCompensator* ptr = new cv::detail::BlocksGainCompensator(blWidth, blHeight, nrFeeds);
		*exposureCompensatorPtr = dynamic_cast<cv::detail::ExposureCompensator*>(ptr);
		return ptr;
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveBlocksGainCompensatorRelease(cv::detail::BlocksGainCompensator** compensator)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		delete* compensator;
		*compensator = 0;
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

cv::detail::BlocksChannelsCompensator* cveBlocksChannelsCompensatorCreate(int blWidth, int blHeight, int nrFeeds, cv::detail::ExposureCompensator** exposureCompensatorPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		cv::detail::BlocksChannelsCompensator* ptr = new cv::detail::BlocksChannelsCompensator(blWidth, blHeight, nrFeeds);
		*exposureCompensatorPtr = dynamic_cast<cv::detail::ExposureCompensator*>(ptr);
		return ptr;
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveBlocksChannelsCompensatorRelease(cv::detail::BlocksChannelsCompensator** compensator)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		delete* compensator;
		*compensator = 0;
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

cv::detail::NoBundleAdjuster* cveNoBundleAdjusterCreate(cv::detail::BundleAdjusterBase** bundleAdjusterBasePtr)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		cv::detail::NoBundleAdjuster* ptr = new cv::detail::NoBundleAdjuster();
		*bundleAdjusterBasePtr = dynamic_cast<cv::detail::BundleAdjusterBase*>(ptr);
		return ptr;
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveNoBundleAdjusterRelease(cv::detail::NoBundleAdjuster** bundleAdjuster)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		delete* bundleAdjuster;
		*bundleAdjuster = 0;
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

cv::detail::BundleAdjusterReproj* cveBundleAdjusterReprojCreate(cv::detail::BundleAdjusterBase** bundleAdjusterBasePtr)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		cv::detail::BundleAdjusterReproj* ptr = new cv::detail::BundleAdjusterReproj();
		*bundleAdjusterBasePtr = dynamic_cast<cv::detail::BundleAdjusterBase*>(ptr);
		return ptr;
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveBundleAdjusterReprojRelease(cv::detail::BundleAdjusterReproj** bundleAdjuster)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		delete* bundleAdjuster;
		*bundleAdjuster = 0;
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

cv::detail::BundleAdjusterRay* cveBundleAdjusterRayCreate(cv::detail::BundleAdjusterBase** bundleAdjusterBasePtr)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		cv::detail::BundleAdjusterRay* ptr = new cv::detail::BundleAdjusterRay();
		*bundleAdjusterBasePtr = dynamic_cast<cv::detail::BundleAdjusterBase*>(ptr);
		return ptr;
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveBundleAdjusterRayRelease(cv::detail::BundleAdjusterRay** bundleAdjuster)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		delete* bundleAdjuster;
		*bundleAdjuster = 0;
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

cv::detail::BundleAdjusterAffine* cveBundleAdjusterAffineCreate(cv::detail::BundleAdjusterBase** bundleAdjusterBasePtr)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		cv::detail::BundleAdjusterAffine* ptr = new cv::detail::BundleAdjusterAffine();
		*bundleAdjusterBasePtr = dynamic_cast<cv::detail::BundleAdjusterBase*>(ptr);
		return ptr;
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveBundleAdjusterAffineRelease(cv::detail::BundleAdjusterAffine** bundleAdjuster)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		delete* bundleAdjuster;
		*bundleAdjuster = 0;
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

cv::detail::BundleAdjusterAffinePartial* cveBundleAdjusterAffinePartialCreate(cv::detail::BundleAdjusterBase** bundleAdjusterBasePtr)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		cv::detail::BundleAdjusterAffinePartial* ptr = new cv::detail::BundleAdjusterAffinePartial();
		*bundleAdjusterBasePtr = dynamic_cast<cv::detail::BundleAdjusterBase*>(ptr);
		return ptr;
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveBundleAdjusterAffinePartialRelease(cv::detail::BundleAdjusterAffinePartial** bundleAdjuster)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		delete* bundleAdjuster;
		*bundleAdjuster = 0;
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

cv::detail::NoSeamFinder* cveNoSeamFinderCreate(cv::detail::SeamFinder** seamFinderPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		cv::detail::NoSeamFinder* ptr = new cv::detail::NoSeamFinder();
		*seamFinderPtr = dynamic_cast<cv::detail::SeamFinder*>(ptr);
		return ptr;
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveNoSeamFinderRelease(cv::detail::NoSeamFinder** seamFinder)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		delete* seamFinder;
		*seamFinder = 0;
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
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
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		cv::detail::VoronoiSeamFinder* ptr = new cv::detail::VoronoiSeamFinder();
		*seamFinderPtr = dynamic_cast<cv::detail::SeamFinder*>(ptr);
		return ptr;
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveVoronoiSeamFinderRelease(cv::detail::VoronoiSeamFinder** seamFinder)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		delete* seamFinder;
		*seamFinder = 0;
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

cv::detail::DpSeamFinder* cveDpSeamFinderCreate(int costFunc, cv::detail::SeamFinder** seamFinderPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		cv::detail::DpSeamFinder* ptr = new cv::detail::DpSeamFinder(static_cast<cv::detail::DpSeamFinder::CostFunction>(costFunc));
		*seamFinderPtr = dynamic_cast<cv::detail::SeamFinder*>(ptr);
		return ptr;
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveDpSeamFinderRelease(cv::detail::DpSeamFinder** seamFinder)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		delete* seamFinder;
		*seamFinder = 0;
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

cv::detail::GraphCutSeamFinder* cveGraphCutSeamFinderCreate(
	int costType,
	float terminalCost,
	float badRegionPenalty,
	cv::detail::SeamFinder** seamFinderPtr)
{
	try
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
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveGraphCutSeamFinderRelease(cv::detail::GraphCutSeamFinder** seamFinder)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		delete* seamFinder;
		*seamFinder = 0;
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

cv::detail::HomographyBasedEstimator* cveHomographyBasedEstimatorCreate(bool isFocalsEstimated, cv::detail::Estimator** estimatorPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		cv::detail::HomographyBasedEstimator* ptr = new cv::detail::HomographyBasedEstimator(isFocalsEstimated);
		*estimatorPtr = dynamic_cast<cv::detail::Estimator*>(ptr);
		return ptr;
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveHomographyBasedEstimatorRelease(cv::detail::HomographyBasedEstimator** estimator)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		delete* estimator;
		*estimator = 0;
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

cv::detail::AffineBasedEstimator* cveAffineBasedEstimatorCreate(cv::detail::Estimator** estimatorPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		cv::detail::AffineBasedEstimator* ptr = new cv::detail::AffineBasedEstimator();
		*estimatorPtr = dynamic_cast<cv::detail::Estimator*>(ptr);
		return ptr;
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveAffineBasedEstimatorRelease(cv::detail::AffineBasedEstimator** estimator)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		delete* estimator;
		*estimator = 0;
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

cv::detail::BestOf2NearestMatcher* cveBestOf2NearestMatcherCreate(
	bool tryUseGpu,
	float matchConf,
	int numMatchesThresh1,
	int numMatchesThresh2,
	cv::detail::FeaturesMatcher** featuresMatcher)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		cv::detail::BestOf2NearestMatcher* ptr = new cv::detail::BestOf2NearestMatcher(tryUseGpu, matchConf, numMatchesThresh1, numMatchesThresh2);
		*featuresMatcher = dynamic_cast<cv::detail::FeaturesMatcher*>(ptr);
		return ptr;
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveBestOf2NearestMatcherRelease(cv::detail::BestOf2NearestMatcher** featuresMatcher)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		delete* featuresMatcher;
		*featuresMatcher = 0;
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

cv::detail::BestOf2NearestRangeMatcher* cveBestOf2NearestRangeMatcherCreate(
	int rangeWidth,
	bool tryUseGpu,
	float matchConf,
	int numMatchesThresh1,
	int numMatchesThresh2,
	cv::detail::FeaturesMatcher** featuresMatcher)
{
	try
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
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveBestOf2NearestRangeMatcherRelease(cv::detail::BestOf2NearestRangeMatcher** featuresMatcher)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		delete* featuresMatcher;
		*featuresMatcher = 0;
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

cv::detail::AffineBestOf2NearestMatcher* cveAffineBestOf2NearestMatcherCreate(
	bool fullAffine,
	bool tryUseGpu,
	float matchConf,
	int numMatchesThresh1,
	cv::detail::FeaturesMatcher** featuresMatcher)
{
	try
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
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveAffineBestOf2NearestMatcherRelease(cv::detail::AffineBestOf2NearestMatcher** featuresMatcher)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		delete* featuresMatcher;
		*featuresMatcher = 0;
	#else
		throw_no_stitching();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}


cv::detail::PlaneWarperGpu* cveDetailPlaneWarperGpuCreate(float scale, cv::detail::RotationWarper** rotationWarper)
{
	try
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
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveDetailPlaneWarperGpuRelease(cv::detail::PlaneWarperGpu** warper)
{
	try
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
	CVAPI_CATCH_CV_ERRORS_VOID
}
cv::PlaneWarperGpu* cvePlaneWarperGpuCreate(cv::WarperCreator** warperCreator)
{
	try
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
	CVAPI_CATCH_CV_ERRORS(0)
}
void cvePlaneWarperGpuRelease(cv::PlaneWarperGpu** warperCreator)
{
	try
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
	CVAPI_CATCH_CV_ERRORS_VOID
}

cv::detail::CylindricalWarperGpu* cveDetailCylindricalWarperGpuCreate(float scale, cv::detail::RotationWarper** rotationWarper)
{
	try
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
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveDetailCylindricalWarperGpuRelease(cv::detail::CylindricalWarperGpu** warper)
{
	try
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
	CVAPI_CATCH_CV_ERRORS_VOID
}
cv::CylindricalWarperGpu* cveCylindricalWarperGpuCreate(cv::WarperCreator** warperCreator)
{
	try
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
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveCylindricalWarperGpuRelease(cv::CylindricalWarperGpu** warperCreator)
{
	try
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
	CVAPI_CATCH_CV_ERRORS_VOID
}

cv::detail::SphericalWarperGpu* cveDetailSphericalWarperGpuCreate(float scale, cv::detail::RotationWarper** rotationWarper)
{
	try
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
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveDetailSphericalWarperGpuRelease(cv::detail::SphericalWarperGpu** warper)
{
	try
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
	CVAPI_CATCH_CV_ERRORS_VOID
}

cv::SphericalWarperGpu* cveSphericalWarperGpuCreate(cv::WarperCreator** warperCreator)
{
	try
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
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveSphericalWarperGpuRelease(cv::SphericalWarperGpu** warper)
{
	try
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
	CVAPI_CATCH_CV_ERRORS_VOID
}


cv::detail::CameraParams* cveCameraParamsCreate()
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		return new cv::detail::CameraParams();
	#else
		throw_no_stitching();
	#endif	
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveCameraParamsRelease(cv::detail::CameraParams** params)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		delete* params;
		*params = 0;
	#else
		throw_no_stitching();
	#endif	
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveCameraParamsGetK(cv::detail::CameraParams* params, cv::_OutputArray* k)
{
	try
	{
	#ifdef HAVE_OPENCV_STITCHING
		cv::Mat m = params->K();
		m.copyTo(*k);
	#else
		throw_no_stitching();
	#endif		
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

