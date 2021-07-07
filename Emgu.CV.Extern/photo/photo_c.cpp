//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "photo_c.h"

void cveInpaint(cv::_InputArray* src, cv::_InputArray* inpaintMask, cv::_OutputArray* dst, double inpaintRadius, int flags)
{
#ifdef HAVE_OPENCV_PHOTO
	cv::inpaint(*src, *inpaintMask, *dst, inpaintRadius, flags);
#else
	throw_no_photo();
#endif
}
void cveFastNlMeansDenoising(cv::_InputArray* src, cv::_OutputArray* dst, float h, int templateWindowSize, int searchWindowSize)
{
#ifdef HAVE_OPENCV_PHOTO
	cv::fastNlMeansDenoising(*src, *dst, h, templateWindowSize, searchWindowSize);
#else
	throw_no_photo();
#endif
}

void cveFastNlMeansDenoisingColored(cv::_InputArray* src, cv::_OutputArray* dst, float h, float hColor, int templateWindowSize, int searchWindowSize)
{
#ifdef HAVE_OPENCV_PHOTO
	cv::fastNlMeansDenoisingColored(*src, *dst, h, hColor, templateWindowSize, searchWindowSize);
#else
	throw_no_photo();
#endif
}

void cudaNonLocalMeans(const cv::cuda::GpuMat* src, cv::cuda::GpuMat* dst, float h, int searchWindow, int blockSize, int borderMode, cv::cuda::Stream* stream)
{
#ifdef HAVE_OPENCV_PHOTO
	cv::cuda::nonLocalMeans(*src, *dst, h, searchWindow, blockSize, borderMode, stream ? *stream : cv::cuda::Stream::Null());
#else
	throw_no_photo();
#endif
}

void cveEdgePreservingFilter(cv::_InputArray* src, cv::_OutputArray* dst, int flags, float sigmaS, float sigmaR)
{
#ifdef HAVE_OPENCV_PHOTO
	cv::edgePreservingFilter(*src, *dst, flags, sigmaS, sigmaR);
#else
	throw_no_photo();
#endif
}

void cveDetailEnhance(cv::_InputArray* src, cv::_OutputArray* dst, float sigmaS, float sigmaR)
{
#ifdef HAVE_OPENCV_PHOTO
	cv::detailEnhance(*src, *dst, sigmaS, sigmaR);
#else
	throw_no_photo();
#endif
}

void cvePencilSketch(cv::_InputArray* src, cv::_OutputArray* dst1, cv::_OutputArray* dst2, float sigmaS, float sigmaR, float shadeFactor)
{
#ifdef HAVE_OPENCV_PHOTO
	cv::pencilSketch(*src, *dst1, *dst2, sigmaS, sigmaR, shadeFactor);
#else
	throw_no_photo();
#endif
}

void cveStylization(cv::_InputArray* src, cv::_OutputArray* dst, float sigmaS, float sigmaR)
{
#ifdef HAVE_OPENCV_PHOTO
	cv::stylization(*src, *dst, sigmaS, sigmaR);
#else
	throw_no_photo();
#endif
}

void cveColorChange(cv::_InputArray* src, cv::_InputArray* mask, cv::_OutputArray* dst, float redMul, float greenMul, float blueMul)
{
#ifdef HAVE_OPENCV_PHOTO
	cv::colorChange(*src, mask ? *mask : (cv::InputArray) cv::noArray(), dst ? *dst : (cv::OutputArray) cv::noArray(), redMul, greenMul, blueMul);
#else
	throw_no_photo();
#endif
}

void cveIlluminationChange(cv::_InputArray* src, cv::_InputArray* mask, cv::_OutputArray* dst, float alpha, float beta)
{
#ifdef HAVE_OPENCV_PHOTO
	cv::illuminationChange(*src, mask ? *mask : (cv::InputArray) cv::noArray(), *dst, alpha, beta);
#else
	throw_no_photo();
#endif
}

void cveTextureFlattening(cv::_InputArray* src, cv::_InputArray* mask, cv::_OutputArray* dst, float lowThreshold, float highThreshold, int kernelSize)
{
#ifdef HAVE_OPENCV_PHOTO
	cv::textureFlattening(*src, mask ? *mask : (cv::InputArray) cv::noArray(), *dst, lowThreshold, highThreshold, kernelSize);
#else
	throw_no_photo();
#endif
}

