//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2017 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "photo_c.h"

void cveInpaint( cv::_InputArray* src, cv::_InputArray* inpaintMask, cv::_OutputArray* dst, double inpaintRadius, int flags )
{
   cv::inpaint(*src, *inpaintMask, *dst, inpaintRadius, flags);
}
void cveFastNlMeansDenoising(cv::_InputArray* src, cv::_OutputArray* dst, float h, int templateWindowSize, int searchWindowSize)
{
   cv::fastNlMeansDenoising(*src, *dst, h, templateWindowSize, searchWindowSize);
}

void cveFastNlMeansDenoisingColored(cv::_InputArray* src, cv::_OutputArray* dst, float h, float hColor, int templateWindowSize, int searchWindowSize)
{
   cv::fastNlMeansDenoisingColored(*src, *dst, h, hColor, templateWindowSize, searchWindowSize);
}

void cudaNonLocalMeans(const cv::cuda::GpuMat* src, cv::cuda::GpuMat* dst, float h, int searchWindow, int blockSize, int borderMode, cv::cuda::Stream* stream)
{
   cv::cuda::nonLocalMeans(*src, *dst, h, searchWindow, blockSize, borderMode, stream ? *stream : cv::cuda::Stream::Null());
}

void cveEdgePreservingFilter(cv::_InputArray* src, cv::_OutputArray* dst, int flags, float sigmaS, float sigmaR)
{
   cv::edgePreservingFilter(*src, *dst, flags, sigmaS, sigmaR);
}

void cveDetailEnhance(cv::_InputArray* src, cv::_OutputArray* dst, float sigmaS, float sigmaR)
{
   cv::detailEnhance(*src, *dst, sigmaS, sigmaR);
}

void cvePencilSketch(cv::_InputArray* src, cv::_OutputArray* dst1, cv::_OutputArray* dst2, float sigmaS, float sigmaR, float shadeFactor)
{
   cv::pencilSketch(*src, *dst1, *dst2, sigmaS, sigmaR, shadeFactor);
}

void cveStylization(cv::_InputArray* src, cv::_OutputArray* dst, float sigmaS, float sigmaR)
{
   cv::stylization(*src, *dst, sigmaS, sigmaR);
}

void cveColorChange(cv::_InputArray* src, cv::_InputArray* mask, cv::_OutputArray* dst, float redMul, float greenMul, float blueMul)
{
   cv::colorChange(*src, mask ? *mask : (cv::InputArray) cv::noArray(), dst ? *dst : (cv::OutputArray) cv::noArray(), redMul, greenMul, blueMul);
}

void cveIlluminationChange(cv::_InputArray* src, cv::_InputArray* mask, cv::_OutputArray* dst, float alpha, float beta)
{
   cv::illuminationChange(*src, mask ? *mask : (cv::InputArray) cv::noArray(), *dst, alpha, beta);
}

void cveTextureFlattening(cv::_InputArray* src, cv::_InputArray* mask, cv::_OutputArray* dst, float lowThreshold, float highThreshold, int kernelSize)
{
   cv::textureFlattening(*src, mask ? *mask : (cv::InputArray) cv::noArray(), *dst, lowThreshold, highThreshold, kernelSize);
}

void cveDecolor(cv::_InputArray* src, cv::_OutputArray* grayscale, cv::_OutputArray* colorBoost)
{
   cv::decolor(*src, *grayscale, *colorBoost);
}

void cveSeamlessClone(cv::_InputArray* src, cv::_InputArray* dst, cv::_InputArray* mask, CvPoint* p, cv::_OutputArray* blend, int flags)
{
   cv::seamlessClone(*src, *dst, *mask, *p, *blend, flags);
}

void cveDenoiseTVL1(const std::vector< cv::Mat >* observations, cv::Mat* result, double lambda, int niters)
{
	cv::denoise_TVL1(*observations, *result, lambda, niters);
}

void cveCalibrateCRFProcess(cv::CalibrateCRF* calibrateCRF, cv::_InputArray* src, cv::_OutputArray* dst, cv::_InputArray* times)
{
   calibrateCRF->process(*src, *dst, *times);
}

cv::CalibrateDebevec* cveCalibrateDebevecCreate(int samples, float lambda, bool random, cv::CalibrateCRF** calibrateCRF)
{
   cv::Ptr<cv::CalibrateDebevec> res = cv::createCalibrateDebevec(samples, lambda, random);
   res.addref();
   *calibrateCRF = dynamic_cast<cv::CalibrateCRF*>(res.get());
   return res.get();
}
void cveCalibrateDebevecRelease(cv::CalibrateDebevec** calibrateDebevec)
{
   delete *calibrateDebevec;
   *calibrateDebevec = 0;
}

cv::CalibrateRobertson* cveCalibrateRobertsonCreate(int maxIter, float threshold, cv::CalibrateCRF** calibrateCRF)
{
   cv::Ptr<cv::CalibrateRobertson> res = cv::createCalibrateRobertson(maxIter, threshold);
   res.addref();
   *calibrateCRF = dynamic_cast<cv::CalibrateCRF*>(res.get());
   return res.get();
}
void cveCalibrateRobertsonRelease(cv::CalibrateRobertson** calibrateRobertson)
{
   delete *calibrateRobertson;
   calibrateRobertson = 0;
}

