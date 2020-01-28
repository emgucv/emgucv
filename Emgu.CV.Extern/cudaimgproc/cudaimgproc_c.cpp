//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "cudaimgproc_c.h"

void cudaBlendLinear(
	cv::_InputArray* img1, cv::_InputArray* img2,
	cv::_InputArray* weights1, cv::_InputArray* weights2,
	cv::_OutputArray* result, cv::cuda::Stream* stream)
{
#ifdef HAVE_OPENCV_CUDAIMGPROC
	cv::cuda::blendLinear(*img1, *img2, *weights1, *weights2, *result, stream ? *stream : cv::cuda::Stream::Null());
#else
	throw_no_cudaimgproc();
#endif
}

void cudaCvtColor(cv::_InputArray* src, cv::_OutputArray* dst, int code, int dcn, cv::cuda::Stream* stream)
{
#ifdef HAVE_OPENCV_CUDAIMGPROC
	cv::cuda::cvtColor(*src, *dst, code, dcn, stream ? *stream : cv::cuda::Stream::Null());
#else
	throw_no_cudaimgproc();
#endif	
}

void cudaDemosaicing(cv::_InputArray* src, cv::_OutputArray* dst, int code, int dcn, cv::cuda::Stream* stream)
{
#ifdef HAVE_OPENCV_CUDAIMGPROC
	cv::cuda::demosaicing(*src, *dst, code, dcn, stream ? *stream : cv::cuda::Stream::Null());
#else
	throw_no_cudaimgproc();
#endif
}

void cudaSwapChannels(cv::_InputOutputArray* image, const int* dstOrder, cv::cuda::Stream* stream)
{
#ifdef HAVE_OPENCV_CUDAIMGPROC
	cv::cuda::swapChannels(*image, dstOrder, stream ? *stream : cv::cuda::Stream::Null());
#else
	throw_no_cudaimgproc();
#endif
}

void cudaAlphaComp(cv::_InputArray* img1, cv::_InputArray* img2, cv::_OutputArray* dst, int alphaOp, cv::cuda::Stream* stream)
{
#ifdef HAVE_OPENCV_CUDAIMGPROC
	cv::cuda::alphaComp(*img1, *img2, *dst, alphaOp, stream ? *stream : cv::cuda::Stream::Null());
#else
	throw_no_cudaimgproc();
#endif
}

void cudaMeanShiftFiltering(cv::_InputArray* src, cv::_OutputArray* dst, int sp, int sr,
	CvTermCriteria* criteria, cv::cuda::Stream* stream)
{
#ifdef HAVE_OPENCV_CUDAIMGPROC
	cv::cuda::meanShiftFiltering(*src, *dst, sp, sr, *criteria, stream ? *stream : cv::cuda::Stream::Null());
#else
	throw_no_cudaimgproc();
#endif
}

void cudaMeanShiftProc(cv::_InputArray* src, cv::_OutputArray* dstr, cv::_OutputArray* dstsp, int sp, int sr,
	CvTermCriteria* criteria, cv::cuda::Stream* stream)
{
#ifdef HAVE_OPENCV_CUDAIMGPROC
	cv::cuda::meanShiftProc(*src, *dstr, *dstsp, sp, sr, *criteria, stream ? *stream : cv::cuda::Stream::Null());
#else
	throw_no_cudaimgproc();
#endif
}

void cudaMeanShiftSegmentation(cv::_InputArray* src, cv::_OutputArray* dst, int sp, int sr, int minsize,
	CvTermCriteria* criteria, cv::cuda::Stream* stream)
{
#ifdef HAVE_OPENCV_CUDAIMGPROC
	cv::cuda::meanShiftSegmentation(*src, *dst, sp, sr, minsize, *criteria, stream ? *stream : cv::cuda::Stream::Null());
#else
	throw_no_cudaimgproc();
#endif
}

