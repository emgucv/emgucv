//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "photo_c.h"

void cveInpaint(cv::_InputArray* src, cv::_InputArray* inpaintMask, cv::_OutputArray* dst, double inpaintRadius, int flags)
{
	try
	{
	#ifdef HAVE_OPENCV_PHOTO
		cv::inpaint(*src, *inpaintMask, *dst, inpaintRadius, flags);
	#else
		throw_no_photo();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveFastNlMeansDenoising(cv::_InputArray* src, cv::_OutputArray* dst, float h, int templateWindowSize, int searchWindowSize)
{
	try
	{
	#ifdef HAVE_OPENCV_PHOTO
		cv::fastNlMeansDenoising(*src, *dst, h, templateWindowSize, searchWindowSize);
	#else
		throw_no_photo();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveFastNlMeansDenoisingColored(cv::_InputArray* src, cv::_OutputArray* dst, float h, float hColor, int templateWindowSize, int searchWindowSize)
{
	try
	{
	#ifdef HAVE_OPENCV_PHOTO
		cv::fastNlMeansDenoisingColored(*src, *dst, h, hColor, templateWindowSize, searchWindowSize);
	#else
		throw_no_photo();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

/*
void cudaNonLocalMeans(const cv::cuda::GpuMat* src, cv::cuda::GpuMat* dst, float h, int searchWindow, int blockSize, int borderMode, cv::cuda::Stream* stream)
{
#ifdef HAVE_OPENCV_PHOTO
	cv::cuda::nonLocalMeans(*src, *dst, h, searchWindow, blockSize, borderMode, stream ? *stream : cv::cuda::Stream::Null());
#else
	throw_no_photo();
#endif
}
*/

void cveEdgePreservingFilter(cv::_InputArray* src, cv::_OutputArray* dst, int flags, float sigmaS, float sigmaR)
{
	try
	{
	#ifdef HAVE_OPENCV_PHOTO
		cv::edgePreservingFilter(*src, *dst, flags, sigmaS, sigmaR);
	#else
		throw_no_photo();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveDetailEnhance(cv::_InputArray* src, cv::_OutputArray* dst, float sigmaS, float sigmaR)
{
	try
	{
	#ifdef HAVE_OPENCV_PHOTO
		cv::detailEnhance(*src, *dst, sigmaS, sigmaR);
	#else
		throw_no_photo();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cvePencilSketch(cv::_InputArray* src, cv::_OutputArray* dst1, cv::_OutputArray* dst2, float sigmaS, float sigmaR, float shadeFactor)
{
	try
	{
	#ifdef HAVE_OPENCV_PHOTO
		cv::pencilSketch(*src, *dst1, *dst2, sigmaS, sigmaR, shadeFactor);
	#else
		throw_no_photo();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveStylization(cv::_InputArray* src, cv::_OutputArray* dst, float sigmaS, float sigmaR)
{
	try
	{
	#ifdef HAVE_OPENCV_PHOTO
		cv::stylization(*src, *dst, sigmaS, sigmaR);
	#else
		throw_no_photo();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveColorChange(cv::_InputArray* src, cv::_InputArray* mask, cv::_OutputArray* dst, float redMul, float greenMul, float blueMul)
{
	try
	{
	#ifdef HAVE_OPENCV_PHOTO
		cv::colorChange(*src, mask ? *mask : (cv::InputArray) cv::noArray(), dst ? *dst : (cv::OutputArray) cv::noArray(), redMul, greenMul, blueMul);
	#else
		throw_no_photo();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveIlluminationChange(cv::_InputArray* src, cv::_InputArray* mask, cv::_OutputArray* dst, float alpha, float beta)
{
	try
	{
	#ifdef HAVE_OPENCV_PHOTO
		cv::illuminationChange(*src, mask ? *mask : (cv::InputArray) cv::noArray(), *dst, alpha, beta);
	#else
		throw_no_photo();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveTextureFlattening(cv::_InputArray* src, cv::_InputArray* mask, cv::_OutputArray* dst, float lowThreshold, float highThreshold, int kernelSize)
{
	try
	{
	#ifdef HAVE_OPENCV_PHOTO
		cv::textureFlattening(*src, mask ? *mask : (cv::InputArray) cv::noArray(), *dst, lowThreshold, highThreshold, kernelSize);
	#else
		throw_no_photo();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveDecolor(cv::_InputArray* src, cv::_OutputArray* grayscale, cv::_OutputArray* colorBoost)
{
	try
	{
	#ifdef HAVE_OPENCV_PHOTO
		cv::decolor(*src, *grayscale, *colorBoost);
	#else
		throw_no_photo();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveSeamlessClone(cv::_InputArray* src, cv::_InputArray* dst, cv::_InputArray* mask, cv::Point* p, cv::_OutputArray* blend, int flags)
{
	try
	{
	#ifdef HAVE_OPENCV_PHOTO
		cv::seamlessClone(*src, *dst, *mask, *p, *blend, flags);
	#else
		throw_no_photo();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveDenoiseTVL1(const std::vector< cv::Mat >* observations, cv::Mat* result, double lambda, int niters)
{
	try
	{
	#ifdef HAVE_OPENCV_PHOTO
		cv::denoise_TVL1(*observations, *result, lambda, niters);
	#else
		throw_no_photo();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveCalibrateCRFProcess(cv::CalibrateCRF* calibrateCRF, cv::_InputArray* src, cv::_OutputArray* dst, cv::_InputArray* times)
{
	try
	{
	#ifdef HAVE_OPENCV_PHOTO
		calibrateCRF->process(*src, *dst, *times);
	#else
		throw_no_photo();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

cv::CalibrateDebevec* cveCalibrateDebevecCreate(int samples, float lambda, bool random, cv::CalibrateCRF** calibrateCRF, cv::Ptr<cv::CalibrateDebevec>** sharedPtr)
{
	try
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
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveCalibrateDebevecRelease(cv::CalibrateDebevec** calibrateDebevec, cv::Ptr<cv::CalibrateDebevec>** sharedPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_PHOTO
		delete *sharedPtr;
		*calibrateDebevec = 0;
		*sharedPtr = 0;
	#else
		throw_no_photo();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

cv::CalibrateRobertson* cveCalibrateRobertsonCreate(int maxIter, float threshold, cv::CalibrateCRF** calibrateCRF, cv::Ptr<cv::CalibrateRobertson>** sharedPtr)
{
	try
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
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveCalibrateRobertsonRelease(cv::CalibrateRobertson** calibrateRobertson, cv::Ptr<cv::CalibrateRobertson>** sharedPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_PHOTO
		delete *sharedPtr;
		calibrateRobertson = 0;
		*sharedPtr = 0;
	#else
		throw_no_photo();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveMergeExposuresProcess(
	cv::MergeExposures* mergeExposures,
	cv::_InputArray* src, cv::_OutputArray* dst,
	cv::_InputArray* times, cv::_InputArray* response)
{
	try
	{
	#ifdef HAVE_OPENCV_PHOTO
		mergeExposures->process(*src, *dst, *times, *response);
	#else
		throw_no_photo();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

cv::MergeDebevec* cveMergeDebevecCreate(cv::MergeExposures** merge, cv::Ptr<cv::MergeDebevec>** sharedPtr)
{
	try
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
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveMergeDebevecRelease(cv::MergeDebevec** merge, cv::Ptr<cv::MergeDebevec>** sharedPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_PHOTO
		delete *sharedPtr;
		*merge = 0;
		*sharedPtr = 0;
	#else
		throw_no_photo();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

cv::MergeMertens* cveMergeMertensCreate(float contrastWeight, float saturationWeight, float exposureWeight, cv::MergeExposures** merge, cv::Ptr<cv::MergeMertens>** sharedPtr)
{
	try
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
	CVAPI_CATCH_CV_ERRORS(0)
}

void cveMergeMertensRelease(cv::MergeMertens** merge, cv::Ptr<cv::MergeMertens>** sharedPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_PHOTO
		delete *sharedPtr;
		*merge = 0;
		*sharedPtr = 0;
	#else
		throw_no_photo();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

cv::MergeRobertson* cveMergeRobertsonCreate(cv::MergeExposures** merge, cv::Ptr<cv::MergeRobertson>** sharedPtr)
{
	try
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
	CVAPI_CATCH_CV_ERRORS(0)
}

void cveMergeRobertsonRelease(cv::MergeRobertson** merge, cv::Ptr<cv::MergeRobertson>** sharedPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_PHOTO
		delete *sharedPtr;
		*merge = 0;
		*sharedPtr = 0;
	#else
		throw_no_photo();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}


void cveTonemapProcess(cv::Tonemap* tonemap, cv::_InputArray* src, cv::_OutputArray* dst)
{
	try
	{
	#ifdef HAVE_OPENCV_PHOTO
		tonemap->process(*src, *dst);
	#else
		throw_no_photo();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
cv::Tonemap* cveTonemapCreate(float gamma, cv::Algorithm** algorithm, cv::Ptr<cv::Tonemap>** sharedPtr)
{
	try
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
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveTonemapRelease(cv::Tonemap** tonemap, cv::Ptr<cv::Tonemap>** sharedPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_PHOTO
		delete *sharedPtr;
		*tonemap = 0;
		*sharedPtr = 0;
	#else
		throw_no_photo();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

cv::TonemapDrago* cveTonemapDragoCreate(float gamma, float saturation, float bias, cv::Tonemap** tonemap, cv::Algorithm** algorithm, cv::Ptr<cv::TonemapDrago>** sharedPtr)
{
	try
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
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveTonemapDragoRelease(cv::TonemapDrago** tonemap, cv::Ptr<cv::TonemapDrago>** sharedPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_PHOTO
		delete *sharedPtr;
		*tonemap = 0;
		*sharedPtr = 0;
	#else
		throw_no_photo();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

cv::TonemapReinhard* cveTonemapReinhardCreate(float gamma, float intensity, float lightAdapt, float colorAdapt, cv::Tonemap** tonemap, cv::Algorithm** algorithm, cv::Ptr<cv::TonemapReinhard>** sharedPtr)
{
	try
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
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveTonemapReinhardRelease(cv::TonemapReinhard** tonemap, cv::Ptr<cv::TonemapReinhard>** sharedPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_PHOTO
		delete *sharedPtr;
		*tonemap = 0;
		*sharedPtr = 0;
	#else
		throw_no_photo();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

cv::TonemapMantiuk* cveTonemapMantiukCreate(float gamma, float scale, float saturation, cv::Tonemap** tonemap, cv::Algorithm** algorithm, cv::Ptr<cv::TonemapMantiuk>** sharedPtr)
{
	try
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
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveTonemapMantiukRelease(cv::TonemapMantiuk** tonemap, cv::Ptr<cv::TonemapMantiuk>** sharedPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_PHOTO
		delete *sharedPtr;
		*tonemap = 0;
		*sharedPtr = 0;
	#else
		throw_no_photo();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveAlignExposuresProcess(cv::AlignExposures* alignExposures, cv::_InputArray* src, std::vector<cv::Mat>* dst, cv::_InputArray* times, cv::_InputArray* response)
{
	try
	{
	#ifdef HAVE_OPENCV_PHOTO
		alignExposures->process(*src, *dst, *times, *response);
	#else
		throw_no_photo();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}


cv::AlignMTB* cveAlignMTBCreate(int maxBits, int excludeRange, bool cut, cv::AlignExposures** alignExposures, cv::Ptr<cv::AlignMTB>** sharedPtr)
{
	try
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
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveAlignMTBRelease(cv::AlignMTB** alignExposures, cv::Ptr<cv::AlignMTB>** sharedPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_PHOTO
		delete *sharedPtr;
		*alignExposures = 0;
		*sharedPtr = 0;
	#else
		throw_no_photo();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

//cuda photo module
void cudaNonLocalMeans(
	cv::_InputArray* src,
	cv::_OutputArray* dst,
	float h,
	int searchWindow,
	int blockSize,
	int borderMode,
	cv::cuda::Stream* stream)
{
#ifdef HAVE_OPENCV_PHOTO
	cv::cuda::nonLocalMeans(*src, *dst, h, searchWindow, blockSize, borderMode, stream ? *stream : cv::cuda::Stream::Null());
#else
	throw_no_photo();
#endif
}

void cudaFastNlMeansDenoising(
	cv::_InputArray* src,
	cv::_OutputArray* dst,
	float h,
	int searchWindow,
	int blockSize,
	cv::cuda::Stream* stream)
{
#ifdef HAVE_OPENCV_PHOTO
	cv::cuda::fastNlMeansDenoising(*src, *dst, h, searchWindow, blockSize, stream ? *stream : cv::cuda::Stream::Null());
#else
	throw_no_photo();
#endif
}

void cudaFastNlMeansDenoisingColored(
	cv::_InputArray* src,
	cv::_OutputArray* dst,
	float hLuminance,
	float photoRender,
	int searchWindow,
	int blockSize,
	cv::cuda::Stream* stream)
{
#ifdef HAVE_OPENCV_PHOTO
	cv::cuda::fastNlMeansDenoisingColored(*src, *dst, hLuminance, photoRender, searchWindow, blockSize, stream ? *stream : cv::cuda::Stream::Null());
#else
	throw_no_photo();
#endif
}


cv::ccm::ColorCorrectionModel* cveColorCorrectionModelCreate1(cv::Mat* src, int constColor)
{
	try
	{
	#ifdef HAVE_OPENCV_PHOTO
		return new cv::ccm::ColorCorrectionModel(*src, static_cast<cv::ccm::ColorCheckerType>(constColor));
	#else
		throw_no_photo();
	#endif	
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

cv::ccm::ColorCorrectionModel* cveColorCorrectionModelCreate2(cv::Mat* src, cv::Mat* colors, int refCs)
{
	try
	{
	#ifdef HAVE_OPENCV_PHOTO
		return new cv::ccm::ColorCorrectionModel(*src, *colors, static_cast<cv::ccm::ColorSpace>(refCs));
	#else
		throw_no_photo();
	#endif	
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

cv::ccm::ColorCorrectionModel* cveColorCorrectionModelCreate3(cv::Mat* src, cv::Mat* colors, int refCs, cv::Mat* colored)
{
	try
	{
	#ifdef HAVE_OPENCV_PHOTO
		return new cv::ccm::ColorCorrectionModel(*src, *colors, static_cast<cv::ccm::ColorSpace>(refCs), *colored);
	#else
		throw_no_photo();
	#endif	
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

void cveColorCorrectionModelRelease(cv::ccm::ColorCorrectionModel** ccm)
{
	try
	{
	#ifdef HAVE_OPENCV_PHOTO
		delete* ccm;
		*ccm = 0;
	#else
		throw_no_photo();
	#endif	
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveColorCorrectionModelCompute(cv::ccm::ColorCorrectionModel* ccm, cv::Mat* result)
{
	try
	{
	#ifdef HAVE_OPENCV_PHOTO
		cv::Mat ccmMat = ccm->compute();
		ccmMat.copyTo(*result);
	#else
		throw_no_photo();
	#endif	
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

/*
void cveColorCorrectionModelGetCCM(cv::ccm::ColorCorrectionModel* ccm, cv::_OutputArray* result)
{
#ifdef HAVE_OPENCV_PHOTO
	cv::Mat m = ccm->getCCM();
	m.copyTo(*result);
#else
	throw_no_objdetect();
#endif	
}*/

void cveColorCorrectionModelCorrectImage(cv::ccm::ColorCorrectionModel* ccm, cv::_InputArray* img, cv::_OutputArray* result, bool islinear)
{
	try
	{
	#ifdef HAVE_OPENCV_PHOTO
		ccm->correctImage(*img, *result, islinear);
	#else
		throw_no_photo();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

//IntelligentScissorsMB
cv::segmentation::IntelligentScissorsMB* cveIntelligentScissorsMBCreate()
{
	try
	{
	#ifdef HAVE_OPENCV_PHOTO
		return new cv::segmentation::IntelligentScissorsMB();
	#else
		throw_no_photo();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveIntelligentScissorsMBRelease(cv::segmentation::IntelligentScissorsMB** ptr)
{
	try
	{
	#ifdef HAVE_OPENCV_PHOTO
		delete* ptr;
		*ptr = 0;
	#else
		throw_no_photo();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveIntelligentScissorsMBSetWeights(
	cv::segmentation::IntelligentScissorsMB* ptr,
	float weightNonEdge,
	float weightGradientDirection,
	float weightGradientMagnitude)
{
	try
	{
	#ifdef HAVE_OPENCV_PHOTO
		ptr->setWeights(weightNonEdge, weightGradientDirection, weightGradientMagnitude);
	#else
		throw_no_photo();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveIntelligentScissorsMBSetEdgeFeatureCannyParameters(
	cv::segmentation::IntelligentScissorsMB* ptr,
	double threshold1,
	double threshold2,
	int apertureSize,
	bool L2gradient)
{
	try
	{
	#ifdef HAVE_OPENCV_PHOTO
		ptr->setEdgeFeatureCannyParameters(threshold1, threshold2, apertureSize, L2gradient);
	#else
		throw_no_photo();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveIntelligentScissorsMBApplyImage(cv::segmentation::IntelligentScissorsMB* ptr, cv::_InputArray* image)
{
	try
	{
	#ifdef HAVE_OPENCV_PHOTO
		ptr->applyImage(*image);
	#else
		throw_no_photo();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveIntelligentScissorsMBApplyImageFeatures(
	cv::segmentation::IntelligentScissorsMB* ptr,
	cv::_InputArray* nonEdge,
	cv::_InputArray* gradientDirection,
	cv::_InputArray* gradientMagnitude,
	cv::_InputArray* image)
{
	try
	{
	#ifdef HAVE_OPENCV_PHOTO
		ptr->applyImageFeatures(*nonEdge, *gradientDirection, *gradientMagnitude, image ? *image : static_cast<cv::InputArray>(cv::noArray()));
	#else
		throw_no_photo();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveIntelligentScissorsMBBuildMap(cv::segmentation::IntelligentScissorsMB* ptr, cv::Point* sourcePt)
{
	try
	{
	#ifdef HAVE_OPENCV_PHOTO
		ptr->buildMap(*sourcePt);
	#else
		throw_no_photo();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveIntelligentScissorsMBGetContour(
	cv::segmentation::IntelligentScissorsMB* ptr,
	cv::Point* targetPt,
	cv::_OutputArray* contour,
	bool backward)
{
	try
	{
	#ifdef HAVE_OPENCV_PHOTO
		ptr->getContour(*targetPt, *contour, backward);
	#else
		throw_no_photo();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