void cveMergeExposuresProcess(
   cv::MergeExposures* mergeExposures, 
   cv::_InputArray* src, cv::_OutputArray* dst,
   cv::_InputArray* times, cv::_InputArray* response)
{
   mergeExposures->process(*src, *dst, *times, *response);
}

cv::MergeDebevec* cveMergeDebevecCreate(cv::MergeExposures** merge)
{
   cv::Ptr<cv::MergeDebevec> res = cv::createMergeDebevec();
   res.addref();
   *merge = dynamic_cast<cv::MergeExposures*>(res.get());
   return res.get();
}
void cveMergeDebevecRelease(cv::MergeDebevec** merge)
{
   delete *merge;
   *merge = 0;
}

cv::MergeMertens* cveMergeMertensCreate(float contrastWeight, float saturationWeight, float exposureWeight, cv::MergeExposures** merge)
{
   cv::Ptr<cv::MergeMertens> res = cv::createMergeMertens(contrastWeight, saturationWeight, exposureWeight);
   res.addref();
   *merge = dynamic_cast<cv::MergeExposures*>(res.get());
   return res.get();
}

void cveMergeMertensRelease(cv::MergeMertens** merge)
{
   delete *merge;
   *merge = 0;
}

cv::MergeRobertson* cveMergeRobertsonCreate(cv::MergeExposures** merge)
{
   cv::Ptr<cv::MergeRobertson> res = cv::createMergeRobertson();
   res.addref();
   *merge = dynamic_cast<cv::MergeExposures*>(res.get());
   return res.get();
}

void cveMergeRobertsonRelease(cv::MergeRobertson** merge)
{
   delete *merge;
   *merge = 0;
}



void cveTonemapProcess(cv::Tonemap* tonemap, cv::_InputArray* src, cv::_OutputArray* dst)
{
   tonemap->process(*src, *dst);
}
cv::Tonemap* cveTonemapCreate(float gamma)
{
   cv::Ptr<cv::Tonemap> tonemap = cv::createTonemap(gamma);
   tonemap.addref();
   return tonemap.get();
}
void cveTonemapRelease(cv::Tonemap** tonemap)
{
   delete *tonemap;
   *tonemap = 0;
}

cv::TonemapDrago* cveTonemapDragoCreate(float gamma, float saturation, float bias, cv::Tonemap** tonemap)
{
   cv::Ptr<cv::TonemapDrago> t = cv::createTonemapDrago(gamma, saturation, bias);
   t.addref();
   *tonemap = dynamic_cast<cv::Tonemap*>(t.get());
   return t.get();
}
void cveTonemapDragoRelease(cv::TonemapDrago** tonemap)
{
   delete *tonemap;
   *tonemap = 0;
}

cv::TonemapDurand* cveTonemapDurandCreate(float gamma, float contrast, float saturation, float sigmaSpace, float sigmaColor, cv::Tonemap** tonemap)
{
   cv::Ptr<cv::TonemapDurand> t = cv::createTonemapDurand(gamma, contrast, saturation, sigmaSpace, sigmaColor);
   t.addref();
   *tonemap = dynamic_cast<cv::Tonemap*>(t.get());
   return t.get();
}
void cveTonemapDurandRelease(cv::TonemapDurand** tonemap)
{
   delete *tonemap;
   *tonemap = 0;
}

cv::TonemapReinhard* cveTonemapReinhardCreate(float gamma, float intensity, float lightAdapt, float colorAdapt, cv::Tonemap** tonemap)
{
   cv::Ptr<cv::TonemapReinhard> t = cv::createTonemapReinhard(gamma, intensity, lightAdapt, colorAdapt);
   t.addref();
   *tonemap = dynamic_cast<cv::Tonemap*>(t.get());
   return t.get();
}
void cveTonemapReinhardRelease(cv::TonemapReinhard** tonemap)
{
   delete *tonemap;
   *tonemap = 0;
}

cv::TonemapMantiuk* cveTonemapMantiukCreate(float gamma, float scale, float saturation, cv::Tonemap** tonemap)
{
   cv::Ptr<cv::TonemapMantiuk> t = cv::createTonemapMantiuk(gamma, scale, saturation);
   t.addref();
   *tonemap = dynamic_cast<cv::Tonemap*>(t.get());
   return t.get();
}
void cveTonemapMantiukRelease(cv::TonemapMantiuk** tonemap)
{
   delete *tonemap;
   *tonemap = 0;
}

void cveAlignExposuresProcess(cv::AlignExposures* alignExposures, cv::_InputArray* src, std::vector<cv::Mat>* dst, cv::_InputArray* times, cv::_InputArray* response)
{
	alignExposures->process(*src, *dst, *times, *response);
}


cv::AlignMTB* cveAlignMTBCreate(int maxBits, int excludeRange, bool cut, cv::AlignExposures** alignExposures)
{
	cv::Ptr<cv::AlignMTB> a = cv::createAlignMTB(maxBits, excludeRange, cut);
	a.addref();
	*alignExposures = dynamic_cast<cv::AlignExposures*>(a.get());
	return a.get();
}
void cveAlignMTBRelease(cv::AlignMTB** alignExposures)
{
	delete *alignExposures;
	*alignExposures = 0;
}