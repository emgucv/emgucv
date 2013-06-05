//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#include "ocl_c.h"

int oclGetDevice(int deviceType)
{
   std::vector< cv::ocl::Info> deviceInfo;
   return cv::ocl::getDevice(deviceInfo, deviceType);
}

cv::ocl::oclMat* oclMatCreateDefault()
{
   return new cv::ocl::oclMat();
}

cv::ocl::oclMat* oclMatCreate(int rows, int cols, int type)
{
   return new cv::ocl::oclMat(rows, cols, type);
}

emgu::size oclMatGetSize(cv::ocl::oclMat* oclMat)
{
   emgu::size s;
   s.width = oclMat->cols;
   s.height = oclMat->rows;
   return s;
   //return oclMat->size();
}

bool oclMatIsEmpty(cv::ocl::oclMat* oclMat)
{
   return oclMat->empty();
}

bool oclMatIsContinuous(cv::ocl::oclMat* oclMat)
{
   return oclMat->isContinuous();
}

int oclMatGetChannels(cv::ocl::oclMat* oclMat)
{
   return oclMat->channels();
}

void oclMatRelease(cv::ocl::oclMat** mat)
{
   delete *mat;
   *mat = 0;
}

//Pefroms blocking upload data to oclMat.
void oclMatUpload(cv::ocl::oclMat* oclMat, CvArr* arr)
{
   cv::Mat mat = cv::cvarrToMat(arr);
   oclMat->upload(mat);
}

//Downloads data from device to host memory. Blocking calls.
void oclMatDownload(cv::ocl::oclMat* oclMat, CvArr* arr)
{
   cv::Mat mat = cv::cvarrToMat(arr);
   oclMat->download(mat);
}

int oclCountNonZero(cv::ocl::oclMat* oclMat)
{
   return cv::ocl::countNonZero(*oclMat);
}

void oclMatAdd(const cv::ocl::oclMat* a, const cv::ocl::oclMat* b, cv::ocl::oclMat* c, const cv::ocl::oclMat* mask)
{
   if (mask)
   {
      cv::ocl::add(*a, *b, *c, *mask);
   }
   else
   {
      cv::ocl::add(*a, *b, *c);
   }
}

void oclMatAddS(const cv::ocl::oclMat* a, const CvScalar scale, cv::ocl::oclMat* c, const cv::ocl::oclMat* mask)
{
   cv::ocl::add(*a, scale, *c, mask? *mask : cv::ocl::oclMat());
}

void oclMatSubtract(const cv::ocl::oclMat* a, const cv::ocl::oclMat* b, cv::ocl::oclMat* c, const cv::ocl::oclMat* mask)
{
   if (mask)
   {
      cv::ocl::subtract(*a, *b, *c, *mask);
   } else
   {
      cv::ocl::subtract(*a, *b, *c);
   }
}

void oclMatSubtractS(const cv::ocl::oclMat* a, const CvScalar scale, cv::ocl::oclMat* c, const cv::ocl::oclMat* mask)
{
   cv::ocl::subtract(*a, scale, *c, mask ? *mask : cv::ocl::oclMat());
}

void oclMatMultiply(const cv::ocl::oclMat* a, const cv::ocl::oclMat* b, cv::ocl::oclMat* c, double scale)
{
   cv::ocl::multiply(*a, b ? *b : cv::ocl::oclMat(), *c, scale);
}

void oclMatDivide(const cv::ocl::oclMat* a, const cv::ocl::oclMat* b, cv::ocl::oclMat* c, double scale)
{
   cv::ocl::divide(*a, b ? *b : cv::ocl::oclMat(), *c, scale);
}

void oclMatFlip(const cv::ocl::oclMat* a, cv::ocl::oclMat* b, int flipCode)
{
   cv::ocl::flip(*a, *b, flipCode);
}

void oclMatBitwiseNot(const cv::ocl::oclMat* src, cv::ocl::oclMat* dst)
{
   cv::ocl::bitwise_not(*src, *dst);
}

void oclMatBitwiseAnd(const cv::ocl::oclMat* src1, const cv::ocl::oclMat* src2, cv::ocl::oclMat* dst, const cv::ocl::oclMat* mask)
{
   cv::ocl::bitwise_and(*src1, *src2, *dst, mask ? *mask : cv::ocl::oclMat());
}

void oclMatBitwiseAndS(const cv::ocl::oclMat* src1, const cv::Scalar sc, cv::ocl::oclMat* dst, const cv::ocl::oclMat* mask)
{
   cv::ocl::bitwise_and(*src1, sc, *dst,  mask ? *mask : cv::ocl::oclMat());
}

