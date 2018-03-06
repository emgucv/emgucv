//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "cudaimgproc_c.h"

void cudaBlendLinear(
   cv::_InputArray* img1, cv::_InputArray* img2, 
   cv::_InputArray* weights1, cv::_InputArray* weights2, 
   cv::_OutputArray* result, cv::cuda::Stream* stream)
{
   cv::cuda::blendLinear(*img1, *img2, *weights1, *weights2, *result, stream ? *stream : cv::cuda::Stream::Null());
}

void cudaCvtColor(cv::_InputArray* src, cv::_OutputArray* dst, int code, int dcn, cv::cuda::Stream* stream)
{
   cv::cuda::cvtColor(*src, *dst, code, dcn, stream ? *stream : cv::cuda::Stream::Null());
}

void cudaDemosaicing(cv::_InputArray* src, cv::_OutputArray* dst, int code, int dcn, cv::cuda::Stream* stream)
{
   cv::cuda::demosaicing(*src, *dst, code, dcn, stream ? *stream : cv::cuda::Stream::Null());
}

void cudaSwapChannels(cv::_InputOutputArray* image, const int* dstOrder, cv::cuda::Stream* stream)
{
   cv::cuda::swapChannels(*image, dstOrder, stream ? *stream : cv::cuda::Stream::Null());
}

void cudaAlphaComp(cv::_InputArray* img1, cv::_InputArray* img2, cv::_OutputArray* dst, int alphaOp, cv::cuda::Stream* stream)
{
   cv::cuda::alphaComp(*img1, *img2, *dst, alphaOp, stream ? *stream : cv::cuda::Stream::Null());
}

void cudaMeanShiftFiltering(cv::_InputArray* src, cv::_OutputArray* dst, int sp, int sr,
                              CvTermCriteria* criteria, cv::cuda::Stream* stream)
{
   cv::cuda::meanShiftFiltering(*src, *dst, sp, sr, *criteria, stream ? *stream : cv::cuda::Stream::Null());
}

void cudaMeanShiftProc(cv::_InputArray* src, cv::_OutputArray* dstr, cv::_OutputArray* dstsp, int sp, int sr,
                         CvTermCriteria* criteria, cv::cuda::Stream* stream)
{
   cv::cuda::meanShiftProc(*src, *dstr, *dstsp, sp, sr, *criteria, stream ? *stream : cv::cuda::Stream::Null());
}

void cudaMeanShiftSegmentation(cv::_InputArray* src, cv::_OutputArray* dst, int sp, int sr, int minsize,
   CvTermCriteria* criteria, cv::cuda::Stream* stream)
{
   cv::cuda::meanShiftSegmentation(*src, *dst, sp, sr, minsize, *criteria, stream ? *stream : cv::cuda::Stream::Null());
}

void cudaCalcHist(cv::_InputArray* src, cv::_OutputArray* hist, cv::cuda::Stream* stream)
{
   cv::cuda::calcHist(*src, *hist, stream ? *stream : cv::cuda::Stream::Null());
}

void cudaEqualizeHist(cv::_InputArray* src, cv::_OutputArray* dst, cv::cuda::Stream* stream)
{
   cv::cuda::equalizeHist(*src, *dst, stream ? *stream : cv::cuda::Stream::Null());
}

void cudaHistEven(cv::_InputArray* src, cv::_OutputArray* hist, int histSize, int lowerLevel, int upperLevel, cv::cuda::Stream* stream)
{
   cv::cuda::histEven(*src, *hist, histSize, lowerLevel, upperLevel, stream ? *stream : cv::cuda::Stream::Null());
}

void cudaHistRange(cv::_InputArray* src, cv::_OutputArray* hist, cv::_InputArray* levels, cv::cuda::Stream* stream)
{
   cv::cuda::histRange(*src, *hist, *levels, stream ? *stream : cv::cuda::Stream::Null());
}

void cudaBilateralFilter(cv::_InputArray* src, cv::_OutputArray* dst, int kernelSize, float sigmaColor, float sigmaSpatial, int borderMode, cv::cuda::Stream* stream)
{
   cv::cuda::bilateralFilter(*src, *dst, kernelSize, sigmaColor, sigmaSpatial, borderMode, stream ? *stream : cv::cuda::Stream::Null());
}