void cudaCalcHist(cv::_InputArray* src, cv::_OutputArray* hist, cv::cuda::Stream* stream)
{
#ifdef HAVE_OPENCV_CUDAIMGPROC
	cv::cuda::calcHist(*src, *hist, stream ? *stream : cv::cuda::Stream::Null());
#else
	throw_no_cudaimgproc();
#endif
}

void cudaEqualizeHist(cv::_InputArray* src, cv::_OutputArray* dst, cv::cuda::Stream* stream)
{
#ifdef HAVE_OPENCV_CUDAIMGPROC
	cv::cuda::equalizeHist(*src, *dst, stream ? *stream : cv::cuda::Stream::Null());
#else
	throw_no_cudaimgproc();
#endif
}

void cudaHistEven(cv::_InputArray* src, cv::_OutputArray* hist, int histSize, int lowerLevel, int upperLevel, cv::cuda::Stream* stream)
{
#ifdef HAVE_OPENCV_CUDAIMGPROC
	cv::cuda::histEven(*src, *hist, histSize, lowerLevel, upperLevel, stream ? *stream : cv::cuda::Stream::Null());
#else
	throw_no_cudaimgproc();
#endif
}

void cudaHistRange(cv::_InputArray* src, cv::_OutputArray* hist, cv::_InputArray* levels, cv::cuda::Stream* stream)
{
#ifdef HAVE_OPENCV_CUDAIMGPROC
	cv::cuda::histRange(*src, *hist, *levels, stream ? *stream : cv::cuda::Stream::Null());
#else
	throw_no_cudaimgproc();
#endif
}

void cudaBilateralFilter(cv::_InputArray* src, cv::_OutputArray* dst, int kernelSize, float sigmaColor, float sigmaSpatial, int borderMode, cv::cuda::Stream* stream)
{
#ifdef HAVE_OPENCV_CUDAIMGPROC
	cv::cuda::bilateralFilter(*src, *dst, kernelSize, sigmaColor, sigmaSpatial, borderMode, stream ? *stream : cv::cuda::Stream::Null());
#else
	throw_no_cudaimgproc();
#endif
}

//----------------------------------------------------------------------------
//
//  CudaCornernessCriteria
//
//----------------------------------------------------------------------------
cv::cuda::CornernessCriteria* cudaCreateHarrisCorner(int srcType, int blockSize, int ksize, double k, int borderType, cv::Ptr<cv::cuda::CornernessCriteria>** sharedPtr)
{
#ifdef HAVE_OPENCV_CUDAIMGPROC
	cv::Ptr<cv::cuda::CornernessCriteria> ptr = cv::cuda::createHarrisCorner(srcType, blockSize, ksize, k, borderType);
	*sharedPtr = new cv::Ptr<cv::cuda::CornernessCriteria>(ptr);
	return (*sharedPtr)->get();
#else
	throw_no_cudaimgproc();
#endif
}

cv::cuda::CornernessCriteria* cudaCreateMinEigenValCorner(int srcType, int blockSize, int ksize, int borderType, cv::Ptr<cv::cuda::CornernessCriteria>** sharedPtr)
{
#ifdef HAVE_OPENCV_CUDAIMGPROC
	cv::Ptr<cv::cuda::CornernessCriteria> ptr = cv::cuda::createMinEigenValCorner(srcType, blockSize, ksize, borderType);
	*sharedPtr = new cv::Ptr<cv::cuda::CornernessCriteria>(ptr);
	return (*sharedPtr)->get();
#else
	throw_no_cudaimgproc();
#endif
}

void cudaCornernessCriteriaCompute(cv::Ptr<cv::cuda::CornernessCriteria>* detector, cv::_InputArray* src, cv::_OutputArray* dst, cv::cuda::Stream* stream)
{
#ifdef HAVE_OPENCV_CUDAIMGPROC
	(*detector)->compute(*src, *dst, stream ? *stream : cv::cuda::Stream::Null());
#else
	throw_no_cudaimgproc();
#endif
}