void oclMatBitwiseOr(const cv::ocl::oclMat* src1, const cv::ocl::oclMat* src2, cv::ocl::oclMat* dst, const cv::ocl::oclMat* mask)
{
   cv::ocl::bitwise_or(*src1, *src2, *dst, mask? *mask : cv::ocl::oclMat());
}

void oclMatBitwiseOrS(const cv::ocl::oclMat* src1, const cv::Scalar sc, cv::ocl::oclMat* dst, const cv::ocl::oclMat* mask)
{
   cv::ocl::bitwise_or(*src1, sc, *dst, mask? *mask : cv::ocl::oclMat());
}

void oclMatBitwiseXor(const cv::ocl::oclMat* src1, const cv::ocl::oclMat* src2, cv::ocl::oclMat* dst, const cv::ocl::oclMat* mask)
{
   cv::ocl::bitwise_xor(*src1, *src2, *dst, mask? *mask : cv::ocl::oclMat());
}

void oclMatBitwiseXorS(const cv::ocl::oclMat* src1, const cv::Scalar sc, cv::ocl::oclMat* dst, const cv::ocl::oclMat* mask)
{
   cv::ocl::bitwise_xor(*src1, sc, *dst, mask? *mask : cv::ocl::oclMat());
}

void oclMatErode( const cv::ocl::oclMat* src, cv::ocl::oclMat* dst, const CvArr* kernel, CvPoint anchor, int iterations, int borderType, cv::Scalar borderValue)
{
   cv::Mat kernelMat = kernel ? cv::cvarrToMat(kernel) : cv::Mat();
   cv::ocl::erode(*src, *dst, kernelMat, anchor, iterations, borderType, borderValue);
}

void oclMatDilate( const cv::ocl::oclMat* src, cv::ocl::oclMat* dst, const CvArr* kernel,  CvPoint anchor, int iterations, int borderType, cv::Scalar borderValue)
{
   cv::Mat kernelMat = kernel ? cv::cvarrToMat(kernel) : cv::Mat();
   cv::ocl::dilate(*src, *dst, kernelMat, anchor, iterations, borderType, borderValue);
}

void oclMatMorphologyEx( const cv::ocl::oclMat* src, cv::ocl::oclMat* dst, int op, const CvArr* kernel, CvPoint anchor, int iterations, int borderType, cv::Scalar borderValue)
{
   cv::Mat kernelMat = kernel ? cv::cvarrToMat(kernel) : cv::Mat();
   cv::ocl::morphologyEx( *src, *dst, op, kernelMat, anchor, iterations, borderType, borderValue);
}

void oclMatCvtColor(const cv::ocl::oclMat* src, cv::ocl::oclMat* dst, int code)
{
   cv::ocl::cvtColor(*src, *dst, code);
}

void oclMatCopy(const cv::ocl::oclMat* src, cv::ocl::oclMat* dst, const cv::ocl::oclMat* mask)
{
   src->copyTo(*dst, mask? * mask : cv::ocl::oclMat());
}

void oclMatResize(const cv::ocl::oclMat* src, cv::ocl::oclMat* dst, double fx, double fy, int interpolation)
{
   cv::ocl::resize(*src, *dst, dst->size(), fx, fy, interpolation);
}

//only support single channel gpuMat
void oclMatMinMaxLoc(const cv::ocl::oclMat* src, 
   double* minVal, double* maxVal, 
   CvPoint* minLoc, CvPoint* maxLoc, 
   const cv::ocl::oclMat* mask)
{
   cv::Point minimunLoc, maximunLoc;
   cv::ocl::oclMat maskMat = mask ? *mask : cv::ocl::oclMat();
   cv::ocl::minMaxLoc(*src, minVal, maxVal, &minimunLoc, &maximunLoc, maskMat);
   maxLoc->x = maximunLoc.x; maxLoc->y = maximunLoc.y;
   minLoc->x = minimunLoc.x; minLoc->y = minimunLoc.y;
}

void oclMatSplit(const cv::ocl::oclMat* src, cv::ocl::oclMat** dst)
{
   int channels = src->channels();
   cv::ocl::oclMat* dstArr = new cv::ocl::oclMat[channels];
   for (int i = 0; i < channels; i++)
      dstArr[i] = *(dst[i]);
   cv::ocl::split(*src, dstArr);
   delete[] dstArr;
}

void oclMatConvertTo(const cv::ocl::oclMat* src, cv::ocl::oclMat* dst, double alpha, double beta)
{
   src->convertTo(*dst, dst->type(), alpha, beta);
}