//----------------------------------------------------------------------------
//
//  CudaCornernessCriteria
//
//----------------------------------------------------------------------------
cv::cuda::CornernessCriteria* cudaCreateHarrisCorner(int srcType, int blockSize, int ksize, double k, int borderType)
{
   cv::Ptr<cv::cuda::CornernessCriteria> ptr = cv::cuda::createHarrisCorner(srcType, blockSize, ksize, k, borderType);
   ptr.addref();
   return ptr.get();
}

cv::cuda::CornernessCriteria* cudaCreateMinEigenValCorner(int srcType, int blockSize, int ksize, int borderType)
{
	cv::Ptr<cv::cuda::CornernessCriteria> ptr = cv::cuda::createMinEigenValCorner(srcType, blockSize, ksize, borderType);
	ptr.addref();
	return ptr.get();
}

void cudaCornernessCriteriaCompute(cv::cuda::CornernessCriteria* detector, cv::_InputArray* src, cv::_OutputArray* dst, cv::cuda::Stream* stream)
{
   detector->compute( *src, *dst, stream ? *stream : cv::cuda::Stream::Null());
}

void cudaCornernessCriteriaRelease(cv::cuda::CornernessCriteria** detector)
{
   delete *detector;
   *detector = 0;
}

//----------------------------------------------------------------------------
//
//  CudaCLAHE
//
//----------------------------------------------------------------------------
cv::cuda::CLAHE* cudaCLAHECreate(double clipLimit, CvSize* tileGridSize)
{
   cv::Size s(tileGridSize->width, tileGridSize->height);
   cv::Ptr<cv::cuda::CLAHE> ptr = cv::cuda::createCLAHE(clipLimit, s);
   ptr.addref();
   return ptr.get();
}
void cudaCLAHEApply(cv::cuda::CLAHE* clahe, cv::_InputArray* src, cv::_OutputArray* dst,  cv::cuda::Stream* stream)
{
   clahe->apply(*src, *dst, stream ? *stream : cv::cuda::Stream::Null());
}
void cudaCLAHERelease(cv::cuda::CLAHE** clahe)
{
   delete *clahe;
   *clahe = 0;
}

//----------------------------------------------------------------------------
//
//  CannyEdgeDetector
//
//----------------------------------------------------------------------------
cv::cuda::CannyEdgeDetector* cudaCreateCannyEdgeDetector(double lowThreshold, double highThreshold, int apertureSize, bool L2gradient)
{
   cv::Ptr<cv::cuda::CannyEdgeDetector> ptr = cv::cuda::createCannyEdgeDetector(lowThreshold, highThreshold, apertureSize, L2gradient);
   ptr.addref();
   return ptr.get();
}
void cudaCannyEdgeDetectorDetect(cv::cuda::CannyEdgeDetector* detector, cv::_InputArray* src, cv::_OutputArray* edges, cv::cuda::Stream* stream)
{
   detector->detect(*src, *edges, stream ? *stream : cv::cuda::Stream::Null());
}
void cudaCannyEdgeDetectorRelease(cv::cuda::CannyEdgeDetector** detector)
{
   delete *detector;
   *detector = 0;
}

//----------------------------------------------------------------------------
//
//  GpuGoodFeaturesToTrackDetector
//
//----------------------------------------------------------------------------
cv::cuda::CornersDetector* cudaGoodFeaturesToTrackDetectorCreate(int srcType, int maxCorners, double qualityLevel, double minDistance, int blockSize, bool useHarrisDetector, double harrisK )
{
	cv::Ptr<cv::cuda::CornersDetector> detector =  cv::cuda::createGoodFeaturesToTrackDetector (srcType, maxCorners, qualityLevel, minDistance, blockSize, useHarrisDetector, harrisK );
	detector.addref();
   return detector.get();
}
void cudaCornersDetectorDetect(cv::cuda::CornersDetector* detector, cv::_InputArray* image, cv::_OutputArray* corners, cv::_InputArray* mask, cv::cuda::Stream* stream)
{
  detector->detect(*image, *corners, mask ? *mask : (cv::InputArray) cv::noArray(), stream ? *stream : cv::cuda::Stream::Null());
}
void cudaCornersDetectorRelease(cv::cuda::CornersDetector** detector)
{
   delete *detector;
   *detector=0;
}