void cudaCornernessCriteriaRelease(cv::Ptr<cv::cuda::CornernessCriteria>** detector)
{
#ifdef HAVE_OPENCV_CUDAIMGPROC
	delete *detector;
	*detector = 0;
#else
	throw_no_cudaimgproc();
#endif
}

//----------------------------------------------------------------------------
//
//  CudaCLAHE
//
//----------------------------------------------------------------------------
cv::cuda::CLAHE* cudaCLAHECreate(double clipLimit, CvSize* tileGridSize, cv::Ptr<cv::cuda::CLAHE>** sharedPtr)
{
#ifdef HAVE_OPENCV_CUDAIMGPROC
	cv::Size s(tileGridSize->width, tileGridSize->height);
	cv::Ptr<cv::cuda::CLAHE> ptr = cv::cuda::createCLAHE(clipLimit, s);
	*sharedPtr = new cv::Ptr<cv::cuda::CLAHE>(ptr);
	return (*sharedPtr)->get();
#else
	throw_no_cudaimgproc();
#endif
}
void cudaCLAHEApply(cv::cuda::CLAHE* clahe, cv::_InputArray* src, cv::_OutputArray* dst, cv::cuda::Stream* stream)
{
#ifdef HAVE_OPENCV_CUDAIMGPROC
	clahe->apply(*src, *dst, stream ? *stream : cv::cuda::Stream::Null());
#else
	throw_no_cudaimgproc();
#endif
}
void cudaCLAHERelease(cv::Ptr<cv::cuda::CLAHE>** clahe)
{
#ifdef HAVE_OPENCV_CUDAIMGPROC
	delete *clahe;
	*clahe = 0;
#else
	throw_no_cudaimgproc();
#endif
}

//----------------------------------------------------------------------------
//
//  CannyEdgeDetector
//
//----------------------------------------------------------------------------
cv::cuda::CannyEdgeDetector* cudaCreateCannyEdgeDetector(double lowThreshold, double highThreshold, int apertureSize, bool L2gradient, cv::Ptr<cv::cuda::CannyEdgeDetector>** sharedPtr)
{
#ifdef HAVE_OPENCV_CUDAIMGPROC
	cv::Ptr<cv::cuda::CannyEdgeDetector> ptr = cv::cuda::createCannyEdgeDetector(lowThreshold, highThreshold, apertureSize, L2gradient);
	*sharedPtr = new cv::Ptr<cv::cuda::CannyEdgeDetector>(ptr);
	return (*sharedPtr)->get();
#else
	throw_no_cudaimgproc();
#endif
}
void cudaCannyEdgeDetectorDetect(cv::cuda::CannyEdgeDetector* detector, cv::_InputArray* src, cv::_OutputArray* edges, cv::cuda::Stream* stream)
{
#ifdef HAVE_OPENCV_CUDAIMGPROC
	detector->detect(*src, *edges, stream ? *stream : cv::cuda::Stream::Null());
#else
	throw_no_cudaimgproc();
#endif
}
void cudaCannyEdgeDetectorRelease(cv::Ptr<cv::cuda::CannyEdgeDetector>** detector)
{
#ifdef HAVE_OPENCV_CUDAIMGPROC
	delete *detector;
	*detector = 0;
#else
	throw_no_cudaimgproc();
#endif
}