void cveDecolor(cv::_InputArray* src, cv::_OutputArray* grayscale, cv::_OutputArray* colorBoost)
{
#ifdef HAVE_OPENCV_PHOTO
	cv::decolor(*src, *grayscale, *colorBoost);
#else
	throw_no_photo();
#endif
}

void cveSeamlessClone(cv::_InputArray* src, cv::_InputArray* dst, cv::_InputArray* mask, CvPoint* p, cv::_OutputArray* blend, int flags)
{
#ifdef HAVE_OPENCV_PHOTO
	cv::seamlessClone(*src, *dst, *mask, *p, *blend, flags);
#else
	throw_no_photo();
#endif
}

void cveDenoiseTVL1(const std::vector< cv::Mat >* observations, cv::Mat* result, double lambda, int niters)
{
#ifdef HAVE_OPENCV_PHOTO
	cv::denoise_TVL1(*observations, *result, lambda, niters);
#else
	throw_no_photo();
#endif
}

void cveCalibrateCRFProcess(cv::CalibrateCRF* calibrateCRF, cv::_InputArray* src, cv::_OutputArray* dst, cv::_InputArray* times)
{
#ifdef HAVE_OPENCV_PHOTO
	calibrateCRF->process(*src, *dst, *times);
#else
	throw_no_photo();
#endif
}

cv::CalibrateDebevec* cveCalibrateDebevecCreate(int samples, float lambda, bool random, cv::CalibrateCRF** calibrateCRF, cv::Ptr<cv::CalibrateDebevec>** sharedPtr)
{
#ifdef HAVE_OPENCV_PHOTO
	cv::Ptr<cv::CalibrateDebevec> res = cv::createCalibrateDebevec(samples, lambda, random);
	*sharedPtr = new cv::Ptr<cv::CalibrateDebevec>(res);
	*calibrateCRF = dynamic_cast<cv::CalibrateCRF*>(res.get());
	return res.get();
#else
	throw_no_photo();
#endif
}
void cveCalibrateDebevecRelease(cv::CalibrateDebevec** calibrateDebevec, cv::Ptr<cv::CalibrateDebevec>** sharedPtr)
{
#ifdef HAVE_OPENCV_PHOTO
	delete *sharedPtr;
	*calibrateDebevec = 0;
	*sharedPtr = 0;
#else
	throw_no_photo();
#endif
}

cv::CalibrateRobertson* cveCalibrateRobertsonCreate(int maxIter, float threshold, cv::CalibrateCRF** calibrateCRF, cv::Ptr<cv::CalibrateRobertson>** sharedPtr)
{
#ifdef HAVE_OPENCV_PHOTO
	cv::Ptr<cv::CalibrateRobertson> res = cv::createCalibrateRobertson(maxIter, threshold);
	*sharedPtr = new cv::Ptr<cv::CalibrateRobertson>(res);
	*calibrateCRF = dynamic_cast<cv::CalibrateCRF*>(res.get());
	return res.get();
#else
	throw_no_photo();
#endif
}
void cveCalibrateRobertsonRelease(cv::CalibrateRobertson** calibrateRobertson, cv::Ptr<cv::CalibrateRobertson>** sharedPtr)
{
#ifdef HAVE_OPENCV_PHOTO
	delete *sharedPtr;
	calibrateRobertson = 0;
	*sharedPtr = 0;
#else
	throw_no_photo();
#endif
}

void cveMergeExposuresProcess(
	cv::MergeExposures* mergeExposures,
	cv::_InputArray* src, cv::_OutputArray* dst,
	cv::_InputArray* times, cv::_InputArray* response)
{
#ifdef HAVE_OPENCV_PHOTO
	mergeExposures->process(*src, *dst, *times, *response);
#else
	throw_no_photo();
#endif
}