//----------------------------------------------------------------------------
//
//  GpuTemplateMatching
//
//----------------------------------------------------------------------------
cv::cuda::TemplateMatching* cudaTemplateMatchingCreate(int srcType, int method, CvSize* blockSize)
{
   cv::Size s(blockSize->width, blockSize->height);
	cv::Ptr<cv::cuda::TemplateMatching> ptr = cv::cuda::createTemplateMatching(srcType, method, s);
   ptr.addref();
   return ptr.get();
}

void cudaTemplateMatchingMatch(cv::cuda::TemplateMatching* tm, cv::_InputArray* image, cv::_InputArray* templ, cv::_OutputArray* result,  cv::cuda::Stream* stream)
{
   tm->match(*image, *templ, *result, stream ? *stream : cv::cuda::Stream::Null());
}

void cudaTemplateMatchingRelease(cv::cuda::TemplateMatching** tm)
{
   delete *tm;
   *tm = 0;
}

//----------------------------------------------------------------------------
//
//  CudaHoughLinesDetector
//
//----------------------------------------------------------------------------
cv::cuda::HoughLinesDetector* cudaHoughLinesDetectorCreate(float rho, float theta, int threshold, bool doSort, int maxLines)
{
   cv::Ptr<cv::cuda::HoughLinesDetector> ptr = cv::cuda::createHoughLinesDetector(rho, theta, threshold, doSort, maxLines);
   ptr.addref();
   return ptr.get();
}
void cudaHoughLinesDetectorDetect(cv::cuda::HoughLinesDetector* detector, cv::_InputArray* src, cv::_OutputArray* lines, cv::cuda::Stream* stream)
{
   detector->detect(*src, *lines, stream ? *stream : cv::cuda::Stream::Null());
}
void cudaHoughLinesDetectorRelease(cv::cuda::HoughLinesDetector** detector)
{
   delete *detector;
   *detector = 0;
}

//----------------------------------------------------------------------------
//
//  CudaHoughSegmentDetector
//
//----------------------------------------------------------------------------
cv::cuda::HoughSegmentDetector* cudaHoughSegmentDetectorCreate(float rho, float theta, int minLineLength, int maxLineGap, int maxLines)
{
   cv::Ptr<cv::cuda::HoughSegmentDetector> ptr = cv::cuda::createHoughSegmentDetector(rho, theta, minLineLength, maxLineGap, maxLines);
   ptr.addref();
   return ptr.get();
}
void cudaHoughSegmentDetectorDetect(cv::cuda::HoughSegmentDetector* detector, cv::_InputArray* src, cv::_OutputArray* lines, cv::cuda::Stream* stream)
{
   detector->detect(*src, *lines, stream ? *stream : cv::cuda::Stream::Null());
}
void cudaHoughSegmentDetectorRelease(cv::cuda::HoughSegmentDetector** detector)
{
   delete *detector;
   *detector = 0;
}

//----------------------------------------------------------------------------
//
//  CudaHoughCirclesDetector
//
//----------------------------------------------------------------------------
cv::cuda::HoughCirclesDetector* cudaHoughCirclesDetectorCreate(float dp, float minDist, int cannyThreshold, int votesThreshold, int minRadius, int maxRadius, int maxCircles)
{
   cv::Ptr<cv::cuda::HoughCirclesDetector> ptr = cv::cuda::createHoughCirclesDetector(dp, minDist, cannyThreshold, votesThreshold, minRadius, maxRadius, maxCircles);
   ptr.addref();
   return ptr.get();

}
void cudaHoughCirclesDetectorDetect(cv::cuda::HoughCirclesDetector* detector, cv::_InputArray* src, cv::_OutputArray* circles, cv::cuda::Stream* stream)
{
   detector->detect(*src, *circles, stream ? *stream : cv::cuda::Stream::Null());
}
void cudaHoughCirclesDetectorRelease(cv::cuda::HoughCirclesDetector** detector)
{
   delete *detector;
   *detector = 0;
}

void cudaGammaCorrection(cv::_InputArray* src, cv::_OutputArray* dst, bool forward, cv::cuda::Stream* stream)
{
   cv::cuda::gammaCorrection(*src, *dst, forward, stream ? *stream : cv::cuda::Stream::Null());
}