//----------------------------------------------------------------------------
//
//  GpuGoodFeaturesToTrackDetector
//
//----------------------------------------------------------------------------
cv::cuda::CornersDetector* cudaGoodFeaturesToTrackDetectorCreate(int srcType, int maxCorners, double qualityLevel, double minDistance, int blockSize, bool useHarrisDetector, double harrisK, cv::Ptr<cv::cuda::CornersDetector>** sharedPtr)
{
#ifdef HAVE_OPENCV_CUDAIMGPROC
	cv::Ptr<cv::cuda::CornersDetector> detector = cv::cuda::createGoodFeaturesToTrackDetector(srcType, maxCorners, qualityLevel, minDistance, blockSize, useHarrisDetector, harrisK);
	*sharedPtr = new cv::Ptr<cv::cuda::CornersDetector>(detector);
	return (*sharedPtr)->get();
#else
	throw_no_cudaimgproc();
#endif
}
void cudaCornersDetectorDetect(cv::cuda::CornersDetector* detector, cv::_InputArray* image, cv::_OutputArray* corners, cv::_InputArray* mask, cv::cuda::Stream* stream)
{
#ifdef HAVE_OPENCV_CUDAIMGPROC
	detector->detect(*image, *corners, mask ? *mask : (cv::InputArray) cv::noArray(), stream ? *stream : cv::cuda::Stream::Null());
#else
	throw_no_cudaimgproc();
#endif
}
void cudaCornersDetectorRelease(cv::Ptr<cv::cuda::CornersDetector>** detector)
{
#ifdef HAVE_OPENCV_CUDAIMGPROC
	delete *detector;
	*detector = 0;
#else
	throw_no_cudaimgproc();
#endif
}

//----------------------------------------------------------------------------
//
//  GpuTemplateMatching
//
//----------------------------------------------------------------------------
cv::cuda::TemplateMatching* cudaTemplateMatchingCreate(int srcType, int method, CvSize* blockSize, cv::Ptr<cv::cuda::TemplateMatching>** sharedPtr)
{
#ifdef HAVE_OPENCV_CUDAIMGPROC
	cv::Size s(blockSize->width, blockSize->height);
	cv::Ptr<cv::cuda::TemplateMatching> ptr = cv::cuda::createTemplateMatching(srcType, method, s);
	*sharedPtr = new cv::Ptr<cv::cuda::TemplateMatching>(ptr);
	return (*sharedPtr)->get();
#else
	throw_no_cudaimgproc();
#endif
}

void cudaTemplateMatchingMatch(cv::cuda::TemplateMatching* tm, cv::_InputArray* image, cv::_InputArray* templ, cv::_OutputArray* result, cv::cuda::Stream* stream)
{
#ifdef HAVE_OPENCV_CUDAIMGPROC
	tm->match(*image, *templ, *result, stream ? *stream : cv::cuda::Stream::Null());
#else
	throw_no_cudaimgproc();
#endif
}

void cudaTemplateMatchingRelease(cv::Ptr<cv::cuda::TemplateMatching>** tm)
{
#ifdef HAVE_OPENCV_CUDAIMGPROC
	delete *tm;
	*tm = 0;
#else
	throw_no_cudaimgproc();
#endif
}

//----------------------------------------------------------------------------
//
//  CudaHoughLinesDetector
//
//----------------------------------------------------------------------------
cv::cuda::HoughLinesDetector* cudaHoughLinesDetectorCreate(float rho, float theta, int threshold, bool doSort, int maxLines, cv::Ptr<cv::cuda::HoughLinesDetector>** sharedPtr)
{
#ifdef HAVE_OPENCV_CUDAIMGPROC
	cv::Ptr<cv::cuda::HoughLinesDetector> ptr = cv::cuda::createHoughLinesDetector(rho, theta, threshold, doSort, maxLines);
	*sharedPtr = new cv::Ptr<cv::cuda::HoughLinesDetector>(ptr);
	return (*sharedPtr)->get();
#else
	throw_no_cudaimgproc();
#endif
}
void cudaHoughLinesDetectorDetect(cv::cuda::HoughLinesDetector* detector, cv::_InputArray* src, cv::_OutputArray* lines, cv::cuda::Stream* stream)
{
#ifdef HAVE_OPENCV_CUDAIMGPROC
	detector->detect(*src, *lines, stream ? *stream : cv::cuda::Stream::Null());
#else
	throw_no_cudaimgproc();
#endif
}
void cudaHoughLinesDetectorRelease(cv::Ptr<cv::cuda::HoughLinesDetector>** detector)
{
#ifdef HAVE_OPENCV_CUDAIMGPROC
	delete *detector;
	*detector = 0;
#else
	throw_no_cudaimgproc();
#endif
}

