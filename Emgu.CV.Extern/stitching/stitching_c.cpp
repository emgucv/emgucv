//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2017 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "stitching_c.h"

cv::Stitcher* cveStitcherCreateDefault(bool tryUseGpu)
{
   cv::Ptr<cv::Stitcher> p = cv::createStitcher(tryUseGpu);
   p.addref();
   return p.get();
}

cv::Stitcher* cveStitcherCreate(int mode, bool tryUseGpu)
{
	cv::Ptr<cv::Stitcher> ptr = cv::Stitcher::create(static_cast<cv::Stitcher::Mode>(mode), tryUseGpu);
	ptr.addref();
	return ptr.get();
}

void cveStitcherRelease(cv::Stitcher** stitcher)
{
   delete *stitcher;
   *stitcher = 0;
}

void cveStitcherSetFeaturesFinder(cv::Stitcher* stitcher, cv::detail::FeaturesFinder* finder)
{
   cv::Ptr<cv::detail::FeaturesFinder> p(finder);
   p.addref();
   stitcher->setFeaturesFinder(p);
}

void cveStitcherSetWarper(cv::Stitcher* stitcher, cv::WarperCreator* creator)
{
   cv::Ptr<cv::WarperCreator> p(creator);
   p.addref();
   stitcher->setWarper(p);
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

int cveStitcherStitch(cv::Stitcher* stitcher, cv::_InputArray* images, cv::_OutputArray* pano)
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

cv::detail::AKAZEFeaturesFinder* cveAKAZEFeaturesFinderCreate(
	int descriptorType,
	int descriptorSize,
	int descriptorChannels,
	float threshold,
	int nOctaves,
	int nOctaveLayers,
	int diffusivity,
	cv::detail::FeaturesFinder** f)
{
	cv::detail::AKAZEFeaturesFinder* finder = new cv::detail::AKAZEFeaturesFinder(
		descriptorType, descriptorSize, descriptorChannels, threshold, nOctaves, nOctaveLayers, diffusivity
	);
	*f = dynamic_cast<cv::detail::FeaturesFinder*>(finder);
	return finder;
}
void cveAKAZEFeaturesFinderRelease(cv::detail::AKAZEFeaturesFinder** finder)
{
	delete *finder;
	*finder = 0;
}


void cveRotationWarperBuildMaps(cv::detail::RotationWarper* warper, CvSize* srcSize, cv::_InputArray* K, cv::_InputArray* R, cv::_OutputArray* xmap, cv::_OutputArray* ymap, CvRect* boundingBox)
{
   *boundingBox = warper->buildMaps(*srcSize, *K, *R, *xmap, *ymap);
}
void cveRotationWarperWarp(cv::detail::RotationWarper* warper, cv::_InputArray* src, cv::_InputArray* K, cv::_InputArray* R, int interpMode, int borderMode, cv::_OutputArray* dst, CvPoint* corner)
{
   *corner = warper->warp(*src, *K, *R, interpMode, borderMode, *dst);
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
   delete *warper;
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
   delete *warper;
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
   delete *warper;
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
   delete *warper;
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
   delete *warper;
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
   delete *warper;
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
   delete *warper;
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
   delete *warper;
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
   delete *warper;
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
   delete *warper;
   *warper = 0;
}

#ifdef HAVE_OPENCV_CUDAWARPING
cv::detail::PlaneWarperGpu* cvePlaneWarperGpuCreate(float scale, cv::WarperCreator** creator, cv::detail::RotationWarper** rotationWarper)
{
   cv::detail::PlaneWarperGpu* ptr = new cv::detail::PlaneWarperGpu(scale);
   *creator = dynamic_cast<cv::WarperCreator*>(ptr);
   *rotationWarper = dynamic_cast<cv::detail::RotationWarper*>(ptr);
   return ptr;
}
void cvePlaneWarperGpuRelease(cv::detail::PlaneWarperGpu** warper)
{
   delete *warper;
   *warper = 0;
}

cv::detail::CylindricalWarperGpu* cveCylindricalWarperGpuCreate(float scale, cv::WarperCreator** creator, cv::detail::RotationWarper** rotationWarper)
{
   cv::detail::CylindricalWarperGpu* ptr = new cv::detail::CylindricalWarperGpu(scale);
   *creator = dynamic_cast<cv::WarperCreator*>(ptr);
   *rotationWarper = dynamic_cast<cv::detail::RotationWarper*>(ptr);
   return ptr;
}
void cveCylindricalWarperGpuRelease(cv::detail::CylindricalWarperGpu** warper)
{
   delete *warper;
   *warper = 0;
}

cv::detail::SphericalWarperGpu* cveSphericalWarperGpuCreate(float scale, cv::WarperCreator** creator, cv::detail::RotationWarper** rotationWarper)
{
   cv::detail::SphericalWarperGpu* ptr = new cv::detail::SphericalWarperGpu(scale);
   *creator = dynamic_cast<cv::WarperCreator*>(ptr);
   *rotationWarper = dynamic_cast<cv::detail::RotationWarper*>(ptr);
   return ptr;
}
void cveSphericalWarperGpuRelease(cv::detail::SphericalWarperGpu** warper)
{
   delete *warper;
   *warper = 0;
}
#endif
