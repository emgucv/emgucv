//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2019 by EMGU Corporation. All rights reserved.
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
   delete *sharedPtr;
   *sharedPtr = 0;
}

void cveStitcherSetFeaturesFinder(cv::Stitcher* stitcher, cv::Feature2D* finder)
{
   cv::Ptr<cv::Feature2D> p(finder, [](cv::Feature2D*){});
   stitcher->setFeaturesFinder(p);
}

void cveStitcherSetWarper(cv::Stitcher* stitcher, cv::WarperCreator* creator)
{
   cv::Ptr<cv::WarperCreator> p(creator, [](cv::WarperCreator*){});
   stitcher->setWarper(p);
}

void cveStitcherSetBlender(cv::Stitcher* stitcher, cv::detail::Blender* b)
{
	cv::Ptr<cv::detail::Blender> blender(b, [](cv::detail::Blender*){});
	stitcher->setBlender(blender);
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


int cveStitcherEstimateTransform(cv::Stitcher* stitcher, cv::_InputArray* images, cv::_InputArray* masks)
{
	return stitcher->estimateTransform(*images, masks? *masks: (cv::InputArrayOfArrays) cv::noArray());
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
   *boundingBox = cvRect(warper->buildMaps(*srcSize, *K, *R, *xmap, *ymap) );
}
void cveRotationWarperWarp(cv::detail::RotationWarper* warper, cv::_InputArray* src, cv::_InputArray* K, cv::_InputArray* R, int interpMode, int borderMode, cv::_OutputArray* dst, CvPoint* corner)
{
   *corner = cvPoint( warper->warp(*src, *K, *R, interpMode, borderMode, *dst) );
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

cv::detail::FeatherBlender* cveFeatherBlenderCreate(float sharpness, cv::detail::Blender** blender)
{
	cv::detail::FeatherBlender* ptr = new cv::detail::FeatherBlender(sharpness);
	*blender = dynamic_cast<cv::detail::Blender*>(ptr);
	return ptr;
}
void cveFeatherBlenderRelease(cv::detail::FeatherBlender** blender)
{
	delete *blender;
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
	delete *blender;
	*blender = 0;
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