//----------------------------------------------------------------------------
//
//  CudaHoughSegmentDetector
//
//----------------------------------------------------------------------------
cv::cuda::HoughSegmentDetector* cudaHoughSegmentDetectorCreate(float rho, float theta, int minLineLength, int maxLineGap, int maxLines, cv::Ptr<cv::cuda::HoughSegmentDetector>** sharedPtr)
{
#ifdef HAVE_OPENCV_CUDAIMGPROC
	cv::Ptr<cv::cuda::HoughSegmentDetector> ptr = cv::cuda::createHoughSegmentDetector(rho, theta, minLineLength, maxLineGap, maxLines);
	*sharedPtr = new cv::Ptr<cv::cuda::HoughSegmentDetector>(ptr);
	return ptr.get();
#else
	throw_no_cudaimgproc();
#endif
}
void cudaHoughSegmentDetectorDetect(cv::cuda::HoughSegmentDetector* detector, cv::_InputArray* src, cv::_OutputArray* lines, cv::cuda::Stream* stream)
{
#ifdef HAVE_OPENCV_CUDAIMGPROC
	detector->detect(*src, *lines, stream ? *stream : cv::cuda::Stream::Null());
#else
	throw_no_cudaimgproc();
#endif
}
void cudaHoughSegmentDetectorRelease(cv::Ptr<cv::cuda::HoughSegmentDetector>** detector)
{
#ifdef HAVE_OPENCV_CUDAIMGPROC
	delete *detector;
	*detector = 0;
#else
	throw_no_cudaimgproc();
#endif
}

//----------------------------------------------------------------------------
//
//  CudaHoughCirclesDetector
//
//----------------------------------------------------------------------------
cv::cuda::HoughCirclesDetector* cudaHoughCirclesDetectorCreate(float dp, float minDist, int cannyThreshold, int votesThreshold, int minRadius, int maxRadius, int maxCircles, cv::Ptr<cv::cuda::HoughCirclesDetector>** sharedPtr)
{
#ifdef HAVE_OPENCV_CUDAIMGPROC
	cv::Ptr<cv::cuda::HoughCirclesDetector> ptr = cv::cuda::createHoughCirclesDetector(dp, minDist, cannyThreshold, votesThreshold, minRadius, maxRadius, maxCircles);
	*sharedPtr = new cv::Ptr<cv::cuda::HoughCirclesDetector>(ptr);
	return ptr.get();
#else
	throw_no_cudaimgproc();
#endif
}
void cudaHoughCirclesDetectorDetect(cv::cuda::HoughCirclesDetector* detector, cv::_InputArray* src, cv::_OutputArray* circles, cv::cuda::Stream* stream)
{
#ifdef HAVE_OPENCV_CUDAIMGPROC
	detector->detect(*src, *circles, stream ? *stream : cv::cuda::Stream::Null());
#else
	throw_no_cudaimgproc();
#endif
}
void cudaHoughCirclesDetectorRelease(cv::Ptr<cv::cuda::HoughCirclesDetector>** detector)
{
#ifdef HAVE_OPENCV_CUDAIMGPROC
	delete *detector;
	*detector = 0;
#else
	throw_no_cudaimgproc();
#endif
}

void cudaGammaCorrection(cv::_InputArray* src, cv::_OutputArray* dst, bool forward, cv::cuda::Stream* stream)
{
#ifdef HAVE_OPENCV_CUDAIMGPROC
	cv::cuda::gammaCorrection(*src, *dst, forward, stream ? *stream : cv::cuda::Stream::Null());
#else
	throw_no_cudaimgproc();
#endif
}