cv::MergeDebevec* cveMergeDebevecCreate(cv::MergeExposures** merge, cv::Ptr<cv::MergeDebevec>** sharedPtr)
{
#ifdef HAVE_OPENCV_PHOTO
	cv::Ptr<cv::MergeDebevec> res = cv::createMergeDebevec();
	*sharedPtr = new cv::Ptr<cv::MergeDebevec>(res);
	*merge = dynamic_cast<cv::MergeExposures*>(res.get());
	return res.get();
#else
	throw_no_photo();
#endif
}
void cveMergeDebevecRelease(cv::MergeDebevec** merge, cv::Ptr<cv::MergeDebevec>** sharedPtr)
{
#ifdef HAVE_OPENCV_PHOTO
	delete *sharedPtr;
	*merge = 0;
	*sharedPtr = 0;
#else
	throw_no_photo();
#endif
}

cv::MergeMertens* cveMergeMertensCreate(float contrastWeight, float saturationWeight, float exposureWeight, cv::MergeExposures** merge, cv::Ptr<cv::MergeMertens>** sharedPtr)
{
#ifdef HAVE_OPENCV_PHOTO
	cv::Ptr<cv::MergeMertens> res = cv::createMergeMertens(contrastWeight, saturationWeight, exposureWeight);
	*sharedPtr = new cv::Ptr<cv::MergeMertens>(res);
	*merge = dynamic_cast<cv::MergeExposures*>(res.get());
	return res.get();
#else
	throw_no_photo();
#endif
}

void cveMergeMertensRelease(cv::MergeMertens** merge, cv::Ptr<cv::MergeMertens>** sharedPtr)
{
#ifdef HAVE_OPENCV_PHOTO
	delete *sharedPtr;
	*merge = 0;
	*sharedPtr = 0;
#else
	throw_no_photo();
#endif
}

cv::MergeRobertson* cveMergeRobertsonCreate(cv::MergeExposures** merge, cv::Ptr<cv::MergeRobertson>** sharedPtr)
{
#ifdef HAVE_OPENCV_PHOTO
	cv::Ptr<cv::MergeRobertson> res = cv::createMergeRobertson();
	*sharedPtr = new cv::Ptr<cv::MergeRobertson>(res);
	*merge = dynamic_cast<cv::MergeExposures*>(res.get());
	return res.get();
#else
	throw_no_photo();
#endif
}

void cveMergeRobertsonRelease(cv::MergeRobertson** merge, cv::Ptr<cv::MergeRobertson>** sharedPtr)
{
#ifdef HAVE_OPENCV_PHOTO
	delete *sharedPtr;
	*merge = 0;
	*sharedPtr = 0;
#else
	throw_no_photo();
#endif
}


void cveTonemapProcess(cv::Tonemap* tonemap, cv::_InputArray* src, cv::_OutputArray* dst)
{
#ifdef HAVE_OPENCV_PHOTO
	tonemap->process(*src, *dst);
#else
	throw_no_photo();
#endif
}
cv::Tonemap* cveTonemapCreate(float gamma, cv::Algorithm** algorithm, cv::Ptr<cv::Tonemap>** sharedPtr)
{
#ifdef HAVE_OPENCV_PHOTO
	cv::Ptr<cv::Tonemap> tonemap = cv::createTonemap(gamma);
	*sharedPtr = new cv::Ptr<cv::Tonemap>(tonemap);
	*algorithm = dynamic_cast<cv::Algorithm*>(tonemap.get());
	return tonemap.get();
#else
	throw_no_photo();
#endif
}
void cveTonemapRelease(cv::Tonemap** tonemap, cv::Ptr<cv::Tonemap>** sharedPtr)
{
#ifdef HAVE_OPENCV_PHOTO
	delete *sharedPtr;
	*tonemap = 0;
	*sharedPtr = 0;
#else
	throw_no_photo();
#endif
}

