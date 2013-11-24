//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#include "cudaimgproc_c.h"

void cudaBlendLinear(
            const cv::cuda::GpuMat* img1, const cv::cuda::GpuMat* img2, 
            const cv::cuda::GpuMat* weights1, const cv::cuda::GpuMat* weights2, 
            cv::cuda::GpuMat* result, cv::cuda::Stream* stream)
{
   cv::cuda::blendLinear(*img1, *img2, *weights1, *weights2, *result, stream ? *stream : cv::cuda::Stream::Null());
}

void cudaCvtColor(const cv::cuda::GpuMat* src, cv::cuda::GpuMat* dst, int code, cv::cuda::Stream* stream)
{
   cv::cuda::cvtColor(*src, *dst, code, dst->channels(), stream ? *stream : cv::cuda::Stream::Null());
}

void cudaSwapChannels(cv::cuda::GpuMat* image, const int* dstOrder, cv::cuda::Stream* stream)
{
   cv::cuda::swapChannels(*image, dstOrder, stream ? *stream : cv::cuda::Stream::Null());
}

void cudaMeanShiftFiltering(const cv::cuda::GpuMat* src, cv::cuda::GpuMat* dst, int sp, int sr,
                              CvTermCriteria* criteria, cv::cuda::Stream* stream)
{
   cv::cuda::meanShiftFiltering(*src, *dst, sp, sr, *criteria, stream ? *stream : cv::cuda::Stream::Null());
}

void cudaMeanShiftProc(const cv::cuda::GpuMat* src, cv::cuda::GpuMat* dstr, cv::cuda::GpuMat* dstsp, int sp, int sr,
                         CvTermCriteria* criteria, cv::cuda::Stream* stream)
{
   cv::cuda::meanShiftProc(*src, *dstr, *dstsp, sp, sr, *criteria, stream ? *stream : cv::cuda::Stream::Null());
}

void cudaMeanShiftSegmentation(const cv::cuda::GpuMat* src, cv::Mat* dst, int sp, int sr, int minsize,
                                 CvTermCriteria* criteria)
{
   cv::cuda::meanShiftSegmentation(*src, *dst, sp, sr, minsize, *criteria);
}

void cudaHistEven(const cv::cuda::GpuMat* src, cv::cuda::GpuMat* hist, cv::cuda::GpuMat* buffer, int histSize, int lowerLevel, int upperLevel, cv::cuda::Stream* stream)
{
   cv::cuda::GpuMat bufferMat = buffer ? *buffer : cv::cuda::GpuMat();
   cv::cuda::histEven(*src, *hist, bufferMat, histSize, lowerLevel, upperLevel, stream ? *stream : cv::cuda::Stream::Null());
}
cv::cuda::CornernessCriteria* cudaCreateHarrisCorner(int srcType, int blockSize, int ksize, double k, int borderType)
{
   cv::Ptr<cv::cuda::CornernessCriteria> ptr = cv::cuda::createHarrisCorner(srcType, blockSize, ksize, k, borderType);
   ptr.addref();
   return ptr.get();
}

void cudaCornernessCriteriaCompute(cv::cuda::CornernessCriteria* detector, const cv::cuda::GpuMat* src, cv::cuda::GpuMat* dst, cv::cuda::Stream* stream)
{
   detector->compute(*src, *dst, stream ? *stream : cv::cuda::Stream::Null());
}

void cudaCornernessCriteriaRelease(cv::cuda::CornernessCriteria** detector)
{
   delete *detector;
   *detector = 0;
}

void cudaBilateralFilter(cv::cuda::GpuMat* src, cv::cuda::GpuMat* dst, int kernelSize, float sigmaColor, float sigmaSpatial, int borderMode, cv::cuda::Stream* stream)
{
   cv::cuda::bilateralFilter(*src, *dst, kernelSize, sigmaColor, sigmaSpatial, borderMode, stream ? *stream : cv::cuda::Stream::Null());
}

//----------------------------------------------------------------------------
//
//  CudaCLAHE
//
//----------------------------------------------------------------------------
cv::cuda::CLAHE* cudaCLAHECreate(double clipLimit, emgu::size* tileGridSize)
{
   cv::Size s(tileGridSize->width, tileGridSize->height);
   cv::Ptr<cv::cuda::CLAHE> ptr = cv::cuda::createCLAHE(clipLimit, s);
   ptr.addref();
   return ptr.get();
}
void cudaCLAHEApply(cv::cuda::CLAHE* clahe, cv::cuda::GpuMat* src, cv::cuda::GpuMat* dst,  cv::cuda::Stream* stream)
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
void cudaCannyEdgeDetectorDetect(cv::cuda::CannyEdgeDetector* detector, cv::cuda::GpuMat* src, cv::cuda::GpuMat* edges)
{
   detector->detect(*src, *edges);
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
void cudaCornersDetectorDetect(cv::cuda::CornersDetector* detector, const cv::cuda::GpuMat* image, cv::cuda::GpuMat* corners, const cv::cuda::GpuMat* mask)
{
   detector->detect (*image, *corners, mask ? *mask : cv::cuda::GpuMat());
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
cv::cuda::TemplateMatching* cudaTemplateMatchingCreate(int srcType, int method, emgu::size* blockSize)
{
   cv::Size s(blockSize->width, blockSize->height);
	cv::Ptr<cv::cuda::TemplateMatching> ptr = cv::cuda::createTemplateMatching(srcType, method, s);
   ptr.addref();
   return ptr.get();
}

void cudaTemplateMatchingMatch(cv::cuda::TemplateMatching* tm, const cv::cuda::GpuMat* image, const cv::cuda::GpuMat* templ, cv::cuda::GpuMat* result,  cv::cuda::Stream* stream)
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
void cudaHoughLinesDetectorDetect(cv::cuda::HoughLinesDetector* detector, cv::cuda::GpuMat* src, cv::cuda::GpuMat* lines)
{
   detector->detect(*src, *lines);
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
void cudaHoughSegmentDetectorDetect(cv::cuda::HoughSegmentDetector* detector, cv::cuda::GpuMat* src, cv::cuda::GpuMat* lines)
{
   detector->detect(*src, *lines);
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
void cudaHoughCirclesDetectorDetect(cv::cuda::HoughCirclesDetector* detector, cv::cuda::GpuMat* src, cv::cuda::GpuMat* circles)
{
   detector->detect(*src, *circles);
}
void cudaHoughCirclesDetectorRelease(cv::cuda::HoughCirclesDetector** detector)
{
   delete *detector;
   *detector = 0;
}

void cudaGammaCorrection(cv::cuda::GpuMat* src, cv::cuda::GpuMat* dst, bool forward, cv::cuda::Stream* stream)
{
   cv::cuda::gammaCorrection(*src, *dst, forward, stream ? *stream : cv::cuda::Stream::Null());
}