cv::TonemapDrago* cveTonemapDragoCreate(float gamma, float saturation, float bias, cv::Tonemap** tonemap, cv::Algorithm** algorithm, cv::Ptr<cv::TonemapDrago>** sharedPtr)
{
#ifdef HAVE_OPENCV_PHOTO
	cv::Ptr<cv::TonemapDrago> t = cv::createTonemapDrago(gamma, saturation, bias);
	*sharedPtr = new cv::Ptr<cv::TonemapDrago>(t);
	*tonemap = dynamic_cast<cv::Tonemap*>(t.get());
	*algorithm = dynamic_cast<cv::Algorithm*>(t.get());
	return t.get();
#else
	throw_no_photo();
#endif
}
void cveTonemapDragoRelease(cv::TonemapDrago** tonemap, cv::Ptr<cv::TonemapDrago>** sharedPtr)
{
#ifdef HAVE_OPENCV_PHOTO
	delete *sharedPtr;
	*tonemap = 0;
	*sharedPtr = 0;
#else
	throw_no_photo();
#endif
}

cv::TonemapReinhard* cveTonemapReinhardCreate(float gamma, float intensity, float lightAdapt, float colorAdapt, cv::Tonemap** tonemap, cv::Algorithm** algorithm, cv::Ptr<cv::TonemapReinhard>** sharedPtr)
{
#ifdef HAVE_OPENCV_PHOTO
	cv::Ptr<cv::TonemapReinhard> t = cv::createTonemapReinhard(gamma, intensity, lightAdapt, colorAdapt);
	*sharedPtr = new cv::Ptr<cv::TonemapReinhard>(t);
	*tonemap = dynamic_cast<cv::Tonemap*>(t.get());
	*algorithm = dynamic_cast<cv::Algorithm*>(t.get());
	return t.get();
#else
	throw_no_photo();
#endif
}
void cveTonemapReinhardRelease(cv::TonemapReinhard** tonemap, cv::Ptr<cv::TonemapReinhard>** sharedPtr)
{
#ifdef HAVE_OPENCV_PHOTO
	delete *sharedPtr;
	*tonemap = 0;
	*sharedPtr = 0;
#else
	throw_no_photo();
#endif
}

cv::TonemapMantiuk* cveTonemapMantiukCreate(float gamma, float scale, float saturation, cv::Tonemap** tonemap, cv::Algorithm** algorithm, cv::Ptr<cv::TonemapMantiuk>** sharedPtr)
{
#ifdef HAVE_OPENCV_PHOTO
	cv::Ptr<cv::TonemapMantiuk> t = cv::createTonemapMantiuk(gamma, scale, saturation);
	*sharedPtr = new cv::Ptr<cv::TonemapMantiuk>(t);
	*tonemap = dynamic_cast<cv::Tonemap*>(t.get());
	*algorithm = dynamic_cast<cv::Algorithm*>(t.get());
	return t.get();
#else
	throw_no_photo();
#endif
}
void cveTonemapMantiukRelease(cv::TonemapMantiuk** tonemap, cv::Ptr<cv::TonemapMantiuk>** sharedPtr)
{
#ifdef HAVE_OPENCV_PHOTO
	delete *sharedPtr;
	*tonemap = 0;
	*sharedPtr = 0;
#else
	throw_no_photo();
#endif
}

void cveAlignExposuresProcess(cv::AlignExposures* alignExposures, cv::_InputArray* src, std::vector<cv::Mat>* dst, cv::_InputArray* times, cv::_InputArray* response)
{
#ifdef HAVE_OPENCV_PHOTO
	alignExposures->process(*src, *dst, *times, *response);
#else
	throw_no_photo();
#endif
}


cv::AlignMTB* cveAlignMTBCreate(int maxBits, int excludeRange, bool cut, cv::AlignExposures** alignExposures, cv::Ptr<cv::AlignMTB>** sharedPtr)
{
#ifdef HAVE_OPENCV_PHOTO
	cv::Ptr<cv::AlignMTB> a = cv::createAlignMTB(maxBits, excludeRange, cut);
	*sharedPtr = new cv::Ptr<cv::AlignMTB>(a);
	*alignExposures = dynamic_cast<cv::AlignExposures*>(a.get());
	return a.get();
#else
	throw_no_photo();
#endif
}
void cveAlignMTBRelease(cv::AlignMTB** alignExposures, cv::Ptr<cv::AlignMTB>** sharedPtr)
{
#ifdef HAVE_OPENCV_PHOTO
	delete *sharedPtr;
	*alignExposures = 0;
	*sharedPtr = 0;
#else
	throw_no_photo();
#endif
}